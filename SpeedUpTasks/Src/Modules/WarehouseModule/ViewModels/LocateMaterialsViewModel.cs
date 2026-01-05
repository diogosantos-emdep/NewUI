using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Map;
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
    class LocateMaterialsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region TaskLog
        //[WMS-M055-01] Storage of new articles without location yet [adadibathina]
        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }

        #endregion //End Of Services

        #region Declaration

        private PendingStorageArticles selectedLocateMaterial;
        private Visibility isLocationIndicatorVisible;
        private Visibility isInformationVisible;
        private string bgColorLocation;
        private string fgColorLocation;
        private string barcodeStr;
        private string locationName;
        private bool isLocationScaned;
        private string rowBgColor;
        private string totalStockBgColor;
        private string totalStockFgColor;
        private string onFocusBgColor;
        private string onFocusFgColor;
        private PendingStorageArticles tempItem;
        List<string> articleLocationList = new List<string>();
        private Int64 totalCurrentItemStock;
        private ImageSource articleImage;

        private List<PendingStorageArticles> selectedLocationList;
        private List<PendingStorageArticles> pendingStorageArticlesList;
        private List<PendingStorageArticles> mainpendingStorageArticlesList;

        int locationIndex = 0;
        int countLocation = 0;
        int indexItem = 0;

        List<PendingStorageArticles> materialSoredList = new List<PendingStorageArticles>();
        bool isNextButtonEnable;
        bool isLocationLast;
        private int trackBarEditvalue;
        private string trackBarFgColor;
        private int windowHeight;
        private int windowWidth;
        private string wrongLocation;
        private string wrongItem;
        private Visibility isWrongLocationErrorVisible = Visibility.Hidden;
        private Visibility isWrongItemErrorVisible = Visibility.Hidden;

        private List<WarehouseLocation> warehouseLocations;
        private ObservableCollection<WarehouseLocation> mapItems;
        Uri svgUri;
        private bool isScaned;
        private bool isWrongBarCode;
        private string scanedBarCode;
        private PendingStorageArticles currentPendingStorageArticle;
        private bool focusUserControl;
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
        public bool IsNotOk { get; set; }
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
        public int WindowHeight
        {
            get { return windowHeight; }
            set
            {
                windowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight"));
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
        public Int64 IdTransitLocation { get; set; }
        public Visibility IsLocationIndicatorVisible
        {
            get { return isLocationIndicatorVisible; }
            set
            {
                isLocationIndicatorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocationIndicatorVisible"));
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
        public string FgColorLocation
        {
            get { return fgColorLocation; }
            set
            {
                fgColorLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FgColorLocation"));
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
        public PendingStorageArticles SelectedLocateMaterial
        {
            get { return selectedLocateMaterial; }
            set
            {
                selectedLocateMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocateMaterial"));
            }
        }
        public List<PendingStorageArticles> SelectedLocationList
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

        public string TrackBarFgColor
        {
            get { return trackBarFgColor; }
            set
            {
                trackBarFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrackBarFgColor"));
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
        public List<PendingStorageArticles> PendingStorageArticlesList
        {
            get { return pendingStorageArticlesList; }
            set
            {
                pendingStorageArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PendingStorageArticlesList"));
            }
        }

        public List<PendingStorageArticles> MainpendingStorageArticlesList
        {
            get { return mainpendingStorageArticlesList; }
            set
            {
                mainpendingStorageArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainpendingStorageArticlesList"));
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
        public string ScanedBarCode
        {
            get { return scanedBarCode; }
            set
            {
                scanedBarCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScanedBarCode"));
            }
        }
        public bool IsScaned
        {
            get { return isScaned; }
            set
            {
                isScaned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsScaned"));
            }
        }
        public bool IsWrongBarCode
        {
            get { return isWrongBarCode; }
            set
            {
                isWrongBarCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongBarCode"));
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
        public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }
        public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        public ICommand HyperlinkClickCommand { get; set; }

        #endregion // End Of ICommands

        #region Constructor

        public LocateMaterialsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor LocateMaterialsViewModel....", category: Category.Info, priority: Priority.Low);
                WindowHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 130;
                CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
                CommandNextButton = new DelegateCommand<object>(NextButtonCommandAction);
                CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
                CommandOnLoaded = new DelegateCommand(LoadedAction); //new DelegateCommand<RoutedEventArgs>(LoadedAction);
                VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
                CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);
                HyperlinkClickCommand = new DelegateCommand<object>(HyperlinkClickCommandAction);
                IsLocationIndicatorVisible = Visibility.Visible;
                TrackBarEditvalue = 0;
                IsInformationVisible = Visibility.Collapsed;
                IsLocationScaned = false;
                IsNextButtonEnable = true;

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

                OnFocusBgColor = "Wheat";
                OnFocusFgColor = "Red";
                GeosApplication.Instance.Logger.Log("Constructor LocateMaterialsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor LocateMaterialsViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method for fill data .
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        /// <param name="idOt"></param>
        /// <param name="objWarehouse"></param>
        public void InIt(List<PendingStorageArticles> tempPendingStorageArticlesList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt....", category: Category.Info, priority: Priority.Low);
                List<WarehouseLocation> suggestedLocations;
                MainpendingStorageArticlesList = tempPendingStorageArticlesList;
                PendingStorageArticlesList = tempPendingStorageArticlesList.Select(book => (PendingStorageArticles)book.Clone()).ToList();

                if (tempPendingStorageArticlesList.Count > 0)
                    IdTransitLocation = tempPendingStorageArticlesList[0].IdWarehouseLocation;

                List<Int64> articleIds = new List<Int64>();

                foreach (var item in PendingStorageArticlesList)
                {
                    if (!articleIds.Any(x => x == item.IdArticle))
                    {
                        articleIds.Add(item.IdArticle);
                    }

                }
                articleIds = articleIds.Distinct().ToList();
                string IdArticlesStr = string.Join(",", articleIds.Select(n => n.ToString()).ToArray());

                //Int64 _idwarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;

                List<ArticleWarehouseLocations> articleWarehouseLocationsList = WarehouseService.GetArticlesWarehouseLocation_V2032(IdArticlesStr, WarehouseCommon.Instance.Selectedwarehouse).OrderBy(aw => aw.FullName).ToList();
                //List<ArticleWarehouseLocations> articleWarehouseLocationsList = WarehouseService.GetArticlesWarehouseLocation(IdArticlesStr, _idwarehouse).ToList();

                WarehouseLocations = new List<WarehouseLocation>();
                MapItems = new ObservableCollection<WarehouseLocation>();

                // code for eliminate articles from list , who don't have any location to move.
                // PendingStorageArticlesList = PendingStorageArticlesList.Where(p => articleWarehouseLocationsList.Any(emp => p.IdArticle == emp.IdArticle)).ToList();
                //   PendingStorageArticlesList = PendingStorageArticlesList.GroupBy(x => x.IdArticle).Select(x => x.FirstOrDefault()).ToList();
                PendingStorageArticlesList = PendingStorageArticlesList.OrderBy(x => x.IdArticle).ToList();
                foreach (var item in PendingStorageArticlesList)
                {
                    ArticleWarehouseLocations articleLocation = articleWarehouseLocationsList.Where(a => a.IdArticle == item.IdArticle).FirstOrDefault();
                    if (articleLocation != null)
                    {
                        item.IdWarehouseLocation = articleLocation.IdWarehouseLocation;
                        item.FullName = articleLocation.FullName;

                        if (!WarehouseLocations.Any(x => x.IdWarehouseLocation == articleLocation.IdWarehouseLocation))
                        {
                            WarehouseLocations.Add(articleLocation.WarehouseLocation);
                        }
                    }
                    else
                    {
                        #region Suggested Location
                        // [001] Changed Service method
                        suggestedLocations = WarehouseService.GetWarehouseLocationToPlaceArticle_V2034(WarehouseCommon.Instance.Selectedwarehouse, item.Reference);
                        if (suggestedLocations.Count > 0)
                        {
                            item.IdWarehouseLocation = suggestedLocations[0].IdWarehouseLocation;
                            item.FullName = "?" + suggestedLocations[0].FullName;
                            WarehouseLocations.Add(new WarehouseLocation { FullName = "?" + suggestedLocations[0].FullName });
                        }
                        else
                        {
                            item.IdWarehouseLocation = 0;
                            item.FullName = "?";
                            WarehouseLocations.Add(new WarehouseLocation { FullName = "?" });
                        }
                        #endregion
                    }
                }

                // Fill Article location list.
                ArticleLocationList = new List<string>();

                foreach (var material in PendingStorageArticlesList)
                {
                    if (!ArticleLocationList.Contains(material.FullName))
                        ArticleLocationList.Add(material.FullName);
                }

                ArticleLocationList = ArticleLocationList.OrderBy(n => n).ToList();
                if (IsScaned)
                {
                    if (ArticleLocationList.Count > 0)
                    {

                        ArticleLocationList = ArticleLocationList.Where(x => x.StartsWith(ScanedBarCode)).ToList();
                        if (ArticleLocationList.Count > 0 && ArticleLocationList != null)
                            IsWrongBarCode = true;
                        else
                        {
                            IsWrongBarCode = false;
                            return;
                        }
                    }
                }

                SelectedLocationList = PendingStorageArticlesList.Where(pm => pm.FullName.Equals(ArticleLocationList[locationIndex].ToString())).Select(p => p).ToList();


                if (ArticleLocationList.Count > 0)
                {
                    LocationName = ArticleLocationList[locationIndex].ToString();
                    SaveAndLoadWarehouseLayoutFile(LocationName.Replace("?", ""));
                }

                if (SelectedLocationList.Count > 0)
                    TotalCurrentItemStock = SelectedLocationList[0].CurrentStock;

                WrongLocation = "";
                if (ArticleLocationList.Count <= 1)
                {
                    IsNextButtonEnable = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InIt...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method InIt....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// To load map and images
        /// </summary>
        /// <param name="warehouseLocationName">Full Name </param>      
        private void SaveAndLoadWarehouseLayoutFile(string warehouseLocationName)
        {
            GeosApplication.Instance.Logger.Log("Method SaveAndLoadWarehouseLayoutFile....", category: Category.Info, priority: Priority.Low);
            WarehouseLocation wlocation = WarehouseLocations.FirstOrDefault(x => x.FullName == warehouseLocationName);

            //Suggested Locations wont be lodded  wlocation loading after the scan
            if (wlocation == null)
            {

                wlocation = WarehouseService.GetWarehouseLocationByFullName(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, warehouseLocationName);
                WarehouseLocations.Add(wlocation);
            }
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
                GeosApplication.Instance.Logger.Log("Method SaveAndLoadWarehouseLayoutFile....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        private void LoadedAction()
        {
            GeosApplication.Instance.Logger.Log("Method LoadedAction....", category: Category.Info, priority: Priority.Low);
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocateMaterialsUserControlView", null, this);
            GeosApplication.Instance.Logger.Log("Method LoadedAction....executed successfully", category: Category.Info, priority: Priority.Low);
        }






        /// <summary>
        /// Method for do scan opration on scanned barcode.
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(TextCompositionEventArgs obj)


        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);

                if (obj.Text == "\r" && !isLocationLast)
                {
                    if (!IsLocationScaned)
                    {
                        #region Suggested Location
                        if (LocationName.Contains("?"))
                        {
                            //For ? check the scaned location is in warehouse
                            if (WarehouseService.IsExistWarehouseLocationFullName(BarcodeStr, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse))
                            {
                                LocationName = BarcodeStr;
                                SaveAndLoadWarehouseLayoutFile(LocationName.Replace("?", ""));
                            }
                        }
                        #endregion

                        if (BarcodeStr.Equals(LocationName))
                        {
                            BarcodeStr = "";
                            BgColorLocation = "#FF008000";
                            FgColorLocation = "#FFFFFFFF";
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
                        if (BarcodeStr.Length < 11)
                        {
                            isProperBarcode = false;
                        }

                        if (isProperBarcode)
                        {
                            //string _idwarehouseStr = BarcodeStr.Substring(0, 3);
                            //string _idWareHouseDeliveryNoteItemStr = BarcodeStr.Substring(3, 8);
                            //string _itemQuantityStr = BarcodeStr.Substring(11, 6);

                            Int64 _idwarehouse = Convert.ToInt64(BarcodeStr.Substring(0, 3));
                            Int64 _idWareHouseDeliveryNoteItem = Convert.ToInt64(BarcodeStr.Substring(3, 8));
                            Int64 _itemQuantity = Convert.ToInt64(BarcodeStr.Substring(11, 6));

                            //&& selectedLocateMaterial.Quantity == _itemQuantity
                            if (SelectedLocateMaterial.IdWarehouse.Equals(_idwarehouse)
                                && SelectedLocateMaterial.IdWareHouseDeliveryNoteItem.Equals(_idWareHouseDeliveryNoteItem)
                                && SelectedLocateMaterial.Quantity >= _itemQuantity && _itemQuantity > 0)
                            {
                                //This is used to avoid minus stock for warehousedeliverynoteitem.
                                Int64 locationAvaibleQuantity = WarehouseService.GetStockForScanItem(SelectedLocateMaterial.IdArticle, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, _idWareHouseDeliveryNoteItem, WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation.Value);

                                if (locationAvaibleQuantity < _itemQuantity)
                                {
                                    IsWrongItemErrorVisible = Visibility.Visible;
                                    WrongItem = String.Format("No location stock available {0}", BarcodeStr);
                                    BarcodeStr = "";
                                    RowBgColor = "#FFFF0000";
                                    OnFocusBgColor = "#FFFF0000";
                                    OnFocusFgColor = "#FFFFFFFF";
                                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                    return;
                                }

                                ScanAction(_idWareHouseDeliveryNoteItem, _itemQuantity);
                                RowBgColor = "#FFF4E27E";
                                BarcodeStr = "";

                                if (SelectedLocateMaterial != null)
                                {
                                    if (SelectedLocateMaterial.Quantity > 0)
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
                            }
                            else
                            {
                                IsWrongItemErrorVisible = Visibility.Visible;
                                WrongItem = "Wrong Item " + BarcodeStr;
                                BarcodeStr = "";
                                RowBgColor = "#FFFF0000";
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                if (SelectedLocateMaterial.Quantity > 0)
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
                            RowBgColor = "#FFFF0000";
                            OnFocusBgColor = "#FFFF0000";
                            OnFocusFgColor = "#FFFFFFFF";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                        }
                    }
                }
                else
                {
                    BarcodeStr = BarcodeStr + obj.Text;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for scan action.
        /// </summary>
        /// <param name="idwarehouseDeliveryNote"></param>
        /// <param name="quantity"></param>
        private void ScanAction(Int64 idWareHouseDeliveryNoteItem, Int64 quantity)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);
                if (countLocation == 0)
                {
                    indexItem = 0;
                    SelectedLocateMaterial = SelectedLocationList[indexItem];
                    //ArticleImage = ByteArrayToImage(SelectedLocateMaterial.ArticleVisualAidsPath, SelectedLocateMaterial.ImagePath);
                    ArticleImage = ByteArrayToBitmapImage(SelectedLocateMaterial.ArticleImageInBytes);
                    //if (SelectedpickingMaterial.DownloadQuantity > 0)
                    {
                        OnFocusBgColor = "#FFF4E27E";
                        OnFocusFgColor = "#FF000000";
                    }

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

                    IsLocationIndicatorVisible = Visibility.Hidden;
                    TrackBarEditvalue = 1;
                    IsInformationVisible = Visibility.Visible;
                    countLocation++;

                    ChangeTotalStockColor(TotalCurrentItemStock, SelectedLocateMaterial.MinimumStock);
                    if (SelectedLocationList != null && SelectedLocationList.Count > 1)
                        IsNextButtonEnable = true;
                }
                if (IsLocationScaned)
                {
                    tempItem = SelectedLocationList.Where(pi => pi.IdArticle == SelectedLocateMaterial.IdArticle && pi.IdWareHouseDeliveryNoteItem == idWareHouseDeliveryNoteItem).FirstOrDefault();

                    #region Suggested Location
                    // Getting location name and its warehouse obj
                    //Suggested Location Entity in db
                    if (tempItem.FullName.Contains("?"))
                    {
                        tempItem.FullName = LocationName;
                        tempItem.WarehouseLocation = WarehouseLocations.FirstOrDefault(x => x.FullName == LocationName);
                        tempItem.IdWarehouseLocation = tempItem.WarehouseLocation.IdWarehouseLocation;
                        SuggestedLocationEntity();

                    }
                    #endregion

                    tempItem.Quantity -= quantity;
                    tempItem.ScannedQty = quantity;

                    tempItem.Comments = "Transfer from " + "\"" + "Transit" + "\"" + " location to " + "\"" + tempItem.FullName + "\"";

                    tempItem.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    SelectedLocateMaterial = tempItem;

                    PendingStorageArticles psa = (PendingStorageArticles)tempItem.Clone();
                    psa.ScannedQty = (-1) * psa.ScannedQty;
                    psa.IdWarehouseLocation = IdTransitLocation;

                    bool IsdiductFromtransit = WarehouseService.InsertIntoArticleStockForLocateMaterial(psa);

                    bool IsInsertArticletoLocation = WarehouseService.InsertIntoArticleStockForLocateMaterial(tempItem);

                    //Remove updated article from list for update the grid data.
                    if (tempItem.Quantity == 0)
                    {
                        MainpendingStorageArticlesList.Remove(MainpendingStorageArticlesList.Where(pi => pi.IdArticle == SelectedLocateMaterial.IdArticle && pi.IdWareHouseDeliveryNoteItem == idWareHouseDeliveryNoteItem).FirstOrDefault());

                        indexItem = indexItem + 1;

                        if (indexItem < SelectedLocationList.Count && tempItem.Quantity == 0)
                        {
                            //SelectedLocateMaterial = SelectedLocationList[indexItem];

                            //ArticleImage = ByteArrayToBitmapImage(SelectedLocateMaterial.ArticleImageInBytes);
                            GoToNextItem(SelectedLocateMaterial);
                        }
                        else
                        {
                            TrackBarEditvalue = 2;
                            NextButtonCommandAction(null);
                        }
                    }
                    else
                    {
                        var selected = MainpendingStorageArticlesList.Where(pi => pi.IdArticle == SelectedLocateMaterial.IdArticle && pi.IdWareHouseDeliveryNoteItem == idWareHouseDeliveryNoteItem).FirstOrDefault();
                        selected.Quantity = tempItem.Quantity;
                    }
                }

                SelectedLocationList = new List<PendingStorageArticles>(SelectedLocationList);

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
            GeosApplication.Instance.Logger.Log("Method ScanAction....executed successfully", category: Category.Info, priority: Priority.Low);
        }





        /// <summary>
        ///  articleWarehouseLocations auto updateing the Suggested location for firsttime 
        /// </summary>
        private void SuggestedLocationEntity()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SuggestedLocationEntity....", category: Category.Info, priority: Priority.Low);
                //Location Entity
                ArticleWarehouseLocations articleWarehouseLocations = new ArticleWarehouseLocations { IdArticle = SelectedLocateMaterial.IdArticle, WarehouseLocation = new WarehouseLocation { FullName = LocationName, IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse }, Position = 0, MinimumStock = 0 };
                WarehouseService.AddArticleWarehouseLocationByFullName(articleWarehouseLocations);
                //Log Entity
                Article article = new Article();
                article.LstArticleWarehouseLocations = null;
                article.MyWarehouse = null;
                article.IsAddedArticleImage = false;
                article.IsDeletedArticleImage = false;
                List<LogEntriesByArticle> ArticleChangeLogList = new List<LogEntriesByArticle>();
                ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = SelectedLocateMaterial.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleLocationAdd").ToString(), tempItem.WarehouseLocation.FullName, WarehouseCommon.Instance.Selectedwarehouse.Name) });
                article.LogEntriesByArticles = new List<LogEntriesByArticle>(ArticleChangeLogList);
                var IsResult = WarehouseService.UpdateArticleDetails(article);
            }
            catch (FaultException<ServiceException> ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in SuggestedLocationEntity() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in SuggestedLocationEntity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SuggestedLocationEntity...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method SuggestedLocationEntity....executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// Method for change Total stock color as per condition.
        /// </summary>
        /// <param name="totalStock"></param>
        /// <param name="minQuantity"></param>
        private void ChangeTotalStockColor(Int64 totalStock, Int64 minQuantity)
        {
            GeosApplication.Instance.Logger.Log("Method ChangeTotalStockColor....", category: Category.Info, priority: Priority.Low);
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
            GeosApplication.Instance.Logger.Log("Method ChangeTotalStockColor....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for scan next location items.
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
                GoToLocation();
            else
                GoToNextItem(SelectedLocateMaterial);
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method NextButtonCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void VectorLayerDataLoaded(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method VectorLayerDataLoaded....", category: Category.Info, priority: Priority.Low);
            MapControl mapControl = obj as MapControl;

            if (mapControl != null)
                mapControl.ZoomToFitLayerItems();
            GeosApplication.Instance.Logger.Log("Method VectorLayerDataLoaded....executed ", category: Category.Info, priority: Priority.Low);
        }

        void ListSourceDataAdapterCustomizeMapItem(object e)
        {
            GeosApplication.Instance.Logger.Log("Method ListSourceDataAdapterCustomizeMapItem....", category: Category.Info, priority: Priority.Low);
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

            GeosApplication.Instance.Logger.Log("Method ListSourceDataAdapterCustomizeMapItem....executed ", category: Category.Info, priority: Priority.Low);
        }

        private void CommandCancelAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommandCancelAction....", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);

            try
            {
                string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                if (Directory.Exists(basePath))
                {
                    Directory.Delete(basePath, true);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in deleting warehouse layout svg files. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method CommandCancelAction....executed ", category: Category.Info, priority: Priority.Low);
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

                PendingStorageArticles article = (PendingStorageArticles)obj;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                articleDetailsViewModel.Init(article.Reference, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                articleDetailsView.DataContext = articleDetailsViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                articleDetailsView.ShowDialog();

                if (articleDetailsViewModel.IsResult)
                {
                    int index = SelectedLocationList.IndexOf(article);
                    if (articleDetailsViewModel.UpdateArticle.IsAddedArticleImage || articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage)
                        SelectedLocationList[index].ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                    if (SelectedLocateMaterial != null)
                    {
                        if (article.IdArticle == SelectedLocateMaterial.IdArticle)
                            ArticleImage = ByteArrayToBitmapImage(articleDetailsViewModel.UpdateArticle.ArticleImageInBytes);
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
        /// This method is for to convert from Bytearray to ImageSource
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

        private void GoToLocation()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetNextLocation....", category: Category.Info, priority: Priority.Low);
                locationIndex++;
                if (ArticleLocationList.Count > locationIndex)
                {
                    TotalCurrentItemStock = 0;
                    SelectedLocationList = PendingStorageArticlesList.Where(pm => pm.FullName.Equals(ArticleLocationList[locationIndex].ToString())).Select(p => p).ToList();
                    LocationName = ArticleLocationList[locationIndex].ToString();
                    MapItems.Clear();
                    SvgUri = null;
                    // Location Might be suggested location if not not will be replace 
                    SaveAndLoadWarehouseLayoutFile(LocationName.Replace("?", ""));
                    IsWrongLocationErrorVisible = Visibility.Hidden;
                    IsWrongItemErrorVisible = Visibility.Hidden;
                    WrongLocation = "";
                    WrongItem = "";
                    //IsNotOk = false;
                    countLocation = 0;
                    TotalCurrentItemStock = SelectedLocationList[0].CurrentStock;
                    IsLocationIndicatorVisible = Visibility.Visible;
                    IsInformationVisible = Visibility.Collapsed;
                    IsLocationScaned = false;
                    TrackBarEditvalue = 0;
                    //BgColorLocation = "#FFF4E27E";
                    //RowBgColor = "#FFF4E27E";

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

                    Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.LocateMaterialsUserControlView", null, this);

                    if (ArticleLocationList.Count - 1 == locationIndex)
                        IsNextButtonEnable = false;
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

        private void GoToNextItem(PendingStorageArticles currentPendingStorageArticle)
        {
            int currentTransferMaterialIndex = SelectedLocationList.IndexOf(currentPendingStorageArticle);
            this.currentPendingStorageArticle = currentPendingStorageArticle;
            if (currentTransferMaterialIndex + 1 < SelectedLocationList.Count)
            {
                SelectedLocateMaterial = SelectedLocationList[currentTransferMaterialIndex + 1];
                currentTransferMaterialIndex++;
                ArticleImage = ByteArrayToBitmapImage(SelectedLocateMaterial.ArticleImageInBytes);
                TotalCurrentItemStock = SelectedLocateMaterial.CurrentStock;
                ChangeTotalStockColor(TotalCurrentItemStock, SelectedLocateMaterial.MinimumStock);
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

                if (currentTransferMaterialIndex + 1 == SelectedLocationList.Count)
                {
                    if (ArticleLocationList.Count - 1 == locationIndex)
                        IsNextButtonEnable = false;
                }
                else if (currentTransferMaterialIndex + 1 < SelectedLocationList.Count)
                    IsNextButtonEnable = true;
                else
                    IsNextButtonEnable = false;
            }
            else
            {
                if (IsNextButtonEnable)
                    GoToLocation();
            }
        }
        #endregion

        //#region Validation

        //bool allowValidation = false;
        //string EnableValidationAndGetError()
        //{
        //    allowValidation = true;
        //    string error = ((IDataErrorInfo)this).Error;
        //    if (!string.IsNullOrEmpty(error))
        //    {
        //        return error;
        //    }
        //    return null;
        //}

        //string IDataErrorInfo.Error
        //{
        //    get
        //    {
        //        if (!allowValidation) return null;
        //        IDataErrorInfo me = (IDataErrorInfo)this;
        //        string error =
        //             me[BindableBase.GetPropertyName(() => WrongLocation)] +
        //              me[BindableBase.GetPropertyName(() => WrongItem)];

        //        if (!string.IsNullOrEmpty(error))
        //            return "Please check inputted data.";

        //        return null;
        //    }
        //}
        //string IDataErrorInfo.this[string columnName]
        //{
        //    get
        //    {
        //        if (!allowValidation) return null;
        //        string WrongLocationProp = BindableBase.GetPropertyName(() => WrongLocation);
        //        string WrongItemProp = BindableBase.GetPropertyName(() => WrongItem);

        //        if (columnName== WrongLocationProp)
        //        {

        //            if (LocationName != BarcodeStr && BarcodeStr.ToString() != string.Empty)
        //            {
        //                return "Wrong location " + BarcodeStr;
        //            }
        //            else
        //            {
        //                return null;
        //                //return WarehouseValidation.GetErrorMessage(WrongLocationProp, WrongLocation);
        //            }  
        //        }
        //        else if (columnName == WrongItemProp)
        //        {
        //            //if (SelectedLocateMaterial != null && BarcodeStr.ToString() != string.Empty)
        //            //{
        //            //   return WarehouseValidation.GetErrorMessage(WrongItemProp, WrongItem);
        //            //}
        //            if (IsNotOk && BarcodeStr.ToString() != string.Empty)
        //                return "Wrong Item " + BarcodeStr;
        //            else
        //                return null;
        //        }
        //        return null;
        //    }
        //}

        //#endregion
    }
}

