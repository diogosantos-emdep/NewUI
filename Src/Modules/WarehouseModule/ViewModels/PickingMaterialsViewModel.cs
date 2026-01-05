using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Map;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Data.Common.WMS;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.CustomControls.ViewModels;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class PickingMaterialsViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region TaskLog

        /// <summary>
        /// [001][WMS M049-16][adadibathina] Print Label in picking
        /// [002][WMS M055-06][adadibathina] Add OK and NOK beeps in picking storage and transfer
        /// [003][WMS M055-10][adadibathina] Batch print of label in picking
        /// [004][GEOS2-190] Do not allow to scan items without Producer [adadibathina]
        /// [005][GEOS2-252] (#65802) FIFO ON/OFF in picking [adadibathina]
        /// [005][GEOS2-1562] Register working time in Picking [sdesai]
        /// [006][GEOS2-1654] Add Serial number in WO label [skhade] - Set Batch Label OFF when SN print.
        /// [007][26-11-2019][skhade][GEOS2-1857] It has been possible to pick more quantity than the required in the item
        /// </summary>

        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }


       //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");

        #endregion //End Of Services

        #region Declaration

        //private List<OtItem> otItemList;
        //private PickingMaterials selectedpickingMaterial;
        private Visibility isLocationIndicatorVisible;
        private Visibility isInformationVisible;
        private string bgColorLocation;
        private string barcodeStr;
        private string locationName;
        private bool isLocationScaned;
        private string rowBgColor;
        private string totalStockBgColor;
        private string totalStockFgColor;
        private Visibility lockedStockIconVisibility;
        private string onFocusBgColor;
        private string onFocusFgColor;
        List<string> articleLocationList = new List<string>();
        private Int64 totalCurrentItemStock;
        private Int64 currentItemLockedStock;
        private ImageSource articleImage;
        private WMSPickingMaterials currentPickingMaterial;


        int currentLocationIndex = 0;
        int countLocation = 0;
        int indexItem = 0;
        //public List<PickingMaterials> materialSoredList = new List<PickingMaterials>();
        //public List<PickingMaterials> UnspectedMaterialList = new List<PickingMaterials>();
        public List<WMSPickingMaterials> materialSoredList = new List<WMSPickingMaterials>();
        public List<WMSPickingMaterials> UnspectedMaterialList = new List<WMSPickingMaterials>();

        bool isNextButtonEnable;
        bool isLocationLast;

        private int trackBarEditvalue;
        private string trackBarFgColor;
        private string fgColorLocation;
        private double windowWidth;
        private double windowHeight;
        private string wrongLocation;
        private string wrongItem;
        private Visibility isWrongLocationErrorVisible = Visibility.Hidden;
        private Visibility isWrongItemErrorVisible = Visibility.Hidden;
        private string prnFilePath;
        private bool focusUserControl;

        private List<WarehouseLocation> warehouseLocations;
        private ObservableCollection<WarehouseLocation> mapItems;
        private Uri svgUri;
        private SerialNumber selectedSerialNumber;
        private bool isScanSerialNumber;
        private string materialSerialNumbers;

        private bool printBatchLabelAfterScanCompleted;
        private bool printBatchLabelPreviousValue;
        private bool printBatchLabelEnabled;
        //string RevisionItemQuantity;

        private bool isBackButtonEnable;
        enum LocationIndex { NextLocation, PreviousLocation }
        Visibility isProducerVisible = Visibility.Collapsed;
        //private Int64 locationStock;
        private string locationStockBgColor;
        private string locationStockFgColor;
        private ArticleWarehouseLocations progressbarArticleStock;
        private ArticleWarehouseLocations progressbarLocationStock;
        private long locationStockValue;
        private long BarcodeScannedQuantity;
        //private List<string> readMeEntriesTitleList;
        private Visibility readMeEntriesTitleVisibility = Visibility.Visible;
        private string readMeEntriesTitle;
        private bool followFIFO = true;
        private byte[] printFile = null;
        private string fifoGotoItem;
        private bool isTimer;
        DispatcherTimer timer = new DispatcherTimer();
        private TimeSpan otWorkingTime;
        private int symbolCount;
        private int timerCount = 0;
        private string infoTooltipBackColor;
        private long idWareHouseDeliveryNoteItem;
        bool autoEnableFIFOProcessStarted = false;
        private Ots oT;
        public Ots OT
        {
            get { return oT; }
            set
            {
                oT = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
            }
        }
        private bool isSaveChanges;
        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
            }
        }
        private List<WorkflowStatus> workflowStatusButtons;
        public List<WorkflowStatus> WorkflowStatusButtons
        {
            get { return workflowStatusButtons; }
            set
            {
                workflowStatusButtons = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusButtons"));
            }
        }
        private List<WorkflowTransition> workflowTransition;
        private List<WorkflowStatus> workflowStatusList;
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
        WorkflowStatus workflowstatus;
        WorkflowStatus oldWorkflowStatus;
        public WorkflowStatus WorkflowStatus
        {
            get { return workflowstatus; }
            set
            {
                workflowstatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatus"));
            }
        }
        public WorkflowStatus OldWorkflowStatus
        {
            get { return oldWorkflowStatus; }
            set
            {
                oldWorkflowStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldWorkflowStatus"));
            }
        }
        #endregion

        #region Public Properties

        public bool IsCanceled { get; set; }
        public Visibility IsWrongLocationErrorVisible
        {
            get { return isWrongLocationErrorVisible; }
            set
            {
                isWrongLocationErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongLocationErrorVisible"));
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

        public string WrongLocation
        {
            get { return wrongLocation; }
            set
            {
                wrongLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WrongLocation"));
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

        public int TrackBarEditvalue
        {
            get { return trackBarEditvalue; }
            set
            {
                trackBarEditvalue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrackBarEditvalue"));
            }
        }

        public string TrackBarFgColor
        {
            get { return trackBarFgColor; }
            set
            {
                trackBarFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrackBarFgColor"));
            }
        }

        public string FgColorLocation
        {
            get { return fgColorLocation; }
            set
            {
                fgColorLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FgColorLocation"));
            }
        }

        public Ots PickingOt { get; set; }

        public Visibility IsLocationIndicatorVisible
        {
            get { return isLocationIndicatorVisible; }
            set
            {
                isLocationIndicatorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocationIndicatorVisible"));
            }
        }

        public Visibility IsInformationVisible
        {
            get { return isInformationVisible; }
            set
            {
                isInformationVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInformationVisible"));
            }
        }

        public string BgColorLocation
        {
            get { return bgColorLocation; }
            set
            {
                bgColorLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BgColorLocation"));
            }
        }

        public bool IsLocationScaned
        {
            get { return isLocationScaned; }
            set
            {
                isLocationScaned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocationScaned"));
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

        public string LocationName
        {
            get { return locationName; }
            set
            {
                locationName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationName"));
            }
        }

        public string RowBgColor
        {
            get { return rowBgColor; }
            set
            {
                rowBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RowBgColor"));
            }
        }

        public string TotalStockBgColor
        {
            get { return totalStockBgColor; }
            set
            {
                totalStockBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalStockBgColor"));
            }
        }

        public string TotalStockFgColor
        {
            get { return totalStockFgColor; }
            set
            {
                totalStockFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalStockFgColor"));
            }
        }

        public Visibility LockedStockIconVisibility
        {
            get { return lockedStockIconVisibility; }
            set
            {
                lockedStockIconVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LockedStockIconVisibility"));
            }
        }

        //public List<OtItem> OtItemList
        //{
        //    get { return otItemList; }
        //    set
        //    {
        //        otItemList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("OtItemList"));
        //    }
        //}
        private List<WMSItemScan> otItemList;
        public List<WMSItemScan> OtItemList
        {
            get { return otItemList; }
            set
            {
                otItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItemList"));
            }
        }
        //public PickingMaterials SelectedpickingMaterial
        //{
        //    get { return selectedpickingMaterial; }
        //    set
        //    {
        //        selectedpickingMaterial = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedpickingMaterial"));
        //    }
        //}

        //public List<PickingMaterials> SelectedLocationList
        //{
        //    get { return selectedLocationList; }
        //    set
        //    {
        //        selectedLocationList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocationList"));
        //    }
        //}

        private WMSPickingMaterials selectedpickingMaterial;
        public WMSPickingMaterials SelectedpickingMaterial
        {
            get { return selectedpickingMaterial; }
            set
            {
                selectedpickingMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedpickingMaterial"));
            }
        }

        private List<WMSPickingMaterials> selectedLocationList;
        public List<WMSPickingMaterials> SelectedLocationList
        {
            get { return selectedLocationList; }
            set
            {
                selectedLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocationList"));
            }
        }


        public List<string> ArticleLocationList
        {
            get { return articleLocationList; }
            set
            {
                articleLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleLocationList"));
            }
        }

        public Int64 TotalCurrentItemStock
        {
            get { return totalCurrentItemStock; }
            set
            {
                totalCurrentItemStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCurrentItemStock"));
            }
        }

        public Int64 CurrentItemLockedStock
        {
            get { return currentItemLockedStock; }
            set
            {
                currentItemLockedStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentItemLockedStock"));
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

        public string OnFocusBgColor
        {
            get { return onFocusBgColor; }
            set
            {
                onFocusBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OnFocusBgColor"));
            }
        }

        public string OnFocusFgColor
        {
            get { return onFocusFgColor; }
            set
            {
                onFocusFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OnFocusFgColor"));
            }
        }

        public bool IsNextButtonEnable
        {
            get { return isNextButtonEnable; }
            set
            {
                isNextButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNextButtonEnable"));
            }
        }

        public List<WarehouseLocation> WarehouseLocations
        {
            get { return warehouseLocations; }
            set
            {
                warehouseLocations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocations"));
            }
        }

        public ObservableCollection<WarehouseLocation> MapItems
        {
            get { return mapItems; }
            set
            {
                mapItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MapItems"));
            }
        }

        public Uri SvgUri
        {
            get { return svgUri; }
            set
            {
                svgUri = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SvgUri"));
            }
        }

        public SerialNumber SelectedSerialNumber
        {
            get { return selectedSerialNumber; }
            set
            {
                selectedSerialNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSerialNumber"));
            }
        }

        public bool IsScanSerialNumber
        {
            get { return isScanSerialNumber; }
            set
            {
                isScanSerialNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsScanSerialNumber"));
            }
        }

        public bool PrintBatchLabelAfterScanCompleted
        {
            get { return printBatchLabelAfterScanCompleted; }
            set
            {
                printBatchLabelAfterScanCompleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PrintBatchLabelAfterScanCompleted"));
            }
        }

        //006
        public bool PrintBatchLabelPreviousValue
        {
            get { return printBatchLabelPreviousValue; }
            set
            {
                printBatchLabelPreviousValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PrintBatchLabelPreviousValue"));
            }
        }

        //006
        public bool PrintBatchLabelEnabled
        {
            get { return printBatchLabelEnabled; }
            set
            {
                printBatchLabelEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PrintBatchLabelEnabled"));
            }
        }

        public bool IsBackButtonEnable
        {
            get { return isBackButtonEnable; }
            set
            {
                isBackButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBackButtonEnable"));
            }
        }

        public Visibility IsProducerVisible
        {
            get { return isProducerVisible; }
            set
            {
                isProducerVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsProducerVisible"));
            }
        }

        public string LocationStockBgColor
        {
            get { return locationStockBgColor; }
            set
            {
                locationStockBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationStockBgColor"));
            }
        }

        public string LocationStockFgColor
        {
            get { return locationStockFgColor; }
            set
            {
                locationStockFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationStockFgColor"));
            }
        }

        public ArticleWarehouseLocations ProgressbarArticleStock
        {
            get { return progressbarArticleStock; }
            set
            {
                progressbarArticleStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressbarArticleStock"));
            }
        }

        public ArticleWarehouseLocations ProgressbarLocationStock
        {
            get { return progressbarLocationStock; }
            set
            {
                progressbarLocationStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressbarLocationStock"));
            }
        }

        public long LocationStockValue
        {
            get { return locationStockValue; }
            set
            {
                locationStockValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationStockValue"));
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
        //public List<string> ReadMeEntriesTitleList
        //{
        //    get
        //    {
        //        return readMeEntriesTitleList;
        //    }

        //    set
        //    {
        //        readMeEntriesTitleList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ReadMeEntriesTitleList"));
        //    }
        //}
        public Visibility ReadMeEntriesTitleVisibility
        {
            get
            {
                return readMeEntriesTitleVisibility;
            }

            set
            {
                readMeEntriesTitleVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReadMeEntriesTitleVisibility"));
            }
        }
        public string ReadMeEntriesTitle
        {
            get
            {
                return readMeEntriesTitle;
            }

            set
            {
                readMeEntriesTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReadMeEntriesTitle"));
            }
        }

        public SymbolsAnimation MatrixView5x8Animation { get; set; }

        public bool FollowFIFO
        {
            get { return followFIFO; }
            set
            {
                followFIFO = value;
                ChangePickingMethodology();
                OnPropertyChanged(new PropertyChangedEventArgs("FollowFIFO"));
            }
        }

        public string FIFOGotoItem
        {
            get { return fifoGotoItem; }
            set
            {
                fifoGotoItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FIFOGotoItem"));
            }
        }
        public bool IsTimer
        {
            get
            {
                return isTimer;
            }

            set
            {
                isTimer = value;
                IsInactivityTimer = value;
                if (isTimer)
                {
                    timerCount = 1;
                    AddOTWorkingTime();
                    OtWorkingTime = WarehouseService.GetOTTotalWorkingTime(PickingOt.IdOT, 12, WarehouseCommon.Instance.Selectedwarehouse);
                    UpdateTime();
                    timer.Tick += new EventHandler(OnTimedEvent);
                    timer.Interval = new TimeSpan(0, 1, 0);
                    timer.Start();
                }
                else
                {
                    AddOTWorkingTime();
                    timer.Tick -= new EventHandler(OnTimedEvent);
                    timer.Interval = new TimeSpan(0, 1, 0);
                    timer.Stop();
                }

                OnPropertyChanged(new PropertyChangedEventArgs("IsTimer"));
            }
        }

        public DigitalGaugeControl digitalGaugeControl { get; set; }
        public TimeSpan OtWorkingTime
        {
            get
            {
                return otWorkingTime;
            }

            set
            {
                otWorkingTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtWorkingTime"));
            }
        }
        public int SymbolCount
        {
            get
            {
                return symbolCount;
            }

            set
            {
                symbolCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SymbolCount"));
            }
        }
        public bool PreviousTimerValue { get; set; }
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
        public long IdWareHouseDeliveryNoteItem
        {
            get
            {
                return idWareHouseDeliveryNoteItem;
            }

            set
            {
                idWareHouseDeliveryNoteItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdWareHouseDeliveryNoteItem"));
            }
        }
        //Shubham[skadam] GEOS2-4436 Ask for location inventory after do the picking of certain references (2/3)  23 10 2023 
        public bool IsSameReference { get; set; }
        public bool IsSameReferenceWithRequiresCountAfterPicking { get; set; }
        InventoryWizzardView inventoryWizzardView = new InventoryWizzardView();
        InventoryWizzardViewModel inventoryWizzardViewModel = new InventoryWizzardViewModel();
        private List<WMSPickingMaterials> selectedLocationListForInventoryWizzard;
        public List<WMSPickingMaterials> SelectedLocationListForInventoryWizzard
        {
            get { return selectedLocationListForInventoryWizzard; }
            set
            {
                selectedLocationListForInventoryWizzard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocationListForInventoryWizzard"));
            }
        }
        private Int64 locationAvaibleQuantityForInventoryWizzard;
        public Int64 LocationAvaibleQuantityForInventoryWizzard
        {
            get { return locationAvaibleQuantityForInventoryWizzard; }
            set
            {
                locationAvaibleQuantityForInventoryWizzard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationAvaibleQuantityForInventoryWizzard"));
            }
        }

        //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
        List<GeosAppSetting> geosAppSettingList;
        public List<GeosAppSetting> GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }
        DateTime startTime;
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartTime"));
            }
        }

        bool isStartTimer;
        public bool IsStartTimer
        {
            get
            {
                return isStartTimer;
            }

            set
            {
                isStartTimer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStartTimer"));
            }
        }

        DispatcherTimer Inactivitytimer = new DispatcherTimer();
        bool isInactivityTimer;
        private int inactivitytimerCount = 0;
        public bool IsInactivityTimer
        {
            get
            {
                return isInactivityTimer;
            }
            set
            {
                isInactivityTimer = value;

                if (isTimer)
                {
                    inactivitytimerCount = 1;
                    Inactivitytimer.Tick += new EventHandler(OnInactivityTimedEvent);
                    Inactivitytimer.Interval = new TimeSpan(0, 0, 10);
                    Inactivitytimer.Start();
                }
                else
                {
                    Inactivitytimer.Tick -= new EventHandler(OnInactivityTimedEvent);
                    Inactivitytimer.Interval = new TimeSpan(0, 0, 10);
                    Inactivitytimer.Stop();
                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsInactivityTimer"));
            }
        }

        string employeeCode;
        public string EmployeeCode
        {
            get
            {
                return employeeCode;
            }

            set
            {
                employeeCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeCode"));
            }
        }

       // [pallavi jadhav][GEOS2-7021][10 - 04 - 2025]
        private List<SendNotificationMail> sendNotificationMailList;
        public List<SendNotificationMail> SendNotificationMailList
        {
            get
            {
                return sendNotificationMailList;
            }

            set
            {
                sendNotificationMailList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SendNotificationMailList"));
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

        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandNextButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandOnLoaded { get; set; }
        public ICommand CommandBackButton { get; set; }
        public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }
        public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        public ICommand HyperlinkClickCommand { get; set; }
        public ICommand DigitalGaugeControlLoadedCommand { get; set; }
        public ICommand ImageClickCommand { get; set; }
        public ICommand CommandKeyDown { get; set; }

        //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
        public ICommand MouseMoveCommand { get; set; }

        public ICommand EnterKeyCommand { get; set; }

        public ICommand PreviewKeyDownCommand { get; set; }

        public ICommand SendNotificationMailCommand { get; set; } //[Pallavi jadhav][GEOS2-7021][07 04 2025]
        #endregion // End Of ICommands

        #region Constructor

        public PickingMaterialsViewModel()
        {
            WindowHeight = System.Windows.SystemParameters.WorkArea.Height - 131;
            WindowWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandNextButton = new DelegateCommand<object>(NextButtonCommandAction);
            CommandBackButton = new DelegateCommand<object>(BackButtonCommandAction);
            CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
            CommandOnLoaded = new DelegateCommand(LoadedAction);
            VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
            CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);
            HyperlinkClickCommand = new DelegateCommand<object>(HyperlinkClickCommandAction);
            DigitalGaugeControlLoadedCommand = new DelegateCommand<object>(DigitalGaugeControlLoadedCommandAction);
            CommandKeyDown = new DelegateCommand<object>(CommandKeyDownAction);
            ImageClickCommand = new DelegateCommand<object>(ImageClickCommandAction);
            SendNotificationMailCommand = new DelegateCommand<object>(SendNotificationMailCommandAction); //[pallavi jadhav][GEOS2-7021][10-04-2025]
            IsLocationIndicatorVisible = Visibility.Visible;
            IsInformationVisible = Visibility.Collapsed;
            LockedStockIconVisibility = Visibility.Hidden;
            IsLocationScaned = false;
            IsNextButtonEnable = true;
            IsBackButtonEnable = false;
            TrackBarEditvalue = 0;
            PrintBatchLabelEnabled = true;
            #region GEOS2-5168
            try
            {
                //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
                StartTime = DateTime.Now;
                MouseMoveCommand = new DelegateCommand<MouseEventArgs>(MouseMoveAction);
                EnterKeyCommand = new DelegateCommand<object>(EnterKeyAction);
                PreviewKeyDownCommand = new DelegateCommand<object>(PreviewKeyDownAction);
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PickingTimer"))
                {
                    if (Convert.ToBoolean(GeosApplication.Instance.UserSettings["PickingTimer"].ToString()))
                        IsStartTimer = true;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in PickingMaterialsViewModel(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 06 05 2024
            EmployeeCode = WarehouseService.GetEmployeeCodeByIdUser(GeosApplication.Instance.ActiveUser.IdUser);
            #endregion
        }



        #endregion

        #region Methods 

        /// <summary>
        /// Method for fill data.
        /// [001][21-02-2019][skhade][WMS M056-21 Allow to scan item in picking]
        /// [002][31-01-2020][cpatil][GEOS2-2025]Barcode scan do nothing in item 01CTC1.17B in picking
        /// </summary>
        /// <param name="Ot">The Ot object</param>
        /// <param name="objWarehouse">The warehouse object.</param>
        /// <param name="workOrderItem">The workorder item.</param>
        public void InIt(Ots Ot, Warehouses objWarehouse, string workOrderItem = null)
        {
            try
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

                GeosApplication.Instance.Logger.Log("Method InIt....", category: Category.Info, priority: Priority.Low);

                FillPikingNotificationMails(objWarehouse);

                PickingOt = (Ots)Ot.Clone();

                if (PickingOt.CountryGroup != null && !string.IsNullOrEmpty(PickingOt.CountryGroup.Name))
                    IsProducerVisible = Visibility.Visible;
                UnspectedMaterialList = new List<WMSPickingMaterials>();
                PickingOt.Status = WarehouseService.GetWorkorderStatus(PickingOt.IdOT);
                PickingMethodology(Ot, objWarehouse);
                prnFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\WO_item_Label.prn";
                ArticleLocationList = new List<string>();
                WarehouseLocations = new List<WarehouseLocation>();
                MapItems = new ObservableCollection<WarehouseLocation>();
                //set info tooltip back color
                GeosAppSetting GeosAppSetting = WorkbenchService.GetGeosAppSettings(37);


                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;

                foreach (var item in OtItemList)
                {
                    foreach (var material in item.PickingMaterialsList)
                    {
                        if (!ArticleLocationList.Contains(material.LocationFullName))
                            ArticleLocationList.Add(material.LocationFullName);

                        if (!WarehouseLocations.Any(x => x.IdWarehouseLocation == material.IdWarehouseLocation))
                        {
                            WarehouseLocations.Add(material.WarehouseLocation);
                        }
                        //[002]
                        material.RevisionItem = item.RevisionItem;
                        try
                        {
                            //Shubham[skadam] GEOS2-4437 & GEOS2-4436 03 11 2023
                            material.Article.IsCountRequiredAfterPicking = item.RevisionItem.Article.IsCountRequiredAfterPicking;
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in InIt...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        materialSoredList.Add(material);
                    }
                    //[rdixit][GEOS2-4930][31.10.2023]
                    if (item.NotInspectedPickingMaterialsList != null || item.ArticleDecomposedList != null)
                    {
                        if (item.NotInspectedPickingMaterialsList?.Count > 0)
                        {
                            foreach (var material in item.NotInspectedPickingMaterialsList)
                            {
                                try
                                {
                                    //Shubham[skadam] GEOS2-4437 & GEOS2-4436 03 11 2023
                                    material.Article.IsCountRequiredAfterPicking = item.RevisionItem.Article.IsCountRequiredAfterPicking;
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in InIt...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }
                                if (!UnspectedMaterialList.Any(i => i.Reference == material.Reference))
                                    UnspectedMaterialList.Add(material);
                            }
                        }
                        if (item.ArticleDecomposedList?.Count > 0)
                        {
                            foreach (var DecomposedArticle in item.ArticleDecomposedList)
                            {
                                if (DecomposedArticle.NotInspectedPickingMaterialsList?.Count > 0)
                                {
                                    foreach (var material in DecomposedArticle.NotInspectedPickingMaterialsList)
                                    {
                                        try
                                        {
                                            //Shubham[skadam] GEOS2-4437 & GEOS2-4436 03 11 2023
                                            material.Article.IsCountRequiredAfterPicking = item.RevisionItem.Article.IsCountRequiredAfterPicking;
                                        }
                                        catch (Exception ex)
                                        {
                                            GeosApplication.Instance.Logger.Log("Get an error in InIt...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                                        }
                                        if (!UnspectedMaterialList.Any(i => i.Reference == material.Reference))
                                            UnspectedMaterialList.Add(material);
                                    }
                                }
                            }
                        }
                    }
                    //Nested decomposition articles.
                    if (item.ArticleDecomposedList != null && item.ArticleDecomposedList.Count > 0)
                    {
                        DecompositionArticles_V2500(item.ArticleDecomposedList);
                    }
                }

                //M059 - 07
                ArticleLocationList = ArticleLocationList.OrderByDescending(n => n.ToUpper() == "TRANSIT").ThenBy(n => n).ToList();

                if (ArticleLocationList != null && ArticleLocationList.Count > 0)
                {
                    ///[001] Added to redirect to the first picking location for the item.
                    if (!string.IsNullOrEmpty(workOrderItem))
                    {
                        string otItemNumber = (workOrderItem.Substring(workOrderItem.Length - 3)).TrimStart('0');
                        WMSItemScan otitem = OtItemList.FirstOrDefault(x => x.RevisionItem.NumItem == otItemNumber);

                        if (otitem != null)
                        {
                            List<string> orderedLocations = otitem.PickingMaterialsList.Select(person => person.LocationFullName).OrderBy(name => name).ToList();
                            string firstLocation = orderedLocations.FirstOrDefault();

                            currentLocationIndex = ArticleLocationList.IndexOf(firstLocation);
                            currentLocationIndex = currentLocationIndex == -1 ? currentLocationIndex = 0 : currentLocationIndex;

                            SetBackButtonEnable();
                            SetNextButtonEnable();
                        }
                    }

                    if (!string.IsNullOrEmpty(FIFOGotoItem))
                    {
                        // shubham[skadam] GEOS2-3260 Add TRANSIT location when FIFO is disabled   09 Aug 2022
                        materialSoredList = materialSoredList.OrderByDescending(n => n.LocationFullName.ToUpper() == "TRANSIT").ThenBy(n => n.RevisionItem.NumItem).ToList();
                        WMSPickingMaterials pickingMaterial = materialSoredList.FirstOrDefault(x => x.RevisionItem.NumItem == FIFOGotoItem);
                        if (pickingMaterial != null)
                        {
                            LocationName = pickingMaterial.LocationFullName;
                            SelectedLocationList = materialSoredList.Where(pm => pm.LocationFullName == LocationName).Select(p => p).ToList();

                            currentLocationIndex = ArticleLocationList.IndexOf(LocationName);
                            currentLocationIndex = currentLocationIndex == -1 ? currentLocationIndex = 0 : currentLocationIndex;

                            SetBackButtonEnable();
                            SetNextButtonEnable();
                        }
                        else
                        {
                            LocationName = ArticleLocationList[currentLocationIndex].ToString();
                            SelectedLocationList = materialSoredList.Where(pm => pm.LocationFullName == LocationName).Select(p => p).ToList();
                        }
                    }
                    else
                    {
                        LocationName = ArticleLocationList[currentLocationIndex].ToString();
                        SelectedLocationList = materialSoredList.Where(pm => pm.LocationFullName == LocationName).Select(p => p).ToList();
                    }



                    FillMapLocation();

                    if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                    {
                        BgColorLocation = "#FF083493";
                        RowBgColor = "PINK";
                        FgColorLocation = "White";
                        TrackBarFgColor = "#FF083493";
                    }
                    else
                    {
                        FgColorLocation = "Black";
                        BgColorLocation = "#FF2AB7FF";
                        RowBgColor = "#FFFFFFFF";
                        TrackBarFgColor = "#FF2AB7FF";
                    }
                }
                else
                {
                    /* IsNextButtonEnable = false;*/ // if location count is zero then next button will be disabled.

                    if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                    {
                        TrackBarFgColor = "#FF083493";
                    }
                    else
                    {
                        TrackBarFgColor = "#FF2AB7FF";
                    }
                }

                if (SelectedLocationList != null && SelectedLocationList.Count > 0)
                    TotalCurrentItemStock = SelectedLocationList.First().CurrentStock;


                if (!string.IsNullOrEmpty(Ot.Comments))
                {
                    ReadMeEntriesTitle = Ot.Comments;
                    ReadMeEntriesTitle = ReadMeEntriesTitle.Replace(System.Environment.NewLine, " | ").Trim(' ', '|', ' ').ToUpper();

                    if (!string.IsNullOrEmpty(ReadMeEntriesTitle))
                    {
                        if (ReadMeEntriesTitle.Length <= 40)
                        {
                            MatrixView5x8Animation = new BlinkingAnimation() { RefreshTime = new TimeSpan(0, 0, 1) };
                            SymbolCount = ReadMeEntriesTitle.Length;
                        }
                        else
                        {
                            MatrixView5x8Animation = new CreepingLineAnimation() { RefreshTime = new TimeSpan(0, 0, 0, 0, 200), Repeat = true };
                            SymbolCount = 54;
                        }
                    }
                    else
                        ReadMeEntriesTitleVisibility = Visibility.Collapsed;
                }
                else
                    ReadMeEntriesTitleVisibility = Visibility.Collapsed;


                IsWrongLocationErrorVisible = Visibility.Hidden;
                IsWrongItemErrorVisible = Visibility.Hidden;
                WrongLocation = "";
                WrongItem = "";
                SetNextButtonEnable();

                //rajashri[GEOS2-4849][28.10.2023]--retriving status from OT popup to scan window(means if status is "To do" retriving "TO do" etc).
                WorkflowTransitionList = new List<WorkflowTransition>(WarehouseService.GetAllWorkflowTransitions_V2320());
                WorkflowStatusList = new List<WorkflowStatus>(WarehouseService.GetAllWorkflowStatus_V2320());
                WorkflowStatus = new WorkflowStatus();
                Ot.WorkflowStatus = new WorkflowStatus();
                if (Ot.IdWorkflowStatus > 0)
                {
                    WorkflowStatus = WorkflowStatusList.Where(i => i.IdWorkflowStatus == Ot.IdWorkflowStatus).FirstOrDefault();
                    Ot.WorkflowStatus = WorkflowStatus;
                }
                ScanAction(0, 0, Ot.WorkflowStatus.IdWorkflowStatus);
                //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
                FillGeosAppSettingList();
                StartTime = DateTime.Now;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method InIt....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InIt...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SetFollowFIFO(bool followFIFO, string FIFOGotoItem)
        {
            this.followFIFO = followFIFO;
            OnPropertyChanged(new PropertyChangedEventArgs("FollowFIFO"));

            this.FIFOGotoItem = FIFOGotoItem;
            OnPropertyChanged(new PropertyChangedEventArgs("FIFOGotoItem"));
        }

        private void ChangePickingMethodology(bool ShowQuestionMsgToAutoEnableFIFO = false)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangePickingMethodology....", category: Category.Info, priority: Priority.Low);

                //006 - Added to ON Batch label if user selected as ON.
                if (IsScanSerialNumber && !PrintBatchLabelAfterScanCompleted && PrintBatchLabelPreviousValue)
                {
                    PrintBatchLabelAfterScanCompleted = true;
                }

                PrintBatchLabelEnabled = true;

                if (ShowQuestionMsgToAutoEnableFIFO)
                {
                    if (!FollowFIFO)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                        string message = System.Windows.Application.Current.FindResource("PickingMaterialDoYouWantToEnableFIFO").ToString();

                        CustomMessageBox.Show(message, Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);

                        if (CustomMessageBox.msgboxresult == MessageBoxResult.Yes)
                        {
                            autoEnableFIFOProcessStarted = true;
                            FollowFIFO = true;
                            autoEnableFIFOProcessStarted = false;
                        }
                    }
                }
                else if (autoEnableFIFOProcessStarted)
                {
                    EnableFIFOPickingMethodolgy();
                }
                else
                {
                    string message = FollowFIFO ? System.Windows.Application.Current.FindResource("PickingMaterialFIFOEnable").ToString()
                                                : System.Windows.Application.Current.FindResource("PickingMaterialFIFODisabled").ToString();

                    CustomMessageBox.Show(message, Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);

                    if (CustomMessageBox.msgboxresult == MessageBoxResult.Yes)
                    {

                        EnableFIFOPickingMethodolgy();

                    }
                    else
                    {
                        SetFollowFIFO(!FollowFIFO, null);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangePickingMethodology....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangePickingMethodology...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EnableFIFOPickingMethodolgy()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method EnableFIFOPickingMethodolgy....", category: Category.Info, priority: Priority.Low);

                string numItems = "";
                string FullLocationName = "";

                if (SelectedpickingMaterial != null)
                {
                    FIFOGotoItem = SelectedpickingMaterial.RevisionItem.NumItem;
                    numItems = SelectedpickingMaterial.RevisionItem.NumItem;
                    FullLocationName = SelectedpickingMaterial.LocationFullName;
                }
                else if (SelectedSerialNumber != null && SelectedSerialNumber.MasterItem != null)
                {
                    FIFOGotoItem = ((WMSPickingMaterials)SelectedSerialNumber.MasterItem).RevisionItem.NumItem;
                    numItems = ((WMSPickingMaterials)SelectedSerialNumber.MasterItem).RevisionItem.NumItem;
                }
                else if (SelectedLocationList != null && SelectedLocationList.Count > 0)
                {
                    FIFOGotoItem = SelectedLocationList.FirstOrDefault().RevisionItem.NumItem;
                    numItems = string.Join(",", SelectedLocationList.Select(i => i.RevisionItem.NumItem).Distinct().ToArray());
                    FullLocationName = string.Join(",", SelectedLocationList.Select(i => i.LocationFullName).Distinct().ToArray()); //SelectedLocationList.Select(x => x.WarehouseLocation.FullName).ToString();
                }
                else
                {
                    FIFOGotoItem = null;
                }

                if (numItems.Contains(","))
                {
                    numItems = "Items " + numItems;
                }
                else
                {
                    numItems = "Item " + numItems;
                }

                Notification notification = new Notification();
                notification.IsNew = 1;
                notification.IdModule = 6;
                notification.Status = "Unread";
                notification.Title = FollowFIFO ? "FIFO has been enabled in picking" : "FIFO has been disabled in picking";
                notification.FromUser = GeosApplication.Instance.ActiveUser.IdUser;
                notification.Message = FollowFIFO ? "FIFO has been enabled in picking for OT " + PickingOt.Code + " by " + GeosApplication.Instance.ActiveUser.FullName + "."
                                                  : "FIFO has been disabled in picking for OT " + PickingOt.Code + ", " + numItems + ", at location " + FullLocationName + ", by " + GeosApplication.Instance.ActiveUser.FullName + ".";
                WarehouseService.AddNotification(notification);
                PreviousTimerValue = IsTimer;
                if (IsTimer)
                    IsTimer = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method EnableFIFOPickingMethodolgy....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EnableFIFOPickingMethodolgy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Selected PickingMethodology
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][cpatil][29-08-2019][GEOS2-1666]FIFO error
        /// [003][cpatil][17-09-2020][GEOS2-2415]Add Date of Expiry in Article comments
        ///  [004][cpatil][21-09-2023][GEOS2-4417]
        /// </summary>
        /// <param name="Ot"></param>
        /// <param name="objWarehouse"></param>
        private void PickingMethodology(Ots Ot, Warehouses objWarehouse)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PickingMethodology....", category: Category.Info, priority: Priority.Low);

                if (FollowFIFO)
                {
                    #region ServiceComments
                    // Service Method Changed from GetRemainingOtItemsByIdOt_V2150 to GetRemainingOtItemsByIdOt_V2360 [06.02.2023][rdixit][GEOS2-3605]
                    // Service Method Changed from GetRemainingOtItemsByIdOt_V2360 to GetRemainingOtItemsByIdOt_V2390 [18.05.2023][rdixit][GEOS2-4411]
                    // OtItemList = WarehouseService.GetRemainingOtItemsByIdOt_V2390(Ot.IdOT, objWarehouse);[Sudhir.Jangra][GEOS2-4540][17/08/2023]


                    //[Sudhir.Jangra][GEOS2-4540][17/08/2023]
                    ///  [004]
                    // OtItemList = WarehouseService.GetRemainingOtItemsByIdOt_V2430(Ot.IdOT, objWarehouse);
                    //[Sudhir.Jangra][GEOS2-4544]
                    //Service GetRemainingOtItemsByIdOt_V2440 updated with GetRemainingOtItemsByIdOt_V2450  [rdixit][GEOS2-4930][31.10.2023]
                    #endregion
                    //WarehouseService = new WarehouseServiceController("localhost:6699");
                    //OtItemList = WarehouseService.GetRemainingOtItemsByIdOt_V2450(Ot.IdOT, objWarehouse);
                    //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 20 03 2024
                    //OtItemList = WarehouseService.GetRemainingOtItemsByIdOt_V2500(Ot.IdOT, objWarehouse);

                    OtItemList = WarehouseService.GetRemainingOtItemsByIdOt_V2540(Ot.IdOT, objWarehouse); //[rahul.gadhave] [GEOS2-5676] [16-07-2024]

                }
                else
                {
                    #region ServiceComments
                    // Service Method Changed from GetRemainingOtItemsByIdOtDisbaledFIFO_V2300 to GetRemainingOtItemsByIdOtDisbaledFIFO_V2360 [06.02.2023][rdixit][GEOS2-3605]
                    // Service Method Changed from GetRemainingOtItemsByIdOtDisbaledFIFO_V2360 to GetRemainingOtItemsByIdOtDisbaledFIFO_V2390 [18.05.2023][rdixit][GEOS2-4411]
                    //  OtItemList = WarehouseService.GetRemainingOtItemsByIdOtDisbaledFIFO_V2390(Ot.IdOT, objWarehouse);[Sudhir.Jangra][GEOS2-4540][17/08/2023]


                    //[Sudhir.Jangra][GEOS2-4540][17/08/2023]
                    ///  [004]
                    //  OtItemList = WarehouseService.GetRemainingOtItemsByIdOtDisbaledFIFO_V2430(Ot.IdOT, objWarehouse);
                    //[Sudhir.jangra][GEOS2-4544]
                    //Service GetRemainingOtItemsByIdOtDisbaledFIFO_V2440 updated with GetRemainingOtItemsByIdOtDisbaledFIFO_V2450  [rdixit][GEOS2-4930][31.10.2023]
                    #endregion
                    //OtItemList = WarehouseService.GetRemainingOtItemsByIdOtDisbaledFIFO_V2450(Ot.IdOT, objWarehouse);
                    //WarehouseService = new WarehouseServiceController("localhost:6699");
                    //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 20 03 2024
                    //OtItemList = WarehouseService.GetRemainingOtItemsByIdOtDisbaledFIFO_V2500(Ot.IdOT, objWarehouse);
                    OtItemList = WarehouseService.GetRemainingOtItemsByIdOtDisbaledFIFO_V2540(Ot.IdOT, objWarehouse); //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
                }

                GeosApplication.Instance.Logger.Log("Method PickingMethodology....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMethodology() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMethodology() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMethodology...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void DecompositionArticles(List<OtItem> ArticleDecomposedList)
        {
            foreach (OtItem decomposedItem in ArticleDecomposedList)
            {
                if (decomposedItem.ArticleDecomposedList != null && decomposedItem.ArticleDecomposedList.Count > 0)
                {
                    DecompositionArticles(decomposedItem.ArticleDecomposedList);    //Calling recursively to add 
                }
                else
                {
                    foreach (var decomposedItemPicking in decomposedItem.PickingMaterialsList)
                    {
                        if (!ArticleLocationList.Contains(decomposedItemPicking.LocationFullName))
                            ArticleLocationList.Add(decomposedItemPicking.LocationFullName);

                        if (!WarehouseLocations.Any(x => x.IdWarehouseLocation == decomposedItemPicking.IdWarehouseLocation))
                        {
                            WarehouseLocations.Add(decomposedItemPicking.WarehouseLocation);
                        }

                        decomposedItemPicking.RevisionItem = decomposedItem.RevisionItem;   //Assigned this to maintain the download and remainging qty for decomposition
                        try
                        {
                            //Shubham[skadam] GEOS2-4437 & GEOS2-4436 03 11 2023
                            decomposedItemPicking.Article.IsCountRequiredAfterPicking = decomposedItem.RevisionItem.WarehouseProduct.Article.IsCountRequiredAfterPicking;
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in DecompositionArticles...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        //materialSoredList.Add(decomposedItemPicking);
                    }
                }
            }
        }

        public void DecompositionArticles_V2500(List<WMSItemScan> ArticleDecomposedList)
        {
            foreach (WMSItemScan decomposedItem in ArticleDecomposedList)
            {
                if (decomposedItem.ArticleDecomposedList != null && decomposedItem.ArticleDecomposedList.Count > 0)
                {
                    DecompositionArticles_V2500(decomposedItem.ArticleDecomposedList);    //Calling recursively to add 
                }
                else
                {
                    foreach (var decomposedItemPicking in decomposedItem.PickingMaterialsList)
                    {
                        if (!ArticleLocationList.Contains(decomposedItemPicking.LocationFullName))
                            ArticleLocationList.Add(decomposedItemPicking.LocationFullName);

                        if (!WarehouseLocations.Any(x => x.IdWarehouseLocation == decomposedItemPicking.IdWarehouseLocation))
                        {
                            WarehouseLocations.Add(decomposedItemPicking.WarehouseLocation);
                        }

                        decomposedItemPicking.RevisionItem = decomposedItem.RevisionItem;   //Assigned this to maintain the download and remainging qty for decomposition
                        try
                        {
                            //Shubham[skadam] GEOS2-4437 & GEOS2-4436 03 11 2023
                            decomposedItemPicking.Article.IsCountRequiredAfterPicking = decomposedItem.RevisionItem.Article.IsCountRequiredAfterPicking;
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in DecompositionArticles...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        materialSoredList.Add(decomposedItemPicking);
                    }
                }
            }
        }

        private void LoadedAction()
        {
            //WinUIDialogWindow detailView = ((WinUIDialogWindow)((System.Windows.RoutedEventArgs)obj).OriginalSource);
            //detailView.Focus();

            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.PickingMaterialsUserControlView", null, this);
        }

        /// <summary>
        /// Fill map location and file.
        /// </summary>
        private void FillMapLocation()
        {
            MapItems.Clear();
            SvgUri = null;
            WarehouseLocation wlocation = WarehouseLocations.FirstOrDefault(x => x.FullName == LocationName);

            if (wlocation != null)
            {
                MapItems.Add(wlocation);
                try
                {
                    if (wlocation.FileName == null)
                    {
                        SvgUri = null;
                    }
                    else
                    {
                        string svgFilePath = string.Format(@"{0}\Data\{1}", Path.GetTempPath(), wlocation.FileName);

                        if (File.Exists(svgFilePath))
                        {
                            SvgUri = new Uri(svgFilePath);
                        }
                        else
                        {
                            byte[] warehouseLayout = GeosRepositoryService.GetCompanyLayoutFile(wlocation.FileName);

                            if (warehouseLayout != null)
                            {
                                string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                                if (!Directory.Exists(basePath))
                                {
                                    Directory.CreateDirectory(basePath);
                                }

                                File.WriteAllBytes(svgFilePath, warehouseLayout);
                                SvgUri = new Uri(svgFilePath);
                            }
                        }
                    }
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Rrror in Loading warehouse layout svg file " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Error in Loading warehouse layout svg file - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in Loading warehouse layout svg file - {0}. ErrorMessage- {1}", wlocation.FileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        /// <summary>
        /// Method for do scan opration on scanned barcode.
        /// [001][GEOS2-190]Do not allow to scan items without Producer[adadibathina]
        /// [002][31-01-2020][cpatil][GEOS2-2025]Barcode scan do nothing in item 01CTC1.17B in picking
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            ScanBarcode(obj.Text);
        }


        /// <summary>
        /// [001][avpawar][GEOS2-3371][Block picking when trying to pick more quantity than the unlocked one]
        /// </summary>
        /// <param name="fullBarcodeText"></param>
        private void ScanBarcode(string fullBarcodeText)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
                if (fullBarcodeText == "\r" && !isLocationLast)
                {
                    #region Scan Location


                    IsWrongItemErrorVisible = Visibility.Hidden;
                    WrongItem = "";

                    if (!IsLocationScaned)
                    {
                        if (BarcodeStr == LocationName)
                        {
                            BarcodeStr = "";
                            BgColorLocation = "#FF008000";
                            ScanAction(0, 0, WorkflowStatus.IdWorkflowStatus);
                            IsLocationScaned = true;
                            IsWrongLocationErrorVisible = Visibility.Hidden;
                            WrongLocation = "";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                            if (SelectedLocationList.Any(a => a.ArticleComment != null && a.ArticleComment != ""))
                            {
                                List<PickingComment> List = new List<PickingComment>();
                                foreach (WMSPickingMaterials item in SelectedLocationList.Where(a => a.ArticleComment != null && a.ArticleComment != ""))
                                {
                                    if (!List.Any(a => a.Reference == item.Reference))
                                    {
                                        PickingComment comment = new PickingComment();
                                        comment.Reference = item.Reference;
                                        comment.OTCode = "[" + item.Reference + "]:";
                                        comment.Comment = item.ArticleComment;
                                        List.Add(comment);
                                    }
                                }
                                CustomMessageBox.Show(List, Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                            }
                            #region GEOS2-4436
                            //Shubham[skadam] GEOS2-4436  23 10 2023 
                            try
                            {
                                List<WMSPickingMaterials> tempLockedForOrderPicking = SelectedLocationList.Where(w => w.Article.IsCountRequiredAfterPicking == 1).ToList();
                                List<WMSPickingMaterials> tempmaterialSoredList = materialSoredList.Where(w => w.Article.IsCountRequiredAfterPicking == 1 & w.LocationFullName.Equals(LocationName)).ToList();
                                if (tempLockedForOrderPicking != null)
                                {
                                    foreach (WMSPickingMaterials pickingMaterials in tempLockedForOrderPicking)
                                    {
                                        if (tempLockedForOrderPicking.Where(w => w.IdArticle == pickingMaterials.IdArticle).Count() > 0) // [nsatpute][07-10-2024][GEOS2-6501]
                                        {
                                            IsSameReferenceWithRequiresCountAfterPicking = true;
                                        }
                                    }
                                    //if (IsSameReferenceWithRequiresCountAfterPicking)
                                    //{
                                    //    ISScanInventoryWizzard();
                                    //}
                                }
                                SelectedLocationListForInventoryWizzard = new List<WMSPickingMaterials>();
                                foreach (WMSPickingMaterials PickingMaterialsitem in SelectedLocationList)
                                {
                                    SelectedLocationListForInventoryWizzard.Add(PickingMaterialsitem);
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                            #endregion
                        }
                        else
                        {
                            IsWrongLocationErrorVisible = Visibility.Visible;
                            WrongLocation = "Wrong Location " + BarcodeStr;
                            IsLocationScaned = false;
                            BarcodeStr = "";
                            BgColorLocation = "#FFFF0000";
                            SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }
                    }
                    else if (IsScanSerialNumber)
                    {
                        WMSPickingMaterials pm = SelectedSerialNumber.MasterItem as WMSPickingMaterials;

                        //Observation comment for serial number.
                        if (string.IsNullOrEmpty(materialSerialNumbers))
                            materialSerialNumbers = String.Format(Application.Current.FindResource("MaterialSerialNumbers").ToString() + "\r\n", pm.Reference);

                        if (pm.ScanSerialNumbers.Exists(x => x.Code == BarcodeStr))     //Show error if already scanned item.
                        {
                            IsWrongItemErrorVisible = Visibility.Visible;
                            WrongItem = "Wrong Item " + BarcodeStr;
                            BarcodeStr = string.Empty;
                            RowBgColor = "#FFFF0000";       // color = cream Red
                            OnFocusBgColor = "#FFFF0000";   // color = cream Red
                            OnFocusFgColor = "#FFFFFFFF";   // color = cream White
                            SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }
                        else
                        {
                            if (pm.SerialNumbers.Exists(x => x.Code == BarcodeStr) && SelectedSerialNumber.Code == null)     //Add as scanned in serial numbers if true
                            {
                                SerialNumber sn = (SerialNumber)pm.SerialNumbers.FirstOrDefault(x => x.Code == BarcodeStr).Clone();

                                Int64 locationAvailableQuantity = WarehouseService.GetStockForScanItem(pm.IdArticle, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, sn.IdWarehouseDeliveryNoteItem, pm.IdWarehouseLocation);
                                LocationAvaibleQuantityForInventoryWizzard = locationAvailableQuantity;
                                if (locationAvailableQuantity < 1)
                                {
                                    UpdateProgressbarLocationStock();
                                    IsWrongItemErrorVisible = Visibility.Visible;
                                    WrongItem = String.Format("No location stock available {0}", BarcodeStr);
                                    BarcodeStr = "";
                                    RowBgColor = "#FFFF0000";
                                    OnFocusBgColor = "#FFFF0000";
                                    OnFocusFgColor = "#FFFFFFFF";
                                    SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                    return;
                                }
                                //show comment
                                SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });

                                if (SelectedpickingMaterial != null && SelectedpickingMaterial.ArticleComment != null)
                                {
                                    if (SelectedpickingMaterial.ArticleCommentDateOfExpiry == null)
                                    {
                                        SelectedpickingMaterial.ShowComment = true;
                                    }
                                    else if (SelectedpickingMaterial.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                                    {
                                        SelectedpickingMaterial.ShowComment = true;
                                    }
                                }

                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                SelectedSerialNumber.Code = BarcodeStr;
                                //assign all values to scanned serial number.
                                SelectedSerialNumber.IdSerialNumber = sn.IdSerialNumber;
                                SelectedSerialNumber.IdArticle = sn.IdArticle;
                                SelectedSerialNumber.IdWarehouseDeliveryNoteItem = sn.IdWarehouseDeliveryNoteItem;
                                SelectedSerialNumber.IdWarehouse = sn.IdWarehouse;

                                OtItem otitem = PickingOt.OtItems.FirstOrDefault(x => x.IdOTItem == pm.IdOtitem);
                                SelectedSerialNumber.IdWarehouseProduct = otitem.RevisionItem.WarehouseProduct.IdWarehouseProduct; // sn.IdWarehouseProduct;
                                SelectedSerialNumber.IsScanned = true;

                                //Prepare Observation.
                                if (pm.Observation == null)
                                {
                                    pm.Observation = new Observation();
                                    pm.Observation.IdRevisionItem = pm.IdRevisionItem;
                                    pm.Observation.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                    pm.Observation.Text = String.Format(Application.Current.FindResource("MaterialSerialNumbers").ToString() + "\r\n", pm.Reference);
                                }

                                pm.Observation.Text += string.Format("\t* {0} ; {1}\r", SelectedSerialNumber.Code, SelectedSerialNumber.ExtraCode);
                                pm.DownloadQuantity -= 1;
                                BarcodeScannedQuantity -= 1;
                                otitem.RevisionItem.RemainingQuantity -= 1;         // quantity;
                                pm.ScannedQty = -1; // * quantity;
                                TotalCurrentItemStock -= 1;

                                if (PickingOt != null && PickingOt.Quotation != null && PickingOt.Quotation.Site != null && PickingOt.OtItems != null && PickingOt.OtItems.Count > 0)
                                {
                                    OtItem oti = PickingOt.OtItems.FirstOrDefault(x => x.IdOTItem == pm.IdOtitem);

                                    if (FollowFIFO)
                                        pm.Comments = PickingOt.Code + " (" + PickingOt.Quotation.Site.Name + ") -> Item " + oti.RevisionItem.NumItem + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                                    else
                                        pm.Comments = PickingOt.Code + " (" + PickingOt.Quotation.Site.Name + ") -> Item " + oti.RevisionItem.NumItem + " [@FIFO=OFF] [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                                }

                                pm.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;

                                if (pm.IdWarehouseProductComponent == 0)
                                    pm.IdWarehouseProductComponent = null;

                                bool IsScanItem = false;
                                if (pm.ScannedQty != 0)
                                {
                                    //IsScanItem = WarehouseService.InsertIntoArticleStock(pm);
                                    //WarehouseService = new WarehouseServiceController("localhost:6699");
                                    IsScanItem = WarehouseService.InsertIntoArticleStock_V2500(pm);
                                }

                                if (IsScanItem)
                                {
                                    //Removed to avoid repeat scan of same item.
                                    pm.SerialNumbers.Remove(pm.SerialNumbers.FirstOrDefault(x => x.IdSerialNumber == sn.IdSerialNumber));

                                    //Upadate total stock of article.
                                    //WarehouseService = new WarehouseServiceController("localhost:6699");
                                    //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 06 05 2024
                                    Tuple<int, Int64> ArticleStockByWarehouse = WarehouseService.GetArticleStockByWarehouseAndGetArticleLockedStockByWarehouse(pm.IdArticle, Convert.ToInt32(pm.IdWarehouse));
                                    //TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(pm.IdArticle, Convert.ToInt32(pm.IdWarehouse));
                                    TotalCurrentItemStock = ArticleStockByWarehouse.Item1;
                                    //CurrentItemLockedStock = WarehouseService.GetArticleLockedStockByWarehouse(pm.IdArticle, Convert.ToInt32(pm.IdWarehouse));
                                    TotalCurrentItemStock = ArticleStockByWarehouse.Item2;
                                    changeTotalStockColor(TotalCurrentItemStock, pm.MinimumStock, CurrentItemLockedStock);

                                    //Update location stock of article.
                                    ProgressbarLocationStock.LocationStock -= 1;    // quantity;
                                    LocationStockValue = ProgressbarLocationStock.LocationStock;
                                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, pm.Article.ArticleWarehouseLocation.MinimumStock);

                                    if (ProgressbarLocationStock.MaximumStock == 0)
                                        LocationStockValue = 0;
                                    else if (ProgressbarLocationStock.MaximumStock < LocationStockValue)
                                        LocationStockValue = ProgressbarLocationStock.MaximumStock;

                                    //Always print single item quantity.
                                    PrintPickedMaterial(pm);

                                    //// Print
                                    //if (PrintBatchLabelAfterScanCompleted) //Batch Print.
                                    //{
                                    //    //RevisionItemQuantity = Convert.ToString(Convert.ToInt64(RevisionItemQuantity) + 1);
                                    //    //pm.BatchPrintQuantity += 1;
                                    //    if (otitem.RevisionItem.RemainingQuantity == 0)     // if (finalRemainingQty == 0) // Scan Completed for one reference
                                    //    {
                                    //        pm.BatchPrintQuantity = Convert.ToInt64(otitem.RevisionItem.Quantity);
                                    //        //RevisionItemQuantity = Convert.ToString(otitem.RevisionItem.Quantity);
                                    //        PrintPickedMaterial(pm);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    //Single item print
                                    //    //RevisionItemQuantity = null;
                                    //    PrintPickedMaterial(pm);
                                    //}

                                    if (otitem.RevisionItem.RemainingQuantity == 0)
                                    {
                                        bool isUpdated = WarehouseService.UpdateItemStatusAndStage_V2035(WarehouseCommon.Instance.Selectedwarehouse, otitem.IdOTItem, 8, GeosApplication.Instance.ActiveUser.IdUser);
                                    }

                                    bool isUpdatedComments = WarehouseService.UpdateRevisionItemComments_V2035(WarehouseCommon.Instance.Selectedwarehouse, pm.IdRevisionItem, SelectedSerialNumber.IdWarehouseDeliveryNoteItem);
                                    //[rdixit][02.10.2023]
                                    //if (isUpdatedComments)
                                    //{
                                    //    if (SelectedpickingMaterial != null)
                                    //    {
                                    //        OtItem MainOtItem = PickingOt.OtItems.FirstOrDefault(i => i.RevisionItem.WarehouseProduct.Article.IdArticle == SelectedpickingMaterial.IdArticle && i.IdOTItem == otitem.IdOTItem);
                                    //        string[] arr = MainOtItem.RevisionItem.NumItem.Split('.');
                                    //        string num = string.Empty; int len = arr.Length; int index = 0;
                                    //        if (len == 3)
                                    //        {
                                    //            while (index < len)
                                    //            {
                                    //                num += arr[index];
                                    //                index++;
                                    //                if (index != len)
                                    //                    num += ".";
                                    //            }
                                    //        }
                                    //        OtItem MainOtItem1 = PickingOt.OtItems.Where(i => i.RevisionItem.NumItem == num).FirstOrDefault();
                                    //        WarehouseService.UpdateOtItemFinishStatus(otitem.IdOT, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, MainOtItem1, MainOtItem1.ParentId);
                                    //    }
                                    //}
                                    if (BarcodeScannedQuantity > 0)
                                    {
                                        SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = pm };
                                        pm.ScanSerialNumbers.Add(scanSerialNumber);
                                        SelectedSerialNumber = scanSerialNumber;
                                        IsScanSerialNumber = true;
                                        BarcodeStr = string.Empty;
                                    }
                                    else if (BarcodeScannedQuantity == 0)
                                    {
                                        IsScanSerialNumber = false;

                                        //006
                                        if (!PrintBatchLabelAfterScanCompleted && PrintBatchLabelPreviousValue)
                                        {
                                            PrintBatchLabelAfterScanCompleted = true;
                                        }

                                        PrintBatchLabelEnabled = true;
                                        indexItem = SelectedLocationList.IndexOf(pm);
                                        indexItem = indexItem + 1;

                                        if (indexItem < SelectedLocationList.Count)
                                        {
                                            GoToNextItem(pm);

                                            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                                            {
                                                OnFocusBgColor = "#FF083493";
                                                OnFocusFgColor = "White";
                                            }
                                            else
                                            {
                                                OnFocusBgColor = "#FF2AB7FF";
                                                OnFocusFgColor = "Black";
                                            }
                                        }
                                        else
                                        {
                                            TrackBarEditvalue = 2;
                                            NextButtonCommandAction(null);
                                            SelectedpickingMaterial = null;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Show error message if scanned item is wrong.
                                IsWrongItemErrorVisible = Visibility.Visible;
                                WrongItem = "Wrong Item " + BarcodeStr;
                                BarcodeStr = string.Empty;
                                OnFocusBgColor = "#FFFF0000";   // color = cream Red
                                OnFocusFgColor = "#FFFFFFFF";   // color = cream White
                                SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            }
                        }

                        BarcodeStr = string.Empty;
                    }
                    else
                    {
                        var isProperBarcode = !string.IsNullOrEmpty(BarcodeStr) && BarcodeStr.All(Char.IsDigit);
                        if (BarcodeStr.Length < 17)
                        {
                            isProperBarcode = false;
                        }

                        if (isProperBarcode)
                        {
                            //string _idwarehouseStr = BarcodeStr.Substring(0, 3);
                            //string _idWareHouseDeliveryNoteItemStr = BarcodeStr.Substring(3, 8);
                            //string _itemQuantityStr = BarcodeStr.Substring(11, 6);

                            //Allow to scan barcode if the idwarehouse belongs to same site then application selected one.
                            //In the articlesstock must be inserted the application selected warehouse.
                            bool IsWarehouseBelongsToSameSite = false;

                            Int64 _idwarehouse = Convert.ToInt64(BarcodeStr.Substring(0, 3));
                            Int64 _idWareHouseDeliveryNoteItem = Convert.ToInt64(BarcodeStr.Substring(3, 8));
                            //Int64 _itemQuantity = Convert.ToInt64(BarcodeStr.Substring(11, 6));
                            BarcodeScannedQuantity = Convert.ToInt64(BarcodeStr.Substring(11, 6));

                            if (WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse == _idwarehouse)
                            {
                                IsWarehouseBelongsToSameSite = true;
                            }
                            else if (WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse != _idwarehouse)
                            {
                                List<Warehouses> warehousesInSameSite = WarehouseCommon.Instance.WarehouseList.Where(x => x.IdSite == WarehouseCommon.Instance.Selectedwarehouse.IdSite).ToList();

                                if (warehousesInSameSite.Any(x => x.IdWarehouse == _idwarehouse))
                                {
                                    IsWarehouseBelongsToSameSite = true;
                                }
                            }

                            bool isSameProducer = true;
                            if (SelectedpickingMaterial != null)
                            {
                                if (PickingOt.ProducerIdCountryGroup != null
                                && PickingOt.ProducerIdCountryGroup != 0 && SelectedpickingMaterial.IdCountryGroup != 0
                                && PickingOt.ProducerIdCountryGroup != SelectedpickingMaterial.IdCountryGroup)
                                {
                                    isSameProducer = false;
                                }
                            }


                            if (SelectedpickingMaterial == null)
                            {
                                if (SelectedLocationList.All(x => x.LockedForOrderPicking == false))
                                {
                                    SetViewInErrorState("This item delivery note is locked and for other all quantity picked.");
                                    return;
                                }
                                else
                                {
                                    SetViewInErrorState("All quantity picked.");
                                    return;
                                }

                            }

                            if (IsWarehouseBelongsToSameSite && isSameProducer
                                && SelectedpickingMaterial.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem
                                && TotalCurrentItemStock >= BarcodeScannedQuantity
                                && (TotalCurrentItemStock - CurrentItemLockedStock) >= BarcodeScannedQuantity
                                && SelectedpickingMaterial.LockedForOrderPicking == true)
                            {
                                if (SelectedpickingMaterial.Manufacturer == null)
                                {
                                    SetViewInErrorState("Wrong Item " + BarcodeStr + ". Missing Producer");
                                    return;
                                }

                                Int64 locationAvaibleQuantity = WarehouseService.GetStockForScanItem(SelectedpickingMaterial.IdArticle, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, _idWareHouseDeliveryNoteItem, SelectedpickingMaterial.IdWarehouseLocation);
                                LocationAvaibleQuantityForInventoryWizzard = locationAvaibleQuantity;
                                if (locationAvaibleQuantity < BarcodeScannedQuantity || BarcodeScannedQuantity == 0)
                                {
                                    UpdateProgressbarLocationStock();
                                    IsWrongItemErrorVisible = Visibility.Visible;
                                    if (BarcodeScannedQuantity == 0)
                                        WrongItem = "Wrong Item " + BarcodeStr;
                                    else
                                        WrongItem = String.Format("No location stock available {0}", BarcodeStr);
                                    BarcodeStr = "";
                                    RowBgColor = "#FFFF0000";
                                    OnFocusBgColor = "#FFFF0000";
                                    OnFocusFgColor = "#FFFFFFFF";
                                    SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                    return;
                                }

                                if (SelectedpickingMaterial.DownloadQuantity >= BarcodeScannedQuantity)
                                {
                                    if (SelectedpickingMaterial.RegisterSerialNumber == 1)
                                    {
                                        if (SelectedpickingMaterial.ScanSerialNumbers == null)
                                            SelectedpickingMaterial.ScanSerialNumbers = new List<SerialNumber>();
                                        BarcodeScannedQuantity = SelectedpickingMaterial.DownloadQuantity;//All SerialNumbers can be Downloaded ntg to do with scanned number 
                                        SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = SelectedpickingMaterial };
                                        SelectedpickingMaterial.ScanSerialNumbers.Add(scanSerialNumber);

                                        //show comment
                                        SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });

                                        if (SelectedpickingMaterial != null && SelectedpickingMaterial.ArticleComment != null)
                                        {
                                            if (SelectedpickingMaterial.ArticleCommentDateOfExpiry == null)
                                            {
                                                SelectedpickingMaterial.ShowComment = true;
                                            }
                                            else if (SelectedpickingMaterial.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                                            {
                                                SelectedpickingMaterial.ShowComment = true;
                                            }
                                        }
                                        SelectedSerialNumber = scanSerialNumber;
                                        IsScanSerialNumber = true;

                                        //006
                                        if (PrintBatchLabelAfterScanCompleted)
                                        {
                                            PrintBatchLabelPreviousValue = true;
                                            PrintBatchLabelAfterScanCompleted = false;
                                        }

                                        PrintBatchLabelEnabled = false;
                                        BarcodeStr = string.Empty;

                                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                        return;
                                    }

                                    if (FollowFIFO == false)
                                    {
                                        string errorMessage = string.Empty;
                                        long MaxScannableQuantity = 0;
                                        long ArticleScannedQuantity = 0;
                                        //002
                                        if (materialSoredList.Any(i => i.IdArticle == SelectedpickingMaterial.IdArticle && i.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem && i.RevisionItem != null && i.RevisionItem.NumItem == SelectedpickingMaterial.RevisionItem.NumItem.ToString()))
                                        {
                                            WMSPickingMaterials scanPickingMaterialItem = materialSoredList.Where(i => i.IdArticle == SelectedpickingMaterial.IdArticle && i.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem && i.RevisionItem != null && i.RevisionItem.NumItem == SelectedpickingMaterial.RevisionItem.NumItem.ToString()).FirstOrDefault();

                                            MaxScannableQuantity = scanPickingMaterialItem.RevisionItem.RemainingQuantity;

                                            ArticleScannedQuantity = Math.Abs(SelectedLocationList.Where(x => x.IdArticle == SelectedpickingMaterial.IdArticle).Sum(x => x.BatchPrintQuantity));
                                        }


                                        // 007
                                        if (MaxScannableQuantity == 0)
                                        {
                                            errorMessage = "All quantity picked.";
                                            SetViewInErrorState(errorMessage);
                                            return;
                                        }

                                        //ArticleScannedQuantity += BarcodeScannedQuantity;
                                        if (BarcodeScannedQuantity > MaxScannableQuantity)
                                        {
                                            errorMessage = "Max available quantity : " + MaxScannableQuantity;
                                            //(MaxScannableQuantity + SelectedLocationList.Where(x => x.IdArticle == SelectedpickingMaterial.IdArticle).Sum(x => x.ScannedQty)).ToString();
                                            SetViewInErrorState(errorMessage);
                                            return;
                                        }
                                    }

                                    ScanAction(_idWareHouseDeliveryNoteItem, BarcodeScannedQuantity, WorkflowStatus.IdWorkflowStatus);

                                    RowBgColor = "#FFF4E27E"; // color = cream Yellow
                                    BarcodeStr = "";

                                    if (SelectedpickingMaterial != null)
                                    {
                                        if (SelectedpickingMaterial.DownloadQuantity > 0)
                                        {
                                            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                                            {
                                                OnFocusBgColor = "#FF083493";
                                                OnFocusFgColor = "White";
                                            }
                                            else
                                            {
                                                OnFocusBgColor = "#FF2AB7FF";
                                                OnFocusFgColor = "Black";
                                            }
                                        }
                                        else
                                        {
                                            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                                            {
                                                OnFocusBgColor = "#FF008000";// color = cream Green
                                                OnFocusFgColor = "#FFFFFFFF";// color = cream White
                                            }
                                            else
                                            {
                                                OnFocusBgColor = "#FF008000";// color = cream Green
                                                OnFocusFgColor = "#FFFFFFFF";// color = cream White
                                            }
                                        }
                                    }

                                    IsWrongItemErrorVisible = Visibility.Hidden;
                                    WrongItem = "";
                                }
                                else
                                {
                                    IsWrongItemErrorVisible = Visibility.Visible;
                                    WrongItem = "Wrong Item " + BarcodeStr;
                                    BarcodeStr = "";
                                    RowBgColor = "#FFFF0000";// color = cream Red
                                    OnFocusBgColor = "#FFFF0000";// color = cream Red
                                    OnFocusFgColor = "#FFFFFFFF";// color = cream White
                                    SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                }
                            }
                            else
                            {
                                if (SelectedpickingMaterial.IdWareHouseDeliveryNoteItem != _idWareHouseDeliveryNoteItem)
                                {
                                    WrongItem = "Wrong Item " + BarcodeStr;
                                    BarcodeStr = "";
                                }
                                else if (TotalCurrentItemStock <= CurrentItemLockedStock)
                                {
                                    //[001] start
                                    //WrongItem = "The Current stock of article is locked. Please contact your supervisor";
                                    WrongItem = "The scanned quantity is greater than the unlocked one. Please contact your supervisor.";
                                    //[001] End

                                    BarcodeStr = "";
                                }
                                else if (BarcodeScannedQuantity > TotalCurrentItemStock)
                                {
                                    WrongItem = $"Scanned quantity {BarcodeScannedQuantity} is greater than total stock {TotalCurrentItemStock}.";
                                    BarcodeStr = "";
                                }
                                else if (BarcodeScannedQuantity > (TotalCurrentItemStock - CurrentItemLockedStock))
                                {
                                    //[001] start
                                    WrongItem = "The scanned quantity is greater than the unlocked one. Please contact your supervisor.";
                                    BarcodeStr = string.Empty;
                                    //WrongItem = $"Scanned quantity {BarcodeScannedQuantity} is greater than unlocked stock {TotalCurrentItemStock - CurrentItemLockedStock}.";
                                    //MessageBoxResult messageBoxResult = CustomMessageBox.Show(WrongItem + " Do you want to pick unlocked stock?", Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                    //if(messageBoxResult==MessageBoxResult.Yes)
                                    //{
                                    //    string barcodePrefix = BarcodeStr.Substring(0, 11);
                                    //    string barcodePostfix = (TotalCurrentItemStock - CurrentItemLockedStock).ToString().PadLeft(6, '0');
                                    //    BarcodeStr = barcodePrefix + barcodePostfix;
                                    //    ScanBarcode("\r");
                                    //    WrongItem = "The Current stock of article is locked. Please contact your supervisor";
                                    //    BarcodeStr = "";
                                    //}
                                    //else
                                    //{
                                    //    WrongItem = "";
                                    //    BarcodeStr = "";
                                    //}
                                    //[001] End
                                }
                                else if (SelectedpickingMaterial.LockedForOrderPicking == false)//[Sudhir.jangra][GEOS2-4544]
                                {
                                    WrongItem = "The Item is Locked";
                                    BarcodeStr = string.Empty;
                                }

                                IsWrongItemErrorVisible = Visibility.Visible;
                                RowBgColor = "#FFFF0000";

                                if (SelectedpickingMaterial.DownloadQuantity > 0)
                                {
                                    OnFocusBgColor = "#FFFF0000";
                                    OnFocusFgColor = "#FFFFFFFF";
                                }
                                SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            }
                        }
                        else
                        {
                            IsWrongItemErrorVisible = Visibility.Visible;
                            WrongItem = "Wrong Item " + BarcodeStr;
                            BarcodeStr = "";
                            RowBgColor = "#FFFF0000";
                            OnFocusBgColor = "#FFFF0000";
                            OnFocusFgColor = "#FFFFFFFF";
                            SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }
                    }

                    #endregion
                }
                else
                {
                    //BarcodeStr = BarcodeStr + fullBarcodeText;
                    //[pramod.misal][GEOS2-5067]][08-01-2023]
                    BarcodeStr = BarcodeStr + fullBarcodeText.ToUpper();

                }
                StartTime = DateTime.Now;
                GeosApplication.Instance.Logger.Log("Method InIt....executed ScanBarcodeAction", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                BarcodeStr = "";
            }
        }

        private void SetViewInErrorState(string message)
        {
            IsWrongItemErrorVisible = Visibility.Visible;
            WrongItem = message;
            BarcodeStr = "";
            RowBgColor = "#FFFF0000";
            OnFocusBgColor = "#FFFF0000";
            OnFocusFgColor = "#FFFFFFFF";
            SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
        }


        /// <summary>
        /// Method for scan action.
        /// </summary>
        /// <param name="idWarehouseDeliveryNoteItem"></param>
        /// <param name="quantity"></param>
        private void ScanAction(Int64 idWarehouseDeliveryNoteItem, Int64 quantity, int currentWorkflowStatus)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);

                if (countLocation == 0)
                {
                    indexItem = 0;
                    if (FollowFIFO)
                    {
                        SelectedpickingMaterial = SelectedLocationList.FirstOrDefault(x => x.LockedForOrderPicking == true);
                    }
                    else
                    {
                        SelectedpickingMaterial = SelectedLocationList[indexItem];
                    }

                    ArticleImage = ByteArrayToImage(SelectedpickingMaterial.ArticleImageInBytes);

                    if (SelectedpickingMaterial.DownloadQuantity > 0)
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                        {
                            OnFocusBgColor = "#FF083493";
                            OnFocusFgColor = "White";
                        }
                        else
                        {
                            OnFocusBgColor = "#FF2AB7FF";
                            OnFocusFgColor = "Black";
                        }
                    }

                    IsLocationIndicatorVisible = Visibility.Hidden;
                    IsInformationVisible = Visibility.Visible;
                    TrackBarEditvalue = 1;
                    countLocation++;

                    //TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                    // CurrentItemLockedStock = WarehouseService.GetArticleLockedStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                    //WarehouseService = new WarehouseServiceController("localhost:6699");
                    //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 06 05 2024
                    Tuple<int, Int64> ArticleStockByWarehouse = WarehouseService.GetArticleStockByWarehouseAndGetArticleLockedStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                    TotalCurrentItemStock = ArticleStockByWarehouse.Item1;
                    CurrentItemLockedStock = ArticleStockByWarehouse.Item2;
                    changeTotalStockColor(TotalCurrentItemStock, SelectedpickingMaterial.MinimumStock, CurrentItemLockedStock);
                    UpdateProgressbarLocationStock();
                    UpdateProgressbarArticleStock();
                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedpickingMaterial.Article.ArticleWarehouseLocation.MinimumStock);

                    if (SelectedLocationList != null && SelectedLocationList.Count > 1) // old code
                        IsNextButtonEnable = true;
                }

                if (SelectedpickingMaterial != null && SelectedpickingMaterial.DownloadQuantity >= quantity && IsLocationScaned)
                {
                    //show comment
                    SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                    if (SelectedpickingMaterial.ArticleComment != null)
                    {
                        if (SelectedpickingMaterial.ArticleCommentDateOfExpiry == null)
                        {
                            SelectedpickingMaterial.ShowComment = true;
                        }
                        else if (SelectedpickingMaterial.ArticleCommentDateOfExpiry.Value.Date >= DateTime.Now.Date)
                        {
                            SelectedpickingMaterial.ShowComment = true;
                        }
                    }
                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                    bool isDecompositionArticle = false;
                    if (SelectedpickingMaterial.IdWarehouseProductComponent != null && SelectedpickingMaterial.IdWarehouseProductComponent > 0)
                    {
                        isDecompositionArticle = true;
                    }

                    if (SelectedpickingMaterial.DownloadQuantity >= quantity)
                    {
                        WMSItemScan otitem = null;
                        if (PickingOt.OtItems != null && PickingOt.OtItems.Count > 0)
                        {
                            otitem = OtItemList.FirstOrDefault(x => x.IdOTItem == SelectedpickingMaterial.IdOtitem);
                        }

                        SelectedpickingMaterial.DownloadQuantity = SelectedpickingMaterial.DownloadQuantity - quantity;

                        if (isDecompositionArticle)
                        {
                            SelectedpickingMaterial.RevisionItem.DownloadedQuantity += quantity;
                            SelectedpickingMaterial.RevisionItem.RemainingQuantity -= quantity;
                        }
                        else
                        {
                            otitem.RevisionItem.RemainingQuantity -= quantity;  //minus remaining qty if item downloaded.
                        }

                        SelectedpickingMaterial.ScannedQty = (-1) * quantity;
                        TotalCurrentItemStock = TotalCurrentItemStock - quantity;

                        if (PickingOt != null && PickingOt.Quotation != null && PickingOt.Quotation.Site != null && otitem != null)
                        {
                            if (FollowFIFO)
                                SelectedpickingMaterial.Comments = PickingOt.Code + " (" + PickingOt.Quotation.Site.Name + ") -> Item " + otitem.RevisionItem.NumItem + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                            else
                                SelectedpickingMaterial.Comments = PickingOt.Code + " (" + PickingOt.Quotation.Site.Name + ") -> Item " + otitem.RevisionItem.NumItem + " [@FIFO=OFF] [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                        }

                        SelectedpickingMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;

                        if (SelectedpickingMaterial.IdWarehouseProductComponent == 0)
                            SelectedpickingMaterial.IdWarehouseProductComponent = null;

                        bool IsScanItem = false;

                        if (SelectedpickingMaterial.ScannedQty != 0)
                        {
                            //IsScanItem = WarehouseService.InsertIntoArticleStock(SelectedpickingMaterial);
                            //WarehouseService = new WarehouseServiceController("localhost:6699");
                            //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 20 03 2024
                            IsScanItem = WarehouseService.InsertIntoArticleStock_V2500(SelectedpickingMaterial);
                        }

                        if (IsScanItem)
                        {
                            //TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                            //CurrentItemLockedStock = WarehouseService.GetArticleLockedStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                            //WarehouseService = new WarehouseServiceController("localhost:6699");
                            //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 06 05 2024
                            Tuple<int, Int64> ArticleStockByWarehouseNew = WarehouseService.GetArticleStockByWarehouseAndGetArticleLockedStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                            TotalCurrentItemStock = ArticleStockByWarehouseNew.Item1;
                            CurrentItemLockedStock = ArticleStockByWarehouseNew.Item2;

                            changeTotalStockColor(TotalCurrentItemStock, SelectedpickingMaterial.MinimumStock, CurrentItemLockedStock);
                            TransitionWorkflowStatus(PickingOt.IdOT);//rajashri[GEOS2-4849]-To update workflowstatus
                            //Update status and comments
                            //Decomposition otitems - if all decomposition otitem articles are downloaded then otitem status is changed to finish
                            if (isDecompositionArticle)
                            {
                                //bool allDownloaded = AreAllDecompositionOtItemsDownloaded(otitem.ArticleDecomposedList);
                                bool allDownloaded = AreAllDecompositionOtItemsDownloaded_V2500(otitem.ArticleDecomposedList);
                                if (allDownloaded)
                                    WarehouseService.UpdateItemStatusAndStage_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedpickingMaterial.IdOtitem, 8, GeosApplication.Instance.ActiveUser.IdUser);
                            }
                            else //Normal item
                            {
                                if (otitem.RevisionItem.RemainingQuantity == 0)
                                {
                                    bool isUpdated = WarehouseService.UpdateItemStatusAndStage_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedpickingMaterial.IdOtitem, 8, GeosApplication.Instance.ActiveUser.IdUser);
                                }
                            }

                            bool isUpdatedComments = WarehouseService.UpdateRevisionItemComments_V2035(WarehouseCommon.Instance.Selectedwarehouse, SelectedpickingMaterial.IdRevisionItem, idWarehouseDeliveryNoteItem);
                            //End
                            //[rdixit][02.10.2023]
                            //if (isUpdatedComments)
                            //{
                            //    if (SelectedpickingMaterial != null)
                            //    {
                            //        OtItem MainOtItem = PickingOt.OtItems.FirstOrDefault(i => i.RevisionItem.WarehouseProduct.Article.IdArticle == SelectedpickingMaterial.IdArticle && i.IdOTItem == otitem.IdOTItem);
                            //        string[] arr = MainOtItem.RevisionItem.NumItem.Split('.');
                            //        string num = string.Empty; int len = arr.Length; int index = 0;
                            //        if (len == 3)
                            //        {
                            //            while (index < len)
                            //            {
                            //                num += arr[index];
                            //                index++;
                            //                if (index != len)
                            //                    num += ".";
                            //            }
                            //        }
                            //        OtItem MainOtItem1 = PickingOt.OtItems.Where(i => i.RevisionItem.NumItem == num).FirstOrDefault();
                            //        WarehouseService.UpdateOtItemFinishStatus(otitem.IdOT, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, MainOtItem1, MainOtItem1.ParentId);
                            //    }
                            //}
                            UpdateProgressbarLocationStock();
                            UpdateProgressbarArticleStock();
                            ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedpickingMaterial.Article.ArticleWarehouseLocation.MinimumStock);

                            if (PrintBatchLabelAfterScanCompleted)
                            {
                                if (isDecompositionArticle)     //Batch print for decomposition otitem.
                                {
                                    if (SelectedpickingMaterial.RevisionItem.RemainingQuantity == 0)   //When all qty downloaded then print label.
                                    {
                                        //RevisionItemQuantity = Convert.ToString(SelectedpickingMaterial.RevisionItem.DownloadedQuantity);
                                        //PrintPickedMaterial(SelectedpickingMaterial);

                                        SelectedpickingMaterial.BatchPrintQuantity += quantity;

                                        List<WMSPickingMaterials> printList = materialSoredList.Where(x => x.IdOtitem == SelectedpickingMaterial.IdOtitem && x.IdWarehouseProductComponent == SelectedpickingMaterial.IdWarehouseProductComponent).ToList();

                                        printList.ToList().ForEach(x => { x.IsPartialBatchPrinted = true; });

                                        List<WMSPickingMaterials> GroupedByMadeIn = printList.GroupBy(l => l.MadeIn)
                                                                                .Select(cl => new WMSPickingMaterials
                                                                                {
                                                                                    MadeIn = cl.First().MadeIn,
                                                                                    Reference = cl.First().Reference,
                                                                                    RevisionItem = cl.First().RevisionItem,
                                                                                    DownloadQuantity = cl.First().DownloadQuantity,
                                                                                    Article = cl.First().Article,
                                                                                    IdCountryGroup = cl.First().IdCountryGroup,
                                                                                    IdOtitem = cl.First().IdOtitem,
                                                                                    PartNumberCode = cl.First().PartNumberCode,
                                                                                    BatchPrintQuantity = cl.Sum(c => c.BatchPrintQuantity),
                                                                                }).ToList();

                                        foreach (WMSPickingMaterials pickingMaterial in GroupedByMadeIn)
                                        {
                                            if (pickingMaterial.BatchPrintQuantity > 0)
                                                PrintPickedMaterial_V2500(pickingMaterial, otitem);
                                        }
                                    }
                                    else //This is used to print partially downloaded item.
                                    {
                                        //RevisionItemQuantity = Convert.ToString(Convert.ToInt64(RevisionItemQuantity) + quantity);
                                        SelectedpickingMaterial.BatchPrintQuantity += quantity;
                                    }
                                }
                                else if (otitem.RevisionItem.RemainingQuantity == 0)
                                {
                                    // Scan Completed for one reference
                                    //RevisionItemQuantity = otitem.RevisionItem.Quantity.ToString();
                                    //PrintPickedMaterial(SelectedpickingMaterial);

                                    SelectedpickingMaterial.BatchPrintQuantity += quantity;

                                    List<WMSPickingMaterials> printList = materialSoredList.Where(x => x.IdOtitem == SelectedpickingMaterial.IdOtitem).ToList();
                                    printList.ToList().ForEach(x => { x.IsPartialBatchPrinted = true; });

                                    List<WMSPickingMaterials> GroupedByMadeIn = printList.GroupBy(l => l.MadeIn)
                                                                            .Select(cl => new WMSPickingMaterials
                                                                            {
                                                                                MadeIn = cl.First().MadeIn,
                                                                                Reference = cl.First().Reference,
                                                                                RevisionItem = cl.First().RevisionItem,
                                                                                DownloadQuantity = cl.First().DownloadQuantity,
                                                                                Article = cl.First().Article,
                                                                                IdCountryGroup = cl.First().IdCountryGroup,
                                                                                IdOtitem = cl.First().IdOtitem,
                                                                                PartNumberCode = cl.First().PartNumberCode,
                                                                                BatchPrintQuantity = cl.Sum(c => c.BatchPrintQuantity),
                                                                            }).ToList();

                                    foreach (WMSPickingMaterials pickingMaterial in GroupedByMadeIn)
                                    {
                                        if (pickingMaterial.BatchPrintQuantity > 0)
                                            PrintPickedMaterial(pickingMaterial);
                                    }
                                }
                                else
                                {
                                    //This is used to print partially downloaded item.
                                    //RevisionItemQuantity = Convert.ToString(Convert.ToInt64(RevisionItemQuantity) + quantity);
                                    SelectedpickingMaterial.BatchPrintQuantity += quantity;
                                }
                            }
                            else
                            {
                                SelectedpickingMaterial.BatchPrintQuantity += quantity;

                                if (isDecompositionArticle)     //decomposition otitem.
                                {
                                    //PrintPickedMaterial(SelectedpickingMaterial, otitem);
                                    PrintPickedMaterial_V2500(SelectedpickingMaterial, otitem);
                                }
                                else
                                {
                                    //PrintPickedMaterial(SelectedpickingMaterial);
                                    PrintPickedMaterial_V2500(SelectedpickingMaterial);

                                }
                            }
                        }

                        //TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                        //CurrentItemLockedStock = WarehouseService.GetArticleLockedStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                        //WarehouseService = new WarehouseServiceController("localhost:6699");
                        //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 06 05 2024
                        Tuple<int, Int64> ArticleStockByWarehouse = WarehouseService.GetArticleStockByWarehouseAndGetArticleLockedStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                        TotalCurrentItemStock = ArticleStockByWarehouse.Item1;
                        CurrentItemLockedStock = ArticleStockByWarehouse.Item2;

                        changeTotalStockColor(TotalCurrentItemStock, SelectedpickingMaterial.MinimumStock, CurrentItemLockedStock);
                        UpdateProgressbarArticleStock();
                        UpdateProgressbarLocationStock();
                        ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedpickingMaterial.Article.ArticleWarehouseLocation.MinimumStock);
                        #region GEOS2-4436
                        //Shubham[skadam] GEOS2-4436 Ask for location inventory after do the picking of certain references (2/3)  23 10 2023 
                        try
                        {
                            if (IsSameReferenceWithRequiresCountAfterPicking)
                            {
                                //if (SelectedpickingMaterial.LockedForOrderPicking == true)
                                if (SelectedpickingMaterial != null)
                                {
                                    //if (SelectedpickingMaterial.BatchPrintQuantity == BarcodeScannedQuantity)
                                    //if (SelectedpickingMaterial.ArticleCurrentStock == BarcodeScannedQuantity)
                                    //PickingMaterials PickingMaterials = SelectedLocationListForInventoryWizzard.Where(w => w.IdWareHouseDeliveryNoteItem == SelectedpickingMaterial.IdWareHouseDeliveryNoteItem).FirstOrDefault();
                                    //if (LocationAvaibleQuantityForInventoryWizzard == BarcodeScannedQuantity)
                                    //if (PickingMaterials.DownloadQuantity == 0)
                                    if (SelectedpickingMaterial.DownloadQuantity == 0)
                                    {
                                        SelectedpickingMaterial.IsInventoryReview = true;
                                        SelectedLocationList.Where(w => w.IdWareHouseDeliveryNoteItem == SelectedpickingMaterial.IdWareHouseDeliveryNoteItem).All(a => a.IsInventoryReview == true);
                                        int count = 0;
                                        List<WMSPickingMaterials> tempLockedForOrderPicking = SelectedLocationList.Where(w => w.IdArticle == SelectedpickingMaterial.IdArticle).ToList();
                                        if (tempLockedForOrderPicking.Count() > 0) // [nsatpute][07-10-2024][GEOS2-6501]
                                        {
                                            foreach (WMSPickingMaterials pickingMaterialsitem in tempLockedForOrderPicking)
                                            {
                                                if (pickingMaterialsitem.IsInventoryReview)
                                                {
                                                    count = count + 1;
                                                }
                                            }
                                        }



                                        if (tempLockedForOrderPicking.Count == count)
                                        {
                                            IsSameReference = true;
                                            ScanInventoryWizzard();
                                            IsSameReference = false;
                                        }
                                    }
                                    else
                                    {
                                        SelectedpickingMaterial.IsInventoryReview = false;
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion
                        if (SelectedpickingMaterial.DownloadQuantity == 0)
                        {
                            if (PrintBatchLabelAfterScanCompleted && SelectedpickingMaterial.BatchPrintQuantity > 0)
                            {
                                PrintBatchLabelPartialDownloadedItem();
                            }

                            indexItem = SelectedLocationList.IndexOf(SelectedpickingMaterial);
                            indexItem = indexItem + 1;
                            if (indexItem < SelectedLocationList.Count)
                            {
                                GoToNextItem(SelectedpickingMaterial);
                            }
                            else
                            {
                                TrackBarEditvalue = 2;
                                NextButtonCommandAction(null);
                                SelectedpickingMaterial = null;
                            }
                        }
                    }
                }

                SelectedLocationList = new List<WMSPickingMaterials>(SelectedLocationList);
                StartTime = DateTime.Now;
                GeosApplication.Instance.Logger.Log("Method ScanAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private bool AreAllDecompositionOtItemsDownloaded(List<OtItem> decomposedOtItems)
        {
            bool isAllDownloaded = false;

            if (decomposedOtItems != null && decomposedOtItems.Count > 0)
            {
                foreach (OtItem decomposedItem in decomposedOtItems)
                {
                    if (decomposedItem.ArticleDecomposedList != null && decomposedItem.ArticleDecomposedList.Count > 0)
                    {
                        isAllDownloaded = AreAllDecompositionOtItemsDownloaded(decomposedItem.ArticleDecomposedList);

                        if (!isAllDownloaded)
                            return false;
                    }
                    else if (decomposedItem.RevisionItem.RemainingQuantity > 0)
                        return false;
                    else if (decomposedItem.RevisionItem.RemainingQuantity == 0)
                        isAllDownloaded = true;
                }
            }

            return isAllDownloaded;
        }

        private bool AreAllDecompositionOtItemsDownloaded_V2500(List<WMSItemScan> decomposedOtItems)
        {
            bool isAllDownloaded = false;

            if (decomposedOtItems != null && decomposedOtItems.Count > 0)
            {
                foreach (WMSItemScan decomposedItem in decomposedOtItems)
                {
                    if (decomposedItem.ArticleDecomposedList != null && decomposedItem.ArticleDecomposedList.Count > 0)
                    {
                        isAllDownloaded = AreAllDecompositionOtItemsDownloaded_V2500(decomposedItem.ArticleDecomposedList);

                        if (!isAllDownloaded)
                            return false;
                    }
                    else if (decomposedItem.RevisionItem.RemainingQuantity > 0)
                        return false;
                    else if (decomposedItem.RevisionItem.RemainingQuantity == 0)
                        isAllDownloaded = true;
                }
            }

            return isAllDownloaded;
        }
        /// <summary>
        /// Method for change Total stock color as per condition.
        /// </summary>
        /// <param name="totalStock"></param>
        /// <param name="minQuantity"></param>
        /// <param name="lockedStock"></param>
        private void changeTotalStockColor(Int64 totalStock, Int64 minQuantity, Int64 lockedStock)
        {
            if (totalStock == 0)
            {
                TotalStockFgColor = "#FFFFFFFF"; //white colour html code
                TotalStockBgColor = "#FFFF0000"; //red colour html code
            }
            else if (totalStock >= minQuantity)
            {
                TotalStockFgColor = "#FFFFFFFF"; //white colour html code
                TotalStockBgColor = "#FF008000"; //green colour html code
            }
            else if (totalStock < minQuantity)
            {
                TotalStockFgColor = "#FF000000"; //black colour html code
                TotalStockBgColor = "#FFFFFF00"; //yellow colour html code
            }

            if (lockedStock != 0)
            {
                if (totalStock <= lockedStock)
                {
                    //set red
                    TotalStockFgColor = "#FFFFFFFF"; //white colour html code
                    TotalStockBgColor = "#FFFF0000"; //red colour html code
                                                     //Show lock icon
                    LockedStockIconVisibility = Visibility.Visible;
                }
                else if (totalStock > lockedStock)
                {
                    //Do not set red
                    //Hide lock icon
                    LockedStockIconVisibility = Visibility.Hidden;
                }
            }
        }

        //[WMS M049-16]
        private void PrintPickedMaterial(WMSPickingMaterials pickingMaterial, OtItem otitem = null)
        {
            GeosApplication.Instance.Logger.Log(" PickingMaterialsViewModel Method printPickedMaterial....", category: Category.Info, priority: Priority.Low);

            try
            {
                string barcode = pickingMaterial.PartNumberCode;
                //string UserCode = WarehouseService.GetEmployeeCodeByIdUser(GeosApplication.Instance.ActiveUser.IdUser); //[GEOS2-4014][rdixit][15.12.2022]
                string UserCode = EmployeeCode;
                //Removed code - [GEOS2-1652]SubItem number in Item Label
                //var revisionNumber = OtItemList.Where(i => i.IdOTItem == pickingMaterial.IdOtitem).FirstOrDefault().RevisionItem.NumItem;
                //string otForPrinting = string.Empty;
                //if (string.IsNullOrEmpty(revisionNumber))
                //{
                //    otForPrinting = PickingOt.Code;
                //}
                //else
                //{
                //    otForPrinting = PickingOt.Code + " (Item " + revisionNumber + ")";
                //}

                string otForPrinting = String.Format("{0} (Item {1})", PickingOt.Code, pickingMaterial.RevisionItem.NumItem);

                string splBarCode = string.Empty;
                Dictionary<string, string> printValues = new Dictionary<string, string>();

                if (printFile == null)
                    printFile = GeosRepositoryService.GetPrintLabelFile(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);

                PrintLabel printLabel = new PrintLabel(printValues, printFile);

                printValues.Add("@CUSTOMER00", String.Format("{0} - {1}", PickingOt.Quotation.Site.Customer.CustomerName, PickingOt.Quotation.Site.SiteNameWithoutCountry));
                printValues.Add("@CUSTOMER01", "");

                //WMS M055-10
                if (PrintBatchLabelAfterScanCompleted)  //Printing label after Scan Completed in one label
                {
                    printValues.Add("@QUANTITY", Convert.ToString(pickingMaterial.BatchPrintQuantity));

                    //[adhatkar][GEOS2-3249][In old, always bind qty 001. Need to change with actul qty with 6 digit. 
                    barcode = barcode.Remove(barcode.Length - 3);
                    string QuanityWith6Digit = Convert.ToString(pickingMaterial.BatchPrintQuantity).PadLeft(6, '0');
                    barcode = barcode + QuanityWith6Digit;
                }
                else    //Printing label after every scan
                {
                    if (pickingMaterial.RegisterSerialNumber == 1)
                    {
                        printValues.Add("@QUANTITY", "1");

                        //[adhatkar][GEOS2-3249][In old, always bind qty 001. Need to change with actul qty with 6 digit. 
                        barcode = barcode.Remove(barcode.Length - 3);
                        string QuanityWith6Digit = "000001";
                        barcode = barcode + QuanityWith6Digit;
                    }
                    else
                    {
                        printValues.Add("@QUANTITY", Math.Abs(pickingMaterial.ScannedQty).ToString());

                        //[adhatkar][GEOS2-3249][In old, always bind qty 001. Need to change with actul qty with 6 digit. 
                        barcode = barcode.Remove(barcode.Length - 3);
                        string QuanityWith6Digit = Math.Abs(pickingMaterial.ScannedQty).ToString().PadLeft(6, '0');
                        barcode = barcode + QuanityWith6Digit;
                    }
                }

                printValues.Add("@USER", UserCode);
                printValues.Add("@OT", otForPrinting);

                if (otitem != null &&
                    otitem.RevisionItem != null &&
                    otitem.RevisionItem.WarehouseProduct != null &&
                    otitem.RevisionItem.WarehouseProduct.Article != null)
                    printValues.Add("@REFERENCE", string.Format("{0} / {1}", otitem.RevisionItem.WarehouseProduct.Article.Reference, pickingMaterial.Reference));
                else
                    printValues.Add("@REFERENCE", pickingMaterial.Reference);

                if (IsScanSerialNumber && SelectedSerialNumber != null)
                    printValues.Add("@SN_VALUE", SelectedSerialNumber.Code);
                else
                    printValues.Add("@SN_VALUE", "");

                printValues.Add("@REFBARCODE", "");
                printValues.Add("@MADEIN", pickingMaterial.MadeIn);
                printValues.Add("@PN", "");

                if (!string.IsNullOrEmpty(barcode))
                    splBarCode = printLabel.SplitStringForBarcode(barcode);

                printValues.Add("@PBARCODE", splBarCode);
                printValues.Add("@WAREHOUSE", WarehouseCommon.Instance.Selectedwarehouse.Name);

                printLabel.Print();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in PrintPickedMaterial() - print label {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log(" PickingMaterialsViewModel Method printPickedMaterial() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void PrintPickedMaterial_V2500(WMSPickingMaterials pickingMaterial, WMSItemScan otitem = null)
        {
            GeosApplication.Instance.Logger.Log(" PickingMaterialsViewModel Method printPickedMaterial....", category: Category.Info, priority: Priority.Low);

            try
            {
                string barcode = pickingMaterial.PartNumberCode;
                //string UserCode = WarehouseService.GetEmployeeCodeByIdUser(GeosApplication.Instance.ActiveUser.IdUser); //[GEOS2-4014][rdixit][15.12.2022]
                string UserCode = EmployeeCode;
                //Removed code - [GEOS2-1652]SubItem number in Item Label
                //var revisionNumber = OtItemList.Where(i => i.IdOTItem == pickingMaterial.IdOtitem).FirstOrDefault().RevisionItem.NumItem;
                //string otForPrinting = string.Empty;
                //if (string.IsNullOrEmpty(revisionNumber))
                //{
                //    otForPrinting = PickingOt.Code;
                //}
                //else
                //{
                //    otForPrinting = PickingOt.Code + " (Item " + revisionNumber + ")";
                //}

                string otForPrinting = String.Format("{0} (Item {1})", PickingOt.Code, pickingMaterial.RevisionItem.NumItem);

                string splBarCode = string.Empty;
                Dictionary<string, string> printValues = new Dictionary<string, string>();

                if (printFile == null)
                    printFile = GeosRepositoryService.GetPrintLabelFile(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);

                PrintLabel printLabel = new PrintLabel(printValues, printFile);

                printValues.Add("@CUSTOMER00", String.Format("{0} - {1}", PickingOt.Quotation.Site.Customer.CustomerName, PickingOt.Quotation.Site.SiteNameWithoutCountry));
                printValues.Add("@CUSTOMER01", "");

                //WMS M055-10
                if (PrintBatchLabelAfterScanCompleted)  //Printing label after Scan Completed in one label
                {
                    printValues.Add("@QUANTITY", Convert.ToString(pickingMaterial.BatchPrintQuantity));

                    //[adhatkar][GEOS2-3249][In old, always bind qty 001. Need to change with actul qty with 6 digit. 
                    barcode = barcode.Remove(barcode.Length - 3);
                    string QuanityWith6Digit = Convert.ToString(pickingMaterial.BatchPrintQuantity).PadLeft(6, '0');
                    barcode = barcode + QuanityWith6Digit;
                }
                else    //Printing label after every scan
                {
                    if (pickingMaterial.RegisterSerialNumber == 1)
                    {
                        printValues.Add("@QUANTITY", "1");

                        //[adhatkar][GEOS2-3249][In old, always bind qty 001. Need to change with actul qty with 6 digit. 
                        barcode = barcode.Remove(barcode.Length - 3);
                        string QuanityWith6Digit = "000001";
                        barcode = barcode + QuanityWith6Digit;
                    }
                    else
                    {
                        printValues.Add("@QUANTITY", Math.Abs(pickingMaterial.ScannedQty).ToString());

                        //[adhatkar][GEOS2-3249][In old, always bind qty 001. Need to change with actul qty with 6 digit. 
                        barcode = barcode.Remove(barcode.Length - 3);
                        string QuanityWith6Digit = Math.Abs(pickingMaterial.ScannedQty).ToString().PadLeft(6, '0');
                        barcode = barcode + QuanityWith6Digit;
                    }
                }

                printValues.Add("@USER", UserCode);
                printValues.Add("@OT", otForPrinting);

                if (otitem != null &&
                    otitem.RevisionItem != null &&
                    otitem.RevisionItem != null &&
                    otitem.RevisionItem.Article != null)
                    printValues.Add("@REFERENCE", string.Format("{0} / {1}", otitem.RevisionItem.Article.Reference, pickingMaterial.Reference));
                else
                    printValues.Add("@REFERENCE", pickingMaterial.Reference);

                if (IsScanSerialNumber && SelectedSerialNumber != null)
                    printValues.Add("@SN_VALUE", SelectedSerialNumber.Code);
                else
                    printValues.Add("@SN_VALUE", "");

                printValues.Add("@REFBARCODE", "");
                printValues.Add("@MADEIN", pickingMaterial.MadeIn);
                printValues.Add("@PN", "");

                if (!string.IsNullOrEmpty(barcode))
                    splBarCode = printLabel.SplitStringForBarcode(barcode);

                printValues.Add("@PBARCODE", splBarCode);
                printValues.Add("@WAREHOUSE", WarehouseCommon.Instance.Selectedwarehouse.Name);

                printLabel.Print();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in PrintPickedMaterial() - print label {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log(" PickingMaterialsViewModel Method printPickedMaterial() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// Method for scan next location items.
        /// [001][2019-03-13][skhade][WMS-M059-04] Partial downloaded item batch label print
        /// </summary>
        /// <param name="obj"></param>
        private void NextButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log(" Method NextButtonCommandAction....", category: Category.Info, priority: Priority.Low);

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

            if (!IsLocationScaned)
            {
                CalculateLocationIndex(LocationIndex.NextLocation);
                GoToLocation(currentLocationIndex, LocationIndex.NextLocation);
            }
            else if (IsScanSerialNumber)
            {
                IsScanSerialNumber = false;

                //006
                if (!PrintBatchLabelAfterScanCompleted && PrintBatchLabelPreviousValue)
                {
                    PrintBatchLabelAfterScanCompleted = true;
                }

                PrintBatchLabelEnabled = true;
                WMSPickingMaterials pm = SelectedSerialNumber.MasterItem as WMSPickingMaterials;
                PrintBatchLabelPartialDownloadedItem(pm);
                pm.ScanSerialNumbers.Remove(pm.ScanSerialNumbers.FirstOrDefault(x => x.Code == null));
                SelectedSerialNumber = null;
                //pm.ScanSerialNumbers = new List<SerialNumber>();
                //pm.DownloadQuantity = pm.SerialNumbers.Count;

                GoToNextItem(pm);
            }
            else
            {
                if (SelectedpickingMaterial == null && SelectedSerialNumber != null)
                {
                    WMSPickingMaterials pm = SelectedSerialNumber.MasterItem as WMSPickingMaterials;
                    SelectedSerialNumber = null;
                    GoToNextItem(pm);
                }
                else
                {
                    //001
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    PrintBatchLabelPartialDownloadedItem();

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

                    GoToNextItem(SelectedpickingMaterial);
                    if (SelectedpickingMaterial != null)
                        changeTotalStockColor(TotalCurrentItemStock, SelectedpickingMaterial.MinimumStock, CurrentItemLockedStock);
                }
            }
            StartTime = DateTime.Now;
            //IsSameReferenceWithRequiresCountAfterPicking = false;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method ScanAction....executed NextButtonCommandAction", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for goto skipped items
        /// </summary>
        /// <param name="obj"></param>
        private void BackButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel  Method BackButtonCommandAction....", category: Category.Info, priority: Priority.Low);

                PrintBatchLabelPartialDownloadedItem();

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

                CalculateLocationIndex(LocationIndex.PreviousLocation);
                GoToLocation(currentLocationIndex, LocationIndex.PreviousLocation);

                if (IsScanSerialNumber)
                {
                    IsScanSerialNumber = false;

                    //006
                    if (!PrintBatchLabelAfterScanCompleted && PrintBatchLabelPreviousValue)
                    {
                        PrintBatchLabelAfterScanCompleted = true;
                    }

                    PrintBatchLabelEnabled = true;
                    WMSPickingMaterials pm = (WMSPickingMaterials)SelectedSerialNumber.MasterItem;
                    pm.ScanSerialNumbers.Remove(pm.ScanSerialNumbers.FirstOrDefault(x => x.Code == null));
                    SelectedSerialNumber = null;
                    //pmItem.ScanSerialNumbers = new List<SerialNumber>();
                    //pmItem.DownloadQuantity = pmItem.SerialNumbers.Count;
                }

                IsSameReferenceWithRequiresCountAfterPicking = false;
                StartTime = DateTime.Now;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method BackButtonCommandAction....executed ", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method BackButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                GeosApplication.Instance.Logger.Log("Get an error in Method ByteArrayToImage...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        void VectorLayerDataLoaded(object obj)
        {
            MapControl mapControl = obj as MapControl;

            if (mapControl != null)
                mapControl.ZoomToFitLayerItems();
        }

        void ListSourceDataAdapterCustomizeMapItem(object e)
        {
            MapRectangle rectangle = ((DevExpress.Xpf.Map.CustomizeMapItemEventArgs)e).MapItem as MapRectangle;
            WarehouseLocation wl = ((DevExpress.Xpf.Map.CustomizeMapItemEventArgs)e).SourceObject as WarehouseLocation;

            //if (wl.HtmlColor != null)
            //    rectangle.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString(wl.HtmlColor);

            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
            {
                rectangle.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2AB7FF");
            }
            else if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
            {
                rectangle.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF083493");
            }

            rectangle.Height = wl.Height;
            rectangle.Width = wl.Width;

            //if (rectangle.Title != null)
            //    rectangle.Title.Text = wl.FullName;
        }



        private void CommandCancelAction(object obj)
        {
            try
            {
                PrintBatchLabelPartialDownloadedItem();

                //if (IsTimer)
                //    IsTimer = false;
                OTWorkingTime tempOTWorkingTime = new OTWorkingTime()
                {
                    IdOT = PickingOt.IdOT,
                    IdStage = 12,
                    IdOperator = GeosApplication.Instance.ActiveUser.IdUser,
                    StartTime = null,
                    EndTime = DateTime.Now
                };

                bool result = WarehouseService.AddCancelledOTWorkingTime_V2550(WarehouseCommon.Instance.Selectedwarehouse, tempOTWorkingTime);

                IsCanceled = true;
                RequestClose(null, null);

                string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                if (Directory.Exists(basePath))
                {
                    Directory.Delete(basePath, true);
                }
                StartTime = DateTime.Now;
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CommandCancelAction Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CommandCancelAction Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in CommandCancelAction(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [WMS-M059-04]Partial downloaded item batch label print.
        /// </summary>
        /// <param name="obj"></param>
        private void PrintBatchLabelPartialDownloadedItem(WMSPickingMaterials SerialNumberPickingMaterials = null)
        {
            if (PrintBatchLabelAfterScanCompleted &&
                SerialNumberPickingMaterials != null &&
                !SerialNumberPickingMaterials.IsPartialBatchPrinted &&
                SerialNumberPickingMaterials.BatchPrintQuantity > 0)
            {
                SerialNumberPickingMaterials.IsPartialBatchPrinted = true;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.FindResource("PickingPrintLabel").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    PrintPickedMaterial(SerialNumberPickingMaterials);
                }
            }
            else if (PrintBatchLabelAfterScanCompleted &&
                    SelectedpickingMaterial != null &&
                    !SelectedpickingMaterial.IsPartialBatchPrinted &&
                    SelectedpickingMaterial.BatchPrintQuantity > 0) // && !string.IsNullOrEmpty(RevisionItemQuantity) && Convert.ToInt64(RevisionItemQuantity) > 0)
            {
                SelectedpickingMaterial.IsPartialBatchPrinted = true;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.FindResource("PickingPrintLabel").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    WMSItemScan otitem = null;
                    if (SelectedpickingMaterial.IdWarehouseProductComponent > 0)
                        otitem = OtItemList.FirstOrDefault(x => x.IdOTItem == SelectedpickingMaterial.IdOtitem);

                    PrintPickedMaterial_V2500(SelectedpickingMaterial, otitem);
                }

                //RevisionItemQuantity = null;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private void HyperlinkClickCommandAction(object obj)
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

                WMSPickingMaterials article = (WMSPickingMaterials)obj;
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
                    int index = SelectedLocationList.IndexOf(article);
                    if (articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage || articleDetailsViewModel.UpdateArticle.IsAddedArticleImage)
                    {
                        foreach (var item in materialSoredList)
                        {
                            if (item.IdArticle == article.IdArticle)
                                item.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        }

                        if (SelectedpickingMaterial != null)
                        {
                            if (article.IdArticle == SelectedpickingMaterial.IdArticle)
                                ArticleImage = ByteArrayToImage(articleDetailsViewModel.UpdateArticle.ArticleImageInBytes);
                        }
                    }
                }
                FocusUserControl = true;
                GeosApplication.Instance.Logger.Log("Method HyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method HyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GoToLocation(int locationIndex, LocationIndex location)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetNextLocation....", category: Category.Info, priority: Priority.Low);

                if (ArticleLocationList.Count > locationIndex)
                {
                    TotalCurrentItemStock = 0;
                    SelectedLocationList = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[locationIndex].ToString()).Select(p => p).ToList();
                    LocationName = ArticleLocationList[locationIndex].ToString();

                    FillMapLocation();

                    countLocation = 0;
                    IsLocationIndicatorVisible = Visibility.Visible;
                    IsInformationVisible = Visibility.Collapsed;
                    IsLocationScaned = false;
                    TrackBarEditvalue = 0;

                    IsWrongLocationErrorVisible = Visibility.Hidden;
                    IsWrongItemErrorVisible = Visibility.Hidden;
                    WrongLocation = "";
                    WrongItem = "";

                    if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                    {
                        BgColorLocation = "#FF083493";
                        RowBgColor = "PINK";
                        FgColorLocation = "White";
                        TrackBarFgColor = "#FF083493";
                    }
                    else
                    {
                        FgColorLocation = "Black";
                        BgColorLocation = "#FF2AB7FF";
                        RowBgColor = "#FFFFFFFF";
                        TrackBarFgColor = "#FF2AB7FF";
                    }

                    if (location == LocationIndex.NextLocation)
                        Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.PickingMaterialsUserControlView", null, this);

                    if (location == LocationIndex.PreviousLocation)
                        Service.GoBack();

                    ChangePickingMethodology(true);
                }
                else
                {
                    IsNextButtonEnable = false;
                    isLocationLast = true;
                }
                GeosApplication.Instance.Logger.Log("Method GetNextLocation....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetNextLocation...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GoToNextItem(WMSPickingMaterials currentPickingMaterial)
        {
            if (currentPickingMaterial != null)
            {
                bool allRowsFalse = true;
                int count = 0;
                int currentPickingMaterialIndex = SelectedLocationList.IndexOf(currentPickingMaterial);
                this.currentPickingMaterial = currentPickingMaterial;
                if (currentPickingMaterialIndex + 1 < SelectedLocationList.Count)
                {
                    if (FollowFIFO)
                    {

                        for (int i = currentPickingMaterialIndex + 1; i < SelectedLocationList.Count; i++)
                        {
                            count = i;
                            if (SelectedLocationList[i].LockedForOrderPicking)
                            {
                                SelectedpickingMaterial = SelectedLocationList[i];
                                allRowsFalse = false;
                                break;
                            }

                        }

                    }
                    else
                    {
                        SelectedpickingMaterial = SelectedLocationList[currentPickingMaterialIndex + 1];
                        currentPickingMaterialIndex++;
                    }
                    if (allRowsFalse && count == SelectedLocationList.Count - 1)
                    {
                        CalculateLocationIndex(LocationIndex.NextLocation);
                        GoToLocation(currentLocationIndex, LocationIndex.NextLocation);
                    }
                    else
                    {
                        ArticleImage = ByteArrayToImage(SelectedpickingMaterial.ArticleImageInBytes);

                        //TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                        //CurrentItemLockedStock = WarehouseService.GetArticleLockedStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                        //WarehouseService = new WarehouseServiceController("localhost:6699");
                        //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 06 05 2024
                        Tuple<int, Int64> ArticleStockByWarehouse = WarehouseService.GetArticleStockByWarehouseAndGetArticleLockedStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                        TotalCurrentItemStock = ArticleStockByWarehouse.Item1;
                        CurrentItemLockedStock = ArticleStockByWarehouse.Item2;

                        UpdateProgressbarLocationStock();
                        UpdateProgressbarArticleStock();
                        ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedpickingMaterial.Article.ArticleWarehouseLocation.MinimumStock);

                        if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                        {
                            OnFocusBgColor = "#FF083493";
                            OnFocusFgColor = "White";
                        }
                        else
                        {
                            OnFocusBgColor = "#FF2AB7FF";
                            OnFocusFgColor = "Black";
                        }

                        if (currentPickingMaterialIndex + 1 == SelectedLocationList.Count)
                        {
                            if (currentLocationIndex >= ArticleLocationList.Count - 1)
                            {
                                IsNextButtonEnable = false;
                            }
                            for (int item = currentLocationIndex + 1; item <= ArticleLocationList.Count - 1; item++)
                            {
                                List<WMSPickingMaterials> material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
                                if (material.Any(x => x.DownloadQuantity > 0))
                                {
                                    IsNextButtonEnable = true;
                                    break;
                                }
                                else
                                {
                                    IsNextButtonEnable = false;
                                }
                            }
                        }
                        else
                        {
                            IsNextButtonEnable = true;
                        }

                        if (currentPickingMaterial.IdArticle != selectedpickingMaterial.IdArticle)
                            ChangePickingMethodology(true);
                    }

                }
                else
                {
                    //last item in last location calc currentLocationIndex++ so when 
                    //back is pressed it will come to same page so if IsNextButtonEnable 
                    //then go to next location  
                    if (IsNextButtonEnable)
                    {
                        CalculateLocationIndex(LocationIndex.NextLocation);
                        GoToLocation(currentLocationIndex, LocationIndex.NextLocation);
                    }
                }
            }
            else
            {
                CalculateLocationIndex(LocationIndex.NextLocation);
                GoToLocation(currentLocationIndex, LocationIndex.NextLocation);
            }

        }

        private void CalculateLocationIndex(LocationIndex locationIndex)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method CalculateLocationIndex....", category: Category.Info, priority: Priority.Low);
                List<WMSPickingMaterials> material;
                switch (locationIndex)
                {
                    case LocationIndex.NextLocation:
                        {
                            currentLocationIndex++;
                            for (int item = currentLocationIndex; item <= ArticleLocationList.Count - 1; item++)
                            {
                                material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
                                if (material.Any(x => x.DownloadQuantity > 0))
                                {
                                    currentLocationIndex = item;
                                    break;
                                }
                                currentLocationIndex++;
                            }
                            SetBackButtonEnable();
                            SetNextButtonEnable();

                        }
                        break;
                    case LocationIndex.PreviousLocation:
                        {
                            currentLocationIndex--;
                            for (int item = currentLocationIndex; item >= 0; item--)
                            {
                                material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
                                if (material.Any(x => x.DownloadQuantity > 0))
                                {
                                    currentLocationIndex = item;
                                    break;
                                }
                                currentLocationIndex--;
                            }
                            SetBackButtonEnable();
                            SetNextButtonEnable();
                        }

                        break;
                }

                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method CalculateLocationIndex........executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method CalculateLocationIndex...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SetBackButtonEnable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method SetBackButtonEnable....", category: Category.Info, priority: Priority.Low);
                for (int item = currentLocationIndex - 1; item >= 0; item--)
                {
                    List<WMSPickingMaterials> material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
                    if (material.Any(x => x.DownloadQuantity > 0))
                    {
                        IsBackButtonEnable = true;
                        break;
                    }
                    else
                    {
                        IsBackButtonEnable = false;
                    }
                }

                if (currentLocationIndex <= 0)
                {
                    IsBackButtonEnable = false;
                }
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method SetBackButtonEnable........executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method SetBackButtonEnable...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SetNextButtonEnable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method SetNextButtonEnable....", category: Category.Info, priority: Priority.Low);
                for (int item = currentLocationIndex + 1; item <= ArticleLocationList.Count - 1; item++)
                {
                    List<WMSPickingMaterials> material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
                    if (material.Any(x => x.DownloadQuantity > 0))
                    {
                        IsNextButtonEnable = true;
                        break;
                    }
                    else
                    {
                        IsNextButtonEnable = false;
                    }
                }

                if (currentLocationIndex >= ArticleLocationList.Count - 1)
                {
                    IsNextButtonEnable = false;
                }
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method SetNextButtonEnable........executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method SetNextButtonEnable...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for change Location stock color as per condition.
        /// </summary>
        /// <param name="locationStock"></param>
        /// <param name="minQuantity"></param>
        private void ChangeLocationStockColor(Int64 locationStock, Int64 minQuantity)
        {
            if (locationStock == 0)
            {
                LocationStockFgColor = "#FFFFFFFF";
                LocationStockBgColor = "#FFFF0000";
            }
            else if (locationStock >= minQuantity)
            {
                LocationStockFgColor = "#FFFFFFFF";
                LocationStockBgColor = "#FF008000";
            }
            else if (locationStock < minQuantity)
            {
                LocationStockFgColor = "#FF000000";
                LocationStockBgColor = "#FFFFFF00";
            }
        }

        /// <summary>
        /// to update Progressbar of LocationStock
        /// </summary>
        private void UpdateProgressbarLocationStock()
        {
            GeosApplication.Instance.Logger.Log("Method UpdateProgressbarLocationStock....", category: Category.Info, priority: Priority.Low);
            try
            {
                ProgressbarLocationStock = WarehouseService.GetArticleStockByWarehouseLocation(SelectedpickingMaterial.IdArticle, SelectedpickingMaterial.Article.ArticleWarehouseLocation.IdWarehouseLocation, SelectedpickingMaterial.IdWarehouse);
                LocationStockValue = ProgressbarLocationStock.LocationStock;

                if (ProgressbarLocationStock.MaximumStock == 0)
                    LocationStockValue = 0;
                else if (ProgressbarLocationStock.MaximumStock < LocationStockValue)
                    LocationStockValue = ProgressbarLocationStock.MaximumStock;

                //if (ProgressbarLocationStock.MaximumStock == 0)
                //    ProgressbarLocationStock.MaximumStock = ProgressbarLocationStock.MaximumStock < ProgressbarLocationStock.LocationStock ? ProgressbarLocationStock.LocationStock : ProgressbarLocationStock.MaximumStock;
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateProgressbarLocationStock Method ScanReference() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateProgressbarLocationStock Method ScanReference() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method UpdateProgressbarLocationStock...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method UpdateProgressbarLocationStock....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        ///  to update Progressbar of ArticleStock for a warehouse 
        ///  [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        private void UpdateProgressbarArticleStock()
        {
            try
            {
                ProgressbarArticleStock = new ArticleWarehouseLocations();
                // [001] Changed Service method
                ProgressbarArticleStock = WarehouseService.GetAVGStockByIdArticle_V2034(SelectedpickingMaterial.IdArticle, WarehouseCommon.Instance.Selectedwarehouse);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateProgressbarArticleStock Method ScanReference() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateProgressbarArticleStock Method ScanReference() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method UpdateProgressbarArticleStock...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void OnTimedEvent(object source, EventArgs e)
        {
            if (IsTimer)
            {
                OtWorkingTime = OtWorkingTime.Add(timer.Interval);
                UpdateTime();
                #region IsTimer
                //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
                try
                {
                    if (IsStartTimer)
                    {
                        DateTime CurrTime = DateTime.Now;
                        TimeSpan span = CurrTime.Subtract(StartTime);
                        if (span.TotalMinutes >= WarehouseCommon.Instance.InactivityMinutes)
                        {
                            IsTimer = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method OnTimedEvent()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
            }
        }

        /// <summary>
        /// Set Text to Digital Gauge Control
        /// </summary>
        void UpdateTime()
        {
            digitalGaugeControl.Text = OtWorkingTime.ToString(@"hh\:mm");
        }

        /// <summary>
        /// Digital Gauge Control Loaded
        /// </summary>
        /// <param name="obj"></param>
        private void DigitalGaugeControlLoadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method DigitalGaugeControlLoadedCommandAction....", category: Category.Info, priority: Priority.Low);
                if (timerCount == 0)
                {
                    digitalGaugeControl = (DigitalGaugeControl)obj;
                    IsTimer = PreviousTimerValue;
                    if (PickingOt != null && !IsTimer)
                    {
                        OtWorkingTime = WarehouseService.GetOTTotalWorkingTime(PickingOt.IdOT, 12, WarehouseCommon.Instance.Selectedwarehouse);
                        digitalGaugeControl.Text = OtWorkingTime.ToString(@"hh\:mm");
                    }
                }
                else
                {
                    DigitalGaugeControl tempdigitalGaugeControl = (DigitalGaugeControl)obj;
                    tempdigitalGaugeControl.Text = digitalGaugeControl.Text;
                    digitalGaugeControl = tempdigitalGaugeControl;
                }
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method DigitalGaugeControlLoadedCommandAction........executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DigitalGaugeControlLoadedCommandAction Method ...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public bool CanCloseMethod()
        {
            bool result = true;
            try
            {
                // Implement your logic here
                // Return true to allow closing, false to cancel
                DateTime? startTime;
                DateTime? endTime;
                startTime = null;
                endTime = DateTime.Now;
                OTWorkingTime tempOTWorkingTime = new OTWorkingTime()
                {
                    IdOT = PickingOt.IdOT,
                    IdStage = 12,
                    IdOperator = GeosApplication.Instance.ActiveUser.IdUser,
                    StartTime = startTime,
                    EndTime = endTime,
                    TransactionOperation = Data.Common.ModelBase.TransactionOperations.Update
                };
                result = WarehouseService.AddOTWorkingTime_V2550(WarehouseCommon.Instance.Selectedwarehouse, tempOTWorkingTime);
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method CanCloseMethod........executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CanCloseMethod() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return result;
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CanCloseMethod() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                return result;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method CanCloseMethod()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return result;
            }
            return result; // or false depending on your logic
        }

        /// <summary>
        /// Method to add OT Working Time 
        /// </summary>
        private void AddOTWorkingTime()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method AddOTWorkingTime....", category: Category.Info, priority: Priority.Low);
                DateTime? startTime;
                DateTime? endTime;
                if (IsTimer)
                {
                    startTime = DateTime.Now;
                    endTime = null;
                    OTWorkingTime tempOTWorkingTime = new OTWorkingTime()
                    {
                        IdOT = PickingOt.IdOT,
                        IdStage = 12,
                        IdOperator = GeosApplication.Instance.ActiveUser.IdUser,
                        StartTime = startTime,
                        EndTime = endTime,
                        TransactionOperation = Data.Common.ModelBase.TransactionOperations.Add
                    };
                    bool result = WarehouseService.AddOTWorkingTime_V2550(WarehouseCommon.Instance.Selectedwarehouse, tempOTWorkingTime);
                }
                else if (!IsTimer && timerCount > 0)
                {
                    startTime = null;
                    endTime = DateTime.Now;
                    OTWorkingTime tempOTWorkingTime = new OTWorkingTime()
                    {
                        IdOT = PickingOt.IdOT,
                        IdStage = 12,
                        IdOperator = GeosApplication.Instance.ActiveUser.IdUser,
                        StartTime = startTime,
                        EndTime = endTime,
                        TransactionOperation = Data.Common.ModelBase.TransactionOperations.Update
                    };
                    bool result = WarehouseService.AddOTWorkingTime_V2550(WarehouseCommon.Instance.Selectedwarehouse, tempOTWorkingTime);
                }
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method AddOTWorkingTime........executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddOTWorkingTime() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddOTWorkingTime() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method AddOTWorkingTime()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ImageClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....", category: Category.Info, priority: Priority.Low);

            try
            {
                WMSPickingMaterials picking = (WMSPickingMaterials)obj;
                IdWareHouseDeliveryNoteItem = picking.IdWareHouseDeliveryNoteItem;
                SelectedLocationList.Where(a => a.IdWareHouseDeliveryNoteItem != IdWareHouseDeliveryNoteItem && a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                SelectedLocationList.Where(a => a.IdWareHouseDeliveryNoteItem == IdWareHouseDeliveryNoteItem).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ImageClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }

        private void CommandKeyDownAction(object obj)
        {

            GeosApplication.Instance.Logger.Log("Method CommandKeyDownAction....", category: Category.Info, priority: Priority.Low);

            try
            {
                var Type = ((System.Windows.RoutedEventArgs)obj).OriginalSource.GetType();
                if (Type.Name != "Image")
                {
                    if (IdWareHouseDeliveryNoteItem > 0)
                    {
                        SelectedLocationList.Where(a => a.IdWareHouseDeliveryNoteItem == IdWareHouseDeliveryNoteItem).ToList().ForEach(a => { a.ShowComment = false; });
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in CommandKeyDownAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method CommandKeyDownAction....executed ", category: Category.Info, priority: Priority.Low);
        }

        //Shubham[skadam] GEOS2-4436 Ask for location inventory after do the picking of certain references (2/3)  23 10 2023 
        public void ScanInventoryWizzard()
        {
            try
            {
                if (IsSameReference)
                {
                    inventoryWizzardView = new InventoryWizzardView();
                    inventoryWizzardViewModel = new InventoryWizzardViewModel();
                    EventHandler Close = delegate { inventoryWizzardView.Close(); };
                    inventoryWizzardViewModel.RequestClose += Close;
                    inventoryWizzardView.DataContext = inventoryWizzardViewModel;
                    //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
                    inventoryWizzardViewModel.SelectedLocationList = SelectedLocationList.Where(x => x.Article.IdArticle == SelectedpickingMaterial.Article.IdArticle).ToList(); // [nsatpute][11-10-2024][nsatpute]
                    inventoryWizzardViewModel.SelectedpickingMaterial = SelectedpickingMaterial;
                    inventoryWizzardViewModel.WorkOrder = PickingOt.Code;
                    inventoryWizzardViewModel.ScanBarcodeAction(LocationName);
                    inventoryWizzardView.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ScanInventoryWizzard. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void ISScanInventoryWizzard()
        {
            try
            {
                //if (IsSameReference)
                {
                    inventoryWizzardView = new InventoryWizzardView();
                    inventoryWizzardViewModel = new InventoryWizzardViewModel();
                    EventHandler Close = delegate { inventoryWizzardView.Close(); };
                    inventoryWizzardViewModel.RequestClose += Close;
                    inventoryWizzardView.DataContext = inventoryWizzardViewModel;
                    //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
                    inventoryWizzardViewModel.SelectedLocationList = SelectedLocationList;
                    inventoryWizzardViewModel.SelectedpickingMaterial = SelectedpickingMaterial;
                    inventoryWizzardViewModel.WorkOrder = PickingOt.Code;
                    inventoryWizzardViewModel.ScanBarcodeAction(LocationName);
                    //inventoryWizzardView.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ISScanInventoryWizzard. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rajashri][GEOS2-4849][28.10.2023]
        public void TransitionWorkflowStatus(long IdOT)
        {

            WorkflowStatus picking = WorkflowStatusList.FirstOrDefault(i => i.Name.ToLower() == "picking");
            WorkflowStatus OldWorkflowStatus = WorkflowStatusList.FirstOrDefault(i => i.IdWorkflowStatus == PickingOt.IdWorkflowStatus);
            List<LogEntriesByOT> LogEntriesByOTList = new List<LogEntriesByOT>();
            List<WorkflowTransition> NextAvailableWorkflowStatusList = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == WorkflowStatus.IdWorkflowStatus).ToList();
            if (NextAvailableWorkflowStatusList.Any(i => i.IdWorkflowStatusTo == picking.IdWorkflowStatus) && picking.IdWorkflowStatus != WorkflowStatus.IdWorkflowStatus)
            {
                LogEntriesByOT logEntriesByOT = new LogEntriesByOT();
                logEntriesByOT.IdOT = IdOT;
                logEntriesByOT.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                logEntriesByOT.Comments = string.Format(System.Windows.Application.Current.FindResource("WorkflowStatusLogEntryByPO").ToString(), OldWorkflowStatus.Name, picking.Name);
                logEntriesByOT.IdLogEntryType = 1;
                logEntriesByOT.IsRtfText = false;
                LogEntriesByOTList.Add(logEntriesByOT);
                IsSaveChanges = WarehouseService.UpdateWorkflowStatusInOT_V2450(IdOT, picking.IdWorkflowStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByOTList);
                if (IsSaveChanges)
                    WorkflowStatus = picking;
            }
            List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == WorkflowStatus.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
            WorkflowStatusButtons = new List<WorkflowStatus>();

            foreach (byte statusbutton in GetCurrentButtons)
            {
                WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
            }
            WorkflowStatusButtons = new List<WorkflowStatus>(WorkflowStatusButtons);
        }

        //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
        private void FillGeosAppSettingList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGeosAppSettingList()...", category: Category.Info, priority: Priority.Low);

                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("118");
                if (GeosAppSettingList.Count > 0)
                {
                    WarehouseCommon.Instance.InactivityMinutes = Convert.ToInt32(GeosAppSettingList[0].DefaultValue);
                }
                GeosApplication.Instance.Logger.Log("Method FillGeosAppSettingList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosAppSettingList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGeosAppSettingList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillGeosAppSettingList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
        private void MouseMoveAction(object obj)
        {
            try
            {
                if (IsStartTimer)
                {
                    StartTime = DateTime.Now;
                    if (IsTimer == false)
                    {
                        IsTimer = true;
                    }
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method MouseMoveAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
        private void EnterKeyAction(object obj)
        {
            try
            {
                if (IsStartTimer)
                {
                    StartTime = DateTime.Now;
                    if (IsTimer == false)
                    {
                        IsTimer = true;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in EnterKeyCommand(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
        private void PreviewKeyDownAction(object obj)
        {
            try
            {
                if (obj is KeyEventArgs)
                {
                    if (IsStartTimer)
                    {
                        StartTime = DateTime.Now;
                        if (IsTimer == false)
                        {
                            IsTimer = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in PreviewKeyDownAction(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        void OnInactivityTimedEvent(object source, EventArgs e)
        {
            if (IsTimer)
            {
                #region IsTimer
                //Shubham[skadam] GEOS2-5168 Timer automático 15 02 2024
                try
                {
                    if (IsStartTimer)
                    {
                        DateTime CurrTime = DateTime.Now;
                        TimeSpan span = CurrTime.Subtract(StartTime);
                        if (span.TotalMinutes >= WarehouseCommon.Instance.InactivityMinutes)
                        {
                            IsTimer = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method OnInactivityTimedEvent()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
            }
        }
        #endregion


        #region [pallavi jadhav][GEOS2-7021][10-04-2025]

        private void FillPikingNotificationMails(Warehouses objWarehouse)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method SendNotificationMailCommandAction....", category: Category.Info, priority: Priority.Low);

                if (SendNotificationMailList == null)
                {
                    SendNotificationMailList = new List<SendNotificationMail>();
                }
                SendNotificationMailList = WarehouseService.GetPikingNotification_V2630(objWarehouse);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Successfully executed..... FillPikingNotificationMails()", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in FillPikingNotificationMails(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SendNotificationMailCommandAction(object obj)  // [pallavi jadhav][GEOS2-7022][10 - 04 - 2025]
        {

            try
            {


                GeosApplication.Instance.Logger.Log(" Method SendNotificationMailCommandAction....", category: Category.Info, priority: Priority.Low);

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

                if (obj is object[] selectedRowData)
                {
                    var temp = selectedRowData[1] as Emdep.Geos.Data.Common.WMS.WMSPickingMaterials;

                    if (temp != null)
                    {
                        string Reference = temp.Reference;
                        string MultipleQTY = Convert.ToString(temp.RevisionItem.Quantity);
                        string PickedQTY = Convert.ToString(temp.DownloadQuantity);
                        #region[rani dhamankar][16-04-2025][GEOS2-7021]
                        Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                        ItemPickerDetails PickerDetails = null;
                        PickerDetails = WarehouseService.GetPickerNameBYDeliveryNoteItem_V2630(temp.IdWareHouseDeliveryNoteItem, warehouse);
                        string Picker = string.Empty;
                        if (PickerDetails != null)
                        {
                            Picker = PickerDetails.ModifiedByName + " " + PickerDetails.ModifiedBySurName;
                        }
                        #endregion
                        if (SendNotificationMailList != null)
                        {
                            foreach (var item in SendNotificationMailList)
                            {
                                bool mailsended = WarehouseService.SendNotificationMail(item.Warehouse, item.EmployeeContactValue, Reference, MultipleQTY, PickedQTY, Picker);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in SendNotificationMailCommandAction(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            //IsSameReferenceWithRequiresCountAfterPicking = false;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method Successfully executed..... SendNotificationMailCommandAction()", category: Category.Info, priority: Priority.Low);
        }

        #endregion
    }
}


