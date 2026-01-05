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
    
    class PickingODNViewModel : NavigationViewModelBase, INotifyPropertyChanged
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
        private Uri svgUri;
        bool isLastLocation;
        private bool isLocationScaned;
        private bool isSkipButtonEnable;
        private string barcodeStr;

        int locationIndex = 0;
        int countLocation = 0;
        int indexItem = 0;

        private List<WarehouseLocation> warehouseLocations;
        private ObservableCollection<WarehouseLocation> mapItems;
        private List<PickingMaterialsSC> pickingMaterialsSCList;
        List<PickingMaterialsSC> materialSoredList = new List<PickingMaterialsSC>();
        private PickingMaterialsSC selectedpickingMaterial;
        private Int64 totalCurrentItemStock;
        private string onFocusBgColor;
        private string onFocusFgColor;
        private PickingMaterialsSC tempItem;
        private PickingMaterialsSC currentPickingMaterialsSC;
        private List<WarehouseLocation> warehouseLocationList;
        private string idArticles;
        private bool focusUserControl;
        private int windowWidth;

        #endregion

        #region Public Properties

        public bool IsFromInit { get; set; }

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
                    ArticleImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Warehouse;component/Assets/Images/ImageEditLogo.png"));
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

        public List<PickingMaterialsSC> PickingMaterialsSCList
        {
            get { return pickingMaterialsSCList; }
            set
            {
                pickingMaterialsSCList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PickingMaterialsSCList"));
            }
        }

        public PickingMaterialsSC SelectedPickingMaterial
        {
            get { return selectedpickingMaterial; }
            set
            {
                selectedpickingMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedpickingMaterial"));
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

        public SupplierComplaint SupplierComplaintDetails { get; set; }

        public List<WarehouseLocation> WarehouseLocationList
        {
            get { return warehouseLocationList; }
            set
            {
                warehouseLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocationList"));
            }
        }

        public List<SupplierComplaintItem> SupplierComplaintItemList { get; set; }

        public int WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowWidth"));
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

        #region Command       
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandOnLoaded { get; set; }
        public ICommand HyperlinkClickCommand { get; set; }
        public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }
        public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandSkipButton { get; set; }

        #endregion

        #region Constructor

        public PickingODNViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PickingOdnScanViewModel ...", category: Category.Info, priority: Priority.Low);
                WindowHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 131;
                WindowWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 100;
                CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
                CommandOnLoaded = new DelegateCommand<RoutedEventArgs>(LoadedAction);
                HyperlinkClickCommand = new DelegateCommand<object>(HyperlinkClickCommandAction);
                VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
                CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);
                CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
                CommandSkipButton = new DelegateCommand<object>(SkipButtonCommandAction);
                TrackBarEditvalue = 0;
                IsInformationVisible = Visibility.Collapsed;
                IsLocationScaned = false;
                IsSkipButtonEnable = false;

                GeosApplication.Instance.Logger.Log("Constructor PickingOdnScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor PickingOdnScanViewModel() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init(SupplierComplaint tempSupplierComplaint)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init() ...", category: Category.Info, priority: Priority.Low);
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

                SupplierComplaintDetails = tempSupplierComplaint;
                WarehouseLocations = new List<WarehouseLocation>();
                MapItems = new ObservableCollection<WarehouseLocation>();

                SupplierComplaintItemList = WarehouseService.GetRemainingSCItemsByIdSC(SupplierComplaintDetails.IdSupplierComplaint, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse).ToList();
                idArticles = String.Join(",", SupplierComplaintItemList.Select(x => x.Article.IdArticle).ToArray()).ToString();

                WarehouseLocationList = new List<WarehouseLocation>(WarehouseService.GetWarehouseLocationsByIdArticles(idArticles, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse));
                WarehouseLocationList = WarehouseLocationList.OrderBy(n => n.FullName).ToList();

                if (WarehouseLocationList != null && WarehouseLocationList.Count > 0)
                {
                    IsFromInit = true;
                    FindNextLocation();
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

                IsWrongLocationErrorVisible = Visibility.Hidden;
                IsWrongItemErrorVisible = Visibility.Hidden;
                WrongLocation = "";
                WrongItem = "";

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
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

        private void CommandCancelAction(object obj)
        {
            bool isOutComplaintItem = WarehouseService.IsOUTSupplierComplaintItem(SupplierComplaintDetails.IdSupplierComplaint, WarehouseCommon.Instance.Selectedwarehouse);
            if (isOutComplaintItem)
            {
                bool isUpdated = WarehouseService.UpdateSupplierComplaint(SupplierComplaintDetails.IdSupplierComplaint, WarehouseCommon.Instance.Selectedwarehouse);
            }
            RequestClose(null, null);
        }

        /// <summary>
        /// Method for forcus form when it  load.
        /// </summary>
        /// <param name="obj"></param>
        private void LoadedAction(RoutedEventArgs obj)
        {
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.PickingODNUserControlView", null, this);
            WinUIDialogWindow detailView = ((WinUIDialogWindow)((System.Windows.RoutedEventArgs)obj).OriginalSource);
            detailView.Focus();
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

                PickingMaterialsSC article = (PickingMaterialsSC)obj;
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
                    int index = PickingMaterialsSCList.IndexOf(article);
                    if (articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage || articleDetailsViewModel.UpdateArticle.IsAddedArticleImage)
                    {
                        foreach (var item in SupplierComplaintItemList)
                        {
                            if (item.IdArticle == article.IdArticle)
                            {
                                item.Article.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                            }
                        }
                    }
                    if (SelectedPickingMaterial != null)
                    {
                        if (article.IdArticle == SelectedPickingMaterial.IdArticle)
                        {
                            if (articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage || articleDetailsViewModel.UpdateArticle.IsAddedArticleImage)
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

        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
                if (obj.Text == "\r" && !isLastLocation)
                {
                    if (!IsLocationScaned)
                    {
                        if (BarcodeStr.Equals(LocationName))
                        {
                            BarcodeStr = "";
                            BgColorLocation = "#FF008000";
                            ScanAction(0, 0);
                            IsLocationScaned = true;
                            IsWrongLocationErrorVisible = Visibility.Hidden;
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                            WrongLocation = "";
                        }
                        else
                        {
                            IsLocationScaned = false;
                            IsWrongLocationErrorVisible = Visibility.Visible;
                            WrongLocation = "Wrong Location " + BarcodeStr;
                            BarcodeStr = "";
                            BgColorLocation = "#FFFF0000";
                            FgColorLocation = "#FFFFFFFF";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }
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
                            bool IsWarehouseBelongsToSameSite = false;

                            Int64 _idwarehouse = Convert.ToInt64(BarcodeStr.Substring(0, 3));
                            Int64 _idWareHouseDeliveryNoteItem = Convert.ToInt64(BarcodeStr.Substring(3, 8));
                            Int64 _itemQuantity = Convert.ToInt64(BarcodeStr.Substring(11, 6));
                            Int64 supplierComplaintItemQuantity = 0;

                            SupplierComplaintItem tempSupplierComplaintItem = SupplierComplaintItemList.FirstOrDefault(x => x.Article.IdArticle == SelectedPickingMaterial.IdArticle);

                            if (tempSupplierComplaintItem != null)
                            {
                                supplierComplaintItemQuantity = tempSupplierComplaintItem.RemainingQuantity;
                            }

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

                            if (IsWarehouseBelongsToSameSite
                               && SelectedPickingMaterial.IdWareHouseDeliveryNoteItem.Equals(_idWareHouseDeliveryNoteItem)
                               && TotalCurrentItemStock >= _itemQuantity && supplierComplaintItemQuantity >= _itemQuantity && _itemQuantity > 0)
                            {
                                ScanAction(_idWareHouseDeliveryNoteItem, _itemQuantity);
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
                                            onFocusFgColor = "Black";
                                        }
                                    }
                                    else
                                    {
                                        OnFocusBgColor = "#FF008000";
                                        OnFocusFgColor = "#FFFFFFFF";
                                    }
                                }
                                IsWrongItemErrorVisible = Visibility.Hidden;
                                WrongItem = "";
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                tempSupplierComplaintItem.RemainingQuantity = supplierComplaintItemQuantity - _itemQuantity;
                            }
                            else
                            {
                                IsWrongItemErrorVisible = Visibility.Visible;
                                WrongItem = "Wrong Item " + BarcodeStr;
                                BarcodeStr = "";
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                if (SelectedPickingMaterial.DownloadQuantity > 0)
                                {
                                    OnFocusBgColor = "#FFFF0000";
                                    OnFocusFgColor = "#FFFFFFFF";
                                }
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

        private void ScanAction(Int64 idWareHouseDeliveryNoteItem, Int64 quantity)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);

                if (countLocation == 0)
                {
                    indexItem = 0;

                    if (PickingMaterialsSCList.Count > 0)
                    {
                        SelectedPickingMaterial = PickingMaterialsSCList[indexItem];
                        ArticleImage = ByteArrayToImage((SupplierComplaintItemList.FirstOrDefault(x => x.IdArticle == SelectedPickingMaterial.IdArticle)).Article.ArticleImageInBytes);

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

                        if (PickingMaterialsSCList != null && PickingMaterialsSCList.Count > 1)
                            IsSkipButtonEnable = true;
                    }
                }

                if (IsLocationScaned)
                {
                    tempItem = PickingMaterialsSCList.Where(pi => pi.IdWareHouseDeliveryNoteItem == SelectedPickingMaterial.IdWareHouseDeliveryNoteItem && pi.DownloadQuantity > 0).FirstOrDefault();

                    if (SelectedPickingMaterial.DownloadQuantity >= quantity)
                    {
                        tempItem.DownloadQuantity = tempItem.DownloadQuantity - quantity;
                        tempItem.ScannedQty = (-1) * quantity;
                        TotalCurrentItemStock -= quantity;
                        tempItem.Comments = string.Format("{0} [STOCK -> {1}]", SupplierComplaintDetails.Code, Convert.ToString(TotalCurrentItemStock));
                        tempItem.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;

                        SupplierComplaintItem tempSupplierComplaintItem = SupplierComplaintItemList.FirstOrDefault(x => x.Article.IdArticle == tempItem.IdArticle);
                        if (tempSupplierComplaintItem != null)
                            tempItem.IdSCitem = tempSupplierComplaintItem.IdSupplierComplaintItem;

                        SelectedPickingMaterial = tempItem;

                        if (tempItem.IdWarehouseProductComponent == 0)
                            tempItem.IdWarehouseProductComponent = null;

                        bool IsScanItem = WarehouseService.InsertIntoArticleStockSC(tempItem);

                        TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedPickingMaterial.IdArticle, Convert.ToInt32(SelectedPickingMaterial.IdWarehouse));
                        ChangeTotalStockColor(TotalCurrentItemStock, SelectedPickingMaterial.MinimumStock);

                        if (SelectedPickingMaterial.DownloadQuantity == 0)
                        {
                            indexItem = PickingMaterialsSCList.IndexOf(SelectedPickingMaterial);
                            indexItem = indexItem + 1;

                            if (indexItem < PickingMaterialsSCList.Count)
                            {
                                GoToNextItem(SelectedPickingMaterial);
                            }
                            else
                            {
                                SkipButtonCommandAction(null);
                                if (!PickingMaterialsSCList.Any(x => x.DownloadQuantity > 0))
                                    TrackBarEditvalue = 2;
                                //SelectedpickingMaterial = null;
                            }
                        }
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

        private void SkipButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log(" Method SkipButtonCommandAction....", category: Category.Info, priority: Priority.Low);

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
                GoToLocation();
            else
                GoToNextItem(SelectedPickingMaterial);

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method SkipButtonCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
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

        private void GoToLocation()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GoToLocation....", category: Category.Info, priority: Priority.Low);
                locationIndex++;
                if (WarehouseLocationList.Count > locationIndex)
                {
                    IsFromInit = false;
                    FindNextLocation();
                }
                else
                {
                    IsSkipButtonEnable = false;
                    isLastLocation = true;
                }
                GeosApplication.Instance.Logger.Log("Method GoToLocation....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GoToLocation...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        private void GoToNextItem(PickingMaterialsSC currentPickingMaterialsSC)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GoToNextItem....", category: Category.Info, priority: Priority.Low);
                int currentPickingMaterialIndex = PickingMaterialsSCList.IndexOf(currentPickingMaterialsSC);
                this.currentPickingMaterialsSC = currentPickingMaterialsSC;
                if (currentPickingMaterialIndex + 1 < PickingMaterialsSCList.Count)
                {
                    SelectedPickingMaterial = PickingMaterialsSCList[currentPickingMaterialIndex + 1];
                    currentPickingMaterialIndex++;
                    ArticleImage = ByteArrayToImage((SupplierComplaintItemList.FirstOrDefault(x => x.IdArticle == SelectedPickingMaterial.IdArticle)).Article.ArticleImageInBytes);
                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedPickingMaterial.IdArticle, Convert.ToInt32(SelectedPickingMaterial.IdWarehouse));
                    ChangeTotalStockColor(TotalCurrentItemStock, SelectedPickingMaterial.MinimumStock);

                    if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                    {
                        OnFocusBgColor = "#FF083493";
                        OnFocusFgColor = "White";
                    }
                    else
                    {
                        OnFocusBgColor = "#FF2AB7FF";
                        onFocusFgColor = "Black";
                    }

                    if (currentPickingMaterialIndex + 1 == PickingMaterialsSCList.Count)
                    {
                        if (WarehouseLocationList.Count - 1 == locationIndex)
                            IsSkipButtonEnable = false;
                        else
                        {
                            for (int i = locationIndex + 1; i < WarehouseLocationList.Count; i++)
                            {
                                // [001] Changed Service method
                                var tempPickingMaterialsSCList = new List<PickingMaterialsSC>(WarehouseService.GetPickingItemsForSupplierComplaintItemArticlesAndLocation_V2034(idArticles, WarehouseCommon.Instance.Selectedwarehouse, WarehouseLocationList[i].IdWarehouseLocation));
                                if (tempPickingMaterialsSCList.Count == 0)
                                {
                                    if (WarehouseLocationList.Count == i + 1)
                                    {
                                        IsSkipButtonEnable = false;
                                        break;
                                    }
                                }
                                else if (tempPickingMaterialsSCList.Count > 0)
                                {
                                    locationIndex = i - 1;
                                    break;
                                }
                            }
                        }
                    }
                    else if (currentPickingMaterialIndex + 1 < PickingMaterialsSCList.Count)
                        IsSkipButtonEnable = true;
                    else
                        IsSkipButtonEnable = false;
                }
                else
                {
                    if (IsSkipButtonEnable)
                        GoToLocation();
                }

                GeosApplication.Instance.Logger.Log("Method GoToNextItem....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GoToNextItem...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [Sprint_61] [16-04-2019]-------[GEOS2-192 Do not display empty locations in ODN picking][sdesai]
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// This method added to find next location which is not empty.
        /// </summary>
        public void FindNextLocation()
        {
            for (int i = locationIndex; i < WarehouseLocationList.Count; i++)
            {
                TotalCurrentItemStock = 0;

                // [001] Changed Service method
                PickingMaterialsSCList = new List<PickingMaterialsSC>(WarehouseService.GetPickingItemsForSupplierComplaintItemArticlesAndLocation_V2034(idArticles, WarehouseCommon.Instance.Selectedwarehouse, WarehouseLocationList[i].IdWarehouseLocation));
                if (PickingMaterialsSCList.Count > 0)
                {
                    IsSkipButtonEnable = true;
                    LocationName = WarehouseLocationList[i].FullName;
                    MapItems.Clear();
                    SvgUri = null;
                    WarehouseLocation wlocation = WarehouseLocationList.FirstOrDefault(x => x.FullName == LocationName);

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
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log("Error in Loading warehouse layout svg file " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
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

                    if (IsFromInit)
                        IsFromInit = false;
                    else
                    {
                        countLocation = 0;
                        IsInformationVisible = Visibility.Collapsed;
                        IsLocationScaned = false;
                        TrackBarEditvalue = 0;
                        IsWrongLocationErrorVisible = Visibility.Hidden;
                        IsWrongItemErrorVisible = Visibility.Hidden;
                        WrongLocation = "";
                        WrongItem = "";
                        Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.PickingODNUserControlView", null, this);
                    }

                    break;
                }

                locationIndex++;
            }

            if (locationIndex + 1 == WarehouseLocationList.Count)
                IsSkipButtonEnable = false;
            else
            {
                for (int i = locationIndex + 1; i < WarehouseLocationList.Count; i++)
                {
                    // [001] Changed Service method
                    var tempPickingMaterialsSCList = new List<PickingMaterialsSC>(WarehouseService.GetPickingItemsForSupplierComplaintItemArticlesAndLocation_V2034(idArticles, WarehouseCommon.Instance.Selectedwarehouse, WarehouseLocationList[i].IdWarehouseLocation));
                    if (tempPickingMaterialsSCList.Count == 0)
                    {
                        if (WarehouseLocationList.Count == i + 1)
                        {
                            IsSkipButtonEnable = false;
                            break;
                        }
                    }
                    else if (tempPickingMaterialsSCList.Count > 0)
                    {
                        locationIndex = i - 1;
                        break;
                    }
                }
            }
        }
        #endregion

    }

}
