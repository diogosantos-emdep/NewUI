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
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class LocationsRefillViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region TaskLog

        /// <summary>
        /// WMS	M054-03	New wizzard Refill similar like transfer [adaibathina]
        /// WMS	M055-06	Add OK and NOK beeps in picking storage and transfer [adadibathina]
        /// WMS	M057-11	Sort Refill by from location [adadibathina]
        /// </summary>

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

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }

        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion //End Of Services

        #region Declaration

        public List<Refill> RefillList;
        private List<LocationRefill> tempLocationsToRefillList;
        private LocationRefill locationFromRefill;
        private LocationRefill locationToRefill;
        private bool isFromLocationScaned;
        private bool isToLocationScaned;
        private string barcodeStr;
        private int trackBarEditvalue;
        private string onFocusBgColor;
        private string onFocusFgColor;
        private string rowBgColor;
        private TransferMaterials selectedRefillMaterials;
        private ImageSource articleImage;
        private int countLocation = 0;
        private Int64 totalCurrentItemStock;
        private string totalStockBgColor;
        private string totalStockFgColor;
        private TransferMaterials tempItem;
        private string bgColorLocation;
        private string bgColorFromLocation;
        private string bgColorToLocation;
        private string fgToColorLocation;
        private string fgFromColorLocation;
        private string trackBarFgColor;
        //bool isLocationLast;
        private string fromLocationName;
        private string toLocationName;
        private List<TransferMaterials> mainRefillMaterialsList;
        private Visibility isFromLocationIndicatorVisible;
        private Visibility isToLocationIndicatorVisible;
        private Visibility isInformationVisible;
        int locationIndex = 0;
        //private List<string> articleLocationList;
        private bool isLocationScaned;
        bool isSkipButtonEnable;
        private int windowWidth;
        private int windowHeight;
        private string wrongFromLocation;
        private Visibility isWrongFromLocationErrorVisible = Visibility.Hidden;

        private string wrongItem;
        private Visibility isWrongItemErrorVisible = Visibility.Hidden;

        private string wrongToLocation;
        private Visibility isWrongToLocationErrorVisible = Visibility.Hidden;
        private ObservableCollection<WarehouseLocation> mapItems;
        private Uri svgUri;

        private string locationStockBgColor;
        private string locationStockFgColor;
        private ArticleWarehouseLocations progressbarArticleStock;
        private ArticleWarehouseLocations progressbarLocationStock;
        private long locationStockValue;
        private bool focusUserControl;
        private bool isBackButtonEnable;
        private TransferMaterials currentTransferMaterial;

        private ImageSource imageSharedLocationReferences;
        private Visibility isSharedLocationReferencesVisible;
        private List<Article> sharedLocationReferences;
        private SerialNumber selectedSerialNumber;
        private bool isScanSerialNumber;
        private string materialSerialNumbers;
        private long barcodeScannedQuantity;

        #endregion

        #region Properties
        public bool FocusUserControl
        {
            get { return focusUserControl; }
            set
            {
                focusUserControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FocusUserControl"));
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

        //Thread FromRefillThread { get; set; }

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

        public bool IsLocationScaned
        {
            get { return isLocationScaned; }
            set
            {
                isLocationScaned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocationScaned"));
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

        public List<TransferMaterials> MainRefillMaterialsList
        {
            get { return mainRefillMaterialsList; }
            set
            {
                mainRefillMaterialsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainRefillMaterialsList"));
            }
        }

        public LocationRefill LocationFromRefill
        {
            get { return locationFromRefill; }
            set
            {
                locationFromRefill = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationFromRefill"));
            }
        }

        public LocationRefill LocationToRefill
        {
            get { return locationToRefill; }
            set
            {
                locationToRefill = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationToRefill"));
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

        public TransferMaterials SelectedRefillMaterials
        {
            get { return selectedRefillMaterials; }
            set
            {
                selectedRefillMaterials = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRefillMaterials"));
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

        public string RowBgColor
        {
            get { return rowBgColor; }
            set
            {
                rowBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RowBgColor"));
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

        public Int64 TotalCurrentItemStock
        {
            get { return totalCurrentItemStock; }
            set
            {
                totalCurrentItemStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCurrentItemStock"));
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
        public string FgToColorLocation
        {
            get { return fgToColorLocation; }
            set
            {
                fgToColorLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FgToColorLocation"));
            }
        }
        public string FgFromColorLocation
        {
            get { return fgFromColorLocation; }
            set
            {
                fgFromColorLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FgFromColorLocation"));
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

        public bool IsSkipButtonEnable
        {
            get { return isSkipButtonEnable; }
            set
            {
                isSkipButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSkipButtonEnable"));
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

        public Uri SvgUri
        {
            get { return svgUri; }
            set
            {
                svgUri = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SvgUri"));
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

        public bool IsBackButtonEnable
        {
            get { return isBackButtonEnable; }
            set
            {
                isBackButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBackButtonEnable"));
            }
        }

        public ImageSource ImageSharedLocationReferences
        {
            get { return imageSharedLocationReferences; }
            set
            {
                imageSharedLocationReferences = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageSharedLocationReferences"));
            }
        }

        public Visibility IsSharedLocationReferencesVisible
        {
            get { return isSharedLocationReferencesVisible; }
            set
            {
                isSharedLocationReferencesVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSharedLocationReferencesVisible"));
            }
        }

        public List<Article> SharedLocationReferences
        {
            get { return sharedLocationReferences; }
            set
            {
                sharedLocationReferences = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SharedLocationReferences"));
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

        //public bool IsItem
        //{
        //    get { return isScanSerialNumber; }
        //    set
        //    {
        //        isScanSerialNumber = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsScanSerialNumber"));
        //    }
        //}

        public long BarcodeScannedQuantity
        {
            get { return barcodeScannedQuantity; }
            set
            {
                barcodeScannedQuantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BarcodeScannedQuantity"));
            }
        }

        #endregion

        #region Command

        public ICommand CommandCancelButton { get; set; }
        public ICommand SkipButtonCommand { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandOnLoaded { get; set; }
        public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        public ICommand HyperlinkClickCommand { get; set; }
        public ICommand BackButtonCommand { get; set; }
        public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }
        public DelegateCommand<object> PreviewMouseDownCommand { get; private set; }

        #endregion

        #region Constructor

        public LocationsRefillViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor LocationsRefillViewModel....", category: Category.Info, priority: Priority.Low);
            WindowWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 100;
            WindowHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 130;
            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
            CommandOnLoaded = new DelegateCommand<RoutedEventArgs>(LoadedAction);
            SkipButtonCommand = new DelegateCommand<object>(SkipButtonCommandAction);
            CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);
            HyperlinkClickCommand = new DelegateCommand<object>(HyperlinkClickCommandAction);
            BackButtonCommand = new DelegateCommand<object>(BackButtonCommandAction);
            VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
            PreviewMouseDownCommand = new DelegateCommand<object>(PreviewMouseDownCommandAction);

            IsFromLocationIndicatorVisible = Visibility.Visible;
            IsToLocationIndicatorVisible = Visibility.Hidden;
            IsInformationVisible = Visibility.Hidden;
            IsLocationScaned = false;
            IsSkipButtonEnable = true;
            IsInformationVisible = Visibility.Collapsed;
            IsSharedLocationReferencesVisible = Visibility.Collapsed;
            TrackBarEditvalue = 0;
            IsBackButtonEnable = true;
            GeosApplication.Instance.Logger.Log("Constructor LocationsRefillViewModel....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region Methods

        public void Init(List<LocationRefill> tempLocationsToRefillList)
        {
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method Init()....", category: Category.Info, priority: Priority.Low);

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

            RefillList = new List<Refill>();
            this.tempLocationsToRefillList = tempLocationsToRefillList;
            mapItems = new ObservableCollection<WarehouseLocation>();
            FilterToReFillList();

            //No Items to refill
            #region No Items to refill
            if (RefillList.Count == 0)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                return;
            }
            #endregion

            LocationToRefill = RefillList.First().LocationToRefill;
            LocationFromRefill = RefillList.First().LocationFromRefill;
            MainRefillMaterialsList = RefillList.First().ReferenceList;
            //SetNextButtonEnable();
            SaveAndLoadWarehouseLayoutFile(LocationFromRefill.FullName);

            #region Coloring

            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
            {
                BgColorFromLocation = "#FF083493";
                FgFromColorLocation = "White";

                FgToColorLocation = "Black";
                TrackBarFgColor = "#FF083493";
            }
            else
            {
                BgColorFromLocation = "#FF2AB7FF";
                FgFromColorLocation = "Black";
                FgToColorLocation = "White";
                TrackBarFgColor = "#FF2AB7FF";
            }

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            #endregion

            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][avpawar][15-06-2021][GEOS2-3097]Allow allocate Serial Numbers in different locations - Refill
        /// </summary>
        public void InitBack()
        {
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method InitBack(int LocationIndex)....", category: Category.Info, priority: Priority.Low);
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

                #region Coloring

                if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                {
                    BgColorFromLocation = "#FF083493";
                    BgColorToLocation = "White";
                    FgFromColorLocation = "White";
                    FgToColorLocation = "Black";
                    TrackBarFgColor = "#FF083493";
                }
                else
                {
                    BgColorFromLocation = "#FF2AB7FF";
                    BgColorToLocation = "#FF333333";
                    FgFromColorLocation = "Black";
                    FgToColorLocation = "White";
                    TrackBarFgColor = "#FF2AB7FF";
                }

                #endregion

                IsFromLocationIndicatorVisible = Visibility.Hidden;
                IsToLocationIndicatorVisible = Visibility.Hidden;
                IsWrongItemErrorVisible = Visibility.Hidden;
                IsWrongToLocationErrorVisible = Visibility.Hidden;
                IsWrongFromLocationErrorVisible = Visibility.Hidden;
                IsWrongItemErrorVisible = Visibility.Hidden;
                IsFromLocationScaned = false;
                IsToLocationScaned = false;
                BarcodeStr = "";
                FromLocationName = "";
                ToLocationName = "";
                WrongToLocation = string.Empty;
                WrongFromLocation = string.Empty;
                WrongItem = string.Empty;
                IsInformationVisible = Visibility.Collapsed;
                IsSharedLocationReferencesVisible = Visibility.Collapsed;
                MapItems.Clear();
                svgUri = null;
                countLocation = 0;
                locationIndex = tempLocationsToRefillList.IndexOf(RefillList[0].LocationToRefill);

                if (RefillList.Count > 0)
                    RefillList.Clear();

                try
                {
                    for (int index = locationIndex - 1; index >= 0; index--)
                    {
                        locationIndex--;
                        LocationRefill toRefill = tempLocationsToRefillList[index];
                        // [001] Changed Service method
                        LocationRefill fromRefill = WarehouseService.GetArticleWarehouseLocation_V2090(WarehouseCommon.Instance.Selectedwarehouse, toRefill.IdArticle, toRefill.Position);

                        if (fromRefill.FullName == null)
                            continue;

                        if (RefillList.Any(x => x.LocationFromRefill.FullName == fromRefill.FullName && x.LocationToRefill.FullName == toRefill.FullName))
                        {
                            continue;
                        }

                        // [001] Changed Service method
                        //List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2034(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                        // [002] Changed Service method
                        List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2160(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                        if (referenceList.Count > 0)
                        {
                            RefillList.Add(new Refill(toRefill, fromRefill, referenceList));

                            if (RefillList.Count >= 2)
                            {
                                IsBackButtonEnable = true;
                                break;
                            }
                        }
                    }
                    if (RefillList.Count == 1)
                        IsBackButtonEnable = false;

                    LocationToRefill = RefillList.First().LocationToRefill;
                    LocationFromRefill = RefillList.First().LocationFromRefill;
                    MainRefillMaterialsList = RefillList.First().ReferenceList;
                    SaveAndLoadWarehouseLayoutFile(LocationFromRefill.FullName);
                    SetSkipButtonEnable();
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in InitBack(int LocationIndex) Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method InitBack(int LocationIndex) Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method InitBack(int LocationIndex)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in InitBack(int LocationIndex)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method InitBack(int LocationIndex)....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][avpawar][15-076-2021][GEOS2-3097]Allow allocate Serial Numbers in different locations - Refill
        /// </summary>
        public void Init(int LocationIndex)
        {
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method Init(int LocationIndex)....", category: Category.Info, priority: Priority.Low);

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
                #region Coloring

                if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                {
                    BgColorFromLocation = "#FF083493";
                    BgColorToLocation = "White";
                    FgFromColorLocation = "White";
                    FgToColorLocation = "Black";
                    TrackBarFgColor = "#FF083493";
                }
                else
                {
                    BgColorFromLocation = "#FF2AB7FF";
                    BgColorToLocation = "#FF333333";
                    FgFromColorLocation = "Black";
                    FgToColorLocation = "White";
                    TrackBarFgColor = "#FF2AB7FF";
                }

                #endregion

                //Wrong Scan item one and click next button scenario
                IsFromLocationIndicatorVisible = Visibility.Hidden;
                IsToLocationIndicatorVisible = Visibility.Hidden;
                IsWrongItemErrorVisible = Visibility.Hidden;
                IsWrongToLocationErrorVisible = Visibility.Hidden;
                IsWrongFromLocationErrorVisible = Visibility.Hidden;
                IsWrongItemErrorVisible = Visibility.Hidden;
                IsFromLocationScaned = false;
                IsToLocationScaned = false;
                BarcodeStr = "";
                FromLocationName = "";
                ToLocationName = "";
                WrongToLocation = string.Empty;
                WrongFromLocation = string.Empty;
                WrongItem = string.Empty;
                IsInformationVisible = Visibility.Collapsed;
                IsSharedLocationReferencesVisible = Visibility.Collapsed;
                MapItems.Clear();
                svgUri = null;
                countLocation = 0;
                locationIndex = tempLocationsToRefillList.IndexOf(RefillList[0].LocationToRefill);

                if (RefillList.Count > 0)
                    RefillList.Clear();

                try
                {
                    GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method FilterToReFillList()....", category: Category.Info, priority: Priority.Low);
                    for (int index = locationIndex + 1; index < tempLocationsToRefillList.Count; index++)
                    {
                        try
                        {
                            locationIndex++;

                            LocationRefill toRefill = tempLocationsToRefillList[index];
                            // [001] Changed Service method
                            LocationRefill fromRefill = WarehouseService.GetArticleWarehouseLocation_V2090(WarehouseCommon.Instance.Selectedwarehouse, toRefill.IdArticle, toRefill.Position);

                            if (fromRefill.FullName == null)
                                continue;

                            if (RefillList.Any(x => x.LocationFromRefill.FullName == fromRefill.FullName && x.LocationToRefill.FullName == toRefill.FullName))
                            {
                                continue;
                            }

                            // [001] Changed Service method
                            //List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2034(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                            // [002] Changed Service method
                            List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2160(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                            if (referenceList.Count > 0)
                            {
                                RefillList.Add(new Refill(toRefill, fromRefill, referenceList));

                                if (RefillList.Count >= 2)
                                {
                                    IsSkipButtonEnable = true;
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in FilterToReFillList...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    if (RefillList.Count == 1)
                        IsSkipButtonEnable = false;

                    LocationToRefill = RefillList.First().LocationToRefill;          // [LocationIndex].LocationToRefill;
                    LocationFromRefill = RefillList.First().LocationFromRefill;      // [LocationIndex].LocationFromRefill;
                    MainRefillMaterialsList = RefillList.First().ReferenceList;      // [LocationIndex].ReferenceList;
                    SaveAndLoadWarehouseLayoutFile(LocationFromRefill.FullName);

                    SetBackButtonEnable();
                    GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method FilterToReFillList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in FilterToReFillList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method FilterToReFillList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method FilterToReFillList...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Init (int LocationIndex)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method Init(int LocationIndex)....executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][avpawar][15-06-2021][GEOS2-3097]Allow allocate Serial Numbers in different locations - Refill
        /// </summary>
        public void FilterToReFillList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method FilterToReFillList()....", category: Category.Info, priority: Priority.Low);

                //Int64 _idwarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                //bool isReferenceFound = false;
                //   Filling with thread if found in 2 reference move forward

                // FromRefillThread = new Thread(() =>
                //{

                foreach (LocationRefill toRefill in tempLocationsToRefillList)
                {
                    try
                    {
                        // [001] Changed Service method
                        LocationRefill fromRefill = WarehouseService.GetArticleWarehouseLocation_V2090(WarehouseCommon.Instance.Selectedwarehouse, toRefill.IdArticle, toRefill.Position);

                        if (fromRefill.FullName == null)
                            continue;

                        if (RefillList.Any(x => x.LocationFromRefill.FullName == fromRefill.FullName && x.LocationToRefill.FullName == toRefill.FullName))
                        {
                            continue;
                        }

                        // [001] Changed Service method
                        //List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2034(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                        // [002] Changed Service method
                        List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2160(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                        if (referenceList.Count > 0)
                        {
                            locationIndex++;
                            RefillList.Add(new Refill(toRefill, fromRefill, referenceList));

                            if (RefillList.Count >= 2)
                            {
                                //isReferenceFound = true;
                                IsSkipButtonEnable = true;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in FilterToReFillList...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    //finally
                    //{
                    //    if (tempLocationsToRefillList.IndexOf(toRefill) == tempLocationsToRefillList.Count - 1)
                    //    {
                    //        //isReferenceFound = true;
                    //    }
                    //}
                }

                if (RefillList.Count == 1)
                {
                    IsSkipButtonEnable = false;
                }

                IsBackButtonEnable = false;

                //if (tempLocationsToRefillList.Count == 0)
                //    isReferenceFound = true;

                //});
                //FromRefillThread.Name = "FromRefillThread";
                //FromRefillThread.Start();

                //while (!isReferenceFound)
                //{
                //    Thread.Sleep(1000);
                //}

                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method FilterToReFillList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FilterToReFillList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method FilterToReFillList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method FilterToReFillList...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for do scan opration on scanned barcode.
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            try
            {
                if (obj.Text == "\r") // && !isLocationLast)
                {
                    GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);

                    #region FromLocation

                    if (!IsFromLocationScaned)
                    {
                        if (LocationFromRefill.FullName.Equals(BarcodeStr))
                        {
                            BarcodeStr = "";
                            //Maps Clearing for scan to location
                            MapItems.Clear();
                            svgUri = null;
                            WrongFromLocation = string.Empty;
                            IsWrongFromLocationErrorVisible = Visibility.Hidden;

                            // Shared references in from location
                            FillSharedLocationReferences();

                            SaveAndLoadWarehouseLayoutFile(LocationToRefill.FullName);
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);

                            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                            {
                                BgColorFromLocation = "#FF008000";
                                BgColorToLocation = "#FF083493";
                                FgFromColorLocation = "White";
                                FgToColorLocation = "White";
                                TrackBarFgColor = "#FF083493";
                            }
                            else
                            {
                                BgColorFromLocation = "#FF008000";
                                BgColorToLocation = "#FF2AB7FF";
                                FgFromColorLocation = "Black";
                                FgToColorLocation = "Black";
                                TrackBarFgColor = "#FF2AB7FF";
                            }

                            IsFromLocationIndicatorVisible = Visibility.Hidden;
                            IsToLocationIndicatorVisible = Visibility.Visible;
                            IsFromLocationScaned = true;
                            TrackBarEditvalue = 1;
                        }
                        else
                        {
                            WrongFromLocation = string.Format("Wrong Location {0}", BarcodeStr);
                            IsWrongFromLocationErrorVisible = Visibility.Visible;
                            BarcodeStr = "";
                            FromLocationName = "";
                            BgColorFromLocation = "#FFFF0000";
                            IsFromLocationScaned = false;
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }
                    }
                    #endregion FromLocation

                    #region ToLocation
                    else if (!IsToLocationScaned)
                    {
                        if (LocationToRefill.FullName.Equals(BarcodeStr))
                        {
                            BarcodeStr = "";

                            WrongToLocation = string.Empty;
                            IsWrongToLocationErrorVisible = Visibility.Hidden;
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);

                            if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                            {
                                BgColorToLocation = "#FF008000";
                                TrackBarFgColor = "#FF083493";
                            }
                            else
                            {
                                BgColorToLocation = "#FF008000";
                                TrackBarFgColor = "#FF2AB7FF";
                            }

                            IsToLocationScaned = true;
                            TrackBarEditvalue = 2;
                            IsSharedLocationReferencesVisible = Visibility.Collapsed;
                            SharedLocationReferences = null;
                            ScanReference(0, 0);
                        }
                        else
                        {
                            WrongToLocation = string.Format("Wrong Location {0}", BarcodeStr);
                            IsWrongToLocationErrorVisible = Visibility.Visible;
                            BarcodeStr = "";
                            ToLocationName = "";
                            BgColorToLocation = "#FFFF0000";
                            IsToLocationScaned = false;
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }

                    }
                    #endregion ToLocation

                    #region ITEM Scanning
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

                                    //Int64 locationAvailableQuantity = WarehouseService.GetStockForScanItem(tm.IdArticle, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, sn.IdWarehouseDeliveryNoteItem, tm.IdWarehouseLocation);
                                    //if (locationAvailableQuantity < 1)
                                    //{
                                    //    UpdateProgressbarLocationStock();
                                    //    IsWrongItemErrorVisible = Visibility.Visible;
                                    //    WrongItem = String.Format("No location stock available {0}", BarcodeStr);
                                    //    BarcodeStr = "";
                                    //    RowBgColor = "#FFFF0000";
                                    //    OnFocusBgColor = "#FFFF0000";
                                    //    OnFocusFgColor = "#FFFFFFFF";
                                    //    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);

                                    //    return;
                                    //}

                                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                    SelectedSerialNumber.Code = BarcodeStr;
                                    //assign all values to scanned serial number.
                                    SelectedSerialNumber.IdSerialNumber = sn.IdSerialNumber;
                                    SelectedSerialNumber.IdArticle = sn.IdArticle;
                                    SelectedSerialNumber.IdWarehouseDeliveryNoteItem = sn.IdWarehouseDeliveryNoteItem;
                                    SelectedSerialNumber.IdWarehouseLocation = tempLocationsToRefillList.Where(x => x.FullName == LocationToRefill.FullName.ToUpper()).Select(x => x.IdWarehouseLocation).FirstOrDefault();
                                    if (SelectedSerialNumber.IdWarehouseLocation.HasValue)
                                        tm.IdWarehouseLocation = SelectedSerialNumber.IdWarehouseLocation.Value;
                                    SelectedSerialNumber.IdWarehouse = sn.IdWarehouse;
                                    SelectedSerialNumber.IsScanned = true;
                                    //tm.ScanSerialNumbers.Add(tm.ScanSerialNumbers.Where(x => x.Code = BarcodeStr))
                                    tm.ScannedQty = 1;
                                    BarcodeScannedQuantity -= 1;

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

                                    bool IsScanItem = false;
                                    if (tm.ScannedQty != 0)
                                    {
                                        TransferMaterials FromTM = (TransferMaterials)tm.Clone();
                                        long? FromTmIdWarehouseLocation = LocationFromRefill.IdWarehouseLocation;
                                        if (FromTmIdWarehouseLocation.HasValue)
                                            FromTM.IdWarehouseLocation = FromTmIdWarehouseLocation.Value;
                                        FromTM.ScannedQty = -1;

                                        IsScanItem = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(FromTM);
                                        IsScanItem = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(tm);
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
                            var isProperBarcode = !string.IsNullOrEmpty(BarcodeStr) && BarcodeStr.All(Char.IsDigit);
                            if (BarcodeStr.Length < 11)
                            {
                                isProperBarcode = false;
                            }

                            if (isProperBarcode)
                            {
                                string _idwarehouseStr = BarcodeStr.Substring(0, 3);
                                string _idWareHouseDeliveryNoteItemStr = BarcodeStr.Substring(3, 8);
                                string _itemQuantityStr = BarcodeStr.Substring(11, 6);

                                Int64 _idwarehouse = Convert.ToInt64(_idwarehouseStr);
                                Int64 _idWareHouseDeliveryNoteItem = Convert.ToInt64(_idWareHouseDeliveryNoteItemStr);
                                Int64 _itemQuantity = Convert.ToInt64(_itemQuantityStr);
                                BarcodeScannedQuantity = Convert.ToInt64(BarcodeStr.Substring(11, 6));

                                ScanReference(_idWareHouseDeliveryNoteItem, _itemQuantity);

                                //if (SelectedRefillMaterials.RegisterSerialNumber == 1)
                                //{
                                //    if (SelectedRefillMaterials.ScanSerialNumbers == null)
                                //        SelectedRefillMaterials.ScanSerialNumbers = new List<SerialNumber>();
                                //    _itemQuantity = SelectedRefillMaterials.Quantity;//All SerialNumbers can be Downloaded ntg to do with scanned number 
                                //    SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = SelectedRefillMaterials };
                                //    SelectedRefillMaterials.ScanSerialNumbers.Add(scanSerialNumber);

                                //    SelectedSerialNumber = scanSerialNumber;
                                //    IsScanSerialNumber = true;
                                //}

                                BarcodeStr = "";
                            }
                            else
                            {
                                WrongItem = string.Format("Wrong Item {0}", BarcodeStr);
                                IsWrongItemErrorVisible = Visibility.Visible;
                                BarcodeStr = "";
                                RowBgColor = "#FFFF0000";
                                OnFocusBgColor = "#FFFF0000";
                                OnFocusFgColor = "#FFFFFFFF";
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    //BarcodeStr = BarcodeStr + obj.Text;
                    //[pramod.misal][GEOS2-5067][08-01-2024]
                    BarcodeStr = BarcodeStr + obj.Text.ToUpper();
                }
            }
            catch (Exception ex)
            {
                BarcodeStr = "";
                GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][2019-07-11][skhade][GEOS2-1709-Warning about shared location references when doing the Refill]
        /// </summary>
        private void FillSharedLocationReferences()
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

                if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                {
                    ImageSharedLocationReferences = new BitmapImage(new Uri("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/wShare.png", UriKind.Relative));
                }
                else
                {
                    ImageSharedLocationReferences = new BitmapImage(new Uri("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/bShare.png", UriKind.Relative));
                }

                List<Article> tempSharedLocationReferences = WarehouseService.GetArticlesByLocation(WarehouseCommon.Instance.Selectedwarehouse, LocationFromRefill.IdWarehouseLocation, LocationToRefill.IdWarehouseLocation);
                //SharedLocationReferences = WarehouseService.GetArticlesByLocation(WarehouseCommon.Instance.Selectedwarehouse, 8454, 8638);

                foreach (TransferMaterials refillItem in MainRefillMaterialsList)
                {
                    tempSharedLocationReferences.Remove(tempSharedLocationReferences.FirstOrDefault(x => x.IdArticle == refillItem.IdArticle));
                }

                SharedLocationReferences = tempSharedLocationReferences;

                if (SharedLocationReferences.Count > 0)
                {
                    IsSharedLocationReferencesVisible = Visibility.Visible;

                    foreach (Article article in SharedLocationReferences)
                    {
                        if (article.ToWarehouseLocation != null)
                        {
                            article.ToWarehouseLocation.HtmlColor = "#FFFF00";
                            MapItems.Add(article.ToWarehouseLocation);
                        }
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in LocationsRefillViewModel FillSharedLocationReferences() Method - {0}", ex.Detail.ErrorMessage), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in LocationsRefillViewModel FillSharedLocationReferences() Method - ServiceUnexceptedException - {0}", ex.Message), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillSharedLocationReferences - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ScanReference(Int64 idWareHouseDeliveryNoteItem, Int64 quantity)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method ScanReference....", category: Category.Info, priority: Priority.Low);

                if (countLocation == 0)
                {
                    SelectedRefillMaterials = MainRefillMaterialsList[0];
                    ArticleImage = ByteArrayToImage(SelectedRefillMaterials.ArticleImageInBytes);

                    // TotalCurrentItemStock = SelectedRefillMaterials.CurrentStock;
                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedRefillMaterials.IdArticle, Convert.ToInt32(SelectedRefillMaterials.IdWarehouse));
                    ChangeTotalStockColor(TotalCurrentItemStock, SelectedRefillMaterials.Article.MyWarehouse.MinimumStock);
                    UpdateProgressbarLocationStock();
                    UpdateProgressbarArticleStock();
                    //ArticleWarehouseLocations articleWarehouseLocations = WarehouseService.GetArticleStockByWarehouseLocation(SelectedRefillMaterials.IdArticle, LocationToRefill.IdWarehouseLocation, SelectedRefillMaterials.IdWarehouse);

                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, LocationToRefill.MinimumStock);
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
                    IsSharedLocationReferencesVisible = Visibility.Collapsed;
                    countLocation++;

                    if (MainRefillMaterialsList.Count > 1)
                        IsSkipButtonEnable = true;
                }
                else if (IsToLocationScaned)
                {
                    tempItem = MainRefillMaterialsList.Where(tm => tm.IdWareHouseDeliveryNoteItem == idWareHouseDeliveryNoteItem).FirstOrDefault();

                    if (tempItem != null)
                    {
                        //This is used to avoid minus stock for warehousedeliverynoteitem.
                        Int64 locationAvaibleQuantity = WarehouseService.GetStockForScanItem(tempItem.IdArticle, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, idWareHouseDeliveryNoteItem, LocationFromRefill.IdWarehouseLocation);

                        if (locationAvaibleQuantity < quantity || quantity == 0)
                        {
                            UpdateProgressbarLocationStock();
                            IsWrongItemErrorVisible = Visibility.Visible;
                            WrongItem = String.Format("No location stock available {0}", BarcodeStr);
                            BarcodeStr = "";
                            RowBgColor = "#FFFF0000";
                            OnFocusBgColor = "#FFFF0000";
                            OnFocusFgColor = "#FFFFFFFF";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            return;
                        }
                        else
                        {
                            IsWrongItemErrorVisible = Visibility.Collapsed;
                            WrongItem = string.Empty;
                        }

                        SelectedRefillMaterials = tempItem;
                        IsWrongItemErrorVisible = Visibility.Hidden;
                        ArticleImage = ByteArrayToImage(SelectedRefillMaterials.ArticleImageInBytes);
                        TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedRefillMaterials.IdArticle, Convert.ToInt32(SelectedRefillMaterials.IdWarehouse));
                        ChangeTotalStockColor(TotalCurrentItemStock, SelectedRefillMaterials.Article.MyWarehouse.MinimumStock);
                        UpdateProgressbarLocationStock();
                        UpdateProgressbarArticleStock();
                        ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, LocationToRefill.MinimumStock);
                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);

                        if (SelectedRefillMaterials != null && tempItem.Quantity >= quantity)
                        {
                            tempItem.Quantity -= quantity;
                            tempItem.ScannedQty = quantity;
                            tempItem.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            tempItem.Comments = "Refill from " + "\"" + LocationFromRefill.FullName + "\"" + " location to " + "\"" + LocationToRefill.FullName + "\"";

                            TransferMaterials psa = (TransferMaterials)tempItem.Clone();
                            psa.IdWarehouseLocation = LocationFromRefill.IdWarehouseLocation;
                            psa.ScannedQty = (-1) * psa.ScannedQty;
                            tempItem.IdWarehouseLocation = LocationToRefill.IdWarehouseLocation;

                            //Change destination warehous location id before insert.
                            //    tempItem.IdWarehouseLocation = IdwarehouseLocarion;
                            //bool IsdiductFromtransit = WarehouseService.InsertIntoArticleStockForTransferMaterial(psa);
                            //bool IsInsertArticletoLocation = WarehouseService.InsertIntoArticleStockForTransferMaterial(tempItem);


                            //bool IsdiductFromtransit = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(psa);
                            //bool IsInsertArticletoLocation = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(tempItem);

                            if (SelectedRefillMaterials.RegisterSerialNumber != 1)
                            {
                                bool IsdiductFromtransit = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(psa);
                                bool IsInsertArticletoLocation = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(tempItem);

                                //Updateing the progress bar of location stock 
                                ProgressbarLocationStock.LocationStock += quantity;
                                LocationStockValue = ProgressbarLocationStock.LocationStock;
                            }

                            // code is for to keep current from location till all value Refill to other location.


                            long _fromLocationWarehouseId = MainRefillMaterialsList[0].IdWarehouseLocation;

                            RefillList[0].LocationToRefill.CurrenStock += quantity;

                            ////Updateing the progress bar of location stock 
                            //ProgressbarLocationStock.LocationStock += quantity;
                            //LocationStockValue = ProgressbarLocationStock.LocationStock;

                            if (ProgressbarLocationStock.MaximumStock == 0)
                                LocationStockValue = 0;
                            else if (ProgressbarLocationStock.MaximumStock < LocationStockValue)
                                LocationStockValue = ProgressbarLocationStock.MaximumStock;

                            //Updating the lable Clr based on LocationStock
                            ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, LocationToRefill.MinimumStock);
                            UpdateProgressbarLocationStock();
                            UpdateProgressbarArticleStock();

                            if (tempItem.Quantity > 0)
                                tempItem.IdWarehouseLocation = _fromLocationWarehouseId;

                            if (SelectedRefillMaterials.RegisterSerialNumber == 1)
                            {
                                if (SelectedRefillMaterials.ScanSerialNumbers == null)
                                    SelectedRefillMaterials.ScanSerialNumbers = new List<SerialNumber>();
                                //_itemQuantity = SelectedRefillMaterials.Quantity;//All SerialNumbers can be Downloaded ntg to do with scanned number 
                                SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = SelectedRefillMaterials };
                                // scanSerialNumber = SelectedTransferMaterials.SerialNumbers
                                SelectedRefillMaterials.ScanSerialNumbers.Add(scanSerialNumber);
                                SelectedSerialNumber = scanSerialNumber;
                                IsScanSerialNumber = true;
                                BarcodeStr = "";
                                return;
                            }

                            //if (SelectedRefillMaterials.RegisterSerialNumber == 1)
                            //{
                            //    if (SelectedRefillMaterials.ScanSerialNumbers == null)
                            //        SelectedRefillMaterials.ScanSerialNumbers = new List<SerialNumber>();
                            //    //_itemQuantity = SelectedRefillMaterials.Quantity;//All SerialNumbers can be Downloaded ntg to do with scanned number 
                            //    SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = SelectedRefillMaterials };
                            //    // scanSerialNumber = SelectedTransferMaterials.SerialNumbers
                            //    //SelectedRefillMaterials.ScanSerialNumbers.Add(scanSerialNumber);
                            //    SelectedSerialNumber = new SerialNumber();
                            //    SelectedSerialNumber.MasterItem = new object();
                            //    SelectedSerialNumber = scanSerialNumber;
                            //    SelectedRefillMaterials = new TransferMaterials();
                            //    SelectedRefillMaterials = tempItem;
                            //    SelectedRefillMaterials.ScanSerialNumbers.Add(scanSerialNumber);
                            //    IsScanSerialNumber = true;
                            //    BarcodeStr = "";
                            //    return;
                            //}


                            //Remove scanned article from list if quantity is zero.
                            if (tempItem.Quantity == 0)
                            {
                                #region GEOS2-5863
                                try
                                {
                                    bool isGoToNextItem = false;
                                    foreach (TransferMaterials Material in MainRefillMaterialsList)
                                    {
                                        if (Material.Quantity == 0)
                                        {
                                            if (IsSkipButtonEnable)
                                            {
                                                isGoToNextItem = true;
                                            }
                                        }
                                        else
                                        {
                                            isGoToNextItem = false;
                                            return;
                                        }
                                    }
                                    #region isGoToNextItem
                                    if (isGoToNextItem)
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
                                        //GoToNextItem(SelectedRefillMaterials);
                                        //SkipButtonCommandAction(new object());
                                        Init(locationIndex);
                                        Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocationsRefillUserControlView", null, this);
                                        IsWrongItemErrorVisible = Visibility.Collapsed;
                                        WrongItem = string.Empty;
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
                                        //return;
                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                       
                                    }
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                }
                                #endregion
                                int index = MainRefillMaterialsList.FindIndex(tr => tr.IdWareHouseDeliveryNoteItem == tempItem.IdWareHouseDeliveryNoteItem);
                                if (index == MainRefillMaterialsList.Count - 1)
                                {
                                    if (IsSkipButtonEnable)
                                    {
                                        Init(locationIndex);
                                        Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocationsRefillUserControlView", null, this);
                                        return;
                                    }
                                }

                                //if (SelectedRefillMaterials.RegisterSerialNumber == 1)
                                //{
                                //    if (SelectedRefillMaterials.ScanSerialNumbers == null)
                                //        SelectedRefillMaterials.ScanSerialNumbers = new List<SerialNumber>();
                                //    //_itemQuantity = SelectedRefillMaterials.Quantity;//All SerialNumbers can be Downloaded ntg to do with scanned number 
                                //    SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = SelectedRefillMaterials };
                                //    // scanSerialNumber = SelectedTransferMaterials.SerialNumbers
                                //    SelectedRefillMaterials.ScanSerialNumbers.Add(scanSerialNumber);
                                //    SelectedSerialNumber = scanSerialNumber;
                                //    IsScanSerialNumber = true;
                                //    BarcodeStr = "";
                                //    return;
                                //}

                                //if (IsScanSerialNumber)
                                //{
                                //    IsWrongItemErrorVisible = Visibility.Hidden;
                                //    WrongItem = string.Empty;

                                //    TransferMaterials tm = SelectedSerialNumber.MasterItem as TransferMaterials;

                                //    //Observation comment for serial number.
                                //    if (string.IsNullOrEmpty(materialSerialNumbers))
                                //        materialSerialNumbers = String.Format(Application.Current.FindResource("MaterialSerialNumbers").ToString() + "\r\n", tm.Reference);

                                //    if (tm.ScanSerialNumbers.Exists(x => x.Code == BarcodeStr))     //Show error if already scanned item.
                                //    {
                                //        IsWrongItemErrorVisible = Visibility.Visible;
                                //        WrongItem = "Wrong Serial Number " + BarcodeStr;
                                //        BarcodeStr = string.Empty;
                                //        RowBgColor = "#FFFF0000";       // color = cream Red
                                //        OnFocusBgColor = "#FFFF0000";   // color = cream Red
                                //        OnFocusFgColor = "#FFFFFFFF";   // color = cream White
                                //                                        //SelectedLocationList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                //        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                //    }
                                //    else
                                //    {
                                //        if (tm.SerialNumbers.Exists(x => x.Code == BarcodeStr) && SelectedSerialNumber.Code == null)     //Add as scanned in serial numbers if true
                                //        {
                                //            SerialNumber sn = (SerialNumber)tm.SerialNumbers.FirstOrDefault(x => x.Code == BarcodeStr).Clone();

                                //            //Int64 locationAvailableQuantity = WarehouseService.GetStockForScanItem(tm.IdArticle, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, sn.IdWarehouseDeliveryNoteItem, tm.IdWarehouseLocation);
                                //            //if (locationAvailableQuantity < 1)
                                //            //{
                                //            //    UpdateProgressbarLocationStock();
                                //            //    IsWrongItemErrorVisible = Visibility.Visible;
                                //            //    WrongItem = String.Format("No location stock available {0}", BarcodeStr);
                                //            //    BarcodeStr = "";
                                //            //    RowBgColor = "#FFFF0000";
                                //            //    OnFocusBgColor = "#FFFF0000";
                                //            //    OnFocusFgColor = "#FFFFFFFF";
                                //            //    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);

                                //            //    return;
                                //            //}

                                //            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                //            SelectedSerialNumber.Code = BarcodeStr;
                                //            //assign all values to scanned serial number.
                                //            SelectedSerialNumber.IdSerialNumber = sn.IdSerialNumber;
                                //            SelectedSerialNumber.IdArticle = sn.IdArticle;
                                //            SelectedSerialNumber.IdWarehouseDeliveryNoteItem = sn.IdWarehouseDeliveryNoteItem;
                                //            SelectedSerialNumber.IdWarehouseLocation = tempLocationsToRefillList.Where(x => x.FullName == LocationToRefill.FullName.ToUpper()).Select(x => x.IdWarehouseLocation).FirstOrDefault();
                                //            if (SelectedSerialNumber.IdWarehouseLocation.HasValue)
                                //                tm.IdWarehouseLocation = SelectedSerialNumber.IdWarehouseLocation.Value;
                                //            SelectedSerialNumber.IdWarehouse = sn.IdWarehouse;
                                //            SelectedSerialNumber.IsScanned = true;
                                //            //tm.ScanSerialNumbers.Add(tm.ScanSerialNumbers.Where(x => x.Code = BarcodeStr))
                                //            tm.ScannedQty = 1;
                                //            BarcodeScannedQuantity -= 1;

                                //            if (BarcodeScannedQuantity > 0)
                                //            {
                                //                SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = tm };
                                //                tm.ScanSerialNumbers.Add(scanSerialNumber);
                                //                SelectedSerialNumber = scanSerialNumber;
                                //                IsScanSerialNumber = true;
                                //                BarcodeStr = string.Empty;
                                //            }
                                //            else if (BarcodeScannedQuantity == 0)
                                //            {
                                //                IsScanSerialNumber = false;
                                //            }

                                //            bool IsScanItem = false;
                                //            if (tm.ScannedQty != 0)
                                //            {
                                //                TransferMaterials FromTM = (TransferMaterials)tm.Clone();
                                //                long? FromTmIdWarehouseLocation = LocationFromRefill.IdWarehouseLocation;
                                //                if (FromTmIdWarehouseLocation.HasValue)
                                //                    FromTM.IdWarehouseLocation = FromTmIdWarehouseLocation.Value;
                                //                FromTM.ScannedQty = -1;

                                //                IsScanItem = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(FromTM);
                                //                IsScanItem = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(tm);
                                //            }
                                //        }
                                //        else
                                //        {
                                //            //Show error message if scanned item is wrong.
                                //            IsWrongItemErrorVisible = Visibility.Visible;
                                //            WrongItem = "Wrong Serial Number " + BarcodeStr;
                                //            BarcodeStr = string.Empty;
                                //            OnFocusBgColor = "#FFFF0000";   // color = cream Red
                                //            OnFocusFgColor = "#FFFFFFFF";   // color = cream White
                                //                                            //PendingStorageArticlesList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                //            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);

                                //        }
                                //    }

                                //    BarcodeStr = string.Empty;
                                //}

                                MainRefillMaterialsList.Remove(tempItem);

                                if (MainRefillMaterialsList.Count == 1)
                                {
                                    SelectedRefillMaterials = MainRefillMaterialsList[0];
                                    ArticleImage = ByteArrayToImage(SelectedRefillMaterials.ArticleImageInBytes);
                                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedRefillMaterials.IdArticle, Convert.ToInt32(SelectedRefillMaterials.IdWarehouse));
                                    ChangeTotalStockColor(TotalCurrentItemStock, SelectedRefillMaterials.Article.MyWarehouse.MinimumStock);
                                    UpdateProgressbarLocationStock();
                                    UpdateProgressbarArticleStock();
                                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, LocationToRefill.MinimumStock);
                                }
                                else if (MainRefillMaterialsList.Count > 0)
                                {
                                    if (MainRefillMaterialsList.Count - 1 > index)
                                        SelectedRefillMaterials = MainRefillMaterialsList[index];
                                    else
                                        SelectedRefillMaterials = MainRefillMaterialsList[index - 1];

                                    ArticleImage = ByteArrayToImage(SelectedRefillMaterials.ArticleImageInBytes);
                                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedRefillMaterials.IdArticle, Convert.ToInt32(SelectedRefillMaterials.IdWarehouse));
                                    ChangeTotalStockColor(TotalCurrentItemStock, SelectedRefillMaterials.Article.MyWarehouse.MinimumStock);
                                    UpdateProgressbarLocationStock();
                                    UpdateProgressbarArticleStock();
                                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, LocationToRefill.MinimumStock);
                                }
                                else if (MainRefillMaterialsList.Count == 0)
                                {
                                    SelectedRefillMaterials = null;
                                    ArticleImage = null;
                                    IsWrongItemErrorVisible = Visibility.Hidden;
                                    WrongItem = String.Empty;

                                    if (IsSkipButtonEnable)
                                    {
                                        TotalCurrentItemStock = 0;

                                        BgColorFromLocation = "#FFF4E27E";
                                        BgColorToLocation = "#FFFFFFFF";
                                        if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                                        {
                                            BgColorFromLocation = "#FF083493";
                                            BgColorToLocation = "#FF083493";
                                            TrackBarFgColor = "#FF083493";
                                        }
                                        else
                                        {
                                            BgColorFromLocation = "#FF2AB7FF";
                                            BgColorToLocation = "#FF2AB7FF";
                                            TrackBarFgColor = "#FF2AB7FF";
                                        }

                                        IsFromLocationScaned = false;
                                        IsToLocationScaned = false;
                                        IsFromLocationIndicatorVisible = Visibility.Visible;
                                        IsToLocationIndicatorVisible = Visibility.Hidden;
                                        IsInformationVisible = Visibility.Hidden;
                                        countLocation = 0;
                                        TrackBarEditvalue = 3;
                                    }

                                    SkipButtonCommandAction(new object());
                                }

                                MainRefillMaterialsList = new List<TransferMaterials>(MainRefillMaterialsList);
                            }
                            else
                            {
                                OnFocusBgColor = "#FFF4E27E";
                                OnFocusFgColor = "#FF000000";
                            }
                        }
                        else
                        {
                            WrongItem = string.Format("Wrong Item {0}", BarcodeStr);
                            IsWrongItemErrorVisible = Visibility.Visible;
                            OnFocusBgColor = "#FFFF0000";
                            OnFocusFgColor = "#FFFFFFFF";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }
                    }
                    else
                    {
                        WrongItem = string.Format("Wrong Item {0}", BarcodeStr);
                        IsWrongItemErrorVisible = Visibility.Visible;
                        OnFocusBgColor = "#FFFF0000";
                        OnFocusFgColor = "#FFFFFFFF";
                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                    }
                }

                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method ScanReference....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method ScanReference() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method ScanReference() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanReference...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for change Total stock color as per condition.
        /// </summary>
        /// <param name="totalStock"></param>
        /// <param name="minQuantity"></param>
        private void ChangeTotalStockColor(Int64 totalStock, Int64 minQuantity)
        {
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method ChangeTotalStockColor....", category: Category.Info, priority: Priority.Low);

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

            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method ChangeTotalStockColor....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for scan next location items.
        /// </summary>
        /// <param name="obj"></param>
        private void SkipButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method NextButtonCommandAction....", category: Category.Info, priority: Priority.Low);

                if (IsSkipButtonEnable)
                {
                    if (IsFromLocationScaned && IsToLocationScaned && MainRefillMaterialsList.Count == 1)
                    {
                        Init(locationIndex);
                        Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocationsRefillUserControlView", null, this);
                    }
                    else if (IsFromLocationScaned && IsToLocationScaned)
                    {
                        GoToNextItem(SelectedRefillMaterials);
                    }
                    else
                    {
                        Init(locationIndex);
                        Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocationsRefillUserControlView", null, this);
                    }
                }
                else
                {
                    TrackBarEditvalue = 4;
                }

                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method NextButtonCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LocationsRefillViewModel Method NextButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource ByteArrayToImage(byte[] articleImageInBytes)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method ByteArrayToImage....", category: Category.Info, priority: Priority.Low);

                if (articleImageInBytes != null)
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(articleImageInBytes);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    biImg.DecodePixelHeight = 10;
                    biImg.DecodePixelWidth = 10;
                    ImageSource imgSrc = biImg as ImageSource;
                    return imgSrc;
                }

                GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method ByteArrayToImage....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ByteArrayToImage...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        private void SaveAndLoadWarehouseLayoutFile(string warehouseLocationName)
        {
            WarehouseLocation wlocation = WarehouseService.GetWarehouseLocationByFullName(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, warehouseLocationName);
            wlocation.FullName = warehouseLocationName;
            wlocation.HtmlColor = null;

            if (wlocation != null)
            {
                MapItems.Add(wlocation);

                try
                {
                    byte[] warehouseLayout = GeosRepositoryService.GetCompanyLayoutFile(wlocation.FileName);

                    if (warehouseLayout != null)
                    {
                        string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                        if (!Directory.Exists(basePath))
                        {
                            Directory.CreateDirectory(basePath);
                        }

                        string svgFilePath = string.Format(@"{0}\Data\{1}", Path.GetTempPath(), wlocation.FileName);
                        File.WriteAllBytes(svgFilePath, warehouseLayout);
                        SvgUri = new Uri(svgFilePath);
                    }
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log("Rrror in Loading warehouse layout svg file " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Error in Loading warehouse layout svg file - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in Loading warehouse layout svg file - {0}. ErrorMessage- {1}", wlocation.FileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        //public void SetNextButtonEnable()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method SetNextButtonEnable....", category: Category.Info, priority: Priority.Low);
        //        //if (FromRefillThread.IsAlive)
        //        //{
        //        //    while (RefillList.Count < locationIndex + 1 && FromRefillThread.IsAlive)
        //        //    {
        //        //        Thread.Sleep(100);
        //        //    }
        //        //}
        //        for (int item = locationIndex + 1; item < RefillList.Count; item++)
        //        {
        //            List<TransferMaterials> material = RefillList[item].ReferenceList;
        //            if (material.Any(x => x.Quantity > 0))
        //            {
        //                IsNextButtonEnable = true;
        //                break;
        //            }
        //            else
        //            {
        //                IsNextButtonEnable = false;
        //            }
        //        }
        //        if (locationIndex >= RefillList.Count - 1)
        //        {
        //            IsNextButtonEnable = false;
        //        }
        //        GeosApplication.Instance.Logger.Log("PickingMaterialsViewModel Method SetNextButtonEnable........executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method SetNextButtonEnable...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        void ListSourceDataAdapterCustomizeMapItem(object e)
        {
            MapRectangle rectangle = ((DevExpress.Xpf.Map.CustomizeMapItemEventArgs)e).MapItem as MapRectangle;
            WarehouseLocation wl = ((DevExpress.Xpf.Map.CustomizeMapItemEventArgs)e).SourceObject as WarehouseLocation;

            if (wl.HtmlColor != null)
            {
                rectangle.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString(wl.HtmlColor);
            }
            else if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
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
        /// Method for forcus form when it  load.
        /// </summary>
        /// <param name="obj"></param>
        private void LoadedAction(RoutedEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method LoadedAction....", category: Category.Info, priority: Priority.Low);
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocationsRefillUserControlView", null, this);
            WinUIDialogWindow detailView = ((WinUIDialogWindow)((System.Windows.RoutedEventArgs)obj).OriginalSource);
            detailView.Focus();
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method LoadedAction....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void CommandCancelAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method CommandCancelAction....", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
            StopThread();
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method CommandCancelAction....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        private void StopThread()
        {
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method StopThread....", category: Category.Info, priority: Priority.Low);
            //  GC.SuppressFinalize(FromRefillThread);
            //FromRefillThread.Abort();
            GeosApplication.Instance.Logger.Log("LocationsRefillViewModel Method StopThread....executed successfully", category: Category.Info, priority: Priority.Low);
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
                ProgressbarLocationStock = WarehouseService.GetArticleStockByWarehouseLocation(SelectedRefillMaterials.IdArticle, LocationToRefill.IdWarehouseLocation, SelectedRefillMaterials.IdWarehouse);
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
                ProgressbarArticleStock = WarehouseService.GetAVGStockByIdArticle_V2034(SelectedRefillMaterials.IdArticle, WarehouseCommon.Instance.Selectedwarehouse);
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

                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;

                TransferMaterials article = null;
                if (obj is TransferMaterials)
                {
                    article = (TransferMaterials)obj;
                    articleDetailsViewModel.Init(article.Reference, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                }
                else if (obj is Article)
                {
                    Article SharedArticle = (Article)obj;
                    articleDetailsViewModel.Init(SharedArticle.Reference, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                }

                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                articleDetailsView.ShowDialog();

                if (articleDetailsViewModel.IsResult && article != null)
                {
                    if (articleDetailsViewModel.UpdateArticle.IsAddedArticleImage || articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage)
                    {
                        foreach (var item in MainRefillMaterialsList)
                        {
                            if (item.IdArticle == article.IdArticle)
                                item.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        }

                        if (SelectedRefillMaterials != null)
                        {
                            if (SelectedRefillMaterials.IdArticle == article.IdArticle)
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

        private void BackButtonCommandAction(object obj)
        {
            if (IsBackButtonEnable)
            {
                InitBack();
                Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocationsRefillUserControlView", null, this);
            }
        }

        private void GoToNextItem(TransferMaterials currentTransferMaterial)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GoToNextItem....", category: Category.Info, priority: Priority.Low);
                int currentTransferMaterialIndex = MainRefillMaterialsList.IndexOf(currentTransferMaterial);
                this.currentTransferMaterial = currentTransferMaterial;
                if (currentTransferMaterialIndex + 1 < MainRefillMaterialsList.Count)
                {
                    SelectedRefillMaterials = MainRefillMaterialsList[currentTransferMaterialIndex + 1];
                    currentTransferMaterialIndex++;
                    ArticleImage = ByteArrayToImage(SelectedRefillMaterials.ArticleImageInBytes);
                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedRefillMaterials.IdArticle, Convert.ToInt32(SelectedRefillMaterials.IdWarehouse));
                    ChangeTotalStockColor(TotalCurrentItemStock, SelectedRefillMaterials.Article.MyWarehouse.MinimumStock);
                    UpdateProgressbarLocationStock();
                    UpdateProgressbarArticleStock();
                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, LocationToRefill.MinimumStock);

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

                    if (currentTransferMaterialIndex + 1 == MainRefillMaterialsList.Count)
                    {
                        if (RefillList.Count == 1)
                            IsSkipButtonEnable = false;
                    }
                }
                else
                {
                    if (IsSkipButtonEnable)
                    {
                        Init(locationIndex);
                        Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocationsRefillUserControlView", null, this);
                    }
                }
                FocusUserControl = true;
                GeosApplication.Instance.Logger.Log("Method GoToNextItem....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method GoToNextItem...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][avpawar][15-06-2021][GEOS2-3097]Allow allocate Serial Numbers in different locations - Refill
        /// </summary>
        private void SetBackButtonEnable()
        {
            try
            {
                int tempLoctionIndex = tempLocationsToRefillList.IndexOf(RefillList[0].LocationToRefill);
                for (int index = tempLoctionIndex - 1; index >= 0; index--)
                {
                    tempLoctionIndex--;
                    LocationRefill toRefill = tempLocationsToRefillList[index];
                    // [001] Changed Service method
                    LocationRefill fromRefill = WarehouseService.GetArticleWarehouseLocation_V2090(WarehouseCommon.Instance.Selectedwarehouse, toRefill.IdArticle, toRefill.Position);

                    if (fromRefill.FullName == null)
                        continue;

                    if (RefillList.Any(x => x.LocationFromRefill.FullName == fromRefill.FullName && x.LocationToRefill.FullName == toRefill.FullName))
                    {
                        continue;
                    }
                    // [001] Changed Service method
                    //List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2034(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                    // [002] Changed Service method
                    List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2160(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                    if (referenceList.Count > 0)
                    {
                        IsBackButtonEnable = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetSkipButtonEnable...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][avpawar][15-06-2021][GEOS2-3097]Allow allocate Serial Numbers in different locations - Refill
        /// </summary>

        private void SetSkipButtonEnable()
        {
            try
            {
                int tempLoctionIndex = tempLocationsToRefillList.IndexOf(RefillList[0].LocationToRefill);
                for (int index = tempLoctionIndex + 1; index < tempLocationsToRefillList.Count; index++)
                {

                    tempLoctionIndex++;

                    LocationRefill toRefill = tempLocationsToRefillList[index];
                    // [001] Changed Service method
                    LocationRefill fromRefill = WarehouseService.GetArticleWarehouseLocation_V2090(WarehouseCommon.Instance.Selectedwarehouse, toRefill.IdArticle, toRefill.Position);

                    if (fromRefill.FullName == null)
                        continue;

                    if (RefillList.Any(x => x.LocationFromRefill.FullName == fromRefill.FullName && x.LocationToRefill.FullName == toRefill.FullName))
                    {
                        continue;
                    }
                    // [001] Changed Service method
                    //List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2034(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                    // [002] Changed Service method
                    List<TransferMaterials> referenceList = WarehouseService.GetRefillMaterialDetails_V2160(fromRefill.FullName, toRefill.FullName, WarehouseCommon.Instance.Selectedwarehouse, toRefill);

                    if (referenceList.Count > 0)
                    {
                        IsSkipButtonEnable = true;
                        break;
                    }

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetSkipButtonEnable...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VectorLayerDataLoaded(object obj)
        {
            MapControl mapControl = obj as MapControl;

            if (mapControl != null)
                mapControl.ZoomToFitLayerItems();
        }

        public void PreviewMouseDownCommandAction(object e)
        {
            if (e is MouseButtonEventArgs)
            {
                MouseButtonEventArgs mbea = e as MouseButtonEventArgs;
                GridControl gridcontrol = (GridControl)((TableView)mbea.Source).DataControl;
                var info = ((TableView)(mbea.Source)).CalcHitInfo((DependencyObject)mbea.OriginalSource);
                if (info.InRowCell && info.Column.Name == "RefillAnotherLocation" && (Boolean)gridcontrol.GetCellValue(info.RowHandle, "IsArticleInRefillExistInAnotherLocation"))
                    CustomMessageBox.Show(Application.Current.FindResource("RefillAnotherLocation").ToString(), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
            }
        }

        #endregion
    }


    public class Refill
    {
        LocationRefill locationToRefill = new LocationRefill();
        public LocationRefill LocationToRefill
        {
            get { return locationToRefill; }
            set { locationToRefill = value; }
        }

        LocationRefill locationFromRefill = new LocationRefill();
        public LocationRefill LocationFromRefill
        {
            get { return locationFromRefill; }
            set { locationFromRefill = value; }
        }

        List<TransferMaterials> referenceList = new List<TransferMaterials>();

        public List<TransferMaterials> ReferenceList
        {
            get { return referenceList; }
            set { referenceList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationToRefill">ToRefill Object</param>
        /// <param name="locationFromRefill">FromRefill object</param>
        /// <param name="referenceList">referenceList</param>
        public Refill(LocationRefill locationToRefill, LocationRefill locationFromRefill, List<TransferMaterials> referenceList)
        {
            this.LocationFromRefill = locationFromRefill;
            this.LocationToRefill = locationToRefill;
            this.ReferenceList = referenceList;
        }

        public Refill()
        {
        }

    }
}

