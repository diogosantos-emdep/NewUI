using DevExpress.DataAccess.DataFederation;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Navigation;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.APM.ViewModels
{

    public partial class ActionPlansViewModel
    {

        private bool IsAlertActive()
        {
            return !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption);
        }
        private List<APMActionPlan> _currentFilteredBase;
        private List<APMActionPlan> _alertFilteredBase;
        private List<APMActionPlan> _baseForSideTileCounts;
        private bool? _lastViewIsTask;

        #region Cache
        private bool _apCacheLoaded;
        private List<APMActionPlan> _apCache;
        private string _lastPeriodIds;
        private bool _taskCacheLoaded;
        private List<APMActionPlanTask> _taskCache;
        private string _lastTaskPeriodIds;
        private List<Responsible> _responsibleCache;
        private List<Responsible> _taskResponsibleCache;
        private List<APMActionPlan> _sideCountsBaseForAll;
        private List<APMActionPlan> _sideCountsBaseForNonAll;
		private Dictionary<string, ImageSource> _inMemoryImageCache = new Dictionary<string, ImageSource>();



		private void InvalidateActionPlanCache()
        {
            _apCacheLoaded = false;
            _apCache = null;
            _lastPeriodIds = null;
        }

        private void InvalidateTaskCache()
        {
            _taskCacheLoaded = false;
            _taskCache = null;
            _lastTaskPeriodIds = null;
        }

        private void FillActionPlan()
        {
            try
            {
                var sw_FillActionPlan = System.Diagnostics.Stopwatch.StartNew();
                var periods = APMCommon.Instance.SelectedPeriod;
                if (periods == null || !periods.Cast<long>().Any())
                {
                    ActionPlanList = new ObservableCollection<APMActionPlan>();
                    sw_FillActionPlan.Stop();
                    //APMServiceLogger.LogServiceCall("FillActionPlan", sw_FillActionPlan.ElapsedMilliseconds, "No periods selected");
                    return;
                }
                string idPeriods = string.Join(",", periods.Cast<long>());
                EnsureCacheLoaded(idPeriods);
                SetActionPlanList(_apCache.Select(ap => (APMActionPlan)ap.Clone()).ToList());
                SetDeletionFlags(ActionPlanList);
                EnableFastFiltering();
                RewireFastFilterDropdownCommands();
                RewireAlertTileBarCommands();
                RewireSideTileBarCommands();
                RewireTopTileBarCommands();
                sw_FillActionPlan.Stop();
                //APMServiceLogger.LogServiceCall("FillActionPlan", sw_FillActionPlan.ElapsedMilliseconds, $"ResultCount: {ActionPlanList?.Count ?? 0}");
            }
            catch (Exception ex)
            {
                try
                {

                    //APMServiceLogger.LogServiceCall("FillActionPlan", 0, "Error: " + ex.Message);
                }
                catch { }
                GeosApplication.Instance.Logger.Log("FillActionPlan error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }


		public async Task LoadResponsibleImagesBackgroundAsync(System.Collections.IEnumerable people)
		{
			if (people == null) return;

			var peopleList = people.OfType<Responsible>().ToList();
			var tasks = new List<Task>();

			string baseUrlRounded = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/Rounded/";
			string baseUrlNormal = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/";

			string currentTheme = "DeepBlue";
			System.Windows.Application.Current.Dispatcher.Invoke(() =>
			{
				currentTheme = ApplicationThemeHelper.ApplicationThemeName;
			});

			foreach (var person in peopleList)
			{

				if (!string.IsNullOrEmpty(person.EmployeeCode) && _inMemoryImageCache.TryGetValue(person.EmployeeCode, out var cachedImg))
				{
					if (person.OwnerImage == null)
					{
						person.OwnerImage = cachedImg;
					}
					continue; 
				}

				var t = Task.Run(async () =>
				{
					if (string.IsNullOrEmpty(person.EmployeeCode)) return;
					if (person.OwnerImage != null) return;

					string url1 = $"{baseUrlRounded}{person.EmployeeCode}.png";
					string url2 = $"{baseUrlNormal}{person.EmployeeCode}.png";

					var img = await Emdep.Geos.Modules.APM.CommonClasses.ImageCacheService.Instance.GetImageAsync(person.EmployeeCode, url1, url2);

					if (img == null)
					{
						img = GetLocalFallbackImage(person.IdGender, currentTheme);
					}

					if (img != null)
					{
						System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
						{
							if (!_inMemoryImageCache.ContainsKey(person.EmployeeCode))
							{
								_inMemoryImageCache[person.EmployeeCode] = img;
							}

							if (person.OwnerImage == null)
							{
								person.OwnerImage = img;
							}
						}), System.Windows.Threading.DispatcherPriority.ContextIdle);
					}
				});
				tasks.Add(t);
			}

			await Task.WhenAll(tasks);
		}
		private ImageSource GetLocalFallbackImage(int? idGender, string themeName)
        {
            string imagePath = "";
            bool isBlackAndBlue = string.Equals(themeName, "BlackAndBlue", StringComparison.OrdinalIgnoreCase);

            
            bool isFemale = (idGender == 1);

            if (isBlackAndBlue)
            {
                if (isFemale)
                    imagePath = "pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserWhite.png";
                else
                    imagePath = "pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserWhite.png";
            }
            else
            {
                if (isFemale)
                    imagePath = "pack://application:,,,/GeosWorkbench;component/Assets/Images/femaleUserBlue.png";
                else
                    imagePath = "pack://application:,,,/GeosWorkbench;component/Assets/Images/maleUserBlue.png";
            }

            try
            {
                var uri = new Uri(imagePath, UriKind.Absolute);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = uri;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze(); 
                return bitmap;
            }
            catch
            {
                return null; 
            }
        }

        private void SetDeletionFlags(IEnumerable<APMActionPlan> plans)
        {
            if (plans == null) return;
            try
            {
                var currentUserId = GeosApplication.Instance.ActiveUser.IdUser;
                var hasPermission = GeosApplication.Instance.IsAPMActionPlanPermission;

                foreach (var ap in plans)
                {
                    if (ap == null) continue;

                    if (ap.CreatedBy == currentUserId || hasPermission)
                        ap.IsActionPlanDeleted = true;
                    else
                        ap.IsActionPlanDeleted = false;

                    if (ap.TaskList != null)
                    {
                        foreach (var t in ap.TaskList)
                        {
                            if (t == null) continue;

                            if (t.CreatedBy == currentUserId ||
                                ap.CreatedBy == currentUserId ||
                                hasPermission)
                            {
                                t.IsTaskDeleted = true;
                            }
                            else
                            {
                                t.IsTaskDeleted = false;
                            }

                            if (t.SubTaskList != null)
                            {
                                foreach (var st in t.SubTaskList)
                                {
                                    if (st == null) continue;

                                    st.IsSubTaskDeleted = (st.CreatedBy == currentUserId ||
                                                          t.CreatedBy == currentUserId ||
                                                          ap.CreatedBy == currentUserId ||
                                                          hasPermission);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("SetDeletionFlags error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }


        private void EnsureCacheLoaded(string idPeriods)
        {
            if (_apCacheLoaded && string.Equals(_lastPeriodIds, idPeriods, StringComparison.Ordinal)) return;
            try
            {
                //IAPMService APMService = new APMServiceController("localhost:6699");
                //_apCache = new List<APMActionPlan>(APMService.GetActionPlanDetails_V2670(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));
                _apCache = new List<APMActionPlan>(APMService.GetActionPlanDetails_V2680(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));
                TempActionPlanList = _apCache.Select(i => (APMActionPlan)i.Clone()).ToList();
                _apCacheLoaded = true;
                _lastPeriodIds = idPeriods;
                BuildResponsibleCacheFromApCache();
            }
            catch (Exception ex)
            {
                _apCacheLoaded = false;
                GeosApplication.Instance.Logger.Log("EnsureCacheLoaded error: " + ex.Message, Category.Exception, Priority.Low);
                _apCache = new List<APMActionPlan>();
            }
        }

        private void EnsureTaskCacheLoaded(string periodIds)
        {
            try
            {
                if (_taskCacheLoaded && string.Equals(_lastTaskPeriodIds, periodIds, StringComparison.Ordinal))
                    return;

                //var tasks = APMService.GetActionPlanTaskDetails_V2670(periodIds, GeosApplication.Instance.ActiveUser.IdUser);
                var tasks = APMService.GetActionPlanTaskDetails_V2680(periodIds, GeosApplication.Instance.ActiveUser.IdUser);
                _taskCache = tasks?.Select(t => (APMActionPlanTask)t.Clone()).ToList() ?? new List<APMActionPlanTask>();

                _taskCacheLoaded = true;
                _lastTaskPeriodIds = periodIds;

                BuildTaskResponsibleCacheFromTaskCache();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("EnsureTaskCacheLoaded error: " + ex.Message, Category.Exception, Priority.Low);
                _taskCacheLoaded = false;
            }
        }

        private void BuildResponsibleCacheFromApCache()
        {
            var sw_BuildResponsible = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var temp = new List<Responsible>();

                if (_apCache == null)
                {
                    _responsibleCache = new List<Responsible>();
                    return;
                }

                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var ap in _apCache)
                {
                    if (ap == null) continue;

                    if (!string.IsNullOrWhiteSpace(ap.EmployeeCode))
                    {
                        var key = ap.EmployeeCode + "_" + ap.IdGender;
                        if (seen.Add(key))
                        {
                            temp.Add(new Responsible
                            {
                                IdEmployee = (UInt32)ap.IdEmployee,
                                EmployeeCode = ap.EmployeeCode,
                                IdGender = ap.IdGender,
                                FullName = ap.FullName,
                                EmployeeCodeWithIdGender = key,
                                IsTaskField = false,
                                ResponsibleDisplayName = ap.ActionPlanResponsibleDisplayName ?? ap.FullName
                            });
                        }
                    }

                    if (ap.TaskList != null)
                    {
                        foreach (var t in ap.TaskList)
                        {
                            if (t == null) continue;

                            if (!string.IsNullOrWhiteSpace(t.EmployeeCode))
                            {
                                var key = t.EmployeeCode + "_" + t.IdGender;
                                int effectiveId =
                                    t.ActionPlanResponsibleIdUser != 0 ? t.ActionPlanResponsibleIdUser :
                                    t.IdActionPlanResponsible != 0 ? t.IdActionPlanResponsible :
                                    t.ResponsibleIdUser != 0 ? t.ResponsibleIdUser : t.IdEmployee;

                                if (seen.Add(key))
                                {
                                    temp.Add(new Responsible
                                    {
                                        IdEmployee = (UInt32)effectiveId,
                                        EmployeeCode = t.EmployeeCode,
                                        IdGender = t.IdGender,
                                        FullName = t.Responsible,
                                        EmployeeCodeWithIdGender = key,
                                        IsTaskField = true,
                                        ResponsibleDisplayName = t.TaskResponsibleDisplayName ?? t.Responsible
                                    });
                                }
                            }

                            if (t.SubTaskList != null)
                            {
                                foreach (var st in t.SubTaskList)
                                {
                                    if (st == null || string.IsNullOrWhiteSpace(st.EmployeeCode)) continue;

                                    var subKey = st.EmployeeCode + "_" + st.IdGender;
                                   
                                    if (seen.Add(subKey))
                                    {
                                        temp.Add(new Responsible
                                        {
                                            IdEmployee = (UInt32)st.IdEmployee,
                                            EmployeeCode = st.EmployeeCode,
                                            IdGender = st.IdGender,
                                            FullName = st.Responsible,
                                            EmployeeCodeWithIdGender = subKey,
                                            IsTaskField = true, 
                                            ResponsibleDisplayName = st.Responsible
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                _responsibleCache = temp
                    .OrderBy(r => r.ResponsibleDisplayName, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(r => r.EmployeeCode, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("BuildResponsibleCacheFromApCache error: " + ex.Message, Category.Exception, Priority.Low);
                _responsibleCache = new List<Responsible>();
            }
            finally
            {
                sw_BuildResponsible.Stop();
                try { } catch { }
            }
        }


        private void BuildTaskResponsibleCacheFromTaskCache()
        {
            var sw_BuildTaskResponsible = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                if (_taskCache == null || _taskCache.Count == 0)
                {
                    _taskResponsibleCache = new List<Responsible>();
                    return;
                }

                var temp = new List<Responsible>();
                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var task in _taskCache)
                {
                    if (task == null) continue;
                    if (string.IsNullOrWhiteSpace(task.EmployeeCode)) continue;

                    var key = task.EmployeeCode + "_" + task.IdGender;
                    if (!seen.Add(key)) continue;

                    var employeeId = task.IdEmployee;

                    temp.Add(new Responsible
                    {
                        IdEmployee = (uint)employeeId,
                        EmployeeCode = task.EmployeeCode,
                        IdGender = task.IdGender,
                        FullName = task.Responsible,
                        EmployeeCodeWithIdGender = key,
                        IsTaskField = true,
                        ResponsibleDisplayName = task.TaskResponsibleDisplayName ?? task.Responsible
                    });
                }

                _taskResponsibleCache = temp
                    .OrderBy(r => r.ResponsibleDisplayName, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(r => r.EmployeeCode, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("BuildTaskResponsibleCacheFromTaskCache error: " + ex.Message, Category.Exception, Priority.Low);
                _taskResponsibleCache = new List<Responsible>();
            }
            finally
            {
                sw_BuildTaskResponsible.Stop();
                //APMServiceLogger.LogServiceCall("BuildTaskResponsibleCacheFromTaskCache", sw_BuildTaskResponsible.ElapsedMilliseconds, $"TaskResponsibles: {_taskResponsibleCache?.Count ?? 0}");
            }
        }

        #endregion


        private bool _fastFilteringEnabled;
        private CancellationTokenSource _filterCts;
        private bool _fastFilterCommandsRewired;
        private const int FastFilterDebounceMs = 60;

        private readonly HashSet<string> _filterTriggerProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
      "SelectedLocation","SelectedPerson","SelectedBussinessUnit","SelectedOrigin","SelectedDepartment","SelectedCustomer","SelectedActionPlansCode",
     "SelectedLocationForTask","SelectedPersonForTask","SelectedBussinessUnitForTask","SelectedOriginForTask","SelectedCustomerForTask","SelectedActionPlansCodeForTask"
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

        private void RewireFastFilterDropdownCommands()
        {
            if (_fastFilterCommandsRewired) return;
            try
            {
                LocationListClosedCommand = MakeClosedCommand("SelectedLocation");
                ResponsibleListClosedCommand = MakeClosedCommand("SelectedPerson");
                BusinessUnitListClosedCommand = MakeClosedCommand("SelectedBussinessUnit");
                OriginListClosedCommand = MakeClosedCommand("SelectedOrigin");
                SelectedDepartmentListClosedCommand = MakeClosedCommand("SelectedDepartment");
                SelectedCustomerListClosedCommand = MakeClosedCommand("SelectedCustomer");
                SelectedActionPlansCodeClosedCommand = MakeClosedCommand("SelectedActionPlansCode");

                LocationListClosedCommandForTask = MakeClosedCommand("SelectedLocationForTask");
                ResponsibleListClosedCommandForTask = MakeClosedCommand("SelectedPersonForTask");
                BusinessUnitListClosedCommandForTask = MakeClosedCommand("SelectedBussinessUnitForTask");
                OriginListClosedCommandForTask = MakeClosedCommand("SelectedOriginForTask");
                CustomerListClosedCommandForTask = MakeClosedCommand("SelectedCustomerForTask");
                SelectedActionPlansCodeClosedCommandForTask = MakeClosedCommand("SelectedActionPlansCodeForTask");

                _fastFilterCommandsRewired = true;
                GeosApplication.Instance.Logger.Log("Fast filter dropdown command rewire done", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("RewireFastFilterDropdownCommands error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }
        private ICommand MakeClosedCommand(string propertyName) => new DevExpress.Mvvm.DelegateCommand<object>(_ => OnMultiSelectClosed(propertyName));
        private void OnMultiSelectClosed(string propertyName)
        {
            try
            {
                UpdateAllSelectionStates(propertyName);
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ApplyInMemoryFiltersAsync(FastFilterDebounceMs);
                }), System.Windows.Threading.DispatcherPriority.ContextIdle);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("OnMultiSelectClosed(" + propertyName + ") error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private void UpdateAllSelectionStates(string propertyName)
        {
            try
            {
                switch (propertyName)
                {
                    case "SelectedLocation":
                        IsActionPlanAllLocationSelected = false;
                        break;
                    case "SelectedPerson":
                        IsActionPlanAllResponsibleSelected = false;
                        break;
                    case "SelectedBussinessUnit":
                        IsActionPlanAllBUSelected = false;
                        break;
                    case "SelectedOrigin":
                        IsActionPlanAllOriginSelected = false;
                        break;
                    case "SelectedDepartment":
                        IsActionPlanAllDepartmentSelected = false;
                        break;
                    case "SelectedCustomer":
                        IsActionPlanAllCustomerSelected = false;
                        break;
                    case "SelectedActionPlansCode":
                        IsActionPlanAllActionPlanSelected = false;
                        break;
                    case "SelectedLocationForTask":
                        IsActionPlanAllLocationSelectedForTask = false;
                        break;
                    case "SelectedPersonForTask":
                        IsActionPlanAllResponsibleSelectedForTask = false;
                        break;
                    case "SelectedBussinessUnitForTask":
                        IsActionPlanAllBUSelectedForTask = false;
                        break;
                    case "SelectedOriginForTask":
                        IsActionPlanAllOriginSelectedForTask = false;
                        break;
                    case "SelectedCustomerForTask":
                        IsActionPlanAllCustomerSelectedForTask = false;
                        break;
                    case "SelectedActionPlansCodeForTask":
                        IsActionPlanAllActionPlanSelectedForTask = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("UpdateAllSelectionStates error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }
        private void ResetCustomFiltersState()
        {
            try
            {

                _lastAppliedTopFilter = null;
                _lastAppliedTaskTopFilter = null;
                MyFilterString = string.Empty;
                MyTaskFilterString = string.Empty;
                CustomFilterStringName = "All";
                CustomTaskFilterStringName = "All";


                if (TopListOfFilterTile != null)
                {
                    var allTile = TopListOfFilterTile.FirstOrDefault(x => IsAllCaption(x.Caption));
                    if (allTile != null)
                        SelectedTopTileBarItem = allTile;
                }


                if (TopTaskListOfFilterTile != null)
                {
                    var allTaskTile = TopTaskListOfFilterTile.FirstOrDefault(x => IsAllCaption(x.Caption));
                    if (allTaskTile != null)
                        SelectedTaskTopTileBarItem = allTaskTile;
                }

                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedTopTileBarItem)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedTaskTopTileBarItem)));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("ResetCustomFiltersState error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }
        private void ApplyInMemoryFiltersAsync(int debounceMs = 0)
        {
            if (_lastViewIsTask == null || _lastViewIsTask.Value != IsTaskGridVisibility)
            {
                _lastViewIsTask = IsTaskGridVisibility;
                _lastAppliedSideCaption = null;
                _lastAppliedSideTileFilter = null;
                SelectedTileBarItem = null;
                PreviousSelectedTileBarItem = null;
                TemPreviousSelectedTileBarItem = null;

                _lastAppliedAlertCaption = null;
                _alertFilteredBase = null;
                _dataBeforeAlertFilter = null;
                PreviousSelectedAlertTileBarItem = null;
                SelectedAlertTileBarItem = null;
                PreviousSelectedActionPlansAlertTileBarItem = null;
                SelectedActionPlansAlertTileBarItem = null;
            }

            try
            {
                _suppressSideTileCountUpdate = false;
                _pendingAllWhileAlert = false;
                _sideTileCountsSnapshot = null;

                CancelOngoingFilter();
                _filterCts = new CancellationTokenSource();
                var token = _filterCts.Token;

                bool isTaskView = IsTaskGridVisibility;

                HashSet<string> locSet = null, buSet = null, originSet = null, deptSet = null, customerSet = null, codeSet = null;
                HashSet<int> personIdSet = null, customerIdSet = null;
                HashSet<string> personNameSet = null;
                bool customerIncludesBlanks = false;

                HashSet<string> locTaskSet = null, buTaskSet = null, originTaskSet = null, customerTaskSet = null, codeTaskSet = null;
                HashSet<int> personTaskIdSet = null, customerTaskIdSet = null;
                HashSet<string> personTaskNameSet = null;
                bool customerTaskIncludesBlanks = false;

                if (!isTaskView)
                {
                    locSet = ToStringHashSet(SelectedLocation);
                    personNameSet = ToResponsibleHashSet(SelectedPerson);
                    buSet = ToStringHashSet(SelectedBussinessUnit);
                    originSet = ToStringHashSet(SelectedOrigin);
                    deptSet = ToStringHashSet(SelectedDepartment);
                    customerSet = ToCustomerHashSet(SelectedCustomer);
                    codeSet = ToStringHashSet(SelectedActionPlansCode);
                    personIdSet = ToResponsibleIdSet(SelectedPerson);
                    customerIdSet = SelectedCustomer?
                        .OfType<APMCustomer>()
                        .Select(c => c.IdSite)
                        .Where(id => id > 0)
                        .ToHashSet() ?? null;
                    customerIncludesBlanks = HasBlanksSelected(SelectedCustomer);
                }
                else
                {
                    locTaskSet = ToStringHashSet(SelectedLocationForTask);
                    personTaskNameSet = ToResponsibleHashSet(SelectedPersonForTask);
                    buTaskSet = ToStringHashSet(SelectedBussinessUnitForTask);
                    originTaskSet = ToStringHashSet(SelectedOriginForTask);
                    customerTaskSet = ToCustomerHashSet(SelectedCustomerForTask);
                    codeTaskSet = ToStringHashSet(SelectedActionPlansCodeForTask);
                    personTaskIdSet = ToResponsibleIdSet(SelectedPersonForTask);
                    customerTaskIdSet = SelectedCustomerForTask?
                        .OfType<APMCustomer>()
                        .Select(c => c.IdSite)
                        .Where(id => id > 0)
                        .ToHashSet() ?? null;
                    customerTaskIncludesBlanks = HasBlanksSelected(SelectedCustomerForTask);
                }

               
                var source = (_apCache != null
                    ? _apCache
                    : ActionPlanList?.Select(ap => (APMActionPlan)ap.Clone()).ToList())
                    ?? new List<APMActionPlan>();

                if (debounceMs > 0)
                {
                    Task.Delay(debounceMs, token).ContinueWith(_ => { }, token);
                }


                Task.Run(() =>
                {
                    try
                    {
                        IEnumerable<APMActionPlan> query = source;

                        bool Matches(HashSet<string> set, string value) =>
                            set == null || set.Count == 0 || (!string.IsNullOrWhiteSpace(value) && set.Contains(value));

                       
                        if (!isTaskView)
                        {
                            // [shweta.thube][GEOS2-10088][14.11.2025]
                            if (locSet != null && locSet.Count > 0)
                            {
                                query = query
                                    .Where(ap =>
                                        locSet.Contains(ap.Location)
                                        ||
                                        (ap.TaskList != null && ap.TaskList.Any(t =>
                                            locSet.Contains(t.Location)
                                            || (t.SubTaskList != null && t.SubTaskList.Any(st => locSet.Contains(st.Location)))
                                        ))
                                    )
                                    .Select(ap =>
                                    {
                                        var clone = (APMActionPlan)ap.Clone();
                                        if (clone.TaskList != null)
                                        {
                                            clone.TaskList = clone.TaskList
                                                .Where(t =>
                                                    locSet.Contains(t.Location)
                                                    || (t.SubTaskList != null && t.SubTaskList.Any(st => locSet.Contains(st.Location)))
                                                )
                                                .Select(t =>
                                                {
                                                    var tClone = (APMActionPlanTask)t.Clone();
                                                    if (tClone.SubTaskList != null)
                                                    {
                                                        tClone.SubTaskList = tClone.SubTaskList
                                                            .Where(st => locSet.Contains(st.Location))
                                                            .ToList();
                                                    }
                                                    return tClone;
                                                })
                                                .ToList();
                                        }
                                        return clone;
                                    });
                            }

                            if ((personIdSet != null && personIdSet.Count > 0) || (personNameSet != null && personNameSet.Count > 0))
                            {
                                bool hasId = personIdSet != null && personIdSet.Count > 0;
                                bool hasName = personNameSet != null && personNameSet.Count > 0;

                                query = query
                                    .Where(ap =>
                                        // 1. NOVO: Verifica se a pessoa filtrada é a dona do Action Plan (Nível Pai)
                                        (hasId && personIdSet.Contains(ap.IdEmployee))
                                        || (hasName && personNameSet.Contains(ap.ActionPlanResponsibleDisplayName ?? ap.FullName))
                                        ||
                                        // 2. EXISTENTE: Verifica se a pessoa tem tarefas ou sub-tarefas (Níveis Filhos)
                                        (ap.TaskList != null && ap.TaskList.Any(t =>
                                            (hasId && personIdSet.Contains(t.IdEmployee))
                                            || (hasName && personNameSet.Contains(t.TaskResponsibleDisplayName ?? t.Responsible))
                                            || (t.SubTaskList != null && t.SubTaskList.Any(st =>
                                                (hasId && personIdSet.Contains(st.IdEmployee))
                                                || (hasName && personNameSet.Contains(st.TaskResponsibleDisplayName ?? st.Responsible))
                                            ))
                                        ))
                                    )
                                    .Select(ap =>
                                    {
                                        var clone = (APMActionPlan)ap.Clone();

                                        if (clone.TaskList != null)
                                        {
                                            clone.TaskList = clone.TaskList
                                                .Where(t =>
                                                    (hasId && personIdSet.Contains(t.IdEmployee))
                                                    || (hasName && personNameSet.Contains(t.TaskResponsibleDisplayName ?? t.Responsible))
                                                    || (t.SubTaskList != null && t.SubTaskList.Any(st =>
                                                        (hasId && personIdSet.Contains(st.IdEmployee))
                                                        || (hasName && personNameSet.Contains(st.TaskResponsibleDisplayName ?? st.Responsible))
                                                    ))
                                                )
                                                .Select(t =>
                                                {
                                                    var tClone = (APMActionPlanTask)t.Clone();
                                                    if (tClone.SubTaskList != null)
                                                    {
                                                        tClone.SubTaskList = tClone.SubTaskList
                                                            .Where(st =>
                                                                (hasId && personIdSet.Contains(st.IdEmployee))
                                                                || (hasName && personNameSet.Contains(st.TaskResponsibleDisplayName ?? st.Responsible))
                                                            )
                                                            .ToList();
                                                    }
                                                    return tClone;
                                                })
                                                .ToList();
                                        }
                                        return clone;
                                    });
                            }

                            if (buSet != null && buSet.Count > 0)
                                query = query.Where(ap =>
                                    Matches(buSet, ap.BusinessUnit) ||
                                    (ap.TaskList != null && ap.TaskList.Any(t => Matches(buSet, t.BusinessUnit))));

                            if (originSet != null && originSet.Count > 0)
                                query = query.Where(ap =>
                                    Matches(originSet, ap.Origin) ||
                                    (ap.TaskList != null && ap.TaskList.Any(t => Matches(originSet, t.Origin) || Matches(originSet, t.TaskOrigin))));

                            if (deptSet != null && deptSet.Count > 0)
                                query = query.Where(ap => Matches(deptSet, ap.Department));

                            if ((customerSet != null && customerSet.Count > 0) ||
                                (customerIdSet != null && customerIdSet.Count > 0) ||
                                customerIncludesBlanks)
                            {
                                query = query.Where(ap =>
                                    (customerIncludesBlanks && ap.IdSite == 0)
                                    || (customerIdSet != null && ap.IdSite > 0 && customerIdSet.Contains(ap.IdSite))
                                    || (customerSet != null && !string.IsNullOrWhiteSpace(ap.GroupName) && customerSet.Contains(ap.GroupName))
                                );
                            }

                            if (codeSet != null && codeSet.Count > 0)
                                query = query.Where(ap => Matches(codeSet, ap.Code));

                            var baseResult = query.OrderBy(ap => ap.Code).ToList();
                            token.ThrowIfCancellationRequested();

                            if (_lastAppliedSideTileFilter != null && !IsAllCaption(_lastAppliedSideTileFilter.Caption))
                            {
                                try
                                {
                                    var expressions = ParseCriteria(_lastAppliedSideTileFilter.FilterCriteria);
                                    if (expressions != null && expressions.Count > 0)
                                    {
                                        bool TaskMatchesAll(APMActionPlan ap, APMActionPlanTask t) =>
                                            expressions.All(expr => PropertyMatches(t, expr.Field, expr.Values) || PropertyMatches(ap, expr.Field, expr.Values));

                                        var sideFiltered = new List<APMActionPlan>();
                                        foreach (var ap in baseResult)
                                        {
                                            if (ap.TaskList == null || ap.TaskList.Count == 0) continue;
                                            var tasks = ap.TaskList.Where(t => TaskMatchesAll(ap, t)).ToList();
                                            if (tasks.Count == 0) continue;
                                            var clone = (APMActionPlan)ap.Clone();
                                            clone.TaskList = tasks;
                                            sideFiltered.Add(clone);
                                        }
                                        return new Tuple<List<APMActionPlan>, List<APMActionPlan>>(baseResult, sideFiltered);
                                    }
                                }
                                catch (Exception sx)
                                {
                                    GeosApplication.Instance.Logger.Log("Side tile intersection error: " + sx.Message, Category.Exception, Priority.Low);
                                }
                            }

                            return new Tuple<List<APMActionPlan>, List<APMActionPlan>>(baseResult, baseResult);
                        }
                        
                        else
                        {
                            var taskSource = source
                                .Where(ap => ap.TaskList != null && ap.TaskList.Count > 0)
                                .SelectMany(ap => ap.TaskList.Select(tk =>
                                {
                                    var clone = (APMActionPlanTask)tk.Clone();
                                    if (string.IsNullOrWhiteSpace(clone.GroupName))
                                        clone.GroupName = ap.GroupName;
                                    if (string.IsNullOrWhiteSpace(clone.Site))
                                        clone.Site = ap.Site;
                                    if (clone.IdSite == 0)
                                        clone.IdSite = ap.IdSite;
                                    return clone;
                                }))
                                .ToList();

                            IEnumerable<APMActionPlanTask> queryTasks = taskSource;

                            if (codeTaskSet != null && codeTaskSet.Count > 0)
                                queryTasks = queryTasks.Where(t => codeTaskSet.Contains(t.Code));

                            if (locTaskSet != null && locTaskSet.Count > 0)
                            {
                                queryTasks = queryTasks.Where(t =>
                                                         locTaskSet.Contains(t.Location)
                                                   || (t.SubTaskList != null && t.SubTaskList.Any(st => locTaskSet.Contains(st.Location)))
                                                                   )
                                                          .Select(t =>
                                                          {
                                                              var tClone = (APMActionPlanTask)t.Clone();
                                                              if (tClone.SubTaskList != null)
                                                              {
                                                                  tClone.SubTaskList = tClone.SubTaskList
                                                                .Where(st => locTaskSet.Contains(st.Location))
                                                                       .ToList();
                                                              }
                                                              return tClone;
                                                          });
                            }

                            bool hasIdFilter = personTaskIdSet != null && personTaskIdSet.Count > 0;
                            bool hasNameFilter = personTaskNameSet != null && personTaskNameSet.Count > 0;

                            if (hasIdFilter)
                            {
                                queryTasks = queryTasks
                                    .Where(t =>
                                        personTaskIdSet.Contains(t.IdEmployee) ||
                                        (t.SubTaskList != null && t.SubTaskList.Any(st => personTaskIdSet.Contains(st.IdEmployee)))
                                    )
                                    .Select(t =>
                                    {
                                        var clone = (APMActionPlanTask)t.Clone();
                                        bool taskIsOwnedByPerson = personTaskIdSet.Contains(t.IdEmployee);
                                        if (!taskIsOwnedByPerson && clone.SubTaskList != null)
                                        {
                                            clone.SubTaskList = clone.SubTaskList
                                                .Where(st => personTaskIdSet.Contains(st.IdEmployee))
                                                .ToList();
                                        }
                                        return clone;
                                    });
                            }
                            else if (hasNameFilter)
                            {
                                queryTasks = queryTasks
                                    .Where(t =>
                                        personTaskNameSet.Contains(t.TaskResponsibleDisplayName ?? t.Responsible) ||
                                        (t.SubTaskList != null && t.SubTaskList.Any(st => personTaskNameSet.Contains(st.TaskResponsibleDisplayName ?? st.Responsible)))
                                    )
                                    .Select(t =>
                                    {
                                        var clone = (APMActionPlanTask)t.Clone();
                                        bool taskIsOwnedByPerson = personTaskNameSet.Contains(t.TaskResponsibleDisplayName ?? t.Responsible);
                                        if (!taskIsOwnedByPerson && clone.SubTaskList != null)
                                        {
                                            clone.SubTaskList = clone.SubTaskList
                                                .Where(st => personTaskNameSet.Contains(st.TaskResponsibleDisplayName ?? st.Responsible))
                                                .ToList();
                                        }
                                        return clone;
                                    });
                            }

                            if (buTaskSet != null && buTaskSet.Count > 0)
                                queryTasks = queryTasks.Where(t => buTaskSet.Contains(t.BusinessUnit));

                            if (originTaskSet != null && originTaskSet.Count > 0)
                                queryTasks = queryTasks.Where(t =>
                                    originTaskSet.Contains(t.TaskOrigin) ||
                                    originTaskSet.Contains(t.Origin));

                            bool hasCustomerNames = customerTaskSet != null && customerTaskSet.Count > 0;
                            bool hasCustomerIds = customerTaskIdSet != null && customerTaskIdSet.Count > 0;

                            if (hasCustomerNames || hasCustomerIds || customerTaskIncludesBlanks)
                            {
                                queryTasks = queryTasks.Where(t =>
                                    (hasCustomerIds && customerTaskIdSet.Contains(t.IdSite)) ||
                                    (hasCustomerNames && !string.IsNullOrWhiteSpace(t.GroupName) && customerTaskSet.Contains(t.GroupName)) ||
                                    (customerTaskIncludesBlanks && string.IsNullOrWhiteSpace(t.GroupName) && t.IdSite == 0)
                                );
                            }

                            var finalTasks = queryTasks
                                .OrderBy(t => t.Code)
                                .ThenBy(t => t.NumItem)
                                .Select(t => (APMActionPlanTask)t.Clone())
                                .ToList();

                            
                            var baseResultDict = source.ToDictionary(k => Convert.ToInt64(k.IdActionPlan), v => v);

                            List<APMActionPlan> apsFromVisibleTasks;
                            if (finalTasks.Count > 0)
                            {
                                apsFromVisibleTasks = finalTasks
                                    .GroupBy(task => Convert.ToInt64(task.IdActionPlan))
                                    .Select(group =>
                                    {
                                        if (!baseResultDict.TryGetValue(group.Key, out var ap)) return null;

                                        var apClone = (APMActionPlan)ap.Clone();
                                        apClone.TaskList = group.Select(taskItem => (APMActionPlanTask)taskItem.Clone()).ToList();
                                        return apClone;
                                    })
                                    .Where(ap => ap != null)
                                    .ToList();
                            }
                            else
                            {
                                apsFromVisibleTasks = source.Select(ap => (APMActionPlan)ap.Clone()).ToList();
                            }

                            try { SetDeletionFlags(apsFromVisibleTasks); } catch { }

                            return new Tuple<List<APMActionPlan>, List<APMActionPlan>>(apsFromVisibleTasks, apsFromVisibleTasks);
                        }

                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("FastFilter exception: " + ex.Message, Category.Exception, Priority.Low);
                        return new Tuple<List<APMActionPlan>, List<APMActionPlan>>(new List<APMActionPlan>(), new List<APMActionPlan>());
                    }

                }, token).ContinueWith(t =>
                {
                    if (t.IsCanceled || token.IsCancellationRequested) return;
                    if (t.Exception != null)
                    {
                        GeosApplication.Instance.Logger.Log("FastFilter exception: " + t.Exception.GetBaseException().Message, Category.Exception, Priority.Low);
                        return;
                    }

                    var baseResult = t.Result.Item1;
                    var finalResult = t.Result.Item2;

                    if (!isTaskView)
                    {
                        _currentFilteredBase = baseResult.Select(ap => (APMActionPlan)ap.Clone()).ToList();
                        SetDeletionFlags(_currentFilteredBase);

                        try { FillStatus(); } catch { }

                        try
                        {
                            ClonedActionPlanList = _currentFilteredBase.Select(ap => (APMActionPlan)ap.Clone()).ToList();
                        }
                        catch { }

                        if ((ListOfPerson == null || ListOfPerson.Count == 0) && _responsibleCache != null)
                            ListOfPerson = new ObservableCollection<Responsible>(_responsibleCache);
                    }
                    else
                    {
                        var apsFromVisibleTasks = finalResult.Select(ap => (APMActionPlan)ap.Clone()).ToList();
                        _currentFilteredBase = apsFromVisibleTasks.Select(ap => (APMActionPlan)ap.Clone()).ToList();
                        try { SetDeletionFlags(_currentFilteredBase); } catch { }

                        try
                        {
                            var flatTasks = apsFromVisibleTasks
                                .Where(ap => ap.TaskList != null)
                                .SelectMany(ap => ap.TaskList.Select(tk =>
                                {
                                    var clone = (APMActionPlanTask)tk.Clone();
                                    if (string.IsNullOrWhiteSpace(clone.GroupName)) clone.GroupName = ap.GroupName;
                                    if (string.IsNullOrWhiteSpace(clone.Site)) clone.Site = ap.Site;
                                    if (clone.IdSite == 0) clone.IdSite = ap.IdSite;
                                    return clone;
                                })).ToList();

                            RebuildTaskStatusBucketsFromTasks(flatTasks);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Rebuild bucket error: " + ex.Message, Category.Exception, Priority.Low);
                        }

                        if ((ListOfPersonForTask == null || ListOfPersonForTask.Count == 0))
                        {
                            if (_taskResponsibleCache != null && _taskResponsibleCache.Count > 0)
                            {
                                ListOfPersonForTask = new ObservableCollection<Responsible>(_taskResponsibleCache);
                            }
                            else if (_responsibleCache != null)
                            {
                                var onlyTaskOwners = _responsibleCache.Where(x => x.IsTaskField).ToList();
                                ListOfPersonForTask = new ObservableCollection<Responsible>(onlyTaskOwners);
                            }

                            if (ListOfPersonForTask != null && ListOfPersonForTask.Count > 0)
                            {
                                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    _ = LoadResponsibleImagesBackgroundAsync(ListOfPersonForTask);
                                }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                            }
                        }

                        if (_taskCache != null && _taskCache.Count > 0 && (ActionPlanCodeList == null || ActionPlanCodeList.Count == 0))
                        {
                            ActionPlanCodeList = _taskCache
                                .Where(task => !string.IsNullOrWhiteSpace(task.Code))
                                .GroupBy(task => task.Code)
                                .Select(g => new APMActionPlan { Code = g.Key })
                                .OrderBy(ap => ap.Code)
                                .ToList();
                        }
                    }

                    RecalculateAllCounts();

                    if (IsAlertActive())
                    {
                        var cap = (_lastAppliedAlertCaption ?? string.Empty).Trim();

                        if (IsTaskGridVisibility)
                        {
                            try
                            {
                                var sel = AlertListOfFilterTile?
                                    .FirstOrDefault(x => string.Equals((x.Caption ?? string.Empty).Trim(), cap, StringComparison.OrdinalIgnoreCase));
                                if (sel != null && !ReferenceEquals(SelectedAlertTileBarItem, sel))
                                {
                                    SelectedAlertTileBarItem = sel;
                                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                var sel2 = AlertListOfFilterTileForGridView?
                                    .FirstOrDefault(x => string.Equals(x.Caption?.Trim(), cap, StringComparison.OrdinalIgnoreCase));
                                if (sel2 != null && !ReferenceEquals(SelectedActionPlansAlertTileBarItem, sel2))
                                {
                                    SelectedActionPlansAlertTileBarItem = sel2;
                                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));
                                }
                            }
                            catch { }
                        }
                    }

                    if (!isTaskView && IsExpand)
                    {
                        ExpandLastUpdatedActionPlan();
                    }

                    RewireAlertTileBarCommands();
                    RewireSideTileBarCommands();
                    RewireTopTileBarCommands();

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("ApplyInMemoryFiltersAsync setup error: " + ex.Message, Category.Exception, Priority.Low);
            }
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
                GeosApplication.Instance.Logger.Log("HasBlanksSelected error: " + ex.Message, Category.Exception, Priority.Low);
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
        private HashSet<string> ToResponsibleHashSet(List<object> list)
        {
            if (list == null || list.Count == 0) return null;

            var names = list
                .OfType<Responsible>()
                .SelectMany(r => new[] { r.FullName, r.ResponsibleDisplayName })
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim());

            return new HashSet<string>(names, StringComparer.OrdinalIgnoreCase);
        }
        private HashSet<string> ToCustomerHashSet(List<object> list)
        {
            if (list == null || list.Count == 0) return null;
            var names = list.OfType<APMCustomer>()
              .Select(c => c.GroupName)
              .Where(s => !string.IsNullOrWhiteSpace(s));
            return names.Any() ? new HashSet<string>(names, StringComparer.OrdinalIgnoreCase) : null;
        }
        private string SafePerson(APMActionPlan ap) =>
    ap.ActionPlanResponsibleDisplayName
    ?? ap.FullName
    ?? ap.Code;

        private void UpdateTaskGridFromFilteredList(
    List<APMActionPlanTask> source,
    HashSet<string> locTaskSet,
    HashSet<string> personTaskNameSet,
    HashSet<int> personTaskIdSet,
    HashSet<string> buTaskSet,
    HashSet<string> originTaskSet,
    HashSet<string> customerTaskSet,
    HashSet<int> customerTaskIdSet,
    HashSet<string> codeTaskSet,
    bool customerTaskIncludesBlanks)
        {
            try
            {
                IEnumerable<APMActionPlanTask> query = source ?? new List<APMActionPlanTask>();

                bool Matches(HashSet<string> set, string value) =>
                    set == null || set.Count == 0 || (!string.IsNullOrWhiteSpace(value) && set.Contains(value));

                if (codeTaskSet != null && codeTaskSet.Count > 0)
                    query = query.Where(t => Matches(codeTaskSet, t.Code));

                //[shweta.thube][GEOS2-10088][14.11.2025]
                if (locTaskSet != null && locTaskSet.Count > 0)
                {
                    query = query
                        .Where(t =>
                            locTaskSet.Contains(t.Location)
                            || (t.SubTaskList != null && t.SubTaskList.Any(st => locTaskSet.Contains(st.Location)))
                        )
                        .Select(t =>
                        {
                            var tClone = (APMActionPlanTask)t.Clone();

                            if (tClone.SubTaskList != null)
                            {
                                tClone.SubTaskList = tClone.SubTaskList
                                    .Where(st => locTaskSet.Contains(st.Location))
                                    .ToList();
                            }

                            return tClone;
                        });
                }


                bool hasId = personTaskIdSet != null && personTaskIdSet.Count > 0;
                bool hasName = personTaskNameSet != null && personTaskNameSet.Count > 0;

                IEnumerable<APMActionPlanTask> filtered = query;

                //[shweta.thube][GEOS2-10088][14.11.2025]
                if (hasId)
                {
                    var byId = filtered
                        .Where(t =>
                            personTaskIdSet.Contains(t.IdEmployee) ||

                            (t.SubTaskList != null && t.SubTaskList.Any(st =>
                                personTaskIdSet.Contains(st.IdEmployee)
                            ))
                        )
                        .Select(t =>
                        {
                            var clone = (APMActionPlanTask)t.Clone();

                            if (clone.SubTaskList != null)
                            {
                                clone.SubTaskList = clone.SubTaskList
                                    .Where(st =>
                                        personTaskIdSet.Contains(st.IdEmployee)
                                    )
                                    .ToList();
                            }

                            return clone;
                        })
                        .ToList();

                    if (byId.Count > 0)
                    {
                        filtered = byId;
                    }
                    else if (hasName)
                    {
                        filtered = filtered
                            .Where(t =>
                                personTaskNameSet.Contains(t.TaskResponsibleDisplayName ?? t.Responsible) ||

                                (t.SubTaskList != null && t.SubTaskList.Any(st =>
                                    personTaskNameSet.Contains(st.TaskResponsibleDisplayName ?? st.Responsible)
                                ))
                            )
                            .Select(t =>
                            {
                                var clone = (APMActionPlanTask)t.Clone();

                                if (clone.SubTaskList != null)
                                {
                                    clone.SubTaskList = clone.SubTaskList
                                        .Where(st =>
                                            personTaskNameSet.Contains(st.TaskResponsibleDisplayName ?? st.Responsible)
                                        )
                                        .ToList();
                                }

                                return clone;
                            })
                            .ToList();
                    }
                }
                else if (hasName)
                {
                    filtered = filtered
                        .Where(t =>
                            personTaskNameSet.Contains(t.TaskResponsibleDisplayName ?? t.Responsible) ||

                            (t.SubTaskList != null && t.SubTaskList.Any(st =>
                                personTaskNameSet.Contains(st.TaskResponsibleDisplayName ?? st.Responsible)
                            ))
                        )
                        .Select(t =>
                        {
                            var clone = (APMActionPlanTask)t.Clone();

                            if (clone.SubTaskList != null)
                            {
                                clone.SubTaskList = clone.SubTaskList
                                    .Where(st =>
                                        personTaskNameSet.Contains(st.TaskResponsibleDisplayName ?? st.Responsible)
                                    )
                                    .ToList();
                            }

                            return clone;
                        })
                        .ToList();
                }
                if (buTaskSet != null && buTaskSet.Count > 0)
                    filtered = filtered.Where(t => Matches(buTaskSet, t.BusinessUnit));

                if (originTaskSet != null && originTaskSet.Count > 0)
                    filtered = filtered.Where(t =>
                        Matches(originTaskSet, t.TaskOrigin) ||
                        Matches(originTaskSet, t.Origin));

                bool hasCustomerNames = customerTaskSet != null && customerTaskSet.Count > 0;
                bool hasCustomerIds = customerTaskIdSet != null && customerTaskIdSet.Count > 0;

                if (hasCustomerNames || hasCustomerIds || customerTaskIncludesBlanks)
                {
                    filtered = filtered.Where(t =>
                        (hasCustomerIds && customerTaskIdSet.Contains(t.IdSite)) ||
                        (hasCustomerNames && !string.IsNullOrWhiteSpace(t.GroupName) && customerTaskSet.Contains(t.GroupName)) ||
                        (customerTaskIncludesBlanks &&
                            string.IsNullOrWhiteSpace(t.GroupName) &&
                            t.IdSite == 0)
                    );
                }

                var finalTasks = filtered
                    .OrderBy(t => t.Code)
                    .ThenBy(t => t.NumItem)
                    .Select(t => (APMActionPlanTask)t.Clone())
                    .ToList();

                TaskGridList = new ObservableCollection<APMActionPlanTask>(finalTasks);
                TempTaskGridList = finalTasks.Select(t => (APMActionPlanTask)t.Clone()).ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("UpdateTaskGridFromFilteredList error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }
        public void ResetFilterState()
        {
            _currentFilteredBase = null;
            _alertFilteredBase = null;
            _baseForSideTileCounts = null;
            _dataBeforeAlertFilter = null;
            _dataBeforeSideTileFilter = null;

            _lastAppliedAlertCaption = null;
            _lastAppliedSideCaption = null;
            _lastAppliedSideTileFilter = null;

            _sideTileCountsSnapshot = null;
            _pendingAllWhileAlert = false;
        }

        private void RefreshTaskGridFromCurrentPlans()
        {
            if (!IsTaskGridVisibility) return;
            try
            {
                var locTaskSet = ToStringHashSet(SelectedLocationForTask);
                var personTaskNameSet = ToResponsibleHashSet(SelectedPersonForTask);
                var personTaskIdSet = ToResponsibleIdSet(SelectedPersonForTask);
                var buTaskSet = ToStringHashSet(SelectedBussinessUnitForTask);
                var originTaskSet = ToStringHashSet(SelectedOriginForTask);
                var customerTaskSet = ToCustomerHashSet(SelectedCustomerForTask);
                var customerTaskIdSet = SelectedCustomerForTask?.OfType<APMCustomer>().Select(c => c.IdSite).Where(id => id > 0).ToHashSet();
                var codeTaskSet = ToStringHashSet(SelectedActionPlansCodeForTask);
                bool customerTaskIncludesBlanks = HasBlanksSelected(SelectedCustomerForTask);

                var baseTasks = new List<APMActionPlanTask>();
                foreach (var plan in ActionPlanList ?? new ObservableCollection<APMActionPlan>())
                {
                    if (plan?.TaskList == null || plan.TaskList.Count == 0) continue;
                    foreach (var t in plan.TaskList)
                    {
                        var ct = (APMActionPlanTask)t.Clone();
                        if (string.IsNullOrWhiteSpace(ct.GroupName)) ct.GroupName = plan.GroupName;
                        if (string.IsNullOrWhiteSpace(ct.Site)) ct.Site = plan.Site;
                        if (ct.IdSite == 0) ct.IdSite = plan.IdSite;
                        baseTasks.Add(ct);
                    }
                }

                UpdateTaskGridFromFilteredList(
                    baseTasks,
                    locTaskSet,
                    personTaskNameSet,
                    personTaskIdSet,
                    buTaskSet,
                    originTaskSet,
                    customerTaskSet,
                    customerTaskIdSet,
                    codeTaskSet,
                    customerTaskIncludesBlanks
                );
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("RefreshTaskGridFromCurrentPlans error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private bool _sideTileCommandsRewired;
        private TileBarFilters _lastAppliedSideTileFilter;
        private static readonly Regex _andSplit = new Regex("\\bAND\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex _inExpr = new Regex("\\[(?<f>[^\\]]+)\\]\\s+In\\s*\\((?<v>[^\\)]+)\\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex _eqExpr = new Regex("\\[(?<f>[^\\]]+)\\]\\s*=\\s*'(?<v>[^']+)'", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private string _lastAppliedSideCaption;
        private List<APMActionPlan> _dataBeforeSideTileFilter;
        private bool _suppressSideTileCountUpdate;
        private bool _pendingAllWhileAlert;
        private bool _lastClickedWasAll;
        private Dictionary<string, int> _sideTileCountsSnapshot;

        private void RewireSideTileBarCommands()
        {
            if (_sideTileCommandsRewired) return;
            try
            {
                var clickCmd = new DevExpress.Mvvm.DelegateCommand<object>(OnSideTileClicked);
                CommandFilterTileClick = clickCmd;
                LeftCommandFilterTileClick = clickCmd;
                LeftCommandFilterTileClickForTask = clickCmd;

                var dblCmd = new DevExpress.Mvvm.DelegateCommand<object>(OnSideTileEditRequested);
                CommandTileBarDoubleClick = dblCmd;
                CommandTaskTileBarDoubleClick = dblCmd;

                _sideTileCommandsRewired = true;
                GeosApplication.Instance.Logger.Log(
                    "Side tile bar commands rewired (grid + task)",
                    Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("RewireSideTileBarCommands error: " + ex.Message,
                    Category.Exception, Priority.Low);
            }
        }

        private void OnSideTileClicked(object arg)
        {
            try
            {
                var filter = ExtractFilterFromEvent(arg) ?? arg as TileBarFilters ?? SelectedTileBarItem as TileBarFilters;
                if (filter == null) return;

                var caption = filter.Caption ?? string.Empty;
                bool isClickingAll = IsAllCaption(caption);

                bool sideWasAlreadyAll = IsAllCaption(_lastAppliedSideCaption) || _lastAppliedSideTileFilter == null;

                if (!isClickingAll)
                {
                    _lastClickedWasAll = false;
                    _pendingAllWhileAlert = false;
                }

                if (isClickingAll)
                {
                    if (IsAlertActive())
                    {
                        if (!_pendingAllWhileAlert)
                        {

                            _pendingAllWhileAlert = true;
                            _lastClickedWasAll = true;

                            if (!ReferenceEquals(filter, SelectedTileBarItem))
                                SelectedTileBarItem = filter;

                            _lastAppliedSideTileFilter = null;
                            _lastAppliedSideCaption = "All";

                            RecalculateAllCounts();
                            return;
                        }
                        else
                        {
                            _pendingAllWhileAlert = false;
                            _lastClickedWasAll = false;
                            _sideTileCountsSnapshot = null;

                            if (!ReferenceEquals(filter, SelectedTileBarItem))
                                SelectedTileBarItem = filter;

                            _lastAppliedSideTileFilter = null;
                            _lastAppliedSideCaption = "All";
                            _lastAppliedAlertCaption = null;


                            SelectedAlertTileBarItem = null;
                            SelectedActionPlansAlertTileBarItem = null;
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));

                            ResetCustomFiltersState();

                            RecalculateAllCounts();
                            return;
                        }
                    }
                    else
                    {

                        if (sideWasAlreadyAll)
                        {
                            ResetCustomFiltersState();
                        }

                        if (!ReferenceEquals(filter, SelectedTileBarItem))
                            SelectedTileBarItem = filter;

                        _lastAppliedSideTileFilter = null;
                        _lastAppliedSideCaption = "All";

                        RecalculateAllCounts();
                        return;
                    }
                }

                if (!ReferenceEquals(filter, SelectedTileBarItem))
                    SelectedTileBarItem = filter;

                _lastAppliedSideTileFilter = filter;
                _lastAppliedSideCaption = caption;

                RecalculateAllCounts();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("OnSideTileClicked error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }


        private TileBarFilters ExtractFilterFromEvent(object arg)
        {
            var mbe = arg as MouseButtonEventArgs;
            if (mbe == null) return null;
            DependencyObject current = mbe.OriginalSource as DependencyObject;
            while (current != null && !(current is TileBarItem)) current = VisualTreeHelper.GetParent(current);
            return (current as TileBarItem)?.DataContext as TileBarFilters;
        }
        private void OnSideTileEditRequested(object obj)
        {
            try
            {
                OnSideTileClicked(obj);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(
                    "OnSideTileEditRequested error: " + ex.Message,
                    Category.Exception, Priority.Low);
            }
        }

        private void ApplySideTileFilter(string criteria, string caption, List<APMActionPlan> effectiveBase)
        {
            try
            {
                var baseSource = (effectiveBase ?? new List<APMActionPlan>());
                if (baseSource.Count == 0)
                {
                    SetActionPlanList(new List<APMActionPlan>());
                    return;
                }

                if (string.IsNullOrWhiteSpace(criteria) || IsAllCaption(caption))
                {
                    SetActionPlanList(baseSource.Select(ClonePlan).ToList());
                    return;
                }
                var expressions = ParseCriteria(criteria);
                if (expressions.Count == 0)
                {
                    SetActionPlanList(baseSource.Select(ClonePlan).ToList());
                    return;
                }
                var result = new List<APMActionPlan>();
                foreach (var ap in baseSource)
                {
                    var tasks = ap.TaskList;
                    if (tasks == null || tasks.Count == 0) continue;
                    var matched = tasks.Where(t => expressions.All(expr => Matches(ap, t, expr))).ToList();
                    if (matched.Count == 0) continue;
                    var clone = ClonePlan(ap);
                    clone.TaskList = matched;
                    result.Add(clone);
                }
                SetActionPlanList(result);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("ApplySideTileFilter (effective base) error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private void SetActionPlanList(List<APMActionPlan> plans)
        {
            ActionPlanList = new ObservableCollection<APMActionPlan>(plans);
            SetDeletionFlags(ActionPlanList);
            try { ClonedActionPlanList = plans.Select(ClonePlan).ToList(); } catch { }
            if (IsExpand)
            {
                ExpandLastUpdatedActionPlan();
            }
        }
        private APMActionPlan ClonePlan(APMActionPlan ap) => (APMActionPlan)ap.Clone();
        private bool Matches(APMActionPlan ap, APMActionPlanTask t, InExpression expr) => PropertyMatches(t, expr.Field, expr.Values) || PropertyMatches(ap, expr.Field, expr.Values);
        private bool IsAllCaption(string caption)
        {
            if (string.IsNullOrWhiteSpace(caption)) return false;
            caption = caption.Trim().ToLowerInvariant();
            return caption == "all" || caption.Contains("todos") || caption.Contains("todas") || caption.Contains("all action plans");
        }
        private class InExpression
        {
            public string Field { get; set; }
            public HashSet<string> Values { get; set; }
        }
        private List<InExpression> ParseCriteria(string criteria)
        {
            var list = new List<InExpression>();
            try
            {
                foreach (var part in _andSplit.Split(criteria).Select(p => p.Trim()).Where(p => p.Length > 0))
                {
                    var mIn = _inExpr.Match(part);
                    if (mIn.Success)
                    {
                        var field = mIn.Groups["f"].Value.Trim();
                        var raw = mIn.Groups["v"].Value;
                        var vals = raw.Split(',').Select(v => v.Trim().Trim('\'', '"')).Where(v => v.Length > 0).ToHashSet(StringComparer.OrdinalIgnoreCase);
                        if (vals.Count > 0) list.Add(new InExpression { Field = field, Values = vals });
                        continue;
                    }
                    var mEq = _eqExpr.Match(part);
                    if (mEq.Success)
                    {
                        var field = mEq.Groups["f"].Value.Trim();
                        var val = mEq.Groups["v"].Value.Trim();
                        list.Add(new InExpression { Field = field, Values = new HashSet<string>(new[] { val }, StringComparer.OrdinalIgnoreCase) });
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("ParseCriteria error: " + ex.Message, Category.Exception, Priority.Low);
            }
            return list;
        }
        private bool PropertyMatches(object obj, string fieldName, HashSet<string> values)
        {
            if (obj == null) return false;
            var last = fieldName.Split('.').Last();
            if (last.Equals("Region", StringComparison.OrdinalIgnoreCase)) last = "Location";
            if (last.Equals("InUseYesOrNo", StringComparison.OrdinalIgnoreCase)) last = "InUse";
            var prop = obj.GetType().GetProperty(last, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
            if (prop == null) return false;
            var valObj = prop.GetValue(obj, null);
            if (valObj == null) return false;
            var valStr = valObj is bool b ? (b ? "Yes" : "No") : valObj.ToString();
            return !string.IsNullOrEmpty(valStr) && values.Contains(valStr);
        }




        private bool _alertCommandsRewired;
        private string _lastAppliedAlertCaption;

        private List<APMActionPlan> _dataBeforeAlertFilter;

        private bool _suppressAlertTileRowUpdate;

        private void RewireAlertTileBarCommands()
        {
            if (_alertCommandsRewired) return;
            try
            {
                CommandAlertFilterTileClick = new DevExpress.Mvvm.DelegateCommand<object>(OnAlertTileClicked);
                CommandActionPlansAlertFilterTileClick = new DevExpress.Mvvm.DelegateCommand<object>(OnActionPlansAlertTileClicked);
                _alertCommandsRewired = true;
                GeosApplication.Instance.Logger.Log("Alert tile bar commands rewired for in-memory filtering", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("RewireAlertTileBarCommands error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private APMAlertTileBarFilters ExtractTile(object obj, bool isActionPlanLevel)
        {
            var tile = obj as APMAlertTileBarFilters;
            if (tile != null) return tile;
            var selArgs = obj as System.Windows.Controls.SelectionChangedEventArgs;
            if (selArgs != null && selArgs.AddedItems != null && selArgs.AddedItems.Count > 0)
            {
                tile = selArgs.AddedItems[0] as APMAlertTileBarFilters;
                if (tile != null) return tile;
            }
            var routed = obj as RoutedEventArgs;
            if (routed != null)
            {
                var fe = routed.OriginalSource as FrameworkElement;
                if (fe?.DataContext is APMAlertTileBarFilters dcTile)
                    return dcTile;
            }
            return isActionPlanLevel ? SelectedActionPlansAlertTileBarItem : SelectedAlertTileBarItem;
        }

        private void OnAlertTileClicked(object obj)
        {
            try
            {
                var tile = ExtractTile(obj, false);
                if (tile == null) return;
                var caption = tile.Caption;
                if (string.IsNullOrWhiteSpace(caption)) return;

                var togglingOff = !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption) &&
                                  string.Equals(_lastAppliedAlertCaption, caption, StringComparison.OrdinalIgnoreCase);

                _dataBeforeAlertFilter = null;
                _alertFilteredBase = null;

                if (togglingOff)
                {
                    _lastAppliedAlertCaption = null;

                    SelectedAlertTileBarItem = null;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                }
                else
                {
                    _lastAppliedAlertCaption = caption;

                    try
                    {
                        var immediateSel = AlertListOfFilterTile?
                            .FirstOrDefault(x => string.Equals((x.Caption ?? string.Empty).Trim(), caption.Trim(), StringComparison.OrdinalIgnoreCase))
                                ?? (tile.Id > 0 ? AlertListOfFilterTile?.FirstOrDefault(x => x.Id == tile.Id) : null);

                        if (immediateSel != null)
                        {
                            SelectedAlertTileBarItem = immediateSel;
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                        }
                    }
                    catch { }
                }

                RecalculateAllCounts();

                System.Windows.Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        if (togglingOff)
                        {
                            SelectedAlertTileBarItem = null;
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                        }
                        else
                        {
                            var sel = AlertListOfFilterTile?
                                .FirstOrDefault(x => string.Equals((x.Caption ?? string.Empty).Trim(), caption.Trim(), StringComparison.OrdinalIgnoreCase))
                                     ?? (tile.Id > 0 ? AlertListOfFilterTile?.FirstOrDefault(x => x.Id == tile.Id) : null);

                            if (sel != null)
                            {
                                SelectedAlertTileBarItem = sel;
                                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                            }
                        }
                    }
                    catch { }
                }), System.Windows.Threading.DispatcherPriority.ContextIdle);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("OnAlertTileClicked error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }


        private void OnActionPlansAlertTileClicked(object obj)
        {
            try
            {
                var tile = ExtractTile(obj, true);
                if (tile == null) return;
                var caption = tile.Caption;
                if (string.IsNullOrWhiteSpace(caption)) return;

                var selectedCaption = caption.Trim();

                bool togglingOff = !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption)
                                   && string.Equals(_lastAppliedAlertCaption, caption, StringComparison.OrdinalIgnoreCase);

                _dataBeforeAlertFilter = null;
                _alertFilteredBase = null;

                if (togglingOff)
                {
                    _lastAppliedAlertCaption = null;

                    SelectedActionPlansAlertTileBarItem = null;
                    SelectedAlertTileBarItem = null;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                }
                else
                {
                    _lastAppliedAlertCaption = caption;

                    SelectedActionPlansAlertTileBarItem = null;
                    SelectedAlertTileBarItem = null;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));

                    try
                    {
                        var immediateSel = AlertListOfFilterTileForGridView?
                            .FirstOrDefault(x => string.Equals(x.Caption?.Trim(), selectedCaption, StringComparison.OrdinalIgnoreCase));
                        if (immediateSel != null)
                        {
                            SelectedActionPlansAlertTileBarItem = immediateSel;
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));
                        }
                    }
                    catch { }
                }

                RecalculateAllCounts();

                System.Windows.Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        if (togglingOff)
                        {
                            SelectedActionPlansAlertTileBarItem = null;
                            SelectedAlertTileBarItem = null;
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                        }
                        else
                        {
                            var again = AlertListOfFilterTileForGridView?
                                .FirstOrDefault(x => string.Equals(x.Caption?.Trim(), selectedCaption, StringComparison.OrdinalIgnoreCase));

                            if (again != null)
                            {
                                SelectedActionPlansAlertTileBarItem = again;
                                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));
                            }
                        }
                    }
                    catch { }
                }), System.Windows.Threading.DispatcherPriority.ContextIdle);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("OnActionPlansAlertTileClicked error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private void UpdateSideTileCounts(List<APMActionPlan> source)
        {
            try
            {
                if (ListOfFilterTile == null || ListOfFilterTile.Count == 0) return;

                var baseForCounts = source ?? new List<APMActionPlan>();

                foreach (var tile in ListOfFilterTile)
                {
                    if (tile == null) continue;
                    int count = 0;

                    if (IsAllCaption(tile.Caption))
                    {
                        count = baseForCounts.Sum(ap => ap.TaskList?.Sum(t => 1 + (t.SubTaskList?.Count ?? 0)) ?? 0);
                    }
                    else
                    {
                        var exprs = ParseCriteria(tile.FilterCriteria);
                        if (exprs != null && exprs.Count > 0)
                        {
                            bool MatchesCriteria(object obj, List<InExpression> expressions)
                            {
                                return expressions.All(e => PropertyMatches(obj, e.Field, e.Values));
                            }

                            foreach (var ap in baseForCounts)
                            {
                                if (ap.TaskList == null) continue;

                                foreach (var t in ap.TaskList)
                                {

                                    bool taskMatches = exprs.All(e => PropertyMatches(t, e.Field, e.Values) || PropertyMatches(ap, e.Field, e.Values));

                                    if (taskMatches)
                                    {
                                        count++;
                                        count += (t.SubTaskList?.Count ?? 0);
                                    }
                                }
                            }
                        }
                    }

                    tile.EntitiesCount = count;
                    tile.EntitiesCountVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("UpdateSideTileCounts error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }
        private void UpdateAlertFiltersBasedOnFilteredData(List<APMActionPlan> source)
        {
            try
            {
                if (_suppressAlertTileRowUpdate) return;

                var basePlans = source ?? new List<APMActionPlan>();

                var allItems = new List<APMActionPlanTask>();

                foreach (var ap in basePlans)
                {
                    if (ap.TaskList == null) continue;

                    foreach (var t in ap.TaskList)
                    {

                        allItems.Add(t);

                        if (t.SubTaskList != null && t.SubTaskList.Count > 0)
                        {
                            foreach (var st in t.SubTaskList)
                            {

                                var subAsTask = (APMActionPlanTask)t.Clone();

                                subAsTask.DueDays = st.DueDays;
                                subAsTask.DueDate = st.DueDate;
                                subAsTask.Status = st.Status;
                                subAsTask.IdLookupStatus = st.IdLookupStatus;
                                subAsTask.Priority = st.Priority;
                                subAsTask.Theme = st.Theme;
                                subAsTask.Responsible = st.Responsible;

                                allItems.Add(subAsTask);
                            }
                        }
                    }
                }

                bool IsOverdue(APMActionPlanTask t) => t.DueDays > 0 && !IsClosed(t);

                int longestOverdueDays = allItems.Where(IsOverdue).Select(t => t.DueDays).DefaultIfEmpty(0).Max();

                int overdue15Count = allItems.Count(t => t.DueDays >= 15 && !IsClosed(t));

                int highPriorityCount = allItems.Count(t => t.DueDays >= 5 && !IsClosed(t) && string.Equals(t.Priority ?? "", "High", StringComparison.OrdinalIgnoreCase));

                string ColorByDays(int d) => d >= 5 ? "Red" : (d > 0 ? "Orange" : "Green");
                string ColorByCount(int c) => c == 0 ? "Green" : (c < 5 ? "Orange" : "Red");

                string mostTheme = GetMostOverdueThemeName(allItems);
                string mostResp = GetMostOverdueResponsibleName(allItems);

                int maxDaysTheme = !string.IsNullOrWhiteSpace(mostTheme)
                    ? allItems.Where(t => IsOverdue(t) && string.Equals((t.Theme ?? "").Trim(), mostTheme.Trim(), StringComparison.OrdinalIgnoreCase))
                              .Select(t => t.DueDays).DefaultIfEmpty(0).Max()
                    : 0;

                int maxDaysResp = !string.IsNullOrWhiteSpace(mostResp)
                    ? allItems.Where(t => IsOverdue(t) && string.Equals((t.TaskResponsibleDisplayName ?? t.Responsible ?? "").Trim(),
                                                                         mostResp.Trim(), StringComparison.OrdinalIgnoreCase))
                              .Select(t => t.DueDays).DefaultIfEmpty(0).Max()
                    : 0;

                void Paint(ObservableCollection<APMAlertTileBarFilters> list)
                {
                    if (list == null || list.Count == 0) return;

                    foreach (var tile in list)
                    {
                        if (tile == null) continue;
                        var norm = System.Text.RegularExpressions.Regex
                                   .Replace((tile.Caption ?? "").Trim().ToLowerInvariant(), @"\s+", " ");

                        if (norm.Contains("longest overdue"))
                        { tile.EntitiesCount = longestOverdueDays.ToString(); tile.BackColor = ColorByDays(longestOverdueDays); tile.EntitiesCountVisibility = Visibility.Visible; continue; }

                        if (norm.Contains("overdue >= 15"))
                        { tile.EntitiesCount = overdue15Count.ToString(); tile.BackColor = ColorByCount(overdue15Count); tile.EntitiesCountVisibility = Visibility.Visible; continue; }

                        if (norm.Contains("high priority overdue"))
                        { tile.EntitiesCount = highPriorityCount.ToString(); tile.BackColor = ColorByCount(highPriorityCount); tile.EntitiesCountVisibility = Visibility.Visible; continue; }

                        if (norm.Contains("most overdue theme"))
                        { tile.EntitiesCount = string.IsNullOrWhiteSpace(mostTheme) ? "---" : mostTheme; tile.BackColor = ColorByDays(maxDaysTheme); tile.EntitiesCountVisibility = Visibility.Visible; continue; }

                        if (norm.Contains("most overdue responsible"))
                        { tile.EntitiesCount = string.IsNullOrWhiteSpace(mostResp) ? "---" : mostResp; tile.BackColor = ColorByDays(maxDaysResp); tile.EntitiesCountVisibility = Visibility.Visible; continue; }

                        if (tile.Id > 0)
                        {
                            var cnt = allItems.Count(t => t.IdLookupStatus == tile.Id);
                            tile.EntitiesCount = cnt.ToString();
                            tile.EntitiesCountVisibility = Visibility.Visible;
                            continue;
                        }

                        if (norm == "to do" || norm == "in progress" || norm == "blocked" || norm == "done")
                        {
                            var wanted = (tile.Caption ?? "").Trim();
                            int cnt = allItems.Count(t => !string.IsNullOrWhiteSpace(t.Status) &&
                                                          string.Equals(t.Status.Trim(), wanted, StringComparison.OrdinalIgnoreCase));
                            tile.EntitiesCount = cnt.ToString();
                            tile.EntitiesCountVisibility = Visibility.Visible;
                            continue;
                        }
                    }
                }

                Paint(AlertListOfFilterTile);
                Paint(AlertListOfFilterTileForGridView);

                var tmp1 = AlertListOfFilterTile;
                AlertListOfFilterTile = null;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AlertListOfFilterTile)));
                AlertListOfFilterTile = tmp1;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AlertListOfFilterTile)));

                var tmp2 = AlertListOfFilterTileForGridView;
                AlertListOfFilterTileForGridView = null;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AlertListOfFilterTileForGridView)));
                AlertListOfFilterTileForGridView = tmp2;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AlertListOfFilterTileForGridView)));

                string Key(string c)
                {
                    if (string.IsNullOrWhiteSpace(c)) return "";
                    var s = c.ToLowerInvariant();
                    if (s.Contains("to do") || s.Contains("to-do")) return "todo";
                    if (s.Contains("in progress") || s.Contains("in-progress")) return "inprogress";
                    if (s.Contains("blocked")) return "blocked";
                    if (s.Contains("done") || s.Contains("completed")) return "done";
                    if (s.Contains("longest") && s.Contains("over") && s.Contains("due")) return "longest";
                    if (s.Contains("overdue >= 15")) return "overdue15";
                    if (s.Contains("high priority overdue")) return "hiprio";
                    if (s.Contains("most overdue theme")) return "mosttheme";
                    if (s.Contains("most overdue responsible")) return "mostresp";
                    return s;
                }

                var snap = _lastAppliedAlertCaption;
                if (!string.IsNullOrWhiteSpace(snap))
                {
                    Func<string, string> Norm = s => (s ?? string.Empty).Trim().ToUpperInvariant();
                    var k = Norm(snap);

                    var list = IsTaskGridVisibility ? AlertListOfFilterTile : AlertListOfFilterTileForGridView;
                    var sel = list?.FirstOrDefault(x => Norm(x.Caption) == k);

                    if (IsTaskGridVisibility)
                    {
                        SelectedAlertTileBarItem = sel;
                        OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                    }
                    else
                    {
                        SelectedActionPlansAlertTileBarItem = sel;
                        OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));
                    }

                    System.Windows.Application.Current?.Dispatcher?.BeginInvoke(new Action(() =>
                    {
                        var list2 = IsTaskGridVisibility ? AlertListOfFilterTile : AlertListOfFilterTileForGridView;
                        var again = list2?.FirstOrDefault(x => Norm(x.Caption) == k);

                        if (IsTaskGridVisibility)
                        {
                            SelectedAlertTileBarItem = again;
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                        }
                        else
                        {
                            SelectedActionPlansAlertTileBarItem = again;
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));
                        }
                    }), System.Windows.Threading.DispatcherPriority.Render);
                }
                else
                {
                    SelectedAlertTileBarItem = null;
                    SelectedActionPlansAlertTileBarItem = null;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedAlertTileBarItem)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedActionPlansAlertTileBarItem)));
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("UpdateAlertFiltersBasedOnFilteredData error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private bool IsClosed(APMActionPlanTask t)
        {
            if (t == null) return false;
            var status = t.Status ?? string.Empty;
            return status.IndexOf("done", StringComparison.OrdinalIgnoreCase) >= 0 ||
       status.IndexOf("closed", StringComparison.OrdinalIgnoreCase) >= 0 ||
         status.IndexOf("completed", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private string GetMostOverdueThemeName(List<APMActionPlanTask> tasks)
        {
            try
            {
                var overdueTasks = tasks.Where(t => t.DueDays > 0 && !IsClosed(t) && !string.IsNullOrWhiteSpace(t.Theme)).ToList();
                if (overdueTasks.Count == 0) return null;


                return overdueTasks
                    .GroupBy(t => t.Theme.Trim())
                    .OrderByDescending(g => g.Count())
                    .ThenBy(g => g.Min(t => t.ActionPlanCode ?? string.Empty))
                    .FirstOrDefault()
                    ?.Key;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("GetMostOverdueThemeName error: " + ex.Message, Category.Exception, Priority.Low);
                return null;
            }
        }

        private string GetMostOverdueResponsibleName(List<APMActionPlanTask> tasks)
        {
            try
            {
                var overdueTasks = tasks.Where(t => t.DueDays > 0 && !IsClosed(t)).ToList();
                if (overdueTasks.Count == 0) return null;
                string GetResp(APMActionPlanTask x) => (x.TaskResponsibleDisplayName ?? x.Responsible ?? string.Empty).Trim();


                return overdueTasks.Where(t => !string.IsNullOrWhiteSpace(GetResp(t)))
                    .GroupBy(t => GetResp(t))
                    .OrderByDescending(g => g.Count())
                    .ThenBy(g => g.Min(t => t.ActionPlanCode ?? string.Empty))
                    .FirstOrDefault()
                    ?.Key;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("GetMostOverdueResponsibleName error: " + ex.Message, Category.Exception, Priority.Low);
                return null;
            }
        }

        private Func<APMActionPlanTask, bool> BuildMostOverdueThemePredicateFromCurrentData(List<APMActionPlan> currentData)
        {
            try
            {
                var tasks = currentData.Where(ap => ap.TaskList != null)
                    .SelectMany(ap => ap.TaskList)
                .Where(t => t.DueDays > 0 && !IsClosed(t) && !string.IsNullOrWhiteSpace(t.Theme))
                    .ToList();

                if (tasks.Count == 0) return t => false;
                var mostTheme = tasks.GroupBy(t => t.Theme.Trim()).OrderByDescending(g => g.Count()).First().Key;
                return t => t.DueDays > 0 && !IsClosed(t) && string.Equals(t.Theme?.Trim(), mostTheme, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("BuildMostOverdueThemePredicateFromCurrentData error: " + ex.Message, Category.Exception, Priority.Low);
                return t => false;
            }
        }

        private Func<APMActionPlanTask, bool> BuildMostOverdueResponsiblePredicateFromCurrentData(List<APMActionPlan> currentData)
        {
            try
            {
                var tasks = currentData.Where(ap => ap.TaskList != null)
               .SelectMany(ap => ap.TaskList)
                .Where(t => t.DueDays > 0 && !IsClosed(t))
        .ToList();

                if (tasks.Count == 0) return t => false;
                string GetResp(APMActionPlanTask x) => (x.TaskResponsibleDisplayName ?? x.Responsible ?? string.Empty).Trim();
                var mostResp = tasks.Where(t => !string.IsNullOrWhiteSpace(GetResp(t))).GroupBy(t => GetResp(t)).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key;

                if (string.IsNullOrWhiteSpace(mostResp)) return t => false;
                return t => t.DueDays > 0 && !IsClosed(t) && string.Equals(GetResp(t), mostResp, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("BuildMostOverdueResponsiblePredicateFromCurrentData error: " + ex.Message, Category.Exception, Priority.Low);
                return t => false;
            }
        }

        private Func<APMActionPlanTask, bool> GetTaskPredicate(string caption)
        {
            if (string.IsNullOrWhiteSpace(caption)) return null;
            caption = caption.ToLowerInvariant();
            var normalized = new string(caption.Where(c => !char.IsWhiteSpace(c)).ToArray());

            bool IsStatusLike(APMActionPlanTask t, string keyword)
            {
                var status = t.Status ?? string.Empty;
                return status.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            bool IsHighPriority(APMActionPlanTask t)
            {
                var pr = t.Priority ?? string.Empty;
                return pr.Equals("High", StringComparison.OrdinalIgnoreCase) || pr.Equals("Critical", StringComparison.OrdinalIgnoreCase);
            }

            if (caption.Contains("to do")) return t => !IsClosed(t) && ((IsStatusLike(t, "to") && IsStatusLike(t, "do")) || IsStatusLike(t, "open"));
            if (caption.Contains("in progress")) return t => !IsClosed(t) && (IsStatusLike(t, "progress") || IsStatusLike(t, "working"));
            if (caption.Contains("blocked")) return t => !IsClosed(t) && (IsStatusLike(t, "blocked") || IsStatusLike(t, "hold"));
            if (caption.Contains("done")) return t => IsClosed(t);
            if (caption.Contains("longest") && caption.Contains("over") && caption.Contains("due")) return t => t.DueDays > 0 && !IsClosed(t);
            if (normalized.Contains("overdue>=15")) return t => t.DueDays >= 15 && !IsClosed(t);
            if (caption.Contains("high priority overdue")) return t => t.DueDays > 0 && IsHighPriority(t) && !IsClosed(t);

            return null;
        }

        private List<APMActionPlan> BuildBaselineForAlertTiles()
        {
            var baseSource = !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption) && _dataBeforeAlertFilter != null
                ? _dataBeforeAlertFilter
                : (_currentFilteredBase ?? ActionPlanList?.Select(ap => (APMActionPlan)ap.Clone()).ToList() ?? new List<APMActionPlan>());

            bool isTaskView = IsTaskGridVisibility;
            var locSet = isTaskView ? ToStringHashSet(SelectedLocationForTask) : ToStringHashSet(SelectedLocation);
            var personNameSet = isTaskView ? ToResponsibleHashSet(SelectedPersonForTask) : ToResponsibleHashSet(SelectedPerson);
            var personIdSet = isTaskView ? ToResponsibleIdSet(SelectedPersonForTask) : ToResponsibleIdSet(SelectedPerson);
            var buSet = isTaskView ? ToStringHashSet(SelectedBussinessUnitForTask) : ToStringHashSet(SelectedBussinessUnit);
            var originSet = isTaskView ? ToStringHashSet(SelectedOriginForTask) : ToStringHashSet(SelectedOrigin);
            var customerSet = isTaskView ? ToCustomerHashSet(SelectedCustomerForTask) : ToCustomerHashSet(SelectedCustomer);
            var customerIdSet = isTaskView
                ? SelectedCustomerForTask?.OfType<APMCustomer>().Select(c => c.IdSite).Where(id => id > 0).ToHashSet()
                : SelectedCustomer?.OfType<APMCustomer>().Select(c => c.IdSite).Where(id => id > 0).ToHashSet();
            var codeSet = isTaskView ? ToStringHashSet(SelectedActionPlansCodeForTask) : ToStringHashSet(SelectedActionPlansCode);

            var baseList = new List<APMActionPlan>();
            foreach (var ap in baseSource)
            {
                if (ap == null) continue;
                bool apMatches = true;
                if (locSet != null && (string.IsNullOrWhiteSpace(ap.Location) || !locSet.Contains(ap.Location))) apMatches = false;


                if (personNameSet != null || personIdSet != null)
                {
                    bool hasName = personNameSet != null && personNameSet.Count > 0;
                    bool hasId = personIdSet != null && personIdSet.Count > 0;
                    bool isMatch = false;


                    if (hasId && personIdSet.Contains(ap.ResponsibleIdUser))
                    {
                        isMatch = true;
                    }

                    if (!isMatch && hasName)
                    {
                        var anyName = (ap.ActionPlanResponsibleDisplayName ?? ap.FullName ?? ap.Code) ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(anyName) && personNameSet.Contains(anyName)) isMatch = true;
                    }

                    if (!isMatch && ap.TaskList != null)
                    {
                        foreach (var t in ap.TaskList)
                        {

                            if (hasId)
                            {
                                if (personIdSet.Contains(t.ActionPlanResponsibleIdUser)
                                    || personIdSet.Contains(t.IdActionPlanResponsible)
                                    || personIdSet.Contains(t.IdEmployee)
                                    || personIdSet.Contains(t.ResponsibleIdUser))
                                {
                                    isMatch = true;
                                    break;
                                }
                            }

                            if (hasName)
                            {
                                var tn = (t.TaskResponsibleDisplayName ?? t.Responsible ?? string.Empty).Trim();
                                if (!string.IsNullOrWhiteSpace(tn) && personNameSet.Contains(tn))
                                {
                                    isMatch = true;
                                    break;
                                }
                            }

                            if (t.SubTaskList != null)
                            {
                                foreach (var st in t.SubTaskList)
                                {
                                    if (hasId && personIdSet.Contains(st.IdEmployee))
                                    {
                                        isMatch = true;
                                        break;
                                    }

                                    if (hasName)
                                    {

                                        var stn = (st.Responsible ?? string.Empty).Trim();
                                        if (!string.IsNullOrWhiteSpace(stn) && personNameSet.Contains(stn))
                                        {
                                            isMatch = true;
                                            break;
                                        }
                                    }
                                }
                                if (isMatch) break;
                            }
                        }
                    }

                    if (!isMatch) apMatches = false;
                }

                if (buSet != null && (string.IsNullOrWhiteSpace(ap.BusinessUnit) || !buSet.Contains(ap.BusinessUnit))) apMatches = false;
                if (buSet != null && (string.IsNullOrWhiteSpace(ap.BusinessUnit) || !buSet.Contains(ap.BusinessUnit))) apMatches = false;
                if (originSet != null && (string.IsNullOrWhiteSpace(ap.Origin) || !originSet.Contains(ap.Origin))) apMatches = false;
                if (customerSet != null && (string.IsNullOrWhiteSpace(ap.GroupName) || !customerSet.Contains(ap.GroupName))) apMatches = false;
                if (customerIdSet != null && customerIdSet.Count > 0 && !customerIdSet.Contains(ap.IdSite)) apMatches = false;
                if (codeSet != null && (string.IsNullOrWhiteSpace(ap.Code) || !codeSet.Contains(ap.Code))) apMatches = false;


                if (isTaskView)
                {
                    bool taskFiltersActive = locSet != null || personNameSet != null || personIdSet != null || buSet != null || originSet != null || customerSet != null || codeSet != null;
                    if (taskFiltersActive)
                    {
                        if (ap.TaskList == null || ap.TaskList.Count == 0)
                        {
                            apMatches = false;
                        }
                        else
                        {
                            var matchingTasks = ap.TaskList.Where(t =>
                            {
                                bool mt = true;
                                if (locSet != null && (string.IsNullOrWhiteSpace(t.Location) || !locSet.Contains(t.Location))) mt = false;
                                if (personNameSet != null)
                                {
                                    var tn = (t.TaskResponsibleDisplayName ?? t.Responsible ?? string.Empty).Trim();
                                    if (string.IsNullOrWhiteSpace(tn) || !personNameSet.Contains(tn)) mt = false;
                                }
                                if (personIdSet != null)
                                {
                                    if (!(personIdSet.Contains(t.ActionPlanResponsibleIdUser)
                                          || personIdSet.Contains(t.IdActionPlanResponsible)
                                          || personIdSet.Contains(t.IdEmployee)
                                          || personIdSet.Contains(t.ResponsibleIdUser))) mt = false;
                                }
                                if (buSet != null && (string.IsNullOrWhiteSpace(t.BusinessUnit) || !buSet.Contains(t.BusinessUnit))) mt = false;
                                if (originSet != null && (string.IsNullOrWhiteSpace(t.Origin) && string.IsNullOrWhiteSpace(t.TaskOrigin) || !(originSet.Contains(t.Origin) || originSet.Contains(t.TaskOrigin)))) mt = false;
                                if (customerSet != null && (string.IsNullOrWhiteSpace(t.GroupName) || !customerSet.Contains(t.GroupName))) mt = false;
                                if (codeSet != null && (string.IsNullOrWhiteSpace(t.Code) || !codeSet.Contains(t.Code))) mt = false;
                                return mt;
                            }).ToList();

                            if (matchingTasks.Count == 0) apMatches = false;
                            else
                            {
                                var clone = (APMActionPlan)ap.Clone();
                                clone.TaskList = matchingTasks.Select(t => (APMActionPlanTask)t.Clone()).ToList();
                                baseList.Add(clone);
                                continue;
                            }
                        }
                    }
                }

                if (apMatches)
                {
                    baseList.Add((APMActionPlan)ap.Clone());
                }
            }

            bool sideActive = !string.IsNullOrWhiteSpace(_lastAppliedSideCaption) && !IsAllCaption(_lastAppliedSideCaption) && _lastAppliedSideTileFilter != null;
            if (!sideActive) return baseList;

            var expressions = ParseCriteria(_lastAppliedSideTileFilter.FilterCriteria);
            if (expressions == null || expressions.Count == 0) return baseList;

            bool TaskMatchesAll(APMActionPlan ap, APMActionPlanTask t) =>
                expressions.All(expr => PropertyMatches(t, expr.Field, expr.Values) || PropertyMatches(ap, expr.Field, expr.Values));

            var filtered = new List<APMActionPlan>();
            foreach (var ap in baseList)
            {
                if (ap.TaskList == null || ap.TaskList.Count == 0) continue;
                var tasks = ap.TaskList.Where(t => TaskMatchesAll(ap, t)).ToList();
                if (tasks.Count == 0) continue;
                var clone = (APMActionPlan)ap.Clone();
                clone.TaskList = tasks;
                filtered.Add(clone);
            }
            return filtered;
        }

        private List<APMActionPlan> ApplyAlertToPlans(List<APMActionPlan> plans, string alertCaption)
        {
            if (string.IsNullOrWhiteSpace(alertCaption)) return plans?.Select(p => (APMActionPlan)p.Clone()).ToList() ?? new List<APMActionPlan>();
            var lc = alertCaption.ToLowerInvariant();
            var source = plans?.Select(p => (APMActionPlan)p.Clone()).ToList() ?? new List<APMActionPlan>();
            if (source.Count == 0) return source;

            if (lc.Contains("longest") && lc.Contains("over") && lc.Contains("due"))
            {
                var allTasks = source.Where(p => p.TaskList != null).SelectMany(p => p.TaskList).ToList();
                var task = allTasks.Where(t => t.DueDays > 0 && !IsClosed(t)).OrderByDescending(t => t.DueDays).FirstOrDefault();
                if (task == null) return new List<APMActionPlan>();
                var selectedPlan = source.FirstOrDefault(x => x.IdActionPlan == task.IdActionPlan);
                if (selectedPlan == null) return new List<APMActionPlan>();
                var planClone = (APMActionPlan)selectedPlan.Clone();
                planClone.TaskList = new List<APMActionPlanTask> { (APMActionPlanTask)task.Clone() };
                return new List<APMActionPlan> { planClone };
            }

            Func<APMActionPlanTask, bool> pred = null;

            if (lc.Contains("most overdue theme"))
            {
                var all = source.Where(p => p.TaskList != null).SelectMany(p => p.TaskList).ToList();
                var theme = GetMostOverdueThemeName(all);
                pred = !string.IsNullOrWhiteSpace(theme)
                    ? new Func<APMActionPlanTask, bool>(t => t.DueDays > 0 && !IsClosed(t) && string.Equals((t.Theme ?? "").Trim(), theme.Trim(), StringComparison.OrdinalIgnoreCase))
                    : BuildMostOverdueThemePredicateFromCurrentData(source);
            }
            else if (lc.Contains("most overdue responsible"))
            {
                var all = source.Where(p => p.TaskList != null).SelectMany(p => p.TaskList).ToList();
                var resp = GetMostOverdueResponsibleName(all);
                if (!string.IsNullOrWhiteSpace(resp))
                {
                    var rn = resp.Trim();
                    pred = t => t.DueDays > 0 && !IsClosed(t) && string.Equals((t.TaskResponsibleDisplayName ?? t.Responsible ?? "").Trim(), rn, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    pred = BuildMostOverdueResponsiblePredicateFromCurrentData(source);
                }
            }
            else if (lc.Contains("overdue >= 15"))
            {
                pred = t => t.DueDays >= 15 && !IsClosed(t);
            }
            else
            {
                pred = GetTaskPredicate(alertCaption);
            }

            if (pred == null) return source;

            var result = new List<APMActionPlan>();
            foreach (var plan in source)
            {
                if (plan.TaskList == null) continue;
                var tasks = plan.TaskList.Where(pred).ToList();
                if (tasks.Count == 0) continue;
                var clone = (APMActionPlan)plan.Clone();
                clone.TaskList = tasks;
                result.Add(clone);
            }
            return result;
        }


        private List<APMActionPlan> BuildBaselineForSideCounts_All()
        {
            var dropdownOnly = (_dataBeforeSideTileFilter ?? _currentFilteredBase ?? new List<APMActionPlan>())
                .Select(ap => (APMActionPlan)ap.Clone()).ToList();

            var withAlert = string.IsNullOrWhiteSpace(_lastAppliedAlertCaption)
                ? dropdownOnly
                : ApplyAlertToPlans(dropdownOnly, _lastAppliedAlertCaption);

            return withAlert;
        }

        private List<APMActionPlan> BuildBaselineForSideCounts_NonAll()
        {
            var baseAll = BuildBaselineForSideCounts_All();

            if (!string.IsNullOrWhiteSpace(_lastAppliedSideCaption)
                && !IsAllCaption(_lastAppliedSideCaption)
                && _lastAppliedSideTileFilter != null
                && !string.IsNullOrWhiteSpace(_lastAppliedSideTileFilter.FilterCriteria))
            {
                var exprs = ParseCriteria(_lastAppliedSideTileFilter.FilterCriteria);
                if (exprs != null && exprs.Count > 0)
                {
                    bool TaskMatchesAll(APMActionPlan ap, APMActionPlanTask t) =>
                        exprs.All(e => PropertyMatches(t, e.Field, e.Values) || PropertyMatches(ap, e.Field, e.Values));

                    var filtered = new List<APMActionPlan>();
                    foreach (var ap in baseAll)
                    {
                        if (ap?.TaskList == null || ap.TaskList.Count == 0) continue;
                        var tasks = ap.TaskList.Where(t => TaskMatchesAll(ap, t)).ToList();
                        if (tasks.Count == 0) continue;
                        var clone = (APMActionPlan)ap.Clone();
                        clone.TaskList = tasks;
                        filtered.Add(clone);
                    }
                    return filtered;
                }
            }
            return baseAll;
        }

        private void UpdateSideTileCountsRespectingRules()
        {
            _sideCountsBaseForAll = BuildBaselineForSideCounts_All();
            _sideCountsBaseForNonAll = BuildBaselineForSideCounts_NonAll();
            UpdateSideTileCounts(_sideCountsBaseForNonAll ?? new List<APMActionPlan>());
            _sideCountsBaseForAll = null;
            _sideCountsBaseForNonAll = null;
        }


        private void HandleTaskGridVisibilityChanged(bool isTaskView)
        {
            bool hadAlert = !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption);
            bool hadSide = !string.IsNullOrWhiteSpace(_lastAppliedSideCaption) && _lastAppliedSideTileFilter != null;

            if (hadAlert || hadSide)
            {
                try
                {
                    if (!_apCacheLoaded)
                    {
                        FillActionPlan();
                    }
                    else if (_apCache != null)
                    {
                        var clones = new List<APMActionPlan>();
                        foreach (var ap in _apCache)
                        {
                            clones.Add((APMActionPlan)ap.Clone());
                        }
                        SetActionPlanList(clones);
                    }
                }
                catch { }
            }

            _lastAppliedSideCaption = null;
            _lastAppliedSideTileFilter = null;
            SelectedTileBarItem = null;
            PreviousSelectedTileBarItem = null;
            TemPreviousSelectedTileBarItem = null;

            _lastAppliedAlertCaption = null;
            _alertFilteredBase = null;
            _dataBeforeAlertFilter = null;
            PreviousSelectedAlertTileBarItem = null;
            SelectedAlertTileBarItem = null;
            PreviousSelectedActionPlansAlertTileBarItem = null;
            SelectedActionPlansAlertTileBarItem = null;
            _pendingAllWhileAlert = false;
            _sideTileCountsSnapshot = null;

            try { CancelOngoingFilter(); } catch { }

            _lastViewIsTask = null;
            ApplyInMemoryFiltersAsync();
        }



        private bool _topTileCommandsRewired;
        private TileBarFilters _lastAppliedTopFilter;
        private TileBarFilters _lastAppliedTaskTopFilter;

        private void RewireTopTileBarCommands()
        {
            if (_topTileCommandsRewired) return;
            try
            {

                CommandFilterTileClick = new DevExpress.Mvvm.DelegateCommand<object>(OnTopTileClicked);


                CommandTaskFilterTileClick = new DevExpress.Mvvm.DelegateCommand<object>(OnTaskTopTileClicked);

                _topTileCommandsRewired = true;
                GeosApplication.Instance.Logger.Log("Top Custom Filter commands rewired for cache", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("RewireTopTileBarCommands error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private void OnTopTileClicked(object obj)
        {
            try
            {
                var selectionArgs = obj as System.Windows.Controls.SelectionChangedEventArgs;
                TileBarFilters tile = null;
                if (selectionArgs != null && selectionArgs.AddedItems.Count > 0)
                    tile = selectionArgs.AddedItems[0] as TileBarFilters;

                if (tile == null) return;

                SelectedTopTileBarItem = tile;
                CustomFilterStringName = tile.Caption;

                if (IsAllCaption(tile.Caption))
                {
                    _lastAppliedTopFilter = null;
                    MyFilterString = string.Empty;
                }
                else
                {
                    _lastAppliedTopFilter = tile;
                    MyFilterString = tile.FilterCriteria;
                }

                RecalculateAllCounts();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("OnTopTileClicked error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private void OnTaskTopTileClicked(object obj)
        {
            try
            {
                var selectionArgs = obj as System.Windows.Controls.SelectionChangedEventArgs;
                TileBarFilters tile = null;
                if (selectionArgs != null && selectionArgs.AddedItems.Count > 0)
                    tile = selectionArgs.AddedItems[0] as TileBarFilters;

                if (tile == null) return;


                SelectedTaskTopTileBarItem = tile;
                CustomTaskFilterStringName = tile.Caption;

                if (IsAllCaption(tile.Caption))
                {

                    _lastAppliedTaskTopFilter = null;
                    MyTaskFilterString = string.Empty;
                }
                else
                {
                    _lastAppliedTaskTopFilter = tile;
                    MyTaskFilterString = tile.FilterCriteria;
                }

                RecalculateAllCounts();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("OnTaskTopTileClicked error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        private List<APMActionPlan> ApplyCustomFilterToPlans(List<APMActionPlan> source, string criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria)) return source;
            var result = new List<APMActionPlan>();

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
                    var clone = (APMActionPlan)ap.Clone();
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
                    var match = Regex.Match(criteria, @"\[(.*?)\]\s*=\s*'(.*?)'");
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
                    var match = Regex.Match(criteria, @"\[(.*?)\]\s+In\s*\((.*?)\)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        string field = match.Groups[1].Value;
                        var vals = match.Groups[2].Value.Split(',').Select(v => v.Trim().Trim('\'')).ToHashSet(StringComparer.OrdinalIgnoreCase);
                        string objVal = GetPropValue(obj, field);
                        return objVal != null && vals.Contains(objVal);
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

        private void UpdateTopTileCounts(List<APMActionPlan> source, bool isTaskView)
        {
            try
            {
                var tilesCollection = isTaskView ? TopTaskListOfFilterTile : TopListOfFilterTile;
                if (tilesCollection == null || tilesCollection.Count == 0) return;

                var baseForCounts = source ?? new List<APMActionPlan>();

                foreach (var tile in tilesCollection)
                {
                    if (tile == null) continue;
                    int count = 0;

                    if (IsAllCaption(tile.Caption))
                    {
                        if (baseForCounts.Count > 0)
                        {

                            count = baseForCounts.Sum(ap =>
                                ap.TaskList?.Sum(t => 1 + (t.SubTaskList?.Count ?? 0)) ?? 0);
                        }
                    }
                    else
                    {
                        if (isTaskView)
                        {
                            var allTasks = baseForCounts.Where(ap => ap.TaskList != null)
                                                        .SelectMany(ap => ap.TaskList).ToList();

                            var filteredTasks = ApplyCustomFilterToTasks(allTasks, tile.FilterCriteria);
                            count = filteredTasks.Sum(t => 1 + (t.SubTaskList?.Count ?? 0));
                        }
                        else
                        {
                            var filteredPlans = ApplyCustomFilterToPlans(baseForCounts, tile.FilterCriteria);

                            count = filteredPlans.Sum(ap =>
                                ap.TaskList?.Sum(t => 1 + (t.SubTaskList?.Count ?? 0)) ?? 0);
                        }
                    }

                    tile.EntitiesCount = count;
                    tile.EntitiesCountVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("UpdateTopTileCounts error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }


        private List<APMActionPlan> ApplyCustomFilterToData(List<APMActionPlan> source, TileBarFilters customFilter)
        {
            if (source == null || source.Count == 0) return new List<APMActionPlan>();

            if (customFilter == null || IsAllCaption(customFilter.Caption))
                return source.Select(ap => (APMActionPlan)ap.Clone()).ToList();


            return ApplyCustomFilterToPlans(source, customFilter.FilterCriteria);
        }

        private List<APMActionPlan> ApplySideFilterToData(List<APMActionPlan> source, TileBarFilters sideFilter)
        {
            if (source == null || source.Count == 0) return new List<APMActionPlan>();

            if (sideFilter == null || IsAllCaption(sideFilter.Caption))
                return source.Select(ap => (APMActionPlan)ap.Clone()).ToList();

            var expressions = ParseCriteria(sideFilter.FilterCriteria);
            if (expressions == null || expressions.Count == 0)
                return source.Select(ap => (APMActionPlan)ap.Clone()).ToList();

            bool TaskMatchesAll(APMActionPlan ap, APMActionPlanTask t) =>
                expressions.All(expr => PropertyMatches(t, expr.Field, expr.Values) || PropertyMatches(ap, expr.Field, expr.Values));

            var filtered = new List<APMActionPlan>();
            foreach (var ap in source)
            {
                if (ap.TaskList == null || ap.TaskList.Count == 0) continue;
                var matchingTasks = ap.TaskList.Where(t => TaskMatchesAll(ap, t)).ToList();
                if (matchingTasks.Count > 0)
                {
                    var clone = (APMActionPlan)ap.Clone();
                    clone.TaskList = matchingTasks.Select(t => (APMActionPlanTask)t.Clone()).ToList();
                    filtered.Add(clone);
                }
            }
            return filtered;
        }

        private List<APMActionPlan> ApplyAlertFilterToData(List<APMActionPlan> source, string alertCaption)
        {
            if (source == null || source.Count == 0) return new List<APMActionPlan>();
            if (string.IsNullOrWhiteSpace(alertCaption))
                return source.Select(ap => (APMActionPlan)ap.Clone()).ToList();

            return ApplyAlertToPlans(source, alertCaption);
        }

        private void RecalculateAllCounts()
        {
            try
            {
                var baseData = _currentFilteredBase?.Select(ap => (APMActionPlan)ap.Clone()).ToList()
                               ?? new List<APMActionPlan>();

                bool isTaskView = IsTaskGridVisibility;

                var activeSideFilter = _lastAppliedSideTileFilter;
                var activeAlertCaption = _lastAppliedAlertCaption;
                var activeCustomFilter = isTaskView ? _lastAppliedTaskTopFilter : _lastAppliedTopFilter;

                var dataForSideCounts = ApplyAlertFilterToData(
                                            ApplyCustomFilterToData(baseData, activeCustomFilter),
                                            activeAlertCaption);

                var dataForCustomCounts = ApplyAlertFilterToData(
                                            ApplySideFilterToData(baseData, activeSideFilter),
                                            activeAlertCaption);

                var dataForAlertCounts = ApplyCustomFilterToData(
                                            ApplySideFilterToData(baseData, activeSideFilter),
                                            activeCustomFilter);

                UpdateSideTileCounts(dataForSideCounts);
                UpdateTopTileCounts(dataForCustomCounts, isTaskView);
                UpdateAlertFiltersBasedOnFilteredData(dataForAlertCounts);


                var finalDataForGrid = ApplyAlertFilterToData(
                                            ApplyCustomFilterToData(
                                                ApplySideFilterToData(baseData, activeSideFilter),
                                                activeCustomFilter),
                                            activeAlertCaption);

                ActionPlanList = new ObservableCollection<APMActionPlan>(finalDataForGrid);
                SetDeletionFlags(ActionPlanList);

                try
                {
                    ClonedActionPlanList = ActionPlanList.Select(ap => (APMActionPlan)ap.Clone()).ToList();
                }
                catch { }

                if (isTaskView)
                {
                    RefreshTaskGridFromCurrentPlans();
                }
                else if (IsExpand)
                {
                    ExpandLastUpdatedActionPlan();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("RecalculateAllCounts error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }




        private void RebuildTaskStatusBucketsFromTasks(IEnumerable<APMActionPlanTask> tasks)
        {
            try
            {
                if (tasks == null) return;

                if (statusList == null)
                    statusList = new List<LookupValue>(APMService.GetLookupValues_V2550(152));

                ListStatus = new ObservableCollection<TaskStatuswise>();


                statusList.ForEach(x => ListStatus.Add(new TaskStatuswise
                {
                    IdLookupValue = x.IdLookupValue,
                    Value = x.Value,
                    HtmlColor = x.HtmlColor,
                    TaskList = new List<APMActionPlanTask>()
                }));

                foreach (var t in tasks)
                {
                    var bucket = ListStatus.FirstOrDefault(p => p.IdLookupValue == t.IdLookupStatus);
                    if (bucket != null)
                    {

                        if (DateTime.Now.Date < t.DueDate.Date.AddDays(2)) t.CardDueColor = "#008000";
                        else if (DateTime.Now.Date < t.DueDate.Date.AddDays(7)) t.CardDueColor = "#FFB913";
                        else t.CardDueColor = "#FF0000";

                        if (t.Status == "Done" || t.Status == "Closed") t.CardDueColor = "#008000";

                        bucket.TaskList.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("RebuildTaskStatusBucketsFromTasks error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

    }
}
