using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Emdep.Geos.Services.Contracts; 
using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Prism.Logging;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    /// <summary>
    /// Partial class contendo toda a lógica de filtros, Side Tiles e Alert Buttons
    /// Adaptado do ActionPlansFiltersCacheViewModelOld.cs
    /// </summary>
    public partial class ActionPlansModernViewModel
    {
        #region Cache Fields (from Old ViewModel)
        
        private bool _apCacheLoaded;
        private List<APMActionPlanModern> _apCache;
        private string _lastPeriodIds;
        private bool _taskCacheLoaded;
        private List<APMActionPlanTask> _taskCache;
        private string _lastTaskPeriodIds;
        private List<Responsible> _responsibleCache;
        private List<Responsible> _taskResponsibleCache;
        private List<APMActionPlanModern> _currentFilteredBase;
        private List<APMActionPlanModern> _alertFilteredBase;
        private List<APMActionPlanModern> _baseForSideTileCounts;
        private List<APMActionPlanModern> _sideCountsBaseForAll;
        private List<APMActionPlanModern> _sideCountsBaseForNonAll;
        
        #endregion

        #region Fast Filtering (from Old ViewModel)
        
        private bool _fastFilteringEnabled;
        private CancellationTokenSource _filterCts;
        private bool _fastFilterCommandsRewired;
        private const int FastFilterDebounceMs = 60;

        private readonly HashSet<string> _filterTriggerProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "SelectedLocation", "SelectedPerson", "SelectedBusinessUnit", "SelectedOrigin", 
            "SelectedDepartment", "SelectedCustomer"
        };

        public void EnableFastFiltering()
        {
            if (_fastFilteringEnabled) return;
            PropertyChanged += OnFastFilterPropertyChanged;
            _fastFilteringEnabled = true;
            ApplyInMemoryFiltersAsync();
        }

        public void DisableFastFiltering()
        {
            if (!_fastFilteringEnabled) return;
            PropertyChanged -= OnFastFilterPropertyChanged;
            _fastFilteringEnabled = false;
            CancelOngoingFilter();
        }

        private void OnFastFilterPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!_fastFilteringEnabled) return;
            if (_filterTriggerProperties.Contains(e.PropertyName))
                ApplyInMemoryFiltersAsync(120);
        }

        private void CancelOngoingFilter()
        {
            try { _filterCts?.Cancel(); } catch { }
            try { _filterCts?.Dispose(); } catch { }
            _filterCts = null;
        }

        #endregion

        #region Side Tiles Logic (from Old ViewModel)
        
        private bool _sideTileCommandsRewired;
        private TileBarFilters _lastAppliedSideTileFilter;
        private static readonly Regex _andSplit = new Regex("\\bAND\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex _inExpr = new Regex("\\[(?<f>[^\\]]+)\\]\\s+In\\s*\\((?<v>[^\\)]+)\\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex _eqExpr = new Regex("\\[(?<f>[^\\]]+)\\]\\s*=\\s*'(?<v>[^']+)'", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private string _lastAppliedSideCaption;
        private List<APMActionPlanModern> _dataBeforeSideTileFilter;
        private bool _suppressSideTileCountUpdate;
        private bool _pendingAllWhileAlert;
        private bool _lastClickedWasAll;
        private Dictionary<string, int> _sideTileCountsSnapshot;

        // Top filter selections (missing in Modern partial)
        private TileBarFilters _lastAppliedTopFilter;
        private TileBarFilters _lastAppliedTaskTopFilter;

        // Property used by XAML bindings to switch between ActionPlan view and Task view
        private bool _isTaskGridVisibility;
        public bool IsTaskGridVisibility
        {
            get => _isTaskGridVisibility;
            set
            {
                if (_isTaskGridVisibility != value)
                {
                    _isTaskGridVisibility = value;
                    OnPropertyChanged(nameof(IsTaskGridVisibility));
                }
            }
        }

        private void RewireSideTileBarCommands()
        {
            if (_sideTileCommandsRewired) return;
            
            try
            {
                // Já está conectado via SideTileClickCommand no constructor
                _sideTileCommandsRewired = true;
                
                GeosApplication.Instance.Logger?.Log("Side tile bar commands rewired", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in RewireSideTileBarCommands: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        private void OnSideTileClickedInternal(object arg)
        {
            try
            {
                TileBarFilters clickedItem = arg as TileBarFilters;
                if (clickedItem == null) return;

                GeosApplication.Instance.Logger?.Log($"Side tile clicked (internal): {clickedItem.Caption}", Category.Info, Priority.Low);

                // Check if clicking "All"
                bool isAll = IsAllCaption(clickedItem.Caption);
                bool wasPreviouslyAll = _lastClickedWasAll;
                _lastClickedWasAll = isAll;

                // Toggle logic: if same item clicked, clear filter
                if (_lastAppliedSideTileFilter != null && 
                    _lastAppliedSideTileFilter.Caption == clickedItem.Caption)
                {
                    // Clear side filter
                    _lastAppliedSideTileFilter = null;
                    _lastAppliedSideCaption = null;
                    SelectedTileBarItem = null;

                    // Restore data
                    if (_dataBeforeSideTileFilter != null)
                    {
                        SetActionPlanList(_dataBeforeSideTileFilter);
                        _dataBeforeSideTileFilter = null;
                    }

                    // Recalculate all counts (updates both side tiles and alert buttons)
                    RecalculateAllCounts();
                    return;
                }

                // Apply new side filter (clear previous if any)
                _lastAppliedSideTileFilter = clickedItem;
                _lastAppliedSideCaption = clickedItem.Caption;
                SelectedTileBarItem = clickedItem;

                // Get effective base for filtering
                List<APMActionPlanModern> effectiveBase = BuildBaselineForSideFilter();

                if (isAll)
                {
                    // Just restore to baseline (without side filter)
                    SetActionPlanList(effectiveBase);
                }
                else
                {
                    // Apply side filter criteria
                    ApplySideTileFilter(clickedItem.FilterCriteria, clickedItem.Caption, effectiveBase);
                }

                // Recalculate all counts (updates both side tiles and alert buttons)
                RecalculateAllCounts();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in OnSideTileClickedInternal: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        private void ApplyTaskLevelFilter(Func<APMActionPlanTask, bool> taskPredicate)
        {
            try
            {
                IsBusy = true;

                // 1. Obter a lista de Planos
                var sourceData = _currentFilteredBase ?? _allDataCache; // Lista de APMActionPlanModern

                // 2. Obter a lista de Tasks (AQUI ESTÁ O TRUQUE: Usar o cache global de tasks)
                // Se _taskCache não estiver acessível aqui, use _tasksCache ou a variável onde guarda todas as tasks
                var allTasksSource = _taskCache ?? new List<APMActionPlanTask>();

                if (sourceData == null || allTasksSource == null) return;

                var filteredView = new ObservableCollection<ActionPlanModernDto>();

                // Criar um dicionário para busca rápida de tasks por plano
                // Isto evita percorrer a lista gigante de tasks dentro do loop de planos
                var tasksByPlan = allTasksSource
                    .Where(t => t != null)
                    .GroupBy(t => t.IdActionPlan)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var planEntity in sourceData)
                {
                    // Tenta encontrar tasks para este plano no dicionário
                    if (!tasksByPlan.ContainsKey(planEntity.IdActionPlan)) continue;

                    var planTasks = tasksByPlan[planEntity.IdActionPlan];

                    // Aplica o filtro (predicado) às tasks deste plano
                    var matchingTasks = planTasks.Where(taskPredicate).ToList();

                    if (matchingTasks.Count > 0)
                    {
                        // Mapear Pai
                        var planDto = MapToDto(planEntity);

                        foreach (var taskEntity in matchingTasks)
                        {
                            // CORREÇÃO: Usar ApmHelpers
                            planDto.Tasks.Add(ApmHelpers.MapToTaskDto(taskEntity));
                        }

                        planDto.TasksCount = planDto.Tasks.Count;
                        filteredView.Add(planDto);
                    }
                }

                ActionPlans = filteredView;
                OnPropertyChanged(nameof(ActionPlans));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in ApplyTaskLevelFilter: {ex.Message}", Category.Exception, Priority.High);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplySideTileFilter(string criteria, string caption, List<APMActionPlanModern> effectiveBase)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criteria) || effectiveBase == null || !effectiveBase.Any())
                {
                    SetActionPlanList(effectiveBase ?? new List<APMActionPlanModern>());
                    return;
                }

                // Parse criteria
                var expressions = ParseCriteria(criteria);
                if (expressions == null || !expressions.Any())
                {
                    SetActionPlanList(effectiveBase);
                    return;
                }

                // Filter action plans based on criteria
                var filtered = ApplySideFilterToData(effectiveBase, _lastAppliedSideTileFilter);
                
                SetActionPlanList(filtered);

                GeosApplication.Instance.Logger?.Log($"Applied side tile filter '{caption}': {filtered.Count} plans", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in ApplySideTileFilter: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        private List<APMActionPlanModern> ApplySideFilterToData(List<APMActionPlanModern> source, TileBarFilters sideFilter)
        {
            if (source == null || source.Count == 0) return new List<APMActionPlanModern>();
            if (sideFilter == null || IsAllCaption(sideFilter.Caption)) return source.ToList();

            var expressions = ParseCriteria(sideFilter.FilterCriteria);
            if (expressions == null || expressions.Count == 0)
                return source.ToList();

            bool TaskMatchesAll(APMActionPlanModern ap, APMActionPlanTask t) =>
                expressions.All(expr => PropertyMatches(t, expr.Field, expr.Values) || PropertyMatches(ap, expr.Field, expr.Values));

            var result = new List<APMActionPlanModern>();
            foreach (var ap in source)
            {
                if (ap.TaskList == null || !ap.TaskList.Any()) continue;

                var matchingTasks = ap.TaskList.Where(t => TaskMatchesAll(ap, t)).ToList();
                if (matchingTasks.Any())
                {
                    var clone = ClonePlan(ap);
                    clone.TaskList = matchingTasks;
                    result.Add(clone);
                }
            }
            return result;
        }

        private List<APMActionPlanModern> ApplyCustomFilterToPlans(List<APMActionPlanModern> source, string criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria)) return source;
            var result = new List<APMActionPlanModern>();

            foreach (var ap in source)
            {
                bool matchPlan = MatchCustomCriteria(ap, criteria);

                var matchingTasks = new List<APMActionPlanTask>();
                if (ap.TaskList != null)
                {
                    foreach (var t in ap.TaskList)
                    {
                        if (MatchCustomCriteria(t, criteria) || matchPlan)
                        {
                            matchingTasks.Add(t);
                        }
                    }
                }

                if (matchingTasks.Count > 0 || (matchPlan && (ap.TaskList == null || ap.TaskList.Count == 0)))
                {
                    var clone = (APMActionPlanModern)ap.Clone();
                    clone.TaskList = matchingTasks.Select(t => (APMActionPlanTask)t.Clone()).ToList();
                    result.Add(clone);
                }
            }
            return result;
        }

        private List<APMActionPlanTask> ApplyCustomFilterToTasks(List<APMActionPlanTask> source, string criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria)) return source;
            return source.Where(t => MatchCustomCriteria(t, criteria)).ToList();
        }

        private bool MatchCustomCriteria(object obj, string criteria)
        {
            if (obj == null || string.IsNullOrWhiteSpace(criteria)) return false;
            try
            {
                if (criteria.StartsWith("StartsWith(", StringComparison.OrdinalIgnoreCase))
                {
                    var content = criteria.Substring(11).TrimEnd(')');
                    var parts = content.Split(',');
                    if (parts.Length >= 2)
                    {
                        string field = parts[0].Trim().Trim('[', ']');
                        string val = parts[1].Trim().Trim('\'', ' ');
                        string objVal = GetPropValue(obj, field);
                        return objVal != null && objVal.StartsWith(val, StringComparison.OrdinalIgnoreCase);
                    }
                }
                else if (criteria.StartsWith("EndsWith(", StringComparison.OrdinalIgnoreCase))
                {
                    var content = criteria.Substring(9).TrimEnd(')');
                    var parts = content.Split(',');
                    if (parts.Length >= 2)
                    {
                        string field = parts[0].Trim().Trim('[', ']');
                        string val = parts[1].Trim().Trim('\'', ' ');
                        string objVal = GetPropValue(obj, field);
                        return objVal != null && objVal.EndsWith(val, StringComparison.OrdinalIgnoreCase);
                    }
                }
                else if (criteria.StartsWith("Contains(", StringComparison.OrdinalIgnoreCase) && !criteria.Contains("Not Contains"))
                {
                    var content = criteria.Substring(9).TrimEnd(')');
                    var parts = content.Split(',');
                    if (parts.Length >= 2)
                    {
                        string field = parts[0].Trim().Trim('[', ']');
                        string val = parts[1].Trim().Trim('\'', ' ');
                        string objVal = GetPropValue(obj, field);
                        return objVal != null && objVal.IndexOf(val, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
                else if (criteria.Contains("="))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(criteria, @"\[(.*?)\]\s*=\s*'(.*?)'");
                    if (match.Success)
                    {
                        string field = match.Groups[1].Value;
                        string val = match.Groups[2].Value;
                        string objVal = GetPropValue(obj, field);
                        return string.Equals(objVal, val, StringComparison.OrdinalIgnoreCase);
                    }
                }
                else if (criteria.Contains(" In ") && !criteria.Contains("Not"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(criteria, @"\[(.*?)\]\s+In\s*\((.*?)\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        string field = match.Groups[1].Value;
                        var vals = match.Groups[2].Value.Split(',').Select(v => v.Trim().Trim('\''));
                        string objVal = GetPropValue(obj, field);
                        return objVal != null && vals.Contains(objVal, StringComparer.OrdinalIgnoreCase);
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private string GetPropValue(object obj, string fieldName)
        {
            if (obj == null) return null;
            var prop = obj.GetType().GetProperty(fieldName);
            if (prop == null) return null;
            var val = prop.GetValue(obj);
            return val?.ToString();
        }

        private void UpdateTopTileCounts(List<APMActionPlanModern> source, bool isTaskView)
        {
            try
            {
                var tilesCollection = isTaskView ? TopTaskListOfFilterTile : TopListOfFilterTile;
                if (tilesCollection == null || tilesCollection.Count == 0) return;

                var baseForCounts = source ?? new List<APMActionPlanModern>();

                foreach (var tile in tilesCollection)
                {
                    if (tile == null) continue;
                    int count = 0;

                    if (IsAllCaption(tile.Caption))
                    {
                        if (baseForCounts.Count > 0)
                        {
                            count = baseForCounts.Sum(ap => ap.TaskList?.Sum(t => 1 + (t.SubTaskList?.Count ?? 0)) ?? 0);
                        }
                    }
                    else
                    {
                        if (isTaskView)
                        {
                            var allTasks = baseForCounts.Where(ap => ap.TaskList != null).SelectMany(ap => ap.TaskList).ToList();
                            var filteredTasks = ApplyCustomFilterToTasks(allTasks, tile.FilterCriteria);
                            count = filteredTasks.Sum(t => 1 + (t.SubTaskList?.Count ?? 0));
                        }
                        else
                        {
                            var filteredPlans = ApplyCustomFilterToPlans(baseForCounts, tile.FilterCriteria);
                            count = filteredPlans.Sum(ap => ap.TaskList?.Sum(t => 1 + (t.SubTaskList?.Count ?? 0)) ?? 0);
                        }
                    }

                    tile.EntitiesCount = count;
                    tile.EntitiesCountVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in UpdateTopTileCounts: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        private List<APMActionPlanModern> ApplyCustomFilterToData(List<APMActionPlanModern> source, TileBarFilters customFilter)
        {
            if (source == null || source.Count == 0) return new List<APMActionPlanModern>();
            if (customFilter == null || IsAllCaption(customFilter.Caption))
                return source.Select(ap => (APMActionPlanModern)ap.Clone()).ToList();

            return ApplyCustomFilterToPlans(source, customFilter.FilterCriteria);
        }

        private List<APMActionPlanModern> ApplyAlertFilterToData(List<APMActionPlanModern> source, string alertCaption)
        {
            if (source == null || source.Count == 0) return new List<APMActionPlanModern>();
            if (string.IsNullOrWhiteSpace(alertCaption))
                return source.Select(ap => (APMActionPlanModern)ap.Clone()).ToList();

            return ApplyAlertToPlans(source, alertCaption);
        }

        private List<APMActionPlanModern> BuildBaselineForSideFilter()
        {
            // If alert is active, use alert-filtered data
            if (IsAlertActive())
            {
                return _alertFilteredBase ?? _allDataCache ?? new List<APMActionPlanModern>();
            }

            // Otherwise use all data or current filtered base
            return _currentFilteredBase ?? _allDataCache ?? new List<APMActionPlanModern>();
        }

        private void UpdateSideTileCountsRespectingRules()
        {
            try
            {
                if (_suppressSideTileCountUpdate) return;

                // Use appropriate base for counts
                var baseForCounts = IsAllCaption(_lastAppliedSideCaption ?? "")
                    ? BuildBaselineForSideCounts_All()
                    : BuildBaselineForSideCounts_NonAll();

                UpdateSideTileCounts(baseForCounts);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in UpdateSideTileCountsRespectingRules: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        private List<APMActionPlanModern> BuildBaselineForSideCounts_All()
        {
            // When "All" is selected, counts should reflect alert-filtered or fully filtered data
            if (IsAlertActive())
                return _alertFilteredBase ?? _allDataCache ?? new List<APMActionPlanModern>();
            
            return _currentFilteredBase ?? _allDataCache ?? new List<APMActionPlanModern>();
        }

        private List<APMActionPlanModern> BuildBaselineForSideCounts_NonAll()
        {
            // When specific side filter is active, counts should reflect that + alert
            if (!string.IsNullOrWhiteSpace(_lastAppliedSideCaption) &&
                !IsAllCaption(_lastAppliedSideCaption) &&
                _lastAppliedSideTileFilter != null &&
                !string.IsNullOrWhiteSpace(_lastAppliedSideTileFilter.FilterCriteria))
            {
                var exprs = ParseCriteria(_lastAppliedSideTileFilter.FilterCriteria);
                if (exprs != null && exprs.Count > 0)
                {
                    bool TaskMatchesAll(APMActionPlanModern ap, APMActionPlanTask t) =>
                        exprs.All(e => PropertyMatches(t, e.Field, e.Values) || PropertyMatches(ap, e.Field, e.Values));

                    var baseData = IsAlertActive() 
                        ? (_alertFilteredBase ?? _allDataCache) 
                        : (_currentFilteredBase ?? _allDataCache);
                    
                    if (baseData == null) return new List<APMActionPlanModern>();

                    var result = new List<APMActionPlanModern>();
                    foreach (var ap in baseData)
                    {
                        if (ap.TaskList == null || !ap.TaskList.Any()) continue;
                        var matchingTasks = ap.TaskList.Where(t => TaskMatchesAll(ap, t)).ToList();
                        if (matchingTasks.Any())
                        {
                            var clone = ClonePlan(ap);
                            clone.TaskList = matchingTasks;
                            result.Add(clone);
                        }
                    }
                    return result;
                }
            }

            return BuildBaselineForSideCounts_All();
        }

        private bool IsAllCaption(string caption)
        {
            if (string.IsNullOrWhiteSpace(caption)) return false;
            var lc = caption.ToLowerInvariant().Trim();
            return lc == "all" || lc == "todos" || lc == "all themes";
        }

        private void RecalculateAllCounts()
        {
            try
            {
                // Follow OLD ViewModel orchestration to keep counts independent
                var baseData = BuildBaselineForAlertTiles();

                if (baseData == null || baseData.Count == 0)
                {
                    baseData = _currentFilteredBase?.Select(ap => (APMActionPlanModern)ap.Clone()).ToList()
                               ?? new List<APMActionPlanModern>();
                }

                bool isTaskView = IsTaskGridVisibility;

                var activeSideFilter = _lastAppliedSideTileFilter;
                var activeAlertCaption = _lastAppliedAlertCaption;
                var activeCustomFilter = isTaskView ? _lastAppliedTaskTopFilter : _lastAppliedTopFilter;

                // data for side counts: base + custom top filters -> then alert
                var dataForSideCounts = ApplyAlertFilterToData(
                                            ApplyCustomFilterToData(baseData, activeCustomFilter),
                                            activeAlertCaption);

                // data for top/custom counts: base + side filter -> then alert
                var dataForCustomCounts = ApplyAlertFilterToData(
                                            ApplySideFilterToData(baseData, activeSideFilter),
                                            activeAlertCaption);

                // data for alert counts: base + side + custom (sem alert)
                var dataForAlertCounts = ApplyCustomFilterToData(
                                            ApplySideFilterToData(baseData, activeSideFilter),
                                            activeCustomFilter);

                // Update UI counts
                UpdateSideTileCounts(dataForSideCounts);
                UpdateTopTileCounts(dataForCustomCounts, isTaskView);
                UpdateAlertButtonCounts(dataForAlertCounts);

                // Final grid: apply side + custom + alert
                var finalDataForGrid = ApplyAlertFilterToData(
                                            ApplyCustomFilterToData(
                                                ApplySideFilterToData(baseData, activeSideFilter),
                                                activeCustomFilter),
                                            activeAlertCaption);

                // Normalize due days and set list
                foreach (var ap in finalDataForGrid)
                {
                    if (ap.TaskList == null) continue;
                    foreach (var task in ap.TaskList)
                    {
                        task.DueDays = task.DueDate < DateTime.Now ? CalculateDueDays(task.DueDate) : 0;
                        if (task.SubTaskList == null) continue;
                        foreach (var st in task.SubTaskList)
                        {
                            st.DueDays = st.DueDate < DateTime.Now ? CalculateDueDays(st.DueDate) : 0;
                        }
                    }
                }

                SetActionPlanList(finalDataForGrid);

                GeosApplication.Instance.Logger?.Log("RecalculateAllCounts completed (modern-aligned)", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in RecalculateAllCounts: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        #endregion

        #region Alert Buttons Logic (from Old ViewModel)
        
        private bool _alertCommandsRewired;
        private string _lastAppliedAlertCaption;
        private List<APMActionPlanModern> _dataBeforeAlertFilter;
        private bool _suppressAlertTileRowUpdate;

        private bool IsAlertActive()
        {
            return !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption);
        }

        private List<APMActionPlanModern> BuildBaselineForAlertTiles()
        {
            // Always return the dropdown-filtered baseline (no side/alert applied)
            return _currentFilteredBase ?? _allDataCache ?? new List<APMActionPlanModern>();
        }

        private void RewireAlertTileBarCommands()
        {
            if (_alertCommandsRewired) return;
            
            try
            {
                // Já está conectado via AlertButtonClickCommand no constructor
                _alertCommandsRewired = true;
                
                GeosApplication.Instance.Logger?.Log("Alert tile bar commands rewired", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in RewireAlertTileBarCommands: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        private void OnAlertButtonClickInternal(APMAlertTileBarFilters clickedItem)
        {
            try
            {
                if (clickedItem == null) return;

                GeosApplication.Instance.Logger?.Log($"Alert button clicked: {clickedItem.Caption}", Category.Info, Priority.Low);

                // Toggle logic
                if (!string.IsNullOrWhiteSpace(_lastAppliedAlertCaption) &&
                    _lastAppliedAlertCaption.Equals(clickedItem.Caption, StringComparison.OrdinalIgnoreCase))
                {
                    // Clear alert filter
                    _lastAppliedAlertCaption = null;
                    SelectedAlertTileBarItem = null;
                    _alertFilteredBase = null;

                    // Restore previous data
                    if (_dataBeforeAlertFilter != null)
                    {
                        SetActionPlanList(_dataBeforeAlertFilter);
                        _dataBeforeAlertFilter = null;
                    }

                    // Recalculate counts
                    RecalculateAllCounts();
                    return;
                }

                // Apply new alert filter
                _lastAppliedAlertCaption = clickedItem.Caption;
                SelectedAlertTileBarItem = clickedItem;

                // Build baseline (considering side filter if active)
                var baseline = BuildBaselineForAlertTiles();
                
                // Store data before alert filter
                if (_dataBeforeAlertFilter == null)
                {
                    _dataBeforeAlertFilter = _currentFilteredBase ?? _allDataCache;
                }

                // Apply alert filter
                var filtered = ApplyAlertToPlans(baseline, clickedItem.Caption);
                
                _alertFilteredBase = filtered;
                SetActionPlanList(filtered);

                // Recalculate all counts
                RecalculateAllCounts();

                GeosApplication.Instance.Logger?.Log($"Applied alert filter '{clickedItem.Caption}': {filtered.Count} plans", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in OnAlertButtonClickInternal: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        private List<APMActionPlanModern> ApplyAlertToPlans(List<APMActionPlanModern> plans, string alertCaption)
        {
            // Se não houver legenda ou planos, devolve cópia limpa
            if (string.IsNullOrWhiteSpace(alertCaption))
                return plans?.Select(p => (APMActionPlanModern)p.Clone()).ToList() ?? new List<APMActionPlanModern>();

            // IMPORTANTE: Trabalhar sempre com clones para não afetar a lista original em memória
            var source = plans?.Select(p => (APMActionPlanModern)p.Clone()).ToList() ?? new List<APMActionPlanModern>();
            if (source.Count == 0) return source;

            var lc = alertCaption.ToLowerInvariant();

            // =========================================================
            // 1. LONGEST OVERDUE (CORREÇÃO ROBUSTA POR INTEIROS)
            // =========================================================
            if (lc.Contains("longest") && lc.Contains("over") && lc.Contains("due"))
            {
                // Passo A: Encontrar o NÚMERO MÁXIMO de dias de atraso em toda a lista
                int maxDaysFound = 0;

                foreach (var p in source)
                {
                    if (p.TaskList == null) continue;
                    foreach (var t in p.TaskList)
                    {
                        // Verifica Task
                        if (!IsClosedExact(t.Status, t.IdLookupStatus))
                        {
                            int d = CalculateDueDays(t.DueDate);
                            if (d > maxDaysFound) maxDaysFound = d;
                        }

                        // Verifica SubTasks
                        if (t.SubTaskList != null)
                        {
                            foreach (var st in t.SubTaskList)
                            {
                                if (!IsClosedExact(st.Status, st.IdLookupStatus))
                                {
                                    int sd = CalculateDueDays(st.DueDate);
                                    if (sd > maxDaysFound) maxDaysFound = sd;
                                }
                            }
                        }
                    }
                }

                // Se não encontrou atrasos (>0), retorna lista vazia
                if (maxDaysFound <= 0) return new List<APMActionPlanModern>();

                // Passo B: Filtrar a lista APENAS com os itens que têm exatamente 'maxDaysFound'
                var result = new List<APMActionPlanModern>();

                foreach (var p in source)
                {
                    if (p.TaskList == null) continue;

                    var matchingTasks = new List<APMActionPlanTask>();
                    bool planHasMatch = false;

                    foreach (var t in p.TaskList)
                    {
                        // Cenário 1: A própria Task é a vencedora
                        bool isTaskWinner = !IsClosedExact(t.Status, t.IdLookupStatus) &&
                                            CalculateDueDays(t.DueDate) == maxDaysFound;

                        // Cenário 2: Alguma SubTask é a vencedora
                        var winningSubTasks = new List<APMActionPlanSubTask>();
                        if (t.SubTaskList != null)
                        {
                            winningSubTasks = t.SubTaskList
                                .Where(st => !IsClosedExact(st.Status, st.IdLookupStatus) &&
                                             CalculateDueDays(st.DueDate) == maxDaysFound)
                                .Select(st => (APMActionPlanSubTask)st.Clone())
                                .ToList();
                        }

                        if (isTaskWinner)
                        {
                            // Se a Task venceu, mostramos a Task limpa (sem subtasks, para focar no problema)
                            var tClone = (APMActionPlanTask)t.Clone();
                            tClone.SubTaskList = new List<APMActionPlanSubTask>();
                            matchingTasks.Add(tClone);
                            planHasMatch = true;
                        }
                        else if (winningSubTasks.Any())
                        {
                            // Se a SubTask venceu, mostramos a Task Pai + Apenas a SubTask vencedora
                            var tClone = (APMActionPlanTask)t.Clone();
                            tClone.SubTaskList = winningSubTasks;
                            matchingTasks.Add(tClone);
                            planHasMatch = true;
                        }
                    }

                    if (planHasMatch && matchingTasks.Count > 0)
                    {
                        var pClone = (APMActionPlanModern)p.Clone();
                        pClone.TaskList = matchingTasks;
                        result.Add(pClone);
                    }
                }

                return result;
            }

            // =========================================================
            // 2. MOST OVERDUE THEME / RESPONSIBLE
            // =========================================================

            Func<APMActionPlanTask, bool> taskPred = null;
            Func<APMActionPlanSubTask, bool> subTaskPred = null;

            if (lc.Contains("most overdue theme"))
            {
                // Usa extração auxiliar para calcular qual é o tema
                // Nota: ExtractAllTasks... deve estar disponível na classe partial
                var allItems = ExtractAllTasksAndSubTasksWithAllFilters(source, null, null, null, null, null, null, null, null, false);

                var overdueItems = allItems
                    .Where(t => CalculateDueDays(t.DueDate) > 0 && !IsClosedExact(t.Status, t.IdLookupStatus))
                    .ToList();

                var mostTheme = overdueItems
                    .Where(t => !string.IsNullOrWhiteSpace(t.Theme))
                    .GroupBy(t => t.Theme.Trim())
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key;

                if (!string.IsNullOrWhiteSpace(mostTheme))
                {
                    taskPred = t => CalculateDueDays(t.DueDate) > 0 && !IsClosedExact(t.Status, t.IdLookupStatus) &&
                                   string.Equals(t.Theme?.Trim(), mostTheme, StringComparison.OrdinalIgnoreCase);

                    subTaskPred = st => CalculateDueDays(st.DueDate) > 0 && !IsClosedExact(st.Status, st.IdLookupStatus) &&
                                       string.Equals(st.Theme?.Trim(), mostTheme, StringComparison.OrdinalIgnoreCase);
                }
                else return new List<APMActionPlanModern>();
            }
            else if (lc.Contains("most overdue responsible"))
            {
                var allItems = ExtractAllTasksAndSubTasksWithAllFilters(source, null, null, null, null, null, null, null, null, false);

                var overdueItems = allItems
                    .Where(t => CalculateDueDays(t.DueDate) > 0 && !IsClosedExact(t.Status, t.IdLookupStatus))
                    .ToList();

                var mostResp = overdueItems
                    .Where(t => !string.IsNullOrWhiteSpace(t.TaskResponsibleDisplayName ?? t.Responsible))
                    .GroupBy(t => (t.TaskResponsibleDisplayName ?? t.Responsible).Trim())
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key;

                if (!string.IsNullOrWhiteSpace(mostResp))
                {
                    taskPred = t => CalculateDueDays(t.DueDate) > 0 && !IsClosedExact(t.Status, t.IdLookupStatus) &&
                                   string.Equals((t.TaskResponsibleDisplayName ?? t.Responsible)?.Trim(), mostResp, StringComparison.OrdinalIgnoreCase);

                    subTaskPred = st => CalculateDueDays(st.DueDate) > 0 && !IsClosedExact(st.Status, st.IdLookupStatus) &&
                                       string.Equals((st.TaskResponsibleDisplayName ?? st.Responsible)?.Trim(), mostResp, StringComparison.OrdinalIgnoreCase);
                }
                else return new List<APMActionPlanModern>();
            }
            else
            {
                // =========================================================
                // 3. FILTROS NORMAIS (To Do, Done, High Prio, etc.)
                // =========================================================
                taskPred = GetTaskPredicate(alertCaption);
                subTaskPred = GetSubTaskPredicate(alertCaption);
            }

            if (taskPred == null) return source; // Se não encontrou predicado, retorna tudo (fallback)

            // =========================================================
            // 4. LOOP DE FILTRAGEM FINAL (Comum)
            // =========================================================
            var finalResult = new List<APMActionPlanModern>();
            foreach (var ap in source)
            {
                if (ap.TaskList == null) continue;
                var matchingTasks = new List<APMActionPlanTask>();

                foreach (var task in ap.TaskList)
                {
                    bool taskMatch = taskPred(task);

                    var matchingSubTasks = new List<APMActionPlanSubTask>();
                    // Verifica subtasks se existirem e houver predicado para elas
                    if (task.SubTaskList != null && subTaskPred != null)
                    {
                        matchingSubTasks = task.SubTaskList
                            .Where(subTaskPred)
                            .Select(st => (APMActionPlanSubTask)st.Clone())
                            .ToList();
                    }

                    if (taskMatch)
                    {
                        var tClone = (APMActionPlanTask)task.Clone();

                        // Se o filtro for específico (ex: High Priority ou Overdue), 
                        // queremos limpar as subtasks que não cumprem o critério, mesmo que o pai cumpra.
                        // Mas se for "To Do", normalmente queremos ver tudo.
                        // Para manter lógica OLD estrita: se houver subtasks que batem, mostra só elas. 
                        // Se não houver, mas o pai bate, mostra o pai (com todas ou nenhumas? No OLD mostra tudo se pai bate).
                        // AQUI: Vamos priorizar mostrar subtasks filtradas se existirem.

                        if (subTaskPred != null && tClone.SubTaskList != null)
                        {
                            if (matchingSubTasks.Any())
                                tClone.SubTaskList = matchingSubTasks;
                            // Se não houver matching subtasks, mantém as originais ou limpa? 
                            // No OLD, se o pai é High Priority, mostra o pai.
                        }
                        matchingTasks.Add(tClone);
                    }
                    else if (matchingSubTasks.Any())
                    {
                        // O Pai não bate no filtro (ex: Pai está Done), mas a SubTask está Open/High Prio.
                        // Temos de incluir o Pai como "wrapper" para mostrar a SubTask.
                        var tClone = (APMActionPlanTask)task.Clone();
                        tClone.SubTaskList = matchingSubTasks;
                        matchingTasks.Add(tClone);
                    }
                }

                if (matchingTasks.Count > 0)
                {
                    var apClone = (APMActionPlanModern)ap.Clone();
                    apClone.TaskList = matchingTasks;
                    finalResult.Add(apClone);
                }
            }

            return finalResult;
        }

        private Func<APMActionPlanTask, bool> GetTaskPredicate(string caption)
        {
            if (string.IsNullOrWhiteSpace(caption)) return null;
            var normalized = caption.ToLowerInvariant().Trim();

            bool IsStatusLike(APMActionPlanTask t, string keyword) =>
                !string.IsNullOrWhiteSpace(t.Status) && t.Status.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;

            bool IsClosed(APMActionPlanTask t) => IsClosedExact(t.Status, t.IdLookupStatus);

            if (caption.IndexOf("to do", StringComparison.OrdinalIgnoreCase) >= 0)
                return t => !IsClosed(t) && (IsStatusLike(t, "to do") || IsStatusLike(t, "open")); //

            if (caption.IndexOf("in progress", StringComparison.OrdinalIgnoreCase) >= 0)
                return t => !IsClosed(t) && (IsStatusLike(t, "in progress") || IsStatusLike(t, "working")); //

            if (caption.IndexOf("blocked", StringComparison.OrdinalIgnoreCase) >= 0)
                return t => !IsClosed(t) && (IsStatusLike(t, "blocked") || IsStatusLike(t, "hold")); //

            if (caption.IndexOf("done", StringComparison.OrdinalIgnoreCase) >= 0)
                return t => IsClosed(t);

            if (normalized.Contains("overdue>=15"))
                return t => !IsClosed(t) && CalculateDueDays(t.DueDate) >= 15;

            if (caption.IndexOf("high priority overdue", StringComparison.OrdinalIgnoreCase) >= 0)
                // Nota: O clique usa >= 5 dias tal como a contagem para ser consistente
                return t => !IsClosed(t) && CalculateDueDays(t.DueDate) >= 5 &&
                            !string.IsNullOrWhiteSpace(t.Priority) &&
                            (t.Priority.Equals("High", StringComparison.OrdinalIgnoreCase) || t.Priority.Equals("Critical", StringComparison.OrdinalIgnoreCase));

            return t => true;
        }

        private Func<APMActionPlanSubTask, bool> GetSubTaskPredicate(string caption)
        {
            if (string.IsNullOrWhiteSpace(caption)) return null;
            var normalized = caption.ToLowerInvariant().Trim();

            bool IsStatusLike(APMActionPlanSubTask st, string keyword)
            {
                return !string.IsNullOrWhiteSpace(st.Status) &&
                    st.Status.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            // Usar IsClosedExact para consistência
            bool IsSubTaskClosed(APMActionPlanSubTask st) => IsClosedExact(st.Status, st.IdLookupStatus);

            bool IsHighPriority(APMActionPlanSubTask st)
            {
                return !string.IsNullOrWhiteSpace(st.Priority) &&
                    (st.Priority.Equals("High", StringComparison.OrdinalIgnoreCase) || st.Priority.Equals("Critical", StringComparison.OrdinalIgnoreCase));
            }

            if (caption.IndexOf("to do", StringComparison.OrdinalIgnoreCase) >= 0)
                return st => !IsSubTaskClosed(st) && (IsStatusLike(st, "to do") || IsStatusLike(st, "open"));

            if (caption.IndexOf("in progress", StringComparison.OrdinalIgnoreCase) >= 0)
                return st => !IsSubTaskClosed(st) && (IsStatusLike(st, "in progress") || IsStatusLike(st, "working"));

            if (caption.IndexOf("blocked", StringComparison.OrdinalIgnoreCase) >= 0)
                return st => !IsSubTaskClosed(st) && (IsStatusLike(st, "blocked") || IsStatusLike(st, "hold"));

            if (caption.IndexOf("done", StringComparison.OrdinalIgnoreCase) >= 0)
                return st => IsSubTaskClosed(st);

            if (normalized.Contains("overdue>=15"))
                return st => !IsSubTaskClosed(st) && CalculateDueDays(st.DueDate) >= 15;

            if (caption.IndexOf("high priority overdue", StringComparison.OrdinalIgnoreCase) >= 0)
                return st => !IsSubTaskClosed(st) && CalculateDueDays(st.DueDate) > 0 && IsHighPriority(st);

            return st => true;
        }

        #endregion

        #region Helper Methods (from Old ViewModel)

        private int CalculateDueDays(DateTime dueDate)
        {
            if (dueDate >= DateTime.Now) return 0;
            return (int)(DateTime.Now - dueDate).TotalDays;
        }

        private bool IsClosedExact(string status, int idLookupStatus)
        {
            if (idLookupStatus == 1982 || idLookupStatus == 10003 || idLookupStatus == 10002) return true;

            if (string.IsNullOrWhiteSpace(status)) return false;

            var s = status.Trim().ToLowerInvariant();
            return s.Contains("done") ||
                   s.Contains("closed") ||
                   s.Contains("completed") || 
                   s.Contains("concluída");   
        }

        private class InExpression
        {
            public string Field { get; set; }
            public HashSet<string> Values { get; set; }
        }

        private bool HasBlanksSelected(List<object> list)
        {
            if (list == null || list.Count == 0) return false;
            try
            {
                return list.Any(o =>
                {
                    if (o == null) return true;

                    var str = o.ToString();
                    if (string.Equals(str, "(Blanks)", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(str, "Blanks", StringComparison.OrdinalIgnoreCase) ||
                        string.IsNullOrWhiteSpace(str))
                        return true;

                    if (o is APMCustomer customer)
                        return customer.IdSite == 0 ||
                            string.IsNullOrWhiteSpace(customer.GroupName) ||
                            string.Equals(customer.GroupName, "Blanks", StringComparison.OrdinalIgnoreCase);

                    return false;
                });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log("HasBlanksSelected error: " + ex.Message, Category.Exception, Priority.Low);
                return false;
            }
        }

        private HashSet<int> ToResponsibleIdSet(List<object> list)
        {
            if (list == null || list.Count == 0) return null;
            try
            {
                var ids = new HashSet<int>();
                foreach (var o in list)
                {
                    if (o == null) continue;
                    var type = o.GetType();
                    var prop =
                        type.GetProperty("ActionPlanResponsibleIdUser") ??
                        type.GetProperty("ResponsibleIdUser") ??
                        type.GetProperty("IdEmployee") ??
                        type.GetProperty("IdUser") ??
                        type.GetProperty("Id") ??
                        type.GetProperty("IdActionPlanResponsible");

                    if (prop == null) continue;
                    var val = prop.GetValue(o, null);
                    if (val == null) continue;
                    if (int.TryParse(val.ToString(), out var id))
                        ids.Add(id);
                }
                return ids.Count == 0 ? null : ids;
            }
            catch
            {
                return null;
            }
        }

        private HashSet<string> ToStringHashSet(List<object> list)
        {
            if (list == null || list.Count == 0) return null;
            return new HashSet<string>(list.Select(o => o?.ToString()).Where(s => !string.IsNullOrWhiteSpace(s)), StringComparer.OrdinalIgnoreCase);
        }

        private string GetLongestOverdueTaskCode(List<TaskOrSubTaskItem> items)
        {
            if (items == null || items.Count == 0) return null;

            int maxDays = 0;
            string code = null;

            foreach (var it in items)
            {
                try
                {
                    if (!IsClosedItem(it) && it.GetActualDueDays() > maxDays)
                    {
                        maxDays = it.GetActualDueDays();
                        // TaskOrSubTaskItem stores ActionPlanCode for subtask and task
                        code = it.ActionPlanCode;
                    }
                }
                catch { }
            }

            return code;
        }

        private bool IsSubTaskClosed(APMActionPlanSubTask st)
        {
            if (st == null) return false;
            if (st.IdLookupStatus == 10003 || st.IdLookupStatus == 10002 || st.IdLookupStatus == 1982) return true;
            if (!string.IsNullOrWhiteSpace(st.Status))
            {
                var ls = st.Status.ToLowerInvariant();
                return ls.Contains("done") || ls.Contains("closed") || ls.Contains("concluída") || ls.Contains("completed");
            }
            return false;
        }

        private int GetActualDueDays(APMActionPlanTask t)
        {
            if (t == null) return 0;
            try
            {
                if (t.DueDate >= DateTime.Now) return 0;
                return (int)(DateTime.Now - t.DueDate).TotalDays;
            }
            catch { return 0; }
        }

        private int GetActualDueDays(APMActionPlanSubTask st)
        {
            if (st == null) return 0;
            try
            {
                if (st.DueDate >= DateTime.Now) return 0;
                return (int)(DateTime.Now - st.DueDate).TotalDays;
            }
            catch { return 0; }
        }

        private HashSet<string> ToResponsibleHashSet(List<object> list)
        {
            if (list == null || list.Count == 0) return null;

            var names = new List<string>();
            foreach (var r in list.OfType<Responsible>())
            {
                if (!string.IsNullOrWhiteSpace(r.FullName)) names.Add(r.FullName.Trim());
                if (!string.IsNullOrWhiteSpace(r.ResponsibleDisplayName)) names.Add(r.ResponsibleDisplayName.Trim());

                var firstName = r.FirstName?.Trim();
                var lastName = r.LastName?.Trim();

                if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
                {
                    names.Add($"{firstName} {lastName}");
                    names.Add($"{lastName}, {firstName}");

                    var lastNameParts = lastName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (lastNameParts.Length > 1)
                    {
                        names.Add($"{firstName} {lastNameParts[lastNameParts.Length - 1]}");
                        names.Add($"{lastNameParts[lastNameParts.Length - 1]}, {firstName}");
                    }
                }
            }

            return new HashSet<string>(names.Where(s => !string.IsNullOrWhiteSpace(s)), StringComparer.OrdinalIgnoreCase);
        }

        private HashSet<string> ToCustomerHashSet(List<object> list)
        {
            if (list == null || list.Count == 0) return null;
            var names = list.OfType<APMCustomer>()
                .Select(c => c.GroupName)
                .Where(s => !string.IsNullOrWhiteSpace(s));
            return names.Any() ? new HashSet<string>(names, StringComparer.OrdinalIgnoreCase) : null;
        }

        private string SafePerson(APMActionPlanModern ap) =>
            ap.ActionPlanResponsibleDisplayName
            ?? ap.FullName
            ?? ap.Code;

        private List<InExpression> ParseCriteria(string criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria)) return new List<InExpression>();

            var result = new List<InExpression>();
            var parts = _andSplit.Split(criteria);

            foreach (var part in parts)
            {
                var inMatch = _inExpr.Match(part);
                if (inMatch.Success)
                {
                    var field = inMatch.Groups["f"].Value;
                    var valuesStr = inMatch.Groups["v"].Value;
                    var values = valuesStr.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => v.Trim().Trim('\'', '\"'))
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);
                    
                    result.Add(new InExpression { Field = field, Values = values });
                    continue;
                }

                var eqMatch = _eqExpr.Match(part);
                if (eqMatch.Success)
                {
                    var field = eqMatch.Groups["f"].Value;
                    var value = eqMatch.Groups["v"].Value;
                    var values = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { value };
                    result.Add(new InExpression { Field = field, Values = values });
                }
            }

            return result;
        }

        private bool PropertyMatches(object obj, string fieldName, HashSet<string> values)
        {
            if (obj == null) return false;
            var prop = obj.GetType().GetProperty(fieldName);
            if (prop == null) return false;
            var valObj = prop.GetValue(obj);
            if (valObj == null) return false;
            var valStr = valObj.ToString();
            return values.Contains(valStr);
        }

        private APMActionPlanModern ClonePlan(APMActionPlanModern source)
        {
            if (source == null) return null;
            return (APMActionPlanModern)source.Clone();
        }

        private void SetActionPlanList(List<APMActionPlanModern> plans)
        {
            try
            {
                if (plans == null)
                {
                    ActionPlans = new System.Collections.ObjectModel.ObservableCollection<ActionPlanModernDto>();
                    return;
                }

                // Convert APMActionPlanModernto ActionPlanModernDto
                var dtos = plans.Select(MapToDto).Where(dto => dto != null).ToList();
                ActionPlans = new System.Collections.ObjectModel.ObservableCollection<ActionPlanModernDto>(dtos);

                GeosApplication.Instance.Logger?.Log($"Set action plan list: {dtos.Count} plans", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in SetActionPlanList: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        // --- Substitui no ActionPlansModernViewModel.Filters.cs ---

        private async void ApplyInMemoryFiltersAsync(int debounceMs = 0)
        {
            try
            {
                // 1. Debounce
                if (debounceMs > 0)
                {
                    CancelOngoingFilter();
                    _filterCts = new CancellationTokenSource();
                    var token = _filterCts.Token;
                    await Task.Delay(debounceMs, token);
                    if (token.IsCancellationRequested) return;
                }

                IsBusy = true;

                // 2. Obter base de dados
                if (_apCache == null || !_apCache.Any())
                {
                    IsBusy = false;
                    return;
                }

                var query = _apCache.AsQueryable();

                // 3. Aplicar Filtros
                // Nota: Usei "Id..." que é o padrão comum. Se der erro, verifica na classe APMActionPlanModernse é "IdLocation" ou "LocationId"

                // -- Location --
                if (SelectedLocation != null && SelectedLocation.Count > 0)
                {
                    var locSet = ToStringHashSet(SelectedLocation);
                    if (locSet != null && locSet.Count > 0)
                    {
                        query = query.Where(x => locSet.Contains(x.Location));
                    }
                }

                // -- Business Unit --
                if (SelectedBusinessUnit != null && SelectedBusinessUnit.Count > 0)
                {
                    var buSet = ToStringHashSet(SelectedBusinessUnit);
                    if (buSet != null && buSet.Count > 0)
                    {
                        query = query.Where(x => buSet.Contains(x.BusinessUnit));
                    }
                }

                // -- Department --
                if (SelectedDepartment != null && SelectedDepartment.Count > 0)
                {
                    var deptSet = ToStringHashSet(SelectedDepartment);
                    if (deptSet != null && deptSet.Count > 0)
                    {
                        query = query.Where(x => deptSet.Contains(x.Department));
                    }
                }

                // -- Customer --
                if (SelectedCustomer != null && SelectedCustomer.Count > 0)
                {
                    var customerSet = ToCustomerHashSet(SelectedCustomer);
                    var customerIdSet = SelectedCustomer?.OfType<APMCustomer>().Select(c => c.IdSite).Where(id => id > 0).ToHashSet();
                    bool customerIncludesBlanks = HasBlanksSelected(SelectedCustomer);
                    
                    if (customerSet != null && customerSet.Count > 0 || customerIdSet != null && customerIdSet.Count > 0 || customerIncludesBlanks)
                    {
                        query = query.Where(x => 
                            (customerIncludesBlanks && x.IdSite == 0) ||
                            (customerIdSet != null && x.IdSite > 0 && customerIdSet.Contains(x.IdSite)) ||
                            (customerSet != null && !string.IsNullOrWhiteSpace(x.GroupName) && customerSet.Contains(x.GroupName)));
                    }
                }

                // -- Origin --
                if (SelectedOrigin != null && SelectedOrigin.Count > 0)
                {
                    var originSet = ToStringHashSet(SelectedOrigin);
                    if (originSet != null && originSet.Count > 0)
                    {
                        query = query.Where(x => originSet.Contains(x.Origin));
                    }
                }

                // -- Responsible (Person) --
                if (SelectedPerson != null && SelectedPerson.Count > 0)
                {
                    var personNameSet = ToResponsibleHashSet(SelectedPerson);
                    var personIdSet = ToResponsibleIdSet(SelectedPerson);
                    bool hasId = personIdSet != null && personIdSet.Count > 0;
                    bool hasName = personNameSet != null && personNameSet.Count > 0;
                    
                    if (hasId || hasName)
                    {
                        query = query.Where(x => 
                            (hasId && personIdSet.Contains(x.ResponsibleIdUser)) ||
                            (hasName && personNameSet.Contains(SafePerson(x))));
                    }
                }

                // -- Search String (Texto Livre) --
                // Nota: APMActionPlanModernnão tem Theme, Status, Priority, Title ou Reference
                // Essas propriedades existem em APMActionPlanTask
                // Para ActionPlan, filtramos por Code e Description
                if (!string.IsNullOrWhiteSpace(SearchString))
                {
                    var search = SearchString.Trim().ToLowerInvariant();
                    query = query.Where(x =>
                        (x.Code != null && x.Code.ToLowerInvariant().Contains(search)) ||
                        (x.Description != null && x.Description.ToLowerInvariant().Contains(search))
                    );
                }

                var filteredList = query.ToList();
                _currentFilteredBase = filteredList;

                // 4. Atualizar a Grid
                SetActionPlanList(filteredList);

                // 5. Atualizar os Tiles
                RecalculateAllCounts();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error filtering: {ex.Message}", Category.Exception, Priority.High);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region ExtractAllTasksAndSubTasksWithAllFilters

        /// <summary>
        /// Extrai todas Tasks E SubTasks como lista flat, aplicando todos os filtros de dropdown
        /// Baseado no OLD ViewModel
        /// </summary>
        private List<TaskOrSubTaskItem> ExtractAllTasksAndSubTasksWithAllFilters(
            List<APMActionPlanModern> plans,
            HashSet<int> personIdSet,
            HashSet<string> personNameSet,
            HashSet<string> locSet,
            HashSet<string> buSet,
            HashSet<string> originSet,
            HashSet<string> customerSet,
            HashSet<int> customerIdSet,
            HashSet<string> codeSet,
            bool customerIncludesBlanks)
        {
            var result = new List<TaskOrSubTaskItem>();
            if (plans == null || plans.Count == 0) return result;

            bool hasPersonId = personIdSet != null && personIdSet.Count > 0;
            bool hasPersonName = personNameSet != null && personNameSet.Count > 0;
            bool hasLoc = locSet != null && locSet.Count > 0;
            bool hasBu = buSet != null && buSet.Count > 0;
            bool hasOrigin = originSet != null && originSet.Count > 0;
            bool hasCustomer = customerSet != null && customerSet.Count > 0 || customerIdSet != null && customerIdSet.Count > 0 || customerIncludesBlanks;
            bool hasCode = codeSet != null && codeSet.Count > 0;

            foreach (var plan in plans)
            {
                // Filtros de ActionPlan
                if (hasLoc && !locSet.Contains(plan.Location)) continue;
                if (hasBu && !buSet.Contains(plan.BusinessUnit)) continue;
                if (hasOrigin && !originSet.Contains(plan.Origin)) continue;
                if (hasCode && !codeSet.Contains(plan.Code)) continue;
                
                if (hasCustomer)
                {
                    bool matchCustomer = (customerIncludesBlanks && plan.IdSite == 0) ||
                                       (customerIdSet != null && plan.IdSite > 0 && customerIdSet.Contains(plan.IdSite)) ||
                                       (customerSet != null && !string.IsNullOrWhiteSpace(plan.GroupName) && customerSet.Contains(plan.GroupName));
                    if (!matchCustomer) continue;
                }

                if (plan.TaskList == null || plan.TaskList.Count == 0) continue;

                foreach (var task in plan.TaskList)
                {
                    // Filtros de Task (Responsible)
                    bool taskMatchesPerson = !hasPersonId && !hasPersonName;
                    if (!taskMatchesPerson)
                    {
                        taskMatchesPerson = (hasPersonId && personIdSet.Contains(task.ResponsibleIdUser)) ||
                                          (hasPersonName && !string.IsNullOrWhiteSpace(task.Responsible) && personNameSet.Contains(task.Responsible));
                    }

                    if (taskMatchesPerson)
                    {
                        // Adiciona a Task
                        var taskItem = TaskOrSubTaskItem.FromTask(task);
                        if (taskItem != null) result.Add(taskItem);
                    }

                    // Adiciona SubTasks
                    if (task.SubTaskList != null && task.SubTaskList.Count > 0)
                    {
                        foreach (var subTask in task.SubTaskList)
                        {
                            bool subTaskMatchesPerson = !hasPersonId && !hasPersonName;
                            if (!subTaskMatchesPerson)
                            {
                                // CORREÇÃO: SubTasks usam IdEmployee, não ResponsibleIdUser
                                subTaskMatchesPerson = (hasPersonId && personIdSet.Contains(subTask.IdEmployee)) ||
                                                     (hasPersonName && !string.IsNullOrWhiteSpace(subTask.Responsible) && personNameSet.Contains(subTask.Responsible));
                            }

                            if (subTaskMatchesPerson)
                            {
                                var subTaskItem = TaskOrSubTaskItem.FromSubTask(subTask, task.ActionPlanCode);
                                if (subTaskItem != null) result.Add(subTaskItem);
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region UpdateAlertButtonCounts

        private void UpdateAlertButtonCounts()
        {
            UpdateAlertButtonCounts(null);
        }
        private void UpdateAlertButtonCounts(List<APMActionPlanModern> source)
        {
            try
            {
                if (AlertListOfFilterTile == null || AlertListOfFilterTile.Count == 0) return;

                // Preparar filtros (Mantém igual)
                var personIdSet = ToResponsibleIdSet(SelectedPerson);
                var personNameSet = ToResponsibleHashSet(SelectedPerson);
                var locSet = ToStringHashSet(SelectedLocation);
                var buSet = ToStringHashSet(SelectedBusinessUnit);
                var originSet = ToStringHashSet(SelectedOrigin);
                var customerSet = ToCustomerHashSet(SelectedCustomer);
                var customerIdSet = SelectedCustomer?.OfType<APMCustomer>().Select(c => c.IdSite).Where(id => id > 0).ToHashSet();
                var codeSet = ToStringHashSet(null);
                bool customerIncludesBlanks = HasBlanksSelected(SelectedCustomer);

                var allItems = ExtractAllTasksAndSubTasksWithAllFilters(
                    source, personIdSet, personNameSet,
                    locSet, buSet, originSet, customerSet, customerIdSet, codeSet,
                    customerIncludesBlanks);

                // --- CORREÇÃO: Tratamento de lista vazia ---
                if (allItems.Count == 0)
                {
                    foreach (var tile in AlertListOfFilterTile)
                    {
                        var cap = (tile.Caption ?? "").ToLowerInvariant();
                        // Se for botão de texto, põe "---", se for numérico põe "0"
                        if (cap.Contains("most overdue"))
                            tile.EntitiesCount = "---";
                        else
                            tile.EntitiesCount = "0";

                        tile.BackColor = "Green";
                        tile.EntitiesCountVisibility = Visibility.Visible;
                    }
                    return;
                }

                // --- CÁLCULOS (Lógica OLD com Aliases) ---

                // Helper local para verificar status com aliases
                bool HasStatus(TaskOrSubTaskItem t, string s1, string s2 = null)
                {
                    if (string.IsNullOrWhiteSpace(t.Status)) return false;
                    if (t.Status.IndexOf(s1, StringComparison.OrdinalIgnoreCase) >= 0) return true;
                    if (s2 != null && t.Status.IndexOf(s2, StringComparison.OrdinalIgnoreCase) >= 0) return true;
                    return false;
                }

                var overdueItems = allItems.Where(t => !IsClosedExact(t.Status, t.IdLookupStatus) && CalculateDueDays(t.DueDate) > 0).ToList();

                int longestOverdueDays = overdueItems.Any() ? overdueItems.Max(t => CalculateDueDays(t.DueDate)) : 0;
                int overdue15Count = allItems.Count(t => !IsClosedExact(t.Status, t.IdLookupStatus) && CalculateDueDays(t.DueDate) >= 15);

                int highPriorityOverdueCount = allItems.Count(t =>
                    !IsClosedExact(t.Status, t.IdLookupStatus) &&
                    CalculateDueDays(t.DueDate) >= 5 && // OLD usa >= 5 para High Prio
                    !string.IsNullOrWhiteSpace(t.Priority) &&
                    (t.Priority.Equals("High", StringComparison.OrdinalIgnoreCase) || t.Priority.Equals("Critical", StringComparison.OrdinalIgnoreCase)));

                // --- CÁLCULO DOS NOMES (Most Overdue) ---
                string mostTheme = null;
                string mostResp = null;
                int maxDaysTheme = 0;
                int maxDaysResp = 0;

                if (overdueItems.Any())
                {
                    // Tema
                    var themeGroup = overdueItems
                        .Where(t => !string.IsNullOrWhiteSpace(t.Theme))
                        .GroupBy(t => t.Theme.Trim())
                        .OrderByDescending(g => g.Count())
                        .FirstOrDefault();

                    if (themeGroup != null)
                    {
                        mostTheme = themeGroup.Key;
                        maxDaysTheme = themeGroup.Max(t => CalculateDueDays(t.DueDate));
                    }

                    // Responsável
                    var respGroup = overdueItems
                        .Where(t => !string.IsNullOrWhiteSpace(t.TaskResponsibleDisplayName ?? t.Responsible))
                        .GroupBy(t => (t.TaskResponsibleDisplayName ?? t.Responsible).Trim())
                        .OrderByDescending(g => g.Count())
                        .FirstOrDefault();

                    if (respGroup != null)
                    {
                        mostResp = respGroup.Key;
                        maxDaysResp = respGroup.Max(t => CalculateDueDays(t.DueDate));
                    }
                }

                // Status Counts
                int todoCount = allItems.Count(t => !IsClosedExact(t.Status, t.IdLookupStatus) && HasStatus(t, "to do", "open"));
                int inProgressCount = allItems.Count(t => !IsClosedExact(t.Status, t.IdLookupStatus) && HasStatus(t, "in progress", "working"));
                int blockedCount = allItems.Count(t => !IsClosedExact(t.Status, t.IdLookupStatus) && HasStatus(t, "blocked", "hold"));
                int doneCount = allItems.Count(t => IsClosedExact(t.Status, t.IdLookupStatus));

                // --- ATUALIZAÇÃO UI ---
                foreach (var tile in AlertListOfFilterTile)
                {
                    var caption = (tile.Caption ?? "").ToLowerInvariant();

                    if (tile.Id > 0)
                    { // IDs Customizados
                        tile.EntitiesCount = allItems.Count(t => t.IdLookupStatus == tile.Id).ToString();
                        tile.EntitiesCountVisibility = Visibility.Visible;
                        continue;
                    }

                    if (caption.Contains("longest overdue"))
                    {
                        tile.EntitiesCount = longestOverdueDays.ToString();
                        tile.BackColor = ColorByDays(longestOverdueDays);
                    }
                    else if (caption.Contains("overdue") && caption.Contains("15"))
                    {
                        tile.EntitiesCount = overdue15Count.ToString();
                        tile.BackColor = ColorByCount(overdue15Count);
                    }
                    else if (caption.Contains("high priority overdue"))
                    {
                        tile.EntitiesCount = highPriorityOverdueCount.ToString();
                        tile.BackColor = ColorByCount(highPriorityOverdueCount);
                    }
                    // --- AQUI SE GARANTE QUE APARECEM STRINGS ---
                    else if (caption.Contains("most overdue theme"))
                    {
                        tile.EntitiesCount = string.IsNullOrWhiteSpace(mostTheme) ? "---" : mostTheme;
                        tile.BackColor = ColorByDays(maxDaysTheme);
                    }
                    else if (caption.Contains("most overdue responsible"))
                    {
                        tile.EntitiesCount = string.IsNullOrWhiteSpace(mostResp) ? "---" : mostResp;
                        tile.BackColor = ColorByDays(maxDaysResp);
                    }
                    // --------------------------------------------
                    else if (caption.Contains("to do"))
                    {
                        tile.EntitiesCount = todoCount.ToString();
                        tile.BackColor = ColorByCount(todoCount);
                    }
                    else if (caption.Contains("in progress"))
                    {
                        tile.EntitiesCount = inProgressCount.ToString();
                        tile.BackColor = ColorByCount(inProgressCount);
                    }
                    else if (caption.Contains("blocked"))
                    {
                        tile.EntitiesCount = blockedCount.ToString();
                        tile.BackColor = ColorByCount(blockedCount);
                    }
                    else if (caption.Contains("done"))
                    {
                        tile.EntitiesCount = doneCount.ToString();
                        tile.BackColor = ColorByCount(doneCount);
                    }

                    tile.EntitiesCountVisibility = Visibility.Visible;
                }

                // Restaura seleção
                if (!string.IsNullOrWhiteSpace(_lastAppliedAlertCaption))
                {
                    var selected = AlertListOfFilterTile.FirstOrDefault(x => string.Equals(x.Caption, _lastAppliedAlertCaption, StringComparison.OrdinalIgnoreCase));
                    if (selected != null) SelectedAlertTileBarItem = selected;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in UpdateAlertButtonCounts: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        private bool IsClosed(string status, int idLookupStatus)
        {
            if (idLookupStatus == 10003 || idLookupStatus == 10002) return true;
            if (!string.IsNullOrWhiteSpace(status))
            {
                var ls = status.ToLowerInvariant();
                return ls.Contains("done") || ls.Contains("closed") || ls.Contains("concluída");
            }
            return false;
        }

        private string ColorByDays(int days)
        {
            if (days == 0) return "Green";
            if (days < 5) return "Yellow";
            if (days < 15) return "Orange";
            return "Red";
        }

        private string ColorByCount(int count)
        {
            if (count == 0) return "Green";
            if (count < 5) return "Yellow";
            if (count < 15) return "Orange";
            return "Red";
        }



        #endregion
    }
}
