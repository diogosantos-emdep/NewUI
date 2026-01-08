using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;

using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    /// <summary>
    /// ViewModel MODERNO para Action Plans - Otimizado para alta performance
    /// Features: Async paging, virtualização, lazy-load master/detail, cache inteligente
    /// </summary>
    public partial class ActionPlansModernViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        private readonly IAPMService _apmService;
        #endregion

        #region Fields
        private ObservableCollection<ActionPlanModernDto> _actionPlans;
        private List<ActionPlanModernDto> _allActionPlansUnfiltered; // Dados originais sem filtro para filtragem local
        private ActionPlanModernDto _selectedActionPlan;
        private ObservableCollection<ActionPlanTaskModernDto> _selectedActionPlanTasks;
        private bool _isBusy;
        private bool _isLoadingMore;
        private string _searchText;
        private int _currentPage;
        private int _pageSize = 100; // Carregar 100 registos de cada vez (otimizado)
        private bool _hasMoreData = true;
        private List<APMActionPlanModern> _allDataCache;
        private CancellationTokenSource _loadCancellationTokenSource;
        private CancellationTokenSource _searchCancellationTokenSource;
        
        // Flag para prevenir loop infinito quando dropdowns mudam
        private bool _isRefreshing = false;

        // Cache inteligente: IdActionPlan -> List<Tasks>
        private readonly Dictionary<long, List<ActionPlanTaskModernDto>> _tasksCache;
        private readonly object _cacheLock = new object();

        // Cache de imagens de responsáveis: EmployeeCode -> ImageSource
        private readonly Dictionary<string, ImageSource> _inMemoryImageCache = new Dictionary<string, ImageSource>();
        #endregion

        #region Properties

        public ObservableCollection<ActionPlanModernDto> ActionPlans
        {
            get => _actionPlans;
            set
            {
                _actionPlans = value;
                OnPropertyChanged(nameof(ActionPlans));
            }
        }

        public ActionPlanModernDto SelectedActionPlan
        {
            get => _selectedActionPlan;
            set
            {
                if (_selectedActionPlan != value)
                {
                    _selectedActionPlan = value;
                    OnPropertyChanged(nameof(SelectedActionPlan));
                }
            }
        }

        public ObservableCollection<ActionPlanTaskModernDto> SelectedActionPlanTasks
        {
            get => _selectedActionPlanTasks;
            set
            {
                _selectedActionPlanTasks = value;
                OnPropertyChanged(nameof(SelectedActionPlanTasks));
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public bool IsLoadingMore
        {
            get => _isLoadingMore;
            set
            {
                _isLoadingMore = value;
                OnPropertyChanged(nameof(IsLoadingMore));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));

                    // Debounce: aguardar 500ms antes de pesquisar
                    _ = DebounceSearchAsync();
                }
            }
        }

        #endregion

        #region Filter Properties

        private List<object> _selectedLocation;
        private List<object> _selectedPerson;
        private List<object> _selectedBusinessUnit;
        private List<object> _selectedOrigin;
        private List<object> _selectedDepartment;
        private List<object> _selectedCustomer;
        private ObservableCollection<Responsible> _listOfPerson;
        private ObservableCollection<APMCustomer> _listOfCustomer;

        // Alert Buttons
        private ObservableCollection<APMAlertTileBarFilters> _alertListOfFilterTile;
        private APMAlertTileBarFilters _selectedAlertTileBarItem;
        private bool _isAlertSectionCollapsed;

        public List<object> SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                if (_isRefreshing) return; // Prevenir loop
                _selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
                _ = RefreshDataAsync(); // MODERNUI: Reload from SQL with filters
            }
        }

        public List<object> SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (_isRefreshing) return; // Prevenir loop
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
                _ = RefreshDataAsync(); // MODERNUI: Reload from SQL with filters
            }
        }

        public List<object> SelectedBusinessUnit
        {
            get => _selectedBusinessUnit;
            set
            {
                if (_isRefreshing) return; // Prevenir loop
                _selectedBusinessUnit = value;
                OnPropertyChanged(nameof(SelectedBusinessUnit));
                _ = RefreshDataAsync(); // MODERNUI: Reload from SQL with filters
            }
        }

        public List<object> SelectedOrigin
        {
            get => _selectedOrigin;
            set
            {
                if (_isRefreshing) return; // Prevenir loop
                _selectedOrigin = value;
                OnPropertyChanged(nameof(SelectedOrigin));
                _ = RefreshDataAsync(); // MODERNUI: Reload from SQL with filters
            }
        }

        public List<object> SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                if (_isRefreshing) return; // Prevenir loop
                _selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
                _ = RefreshDataAsync(); // MODERNUI: Reload from SQL with filters
            }
        }

        public List<object> SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (_isRefreshing) return; // Prevenir loop
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
                _ = RefreshDataAsync(); // MODERNUI: Reload from SQL with filters
            }
        }

        public ObservableCollection<Responsible> ListOfPerson
        {
            get => _listOfPerson;
            set
            {
                _listOfPerson = value;
                OnPropertyChanged(nameof(ListOfPerson));
            }
        }

        public ObservableCollection<APMCustomer> ListOfCustomer
        {
            get => _listOfCustomer;
            set
            {
                _listOfCustomer = value;
                OnPropertyChanged(nameof(ListOfCustomer));
            }
        }

        public ObservableCollection<APMAlertTileBarFilters> AlertListOfFilterTile
        {
            get => _alertListOfFilterTile;
            set
            {
                _alertListOfFilterTile = value;
                OnPropertyChanged(nameof(AlertListOfFilterTile));
            }
        }

        public APMAlertTileBarFilters SelectedAlertTileBarItem
        {
            get => _selectedAlertTileBarItem;
            set
            {
                _selectedAlertTileBarItem = value;
                OnPropertyChanged(nameof(SelectedAlertTileBarItem));
            }
        }

        public bool IsAlertSectionCollapsed
        {
            get => _isAlertSectionCollapsed;
            set
            {
                _isAlertSectionCollapsed = value;
                OnPropertyChanged(nameof(IsAlertSectionCollapsed));
            }
        }

        // Side Tiles (Theme filters)
        private ObservableCollection<TileBarFilters> _listOfFilterTile;
        private TileBarFilters _selectedTileBarItem;

        public ObservableCollection<TileBarFilters> ListOfFilterTile
        {
            get => _listOfFilterTile;
            set
            {
                _listOfFilterTile = value;
                OnPropertyChanged(nameof(ListOfFilterTile));
            }
        }

        public TileBarFilters SelectedTileBarItem
        {
            get => _selectedTileBarItem;
            set
            {
                _selectedTileBarItem = value;
                OnPropertyChanged(nameof(SelectedTileBarItem));
            }
        }

        // Top/Custom Tiles (for custom filters)
        private ObservableCollection<TileBarFilters> _topListOfFilterTile;
        private ObservableCollection<TileBarFilters> _topTaskListOfFilterTile;

        public ObservableCollection<TileBarFilters> TopListOfFilterTile
        {
            get => _topListOfFilterTile;
            set 
            { 
                _topListOfFilterTile = value; 
                OnPropertyChanged(nameof(TopListOfFilterTile)); 
            }
        }

        public ObservableCollection<TileBarFilters> TopTaskListOfFilterTile
        {
            get => _topTaskListOfFilterTile;
            set 
            { 
                _topTaskListOfFilterTile = value; 
                OnPropertyChanged(nameof(TopTaskListOfFilterTile)); 
            }
        }

        #endregion

        #region Missing String Properties

        private string _searchString;
        public string SearchString
        {
            get => _searchString;
            // Debounce maior (500ms) para texto livre
            set { _searchString = value; OnPropertyChanged(nameof(SearchString)); ApplyInMemoryFiltersAsync(500); }
        }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set { _selectedStatus = value; OnPropertyChanged(nameof(SelectedStatus)); ApplyInMemoryFiltersAsync(); }
        }

        private string _selectedPriority;
        public string SelectedPriority
        {
            get => _selectedPriority;
            set { _selectedPriority = value; OnPropertyChanged(nameof(SelectedPriority)); ApplyInMemoryFiltersAsync(); }
        }

        private string _selectedTheme;
        public string SelectedTheme
        {
            get => _selectedTheme;
            set { _selectedTheme = value; OnPropertyChanged(nameof(SelectedTheme)); ApplyInMemoryFiltersAsync(); }
        }

        #endregion

        #region Commands

        public DelegateCommand LoadMoreCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand CancelLoadCommand { get; private set; }
        public DelegateCommand<ActionPlanModernDto> ToggleActionPlanExpandCommand { get; private set; }
        public DelegateCommand<ActionPlanTaskModernDto> ToggleTaskExpandCommand { get; private set; }
        public DelegateCommand ExpandAllActionPlansCommand { get; private set; }
        public DelegateCommand CollapseAllActionPlansCommand { get; private set; }
        public DelegateCommand<object> OpenTaskCommand { get; private set; }
        public DelegateCommand<object> OpenSubTaskCommand { get; private set; }

        // Filter Commands
        public DelegateCommand<object> LocationFilterChangedCommand { get; private set; }
        public DelegateCommand<object> ResponsibleFilterChangedCommand { get; private set; }
        public DelegateCommand<object> BusinessUnitFilterChangedCommand { get; private set; }
        public DelegateCommand<object> OriginFilterChangedCommand { get; private set; }
        public DelegateCommand<object> DepartmentFilterChangedCommand { get; private set; }
        public DelegateCommand<object> CustomerFilterChangedCommand { get; private set; }
        public DelegateCommand ClearFiltersCommand { get; private set; }

        // Alert Button Commands
        public DelegateCommand<APMAlertTileBarFilters> AlertButtonClickCommand { get; private set; }

        // Side Tile Commands
        public DelegateCommand<object> SideTileClickCommand { get; private set; }

        // Toolbar Commands
        public DelegateCommand AddActionPlanCommand { get; private set; }
        public DelegateCommand ImportCommand { get; private set; }
        public DelegateCommand<object> PrintButtonCommand { get; private set; }
        public DelegateCommand ExportButtonCommand { get; private set; }
        public DelegateCommand ExportCustomerButtonCommand { get; private set; }
        public DelegateCommand<object> SelectedYearChangedCommand { get; private set; }

        // Switch View Commands
        public DelegateCommand SwitchToGridViewCommand { get; private set; }
        public DelegateCommand SwitchToTaskViewCommand { get; private set; }
        public DelegateCommand SwitchToTaskCardsCommand { get; private set; }

        #endregion

        #region Constructor

        public ActionPlansModernViewModel()
        {
            try
            {
                // Validações essenciais no construtor
                if (GeosApplication.Instance == null)
                {
                    throw new InvalidOperationException("GeosApplication.Instance is null - Application not initialized");
                }

                if (GeosApplication.Instance.ApplicationSettings == null)
                {
                    throw new InvalidOperationException("ApplicationSettings is null - Configuration not loaded");
                }

                if (!GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
                {
                    throw new InvalidOperationException("ServicePath not found in ApplicationSettings");
                }

                string servicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"]?.ToString();
                if (string.IsNullOrEmpty(servicePath))
                {
                    throw new InvalidOperationException("ServicePath is empty in ApplicationSettings");
                }

                _apmService = new APMServiceController(servicePath);
                _tasksCache = new Dictionary<long, List<ActionPlanTaskModernDto>>();

                ActionPlans = new ObservableCollection<ActionPlanModernDto>();
                SelectedActionPlanTasks = new ObservableCollection<ActionPlanTaskModernDto>();

                // Inicializar CancellationTokenSource para evitar NullRef
                _loadCancellationTokenSource = new CancellationTokenSource();
                _searchCancellationTokenSource = new CancellationTokenSource();

                InitializeCommands();

                GeosApplication.Instance.Logger?.Log("ActionPlansModernViewModel created successfully", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance?.Logger?.Log($"CRITICAL: ActionPlansModernViewModel constructor failed: {ex.Message}", Category.Exception, Priority.High);
                throw; // Re-throw para o chamador saber que houve erro
            }
        }

        private void InitializeCommands()
        {
            LoadMoreCommand = new DelegateCommand(async () => await LoadActionPlansPageAsync(), () => !IsBusy && !IsLoadingMore && _hasMoreData);
            RefreshCommand = new DelegateCommand(async () => await RefreshDataAsync());
            CancelLoadCommand = new DelegateCommand(CancelCurrentLoad);
            ToggleActionPlanExpandCommand = new DelegateCommand<ActionPlanModernDto>(async (ap) => await ToggleActionPlanExpandAsync(ap));
            ToggleTaskExpandCommand = new DelegateCommand<ActionPlanTaskModernDto>(async (task) => await ToggleTaskExpandAsync(task));
            ExpandAllActionPlansCommand = new DelegateCommand(async () => await ExpandAllActionPlansAsync());
            CollapseAllActionPlansCommand = new DelegateCommand(CollapseAllActionPlans);
            OpenTaskCommand = new DelegateCommand<object>(OpenTaskFromEventArgs);
            OpenSubTaskCommand = new DelegateCommand<object>(OpenSubTaskFromEventArgs);

            // Toolbar Commands
            AddActionPlanCommand = new DelegateCommand(AddNewActionPlan);
            ImportCommand = new DelegateCommand(ImportActionPlan);
            PrintButtonCommand = new DelegateCommand<object>(PrintButtonAction);
            ExportButtonCommand = new DelegateCommand(ExportStandardAction);
            ExportCustomerButtonCommand = new DelegateCommand(ExportCustomerAction);
            SelectedYearChangedCommand = new DelegateCommand<object>(SelectedYearChangedAction);

            // Switch View Commands
            SwitchToGridViewCommand = new DelegateCommand(SwitchToGridView);
            SwitchToTaskViewCommand = new DelegateCommand(SwitchToTaskView);
            SwitchToTaskCardsCommand = new DelegateCommand(SwitchToTaskCards);

            // Filter Commands
            LocationFilterChangedCommand = new DelegateCommand<object>(OnLocationFilterChanged);
            ResponsibleFilterChangedCommand = new DelegateCommand<object>(OnResponsibleFilterChanged);
            BusinessUnitFilterChangedCommand = new DelegateCommand<object>(OnBusinessUnitFilterChanged);
            OriginFilterChangedCommand = new DelegateCommand<object>(OnOriginFilterChanged);
            DepartmentFilterChangedCommand = new DelegateCommand<object>(OnDepartmentFilterChanged);
            CustomerFilterChangedCommand = new DelegateCommand<object>(OnCustomerFilterChanged);
            ClearFiltersCommand = new DelegateCommand(ClearAllFilters);

            // Alert Button Commands
            AlertButtonClickCommand = new DelegateCommand<APMAlertTileBarFilters>(OnAlertButtonClick);

            // Side Tile Commands
            SideTileClickCommand = new DelegateCommand<object>(OnSideTileClick);
        }

        #endregion

        #region Initialization

        public async Task InitAsync()
        {
            var startTime = DateTime.Now;
            try
            {
                GeosApplication.Instance.Logger.Log("=== AUTO-LOAD STARTED (pageSize=100, parallel mapping) ===", Category.Info, Priority.Low);

                IsBusy = true;
                _currentPage = 0;
                _hasMoreData = true;

                // Initialize dropdown filter lists from service
                InitializeFilterDropdowns();

                // AUTO-LOAD: Carrega todas as páginas automaticamente
                await LoadAllPagesAsync();

                // Fill filter lists after loading all data
                FillFilterLists();

                // Store unfiltered data for local filtering
                _allActionPlansUnfiltered = ActionPlans.ToList();

                // Populate alert buttons with counts
                PopulateAlertButtons();

                // Populate side tiles with themes
                PopulateSideTiles();

                var elapsed = (DateTime.Now - startTime).TotalSeconds;
                GeosApplication.Instance.Logger.Log($"=== AUTO-LOAD COMPLETED in {elapsed:F2}s - {ActionPlans.Count} Action Plans ===", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in ActionPlansModernViewModel.InitAsync(): {ex.Message}", Category.Exception, Priority.High);
                CustomMessageBox.Show($"Erro ao carregar Action Plans: {ex.Message}", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// AUTO-LOAD: Carrega todas as páginas automaticamente (50 por vez)
        /// </summary>
        private async Task LoadAllPagesAsync()
        {
            try
            {
                while (_hasMoreData && !_loadCancellationTokenSource.Token.IsCancellationRequested)
                {
                    await LoadActionPlansPageAsync();

                    // SEM delay - máxima velocidade!
                    // await Task.Delay(100);
                }

                GeosApplication.Instance.Logger.Log($"Auto-load completed: {ActionPlans.Count} total Action Plans loaded", Category.Info, Priority.Low);


                // Popular os alert buttons após carregar todos os dados
                PopulateAlertButtons();

                // Update alert button counts based on current filtered state
                UpdateAlertButtonCounts();
            }
            catch (OperationCanceledException)
            {
                GeosApplication.Instance.Logger.Log("Auto-load cancelled by user", Category.Info, Priority.Low);
            }
        }

        /// <summary>
        /// Carrega todas as Tasks/SubTasks para todos os ActionPlans no _allDataCache
        /// Necessário para os alert buttons calcularem contagens corretas
        /// </summary>
        private async Task PopulateAllTasksInCacheAsync()
        {
            try
            {
                if (_allDataCache == null || _allDataCache.Count == 0)
                {
                    GeosApplication.Instance.Logger?.Log("PopulateAllTasksInCacheAsync: _allDataCache is empty", Category.Info, Priority.Low);
                    return;
                }

                GeosApplication.Instance.Logger?.Log($"Populating Tasks/SubTasks for {_allDataCache.Count} Action Plans...", Category.Info, Priority.Low);

                var startTime = DateTime.Now;

                string period;
                if (APMCommon.Instance?.SelectedPeriod != null && APMCommon.Instance.SelectedPeriod.Count > 0)
                {
                    var selectedYear = APMCommon.Instance.SelectedPeriod.Cast<long>().FirstOrDefault();
                    period = selectedYear.ToString();
                }
                else
                {
                    period = DateTime.Now.Year.ToString();
                }

                int userId = GeosApplication.Instance.ActiveUser.IdUser;

                // Carregar Tasks para todos os ActionPlans em paralelo (batch de 10 para não sobrecarregar)
                // NOTA: GetTaskListByIdActionPlan_V2680 já retorna as Tasks COM as SubTasks incluídas
                await Task.Run(() =>
                {
                    var options = new ParallelOptions { MaxDegreeOfParallelism = 10 };
                    
                    Parallel.ForEach(_allDataCache, options, actionPlan =>
                    {
                        try
                        {
                            // Carregar Tasks do serviço (já inclui SubTasks)
                            var tasks = _apmService.GetTaskListByIdActionPlan_V2680(actionPlan.IdActionPlan, period, userId);
                            
                            if (tasks != null && tasks.Count > 0)
                            {
                                // Popular TaskList do ActionPlan (entidade, não DTO)
                                // As SubTasks já vêm populadas dentro de cada task.SubTaskList
                                actionPlan.TaskList = tasks;
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger?.Log($"Error loading Tasks for ActionPlan {actionPlan.IdActionPlan}: {ex.Message}", Category.Exception, Priority.Low);
                        }
                    });
                });

                var elapsed = (DateTime.Now - startTime).TotalSeconds;
                
                // Calcular estatísticas
                int totalTasks = _allDataCache.Sum(ap => ap.TaskList?.Count ?? 0);
                int totalSubTasks = _allDataCache.Sum(ap => ap.TaskList?.Sum(t => t.SubTaskList?.Count ?? 0) ?? 0);

                GeosApplication.Instance.Logger?.Log($"Populated {totalTasks} Tasks and {totalSubTasks} SubTasks in {elapsed:F2}s", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in PopulateAllTasksInCacheAsync: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        #endregion

        #region Data Loading - Async Paging



        private async Task LoadActionPlansPageAsync()
        {
            // [FIX LOOP] Impede re-entradas se já estiver a carregar
            if (IsLoadingMore) return;

            try
            {
                if (GeosApplication.Instance == null || GeosApplication.Instance.ActiveUser == null) return;

                // Cancelar pedidos anteriores
                _loadCancellationTokenSource?.Cancel();
                _loadCancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = _loadCancellationTokenSource.Token;

                IsLoadingMore = true;

                // --- 1. CARREGAMENTO DOS DADOS (BACKEND) ---
                if (_allDataCache == null)
                {
                    _allDataCache = await Task.Run(() =>
                    {
                        // [LÓGICA DE ANOS]
                        long selectedYear = DateTime.Now.Year;
                        if (APMCommon.Instance?.SelectedPeriod?.Count > 0)
                        {
                            selectedYear = APMCommon.Instance.SelectedPeriod.Cast<long>().FirstOrDefault();
                        }

                        string period = $"{selectedYear},{selectedYear - 1}";
                        int userId = GeosApplication.Instance.ActiveUser.IdUser;

                        // [MODERNUI - CONSTRUIR FILTROS SQL]
                        // Passar NULL permite ao SQL filtrar diretamente (muito mais rápido)
                        string filterLocation = BuildLocationFilter();
                        string filterResponsible = BuildResponsibleFilter();
                        string filterBusinessUnit = BuildBusinessUnitFilter();
                        string filterOrigin = BuildOriginFilter();
                        string filterDepartment = BuildDepartmentFilter();
                        string filterCustomer = BuildCustomerFilter();
                        string alertFilter = GetCurrentAlertFilter();
                        string themeFilter = GetCurrentThemeFilter();

                        GeosApplication.Instance.Logger?.Log(
                            $"[LoadActionPlansAsync] Calling SP with filters: " +
                            $"Location={filterLocation ?? "null"}, " +
                            $"Responsible={filterResponsible ?? "null"}, " +
                            $"BU={filterBusinessUnit ?? "null"}, " +
                            $"Alert={alertFilter ?? "null"}, " +
                            $"Theme={themeFilter ?? "null"}",
                            Category.Info, Priority.Low);

                        // [CONFIGURAÇÃO DE REDE]
                        string ip = "10.13.3.33";
                        string port = "90";
                        string path = "";
                        var localService = new APMServiceController(ip, port, path);

                        return localService.GetActionPlanDetails_WithCounts(
                            period, 
                            userId, 
                            filterLocation, 
                            filterResponsible, 
                            filterBusinessUnit, 
                            filterOrigin, 
                            filterDepartment, 
                            filterCustomer, 
                            alertFilter, 
                            themeFilter);

                    }, cancellationToken);

                    GeosApplication.Instance.Logger?.Log(
                        $"[LoadActionPlansPageAsync] SP returned {(_allDataCache == null ? "NULL" : _allDataCache.Count + " Action Plans")}",
                        Category.Info, Priority.Low);

                    // Se vier vazio, sair
                    if (_allDataCache == null || _allDataCache.Count == 0)
                    {
                        GeosApplication.Instance.Logger?.Log(
                            "[LoadActionPlansPageAsync] NO ACTION PLANS RETURNED - Aborting",
                            Category.Warn, Priority.Medium);
                        _hasMoreData = false;
                        IsLoadingMore = false;
                        return;
                    }

                    // ==============================================================================
                    // [CORREÇÃO CRÍTICA] Preencher a Lista Mestra de DTOs IMEDIATAMENTE
                    // ==============================================================================
                    // Isto garante que o ApplyFilters tem dados para trabalhar sem ir à BD
                    _allActionPlansUnfiltered = _allDataCache.Select(MapToDto).Where(x => x != null).ToList();

                    // --- 2. SINCRONIZAÇÃO DA UI ---
                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            // Atualiza os contadores laterais usando a lista mestra
                            UpdateSideTileCounts();
                            UpdateAlertButtonCounts();

                            // Se o método PopulateDropdownFilters esperar a lista original (APMActionPlanModern), usa o _allDataCache
                            PopulateDropdownFilters(_allDataCache);
                        });
                    }
                }

                if (cancellationToken.IsCancellationRequested) return;

                // --- 3. PAGINAÇÃO E VISUALIZAÇÃO NA GRID ---

                // MODERNUI FIX: Se Theme ou Alert ativos, o SP JÁ FILTROU!
                // NÃO aplicar filtros in-memory (ApplyFilters) porque o novo SP não retorna ThemeAggregates
                bool hasSideTile = _lastAppliedSideTileFilter != null && !IsAllCaption(_lastAppliedSideTileFilter.Caption);
                bool hasAlert = !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption);
                
                if (hasSideTile || hasAlert)
                {
                    // Theme/Alert: SP já filtrou, apenas mostrar tudo (sem paginação)
                    GeosApplication.Instance.Logger?.Log(
                        $"[LoadActionPlansPageAsync] Theme/Alert active - showing all {_allActionPlansUnfiltered.Count} APs returned by SP",
                        Category.Info, Priority.Low);
                    
                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            foreach (var dto in _allActionPlansUnfiltered)
                            {
                                ActionPlans.Add(dto);
                            }
                        });
                    }
                    _hasMoreData = false;
                }
                else
                {
                    // Verifica se há filtros de DROPDOWN ativos
                    bool hasDropdowns = (SelectedLocation != null && SelectedLocation.Count > 0) ||
                                        (SelectedPerson != null && SelectedPerson.Count > 0) ||
                                        (SelectedBusinessUnit != null && SelectedBusinessUnit.Count > 0) ||
                                        (SelectedOrigin != null && SelectedOrigin.Count > 0) ||
                                        (SelectedDepartment != null && SelectedDepartment.Count > 0) ||
                                        (SelectedCustomer != null && SelectedCustomer.Count > 0);

                    if (hasDropdowns)
                    {
                        // [MODO FILTRO DROPDOWN] Mostra tudo o que corresponder (sem paginação)
                        _pageSize = 999999;
                        ApplyFilters(); // Este método vai preencher a ActionPlans
                        _hasMoreData = false;
                    }
                    else
                    {
                        // [MODO PAGINAÇÃO] Mostra em blocos de 50
                        _pageSize = 50;
                        var skip = _currentPage * _pageSize;

                        // Paginar sobre a lista mestra de DTOs
                        var pagedDtos = _allActionPlansUnfiltered.Skip(skip).Take(_pageSize).ToList();

                        if (Application.Current != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                foreach (var dto in pagedDtos)
                                {
                                    ActionPlans.Add(dto);
                                }
                            });
                        }

                        _currentPage++;

                        // Verificar se ainda há mais dados para scroll
                        _hasMoreData = (pagedDtos.Count == _pageSize) && (_allActionPlansUnfiltered.Count > (skip + pagedDtos.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in LoadActionPlansPageAsync: {ex.Message}", Category.Exception, Priority.High);
            }
            finally
            {
                IsLoadingMore = false;
                LoadMoreCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Master/Detail - Lazy Load Tasks

        /// <summary>
        /// Carrega tasks para um Action Plan específico (chamado ao expandir a row)
        /// </summary>
        public async Task LoadTasksForActionPlanAsync(ActionPlanModernDto actionPlan)
        {
            GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] INÍCIO - ActionPlan {actionPlan?.Code}", Category.Info, Priority.Low);

            // 1. Validações Iniciais
            if (actionPlan == null)
            {
                GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] ActionPlan is NULL - ABORTING", Category.Info, Priority.Low);
                return;
            }

            if (actionPlan.IsLoadingTasks)
            {
                GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] Already loading for {actionPlan.Code} - ABORTING", Category.Info, Priority.Low);
                return;
            }

            // Se já tem tasks carregadas, não recarregar (evita chamadas redundantes)
            if (actionPlan.Tasks != null && actionPlan.Tasks.Count > 0)
            {
                GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} já tem {actionPlan.Tasks.Count} tasks carregadas - ABORTING", Category.Info, Priority.Low);
                return;
            }

            try
            {
                GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - Setting IsLoadingTasks = true", Category.Info, Priority.Low);
                actionPlan.IsLoadingTasks = true;

                // 2. Preparar Parâmetros
                string period = DateTime.Now.Year.ToString();
                if (APMCommon.Instance?.SelectedPeriod != null && APMCommon.Instance.SelectedPeriod.Count > 0)
                {
                    period = APMCommon.Instance.SelectedPeriod.Cast<long>().FirstOrDefault().ToString();
                }
                int userId = GeosApplication.Instance.ActiveUser.IdUser;

                // 3. Chamada ao Serviço (Executado em Thread Secundária)
                List<APMActionPlanTask> tasksEntityList = null;

                try
                {
                    // Tentativa preferencial: V2680PT
                    GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - Attempting GetTaskListByIdActionPlan_V2680PT", Category.Info, Priority.Low);

                    tasksEntityList = await Task.Run(() => _apmService.GetTaskListByIdActionPlan_V2680PT(
                        actionPlan.IdActionPlan,
                        period,
                        userId,
                        null, null, null, null, null, null, null, null // Filtros a null para trazer tudo deste plano
                    ));

                    GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - V2680PT returned: {(tasksEntityList == null ? "NULL" : tasksEntityList.Count + " tasks")}", Category.Info, Priority.Low);
                }
                catch (Exception exPT)
                {
                    // Fallback para V2680 se o novo endpoint falhar
                    GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - V2680PT failed: {exPT.Message}. Falling back to V2680.", Category.Warn, Priority.Medium);

                    try
                    {
                        tasksEntityList = await Task.Run(() => _apmService.GetTaskListByIdActionPlan_V2680(actionPlan.IdActionPlan, period, userId));
                        GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - V2680 fallback returned tasks", Category.Info, Priority.Low);
                    }
                    catch (Exception exV2680)
                    {
                        GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - V2680 fallback also failed: {exV2680.Message}", Category.Exception, Priority.High);
                        throw;
                    }
                }

                // 4. Mapeamento (DTOs)
                var loadedTasks = new ObservableCollection<ActionPlanTaskModernDto>();

                if (tasksEntityList != null && tasksEntityList.Count > 0)
                {
                    GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - Processing {tasksEntityList.Count} tasks", Category.Info, Priority.Low);

                    foreach (var taskEntity in tasksEntityList)
                    {
                        var taskDto = MapTaskToDto(taskEntity);

                        if (taskDto != null)
                        {
                            // Mapeamento de SubTasks
                            if (taskEntity.SubTaskList != null && taskEntity.SubTaskList.Count > 0)
                            {
                                var subDtos = new ObservableCollection<ActionPlanTaskModernDto>();
                                foreach (var subEntity in taskEntity.SubTaskList)
                                {
                                    var subDto = MapSubTaskToActionPlanTaskDto(subEntity);
                                    if (subDto != null) subDtos.Add(subDto);
                                }

                                taskDto.SubTasks = subDtos;
                                taskDto.IsLoadingSubTasks = false;
                                taskDto.TotalSubTasks = subDtos.Count;

                                // Nota: Assume que o método helper IsClosedExact existe na classe
                                taskDto.CompletedSubTasks = subDtos.Count(x => IsClosedExact(x.Status, x.IdLookupStatus));
                            }
                            else
                            {
                                taskDto.TotalSubTasks = 0;
                                taskDto.CompletedSubTasks = 0;
                            }

                            loadedTasks.Add(taskDto);
                        }
                    }
                }
                else
                {
                    GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - Service returned NULL or empty list", Category.Info, Priority.Low);
                }

                // 5. Atualização da UI (Thread Principal)
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // Atribui tasks ao objeto DTO existente
                        actionPlan.Tasks = loadedTasks;
                        actionPlan.VisibleTasks = loadedTasks;
                        actionPlan.TasksCount = loadedTasks.Count;

                        // Garante que a coleção de SubTasks não é nula (evita erros de binding no XAML)
                        foreach (var t in actionPlan.Tasks ?? new ObservableCollection<ActionPlanTaskModernDto>())
                        {
                            if (t.SubTasks == null) t.SubTasks = new ObservableCollection<ActionPlanTaskModernDto>();
                        }

                        GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - ASSIGNED {loadedTasks.Count} tasks to Tasks collection", Category.Info, Priority.Low);

                    });
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] EXCEPTION for {actionPlan.Code}: {ex.Message}", Category.Exception, Priority.High);

                // Em caso de erro, garante que a UI recebe uma lista vazia para sair do estado de loading
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (actionPlan.Tasks == null) actionPlan.Tasks = new ObservableCollection<ActionPlanTaskModernDto>();
                    });
                }
            }
            finally
            {
                actionPlan.IsLoadingTasks = false;
                GeosApplication.Instance.Logger?.Log($"[LoadTasksForActionPlanAsync] {actionPlan.Code} - FINISHED", Category.Info, Priority.Low);
            }
        }
        private bool IsFilterActive()
        {
            // Verifica Dropdowns
            bool hasDropdowns = (SelectedLocation != null && SelectedLocation.Count > 0) ||
                                (SelectedPerson != null && SelectedPerson.Count > 0) ||
                                (SelectedBusinessUnit != null && SelectedBusinessUnit.Count > 0) ||
                                (SelectedOrigin != null && SelectedOrigin.Count > 0) ||
                                (SelectedDepartment != null && SelectedDepartment.Count > 0) ||
                                (SelectedCustomer != null && SelectedCustomer.Count > 0);

            // Verifica Tiles e Alertas
            bool hasSideTile = _lastAppliedSideTileFilter != null && !IsAllCaption(_lastAppliedSideTileFilter.Caption);
            bool hasAlert = !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption);

            return hasDropdowns || hasSideTile || hasAlert;
        }

        private async Task ToggleActionPlanExpandAsync(ActionPlanModernDto actionPlan)
        {
            if (actionPlan == null)
            {
                GeosApplication.Instance.Logger?.Log("[ToggleActionPlanExpandAsync] actionPlan is NULL", Category.Warn, Priority.Medium);
                return;
            }

            GeosApplication.Instance.Logger?.Log($"[ToggleActionPlanExpandAsync] {actionPlan.Code} - Current IsExpanded={actionPlan.IsExpanded}, TasksCount={actionPlan.TasksCount}, toggling...", Category.Info, Priority.Low);
            
            actionPlan.IsExpanded = !actionPlan.IsExpanded;
            
            GeosApplication.Instance.Logger?.Log($"[ToggleActionPlanExpandAsync] {actionPlan.Code} - New IsExpanded={actionPlan.IsExpanded}, TasksCount={actionPlan.TasksCount}", Category.Info, Priority.Low);

            if (actionPlan.IsExpanded)
            {
                GeosApplication.Instance.Logger?.Log($"[ToggleActionPlanExpandAsync] {actionPlan.Code} - Calling LoadTasksForActionPlanAsync...", Category.Info, Priority.Low);
                await LoadTasksForActionPlanAsync(actionPlan);
                GeosApplication.Instance.Logger?.Log($"[ToggleActionPlanExpandAsync] {actionPlan.Code} - LoadTasksForActionPlanAsync completed. Tasks.Count={actionPlan.Tasks?.Count ?? 0}", Category.Info, Priority.Low);
            }
            else
            {
                GeosApplication.Instance.Logger?.Log($"[ToggleActionPlanExpandAsync] {actionPlan.Code} - Collapsed (no action needed)", Category.Info, Priority.Low);
            }
        }

        /// <summary>
        /// Toggle expand/collapse para uma Task (mostrar Sub-Tasks)
        /// </summary>
        private async Task ToggleTaskExpandAsync(ActionPlanTaskModernDto task)
        {
            if (task == null) return;

            task.IsExpanded = !task.IsExpanded;

            if (task.IsExpanded)
            {
                await LoadSubTasksForTaskAsync(task);
            }
        }

        private ActionPlanTaskModernDto MapSubTaskToActionPlanTaskDto(Emdep.Geos.Data.Common.APM.APMActionPlanSubTask subTask)
        {
            if (subTask == null) return null;

            // CORREÇÃO DA COR: Usar System.Drawing.ColorTranslator
            System.Drawing.Color statusColor = System.Drawing.Color.Transparent;
            try
            {
                if (!string.IsNullOrEmpty(subTask.StatusHTMLColor))
                {
                    statusColor = System.Drawing.ColorTranslator.FromHtml(subTask.StatusHTMLColor);
                }
            }
            catch { /* Ignorar erro de cor inválida */ }

            return new ActionPlanTaskModernDto
            {
                IdTask = subTask.IdActionPlanTask,
                IdActionPlan = subTask.IdActionPlan,
                IdParent = subTask.IdParent,
                TaskNumber = subTask.TaskNumber,
                Code = subTask.Code ?? string.Empty,
                Title = subTask.Title ?? string.Empty,
                Description = subTask.Description ?? string.Empty,
                Responsible = subTask.Responsible ?? string.Empty,
                IdLookupStatus = subTask.IdLookupStatus,
                Status = subTask.Status ?? string.Empty,

                // Atribuição corrigida (System.Drawing.Color)
                StatusColor = statusColor,

                Priority = subTask.Priority ?? string.Empty,
                Theme = subTask.Theme ?? string.Empty,
                // Nota: ActionPlanTaskModernDto não tem ThemeHTMLColor - removido
                OpenDate = subTask.CreatedIn,
                DueDate = subTask.DueDate,
                DueDateDisplay = subTask.DueDate != DateTime.MinValue ? subTask.DueDate.ToString("dd/MM/yyyy") : "-",
                DueDays = subTask.DueDays,
                DueDaysColor = subTask.CardDueColor ?? string.Empty,
                Percentage = subTask.Progress, // Usa Percentage em vez de Progress
                CommentsCount = subTask.CommentsCount,
                Duration = subTask.Duration,
                // Nota: ActionPlanTaskModernDto não tem CloseDate - removido
                OriginalDueDate = subTask.OriginalDueDate,
                DelegatedTo = subTask.DelegatedTo ?? string.Empty,
                // Nota: ActionPlanTaskModernDto não tem Location - removido
                OriginWeek = subTask.OriginWeek ?? string.Empty,

                IsExpanded = false,
                IsLoadingSubTasks = false,

                // Inicialização correta
                SubTasks = new ObservableCollection<ActionPlanTaskModernDto>()
            };
        }

        public async Task LoadSubTasksForTaskAsync(ActionPlanTaskModernDto task)
        {
            if (task == null) return;

            // Se já tem sub-tasks carregadas, não recarregar
            if (task.SubTasks != null && task.SubTasks.Count > 0) return;

            try
            {
                task.IsLoadingSubTasks = true;

                var subTasksData = await Task.Run(() =>
                {
                    lock (_cacheLock)
                    {
                        if (_allDataCache != null)
                        {
                            foreach (var ap in _allDataCache)
                            {
                                if (ap.TaskList != null)
                                {
                                    var foundTask = ap.TaskList.FirstOrDefault(t => t.IdActionPlanTask == task.IdTask);

                                    // Acede à lista de dados brutos (APMActionPlanSubTask)
                                    if (foundTask?.SubTaskList != null)
                                    {
                                        return foundTask.SubTaskList;
                                    }
                                }
                            }
                        }
                    }
                    return new List<Emdep.Geos.Data.Common.APM.APMActionPlanSubTask>();
                });

                // CONVERSÃO CORRETA: Mapear para ActionPlanTaskModernDto
                var subTaskDtos = subTasksData
                                    .Select(MapSubTaskToActionPlanTaskDto)
                                    .Where(dto => dto != null)
                                    .ToList();

                // Atualizar na UI Thread
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // Aqui o tipo já bate certo: ObservableCollection<ActionPlanTaskModernDto>
                        task.SubTasks = new ObservableCollection<ActionPlanTaskModernDto>(subTaskDtos);
                    });
                }
                else
                {
                    task.SubTasks = new ObservableCollection<ActionPlanTaskModernDto>(subTaskDtos);
                }

                GeosApplication.Instance.Logger?.Log($"Loaded {subTaskDtos.Count} sub-tasks for Task {task.Code}", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error loading sub-tasks for Task {task.Code}: {ex.Message}", Category.Exception, Priority.High);
            }
            finally
            {
                task.IsLoadingSubTasks = false;
            }
        }

        /// <summary>
        /// Expandir todos os Action Plans
        /// </summary>
        private async Task ExpandAllActionPlansAsync()
        {
            if (ActionPlans == null) return;

            foreach (var actionPlan in ActionPlans)
            {
                if (!actionPlan.IsExpanded)
                {
                    actionPlan.IsExpanded = true;
                    await LoadTasksForActionPlanAsync(actionPlan);
                }
            }
        }

        /// <summary>
        /// Colapsar todos os Action Plans
        /// </summary>
        private void CollapseAllActionPlans()
        {
            if (ActionPlans == null) return;

            foreach (var actionPlan in ActionPlans)
            {
                actionPlan.IsExpanded = false;
                // Também colapsar todas as tasks
                if (actionPlan.Tasks != null)
                {
                    foreach (var task in actionPlan.Tasks)
                    {
                        task.IsExpanded = false;
                    }
                }
            }
        }

        private async Task LoadTasksForActionPlanAsync(long idActionPlan)
        {
            try
            {
                lock (_cacheLock)
                {
                    if (_tasksCache.ContainsKey(idActionPlan))
                    {
                        var cachedTasks = _tasksCache[idActionPlan];
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            SelectedActionPlanTasks.Clear();
                            foreach (var task in cachedTasks) SelectedActionPlanTasks.Add(task);
                        });
                        return;
                    }
                }

                // Validações essenciais
                if (GeosApplication.Instance?.ActiveUser == null)
                {
                    GeosApplication.Instance.Logger?.Log("LoadTasksForActionPlanAsync: ActiveUser is null", Category.Exception, Priority.High);
                    return;
                }

                if (SelectedActionPlan != null) SelectedActionPlan.IsLoadingTasks = true;

                var tasks = await Task.Run(() =>
                {
                    // Obter período do APMCommon (consistente com LoadActionPlansPageAsync)
                    string period;
                    if (APMCommon.Instance?.SelectedPeriod != null && APMCommon.Instance.SelectedPeriod.Count > 0)
                    {
                        var selectedYear = APMCommon.Instance.SelectedPeriod.Cast<long>().FirstOrDefault();
                        period = selectedYear.ToString();
                    }
                    else
                    {
                        period = DateTime.Now.Year.ToString();
                    }

                    int userId = GeosApplication.Instance.ActiveUser.IdUser;

                    // GetTaskListByIdActionPlan_V2680(Int64 IdActionPlan, string selectionPeriod, Int32 idUser)
                    return _apmService.GetTaskListByIdActionPlan_V2680(idActionPlan, period, userId);
                });

                // Proteção contra resultado null
                if (tasks == null)
                {
                    GeosApplication.Instance.Logger?.Log("GetTaskListByIdActionPlan_V2680 returned null", Category.Warn, Priority.Low);
                    tasks = new List<APMActionPlanTask>();
                }

                var taskDtos = tasks.Select(MapTaskToDto).Where(dto => dto != null).ToList();

                lock (_cacheLock)
                {
                    if (!_tasksCache.ContainsKey(idActionPlan)) _tasksCache[idActionPlan] = taskDtos;
                }

                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SelectedActionPlanTasks.Clear();
                        foreach (var task in taskDtos) SelectedActionPlanTasks.Add(task);
                    });
                }
                else
                {
                    SelectedActionPlanTasks.Clear();
                    foreach (var task in taskDtos) SelectedActionPlanTasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error loading tasks: {ex.Message}", Category.Exception, Priority.High);
            }
            finally
            {
                if (SelectedActionPlan != null) SelectedActionPlan.IsLoadingTasks = false;
            }
        }

        #endregion

        #region Search with Debounce

        private async Task DebounceSearchAsync()
        {
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _searchCancellationTokenSource.Token;

            try
            {
                await Task.Delay(500, cancellationToken); // Debounce: 500ms

                if (!cancellationToken.IsCancellationRequested)
                {
                    await RefreshDataAsync();
                }
            }
            catch (OperationCanceledException)
            {
                // Normal quando o utilizador continua a digitar
            }
        }

        #endregion

        #region Refresh & Cancel

        private async Task RefreshDataAsync()
        {
            try
            {
                IsBusy = true;

                // Limpar dados existentes
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ActionPlans.Clear();
                        SelectedActionPlanTasks.Clear();
                    });
                }
                else
                {
                    ActionPlans.Clear();
                    SelectedActionPlanTasks.Clear();
                }

                // Limpar TODOS os caches (incluindo data cache)
                lock (_cacheLock)
                {
                    _tasksCache.Clear();
                }
                _allDataCache = null; // Forçar reload do serviço

                // Reset paginação
                _currentPage = 0;
                _hasMoreData = true;

                // Recarregar com AUTO-LOAD
                await LoadAllPagesAsync();

                // Refresh filter lists after reload
                FillFilterLists();

                // Store unfiltered data for local filtering
                _allActionPlansUnfiltered = ActionPlans.ToList();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CancelCurrentLoad()
        {
            _loadCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource?.Cancel();
        }

        #endregion

        #region Mapping - Entity to DTO

        private ActionPlanModernDto MapToDto(APMActionPlanModern entity)
        {
            if (entity == null) return null;

            try
            {
                // Helper para cores
                System.Drawing.Color ConvertColor(string colorStr)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(colorStr)) return System.Drawing.Color.Transparent;
                        return System.Drawing.ColorTranslator.FromHtml(colorStr);
                    }
                    catch { return System.Drawing.Color.Transparent; }
                }

                var dto = new ActionPlanModernDto
                {
                    // --- IDs BÁSICOS ---
                    IdActionPlan = entity.IdActionPlan,
                    Code = entity.Code ?? string.Empty,
                    Title = entity.Description ?? string.Empty,

                    // --- RESPONSÁVEL ---
                    Responsible = !string.IsNullOrEmpty(entity.Responsible)
                                  ? entity.Responsible
                                  : (entity.ActionPlanResponsibleDisplayName ?? string.Empty),

                    EmployeeCode = entity.EmployeeCode ?? string.Empty,
                    IdGender = entity.IdGender,
                    IdEmployee = entity.IdEmployee,

                    // --- LOCALIZAÇÃO ---
                    Location = entity.Location ?? string.Empty,
                    IdLocation = entity.IdCompany,

                    // =================================================================
                    // CORREÇÃO CRÍTICA: MAPEAMENTO DOS IDs PARA FILTROS
                    // (Sem isto, os Dropdowns não funcionam!)
                    // =================================================================
                    IdLookupBusinessUnit = entity.IdLookupBusinessUnit, // <--- ADICIONAR ISTO
                    IdLookupOrigin = entity.IdLookupOrigin,             // <--- ADICIONAR ISTO
                    IdDepartment = entity.IdDepartment,                 // <--- Já tinhas, mas confirma

                    // --- AGREGADOS PARA FILTRO RÁPIDO ---
                    ThemeAggregates = entity.ThemeAggregates,
                    StatusAggregates = entity.StatusAggregates,

                    // --- ESTATÍSTICAS NOVAS ---
                    Stat_Overdue15 = entity.Stat_Overdue15,
                    Stat_HighPriorityOverdue = entity.Stat_HighPriorityOverdue,
                    Stat_MaxDueDays = entity.Stat_MaxDueDays,

                    // --- CONTAGENS ---
                    TasksCount = entity.TotalActionItems,
                    TotalActionItems = entity.TotalActionItems,
                    TotalOpenItems = entity.TotalOpenItems,
                    TotalClosedItems = entity.TotalClosedItems,
                    Percentage = entity.Percentage,

                    // --- COR ---
                    TotalClosedColor = ConvertColor(entity.TotalClosedColor),

                    // --- TEXTOS ---
                    // Action Plans não têm Status/Priority (são das tasks)
                    Status = string.Empty, 

                    BusinessUnit = entity.BusinessUnit ?? string.Empty,
                    BusinessUnitHTMLColor = entity.BusinessUnitHTMLColor,

                    Priority = string.Empty, 
                    Origin = entity.Origin ?? string.Empty,

                    IdSite = entity.IdSite,
                    Site = entity.Site ?? string.Empty,
                    GroupName = entity.GroupName ?? string.Empty,
                    Department = entity.Department ?? string.Empty,

                    

                    CountryIconUrl = entity.CountryIconUrl ?? string.Empty
                };

                // --- CARREGAMENTO DE TAREFAS (Lazy Loading) ---
                // Não inicializar Tasks como vazio! Deixar NULL para que LoadTasksForActionPlanAsync funcione
                // quando o utilizador clicar no botão de expandir (+)
                if (entity.TaskList != null && entity.TaskList.Count > 0)
                {
                    var taskDtos = entity.TaskList.Select(MapTaskToDto).Where(t => t != null).ToList();
                    dto.Tasks = new ObservableCollection<ActionPlanTaskModernDto>(taskDtos);
                }
                // Se TaskList estiver vazio/null, deixar dto.Tasks = null (não inicializar)
                // para permitir lazy-loading quando o utilizador expandir

                return dto;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"MapToDto Error: {ex.Message}", Category.Exception, Priority.High);
                return null;
            }
        }

        private ActionPlanTaskModernDto MapTaskToDto(APMActionPlanTask entity)
        {
            System.Drawing.Color ConvertColor(string colorStr)
            {
                try
                {
                    if (string.IsNullOrEmpty(colorStr)) return System.Drawing.Color.Transparent;
                    return System.Drawing.ColorTranslator.FromHtml(colorStr);
                }
                catch { return System.Drawing.Color.Transparent; }
            }

            return new ActionPlanTaskModernDto
            {
                // APMActionPlanTask usa IdActionPlanTask (não IdTask)
                IdTask = entity.IdActionPlanTask,
                IdLookupStatus = entity.IdLookupStatus,

                IdActionPlan = entity.IdActionPlan,
                TaskNumber = entity.TaskNumber,
                Code = entity.Code ?? string.Empty,
                Title = entity.Title ?? string.Empty,
                Description = entity.Description ?? string.Empty,

                // APMActionPlanTask já tem Responsible (não ResponsibleName)
                Responsible = entity.Responsible ?? string.Empty,

                Status = entity.Status ?? string.Empty,
                Priority = entity.Priority ?? string.Empty,
                Theme = entity.Theme ?? string.Empty,

                // Datas
                OpenDate = entity.OpenDate,
                OpenDateDisplay = entity.OpenDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                OriginalDueDate = entity.OriginalDueDate,
                OriginalDueDateDisplay = entity.OriginalDueDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                DueDate = entity.DueDate,
                DueDateDisplay = entity.DueDate.ToString("dd/MM/yyyy"),

                // Duration e Due Days
                Duration = entity.Duration,
                DueDays = entity.DueDays,
                DueDaysColor = entity.DueColor ?? string.Empty,

                // Last Updated e Change Count
                LastUpdated = entity.LastUpdated,
                LastUpdatedDisplay = entity.LastUpdated?.ToString("dd/MM/yyyy") ?? string.Empty,
                ChangeCount = entity.ChangeCount,

                // Delegated To e OT Item
                DelegatedTo = entity.DelegatedTo ?? string.Empty,
                OTItem = entity.NumItem ?? string.Empty,
                OriginWeek = entity.OriginWeek ?? string.Empty,

                // Counts
                CommentsCount = entity.CommentsCount,
                FileCount = entity.FileCount,
                ParticipantCount = entity.ParticipantCount,

                // APMActionPlanTask usa Progress (não Percentage)
                Percentage = entity.Progress,

                // SubTasks não têm propriedades diretas - calcular de SubTaskList
                TotalSubTasks = entity.SubTaskList?.Count ?? 0,
                CompletedSubTasks = entity.SubTaskList?.Count(s => s.IdLookupStatus == 3) ?? 0,

                // APMActionPlanTask usa StatusHTMLColor (não StatusColor)
                StatusColor = ConvertColor(entity.StatusHTMLColor)

            };
        }

        private SubTaskModernDto MapSubTaskToDto(APMActionPlanSubTask entity)
        {
            if (entity == null) return null;

            System.Drawing.Color ConvertColor(string colorStr)
            {
                try
                {
                    if (string.IsNullOrEmpty(colorStr)) return System.Drawing.Color.Transparent;
                    return System.Drawing.ColorTranslator.FromHtml(colorStr);
                }
                catch { return System.Drawing.Color.Transparent; }
            }

            return new SubTaskModernDto
            {
                IdSubTask = entity.IdActionPlanTask,
                IdTask = entity.IdParent,
                IdActionPlan = entity.IdActionPlan,
                Code = entity.Code ?? string.Empty,
                Title = entity.Title ?? string.Empty,
                Description = entity.Description ?? string.Empty,
                Responsible = entity.Responsible ?? string.Empty,
                Status = entity.Status ?? string.Empty,
                Priority = entity.Priority ?? string.Empty,
                DueDate = entity.DueDate,
                DueDateDisplay = entity.DueDate.ToString("dd/MM/yyyy"),
                Percentage = entity.Progress,
                StatusColor = ConvertColor(entity.StatusHTMLColor)
            };
        }

        #endregion

        #region Open Task/SubTask Methods

        /// <summary>
        /// Extrai a Task dos EventArgs e chama OpenTaskAction
        /// </summary>
        private void OpenTaskFromEventArgs(object args)
        {
            try
            {
                if (args is DevExpress.Xpf.Grid.RowDoubleClickEventArgs rowArgs)
                {
                    var taskDto = rowArgs.Source?.DataContext as ActionPlanTaskModernDto;
                    if (taskDto == null && rowArgs.Source is FrameworkElement fe)
                    {
                        taskDto = fe.DataContext as ActionPlanTaskModernDto;
                    }

                    // Tentar obter do RowData
                    if (taskDto == null)
                    {
                        var view = rowArgs.Source as TableView;
                        if (view?.Grid?.SelectedItem is ActionPlanTaskModernDto selected)
                        {
                            taskDto = selected;
                        }
                    }

                    if (taskDto != null)
                    {
                        OpenTaskAction(taskDto);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error extracting task from event args: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Extrai a SubTask dos EventArgs e chama OpenSubTaskAction
        /// </summary>
        private void OpenSubTaskFromEventArgs(object args)
        {
            try
            {
                if (args is DevExpress.Xpf.Grid.RowDoubleClickEventArgs rowArgs)
                {
                    var subTaskDto = rowArgs.Source?.DataContext as SubTaskModernDto;
                    if (subTaskDto == null && rowArgs.Source is FrameworkElement fe)
                    {
                        subTaskDto = fe.DataContext as SubTaskModernDto;
                    }

                    // Tentar obter do RowData
                    if (subTaskDto == null)
                    {
                        var view = rowArgs.Source as TableView;
                        if (view?.Grid?.SelectedItem is SubTaskModernDto selected)
                        {
                            subTaskDto = selected;
                        }
                    }

                    if (subTaskDto != null)
                    {
                        OpenSubTaskAction(subTaskDto);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error extracting subtask from event args: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Abre a janela de edição de Task
        /// </summary>
        private void OpenTaskAction(ActionPlanTaskModernDto taskDto)
        {
            try
            {
                if (taskDto == null) return;

                GeosApplication.Instance.Logger?.Log("Opening Task for edit...", Category.Info, Priority.Low);

                // Buscar a task original do cache
                APMActionPlanTask originalTask = null;
                if (_allDataCache != null)
                {
                    var actionPlan = _allDataCache.FirstOrDefault(ap => ap.IdActionPlan == taskDto.IdActionPlan);
                    if (actionPlan?.TaskList != null)
                    {
                        originalTask = actionPlan.TaskList.FirstOrDefault(t => t.IdActionPlanTask == taskDto.IdTask);
                    }
                }

                if (originalTask == null)
                {
                    // Se não encontrou no cache, buscar do serviço
                    originalTask = new APMActionPlanTask
                    {
                        IdActionPlanTask = taskDto.IdTask,
                        IdActionPlan = taskDto.IdActionPlan,
                        TaskNumber = taskDto.TaskNumber,
                        Code = taskDto.Code,
                        Title = taskDto.Title,
                        Description = taskDto.Description,
                        Responsible = taskDto.Responsible,
                        Status = taskDto.Status,
                        Priority = taskDto.Priority,
                        Theme = taskDto.Theme,
                        OpenDate = taskDto.OpenDate,
                        OriginalDueDate = taskDto.OriginalDueDate,
                        DueDate = taskDto.DueDate ?? DateTime.Now,
                        Duration = taskDto.Duration,
                        DueDays = taskDto.DueDays,
                        LastUpdated = taskDto.LastUpdated,
                        ChangeCount = taskDto.ChangeCount,
                        DelegatedTo = taskDto.DelegatedTo,
                        OriginWeek = taskDto.OriginWeek,
                        CommentsCount = taskDto.CommentsCount,
                        FileCount = taskDto.FileCount,
                        ParticipantCount = taskDto.ParticipantCount,
                        Progress = (int)taskDto.Percentage
                    };
                }

                // Criar e configurar a view de edição
                var addEditTaskView = new AddEditTaskView();
                var addEditTaskViewModel = new AddEditTaskViewModel();

                EventHandler closeHandler = delegate { addEditTaskView.Close(); };
                addEditTaskViewModel.RequestClose += closeHandler;
                addEditTaskViewModel.IsNew = false;
                addEditTaskViewModel.WindowHeader = Application.Current.FindResource("EditActionPlansHeader")?.ToString() ?? "Edit Task";

                // Configurar lista de Action Plans
                if (_allDataCache != null)
                {
                   // APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(_allDataCache.OrderBy(ap => ap.Code));
                }

                // Encontrar o Action Plan responsável
                var parentActionPlan = _allDataCache?.FirstOrDefault(ap => ap.IdActionPlan == taskDto.IdActionPlan);
                if (parentActionPlan != null)
                {
                    originalTask.IdActionPlanResponsible = parentActionPlan.IdEmployee;
                }

                addEditTaskViewModel.OtItemVisibility = Visibility.Collapsed;
                addEditTaskViewModel.EditInit(originalTask);
                addEditTaskView.DataContext = addEditTaskViewModel;
                addEditTaskView.ShowDialog();

                // Atualizar a grid se foi guardado
                if (addEditTaskViewModel.IsSave)
                {
                    UpdateTaskInGrid(taskDto, addEditTaskViewModel.UpdatedTask);
                }

                GeosApplication.Instance.Logger?.Log("Task edit completed", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error opening task: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Abre a janela de edição de SubTask
        /// </summary>
        private void OpenSubTaskAction(SubTaskModernDto subTaskDto)
        {
            try
            {
                if (subTaskDto == null) return;

                GeosApplication.Instance.Logger?.Log("Opening SubTask for edit...", Category.Info, Priority.Low);

                // Buscar a subtask original do cache
                APMActionPlanSubTask originalSubTask = null;
                if (_allDataCache != null)
                {
                    foreach (var actionPlan in _allDataCache)
                    {
                        if (actionPlan.TaskList == null) continue;
                        foreach (var task in actionPlan.TaskList)
                        {
                            if (task.SubTaskList == null) continue;
                            originalSubTask = task.SubTaskList.FirstOrDefault(st => st.IdActionPlanTask == subTaskDto.IdSubTask);
                            if (originalSubTask != null) break;
                        }
                        if (originalSubTask != null) break;
                    }
                }

                if (originalSubTask == null)
                {
                    // Criar objeto básico se não encontrou
                    originalSubTask = new APMActionPlanSubTask
                    {
                        IdActionPlanTask = subTaskDto.IdSubTask,
                        IdActionPlan = subTaskDto.IdActionPlan,
                        Code = subTaskDto.Code,
                        Title = subTaskDto.Title,
                        Description = subTaskDto.Description,
                        Responsible = subTaskDto.Responsible,
                        Status = subTaskDto.Status,
                        Priority = subTaskDto.Priority,
                        DueDate = subTaskDto.DueDate ?? DateTime.Now,
                        Progress = (int)subTaskDto.Percentage
                    };
                }

                // Criar e configurar a view de edição de SubTask
                var addEditSubTaskView = new AddEditSubTaskView();
                var addEditSubTaskViewModel = new AddEditSubTaskViewModel();

                EventHandler closeHandler = delegate { addEditSubTaskView.Close(); };
                addEditSubTaskViewModel.RequestClose += closeHandler;
                addEditSubTaskViewModel.IsNew = false;
                addEditSubTaskViewModel.WindowHeader = Application.Current.FindResource("EditSubTaskHeader")?.ToString() ?? "Edit Sub-Task";

                addEditSubTaskViewModel.EditInit(originalSubTask);
                addEditSubTaskView.DataContext = addEditSubTaskViewModel;
                addEditSubTaskView.ShowDialog();

                // Atualizar a grid se foi guardado
                if (addEditSubTaskViewModel.IsSave)
                {
                    UpdateSubTaskInGrid(subTaskDto, addEditSubTaskViewModel.UpdatedSubTask);
                }

                GeosApplication.Instance.Logger?.Log("SubTask edit completed", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error opening subtask: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Atualiza a task na grid após edição
        /// </summary>
        private void UpdateTaskInGrid(ActionPlanTaskModernDto taskDto, APMActionPlanTask updatedTask)
        {
            if (updatedTask == null || taskDto == null) return;

            try
            {
                // Atualizar o DTO
                taskDto.Title = updatedTask.Title;
                taskDto.Description = updatedTask.Description;
                taskDto.Responsible = updatedTask.Responsible;
                taskDto.Status = updatedTask.Status;
                taskDto.Priority = updatedTask.Priority;
                taskDto.Theme = updatedTask.Theme;
                taskDto.OpenDate = updatedTask.OpenDate;
                taskDto.OpenDateDisplay = updatedTask.OpenDate?.ToString("dd/MM/yyyy") ?? string.Empty;
                taskDto.OriginalDueDate = updatedTask.OriginalDueDate;
                taskDto.OriginalDueDateDisplay = updatedTask.OriginalDueDate?.ToString("dd/MM/yyyy") ?? string.Empty;
                taskDto.DueDate = updatedTask.DueDate;
                taskDto.DueDateDisplay = updatedTask.DueDate.ToString("dd/MM/yyyy");
                taskDto.Duration = updatedTask.Duration;
                taskDto.DueDays = updatedTask.DueDays;
                taskDto.DueDaysColor = updatedTask.DueColor ?? string.Empty;
                taskDto.LastUpdated = updatedTask.LastUpdated;
                taskDto.LastUpdatedDisplay = updatedTask.LastUpdated?.ToString("dd/MM/yyyy") ?? string.Empty;
                taskDto.ChangeCount = updatedTask.ChangeCount;
                taskDto.DelegatedTo = updatedTask.DelegatedTo;
                taskDto.Percentage = updatedTask.Progress;

                // Atualizar no cache também
                if (_allDataCache != null)
                {
                    var actionPlan = _allDataCache.FirstOrDefault(ap => ap.IdActionPlan == taskDto.IdActionPlan);
                    if (actionPlan?.TaskList != null)
                    {
                        var index = actionPlan.TaskList.FindIndex(t => t.IdActionPlanTask == taskDto.IdTask);
                        if (index >= 0)
                        {
                            actionPlan.TaskList[index] = updatedTask;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error updating task in grid: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Atualiza a subtask na grid após edição
        /// </summary>
        private void UpdateSubTaskInGrid(SubTaskModernDto subTaskDto, APMActionPlanSubTask updatedSubTask)
        {
            if (updatedSubTask == null || subTaskDto == null) return;

            try
            {
                // Atualizar o DTO
                subTaskDto.Title = updatedSubTask.Title;
                subTaskDto.Description = updatedSubTask.Description;
                subTaskDto.Responsible = updatedSubTask.Responsible;
                subTaskDto.Status = updatedSubTask.Status;
                subTaskDto.Priority = updatedSubTask.Priority;
                subTaskDto.DueDate = updatedSubTask.DueDate;
                subTaskDto.DueDateDisplay = updatedSubTask.DueDate.ToString("dd/MM/yyyy");
                subTaskDto.Percentage = updatedSubTask.Progress;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error updating subtask in grid: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        #endregion

        #region Toolbar Action Methods

        /// <summary>
        /// Add new action plan
        /// </summary>
        private void AddNewActionPlan()
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("AddNewActionPlan called", Category.Info, Priority.Low);

                // TODO: Open the Add Action Plan window
                // var window = new ActionPlanEditWindow();
                // window.ShowDialog();

                MessageBox.Show("Add New Action Plan functionality - to be implemented", "Add Action Plan", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in AddNewActionPlan: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Import action plan
        /// </summary>
        private void ImportActionPlan()
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("ImportActionPlan called", Category.Info, Priority.Low);

                // TODO: Implement import functionality
                MessageBox.Show("Import Action Plan functionality - to be implemented", "Import", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in ImportActionPlan: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Print button action
        /// </summary>
        private void PrintButtonAction(object parameter)
        {
            try
            {
                string printType = parameter?.ToString() ?? "Standard";
                GeosApplication.Instance.Logger?.Log($"PrintButtonAction called with type: {printType}", Category.Info, Priority.Low);

                // TODO: Implement print functionality based on type (Standard/Customer)
                MessageBox.Show($"Print {printType} functionality - to be implemented", "Print", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in PrintButtonAction: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Export standard action
        /// </summary>
        private void ExportStandardAction()
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("ExportStandardAction called", Category.Info, Priority.Low);

                // TODO: Implement standard export functionality
                MessageBox.Show("Export Standard functionality - to be implemented", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in ExportStandardAction: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Export customer action
        /// </summary>
        private void ExportCustomerAction()
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("ExportCustomerAction called", Category.Info, Priority.Low);

                // TODO: Implement customer export functionality
                MessageBox.Show("Export Customer functionality - to be implemented", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in ExportCustomerAction: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Year changed action - reload data with new period
        /// </summary>
        private async void SelectedYearChangedAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("SelectedYearChangedAction called", Category.Info, Priority.Low);

                // Refresh data with new period selection
                await RefreshDataAsync();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in SelectedYearChangedAction: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Switch to Grid View
        /// </summary>
        private void SwitchToGridView()
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("SwitchToGridView called", Category.Info, Priority.Low);
                // TODO: Implement grid view switching
                MessageBox.Show("Switch to Grid View - to be implemented", "Switch View", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in SwitchToGridView: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Switch to Task View
        /// </summary>
        private void SwitchToTaskView()
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("SwitchToTaskView called", Category.Info, Priority.Low);
                // TODO: Implement task view switching
                MessageBox.Show("Switch to Task View - to be implemented", "Switch View", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in SwitchToTaskView: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Switch to Task Cards View
        /// </summary>
        private void SwitchToTaskCards()
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("SwitchToTaskCards called", Category.Info, Priority.Low);
                // TODO: Implement task cards view switching
                MessageBox.Show("Switch to Task Cards View - to be implemented", "Switch View", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in SwitchToTaskCards: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        #endregion

        #region Filter Methods

        /// <summary>
        /// Initializes the filter dropdown lists (Location, BusinessUnit, Origin, Department) from service
        /// </summary>
        private void InitializeFilterDropdowns()
        {
            try
            {
                // Fill Location list
                if (APMCommon.Instance.LocationList == null || APMCommon.Instance.LocationList.Count == 0)
                {
                    var locations = _apmService.GetAuthorizedLocationListByIdUser_V2570(GeosApplication.Instance.ActiveUser.IdUser);
                    APMCommon.Instance.LocationList = new List<Company>(locations);
                    GeosApplication.Instance.Logger?.Log($"LocationList initialized with {APMCommon.Instance.LocationList.Count} items", Category.Info, Priority.Low);
                }

                // Fill Business Unit list (lookupTypeId = 2)
                if (APMCommon.Instance.BusinessUnitList == null || APMCommon.Instance.BusinessUnitList.Count == 0)
                {
                    var businessUnits = _apmService.GetLookupValues_V2550(2);
                    APMCommon.Instance.BusinessUnitList = new List<LookupValue>(businessUnits);
                    GeosApplication.Instance.Logger?.Log($"BusinessUnitList initialized with {APMCommon.Instance.BusinessUnitList.Count} items", Category.Info, Priority.Low);
                }

                // Fill Origin list (lookupTypeId = 154)
                if (APMCommon.Instance.OriginList == null || APMCommon.Instance.OriginList.Count == 0)
                {
                    var origins = _apmService.GetLookupValues_V2550(154);
                    APMCommon.Instance.OriginList = new List<LookupValue>(origins);
                    GeosApplication.Instance.Logger?.Log($"OriginList initialized with {APMCommon.Instance.OriginList.Count} items", Category.Info, Priority.Low);
                }

                // Fill Department list
                if (APMCommon.Instance.DepartmentList == null || APMCommon.Instance.DepartmentList.Count == 0)
                {
                    var departments = _apmService.GetDepartmentsForActionPlan_V2590();
                    APMCommon.Instance.DepartmentList = new List<Department>(departments);
                    GeosApplication.Instance.Logger?.Log($"DepartmentList initialized with {APMCommon.Instance.DepartmentList.Count} items", Category.Info, Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in InitializeFilterDropdowns: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Fills the filter lists (Responsible, Customer) based on loaded Action Plans
        /// </summary>
        private void FillFilterLists()
        {
            try
            {
                if (ActionPlans == null || ActionPlans.Count == 0)
                {
                    ListOfPerson = new ObservableCollection<Responsible>();
                    ListOfCustomer = new ObservableCollection<APMCustomer>();
                    return;
                }

                // Build Responsible list from Action Plans, Tasks, and SubTasks
                var responsibleList = new List<Responsible>();
                var seenResponsible = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Build Customer list from Action Plans (using IdSite like Old UI)
                var customerList = new List<APMCustomer>();
                var seenCustomer = new HashSet<int>();

                foreach (var ap in ActionPlans)
                {
                    if (ap == null) continue;

                    // Add responsible from Action Plan (with EmployeeCode and IdGender for image loading)
                    if (!string.IsNullOrEmpty(ap.EmployeeCode))
                    {
                        string key = ap.EmployeeCode + "_" + ap.IdGender;
                        if (seenResponsible.Add(key))
                        {
                            // Check if image is in cache
                            ImageSource cachedImg = null;
                            if (_inMemoryImageCache.ContainsKey(ap.EmployeeCode))
                            {
                                cachedImg = _inMemoryImageCache[ap.EmployeeCode];
                            }

                            responsibleList.Add(new Responsible
                            {
                                IdEmployee = (UInt32)ap.IdEmployee,
                                EmployeeCode = ap.EmployeeCode,
                                IdGender = ap.IdGender,
                                FullName = ap.Responsible,
                                EmployeeCodeWithIdGender = key,
                                ResponsibleDisplayName = ap.Responsible,
                                OwnerImage = cachedImg
                            });
                        }
                    }
                    else if (!string.IsNullOrEmpty(ap.Responsible))
                    {
                        // Fallback: use Responsible name as key if no EmployeeCode
                        string key = ap.Responsible.Trim().ToLower();
                        if (seenResponsible.Add(key))
                        {
                            responsibleList.Add(new Responsible
                            {
                                FullName = ap.Responsible,
                                ResponsibleDisplayName = ap.Responsible
                            });
                        }
                    }

                    // Add customer from Action Plan (using IdSite like Old UI)
                    if (ap.IdSite > 0)
                    {
                        if (seenCustomer.Add(ap.IdSite))
                        {
                            customerList.Add(new APMCustomer
                            {
                                IdSite = ap.IdSite,
                                Site = ap.Site,
                                GroupName = ap.GroupName
                            });
                        }
                    }
                    else
                    {
                        // Add "Blanks" entry for items without Site
                        if (seenCustomer.Add(0))
                        {
                            customerList.Insert(0, new APMCustomer
                            {
                                IdSite = 0,
                                Site = ap.Site,
                                GroupName = "Blanks"
                            });
                        }
                    }

                    // Process Tasks
                    if (ap.Tasks != null)
                    {
                        foreach (var task in ap.Tasks)
                        {
                            if (task == null) continue;

                            // Add responsible from Task
                            if (!string.IsNullOrEmpty(task.Responsible))
                            {
                                string taskKey = task.Responsible.Trim().ToLower();
                                if (seenResponsible.Add(taskKey))
                                {
                                    responsibleList.Add(new Responsible
                                    {
                                        FullName = task.Responsible,
                                        ResponsibleDisplayName = task.Responsible
                                    });
                                }
                            }

                            // Process SubTasks
                            if (task.SubTasks != null)
                            {
                                foreach (var sub in task.SubTasks)
                                {
                                    if (sub == null) continue;

                                    // Add responsible from SubTask
                                    if (!string.IsNullOrEmpty(sub.Responsible))
                                    {
                                        string subKey = sub.Responsible.Trim().ToLower();
                                        if (seenResponsible.Add(subKey))
                                        {
                                            responsibleList.Add(new Responsible
                                            {
                                                FullName = sub.Responsible,
                                                ResponsibleDisplayName = sub.Responsible
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Sort and assign
                ListOfPerson = new ObservableCollection<Responsible>(
                    responsibleList.OrderBy(r => r.ResponsibleDisplayName, StringComparer.OrdinalIgnoreCase));

                ListOfCustomer = new ObservableCollection<APMCustomer>(
                    customerList.OrderBy(c => c.GroupName, StringComparer.OrdinalIgnoreCase));

                GeosApplication.Instance.Logger?.Log(
                    $"Filter lists populated: {ListOfPerson.Count} responsibles, {ListOfCustomer.Count} customers",
                    Category.Info, Priority.Low);

                // Load responsible images in background
                _ = LoadResponsibleImagesBackgroundAsync(ListOfPerson);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in FillFilterLists: {ex.Message}", Category.Exception, Priority.High);
                ListOfPerson = new ObservableCollection<Responsible>();
                ListOfCustomer = new ObservableCollection<APMCustomer>();
            }
        }

        /// <summary>
        /// Loads responsible images in background using cache
        /// </summary>
        private async Task LoadResponsibleImagesBackgroundAsync(IEnumerable<Responsible> people)
        {
            if (people == null) return;

            var peopleList = people.ToList();
            var tasks = new List<Task>();

            string baseUrlRounded = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/Rounded/";
            string baseUrlNormal = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/";

            string currentTheme = "DeepBlue";
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                currentTheme = ApplicationThemeHelper.ApplicationThemeName;
            });

            foreach (var person in peopleList)
            {
                // Skip if already has image from cache
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
                    try
                    {
                        if (string.IsNullOrEmpty(person.EmployeeCode)) return;
                        if (person.OwnerImage != null) return;

                        string url1 = $"{baseUrlRounded}{person.EmployeeCode}.png";
                        string url2 = $"{baseUrlNormal}{person.EmployeeCode}.png";

                        var img = await ImageCacheService.Instance.GetImageAsync(person.EmployeeCode, url1, url2);

                        if (img == null)
                        {
                            img = GetLocalFallbackImage(person.IdGender, currentTheme);
                        }

                        if (img != null)
                        {
                            Application.Current?.Dispatcher?.BeginInvoke(new Action(() =>
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
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger?.Log($"Error loading image for {person.EmployeeCode}: {ex.Message}", Category.Warn, Priority.Low);
                    }
                });
                tasks.Add(t);
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Gets fallback image based on gender and theme
        /// </summary>
        private ImageSource GetLocalFallbackImage(int idGender, string themeName)
        {
            try
            {
                string imagePath;
                bool isBlackAndBlue = string.Equals(themeName, "BlackAndBlue", StringComparison.OrdinalIgnoreCase);
                bool isFemale = (idGender == 1);

                if (isBlackAndBlue)
                {
                    imagePath = isFemale
                        ? "/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_White.png"
                        : "/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_White.png";
                }
                else
                {
                    imagePath = isFemale
                        ? "/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_Blue.png"
                        : "/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_Blue.png";
                }

                return new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Applies filters locally to the unfiltered data - INSTANT filtering without server call
        /// </summary>
        private void ApplyFilters()
        {
            try
            {
                if (_allActionPlansUnfiltered == null || !_allActionPlansUnfiltered.Any())
                {
                    if (ActionPlans == null) ActionPlans = new ObservableCollection<ActionPlanModernDto>();
                    return;
                }

                IEnumerable<ActionPlanModernDto> query = _allActionPlansUnfiltered;

                // 1. LOCALIZAÇÃO (Cast para Company)
                if (SelectedLocation != null && SelectedLocation.Count > 0)
                {
                    // Usamos Company direto (assumindo que o using está lá)
                    var ids = SelectedLocation.Cast<Company>()
                                              .Select(x => x.IdCompany).ToList();
                    query = query.Where(x => ids.Contains(x.IdLocation));
                }

                // 2. RESPONSÁVEL
                if (SelectedPerson != null && SelectedPerson.Count > 0)
                {
                    // Cast para Responsible
                    var names = SelectedPerson.Cast<Emdep.Geos.Data.Common.APM.Responsible>()
                                              .Select(x => x.FullName).ToList();
                    query = query.Where(x => names.Contains(x.Responsible));
                }

                // 3. BUSINESS UNIT (Correção do Erro CS0234 e CS1503)
                if (SelectedBusinessUnit != null && SelectedBusinessUnit.Count > 0)
                {
                    // REMOVI O NAMESPACE LONGO. Usa apenas 'LookupValue'.
                    // ADICIONEI '(int)' para garantir que comparamos int com int.
                    var ids = SelectedBusinessUnit.Cast<LookupValue>()
                                                  .Select(x => (int)x.IdLookupValue).ToList();

                    query = query.Where(x => ids.Contains(x.IdLookupBusinessUnit));
                }

                // 4. ORIGEM (Correção do Erro)
                if (SelectedOrigin != null && SelectedOrigin.Count > 0)
                {
                    // REMOVI O NAMESPACE LONGO.
                    var ids = SelectedOrigin.Cast<LookupValue>()
                                            .Select(x => (int)x.IdLookupValue).ToList();

                    query = query.Where(x => ids.Contains(x.IdLookupOrigin));
                }

                // 5. DEPARTAMENTO
                if (SelectedDepartment != null && SelectedDepartment.Count > 0)
                {
                    // Força a conversão para int, pois IdDepartment as vezes é long ou short
                    var ids = SelectedDepartment.Cast<Emdep.Geos.Data.Common.Hrm.Department>()
                                                .Select(x => (int)x.IdDepartment).ToList();
                    query = query.Where(x => ids.Contains(x.IdDepartment));
                }

                // 7. SIDE TILES (Temas)
                if (_lastAppliedSideTileFilter != null && !IsAllCaption(_lastAppliedSideTileFilter.Caption))
                {
                    string temaAlvo = _lastAppliedSideTileFilter.Caption;
                    query = query.Where(ap => !string.IsNullOrEmpty(ap.ThemeAggregates) &&
                                              ap.ThemeAggregates.IndexOf(temaAlvo, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                // 8. ALERTAS
                if (!string.IsNullOrWhiteSpace(_lastAppliedAlertCaption))
                {
                    string alert = _lastAppliedAlertCaption;
                    if (alert.Contains("Overdue")) query = query.Where(ap => ap.Stat_Overdue15 > 0);
                    else if (alert.Contains("High Priority")) query = query.Where(ap => ap.Stat_HighPriorityOverdue > 0);
                    else if (alert.Contains("Open")) query = query.Where(ap => ap.TotalOpenItems > 0);
                    else if (alert.Contains("Done") || alert.Contains("Closed")) query = query.Where(ap => ap.TotalOpenItems == 0);
                }

                // Atualizar Grid
                ActionPlans = new ObservableCollection<ActionPlanModernDto>(query.ToList());
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error ApplyFilters: {ex.Message}", Prism.Logging.Category.Exception, Prism.Logging.Priority.High);
            }
        }


        /// <summary>
        /// Verifica se um item (Action Plan, Task ou SubTask) corresponde aos filtros ativos
        /// </summary>
        private bool CheckItemMatchesFilters(
            long? idLocation, string responsible, string businessUnit, string origin, long? idDepartment, long? idSite,
            List<Company> locations, List<Responsible> responsibles, List<LookupValue> businessUnits,
            List<LookupValue> origins, List<Department> departments, List<APMCustomer> customers,
            bool hasLocationFilter, bool hasResponsibleFilter, bool hasBusinessUnitFilter,
            bool hasOriginFilter, bool hasDepartmentFilter, bool hasCustomerFilter)
        {
            bool matches = true;

            if (hasLocationFilter && locations != null && locations.Any())
            {
                matches = matches && locations.Any(loc => idLocation == loc.IdCompany);
            }

            if (hasResponsibleFilter && responsibles != null && responsibles.Any())
            {
                matches = matches && !string.IsNullOrEmpty(responsible) &&
                           responsibles.Any(r => r.FullName.Equals(responsible, StringComparison.OrdinalIgnoreCase));
            }

            if (hasBusinessUnitFilter && businessUnits != null && businessUnits.Any())
            {
                matches = matches && !string.IsNullOrEmpty(businessUnit) &&
                           businessUnits.Any(bu => bu.Value.Equals(businessUnit, StringComparison.OrdinalIgnoreCase));
            }

            if (hasOriginFilter && origins != null && origins.Any())
            {
                matches = matches && !string.IsNullOrEmpty(origin) &&
                           origins.Any(o => o.Value.Equals(origin, StringComparison.OrdinalIgnoreCase));
            }

            if (hasDepartmentFilter && departments != null && departments.Any())
            {
                matches = matches && departments.Any(dept => idDepartment == dept.IdDepartment);
            }

            if (hasCustomerFilter && customers != null && customers.Any())
            {
                matches = matches && customers.Any(c => idSite == c.IdSite);
            }

            return matches;
        }

        /// <summary>
        /// Location filter changed
        /// </summary>
        private void OnLocationFilterChanged(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("Location filter changed", Category.Info, Priority.Low);
                ApplyFilters();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in OnLocationFilterChanged: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Responsible filter changed
        /// </summary>
        private void OnResponsibleFilterChanged(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("Responsible filter changed", Category.Info, Priority.Low);
                ApplyFilters();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in OnResponsibleFilterChanged: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Business Unit filter changed
        /// </summary>
        private void OnBusinessUnitFilterChanged(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("Business Unit filter changed", Category.Info, Priority.Low);
                ApplyFilters();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in OnBusinessUnitFilterChanged: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Origin filter changed
        /// </summary>
        private void OnOriginFilterChanged(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("Origin filter changed", Category.Info, Priority.Low);
                ApplyFilters();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in OnOriginFilterChanged: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Department filter changed
        /// </summary>
        private void OnDepartmentFilterChanged(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("Department filter changed", Category.Info, Priority.Low);
                ApplyFilters();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in OnDepartmentFilterChanged: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Customer filter changed
        /// </summary>
        private void OnCustomerFilterChanged(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("Customer filter changed", Category.Info, Priority.Low);
                ApplyFilters();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in OnCustomerFilterChanged: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        /// <summary>
        /// Clear all filters
        /// </summary>
        private void ClearAllFilters()
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("Clearing all filters", Category.Info, Priority.Low);

                SelectedLocation = new List<object>();
                SelectedPerson = new List<object>();
                SelectedBusinessUnit = new List<object>();
                SelectedOrigin = new List<object>();
                SelectedDepartment = new List<object>();
                SelectedCustomer = new List<object>();

                ApplyFilters();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in ClearAllFilters: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        #endregion

        #region Alert Buttons

        /// <summary>
        /// Popula os Alert Buttons inicialmente
        /// </summary>
        private void PopulateAlertButtons()
        {
            try
            {
                if (_allDataCache == null || !_allDataCache.Any()) return;

                Dictionary<string, int> ParseAggregates(string aggregates, char expectedPrefix)
                {
                    var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    if (string.IsNullOrWhiteSpace(aggregates)) return result;

                    foreach (var rawPart in aggregates.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var part = rawPart?.Trim();
                        if (string.IsNullOrEmpty(part) || part.Length < 3) continue;
                        if (part[1] != '|') continue;
                        if (expectedPrefix != '\0' && part[0] != expectedPrefix) continue;

                        var payload = part.Substring(2);
                        var idx = payload.LastIndexOf(':');
                        if (idx <= 0 || idx >= payload.Length - 1) continue;

                        var key = payload.Substring(0, idx).Trim();
                        var valueStr = payload.Substring(idx + 1).Trim();
                        if (string.IsNullOrEmpty(key)) continue;
                        if (!int.TryParse(valueStr, out var value)) continue;

                        if (result.TryGetValue(key, out var existing))
                            result[key] = existing + value;
                        else
                            result[key] = value;
                    }

                    return result;
                }

                int GetCount(Dictionary<string, int> dict, string key)
                {
                    if (dict == null || string.IsNullOrWhiteSpace(key)) return 0;
                    return dict.TryGetValue(key, out var v) ? v : 0;
                }

                int longestOverdueDays = _allDataCache.Max(p => p.Stat_MaxDueDays);

                //// 2. Overdue >= 15 (Soma dos contadores de cada plano)
                int overdue15Count = _allDataCache.Sum(p => p.Stat_Overdue15);

                //// 3. High Priority (Soma dos contadores de cada plano)
                int highPriorityCount = _allDataCache.Sum(p => p.Stat_HighPriorityOverdue);

                //// 4. Status Counts - Calcular a partir de tasks/subtasks carregadas + fallback para TotalOpenItems
                int todoCount = 0;
                int inProgressCount = 0;
                int blockedCount = 0;
                int doneCount = 0;

                void AccumulateStatus(string status, int idLookupStatus)
                {
                    if (IsClosedExact(status, idLookupStatus))
                    {
                        doneCount++;
                        return;
                    }

                    var statusLower = (status ?? string.Empty).ToLowerInvariant();

                    if (statusLower.Contains("blocked") || statusLower.Contains("hold"))
                        blockedCount++;
                    else if (statusLower.Contains("in progress") || statusLower.Contains("working"))
                        inProgressCount++;
                    else
                        todoCount++;
                }

                foreach (var plan in _allDataCache)
                {
                    if (plan.TaskList != null && plan.TaskList.Any())
                    {
                        // Plan has loaded tasks - count from client data
                        foreach (var task in plan.TaskList)
                        {
                            AccumulateStatus(task.Status, task.IdLookupStatus);

                            if (task.SubTaskList == null || task.SubTaskList.Count == 0) continue;
                            foreach (var subTask in task.SubTaskList)
                                AccumulateStatus(subTask.Status, subTask.IdLookupStatus);
                        }
                    }
                    else
                    {
                        // Lazy-loaded plan - prefer server aggregates (precise, includes tasks + subtasks)
                        var statusAgg = ParseAggregates(plan.StatusAggregates, 'S');
                        if (statusAgg.Count > 0)
                        {
                            todoCount += GetCount(statusAgg, "To do");
                            inProgressCount += GetCount(statusAgg, "In progress");
                            blockedCount += GetCount(statusAgg, "Blocked");
                            doneCount += GetCount(statusAgg, "Closed");
                        }
                        else
                        {
                            // fallback (older servers / missing aggregates)
                            todoCount += plan.TotalOpenItems;
                        }
                    }
                }

                // Helpers de cor (mantém igual)
                string ColorByDays(int d) => d >= 5 ? "Red" : (d > 0 ? "Orange" : "Green");
                string ColorByCount(int c) => c == 0 ? "Green" : (c < 5 ? "Orange" : "Red");

                // Os "Most Overdue" ficam vazios por agora para não pesar (ou trazes do SQL se quiseres)
                string mostTheme = "---";
                string mostResp = "---";

                AlertListOfFilterTile = new ObservableCollection<APMAlertTileBarFilters>();

                // --- CRIAÇÃO DOS BOTÕES ---

                // Botão 1: Longest
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters()
                {
                    Caption = "Longest Overdue Days",
                    Id = 0,
                    BackColor = ColorByDays(longestOverdueDays),
                    EntitiesCount = longestOverdueDays.ToString(),
                    EntitiesCountVisibility = Visibility.Collapsed,  // REMOVIDO LABEL
                    Height = 50,
                    width = 150,
                    Type = "LongestOverdue",
                    IsSelected = false
                });

                // Botão 2: Overdue 15
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters()
                {
                    Caption = "Overdue >= 15 days",
                    Id = 1,
                    BackColor = ColorByCount(overdue15Count),
                    EntitiesCount = overdue15Count.ToString(),
                    EntitiesCountVisibility = Visibility.Collapsed,  // REMOVIDO LABEL
                    Height = 50,
                    width = 150,
                    Type = "Overdue15",
                    IsSelected = false
                });

                // Botão 3: High Priority
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters()
                {
                    Caption = "High Priority Overdue",
                    Id = 2,
                    BackColor = ColorByCount(highPriorityCount),
                    EntitiesCount = highPriorityCount.ToString(),
                    EntitiesCountVisibility = Visibility.Collapsed,  // REMOVIDO LABEL
                    Height = 50,
                    width = 150,
                    Type = "HighPriorityOverdue",
                    IsSelected = false
                });

                // Botões 4 e 5 (Most Overdue) - Opcionais, deixamos com placeholders
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "Most Overdue Theme", Id = 3, BackColor = "Green", EntitiesCount = mostTheme, EntitiesCountVisibility = Visibility.Collapsed, Height = 50, width = 150, Type = "MostOverdueTheme" });
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "Most Overdue Responsible", Id = 4, BackColor = "Green", EntitiesCount = mostResp, EntitiesCountVisibility = Visibility.Collapsed, Height = 50, width = 150, Type = "MostOverdueResponsible" });

                // Botões de Status (To do, In progress, Blocked, Closed)
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "To do", Id = 0, BackColor = "Green", EntitiesCount = todoCount.ToString(), EntitiesCountVisibility = Visibility.Collapsed, Height = 50, width = 150, Type = "StatusToDo" });
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "In progress", Id = 0, BackColor = "Green", EntitiesCount = inProgressCount.ToString(), EntitiesCountVisibility = Visibility.Collapsed, Height = 50, width = 150, Type = "StatusInProgress" });
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "Blocked", Id = 0, BackColor = "Green", EntitiesCount = blockedCount.ToString(), EntitiesCountVisibility = Visibility.Collapsed, Height = 50, width = 150, Type = "StatusBlocked" });
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "Closed", Id = 0, BackColor = "Green", EntitiesCount = doneCount.ToString(), EntitiesCountVisibility = Visibility.Collapsed, Height = 50, width = 150, Type = "StatusClosed" });

                GeosApplication.Instance.Logger?.Log("Alert buttons populated (Optimized)", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error PopulateAlertButtons: {ex.Message}", Category.Exception, Priority.High);
            }
        }


        /// <summary>
        /// Manipula o clique nos Alert Buttons - FILTRAR a grid
        /// </summary>
        private void OnAlertButtonClick(APMAlertTileBarFilters clickedItem)
        {
            // Delegate to internal method from Filters partial class
            OnAlertButtonClickInternal(clickedItem);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// Classe auxiliar para representar Tasks e SubTasks uniformemente
        /// </summary>
        private class TaskOrSubTaskItem
        {
            public int DueDays { get; set; }
            public DateTime DueDate { get; set; }
            public int IdLookupStatus { get; set; }
            public string Status { get; set; }
            public string Priority { get; set; }
            public string Theme { get; set; }
            public string Location { get; set; }
            public string Responsible { get; set; }
            public string TaskResponsibleDisplayName { get; set; }
            public string ActionPlanCode { get; set; }
            public bool IsTask { get; set; }

            public int GetActualDueDays()
            {
                if (DueDate >= DateTime.Now) return 0;
                return (int)(DateTime.Now - DueDate).TotalDays;
            }

            public static TaskOrSubTaskItem FromTask(APMActionPlanTask t)
            {
                return new TaskOrSubTaskItem
                {
                    DueDays = t.DueDays,
                    DueDate = t.DueDate,
                    IdLookupStatus = t.IdLookupStatus,
                    Status = t.Status,
                    Priority = t.Priority,
                    Theme = t.Theme,
                    Location = t.Location,
                    Responsible = t.Responsible,
                    TaskResponsibleDisplayName = t.TaskResponsibleDisplayName,
                    ActionPlanCode = t.ActionPlanCode,
                    IsTask = true
                };
            }

            public static TaskOrSubTaskItem FromSubTask(APMActionPlanSubTask st, string actionPlanCode)
            {
                return new TaskOrSubTaskItem
                {
                    DueDays = st.DueDays,
                    DueDate = st.DueDate,
                    IdLookupStatus = st.IdLookupStatus,
                    Status = st.Status,
                    Priority = st.Priority,
                    Theme = st.Theme,
                    Location = st.Location,
                    Responsible = st.Responsible,
                    TaskResponsibleDisplayName = st.TaskResponsibleDisplayName,
                    ActionPlanCode = actionPlanCode,
                    IsTask = false
                };
            }
        }

        #endregion

        #region Helper Methods for Alert Filtering

        private List<TaskOrSubTaskItem> ExtractAllTasksAndSubTasks(List<APMActionPlan> plans)
        {
            var result = new List<TaskOrSubTaskItem>();
            if (plans == null) return result;

            foreach (var ap in plans)
            {
                if (ap.TaskList == null) continue;
                foreach (var t in ap.TaskList)
                {
                    result.Add(TaskOrSubTaskItem.FromTask(t));
                    if (t.SubTaskList != null)
                    {
                        foreach (var st in t.SubTaskList)
                        {
                            result.Add(TaskOrSubTaskItem.FromSubTask(st, t.ActionPlanCode));
                        }
                    }
                }
            }
            return result;
        }
        private void PopulateDropdownFilters(List<APMActionPlanModern> data)
        {
            _isRefreshing = true; // PREVENIR LOOP INFINITO!
            
            try
            {
                if (data == null || data.Count == 0) return;

                // 1. Atualizar Filtro de Departamentos
                var uniqueDepts = data
                    .Where(x => !string.IsNullOrEmpty(x.Department))
                    .Select(x => x.Department)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                // Se tiveres uma lista na ViewModel (ex: DepartmentFilterList), podes limpá-la e adicionar estes itens.
            }
            catch (Exception ex)
            {
                // Category.Exception existe no teu Logger
                GeosApplication.Instance.Logger?.Log($"Error populating dropdowns: {ex.Message}", Category.Exception, Priority.Low);
            }
            finally
            {
                _isRefreshing = false; // DESATIVAR flag
            }
        }

        private bool IsClosedItem(TaskOrSubTaskItem item)
        {
            if (item == null) return true;
            var s = (item.Status ?? string.Empty).Trim().ToLowerInvariant();
            return s == "done" || s == "completed" || s == "closed" || item.IdLookupStatus == 1982;
        }

        private bool IsClosed(APMActionPlanTask t)
        {
            if (t == null) return false;
            var status = t.Status ?? string.Empty;
            return status.IndexOf("done", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   status.IndexOf("closed", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   status.IndexOf("completed", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   t.IdLookupStatus == 1982;
        }

        private string GetMostOverdueThemeNameFromItems(System.Collections.IEnumerable items)
        {
            try
            {
                if (items == null) return null;

                // Converter para dynamic para aceitar tanto DTOs modernos como Items antigos
                var dynamicItems = items.Cast<dynamic>().ToList();

                // Filtra itens com DueDays > 0 (ou GetActualDueDays se for o tipo antigo)
                var overdueItems = dynamicItems.Where(t =>
                {
                    int days = 0;
                    // Tenta obter os dias de atraso de forma segura
                    try { days = t.DueDays; } catch { try { days = t.GetActualDueDays(); } catch { } }

                    // Tenta verificar status
                    bool isClosed = false;
                    try { isClosed = IsClosed(t.Status, (int)t.IdLookupStatus); } catch { }

                    return days > 0 && !isClosed;
                }).ToList();

                if (!overdueItems.Any()) return null;

                return overdueItems
                    .Where(t => !string.IsNullOrWhiteSpace(t.Theme))
                    .GroupBy(t => ((string)t.Theme).Trim())
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()
                    ?.Key;
            }
            catch
            {
                return null;
            }
        }

        private string GetMostOverdueResponsibleNameFromItems(System.Collections.IEnumerable items)
        {
            try
            {
                if (items == null) return null;

                var dynamicItems = items.Cast<dynamic>().ToList();

                var overdueItems = dynamicItems.Where(t =>
                {
                    int days = 0;
                    try { days = t.DueDays; } catch { try { days = t.GetActualDueDays(); } catch { } }

                    bool isClosed = false;
                    try { isClosed = IsClosed(t.Status, (int)t.IdLookupStatus); } catch { }

                    return days > 0 && !isClosed;
                }).ToList();

                if (!overdueItems.Any()) return null;

                return overdueItems
                    .Where(t => !string.IsNullOrWhiteSpace(t.Responsible)) // Assumindo que Responsible existe em ambos
                    .GroupBy(t => ((string)t.Responsible).Trim())
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()
                    ?.Key;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Side Tiles (Theme Filters)

        /// <summary>
        /// Popula os Side Tiles (filtros por Theme)
        /// </summary>
        private async void PopulateSideTiles()
        {
            try
            {
                GeosApplication.Instance.Logger?.Log("PopulateSideTiles started", Category.Info, Priority.Low);

                // Obter themes do serviço (Background thread)
                var themes = await Task.Run(() => _apmService.GetLookupValues_V2550(155).ToList());

                if (_allDataCache == null)
                {
                    ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    return;
                }

                // --- MUDANÇA CRÍTICA ---
                // Não usamos ExtractAllTasksAndSubTasks. Usamos a contagem direta dos Planos.

                var sideTiles = new ObservableCollection<TileBarFilters>();

                // 1. Adicionar tile "All"
                sideTiles.Add(new TileBarFilters
                {
                    Caption = "All",
                    Id = 0,
                    BackColor = "#FFFFFF",
                    FilterCriteria = "",
                    EntitiesCount = 0,
                    EntitiesCountVisibility = Visibility.Collapsed,  // REMOVIDO LABEL
                    Height = 60,
                    width = 230
                });

                // 2. Adicionar tiles por Theme
                foreach (var theme in themes.OrderBy(x => x.Value))
                {
                    // --- LÓGICA NOVA (RÁPIDA) ---
                    // Conta quantos Planos têm este tema na string "Stat_ThemesList"
                    string themeName = (theme.Value ?? "").Trim();

                    sideTiles.Add(new TileBarFilters
                    {
                        Caption = theme.Value,
                        Id = theme.IdLookupValue,
                        BackColor = theme.HtmlColor,
                        ForeColor = null,
                        FilterCriteria = $"[Theme] IN ('{theme.Value}')",
                        EntitiesCount = 0,
                        EntitiesCountVisibility = Visibility.Collapsed,  // REMOVIDO LABEL
                        Height = 60,
                        width = 230
                    });
                }

                ListOfFilterTile = sideTiles;

                // Atualiza contagens com a heurística correta (tasks + subtasks, sem Done)
                UpdateSideTileCounts();

                GeosApplication.Instance.Logger?.Log($"PopulateSideTiles completed with {sideTiles.Count} tiles", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in PopulateSideTiles: {ex.Message}", Category.Exception, Priority.High);
            }
        }

        // MUDANÇA: O parâmetro tem de ser APMActionPlanModern
        private void UpdateSideTileCounts(List<APMActionPlanModern> dataToCount = null)
        {
            try
            {
                if (ListOfFilterTile == null || !ListOfFilterTile.Any()) return;

                var sourcePlans = dataToCount ?? _allDataCache ?? new List<APMActionPlanModern>();

                // Reaplica filtros de dropdown para garantir alinhamento com o Classic UI
                var personIdSet = ToResponsibleIdSet(SelectedPerson);
                var personNameSet = ToResponsibleHashSet(SelectedPerson);
                var locSet = ToStringHashSet(SelectedLocation);
                var buSet = ToStringHashSet(SelectedBusinessUnit);
                var originSet = ToStringHashSet(SelectedOrigin);
                var customerSet = ToCustomerHashSet(SelectedCustomer);
                var customerIdSet = SelectedCustomer?.OfType<APMCustomer>().Select(c => c.IdSite).Where(id => id > 0).ToHashSet();
                var codeSet = (HashSet<string>)null;
                bool customerIncludesBlanks = HasBlanksSelected(SelectedCustomer);

                // Split plans into those with loaded tasks and those still lazy (no TaskList)
                var plansWithTasks = sourcePlans.Where(p => p.TaskList != null && p.TaskList.Count > 0).ToList();
                var plansWithoutTasks = sourcePlans.Except(plansWithTasks).ToList();

                var itemsFromLoaded = ExtractAllTasksAndSubTasksWithAllFilters(
                    plansWithTasks,
                    personIdSet,
                    personNameSet,
                    locSet,
                    buSet,
                    originSet,
                    customerSet,
                    customerIdSet,
                    codeSet,
                    customerIncludesBlanks);

                bool IsOpen(TaskOrSubTaskItem item) => !IsClosedExact(item.Status, item.IdLookupStatus);

                Dictionary<string, int> ParseAggregates(string aggregates, char expectedPrefix)
                {
                    var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    if (string.IsNullOrWhiteSpace(aggregates)) return result;

                    foreach (var rawPart in aggregates.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var part = rawPart?.Trim();
                        if (string.IsNullOrEmpty(part) || part.Length < 3) continue;
                        if (part[1] != '|') continue;
                        if (expectedPrefix != '\0' && part[0] != expectedPrefix) continue;

                        var payload = part.Substring(2);
                        var idx = payload.LastIndexOf(':');
                        if (idx <= 0 || idx >= payload.Length - 1) continue;

                        var key = payload.Substring(0, idx).Trim();
                        var valueStr = payload.Substring(idx + 1).Trim();
                        if (string.IsNullOrEmpty(key)) continue;
                        if (!int.TryParse(valueStr, out var value)) continue;

                        if (result.TryGetValue(key, out var existing))
                            result[key] = existing + value;
                        else
                            result[key] = value;
                    }

                    return result;
                }

                int GetCount(Dictionary<string, int> dict, string key)
                {
                    if (dict == null || string.IsNullOrWhiteSpace(key)) return 0;
                    return dict.TryGetValue(key, out var v) ? v : 0;
                }

                int GetOpenCountFromStatusAggregates(APMActionPlanModern plan)
                {
                    if (plan == null) return 0;

                    var statusAgg = ParseAggregates(plan.StatusAggregates, 'S');
                    if (statusAgg.Count > 0)
                    {
                        // Open = everything except Closed (labels are case-insensitive)
                        var closed = GetCount(statusAgg, "Closed");
                        var total = statusAgg.Values.Sum();
                        var open = total - closed;
                        return open < 0 ? 0 : open;
                    }

                    // Fallback: older servers only provide task-level open totals
                    return plan.TotalOpenItems;
                }

                foreach (var tile in ListOfFilterTile)
                {
                    if (tile == null) continue;

                    int count = 0;

                    if (IsAllCaption(tile.Caption))
                    {
                        // Open items from loaded plans + aggregated open counts from unloaded plans
                        int loadedCount = itemsFromLoaded.Count(IsOpen);
                        int unloadedCount = plansWithoutTasks.Sum(GetOpenCountFromStatusAggregates);
                        count = loadedCount + unloadedCount;
                    }
                    else
                    {
                        // Try to compute match from loaded items first
                        var exprs = ParseCriteria(tile.FilterCriteria);
                        if (exprs != null && exprs.Count > 0)
                        {
                            bool Matches(TaskOrSubTaskItem item) => exprs.All(e => PropertyMatches(item, e.Field, e.Values));
                            int loadedMatches = itemsFromLoaded.Where(IsOpen).Count(Matches);

                            // For unloaded plans we have limited info: use Stat_ThemesList and per-plan totals as fallback
                            int unloadedMatches = 0;
                            string filterCaption = (tile.Caption ?? "").Trim();

                            if (!string.IsNullOrEmpty(filterCaption))
                            {
                                foreach (var p in plansWithoutTasks)
                                {
                                    // Prefer precise per-theme open counts if provided by the server
                                    var themeAgg = ParseAggregates(p.ThemeAggregates, 'T');
                                    if (themeAgg.Count > 0)
                                    {
                                        unloadedMatches += GetCount(themeAgg, filterCaption);
                                        continue;
                                    }

                                    // Fallback: legacy approximation based on theme list
                                    if (!string.IsNullOrWhiteSpace(p.Stat_ThemesList) &&
                                        p.Stat_ThemesList.IndexOf(filterCaption, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        unloadedMatches += p.TotalOpenItems;
                                    }
                                }
                            }

                            count = loadedMatches + unloadedMatches;
                        }
                    }

                    tile.EntitiesCount = count;
                    tile.EntitiesCountVisibility = Visibility.Visible;
                }

                // Refresh visual
                var tmp = ListOfFilterTile;
                ListOfFilterTile = null;
                OnPropertyChanged(nameof(ListOfFilterTile));
                ListOfFilterTile = tmp;
                OnPropertyChanged(nameof(ListOfFilterTile));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in UpdateSideTileCounts: {ex.Message}", Category.Exception, Priority.High);
            }
        }
        /// <summary>
        /// Manipula o clique nos Side Tiles - FILTRAR a grid por Theme
        /// </summary>
        private void OnSideTileClick(object parameter)
        {
            // Delegate to internal method from Filters partial class
            OnSideTileClickedInternal(parameter);
        }

        #endregion

        #region Filter Parameter Builders (ModernUI - SQL Filter Support)

        /// <summary>
        /// Constrói string CSV de IDs de Location a partir de SelectedLocation
        /// Formato: "1,2,3" ou null se vazio
        /// </summary>
        private string BuildLocationFilter()
        {
            if (SelectedLocation == null || SelectedLocation.Count == 0)
                return null;

            try
            {
                var ids = SelectedLocation
                    .OfType<Company>()
                    .Select(c => c.IdCompany)
                    .Where(id => id > 0)
                    .Distinct()
                    .OrderBy(id => id);

                return ids.Any() ? string.Join(",", ids) : null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error building Location filter: {ex.Message}", Category.Exception, Priority.Low);
                return null;
            }
        }

        /// <summary>
        /// Constrói string CSV de IDs de Responsible (IdEmployee) a partir de SelectedPerson
        /// Formato: "10,20,30" ou null se vazio
        /// </summary>
        private string BuildResponsibleFilter()
        {
            if (SelectedPerson == null || SelectedPerson.Count == 0)
                return null;

            try
            {
                var ids = SelectedPerson
                    .OfType<Emdep.Geos.Data.Common.APM.Responsible>()
                    .Select(r => r.IdEmployee)
                    .Where(id => id > 0)
                    .Distinct()
                    .OrderBy(id => id);

                return ids.Any() ? string.Join(",", ids) : null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error building Responsible filter: {ex.Message}", Category.Exception, Priority.Low);
                return null;
            }
        }

        /// <summary>
        /// Constrói string CSV de IDs de Business Unit a partir de SelectedBusinessUnit
        /// </summary>
        private string BuildBusinessUnitFilter()
        {
            if (SelectedBusinessUnit == null || SelectedBusinessUnit.Count == 0)
                return null;

            try
            {
                var ids = SelectedBusinessUnit
                    .OfType<LookupValue>()
                    .Select(bu => bu.IdLookupValue)
                    .Where(id => id > 0)
                    .Distinct()
                    .OrderBy(id => id);

                return ids.Any() ? string.Join(",", ids) : null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error building BusinessUnit filter: {ex.Message}", Category.Exception, Priority.Low);
                return null;
            }
        }

        /// <summary>
        /// Constrói string CSV de IDs de Origin a partir de SelectedOrigin
        /// </summary>
        private string BuildOriginFilter()
        {
            if (SelectedOrigin == null || SelectedOrigin.Count == 0)
                return null;

            try
            {
                var ids = SelectedOrigin
                    .OfType<LookupValue>()
                    .Select(o => o.IdLookupValue)
                    .Where(id => id > 0)
                    .Distinct()
                    .OrderBy(id => id);

                return ids.Any() ? string.Join(",", ids) : null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error building Origin filter: {ex.Message}", Category.Exception, Priority.Low);
                return null;
            }
        }

        /// <summary>
        /// Constrói string CSV de IDs de Department a partir de SelectedDepartment
        /// </summary>
        private string BuildDepartmentFilter()
        {
            if (SelectedDepartment == null || SelectedDepartment.Count == 0)
                return null;

            try
            {
                var ids = SelectedDepartment
                    .OfType<Department>()
                    .Select(d => d.IdDepartment)
                    .Where(id => id > 0)
                    .Distinct()
                    .OrderBy(id => id);

                return ids.Any() ? string.Join(",", ids) : null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error building Department filter: {ex.Message}", Category.Exception, Priority.Low);
                return null;
            }
        }

        /// <summary>
        /// Constrói string CSV de IDs de Customer (IdSite) a partir de SelectedCustomer
        /// </summary>
        private string BuildCustomerFilter()
        {
            if (SelectedCustomer == null || SelectedCustomer.Count == 0)
                return null;

            try
            {
                var ids = SelectedCustomer
                    .OfType<APMCustomer>()
                    .Select(c => c.IdSite)
                    .Where(id => id > 0)
                    .Distinct()
                    .OrderBy(id => id);

                return ids.Any() ? string.Join(",", ids) : null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error building Customer filter: {ex.Message}", Category.Exception, Priority.Low);
                return null;
            }
        }

        /// <summary>
        /// Obtém o filtro de Alert atual baseado no botão ativo
        /// Retorna: 'ToDo', 'InProgress', 'Blocked', 'Closed', 'Overdue15', 'HighPriorityOverdue', etc.
        /// </summary>
        private string GetCurrentAlertFilter()
        {
            // A lógica de AlertButtons está no ActionPlansModernViewModel.Filters.cs
            // Precisamos identificar qual botão está ativo

            if (AlertListOfFilterTile == null || !AlertListOfFilterTile.Any())
                return null;

            var activeButton = AlertListOfFilterTile.FirstOrDefault(b => b.IsSelected);
            if (activeButton == null)
                return null;

            // MODERNUI FIX: Usar Caption EXATAMENTE como aparece nos botões (linha 2782-2831)
            var caption = activeButton.Caption ?? "";
            
            GeosApplication.Instance.Logger?.Log($"[GetCurrentAlertFilter] Active button Caption: '{caption}'", Category.Info, Priority.Low);

            // Mapear Caption para valor SQL que a SP espera
            if (caption == "Longest Overdue Days")
                return "LongestOverdue";
            if (caption == "High Priority Overdue")
                return "HighPriorityOverdue";
            if (caption == "Overdue > 15 Days")
                return "Overdue15";
            if (caption == "Most Overdue Theme")
                return "MostOverdueTheme";
            if (caption == "To do")
                return "ToDo";
            if (caption == "In progress")
                return "InProgress";
            if (caption == "Blocked")
                return "Blocked";
            if (caption == "Closed")
                return "Closed";

            GeosApplication.Instance.Logger?.Log($"[GetCurrentAlertFilter] Unknown caption '{caption}' - returning null", Category.Info, Priority.Low);
            return null;
        }

        /// <summary>
        /// Obtém o Theme Filter atual baseado no Side Tile selecionado
        /// </summary>
        private string GetCurrentThemeFilter()
        {
            // [MODERNUI FIX] Usar _lastAppliedSideTileFilter do partial class Filters.cs
            if (_lastAppliedSideTileFilter == null || IsAllCaption(_lastAppliedSideTileFilter.Caption))
                return null;

            return _lastAppliedSideTileFilter.Caption; // Ex: "Safety", "Quality"
        }

        #endregion

        #region Cleanup

        public void Dispose()
        {
            _loadCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource?.Cancel();
            _loadCancellationTokenSource?.Dispose();
            _searchCancellationTokenSource?.Dispose();

            lock (_cacheLock)
            {
                _tasksCache.Clear();
            }
        }

        #endregion


    }
}
