using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
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
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class RefundWorkOrderViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Services
       
      IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }

        #endregion //End Of Services

        #region Declaration

        private int windowHeight;
        private int trackBarEditvalue;
        private string trackBarFgColor;
        private string fgColorLocation;
        private string bgColorLocation;
        private string locationName;
        private string wrongLocation;
        private string wrongItem;
        private Visibility isWrongLocationErrorVisible = Visibility.Hidden;
        private Visibility isWrongItemErrorVisible = Visibility.Hidden;
        private Visibility isInformationVisible;
        private ImageSource articleImage;
        private string totalStockBgColor;
        private string totalStockFgColor;
        private string onFocusBgColor;
        private string onFocusFgColor;
        private Uri svgUri;
        bool isLastLocation;
        private bool isLocationScaned;
        private bool isSkipButtonEnable;
        private string barcodeStr;
        private bool isBackButtonEnable;
        private bool isScanSerialNumber;
        private string materialSerialNumbers;
        private SerialNumber selectedSerialNumber;
        private Int64 totalCurrentItemStock;
        int currentLocationIndex = 0;
        int indexItem = 0;
        int countLocation = 0;
        Visibility isProducerVisible = Visibility.Collapsed;

        private List<PickingMaterials> pickingMaterialsList;
        List<PickingMaterials> materialSoredList = new List<PickingMaterials>();
        private ObservableCollection<WarehouseLocation> mapItems;
        private List<WarehouseLocation> warehouseLocations;
        private List<string> articleLocationList;
        private List<PickingMaterials> referenceArticlesList;
        private PickingMaterials selectedPickingMaterial;
        private PickingMaterials currentPickingMaterial;
        enum LocationIndex { NextLocation, PreviousLocation }
        private string locationStockBgColor;
        private string locationStockFgColor;
        private ArticleWarehouseLocations progressbarArticleStock;
        private ArticleWarehouseLocations progressbarLocationStock;
        private long BarcodeScannedQuantity;
        private bool focusUserControl;
        private List<ArticleWarehouseLocations> allPossibleLocationOfArticleList;
        private List<ArticleWarehouseLocations> selectedLocation;
        private bool isScannedLocationPresentinArticleLocations;
        private string idArticle;
        private string articleReferenceForPopUpValue;
        private string scannedQuantityForPopUpValue;
        private PickingMaterials selectedPickingMaterialClone;
        private PickingMaterials selectedRefundMaterial;
        private bool isDeductFromRefund;
        private bool isScannedLocationOnceChecked;
        private bool isCancelForNonSerialNumber;
        private bool isCancelForSerialNumber;

        //private TransferMaterials selectedRefundMaterialForTransfer;

        #endregion

        #region Public Properties

        public bool FocusUserControl
        {
            get { return focusUserControl; }
            set
            {
                focusUserControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FocusUserControl"));
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

        public string BgColorLocation
        {
            get { return bgColorLocation; }
            set
            {
                bgColorLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BgColorLocation"));
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

        public Visibility IsInformationVisible
        {
            get { return isInformationVisible; }
            set
            {
                isInformationVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInformationVisible"));
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
                    ArticleImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Warehouse;component/Assets/Images/ImageEditLogo.png"));
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleImage"));
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

        public Uri SvgUri
        {
            get { return svgUri; }
            set
            {
                svgUri = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SvgUri"));
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

        public bool IsSkipButtonEnable
        {
            get { return isSkipButtonEnable; }
            set
            {
                isSkipButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSkipButtonEnable"));
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

        public bool IsBackButtonEnable
        {
            get { return isBackButtonEnable; }
            set
            {
                isBackButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBackButtonEnable"));
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

        public SerialNumber SelectedSerialNumber
        {
            get { return selectedSerialNumber; }
            set
            {
                selectedSerialNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSerialNumber"));
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

        public Visibility IsProducerVisible
        {
            get { return isProducerVisible; }
            set
            {
                isProducerVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsProducerVisible"));
            }
        }

        public List<PickingMaterials> PickingMaterialsList
        {
            get { return pickingMaterialsList; }
            set
            {
                pickingMaterialsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PickingMaterialsList"));
            }
        }

        public Ots PickingOt { get; set; }

        public ObservableCollection<WarehouseLocation> MapItems
        {
            get { return mapItems; }
            set
            {
                mapItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MapItems"));
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

        public List<string> ArticleLocationList
        {
            get { return articleLocationList; }
            set
            {
                articleLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleLocationList"));
            }
        }

        public List<PickingMaterials> ReferenceArticlesList
        {
            get { return referenceArticlesList; }
            set
            {
                referenceArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReferenceArticlesList"));
            }
        }

        public PickingMaterials SelectedPickingMaterial
        {
            get { return selectedPickingMaterial; }
            set
            {
                selectedPickingMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPickingMaterial"));
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

        public List<ArticleWarehouseLocations> AllPossibleLocationOfArticleList
        {
            get { return allPossibleLocationOfArticleList; }
            set
            {
                allPossibleLocationOfArticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPossibleLocationOfArticleList"));
            }
        }

        public List<ArticleWarehouseLocations> SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                selectedLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocation"));
            }
        }

        public bool IsScannedLocationPresentinArticleLocations
        {
            get { return isScannedLocationPresentinArticleLocations; }
            set
            {
                isScannedLocationPresentinArticleLocations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsScannedLocationPresentinArticleLocations"));
            }
        }

        public string IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdArticle"));
            }
        }

        public string ArticleReferenceForPopUpValue
        {
            get { return articleReferenceForPopUpValue; }
            set
            {
                articleReferenceForPopUpValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleReferenceForPopUpValue"));
            }
        }

        public string ScannedQuantityForPopUpValue
        {
            get { return scannedQuantityForPopUpValue; }
            set
            {
                scannedQuantityForPopUpValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScannedQuantityForPopUpValue"));
            }
        }

        public PickingMaterials SelectedPickingMaterialClone
        {
            get { return selectedPickingMaterialClone; }
            set
            {
                selectedPickingMaterialClone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPickingMaterialClone"));
            }
        }

        public PickingMaterials SelectedRefundMaterial
        {
            get { return selectedRefundMaterial; }
            set
            {
                selectedRefundMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRefundMaterial"));
            }
        }

        public bool IsDeductFromRefund
        {
            get { return isDeductFromRefund; }
            set
            {
                isDeductFromRefund = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeductFromRefund"));
            }
        }

        public bool IsScannedLocationOnceChecked
        {
            get { return isScannedLocationOnceChecked; }
            set
            {
                isScannedLocationOnceChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsScannedLocationOnceChecked"));
            }
        }

        public bool IsCancelForNonSerialNumber
        {
            get { return isCancelForNonSerialNumber; }
            set
            {
                isCancelForNonSerialNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancelForNonSerialNumber"));
            }
        }

        public bool IsCancelForSerialNumber
        {
            get { return isCancelForSerialNumber; }
            set
            {
                isCancelForSerialNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancelForSerialNumber"));
            }
        }

        //public TransferMaterials SelectedRefundMaterialForTransfer
        //{
        //    get { return selectedRefundMaterialForTransfer; }
        //    set
        //    {
        //        selectedRefundMaterialForTransfer = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedRefundMaterialForTransfer"));
        //    }
        //}
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

        #region Commands

        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandOnLoaded { get; set; }
        public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }
        public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        public ICommand HyperlinkClickCommand { get; set; }
        public ICommand CommandSkipButton { get; set; }
        public ICommand CommandBackButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }

        #endregion

        #region Constructor

        public RefundWorkOrderViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor RefundWorkOrderViewModel ...", category: Category.Info, priority: Priority.Low);

                CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
                CommandOnLoaded = new DelegateCommand<RoutedEventArgs>(LoadedAction);
                VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
                CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);
                HyperlinkClickCommand = new DelegateCommand<object>(HyperlinkClickCommandAction);
                CommandSkipButton = new DelegateCommand<object>(SkipButtonCommandAction);
                CommandBackButton = new DelegateCommand<object>(BackButtonCommandAction);
                CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);

                WindowHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 130;
                IsSkipButtonEnable = true;
                IsBackButtonEnable = false;
                TrackBarEditvalue = 0;
                IsInformationVisible = Visibility.Collapsed;
                IsLocationScaned = false;
                GeosApplication.Instance.Logger.Log("Constructor RefundWorkOrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor RefundWorkOrderViewModel() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        private void CommandCancelAction(object obj)
        {
            try
            {
                RequestClose(null, null);
                string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                if (Directory.Exists(basePath))
                {
                    Directory.Delete(basePath, true);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in CommandCancelAction(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for forcus form when it  load.
        /// </summary>
        /// <param name="obj"></param>
        private void LoadedAction(RoutedEventArgs obj)
        {
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.RefundWorkOrderViewUserControlView", null, this);
            WinUIDialogWindow detailView = ((WinUIDialogWindow)((System.Windows.RoutedEventArgs)obj).OriginalSource);
            detailView.Focus();
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
                    if (articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage || articleDetailsViewModel.UpdateArticle.IsAddedArticleImage)
                    {
                        foreach (var item in materialSoredList)
                        {
                            if (item.IdArticle == article.IdArticle)
                                item.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        }

                        if (SelectedPickingMaterial != null)
                        {
                            if (article.IdArticle == SelectedPickingMaterial.IdArticle)
                                ArticleImage = ByteArrayToImage(articleDetailsViewModel.UpdateArticle.ArticleImageInBytes);
                        }
                    }

                    if (articleDetailsViewModel.UpdateArticle.LstArticleWarehouseLocations.Count > 0)
                    {
                        if (SelectedPickingMaterial != null)
                        {
                            if (SelectedPickingMaterial.IdArticle == article.IdArticle)
                                UpdateProgressbarLocationStock();
                        }
                    }
                }

                FocusUserControl = true;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
            try
            {
                GeosApplication.Instance.Logger.Log("Method BackButtonCommandAction....", category: Category.Info, priority: Priority.Low);
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
                    PickingMaterials pm = (PickingMaterials)SelectedSerialNumber.MasterItem;
                    pm.ScanSerialNumbers.Remove(pm.ScanSerialNumbers.FirstOrDefault(x => x.Code == null));
                    SelectedSerialNumber = null;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method BackButtonCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method BackButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SkipButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SkipButtonCommandAction....", category: Category.Info, priority: Priority.Low);
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
                    PickingMaterials pm = SelectedSerialNumber.MasterItem as PickingMaterials;
                    //pm.ScanSerialNumbers = new List<SerialNumber>();
                    //pm.DownloadQuantity = pm.SerialNumbers.Count;
                    pm.ScanSerialNumbers.Remove(pm.ScanSerialNumbers.FirstOrDefault(x => x.Code == null));
                    SelectedSerialNumber = null;

                    GoToNextItem(pm);
                }
                else
                {
                    if (SelectedPickingMaterial == null && SelectedSerialNumber != null)
                    {
                        PickingMaterials pm = SelectedSerialNumber.MasterItem as PickingMaterials;
                        GoToNextItem(pm);
                    }
                    else
                    {
                        GoToNextItem(SelectedPickingMaterial);
                        if (SelectedPickingMaterial != null)
                            ChangeTotalStockColor(TotalCurrentItemStock, SelectedPickingMaterial.MinimumStock);
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SkipButtonCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SkipButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Set Next Button Enable
        /// </summary>
        public void SetSkipButtonEnable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("RefundWorkOrderViewModel Method SetNextButtonEnable....", category: Category.Info, priority: Priority.Low);
                for (int item = currentLocationIndex + 1; item <= ArticleLocationList.Count - 1; item++)
                {
                    List<PickingMaterials> material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
                    if (material.Any(x => x.DownloadQuantity > 0))
                    {
                        IsSkipButtonEnable = true;
                        break;
                    }
                    else
                    {
                        IsSkipButtonEnable = false;
                    }
                }

                if (currentLocationIndex >= ArticleLocationList.Count - 1)
                {
                    IsSkipButtonEnable = false;
                }
                GeosApplication.Instance.Logger.Log("RefundWorkOrderViewModel Method SetNextButtonEnable........executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefundWorkOrderViewModel Method SetNextButtonEnable...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Set Back Button Enable
        /// </summary>
        public void SetBackButtonEnable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("RefundWorkOrderViewModel Method SetBackButtonEnable....", category: Category.Info, priority: Priority.Low);

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
                GeosApplication.Instance.Logger.Log("RefundWorkOrderViewModel Method SetBackButtonEnable........executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefundWorkOrderViewModel Method SetBackButtonEnable...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for change Total stock color as per condition.
        /// </summary>
        /// <param name="totalStock"></param>
        /// <param name="minQuantity"></param>
        private void ChangeTotalStockColor(Int64 totalStock, Int64 minQuantity)
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

        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
                if (obj.Text == "\r" && !isLastLocation)
                {
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
                            OnFocusBgColor = "#FFFF0000";   // color = cream Red
                            OnFocusFgColor = "#FFFFFFFF";   // color = cream White
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }
                        else
                        {
                            if (pm.SerialNumbers.Exists(x => x.Code == BarcodeStr) && SelectedSerialNumber.Code == null)     //Add as scanned in serial numbers if true
                            {
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                SelectedSerialNumber.Code = BarcodeStr;
                                SerialNumber sn = (SerialNumber)pm.SerialNumbers.FirstOrDefault(x => x.Code == BarcodeStr).Clone();
                                //assign all values to scanned serial number.
                                SelectedSerialNumber.IdSerialNumber = sn.IdSerialNumber;
                                SelectedSerialNumber.IdArticle = sn.IdArticle;
                                SelectedSerialNumber.IdWarehouseDeliveryNoteItem = sn.IdWarehouseDeliveryNoteItem;
                                SelectedSerialNumber.IdWarehouse = sn.IdWarehouse;
                                //SelectedSerialNumber.IdWarehouseLocation = SelectedPickingMaterialClone.IdWarehouseLocation;

                                OtItem otitem = PickingOt.OtItems.FirstOrDefault(x => x.IdOTItem == pm.IdOtitem);
                                SelectedSerialNumber.IdWarehouseProduct = null; // otitem.RevisionItem.WarehouseProduct.IdWarehouseProduct; // sn.IdWarehouseProduct;
                                SelectedSerialNumber.IsScanned = true;

                                //Prepare Observation.
                                //if (pm.Observation == null)
                                //{
                                pm.Observation = new Observation();
                                pm.Observation.IdRevisionItem = pm.IdRevisionItem;
                                pm.Observation.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                pm.Observation.Text = String.Format(Application.Current.FindResource("MaterialSerialNumbers").ToString() + "\r\n", pm.Reference);
                                //}

                                pm.Observation.Text += string.Format("\t* {0} ; {1}\r", SelectedSerialNumber.Code, SelectedSerialNumber.ExtraCode);
                                pm.DownloadQuantity -= 1;
                                BarcodeScannedQuantity -= 1;

                                //if (pm.DownloadQuantity > 0)
                                //{
                                //    SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = pm };
                                //    pm.ScanSerialNumbers.Add(scanSerialNumber);
                                //    SelectedSerialNumber = scanSerialNumber;
                                //    IsScanSerialNumber = true;
                                //    BarcodeStr = string.Empty;
                                //}
                                //else if (pm.DownloadQuantity == 0)
                                //{

                                int quantity = 1; // pm.ScanSerialNumbers.Count;
                                pm.ScannedQty = quantity;

                                TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(pm.IdArticle, Convert.ToInt32(pm.IdWarehouse));
                                TotalCurrentItemStock += quantity;

                                if (PickingOt != null && PickingOt.Quotation != null && PickingOt.Quotation.Site != null && PickingOt.OtItems != null && PickingOt.OtItems.Count > 0)
                                {
                                    OtItem oti = PickingOt.OtItems.FirstOrDefault(x => x.IdOTItem == pm.IdOtitem);
                                    pm.Comments = PickingOt.Code + " (" + PickingOt.Quotation.Site.Name + ") -> Item " + oti.RevisionItem.NumItem + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                                }

                                pm.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;

                                if (pm.IdWarehouseProductComponent == 0)
                                    pm.IdWarehouseProductComponent = null;

                                //[GEOS2-1957]
                                bool IsScanItem = false;
                                //pm.ScanSerialNumbers
                                if (pm.ScannedQty != 0 && IsScannedLocationPresentinArticleLocations == true) // && IsScannedLocationPresentinArticleLocations == true)
                                {
                                    foreach (var TEMP in pm.ScanSerialNumbers)
                                    {
                                        TEMP.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        TEMP.IdWarehouse = Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                                        TEMP.IdWarehouseLocation = pm.IdWarehouseLocation;
                                    }
                                    IsScanItem = WarehouseService.InsertIntoArticleStockForRefund_V2170(pm); //InsertIntoArticleStockForRefund_V2170
                                }

                                if (pm.ScannedQty != 0 && IsScannedLocationPresentinArticleLocations == false)
                                {

                                    //}
                                    //else
                                    //{                                   
                                    bool isNext = CustomMessageBox.Show(string.Format(Application.Current.Resources["NewLocationWarningMessage"].ToString(), LocationName, ArticleReferenceForPopUpValue, ScannedQuantityForPopUpValue), "Yellow");

                                    if (isNext)
                                    {
                                        AllPossibleLocationOfArticleList = WarehouseService.GetAllPossibleLocationOfArticle(IdArticle, WarehouseCommon.Instance.Selectedwarehouse); //WarehouseCommon.Instance.Selectedwarehouse).Where(al => !al.FullName.Equals(LocationName)).OrderBy(aw => aw.FullName).ToList();
                                        //AllPossibleLocationOfArticleList.AddRange(AllPossibleLocationOfArticleList);
                                        if (AllPossibleLocationOfArticleList.Count > 1)
                                        {
                                            NewLocationView newLocationView = new NewLocationView();
                                            NewLocationViewModel newLocationViewModel = new NewLocationViewModel();
                                            EventHandler handler = delegate { newLocationView.Close(); };
                                            newLocationView.DataContext = newLocationViewModel;
                                            newLocationViewModel.RequestClose += handler;
                                            //pm.ScannedQty = (-1) * pm.ScannedQty;
                                            if (pm.ScanSerialNumbers.Count == 0)
                                            {
                                                pm.ScanSerialNumbers.Add(SelectedSerialNumber); //= pm.SerialNumbers.FirstOrDefault();
                                            }
                                            newLocationViewModel.Init(pm, AllPossibleLocationOfArticleList, IsScanSerialNumber, pm.ScannedQty, LocationName, TotalCurrentItemStock, BarcodeStr);
                                            newLocationView.ShowDialogWindow();
                                            if (newLocationViewModel.IsSave == true)
                                            {
                                                IsScanItem = true;
                                            }
                                            else
                                            {
                                                IsScanItem = false;
                                            }

                                            if (newLocationViewModel.IsBackButtonPressed == true)
                                            {
                                                pm.DownloadQuantity = pm.DownloadQuantity + quantity;
                                                pm.RevisionItem.Quantity = pm.RevisionItem.Quantity + quantity;
                                                newLocationViewModel.IsBackButtonPressed = false;
                                                SelectedSerialNumber = new SerialNumber();
                                                SelectedSerialNumber.MasterItem = pm;
                                                //SerialNumber ScanSerialNumberClone = new SerialNumber();
                                                //ScanSerialNumberClone = pm.ScanSerialNumbers.FirstOrDefault();
                                                pm.ScanSerialNumbers = new List<SerialNumber>();
                                                // BarcodeScannedQuantity = (-1) * (BarcodeScannedQuantity + 1) ;
                                                BarcodeScannedQuantity = BarcodeScannedQuantity + 1;
                                                //BarcodeStr = String.Empty;
                                            }

                                        }
                                        else if (AllPossibleLocationOfArticleList.Count == 0)
                                        {
                                            MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(string.Format(Application.Current.Resources["AddNewLocationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                            pm.DownloadQuantity = pm.DownloadQuantity + quantity;
                                            pm.RevisionItem.Quantity = pm.RevisionItem.Quantity + quantity;
                                            return;
                                        }
                                        else
                                        {
                                            //refund entry
                                            IsScanItem = WarehouseService.InsertIntoArticleStockForRefund_V2170(pm);

                                            TransferMaterials SelectedRefundMaterialForTransfer = new TransferMaterials();
                                            SelectedRefundMaterialForTransfer.IdArticle = pm.IdArticle;
                                            SelectedRefundMaterialForTransfer.CostPrice = pm.CostPrice;
                                            SelectedRefundMaterialForTransfer.UnitPrice = pm.UnitPrice;
                                            SelectedRefundMaterialForTransfer.IdWarehouseLocation = pm.IdWarehouseLocation;
                                            SelectedRefundMaterialForTransfer.IdWarehouse = pm.IdWarehouse;
                                            SelectedRefundMaterialForTransfer.IdWareHouseDeliveryNoteItem = pm.IdWareHouseDeliveryNoteItem;
                                            SelectedRefundMaterialForTransfer.ScannedQty = (-1) * pm.ScannedQty;
                                            SelectedRefundMaterialForTransfer.Comments = "Transfer from " + "\"" + LocationName + "\"" + " location to " + "\"" + AllPossibleLocationOfArticleList[0].FullName + "\"" + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                                            SelectedRefundMaterialForTransfer.ModifiedBy = pm.ModifiedBy;
                                            SelectedRefundMaterialForTransfer.IdCurrency = pm.IdCurrency;
                                            //if (IsScanSerialNumber == true)
                                            //{
                                            //    foreach (var temp in pm.ScanSerialNumbers)
                                            //    {
                                            //        //temp.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            //        temp.IdWarehouseLocation = pm.IdWarehouseLocation;
                                            //        temp.IdWarehouse = Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                                            //    }
                                            //    SelectedRefundMaterialForTransfer.ScanSerialNumbers = pm.ScanSerialNumbers;
                                            //}

                                            Boolean IsTransferFromOldLocation = false;
                                            IsTransferFromOldLocation = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(SelectedRefundMaterialForTransfer);

                                            SelectedRefundMaterialForTransfer.ScannedQty = (1) * pm.ScannedQty;
                                            SelectedRefundMaterialForTransfer.IdWarehouseLocation = AllPossibleLocationOfArticleList[0].IdWarehouseLocation;

                                            if (IsScanSerialNumber == true)
                                            {
                                                if (pm.ScanSerialNumbers.Count == 0)
                                                {
                                                    pm.ScanSerialNumbers.Add(SelectedSerialNumber);
                                                    //pm.ScanSerialNumbers.ToList().ForEach(SS => SS.MasterItem = null);
                                                }
                                                foreach (var temp in pm.ScanSerialNumbers)
                                                {
                                                    //temp.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                    temp.IdWarehouseLocation = AllPossibleLocationOfArticleList[0].IdWarehouseLocation;
                                                    temp.IdWarehouse = Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                                                }
                                                SelectedRefundMaterialForTransfer.ScanSerialNumbers = pm.ScanSerialNumbers;
                                                SelectedRefundMaterialForTransfer.ScanSerialNumbers.ToList().ForEach(SS => SS.MasterItem = null);
                                            }
                                            SelectedRefundMaterialForTransfer.RegisterSerialNumber = pm.RegisterSerialNumber;
                                            //SelectedRefundMaterialForTransfer.SerialNumbers = pm.SerialNumbers;
                                            //SelectedRefundMaterialForTransfer.SerialNumbers.ToList().ForEach(SN => SN.IsScanned = true);
                                            // pm.ScanSerialNumbers.ToList().ForEach(xx => xx.MasterItem = pm); //new List<SerialNumber>(pm.ScanSerialNumbers);
                                            Boolean IsTransferToNewLocation = false;
                                            IsTransferToNewLocation = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(SelectedRefundMaterialForTransfer);

                                            //if (IsScanSerialNumber == true)
                                            //{
                                            //    foreach (var temp in SelectedRefundMaterial.ScanSerialNumbers)
                                            //    {
                                            //        temp.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            //        temp.IdWarehouseLocation = pm.IdWarehouseLocation;
                                            //        temp.IdWarehouse = Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                                            //    }
                                            //}
                                            //IsDeductFromRefund = WarehouseService.InsertIntoArticleStockForRefund_V2170(SelectedRefundMaterial);

                                            //SelectedRefundMaterial = pm;
                                            //SelectedRefundMaterial.ScannedQty = (-1) * pm.ScannedQty;
                                            //if (IsScanSerialNumber == true)
                                            //{
                                            //    foreach (var temp in SelectedRefundMaterial.ScanSerialNumbers)
                                            //    {
                                            //        temp.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            //        temp.IdWarehouseLocation = pm.IdWarehouseLocation;
                                            //        temp.IdWarehouse = Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                                            //    }
                                            //}
                                            //IsDeductFromRefund = WarehouseService.InsertIntoArticleStockForRefund_V2170(SelectedRefundMaterial);

                                            //pm.IdWarehouseLocation = AllPossibleLocationOfArticleList[0].IdWarehouseLocation;
                                            //pm.ScannedQty = (-1) * pm.ScannedQty;
                                            //if (IsScanSerialNumber == true)
                                            //{
                                            //    foreach (var temp in pm.ScanSerialNumbers)
                                            //    {
                                            //        temp.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            //        temp.IdWarehouseLocation = pm.IdWarehouseLocation;
                                            //        temp.IdWarehouse = Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                                            //    }
                                            //}
                                            //IsScanItem = WarehouseService.InsertIntoArticleStockForRefund_V2170(pm); //InsertIntoArticleStockForRefund_V2170
                                        }
                                    }

                                    else
                                    {
                                        pm.DownloadQuantity = pm.DownloadQuantity + quantity;
                                        pm.RevisionItem.Quantity = pm.RevisionItem.Quantity + quantity;
                                        SelectedSerialNumber = new SerialNumber();
                                        SelectedSerialNumber.MasterItem = pm;
                                        pm.ScanSerialNumbers = new List<SerialNumber>();
                                       SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = pm };
                                        pm.ScanSerialNumbers.Add(scanSerialNumber);
                                        SelectedSerialNumber = scanSerialNumber;
                                        IsScanSerialNumber = true;
                                        BarcodeStr = string.Empty;
                                        BarcodeScannedQuantity = BarcodeScannedQuantity + 1;
                                        //BarcodeStr = String.Empty;
                                        //IsLocationScaned = false;
                                        //BarcodeStr = LocationName;
                                        //BgColorLocation = "#FF008000";
                                        //IsLocationScaned = true;
                                        //IsWrongLocationErrorVisible = Visibility.Hidden;
                                        //WrongLocation = "";
                                        return;
                                    }

                                }

                                if (IsScanItem)
                                {
                                    ChangeTotalStockColor(TotalCurrentItemStock, pm.MinimumStock);
                                    pm.SerialNumbers.Remove(pm.SerialNumbers.FirstOrDefault(x => x.IdSerialNumber == sn.IdSerialNumber));
                                    bool isUpdatedComments = WarehouseService.UpdateRevisionItemCommentsForRefund(WarehouseCommon.Instance.Selectedwarehouse, pm.IdRevisionItem, SelectedSerialNumber.IdWarehouseDeliveryNoteItem);
                                    bool isRevisionItemstage = WarehouseService.UpdateItemStatusAndStageForRefund(WarehouseCommon.Instance.Selectedwarehouse, pm.IdOtitem, 1, GeosApplication.Instance.ActiveUser.IdUser);
                                }

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
                                    indexItem = ReferenceArticlesList.IndexOf(pm);
                                    indexItem = indexItem + 1;

                                    if (indexItem < ReferenceArticlesList.Count)
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
                                        SkipButtonCommandAction(null);
                                        SelectedPickingMaterial = null;
                                        BarcodeStr = string.Empty;
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
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            }
                        }
                    }
                    else
                    {
                        var isProperBarcode = !string.IsNullOrEmpty(BarcodeStr) && BarcodeStr.All(Char.IsDigit);
                        if (BarcodeStr.Length < 11)
                        {
                            isProperBarcode = false;
                        }

                        if (isProperBarcode)
                        {
                            //Allow to scan barcode if the idwarehouse belongs to same site then application selected one.
                            //In the articlesstock must be inserted the application selected warehouse.
                            bool IsWarehouseBelongsToSameSite = false;

                            Int64 _idwarehouse = Convert.ToInt64(BarcodeStr.Substring(0, 3));
                            Int64 _idWareHouseDeliveryNoteItem = Convert.ToInt64(BarcodeStr.Substring(3, 8));
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
                                && PickingOt.ProducerIdCountryGroup != 0 && SelectedPickingMaterial.IdCountryGroup != 0
                                && PickingOt.ProducerIdCountryGroup != SelectedPickingMaterial.IdCountryGroup)
                            {
                                isSameProducer = false;
                            }

                            if (IsWarehouseBelongsToSameSite && isSameProducer
                                && SelectedPickingMaterial.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem
                                && SelectedPickingMaterial.DownloadQuantity >= BarcodeScannedQuantity && BarcodeScannedQuantity > 0)
                            {
                                if (SelectedPickingMaterial.DownloadQuantity >= BarcodeScannedQuantity)
                                {
                                    if (SelectedPickingMaterial.RegisterSerialNumber == 1)
                                    {
                                        //{GEOS2-1957]
                                        List<Int64> IdArticles = new List<Int64>();
                                        foreach (var item in ReferenceArticlesList)
                                        {
                                            if (!IdArticles.Contains(item.IdArticle))
                                            {
                                                IdArticles.Add(item.IdArticle);
                                            }
                                        }

                                        IdArticles.Sort();
                                        IdArticle = string.Join(",", IdArticles.Select(n => n.ToString()).ToArray());

                                        if (IsScannedLocationOnceChecked == false)
                                        {
                                            IsScannedLocationPresentinArticleLocations = WarehouseService.IsIdWarehouseLocationPresent(WarehouseCommon.Instance.Selectedwarehouse, IdArticle, SelectedPickingMaterial);
                                            IsScannedLocationOnceChecked = true;
                                        }
                                        ArticleReferenceForPopUpValue = SelectedPickingMaterial.Reference;
                                        ScannedQuantityForPopUpValue = BarcodeScannedQuantity.ToString();
                                        SelectedPickingMaterialClone = SelectedPickingMaterial;
                                        if (SelectedPickingMaterial.ScanSerialNumbers == null)
                                            SelectedPickingMaterial.ScanSerialNumbers = new List<SerialNumber>();

                                        SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = SelectedPickingMaterial };
                                        SelectedPickingMaterial.ScanSerialNumbers.Add(scanSerialNumber);
                                        SelectedSerialNumber = scanSerialNumber;
                                        IsScanSerialNumber = true;
                                        BarcodeStr = string.Empty;
                                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                        return;
                                    }

                                    //List<Int64> IdArticles = new List<Int64>();
                                    //foreach (var item in ReferenceArticlesList)
                                    //{
                                    //    if (!IdArticles.Contains(item.IdArticle))
                                    //    {
                                    //        IdArticles.Add(item.IdArticle);
                                    //    }
                                    //}

                                    //IdArticles.Sort();
                                    //string IdArticlesStr = string.Join(",", IdArticles.Select(n => n.ToString()).ToArray());
                                    //ArticleWarehouseLocationsList = WarehouseService.GetArticlesWarehouseLocationTransfer_V2033(IdArticlesStr, WarehouseCommon.Instance.Selectedwarehouse).Where(al => !al.FullName.Equals(LocationName)).OrderBy(aw => aw.FullName).ToList();
                                    ////ArticleWarehouseLocationsList = WarehouseService.GetArticleWarehouseLocationByIdArticle(WarehouseCommon.Instance.Selectedwarehouse, 6464);
                                    //ArticleWarehouseLocationsList.RemoveAll(al => al.FullName == "TRANSIT");
                                    //if (!ArticleWarehouseLocationsList.Any(al => al.FullName == LocationName))
                                    //{
                                    //    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["NewLocationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OKCancel);
                                    //    if (MessageBoxResult == MessageBoxResult.OK)
                                    //    {

                                    //    }
                                    //    else
                                    //    {
                                    //        return;
                                    //    }
                                    //}
                                    //SelectedLocation = ArticleWarehouseLocationsList.Where(al => al.FullName == )

                                    ScanAction(_idWareHouseDeliveryNoteItem, BarcodeScannedQuantity);
                                    BarcodeStr = "";

                                    if (SelectedPickingMaterial != null)
                                    {
                                        if (SelectedPickingMaterial.DownloadQuantity > 0)
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

                                    //bool isIdWarehouseLocationPresent = false;

                                    //isIdWarehouseLocationPresent = WarehouseService.IsIdWarehouseLocationPresent(WarehouseCommon.Instance.Selectedwarehouse, IdArticlesStr,SelectedPickingMaterial);

                                    //if (isIdWarehouseLocationPresent == false)
                                    //{
                                    //    //PopUp msg
                                    //}
                                }
                                else
                                {
                                    IsWrongItemErrorVisible = Visibility.Visible;
                                    WrongItem = "Wrong Item " + BarcodeStr;
                                    BarcodeStr = "";
                                    OnFocusBgColor = "#FFFF0000";// color = cream Red
                                    OnFocusFgColor = "#FFFFFFFF";// color = cream White
                                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                }
                            }
                            else
                            {
                                IsWrongItemErrorVisible = Visibility.Visible;
                                WrongItem = "Wrong Item " + BarcodeStr;
                                BarcodeStr = "";

                                if (SelectedPickingMaterial.DownloadQuantity > 0)
                                {
                                    OnFocusBgColor = "#FFFF0000";
                                    OnFocusFgColor = "#FFFFFFFF";
                                }

                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            }
                        }
                        else
                        {
                            IsWrongItemErrorVisible = Visibility.Visible;
                            WrongItem = "Wrong Item " + BarcodeStr;
                            BarcodeStr = "";
                            OnFocusBgColor = "#FFFF0000";
                            OnFocusFgColor = "#FFFFFFFF";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }
                    }
                }
                else
                {
                    //BarcodeStr = BarcodeStr + obj.Text;
                    //[pramod.misal][GEOS2-5067][08-01-2024]
                    BarcodeStr = BarcodeStr + obj.Text.ToUpper();
                }
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                BarcodeStr = "";
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for scan action.
        /// [cpatil][GEOS2-5529][17-05-2024]
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
                    SelectedPickingMaterial = ReferenceArticlesList[indexItem];
                    ArticleImage = ByteArrayToImage(SelectedPickingMaterial.ArticleImageInBytes);

                    if (SelectedPickingMaterial.DownloadQuantity > 0)
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

                    IsInformationVisible = Visibility.Visible;
                    TrackBarEditvalue = 1;
                    countLocation++;
                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedPickingMaterial.IdArticle, Convert.ToInt32(SelectedPickingMaterial.IdWarehouse));
                    ChangeTotalStockColor(TotalCurrentItemStock, SelectedPickingMaterial.MinimumStock);
                    UpdateProgressbarLocationStock();
                    UpdateProgressbarArticleStock();
                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedPickingMaterial.Article.ArticleWarehouseLocation.MinimumStock);

                    if (ReferenceArticlesList != null && ReferenceArticlesList.Count > 1) // old code
                        IsSkipButtonEnable = true;
                }

                if (SelectedPickingMaterial != null && SelectedPickingMaterial.DownloadQuantity >= quantity && IsLocationScaned)
                {
                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);

                    if (SelectedPickingMaterial.DownloadQuantity >= quantity)
                    {
                        SelectedPickingMaterial.DownloadQuantity = SelectedPickingMaterial.DownloadQuantity - quantity;
                        SelectedPickingMaterial.ScannedQty = quantity;
                        TotalCurrentItemStock = TotalCurrentItemStock + quantity;

                        if (PickingOt != null && PickingOt.Quotation != null && PickingOt.Quotation.Site != null)
                        {
                            SelectedPickingMaterial.Comments = PickingOt.Code + " (" + PickingOt.Quotation.Site.Name + ") -> Item " + SelectedPickingMaterial.RevisionItem.NumItem + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                        }
                        SelectedPickingMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        if (SelectedPickingMaterial.IdWarehouseProductComponent == 0)
                            SelectedPickingMaterial.IdWarehouseProductComponent = null;

                        SelectedPickingMaterial.RevisionItem.Quantity -= quantity;

                        //{GEOS2-1957]
                        List<Int64> IdArticles = new List<Int64>();
                        foreach (var item in ReferenceArticlesList)
                        {
                            if (!IdArticles.Contains(item.IdArticle))
                            {
                                IdArticles.Add(item.IdArticle);
                            }
                        }

                        IdArticles.Sort();
                        IdArticle = string.Join(",", IdArticles.Select(n => n.ToString()).ToArray());

                        if (IsScannedLocationOnceChecked == false)
                        {
                            IsScannedLocationPresentinArticleLocations = WarehouseService.IsIdWarehouseLocationPresent(WarehouseCommon.Instance.Selectedwarehouse, IdArticle, SelectedPickingMaterial);
                            //[cpatil][GEOS2-5529][17-05-2024]
                            if(!IsScannedLocationPresentinArticleLocations)
                            {
                                ArticleWarehouseLocations articleWarehouseLocations = new ArticleWarehouseLocations { IdArticle = SelectedPickingMaterial.IdArticle, WarehouseLocation = new WarehouseLocation { FullName = SelectedPickingMaterial.LocationFullName, IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse }, Position = 0, MinimumStock = 0 };
                                //Service Method is changed from AddArticleWarehouseLocationByFullName to AddArticleWarehouseLocationByFullName_V2360
                                articleWarehouseLocations= WarehouseService.AddArticleWarehouseLocationByFullName_V2360(articleWarehouseLocations);
                                if (articleWarehouseLocations.IdArticleWarehouseLocation > 0)
                                {
                                    IsScannedLocationPresentinArticleLocations = true;
                                    Article article = new Article();
                                    article.LstArticleWarehouseLocations = null;
                                    article.MyWarehouse = null;
                                    article.IsAddedArticleImage = false;
                                    article.IsDeletedArticleImage = false;
                                    List<LogEntriesByArticle> ArticleChangeLogList = new List<LogEntriesByArticle>();
                                    ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = SelectedPickingMaterial.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleLocationAdd").ToString(), SelectedPickingMaterial.LocationFullName, WarehouseCommon.Instance.Selectedwarehouse.Name) });
                                    article.LogEntriesByArticles = new List<LogEntriesByArticle>(ArticleChangeLogList);
                                    var IsResult = WarehouseService.UpdateArticleDetails(article);
                                }
                            }
                            IsScannedLocationOnceChecked = true;
                        }
                        bool IsScanItem = false;
                        if (SelectedPickingMaterial.ScannedQty != 0 && IsScannedLocationPresentinArticleLocations == true)
                        {
                            IsScanItem = WarehouseService.InsertIntoArticleStockForRefund_V2170(SelectedPickingMaterial); //InsertIntoArticleStockForRefund_V2170
                        }

                        if (SelectedPickingMaterial.ScannedQty != 0 && IsScannedLocationPresentinArticleLocations == false)
                        {

                            // MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["NewLocationWarningMessage"].ToString(), LocationName, SelectedPickingMaterial.Reference, SelectedPickingMaterial.ScannedQty), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OKCancel);
                            bool isNext = CustomMessageBox.Show(string.Format(Application.Current.Resources["NewLocationWarningMessage"].ToString(), LocationName, SelectedPickingMaterial.Reference, SelectedPickingMaterial.ScannedQty), "Yellow");

                            if (isNext) //(MessageBoxResult == MessageBoxResult.OK)
                            {
                                AllPossibleLocationOfArticleList = WarehouseService.GetAllPossibleLocationOfArticle(IdArticle, WarehouseCommon.Instance.Selectedwarehouse); //WarehouseCommon.Instance.Selectedwarehouse).Where(al => !al.FullName.Equals(LocationName)).OrderBy(aw => aw.FullName).ToList();

                                if (AllPossibleLocationOfArticleList.Count > 1)
                                {
                                    NewLocationView newLocationView = new NewLocationView();
                                    NewLocationViewModel newLocationViewModel = new NewLocationViewModel();
                                    EventHandler handler = delegate { newLocationView.Close(); };
                                    newLocationView.DataContext = newLocationViewModel;
                                    newLocationViewModel.RequestClose += handler;
                                    newLocationViewModel.Init(SelectedPickingMaterial, AllPossibleLocationOfArticleList, IsScanSerialNumber, SelectedPickingMaterial.ScannedQty, LocationName, TotalCurrentItemStock, BarcodeStr);
                                    newLocationView.ShowDialogWindow();
                                    if (newLocationViewModel.IsSave == true)
                                    {
                                        IsScanItem = true;
                                    }
                                    else
                                    {
                                        IsScanItem = false;
                                    }

                                    if (newLocationViewModel.IsBackButtonPressed == true)
                                    {
                                        SelectedPickingMaterial.DownloadQuantity = SelectedPickingMaterial.DownloadQuantity + quantity;
                                        SelectedPickingMaterial.RevisionItem.Quantity = SelectedPickingMaterial.RevisionItem.Quantity + quantity;
                                        newLocationViewModel.IsBackButtonPressed = false;
                                    }
                                }
                                else if (AllPossibleLocationOfArticleList.Count == 0)
                                {
                                    MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(string.Format(Application.Current.Resources["AddNewLocationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                    SelectedPickingMaterial.DownloadQuantity = SelectedPickingMaterial.DownloadQuantity + quantity;
                                    SelectedPickingMaterial.RevisionItem.Quantity = SelectedPickingMaterial.RevisionItem.Quantity + quantity;
                                    return;
                                }
                                else
                                {
                                    // refund entry
                                    IsScanItem = WarehouseService.InsertIntoArticleStockForRefund_V2170(SelectedPickingMaterial);

                                    TransferMaterials SelectedRefundMaterial = new TransferMaterials();
                                    SelectedRefundMaterial.IdArticle = SelectedPickingMaterial.IdArticle;
                                    SelectedRefundMaterial.CostPrice = SelectedPickingMaterial.CostPrice;
                                    SelectedRefundMaterial.UnitPrice = SelectedPickingMaterial.UnitPrice;
                                    SelectedRefundMaterial.IdWarehouseLocation = SelectedPickingMaterial.IdWarehouseLocation;
                                    SelectedRefundMaterial.IdWarehouse = SelectedPickingMaterial.IdWarehouse;
                                    SelectedRefundMaterial.IdWareHouseDeliveryNoteItem = SelectedPickingMaterial.IdWareHouseDeliveryNoteItem;
                                    SelectedRefundMaterial.ScannedQty = (-1) * SelectedPickingMaterial.ScannedQty;
                                    SelectedRefundMaterial.Comments = "Transfer from " + "\"" + LocationName + "\"" + " location to " + "\"" + AllPossibleLocationOfArticleList[0].FullName + "\"" + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                                    SelectedRefundMaterial.ModifiedBy = SelectedPickingMaterial.ModifiedBy;
                                    SelectedRefundMaterial.IdCurrency = SelectedPickingMaterial.IdCurrency;

                                    IsDeductFromRefund = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(SelectedRefundMaterial);

                                    SelectedRefundMaterial.IdWarehouseLocation = AllPossibleLocationOfArticleList[0].IdWarehouseLocation;
                                    SelectedRefundMaterial.ScannedQty = SelectedPickingMaterial.ScannedQty;

                                    bool IssavedToNewLocation = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(SelectedRefundMaterial);

                                    //SelectedPickingMaterial.IdWarehouseLocation = AllPossibleLocationOfArticleList[0].IdWarehouseLocation;
                                    //SelectedPickingMaterial.ScannedQty = (-1) * SelectedPickingMaterial.ScannedQty;
                                    //IsScanItem = WarehouseService.InsertIntoArticleStockForRefund_V2170(SelectedPickingMaterial); // InsertIntoArticleStockForRefund_V2170
                                }

                            }
                            else
                            {
                                SelectedPickingMaterial.DownloadQuantity = SelectedPickingMaterial.DownloadQuantity + quantity;
                                SelectedPickingMaterial.RevisionItem.Quantity = SelectedPickingMaterial.RevisionItem.Quantity + quantity;
                                IsCancelForNonSerialNumber = false;
                                return;
                            }
                        }

                        if (IsScanItem)
                        {
                            TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedPickingMaterial.IdArticle, Convert.ToInt32(SelectedPickingMaterial.IdWarehouse));
                            ChangeTotalStockColor(TotalCurrentItemStock, SelectedPickingMaterial.MinimumStock);
                            UpdateProgressbarLocationStock();
                            UpdateProgressbarArticleStock();
                            ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedPickingMaterial.Article.ArticleWarehouseLocation.MinimumStock);
                            bool isUpdatedComments = WarehouseService.UpdateRevisionItemCommentsForRefund(WarehouseCommon.Instance.Selectedwarehouse, SelectedPickingMaterial.IdRevisionItem, idWarehouseDeliveryNoteItem);
                            bool isRevisionItemstage = WarehouseService.UpdateItemStatusAndStageForRefund(WarehouseCommon.Instance.Selectedwarehouse, SelectedPickingMaterial.IdOtitem, 1, GeosApplication.Instance.ActiveUser.IdUser);
                        }

                        if (SelectedPickingMaterial.DownloadQuantity == 0)
                        {
                            indexItem = ReferenceArticlesList.IndexOf(SelectedPickingMaterial);
                            indexItem = indexItem + 1;
                            if (indexItem < ReferenceArticlesList.Count)
                            {
                                GoToNextItem(SelectedPickingMaterial);
                            }
                            else
                            {
                                TrackBarEditvalue = 2;
                                SkipButtonCommandAction(null);
                                SelectedPickingMaterial = null;
                            }
                        }
                    }
                }
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

        //private void C2Method()
        //{
        //    AllPossibleLocationOfArticleList = WarehouseService.GetAllPossibleLocationOfArticle(IdArticle, WarehouseCommon.Instance.Selectedwarehouse); //WarehouseCommon.Instance.Selectedwarehouse).Where(al => !al.FullName.Equals(LocationName)).OrderBy(aw => aw.FullName).ToList();

        //    if (AllPossibleLocationOfArticleList.Count > 1)
        //    {
        //        NewLocationView newLocationView = new NewLocationView();
        //        NewLocationViewModel newLocationViewModel = new NewLocationViewModel();
        //        newLocationView.DataContext = newLocationViewModel;
        //        newLocationViewModel.Init(SelectedPickingMaterial, AllPossibleLocationOfArticleList);
        //        newLocationView.ShowDialogWindow();
        //        if (newLocationViewModel.IsSave == true)
        //        {
        //            IsScanItem = true;
        //        }
        //        else
        //        {
        //            IsScanItem = false;
        //        }
        //    }
        //    else
        //    {
        //        SelectedPickingMaterial.IdWarehouseLocation = AllPossibleLocationOfArticleList[0].IdWarehouseLocation;
        //        IsScanItem = WarehouseService.InsertIntoArticleStock(SelectedPickingMaterial);
        //    }
        //}

        //private void C1Method()
        //{
        //    RequestClose(null, null);
        //}

        /// <summary>
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][avpawar][GEOS2-1957][Allow Refund of items with picking location unassigned]
        /// </summary>
        public void Init(Ots ot, string workOrderItem = null)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init....", category: Category.Info, priority: Priority.Low);
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

                ArticleLocationList = new List<string>();
                WarehouseLocations = new List<WarehouseLocation>();
                MapItems = new ObservableCollection<WarehouseLocation>();
                PickingOt = (Ots)ot.Clone();
                PickingOt.Status = WarehouseService.GetWorkorderStatus(PickingOt.IdOT);

                if (PickingOt.CountryGroup != null && !string.IsNullOrEmpty(PickingOt.CountryGroup.Name))
                    IsProducerVisible = Visibility.Visible;

                // [001] Changed Service method
                //PickingMaterialsList = new List<PickingMaterials>(WarehouseService.GetArticleStockDetailForRefund_V2160(ot.IdOT, WarehouseCommon.Instance.Selectedwarehouse).ToList());

                // [002] Changed Service method
                PickingMaterialsList = new List<PickingMaterials>(WarehouseService.GetArticleStockDetailForRefund_V2170(ot.IdOT, WarehouseCommon.Instance.Selectedwarehouse).ToList());
                PickingMaterialsList = PickingMaterialsList.OrderBy(n => n.LocationFullName).ToList();

                foreach (PickingMaterials material in PickingMaterialsList)
                {
                    if (!ArticleLocationList.Contains(material.LocationFullName))
                        ArticleLocationList.Add(material.LocationFullName);

                    if (!WarehouseLocations.Any(x => x.IdWarehouseLocation == material.IdWarehouseLocation))
                        WarehouseLocations.Add(material.WarehouseLocation);

                    materialSoredList.Add(material);
                }

                ArticleLocationList = ArticleLocationList.OrderByDescending(n => n.ToUpper() == "TRANSIT").ThenBy(n => n).ToList();

                if (ArticleLocationList != null && ArticleLocationList.Count > 0)
                {
                    ///[001] Added to redirect to the first picking location for the item.
                    if (!string.IsNullOrEmpty(workOrderItem))
                    {
                        string otItemNumber = (workOrderItem.Substring(workOrderItem.Length - 3)).TrimStart('0');
                        PickingMaterials otitem = PickingMaterialsList.FirstOrDefault(x => x.RevisionItem.NumItem == otItemNumber);

                        if (otitem != null)
                        {
                            //List<string> orderedLocations = otitem.PickingMaterialsList.Select(person => person.LocationFullName).OrderBy(name => name).ToList();
                            string firstLocation = otitem.LocationFullName;

                            currentLocationIndex = ArticleLocationList.IndexOf(firstLocation);
                            currentLocationIndex = currentLocationIndex == -1 ? currentLocationIndex = 0 : currentLocationIndex;

                            //SetNextButtonEnable();
                            if (otitem.LocationFullName == ArticleLocationList.First() && otitem.LocationFullName == ArticleLocationList.Last())
                            {
                                IsBackButtonEnable = false;
                                IsSkipButtonEnable = false;
                            }
                            else if (otitem.LocationFullName == ArticleLocationList.First())
                            {
                                IsBackButtonEnable = false;
                                IsSkipButtonEnable = true;
                            }
                            else if (otitem.LocationFullName == ArticleLocationList.Last())
                            {
                                IsBackButtonEnable = true;
                                IsSkipButtonEnable = false;
                            }
                        }
                    }

                    ReferenceArticlesList = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[currentLocationIndex].ToString()).Select(p => p).ToList();
                    LocationName = ArticleLocationList[currentLocationIndex].ToString();

                    FillMapLocation();

                    //WarehouseLocation wlocation = WarehouseLocations.FirstOrDefault(x => x.FullName == LocationName);
                    //if (wlocation != null)
                    //{
                    //    MapItems.Add(wlocation);
                    //    byte[] warehouseLayout = GeosRepositoryService.GetCompanyLayoutFile(wlocation.FileName);

                    //    if (warehouseLayout != null)
                    //    {
                    //        string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                    //        if (!Directory.Exists(basePath))
                    //        {
                    //            Directory.CreateDirectory(basePath);
                    //        }

                    //        string svgFilePath = string.Format(@"{0}\Data\{1}", Path.GetTempPath(), wlocation.FileName);
                    //        File.WriteAllBytes(svgFilePath, warehouseLayout);
                    //        SvgUri = new Uri(svgFilePath);
                    //    }
                    //}

                    if (ArticleLocationList.Count == 1)
                        IsSkipButtonEnable = false;

                    if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                    {
                        BgColorLocation = "#FF083493";
                        FgColorLocation = "White";
                        TrackBarFgColor = "#FF083493";
                    }
                    else
                    {
                        FgColorLocation = "Black";
                        BgColorLocation = "#FF2AB7FF";
                        TrackBarFgColor = "#FF2AB7FF";
                    }
                }
                else
                {
                    IsSkipButtonEnable = false; // if location count is zero then next button will be disabled.
                    if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                    {
                        TrackBarFgColor = "#FF083493";
                    }
                    else
                    {
                        TrackBarFgColor = "#FF2AB7FF";
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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

        private void CalculateLocationIndex(LocationIndex locationIndex)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("RefundWorkOrderViewModel Method CalculateLocationIndex....", category: Category.Info, priority: Priority.Low);
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
                            SetSkipButtonEnable();

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
                            SetSkipButtonEnable();
                        }
                        break;
                }


                GeosApplication.Instance.Logger.Log("RefundWorkOrderViewModel Method CalculateLocationIndex........executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefundWorkOrderViewModel Method CalculateLocationIndex...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    ReferenceArticlesList = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[locationIndex].ToString()).Select(p => p).ToList();
                    LocationName = ArticleLocationList[locationIndex].ToString();
                    FillMapLocation();

                    //MapItems.Clear();
                    //SvgUri = null;
                    //WarehouseLocation wlocation = WarehouseLocations.FirstOrDefault(x => x.FullName == LocationName);
                    //if (wlocation != null)
                    //{
                    //    MapItems.Add(wlocation);
                    //    try
                    //    {
                    //        if (wlocation.FileName == null)
                    //        {
                    //            SvgUri = null;
                    //        }
                    //        else
                    //        {
                    //            byte[] warehouseLayout = GeosRepositoryService.GetCompanyLayoutFile(wlocation.FileName);

                    //            if (warehouseLayout != null)
                    //            {
                    //                string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                    //                if (!Directory.Exists(basePath))
                    //                {
                    //                    Directory.CreateDirectory(basePath);
                    //                }

                    //                string svgFilePath = string.Format(@"{0}\Data\{1}", Path.GetTempPath(), wlocation.FileName);
                    //                File.WriteAllBytes(svgFilePath, warehouseLayout);
                    //                SvgUri = new Uri(svgFilePath);
                    //            }
                    //        }
                    //    }
                    //    catch (FaultException<ServiceException> ex)
                    //    {
                    //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    //        GeosApplication.Instance.Logger.Log("Error in Loading warehouse layout svg file " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //    }
                    //    catch (ServiceUnexceptedException ex)
                    //    {
                    //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    //        GeosApplication.Instance.Logger.Log("Error in Loading warehouse layout svg file - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        GeosApplication.Instance.Logger.Log(string.Format("Error in Loading warehouse layout svg file - {0}. ErrorMessage- {1}", wlocation.FileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    //    }
                    //}

                    countLocation = 0;
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
                        FgColorLocation = "White";
                        TrackBarFgColor = "#FF083493";
                    }
                    else
                    {
                        FgColorLocation = "Black";
                        BgColorLocation = "#FF2AB7FF";
                        TrackBarFgColor = "#FF2AB7FF";
                    }

                    if (location == LocationIndex.NextLocation)
                        Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.RefundWorkOrderViewUserControlView", null, this);

                    if (location == LocationIndex.PreviousLocation)
                        Service.GoBack();
                }
                else
                {
                    IsSkipButtonEnable = false;
                    isLastLocation = true;
                }

                GeosApplication.Instance.Logger.Log("Method GetNextLocation....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetNextLocation...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        private void GoToNextItem(PickingMaterials currentPickingMaterial)
        {
            int currentPickingMaterialIndex = ReferenceArticlesList.IndexOf(currentPickingMaterial);
            this.currentPickingMaterial = currentPickingMaterial;

            if (currentPickingMaterialIndex + 1 < ReferenceArticlesList.Count)
            {
                SelectedPickingMaterial = ReferenceArticlesList[currentPickingMaterialIndex + 1];
                currentPickingMaterialIndex++;
                ArticleImage = ByteArrayToImage(SelectedPickingMaterial.ArticleImageInBytes);
                TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedPickingMaterial.IdArticle, Convert.ToInt32(SelectedPickingMaterial.IdWarehouse));
                ChangeTotalStockColor(TotalCurrentItemStock, SelectedPickingMaterial.MinimumStock);
                UpdateProgressbarLocationStock();
                UpdateProgressbarArticleStock();
                ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedPickingMaterial.Article.ArticleWarehouseLocation.MinimumStock);

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

                if (currentPickingMaterialIndex + 1 == ReferenceArticlesList.Count)
                {
                    if (currentLocationIndex >= ArticleLocationList.Count - 1)
                    {
                        IsSkipButtonEnable = false;
                    }

                    for (int item = currentLocationIndex + 1; item <= ArticleLocationList.Count - 1; item++) //TREU
                    {
                        List<PickingMaterials> material = materialSoredList.Where(pm => pm.LocationFullName == ArticleLocationList[item].ToString()).Select(p => p).ToList();
                        if (material.Any(x => x.DownloadQuantity > 0))
                        {
                            IsSkipButtonEnable = true;
                            break;
                        }
                        else
                        {
                            IsSkipButtonEnable = false;
                        }
                    }
                }
                else
                    IsSkipButtonEnable = true;
            }
            else
            {
                if (IsSkipButtonEnable)
                {
                    CalculateLocationIndex(LocationIndex.NextLocation);
                    GoToLocation(currentLocationIndex, LocationIndex.NextLocation);
                }
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
                ProgressbarLocationStock = new ArticleWarehouseLocations();
                ProgressbarLocationStock = WarehouseService.GetArticleStockByWarehouseLocation(SelectedPickingMaterial.IdArticle, SelectedPickingMaterial.Article.ArticleWarehouseLocation.IdWarehouseLocation, SelectedPickingMaterial.IdWarehouse);

                if (ProgressbarLocationStock.MaximumStock == 0)
                    ProgressbarLocationStock.MaximumStock = ProgressbarLocationStock.MaximumStock < ProgressbarLocationStock.LocationStock ? ProgressbarLocationStock.LocationStock : ProgressbarLocationStock.MaximumStock;
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
                GeosApplication.Instance.Logger.Log("Get an error in Method UpdateProgressbarLocationStock...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                ProgressbarArticleStock = WarehouseService.GetAVGStockByIdArticle_V2034(SelectedPickingMaterial.IdArticle, WarehouseCommon.Instance.Selectedwarehouse);
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
                GeosApplication.Instance.Logger.Log("Get an error in UpdateProgressbarArticleStock Method UpdateProgressbarArticleStock...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
