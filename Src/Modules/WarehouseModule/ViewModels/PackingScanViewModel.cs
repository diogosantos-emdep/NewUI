using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common;
using System.Windows.Input;
using System.Windows;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Core;
using System.Collections.ObjectModel;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Warehouse.Views;
using System.Windows.Media.Imaging;
using System.IO;
using DevExpress.Xpf.Accordion;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Hardware;
using Emdep.Geos.Hardware.Balances;
using System.IO.Ports;
using Emdep.Geos.Hardware.Balances.Sartorius;
using System.Timers;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpo.DB;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Data.Common.SAM;
using DevExpress.Xpf.Grid;
using DevExpress.Data.Filtering;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class PackingScanViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region TaskLog
        // [001][GEOS2-1631] WMS packaging
        // [002][Sprint_78][GEOS2-1889][Add a new setting to specify the customers requiring one order one box]
        // [003][Sprint_78][GEOS2-1991][Show connection indicator for Weighing Machine]
        #endregion

        #region Services
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");

        #endregion

        #region Declaration
        private List<WorkflowTransition> workflowTransition;
        private List<WorkflowStatus> workflowStatusList;
        private double windowWidth;
        private double windowHeight;
        private Visibility isBoxesVisible = Visibility.Visible;
        private ObservableCollection<PackingBoxType> packingBoxTypeList;
        private ObservableCollection<WOItem> packedItemsList;
        private ObservableCollection<PackingCompany> packingCompanyList;
        private ObservableCollection<WOItem> unPackedItemsList;
        private object selectedItem;
        private bool focusUserControl;
        private bool isScanBox;
        private bool isAutoWeighing;
        private bool isScanFromPackedItems;
        private Visibility packingBoxDetailsVisibility;
        private bool isScanDirectionArrow;
        private WOItem selectedUnPackedItem;
        private Visibility isWrongItemErrorVisible = Visibility.Hidden;
        private string wrongItem;
        private string barcodeStr;
        private WOItem selectedPackedItem;
        private ImageSource articleImage;
        private double estimatedWeight = 00;
        private PackingBox selectedPackingBox;
        //  private List<int> customersIdList;
        private int totalPackingBoxCount;
        private List<Company> companyList;
        private bool isBoxWeightTolerance;
        private double boxWeightTolerance;
        private double scaleWeight = 00;
        private int totalItems;
        IBalance balance;
        Timer timer;
        private string scaleWeightValue = "0KG";
        private GeosAppSetting geosAppSetting;
        private Ots oT;
        private int stateIndex;
        private string toolTip;
        private string strWeighingIsOff = "Auto Weighing is OFF";
        private string MachineConnected = "Machine is connected";
        private string MachineNOTConnected = "Machine is NOT connected";
        private Int64 idOTUnPackedItem;
        private Int64 idOTPackedItem;
        private string infoTooltipBackColor;
        List<int> oneOrderOneBoxSiteIds;
        private bool isUnpackedSearchSelected;
        private string filterString;
        private string packedFilterString;
        //[pramod.misal][GEOS2-5792][20.06.2024]
        private Visibility isHeaderVisible = Visibility.Visible;
        private string selectedPlantHeader;//[Sudhir.Jangra][GEOS2-5705]
        private string ClonedSelectedPlantHeader { get; set; }//[Sudhir.Jangra][GEOS2-5705]
        private string selectedPlantTotalItem;//[Sudhir.Jangra][GEOS2-5705]
        private string selectedPlantTotalItemPercentage;//[Sudhir.jangra][GEOS2-5705]
        private string selectedPlantItemColor;//[Sudhir.Jangra][GEOS2-5705]
        private Visibility isBoxImageVisibleForRightGroup;//[Sudhir.jangra][GEOS2-5705]
        ObservableCollection<WOItem> otItemsListForLeftGroup;//[Sudhir.jangra][GEOS2-5705]
        private bool isCheckedToggleButton;
        private Visibility isToggleButtonVisible;
        public int NumberOfRecordsToShow { get; set; }
        public bool isNextButtonNeedToWork { get; set; }
        #endregion

        #region Properties
        ObservableCollection<WOItem> latestPackedItemsList;
        public ObservableCollection<WOItem> LatestPackedItemsList
        {
            get { return latestPackedItemsList; }
            set
            {
                latestPackedItemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LatestPackedItemsList"));
            }
        }

        public List<WorkflowStatus> WorkflowStatusList
        {
            get { return workflowStatusList; }
            set
            {
                workflowStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusList"));
            }
        }
        public List<WorkflowTransition> WorkflowTransitionList
        {
            get { return workflowTransition; }
            set
            {
                workflowTransition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransitionList"));
            }
        }
        public double WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowWidth"));
            }
        }

        public double WindowHeight
        {
            get { return windowHeight; }
            set
            {
                windowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight"));
            }
        }

        public Visibility IsBoxesVisible
        {
            get { return isBoxesVisible; }
            set
            {
                isBoxesVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBoxesVisible"));
            }
        }

        public ObservableCollection<PackingBoxType> PackingBoxTypeList
        {
            get { return packingBoxTypeList; }
            set
            {
                packingBoxTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackingBoxTypeList"));
            }
        }

        public ObservableCollection<WOItem> PackedItemsList
        {
            get { return packedItemsList; }
            set
            {
                packedItemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackedItemsList"));
            }
        }

        public ObservableCollection<PackingCompany> PackingCompanyList
        {
            get { return packingCompanyList; }
            set
            {
                packingCompanyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackingCompanyList"));
            }
        }

        public ObservableCollection<WOItem> UnPackedItemsList
        {
            get { return unPackedItemsList; }
            set
            {
                unPackedItemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UnPackedItemsList"));
            }
        }

        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        public bool FocusUserControl
        {
            get { return focusUserControl; }
            set
            {
                focusUserControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FocusUserControl"));
            }
        }

        public bool IsScanBox
        {
            get { return isScanBox; }
            set
            {
                isScanBox = value;
                FocusUserControl = true;
                SetBoxVisibilityTemplate();
                OnPropertyChanged(new PropertyChangedEventArgs("IsScanBox"));
            }
        }

        public bool IsAutoWeighing
        {
            get { return isAutoWeighing; }
            set
            {
                isAutoWeighing = value;

                if (isAutoWeighing)
                    GetScaleWeight();
                else
                {
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Enabled = false;
                        timer.Elapsed -= new ElapsedEventHandler(timer_Tick);

                    }
                    if (balance != null)
                    {
                        balance.Dispose();
                        balance = null;
                    }

                    StateIndex = 0;
                    ToolTip = strWeighingIsOff;
                    try
                    {
                        //Shubham[skadam]  GEOS2-8812 EWCN Scale support in WMS 1of 1 03 07 2025
                        GeosApplication.Instance.Logger.Log("Method GetScaleWeight... WeighingIsOff " + ToolTip, category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex) {     }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("IsAutoWeighing"));
            }
        }

        public bool IsScanFromPackedItems
        {
            get { return isScanFromPackedItems; }
            set
            {
                isScanFromPackedItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsScanFromPackedItems"));
            }
        }

        public Visibility PackingBoxDetailsVisibility
        {
            get { return packingBoxDetailsVisibility; }
            set
            {
                packingBoxDetailsVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackingBoxDetailsVisibility"));
            }
        }

        /// <summary>
        /// True -> when unpacked to packed
        /// False -> when packed to unpacked
        /// </summary>
        public bool IsScanDirectionArrow
        {
            get { return isScanDirectionArrow; }
            set
            {
                isScanDirectionArrow = value;
                FocusUserControl = true;
                SetErrorVisibilityTemplate();
                OnPropertyChanged(new PropertyChangedEventArgs("IsScanDirectionArrow"));

            }
        }

        public WOItem SelectedUnPackedItem
        {
            get { return selectedUnPackedItem; }
            set
            {
                selectedUnPackedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedUnPackedItem"));
            }
        }

        public Visibility IsWrongItemErrorVisible
        {
            get { return isWrongItemErrorVisible; }
            set
            {
                isWrongItemErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongItemErrorVisible"));
            }
        }

        public string WrongItem
        {
            get { return wrongItem; }
            set
            {
                wrongItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WrongItem"));
            }
        }

        public string BarcodeStr
        {
            get { return barcodeStr; }
            set
            {
                barcodeStr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BarcodeStr"));
            }
        }

        public WOItem SelectedPackedItem
        {
            get { return selectedPackedItem; }
            set
            {
                selectedPackedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPackedItem"));
            }
        }

        public ImageSource ArticleImage
        {
            get { return articleImage; }
            set
            {
                articleImage = value;
                if (articleImage == null)
                {
                    ArticleImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Warehouse;component/Assets/Images/ImageEditLogo.png"));
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleImage"));
            }
        }

        public double EstimatedWeight
        {
            get { return estimatedWeight; }
            set
            {
                estimatedWeight = value;
                CheckBoxWeightTolerance();
                OnPropertyChanged(new PropertyChangedEventArgs("EstimatedWeight"));
            }
        }

        public PackingBox SelectedPackingBox
        {
            get { return selectedPackingBox; }
            set
            {
                selectedPackingBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPackingBox"));
            }
        }

        //public List<int> CustomersIdList
        //{
        //    get { return customersIdList; }
        //    set
        //    {
        //        customersIdList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CustomersIdList"));
        //    }
        //}

        //public List<int> GeosAppSettingCustomerIdList
        //{
        //    get
        //    {
        //        return geosAppSettingCustomerIdList;
        //    }
        //    set
        //    {
        //        geosAppSettingCustomerIdList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingCustomerIdList"));
        //    }
        //}

        public int TotalPackingBoxCount
        {
            get { return totalPackingBoxCount; }
            set
            {
                totalPackingBoxCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPackingBoxCount"));
            }
        }

        public List<Company> CompanyList
        {
            get { return companyList; }
            set
            {
                companyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPackingBoxCount"));
            }
        }

        public bool IsBoxWeightTolerance
        {
            get { return isBoxWeightTolerance; }
            set
            {
                isBoxWeightTolerance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBoxWeightTolerance"));
            }
        }

        public double ScaleWeight
        {
            get { return scaleWeight; }
            set
            {
                scaleWeight = value;
                CheckBoxWeightTolerance();
                OnPropertyChanged(new PropertyChangedEventArgs("ScaleWeight"));
            }
        }

        public int TotalItems
        {
            get { return totalItems; }
            set
            {
                totalItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalItems"));
            }
        }

        Dictionary<string, string> PrintValues { get; set; }

        PrintLabel PrintLabel { get; set; }

        public string ScaleWeightValue
        {
            get { return scaleWeightValue; }
            set
            {
                scaleWeightValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScaleWeightValue"));
            }
        }

        public GeosAppSetting GeosAppSetting
        {
            get { return geosAppSetting; }
            set
            {
                geosAppSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSetting"));
            }
        }

        public Ots OT
        {
            get { return oT; }
            set
            {
                oT = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
            }
        }

        public int StateIndex
        {
            get { return stateIndex; }
            set
            {
                stateIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StateIndex"));
            }
        }

        public string ToolTip
        {
            get
            {
                return toolTip;
            }
            set
            {
                toolTip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTip"));
            }
        }
        public long IdOTUnPackedItem
        {
            get
            {
                return idOTUnPackedItem;
            }

            set
            {
                idOTUnPackedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTUnPackedItem"));
            }
        }
        public long IdOTPackedItem
        {
            get
            {
                return idOTPackedItem;
            }

            set
            {
                idOTPackedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTPackedItem"));
            }
        }
        public string InfoTooltipBackColor
        {
            get
            {
                return infoTooltipBackColor;
            }

            set
            {
                infoTooltipBackColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InfoTooltipBackColor"));
            }
        }

        public bool IsUnpackedSearchSelected
        {
            get { return isUnpackedSearchSelected; }
            set
            {
                isUnpackedSearchSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUnpackedSearchSelected"));
            }
        }

        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterString"));
            }
        }

        public string PackedFilterString
        {
            get { return packedFilterString; }
            set
            {
                packedFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackedFilterString"));
            }
        }

        bool isWorkOrder = false;
        public bool IsWorkOrder
        {
            get { return isWorkOrder; }
            set
            {
                isWorkOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWorkOrder"));
            }
        }

        ObservableCollection<WOItem> unPackedItemsListWorkOrder;
        public ObservableCollection<WOItem> UnPackedItemsListForWorkOrder
        {
            get { return unPackedItemsListWorkOrder; }
            set
            {
                unPackedItemsListWorkOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UnPackedItemsList"));
            }
        }

        ObservableCollection<WOItem> unPackedItemsListNew;
        public ObservableCollection<WOItem> UnPackedItemsListNew
        {
            get { return unPackedItemsListNew; }
            set
            {
                unPackedItemsListNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UnPackedItemsListNew"));
            }
        }

        string workOrder;
        public string WorkOrder
        {
            get { return workOrder; }
            set
            {
                workOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrder"));
            }
        }
        Int64 idOT;
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOT"));
            }
        }

        //[pramod.misal][GEOS2-5792][20.06.2024]
        public Visibility IsHeaderVisible
        {
            get { return isHeaderVisible; }
            set
            {
                isHeaderVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsHeaderVisible"));
            }
        }
        public bool IsCheckedToggleButton
        {
            get { return isCheckedToggleButton; }
            set
            {
                isCheckedToggleButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedToggleButton"));
            }
        }

        //[sudhir.Jangra][GEOs2-5705]
        public string SelectedPlantHeader
        {
            get { return selectedPlantHeader; }
            set
            {
                selectedPlantHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantHeader"));
            }
        }

        //[sudhir.Jangra][GEOs2-5705]
        public string SelectedPlantTotalItem
        {
            get { return selectedPlantTotalItem; }
            set
            {
                selectedPlantTotalItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantTotalItem"));
            }
        }

        //[sudhir.Jangra][GEOs2-5705]
        public string SelectedPlantTotalItemPercentage
        {
            get { return selectedPlantTotalItemPercentage; }
            set
            {
                selectedPlantTotalItemPercentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantTotalItemPercentage"));
            }
        }
        //[sudhir.Jangra][GEOs2-5705]
        public string SelectedPlantItemColor
        {
            get { return selectedPlantItemColor; }
            set
            {
                selectedPlantItemColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantItemColor"));
            }
        }

        //[Sudhir.jangra][GEOS2-5705]
        public Visibility IsBoxImageVisibleForRightGroup
        {
            get { return isBoxImageVisibleForRightGroup; }
            set
            {
                isBoxImageVisibleForRightGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBoxImageVisibleForRightGroup"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5705]
        public ObservableCollection<WOItem> OTItemListForLeftGroup
        {
            get { return otItemsListForLeftGroup; }
            set
            {
                otItemsListForLeftGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTItemListForLeftGroup"));
                UpdateVisibleRecords();
                if (OTItemListForLeftGroup.Count > 5)
                {
                    IsToggleButtonVisible = Visibility.Visible;
                }
                else
                {
                    IsToggleButtonVisible = Visibility.Collapsed;
                }
            }
        }

        private ObservableCollection<WOItem> _visibleRecords;

        public ObservableCollection<WOItem> VisibleRecords
        {
            get { return _visibleRecords; }
            set
            {
                _visibleRecords = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRecords"));

            }
        }

        private ObservableCollection<WOItem> topVisibleRecords;

        public ObservableCollection<WOItem> TopVisibleRecords
        {
            get { return topVisibleRecords; }
            set
            {
                topVisibleRecords = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TopVisibleRecords"));

            }

        }

        private int _currentRecordIndex;

        public int CurrentRecordIndex
        {
            get { return _currentRecordIndex; }
            set
            {
                if (_currentRecordIndex != value)
                {
                    _currentRecordIndex = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CurrentRecordIndex"));
                    UpdateVisibleRecords();
                }
            }
        }

        public Visibility IsToggleButtonVisible
        {
            get { return isToggleButtonVisible; }
            set
            {
                isToggleButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsToggleButtonVisible"));
            }
        }
        #endregion

        #region Events

        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events

        #region ICommands
        public ICommand CommandOnLoaded { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand HidePanelCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand WorkOrderHyperlinkClickCommand { get; set; }
        public ICommand ArticleReferenceHyperlinkClickCommand { get; set; }
        public ICommand AddButtonCommand { get; set; }
        public ICommand EditBoxCommand { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand ItemExpandedCommand { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand OpenCloseBoxCommand { get; set; }
        //public ICommand UnPackedItemImageClickCommand { get; set; }
        public ICommand UnPackedItemselectedCommand { get; set; }
        public ICommand PackedItemImageClickCommand { get; set; }
        public ICommand CommandKeyDown { get; set; }
        public ICommand HandleEnterKeyCommand { get; set; }
        public ICommand HandleLeaveKeyCommand { get; set; }
        public ICommand HideHeaderPanelCommand { get; set; } //[pramod.misal][26.06.2024][GEOS2-5705]
        public ICommand SlideDataForwardCommand { get; } //[pramod.misal][26.06.2024][GEOS2-5705]
        public ICommand SlideDataBackwardCommand { get; }  //[pramod.misal][26.06.2024][GEOS2-5705]

        #endregion

        #region Constructor
        public PackingScanViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PackingScanViewModel...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                GeosApplication.Instance.Logger.Log("Constructor PackingScanViewModel....", category: Category.Info, priority: Priority.Low);
                WindowHeight = System.Windows.SystemParameters.WorkArea.Height - 95;
                WindowWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                CommandOnLoaded = new DelegateCommand(LoadedAction);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                SelectedItemChangedCommand = new DelegateCommand(SelectedItemChangedCommandAction);
                WorkOrderHyperlinkClickCommand = new DelegateCommand<object>(WorkOrderHyperlinkClickCommandAction);
                ArticleReferenceHyperlinkClickCommand = new DelegateCommand<object>(ArticleReferenceHyperlinkClickCommandAction);
                AddButtonCommand = new RelayCommand(new Action<object>(AddNewBox));
                EditBoxCommand = new RelayCommand(new Action<object>(EditBoxCommandAction));
                CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
                ItemExpandedCommand = new DelegateCommand<AccordionItemExpandedEventArgs>(ItemExpandedCommandAction);
                DeleteButtonCommand = new DelegateCommand<object>(DeleteButtonCommandAction);
                OpenCloseBoxCommand = new DelegateCommand<object>(OpenCloseBoxCommandAction);
                PackedItemImageClickCommand = new DelegateCommand<object>(PackedItemImageClickCommandAction);       //ImageClick
                //UnPackedItemImageClickCommand = new DelegateCommand<object>(UnPackedItemImageClickCommandAction);   //ImageClick
                UnPackedItemselectedCommand = new DelegateCommand<object>(UnPackedItemselectedCommandAction);  //RowClick [rdixit][08.02.2023][GEOS2-3406]
                CommandKeyDown = new DelegateCommand<object>(CommandKeyDownAction);
                //[Sudhir.Jangra][GEOS2-5739]
                HandleEnterKeyCommand = new DelegateCommand<MouseEventArgs>(OnHandleEnterKey);
                HandleLeaveKeyCommand = new DelegateCommand<MouseEventArgs>(OnHandleLeaveKey);
                //[pramod.misal][26.06.2024][GEOS2-5705]
                HideHeaderPanelCommand = new RelayCommand(new Action<object>(HideHeaderPanel));
                SlideDataForwardCommand = new RelayCommand(new Action<object>(SlideDataForward));
                SlideDataBackwardCommand = new RelayCommand(new Action<object>(SlideDataBackward));
                CurrentRecordIndex = 0;
                PackingBoxDetailsVisibility = Visibility.Collapsed;
                IsScanDirectionArrow = true;
                WorkflowTransitionList = new List<WorkflowTransition>(WarehouseService.GetAllWorkflowTransitions_V2320());
                WorkflowStatusList = new List<WorkflowStatus>(WarehouseService.GetAllWorkflowStatus_V2320());
                // GetCustomersRequiremrntSettings();
                LatestPackedItemsList = new ObservableCollection<WOItem>();
                FilterString = string.Empty;
                PackedFilterString = string.Empty;
                IsWorkOrder = false;
                IsBoxImageVisibleForRightGroup = Visibility.Hidden;
                IsHeaderVisible = Visibility.Collapsed;
                IsCheckedToggleButton = false;
                //  NumberOfRecordsToShow = 5;



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor PackingScanViewModel....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PackingScanViewModel...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init(List<Company> list)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);
                CompanyList = list;
                List<Company> tempCompanyList = list.Where(x => x.ShortName != null).ToList();
                tempCompanyList = tempCompanyList.GroupBy(customer => customer.IdCompany).Select(group => group.First()).ToList();
                string idSites = string.Join(",", tempCompanyList.Select(i => i.IdCompany));
                //PackingCompanyList = new ObservableCollection<PackingCompany>(WarehouseService.GetCompanyPackingWorkOrders_V2240(WarehouseCommon.Instance.Selectedwarehouse, idSites));
                //WarehouseService = new WarehouseServiceController("localhost:6699");
                //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
                //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
                PackingCompanyList = new ObservableCollection<PackingCompany>(WarehouseService.GetCompanyPackingWorkOrders_V2400(WarehouseCommon.Instance.Selectedwarehouse, idSites));

                if (PackingCompanyList.Count > 0)
                    SelectedItem = PackingCompanyList.FirstOrDefault();

                IsScanBox = true;
                TotalPackingBoxCount = PackingCompanyList.Sum(x => x.PackingBoxes.Count);

                var GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("23");

                if (GeosAppSettingList.Count > 0)
                    boxWeightTolerance = Convert.ToDouble(GeosAppSettingList.Select(x => x.DefaultValue).FirstOrDefault());

                //set info tooltip back color
                GeosAppSetting GeosAppSetting = WorkbenchStartUp.GetGeosAppSettings(37);
                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;

                GeosAppSetting = WorkbenchStartUp.GetGeosAppSettings(34);
                oneOrderOneBoxSiteIds = GeosAppSetting.DefaultValue.Split(',').Select(Int32.Parse).ToList();

                GeosApplication.Instance.Logger.Log("Method Init executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void InitByidSites(List<Company> list)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);
                CompanyList = list;
                List<Company> tempCompanyList = list.Where(x => x.ShortName != null).ToList();
                tempCompanyList = tempCompanyList.GroupBy(customer => customer.IdCompany).Select(group => group.First()).ToList();
                string idSites = string.Join(",", tempCompanyList.Select(i => i.IdCompany));
                //Shubham[skadam] GEOS2-5704 Show the current packing % status (2/3)   24 06 2024
                //WarehouseService = new WarehouseServiceController("localhost:6699");
                PackingCompanyList = new ObservableCollection<PackingCompany>(WarehouseService.GetCompanyPackingWorkOrders_V2530(WarehouseCommon.Instance.Selectedwarehouse, idSites, null));

                if (PackingCompanyList.Count > 0)
                    SelectedItem = PackingCompanyList.FirstOrDefault();

                IsScanBox = true;
                TotalPackingBoxCount = PackingCompanyList.Sum(x => x.PackingBoxes.Count);

                var GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("23");

                if (GeosAppSettingList.Count > 0)
                    boxWeightTolerance = Convert.ToDouble(GeosAppSettingList.Select(x => x.DefaultValue).FirstOrDefault());

                //set info tooltip back color
                GeosAppSetting GeosAppSetting = WorkbenchStartUp.GetGeosAppSettings(37);
                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;

                GeosAppSetting = WorkbenchStartUp.GetGeosAppSettings(34);
                oneOrderOneBoxSiteIds = GeosAppSetting.DefaultValue.Split(',').Select(Int32.Parse).ToList();

                GeosApplication.Instance.Logger.Log("Method Init executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void InitByWorkOrder(string WorkOrderCode)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);
                //CompanyList = list;
                //List<Company> tempCompanyList = list.Where(x => x.ShortName != null).ToList();
                //tempCompanyList = tempCompanyList.GroupBy(customer => customer.IdCompany).Select(group => group.First()).ToList();
                //string idSites = string.Join(",", tempCompanyList.Select(i => i.IdCompany));

                //PackingCompanyList = new ObservableCollection<PackingCompany>(WarehouseService.GetCompanyPackingWorkOrders_V2240(WarehouseCommon.Instance.Selectedwarehouse, idSites));
                //WarehouseService = new WarehouseServiceController("localhost:6699");
                //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
                //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
                //Shubham[skadam] GEOS2-5704 Show the current packing % status (2/3)   24 06 2024
                //WarehouseService = new WarehouseServiceController("localhost:6699");
                PackingCompanyList = new ObservableCollection<PackingCompany>(WarehouseService.GetCompanyPackingWorkOrders_V2530(WarehouseCommon.Instance.Selectedwarehouse, null, WorkOrderCode));

                if (PackingCompanyList.Count > 0)
                    SelectedItem = PackingCompanyList.FirstOrDefault();

                IsScanBox = true;
                TotalPackingBoxCount = PackingCompanyList.Sum(x => x.PackingBoxes.Count);

                var GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("23");

                if (GeosAppSettingList.Count > 0)
                    boxWeightTolerance = Convert.ToDouble(GeosAppSettingList.Select(x => x.DefaultValue).FirstOrDefault());

                //set info tooltip back color
                GeosAppSetting GeosAppSetting = WorkbenchStartUp.GetGeosAppSettings(37);
                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;

                GeosAppSetting = WorkbenchStartUp.GetGeosAppSettings(34);
                oneOrderOneBoxSiteIds = GeosAppSetting.DefaultValue.Split(',').Select(Int32.Parse).ToList();

                GeosApplication.Instance.Logger.Log("Method Init executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void LoadedAction()
        {
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.PackingScanUserControlView", null, this);
        }

        private void CancelButtonCommandAction(object obj)
        {
            if (timer != null)
            {
                timer.Elapsed -= new ElapsedEventHandler(timer_Tick);
                timer.Enabled = false;
            }

            if (balance != null)
                balance.Dispose();
            //rajashri[GEOS2-4849][3-11-2023]
            //Added
            if (LatestPackedItemsList?.Count > 0)
            {
                foreach (var item in LatestPackedItemsList.Select(i => i.IdOT).Distinct())//foreach (var item in OriginalUnPackedItemsList)
                {
                    WorkflowStatus status = new WorkflowStatus();
                    LogEntriesByOT logEntriesByOT = new LogEntriesByOT();
                    List<LogEntriesByOT> LogEntriesByOTList = new List<LogEntriesByOT>();
                    //Service Updated from GetWorkOrderByIdOt_V2450 to GetWorkOrderByIdOt_V2460 by [rdixit][30.11.2023][GEOS2-5068]
                    OT = WarehouseService.GetWorkOrderByIdOt_V2460(item, WarehouseCommon.Instance.Selectedwarehouse);
                    WorkflowStatus OldWorkflowStatus = WorkflowStatusList.FirstOrDefault(i => i.IdWorkflowStatus == OT.IdWorkflowStatus);
                    if (OT.OtItems != null)
                    {
                        if (OT.OtItems.All(i => i.IdItemOtStatus == 9))
                        {
                            status = WorkflowStatusList.FirstOrDefault(i => i.Name.ToLower() == "packed");
                            if (status.IdWorkflowStatus != 51)
                            {

                                logEntriesByOT.IdOT = item;
                                logEntriesByOT.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                logEntriesByOT.Comments = string.Format(System.Windows.Application.Current.FindResource("WorkflowStatusLogEntryByPO").ToString(), OldWorkflowStatus.Name, status.Name);
                                logEntriesByOT.IdLogEntryType = 1;
                                logEntriesByOT.IsRtfText = false;
                                LogEntriesByOTList.Add(logEntriesByOT);
                                WarehouseService.UpdateWorkflowStatusInOT_V2450(item, status.IdWorkflowStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByOTList);
                            }
                        }
                        else if (OT.OtItems.Any(i => i.IdItemOtStatus == 9))
                        {
                            status = WorkflowStatusList.FirstOrDefault(i => i.Name.ToLower() == "packing");
                            if (status.IdWorkflowStatus != 50 && status.IdWorkflowStatus != 51)
                            {
                                if (OldWorkflowStatus.IdWorkflowStatus == 48)
                                {
                                    logEntriesByOT.IdOT = item;
                                    logEntriesByOT.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                    logEntriesByOT.Comments = string.Format(System.Windows.Application.Current.FindResource("WorkflowStatusLogEntryByPO").ToString(), OldWorkflowStatus.Name, status.Name);
                                    logEntriesByOT.IdLogEntryType = 1;
                                    logEntriesByOT.IsRtfText = false;
                                    LogEntriesByOTList.Add(logEntriesByOT);
                                }
                                WarehouseService.UpdateWorkflowStatusInOT_V2450(item, status.IdWorkflowStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByOTList);
                            }
                        }
                    }
                }
            }
            //End
            RequestClose(null, null);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private void HidePanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);

                if (IsBoxesVisible == Visibility.Collapsed)
                    IsBoxesVisible = Visibility.Visible;
                else
                    IsBoxesVisible = Visibility.Collapsed;

                FocusUserControl = true;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectedItemChangedCommandAction()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction...", category: Category.Info, priority: Priority.Low);

                //if (SelectedItem is PackingCompany)
                //{
                //    PackingCompany tempPackingCompany = (PackingCompany)SelectedItem;
                //    UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany));
                //    PackingBoxDetailsVisibility = Visibility.Collapsed;
                //    if (SelectedPackedItem != null)
                //        SelectedPackedItem = null;
                //}

                if (SelectedItem is PackingBox)
                {
                    if (!DXSplashScreen.IsActive)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            Window win = new Window()
                            {
                                ShowActivated = false,
                                WindowStyle = WindowStyle.None,
                                ResizeMode = ResizeMode.NoResize,
                                AllowsTransparency = true,
                                Background = new SolidColorBrush(Colors.Transparent),
                                ShowInTaskbar = false,
                                Topmost = true,
                                SizeToContent = SizeToContent.WidthAndHeight,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    if (((PackingBox)SelectedItem).IsClosed == 0)
                        PackingBoxDetailsVisibility = Visibility.Visible;
                    else if (((PackingBox)SelectedItem).IsClosed == 1)
                        PackingBoxDetailsVisibility = Visibility.Collapsed;

                    //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2039(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite));
                    //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2051(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite));
                    //WarehouseService = new WarehouseServiceController("localhost:6699");
                    //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
                    //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
                    //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2400(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite, ((PackingBox)SelectedItem).IdCountryGroup));
                    //Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
                    //WarehouseService = new WarehouseServiceController("localhost:6699");
                    UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2530(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite, ((PackingBox)SelectedItem).IdCountryGroup));
                    if (IsWorkOrder)
                    {
                        try
                        {
                            UnPackedItemsListNew = new ObservableCollection<WOItem>();
                            //UnPackedItemsListNew.AddRange(UnPackedItemsList);
                            foreach (WOItem unPackedItem in UnPackedItemsList)
                            {
                                if (IdOT == unPackedItem.IdOT)
                                {
                                    UnPackedItemsListNew.Add(unPackedItem);
                                }
                            }
                            UnPackedItemsList = new ObservableCollection<WOItem>();
                            UnPackedItemsList.AddRange(UnPackedItemsListNew);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2530(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite, ((PackingBox)SelectedItem).IdCountryGroup));
                    //[Sudhir.Jangra][GEOS2-5740]
                    // UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2530(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite, ((PackingBox)SelectedItem).IdCountryGroup));

                    //PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2039(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox));

                    //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
                    //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
                    //PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2400(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox));
                    //Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
                    //WarehouseService = new WarehouseServiceController("localhost:6699");
                    PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2530(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox));
                    //[Sudhir.jangra][GEOS2-5740]
                    // PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2530(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox));



                    SelectedPackingBox = (PackingBox)SelectedItem;
                    SetBoxVisibilityTemplate();

                    if (IsAutoWeighing)
                    {
                        if (balance != null)
                        {
                            balance.Dispose();
                            balance = null;
                            GetScaleWeight();
                        }
                    }

                    CalculateAndDisplayEstimatedWeight();
                    ToolTip = strWeighingIsOff;
                }

                List<WOItem> combinedList = UnPackedItemsList.Concat(PackedItemsList).ToList();

                // OTItemListForLeftGroup=new ObservableCollection<WOItem>(combinedList.GroupBy(x => x.WorkOrder).Select(a => new { WorkOrder = a.Key, TotalWorkOrderCount = a.Count() }).ToList());
                var workOrderCounts = combinedList.GroupBy(x => x.WorkOrder).Select(group =>
                {
                    var totalCount = group.Count();
                    var unPackedCount = UnPackedItemsList.Count(item => item.WorkOrder == group.Key);
                    var PackedCount = PackedItemsList.Count(item => item.WorkOrder == group.Key);
                    int totalItems = unPackedCount + PackedCount;
                    var percentage = (double)PackedCount / totalItems * 100;
                    int roundedPercentage = (int)Math.Round(percentage);

                    return new WOItem
                    {
                        WorkOrder = group.Key,
                        TotalWorkOrderCount = totalCount,
                        UnpackedCount = unPackedCount,
                        PackedCount = PackedCount,
                        TotalItemCounted = $"{unPackedCount}/{totalCount}",
                        TotalCountPercentage = roundedPercentage + "%",
                        TotalCountColor = percentage == 0 ? "red" : (percentage == 100 ? "green" : "orange")
                    };
                });

                OTItemListForLeftGroup = new ObservableCollection<WOItem>(workOrderCounts);


                if (OTItemListForLeftGroup != null)
                {
                    //[Sudhir.Jangra][GEOS2-5705]
                    int packedItemCount = 0;
                    int unpackedItemCount = 0;


                    foreach (var item in OTItemListForLeftGroup)
                    {
                        packedItemCount += (int)item.PackedCount;
                        unpackedItemCount += (int)item.UnpackedCount;
                    }


                    int TotalItems = packedItemCount + unpackedItemCount;
                    if (TotalItems != 0)
                    {
                        double percentage1 = ((double)packedItemCount / TotalItems) * 100;
                        //  double percentage = ((double)unpackedItemCount / packedItemCount) * 100;
                        int roundedPercentage1 = (int)Math.Round(percentage1);
                        SelectedPlantTotalItem = unpackedItemCount + "/" + TotalItems;
                        SelectedPlantTotalItemPercentage = roundedPercentage1 + "%";
                        if (roundedPercentage1 == 0)
                        {
                            SelectedPlantItemColor = "Red";
                        }
                        else if (roundedPercentage1 > 0 && roundedPercentage1 != 100)
                        {
                            SelectedPlantItemColor = "Orange";
                        }
                        else if (roundedPercentage1 == 100)
                        {
                            SelectedPlantItemColor = "Green";
                        }
                        IsHeaderVisible = Visibility.Visible;
                        IsBoxImageVisibleForRightGroup = Visibility.Visible;
                        IsCheckedToggleButton = true;
                        SelectedPlantHeader = ClonedSelectedPlantHeader;
                    }
                    else
                    {
                        SelectedPlantTotalItem = string.Empty;
                        SelectedPlantTotalItemPercentage = string.Empty;
                        SelectedPlantItemColor = string.Empty;
                        IsHeaderVisible = Visibility.Collapsed;
                        IsBoxImageVisibleForRightGroup = Visibility.Hidden;
                        IsCheckedToggleButton = false;
                        OTItemListForLeftGroup = new ObservableCollection<WOItem>();
                        SelectedPlantHeader = string.Empty;
                    }
                }



                //[pramod.misal][26.06.2024][GEOS2-5705]
                VisibleRecords = new ObservableCollection<WOItem>();
                CurrentRecordIndex = 0; // Start at the first record
                UpdateVisibleRecords();
                FilterString = string.Empty;
                PackedFilterString = string.Empty;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SelectedItemChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UpdateVisibleRecords()
        {
            if (VisibleRecords == null)
                VisibleRecords = new ObservableCollection<WOItem>();
            // Take 5 items for TopThreeVisibleRecords
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            VisibleRecords.Clear();

            if (OTItemListForLeftGroup != null && OTItemListForLeftGroup.Count > 0)
            {
                // Calculate the number of items to display
                int itemCount = Math.Min(NumberOfRecordsToShow, OTItemListForLeftGroup.Count - CurrentRecordIndex);

                for (int i = CurrentRecordIndex; i < CurrentRecordIndex + itemCount; i++)
                {
                    VisibleRecords.Add(OTItemListForLeftGroup[i]);
                }



                if (screenWidth == 1920)
                {
                    TopVisibleRecords = new ObservableCollection<WOItem>(VisibleRecords.Take(5));
                    NumberOfRecordsToShow = 5;
                }
                else if (screenWidth == 1536)
                {
                    TopVisibleRecords = new ObservableCollection<WOItem>(VisibleRecords.Take(3));
                    NumberOfRecordsToShow = 3;
                }
                else if (screenWidth == 1280 || screenWidth >= 1090 || screenWidth <= 1095)
                {
                    TopVisibleRecords = new ObservableCollection<WOItem>(VisibleRecords.Take(2));
                    NumberOfRecordsToShow = 2;
                }
                else
                {
                    TopVisibleRecords = new ObservableCollection<WOItem>(VisibleRecords.Take(1));
                    NumberOfRecordsToShow = 1;
                }


            }
            else
            {
                TopVisibleRecords = new ObservableCollection<WOItem>();
            }

            if (TopVisibleRecords != null)
            {
                if (TopVisibleRecords.Count == 1 || TopVisibleRecords.Count == 2 || TopVisibleRecords.Count == 3 || TopVisibleRecords.Count == 4)
                {
                    if (screenWidth == 1536 && OTItemListForLeftGroup.Count == 3 || OTItemListForLeftGroup.Count == 4)
                    {
                        if (OTItemListForLeftGroup.Count == 3)
                        {
                            isNextButtonNeedToWork = false;
                            IsToggleButtonVisible = Visibility.Visible;
                        }
                        else if (OTItemListForLeftGroup.Count == 4)
                        {
                            isNextButtonNeedToWork = false;
                            IsToggleButtonVisible = Visibility.Visible;
                        }

                    }
                    else if ((screenWidth == 1280 || screenWidth == 1093) && OTItemListForLeftGroup.Count == 2)
                    {
                        isNextButtonNeedToWork = false;
                        IsToggleButtonVisible = Visibility.Visible;
                    }
                    else
                    {
                        
                        isNextButtonNeedToWork = true;
                        if (TopVisibleRecords.Count == OTItemListForLeftGroup.Count)
                        {
                            IsToggleButtonVisible = Visibility.Collapsed;
                        }
                        else
                        {
                            IsToggleButtonVisible = Visibility.Visible;
                        }

                    }
                }
                else
                {

                    if (TopVisibleRecords.Count == OTItemListForLeftGroup.Count)
                    {
                        IsToggleButtonVisible = Visibility.Collapsed;
                        isNextButtonNeedToWork = true;
                    }
                    else
                    {
                        IsToggleButtonVisible = Visibility.Visible;
                        isNextButtonNeedToWork = false;
                    }
                }
            }

        }


        private void CalculateAndDisplayEstimatedWeight()
        {
            double calculatedEstimatedWeight = 0;
            StringBuilder calculatedEstimatedWeightExpression = new StringBuilder();
            StringBuilder calculationLog = new StringBuilder();

            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateAndDisplayEstimatedWeight...", category: Category.Info, priority: Priority.Low);
                calculatedEstimatedWeightExpression.Append($"{Environment.NewLine}=");

                if (SelectedItem != null && SelectedItem is PackingBox && PackedItemsList != null)
                {
                    calculationLog.Append($"{Environment.NewLine}");
                    calculationLog.Append($"{Environment.NewLine}Box ID={((PackingBox)SelectedItem).IdPackingBox}, " +
                            $"Name={((PackingBox)SelectedItem).BoxNumber}, " +
                            $"Net Weight={((PackingBox)SelectedItem).NetWeight} {((PackingBox)SelectedItem).WeightMeasurementUnit}");
                    calculatedEstimatedWeight += ((PackingBox)SelectedItem).NetWeight;
                    calculatedEstimatedWeightExpression.Append($"{((PackingBox)SelectedItem).NetWeight.ToString()}");

                    foreach (var item in PackedItemsList)
                    {
                        calculationLog.Append($"{Environment.NewLine}Packed Article ID={item.IdArticle}, " +
                            $"Reference={item.Reference}, Weight={item.ArticleWeight}, Current Qty={item.Qty}, " +
                            $"OriginalQty={item.OriginalQty}");

                        calculatedEstimatedWeight += (item.ArticleWeight * item.Qty);
                        calculatedEstimatedWeightExpression.Append($"+({item.ArticleWeight.ToString()}*{item.Qty.ToString()})");
                    }
                    EstimatedWeight = calculatedEstimatedWeight;
                    // EstimatedWeight = ((PackingBox)SelectedItem).NetWeight + PackedItemsList.Sum(x => x.ArticleWeight * x.Qty);
                }
                else
                {
                    EstimatedWeight = 0;
                }

                calculationLog.Append(calculatedEstimatedWeightExpression);
                calculationLog.Append($"{Environment.NewLine}EstimatedWeight={calculatedEstimatedWeight}");
                calculationLog.Append($"{Environment.NewLine}");

                GeosApplication.Instance.Logger.Log($"{calculationLog}", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.Logger.Log("Method CalculateAndDisplayEstimatedWeight executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CalculateAndDisplayEstimatedWeight() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to open Work Order details view
        /// </summary>
        /// <param name="obj"></param>
        private void WorkOrderHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                WOItem unPackedItem = (WOItem)obj;
                WorkOrderItemDetailsViewModel workOrderItemDetailsViewModel = new WorkOrderItemDetailsViewModel();
                WorkOrderItemDetailsView workOrderItemDetailsView = new WorkOrderItemDetailsView();
                EventHandler handle = delegate { workOrderItemDetailsView.Close(); };
                workOrderItemDetailsViewModel.RequestClose += handle;
                workOrderItemDetailsViewModel.OtSite = WarehouseCommon.Instance.Selectedwarehouse.Company;//[Sudhir.Jangra][GEOS2-5644]
                workOrderItemDetailsViewModel.Init(unPackedItem.IdOT, WarehouseCommon.Instance.Selectedwarehouse);
                workOrderItemDetailsView.DataContext = workOrderItemDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                workOrderItemDetailsView.ShowDialogWindow();
                FocusUserControl = true;
                GeosApplication.Instance.Logger.Log("Method HyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method HyperlinkClickCommandAction...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to open Article Details.
        /// </summary>
        /// <param name="obj"></param>
        private void ArticleReferenceHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleReferenceHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                WOItem article = (WOItem)obj;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                articleDetailsViewModel.Init(article.Reference, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                articleDetailsView.ShowDialog();

                if (articleDetailsViewModel.IsResult)
                {
                    if (articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage || articleDetailsViewModel.UpdateArticle.IsAddedArticleImage)
                    {
                        foreach (var item in UnPackedItemsList)
                        {
                            if (item.IdArticle == article.IdArticle)
                                item.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        }

                        foreach (var item in PackedItemsList)
                        {
                            if (item.IdArticle == article.IdArticle)
                                item.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        }

                        if (SelectedPackedItem != null)
                        {
                            if (article.IdArticle == SelectedPackedItem.IdArticle)
                                ArticleImage = ByteArrayToImage(articleDetailsViewModel.UpdateArticle.ArticleImageInBytes);
                        }
                    }
                }

                FocusUserControl = true;
                GeosApplication.Instance.Logger.Log("Method ArticleReferenceHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ArticleReferenceHyperlinkClickCommandAction...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewBox(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewBox....", category: Category.Info, priority: Priority.Low);
                NewBoxView newBoxView = new NewBoxView();
                NewBoxViewModel newBoxViewModel = new NewBoxViewModel();
                EventHandler handler = delegate { newBoxView.Close(); };
                newBoxViewModel.RequestClose += handler;
                newBoxView.DataContext = newBoxViewModel;

                newBoxViewModel.WindowHeader = System.Windows.Application.Current.FindResource("NewBox").ToString();
                newBoxViewModel.IsOpenCloseButtonVisibile = Visibility.Collapsed;
                newBoxViewModel.isNew = true;
                newBoxViewModel.Init(CompanyList, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                newBoxView.ShowDialogWindow();
                if (newBoxViewModel.IsSave)
                {
                    if (PackingCompanyList.Any(x => x.IdCompany == newBoxViewModel.NewPackingBox.IdSite))
                    {
                        SelectedItem = PackingCompanyList.Where(x => x.IdCompany == newBoxViewModel.NewPackingBox.IdSite).FirstOrDefault();//[rdixit][GEOS2-3816][15.07.2022][Updated SelectedItem value as per newBoxViewModel]
                        if (SelectedItem is PackingCompany)
                        {
                            PackingCompany tempPackingCompany = (PackingCompany)SelectedItem;
                            tempPackingCompany.PackingBoxes.Add(newBoxViewModel.NewPackingBox);
                            SelectedItem = newBoxViewModel.NewPackingBox;
                        }
                        else if (SelectedItem is PackingBox)
                        {
                            PackingCompany tempPackingCompany = PackingCompanyList.FirstOrDefault(x => x.IdCompany == newBoxViewModel.NewPackingBox.IdSite);
                            tempPackingCompany.PackingBoxes.Add(newBoxViewModel.NewPackingBox);
                            SelectedItem = newBoxViewModel.NewPackingBox;
                        }
                    }
                    else
                    {
                        PackingCompany packingCompany = new PackingCompany();
                        packingCompany.IdCompany = newBoxViewModel.CustomersList[newBoxViewModel.SelectedCustomerIndex].IdCompany;
                        packingCompany.ShortName = newBoxViewModel.CustomersList[newBoxViewModel.SelectedCustomerIndex].ShortName;
                        packingCompany.Name = newBoxViewModel.CustomersList[newBoxViewModel.SelectedCustomerIndex].Name;

                        packingCompany.PackingBoxes = new ObservableCollection<PackingBox>();
                        packingCompany.PackingBoxes.Add(newBoxViewModel.NewPackingBox);
                        PackingCompanyList.Add(packingCompany);
                        SelectedItem = newBoxViewModel.NewPackingBox;
                    }

                    TotalPackingBoxCount = PackingCompanyList.Sum(x => x.PackingBoxes.Count);
                    FilterString = string.Empty;
                    PackedFilterString = string.Empty;
                }
                GeosApplication.Instance.Logger.Log("Method AddNewBox....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewBox...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][avpawar][GEOS2-3404][Display the number of packed items INSTEAD of packed quantity in the BOX in Packing]
        /// </summary>
        /// <param name="obj"></param>
        private void EditBoxCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditBoxCommandAction....", category: Category.Info, priority: Priority.Low);
                if (SelectedItem is PackingBox)
                {
                    NewBoxView newBoxView = new NewBoxView();
                    EditBoxViewModel editBoxViewModel = new EditBoxViewModel();
                    EventHandler handler = delegate { newBoxView.Close(); };
                    editBoxViewModel.RequestClose += handler;
                    newBoxView.DataContext = editBoxViewModel;
                    editBoxViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditBox").ToString();
                    editBoxViewModel.IsOpenCloseButtonVisibile = Visibility.Visible;
                    editBoxViewModel.Init((PackingBox)SelectedItem, CompanyList, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                    newBoxView.ShowDialogWindow();

                    if (editBoxViewModel.IsResult)
                    {
                        PackingBox tempPackingBox = (PackingBox)SelectedItem;
                        tempPackingBox.BoxNumber = editBoxViewModel.UpdatePackingBox.BoxNumber;
                        tempPackingBox.IdPackingBoxType = editBoxViewModel.UpdatePackingBox.IdPackingBoxType;
                        tempPackingBox.Length = editBoxViewModel.UpdatePackingBox.Length;
                        tempPackingBox.Width = editBoxViewModel.UpdatePackingBox.Width;
                        tempPackingBox.Height = editBoxViewModel.UpdatePackingBox.Height;
                        tempPackingBox.NetWeight = editBoxViewModel.UpdatePackingBox.NetWeight;
                        tempPackingBox.GrossWeight = editBoxViewModel.UpdatePackingBox.GrossWeight;
                        tempPackingBox.IsClosed = editBoxViewModel.UpdatePackingBox.IsClosed;
                        tempPackingBox.IdCountryGroup = editBoxViewModel.UpdatePackingBox.IdCountryGroup;
                        tempPackingBox.CountryGroup = editBoxViewModel.UpdatePackingBox.CountryGroup;
                        tempPackingBox.IsVisibleCountryGroup = editBoxViewModel.UpdatePackingBox.IsVisibleCountryGroup;
                        tempPackingBox.IsStackable = editBoxViewModel.UpdatePackingBox.IsStackable;
                        if (tempPackingBox.IdSite == editBoxViewModel.UpdatePackingBox.IdSite)
                        {
                            SelectedItem = editBoxViewModel.UpdatePackingBox;
                        }
                        else
                        {
                            PackingCompany company = PackingCompanyList.FirstOrDefault(x => x.IdCompany == tempPackingBox.IdSite);
                            company.PackingBoxes.Remove(tempPackingBox);
                            if (PackingCompanyList.Any(x => x.IdCompany == editBoxViewModel.UpdatePackingBox.IdSite))
                            {
                                PackingCompany packingCompany = PackingCompanyList.FirstOrDefault(x => x.IdCompany == editBoxViewModel.UpdatePackingBox.IdSite);
                                packingCompany.PackingBoxes.Add(editBoxViewModel.UpdatePackingBox);
                                SelectedItem = editBoxViewModel.UpdatePackingBox;

                                //[001] Start
                                // ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Count;
                                //[001] End
                            }
                            else
                            {
                                PackingCompany packingCompany = new PackingCompany();
                                packingCompany.IdCompany = editBoxViewModel.CustomersList[editBoxViewModel.SelectedCustomerIndex].IdCompany;
                                packingCompany.ShortName = editBoxViewModel.CustomersList[editBoxViewModel.SelectedCustomerIndex].ShortName;
                                packingCompany.Name = editBoxViewModel.CustomersList[editBoxViewModel.SelectedCustomerIndex].Name;
                                packingCompany.PackingBoxes = new ObservableCollection<PackingBox>();
                                packingCompany.PackingBoxes.Add(editBoxViewModel.UpdatePackingBox);
                                PackingCompanyList.Add(packingCompany);
                                SelectedItem = editBoxViewModel.UpdatePackingBox;

                                UpdateItemsInBoxCount();
                                ////[001] start
                                ////((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                //((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Count;
                                ////[001] End
                            }
                        }
                        FilterString = string.Empty;
                        PackedFilterString = string.Empty;
                    }

                    // FocusUserControl = true;
                }

                GeosApplication.Instance.Logger.Log("Method EditBoxCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditBoxCommandAction...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private bool ErrorPleaseOpenTheBoxBeforeScanningItems()
        {
            bool result = false;

            if (SelectedItem != null && SelectedItem is PackingBox)
            {
                PackingBox editPackingBox = (PackingBox)SelectedItem;

                if (IsScanBox == false && editPackingBox.IsClosed == 1)
                {
                    WrongItem = "Please open the box before scanning items.";
                    IsWrongItemErrorVisible = Visibility.Visible;
                    BarcodeStr = "";
                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// [001][avpawar][3407][It MUST be NOT possible to scan an item if the “Selected” box is CLOSED. ONLY we will allow to scan an item (pack or unpack) if the selected box is Open]
        /// [002][avpawar][GEOS2-3404][Display the number of packed items INSTEAD of packed quantity in the BOX in Packing]
        /// [003][cpatil][GEOS2-5513][17-05-2024]
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
                if (!IsUnpackedSearchSelected)
                {
                    if (obj.Text == "\r")
                    {
                        string _partNumberCode = string.Empty;
                        Int64 _qty = 0;
                        int _barcodeLength = BarcodeStr.Length;

                        //[001] start
                        if (ErrorPleaseOpenTheBoxBeforeScanningItems())
                            return;
                        //[001] End

                        var isProperBarcode = !string.IsNullOrEmpty(BarcodeStr);
                        if (IsScanBox)
                        {
                            ProcessScanBoxOption();
                            return;
                        }

                        if (SelectedItem is PackingBox && !IsScanBox)
                        {
                            if (isProperBarcode && BarcodeStr.Length >= 17)
                            {
                                bool result = long.TryParse(BarcodeStr.Substring(BarcodeStr.Length - 6), out _qty);
                                if (!result)
                                {
                                    SetErrorTemplate();
                                    return;
                                }
                                // _qty = Convert.ToInt64(BarcodeStr.Substring(BarcodeStr.Length - 6));
                                _partNumberCode = BarcodeStr.Substring(0, (_barcodeLength - 6));
                            }

                            if (IsScanDirectionArrow)
                            {
                                if (isProperBarcode && BarcodeStr.Length >= 17)
                                {

                                    WOItem unPackedItem = UnPackedItemsList.FirstOrDefault(x => x.PartNumberCode.Substring(0, (x.PartNumberCode.Length - 3)).ToUpperInvariant() == _partNumberCode.ToUpperInvariant());
                                    if (unPackedItem == null)
                                    {
                                        SetErrorTemplate();
                                        return;
                                    }

                                    bool selectedItemIsValidForPacking = OneOrderOneBoxValidation(unPackedItem);
                                    if (!selectedItemIsValidForPacking)
                                    {
                                        OneOrderOneBoxErrorTemplate();       //[002] Added
                                        return;
                                    }
                                    //[cpatil][GEOS2-5513][17-05-2024]
                                    string stageName = WarehouseService.GetMandatoryStageNameOpenIfExist(unPackedItem.IdPartNumber, WarehouseCommon.Instance.Selectedwarehouse);
                                    if (!string.IsNullOrEmpty(stageName))
                                    {
                                        WrongItem = string.Format(Application.Current.FindResource("OpenMandatoryStageErrorMsg").ToString(), stageName);
                                        IsWrongItemErrorVisible = Visibility.Visible;
                                        BarcodeStr = "";
                                        UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                        PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);   // [003] Added
                                        return;
                                    }

                                    #region GEOS2-4421&GEOS2-4422
                                    //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
                                    //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
                                    try
                                    {
                                        if (SelectedPackingBox != null)
                                        {
                                            if (SelectedPackingBox.ItemsInBox > 0 && SelectedPackingBox.ItemsInBox != 0)
                                            {
                                                if (SelectedPackingBox.IdCarriageMethod != unPackedItem.IdCarriageMethod)
                                                {
                                                    SelectedUnPackedItem = unPackedItem;
                                                    //string WrongItemMessage = System.Windows.Application.Current.FindResource("PackingBox_ShippingError").ToString();
                                                    string WrongItemMessage = System.Windows.Application.Current.FindResource("PackingBox_ShippingErrorNew").ToString();//Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
                                                    SetErrorTemplateForCarriageMethod(WrongItemMessage);
                                                    return;
                                                }
                                            }

                                            if (SelectedPackingBox.IdCountryGroup > 0 && SelectedPackingBox.IdCountryGroup != null)
                                            {
                                                if (SelectedPackingBox.IdCountryGroup != unPackedItem.IdCountryGroup)
                                                {
                                                    string WrongItemMessage = System.Windows.Application.Current.FindResource("WMSCountryGroupErrorMessageForScanning").ToString();
                                                    //if (SelectedPackingBox.IdCountryGroup==1)
                                                    //{
                                                    //    WrongItemMessage = System.Windows.Application.Current.FindResource("WMSCountryGroupErrorMessageEURO").ToString();//Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
                                                    //}
                                                    //if (SelectedPackingBox.IdCountryGroup == 2)
                                                    //{
                                                    //    WrongItemMessage = System.Windows.Application.Current.FindResource("WMSCountryGroupErrorMessageNOTEURO").ToString();//Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
                                                    //}
                                                    //else
                                                    //{
                                                    //    WrongItemMessage = System.Windows.Application.Current.FindResource("WMSCountryGroupErrorMessageNew").ToString();//Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
                                                    //}
                                                    SetErrorTemplateForCarriageMethod(string.Format(WrongItemMessage, unPackedItem.CountryGroup.Name, SelectedPackingBox.CountryGroup.Name));
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    #endregion
                                    #region Commented
                                    //if (PackedItemsList != null && PackedItemsList.Count >= 1)      //[002] start
                                    //{
                                    //    if (oneOrderOneBoxSiteIds != null && oneOrderOneBoxSiteIds.Any(x => x == ((PackingBox)SelectedItem).IdSite))     //checking idSite with GeosAppSettingFile
                                    //    {
                                    //        WOItem packedItem = (WOItem)unPackedItem.Clone();

                                    //        if (PackedItemsList[0].IdOffer == unPackedItem.IdOffer)
                                    //        {
                                    //            PackedItemsList.Add(packedItem);
                                    //            UnPackedItemsList.Remove(UnPackedItemsList.FirstOrDefault(x => x.IdArticle == packedItem.IdArticle));

                                    //            bool result = WarehouseService.UpdatePackingBoxInPartnumber_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedPackingBox.IdPackingBox, packedItem.IdOTItem);

                                    //            if (result)
                                    //            {
                                    //                bool isOTItemStatus = WarehouseService.UpdateOTItemStatus_V2035(WarehouseCommon.Instance.Selectedwarehouse, packedItem.IdOTItem, GeosApplication.Instance.ActiveUser.IdUser);
                                    //            }

                                    //             //[002] start
                                    //             //((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                    //             ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Count;
                                    //            //[002] End

                                    //            //show comment
                                    //            UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    //            PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    //            if (SelectedPackedItem.ArticleComment != null)
                                    //            {
                                    //                if (SelectedPackedItem.ArticleCommentDateOfExpiry == null)
                                    //                {
                                    //                    SelectedPackedItem.ShowComment = true;
                                    //                }
                                    //                else if (SelectedPackedItem.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                                    //                {
                                    //                    SelectedPackedItem.ShowComment = true;
                                    //                }

                                    //            }
                                    //            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                    //        }
                                    //        else
                                    //        {
                                    //            //SetErrorTemplate();
                                    //            OneOrderOneBoxErrorTemplate();       //[002] Added
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (unPackedItem != null)
                                    //        {
                                    //            WOItem packedItem = (WOItem)unPackedItem.Clone();

                                    //            if (_qty <= packedItem.Qty)
                                    //            {
                                    //                if (_qty == unPackedItem.Qty)
                                    //                {
                                    //                    UnPackedItemsList.Remove(unPackedItem);
                                    //                    SelectedUnPackedItem = null;
                                    //                    unPackedItem.Qty = unPackedItem.Qty - _qty;
                                    //                }
                                    //                else if (_qty < unPackedItem.Qty)
                                    //                {
                                    //                    unPackedItem.Qty = unPackedItem.Qty - _qty;
                                    //                    SelectedUnPackedItem = unPackedItem;
                                    //                }

                                    //                //if (CustomersIdList.Contains(((PackingBox)SelectedItem).IdSite))
                                    //                //{
                                    //                //    if (PackedItemsList.Count == 0)
                                    //                //    {
                                    //                //        if (unPackedItem.Qty == 0)
                                    //                //        {
                                    //                //            PackedItemsList.Add(packedItem);
                                    //                //            SelectedPackedItem = packedItem;
                                    //                //            SelectedPackedItem.Qty = packedItem.OriginalQty;
                                    //                //            EstimatedWeight = EstimatedWeight + packedItem.ArticleWeight * packedItem.OriginalQty;
                                    //                //        }
                                    //                //    }
                                    //                //    else
                                    //                //    {
                                    //                //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomersRequiringOneBoxOneWorkOrderMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    //                //        return;
                                    //                //    }
                                    //                //}
                                    //                //else
                                    //                //{
                                    //                    if (unPackedItem.Qty == 0)
                                    //                    {
                                    //                        PackedItemsList.Add(packedItem);
                                    //                        SelectedPackedItem = packedItem;
                                    //                        SelectedPackedItem.Qty = packedItem.OriginalQty;
                                    //                        EstimatedWeight = EstimatedWeight + packedItem.ArticleWeight * packedItem.OriginalQty;
                                    //                    }
                                    //                //}

                                    //                if (SelectedPackedItem != null)
                                    //                    ArticleImage = ByteArrayToImage(SelectedPackedItem.ArticleImageInBytes);

                                    //                if (unPackedItem.Qty == 0)
                                    //                {
                                    //                    bool result = WarehouseService.UpdatePackingBoxInPartnumber_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedPackingBox.IdPackingBox, packedItem.IdOTItem);
                                    //                    if (result)
                                    //                    {
                                    //                        bool isOTItemStatus = WarehouseService.UpdateOTItemStatus_V2035(WarehouseCommon.Instance.Selectedwarehouse, packedItem.IdOTItem, GeosApplication.Instance.ActiveUser.IdUser);
                                    //                    }
                                    //                }

                                    //                //[002] Start
                                    //                //((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                    //                ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Count;
                                    //                //[002] End

                                    //                //show comment
                                    //                UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    //                PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    //                if (SelectedPackedItem != null && SelectedPackedItem.ArticleComment != null)
                                    //                {
                                    //                    if (SelectedPackedItem.ArticleCommentDateOfExpiry == null)
                                    //                    {
                                    //                        SelectedPackedItem.ShowComment = true;
                                    //                    }
                                    //                    else if (SelectedPackedItem.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                                    //                    {
                                    //                        SelectedPackedItem.ShowComment = true;
                                    //                    }

                                    //                }
                                    //                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                    //                WrongItem = "";
                                    //                IsWrongItemErrorVisible = Visibility.Collapsed;
                                    //                BarcodeStr = "";
                                    //            }
                                    //            else
                                    //            {
                                    //                WrongItem = String.Format("Max available quantity : {0}", packedItem.Qty);
                                    //                IsWrongItemErrorVisible = Visibility.Visible;
                                    //                BarcodeStr = "";
                                    //                SelectedUnPackedItem = unPackedItem;
                                    //                UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    //                PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    //                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    #endregion
                                    if (unPackedItem != null)
                                    {
                                        WOItem packedItem = (WOItem)unPackedItem.Clone();

                                        if (_qty <= packedItem.Qty)
                                        {
                                            //End
                                            if (_qty == unPackedItem.Qty)
                                            {
                                                UnPackedItemsList.Remove(unPackedItem);
                                                SelectedUnPackedItem = null;
                                                unPackedItem.Qty = unPackedItem.Qty - _qty;
                                                //Added rajashri[GEOS2-4849]
                                                if (!LatestPackedItemsList.Any(i => i.IdOT == packedItem.IdOT && i.IdOTItem == packedItem.IdOTItem))
                                                    LatestPackedItemsList.Add(packedItem);
                                            }
                                            else if (_qty < unPackedItem.Qty)
                                            {
                                                unPackedItem.Qty = unPackedItem.Qty - _qty;
                                                SelectedUnPackedItem = unPackedItem;
                                            }

                                            if (unPackedItem.Qty == 0)
                                            {
                                                PackedItemsList.Add(packedItem);
                                                SelectedPackedItem = packedItem;
                                                SelectedPackedItem.Qty = packedItem.OriginalQty;
                                                CalculateAndDisplayEstimatedWeight();
                                                //EstimatedWeight = EstimatedWeight + packedItem.ArticleWeight * packedItem.OriginalQty;
                                            }

                                            if (SelectedPackedItem != null)
                                                ArticleImage = ByteArrayToImage(SelectedPackedItem.ArticleImageInBytes);

                                            if (unPackedItem.Qty == 0)
                                            {
                                                bool result = WarehouseService.UpdatePackingBoxInPartnumber_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedPackingBox.IdPackingBox, packedItem.IdOTItem);
                                                if (result)
                                                {
                                                    bool isOTItemStatus = WarehouseService.UpdateOTItemStatus_V2035(WarehouseCommon.Instance.Selectedwarehouse, packedItem.IdOTItem, GeosApplication.Instance.ActiveUser.IdUser);
                                                }
                                            }

                                            UpdateItemsInBoxCount();
                                            ////[002] Start
                                            ////((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                            //((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Count;
                                            ////[002] End

                                            ShowCommentForExpiredArticles();
                                            SetUIForScanIsSuccessful();
                                            #region GEOS2-5705
                                            List<WOItem> combinedList = UnPackedItemsList.Concat(PackedItemsList).ToList();

                                            // OTItemListForLeftGroup=new ObservableCollection<WOItem>(combinedList.GroupBy(x => x.WorkOrder).Select(a => new { WorkOrder = a.Key, TotalWorkOrderCount = a.Count() }).ToList());
                                            var workOrderCounts = combinedList.GroupBy(x => x.WorkOrder).Select(group =>
                                            {
                                                var totalCount = group.Count();
                                                var unPackedCount = UnPackedItemsList.Count(item => item.WorkOrder == group.Key);
                                                var PackedCount = PackedItemsList.Count(item => item.WorkOrder == group.Key);
                                                int totalItems = unPackedCount + PackedCount;
                                                var percentage = (double)PackedCount / totalItems * 100;
                                                int roundedPercentage = (int)Math.Round(percentage);

                                                return new WOItem
                                                {
                                                    WorkOrder = group.Key,
                                                    TotalWorkOrderCount = totalCount,
                                                    UnpackedCount = unPackedCount,
                                                    PackedCount = PackedCount,
                                                    TotalItemCounted = $"{unPackedCount}/{totalCount}",
                                                    TotalCountPercentage = roundedPercentage + "%",
                                                    TotalCountColor = percentage == 0 ? "red" : (percentage == 100 ? "green" : "orange")
                                                };
                                            });

                                            OTItemListForLeftGroup = new ObservableCollection<WOItem>(workOrderCounts);


                                            if (OTItemListForLeftGroup != null)
                                            {
                                                //[Sudhir.Jangra][GEOS2-5705]
                                                int packedItemCount = 0;
                                                int unpackedItemCount = 0;


                                                foreach (var item in OTItemListForLeftGroup)
                                                {
                                                    packedItemCount += (int)item.PackedCount;
                                                    unpackedItemCount += (int)item.UnpackedCount;
                                                }


                                                int TotalItems = packedItemCount + unpackedItemCount;
                                                if (TotalItems != 0)
                                                {
                                                    double percentage1 = ((double)packedItemCount / TotalItems) * 100;
                                                    //  double percentage = ((double)unpackedItemCount / packedItemCount) * 100;
                                                    int roundedPercentage1 = (int)Math.Round(percentage1);
                                                    SelectedPlantTotalItem = unpackedItemCount + "/" + TotalItems;
                                                    SelectedPlantTotalItemPercentage = roundedPercentage1 + "%";
                                                    if (roundedPercentage1 == 0)
                                                    {
                                                        SelectedPlantItemColor = "Red";
                                                    }
                                                    else if (roundedPercentage1 > 0 && roundedPercentage1 != 100)
                                                    {
                                                        SelectedPlantItemColor = "Orange";
                                                    }
                                                    else if (roundedPercentage1 == 100)
                                                    {
                                                        SelectedPlantItemColor = "Green";
                                                    }
                                                    IsHeaderVisible = Visibility.Visible;
                                                    IsBoxImageVisibleForRightGroup = Visibility.Visible;
                                                    IsCheckedToggleButton = true;
                                                    SelectedPlantHeader = ClonedSelectedPlantHeader;
                                                }
                                                else
                                                {
                                                    SelectedPlantTotalItem = string.Empty;
                                                    SelectedPlantTotalItemPercentage = string.Empty;
                                                    SelectedPlantItemColor = string.Empty;
                                                    IsHeaderVisible = Visibility.Collapsed;
                                                    IsBoxImageVisibleForRightGroup = Visibility.Hidden;
                                                    IsCheckedToggleButton = false;
                                                    OTItemListForLeftGroup = new ObservableCollection<WOItem>();
                                                    SelectedPlantHeader = string.Empty;
                                                }
                                            }



                                            //[pramod.misal][26.06.2024][GEOS2-5705]
                                            VisibleRecords = new ObservableCollection<WOItem>();
                                            CurrentRecordIndex = 0; // Start at the first record
                                            #endregion
                                        }
                                        else
                                        {
                                            SetErrorTemplateForMaxAvailableQuantity(unPackedItem, packedItem);
                                            return;
                                        }
                                    }
                                    else
                                        SetErrorTemplate();
                                    return;
                                    //} // [002] End
                                }
                                else
                                    SetErrorTemplate();
                                return;
                            }
                            else if (!IsScanDirectionArrow)
                            {
                                if (isProperBarcode && BarcodeStr.Length >= 17)
                                {

                                    WOItem packedItemToUnPackedItem = PackedItemsList.FirstOrDefault(x =>
                                            x.PartNumberCode.Substring(0, (x.PartNumberCode.Length - 3)).ToUpperInvariant() ==
                                            _partNumberCode.ToUpperInvariant());

                                    if (packedItemToUnPackedItem != null)
                                    {
                                        WOItem unPackedItem = (WOItem)packedItemToUnPackedItem.Clone();
                                        if (_qty <= unPackedItem.Qty)
                                        {

                                            if (_qty == packedItemToUnPackedItem.Qty)
                                            {
                                                PackedItemsList.Remove(packedItemToUnPackedItem);
                                                UnPackedItemsList.Add(unPackedItem);
                                                ArticleImage = null;
                                                SelectedPackedItem = null;
                                                CalculateAndDisplayEstimatedWeight();
                                                // EstimatedWeight = EstimatedWeight - packedItemToUnPackedItem.ArticleWeight * unPackedItem.OriginalQty;
                                                SelectedUnPackedItem = unPackedItem;
                                                SelectedUnPackedItem.Qty = unPackedItem.OriginalQty;
                                                packedItemToUnPackedItem.Qty = packedItemToUnPackedItem.Qty - _qty;
                                                //[rdixit][GEOS2-4849][07.11.2023]
                                                if (LatestPackedItemsList == null)
                                                    LatestPackedItemsList = new ObservableCollection<WOItem>();

                                                if (LatestPackedItemsList.Any(i => i.IdOT == packedItemToUnPackedItem.IdOT && i.IdOTItem == packedItemToUnPackedItem.IdOTItem))
                                                    LatestPackedItemsList.Remove(packedItemToUnPackedItem);
                                                else if (!LatestPackedItemsList.Any(i => i.IdOT == packedItemToUnPackedItem.IdOT))
                                                    LatestPackedItemsList.Add(packedItemToUnPackedItem);
                                            }
                                            else if (_qty < unPackedItem.Qty)
                                            {
                                                packedItemToUnPackedItem.Qty = packedItemToUnPackedItem.Qty - _qty;
                                            }

                                            if (SelectedPackedItem != null)
                                                ArticleImage = ByteArrayToImage(SelectedPackedItem.ArticleImageInBytes);

                                            if (packedItemToUnPackedItem.Qty == 0)
                                            {
                                                bool result = WarehouseService.UpdateUnPackingBoxInPartnumber_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedPackingBox.IdPackingBox, unPackedItem.IdOTItem);
                                                if (result)
                                                {
                                                    bool isOTItemStatusToFinished = WarehouseService.UpdateOTItemStatusToFinished_V2035(WarehouseCommon.Instance.Selectedwarehouse, unPackedItem.IdOTItem, GeosApplication.Instance.ActiveUser.IdUser);
                                                }
                                            }

                                            UpdateItemsInBoxCount();
                                            ShowCommentForExpiredArticles();
                                            SetUIForScanIsSuccessful();
                                            ////show comment

                                            #region GEOS2-5705
                                            List<WOItem> combinedList = UnPackedItemsList.Concat(PackedItemsList).ToList();

                                            // OTItemListForLeftGroup=new ObservableCollection<WOItem>(combinedList.GroupBy(x => x.WorkOrder).Select(a => new { WorkOrder = a.Key, TotalWorkOrderCount = a.Count() }).ToList());
                                            var workOrderCounts = combinedList.GroupBy(x => x.WorkOrder).Select(group =>
                                            {
                                                var totalCount = group.Count();
                                                var unPackedCount = UnPackedItemsList.Count(item => item.WorkOrder == group.Key);
                                                var PackedCount = PackedItemsList.Count(item => item.WorkOrder == group.Key);
                                                int totalItems = unPackedCount + PackedCount;
                                                var percentage = (double)PackedCount / totalItems * 100;
                                                int roundedPercentage = (int)Math.Round(percentage);

                                                return new WOItem
                                                {
                                                    WorkOrder = group.Key,
                                                    TotalWorkOrderCount = totalCount,
                                                    UnpackedCount = unPackedCount,
                                                    PackedCount = PackedCount,
                                                    TotalItemCounted = $"{unPackedCount}/{totalCount}",
                                                    TotalCountPercentage = roundedPercentage + "%",
                                                    TotalCountColor = percentage == 0 ? "red" : (percentage == 100 ? "green" : "orange")
                                                };
                                            });

                                            OTItemListForLeftGroup = new ObservableCollection<WOItem>(workOrderCounts);


                                            if (OTItemListForLeftGroup != null)
                                            {
                                                //[Sudhir.Jangra][GEOS2-5705]
                                                int packedItemCount = 0;
                                                int unpackedItemCount = 0;


                                                foreach (var item in OTItemListForLeftGroup)
                                                {
                                                    packedItemCount += (int)item.PackedCount;
                                                    unpackedItemCount += (int)item.UnpackedCount;
                                                }


                                                int TotalItems = packedItemCount + unpackedItemCount;
                                                if (TotalItems != 0)
                                                {
                                                    double percentage1 = ((double)packedItemCount / TotalItems) * 100;
                                                    //  double percentage = ((double)unpackedItemCount / packedItemCount) * 100;
                                                    int roundedPercentage1 = (int)Math.Round(percentage1);
                                                    SelectedPlantTotalItem = unpackedItemCount + "/" + TotalItems;
                                                    SelectedPlantTotalItemPercentage = roundedPercentage1 + "%";
                                                    if (roundedPercentage1 == 0)
                                                    {
                                                        SelectedPlantItemColor = "Red";
                                                    }
                                                    else if (roundedPercentage1 > 0 && roundedPercentage1 != 100)
                                                    {
                                                        SelectedPlantItemColor = "Orange";
                                                    }
                                                    else if (roundedPercentage1 == 100)
                                                    {
                                                        SelectedPlantItemColor = "Green";
                                                    }
                                                    IsHeaderVisible = Visibility.Visible;
                                                    IsBoxImageVisibleForRightGroup = Visibility.Visible;
                                                    IsCheckedToggleButton = true;
                                                    SelectedPlantHeader = ClonedSelectedPlantHeader;
                                                }
                                                else
                                                {
                                                    SelectedPlantTotalItem = string.Empty;
                                                    SelectedPlantTotalItemPercentage = string.Empty;
                                                    SelectedPlantItemColor = string.Empty;
                                                    IsHeaderVisible = Visibility.Collapsed;
                                                    IsBoxImageVisibleForRightGroup = Visibility.Hidden;
                                                    IsCheckedToggleButton = false;
                                                    OTItemListForLeftGroup = new ObservableCollection<WOItem>();
                                                    SelectedPlantHeader = string.Empty;
                                                }
                                            }



                                            //[pramod.misal][26.06.2024][GEOS2-5705]
                                            VisibleRecords = new ObservableCollection<WOItem>();
                                            CurrentRecordIndex = 0; // Start at the first record
                                            #endregion

                                        }
                                        else
                                        {
                                            SetErrorTemplateForMaxAvailableQuantityForPackedToUnpacked(unPackedItem);
                                        }
                                    }
                                    else
                                        SetErrorTemplate();
                                }
                                else
                                    SetErrorTemplate();
                            }
                            CalculateAndDisplayEstimatedWeight();
                        }
                        else if (SelectedItem is PackingCompany && !IsScanBox)
                        {
                            SetErrorTemplate("Scan box first");

                        }
                       

                    }
                    else
                    {
                        BarcodeStr = BarcodeStr + obj.Text;
                    }

                   
                    UpdateVisibleRecords();
                }

                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                BarcodeStr = "";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                BarcodeStr = "";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                //BarcodeStr = "";
                SetErrorTemplate();
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanBarcodeAction...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UpdateItemsInBoxCount()
        {
            if (SelectedItem != null && SelectedItem is PackingBox)
            {
                //[002] Start
                //((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Count;
                //[002] End
                //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
                //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
                try
                {
                    if (PackedItemsList.Count == 0)
                    {
                        ((PackingBox)SelectedItem).CarriageMethod = null;
                        ((PackingBox)SelectedItem).CarriageMethodHtmlColor = null;
                        ((PackingBox)SelectedItem).IdCarriageMethod = 0;
                    }
                    else
                    {
                        ((PackingBox)SelectedItem).CarriageMethod = PackedItemsList.FirstOrDefault().CarriageMethod;
                        ((PackingBox)SelectedItem).CarriageMethodHtmlColor = PackedItemsList.FirstOrDefault().CarriageMethodHtmlColor;
                        ((PackingBox)SelectedItem).IdCarriageMethod = PackedItemsList.FirstOrDefault().IdCarriageMethod;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void SetErrorTemplateForMaxAvailableQuantityForPackedToUnpacked(WOItem unPackedItem)
        {
            WrongItem = String.Format("Max available quantity : {0}", unPackedItem.Qty);
            IsWrongItemErrorVisible = Visibility.Visible;
            BarcodeStr = "";
            if (UnPackedItemsList != null)
            {
                UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            }
            if (PackedItemsList != null)
            {
                PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            }
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
        }

        private void SetUIForScanIsSuccessful()
        {
            //SetUIForScanIsSuccessful
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
            WrongItem = "";
            IsWrongItemErrorVisible = Visibility.Collapsed;
            BarcodeStr = "";
        }

        private bool OneOrderOneBoxValidation(WOItem unPackedItem)
        {
            //OneOrderOneBoxValidation
            // Perform validation - Only one Offer's items are allowed in One Packing Box, for the selected Sites in system settings.
            bool atLeastOneItemPresentInPackedItemsList = (PackedItemsList != null && PackedItemsList.Count >= 1);
            bool selectedItemSiteExistWithSystemSettingSitesList =
                (SelectedItem != null && SelectedItem is PackingBox &&
                oneOrderOneBoxSiteIds != null &&
                oneOrderOneBoxSiteIds.Any(x => x == ((PackingBox)SelectedItem).IdSite));     //checking idSite with GeosAppSettingFile
            bool selectedItemIsValidForPacking = true;

            if (atLeastOneItemPresentInPackedItemsList && selectedItemSiteExistWithSystemSettingSitesList)
            {
                selectedItemIsValidForPacking = (PackedItemsList[0].IdOffer == unPackedItem.IdOffer);
            }

            return selectedItemIsValidForPacking;
        }

        private void ShowCommentForExpiredArticles()
        {
            //ShowCommentForExpiredArticles
            UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            if (SelectedPackedItem != null && SelectedPackedItem.ArticleComment != null)
            {
                if (SelectedPackedItem.ArticleCommentDateOfExpiry == null)
                {
                    SelectedPackedItem.ShowComment = true;
                }
                else if (SelectedPackedItem.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                {
                    SelectedPackedItem.ShowComment = true;
                }

            }
        }

        private void SetErrorTemplateForMaxAvailableQuantity(WOItem unPackedItem, WOItem packedItem)
        {
            // SetErrorTemplateForMaxAvailableQuantity
            WrongItem = String.Format("Max available quantity : {0}", packedItem.Qty);
            IsWrongItemErrorVisible = Visibility.Visible;
            BarcodeStr = "";
            SelectedUnPackedItem = unPackedItem;

            if (UnPackedItemsList != null)
            {
                UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            }

            if (PackedItemsList != null)
            {
                PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            }

            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
        }

        private void ProcessScanBoxOption()
        {
            var isProperBarcode = !string.IsNullOrEmpty(BarcodeStr);
            if (isProperBarcode)
            {
                var tempBarcode = BarcodeStr.Substring(1).TrimStart(new char[] { '0' });
                double scannedIdPackingBox = 0;
                bool result = double.TryParse(tempBarcode, out scannedIdPackingBox);

                if (!result)
                {
                    SetWrongBoxVisibilityTemplate();
                    return;
                }

                PackingBox tempPackingBox = PackingCompanyList.SelectMany(x => x.PackingBoxes.Where(y => y.IdPackingBox == (long)(Convert.ToDouble(tempBarcode)))).ToList().FirstOrDefault();
                if (tempPackingBox != null)
                {
                    string barcode = "B" + tempPackingBox.IdPackingBox.ToString().PadLeft(9, '0');
                    if (barcode.Equals(BarcodeStr))
                    {
                        SelectedItem = tempPackingBox;
                        SetBoxVisibilityTemplate();
                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                        if (IsAutoWeighing)
                        {
                            if (balance != null)
                            {
                                balance.Dispose();
                                balance = null;
                                GetScaleWeight();
                            }
                        }
                        return;
                    }
                    else
                    {
                        SetWrongBoxVisibilityTemplate();
                        return;
                    }
                }
                else
                {
                    SetWrongBoxVisibilityTemplate();
                    return;
                }
            }
            else
            {
                SetWrongBoxVisibilityTemplate();
                return;
            }
        }

        private void SetErrorTemplate(string WrongItemMessage = null)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetErrorTemplate....", category: Category.Info, priority: Priority.Low);

                if (WrongItemMessage != null)
                {
                    WrongItem = WrongItemMessage;
                }
                else
                {
                    //WrongItem = "Wrong Item " + BarcodeStr;
                    //Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
                    if (IsScanDirectionArrow)
                    {
                        WrongItem = System.Windows.Application.Current.FindResource("ScannedUnpackedItems").ToString() + " " + BarcodeStr;
                    }
                    else
                    {
                        WrongItem = System.Windows.Application.Current.FindResource("ScannedPackedItems").ToString() + " " + BarcodeStr;
                    }

                }

                IsWrongItemErrorVisible = Visibility.Visible;
                BarcodeStr = "";
                if (UnPackedItemsList != null)
                {
                    UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                }
                if (PackedItemsList != null)
                {
                    PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                }
                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                GeosApplication.Instance.Logger.Log("Method SetErrorTemplate....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetErrorTemplate...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
        //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
        private void SetErrorTemplateForCarriageMethod(string WrongItemMessage = null)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetErrorTemplateForCarriageMethod....", category: Category.Info, priority: Priority.Low);

                if (WrongItemMessage != null)
                {
                    WrongItem = WrongItemMessage;
                }
                else
                {
                    WrongItem = "Wrong Item " + BarcodeStr;
                }

                IsWrongItemErrorVisible = Visibility.Visible;
                BarcodeStr = "";
                if (UnPackedItemsList != null)
                {
                    UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                }
                if (PackedItemsList != null)
                {
                    PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                }
                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                GeosApplication.Instance.Logger.Log("Method SetErrorTemplateForCarriageMethod....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SetErrorTemplateForCarriageMethod...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// For OneOrderOneBox Error
        /// </summary>
        private void OneOrderOneBoxErrorTemplate()
        {
            PackingCompany packingCompany = PackingCompanyList.FirstOrDefault(c => c.IdCompany == SelectedPackingBox.IdSite);
            WrongItem = string.Format(Application.Current.FindResource("OneOrderOneBoxErrorMsg").ToString(), packingCompany.ShortName);
            IsWrongItemErrorVisible = Visibility.Visible;
            UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
        }

        private void SetErrorVisibilityTemplate()
        {
            IsWrongItemErrorVisible = Visibility.Collapsed;
            WrongItem = "";
            BarcodeStr = "";
        }

        private void SetWrongBoxVisibilityTemplate()
        {
            //WrongItem = "Wrong Box " + BarcodeStr;
            //Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
            //WrongItem = "Scanned code does not match the selected box " + BarcodeStr;
            WrongItem = System.Windows.Application.Current.FindResource("Scannedbox").ToString() + " " + BarcodeStr;
            IsWrongItemErrorVisible = Visibility.Visible;
            BarcodeStr = "";

            if (UnPackedItemsList != null)
            {
                UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            }

            if (PackedItemsList != null)
            {
                PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            }

            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
        }

        private void SetBoxVisibilityTemplate()
        {
            WrongItem = "";
            IsWrongItemErrorVisible = Visibility.Collapsed;
            BarcodeStr = "";
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource ByteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn != null)
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(byteArrayIn);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    biImg.DecodePixelHeight = 10;
                    biImg.DecodePixelWidth = 10;

                    ImageSource imgSrc = biImg as ImageSource;

                    GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....executed successfully", category: Category.Info, priority: Priority.Low);
                    return imgSrc;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ByteArrayToImage...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        ///// <summary>
        ///// For GEOS2-3415, we will stop using old setting GeosAppSettings("22"). Keep using GeosAppSettings("34").
        ///// Method to Get Customers Requiremrnt Settings [ Customers_Requiring_One_Box_One_WorkOrder]
        ///// </summary>
        //private void GetCustomersRequiremrntSettings()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method GetCustomersRequiremrntSettings()...", category: Category.Info, priority: Priority.Low);

        //        List<GeosAppSetting> GeosAppSettingList = WorkbenchStartUp.GetSelectedGeosAppSettings("22");
        //        if (GeosAppSettingList.Count > 0)
        //        {
        //            string[] GeosAppSettingDefaultValues = GeosAppSettingList[0].DefaultValue.Split(',');
        //            CustomersIdList = GeosAppSettingDefaultValues.ToList().ConvertAll(int.Parse);
        //        }

        //        GeosApplication.Instance.Logger.Log("Method GetCustomersRequiremrntSettings()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<Services.Contracts.ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in GetCustomersRequiremrntSettings() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in GetCustomersRequiremrntSettings() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in Method GetCustomersRequiremrntSettings()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        /// <summary>
        ///  method to Check Box Weight Tolerance
        /// </summary>
        private void CheckBoxWeightTolerance()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckBoxWeightTolerance...", category: Category.Info, priority: Priority.Low);
                double boxWeightToleranceDifferance = ScaleWeight - boxWeightTolerance;
                double boxWeightToleranceDifferanceValue = ScaleWeight + boxWeightTolerance;

                if (boxWeightToleranceDifferance <= EstimatedWeight && EstimatedWeight <= boxWeightToleranceDifferanceValue)
                    IsBoxWeightTolerance = true;
                else
                    IsBoxWeightTolerance = false;

                GeosApplication.Instance.Logger.Log("Method CheckBoxWeightTolerance executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CheckBoxWeightTolerance() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// [001][cpatil][17-09-2020][GEOS2-2415]Add Date of Expiry in Article comments
        /// [002][cpatil][19-07-2023][GEOS2-4687]service not updated when expanding box
        private async void ItemExpandedCommandAction(AccordionItemExpandedEventArgs e)
        {
            if (e.Item is PackingCompany)
            {
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                PackingCompany tempPackingCompany = (PackingCompany)e.Item;
                #region GEOS2-5705
                int packedItemCount = 0;
                int unpackedItemCount = 0;
                SelectedPlantHeader = tempPackingCompany.ShortName;
                ClonedSelectedPlantHeader = SelectedPlantHeader;
                #endregion


                if (tempPackingCompany.PackingBoxes != null && tempPackingCompany.PackingBoxes.Count == 1)
                {
                    SelectedItem = tempPackingCompany.PackingBoxes.FirstOrDefault();
                    if (((PackingBox)SelectedItem).IsClosed == 0)
                        PackingBoxDetailsVisibility = Visibility.Visible;
                    else if (((PackingBox)SelectedItem).IsClosed == 1)
                        PackingBoxDetailsVisibility = Visibility.Collapsed;
                    //[001] [002]
                    //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2039(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite));
                    await Task.Run(() => UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2400(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite, ((PackingBox)SelectedItem).IdCountryGroup)));
                    if (IsWorkOrder)
                    {
                        try
                        {
                            UnPackedItemsListNew = new ObservableCollection<WOItem>();
                            //UnPackedItemsListNew.AddRange(UnPackedItemsList);
                            foreach (WOItem unPackedItem in UnPackedItemsList)
                            {
                                if (IdOT == unPackedItem.IdOT)
                                {
                                    UnPackedItemsListNew.Add(unPackedItem);
                                }
                            }
                            UnPackedItemsList = new ObservableCollection<WOItem>();
                            UnPackedItemsList.AddRange(UnPackedItemsListNew);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    //[Sudhir.Jangra][GEOS2-5740]
                    // await Task.Run(() => UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2530(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdSite, ((PackingBox)SelectedItem).IdCountryGroup)));


                    //PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2039(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox));
                    await Task.Run(() => PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2400(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox)));
                    //[Sudhir.Jangra][GEOS2-5740]
                    //await Task.Run(() => PackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackedItemByIdPackingBox_V2530(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox)));


                    SelectedPackingBox = (PackingBox)SelectedItem;
                    SetBoxVisibilityTemplate();
                    CalculateAndDisplayEstimatedWeight();
                    //  EstimatedWeight = ((PackingBox)SelectedItem).NetWeight + PackedItemsList.Sum(x => x.ArticleWeight * x.Qty);
                }
                else
                {
                    //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2039(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany));

                    if (tempPackingCompany.PackingBoxes.Select(x => x.IdCountryGroup).Distinct().Count() > 1)
                    {
                        UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2039(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany));
                        if (IsWorkOrder)
                        {
                            try
                            {
                                UnPackedItemsListNew = new ObservableCollection<WOItem>();
                                //UnPackedItemsListNew.AddRange(UnPackedItemsList);
                                foreach (WOItem unPackedItem in UnPackedItemsList)
                                {
                                    if (IdOT == unPackedItem.IdOT)
                                    {
                                        UnPackedItemsListNew.Add(unPackedItem);
                                    }
                                }
                                UnPackedItemsList = new ObservableCollection<WOItem>();
                                UnPackedItemsList.AddRange(UnPackedItemsListNew);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                        //UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2039(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany));
                    }
                    else
                    {
                        UnPackedItemsList = new ObservableCollection<WOItem>();
                        foreach (var item in tempPackingCompany.PackingBoxes.Select(x => x.IdCountryGroup).Distinct())
                        {
                            // await Task.Run(() => UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetRevisionItemPackingWorkOrders_V2400(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany, item)));
                            UnPackedItemsList.AddRange((WarehouseService.GetRevisionItemPackingWorkOrders_V2400(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany, item)));
                            if (IsWorkOrder)
                            {
                                try
                                {
                                    UnPackedItemsListNew = new ObservableCollection<WOItem>();
                                    //UnPackedItemsListNew.AddRange(UnPackedItemsList);
                                    foreach (WOItem unPackedItem in UnPackedItemsList)
                                    {
                                        if (IdOT == unPackedItem.IdOT)
                                        {
                                            UnPackedItemsListNew.Add(unPackedItem);
                                        }
                                    }
                                    UnPackedItemsList = new ObservableCollection<WOItem>();
                                    UnPackedItemsList.AddRange(UnPackedItemsListNew);
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in SelectedItemChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                                }
                            }
                            //UnPackedItemsList.AddRange((WarehouseService.GetRevisionItemPackingWorkOrders_V2400(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany, item)));
                            //[Sudhir.Jangra][GEOS2-5740]
                            //    UnPackedItemsList.AddRange((WarehouseService.GetRevisionItemPackingWorkOrders_V2530(WarehouseCommon.Instance.Selectedwarehouse, tempPackingCompany.IdCompany, item)));

                        }
                    }

                    PackingBoxDetailsVisibility = Visibility.Collapsed;
                }

                //foreach (var item in tempPackingCompany.PackingBoxes)
                //{
                //    packedItemCount += (int)item.ItemsInBox;
                //}

                //if (UnPackedItemsList == null)
                //{
                //    unpackedItemCount = 0;
                //}
                //else
                //{
                //    unpackedItemCount = UnPackedItemsList.Count();
                //}
                //int TotalItems = packedItemCount + unpackedItemCount;
                //double percentage = ((double)packedItemCount / TotalItems) * 100;
                ////  double percentage = ((double)unpackedItemCount / packedItemCount) * 100;
                //int roundedPercentage = (int)Math.Round(percentage);
                //SelectedPlantTotalItem = unpackedItemCount + "/" + packedItemCount;
                //SelectedPlantTotalItemPercentage = roundedPercentage + "%";
                //if (roundedPercentage == 0)
                //{
                //    SelectedPlantItemColor = "Red";
                //}
                //else if (roundedPercentage >= 0)
                //{
                //    SelectedPlantItemColor = "Orange";
                //}
                //else if (roundedPercentage == 100)
                //{
                //    SelectedPlantItemColor = "Green";
                //}
                SelectedPlantTotalItem = string.Empty;
                SelectedPlantTotalItemPercentage = string.Empty;
                SelectedPlantItemColor = string.Empty;
                IsHeaderVisible = Visibility.Collapsed;
                IsBoxImageVisibleForRightGroup = Visibility.Hidden;
                IsCheckedToggleButton = false;
                OTItemListForLeftGroup = new ObservableCollection<WOItem>();

                FilterString = string.Empty;

                PackedFilterString = string.Empty;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }

        private void DeleteButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction...", category: Category.Info, priority: Priority.Low);

                if (SelectedItem is PackingBox)
                {
                    PackingBox tempPackingBox = (PackingBox)SelectedItem;
                    if (tempPackingBox.ItemsInBox == 0)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeletePackingBoxMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            bool isDelete = WarehouseService.RemovePackingBox(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)SelectedItem).IdPackingBox);
                            if (isDelete)
                            {
                                PackingCompany tempPackingCompany = PackingCompanyList.FirstOrDefault(x => x.IdCompany == tempPackingBox.IdSite);
                                if (tempPackingCompany != null)
                                {
                                    tempPackingCompany.PackingBoxes.Remove(tempPackingBox);
                                }
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeletePackingBoxSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            }
                        }
                    }
                    else
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NotAllowToDeletePackingBox").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteButtonCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteButtonCommandAction() method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteButtonCommandAction() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenCloseBoxCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenCloseButtonCommandAction...", category: Category.Info, priority: Priority.Low);
                if (SelectedItem is PackingBox)
                {
                    PackingBox editPackingBox = (PackingBox)SelectedItem;
                    bool isBoxButtonCloseable = false;
                    bool isPackingBoxClosed = false;
                    if (editPackingBox.IsClosed == 1)
                    {
                        editPackingBox.IsClosed = 0;
                        PackingBoxDetailsVisibility = Visibility.Visible;
                        isBoxButtonCloseable = true;
                    }
                    else if (editPackingBox.IsClosed == 0)
                    {
                        if (editPackingBox.Length != 0 && editPackingBox.Width != 0 && editPackingBox.Height != 0 && editPackingBox.NetWeight != 0 && editPackingBox.GrossWeight != 0 && editPackingBox.GrossWeight - editPackingBox.NetWeight >= 0)
                        {
                            editPackingBox.IsClosed = 1;
                            PackingBoxDetailsVisibility = Visibility.Collapsed;
                            isBoxButtonCloseable = true;
                        }
                        else
                        {
                            if (editPackingBox.GrossWeight == 0 || !(editPackingBox.GrossWeight - editPackingBox.NetWeight >= 0))
                            {
                                string errorMessage1 = string.Format(System.Windows.Application.Current.FindResource("GrossWeightClosedButtonErrorMessage1").ToString());
                                string errorMessage2 = string.Format(System.Windows.Application.Current.FindResource("GrossWeightClosedButtonErrorMessage2").ToString());
                                string errorMessage = errorMessage1 + "\n" + errorMessage2;
                                string popUpWarningColor = Application.Current.Resources["PopUpWarningColor"].ToString();
                                CustomMessageBox.Show(
                                    errorMessage,
                                    popUpWarningColor,
                                    CustomMessageBox.MessageImagePath.NotOk,
                                    MessageBoxButton.OK
                                ); isBoxButtonCloseable = false;
                                EditBoxCommandActionAfterPressCloseButton(obj);
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ClosedButtonErrorMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                isBoxButtonCloseable = false;
                                EditBoxCommandActionAfterPressCloseButton(obj);
                            }

                        }

                    }
                    if (isBoxButtonCloseable)
                    {
                        isPackingBoxClosed = WarehouseService.UpdateIsClosedInPackingBox(WarehouseCommon.Instance.Selectedwarehouse, editPackingBox.IdPackingBox, editPackingBox.IsClosed);
                    }
                    else
                    {
                        isPackingBoxClosed = false;
                    }

                    if (isPackingBoxClosed && editPackingBox.IsClosed == 1)
                        PrintBoxLabel(editPackingBox);
                }
                GeosApplication.Instance.Logger.Log("Method OpenCloseBoxCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenCloseBoxCommandAction() method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenCloseBoxCommandAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenCloseBoxCommandAction() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintBoxLabel(PackingBox editPackingBox)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintBoxLabel...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                List<BoxPrint> tempPackingBoxList = WarehouseService.GetWorkorderByIdPackingBox_V2210(WarehouseCommon.Instance.Selectedwarehouse, editPackingBox.IdPackingBox);
                PrintValues = new Dictionary<string, string>();
                byte[] printFile = GeosRepositoryService.GetBoxLabelFile(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);
                PrintLabel = new PrintLabel(PrintValues, printFile);
                CreatePrintValues(tempPackingBoxList);
                PrintLabel.Print();

                if (printFile != null)
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("BoxLabelPrintSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintBoxLabel executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintBoxLabel() method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintBoxLabel() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PrintBoxLabel() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CreatePrintValues(List<BoxPrint> tempPackingBoxList)
        {
            try
            {
                string UserCode = WarehouseService.GetEmployeeCodeByIdUser(GeosApplication.Instance.ActiveUser.IdUser);//[GEOS2-4014][rdixit][15.12.2022]
                GeosApplication.Instance.Logger.Log("Method CreatePrintValues...", category: Category.Info, priority: Priority.Low);
                PrintValues.Add("@USER", UserCode);

                #region SiteName(As per GPM)
                string customerName = String.Format("{0} - {1}", tempPackingBoxList.FirstOrDefault().CustomerName, tempPackingBoxList.FirstOrDefault().SiteNameWithCountry);

                if (customerName.Length > 26)
                {
                    int index = 0;
                    string firstLine = GetFirstLine(customerName, ref index);
                    PrintValues.Add("@CUSTOMER00", firstLine);
                    PrintValues.Add("@CUSTOMER01", customerName.Substring(index));
                }
                else
                {
                    PrintValues.Add("@CUSTOMER00", customerName);
                    PrintValues.Add("@CUSTOMER01", "");
                }
                #endregion

                PrintValues.Add("@OT00", "");
                PrintValues.Add("@OT01", "");
                PrintValues.Add("@OT02", "");
                PrintValues.Add("@OT03", "");
                PrintValues.Add("@OT04", "");
                PrintValues.Add("@OT05", "");
                PrintValues.Add("@OT06", "");
                PrintValues.Add("@OT07", "");
                PrintValues.Add("@OT08", "");
                PrintValues.Add("@OT09", "");
                int id = 0;
                foreach (BoxPrint item in tempPackingBoxList)
                {
                    PrintValues["@OT0" + id] = item.OtCode;
                    id++;
                }

                PrintValues.Add("@BOX_NUMBER", tempPackingBoxList.FirstOrDefault().BoxNumber);
                PrintValues.Add("@BOX_ID", GetPackingBoxBarCode(tempPackingBoxList.FirstOrDefault().IdPackingBox.ToString()));
                PrintValues.Add("@WEIGHT", tempPackingBoxList.FirstOrDefault().GrossWeight.ToString() + "Kg");
                PrintValues.Add("@CARRIAGE_METHOD_CODE", tempPackingBoxList.FirstOrDefault().CarriageMethodAbbreviation);
                PrintValues.Add("@CARRIAGE_METHOD_NAME", tempPackingBoxList.FirstOrDefault().CarriageMethodValue);
                PrintValues.Add("@WAREHOUSE", WarehouseCommon.Instance.Selectedwarehouse.Name);
                GeosApplication.Instance.Logger.Log("Method CreatePrintValues executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreatePrintValues() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private string GetFirstLine(string customer, ref int i)
        {
            string temp = "";

            for (int j = 0; j < customer.Length; j++)
            {
                if (customer[j] == ' ')
                {
                    if (j > 26)
                    {
                        break;
                    }
                    else
                    {
                        i = j + 1;
                        temp = customer.Substring(0, j);
                    }
                }
            }
            return temp;
        }

        private string GetPackingBoxBarCode(string idPackingBox)
        {
            string barcode = "";
            barcode = "B" + idPackingBox.PadLeft(9, '0');
            return barcode;
        }

        /// <summary>
        /// Method to get Scale Weight
        /// [003][Sprint_78][GEOS2-1991][Show connection indicator for Weighing Machine]
        /// [004][cpatil][17-02-2022][GEOS2-3559][Add Support for the new scale in EWRO]
        /// </summary>
        private void GetScaleWeight()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetScaleWeight...", category: Category.Info, priority: Priority.Low);

                if (!string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedScaleModel))
                {
                    if (balance == null)
                    {
                        //Shubham[skadam]  GEOS2-8812 EWCN Scale support in WMS 1of 1 03 07 2025
                        if (Enum.TryParse(WarehouseCommon.Instance.SelectedScaleModel, out Balance.Model modelEnum))
                        {
                            if (!string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedPort) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedBaudRate) 
                                && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedParity) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedDataBit)
                                && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedStopBit))
                            {
                                balance = new  BalanceFactory().CreateBalance(modelEnum);
                                balance.Communication = new SerialCommunication
                                    (WarehouseCommon.Instance.SelectedPort, 
                                    Convert.ToInt32(WarehouseCommon.Instance.SelectedBaudRate),
                                    (Parity)Enum.Parse(typeof(Parity), WarehouseCommon.Instance.SelectedParity, true), 
                                    Convert.ToInt32(WarehouseCommon.Instance.SelectedDataBit),
                                    (StopBits)Enum.Parse(typeof(StopBits), WarehouseCommon.Instance.SelectedStopBit)
                                    );

                                try
                                {
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... modelEnum: " + modelEnum.ToString(), category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedPort: " + WarehouseCommon.Instance.SelectedPort, category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedParity: " + WarehouseCommon.Instance.SelectedParity, category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedStopBit: " + WarehouseCommon.Instance.SelectedStopBit, category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedBaudRate: " + WarehouseCommon.Instance.SelectedBaudRate, category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedDataBit: " + WarehouseCommon.Instance.SelectedDataBit, category: Category.Info, priority: Priority.Low);
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in GetScaleWeight() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                                }

                                if (timer == null)
                                {
                                    timer = new Timer();
                                    timer.Interval = 1000;
                                }
                                timer.Elapsed += new ElapsedEventHandler(timer_Tick);
                                timer.Enabled = true;
                            }
                            else
                            {
                                StateIndex = 3;
                                ToolTip = MachineNOTConnected;
                                ScaleWeightValue = "0KG";
                                try
                                {
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... modelEnum: " + modelEnum.ToString(), category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedPort: " + WarehouseCommon.Instance.SelectedPort, category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedParity: " + WarehouseCommon.Instance.SelectedParity, category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedStopBit: " + WarehouseCommon.Instance.SelectedStopBit, category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedBaudRate: " + WarehouseCommon.Instance.SelectedBaudRate, category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... SelectedDataBit: " + WarehouseCommon.Instance.SelectedDataBit, category: Category.Info, priority: Priority.Low);
                                    GeosApplication.Instance.Logger.Log("Method GetScaleWeight... MachineNOTConnected: " + MachineNOTConnected, category: Category.Info, priority: Priority.Low);
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in GetScaleWeight() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                                }

                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SelectCOMSettingsMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                        }
                        #region OldCode
                        /*
                        if (WarehouseCommon.Instance.SelectedScaleModel == Hardware.BalanceNew.Model.SARTORIUS_MIRAS_IW2.ToString())
                        {
                            if (!string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedPort) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedBaudRate) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedParity) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedDataBit) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedStopBit))
                            {
                                balance = new Miras_IW2();
                                balance.Communication = new SerialCommunication(WarehouseCommon.Instance.SelectedPort, Convert.ToInt32(WarehouseCommon.Instance.SelectedBaudRate), (Parity)Enum.Parse(typeof(Parity), WarehouseCommon.Instance.SelectedParity, true), Convert.ToInt32(WarehouseCommon.Instance.SelectedDataBit), (StopBits)Enum.Parse(typeof(StopBits), WarehouseCommon.Instance.SelectedStopBit));

                                if (timer == null)
                                {
                                    timer = new Timer();
                                    timer.Interval = 1000;
                                }
                                timer.Elapsed += new ElapsedEventHandler(timer_Tick);
                                timer.Enabled = true;
                            }
                            else
                            {
                                StateIndex = 3;
                                ToolTip = MachineNOTConnected;
                                ScaleWeightValue = "0KG";
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SelectCOMSettingsMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                        }
                        else if (WarehouseCommon.Instance.SelectedScaleModel == Hardware.BalanceNew.Model.SARTORIUS_MIDRICS_MW1.ToString())
                        {
                            if (!string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedPort) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedBaudRate) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedParity) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedDataBit) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedStopBit))
                            {
                                balance = new Midrics_MW1();
                                balance.Communication = new SerialCommunication(WarehouseCommon.Instance.SelectedPort, Convert.ToInt32(WarehouseCommon.Instance.SelectedBaudRate), (Parity)Enum.Parse(typeof(Parity), WarehouseCommon.Instance.SelectedParity, true), Convert.ToInt32(WarehouseCommon.Instance.SelectedDataBit), (StopBits)Enum.Parse(typeof(StopBits), WarehouseCommon.Instance.SelectedStopBit));

                                if (timer == null)
                                {
                                    timer = new Timer();
                                    timer.Interval = 1000;
                                }

                                timer.Elapsed += new ElapsedEventHandler(timer_Tick);
                                timer.Enabled = true;
                            }
                            else
                            {
                                StateIndex = 3;
                                ToolTip = MachineNOTConnected;
                                ScaleWeightValue = "0KG";
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SelectCOMSettingsMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                        }
                        else if (WarehouseCommon.Instance.SelectedScaleModel == Hardware.BalanceNew.Model.SWS_FLUX_1T.ToString())// Added [004]
                        {
                            if (!string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedPort) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedBaudRate) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedParity) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedDataBit) && !string.IsNullOrEmpty(WarehouseCommon.Instance.SelectedStopBit))
                            {
                                balance = new Emdep.Geos.Hardware.Balances.Sws.Flux_1T();
                                balance.Communication = new SerialCommunication(WarehouseCommon.Instance.SelectedPort, Convert.ToInt32(WarehouseCommon.Instance.SelectedBaudRate), (Parity)Enum.Parse(typeof(Parity), WarehouseCommon.Instance.SelectedParity, true), Convert.ToInt32(WarehouseCommon.Instance.SelectedDataBit), (StopBits)Enum.Parse(typeof(StopBits), WarehouseCommon.Instance.SelectedStopBit));

                                if (timer == null)
                                {
                                    timer = new Timer();
                                    timer.Interval = 1000;
                                }

                                timer.Elapsed += new ElapsedEventHandler(timer_Tick);
                                timer.Enabled = true;
                            }
                            else
                            {
                                StateIndex = 3;
                                ToolTip = MachineNOTConnected;
                                ScaleWeightValue = "0KG";
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SelectCOMSettingsMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                        }
                        */
                        #endregion
                    }
                }
                else
                {
                    StateIndex = 3;
                    ToolTip = MachineNOTConnected;
                    ScaleWeightValue = "0KG";
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SelectScaleModelMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method GetScaleWeight executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (BalanceException ex)
            {
                StateIndex = 3;
                ToolTip = MachineNOTConnected;
                GeosApplication.Instance.Logger.Log("Get an error in GetScaleWeight() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                StateIndex = 3;
                ToolTip = MachineNOTConnected;
                ScaleWeightValue = "Error";
                GeosApplication.Instance.Logger.Log("Get an error in GetScaleWeight() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Timmer
        /// [003][Sprint_78][GEOS2-1991][Show connection indicator for Weighing Machine]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                timer.Enabled = false;
                if (!IsAutoWeighing) return;

                WeightMeasurement measure = null;

                try
                {
                    GeosApplication.Instance.Logger.Log("Method timer_Tick...", category: Category.Info, priority: Priority.Low);
                    measure = (balance).GetWeight();
                }
                catch (BalanceException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Method timer_Tick BalanceException - {0}.", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                if (isAutoWeighing)
                {
                    if (measure != null)
                    {
                        WarehouseCommon.Instance.ScaleWeight = ScaleWeight = Convert.ToDouble(measure.Value);
                        ScaleWeightValue = measure.Value + measure.Unit.ToString();
                        StateIndex = 1;
                        ToolTip = MachineConnected;
                        try
                        {
                            GeosApplication.Instance.Logger.Log("Method GetScaleWeight... measure.Value: " + measure.Value, category: Category.Info, priority: Priority.Low);
                            GeosApplication.Instance.Logger.Log("Method GetScaleWeight... measure.Unit: " + measure.Unit.ToString(), category: Category.Info, priority: Priority.Low);
                            GeosApplication.Instance.Logger.Log("Method GetScaleWeight... ScaleWeight: " + WarehouseCommon.Instance.ScaleWeight, category: Category.Info, priority: Priority.Low);
                            GeosApplication.Instance.Logger.Log("Method GetScaleWeight... ScaleWeightValue: " + ScaleWeightValue, category: Category.Info, priority: Priority.Low);
                            GeosApplication.Instance.Logger.Log("Method GetScaleWeight... ToolTip: " + ToolTip, category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in timer_Tick() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    else
                    {
                        StateIndex = 3;
                        ToolTip = MachineNOTConnected;
                        GeosApplication.Instance.Logger.Log(string.Format("Method timer_Tick Weight Measurement not received"), category: Category.Info, priority: Priority.Low);
                    }
                }
                timer.Enabled = true;
                GeosApplication.Instance.Logger.Log("Method timer_Tick executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in timer_Tick() method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PackedItemImageClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PackedItemImageClickCommandAction....", category: Category.Info, priority: Priority.Low);

            try
            {
                WOItem woItem = (WOItem)obj;
                IdOTPackedItem = woItem.IdOTItem;
                PackedItemsList.Where(a => a.IdOTItem != IdOTPackedItem && a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                PackedItemsList.Where(a => a.IdOTItem == IdOTPackedItem).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in PackedItemImageClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method PackedItemImageClickCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }

        #region [rdixit][08.02.2023][GEOS2-3406] Commented Imageclick event and added ItemSelect event for ToolTip
        /*private void UnPackedItemImageClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method UnPackedItemImageClickCommandAction....", category: Category.Info, priority: Priority.Low);

            try
            {
                WOItem picking = (WOItem)obj;
                IdOTUnPackedItem = picking.IdOTItem;
                UnPackedItemsList.Where(a => a.IdOTItem != IdOTUnPackedItem && a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                UnPackedItemsList.Where(a => a.IdOTItem == IdOTUnPackedItem).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in UnPackedItemImageClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method UnPackedItemImageClickCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }*/
        //[rdixit][08.02.2023][GEOS2-3406]
        private void UnPackedItemselectedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method UnPackedItemselectedCommandAction....", category: Category.Info, priority: Priority.Low);

            try
            {
                WOItem picking = (WOItem)obj;
                IdOTUnPackedItem = picking.IdOTItem;
                if (picking.ArticleComment != null)
                {
                    UnPackedItemsList.Where(a => a.IdOTItem != IdOTUnPackedItem && a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                    UnPackedItemsList.Where(a => a.IdOTItem == IdOTUnPackedItem).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
                }
                else
                {
                    UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in UnPackedItemselectedCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method UnPackedItemselectedCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }
        #endregion

        private void CommandKeyDownAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommandKeyDownAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                var Type = ((System.Windows.RoutedEventArgs)obj).OriginalSource.GetType();
                if (Type.Name != "Image" && Type.Name != "InplaceBaseEdit")
                {
                    if (IdOTUnPackedItem > 0)
                    {
                        UnPackedItemsList.Where(a => a.IdOTItem == IdOTUnPackedItem).ToList().ForEach(a => { a.ShowComment = false; });
                    }
                    if (IdOTPackedItem > 0)
                    {
                        PackedItemsList.Where(a => a.IdOTItem == IdOTPackedItem).ToList().ForEach(a => { a.ShowComment = false; });
                    }
                }
                else
                {
                    if (((System.Windows.UIElement)((System.Windows.RoutedEventArgs)obj).OriginalSource).Uid == "IdUnPackedImage" && PackedItemsList != null)
                    {
                        PackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                    }
                    else if (((System.Windows.UIElement)((System.Windows.RoutedEventArgs)obj).OriginalSource).Uid == "IdPackedItem" && UnPackedItemsList != null)
                    {
                        UnPackedItemsList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                    }
                    else if (Type.Name != "InplaceBaseEdit" && Type.Name != "Image")
                    {
                        if (IdOTUnPackedItem > 0)
                        {
                            UnPackedItemsList.Where(a => a.IdOTItem == IdOTUnPackedItem).ToList().ForEach(a => { a.ShowComment = false; });
                        }
                        if (IdOTPackedItem > 0)
                        {
                            PackedItemsList.Where(a => a.IdOTItem == IdOTPackedItem).ToList().ForEach(a => { a.ShowComment = false; });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in CommandKeyDownAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method CommandKeyDownAction....executed ", category: Category.Info, priority: Priority.Low);
        }

        //[Sudhir.Jangra][GEOS2-4542][09/08/2023]
        private void EditBoxCommandActionAfterPressCloseButton(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditBoxCommandAction....", category: Category.Info, priority: Priority.Low);
                if (SelectedItem is PackingBox)
                {
                    NewBoxView newBoxView = new NewBoxView();
                    EditBoxViewModel editBoxViewModel = new EditBoxViewModel();
                    EventHandler handler = delegate { newBoxView.Close(); };
                    editBoxViewModel.RequestClose += handler;
                    newBoxView.DataContext = editBoxViewModel;
                    editBoxViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditBox").ToString();
                    editBoxViewModel.IsOpenCloseButtonVisibile = Visibility.Visible;
                    editBoxViewModel.isWeightAndSizeValueNull = true;
                    editBoxViewModel.Init((PackingBox)SelectedItem, CompanyList, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                    newBoxView.ShowDialogWindow();

                    if (editBoxViewModel.IsResult)
                    {
                        PackingBox tempPackingBox = (PackingBox)SelectedItem;
                        tempPackingBox.BoxNumber = editBoxViewModel.UpdatePackingBox.BoxNumber;
                        tempPackingBox.IdPackingBoxType = editBoxViewModel.UpdatePackingBox.IdPackingBoxType;
                        tempPackingBox.Length = editBoxViewModel.UpdatePackingBox.Length;
                        tempPackingBox.Width = editBoxViewModel.UpdatePackingBox.Width;
                        tempPackingBox.Height = editBoxViewModel.UpdatePackingBox.Height;
                        tempPackingBox.NetWeight = editBoxViewModel.UpdatePackingBox.NetWeight;
                        tempPackingBox.GrossWeight = editBoxViewModel.UpdatePackingBox.GrossWeight;
                        tempPackingBox.IsClosed = editBoxViewModel.UpdatePackingBox.IsClosed;
                        tempPackingBox.IdCountryGroup = editBoxViewModel.UpdatePackingBox.IdCountryGroup;
                        tempPackingBox.CountryGroup = editBoxViewModel.UpdatePackingBox.CountryGroup;
                        tempPackingBox.IsVisibleCountryGroup = editBoxViewModel.UpdatePackingBox.IsVisibleCountryGroup;
                        tempPackingBox.IsStackable = editBoxViewModel.UpdatePackingBox.IsStackable;
                        if (tempPackingBox.IdSite == editBoxViewModel.UpdatePackingBox.IdSite)
                        {
                            SelectedItem = editBoxViewModel.UpdatePackingBox;
                        }
                        else
                        {
                            PackingCompany company = PackingCompanyList.FirstOrDefault(x => x.IdCompany == tempPackingBox.IdSite);
                            company.PackingBoxes.Remove(tempPackingBox);
                            if (PackingCompanyList.Any(x => x.IdCompany == editBoxViewModel.UpdatePackingBox.IdSite))
                            {
                                PackingCompany packingCompany = PackingCompanyList.FirstOrDefault(x => x.IdCompany == editBoxViewModel.UpdatePackingBox.IdSite);
                                packingCompany.PackingBoxes.Add(editBoxViewModel.UpdatePackingBox);
                                SelectedItem = editBoxViewModel.UpdatePackingBox;

                                //[001] Start
                                // ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                ((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Count;
                                //[001] End
                            }
                            else
                            {
                                PackingCompany packingCompany = new PackingCompany();
                                packingCompany.IdCompany = editBoxViewModel.CustomersList[editBoxViewModel.SelectedCustomerIndex].IdCompany;
                                packingCompany.ShortName = editBoxViewModel.CustomersList[editBoxViewModel.SelectedCustomerIndex].ShortName;
                                packingCompany.Name = editBoxViewModel.CustomersList[editBoxViewModel.SelectedCustomerIndex].Name;
                                packingCompany.PackingBoxes = new ObservableCollection<PackingBox>();
                                packingCompany.PackingBoxes.Add(editBoxViewModel.UpdatePackingBox);
                                PackingCompanyList.Add(packingCompany);
                                SelectedItem = editBoxViewModel.UpdatePackingBox;

                                UpdateItemsInBoxCount();
                                ////[001] start
                                ////((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Sum(x => x.Qty);
                                //((PackingBox)SelectedItem).ItemsInBox = PackedItemsList.Count;
                                ////[001] End
                            }
                        }
                    }

                    // FocusUserControl = true;
                }

                GeosApplication.Instance.Logger.Log("Method EditBoxCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditBoxCommandAction...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void HideHeaderPanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);
                if (IsHeaderVisible == Visibility.Collapsed)
                {
                    IsHeaderVisible = Visibility.Visible;
                    IsCheckedToggleButton = true;
                }
                else
                {
                    IsHeaderVisible = Visibility.Collapsed;
                    IsCheckedToggleButton = false;
                }


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][26.06.2024][GEOS2-5705]
        private void SlideDataForward(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SlideDataForward ...", category: Category.Info, priority: Priority.Low);
                if (isNextButtonNeedToWork)
                {
                    return;
                }
                else
                {
                    // Calculate the remaining items after the current index
                    int remainingItems = OTItemListForLeftGroup.Count - (CurrentRecordIndex + NumberOfRecordsToShow);

                    if (remainingItems > 0)
                    {
                        // Increment by NumberOfRecordsToShow if there are still more items to show
                        CurrentRecordIndex += NumberOfRecordsToShow;
                    }
                    else
                    {
                        // Move to the last few items if less than NumberOfRecordsToShow remaining
                        CurrentRecordIndex = OTItemListForLeftGroup.Count - NumberOfRecordsToShow;
                        if (CurrentRecordIndex < 0)
                        {
                            CurrentRecordIndex = 0;
                        }
                    }

                    UpdateVisibleRecords();
                }



            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SlideDataForward()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][26.06.2024][GEOS2-5705]
        private void SlideDataBackward(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SlideDataBackward ...", category: Category.Info, priority: Priority.Low);


                if (CurrentRecordIndex > 0)
                {
                    CurrentRecordIndex = Math.Max(0, CurrentRecordIndex - NumberOfRecordsToShow);
                }

                UpdateVisibleRecords();

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SlideDataBackward()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }



        private void OnHandleEnterKey(MouseEventArgs e)
        {
            IsUnpackedSearchSelected = true;
        }
        private void OnHandleLeaveKey(MouseEventArgs e)
        {
            IsUnpackedSearchSelected = false;
        }
        #endregion
    }
}
