using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Map;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
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

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }

        #endregion //End Of Services

        #region Declaration

        private List<OtItem> otItemList;
        private PickingMaterials selectedpickingMaterial;
        private Visibility isLocationIndicatorVisible;
        private Visibility isInformationVisible;
        private string bgColorLocation;
        private string barcodeStr;
        private string locationName;
        private bool isLocationScaned;
        private string rowBgColor;
        private string totalStockBgColor;
        private string totalStockFgColor;
        private string onFocusBgColor;
        private string onFocusFgColor;
        List<string> articleLocationList = new List<string>();
        private Int64 totalCurrentItemStock;
        private ImageSource articleImage;
        private PickingMaterials currentPickingMaterial;
        private List<PickingMaterials> selectedLocationList;

        int currentLocationIndex = 0;
        int countLocation = 0;
        int indexItem = 0;
        public List<PickingMaterials> materialSoredList = new List<PickingMaterials>();
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

        public List<OtItem> OtItemList
        {
            get { return otItemList; }
            set
            {
                otItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItemList"));
            }
        }

        public PickingMaterials SelectedpickingMaterial
        {
            get { return selectedpickingMaterial; }
            set
            {
                selectedpickingMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedpickingMaterial"));
            }
        }

        public List<PickingMaterials> SelectedLocationList
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
            IsLocationIndicatorVisible = Visibility.Visible;
            IsInformationVisible = Visibility.Collapsed;
            IsLocationScaned = false;
            IsNextButtonEnable = true;
            IsBackButtonEnable = false;
            TrackBarEditvalue = 0;
            PrintBatchLabelEnabled = true;
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
                PickingOt = (Ots)Ot.Clone();

                if (PickingOt.CountryGroup != null && !string.IsNullOrEmpty(PickingOt.CountryGroup.Name))
                    IsProducerVisible = Visibility.Visible;

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
                        materialSoredList.Add(material);
                    }

                    //Nested decomposition articles.
                    if (item.ArticleDecomposedList != null && item.ArticleDecomposedList.Count > 0)
                    {
                        DecompositionArticles(item.ArticleDecomposedList);
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
                        OtItem otitem = OtItemList.FirstOrDefault(x => x.RevisionItem.NumItem == otItemNumber);

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
                        PickingMaterials pickingMaterial = materialSoredList.FirstOrDefault(x => x.RevisionItem.NumItem == FIFOGotoItem);
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

        private void ChangePickingMethodology()
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
                string message = FollowFIFO ? System.Windows.Application.Current.FindResource("PickingMaterialFIFOEnable").ToString()
                                            : System.Windows.Application.Current.FindResource("PickingMaterialFIFODisabled").ToString();

                CustomMessageBox.Show(message, Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);

                if (CustomMessageBox.msgboxresult == MessageBoxResult.Yes)
                {
                    if (SelectedpickingMaterial != null)
                    {
                        FIFOGotoItem = SelectedpickingMaterial.RevisionItem.NumItem;
                    }
                    else if (SelectedSerialNumber != null && SelectedSerialNumber.MasterItem != null)
                    {
                        FIFOGotoItem = ((PickingMaterials)SelectedSerialNumber.MasterItem).RevisionItem.NumItem;
                    }
                    else if (SelectedLocationList != null && SelectedLocationList.Count > 0)
                    {
                        FIFOGotoItem = SelectedLocationList.FirstOrDefault().RevisionItem.NumItem;
                    }
                    else
                    {
                        FIFOGotoItem = null;
                    }

                    //Observation observation = new Observation { IdRevisionItem = PickingOt.IdOT, IdUser = GeosApplication.Instance.ActiveUser.IdUser };
                    //observation.Text = FollowFIFO ? "FIFO has been enabled in picking" : "FIFO has been disabled in picking";
                    //  WarehouseService.AddObservation(observation);

                    Notification notification = new Notification();
                    notification.IsNew = 1;
                    notification.IdModule = 6;
                    notification.Status = "Unread";
                    notification.Title = FollowFIFO ? "FIFO has been enabled in picking" : "FIFO has been disabled in picking";
                    notification.FromUser = GeosApplication.Instance.ActiveUser.IdUser;
                    notification.Message = FollowFIFO ? "FIFO has been enabled in picking for OT " + PickingOt.Code + " by " + GeosApplication.Instance.ActiveUser.FullName + "."
                                                      : "FIFO has been disabled in picking for OT " + PickingOt.Code + " by " + GeosApplication.Instance.ActiveUser.FullName + ".";
                    WarehouseService.AddNotification(notification);
                    PreviousTimerValue = IsTimer;
                    if (IsTimer)
                        IsTimer = false;
                    RequestClose(null, null);
                }
                else
                {
                    SetFollowFIFO(!FollowFIFO, null);
                }

                GeosApplication.Instance.Logger.Log("Method ChangePickingMethodology....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangePickingMethodology...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Selected PickingMethodology
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][cpatil][29-08-2019][GEOS2-1666]FIFO error
        /// [003][cpatil][17-09-2020][GEOS2-2415]Add Date of Expiry in Article comments
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
                    // [001] Changed Service method
                    // [003] Changed Service method
                    //OtItemList = WarehouseService.GetRemainingOtItemsByIdOt_V2034(Ot.IdOT, objWarehouse);
                    OtItemList = WarehouseService.GetRemainingOtItemsByIdOt_V2051(Ot.IdOT, objWarehouse);
                }
                else
                {
                    // [002] Changed Service method
                    // [003] Changed Service method
                    //OtItemList = WarehouseService.GetRemainingOtItemsByIdOtDisbaledFIFO_V2035(Ot.IdOT, objWarehouse);
                    OtItemList = WarehouseService.GetRemainingOtItemsByIdOtDisbaledFIFO_V2051(Ot.IdOT, objWarehouse);
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
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
                if (obj.Text == "\r" && !isLocationLast)
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
                            ScanAction(0, 0);
                            IsLocationScaned = true;
                            IsWrongLocationErrorVisible = Visibility.Hidden;
                            WrongLocation = "";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
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
                        PickingMaterials pm = SelectedSerialNumber.MasterItem as PickingMaterials;

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
                                    IsScanItem = WarehouseService.InsertIntoArticleStock(pm);
                                }

                                if (IsScanItem)
                                {
                                    //Removed to avoid repeat scan of same item.
                                    pm.SerialNumbers.Remove(pm.SerialNumbers.FirstOrDefault(x => x.IdSerialNumber == sn.IdSerialNumber));

                                    //Upadate total stock of article.
                                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(pm.IdArticle, Convert.ToInt32(pm.IdWarehouse));
                                    changeTotalStockColor(TotalCurrentItemStock, pm.MinimumStock);

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

                            if (PickingOt.ProducerIdCountryGroup != null
                                && PickingOt.ProducerIdCountryGroup != 0 && SelectedpickingMaterial.IdCountryGroup != 0
                                && PickingOt.ProducerIdCountryGroup != SelectedpickingMaterial.IdCountryGroup)
                            {
                                isSameProducer = false;
                            }

                            if (SelectedpickingMaterial == null)
                            {
                                SetViewInErrorState("All quantity picked.");
                                return;
                            }

                            if (IsWarehouseBelongsToSameSite && isSameProducer
                                && SelectedpickingMaterial.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem
                                && TotalCurrentItemStock >= BarcodeScannedQuantity)
                            {
                                if (SelectedpickingMaterial.Manufacturer == null)
                                {
                                    SetViewInErrorState("Wrong Item " + BarcodeStr + ". Missing Producer");
                                    return;
                                }

                                Int64 locationAvaibleQuantity = WarehouseService.GetStockForScanItem(SelectedpickingMaterial.IdArticle, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, _idWareHouseDeliveryNoteItem, SelectedpickingMaterial.IdWarehouseLocation);

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
                                            PickingMaterials scanPickingMaterialItem = materialSoredList.Where(i => i.IdArticle == SelectedpickingMaterial.IdArticle && i.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem && i.RevisionItem != null && i.RevisionItem.NumItem == SelectedpickingMaterial.RevisionItem.NumItem.ToString()).FirstOrDefault();

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

                                    ScanAction(_idWareHouseDeliveryNoteItem, BarcodeScannedQuantity);
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
                                IsWrongItemErrorVisible = Visibility.Visible;
                                WrongItem = "Wrong Item " + BarcodeStr;
                                BarcodeStr = "";
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
                    BarcodeStr = BarcodeStr + obj.Text;
                }

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
        private void ScanAction(Int64 idWarehouseDeliveryNoteItem, Int64 quantity)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);

                if (countLocation == 0)
                {
                    indexItem = 0;
                    SelectedpickingMaterial = SelectedLocationList[indexItem];
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

                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                    changeTotalStockColor(TotalCurrentItemStock, SelectedpickingMaterial.MinimumStock);
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
                        OtItem otitem = null;
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
                            IsScanItem = WarehouseService.InsertIntoArticleStock(SelectedpickingMaterial);
                        }

                        if (IsScanItem)
                        {
                            TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                            changeTotalStockColor(TotalCurrentItemStock, SelectedpickingMaterial.MinimumStock);

                            //Update status and comments
                            //Decomposition otitems - if all decomposition otitem articles are downloaded then otitem status is changed to finish
                            if (isDecompositionArticle)
                            {
                                bool allDownloaded = AreAllDecompositionOtItemsDownloaded(otitem.ArticleDecomposedList);
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

                                        List<PickingMaterials> printList = materialSoredList.Where(x => x.IdOtitem == SelectedpickingMaterial.IdOtitem && x.IdWarehouseProductComponent == SelectedpickingMaterial.IdWarehouseProductComponent).ToList();

                                        printList.ToList().ForEach(x => { x.IsPartialBatchPrinted = true; });

                                        List<PickingMaterials> GroupedByMadeIn = printList.GroupBy(l => l.MadeIn)
                                                                                .Select(cl => new PickingMaterials
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

                                        foreach (PickingMaterials pickingMaterial in GroupedByMadeIn)
                                        {
                                            if (pickingMaterial.BatchPrintQuantity > 0)
                                                PrintPickedMaterial(pickingMaterial, otitem);
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

                                    List<PickingMaterials> printList = materialSoredList.Where(x => x.IdOtitem == SelectedpickingMaterial.IdOtitem).ToList();
                                    printList.ToList().ForEach(x => { x.IsPartialBatchPrinted = true; });

                                    List<PickingMaterials> GroupedByMadeIn = printList.GroupBy(l => l.MadeIn)
                                                                            .Select(cl => new PickingMaterials
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

                                    foreach (PickingMaterials pickingMaterial in GroupedByMadeIn)
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
                                    PrintPickedMaterial(SelectedpickingMaterial, otitem);
                                }
                                else
                                {
                                    PrintPickedMaterial(SelectedpickingMaterial);
                                }
                            }
                        }

                        TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
                        changeTotalStockColor(TotalCurrentItemStock, SelectedpickingMaterial.MinimumStock);
                        UpdateProgressbarArticleStock();
                        UpdateProgressbarLocationStock();
                        ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedpickingMaterial.Article.ArticleWarehouseLocation.MinimumStock);

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

                SelectedLocationList = new List<PickingMaterials>(SelectedLocationList);
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

        /// <summary>
        /// Method for change Total stock color as per condition.
        /// </summary>
        /// <param name="totalStock"></param>
        /// <param name="minQuantity"></param>
        private void changeTotalStockColor(Int64 totalStock, Int64 minQuantity)
        {
            if (totalStock == 0)
            {
                TotalStockFgColor = "#FFFFFFFF";
                TotalStockBgColor = "#FFFF0000";
            }
            else if (totalStock >= minQuantity)
            {
                TotalStockFgColor = "#FFFFFFFF";
                TotalStockBgColor = "#FF008000";
            }
            else if (totalStock < minQuantity)
            {
                TotalStockFgColor = "#FF000000";
                TotalStockBgColor = "#FFFFFF00";
            }
        }

        //[WMS M049-16]
        private void PrintPickedMaterial(PickingMaterials pickingMaterial, OtItem otitem = null)
        {
            GeosApplication.Instance.Logger.Log(" PickingMaterialsViewModel Method printPickedMaterial....", category: Category.Info, priority: Priority.Low);

            try
            {
                string barcode = pickingMaterial.PartNumberCode;

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
                }
                else    //Printing label after every scan
                {
                    if (pickingMaterial.RegisterSerialNumber == 1)
                        printValues.Add("@QUANTITY", "1");
                    else
                        printValues.Add("@QUANTITY", Math.Abs(pickingMaterial.ScannedQty).ToString());
                }

                printValues.Add("@USER", GeosApplication.Instance.ActiveUser.IdUser.ToString());
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
                PickingMaterials pm = SelectedSerialNumber.MasterItem as PickingMaterials;
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
                    PickingMaterials pm = SelectedSerialNumber.MasterItem as PickingMaterials;
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
                        changeTotalStockColor(TotalCurrentItemStock, SelectedpickingMaterial.MinimumStock);
                }
            }

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
                    PickingMaterials pm = (PickingMaterials)SelectedSerialNumber.MasterItem;
                    pm.ScanSerialNumbers.Remove(pm.ScanSerialNumbers.FirstOrDefault(x => x.Code == null));
                    SelectedSerialNumber = null;
                    //pmItem.ScanSerialNumbers = new List<SerialNumber>();
                    //pmItem.DownloadQuantity = pmItem.SerialNumbers.Count;
                }

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

                bool result = WarehouseService.AddCancelledOTWorkingTime(WarehouseCommon.Instance.Selectedwarehouse, tempOTWorkingTime);

                IsCanceled = true;
                RequestClose(null, null);

                string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                if (Directory.Exists(basePath))
                {
                    Directory.Delete(basePath, true);
                }
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
        private void PrintBatchLabelPartialDownloadedItem(PickingMaterials SerialNumberPickingMaterials = null)
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
                    OtItem otitem = null;
                    if (SelectedpickingMaterial.IdWarehouseProductComponent > 0)
                        otitem = OtItemList.FirstOrDefault(x => x.IdOTItem == SelectedpickingMaterial.IdOtitem);

                    PrintPickedMaterial(SelectedpickingMaterial, otitem);
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

                PickingMaterials article = (PickingMaterials)obj;
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

        private void GoToNextItem(PickingMaterials currentPickingMaterial)
        {
            int currentPickingMaterialIndex = SelectedLocationList.IndexOf(currentPickingMaterial);
            this.currentPickingMaterial = currentPickingMaterial;
            if (currentPickingMaterialIndex + 1 < SelectedLocationList.Count)
            {
                SelectedpickingMaterial = SelectedLocationList[currentPickingMaterialIndex + 1];
                currentPickingMaterialIndex++;
                ArticleImage = ByteArrayToImage(SelectedpickingMaterial.ArticleImageInBytes);

                TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedpickingMaterial.IdArticle, Convert.ToInt32(SelectedpickingMaterial.IdWarehouse));
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
                        List<PickingMaterials> material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
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
                    IsNextButtonEnable = true;
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

        private void CalculateLocationIndex(LocationIndex locationIndex)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method CalculateLocationIndex....", category: Category.Info, priority: Priority.Low);
                List<PickingMaterials> material;
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
                    List<PickingMaterials> material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
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
                    List<PickingMaterials> material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
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
                        EndTime = endTime
                    };
                    bool result = WarehouseService.AddOTWorkingTime(WarehouseCommon.Instance.Selectedwarehouse, tempOTWorkingTime);
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
                        EndTime = endTime
                    };
                    bool result = WarehouseService.AddOTWorkingTime(WarehouseCommon.Instance.Selectedwarehouse, tempOTWorkingTime);
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
                PickingMaterials picking = (PickingMaterials)obj;
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

        #endregion
    }
}


