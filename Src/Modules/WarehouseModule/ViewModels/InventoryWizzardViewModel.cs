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
using System.Windows.Controls;
using Emdep.Geos.Data.Common.WMS;
using Emdep.Geos.Data.Common.SCM;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class InventoryWizzardViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region TaskLogs

        /// <summary>
        /// [000][skhade][21-01-2020][GEOS2-2029] Add the old version inventory in Warehouse section
        /// </summary>

        #endregion

        #region Services
       IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }
        ICrmService CrmService = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        private string comments = string.Empty;
        private bool focusUserControl;
        private Visibility isCommentVisible;
        private bool isCommentFocusable;
        private Visibility isWrongCommentErrorVisible;
        private bool isCommentEnable;
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
        private List<InventoryMaterial> inventoryCloneInventoryMaterial;// [rani dhamankar][08-04-2025][GEOS2-6758]
        private bool isAnyItemChecked;//[rani dhamankar][08-04-2025][GEOS2-6758]
        public bool IsAnyItemChecked
        {
            get => isAnyItemChecked;
            set
            {
                if (isAnyItemChecked != value)
                {
                    isAnyItemChecked = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsAnyItemChecked"));
                }
            }
        }
       
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

        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comments"));
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

        public Visibility IsWrongCommentErrorVisible
        {
            get { return isWrongCommentErrorVisible; }
            set
            {
                isWrongCommentErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongCommentErrorVisible"));
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
        // [rani dhamankar][08-04-2025][GEOS2-6758]
        public List<InventoryMaterial> InventoryCloneInventoryMaterial
        {
            get { return inventoryCloneInventoryMaterial; }
            set
            {
                inventoryCloneInventoryMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InventoryCloneInventoryMaterial"));
            }
        }
        //Shubham[skadam] GEOS2-4436 Ask for location inventory after do the picking of certain references (2/3)  23 10 2023 
        Visibility isCancelButtonVisible;
        public Visibility IsCancelButtonVisible
        {
            get
            {
                return isCancelButtonVisible;
            }

            set
            {
                isCancelButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancelButtonVisible"));
            }
        }

        bool isCancelButtonEnabled = false;
        public bool IsCancelButtonEnabled
        {
            get { return isCancelButtonEnabled; }
            set
            {
                isCancelButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancelButtonEnabled"));
            }
        }

        #region [rani dhamankar][15-04 -2025][GEOS2-6758]
        bool isUndoButtonEnabled = false;
        public bool IsUndoButtonEnabled
        {
            get { return isUndoButtonEnabled; }
            set
            {
                isUndoButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUndoButtonEnabled"));
            }
        }
        #endregion
        //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
        bool isEmailSend = false;
        public bool IsEmailSend
        {
            get { return isEmailSend; }
            set
            {
                isEmailSend = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmailSend"));
            }
        }

        string readMailTemplate = string.Empty;
        public string ReadMailTemplate
        {
            get { return readMailTemplate; }
            set
            {
                readMailTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReadMailTemplate"));
            }
        }

        List<WMSPickingMaterials> selectedLocationList;
        public List<WMSPickingMaterials> SelectedLocationList
        {
            get { return selectedLocationList; }
            set
            {
                selectedLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocationList"));
            }
        }

        WMSPickingMaterials selectedpickingMaterial;
        public WMSPickingMaterials SelectedpickingMaterial
        {
            get { return selectedpickingMaterial; }
            set
            {
                selectedpickingMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedpickingMaterial"));
            }
        }

        GeosAppSetting geosAppSetting;
        public GeosAppSetting GeosAppSetting
        {
            get { return geosAppSetting; }
            set
            {
                geosAppSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSetting"));
            }
        }

        string mailTo =string.Empty;
        public string MailTo
        {
            get { return mailTo; }
            set
            {
                mailTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MailTo"));
            }
        }

        string subject = "[WMS] Inventory mismatch for the reference";
        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Subject"));
            }
        }

        string mailFrom = "WMS-noreply@emdep.com";
        public string MailFrom
        {
            get { return mailFrom; }
            set
            {
                mailFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MailFrom"));
            }
        }

        byte[] attachmentData ;
        public byte[] AttachmentData
        {
            get { return attachmentData; }
            set
            {
                attachmentData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentData"));
            }
        }

        string workOrder =string.Empty;
        public string WorkOrder
        {
            get { return workOrder; }
            set
            {
                workOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrder"));
            }
        }

        string item = string.Empty;
        public string Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Item"));
            }
        }
        string user = GeosApplication.Instance.ActiveUser.FirstName +" "+ GeosApplication.Instance.ActiveUser.LastName;
        public string User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged(new PropertyChangedEventArgs("User"));
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
        public ICommand CommandInsertComment { get; set; }
        public ICommand CommandUndoButton { get; set; }// [rani dhamankar][07-04-2025][GEOS2-6758]
        public ICommand IsCheckedChanged { get; set; }//[rani dhamankar][16-04-2025][GEOS2-6758]
        #endregion // ICommands

        #region Constructor

        public InventoryWizzardViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor InventoryWizzardViewModel()...", category: Category.Info, priority: Priority.Low);

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
            CommandInsertComment = new DelegateCommand<object>(AddComment);
            CommandUndoButton = new DelegateCommand<object>(CommandUndoAction);//[rani dhamankar][07-04-2025][GEOS2-6758]

            GeosApplication.Instance.Logger.Log("Constructor InventoryWizzardViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            Comments = null;
            IsCommentVisible = Visibility.Collapsed;
            IsCommentEnable = false;
            //Shubham[skadam] GEOS2-4436 Ask for location inventory after do the picking of certain references (2/3)  23 10 2023 
            IsCancelButtonVisible = Visibility.Visible;
            IsCancelButtonEnabled = true;
            //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
            GetGeosAppSettings();
            IsCheckedChanged = new DelegateCommand<object>(IsCheckedChangedCommandAction);//[rani dhamankar][16-04-2025][GEOS2-6758]

        }

        #endregion // Constructor

        #region Methods

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
        /// to update Progressbar of ArticleStock for a warehouse 
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
        private void ChangeTotalStockColor(Int64 totalStock, Int64 minQuantity)
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

        #endregion

        #region Command Actions

        /// <summary>
        /// Scan Barcode
        /// [001][vsana][22-11-2020][GEOS2-2426]AutoSort for the new locations created 
        /// [002][cpatil][23-11-2020][GEOS2-2280][GEOS2-3476]Location not assigned to the article but with stock after an audit of REF. 05PGM1125720
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
                        //[002]
                        InventoryMaterials = new ObservableCollection<InventoryMaterial>(WarehouseService.GetInventoryArticleByIdWarehouseLocation_V2210(WarehouseCommon.Instance.Selectedwarehouse, ScannedWarehouseLocation.IdWarehouseLocation));
                        InventoryMaterials.ForEach(x =>
                        {
                            x.RemainingQty = x.AvailableQty * -1;
                            x.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            x.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                        });
                        #region  [rani dhamankar][10-04-2025][GEOS2-6758]
                        InventoryCloneInventoryMaterial = new List<InventoryMaterial>();
                        if (InventoryMaterials.Count > 0)
                        {
                            InventoryCloneInventoryMaterial = InventoryMaterials.Select(x => (InventoryMaterial)x.Clone()).ToList();
                        }
                        #endregion
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

                            // [002] Changed Service method
                            List<InventoryMaterial> tempInventoryMaterials = WarehouseService.GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2210(WarehouseCommon.Instance.Selectedwarehouse, _idWareHouseDeliveryNoteItem);
                            tempInventoryMaterials.ForEach(x =>
                            {
                                x.AvailableQty = 0;
                                x.RemainingQty = x.AvailableQty * -1;
                                x.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                x.IdWarehouseLocation = x.IdWarehouseLocation == 0 ? 0 : WarehouseService.IsExistArticleWarehouseLocation(x.IdArticle, ScannedWarehouseLocation.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse) ? ScannedWarehouseLocation.IdWarehouseLocation : 0;
                            });

                            InventoryMaterials.AddRange(tempInventoryMaterials);
                            
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }

                        if (InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == _idWareHouseDeliveryNoteItem) != null && _itemQuantity > 0)
                        {
                            IsWrongItemErrorVisible = Visibility.Collapsed;
                            ScanAction(_idWareHouseDeliveryNoteItem, _itemQuantity, _idwarehouse);
                            IsCommentVisible = Visibility.Visible;
                            IsCommentEnable = true;
                           

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
                    //BarcodeStr += obj.Text;
                    //[pramod.misal][GEOS2-5076][08-01-2023]
                    BarcodeStr += obj.Text.ToUpper(); ;
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
                SelectedInventoryMaterial.IsCheckboxEnabled = true; //[rani dhamankar][07-04-2025][GEOS2-6758]
                if (PriorIdWareHouseDeliveryNoteItem != idWareHouseDeliveryNoteItem)
                {
                    PriorIdWareHouseDeliveryNoteItem = idWareHouseDeliveryNoteItem;
                    UpdateProgressbarArticleStock();
                    UpdateProgressbarLocationStock();
                    ChangeLocationStockColor(ProgressbarLocationStock.LocationStock, SelectedInventoryMaterial.AwlMinimumStock);
                    ChangeTotalStockColor(TotalCurrentItemStock, SelectedInventoryMaterial.MinimumStock);
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
            Service.Navigate("Emdep.Geos.Modules.Warehouse.Views.InventoryWizzardUserControlView", null, this);
        }

        /// <summary>
        /// Accept Click
        /// [001][GEOS2-3149][16-09-2021][Add a new section “Comments” in the “Inventory” option in Warehouse]
        /// </summary>
        private void AcceptAction(object obj)
        {
            try
            {
                //if (Comments == string.Empty || Comments == null || Comments == "")
                //{
                //    IsWrongCommentErrorVisible = Visibility.Visible;
                //    IsCommentEnable = true;
                //    FocusUserControl = true;
                //    return;
                //}

                TableView inventoryTabelView = (TableView)obj;
                inventoryTabelView.ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.RowAndCell;

                if (InventoryMaterials.Any(x => x.RemainingQty != 0 && x.IdReason <= 0))
                {
                    FocusUserControl = true;
                    return;
                }

                MessageBoxResult result = CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("InventoryRegularizeStock").ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    EnabledGridFormatCondition = true;
                    TrackBarEditvalue = 2;
                    SelectedInventoryMaterial = null;

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

                    if (InventoryMaterials.Any(x => x.IdWarehouseLocation == 0))
                    {
                        List<InventoryMaterial> inventoryMaterialList = InventoryMaterials.Where(x => x.IdWarehouseLocation == 0).ToList();
                        List<ArticleWarehouseLocations> tempArticleWarehouseLocations = new List<ArticleWarehouseLocations>();

                        foreach (InventoryMaterial inventoryMaterial in inventoryMaterialList)
                        {
                            ArticleWarehouseLocations articleWarehouseLocations = new ArticleWarehouseLocations();
                            articleWarehouseLocations.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                            articleWarehouseLocations.IdArticle = inventoryMaterial.IdArticle;
                            tempArticleWarehouseLocations.Add(articleWarehouseLocations);
                            inventoryMaterial.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                            inventoryMaterial.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        }

                        List<ArticleWarehouseLocations> articleWarehouseLocationsList = WarehouseService.AddArticleWarehouseLocationIFNotExist(tempArticleWarehouseLocations);
                    }
                    //[001] changed service method InsertInventoryMaterialIntoArticleStock_V2034 to InsertInventoryMaterialIntoArticleStock_V2190
                    List<InventoryMaterial> inventoryMaterials = (WarehouseService.InsertInventoryMaterialIntoArticleStock_V2190(InventoryMaterials.Where(x => x.RemainingQty != 0).ToList(), Comments));
                    #region GEOS2-4437
                    //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
                    try
                    {
                        List<InventoryMaterial> tempInventoryMaterial = InventoryMaterials.Where(w => w.ScannedQty != 0).ToList();
                        foreach (InventoryMaterial InventoryMaterialitem in tempInventoryMaterial)
                        {
                            if (InventoryMaterialitem.ScannedQty == InventoryMaterialitem.AvailableQty)
                            {
                                IsEmailSend = false;
                            }
                            else
                            {
                                IsEmailSend = true;
                                break;
                            }
                        }
                        if (IsEmailSend)
                        {
                            TakeScreenshot();
                            bool IsSend = WarehouseService.IsEmailSendAfterPickingInventory_V2450(subject, ReadMailTemplate, MailTo, MailFrom, AttachmentData);
                            IsEmailSend = false;
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            RequestClose(null, null);
                        }
                        if (IsCancelButtonEnabled==false)
                        {
                            RequestClose(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log(string.Format("Error in InsertInventoryMaterialIntoArticleStock - ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                    #endregion
                    string WarningMessage = "Stock Regularization Failed For :" + "\n";

                    inventoryMaterials.ForEach(x =>
                    {
                        WarningMessage = WarningMessage + string.Format("Reference - {0}  Article Current Stock - {1} Diff - {2} \n", x.Reference, x.ArticleTotalStock, x.RemainingQty);
                    });

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    if (inventoryMaterials.Count > 0)
                    {
                        CustomMessageBox.Show(WarningMessage, Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }


                    new Thread(() =>
                    {
                        Thread.Sleep(1000);
                        LoadedAction();
                        SvgUri = null;
                        InventoryMaterials = new ObservableCollection<InventoryMaterial>();
                        EnabledGridFormatCondition = false;
                        EnableAccept = false;
                        LocationName = string.Empty;
                        Comments = null;
                          IsInformationVisible = Visibility.Collapsed;
                        IsWrongLocationErrorVisible = Visibility.Hidden;
                        IsLocationScanned = false;
                        WrongItem = string.Empty;
                        BarcodeStr = string.Empty;
                        IsWrongItemErrorVisible = Visibility.Hidden;
                        TrackBarEditvalue = 0;
                        PriorIdWareHouseDeliveryNoteItem = 0;
                    }).Start();

                    MapItems.Clear();
                }
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
                GeosApplication.Instance.Logger.Log(string.Format("Error in InsertInventoryMaterialIntoArticleStock - ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                //Shubham[skadam] GEOS2-4436 Ask for location inventory after do the picking of certain references (2/3)  23 10 2023 
                if (IsCancelButtonEnabled)
                {
                    RequestClose(null, null);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in CommandCancelAction(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

        /// <summary>
        ///   [001][GEOS2-3149][16-09-2021][Add a new section “Comments” in the “Inventory” option in Warehouse]
        /// </summary>
        /// <param name="_obj"></param>
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

        //Shubham[skadam] GEOS2-4436 Ask for location inventory after do the picking of certain references (2/3)  23 10 2023 
        public void ScanBarcodeAction(string ScanLocationName)
        {
            try
            {
                IsLocationScanned = false;
                IsCancelButtonEnabled = false;
                IsCancelButtonVisible = Visibility.Visible;
                IsEmailSend = false;
                if (IsLocationScanned == false)
                {
                    LocationName = string.Copy(ScanLocationName);
                    BarcodeStr = string.Copy(ScanLocationName);
                    ReadMailTemplate =WarehouseService.ReadMailTemplate("InventoryWizzardMailFormat.html");
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
                        //[002]
                        InventoryMaterials = new ObservableCollection<InventoryMaterial>(WarehouseService.GetInventoryArticleByIdWarehouseLocation_V2210(WarehouseCommon.Instance.Selectedwarehouse, ScannedWarehouseLocation.IdWarehouseLocation));
                        InventoryMaterials = InventoryMaterials.Where(x => x.IdArticle == SelectedpickingMaterial.Article.IdArticle).ToObservableCollection(); // [nsatpute][11-10-2024][nsatpute]
                        InventoryMaterials.ForEach(x =>
                        {
                            x.RemainingQty = x.AvailableQty * -1;
                            x.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            x.IdWarehouseLocation = ScannedWarehouseLocation.IdWarehouseLocation;
                        });
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
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error  ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed ScanBarcodeAction", category: Category.Info, priority: Priority.Low);

  			//Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
            try
            {
                //IsCancelButtonEnabled = true;
                Subject = Subject + " " + SelectedpickingMaterial.Reference;
                Item = SelectedpickingMaterial.RevisionItem.NumItem;
                ReadMailTemplate = ReadMailTemplate.Replace("[item]", Item.ToString());
                ReadMailTemplate = ReadMailTemplate.Replace("[WorkOrder]", WorkOrder.ToString());
                ReadMailTemplate = ReadMailTemplate.Replace("[User]", User.ToString());
                //IsEmailSend = true;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
           
        }

        //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
        Dictionary<string, int> GeosAppSettingList = new Dictionary<string, int>();
        public void GetGeosAppSettings()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetGeosAppSettings....", category: Category.Info, priority: Priority.Low);
                GeosAppSetting = WorkbenchService.GetGeosAppSettings(111);
                GeosAppSettingList = new Dictionary<string, int>();
                List<string> SplitGeosAppSettingsValue = GeosAppSetting.DefaultValue.Split(',').ToList();
                foreach (string item in SplitGeosAppSettingsValue)
                {
                    string DefaultValue=item.Replace("(", String.Empty).Replace(")", String.Empty);
                    List<string> SplitDefaultValueValue=DefaultValue.Split(';').ToList();
                    try
                    {
                        try
                        {
                            GeosAppSettingList.Add(SplitDefaultValueValue.LastOrDefault(), Convert.ToInt32(SplitDefaultValueValue[0]));
                        }
                        catch (Exception ex)
                        {
                        }
                        var myKey = GeosAppSettingList.Where(x => x.Value == WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse).Select(s => s.Key);
                        if (myKey!=null)
                        {
                            MailTo = String.Join(",", myKey);
                        }
                    }
                    catch (Exception)  {}
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error GetGeosAppSettings...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method GetGeosAppSettings....executed", category: Category.Info, priority: Priority.Low);
        }

        //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
        public void TakeScreenshot()
        {

            try
            {
                #region PrimaryScreen
                ////Create a new bitmap.
                //var bmpScreenshot = new System.Drawing.Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                //                               System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height,
                //                               System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                //// Create a graphics object from the bitmap.
                //var gfxScreenshot = System.Drawing.Graphics.FromImage(bmpScreenshot);

                //// Take the screenshot from the upper left corner to the right bottom corner.
                //gfxScreenshot.CopyFromScreen(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X,
                //                            System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y,
                //                            0,
                //                            0,
                //                            System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size,
                //                            System.Drawing.CopyPixelOperation.SourceCopy);

                //// Save the screenshot to the specified path that the user has chosen.
                //bmpScreenshot.Save("Screenshot.png", System.Drawing.Imaging.ImageFormat.Png);
                #endregion
                // Use this version to capture the full extended desktop (i.e. multiple screens)

                #region VirtualScreen
                //System.Drawing.Bitmap screenshot = new System.Drawing.Bitmap(System.Windows.Forms.SystemInformation.VirtualScreen.Width,
                //                               System.Windows.Forms.SystemInformation.VirtualScreen.Height,
                //                               System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                //System.Drawing.Graphics screenGraph = System.Drawing.Graphics.FromImage(screenshot);

                //screenGraph.CopyFromScreen(System.Windows.Forms.SystemInformation.VirtualScreen.X,
                //                           System.Windows.Forms.SystemInformation.VirtualScreen.Y,
                //                           0,
                //                           0,
                //                           System.Windows.Forms.SystemInformation.VirtualScreen.Size,
                //                           System.Drawing.CopyPixelOperation.SourceCopy);

                ////screenshot.Save("Screenshot.png", System.Drawing.Imaging.ImageFormat.Png);
                //using (var stream = new MemoryStream())
                //{
                //    screenshot.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                //    AttachmentData = stream.ToArray();
                //}
                //System.IO.File.WriteAllBytes(@"C:\Temp\image.png", AttachmentData);
                #endregion

                System.Drawing.Point mousePosition = System.Windows.Forms.Cursor.Position;
                System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromPoint(mousePosition);
                //Create a new bitmap.
                var bmpScreenshot = new System.Drawing.Bitmap(screen.Bounds.Width,
                                               screen.Bounds.Height-35,
                                               System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = System.Drawing.Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(screen.Bounds.X,
                                            screen.Bounds.Y,
                                            0,
                                            0,
                                            screen.Bounds.Size,
                                            System.Drawing.CopyPixelOperation.SourceCopy);

                using (var stream = new MemoryStream())
                {
                    bmpScreenshot.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    AttachmentData = stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error TakeScreenshot...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region [rani dhamankar][07-04-2025][GEOS2-6758]

        private void IsCheckedChangedCommandAction(object obj)
        {
            IsAnyItemChecked = InventoryMaterials.Any(i => i.IsChecked);
            IsUndoButtonEnabled = IsAnyItemChecked;
            FocusUserControl = true;
        }

        private void CommandUndoAction(object obj)
        {
            try
            {
                if (InventoryMaterials.Any(x => x.ScannedQty != 0))
                {

                    List<InventoryMaterial> checkedInventoryMaterials = InventoryMaterials.Where(a => a.IsChecked).ToList();
                    if (checkedInventoryMaterials.Count > 0)
                    {
                        MessageBoxResult result = CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("InventoryUndoScanItem").ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            foreach (InventoryMaterial inventoryMaterialItem in checkedInventoryMaterials)
                            {
                                var oldMaterial = InventoryCloneInventoryMaterial.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == inventoryMaterialItem.IdWareHouseDeliveryNoteItem);
                                if (oldMaterial != null)
                                {
                                    inventoryMaterialItem.AvailableQty = oldMaterial.AvailableQty;
                                    inventoryMaterialItem.ScannedQty = oldMaterial.ScannedQty;
                                    inventoryMaterialItem.RemainingQty = oldMaterial.RemainingQty;
                                    inventoryMaterialItem.IdReason = oldMaterial.IdReason;
                                    inventoryMaterialItem.IsChecked = false;
                                    inventoryMaterialItem.IsCheckboxEnabled = false;
                                }
                                else
                                {
                                    var itemToRemove = InventoryMaterials.FirstOrDefault(x => x.IdWareHouseDeliveryNoteItem == inventoryMaterialItem.IdWareHouseDeliveryNoteItem);
                                    if (itemToRemove != null)
                                    {
                                        InventoryMaterials.Remove(itemToRemove);
                                    }
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in CommandUndoAction(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
