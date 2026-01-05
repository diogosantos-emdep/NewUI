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
    public class StorageArticleViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region TaskLog
        //[Sprint_63] [14-05-2019]----(#65800) Storage of articles by item---[sdsai]
        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }
        IWorkbenchStartUp WorkbenchStartup = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion //End Of Services

        #region Declaration
        //private str imageInfo;
        private int windowWidth;
        private int windowHeight;
        private Visibility isInformationVisible;
        private string barcodeStr;
        private string locationName;
        private string totalStockBgColor;
        private string totalStockFgColor;
        private string fillingComment;
        private string isWrongLocationErrorVisible = null;
        private Visibility isWrongItemErrorVisible = Visibility.Hidden;
        private List<PendingStorageArticles> pendingStorageArticlesList;
        private string wrongItem;
        private bool isReferenceScanned;
        private ImageSource articleImage;
        private PendingStorageArticles selectedStorageMaterial;
        private List<PendingStorageArticles> storageArticlesList;
        private ObservableCollection<WarehouseLocation> mapItems;
        Uri svgUri;
        private Int64 totalCurrentItemStock;
        private ArticleWarehouseLocations progressbarArticleStock;
        private ArticleWarehouseLocations progressbarLocationStock;
        private long locationStockValue;
        private string locationStockBgColor;
        private string locationStockFgColor;
        private int trackBarEditvalue;
        private string wrongLocation;
        bool isSkipButtonEnable;
        private bool focusUserControl;
        private PendingStorageArticles tempPendingStorageArticle;
        private WarehouseLocation wlocation;
        private long scannedQuantity;
        private long scannedSerialQuantity;
        private string onFocusBgColor;
        private string onFocusFgColor;
        private string rowBgColor;
        private List<ArticleWarehouseLocations> articleWarehouseLocationsList;
        private bool isToolTipShow;
        private string toolTipVisibility;
        private string infoTooltipBackColor;
        private long idWareHouseDeliveryNoteItem;
        private SerialNumber selectedSerialNumber;
        private bool isScanSerialNumber;
        private string articleSerialNumbers;
        private string description;
        private List<SerialNumber> lstScanSerialNumber;



        #endregion

        #region Public Properties
        public string RowBgColor
        {
            get { return rowBgColor; }
            set
            {
                rowBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RowBgColor"));
            }
        }
        public Int64 IdTransitLocation { get; set; }

        public Boolean IsToolTipShow
        {
            get { return isToolTipShow; }
            set
            {
                isToolTipShow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsToolTipShow"));
            }
        }
        public String ToolTipVisibility
        {
            get { return toolTipVisibility; }
            set
            {
                toolTipVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTipVisibility"));
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
        public Visibility IsInformationVisible
        {
            get
            {
                return isInformationVisible;
            }

            set
            {
                isInformationVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInformationVisible"));
            }
        }

        public string BarcodeStr
        {
            get
            {
                return barcodeStr;
            }

            set
            {
                barcodeStr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BarcodeStr"));
            }
        }

        public string LocationName
        {
            get
            {
                return locationName;
            }

            set
            {
                locationName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationName"));
            }
        }

        public string TotalStockBgColor
        {
            get
            {
                return totalStockBgColor;
            }

            set
            {
                totalStockBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalStockBgColor"));
            }
        }

        public string TotalStockFgColor
        {
            get
            {
                return totalStockFgColor;
            }

            set
            {
                totalStockFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalStockFgColor"));
            }
        }
        public string IsWrongLocationErrorVisible
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
        public List<PendingStorageArticles> PendingStorageArticlesList
        {
            get
            {
                return pendingStorageArticlesList;
            }

            set
            {
                pendingStorageArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PendingStorageArticlesList"));
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
        public bool IsReferenceScanned
        {
            get
            {
                return isReferenceScanned;
            }

            set
            {
                isReferenceScanned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReferenceScanned"));
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
        public PendingStorageArticles SelectedStorageMaterial
        {
            get { return selectedStorageMaterial; }
            set
            {
                selectedStorageMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStorageMaterial"));
            }
        }
        public List<PendingStorageArticles> StorageArticlesList
        {
            get
            {
                return storageArticlesList;
            }

            set
            {
                storageArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StorageArticlesList"));
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
        public Int64 TotalCurrentItemStock
        {
            get { return totalCurrentItemStock; }
            set
            {
                totalCurrentItemStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCurrentItemStock"));
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
        public int TrackBarEditvalue
        {
            get { return trackBarEditvalue; }
            set
            {
                trackBarEditvalue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrackBarEditvalue"));
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
        public bool IsSkipButtonEnable
        {
            get
            {
                return isSkipButtonEnable;
            }

            set
            {
                isSkipButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSkipButtonEnable"));
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
            get
            {
                return onFocusFgColor;
            }

            set
            {
                onFocusFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OnFocusFgColor"));
            }
        }
        public List<ArticleWarehouseLocations> ArticleWarehouseLocationsList
        {
            get
            {
                return articleWarehouseLocationsList;
            }

            set
            {
                articleWarehouseLocationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleWarehouseLocationsList"));
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
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
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

        public List<SerialNumber> LstScanSerialNumber
        {
            get
            {
                return lstScanSerialNumber;
            }

            set
            {
                lstScanSerialNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstScanSerialNumber"));
            }
        }

        #endregion

        #region ICommands
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandOnLoaded { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }
        public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        public ICommand HyperlinkClickCommand { get; set; }
        public ICommand CommandSkipButton { get; set; }
        public ICommand ImageClickCommand { get; set; }
        public ICommand CommandKeyDown { get; set; }
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
        #endregion

        #region Constructor
        public StorageArticleViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor StorageArticleViewModel....", category: Category.Info, priority: Priority.Low);
                WindowHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 133;
                WindowWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 160;
                CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
                CommandOnLoaded = new DelegateCommand(LoadedAction);
                CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
                VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
                CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);
                HyperlinkClickCommand = new DelegateCommand<object>(HyperlinkClickCommandAction);
                CommandSkipButton = new DelegateCommand(CommandSkipButtonAction);
                CommandKeyDown = new DelegateCommand<object>(CommandKeyDownAction);
                ImageClickCommand = new DelegateCommand<object>(ImageClickCommandAction);
                IsInformationVisible = Visibility.Collapsed;
                IsReferenceScanned = false;
                TrackBarEditvalue = 0;
                IsSkipButtonEnable = false;
                GeosApplication.Instance.Logger.Log("Constructor StorageArticleViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor StorageArticleViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
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


        private void ImageClickCommandAction()
        {
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....", category: Category.Info, priority: Priority.Low);
            
            try
            {
                //((ToolTip)((FrameworkElement)sender).ToolTip).IsOpen = true;
                IsToolTipShow = true;
                ToolTipVisibility = "Visible";
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ImageClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// Method to load
        /// </summary>
        private void LoadedAction()
        {
            GeosApplication.Instance.Logger.Log("Method LoadedAction....", category: Category.Info, priority: Priority.Low);
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.StorageArticleUserControlView", null, this);
            GeosApplication.Instance.Logger.Log("Method LoadedAction....executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Method to Scan Barcode
        /// [001][cpatil][17-09-2020][GEOS2-2415]Add Date of Expiry in Article comments
        /// [002][vsana][22-11-2020][GEOS2-2426]AutoSort for the new locations created 
        /// [003][cpatil][20-05-2024][GEOS2-5632]
        /// [004][cpatil][GEOS2-6492][05-02-2025]
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
                if (obj.Text == "\r")
                {
                    #region Scan Location

                    IsWrongItemErrorVisible = Visibility.Hidden;
                    WrongItem = "";
                    if (!IsReferenceScanned)
                    {
                        var isProperBarcode = !string.IsNullOrEmpty(BarcodeStr) && BarcodeStr.All(Char.IsDigit);
                        if (BarcodeStr.Length < 11)
                        {
                            isProperBarcode = false;
                        }
                        if (isProperBarcode)
                        {
                            Int64 _idwarehouse = Convert.ToInt64(BarcodeStr.Substring(0, 3));
                            long _idWareHouseDeliveryNoteItem = long.Parse(BarcodeStr.Substring(3, 8));
                            scannedQuantity = Convert.ToInt64(BarcodeStr.Substring(11, 6));
                            //var transitLLocation = WarehouseService.GetWarehouseLocationByFullName(_idwarehouse, "TRANSIT");

                            //tempPendingStorageArticle = WarehouseService.GetArticleDetailByIdWarehouseDeliveryNoteItem_V2034(WarehouseCommon.Instance.Selectedwarehouse, _idWareHouseDeliveryNoteItem);

                            //[001]
                            // tempPendingStorageArticle = WarehouseService.GetArticleDetailByIdWarehouseDeliveryNoteItem_V2051(WarehouseCommon.Instance.Selectedwarehouse, _idWareHouseDeliveryNoteItem);
                            //[002]
                            tempPendingStorageArticle = WarehouseService.GetArticleDetailByIdWarehouseDeliveryNoteItem_V2150(WarehouseCommon.Instance.Selectedwarehouse, _idWareHouseDeliveryNoteItem);


                            if (tempPendingStorageArticle == null || scannedQuantity == 0)
                                SetWrongItemError();
                            else
                            {
                               // [003]
                                if (tempPendingStorageArticle.Quantity >= scannedQuantity)
                                {
                                    tempPendingStorageArticle.RemainingQtyAfterScan = tempPendingStorageArticle.Quantity - scannedQuantity;
                                    if (tempPendingStorageArticle.Quantity == scannedQuantity)
                                        tempPendingStorageArticle.IsRemainingQtyEqualScannedQty = true;
                                    else
                                        tempPendingStorageArticle.IsRemainingQtyEqualScannedQty = false;

                                    if(scannedQuantity==0)
                                    {
                                        tempPendingStorageArticle.IsRemainingQtyEqualScannedQty = false;
                                    }

                                    ScanAction();
                                    
                                        if (SelectedStorageMaterial.RegisterSerialNumber == 1)
                                            {
                                            if (SelectedStorageMaterial.ScanSerialNumbers == null)
                                                SelectedStorageMaterial.ScanSerialNumbers = new List<SerialNumber>();
                                            scannedSerialQuantity = SelectedStorageMaterial.Quantity;//All SerialNumbers can be Downloaded ntg to do with scanned number 
                                            SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = SelectedStorageMaterial };
                                            SelectedStorageMaterial.ScanSerialNumbers.Add(scanSerialNumber);
                                            SelectedSerialNumber = scanSerialNumber;
                                            IsScanSerialNumber = true;
                                           
                                    }

                                }
                                else
                                {
                                    SetWrongItemError();
                                    SelectedStorageMaterial = null;
                                }
                            }
                        }
                        else
                            SetWrongItemError();
                    }
                    else if (IsScanSerialNumber)
                    {
                        PendingStorageArticles pm = SelectedSerialNumber.MasterItem as PendingStorageArticles;

                        //Observation comment for serial number.
                        if (string.IsNullOrEmpty(articleSerialNumbers))
                            articleSerialNumbers = String.Format(Application.Current.FindResource("ArticleSerialNumbers").ToString() + "\r\n", pm.Reference);
                        if (pm.ScanSerialNumbers.Exists(x => x.Code == BarcodeStr))     //Show error if already scanned item.
                        {
                            IsWrongItemErrorVisible = Visibility.Visible;
                            WrongItem = "Wrong Serial Number " + BarcodeStr;
                            BarcodeStr = string.Empty;
                            PendingStorageArticlesList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            return;
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
                                    PendingStorageArticlesList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);

                                    return;
                                }
                                //show comment
                                PendingStorageArticlesList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });

                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                                SelectedSerialNumber.Code = BarcodeStr;
                                //assign all values to scanned serial number.
                                SelectedSerialNumber.IdSerialNumber = sn.IdSerialNumber;
                                SelectedSerialNumber.IdArticle = sn.IdArticle;
                                SelectedSerialNumber.IdWarehouseDeliveryNoteItem = sn.IdWarehouseDeliveryNoteItem;
                                SelectedSerialNumber.IdWarehouse = sn.IdWarehouse;
                                SelectedSerialNumber.IdWarehouseLocation = pm.IdWarehouseLocation;
                                SelectedSerialNumber.IsScanned = true;
                                //scannedSerialQuantity = scannedQuantity;
                                scannedSerialQuantity -= 1;
                                if (LstScanSerialNumber == null)
                                    LstScanSerialNumber = new List<SerialNumber>();

                                LstScanSerialNumber.Add(SelectedSerialNumber);
                                //TotalCurrentItemStock -= 1;
                                //bool IsScanItem = false;
                                //if (pm.ScannedQty != 0)
                                //{
                                //    IsScanItem = true;
                                //}

                                if (scannedSerialQuantity > 0)
                                {
                                    SerialNumber scanSerialNumber = new SerialNumber() { Code = null, MasterItem = pm };
                                    pm.ScanSerialNumbers.Add(scanSerialNumber);
                                    SelectedSerialNumber = scanSerialNumber;
                                    IsScanSerialNumber = true;
                                    BarcodeStr = string.Empty;
                                }
                                else if (scannedSerialQuantity == 0)
                                {
                                    BarcodeStr = string.Empty;
                                    IsScanSerialNumber = false;
                                }
                            }
                            else
                            {
                                //Show error message if scanned serial  is wrong.
                                IsWrongItemErrorVisible = Visibility.Visible;
                                WrongItem = "Wrong Serial Number " + BarcodeStr;
                                BarcodeStr = string.Empty;
                                PendingStorageArticlesList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                return;
                            }

                        }
                       
                    }
                    else
                    {
                        if (LocationName.Contains("?"))
                        {
                            //[001] Changed service method IsExistWarehouseLocationFullName to IsExistWarehouseLocationFullName_V2080
                            //[005][cpatil][GEOS2-6492][05-02-2025] Added one caondition not allow to add transit 
                            if (BarcodeStr.ToUpper()!="TRANSIT" && WarehouseService.IsExistWarehouseLocationFullName_V2080(BarcodeStr, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse))
                                SetLocationDetails();
                            else
                                SetWrongLocationError();
                        }
                        else if (BarcodeStr.Equals(LocationName))
                            SetLocationDetails();
                        else
                            SetWrongLocationError();

                    }
                    #endregion
                }
                else
                {
                    BarcodeStr = BarcodeStr + obj.Text;
                }
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed ScanBarcodeAction", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                BarcodeStr = string.Empty;
            }
        }

        public void Init(List<PendingStorageArticles> PendingStorageArticleList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init....", category: Category.Info, priority: Priority.Low);
                StorageArticlesList = PendingStorageArticleList;
                IdTransitLocation = StorageArticlesList[0].IdWarehouseLocation;
                PendingStorageArticlesList = new List<PendingStorageArticles>();
                PendingStorageArticlesList.Add(new PendingStorageArticles());

                ToolTipVisibility = "Hidden";

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
                //set info tooltip back color
                GeosAppSetting GeosAppSetting = WorkbenchStartup.GetGeosAppSettings(37);
                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;
                GeosApplication.Instance.Logger.Log("Method Init....executed ScanBarcodeAction", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        private void SaveAndLoadWarehouseLayoutFile(string warehouseLocationName)
        {
            GeosApplication.Instance.Logger.Log("Method SaveAndLoadWarehouseLayoutFile....", category: Category.Info, priority: Priority.Low);
            wlocation = WarehouseService.GetWarehouseLocationByFullName(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, warehouseLocationName);

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

                    if (articleDetailsViewModel.UpdateArticle.IsAddedArticleImage || articleDetailsViewModel.UpdateArticle.IsDeletedArticleImage)
                    {
                        SelectedStorageMaterial.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        ArticleImage = ByteArrayToImage(articleDetailsViewModel.UpdateArticle.ArticleImageInBytes);
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
        private void UpdateProgressbarLocationStock()
        {
            GeosApplication.Instance.Logger.Log("Method UpdateProgressbarLocationStock....", category: Category.Info, priority: Priority.Low);
            try
            {
                ProgressbarLocationStock = WarehouseService.GetArticleStockByWarehouseLocation(SelectedStorageMaterial.IdArticle, wlocation.IdWarehouseLocation, SelectedStorageMaterial.IdWarehouse);
                LocationStockValue = ProgressbarLocationStock.LocationStock;

                if (ProgressbarLocationStock.MaximumStock == 0)
                    LocationStockValue = 0;
                else if (ProgressbarLocationStock.MaximumStock < LocationStockValue)
                    LocationStockValue = ProgressbarLocationStock.MaximumStock;
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
        /// </summary>
        private void UpdateProgressbarArticleStock()
        {
            try
            {
                ProgressbarArticleStock = new ArticleWarehouseLocations();
                ProgressbarArticleStock = WarehouseService.GetAVGStockByIdArticle_V2034(SelectedStorageMaterial.IdArticle, WarehouseCommon.Instance.Selectedwarehouse);
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
        private void SetWrongItemError()
        {
            IsWrongItemErrorVisible = Visibility.Visible;
            WrongItem = "Wrong item " + BarcodeStr;
            BarcodeStr = string.Empty;
            PendingStorageArticlesList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
            return;
        }

        private void SetWrongLocationError()
        {
            IsWrongLocationErrorVisible = "Visible";
            WrongLocation = "Wrong Location " + BarcodeStr;
            BarcodeStr = string.Empty;
            PendingStorageArticlesList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
        }

        private void SetLocationDetails()
        {
            TrackBarEditvalue = 2;
            ScanAction();
            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
            WrongLocation = string.Empty;
            BarcodeStr = string.Empty;
        }

        
        private void ScanAction()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);
                if (!IsReferenceScanned)
                {
                    MapItems = new ObservableCollection<WarehouseLocation>();
                    PendingStorageArticlesList = new List<PendingStorageArticles>();
                    PendingStorageArticlesList.Add((PendingStorageArticles)tempPendingStorageArticle.Clone());
                    SelectedStorageMaterial = PendingStorageArticlesList[0];
                    ArticleImage = ByteArrayToImage(SelectedStorageMaterial.ArticleImageInBytes);
                    Description = SelectedStorageMaterial.Description;
                    SelectedStorageMaterial.Quantity = scannedQuantity;

                    //[rdixit][08.02.2023][GEOS2-4133] Changed Service method to Version V2360
                    //ArticleWarehouseLocationsList = WarehouseService.GetArticlesWarehouseLocation_V2360(tempPendingStorageArticle.IdArticle.ToString(), WarehouseCommon.Instance.Selectedwarehouse);
                    //[pramod.misal][GEOS2-5524][17.05.2024] Changed Service method to Version V2520
                    //ArticleWarehouseLocationsList = WarehouseService.GetArticlesWarehouseLocation_V2520(tempPendingStorageArticle.IdArticle.ToString(), WarehouseCommon.Instance.Selectedwarehouse, SelectedStorageMaterial.Quantity);
                    //WarehouseService = new WarehouseServiceController("localhost:6699");
                    //Shubham[skadam] GEOS2-5992 Improvements in the filling criteria developed in Ticket IESD-96777 22 11 2024.
                    ArticleWarehouseLocationsList = WarehouseService.GetArticlesWarehouseLocation_V2580_New(tempPendingStorageArticle.IdArticle.ToString(), WarehouseCommon.Instance.Selectedwarehouse, SelectedStorageMaterial.Quantity);
                    if (ArticleWarehouseLocationsList.Count > 0)
                    {
                        if (ArticleWarehouseLocationsList[0].WarehouseLocation?.FullName != null)
                            LocationName = ArticleWarehouseLocationsList[0].WarehouseLocation.FullName;
                        else
                        {
                            List<WarehouseLocation> suggestedLocations;
                            //[rdixit][08.02.2023][GEOS2-4133] Changed Service method to Version V2360
                            suggestedLocations = WarehouseService.GetWarehouseLocationToPlaceArticle_V2360(WarehouseCommon.Instance.Selectedwarehouse, SelectedStorageMaterial.Reference);
                            if (suggestedLocations.Count > 0)
                            {
                                LocationName = "?" + suggestedLocations[0].FullName;
                            }
                            else
                            {
                                LocationName = "?";
                            }
                        }
                    }
                    else
                    {
                        List<WarehouseLocation> suggestedLocations;
                        //[rdixit][08.02.2023][GEOS2-4133] Changed Service method to Version V2360
                        suggestedLocations = WarehouseService.GetWarehouseLocationToPlaceArticle_V2360(WarehouseCommon.Instance.Selectedwarehouse, SelectedStorageMaterial.Reference);
                        if (suggestedLocations.Count > 0)
                        {
                            LocationName = "?" + suggestedLocations[0].FullName;
                        }
                        else
                        {
                            LocationName = "?";
                        }
                    }
                    
                    SaveAndLoadWarehouseLayoutFile(LocationName.Replace("?", ""));
                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedStorageMaterial.IdArticle, (int)SelectedStorageMaterial.IdWarehouse);
                    ChangeTotalStockColor(TotalCurrentItemStock, SelectedStorageMaterial.MinimumStock);

                    UpdateProgressbarLocationStock();
                    UpdateProgressbarArticleStock();
                    Int64 minstock = WarehouseService.GetMinimumStockByLocationFullName(WarehouseCommon.Instance.Selectedwarehouse, wlocation.FullName, SelectedStorageMaterial.IdArticle);
                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, minstock);
                    //show comment
                    PendingStorageArticlesList.Where(a => a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                    PendingStorageArticlesList.Where(a => a.IdWareHouseDeliveryNoteItem == SelectedStorageMaterial.IdWareHouseDeliveryNoteItem && SelectedStorageMaterial.ArticleComment!=null).ToList().ForEach(a => { a.ShowComment = true; });
                    GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                    

                    TrackBarEditvalue = 1;
                    IsReferenceScanned = true;
                    IsInformationVisible = Visibility.Visible;
                    BarcodeStr = string.Empty;
                    IsSkipButtonEnable = true;
                    IsWrongLocationErrorVisible = "Hidden";
                   
                }
                else
                {
                    tempPendingStorageArticle.FullName = LocationName.Replace("?", "");
                    if (LocationName.Contains("?"))
                    {

                        SaveAndLoadWarehouseLayoutFile(BarcodeStr);
                        tempPendingStorageArticle.WarehouseLocation = wlocation;
                        tempPendingStorageArticle.IdWarehouseLocation = tempPendingStorageArticle.WarehouseLocation.IdWarehouseLocation;
                        SuggestedLocationEntity();
                    }
                    tempPendingStorageArticle.ScannedQty = scannedQuantity;
                    tempPendingStorageArticle.IdWarehouseLocation = wlocation.IdWarehouseLocation;
                    tempPendingStorageArticle.Comments = "Transfer from " + "\"" + "Transit" + "\"" + " location to " + "\"" + BarcodeStr + "\"";
                    tempPendingStorageArticle.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    tempPendingStorageArticle.UnitPrice = Math.Round(tempPendingStorageArticle.UnitPrice, 4);
                    tempPendingStorageArticle.CostPrice = Math.Round(tempPendingStorageArticle.CostPrice, 4);
                    tempPendingStorageArticle.ScanSerialNumbers = LstScanSerialNumber;

                    PendingStorageArticles psa = (PendingStorageArticles)tempPendingStorageArticle.Clone();
                    psa.ScannedQty = (-1) * psa.ScannedQty;
                    psa.IdWarehouseLocation = WarehouseCommon.Instance.Selectedwarehouse.IdTransitLocation.Value;//[rdixit][GEOS2-3868 & GEOS2-4051][19.01.2023]
                  
                    bool IsdiductFromtransit = WarehouseService.InsertIntoArticleStockForLocateMaterialForTransit(psa);
                    if (IsdiductFromtransit)
                    {
                        bool IsInsertArticletoLocation = WarehouseService.InsertIntoArticleStockForLocateMaterial_V2150(tempPendingStorageArticle);
                        LstScanSerialNumber = new List<SerialNumber>();
                        tempPendingStorageArticle.Quantity = tempPendingStorageArticle.Quantity - scannedQuantity;
                        if (tempPendingStorageArticle.Quantity == 0)
                            StorageArticlesList.Remove(StorageArticlesList.FirstOrDefault(x => x.IdArticle == tempPendingStorageArticle.IdArticle && x.IdWareHouseDeliveryNoteItem == tempPendingStorageArticle.IdWareHouseDeliveryNoteItem));
                        CommandSkipButtonAction();
                    }
                    else
                    {
                        CommandSkipButtonAction();
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ScanAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  ScanAction Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CommandSkipButtonAction()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandSkipButtonAction....", category: Category.Info, priority: Priority.Low);
                PendingStorageArticlesList = new List<PendingStorageArticles>();
                PendingStorageArticlesList.Add(new PendingStorageArticles());
                SelectedStorageMaterial = null;
                IsInformationVisible = Visibility.Collapsed;
                IsReferenceScanned = false;
                TrackBarEditvalue = 0;
                IsSkipButtonEnable = false;
                LocationName = string.Empty;
                IsWrongLocationErrorVisible = "null";
                WrongLocation = string.Empty;
                WrongItem = "";
                Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.StorageArticleUserControlView", null, this);
                GeosApplication.Instance.Logger.Log("Method CommandSkipButtonAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandSkipButtonAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  articleWarehouseLocations auto updateing the Suggested location for firsttime 
        /// </summary>
        private void SuggestedLocationEntity()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SuggestedLocationEntity....", category: Category.Info, priority: Priority.Low);
                ArticleWarehouseLocations articleWarehouseLocations = new ArticleWarehouseLocations { IdArticle = tempPendingStorageArticle.IdArticle, WarehouseLocation = new WarehouseLocation { FullName = BarcodeStr, IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse }, Position = 0, MinimumStock = 0 };
                //Service Method is changed from AddArticleWarehouseLocationByFullName to AddArticleWarehouseLocationByFullName_V2360
                WarehouseService.AddArticleWarehouseLocationByFullName_V2360(articleWarehouseLocations);
                Article article = new Article();
                article.LstArticleWarehouseLocations = null;
                article.MyWarehouse = null;
                article.IsAddedArticleImage = false;
                article.IsDeletedArticleImage = false;
                List<LogEntriesByArticle> ArticleChangeLogList = new List<LogEntriesByArticle>();
                ArticleChangeLogList.Add(new LogEntriesByArticle() { IdArticle = tempPendingStorageArticle.IdArticle, IdLogEntryType = 1, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleLocationAdd").ToString(), tempPendingStorageArticle.WarehouseLocation.FullName, WarehouseCommon.Instance.Selectedwarehouse.Name) });
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

        private void ImageClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....", category: Category.Info, priority: Priority.Low);

            try
            {
                PendingStorageArticles pendingStorageArticle = (PendingStorageArticles)obj;
                IdWareHouseDeliveryNoteItem = pendingStorageArticle.IdWareHouseDeliveryNoteItem;
                PendingStorageArticlesList.Where(a => a.IdWareHouseDeliveryNoteItem != IdWareHouseDeliveryNoteItem && a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                PendingStorageArticlesList.Where(a => a.IdWareHouseDeliveryNoteItem == IdWareHouseDeliveryNoteItem).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
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
                        PendingStorageArticlesList.Where(a => a.IdWareHouseDeliveryNoteItem == IdWareHouseDeliveryNoteItem).ToList().ForEach(a => { a.ShowComment = false; });
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
