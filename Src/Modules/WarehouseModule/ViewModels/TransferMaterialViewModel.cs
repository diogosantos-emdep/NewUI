using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Map;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
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

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class TransferMaterialViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services

        #region Declaration
        private Visibility isFromLocationIndicatorVisible;
        private Visibility isToLocationIndicatorVisible;
        private Visibility isInformationVisible;
        private string bgColorLocation;
        private string bgColorFromLocation;
        private string bgColorToLocation;
        private string barcodeStr;
        private string fromLocationName;
        private string toLocationName;
        private bool isLocationScaned;
        private bool isFromLocationScaned;
        private bool isToLocationScaned;
        private string rowBgColor;
        private string totalStockBgColor;
        private string totalStockFgColor;
        private string onFocusBgColor;
        private string onFocusFgColor;
        private TransferMaterials tempItem;
        List<string> articleLocationList = new List<string>();
        private Int64 totalCurrentItemStock;
        private ImageSource articleImage;

        private List<ArticleWarehouseLocations> articleWarehouseLocationsList;
        private TransferMaterials selectedTransferMaterials;

        int locationIndex = 0;
        int countLocation = 0;
        int indexItem = 0;

        List<TransferMaterials> transferMaterialsList;
        List<TransferMaterials> mainTransferMaterialsList;
        bool isNextButtonEnable;
        bool isLocationLast;

        private int trackBarEditvalue;
        private string trackBarFgColor;
        private string fgColorLocation;
        private Int64 fromIdWarehouseLocation;

        private string wrongFromLocation;
        private Visibility isWrongFromLocationErrorVisible = Visibility.Hidden;

        private string wrongItem;
        private Visibility isWrongItemErrorVisible = Visibility.Hidden;

        private string wrongToLocation;
        private Visibility isWrongToLocationErrorVisible = Visibility.Hidden;

        private string locationStockBgColor;
        private string locationStockFgColor;
        private ArticleWarehouseLocations progressbarArticleStock;
        private ArticleWarehouseLocations progressbarLocationStock;
        private long locationStockValue;
        private bool focusUserControl;
        private int windowWidth;
        private int windowHeight;
        private ObservableCollection<WarehouseLocation> mapItems;
        private Uri svgUri;
        private ObservableCollection<WarehouseLocation> warehouseAllLocationList;
        //private bool isScanSerialNumbers;
        private SerialNumber selectedSerialNumber;
        private bool isScanSerialNumber;
        private string materialSerialNumbers;
        private long barcodeScannedQuantity;

        #endregion

        #region Public Properties

        public string FgColorLocation
        {
            get { return fgColorLocation; }
            set
            {
                fgColorLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FgColorLocation"));
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
        public List<TransferMaterials> TransferMaterialsList
        {
            get { return transferMaterialsList; }
            set
            {
                transferMaterialsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TransferMaterialsList"));
            }
        }

        public List<TransferMaterials> MainTransferMaterialsList
        {
            get { return mainTransferMaterialsList; }
            set
            {
                mainTransferMaterialsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainTransferMaterialsList"));
            }
        }

        public Visibility IsFromLocationIndicatorVisible
        {
            get { return isFromLocationIndicatorVisible; }
            set
            {
                isFromLocationIndicatorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFromLocationIndicatorVisible"));
            }
        }

        public Visibility IsToLocationIndicatorVisible
        {
            get { return isToLocationIndicatorVisible; }
            set
            {
                isToLocationIndicatorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsToLocationIndicatorVisible"));
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

        public string BgColorFromLocation
        {
            get { return bgColorFromLocation; }
            set
            {
                bgColorFromLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bgColorFromLocation"));
            }
        }

        public string BgColorToLocation
        {
            get { return bgColorToLocation; }
            set
            {
                bgColorToLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BgColorToLocation"));
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

        public bool IsFromLocationScaned
        {
            get { return isFromLocationScaned; }
            set
            {
                isFromLocationScaned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFromLocationScaned"));
            }
        }

        public bool IsToLocationScaned
        {
            get { return isToLocationScaned; }
            set
            {
                isToLocationScaned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsToLocationScaned"));
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


        public long BarcodeScannedQuantity
        {
            get { return barcodeScannedQuantity; }
            set
            {
                barcodeScannedQuantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BarcodeScannedQuantity"));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public string FromLocationName
        {
            get { return fromLocationName; }
            set
            {
                fromLocationName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromLocationName"));
            }
        }

        public string ToLocationName
        {
            get { return toLocationName; }
            set
            {
                toLocationName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToLocationName"));
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

        public List<ArticleWarehouseLocations> ArticleWarehouseLocationsList
        {
            get { return articleWarehouseLocationsList; }
            set
            {
                articleWarehouseLocationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleWarehouseLocationsList"));
            }
        }

        public TransferMaterials SelectedTransferMaterials
        {
            get { return selectedTransferMaterials; }
            set
            {
                selectedTransferMaterials = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTransferMaterials"));
            }
        }

        public Int64 FromIdWarehouseLocation
        {
            get { return fromIdWarehouseLocation; }
            set
            {
                fromIdWarehouseLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromIdWarehouseLocation"));
            }
        }

        public string WrongFromLocation
        {
            get { return wrongFromLocation; }
            set
            {
                wrongFromLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WrongFromLocation"));
            }
        }

        public Visibility IsWrongFromLocationErrorVisible
        {
            get { return isWrongFromLocationErrorVisible; }
            set
            {
                isWrongFromLocationErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongFromLocationErrorVisible"));
            }
        }

        public string WrongToLocation
        {
            get { return wrongToLocation; }
            set
            {
                wrongToLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WrongToLocation"));
            }
        }

        public Visibility IsWrongToLocationErrorVisible
        {
            get { return isWrongToLocationErrorVisible; }
            set
            {
                isWrongToLocationErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongToLocationErrorVisible"));
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

        public Visibility IsWrongItemErrorVisible
        {
            get { return isWrongItemErrorVisible; }
            set
            {
                isWrongItemErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongItemErrorVisible"));
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
        public int WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowWidth"));
            }
        }

        public int WindowHeight
        {
            get { return windowHeight; }
            set
            {
                windowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight"));
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

        public ObservableCollection<WarehouseLocation> WarehouseAllLocationList
        {
            get { return warehouseAllLocationList; }
            set
            {
                warehouseAllLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseAllLocationList"));
            }
        }

        public WarehouseLocation ToWarehouseLocation
        {
            get { return toWarehouseLocation; }
            set
            {
                toWarehouseLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToWarehouseLocation"));
            }
        }

        //public bool IsScanSerialNumbers
        //{
        //    get { return isScanSerialNumbers; }
        //    set
        //    {
        //        isScanSerialNumbers = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsScanSerialNumbers"));
        //    }
        //}

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
        //public ICommand CommandNextButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandOnLoaded { get; set; }
        public ICommand HyperlinkClickCommand { get; set; }
        public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }
        public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        #endregion // End Of ICommands

        #region Constructor

        public TransferMaterialViewModel()
        {
            WindowHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 131;
            WindowWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 100;
            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            // CommandNextButton = new DelegateCommand<object>(NextButtonCommandAction);
            CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
            CommandOnLoaded = new DelegateCommand<RoutedEventArgs>(LoadedAction); //new DelegateCommand<RoutedEventArgs>(LoadedAction);
            HyperlinkClickCommand = new DelegateCommand<object>(HyperlinkClickCommandAction);
            VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
            CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);

            IsFromLocationIndicatorVisible = Visibility.Visible;
            IsToLocationIndicatorVisible = Visibility.Hidden;
            IsInformationVisible = Visibility.Collapsed;
            IsLocationScaned = false;
            IsNextButtonEnable = true;

            //BgColorFromLocation = "#FFF4E27E";
            //RowBgColor = "#FFF4E27E";
            TrackBarEditvalue = 0;

            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
            {
                //BgColorFromLocation = "#FF083493";
                //BgColorToLocation = "#FF083493";
                FgColorLocation = "White";
                TrackBarFgColor = "#FF083493";
            }
            else
            {
                //BgColorFromLocation = "#FF2AB7FF";
                //BgColorToLocation = "#FF2AB7FF";
                FgColorLocation = "Black";
                TrackBarFgColor = "#FF2AB7FF";
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Method for forcus form when it  load.
        /// </summary>
        /// <param name="obj"></param>
        private void LoadedAction(RoutedEventArgs obj)
        {
            //WinUIDialogWindow detailView = ((WinUIDialogWindow)((System.Windows.RoutedEventArgs)obj).OriginalSource);
            //detailView.Focus();
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.TransferMaterialUserControlView", null, this);
        }

        /// <summary>
        /// Method for fill article details of from location. 
        /// [001][vsana][22-11-2020][GEOS2-2426]AutoSort for the new locations created 
        /// </summary>
        /// <param name="fromLocationName"></param>
        private void FillDetailsFromLocation(string fromLocationName)
        {
            GeosApplication.Instance.Logger.Log("Method FillDetailsFromLocation....", category: Category.Info, priority: Priority.Low);

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

            try
            {
                if (WarehouseCommon.Instance.Selectedwarehouse != null)
                {
                    //  Int64 _idwarehouse = ((Warehouses)WarehouseCommon.Instance.SelectedwarehouseList[0]).IdWarehouse;

                    // Int64 _idwarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                    //[001] Changed service method GetMaterialDetailsByLocationName_V2034 to GetMaterialDetailsByLocationName_V2080
                    //MainTransferMaterialsList = WarehouseService.GetMaterialDetailsByLocationName_V2080(fromLocationName, WarehouseCommon.Instance.Selectedwarehouse);

                    MainTransferMaterialsList = WarehouseService.GetMaterialDetailsByLocationName_V2150(fromLocationName, WarehouseCommon.Instance.Selectedwarehouse);
                    if (MainTransferMaterialsList != null && MainTransferMaterialsList.Count > 0)
                    {
                        FromIdWarehouseLocation = MainTransferMaterialsList[0].IdWarehouseLocation;
                        TransferMaterialsList = new List<TransferMaterials>();

                        // clone main list to binding list. 
                        if (MainTransferMaterialsList != null)
                            MainTransferMaterialsList.ForEach((item) =>
                            {
                                TransferMaterialsList.Add((TransferMaterials)item.Clone());
                                // List<SerialNumber> ScanSerialNumbers = new List<SerialNumber>();
                                // foreach(var temp in TransferMaterialsList)
                                // {
                                //     //ScanSerialNumbers.AddRange(temp.SerialNumbers);
                                //     SerialNumber ObjSerial = new SerialNumber();
                                //     ObjSerial.Code = "123".ToString();
                                //     ObjSerial.IdWarehouse = 18;
                                //     temp.ScanSerialNumbers = new List<SerialNumber>();
                                //     temp.ScanSerialNumbers.Add(ObjSerial);
                                // }
                                //// ScanSerialNumbers.Add(TransferMaterialsList.)
                            });

                        TransferMaterialsList = new List<TransferMaterials>(TransferMaterialsList);

                        List<Int64> IdArticles = new List<Int64>();
                        foreach (var item in TransferMaterialsList)
                        {
                            if (!IdArticles.Contains(item.IdArticle))
                            {
                                IdArticles.Add(item.IdArticle);
                            }
                        }

                        IdArticles.Sort();
                        string IdArticlesStr = string.Join(",", IdArticles.Select(n => n.ToString()).ToArray());
                        // fill all available location of articles 
                        ArticleWarehouseLocationsList = WarehouseService.GetArticlesWarehouseLocationTransfer_V2033(IdArticlesStr, WarehouseCommon.Instance.Selectedwarehouse).Where(al => !al.FullName.Equals(fromLocationName)).OrderBy(aw => aw.FullName).ToList();
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method FillDetailsFromLocation....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDetailsFromLocation() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDetailsFromLocation() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDetailsFromLocation...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        ArticleWarehouseLocations selectedLocation;
        List<TransferMaterials> commonTransferMaterialsList;
        List<TransferMaterials> notCommonTransferMaterialsList;
        private WarehouseLocation toWarehouseLocation;
        private string description;


        /// <summary>
        /// Method for do scan opration on scanned barcode.
        /// <para>[001][skale][2019-09-04][GEOS2-78] Item scan with 0 qty in transfer?</para>
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            try
            {
                if (obj.Text == "\r" && !isLocationLast)
                {
                    GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);

                    #region 1ST CONDITION
                    if (!IsFromLocationScaned)
                    {
                        FromLocationName = BarcodeStr;
                        FillDetailsFromLocation(FromLocationName);//fill article details as per location.

                        if (MainTransferMaterialsList != null && MainTransferMaterialsList.Count > 0)
                        {
                            BarcodeStr = "";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                            WrongFromLocation = string.Empty;
                            IsWrongFromLocationErrorVisible = Visibility.Hidden;

                            //BgColorFromLocation = "#FF008000";
                            // BgColorToLocation = "#FFF4E27E";
                            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                            {
                                BgColorFromLocation = "#FF008000";
                                BgColorToLocation = "#FF083493";
                                FgColorLocation = "White";
                                TrackBarFgColor = "#FF083493";
                            }
                            else
                            {
                                BgColorFromLocation = "#FF008000";
                                BgColorToLocation = "#FF2AB7FF";
                                FgColorLocation = "Black";
                                TrackBarFgColor = "#FF2AB7FF";
                            }

                            IsFromLocationIndicatorVisible = Visibility.Hidden;
                            IsToLocationIndicatorVisible = Visibility.Visible;
                            IsFromLocationScaned = true;
                            TrackBarEditvalue = 1;
                            MapItems = new ObservableCollection<WarehouseLocation>();
                            FillMapLocation();
                            IsInformationVisible = Visibility.Collapsed;
                        }
                        else
                        {
                            WrongFromLocation = string.Format("Wrong Location {0}", BarcodeStr);
                            IsWrongFromLocationErrorVisible = Visibility.Visible;
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            BarcodeStr = "";
                            FromLocationName = "";
                            BgColorFromLocation = "#FFFF0000";
                            IsFromLocationScaned = false;
                        }

                    }
                    #endregion

                    #region 2ND CONDITION

                    else if (!IsToLocationScaned)
                    {
                        ToLocationName = BarcodeStr;
                        bool transitLocationExistsInWarehouseAndAllowedInTransferToLocation = false;
                        if (ToLocationName.Equals("TRANSIT", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var transitLocationDetails = ArticleWarehouseLocationsList.FirstOrDefault(lc => lc.FullName.Equals(ToLocationName, StringComparison.InvariantCultureIgnoreCase));
                            transitLocationExistsInWarehouseAndAllowedInTransferToLocation = (transitLocationDetails != null);
                        }

                        //Service updated from GetWarehouseLocationsByIdWarehouse_V2034 to  GetWarehouseLocationsByIdWarehouse_Transfer by [rdixit][GEOS2-4132][08.02.2023]
                        WarehouseAllLocationList = new ObservableCollection<WarehouseLocation>(WarehouseService.GetWarehouseLocationsByIdWarehouse_Transfer(WarehouseCommon.Instance.Selectedwarehouse));

                        if (WarehouseAllLocationList.Any(y => y.FullName.Equals(ToLocationName, StringComparison.InvariantCultureIgnoreCase))
                            || transitLocationExistsInWarehouseAndAllowedInTransferToLocation)
                        {
                            BarcodeStr = "";
                            WrongToLocation = string.Empty;
                            IsWrongToLocationErrorVisible = Visibility.Hidden;
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);

                            ToWarehouseLocation = WarehouseAllLocationList.Where(x => x.FullName == ToLocationName).FirstOrDefault();

                            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                            {
                                //BgColorFromLocation = "#FF008000";
                                BgColorToLocation = "#FF008000";
                                FgColorLocation = "White";
                                TrackBarFgColor = "#FF083493";
                            }
                            else
                            {
                                //BgColorFromLocation = "#FF008000";
                                BgColorToLocation = "#FF008000";
                                FgColorLocation = "Black";
                                TrackBarFgColor = "#FF2AB7FF";
                            }
                            IsToLocationScaned = true;
                            TrackBarEditvalue = 2;
                        }
                        else
                        {
                            WrongToLocation = string.Format("Wrong Location {0}", BarcodeStr);
                            IsWrongToLocationErrorVisible = Visibility.Visible;
                            BarcodeStr = "";
                            ToLocationName = "";
                            BgColorToLocation = "#FFFF0000";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            IsToLocationScaned = false;
                            TrackBarEditvalue = 2;
                        }



                        //if (ToLocationName.Equals("TRANSIT", StringComparison.InvariantCultureIgnoreCase))
                        //    selectedLocation = ArticleWarehouseLocationsList.Where(lc => lc.FullName.Equals(ToLocationName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        //else
                        //    selectedLocation = ArticleWarehouseLocationsList.Where(lc => lc.FullName == ToLocationName).FirstOrDefault();

                        //if (selectedLocation != null)
                        //{
                        //    WrongToLocation = string.Empty;
                        //    IsWrongToLocationErrorVisible = Visibility.Hidden;
                        //    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                        //    //Short list articles as per destination.if article are not belong to location then that articles will be removed. 

                        //    if (!ToLocationName.Equals("TRANSIT", StringComparison.InvariantCultureIgnoreCase))
                        //    {
                        //        List<Int32> locatiolist = ArticleWarehouseLocationsList.Where(tr => tr.IdWarehouseLocation == selectedLocation.IdWarehouseLocation).Select
                        //                 (e => e.IdArticle).ToList();
                        //        TransferMaterialsList = MainTransferMaterialsList.Where(io => locatiolist.Contains(io.IdArticle)).ToList();

                        //        commonTransferMaterialsList = MainTransferMaterialsList.Where(io => locatiolist.Contains(io.IdArticle)).ToList();
                        //        notCommonTransferMaterialsList = MainTransferMaterialsList.Where(io => !locatiolist.Contains(io.IdArticle)).ToList();
                        //    }

                        //    BarcodeStr = "";
                        //    ScanAction(0, 0, selectedLocation.IdWarehouseLocation);
                        //    //BgColorToLocation = "#FF008000";
                        //    if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                        //    {
                        //        //BgColorFromLocation = "#FF008000";
                        //        BgColorToLocation = "#FF008000";
                        //        FgColorLocation = "White";
                        //        TrackBarFgColor = "#FF083493";
                        //    }
                        //    else
                        //    {
                        //        //BgColorFromLocation = "#FF008000";
                        //        BgColorToLocation = "#FF008000";
                        //        FgColorLocation = "Black";
                        //        TrackBarFgColor = "#FF2AB7FF";
                        //    }
                        //    IsToLocationScaned = true;
                        //    TrackBarEditvalue = 2;
                        //}
                        //else
                        //{
                        //    //string _tempToLocation = BarcodeStr;
                        //    WrongToLocation = string.Format("Wrong Location {0}", BarcodeStr);
                        //    IsWrongToLocationErrorVisible = Visibility.Visible;
                        //    BarcodeStr = "";
                        //    ToLocationName = "";
                        //    BgColorToLocation = "#FFFF0000";
                        //    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        //    IsToLocationScaned = false;
                        //    //IsToLocationScaned = true;
                        //    //BarcodeStr = "";
                        //}
                    }
                    #endregion

                    #region 3RD CONDITION

                    else if (IsFromLocationScaned && IsToLocationScaned)
                    {

                        #region SCAN Serial Number "IF CONDITION" IS HERE

                        if (IsScanSerialNumber)
                        {
                            IsWrongItemErrorVisible = Visibility.Hidden;
                            WrongItem = string.Empty;

                            TransferMaterials tm = SelectedSerialNumber.MasterItem as TransferMaterials;

                            //Observation comment for serial number.
                            if (string.IsNullOrEmpty(materialSerialNumbers))
                                materialSerialNumbers = String.Format(Application.Current.FindResource("MaterialSerialNumbers").ToString() + "\r\n", tm.Reference);

                            if (tm.ScanSerialNumbers.Exists(x => x.Code == BarcodeStr))     //Show error if already scanned item.
                            {
                                IsWrongItemErrorVisible = Visibility.Visible;
                                WrongItem = "Wrong Serial Number " + BarcodeStr;
                                BarcodeStr = string.Empty;
                                RowBgColor = "#FFFF0000";       // color = cream Red
                                OnFocusBgColor = "#FFFF0000";   // color = cream Red
                                OnFocusFgColor = "#FFFFFFFF";   // color = cream White
                                                                //SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            }
                            else
                            {
                                if (tm.SerialNumbers.Exists(x => x.Code == BarcodeStr) && SelectedSerialNumber.Code == null)     //Add as scanned in serial numbers if true
                                {
                                    SerialNumber sn = (SerialNumber)tm.SerialNumbers.FirstOrDefault(x => x.Code == BarcodeStr).Clone();
                                 //  GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                    SelectedSerialNumber.Code = BarcodeStr;
                                    //assign all values to scanned serial number.
                                    SelectedSerialNumber.IdSerialNumber = sn.IdSerialNumber;
                                    SelectedSerialNumber.IdArticle = sn.IdArticle;
                                    SelectedSerialNumber.IdWarehouseDeliveryNoteItem = sn.IdWarehouseDeliveryNoteItem;
                                    //SelectedSerialNumber.IdWarehouseLocation = WarehouseAllLocationList.Where(x => x.FullName == ToLocationName.ToUpper()).Select(x => x.IdWarehouseLocation).FirstOrDefault();
                                    //if (SelectedSerialNumber.IdWarehouseLocation.HasValue)
                                    //    tm.IdWarehouseLocation = SelectedSerialNumber.IdWarehouseLocation.Value;
                                    SelectedSerialNumber.IdWarehouseLocation = sn.IdWarehouseLocation;
                                    SelectedSerialNumber.IdWarehouse = sn.IdWarehouse;
                                    SelectedSerialNumber.IsScanned = true;
                                    //tm.ScanSerialNumbers.Add(tm.ScanSerialNumbers.Where(x => x.Code = BarcodeStr))
                                    tm.ScannedQty = 1;
                                    BarcodeScannedQuantity -= 1;
                                    ScanAction(sn.IdWarehouseDeliveryNoteItem, tm.ScannedQty, selectedLocation.IdWarehouseLocation);
                                  //  GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                    BarcodeStr = "";
                                    if (BarcodeScannedQuantity > 0)
                                    {
                                        SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = tm };
                                        tm.ScanSerialNumbers.Add(scanSerialNumber);
                                        SelectedSerialNumber = scanSerialNumber;
                                        IsScanSerialNumber = true;
                                        BarcodeStr = string.Empty;
                                    }
                                    else if (BarcodeScannedQuantity == 0)
                                    {
                                        IsScanSerialNumber = false;
                                    }
                                }
                                else
                                {
                                    //Show error message if scanned item is wrong.
                                    IsWrongItemErrorVisible = Visibility.Visible;
                                    WrongItem = "Wrong Serial Number " + BarcodeStr;
                                    BarcodeStr = string.Empty;
                                    OnFocusBgColor = "#FFFF0000";   // color = cream Red
                                    OnFocusFgColor = "#FFFFFFFF";   // color = cream White
                                                                    //PendingStorageArticlesList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);

                                }
                             
                            }

                            BarcodeStr = string.Empty;
                        }
                        #endregion

                        else
                        {
                            IsWrongItemErrorVisible = Visibility.Hidden;
                            WrongItem = string.Empty;
                            var isNumeric = !string.IsNullOrEmpty(BarcodeStr) && BarcodeStr.All(Char.IsDigit);
                            if (isNumeric && BarcodeStr.Length >= 17)
                            {
                                string _idwarehouseStr = BarcodeStr.Substring(0, 3);
                                string _idWareHouseDeliveryNoteItemStr = BarcodeStr.Substring(3, 8);
                                string _itemQuantityStr = BarcodeStr.Substring(11, 6);

                                Int64 _idwarehouse = Convert.ToInt64(_idwarehouseStr);
                                Int64 _idWareHouseDeliveryNoteItem = Convert.ToInt64(_idWareHouseDeliveryNoteItemStr);
                                Int64 _itemQuantity = Convert.ToInt64(_itemQuantityStr);
                                BarcodeScannedQuantity = Convert.ToInt64(BarcodeStr.Substring(11, 6));
                                //ArticleWarehouseLocationsList.AddRange(ToWarehouseLocation);
                                //WarehouseService.AddArticleWarehouseLocationByFullName(ToWarehouseLocation);

                                // selectedLocation = WarehouseService.GetArticleWarehouseLocation(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, );
                                //WarehouseService.GetArticleDetailByIdWarehouseDeliveryNoteItem_V2150()
                                BarcodeStr = "";
                                WrongToLocation = string.Empty;
                                IsWrongToLocationErrorVisible = Visibility.Hidden;
                                if (ToLocationName.Equals("TRANSIT", StringComparison.InvariantCultureIgnoreCase))
                                    selectedLocation = ArticleWarehouseLocationsList.Where(lc => lc.FullName.Equals(ToLocationName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                else
                                    selectedLocation = ArticleWarehouseLocationsList.Where(lc => lc.FullName.Equals(ToLocationName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                                if (selectedLocation != null)
                                {
                                    if (!ToLocationName.Equals("TRANSIT", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        List<Int32> locatiolist = ArticleWarehouseLocationsList.Where(tr => tr.IdWarehouseLocation == selectedLocation.IdWarehouseLocation).Select
                                                 (e => e.IdArticle).ToList();
                                        //TransferMaterialsList = MainTransferMaterialsList.Where(io => locatiolist.Contains(io.IdArticle)).ToList();
                                        TransferMaterialsList = MainTransferMaterialsList;
                                        commonTransferMaterialsList = MainTransferMaterialsList.Where(io => locatiolist.Contains(io.IdArticle)).ToList();
                                        notCommonTransferMaterialsList = MainTransferMaterialsList.Where(io => !locatiolist.Contains(io.IdArticle)).ToList();
                                    }

                                    ScanAction(0, 0, selectedLocation.IdWarehouseLocation);
                                }
                                else
                                {
                                    //ADD LOCATION
                                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["TransferMaterialAddNewLocationWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                    if (MessageBoxResult == MessageBoxResult.OK)
                                    {
                                        WarehouseLocation toWarehouseLocation = WarehouseService.GetWarehouseLocationByFullName(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, ToLocationName);

                                        ArticleWarehouseLocations ObjArticleWarehouseLocations = new ArticleWarehouseLocations()
                                        {
                                            FullName = ToLocationName,
                                            IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse,
                                            TransactionOperation = ModelBase.TransactionOperations.Add,
                                            Name = ToLocationName.Substring(ToLocationName.Length - 3),
                                            IdArticle = MainTransferMaterialsList.Where(x => x.IdWarehouse == WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse &&
                                                                                      x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem).Select(y => y.IdArticle).FirstOrDefault(),

                                            //IdArticle = TransferMaterialsList.Where(x => x.IdWarehouse == WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse &&
                                            //x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem).Select(y => y.IdArticle).FirstOrDefault(),
                                            //TransferMaterialsList.Where(x => x.IdWarehouse == WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse).Select(y => y.IdArticle).FirstOrDefault(),
                                            //IdWarehouseLocation = TransferMaterialsList.Where(x => x.IdWarehouse == WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse).Select(y => y.IdWarehouseLocation).FirstOrDefault(),
                                            IdWarehouseLocation = toWarehouseLocation.IdWarehouseLocation,
                                            Position = 0,
                                            MinimumStock = 0,
                                            // WarehouseLocation = new WarehouseLocation { FullName = ToLocationName, IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse },
                                            WarehouseLocation = toWarehouseLocation, //new WarehouseLocation { FullName = ToLocationName, IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse },
                                        };

                                        //selectedLocation = ObjArticleWarehouseLocations;
                                        //Service Method is changed from AddArticleWarehouseLocationByFullName to AddArticleWarehouseLocationByFullName_V2360
                                        ArticleWarehouseLocations ObjArticleWarehouseLocations1 = WarehouseService.AddArticleWarehouseLocationByFullName_V2360(ObjArticleWarehouseLocations);
                                        FillDetailsFromLocation(FromLocationName);
                                        selectedLocation = ArticleWarehouseLocationsList.Where(lc => lc.FullName.Equals(ToLocationName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                                        ScanAction(0, 0, selectedLocation.IdWarehouseLocation);

                                        Notification notification = new Notification();
                                        notification.IsNew = 1;
                                        notification.IdModule = 6;
                                        notification.Status = "Unread";
                                        notification.Title = "A new location has been added.";
                                        notification.FromUser = GeosApplication.Instance.ActiveUser.IdUser;
                                        notification.Message = "A new location " + ToLocationName + " has been added in the article " + MainTransferMaterialsList.Where(y => y.IdArticle == selectedLocation.IdArticle).Select(x => x.Reference).ToList().FirstOrDefault()
                                                            + " during transfer from location " + FromLocationName;  //+MainTransferMaterialsList.Where(io => locatiolist.Contains(io.IdArticle)).Select(x => x.Reference).ToList().FirstOrDefault()
                                        WarehouseService.AddNotification(notification);


                                        //foreach (TransferMaterials obj1 in MainTransferMaterialsList.Where(x => x.LocationFullName == FromLocationName).ToList())
                                        //{

                                        //    ArticleWarehouseLocations objArticleWarehouseLocations = new ArticleWarehouseLocations()
                                        //    {
                                        //        FullName = ToLocationName,
                                        //        IdWarehouse = obj1.IdWarehouse,
                                        //        TransactionOperation = ModelBase.TransactionOperations.Add,
                                        //        Name = ToLocationName.Substring(ToLocationName.Length - 3),

                                        //        IdArticle = obj1.IdArticle,

                                        //    };
                                        //}
                                    }
                                }
                                IsScanSerialNumber = false;
                                tempItem = TransferMaterialsList.Where(tm => tm.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem).FirstOrDefault();

                                if (tempItem != null)
                                {
                                    //This is used to check for registerserialnumber.
                                    SelectedTransferMaterials = tempItem;
                                    if (SelectedTransferMaterials.RegisterSerialNumber == 1)
                                    {
                                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                        if (SelectedTransferMaterials.ScanSerialNumbers == null)
                                            SelectedTransferMaterials.ScanSerialNumbers = new List<SerialNumber>();
                                        //_itemQuantity = SelectedRefillMaterials.Quantity;//All SerialNumbers can be Downloaded ntg to do with scanned number 
                                        SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = SelectedTransferMaterials };
                                        // scanSerialNumber = SelectedTransferMaterials.SerialNumbers
                                        SelectedTransferMaterials.ScanSerialNumbers.Add(scanSerialNumber);
                                        SelectedSerialNumber = scanSerialNumber;
                                        IsScanSerialNumber = true;
                                        BarcodeStr = "";
                                        return;
                                    }
                                }
                                if (!IsScanSerialNumber)
                                {
                                    ScanAction(_idWareHouseDeliveryNoteItem, _itemQuantity, selectedLocation.IdWarehouseLocation);
                                    BarcodeStr = "";
                                }                          
                            }
                            else
                            {
                                WrongItem = string.Format("Wrong Item {0}", BarcodeStr);
                                IsWrongItemErrorVisible = Visibility.Visible;
                                BarcodeStr = "";
                                RowBgColor = "#FFFF0000";
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                OnFocusBgColor = "#FFFF0000";
                                OnFocusFgColor = "#FFFFFFFF";
                            }
                        }
                    }
                    #endregion

                }
                else
                {
                    //BarcodeStr = BarcodeStr + obj.Text;
                    //[pramod.misal][GEOS2-5067][08-01-2023]
                    BarcodeStr = BarcodeStr + obj.Text.ToUpper();
                }
            }
            catch (Exception ex)
            {
                BarcodeStr = "";
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for scan action.
        /// </summary>
        /// <param name="idwarehouseDeliveryNote"></param>
        /// <param name="quantity"></param>
        private void ScanAction(Int64 idWareHouseDeliveryNoteItem, Int64 quantity, Int64 IdwarehouseLocarion)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);

                if (countLocation == 0)
                {
                    SelectedTransferMaterials = TransferMaterialsList.FirstOrDefault();
                    //ArticleImage = ByteArrayToImage(SelectedTransferMaterials.ArticleVisualAidsPath, SelectedTransferMaterials.ImagePath);
                    ArticleImage = ByteArrayToBitmapImage(SelectedTransferMaterials.ArticleImageInBytes);
                    Description = SelectedTransferMaterials.Description;
                    TotalCurrentItemStock = SelectedTransferMaterials.CurrentStock;
                    changeTotalStockColor(TotalCurrentItemStock, SelectedTransferMaterials.MinimumStock);
                    UpdateProgressbarArticleStock();
                    UpdateProgressbarLocationStock();
                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, selectedLocation.MinimumStock);
                    //OnFocusBgColor = "#FFF4E27E";
                    //OnFocusFgColor = "#FF000000";
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

                    IsFromLocationIndicatorVisible = Visibility.Hidden;
                    IsToLocationIndicatorVisible = Visibility.Hidden;
                    IsInformationVisible = Visibility.Visible;
                    countLocation++;
                   
                }
                else if (IsToLocationScaned)
                {
                    tempItem = TransferMaterialsList.Where(tm => tm.IdWareHouseDeliveryNoteItem == idWareHouseDeliveryNoteItem).FirstOrDefault();

                    if (tempItem != null)
                    {
                        //This is used to avoid minus stock for warehousedeliverynoteitem.
                        SelectedTransferMaterials = tempItem;
                        Int64 locationAvaibleQuantity = WarehouseService.GetStockForScanItem(tempItem.IdArticle, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, idWareHouseDeliveryNoteItem, FromIdWarehouseLocation);

                        if (locationAvaibleQuantity < quantity || quantity == 0)
                        {

                            IsWrongItemErrorVisible = Visibility.Visible;
                            WrongItem = String.Format("No location stock available {0}", BarcodeStr);
                            BarcodeStr = "";
                            RowBgColor = "#FFFF0000";
                            OnFocusBgColor = "#FFFF0000";
                            OnFocusFgColor = "#FFFFFFFF";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            ArticleImage = ByteArrayToBitmapImage(SelectedTransferMaterials.ArticleImageInBytes);
                            TotalCurrentItemStock = SelectedTransferMaterials.CurrentStock;
                            changeTotalStockColor(TotalCurrentItemStock, SelectedTransferMaterials.MinimumStock);
                            UpdateProgressbarArticleStock();
                            UpdateProgressbarLocationStock();
                            ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, selectedLocation.MinimumStock);
                            UpdateProgressbarLocationStock();
                            return;
                        }

                        WrongItem = string.Empty;
                        IsWrongItemErrorVisible = Visibility.Hidden;
                         GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);

                        //ArticleImage = ByteArrayToImage(SelectedTransferMaterials.ArticleVisualAidsPath, SelectedTransferMaterials.ImagePath);
                        ArticleImage = ByteArrayToBitmapImage(SelectedTransferMaterials.ArticleImageInBytes);


                        TotalCurrentItemStock = SelectedTransferMaterials.CurrentStock;
                        changeTotalStockColor(TotalCurrentItemStock, SelectedTransferMaterials.MinimumStock);
                        UpdateProgressbarArticleStock();
                        UpdateProgressbarLocationStock();
                        ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, selectedLocation.MinimumStock);

                        //if (SelectedTransferMaterials.RegisterSerialNumber == 1)
                        //{
                        //    if (SelectedTransferMaterials.ScanSerialNumbers == null)
                        //        SelectedTransferMaterials.ScanSerialNumbers = new List<SerialNumber>();
                        //    //_itemQuantity = SelectedTransferMaterials.Quantity;//All SerialNumbers can be Downloaded ntg to do with scanned number 
                        //    SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = SelectedTransferMaterials };
                        //    // scanSerialNumber = SelectedTransferMaterials.SerialNumbers
                        //    SelectedTransferMaterials.ScanSerialNumbers.Add(scanSerialNumber);
                        //    SelectedSerialNumber = scanSerialNumber;
                        //    IsScanSerialNumber = true;
                        //    tempItem.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        //    tempItem.Comments = "Transfer from " + "\"" + FromLocationName + "\"" + " location to " + "\"" + ToLocationName + "\"";
                        //}


                        if (SelectedTransferMaterials != null && tempItem.Quantity >= quantity)
                        {
                            tempItem.Quantity -= quantity;
                            tempItem.ScannedQty = quantity;
                            tempItem.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            tempItem.Comments = "Transfer from " + "\"" + FromLocationName + "\"" + " location to " + "\"" + ToLocationName + "\"";

                            TransferMaterials psa = (TransferMaterials)tempItem.Clone();
                            psa.IdWarehouseLocation = FromIdWarehouseLocation;
                            psa.ScannedQty = (-1) * psa.ScannedQty;

                            //Change destination warehous location id before insert.
                            tempItem.IdWarehouseLocation = IdwarehouseLocarion;

                            //bool IsdiductFromtransit = WarehouseService.InsertIntoArticleStockForTransferMaterial(psa);
                            //bool IsInsertArticletoLocation = WarehouseService.InsertIntoArticleStockForTransferMaterial(tempItem);

                            
                                bool IsdiductFromtransit = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(psa);
                                bool IsInsertArticletoLocation = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(tempItem);

                                //Updateing the progress bar of location stock 
                                ProgressbarLocationStock.LocationStock += quantity;
                                LocationStockValue = ProgressbarLocationStock.LocationStock;
                            
                            if (ProgressbarLocationStock.MaximumStock == 0)
                                LocationStockValue = 0;
                            else if (ProgressbarLocationStock.MaximumStock < LocationStockValue)
                                LocationStockValue = ProgressbarLocationStock.MaximumStock;

                            //Updating the lable Clr based on LocationStock
                            ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, selectedLocation.MinimumStock);
                            UpdateProgressbarArticleStock();
                            UpdateProgressbarLocationStock();

                            // code is for to keep current from location till all value transfer to other location.
                            long _fromLocationWarehouseId = MainTransferMaterialsList[0].IdWarehouseLocation;

                            if (tempItem.Quantity > 0)
                                tempItem.IdWarehouseLocation = _fromLocationWarehouseId;

                          

                            //Remove scanned article from list if quantity is zero.
                            if (tempItem.Quantity == 0)
                            {
                                int index = TransferMaterialsList.FindIndex(tr => tr.IdWareHouseDeliveryNoteItem == tempItem.IdWareHouseDeliveryNoteItem);
                                TransferMaterialsList.Remove(tempItem);

                                if (TransferMaterialsList.Count == 1)
                                {
                                    SelectedTransferMaterials = TransferMaterialsList[0];
                                    //ArticleImage = ByteArrayToImage(SelectedTransferMaterials.ArticleVisualAidsPath, SelectedTransferMaterials.ImagePath);
                                    ArticleImage = ByteArrayToBitmapImage(SelectedTransferMaterials.ArticleImageInBytes);
                                    TotalCurrentItemStock = SelectedTransferMaterials.CurrentStock;
                                    changeTotalStockColor(TotalCurrentItemStock, SelectedTransferMaterials.MinimumStock);
                                    UpdateProgressbarArticleStock();
                                    UpdateProgressbarLocationStock();
                                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, selectedLocation.MinimumStock);
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
                                else if (TransferMaterialsList.Count > 0)
                                {
                                    if (TransferMaterialsList.Count - 1 > index)
                                        SelectedTransferMaterials = TransferMaterialsList[index];
                                    else
                                        SelectedTransferMaterials = TransferMaterialsList[index - 1];

                                    //ArticleImage = ByteArrayToImage(SelectedTransferMaterials.ArticleVisualAidsPath, SelectedTransferMaterials.ImagePath);
                                    ArticleImage = ByteArrayToBitmapImage(SelectedTransferMaterials.ArticleImageInBytes);
                                    TotalCurrentItemStock = SelectedTransferMaterials.CurrentStock;
                                    UpdateProgressbarArticleStock();
                                    UpdateProgressbarLocationStock();
                                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, selectedLocation.MinimumStock);
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
                                else if (TransferMaterialsList.Count == 0)
                                {
                                    SelectedTransferMaterials = null;
                                    ArticleImage = null;
                                    TotalCurrentItemStock = 0;


                                    //BgColorFromLocation = "#FFF4E27E";
                                    //BgColorToLocation = "#FFFFFFFF";
                                    if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                                    {
                                        BgColorFromLocation = "#FF083493";

                                        BgColorToLocation = "White";
                                        FgColorLocation = "White";
                                        TrackBarFgColor = "#FF083493";
                                    }
                                    else
                                    {
                                        BgColorFromLocation = "#FF2AB7FF";
                                        BgColorToLocation = "#Black";
                                        FgColorLocation = "Black";
                                        TrackBarFgColor = "#FF2AB7FF";
                                    }

                                    IsFromLocationScaned = false;
                                    IsToLocationScaned = false;
                                    BarcodeStr = "";
                                    FromLocationName = "";
                                    ToLocationName = "";

                                    IsFromLocationIndicatorVisible = Visibility.Visible;
                                    IsToLocationIndicatorVisible = Visibility.Hidden;
                                    IsInformationVisible = Visibility.Collapsed;
                                    MapItems.Clear();
                                    countLocation = 0;
                                    TrackBarEditvalue = 0;
                                }

                                TransferMaterialsList = new List<TransferMaterials>(TransferMaterialsList);
                            }
                            else
                            {
                                OnFocusBgColor = "#FFF4E27E";
                                OnFocusFgColor = "#FF000000";
                            }
                        }
                        else
                        {
                            OnFocusBgColor = "#FFFF0000";
                            OnFocusFgColor = "#FFFFFFFF";
                            WrongItem = string.Format("Wrong Item {0}", BarcodeStr);
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            IsWrongItemErrorVisible = Visibility.Visible;
                        }

                    }
                    else
                    {
                        OnFocusBgColor = "#FFFF0000";
                        OnFocusFgColor = "#FFFFFFFF";
                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        WrongItem = string.Format("Wrong Item {0}", BarcodeStr);
                        IsWrongItemErrorVisible = Visibility.Visible;
                        SelectedTransferMaterials = null;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ScanAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
            else if (totalStock > minQuantity)
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

        /// <summary>
        /// Method for scan next location items.
        /// </summary>
        /// <param name="obj"></param>
        //private void NextButtonCommandAction(object obj)
        //{
        //    locationIndex++;
        //    if (ArticleLocationList.Count > locationIndex)
        //    {
        //        TotalCurrentItemStock = 0;


        //        IsFromLocationIndicatorVisible = Visibility.Visible;
        //        IsInformationVisible = Visibility.Hidden;
        //        IsLocationScaned = false;

        //        bgColorFromLocation = "#FFF4E27E";
        //        RowBgColor = "#FFF4E27E";

        //        TrackBarEditvalue = 0;
        //        if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
        //            TrackBarFgColor = "#FF083493";
        //        else
        //            TrackBarFgColor = "#FF2AB7FF";

        //        Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocateMaterialsUserControlView", null, this);

        //        if (ArticleLocationList.Count - 1 == locationIndex)
        //            IsNextButtonEnable = false;
        //    }
        //    else
        //    {
        //        IsNextButtonEnable = false;
        //        isLocationLast = true;
        //    }
        //}


        /// <summary>
        /// to update Progressbar of LocationStock
        /// </summary>
        private void UpdateProgressbarLocationStock()
        {
            GeosApplication.Instance.Logger.Log("Method UpdateProgressbarLocationStock....", category: Category.Info, priority: Priority.Low);
            try
            {
                ProgressbarLocationStock = WarehouseService.GetArticleStockByWarehouseLocation(SelectedTransferMaterials.IdArticle, selectedLocation.IdWarehouseLocation, SelectedTransferMaterials.IdWarehouse);
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
                ProgressbarArticleStock = WarehouseService.GetAVGStockByIdArticle_V2034(SelectedTransferMaterials.IdArticle, WarehouseCommon.Instance.Selectedwarehouse);

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

        public byte[] GetArticleImageInBytes(string ArticleVisualAidsPath, string ImagePath)
        {
            if (!Directory.Exists(ArticleVisualAidsPath))
            {
                return null;
            }

            string fileUploadPath = ArticleVisualAidsPath + ImagePath;

            if (!File.Exists(fileUploadPath))
            {
                return null;
            }

            if (!string.IsNullOrEmpty(ImagePath))
            {
                byte[] bytes = null;

                try
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }

                    return bytes;
                }

                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method GetArticleImageInBytes...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }

            return null;
        }


        private void CommandCancelAction(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Method to convert Byte Array To Bitmap Image
        /// WMS M057-10	After scan location in transfer article then article image not display-----sdesai
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();

                using (var mem = new MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
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

                TransferMaterials article = (TransferMaterials)obj;
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
                    int index = TransferMaterialsList.IndexOf(article);
                    if (articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage || articleDetailsViewModel.UpdateArticle.IsAddedArticleImage)
                    {
                        foreach (var item in MainTransferMaterialsList)
                        {
                            if (item.IdArticle == article.IdArticle)
                                item.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        }

                        if (SelectedTransferMaterials != null)
                        {
                            if (article.IdArticle == SelectedTransferMaterials.IdArticle)
                                ArticleImage = ByteArrayToBitmapImage(articleDetailsViewModel.UpdateArticle.ArticleImageInBytes);
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


        /// <summary>
        /// Fill map location and file.
        /// </summary>
        private void FillMapLocation()
        {
            MapItems.Clear();
            SvgUri = null;
            WarehouseLocation wlocation = WarehouseService.GetWarehouseLocationByFullName(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, FromLocationName);

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
        #endregion
    }
}
