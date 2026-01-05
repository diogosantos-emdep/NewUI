using DevExpress.Mvvm;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.Data.Common;
using System.ComponentModel;
using System.IO;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Grid;
using Emdep.Geos.UI.Converters;
using DevExpress.Xpf.Editors;
using Emdep.Geos.UI.Validations;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using DevExpress.Mvvm.UI;
using System.ServiceModel;
using Emdep.Geos.UI.Helper;
//using Emdep.Geos.Modules.Warehouse.Reports;
using DevExpress.Xpf.Printing;
using System.Drawing;
using System.Drawing.Imaging;
using Emdep.Geos.Modules.Warehouse.Reports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.BarCode;
using System.Drawing.Printing;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class EditDeliveryNoteViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {

        #region TaskLog

        //[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        #endregion

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration

        WarehouseDeliveryNote warehouseDeliveryNote;

        private DateTime deliveryDate;
        private string supplierTypeName;
        private string supplierTypeColor;
        private string attchedDocument;

        private ObservableCollection<WarehouseDeliveryNoteItem> warehouseDeliveryNoteItems;
        private WarehouseDeliveryNoteItem selectedGridRow;

        private string informationError;

        private bool isUpdoadStock;
        private bool isDownloadStock;
        private double dialogHeight;
        private double dialogWidth;
        public WarehouseDeliveryNote WarehouseDeliveryNote
        {
            get
            {
                return warehouseDeliveryNote;
            }

            set
            {
                warehouseDeliveryNote = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseDeliveryNote"));
            }
        }

        public ObservableCollection<WarehouseDeliveryNoteItem> WarehouseDeliveryNoteItems
        {
            get
            {
                return warehouseDeliveryNoteItems;
            }

            set
            {
                warehouseDeliveryNoteItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseDeliveryNoteItems"));
            }
        }

        public bool IsUpdoadStock
        {
            get { return isUpdoadStock; }
            set
            {
                if (!WarehouseCommon.Instance.IsPermissionReadOnly)
                {
                    isUpdoadStock = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsUpdoadStock"));
                }
            }
        }

        public bool IsDownloadStock
        {
            get { return isDownloadStock; }
            set
            {
                if (!WarehouseCommon.Instance.IsPermissionReadOnly)
                {
                    isDownloadStock = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsDownloadStock"));
                }
            }
        }

        public string AttchedDocument
        {
            get
            {
                return attchedDocument;
            }

            set
            {
                attchedDocument = value;
            }
        }

        private Attachment attachment;
        private ObservableCollection<WarehouseDeliveryNote> warehouseDeliveryNoteDetails;

        private string uniqueFileName;
        private string fileName;
        private bool isBusy;

        private string supplierReference;
        private string articleSupplier;
        private string deliveryNoteCode;

        private byte[] pdfFileInBytes;
        private Warehouses warehouse;
        // private string serialNumberCode;
        // private Int32 quantity;
        private ManufacturersByArticle producer;
        private ManufacturersByArticle selectedProducer;
        private List<ManufacturersByArticle> listManufacturer;
        private Int32 idArticle;
        private ImageSource referenceImage;
        byte[] ReferenceImageByte = null;
        //private string code;
        // private string selectedManufacturer;
        private bool isReferenceDeleted;
        private bool isReferenceImageExist;
        private bool isDeliveryNoteSaved = false;
        private string currentStockFgColor;
        private string currentStockBgColor;

        private List<WarehouseDeliveryNoteItem> deletedReferenceRowList = new List<WarehouseDeliveryNoteItem>();

        #endregion

        #region Properties

        public Bitmap ReportHeaderImage { get; set; }
        public Warehouses Warehouse { get; set; }
        public string ArticleSupplier { get; set; }
        public string DeliveryNoteCode { get; set; }

        public bool IsDeliveryNoteSaved { get; set; }

        public Int32 IdArticle { get; set; }

        public bool IsReferenceImageExist { get; set; }

        //public string Code
        //{
        //    get { return code; }
        //    set
        //    {
        //        code = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Code"));
        //    }
        //}
        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
        }


        public List<WarehouseDeliveryNoteItem> DeletedReferenceRowList
        {
            get
            {
                return deletedReferenceRowList;
            }

            set
            {
                deletedReferenceRowList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedReferenceRowList"));
            }
        }
        public bool IsReferenceDeleted
        {
            get { return isReferenceDeleted; }
            set
            {
                isReferenceDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReferenceDeleted"));
            }
        }


        public List<ManufacturersByArticle> ListManufacturer
        {
            get { return listManufacturer; }
            set
            {
                listManufacturer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListManufacturer"));
            }
        }
        public ImageSource ReferenceImage
        {
            get { return referenceImage; }
            set { referenceImage = value; OnPropertyChanged(new PropertyChangedEventArgs("ReferenceImage")); }
        }

        //public string SelectedManufacturer
        //{
        //    get { return selectedManufacturer; }
        //    set
        //    {
        //        selectedManufacturer = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedManufacturer"));
        //    }
        //}

        public ManufacturersByArticle Producer
        {
            get { return producer; }
            set
            {
                producer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Producer"));
            }
        }

        public ManufacturersByArticle SelectedProducer
        {
            get { return selectedProducer; }
            set
            {
                selectedProducer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProducer"));
            }
        }



        //public Int32 Quantity
        //{
        //    get { return quantity; }
        //    set
        //    {
        //        quantity = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Quantity"));
        //    }
        //}
        //public string SerialNumberCode
        //{
        //    get { return serialNumberCode; }
        //    set { serialNumberCode = value; OnPropertyChanged(new PropertyChangedEventArgs("SerialNumberCode")); }
        //}
        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set { deliveryDate = value; OnPropertyChanged(new PropertyChangedEventArgs("DeliveryDate")); }
        }

        public string SupplierReference
        {
            get { return supplierReference; }
            set { supplierReference = value; OnPropertyChanged(new PropertyChangedEventArgs("SupplierReference")); }
        }


        public Attachment Attachment
        {
            get { return attachment; }
            set { attachment = value; OnPropertyChanged(new PropertyChangedEventArgs("Attachment")); }
        }
        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set { uniqueFileName = value; OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName")); }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged(new PropertyChangedEventArgs("FileName")); }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public ObservableCollection<WarehouseDeliveryNote> WarehouseDeliveryNoteDetails
        {
            get
            {
                return warehouseDeliveryNoteDetails;
            }
            set
            {
                warehouseDeliveryNoteDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseDeliveryNoteDetails"));
            }
        }

        public string SupplierTypeName
        {
            get
            {
                return supplierTypeName;
            }

            set
            {
                supplierTypeName = value;
            }
        }

        public string SupplierTypeColor
        {
            get
            {
                return supplierTypeColor;
            }

            set
            {
                supplierTypeColor = value;
            }
        }

        public byte[] PdfFileInBytes
        {
            get
            {
                return pdfFileInBytes;
            }

            set
            {
                pdfFileInBytes = value;
            }
        }

        public WarehouseDeliveryNoteItem SelectedGridRow
        {
            get
            {
                return selectedGridRow;
            }

            set
            {
                selectedGridRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGridRow"));

                if (selectedGridRow != null)
                {
                    if (selectedGridRow.Uploaded == 0)
                    {
                        IsUpdoadStock = true;
                        IsDownloadStock = false;
                    }
                    else
                    {
                        IsUpdoadStock = false;
                        IsDownloadStock = true;
                    }

                    SetReferenceImage(selectedGridRow.WarehousePurchaseOrderItem.Article.ArticleImageInBytes);
                    ChangeCurrentStockColor(selectedGridRow.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock, selectedGridRow.WarehousePurchaseOrderItem.Article.MyWarehouse.MinimumStock);
                }
            }
        }

        public string CurrentStockFgColor
        {
            get
            {
                return currentStockFgColor;
            }

            set
            {
                currentStockFgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentStockFgColor"));
            }
        }

        public string CurrentStockBgColor
        {
            get
            {
                return currentStockBgColor;
            }

            set
            {
                currentStockBgColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentStockBgColor"));
            }
        }
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
        public double GridWidth { get; set; }
        public double GridColumnWidth { get; set; }
        #endregion

        #region Public ICommands
        public ICommand DNCancelButtonCommand { get; set; }
        public ICommand DNAcceptButtonCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand AddNewRowCommand { get; set; }
        public ICommand QuantityEditValueChangedCommand { get; set; }
        public ICommand DeleteItemRowCommand { get; set; }
        public ICommand PrintRowCommand { get; set; }
        public ICommand CommandPrintLabel { get; set; }
        public ICommand CellValueChangedCommand { get; set; }
        public ICommand ValidateRowCommand { get; set; }
        public ICommand AddNewManufacturerCommand { get; set; }
        public ICommand PreviewKeyDownCommand { get; set; }
        public ICommand AttachDocumentCommand { get; set; }
        public ICommand UploadMaterialCommand { get; set; }
        public ICommand DownloadMaterialCommand { get; set; }
        public ICommand AttachedDocumentSingleClickCommand { get; set; }

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

        #region Constructor
        public EditDeliveryNoteViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DeliveryNoteViewModel ...", category: Category.Info, priority: Priority.Low);
                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 100;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 250;
                GridWidth = DialogWidth * 2 / 3;
                GridColumnWidth = DialogWidth / 4;
                DeliveryDate = DateTime.Now;
                DNCancelButtonCommand = new DelegateCommand<object>(DNCancelButtonAction);
                DNAcceptButtonCommand = new DelegateCommand<object>(SaveDeliveryNote);
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                DeleteItemRowCommand = new DelegateCommand<object>(DeleteItemCommandAction);
                PrintRowCommand = new DelegateCommand<object>(PrintCommandAction);
                AddNewRowCommand = new DelegateCommand<ShowingEditorEventArgs>(AddNewRowCommandAction);
                CommandPrintLabel = new RelayCommand(new Action<object>(PrintLabelViewWindowShow));
                CellValueChangedCommand = new DelegateCommand<CellValueChangedEventArgs>(CellValueChangingAction);
                AddNewManufacturerCommand = new DelegateCommand<object>(AddNewManufacturerCommandAction);
                PreviewKeyDownCommand = new DelegateCommand<KeyEventArgs>(PreviewKeyDownCommandAction);
                AttachDocumentCommand = new DelegateCommand<object>(AttachDocumentAction);
                UploadMaterialCommand = new DelegateCommand<object>(UploadMaterialAction);
                DownloadMaterialCommand = new DelegateCommand<object>(DownloadMaterialAction);
                AttachedDocumentSingleClickCommand = new DelegateCommand<object>(AttachedDocumentSingleClickCommandAction);

                ReportHeaderImage = ResizeImage(new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.EmdepMonoBlue));

                GeosApplication.Instance.Logger.Log("Constructor DeliveryNoteViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor DeliveryNoteViewModel() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Method for attach documents.
        /// </summary>
        /// <param name="obj"></param>
        private void AttachDocumentAction(object obj)
        {

        }

        /// <summary>
        /// Method for Upload Materials.
        /// </summary>
        /// <param name="obj"></param>
        private void UploadMaterialAction(object obj)
        {
            if (SelectedGridRow != null)
            {
                try
                {
                    WarehouseDeliveryNoteItem WarehouseDeliveryNoteItem = SelectedGridRow;

                    if (WarehouseDeliveryNoteItem.IdManufacturerByArticle <= 0)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditDNArticleWithNoProducer").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    WarehouseDeliveryNoteItem.Uploaded = 1;

                    //If article isGeneric then no need insert into article stock.
                    if (WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.IsGeneric == 0)
                    {
                        int Totalquantity = WarehouseDeliveryNoteItems.Sum(x => x.Quantity);
                        double importExpenses = (double)WarehouseDeliveryNote.ImportExpenses;
                        double ImExpEachArt = importExpenses / Totalquantity;
                        double price = WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.UnitPrice;
                        double discount = WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Discount;
                        double IVA = WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.IVA;

                        int quantity = WarehouseDeliveryNoteItem.Quantity;
                        double unitImportExpence = ((CalculatePrice(price, quantity, discount, 0, 0, 0)) / quantity) + ImExpEachArt;
                        double inputPrice = CalculatePrice(unitImportExpence, 1, discount, IVA, 0);

                        ///Insert
                        ArticlesStock articlesStock = new ArticlesStock();
                        articlesStock.IdArticle = Convert.ToUInt32(WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.IdArticle);
                        articlesStock.IdOtItem = null;
                        articlesStock.Quantity = WarehouseDeliveryNoteItem.Quantity;
                        articlesStock.Price = Convert.ToUInt32(WarehouseDeliveryNoteItem.Quantity);
                        articlesStock.UnitPrice = inputPrice;
                        articlesStock.UploadedIn = DateTime.Now;
                        articlesStock.Comments = string.Format("Recibido en el albaran nº {0} [STOCK -> {1}]", WarehouseDeliveryNote.Code, Convert.ToString(Convert.ToInt32(WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock) + WarehouseDeliveryNoteItem.Quantity));
                        articlesStock.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        articlesStock.IdWarehouseProductComponent = null;
                        articlesStock.IdWarehouse = Convert.ToUInt64(WarehouseDeliveryNote.WarehousePurchaseOrder.IdWarehouse);
                        articlesStock.IdWarehouseLocation = null;
                        articlesStock.IdWarehouseDeliveryNoteItem = Convert.ToUInt64(WarehouseDeliveryNoteItem.IdWarehouseDeliveryNoteItem);
                        articlesStock.IdCurrency = WarehouseDeliveryNote.WarehousePurchaseOrder.IdCurrency;

                        WarehouseDeliveryNoteItem.ArticlesStock = articlesStock;
                    }

                    bool result = WarehouseService.UpdateWarehouseDeliveryNoteItem(WarehouseDeliveryNoteItem);

                    if (result)
                    {
                        IsUpdoadStock = false;
                        IsDownloadStock = true;

                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditDNStockUploadSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        if (WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.IsGeneric == 0)
                        {
                            WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock = WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock + WarehouseDeliveryNoteItem.Quantity;
                            ChangeCurrentStockColor(WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock, WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.MinimumStock);
                        }
                    }
                    else
                    {
                        IsUpdoadStock = true;
                        IsDownloadStock = false;
                    }
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in UploadMaterialAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in UploadMaterialAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Method UploadMaterialAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        /// <summary>
        /// Method for Download Materials.
        /// </summary>
        /// <param name="obj"></param>
        private void DownloadMaterialAction(object obj)
        {
            if (SelectedGridRow != null)
            {
                try
                {
                    WarehouseDeliveryNoteItem WarehouseDeliveryNoteItem = SelectedGridRow;

                    WarehouseDeliveryNoteItem.Uploaded = 0;

                    //int stock = WarehouseService.GetArticleStockByWarehouse(Convert.ToInt32(WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.IdArticle), Convert.ToInt32(WarehouseDeliveryNote.WarehousePurchaseOrder.IdWarehouse));

                    int stock = Convert.ToInt32(WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock);

                    if (stock < WarehouseDeliveryNoteItem.Quantity)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditDNLessStockMessage").ToString(), WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.Reference, stock), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        WarehouseDeliveryNoteItem.ArticlesStock = null;
                    }
                    else if (WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.IsGeneric == 0) //If article isGeneric then no need insert into article stock.
                    {
                        double custdutyamt = 0;
                        double custduty = WarehouseDeliveryNoteItem.CustomsDuty;
                        double totalQuantity = WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Quantity;
                        double price1 = WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.UnitPrice;
                        double discount = WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Discount;
                        double IVA = WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.IVA;

                        double totalPrice = CalculatePrice(price1, (int)totalQuantity, discount, IVA, 0);
                        double inputPrice = (totalPrice / totalQuantity);

                        if (custduty > 0)
                        {
                            custdutyamt = (double)(inputPrice / 100) * custduty;
                            inputPrice = inputPrice + custdutyamt;
                        }

                        //Remove
                        ArticlesStock asRemove = new ArticlesStock();
                        asRemove.IdArticle = Convert.ToUInt32(WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.IdArticle);
                        asRemove.IdOtItem = null;
                        asRemove.Quantity = WarehouseDeliveryNoteItem.Quantity * (-1);
                        asRemove.Price = Convert.ToUInt32(WarehouseDeliveryNoteItem.Quantity);
                        asRemove.UnitPrice = inputPrice;
                        asRemove.UploadedIn = DateTime.Now;
                        asRemove.Comments = string.Format("Desrecepcionado albaran nº {0} [STOCK -> {1}]", WarehouseDeliveryNote.Code, Convert.ToString(Convert.ToInt32(WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock) - WarehouseDeliveryNoteItem.Quantity));
                        asRemove.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        asRemove.IdWarehouseProductComponent = null;
                        asRemove.IdWarehouse = Convert.ToUInt64(WarehouseDeliveryNote.WarehousePurchaseOrder.IdWarehouse);
                        asRemove.IdWarehouseLocation = null;
                        asRemove.IdWarehouseDeliveryNoteItem = 0; // Convert.ToUInt64(WarehouseDeliveryNoteItem.IdWarehouseDeliveryNoteItem);
                        asRemove.IdCurrency = WarehouseDeliveryNote.WarehousePurchaseOrder.IdCurrency;

                        WarehouseDeliveryNoteItem.ArticlesStock = asRemove;
                    }

                    bool result = WarehouseService.UpdateWarehouseDeliveryNoteItem(WarehouseDeliveryNoteItem);

                    if (result)
                    {
                        IsUpdoadStock = true;
                        IsDownloadStock = false;

                        if (!(stock < WarehouseDeliveryNoteItem.Quantity))
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditDNStockDownloadSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                            if (WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.IsGeneric == 0)
                            {
                                WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock = WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock - WarehouseDeliveryNoteItem.Quantity;
                                ChangeCurrentStockColor(WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.CurrentStock, WarehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.MinimumStock);
                            }
                        }
                    }
                    else
                    {
                        IsUpdoadStock = false;
                        IsDownloadStock = true;
                    }
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in DownloadMaterialAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in DownloadMaterialAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Method DownloadMaterialAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        static internal double CalculatePrice(double basePrice, int quantity, double discount, double iva, double overCost)
        {
            //[001] Round-Off the value for base price and then calculate the total value
            basePrice = Convert.ToDouble(basePrice.ToString("n5"));
            double total = (basePrice * quantity) * ((100 + iva) / 100) * ((100 - discount) / 100) * ((100 + overCost) / 100);
            return total;
        }

        static internal double CalculatePrice(double basePrice, int quantity, double discount, double iva, double overCost, double customduty)
        {
            //[001] Round-Off the value for base price and then calculate the total value
            basePrice = Convert.ToDouble(basePrice.ToString("n5"));
            double total = (basePrice * quantity) * ((100 + iva) / 100) * ((100 - discount) / 100) * ((100 + overCost) / 100);
            total = total + ((total / 100) * customduty);
            return total;
        }


        private void AttachedDocumentSingleClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AttachedDocumentSingleClickCommandAction()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DeliveryNotePDFView DeliveryNotePDFView = new DeliveryNotePDFView();
                DeliveryNotePDFViewModel DeliveryNotePDFViewModel = new DeliveryNotePDFViewModel();
                DeliveryNotePDFViewModel.OpenPdfByCode(warehouseDeliveryNote.WarehousePurchaseOrder, warehouseDeliveryNote);
                DeliveryNotePDFView.DataContext = DeliveryNotePDFViewModel;
                DeliveryNotePDFView.Show();

                GeosApplication.Instance.Logger.Log("Method AttachedDocumentSingleClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AttachedDocumentSingleClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AttachedDocumentSingleClickCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                CustomMessageBox.Show(string.Format("Could not find file '{0}'.", warehouseDeliveryNote.PdfFilePath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AttachedDocumentSingleClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method For Focus Change on Enter Key Press
        /// </summary>
        /// <param name="obj"></param>
        /// 
        public void PreviewKeyDownCommandAction(KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ColumnBase col;
                TableView table = e.Source as TableView;
                do
                {
                    table.FocusedView.MoveNextCell();
                    col = (table.FocusedView as TableView).Grid.CurrentColumn;
                } while (col.ReadOnly);
                e.Handled = true;
                return;
            }
        }
        public void AddNewManufacturerCommandAction(object obj)
        {
            WarehouseDeliveryNoteItem warehouseDeliveryNoteItem = obj as WarehouseDeliveryNoteItem;
            IdArticle = warehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.IdArticle;

            ListManufacturer = new List<ManufacturersByArticle>();
            ListManufacturer = warehouseDeliveryNoteItem.ManufacturersByArticle;

            AddManufacturerView addManufacturerView = new AddManufacturerView();
            AddManufacturerViewModel addManufacturerViewModel = new AddManufacturerViewModel();
            EventHandler handle = delegate { addManufacturerView.Close(); };
            addManufacturerViewModel.RequestClose += handle;
            addManufacturerViewModel.Init(IdArticle, ListManufacturer);
            addManufacturerView.DataContext = addManufacturerViewModel;
            addManufacturerView.ShowDialog();
            if (addManufacturerViewModel.IsManufacturerAdded)
            {
                warehouseDeliveryNoteItem.ManufacturersByArticle.Add(addManufacturerViewModel.ArticleManufacturer);
                warehouseDeliveryNoteItem.ManufacturersByArticle = new List<ManufacturersByArticle>(warehouseDeliveryNoteItem.ManufacturersByArticle.ToList());

                SelectedProducer = warehouseDeliveryNoteItem.ManufacturersByArticle.Last();
                if (warehouseDeliveryNoteItem.PrevDNIdManufacturerByArticle != null && warehouseDeliveryNoteItem.PrevDNIdManufacturerByArticle != SelectedProducer.IdManufacturerByArticle && SelectedProducer.IdManufacturer == 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("ManufacturerChangedConfirm").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.No)
                    {
                        foreach (var item in warehouseDeliveryNoteItem.ManufacturersByArticle)
                        {
                            if (item.IdManufacturerByArticle == warehouseDeliveryNoteItem.PrevDNIdManufacturerByArticle)
                                warehouseDeliveryNoteItem.Producer = item;
                        }
                    }
                    else
                    {
                        //warehouseDeliveryNoteItem.Producer = warehouseDeliveryNoteItem.ManufacturersByArticle.Last();
                        warehouseDeliveryNoteItem.Producer = SelectedProducer;
                    }
                }
                else
                {
                    warehouseDeliveryNoteItem.Producer = SelectedProducer;
                }
            }
        }

        public ImageSource SetReferenceImage(byte[] Image)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetReferenceImage ...", category: Category.Info, priority: Priority.Low);
                ReferenceImageByte = Image;

                if (ReferenceImageByte != null)
                    ReferenceImage = ByteArrayToBitmapImage(ReferenceImageByte);
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        ReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Warehouse;component/Assets/Images/ImageEditLogo.png"));
                    else
                        ReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Warehouse;component/Assets/Images/ImageEditLogo.png"));
                }

                GeosApplication.Instance.Logger.Log("Method SetReferenceImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetReferenceImage() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetReferenceImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetReferenceImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return ReferenceImage;

        }

        /// <summary>
        /// Method For Cell value change of Quantity
        /// </summary>
        /// <param name="e"></param>
        public void CellValueChangingAction(CellValueChangedEventArgs e)
        {
            try
            {
                TableView dxTableView = e.OriginalSource as TableView;
                GridControl dxGrid = dxTableView.DataControl as GridControl;
                if (e.Column.Name == "Quantity" && e.Row is WarehouseDeliveryNoteItem)
                {
                    int oldValue = (int)e.OldValue;
                    int newValue = Convert.ToInt32(e.Value);

                    WarehouseDeliveryNoteItem warehouseDeliveryNoteItem = e.Row as WarehouseDeliveryNoteItem;
                    if (warehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.RegisterSerialNumber == 1)
                    {
                        if (newValue > oldValue)
                        {
                            int Diffrance = newValue - oldValue;
                            for (int i = 0; i < Diffrance; i++)
                            {
                                SerialNumber serialNumber = new SerialNumber();
                                //SerialNumberCode=string.Empty;
                                serialNumber.Code = "";
                                if (warehouseDeliveryNoteItem.SerialNumbers == null)
                                {
                                    warehouseDeliveryNoteItem.SerialNumbers = new List<SerialNumber>();
                                }
                                warehouseDeliveryNoteItem.SerialNumbers.Add(serialNumber);

                                warehouseDeliveryNoteItem.SerialNumbers = new List<SerialNumber>(warehouseDeliveryNoteItem.SerialNumbers.ToList());
                            }
                        }
                        else
                        {
                            int Diffrance = oldValue - newValue;
                            int counter = warehouseDeliveryNoteItem.SerialNumbers.Count;

                            for (int i = counter - 1; i >= newValue; i--)
                            {
                                warehouseDeliveryNoteItem.SerialNumbers.RemoveAt(i);
                                warehouseDeliveryNoteItem.SerialNumbers = new List<SerialNumber>(warehouseDeliveryNoteItem.SerialNumbers.ToList());
                            }
                        }
                    }
                }
                if (e.Column.Name == "Producer" && e.Value is ManufacturersByArticle)
                {

                    WarehouseDeliveryNoteItem warehouseDeliveryNoteItem = e.Row as WarehouseDeliveryNoteItem;
                    IdArticle = warehouseDeliveryNoteItem.WarehousePurchaseOrderItem.Article.IdArticle;
                    SelectedProducer = (ManufacturersByArticle)e.Value;
                    if (warehouseDeliveryNoteItem.PrevDNIdManufacturerByArticle != null && warehouseDeliveryNoteItem.PrevDNIdManufacturerByArticle != SelectedProducer.IdManufacturerByArticle && SelectedProducer.IdManufacturer != 0)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("ManufacturerChangedConfirm").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.No)
                        {
                            foreach (var item in warehouseDeliveryNoteItem.ManufacturersByArticle)
                            {
                                if (item.IdManufacturerByArticle == warehouseDeliveryNoteItem.PrevDNIdManufacturerByArticle)
                                    warehouseDeliveryNoteItem.Producer = item;

                            }
                        }
                    }
                    //    if (SelectedProducer.IdManufacturer == 0)
                    //    {
                    //        ListManufacturer = new List<ManufacturersByArticle>();
                    //        ListManufacturer = warehouseDeliveryNoteItem.ManufacturersByArticle;
                    //        AddManufacturerView addManufacturerView = new AddManufacturerView();
                    //        AddManufacturerViewModel addManufacturerViewModel = new AddManufacturerViewModel();
                    //        EventHandler handle = delegate { addManufacturerView.Close(); };
                    //        addManufacturerViewModel.RequestClose += handle;
                    //        addManufacturerViewModel.Init(IdArticle, ListManufacturer);
                    //        addManufacturerView.DataContext = addManufacturerViewModel;
                    //        addManufacturerView.ShowDialog();
                    //        if (addManufacturerViewModel.IsManufacturerAdded)
                    //        {
                    //            warehouseDeliveryNoteItem.ManufacturersByArticle.Add(addManufacturerViewModel.ArticleManufacturer);
                    //            warehouseDeliveryNoteItem.ManufacturersByArticle = new List<ManufacturersByArticle>(warehouseDeliveryNoteItem.ManufacturersByArticle.ToList());
                    //            warehouseDeliveryNoteItem.Producer = warehouseDeliveryNoteItem.ManufacturersByArticle.Last();

                    //            //if (this.GridControlService != null)
                    //            //    this.GridControlService.HideEditor();
                    //            //e.Value = warehouseDeliveryNoteItem.ManufacturersByArticle;
                    //            // dxGrid.View.CommitEditing();
                    //            dxGrid.View.CloseEditor();

                    //        }
                    //    }
                }

            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// ImageSource to bytes
        /// </summary>
        /// <param name="encoder"></param>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static byte[] ImageSourceToBytes(BitmapEncoder encoder, ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;
            encoder = new TiffBitmapEncoder();
            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        //Bitmap GetImage1(string path)
        //{
        //    return new BitmapImage(new Uri(path, UriKind.Relative));
        //}
        /// <summary>
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// [002][skale][23-12-2019][GEOS2-1855]Allow to set a custom quantity when printing the DN item label in Delivery Note
        /// </summary>
        public void PrintCommandAction(object parameter)
        {
            GeosApplication.Instance.Logger.Log("Method PrintCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (parameter != null)
                {
                    TableView detailView = (TableView)((object[])parameter)[0];
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                    //[002] added
                    DeliveryNotePrintPreviewView deliveryNotePrintPreviewView = new DeliveryNotePrintPreviewView();
                    DeliveryNotePrintPreviewViewModel deliveryNotePrintPreviewViewModel = new DeliveryNotePrintPreviewViewModel();
                    EventHandler handler = delegate { deliveryNotePrintPreviewView.Close(); };
                    deliveryNotePrintPreviewViewModel.RequestClose += handler;
                    deliveryNotePrintPreviewView.DataContext = deliveryNotePrintPreviewViewModel;
                    // WarehouseDeliveryNoteItem deleteItem = parameter as WarehouseDeliveryNoteItem;
                    WarehouseDeliveryNoteItem deleteItem = (WarehouseDeliveryNoteItem)((object[])parameter)[1];
                    deliveryNotePrintPreviewViewModel.Quantity = Convert.ToInt32(deleteItem.Quantity);
                    deliveryNotePrintPreviewViewModel.MaxQuantity = Convert.ToInt32(deleteItem.Quantity);
                    var ownerInfo = (detailView as FrameworkElement);
                    deliveryNotePrintPreviewView.Owner = Window.GetWindow(ownerInfo);
                    deliveryNotePrintPreviewView.ShowDialogWindow();
                   
                    if (deliveryNotePrintPreviewViewModel.IsAccept)
                    {
                        //[002] Comment this code
                        #region Old Code
                        //    WarehouseDeliveryNoteItem deleteItem = parameter as WarehouseDeliveryNoteItem;
                        //    DeliveryNoteReport del = new DeliveryNoteReport();
                        //    del.imgLogo.Image = ReportHeaderImage;
                        //    del.lblOperator.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.xrLblOperatorName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 15, System.Drawing.FontStyle.Regular);
                        //    del.lblSupplier.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.xrLblSupplierName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Regular);
                        //    del.lblProducer.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.xrLblProducerName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Regular);
                        //    del.lblDeliveryNoteCode.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.lblDeliveryNoteDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.xrLblDeliveryNoteCode.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Regular);
                        //    del.xrLblDeliveryNoteDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 55, System.Drawing.FontStyle.Bold);
                        //    del.lblReference.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.xrLblReference.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 55, System.Drawing.FontStyle.Bold);
                        //    del.lblQuantity.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.xrLblQuantity.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 65, System.Drawing.FontStyle.Bold);
                        //    del.lblMadeIn.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.xrLblMadeIn.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.lblTradeGroup.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 80, System.Drawing.FontStyle.Bold);
                        //    del.lblZone.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.lblDeliveryNoteDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        //    del.xrLblZone.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 76, System.Drawing.FontStyle.Bold);
                        //    del.xrBarCode1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 70, System.Drawing.FontStyle.Bold);
                        //    del.lblItem.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);


                        //    List<DeliveryNoteItemPrintDetails> lst = new List<DeliveryNoteItemPrintDetails>();
                        //    DeliveryNoteItemPrintDetails deliveryNoteItemPrintDetails = new DeliveryNoteItemPrintDetails();

                        //    deliveryNoteItemPrintDetails.Name = GeosApplication.Instance.ActiveUser.FullName;
                        //    deliveryNoteItemPrintDetails.SupplierName = ArticleSupplier;
                        //    deliveryNoteItemPrintDetails.Code = DeliveryNoteCode;
                        //    del.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                        //    del.lblTradeGroup.ForeColor = System.Drawing.Color.Black;

                        //    if (deleteItem.Producer != null)
                        //    {

                        //        deliveryNoteItemPrintDetails.ProducerName = deleteItem.Producer.Manufacturer.Name;
                        //        deliveryNoteItemPrintDetails.MadeIn = deleteItem.Producer.Country.Name;
                        //        if (deleteItem.Producer.CountryGroup != null)
                        //        {
                        //            del.lblTradeGroup.Text = deleteItem.Producer.CountryGroup.Name;
                        //            if (deleteItem.Producer.CountryGroup.IsFreeTrade == 1)
                        //            {
                        //                del.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                        //                del.lblTradeGroup.ForeColor = System.Drawing.Color.Black;
                        //                deliveryNoteItemPrintDetails.TradeGroup = deleteItem.Producer.CountryGroup.Name;
                        //                del.lblTradeGroup.Text = deleteItem.Producer.CountryGroup.Name;
                        //            }
                        //            else
                        //            {

                        //                del.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        //                del.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                        //                deliveryNoteItemPrintDetails.TradeGroup = deleteItem.Producer.Country.Iso;
                        //                del.lblTradeGroup.Text = deleteItem.Producer.Country.Iso;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            del.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        //            del.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                        //            deliveryNoteItemPrintDetails.TradeGroup = deleteItem.Producer.Country.Iso;
                        //            del.lblTradeGroup.Text = deleteItem.Producer.Country.Iso;
                        //        }
                        //    }


                        //    deliveryNoteItemPrintDetails.Quantity = Convert.ToInt32(deleteItem.Quantity);
                        //    // [001] Changed Service method
                        //    string[] zoneSplit = WarehouseService.GetZoneByIdArticle_V2034(deleteItem.WarehousePurchaseOrderItem.Article.IdArticle, WarehouseCommon.Instance.Selectedwarehouse).ToString().Split('.');
                        //    if (zoneSplit != null)
                        //        deliveryNoteItemPrintDetails.Zone = zoneSplit[0];
                        //    deliveryNoteItemPrintDetails.Reference = deleteItem.WarehousePurchaseOrderItem.Article.Reference;
                        //    //deliveryNoteItemPrintDetails.Reference = "WSAF - 10150120095 - 0606426RBLNANN - RGTMDB - FRAME - Lokesh-sharma";
                        //    deliveryNoteItemPrintDetails.DeliveryNoteDate = DeliveryDate.Date;
                        //    deliveryNoteItemPrintDetails.DeliveryNoteDateString = DeliveryDate.Date.ToShortDateString();
                        //    deliveryNoteItemPrintDetails.Barcode = GenerateBarcodeString(deleteItem); //"00200088904000140";
                        //    lst.Add(deliveryNoteItemPrintDetails);
                        //    del.DataSource = lst;
                        //    DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                        //    window.PreviewControl.DocumentSource = del;
                        //    del.CreateDocument();
                        //    window.Show();
                        //    ReportPrintTool printTool = new ReportPrintTool(del);
                        //    printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                        //}
                        #endregion

                        DeliveryNoteReport del = new DeliveryNoteReport();
                        del=ReportStyle(del);
                        List<DeliveryNoteItemPrintDetails> lst = new List<DeliveryNoteItemPrintDetails>();
                        DeliveryNoteItemPrintDetails deliveryNoteItemPrintDetails = new DeliveryNoteItemPrintDetails();
                        deliveryNoteItemPrintDetails.Name = GeosApplication.Instance.ActiveUser.FullName;
                        deliveryNoteItemPrintDetails.SupplierName = ArticleSupplier;
                        deliveryNoteItemPrintDetails.Code = DeliveryNoteCode;
                        if (deleteItem.Producer != null)
                        {
                            deliveryNoteItemPrintDetails.ProducerName = deleteItem.Producer.Manufacturer.Name;
                            deliveryNoteItemPrintDetails.MadeIn = deleteItem.Producer.Country.Name;
                            if (deleteItem.Producer.CountryGroup != null)
                            {
                                del.lblTradeGroup.Text = deleteItem.Producer.CountryGroup.Name;
                                if (deleteItem.Producer.CountryGroup.IsFreeTrade == 1)
                                {
                                    del.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                                    del.lblTradeGroup.ForeColor = System.Drawing.Color.Black;
                                    deliveryNoteItemPrintDetails.TradeGroup = deleteItem.Producer.CountryGroup.Name;
                                    del.lblTradeGroup.Text = deleteItem.Producer.CountryGroup.Name;
                                }
                                else
                                {

                                    del.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                                    del.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                                    deliveryNoteItemPrintDetails.TradeGroup = deleteItem.Producer.Country.Iso;
                                    del.lblTradeGroup.Text = deleteItem.Producer.Country.Iso;
                                }
                            }
                            else
                            {
                                del.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                                del.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                                deliveryNoteItemPrintDetails.TradeGroup = deleteItem.Producer.Country.Iso;
                                del.lblTradeGroup.Text = deleteItem.Producer.Country.Iso;
                            }
                        }
                        //[002]added
                        deliveryNoteItemPrintDetails.Quantity = deliveryNotePrintPreviewViewModel.Quantity;
                        del.xrLblPlant.Text = WarehouseCommon.Instance.Selectedwarehouse.Name;
                        // [001] Changed Service method
                        del.xrLblWarehouselocation.Text = WarehouseService.GetZoneByIdArticle_V2034(deleteItem.WarehousePurchaseOrderItem.Article.IdArticle, WarehouseCommon.Instance.Selectedwarehouse).ToString().Trim();
                        deliveryNoteItemPrintDetails.Reference = deleteItem.WarehousePurchaseOrderItem.Article.Reference;
                        deliveryNoteItemPrintDetails.DeliveryNoteDate = DeliveryDate.Date;
                        deliveryNoteItemPrintDetails.DeliveryNoteDateString = DeliveryDate.Date.ToShortDateString();
                        deliveryNoteItemPrintDetails.Barcode = GenerateBarcodeString(deleteItem, deliveryNoteItemPrintDetails.Quantity); //"00200088904000140";
                        lst.Add(deliveryNoteItemPrintDetails);
                        del.DataSource = lst;
                        DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                        window.PreviewControl.DocumentSource = del;
                        del.CreateDocument();
                        //create no of copies

                        //for (int i = 0; i < deliveryNotePrintPreviewViewModel.Copies - 1; i++)
                        //{
                        //    DeliveryNoteReport reportCopy = new DeliveryNoteReport();
                        //    reportCopy=ReportStyle(reportCopy);
                        //    reportCopy.xrBarCode1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrBarCode1_BeforePrint);
                        //    reportCopy.imgLogo.Image = ReportHeaderImage;
                        //    reportCopy.xrLblPlant.Text = WarehouseCommon.Instance.Selectedwarehouse.Name;
                        //    reportCopy.xrLblWarehouselocation.Text = del.xrLblWarehouselocation.Text;

                        //    if (deleteItem.Producer != null)
                        //    {
                        //        deliveryNoteItemPrintDetails.ProducerName = deleteItem.Producer.Manufacturer.Name;
                        //        deliveryNoteItemPrintDetails.MadeIn = deleteItem.Producer.Country.Name;
                        //        if (deleteItem.Producer.CountryGroup != null)
                        //        {
                        //            reportCopy.lblTradeGroup.Text = deleteItem.Producer.CountryGroup.Name;
                        //            if (deleteItem.Producer.CountryGroup.IsFreeTrade == 1)
                        //            {
                        //                reportCopy.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                        //                reportCopy.lblTradeGroup.ForeColor = System.Drawing.Color.Black;
                        //                deliveryNoteItemPrintDetails.TradeGroup = deleteItem.Producer.CountryGroup.Name;
                        //                reportCopy.lblTradeGroup.Text = deleteItem.Producer.CountryGroup.Name;
                        //            }
                        //            else
                        //            {

                        //                reportCopy.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        //                reportCopy.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                        //                deliveryNoteItemPrintDetails.TradeGroup = deleteItem.Producer.Country.Iso;
                        //                reportCopy.lblTradeGroup.Text = deleteItem.Producer.Country.Iso;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            reportCopy.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        //            reportCopy.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                        //            deliveryNoteItemPrintDetails.TradeGroup = deleteItem.Producer.Country.Iso;
                        //            reportCopy.lblTradeGroup.Text = deleteItem.Producer.Country.Iso;
                        //        }
                        //    }

                        //    reportCopy.DataSource = lst;
                        //    reportCopy.CreateDocument();

                        //    del.ModifyDocument(x =>
                        //    {
                        //        x.AddPages(reportCopy.Pages);
                        //    });
                        //}

                        window.Show();
                        ReportPrintTool printTool = new ReportPrintTool(del);
                        printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                    }
                    //end
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Method PrintCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }




        public DeliveryNoteReport ReportStyle(DeliveryNoteReport DeliveryNoteReport)
        {
            DeliveryNoteReport.xrBarCode1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrBarCode1_BeforePrint);
            DeliveryNoteReport.imgLogo.Image = ReportHeaderImage;
            DeliveryNoteReport.lblOperator.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.xrLblOperatorName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 15, System.Drawing.FontStyle.Regular);
            DeliveryNoteReport.lblSupplier.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.xrLblSupplierName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Regular);
            DeliveryNoteReport.lblProducer.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.xrLblProducerName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Regular);
            DeliveryNoteReport.lblDeliveryNoteCode.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.lblDeliveryNoteDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.xrLblDeliveryNoteCode.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Regular);
            DeliveryNoteReport.xrLblDeliveryNoteDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 55, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.lblReference.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.xrLblReference.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 55, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.lblQuantity.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.xrLblQuantity.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 65, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.lblMadeIn.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.xrLblMadeIn.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.lblTradeGroup.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 80, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.lblWarehouselocation.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.lblDeliveryNoteDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.xrBarCode1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 70, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.lblItem.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);

            DeliveryNoteReport.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
            DeliveryNoteReport.lblTradeGroup.ForeColor = System.Drawing.Color.Black;

            DeliveryNoteReport.xrLblWarehouselocation.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 38, System.Drawing.FontStyle.Bold);
            DeliveryNoteReport.xrLblPlant.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
            return DeliveryNoteReport;
        }

        /// <summary>
        /// [000][skale][23-12-2019][GEOS2-1855] Allow to set a custom quantity when printing the DN item label in Delivery Note
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrBarCode1_BeforePrint(object sender, PrintEventArgs e)
        {
            XRBarCode label = (XRBarCode)sender;
            label.AutoModule = false;

            BarCodeError berror = label.Validate();
            if (berror == BarCodeError.ControlBoundsTooSmall)
            {
                label.AutoModule = true;
            }
        }
        /// <summary>
        /// method for resize image.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private Bitmap ResizeImage(Bitmap bitmap)
        {
            GeosApplication.Instance.Logger.Log("Method ResizeImage ...", category: Category.Info, priority: Priority.Low);

            Bitmap resized = new Bitmap(300, 100);

            try
            {

                Graphics g = Graphics.FromImage(resized);

                g.DrawImage(bitmap, new Rectangle(0, 0, resized.Width, resized.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
                g.Dispose();

                //Save picture in users temp folder.
                string myTempFile = Path.Combine(Path.GetTempPath(), "EmdepLogo.jpg");

                //delete if already image exist there.
                if (File.Exists(myTempFile))
                {
                    File.Delete(myTempFile);
                }

                resized.Save(myTempFile, ImageFormat.Jpeg);

                return resized;

                GeosApplication.Instance.Logger.Log("Method ResizeImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ResizeImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return resized;
        }

        /// <summary>
        /// Method For Delete Row
        /// </summary>
        /// <param name="parameter"></param>
        public void DeleteItemCommandAction(object parameter)
        {
            try
            {
                WarehouseDeliveryNoteItem deleteItem = parameter as WarehouseDeliveryNoteItem;

                GeosApplication.Instance.Logger.Log("Method DeleteItemCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (parameter != null)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("EditDeliveryNoteDeleteItemsConfirm").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        List<WarehouseDeliveryNoteItem> TempWarehouseDeliveryNoteItem = new List<WarehouseDeliveryNoteItem>();
                        foreach (var item in WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems)
                        {
                            TempWarehouseDeliveryNoteItem.Add(item);
                        }
                        for (int i = 0; i < TempWarehouseDeliveryNoteItem.Count; i++)
                        {
                            if (TempWarehouseDeliveryNoteItem[i].IdWarehousePurchaseOrderItem == deleteItem.IdWarehousePurchaseOrderItem)
                            {
                                DeletedReferenceRowList.Add(WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems[i]);
                                WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems.Remove(WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems[i]);

                            }
                        }
                        WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems = new List<WarehouseDeliveryNoteItem>(WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems);
                        IsReferenceDeleted = true;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteItemCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteItemCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for change Total stock color as per condition.
        /// </summary>
        /// <param name="totalStock"></param>
        /// <param name="minQuantity"></param>
        private void ChangeCurrentStockColor(Int64 CurrentStock, Int64 minQuantity)
        {
            if (CurrentStock == 0)
            {
                CurrentStockFgColor = "#FFFFFFFF";
                CurrentStockBgColor = "#FFFF0000";
            }
            else if (CurrentStock > minQuantity)
            {
                CurrentStockFgColor = "#FFFFFFFF";
                CurrentStockBgColor = "#FF008000";
            }
            else if (CurrentStock < minQuantity)
            {
                CurrentStockFgColor = "#FF000000";
                CurrentStockBgColor = "#FFFFFF00";
            }

        }

        /// <summary>
        /// Method For Adding New Row
        /// </summary>
        /// <param name="obj"></param>
        public void AddNewRowCommandAction(ShowingEditorEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewRowCommandAction ...", category: Category.Info, priority: Priority.Low);


                // TableView dxTableView = obj.OriginalSource as TableView;
                // GridControl dxGrid = dxTableView.DataControl as GridControl;

                //if (obj.RowHandle != GridControl.NewItemRowHandle && obj.Column.FieldName == "Supplier Ref")
                //{
                //    obj.Cancel = true;
                //    obj.Handled = true;
                //}
                //if (obj.RowHandle != GridControl.NewItemRowHandle && obj.Column.FieldName == "ArticleBySupplier.Reference")
                //{
                //    obj.Cancel = true;
                //    obj.Handled = true;
                //}
                //if (obj.RowHandle != GridControl.NewItemRowHandle && obj.Column.FieldName == "WarehousePurchaseOrderItem.Description")
                //{
                //    obj.Cancel = true;
                //    obj.Handled = true;
                //}

                GeosApplication.Instance.Logger.Log("Method AddNewRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewRowCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method For Initialize 
        /// </summary>
        /// <param name="warehousePurchaseOrder"></param>
        public void Init(WarehouseDeliveryNote warehouseDeliveryNote)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);

                WarehouseDeliveryNote = warehouseDeliveryNote;

                DeliveryNoteCode = warehouseDeliveryNote.Code;
                ArticleSupplier = warehouseDeliveryNote.WarehousePurchaseOrder.ArticleSupplier.Name;

                AttchedDocument = string.Format(@"DN_{0}.pdf", warehouseDeliveryNote.Code);

                WarehouseDeliveryNoteItems = new ObservableCollection<WarehouseDeliveryNoteItem>(warehouseDeliveryNote.WarehouseDeliveryNoteItems.ToList());

                if (WarehouseDeliveryNoteItems != null && WarehouseDeliveryNoteItems.Count > 0)
                {
                    SelectedGridRow = WarehouseDeliveryNoteItems[0];

                    foreach (WarehouseDeliveryNoteItem item in WarehouseDeliveryNoteItems)
                    {
                        if (item.IdManufacturerByArticle > 0)
                        {
                            item.Producer = item.ManufacturersByArticle.FirstOrDefault(x => x.IdManufacturerByArticle == item.IdManufacturerByArticle);
                        }

                    }
                }


                //WarehouseDeliveryNote warehouseDN = WarehouseService.GenerateDeliveryNote(warehousePurchaseOrder);
                //warehouseDN.WarehousePurchaseOrder = warehousePurchaseOrder;
                //warehouseDN.IdWarehousePurchaseOrder = warehousePurchaseOrder.IdWarehousePurchaseOrder;

                //WarehouseDeliveryNoteDetails = new ObservableCollection<WarehouseDeliveryNote>();
                //WarehouseDeliveryNoteDetails.Add(warehouseDN);
                //Warehouse = warehousePurchaseOrder.Warehouse;

                SupplierReference = warehouseDeliveryNote.SupplierCode;


                if (warehouseDeliveryNote.WarehousePurchaseOrder.ArticleSupplier.ArticleSupplierType != null)
                {
                    SupplierTypeName = warehouseDeliveryNote.WarehousePurchaseOrder.ArticleSupplier.ArticleSupplierType.Name;
                    SupplierTypeColor = warehouseDeliveryNote.WarehousePurchaseOrder.ArticleSupplier.ArticleSupplierType.HtmlColor;
                }

                DeliveryDate = warehouseDeliveryNote.DeliveryDate;

                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Browse File to choose pdf only
        /// </summary>
        /// <param name="obj"></param>
        public void BrowseFileCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            IsBusy = true;
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    WarehouseDeliveryNote.PdfFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    bool isSaved = WarehouseService.SaveWarehouseDeliveryNotePdf(WarehouseDeliveryNote);

                    if (isSaved)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditDeliveryNoteSaveSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show(string.Format("{0}", ex.Message), "Red", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            }

            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// Method for Close 
        /// </summary>
        /// <param name="obj"></param>
        private void DNCancelButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DNCancelButtonAction()...", category: Category.Info, priority: Priority.Low);

                IsDeliveryNoteSaved = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method DNCancelButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DNCancelButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for save Delivery Note
        /// </summary>
        /// <param name="obj"></param>
        public void SaveDeliveryNote(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveDeliveryNote ...", category: Category.Info, priority: Priority.Low);
                InformationError = null;
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("DeliveryDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SupplierReference"));
                PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                //PropertyChanged(this, new PropertyChangedEventArgs("Quantity"));
                //PropertyChanged(this, new PropertyChangedEventArgs("Producer"));
                //PropertyChanged(this, new PropertyChangedEventArgs("Code"));

                if (error != null)
                {

                    //dxTableView.ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.RowAndCell;
                    //for (int i = 0; i < WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems.Count; i++)
                    //    if (WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems[i] != null)
                    //    {
                    //        //MessageBox.Show("Correct values!");
                    //        return;
                    //    }
                    //MessageBox.Show("OK");
                    return;
                }

                TableView dxTableView = obj as TableView;
                //dxTableView.ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.RowAndCell;
                GridColumnValidationHelper.SetAllowValidation(dxTableView, true);
                dxTableView.Grid.RefreshData();



                //GridColumnValidationHelper.SetAllowValidation(dxTableView.Grid.DetailDescriptor, true);
                //for (int i = 0; i < dxTableView.Grid.VisibleRowCount; i++)
                //{
                //    var rowHandle = dxTableView.Grid.GetRowHandleByVisibleIndex(i);
                //    if (dxTableView.Grid.IsMasterRowExpanded(rowHandle))
                //        dxTableView.Grid.GetDetail(rowHandle).RefreshData();
                //}

                WarehouseDeliveryNoteDetails[0].Code = DeliveryNoteCode;
                WarehouseDeliveryNoteDetails[0].DeliveryDate = DeliveryDate;
                WarehouseDeliveryNoteDetails[0].CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                WarehouseDeliveryNoteDetails[0].ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                WarehouseDeliveryNoteDetails[0].SupplierCode = SupplierReference.Trim();
                WarehouseDeliveryNoteDetails[0].ExchangeRate = 0;
                WarehouseDeliveryNoteDetails[0].IdCurrency = 1;
                WarehouseDeliveryNoteDetails[0].CurrentlyAccessedBy = GeosApplication.Instance.ActiveUser.IdUser;
                WarehouseDeliveryNoteDetails[0].ZF01 = "";
                WarehouseDeliveryNoteDetails[0].ImportExpenses = 0;
                WarehouseDeliveryNoteDetails[0].IdCurrencyImportExpenses = 0;
                WarehouseDeliveryNoteDetails[0].PdfFileInBytes = PdfFileInBytes;

                List<WarehouseDeliveryNoteItem> TempWarehouseDeliveryNoteItem = new List<WarehouseDeliveryNoteItem>();
                foreach (var item in WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems)
                {
                    TempWarehouseDeliveryNoteItem.Add(item);
                }

                foreach (var item in TempWarehouseDeliveryNoteItem)
                {
                    if (item.Quantity != 0 && item.Producer != null)//&& item.WarehousePurchaseOrderItem.Article.ArticleImageInBytes!=null)
                    {
                        item.Uploaded = 1;
                        item.IdManufacturerByArticle = item.Producer.IdManufacturerByArticle;
                    }
                    else
                    {
                        item.Uploaded = 0;
                        WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems.Remove(item);
                    }
                }

                WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems = new List<WarehouseDeliveryNoteItem>(WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems);
                if (WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems.Count > 0)
                {
                    WarehouseDeliveryNoteDetails[0] = WarehouseService.AddWarehouseDeliveryNote(WarehouseDeliveryNoteDetails[0]);

                }


                if (WarehouseDeliveryNoteDetails[0].IdWarehouseDeliveryNote > 0)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeliveryNoteSaveSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    IsDeliveryNoteSaved = true;
                    RequestClose(null, null);
                }
                else
                {
                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeliveryNoteSaveUnSuccessfull").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    IsDeliveryNoteSaved = false;
                    WarehouseDeliveryNoteDetails[0].WarehouseDeliveryNoteItems = new List<WarehouseDeliveryNoteItem>(TempWarehouseDeliveryNoteItem);

                }


                GeosApplication.Instance.Logger.Log("Method SaveDeliveryNote() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SaveDeliveryNote() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }
        /// <summary>
        /// Method For Print Label
        /// </summary>
        /// <param name="obj"></param>
        private void PrintLabelViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintLabelViewWindowShow()...", category: Category.Info, priority: Priority.Low);
                // DXSplashScreen.Show<SplashScreenView>();
                //PrintLabelView printLabelView = new PrintLabelView();
                //PrintLabelViewModel printLabelViewModel = new PrintLabelViewModel();
                //EventHandler handle = delegate { printLabelView.Close(); };
                //printLabelViewModel.RequestClose += handle;
                //printLabelView.DataContext = printLabelViewModel;
                //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                //printLabelView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method PrintLabelViewWindowShow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintLabelViewWindowShow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return null;
        }


        /// <summary>
        ///  This method is for to get image in bitmap by path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }

        /// <summary>
        /// Method for create barcode.
        /// </summary>
        /// [001][avpawar][5-2-2020][GEOS2-2027] Wrong barcodes in DN A4 labels
        /// <param name="deleteItem"></param>
        /// <returns></returns>
        private string GenerateBarcodeString(WarehouseDeliveryNoteItem deleteItem,long selectedQuantity)
        {

            GeosApplication.Instance.Logger.Log("Method GenerateBarcodeString ...", category: Category.Info, priority: Priority.Low);

            string idwarehouseStr = string.Empty;
            string idwarehousedeliveryNoteItemStr = string.Empty;
            string QuantityStr = string.Empty;
            string BarcodeStr = string.Empty;

            try
            {


                //int qtyLenth = deleteItem.Quantity.ToString().Length;
                int qtyLenth = selectedQuantity.ToString().Length;          //[001] Added
                int idwarehousedeliveryNoteIteLenth = deleteItem.IdWarehouseDeliveryNoteItem.ToString().Length;
                int idwarehouseLenth = deleteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.IdWarehouse.ToString().Length;

                //for warehuse;
                for (int i = 0; i < 3 - idwarehouseLenth; i++)
                {

                    if (!string.IsNullOrEmpty(idwarehouseStr) && idwarehouseStr.Length > 0)
                    {
                        idwarehouseStr = idwarehouseStr + "0";
                    }
                    else
                    {
                        idwarehouseStr = "0";
                    }
                }

                idwarehouseStr = idwarehouseStr + deleteItem.WarehousePurchaseOrderItem.Article.MyWarehouse.IdWarehouse.ToString();

                //for idwarehuseDeleveryNoteItem;
                for (int i = 0; i < 8 - idwarehousedeliveryNoteIteLenth; i++)
                {

                    if (!string.IsNullOrEmpty(idwarehousedeliveryNoteItemStr) && idwarehousedeliveryNoteItemStr.Length > 0)
                    {
                        idwarehousedeliveryNoteItemStr = idwarehousedeliveryNoteItemStr + "0";
                    }
                    else
                    {
                        idwarehousedeliveryNoteItemStr = "0";
                    }
                }

                idwarehousedeliveryNoteItemStr = idwarehousedeliveryNoteItemStr + deleteItem.IdWarehouseDeliveryNoteItem.ToString();

                //for quantity;
                for (int i = 0; i < 6 - qtyLenth; i++)
                {

                    if (!string.IsNullOrEmpty(QuantityStr) && QuantityStr.Length > 0)
                    {
                        QuantityStr = QuantityStr + "0";
                    }
                    else
                    {
                        QuantityStr = "0";
                    }
                }

                // QuantityStr = QuantityStr + deleteItem.Quantity.ToString();

                 QuantityStr = QuantityStr + selectedQuantity.ToString();

                BarcodeStr = idwarehouseStr + idwarehousedeliveryNoteItemStr + QuantityStr;

                GeosApplication.Instance.Logger.Log("Method GenerateBarcodeString() executed successfully", category: Category.Info, priority: Priority.Low);

            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GenerateBarcodeString() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return BarcodeStr;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                     me[BindableBase.GetPropertyName(() => DeliveryDate)] +
                     me[BindableBase.GetPropertyName(() => InformationError)] +
                     me[BindableBase.GetPropertyName(() => SupplierReference)];// +
                                                                               //me[BindableBase.GetPropertyName(() => Code)];// +
                                                                               // me[BindableBase.GetPropertyName(() => Quantity)] +
                                                                               // me[BindableBase.GetPropertyName(() => Producer)];


                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string DeliveryDateProp = BindableBase.GetPropertyName(() => DeliveryDate);
                string SupplierReferenceProp = BindableBase.GetPropertyName(() => SupplierReference);
                string informationError = BindableBase.GetPropertyName(() => InformationError);
                //string CodeProp = BindableBase.GetPropertyName(() => Code);
                //string QuantityProp = BindableBase.GetPropertyName(() => Quantity);
                //string SelectedProducerProp = BindableBase.GetPropertyName(() => Producer);


                if (columnName == DeliveryDateProp)
                    return WarehouseValidation.GetErrorMessage(DeliveryDateProp, DeliveryDate);
                else if (columnName == SupplierReferenceProp)
                    return WarehouseValidation.GetErrorMessage(SupplierReferenceProp, SupplierReference);
                else if (columnName == informationError)

                    return WarehouseValidation.GetErrorMessage(informationError, InformationError);

                //else if (columnName == CodeProp)
                //{
                //    //if (Code == null)
                //    //{
                //    //    return "Please Enter Serial Number";
                //    //}
                //    return WarehouseValidation.GetErrorMessage(CodeProp, Code);
                //}
                //else if (columnName == QuantityProp)
                //{
                //    //if (Quantity == 0)
                //    // Quantity = -1;
                //    // return "Error";
                //    return WarehouseValidation.GetErrorMessage(QuantityProp, Quantity);
                //}
                //else if (columnName == SelectedProducerProp)
                //{
                //    return WarehouseValidation.GetErrorMessage(SelectedProducerProp, Producer);
                //}

                return null;
            }
        }
        #endregion
    }
}
