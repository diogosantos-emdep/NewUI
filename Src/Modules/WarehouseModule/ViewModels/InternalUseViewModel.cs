using DevExpress.DataProcessing;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Map;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class InternalUseViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Public Events
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

        #region Declarations

        private double dialogHeight;
        private double dialogWidth;

        private Visibility isInformationVisible;
        private Visibility isWrongItemErrorVisible;
        private Visibility isWrongLocationErrorVisible;
        private Visibility isWrongCommentErrorVisible;

        private bool isLocationScanned;
        private string locationName;

        private string barcodeStr;
        private int trackBarEditvalue;

        private string wrongLocation;
        private string wrongItem;
        private WarehouseLocation scannedWarehouseLocation;
        private Uri svgUri;
        private ImageSource articleImage;
        private bool enableAccept;

        ObservableCollection<InventoryMaterial> inventoryMaterials;
        InventoryMaterial selectedInventoryMaterial;
        private ObservableCollection<WarehouseLocation> mapItems;
        public long PriorIdWareHouseDeliveryNoteItem { get; set; }
        private ArticleWarehouseLocations progressbarArticleStock;
        private Int64 totalCurrentItemStock;
        private ArticleWarehouseLocations progressbarLocationStock;
        private long locationStockValue;
        private string locationStockBgColor;
        private string locationStockFgColor;

        private string totalStockFgColor;
        private string totalStockBgColor;
        private bool enabledGridFormatCondition;
        private bool focusUserControl;
        private WarehouseInventoryAudit selectedWarehouseInventoryAudit;
        private string comments= string.Empty;
      
        private int selectedIndex = 0;
        private List<InventoryMaterial> warehouseInventoryAuditItems {get; set; }

        private Visibility isCommentVisible;
        private bool isCommentFocusable;
        private bool isCommentEnable;
        private Visibility isPickVisibility;
        private Visibility isRefundVisibility;
        private InventoryMaterial selectedpickingMaterial;
        private InventoryMaterial selectedRefundMaterial;
        private bool isAcceptButtonEnable;
        private string onFocusBgColor;
        private string onFocusFgColor;
        ObservableCollection<InventoryMaterial> ClonedInventoryMaterials { get; set; }





        #endregion

        #region Properties

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
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

        public Visibility IsWrongItemErrorVisible
        {
            get { return isWrongItemErrorVisible; }
            set
            {
                isWrongItemErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongItemErrorVisible"));
            }
        }

        public Visibility IsWrongCommentErrorVisible
        {
            get { return isWrongCommentErrorVisible; }
            set
            {
                isWrongCommentErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongCommentErrorVisible"));
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

        public bool IsLocationScanned
        {
            get { return isLocationScanned; }
            set
            {
                isLocationScanned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocationScanned"));
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

        public string WrongItem
        {
            get { return wrongItem; }
            set
            {
                wrongItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WrongItem"));
            }
        }

        public WarehouseLocation ScannedWarehouseLocation
        {
            get { return scannedWarehouseLocation; }
            set { scannedWarehouseLocation = value; }
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

        public bool EnableAccept
        {
            get { return enableAccept; }
            set
            {
                enableAccept = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnableAccept"));
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

        public InventoryMaterial SelectedInventoryMaterial
        {
            get { return selectedInventoryMaterial; }
            set
            {
                selectedInventoryMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedInventoryMaterial"));
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

        public ArticleWarehouseLocations ProgressbarArticleStock
        {
            get { return progressbarArticleStock; }
            set
            {
                progressbarArticleStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressbarArticleStock"));
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

        public bool EnabledGridFormatCondition
        {
            get { return enabledGridFormatCondition; }
            set
            {
                enabledGridFormatCondition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnabledGridFormatCondition"));
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

        public WarehouseInventoryAudit SelectedWarehouseInventoryAudit
        {
            get { return selectedWarehouseInventoryAudit; }
            set { selectedWarehouseInventoryAudit = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedWarehouseInventoryAudit")); }
        }

        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comments"));
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

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }

            set
            {
                    selectedIndex = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndex"));
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
                    Comments = string.Empty;
                    IsCommentEnable = false;
                    BarcodeStr = string.Empty;
                    IsAcceptButtonEnable = false;
                    IsWrongCommentErrorVisible = Visibility.Hidden;
                    MapItems.Clear();
            }
        }
        public Visibility IsCommentVisible
        {
            get
            {
                return isCommentVisible;
            }

            set
            {
                isCommentVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCommentVisible"));
            }
        }

        public bool IsCommentFocusable
        {
            get
            {
                return isCommentFocusable;
            }

            set
            {
                isCommentFocusable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCommentFocusable"));
            }
        }

        public bool IsCommentEnable
        {
            get
            {
                return isCommentEnable;
            }

            set
            {
                isCommentEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCommentEnable"));
            }
        }

        public Visibility IsPickVisibility
        {
            get
            {
                return isPickVisibility;
            }

            set
            {
                isPickVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPickVisibility"));
            }
        }

        public Visibility IsRefundVisibility
        {
            get
            {
                return isRefundVisibility;
            }

            set
            {
                isRefundVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRefundVisibility"));
            }
        }

        public InventoryMaterial SelectedpickingMaterial
        {
            get { return selectedpickingMaterial; }
            set
            {
                selectedpickingMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedpickingMaterial"));
            }
        }

        public InventoryMaterial SelectedRefundMaterial
        {
            get
            {
                return selectedRefundMaterial;
            }

            set
            {
                selectedRefundMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRefundMaterial"));
            }
        }

        public bool IsAcceptButtonEnable
        {
            get
            {
                return isAcceptButtonEnable;
            }

            set
            {
                isAcceptButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptButtonEnable"));

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

        #endregion

        #region ICommands

        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandInsertComment { get; set; }
        public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        public ICommand CommandAcceptButton { get; set; }
        public ICommand HyperlinkClickCommand { get; set; }
        public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }

       

        #endregion


        #region Constructor

        public InternalUseViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor InternalUseViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 90;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 100;

                IsInformationVisible = Visibility.Collapsed;
                IsWrongItemErrorVisible = Visibility.Hidden;
                IsWrongLocationErrorVisible = Visibility.Hidden;
                IsWrongCommentErrorVisible = Visibility.Hidden;
                IsPickVisibility = Visibility.Hidden;
                IsRefundVisibility = Visibility.Hidden;

                MapItems = new ObservableCollection<WarehouseLocation>();

                EscapeButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                CommandCancelButton = new RelayCommand(new Action<object>(CloseWindow));

                CommandScanBarcode = new DelegateCommand<object>(ScanBarcodeAction);
                CommandInsertComment = new DelegateCommand<object>(AddComment);
                CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);
                CommandAcceptButton = new DelegateCommand<object>(AcceptAction);
                HyperlinkClickCommand = new DelegateCommand<object>(HyperlinkClickCommandAction);
                VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
                IsCommentVisible = Visibility.Collapsed;
                IsCommentEnable = false;
                //FocusUserControl = true;


                GeosApplication.Instance.Logger.Log("Constructor InternalUseViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor InternalUseViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }


        #endregion


        #region Methods


        private void AddComment(object _obj)
        {
            try
            {
                TextBox obj = (TextBox)_obj;

                BarcodeStr = obj.Text;
                Comments = string.Copy(BarcodeStr);
                IsWrongCommentErrorVisible = Visibility.Hidden;
            }
            catch (Exception ex)
            {
            }
        }

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// [001][vsana][22-11-2020][GEOS2-2426]AutoSort for the new locations created 
        private void ScanBarcodeAction(object _obj)
        {
            GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                TextCompositionEventArgs obj = (TextCompositionEventArgs)_obj;

                if (obj.Text == "\r" && IsLocationScanned == false)
                {
                    LocationName = string.Copy(BarcodeStr);
                    //[001] Changed service method IsExistWarehouseLocationFullName to IsExistWarehouseLocationFullName_V2080
                    if (WarehouseService.IsExistWarehouseLocationFullName_V2080(BarcodeStr, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse))
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

                        TrackBarEditvalue = 1;
                        LoadWarehouseLayoutFile(LocationName);

                        if (SelectedIndex == 0)
                        {
                            InventoryMaterials = new ObservableCollection<InventoryMaterial>(WarehouseService.GetInternalUsePickArticleByIdWarehouseLocation(WarehouseCommon.Instance.Selectedwarehouse, ScannedWarehouseLocation.IdWarehouseLocation));

                            warehouseInventoryAuditItems = new List<InventoryMaterial>();

                            InventoryMaterials.ForEach(x =>
                            {
                                x.RemainingQty = x.AvailableQty * 1;
                                x.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                x.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                            });
                        }
                        else if (SelectedIndex == 1)
                        {
                            InventoryMaterials = new ObservableCollection<InventoryMaterial>(WarehouseService.GetInternalUsePickArticleByIdWarehouseLocation(WarehouseCommon.Instance.Selectedwarehouse, ScannedWarehouseLocation.IdWarehouseLocation));
                            InventoryMaterials.ForEach(x =>
                            {
                                x.RemainingQty = x.AvailableQty * 1;
                                x.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                x.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                            });
                        }


                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        IsInformationVisible = Visibility.Collapsed;
                        IsWrongLocationErrorVisible = Visibility.Collapsed;
                        IsLocationScanned = true;
                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepOkFilePath);
                    }
                    else
                    {
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

                        ClonedInventoryMaterials = new ObservableCollection<InventoryMaterial>();
                        ClonedInventoryMaterials = InventoryMaterials;

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
                            if (SelectedIndex == 1)
                                InventoryMaterials.AddRange(tempInventoryMaterials);

                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }

                        if (InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem) != null && _itemQuantity > 0)
                        {
                            if (SelectedIndex == 0)
                            {
                                if (ClonedInventoryMaterials.Count != InventoryMaterials.Count)
                                {
                                    if (_itemQuantity > InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem).RemainingQty)
                                    {
                                        IsWrongItemErrorVisible = Visibility.Visible;
                                        WrongItem = "Wrong Quantity " + _itemQuantity;
                                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                        BarcodeStr = string.Empty;
                                        return;
                                    }
                                }
                                if (ClonedInventoryMaterials.Count == InventoryMaterials.Count)
                                {
                                    if (_itemQuantity > InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem).RemainingQty)
                                    {
                                        IsWrongItemErrorVisible = Visibility.Visible;
                                        WrongItem = "Wrong Quantity " + _itemQuantity;
                                        GeosApplication.Instance.PlaySound(WarehouseCommon.Instance.BeepNotOkFilePath);
                                        BarcodeStr = string.Empty;
                                        return;
                                    }
                                }
                            }

                            IsWrongItemErrorVisible = Visibility.Collapsed;

                            ScanAction(_idWareHouseDeliveryNoteItem, _itemQuantity, _idwarehouse);
                            IsCommentVisible = Visibility.Visible;
                            IsCommentEnable = true;

                            if (SelectedInventoryMaterial != null)
                            {
                                if (SelectedInventoryMaterial.ScannedQty > 0)
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
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error  ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed ScanBarcodeAction", category: Category.Info, priority: Priority.Low);
        }

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
            catch (System.ServiceModel.FaultException<ServiceException> ex)
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

        private void ScanAction(Int64 idWareHouseDeliveryNoteItem, Int64 quantity, Int64 IdwarehouseLocarion)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);
                IsInformationVisible = Visibility.Visible;
                EnableAccept = true;
                
                SelectedInventoryMaterial = InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == idWareHouseDeliveryNoteItem);
                ArticleImage = WarehouseCommon.Instance.ByteArrayToImage(SelectedInventoryMaterial.ArticleImageInBytes);

                if (SelectedIndex == 0)
                {
                    IsPickVisibility = Visibility.Visible;
                    IsRefundVisibility = Visibility.Hidden;
                    SelectedInventoryMaterial.RemainingQty -= quantity;
                    SelectedInventoryMaterial.ScannedQty += quantity;
                   IsAcceptButtonEnable = true;
                }
                else
                {
                    IsRefundVisibility = Visibility.Visible;
                    IsPickVisibility = Visibility.Hidden;
                    SelectedInventoryMaterial.RemainingQty += quantity;
                    SelectedInventoryMaterial.ScannedQty += quantity;
                   IsAcceptButtonEnable = true;
                }

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
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction Method ScanAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction Method ScanAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error  ScanAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ScanAction....executed ScanAction", category: Category.Info, priority: Priority.Low);
        }

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
        }

        private void AcceptAction(object obj)
        {
            try
            {
                if ( Comments == string.Empty || Comments == null || Comments == "" )
                {
                    IsWrongCommentErrorVisible = Visibility.Visible;
                    IsCommentEnable = true;
                    FocusUserControl = true;
                    return;
                }

                TableView inventoryTabelView = (TableView)obj;
                inventoryTabelView.ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.RowAndCell;


                
                foreach (InventoryMaterial item in InventoryMaterials.Where(a=>a.ScannedQty > 0))
                {
                    TotalCurrentItemStock = WarehouseService.GetArticleStockByWarehouse(item.IdArticle, Convert.ToInt32(item.IdWarehouse));
                    if (SelectedIndex == 0)
                    {
                        SelectedpickingMaterial = new InventoryMaterial();
                        bool IsScanItem = false;

                        SelectedpickingMaterial.IdArticle = item.IdArticle;
                        SelectedpickingMaterial.IdOtitem = item.IdOtitem;
                        SelectedpickingMaterial.ScannedQty = item.ScannedQty * -1;
                        SelectedpickingMaterial.CostPrice = item.CostPrice;
                        SelectedpickingMaterial.UnitPrice = item.UnitPrice;
                        SelectedpickingMaterial.Comments = "[WAREHOUSE] Internal Use. " + "("+  Comments  +")" + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock - item.ScannedQty) + "]";
                        SelectedpickingMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        SelectedpickingMaterial.IdWarehouse = item.IdWarehouse;
                        SelectedpickingMaterial.IdWareHouseDeliveryNoteItem = item.IdWareHouseDeliveryNoteItem;
                        SelectedpickingMaterial.IdWarehouseLocation = item.IdWarehouseLocation;
                        SelectedpickingMaterial.IdCurrency = item.IdCurrency;
                        SelectedpickingMaterial.RemainingQty = item.RemainingQty;

                        if (SelectedpickingMaterial.ScannedQty != 0)
                        {
                            IsScanItem = WarehouseService.InsertIntoArticleStockForInternalUse(SelectedpickingMaterial);
                        }
                    }
                    else if(SelectedIndex == 1)
                    {
                        SelectedRefundMaterial = new InventoryMaterial();
                        bool IsScanItem = false;

                        SelectedRefundMaterial.IdArticle = item.IdArticle;
                        SelectedRefundMaterial.IdOtitem = item.IdOtitem;
                        SelectedRefundMaterial.ScannedQty = item.ScannedQty;
                        SelectedRefundMaterial.CostPrice = item.CostPrice;
                        SelectedRefundMaterial.UnitPrice = item.UnitPrice;
                        SelectedRefundMaterial.Comments = "[WAREHOUSE] Internal Use. " + "("+  Comments  +")" + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock + item.ScannedQty) + "]";
                        SelectedRefundMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        SelectedRefundMaterial.IdWarehouse = item.IdWarehouse;
                        SelectedRefundMaterial.IdWareHouseDeliveryNoteItem = item.IdWareHouseDeliveryNoteItem;
                        if (item.IdWarehouseLocation != InventoryMaterials.FirstOrDefault().IdWarehouseLocation)
                            SelectedRefundMaterial.IdWarehouseLocation = InventoryMaterials.FirstOrDefault().IdWarehouseLocation;
                        else
                            SelectedRefundMaterial.IdWarehouseLocation = item.IdWarehouseLocation;
                        SelectedRefundMaterial.IdCurrency = item.IdCurrency;
                        SelectedRefundMaterial.RemainingQty = item.RemainingQty;

                        // If found any warehousedeliverynoteitem which is not registered in db then add seperate entry for this.
                        //if (InventoryMaterials.Any(x => x.IdWarehouseLocation == 0))
                        {
                            //List<InventoryMaterial> inventoryMaterialList = InventoryMaterials.Where(x => x.IdWarehouseLocation == 0).ToList();

                            List<ArticleWarehouseLocations> tempArticleWarehouseLocations = new List<ArticleWarehouseLocations>();
                            foreach (InventoryMaterial inventoryMaterial in InventoryMaterials)
                            {
                                ArticleWarehouseLocations articleWarehouseLocation = new ArticleWarehouseLocations();
                                articleWarehouseLocation.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                                articleWarehouseLocation.IdArticle = inventoryMaterial.IdArticle;
                                articleWarehouseLocation.Position = 1;
                                articleWarehouseLocation.MinimumStock = inventoryMaterial.MinimumStock;
                                articleWarehouseLocation.MaximumStock = 0;
                                tempArticleWarehouseLocations.Add(articleWarehouseLocation);
                                inventoryMaterial.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                                inventoryMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }

                            List<ArticleWarehouseLocations> articleWarehouseLocationsList = WarehouseService.AddArticleWarehouseLocationIFNotExist(tempArticleWarehouseLocations);
                        }


                        if (SelectedRefundMaterial.ScannedQty != 0)
                        {
                            IsScanItem = WarehouseService.InsertIntoArticleStockForInternalUse(SelectedRefundMaterial);
                        }
                    }
                }
            
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                new Thread(() =>
                {
                    Thread.Sleep(1000);
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
                    Comments = string.Empty;
                    IsCommentEnable = false;
                    BarcodeStr = string.Empty;
                    IsAcceptButtonEnable = false;
                    IsWrongCommentErrorVisible = Visibility.Hidden;
                }).Start();

                MapItems.Clear();
                //end

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("ERrror in AcceptAction " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AcceptAction - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in AcceptAction - ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

        void VectorLayerDataLoaded(object obj)
        {
            MapControl mapControl = obj as MapControl;

            if (mapControl != null)
                mapControl.ZoomToFitLayerItems();
        }

        #endregion

        public void Dispose()
        {
        }
    }

   
}
