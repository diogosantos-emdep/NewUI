using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class DeliveryNotePrintViewModel : INotifyPropertyChanged, IDisposable
    {
        #region TaskLog

        /// <summary>
        /// [WMS-M059-02] Print of reception label in article locations section [adaibathina]
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
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services

        #region Declaration
        private int quantity = 1;
        private int copies = 1;
        private WarehouseDeliveryNote warehouseDeliveryNote;
        private string selectedLabelSize;
        #endregion

        #region Properties
        PrintLabel PrintLabel { get; set; }
        Dictionary<string, string> PrintValues { get; set; }
        Article ArticleData { get; set; }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(new PropertyChangedEventArgs("Quantity")); }
        }
        public int Copies
        {
            get { return copies; }
            set { copies = value; OnPropertyChanged(new PropertyChangedEventArgs("Copies")); }
        }
        public WarehouseDeliveryNote WarehouseDeliveryNote
        {
            get { return warehouseDeliveryNote; }
            set { warehouseDeliveryNote = value; OnPropertyChanged(new PropertyChangedEventArgs("WarehouseDeliveryNote")); }
        }

        public string SelectedLabelSize
        {
            get
            {
                return selectedLabelSize;
            }
            set
            {
                selectedLabelSize = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLabelSize"));
            }
        }

        #endregion

        #region Command       
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandPrintButton { get; set; }

        #endregion

        #region Constructor 

        public DeliveryNotePrintViewModel(WarehouseDeliveryNote warehouseDeliveryNote, Article ArticleData)
        {
            GeosApplication.Instance.Logger.Log("Constructor DeliveryNotePrintViewModel....", category: Category.Info, priority: Priority.Low);

            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandPrintButton = new DelegateCommand<object>(PrintAction);
            this.WarehouseDeliveryNote = warehouseDeliveryNote;
            this.ArticleData = ArticleData;
            GeosApplication.Instance.Logger.Log("Constructor DeliveryNotePrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        #endregion

        #region Methods


        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Init DeliveryNotePrintViewModel....", category: Category.Info, priority: Priority.Low);

                PrintValues = new Dictionary<string, string>();
                SelectedLabelSize = "Normal";

                GeosApplication.Instance.Logger.Log("Init DeliveryNotePrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Print Label
        /// [001][GEOS2-1711][avpawar][Add support to print small Delivery Note labels]
        /// </summary>
        /// <param name="obj"></param>
        private void PrintAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PrintAction DeliveryNotePrintViewModel....", category: Category.Info, priority: Priority.Low);

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

                PrintValues = new Dictionary<string, string>();

                //[001] Start
                if (SelectedLabelSize == "Normal")
                {
                    byte[] printFile = GeosRepositoryService.GetPrintDNItemLabelFile(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);

                    PrintLabel = new PrintLabel(PrintValues, printFile);

                    WarehouseDeliveryNote warehouseDeliveryNote = WarehouseService.GetLabelPrintDetails_V2034(ArticleData.IdArticle, WarehouseCommon.Instance.Selectedwarehouse, WarehouseDeliveryNote.WarehouseDeliveryNoteItems[0].IdWarehouseDeliveryNoteItem);

                    CreatePrintValues(warehouseDeliveryNote);

                    for (int Copy = 1; Copy <= Copies; Copy++)
                    {
                        PrintLabel.Print();
                    }
                }
                else
                {
                    byte[] printFile = GeosRepositoryService.GetPrintSmallDNItemLabelFile(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);

                    PrintLabel = new PrintLabel(PrintValues, printFile);

                    WarehouseDeliveryNote warehouseDeliveryNote = WarehouseService.GetLabelPrintDetails_V2034(ArticleData.IdArticle, WarehouseCommon.Instance.Selectedwarehouse, WarehouseDeliveryNote.WarehouseDeliveryNoteItems[0].IdWarehouseDeliveryNoteItem);

                    CreatePrintValues(warehouseDeliveryNote);

                    for (int Copy = 1; Copy <= Copies; Copy++)
                    {
                        PrintLabel.Print();
                    }
                }
                //[001] End

                //PrintLabel = new PrintLabel(PrintValues, printFile);

                //WarehouseDeliveryNote warehouseDeliveryNote = WarehouseService.GetLabelPrintDetails_V2034(ArticleData.IdArticle, WarehouseCommon.Instance.Selectedwarehouse, WarehouseDeliveryNote.WarehouseDeliveryNoteItems[0].IdWarehouseDeliveryNoteItem);

                //CreatePrintValues(warehouseDeliveryNote);

                //for (int Copy = 1; Copy <= Copies; Copy++)
                //{
                //    PrintLabel.Print();
                //}

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("PrintAction DeliveryNotePrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CreatePrintValues(WarehouseDeliveryNote warehouseDeliveryNote)
        {
            try
            {
                //warehouseDeliveryNote from serveice
                //WarehouseDeliveryNote from ui
                PrintValues.Add("@WAREHOUSE", WarehouseCommon.Instance.Selectedwarehouse.Name);

                string madeIn = string.Empty;
                string Ot = string.Format("{0} ({1})", WarehouseDeliveryNote.Code, warehouseDeliveryNote.DeliveryDate.ToShortDateString()); //  WarehouseDeliveryNote.Code + " (" + warehouseDeliveryNote.DeliveryDate.ToShortDateString() + ")";
                string reference = ArticleData.Reference;
                string producer = string.Empty;

                if (warehouseDeliveryNote.WarehouseDeliveryNoteItems != null && warehouseDeliveryNote.WarehouseDeliveryNoteItems.Count > 0)
                {
                    if (warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle != null &&
                        warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle.Count > 0)
                    {
                        madeIn = warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle[0].Country?.Name;

                        if (!string.IsNullOrEmpty(warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle[0].Manufacturer.Code))
                            producer = warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle[0].Manufacturer.Code;
                        else
                            producer = warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle[0].Manufacturer.Name;

                        if (producer.Length > 35)
                        {
                            int index = 0;
                            string firstLine = GetFirstLine(producer, ref index, 35);
                            PrintValues.Add("@PRODUCER00", firstLine);
                            PrintValues.Add("@PRODUCER01", producer.Substring(index));
                        }
                        else
                        {
                            PrintValues.Add("@PRODUCER00", producer);
                            PrintValues.Add("@PRODUCER01", string.Empty);
                        }

                        if (warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle[0].Country?.CountryGroup != null)
                        {
                            PrintValues.Add("@CO", warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle[0].Country.CountryGroup.Name);

                            if (warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle[0].Country?.CountryGroup?.Name ==
                                warehouseDeliveryNote.WarehousePurchaseOrder?.ArticleSupplier?.Country?.CountryGroup?.Name)
                            {
                                PrintValues.Add("@BCOZ", "W");
                            }
                            else
                            {
                                PrintValues.Add("@BCOZ", "B");
                            }

                            if (warehouseDeliveryNote.WarehouseDeliveryNoteItems[0].ManufacturersByArticle[0].Country.CountryGroup.Name ==
                                warehouseDeliveryNote.WarehousePurchaseOrder?.ArticleSupplier?.Country?.CountryGroup?.Name)
                            {
                                PrintValues.Add("@BCO", "B");
                            }
                            else
                            {
                                PrintValues.Add("@BCO", "W");
                            }
                        }
                    }
                    else
                    {
                        PrintValues.Add("@PRODUCER00", string.Empty);
                        PrintValues.Add("@PRODUCER01", string.Empty);
                        PrintValues.Add("@CO", string.Empty);
                        PrintValues.Add("@BCOZ", "W");
                        PrintValues.Add("@BCO", "W");
                    }

                    //Barcode
                    string dnItem = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse.ToString().PadLeft(3, '0')
                        + WarehouseDeliveryNote.WarehouseDeliveryNoteItems[0].IdWarehouseDeliveryNoteItem.ToString().PadLeft(8, '0') + Convert.ToInt64(quantity).ToString("D6"); // Quantity.ToString().PadLeft(6, '0');
                    dnItem = dnItem.Substring(0, dnItem.Length - 1) + ">6" + dnItem.Substring(dnItem.Length - 1, 1);
                    PrintValues.Add("@DNITEM", dnItem);
                }

                string supplier = string.Empty;
                // ArticleSupplier of Warehouse Purchase Order
                if (warehouseDeliveryNote.WarehousePurchaseOrder != null)
                {
                    if (!string.IsNullOrEmpty(warehouseDeliveryNote.WarehousePurchaseOrder.ArticleSupplier.Code))
                        supplier = warehouseDeliveryNote.WarehousePurchaseOrder.ArticleSupplier.Code;
                    else
                        supplier = warehouseDeliveryNote.WarehousePurchaseOrder.ArticleSupplier.Name;

                    if (supplier.Length > 35)
                    {
                        int index = 0;
                        string firstLine = GetFirstLine(supplier, ref index, 35);
                        PrintValues.Add("@CUSTOMER00", firstLine);
                        PrintValues.Add("@CUSTOMER01", supplier.Substring(index));
                    }
                    else
                    {
                        PrintValues.Add("@CUSTOMER00", supplier);
                        PrintValues.Add("@CUSTOMER01", "");
                    }
                }
                // Supplier of Supplier Complaint
                else if (warehouseDeliveryNote.SupplierComplaint != null && warehouseDeliveryNote.SupplierComplaint.Supplier != null)
                {
                    if (!string.IsNullOrEmpty(warehouseDeliveryNote.SupplierComplaint.Supplier.Code))
                        supplier = warehouseDeliveryNote.SupplierComplaint.Supplier.Code;
                    else
                        supplier = warehouseDeliveryNote.SupplierComplaint.Supplier.Name;

                    if (supplier.Length > 35)
                    {
                        int index = 0;
                        string firstLine = GetFirstLine(supplier, ref index, 35);
                        PrintValues.Add("@CUSTOMER00", firstLine);
                        PrintValues.Add("@CUSTOMER01", supplier.Substring(index));
                    }
                    else
                    {
                        PrintValues.Add("@CUSTOMER00", supplier);
                        PrintValues.Add("@CUSTOMER01", "");
                    }
                }

                if (madeIn.Length > 24)
                {
                    int index = 0;
                    string firstLine2 = GetFirstLine(madeIn, ref index, 24);
                    PrintValues.Add("@MADEIN00", firstLine2);
                    PrintValues.Add("@MADEIN01", madeIn.Substring(index));
                }
                else
                {
                    PrintValues.Add("@MADEIN00", madeIn);
                    PrintValues.Add("@MADEIN01", "");
                }

                PrintValues.Add("@QTTY", Quantity.ToString());
                PrintValues.Add("@REFERENCE", reference);
                PrintValues.Add("@USER", GeosApplication.Instance.ActiveUser.IdUser.ToString());

                //PrintValues.Add("@OT", Ot);
                //string referenceBarcode = PrintLabel.SplitStringForBarcode(reference);
                //PrintValues.Add("@REFBARCODE", referenceBarcode);
                //PrintValues.Add("@PN", string.Empty);
                //PrintValues.Add("@PBARCODE", string.Empty);

                PrintValues.Add("@DNCODE", WarehouseDeliveryNote.Code);
                PrintValues.Add("@DNDATE", warehouseDeliveryNote.DeliveryDate.ToShortDateString());
                PrintValues.Add("@WHLOC00", WarehouseDeliveryNote.MasterItem.WarehouseLocation.FullName);
                PrintValues.Add("@WHLOC01", string.Empty);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CreatePrintValues()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private string GetFirstLine(string s, ref int i, int longTotal)
        {
            string temp = "";

            for (int j = 0; j < s.Length; j++)
            {
                if (s[j] == ' ')
                {
                    if (j > longTotal)
                    {
                        break;
                    }
                    else
                    {
                        i = j + 1;
                        temp = s.Substring(0, j);
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// Cancel Action
        /// </summary>
        /// <param name="obj"></param>
        private void CommandCancelAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("CommandCancelAction DeliveryNotePrintViewModel....", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
            GeosApplication.Instance.Logger.Log("CommandCancelAction DeliveryNotePrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// obj Dispose
        /// </summary>
        public void Dispose()
        {
            GeosApplication.Instance.Logger.Log("Dispose DeliveryNotePrintViewModel....", category: Category.Info, priority: Priority.Low);
            GC.SuppressFinalize(this);
            GeosApplication.Instance.Logger.Log("Dispose DeliveryNotePrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

    }
}
