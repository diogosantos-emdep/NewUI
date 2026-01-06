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
                _selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
                ApplyInMemoryFiltersAsync();
            }
        }

        public List<object> SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
                ApplyInMemoryFiltersAsync();
            }
        }

        public List<object> SelectedBusinessUnit
        {
            get => _selectedBusinessUnit;
            set
            {
                _selectedBusinessUnit = value;
                OnPropertyChanged(nameof(SelectedBusinessUnit));
                ApplyInMemoryFiltersAsync();
            }
        }

        public List<object> SelectedOrigin
        {
            get => _selectedOrigin;
            set
            {
                _selectedOrigin = value;
                OnPropertyChanged(nameof(SelectedOrigin));
                ApplyInMemoryFiltersAsync();
            }
        }

        public List<object> SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
                ApplyInMemoryFiltersAsync();
            }
        }

        public List<object> SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
                ApplyInMemoryFiltersAsync();
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
            // [FIX LOOP 1] Impede re-entradas se já estiver a carregar (Evita loop infinito no scroll)
            if (IsLoadingMore) return;

            try
            {
                if (GeosApplication.Instance == null || GeosApplication.Instance.ActiveUser == null) return;

                // Cancelar pedidos anteriores pendentes para poupar rede
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
                        // Obtém o ano selecionado na UI (ex: 2025)
                        long selectedYear = DateTime.Now.Year;
                        if (APMCommon.Instance?.SelectedPeriod?.Count > 0)
                        {
                            selectedYear = APMCommon.Instance.SelectedPeriod.Cast<long>().FirstOrDefault();
                        }

                        // Cria a string para o SQL: "2025,2024"
                        // O SQL vai trazer: Tudo de 2025 + Tudo de 2024 + Tudo o que for mais antigo e estiver ABERTO
                        string period = $"{selectedYear},{selectedYear - 1}";

                        int userId = GeosApplication.Instance.ActiveUser.IdUser;

                        // Filtros de Texto (Botões de Alerta e Tiles Laterais)
                        string filterAlert = !string.IsNullOrEmpty(_lastAppliedAlertCaption) ? _lastAppliedAlertCaption : null;
                        string filterTheme = null;

                        if (_lastAppliedSideTileFilter != null && !IsAllCaption(_lastAppliedSideTileFilter.Caption))
                        {
                            filterTheme = _lastAppliedSideTileFilter.Caption;
                        }

                        // [CONFIGURAÇÃO DE REDE] - IP do Slave/App Server correto
                        string ip = "10.13.3.33";
                        string port = "90";
                        string path = ""; // Vazio porque o serviço está na raiz (:90/APMService.svc)

                        // Instancia o controlador com os dados manuais (Ignora app.config)
                        var localService = new APMServiceController(ip, port, path);

                        return localService.GetActionPlanDetails_WithCounts(period, userId, filterAlert, filterTheme);

                    }, cancellationToken);

                    // Se a lista vier vazia ou nula, paramos aqui
                    if (_allDataCache == null || _allDataCache.Count == 0)
                    {
                        _hasMoreData = false;
                        IsLoadingMore = false; // Importante libertar a flag
                        return;
                    }

                    // --- 2. SINCRONIZAÇÃO DA UI (Counts e Dropdowns) ---
                    // Executamos na Thread Principal porque vamos mexer na UI
                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UpdateSideTileCounts(_allDataCache);
                            UpdateAlertButtonCounts(_allDataCache);
                            PopulateDropdownFilters(_allDataCache);
                        });
                    }
                }

                if (cancellationToken.IsCancellationRequested) return;

                // --- 3. PAGINAÇÃO E VISUALIZAÇÃO NA GRID ---

                // Verifica se há algum filtro visual aplicado
                bool isFiltering = !string.IsNullOrEmpty(_lastAppliedAlertCaption) ||
                                   (_lastAppliedSideTileFilter != null && !IsAllCaption(_lastAppliedSideTileFilter.Caption));

                if (isFiltering)
                {
                    // Se estiver a filtrar, desligamos a paginação para mostrar todos os resultados filtrados
                    _pageSize = 999999;
                    if (_allDataCache.Count > 0) await PopulateAllTasksInCacheAsync();
                }
                else
                {
                    // Paginação normal
                    _pageSize = 50;
                }

                var skip = _currentPage * _pageSize;
                var pagedData = _allDataCache.Skip(skip).Take(_pageSize).ToList();
                var dtos = pagedData.Select(MapToDto).Where(dto => dto != null).ToList();

                // Adiciona os itens à lista observável da Grid
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var dto in dtos)
                        {
                            if (isFiltering) dto.IsExpanded = true;
                            ActionPlans.Add(dto);
                        }
                    });
                }

                _currentPage++;

                // [FIX LOOP 2] Cálculo correto de 'HasMoreData' para impedir scroll infinito desnecessário
                if (isFiltering)
                {
                    _hasMoreData = false;
                }
                else
                {
                    // Só dizemos que há mais dados se a página atual veio cheia E ainda sobraram itens no cache
                    _hasMoreData = (pagedData.Count == _pageSize) && (_allDataCache.Count > (skip + pagedData.Count));
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
            if (actionPlan == null) return;

            // Se já tem tasks carregadas, não recarregar
            if (actionPlan.Tasks != null && actionPlan.Tasks.Count > 0) return;

            try
            {
                actionPlan.IsLoadingTasks = true;

                // Verificar cache primeiro
                List<ActionPlanTaskModernDto> taskDtos;
                lock (_cacheLock)
                {
                    if (_tasksCache.ContainsKey(actionPlan.IdActionPlan))
                    {
                        taskDtos = _tasksCache[actionPlan.IdActionPlan];
                        actionPlan.Tasks = new ObservableCollection<ActionPlanTaskModernDto>(taskDtos);
                        return;
                    }
                }

                // Carregar do serviço
                var tasks = await Task.Run(() =>
                {
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
                    return _apmService.GetTaskListByIdActionPlan_V2680(actionPlan.IdActionPlan, period, userId);
                });

                if (tasks == null) tasks = new List<APMActionPlanTask>();

                taskDtos = tasks.Select(MapTaskToDto).Where(dto => dto != null).ToList();

                // Guardar no cache
                lock (_cacheLock)
                {
                    if (!_tasksCache.ContainsKey(actionPlan.IdActionPlan))
                        _tasksCache[actionPlan.IdActionPlan] = taskDtos;
                }

                // Popular no Action Plan
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        actionPlan.Tasks = new ObservableCollection<ActionPlanTaskModernDto>(taskDtos);
                    });
                }
                else
                {
                    actionPlan.Tasks = new ObservableCollection<ActionPlanTaskModernDto>(taskDtos);
                }

                GeosApplication.Instance.Logger?.Log($"Loaded {taskDtos.Count} tasks for Action Plan {actionPlan.Code}", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error loading tasks for Action Plan {actionPlan.Code}: {ex.Message}", Category.Exception, Priority.High);
            }
            finally
            {
                actionPlan.IsLoadingTasks = false;
            }
        }

        /// <summary>
        /// Toggle expand/collapse para um Action Plan
        /// </summary>
        private async Task ToggleActionPlanExpandAsync(ActionPlanModernDto actionPlan)
        {
            if (actionPlan == null) return;

            actionPlan.IsExpanded = !actionPlan.IsExpanded;

            if (actionPlan.IsExpanded)
            {
                await LoadTasksForActionPlanAsync(actionPlan);
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

        /// <summary>
        /// Carrega sub-tasks para uma Task específica (chamado ao expandir)
        /// </summary>
        public async Task LoadSubTasksForTaskAsync(ActionPlanTaskModernDto task)
        {
            if (task == null) return;

            // Se já tem sub-tasks carregadas, não recarregar
            if (task.SubTasks != null && task.SubTasks.Count > 0) return;

            try
            {
                task.IsLoadingSubTasks = true;

                // Carregar sub-tasks do serviço (via Task.SubTaskList que já foi carregado com a task)
                // Ou podemos buscar do cache de tasks se necessário
                var subTasks = await Task.Run(() =>
                {
                    // Procurar no cache primeiro
                    lock (_cacheLock)
                    {
                        // Tentar obter do allDataCache que tem TaskList com SubTaskList
                        if (_allDataCache != null)
                        {
                            foreach (var ap in _allDataCache)
                            {
                                if (ap.TaskList != null)
                                {
                                    var foundTask = ap.TaskList.FirstOrDefault(t => t.IdActionPlanTask == task.IdTask);
                                    if (foundTask?.SubTaskList != null)
                                    {
                                        return foundTask.SubTaskList;
                                    }
                                }
                            }
                        }
                    }
                    return new List<Data.Common.APM.APMActionPlanSubTask>();
                });

                var subTaskDtos = subTasks.Select(MapSubTaskToDto).Where(dto => dto != null).ToList();

                // Popular na Task
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        task.SubTasks = new ObservableCollection<SubTaskModernDto>(subTaskDtos);
                    });
                }
                else
                {
                    task.SubTasks = new ObservableCollection<SubTaskModernDto>(subTaskDtos);
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

        private ActionPlanModernDto MapToDto(APMActionPlanModern entity) // Nota: Recebe a classe Modern
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
                    IdActionPlan = entity.IdActionPlan,
                    Code = entity.Code ?? string.Empty,
                    Title = entity.Description ?? string.Empty,

                    // --- CORREÇÃO RESPONSÁVEL ---
                    // Usa a propriedade Responsible direta se existir, senão usa o DisplayName
                    Responsible = !string.IsNullOrEmpty(entity.Responsible)
                                  ? entity.Responsible
                                  : (entity.ActionPlanResponsibleDisplayName ?? string.Empty),

                    EmployeeCode = entity.EmployeeCode ?? string.Empty,
                    IdGender = entity.IdGender,
                    IdEmployee = entity.IdEmployee,
                    Location = entity.Location ?? string.Empty,
                    IdLocation = entity.IdCompany,

                    ThemeAggregates = entity.ThemeAggregates,
                    StatusAggregates = entity.StatusAggregates,
                    // --- CONTAGENS (Vêm do SQL/C#) ---
                    TasksCount = entity.TotalActionItems, // Total de itens (não a contagem da lista, que pode vir vazia)
                    TotalActionItems = entity.TotalActionItems,
                    TotalOpenItems = entity.TotalOpenItems,
                    TotalClosedItems = entity.TotalClosedItems,
                    Percentage = entity.Percentage,

                    // --- ESTATÍSTICAS NOVAS ---
                    Stat_Overdue15 = entity.Stat_Overdue15,
                    Stat_HighPriorityOverdue = entity.Stat_HighPriorityOverdue,
                    Stat_MaxDueDays = entity.Stat_MaxDueDays,

                    // --- COR ---
                    TotalClosedColor = ConvertColor(entity.TotalClosedColor),

                    // Restantes campos
                    Status = entity.BusinessUnit ?? string.Empty,
                    BusinessUnit = entity.BusinessUnit ?? string.Empty,
                    BusinessUnitHTMLColor = entity.BusinessUnitHTMLColor, // Mapear cor da BU se existir

                    Priority = entity.Origin ?? string.Empty,
                    Origin = entity.Origin ?? string.Empty,

                    IdSite = entity.IdSite,
                    Site = entity.Site ?? string.Empty,
                    GroupName = entity.GroupName ?? string.Empty,

                    IdDepartment = entity.IdDepartment,
                    Department = entity.Department ?? string.Empty,

                    // Country (assumindo que mapeaste as propriedades "Country..." na classe Modern)
                    CountryIconUrl = entity.CountryIconUrl ?? string.Empty
                };

                // --- CARREGAMENTO DE TAREFAS (Se já vierem preenchidas) ---
                if (entity.TaskList != null && entity.TaskList.Count > 0)
                {
                    var taskDtos = entity.TaskList.Select(MapTaskToDto).Where(t => t != null).ToList();
                    dto.Tasks = new ObservableCollection<ActionPlanTaskModernDto>(taskDtos);
                }
                else
                {
                    dto.Tasks = new ObservableCollection<ActionPlanTaskModernDto>();
                }

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
                if (_allActionPlansUnfiltered == null || _allActionPlansUnfiltered.Count == 0)
                {
                    return;
                }

                // Verificar se há algum filtro ativo
                bool hasLocationFilter = SelectedLocation != null && SelectedLocation.Count > 0;
                bool hasResponsibleFilter = SelectedPerson != null && SelectedPerson.Count > 0;
                bool hasBusinessUnitFilter = SelectedBusinessUnit != null && SelectedBusinessUnit.Count > 0;
                bool hasOriginFilter = SelectedOrigin != null && SelectedOrigin.Count > 0;
                bool hasDepartmentFilter = SelectedDepartment != null && SelectedDepartment.Count > 0;
                bool hasCustomerFilter = SelectedCustomer != null && SelectedCustomer.Count > 0;

                if (!hasLocationFilter && !hasResponsibleFilter && !hasBusinessUnitFilter &&
                    !hasOriginFilter && !hasDepartmentFilter && !hasCustomerFilter)
                {
                    // Sem filtros - mostrar tudo
                    ActionPlans = new ObservableCollection<ActionPlanModernDto>(_allActionPlansUnfiltered);
                    return;
                }

                // Preparar listas de filtro
                var locations = hasLocationFilter ? SelectedLocation.Cast<Company>().ToList() : null;
                var responsibles = hasResponsibleFilter ? SelectedPerson.Cast<Responsible>().Where(r => !string.IsNullOrEmpty(r.FullName)).ToList() : null;
                var businessUnits = hasBusinessUnitFilter ? SelectedBusinessUnit.Cast<LookupValue>().Where(bu => !string.IsNullOrEmpty(bu.Value)).ToList() : null;
                var origins = hasOriginFilter ? SelectedOrigin.Cast<LookupValue>().Where(o => !string.IsNullOrEmpty(o.Value)).ToList() : null;
                var departments = hasDepartmentFilter ? SelectedDepartment.Cast<Department>().ToList() : null;
                var customers = hasCustomerFilter ? SelectedCustomer.Cast<APMCustomer>().ToList() : null;

                var filtered = new List<ActionPlanModernDto>();

                foreach (var apDto in _allActionPlansUnfiltered)
                {
                    // Verificar se o Action Plan corresponde aos filtros
                    bool apMatches = CheckItemMatchesFilters(
                        apDto.IdLocation, apDto.Responsible, apDto.BusinessUnit, apDto.Origin, apDto.IdDepartment, apDto.IdSite,
                        locations, responsibles, businessUnits, origins, departments, customers,
                        hasLocationFilter, hasResponsibleFilter, hasBusinessUnitFilter, hasOriginFilter, hasDepartmentFilter, hasCustomerFilter);

                    if (apMatches)
                    {
                        // Action Plan corresponde - incluir com todas as Tasks/SubTasks
                        filtered.Add(apDto);
                    }
                    else if (_allDataCache != null)
                    {
                        // Action Plan não corresponde - verificar Tasks e SubTasks
                        var sourceAP = _allDataCache.FirstOrDefault(ap => ap.IdActionPlan == apDto.IdActionPlan);
                        if (sourceAP?.TaskList != null && sourceAP.TaskList.Any())
                        {
                            var filteredTasks = new List<ActionPlanTaskModernDto>();

                            foreach (var task in sourceAP.TaskList)
                            {
                                bool taskMatches = CheckItemMatchesFilters(
                                    task.IdCompany, task.Responsible, task.BusinessUnit, task.Origin, null, task.IdSite,
                                    locations, responsibles, businessUnits, origins, departments, customers,
                                    hasLocationFilter, hasResponsibleFilter, hasBusinessUnitFilter, hasOriginFilter, hasDepartmentFilter, hasCustomerFilter);

                                if (taskMatches)
                                {
                                    // Task corresponde - incluir com todas as SubTasks
                                    var taskDto = MapTaskToDto(task);
                                    if (taskDto != null)
                                    {
                                        filteredTasks.Add(taskDto);
                                    }
                                }
                                else if (task.SubTaskList != null && task.SubTaskList.Any())
                                {
                                    // Task não corresponde - verificar SubTasks
                                    var filteredSubTasks = new List<SubTaskModernDto>();

                                    foreach (var subTask in task.SubTaskList)
                                    {
                                        bool subTaskMatches = CheckItemMatchesFilters(
                                            subTask.IdCompany, subTask.Responsible, subTask.BusinessUnit, subTask.Origin, null, subTask.IdSite,
                                            locations, responsibles, businessUnits, origins, departments, customers,
                                            hasLocationFilter, hasResponsibleFilter, hasBusinessUnitFilter, hasOriginFilter, hasDepartmentFilter, hasCustomerFilter);

                                        if (subTaskMatches)
                                        {
                                            var subTaskDto = MapSubTaskToDto(subTask);
                                            if (subTaskDto != null)
                                            {
                                                filteredSubTasks.Add(subTaskDto);
                                            }
                                        }
                                    }

                                    // Se alguma SubTask corresponder, incluir a Task só com essas SubTasks
                                    if (filteredSubTasks.Any())
                                    {
                                        var taskDto = MapTaskToDto(task);
                                        if (taskDto != null)
                                        {
                                            taskDto.SubTasks = new ObservableCollection<SubTaskModernDto>(filteredSubTasks);
                                            filteredTasks.Add(taskDto);
                                        }
                                    }
                                }
                            }

                            // Se alguma Task corresponder (ou SubTask), incluir o Action Plan só com essas Tasks
                            if (filteredTasks.Any())
                            {
                                var filteredAP = new ActionPlanModernDto
                                {
                                    IdActionPlan = apDto.IdActionPlan,
                                    Code = apDto.Code,
                                    Title = apDto.Title,
                                    IdLocation = apDto.IdLocation,
                                    Location = apDto.Location,
                                    CountryIconUrl = apDto.CountryIconUrl,
                                    Responsible = apDto.Responsible,
                                    BusinessUnit = apDto.BusinessUnit,
                                    Origin = apDto.Origin,
                                    IdDepartment = apDto.IdDepartment,
                                    Department = apDto.Department,
                                    IdSite = apDto.IdSite,
                                    Site = apDto.Site,
                                    Status = apDto.Status,
                                    EmployeeCode = apDto.EmployeeCode,
                                    IdGender = apDto.IdGender,
                                    IdEmployee = apDto.IdEmployee,
                                    IsExpanded = apDto.IsExpanded,
                                    TasksCount = apDto.TasksCount,
                                    TotalActionItems = apDto.TotalActionItems,
                                    TotalOpenItems = apDto.TotalOpenItems,
                                    TotalClosedItems = apDto.TotalClosedItems,
                                    Percentage = apDto.Percentage,
                                    DueDateDisplay = apDto.DueDateDisplay,
                                    Priority = apDto.Priority,
                                    Tasks = new ObservableCollection<ActionPlanTaskModernDto>(filteredTasks)
                                };
                                filtered.Add(filteredAP);
                            }
                        }
                    }
                }

                // Apply the filter - create new collection
                ActionPlans = new ObservableCollection<ActionPlanModernDto>(filtered);

                // Update alert button counts based on filtered data
                UpdateAlertButtonCounts();

                // Update side tile counts based on filtered data
                UpdateSideTileCounts();

                GeosApplication.Instance.Logger?.Log(
                    $"Local filter applied (hierarchical): {ActionPlans.Count} Action Plans, filtered Tasks/SubTasks shown",
                    Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger?.Log($"Error in ApplyFilters: {ex.Message}", Category.Exception, Priority.High);
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
                    EntitiesCountVisibility = Visibility.Visible,
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
                    EntitiesCountVisibility = Visibility.Visible,
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
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 50,
                    width = 150,
                    Type = "HighPriorityOverdue",
                    IsSelected = false
                });

                // Botões 4 e 5 (Most Overdue) - Opcionais, deixamos com placeholders
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "Most Overdue Theme", Id = 3, BackColor = "Green", EntitiesCount = mostTheme, EntitiesCountVisibility = Visibility.Visible, Height = 50, width = 150, Type = "MostOverdueTheme" });
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "Most Overdue Responsible", Id = 4, BackColor = "Green", EntitiesCount = mostResp, EntitiesCountVisibility = Visibility.Visible, Height = 50, width = 150, Type = "MostOverdueResponsible" });

                // Botões de Status (To do, In progress, Blocked, Closed)
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "To do", Id = 0, BackColor = "Green", EntitiesCount = todoCount.ToString(), EntitiesCountVisibility = Visibility.Visible, Height = 50, width = 150, Type = "StatusToDo" });
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "In progress", Id = 0, BackColor = "Green", EntitiesCount = inProgressCount.ToString(), EntitiesCountVisibility = Visibility.Visible, Height = 50, width = 150, Type = "StatusInProgress" });
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "Blocked", Id = 0, BackColor = "Green", EntitiesCount = blockedCount.ToString(), EntitiesCountVisibility = Visibility.Visible, Height = 50, width = 150, Type = "StatusBlocked" });
                AlertListOfFilterTile.Add(new APMAlertTileBarFilters() { Caption = "Closed", Id = 0, BackColor = "Green", EntitiesCount = doneCount.ToString(), EntitiesCountVisibility = Visibility.Visible, Height = 50, width = 150, Type = "StatusClosed" });

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
            if (data == null || data.Count == 0) return;

            try
            {
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
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 60,
                    width = 230
                });

                // 2. Adicionar tiles por Theme
                foreach (var theme in themes.OrderBy(x => x.Position))
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
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 60,
                        width = 230
                    });
                }

                ListOfFilterTile = sideTiles;

                // Atualiza contagens com a heurística correta (tasks + subtasks, sem Done)
                UpdateSideTileCountsRespectingRules();

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
