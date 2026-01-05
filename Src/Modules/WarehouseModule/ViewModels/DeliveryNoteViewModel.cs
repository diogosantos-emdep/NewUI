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
using Emdep.Geos.UI.Common;
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

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class DeliveryNoteViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
		#region TaskLog
		
		//[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
		#endregion
		
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private Attachment attachment;
        private ObservableCollection<WarehouseDeliveryNote> warehouseDeliveryNoteDetails;
        private WarehouseDeliveryNoteItem selectedGridRow;
        private string uniqueFileName;
        private string fileName;
        private bool isBusy;
        private DateTime deliveryDate;
        private string supplierReference;
        private string articleSupplier;
        private string deliveryNoteCode;
        private string supllierTypeName;
        private string supplierTypeColor;
        private byte[] pdfFileInBytes;
        private Warehouses warehouse;
        private string informationError;

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

        private List<WarehouseDeliveryNoteItem> deletedReferenceRowList = new List<WarehouseDeliveryNoteItem>();

        #endregion


        #region Properties
        public Warehouses Warehouse { get; set; }
        public string ArticleSupplier { get; set; }
        public long IdCountryGroup { get; set; }
        public string DeliveryNoteCode { get; set; }

        public bool IsDeliveryNoteSaved { get; set; }

        public Int32 IdArticle { get; set; }

        public bool IsReferenceImageExist { get; set; }
        public string Country { get; set; }
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

        public string SupllierTypeName
        {
            get
            {
                return supllierTypeName;
            }

            set
            {
                supllierTypeName = value;
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
                    SetReferenceImage(selectedGridRow.WarehousePurchaseOrderItem.Article.ArticleImageInBytes);
            }
        }




        #endregion

        #region Public ICommands
        public ICommand DNCancelButtonCommand { get; set; }
        public ICommand DNAcceptButtonCommand { get; set; }

        public ICommand ChooseFileCommand { get; set; }

        public ICommand AddNewRowCommand { get; set; }

        public ICommand QuantityEditValueChangedCommand { get; set; }

        public ICommand DeleteItemRowCommand { get; set; }

        public ICommand CommandPrintLabel { get; set; }

        public ICommand CellValueChangedCommand { get; set; }

        public ICommand ValidateRowCommand { get; set; }

        public ICommand AddNewManufacturerCommand { get; set; }
        public ICommand PreviewKeyDownCommand { get; set; }





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
        public DeliveryNoteViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DeliveryNoteViewModel ...", category: Category.Info, priority: Priority.Low);
                DeliveryDate = DateTime.Now;
                DNCancelButtonCommand = new DelegateCommand<object>(DNCancelButtonAction);
                DNAcceptButtonCommand = new DelegateCommand<object>(SaveDeliveryNote);
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                DeleteItemRowCommand = new DelegateCommand<object>(DeleteItemCommandAction);
                AddNewRowCommand = new DelegateCommand<ShowingEditorEventArgs>(AddNewRowCommandAction);
                CommandPrintLabel = new RelayCommand(new Action<object>(PrintLabelViewWindowShow));
                CellValueChangedCommand = new DelegateCommand<CellValueChangedEventArgs>(CellValueChangingAction);
                AddNewManufacturerCommand = new DelegateCommand<object>(AddNewManufacturerCommandAction);
                PreviewKeyDownCommand = new DelegateCommand<KeyEventArgs>(PreviewKeyDownCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor DeliveryNoteViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor DeliveryNoteViewModel() Method " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }


        #endregion
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

                if (IdCountryGroup == addManufacturerViewModel.ArticleManufacturer.Country.IdCountryGroup)
                {
                    addManufacturerViewModel.ArticleManufacturer.IsSameCountryGroup = true;
                }
                else
                {
                    addManufacturerViewModel.ArticleManufacturer.IsSameCountryGroup = false;
                }

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
                    if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                        ReferenceImage = GetImage("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/wNA.png");
                    else
                        ReferenceImage = GetImage("/Emdep.Geos.Modules.Warehouse;component/Assets/Images/bNA.png");
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

        #region Methods
        /// <summary>
        /// Method For Delete Row
        /// </summary>
        /// <param name="parameter"></param>
        public void DeleteItemCommandAction(object parameter)
        {
            try
            {
                WarehouseDeliveryNoteItem deleteItem = parameter as WarehouseDeliveryNoteItem;

                GeosApplication.Instance.Logger.Log("Method convert DeleteItemCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (parameter != null)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteItemsConfirm").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
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
        public void Init(WarehousePurchaseOrder warehousePurchaseOrder)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);


                WarehouseDeliveryNote warehouseDN = WarehouseService.GenerateDeliveryNote(warehousePurchaseOrder);
                warehouseDN.WarehousePurchaseOrder = warehousePurchaseOrder;
                warehouseDN.IdWarehousePurchaseOrder = warehousePurchaseOrder.IdWarehousePurchaseOrder;

                WarehouseDeliveryNoteDetails = new ObservableCollection<WarehouseDeliveryNote>();
                WarehouseDeliveryNoteDetails.Add(warehouseDN);
                Warehouse = warehousePurchaseOrder.Warehouse;
                ArticleSupplier = warehousePurchaseOrder.ArticleSupplier.Name;
                IdCountryGroup = warehousePurchaseOrder.ArticleSupplier.Country.IdCountryGroup;
                DeliveryNoteCode = WarehouseDeliveryNoteDetails[0].Code;
                Country = warehousePurchaseOrder.ArticleSupplier.Country.Name;

                if (warehousePurchaseOrder.ArticleSupplier.ArticleSupplierType != null)
                {
                    SupllierTypeName = warehousePurchaseOrder.ArticleSupplier.ArticleSupplierType.Name;
                    SupplierTypeColor = warehousePurchaseOrder.ArticleSupplier.ArticleSupplierType.HtmlColor;
                }

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
                    PdfFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
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
        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
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
                //string CodeProp = BindableBase.GetPropertyName(() => Code);
                //string QuantityProp = BindableBase.GetPropertyName(() => Quantity);
                //string SelectedProducerProp = BindableBase.GetPropertyName(() => Producer);
                string informationError = BindableBase.GetPropertyName(() => InformationError);

                if (columnName == DeliveryDateProp)
                    return WarehouseValidation.GetErrorMessage(DeliveryDateProp, DeliveryDate);
                else if (columnName == SupplierReferenceProp)
                    return WarehouseValidation.GetErrorMessage(SupplierReferenceProp, SupplierReference);
                else if (columnName == informationError)
                {
                    return WarehouseValidation.GetErrorMessage(informationError, InformationError);
                }
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
