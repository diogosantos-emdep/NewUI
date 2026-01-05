using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm.Native;
using System.Windows;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.IO;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Map;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Warehouse.Views;
using System.Threading;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Grid;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class InventoryViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region TaskLogs

        /// <summary>
        /// GEOS2-65 Inventory wizzard [adadibathina]
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>

        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region public Events
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

        #region Declaration

        private bool enableAccept;
        private int trackBarEditvalue;
        private string trackBarFgColor;
        private string fgColorLocation;
        private int windowWidth;
        private int windowHeight;
        private Visibility isInformationVisible;
        private Uri svgUri;
        private string locationName;
        private WarehouseLocation scannedWarehouseLocation;
        ObservableCollection<InventoryMaterial> inventoryMaterials;
        InventoryMaterial selectedInventoryMaterial;
        private string bgColorLocation;
        private ArticleWarehouseLocations progressbarLocationStock;
        private Visibility isWrongLocationErrorVisible;
        private string wrongLocation;
        private string wrongItem;
        private Visibility isWrongItemErrorVisible;
        private ImageSource articleImage;
        private long locationStockValue;
        private bool focusUserControl;
        private ArticleWarehouseLocations progressbarArticleStock;
        private string rowBgColor;
        private string onFocusBgColor;
        private string onFocusFgColor;
        private string locationStockBgColor;
        private string locationStockFgColor;
        private Int64 totalCurrentItemStock;
        private string totalStockFgColor;
        private string totalStockBgColor;
        private bool isLocationScanned;
        private ObservableCollection<WarehouseLocation> mapItems;
        private bool enabledGridFormatCondition;
        private ObservableCollection<LookupValue> reasonList;

        //[000] added
        private WarehouseInventoryAudit selectedWarehouseInventoryAudit;
        private bool approvedColumnVisibility;
        private bool isOKColumnVisibility;
        private List<WarehouseInventoryAuditItem> warehouseInventoryAuditItems; //This is used to get previously saved data
        #endregion

        #region Public Properties

        private string BarcodeStr { get; set; }

        public bool IsSaved { get; set; }

        Dictionary<long, int> ToatalStock { get; set; }

        public long PriorIdWareHouseDeliveryNoteItem { get; set; }

        public string LocationStockBgColor
        {
            get { return locationStockBgColor; }
            set
            {
                locationStockBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationStockBgColor"));
            }
        }

        public bool EnableAccept
        {
            get { return enableAccept; }
            set
            {
                enableAccept = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnableAccept"));
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

        public bool IsLocationScanned
        {
            get { return isLocationScanned; }
            set
            {
                isLocationScanned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocationScanned"));
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

        public Int64 TotalCurrentItemStock
        {
            get { return totalCurrentItemStock; }
            set
            {
                totalCurrentItemStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCurrentItemStock"));
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

        public string OnFocusFgColor
        {
            get { return onFocusFgColor; }
            set
            {
                onFocusFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OnFocusFgColor"));
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

        public string RowBgColor
        {
            get { return rowBgColor; }
            set
            {
                rowBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RowBgColor"));
            }
        }

        public InventoryMaterial SelectedInventoryMaterial
        {
            get { return selectedInventoryMaterial; }
            set
            {
                selectedInventoryMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedInventoryMaterial"));
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

        public ObservableCollection<InventoryMaterial> InventoryMaterials
        {
            get { return inventoryMaterials; }
            set
            {
                inventoryMaterials = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InventoryMaterials"));
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

        public string LocationName
        {
            get { return locationName; }
            set
            {
                locationName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationName"));
            }
        }

        public WarehouseLocation ScannedWarehouseLocation
        {
            get { return scannedWarehouseLocation; }
            set { scannedWarehouseLocation = value; }
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

        public Visibility IsInformationVisible
        {
            get { return isInformationVisible; }
            set
            {
                isInformationVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInformationVisible"));
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

        public Visibility IsWrongItemErrorVisible
        {
            get { return isWrongItemErrorVisible; }
            set
            {
                isWrongItemErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongItemErrorVisible"));
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

        public ArticleWarehouseLocations ProgressbarLocationStock
        {
            get { return progressbarLocationStock; }
            set
            {
                progressbarLocationStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressbarLocationStock"));
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

        public bool EnabledGridFormatCondition
        {
            get { return enabledGridFormatCondition; }
            set
            {
                enabledGridFormatCondition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnabledGridFormatCondition"));
            }
        }

        public ObservableCollection<LookupValue> ReasonList
        {
            get { return reasonList; }
            set
            {
                reasonList = value;
                RaisePropertyChanged("ReasonList");
            }
        }

        //[000]added
        public WarehouseInventoryAudit SelectedWarehouseInventoryAudit
        {
            get { return selectedWarehouseInventoryAudit; }
            set { selectedWarehouseInventoryAudit = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedWarehouseInventoryAudit")); }
        }

        public bool ApprovedColumnVisibility
        {
            get { return approvedColumnVisibility; }
            set
            {
                approvedColumnVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ApprovedColumnVisibility"));
            }
        }

        public bool IsOKColumnVisibility
        {
            get { return isOKColumnVisibility; }
            set
            {
                isOKColumnVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOKColumnVisibility"));
            }
        }

        public List<WarehouseInventoryAuditItem> WarehouseInventoryAuditItems
        {
            get { return warehouseInventoryAuditItems; }
            set
            {
                warehouseInventoryAuditItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseInventoryAuditItems"));
            }
        }

        #endregion

        #region ICommands

        public ICommand CommandCancelButton { get; set; }
        public ICommand HyperlinkClickCommand { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandOnLoaded { get; set; }
        public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }
        public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        public ICommand CommandAcceptButton { get; set; }
        public ICommand ReasonCellValueChanged { get; set; }

        #endregion // ICommands

        #region Constructor

        public InventoryViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor InventoryViewModel()...", category: Category.Info, priority: Priority.Low);

            WindowHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 90;
            WindowWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 100;
            IsInformationVisible = Visibility.Collapsed;
            IsWrongItemErrorVisible = Visibility.Hidden;
            IsWrongItemErrorVisible = Visibility.Hidden;
            IsWrongLocationErrorVisible = Visibility.Hidden;
            MapItems = new ObservableCollection<WarehouseLocation>();
            FillReasonsList();
            CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandOnLoaded = new DelegateCommand(LoadedAction);
            VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
            CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);
            CommandAcceptButton = new DelegateCommand<object>(AcceptAction);
            HyperlinkClickCommand = new DelegateCommand<object>(HyperlinkClickCommandAction);
            ReasonCellValueChanged = new DelegateCommand<object>(CellValueChangingCommandAction);

            GeosApplication.Instance.Logger.Log("Constructor InventoryViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="SelectedInventoryAuditItem"></param>
        public void Init(WarehouseInventoryAudit SelectedInventoryAudit)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                SelectedWarehouseInventoryAudit = (WarehouseInventoryAudit)SelectedInventoryAudit.Clone();

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Scan Barcode
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                if (obj.Text == "\r" && IsLocationScanned == false)
                {
                    LocationName = string.Copy(BarcodeStr);

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

                    if (WarehouseService.IsExistWarehouseLocationFullName(BarcodeStr, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse))
                    {
                        TrackBarEditvalue = 1;
                        LoadWarehouseLayoutFile(LocationName);

                        //[002] FirstTime make remaining qty as
                        InventoryMaterials = new ObservableCollection<InventoryMaterial>(WarehouseService.GetInventoryArticleByIdWarehouseLocation(WarehouseCommon.Instance.Selectedwarehouse, ScannedWarehouseLocation.IdWarehouseLocation));
                        warehouseInventoryAuditItems = new List<WarehouseInventoryAuditItem>(WarehouseService.GetWarehouseInventoryAuditItemsByInventoryAudit(WarehouseCommon.Instance.Selectedwarehouse, SelectedWarehouseInventoryAudit, ScannedWarehouseLocation));

                        if (warehouseInventoryAuditItems.Count > 0)
                        {
                            foreach (WarehouseInventoryAuditItem auditItem in warehouseInventoryAuditItems)
                            {
                                InventoryMaterial inventoryMaterial = InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == auditItem.IdWarehouseDeliveryNoteItem && x.IdArticle == auditItem.IdArticle && x.IdWarehouseLocation == auditItem.IdWarehouseLocation);

                                if (inventoryMaterial != null)
                                {
                                    inventoryMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    inventoryMaterial.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                                    inventoryMaterial.IdReason = auditItem.IdReason;
                                    inventoryMaterial.Reason = ReasonList.FirstOrDefault(x => x.IdLookupValue == auditItem.IdReason);
                                    inventoryMaterial.ScannedQty = auditItem.CurrentQuantity;
                                    inventoryMaterial.AvailableQty = auditItem.ExpectedQuantity;
                                    inventoryMaterial.RemainingQty = auditItem.CurrentQuantity - auditItem.ExpectedQuantity;
                                    inventoryMaterial.WarehouseInventoryAuditItem = auditItem;
                                    inventoryMaterial.IsApproved = auditItem.IdApprover > 0 ? true : false;

                                    if (auditItem.IsOK == 1)
                                    {
                                        inventoryMaterial.IsOK = true;
                                    }
                                    else if (auditItem.IsOK == 0)
                                    {
                                        inventoryMaterial.IsOK = false;
                                    }
                                }
                                else
                                {
                                    InventoryMaterial iMaterial = new InventoryMaterial();
                                    iMaterial.IdArticle = auditItem.IdArticle;

                                    if (auditItem.Article != null)
                                    {
                                        iMaterial.Reference = auditItem.Article.Reference;

                                        if (auditItem.Article.ArticlesStock != null)
                                        {
                                            iMaterial.UnitPrice = auditItem.Article.ArticlesStock.UnitPrice;
                                            iMaterial.CostPrice = auditItem.Article.ArticlesStock.Price;
                                            iMaterial.IdCurrency = auditItem.Article.ArticlesStock.IdCurrency;
                                        }
                                    }

                                    iMaterial.IdWareHouseDeliveryNoteItem = auditItem.IdWarehouseDeliveryNoteItem;
                                    iMaterial.WarehouseDeliveryNote = auditItem.WarehouseDeliveryNoteItem.WarehouseDeliveryNote;
                                    iMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    iMaterial.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                                    iMaterial.IdReason = auditItem.IdReason;
                                    iMaterial.Reason = ReasonList.FirstOrDefault(x => x.IdLookupValue == auditItem.IdReason);
                                    iMaterial.ScannedQty = auditItem.CurrentQuantity;
                                    iMaterial.AvailableQty = auditItem.ExpectedQuantity;
                                    iMaterial.RemainingQty = auditItem.CurrentQuantity - auditItem.ExpectedQuantity;
                                    iMaterial.WarehouseInventoryAuditItem = auditItem;
                                    iMaterial.IsApproved = auditItem.IdApprover > 0 ? true : false;

                                    if (auditItem.IsOK == 1)
                                    {
                                        iMaterial.IsOK = true;
                                    }
                                    else if (auditItem.IsOK == 0)
                                    {
                                        iMaterial.IsOK = false;
                                    }

                                    iMaterial.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                                    InventoryMaterials.Add(iMaterial);
                                }
                            }
                        }
                        else
                        {
                            InventoryMaterials.ForEach(x =>
                            {
                                x.RemainingQty = x.AvailableQty * -1;
                                x.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                x.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                            });
                        }

                        if (InventoryMaterials.Count > 0)
                        {
                            if (InventoryMaterials.Any(x => x.IsOK != null))
                                IsOKColumnVisibility = true;

                            if (InventoryMaterials.Any(x => x.IsApproved == true))
                                ApprovedColumnVisibility = true;
                        }

                        //end

                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        IsInformationVisible = Visibility.Collapsed;
                        IsWrongLocationErrorVisible = Visibility.Collapsed;
                        IsLocationScanned = true;
                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        IsWrongLocationErrorVisible = Visibility.Visible;
                        WrongLocation = "Wrong Location " + BarcodeStr;
                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                    }

                    BarcodeStr = string.Empty;
                }
                else if (obj.Text == "\r" && IsLocationScanned == true)
                {
                    var isProperBarcode = !string.IsNullOrEmpty(BarcodeStr) && BarcodeStr.All(Char.IsDigit);
                    if (BarcodeStr.Length < 17)
                    {
                        isProperBarcode = false;
                    }

                    if (isProperBarcode)
                    {
                        Int64 _idwarehouse = Convert.ToInt64(BarcodeStr.Substring(0, 3));
                        Int64 _idWareHouseDeliveryNoteItem = Convert.ToInt64(BarcodeStr.Substring(3, 8));
                        Int64 _itemQuantity = Convert.ToInt64(BarcodeStr.Substring(11, 6));

                        if (InventoryMaterials.Any(x => x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem && x.IsApproved == true))
                        {
                            SelectedInventoryMaterial = InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem);
                            IsWrongItemErrorVisible = Visibility.Visible;
                            WrongItem = "This item is approved";
                            GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                            return;
                        }

                        if (!InventoryMaterials.Any(x => x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem))
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
                                    return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                                }, null, null);
                            }

                            // [001] Changed Service method
                            List<InventoryMaterial> tempInventoryMaterials = WarehouseService.GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2034(WarehouseCommon.Instance.Selectedwarehouse, _idWareHouseDeliveryNoteItem);
                            tempInventoryMaterials.ForEach(x =>
                            {
                                x.AvailableQty = 0;
                                x.RemainingQty = x.AvailableQty * -1;
                                x.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            });

                            InventoryMaterials.AddRange(tempInventoryMaterials);

                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }

                        if (InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem) != null && _itemQuantity > 0)
                        {
                            IsWrongItemErrorVisible = Visibility.Collapsed;
                            ScanAction(_idWareHouseDeliveryNoteItem, _itemQuantity, _idwarehouse);
                        }
                        else
                        {
                            isProperBarcode = false;
                        }
                    }

                    if (isProperBarcode == false)
                    {
                        IsWrongItemErrorVisible = Visibility.Visible;
                        WrongItem = "Wrong Item " + BarcodeStr;
                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                    }

                    BarcodeStr = string.Empty;
                }
                else
                {
                    BarcodeStr += obj.Text;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error  ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed ScanBarcodeAction", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// ScanAction
        /// scanned proper barcode item
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="idWareHouseDeliveryNoteItem"></param>
        /// <param name="quantity"></param>
        /// <param name="IdwarehouseLocarion"></param>
        private void ScanAction(Int64 idWareHouseDeliveryNoteItem, Int64 quantity, Int64 IdwarehouseLocarion)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);
                IsInformationVisible = Visibility.Visible;
                EnableAccept = true;

                SelectedInventoryMaterial = InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == idWareHouseDeliveryNoteItem);
                ArticleImage = WarehouseCommon.Instance.ByteArrayToImage(SelectedInventoryMaterial.ArticleImageInBytes);
                SelectedInventoryMaterial.RemainingQty += quantity;
                SelectedInventoryMaterial.ScannedQty += quantity;

                ////[000]added
                //if (SelectedInventoryMaterial.WarehouseInventoryAuditItem != null)
                //{
                //    if (SelectedInventoryMaterial.ScannedQty == SelectedInventoryMaterial.AvailableQty)
                //    {
                //        SelectedInventoryMaterial.WarehouseInventoryAuditItem.IsOK = 1;
                //    }
                //    else
                //        SelectedInventoryMaterial.WarehouseInventoryAuditItem.IsOK = 0;
                //}//end

                if (PriorIdWareHouseDeliveryNoteItem != idWareHouseDeliveryNoteItem)
                {
                    PriorIdWareHouseDeliveryNoteItem = idWareHouseDeliveryNoteItem;
                    UpdateProgressbarArticleStock();
                    UpdateProgressbarLocationStock();
                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedInventoryMaterial.AwlMinimumStock);
                    changeTotalStockColor(TotalCurrentItemStock, SelectedInventoryMaterial.MinimumStock);
                }

                GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction Method ScanReference() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction Method ScanReference() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error  ScanAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ScanAction....executed ScanBarcodeAction", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// HyperlinkClick To open article
        /// </summary>
        /// <param name="obj"></param>
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

                InventoryMaterial article = (InventoryMaterial)obj;
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
                        InventoryMaterial inventoryMaterial = InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == article.IdWareHouseDeliveryNoteItem);
                        inventoryMaterial.ArticleImageInBytes = articleDetailsViewModel.UpdateArticle.ArticleImageInBytes;
                        if (inventoryMaterial.IdWareHouseDeliveryNoteItem == SelectedInventoryMaterial.IdWareHouseDeliveryNoteItem)
                            ArticleImage = WarehouseCommon.Instance.ByteArrayToImage(articleDetailsViewModel.UpdateArticle.ArticleImageInBytes);
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
        /// to update Progressbar of LocationStock
        /// </summary>
        private void UpdateProgressbarLocationStock()
        {
            GeosApplication.Instance.Logger.Log("Method UpdateProgressbarLocationStock....", category: Category.Info, priority: Priority.Low);
            try
            {
                ProgressbarLocationStock = WarehouseService.GetArticleStockByWarehouseLocation(SelectedInventoryMaterial.IdArticle, ScannedWarehouseLocation.IdWarehouseLocation, SelectedInventoryMaterial.IdWarehouse);
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
                GeosApplication.Instance.Logger.Log("Get an error in UpdateProgressbarLocationStock Method ScanReference() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateProgressbarLocationStock Method ScanReference() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(SelectedInventoryMaterial.IdArticle, Convert.ToInt32(SelectedInventoryMaterial.IdWarehouse));
                // [001] Changed Service method
                ProgressbarArticleStock = WarehouseService.GetAVGStockByIdArticle_V2034(SelectedInventoryMaterial.IdArticle, WarehouseCommon.Instance.Selectedwarehouse);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateProgressbarArticleStock Method ScanReference() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateProgressbarArticleStock Method ScanReference() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingMaterialsViewModel Method UpdateProgressbarArticleStock...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// ChangeLocationStockColor
        /// </summary>
        /// <param name="locationStock"></param>
        /// <param name="minQuantity"></param>
        private void ChangeLocationStockColor(Int64 locationStock, Int64 minQuantity)
        {
            GeosApplication.Instance.Logger.Log("Method ChangeLocationStockColor....", category: Category.Info, priority: Priority.Low);
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
            GeosApplication.Instance.Logger.Log("Method ChangeLocationStockColor....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// changeTotalStockColor
        /// </summary>
        /// <param name="totalStock"></param>
        /// <param name="minQuantity"></param>
        private void changeTotalStockColor(Int64 totalStock, Int64 minQuantity)
        {
            GeosApplication.Instance.Logger.Log("Method changeTotalStockColor....", category: Category.Info, priority: Priority.Low);
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
            GeosApplication.Instance.Logger.Log("Method changeTotalStockColor....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Load Layout
        /// </summary>
        /// <param name="warehouseLocationName"></param>
        private void LoadWarehouseLayoutFile(string warehouseLocationName)
        {
            GeosApplication.Instance.Logger.Log("Method SaveAndLoadWarehouseLayoutFile....", category: Category.Info, priority: Priority.Low);
            MapItems.Clear();
            ScannedWarehouseLocation = WarehouseService.GetWarehouseLocationByFullName(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, warehouseLocationName);
            try
            {
                MapItems.Add(ScannedWarehouseLocation);
                byte[] warehouseLayout = GeosRepositoryService.GetCompanyLayoutFile(ScannedWarehouseLocation.FileName);

                if (warehouseLayout != null)
                {
                    string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }

                    string svgFilePath = string.Format(@"{0}\Data\{1}", Path.GetTempPath(), ScannedWarehouseLocation.FileName);
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
                GeosApplication.Instance.Logger.Log(string.Format("Error in Loading warehouse layout svg file - {0}. ErrorMessage- {1}", warehouseLocationName, ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method SaveAndLoadWarehouseLayoutFile....executed successfully", category: Category.Info, priority: Priority.Low);
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

        private void LoadedAction()
        {
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.InventoryUserControlView", null, this);
        }

        /// <summary>
        /// Accept Click
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        private void AcceptAction(object obj)
        {
            try
            {
                TableView inventoryTabelView = (TableView)obj;
                inventoryTabelView.ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.RowAndCell;

                if (InventoryMaterials.Any(x => x.RemainingQty != 0 && x.IdReason <= 0))
                {
                    FocusUserControl = true;
                    return;
                }

                //WarehouseCommon.Instance.IsStockRegularizationApproval = true;

                MessageBoxResult messageBoxResult = MessageBoxResult.No;
                if (WarehouseCommon.Instance.IsStockRegularizationApproval)
                    messageBoxResult = CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("InventoryRegularizeStock").ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);

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

                // If found any warehousedeliverynoteitem which is not registered in db then add seperate entry for this.
                if (InventoryMaterials.Any(x => x.IdWarehouseLocation == 0))
                {
                    List<InventoryMaterial> inventoryMaterialList = InventoryMaterials.Where(x => x.IdWarehouseLocation == 0).ToList();

                    List<ArticleWarehouseLocations> tempArticleWarehouseLocations = new List<ArticleWarehouseLocations>();
                    foreach (InventoryMaterial inventoryMaterial in inventoryMaterialList)
                    {
                        ArticleWarehouseLocations articleWarehouseLocation = new ArticleWarehouseLocations();
                        articleWarehouseLocation.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                        articleWarehouseLocation.IdArticle = inventoryMaterial.IdArticle;
                        tempArticleWarehouseLocations.Add(articleWarehouseLocation);
                        inventoryMaterial.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                        inventoryMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    }

                    List<ArticleWarehouseLocations> articleWarehouseLocationsList = WarehouseService.AddArticleWarehouseLocationIFNotExist(tempArticleWarehouseLocations);
                }

                //[000]added
                List<WarehouseInventoryAuditItem> tempwarehouseInventoryAuditItemsList = new List<WarehouseInventoryAuditItem>();

                foreach (InventoryMaterial inventoryMaterial in InventoryMaterials)
                {
                    if (inventoryMaterial.IsApproved)
                        continue;

                    WarehouseInventoryAuditItem WarehouseInventoryAuditItem = new WarehouseInventoryAuditItem();

                    WarehouseInventoryAuditItem.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                    WarehouseInventoryAuditItem.IdArticle = inventoryMaterial.IdArticle;
                    WarehouseInventoryAuditItem.IdWarehouseDeliveryNoteItem = inventoryMaterial.IdWareHouseDeliveryNoteItem;

                    WarehouseInventoryAuditItem.ExpectedQuantity = inventoryMaterial.AvailableQty;
                    WarehouseInventoryAuditItem.CurrentQuantity = inventoryMaterial.ScannedQty;

                    if (inventoryMaterial.AvailableQty == inventoryMaterial.ScannedQty)
                        WarehouseInventoryAuditItem.IsOK = 1;
                    else
                        WarehouseInventoryAuditItem.IsOK = 0;

                    if (messageBoxResult == MessageBoxResult.Yes && WarehouseCommon.Instance.IsStockRegularizationApproval)
                        WarehouseInventoryAuditItem.IdApprover = GeosApplication.Instance.ActiveUser.IdUser;
                    else
                        WarehouseInventoryAuditItem.IdApprover = 0;

                    WarehouseInventoryAuditItem.IdReporter = GeosApplication.Instance.ActiveUser.IdUser;

                    WarehouseInventoryAuditItem.IdReason = inventoryMaterial.IdReason;

                    WarehouseInventoryAuditItem.IdModifier = GeosApplication.Instance.ActiveUser.IdUser;
                    WarehouseInventoryAuditItem.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                    WarehouseInventoryAuditItem.IdWarehouseInventoryAudit = SelectedWarehouseInventoryAudit.IdWarehouseInventoryAudit;

                    tempwarehouseInventoryAuditItemsList.Add(WarehouseInventoryAuditItem);
                }

                if (tempwarehouseInventoryAuditItemsList.Count > 0)
                {
                    WarehouseService.AddUpdateWarehouseInventoryAuditItems(WarehouseCommon.Instance.Selectedwarehouse, tempwarehouseInventoryAuditItemsList);
                }

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    EnabledGridFormatCondition = true;
                    TrackBarEditvalue = 2;
                    SelectedInventoryMaterial = null;

                    List<InventoryMaterial> inventoryMaterials = (WarehouseService.InsertInventoryMaterialIntoArticleStock_V2034(InventoryMaterials.Where(x => x.RemainingQty != 0 && !x.IsApproved).ToList()));

                    if (inventoryMaterials.Count > 0)
                    {
                        string WarningMessage = "Stock Regularization Failed For :" + "\n";

                        inventoryMaterials.ForEach(x =>
                        {
                            WarningMessage = WarningMessage + string.Format("Reference - {0}  Article Current Stock - {1} Diff - {2} \n", x.Reference, x.ArticleTotalStock, x.RemainingQty);
                        });

                        CustomMessageBox.Show(WarningMessage, Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                new Thread(() =>
                    {
                        Thread.Sleep(1000);
                        LoadedAction();
                        SvgUri = null;
                        InventoryMaterials = new ObservableCollection<InventoryMaterial>();
                        EnabledGridFormatCondition = false;
                        EnableAccept = false;
                        LocationName = string.Empty;
                        IsInformationVisible = Visibility.Collapsed;
                        IsWrongLocationErrorVisible = Visibility.Hidden;
                        IsLocationScanned = false;
                        IsWrongItemErrorVisible = Visibility.Hidden;
                        TrackBarEditvalue = 0;
                        PriorIdWareHouseDeliveryNoteItem = 0;
                    }).Start();

                MapItems.Clear();
                //end

                #region Old Code

                //[000] add comment
                //if (result == MessageBoxResult.Yes)
                //{
                //    EnabledGridFormatCondition = true;
                //    TrackBarEditvalue = 2;
                //    SelectedInventoryMaterial = null;

                //    if (!DXSplashScreen.IsActive)
                //    {
                //        DXSplashScreen.Show(x =>
                //        {
                //            Window win = new Window()
                //            {
                //                ShowActivated = false,
                //                WindowStyle = WindowStyle.None,
                //                ResizeMode = ResizeMode.NoResize,
                //                AllowsTransparency = true,
                //                Background = new SolidColorBrush(Colors.Transparent),
                //                ShowInTaskbar = false,
                //                Topmost = true,
                //                SizeToContent = SizeToContent.WidthAndHeight,
                //                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //            };
                //            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                //            win.Topmost = false;
                //            return win;
                //        }, x =>
                //        {
                //            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                //        }, null, null);
                //    }

                //    if (InventoryMaterials.Any(x => x.IdWarehouseLocation == 0))
                //    {
                //        List<InventoryMaterial> inventoryMaterialList = InventoryMaterials.Where(x => x.IdWarehouseLocation == 0).ToList();
                //        List<ArticleWarehouseLocations> tempArticleWarehouseLocations = new List<ArticleWarehouseLocations>();

                //        foreach (InventoryMaterial inventoryMaterial in inventoryMaterialList)
                //        {
                //            ArticleWarehouseLocations articleWarehouseLocations = new ArticleWarehouseLocations();
                //            articleWarehouseLocations.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                //            articleWarehouseLocations.IdArticle = inventoryMaterial.IdArticle;
                //            tempArticleWarehouseLocations.Add(articleWarehouseLocations);
                //            inventoryMaterial.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                //            inventoryMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                //        }

                //        List<ArticleWarehouseLocations> articleWarehouseLocationsList = WarehouseService.AddArticleWarehouseLocationIFNotExist(tempArticleWarehouseLocations);
                //    }

                //    List<InventoryMaterial> inventoryMaterials = (WarehouseService.InsertInventoryMaterialIntoArticleStock_V2034(InventoryMaterials.Where(x => x.RemainingQty != 0).ToList()));


                //    string WarningMessage = "Stock Regularization Failed For :" + "\n";

                //    inventoryMaterials.ForEach(x =>
                //    {
                //        WarningMessage = WarningMessage + string.Format("Reference - {0}  Article Current Stock - {1} Diff - {2} \n", x.Reference, x.ArticleTotalStock, x.RemainingQty);
                //    });

                //    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //    if (inventoryMaterials.Count > 0)
                //    {
                //        CustomMessageBox.Show(WarningMessage, Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    }

                //    new Thread(() =>
                //    {
                //        Thread.Sleep(1000);
                //        LoadedAction();
                //        SvgUri = null;
                //        InventoryMaterials = new ObservableCollection<InventoryMaterial>();
                //        EnabledGridFormatCondition = false;
                //        EnableAccept = false;
                //        LocationName = string.Empty;
                //        IsInformationVisible = Visibility.Collapsed;
                //        IsWrongLocationErrorVisible = Visibility.Hidden;
                //        IsLocationScanned = false;
                //        IsWrongItemErrorVisible = Visibility.Hidden;
                //        TrackBarEditvalue = 0;
                //        PriorIdWareHouseDeliveryNoteItem = 0;
                //    }).Start();

                //    MapItems.Clear();
                //}

                #endregion
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("ERrror in InsertInventoryMaterialIntoArticleStock " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in LInsertInventoryMaterialIntoArticleStock - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in InsertInventoryMaterialIntoArticleStock - ErrorMessage- {1}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Close
        /// </summary>
        /// <param name="obj"></param>
        private void CommandCancelAction(object obj)
        {
            try
            {
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in CommandCancelAction(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillReasonsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillReasonsList()...", category: Category.Info, priority: Priority.Low);

                List<LookupValue> reasons = CrmService.GetLookupValues(44).ToList();
                reasons.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                ReasonList = new ObservableCollection<LookupValue>(reasons.ToList());

                GeosApplication.Instance.Logger.Log("Method FillReasonsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillReasonsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillReasonsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillReasonsList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CellValueChangingCommandAction(object obj)
        {
            CellValueEventArgs e = obj as CellValueEventArgs;

            if ((((CellValueEventArgs)e).Column).FieldName == "IdReason")
            {
                SelectedInventoryMaterial.Reason = ReasonList.FirstOrDefault(x => x.IdLookupValue == Convert.ToInt32(e.Value));
                e.Source.PostEditor();
            }

            FocusUserControl = true;
        }

        #endregion
    }
}
