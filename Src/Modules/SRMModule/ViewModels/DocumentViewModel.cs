using System;
using System.ComponentModel;
using System.Windows.Media;
using DevExpress.Xpf.Core;
using System.Windows;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Modules.SRM.Views;
using DevExpress.Mvvm.UI;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using System.IO;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using DevExpress.Spreadsheet;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    class DocumentViewModel
    {

        #region Service
        //IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration

        private bool isInIt = false;
        private MemoryStream pdfDoc;
        private string fileName;
        private bool isPresent;
        #endregion

        #region Properties    
        public MemoryStream PdfDoc
        {
            get { return pdfDoc; }
            set
            {
                pdfDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PdfDoc"));
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

        public bool IsPresent
        {
            get
            {
                return isPresent;
            }

            set
            {
                isPresent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPresent"));
            }
        }

        bool isGenerated;
        public bool IsGenerated
        {
            get
            {
                return isGenerated;
            }

            set
            {
                isGenerated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGenerated"));
            }
        }

        #endregion

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
        #endregion // end public events region

        public DocumentViewModel()
        {

        }

        //public void OpenPdf(byte[] v, object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenPdf()...", category: Category.Info, priority: Priority.Low);
        //        isInIt = true;
        //        if (!DXSplashScreen.IsActive )
        //        {
        //            DXSplashScreen.Show(x =>
        //            {
        //                Window win = new Window()
        //                {
        //                    ShowActivated = false,
        //                    WindowStyle = WindowStyle.None,
        //                    ResizeMode = ResizeMode.NoResize,
        //                    AllowsTransparency = true,
        //                    Background = new SolidColorBrush(Colors.Transparent),
        //                    ShowInTaskbar = false,
        //                    Topmost = true,
        //                    SizeToContent = SizeToContent.WidthAndHeight,
        //                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //                };
        //                WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //                win.Topmost = false;
        //                return win;
        //            }, x =>
        //            {
        //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //            }, null, null);
        //        }

        //        byte[] temp = null;

        //        if (selectedPurchaseOrder is WarehousePurchaseOrder)
        //        {
        //            WarehousePurchaseOrder warehousePurchaseOrder = (WarehousePurchaseOrder)obj;
        //            FileName = warehousePurchaseOrder.Code;
        //            //temp = warehousePurchaseOrder.ProductTypeAttachedDocInBytes;
        //            temp = SRMService.GetPurchaseOrderPdf(selectedPurchaseOrder.AttachPdf.ToString());
        //        }
        //        if (temp != null)
        //        {
        //            PdfDoc = new MemoryStream(temp);
        //            IsPresent = true;
        //        }
        //        else
        //        {
        //            CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //            IsPresent = false;
        //        }

        //        isInIt = false;
        //        if (DXSplashScreen.IsActive ) { DXSplashScreen.Close(); }

        //        GeosApplication.Instance.Logger.Log("Method OpenPdf()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in OpenPdf() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        #region Methods

        public void OpenPdf(WarehousePurchaseOrder selectedPurchaseOrder, object obj)
        {
            try
            {
                SRMService = new SRMServiceController((selectedPurchaseOrder.Warehouse != null && selectedPurchaseOrder.Warehouse.Company.ServiceProviderUrl != null) ? selectedPurchaseOrder.Warehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosApplication.Instance.Logger.Log("Method OpenPdf()...", category: Category.Info, priority: Priority.Low);
                isInIt = true;
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

                byte[] temp = null;

                if (selectedPurchaseOrder is WarehousePurchaseOrder)
                {
                    WarehousePurchaseOrder warehousePurchaseOrder = (WarehousePurchaseOrder)obj;
                    FileName = warehousePurchaseOrder.Code;
                    temp = SRMService.GetPurchaseOrderPdf(warehousePurchaseOrder.AttachPdf.ToString());
                }
                if (temp != null)
                {
                    PdfDoc = new MemoryStream(temp);
                    IsPresent = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                #region [GEOS2-4453][21.07.2023][rdixit]
                else
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format("Could not find file '{0}'. \nDo you want to generate it?", selectedPurchaseOrder.AttachPdf), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OKCancel, MessageBoxResult.Cancel);
                    if (MessageBoxResult == MessageBoxResult.OK)
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
                        GeneratePurchaseOrderReport(selectedPurchaseOrder);
                        if (IsGenerated)
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show(string.Format("File Generated Successfully."), "green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                    isInIt = false;
                    GeosApplication.Instance.Logger.Log("Method OpenPdf()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                #endregion
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPdf() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", selectedPurchaseOrder.AttachPdf), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OpenPdfByCode(WarehousePurchaseOrder warehousePurchaseOrder, WarehouseDeliveryNote warehouseDeliveryNote)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPdfByCode()...", category: Category.Info, priority: Priority.Low);
                isInIt = true;
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
                EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(SRMCommon.Instance.Selectedwarehouse.IdSite));
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == emdepSite.ShortName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                IWarehouseService WarehouseServiceS = new WarehouseServiceController(serviceurl);
                byte[] temp = WarehouseServiceS.GetDeliveryNotePdf(warehouseDeliveryNote);
                PdfDoc = new MemoryStream(temp);

                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdfByCode()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByCode()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        }


        public void GeneratePurchaseOrderReport(WarehousePurchaseOrder selectedPurchaseOrder)
        {
            SRMService = new SRMServiceController((selectedPurchaseOrder.Warehouse != null && selectedPurchaseOrder.Warehouse.Company.ServiceProviderUrl != null) ? selectedPurchaseOrder.Warehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
            //WarehousePurchaseOrder po = SRMService.GetOfferExportChart(Convert.ToInt32(selectedPurchaseOrder.IdWarehousePurchaseOrder), Convert.ToInt32(selectedPurchaseOrder.IdWarehouse));
            WarehousePurchaseOrder po = SRMService.GetOfferExportChart_V2520(Convert.ToInt32(selectedPurchaseOrder.IdWarehousePurchaseOrder), Convert.ToInt32(selectedPurchaseOrder.IdWarehouse));  //Updated by chitra.girigosavi ON [2/04/2024] GEOS2-5406 Changes in Supplier PO Management(2/3)
            if (po == null || string.IsNullOrEmpty(po?.Code))
            {
                GeosApplication.Instance.Logger.Log("Get an error in GeneratePurchaseOrderReport() Method - ServiceUnexceptedException Po data not found. ", category: Category.Info, priority: Priority.Low);
                throw new FileNotFoundException("File does not exist or File is open.");
            }
            else if (po.Templatefilebytes == null)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GeneratePurchaseOrderReport() Method - ServiceUnexceptedException Template file not found ", category: Category.Info, priority: Priority.Low);
                throw new FileNotFoundException("Template File does not exist or File is open.");
            }
            else
            {
                CultureInfo myCIintl = new CultureInfo("en-GB", true);
                byte[] bytesexcel = null; byte[] bytespdf = null;
                FileStream stream = null;
                #region File code
                Workbook workbook = new Workbook();
                // Load the workbook from the byte array
                using (MemoryStream stream1 = new MemoryStream(po.Templatefilebytes))
                {
                    workbook.LoadDocument(stream1, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                }
                Worksheet wsDl = workbook.Worksheets[2];
                string filenameExcel = null;
                string filenamePDF = null;
                string PODeliveryDate = null;
                DateTime PODate = Convert.ToDateTime(DateTime.Now);
                if (po.DeliveryDate != null)
                {
                    //PODate = DateTime.ParseExact(po.DeliveryDate?.Date.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    PODate = po.DeliveryDate.Value.Date;//chitra.girigosavi[GEOS2-7228][18/03/2025]
                }
                PODeliveryDate = PODate.ToString("d", CultureInfo.CurrentCulture);//chitra.girigosavi[GEOS2-7228][18/03/2025]
                //PODeliveryDate = PODate.ToShortDateString();//string.Concat(PODate.Year, PODate.Month.ToString().PadLeft(2, '0'), PODate.Day.ToString().PadLeft(2, '0'));
                //string updatedExcelFilePath = excelTemplateInByte.Keys.FirstOrDefault().Replace("{0}", po.WarehouseName);
                //updatedExcelFilePath = updatedExcelFilePath.Replace("{YEAR}", po.DeliveryDate?.Year.ToString());
                //updatedExcelFilePath = updatedExcelFilePath.Replace("{PO_Number}", po.Code);
                //updatedExcelFilePath = updatedExcelFilePath + @"\";
                string updatedExcelFilePath = @"C:\Temp\";
                if (!System.IO.Directory.Exists(updatedExcelFilePath))
                {
                    System.IO.Directory.CreateDirectory(updatedExcelFilePath);
                }
                filenameExcel = "PO_" + po.Code + ".xlsx";
                filenamePDF = "PO_" + po.Code + ".pdf";
                #endregion

                #region getIndexValue
                Int32 Index_PoCode = 0;
                Int32 Index_PoDate = 0;
                Int32 Index_PoDeliveryDate = 0;
                Int32 Index_PoIncoterms = 0;
                Int32 Index_PaymentTerms = 0;
                Int32 Index_PlantRegisteredName = 0;
                Int32 Index_PlantStreet = 0;
                Int32 Index_PlantZipCode = 0;
                Int32 Index_PlantCity = 0;
                Int32 Index_PlantState = 0;
                Int32 Index_PlantCountry = 0;
                Int32 Index_PlantPhone = 0;
                Int32 Index_PlantRegistrationNumber = 0;
                Int32 Index_SupplierName = 0;
                Int32 Index_SupplierStreet = 0;
                Int32 Index_SupplierZipCode = 0;
                Int32 Index_SupplierCity = 0;
                Int32 Index_SupplierState = 0;
                Int32 Index_SupplierCountry = 0;
                Int32 Index_SupplierPhone = 0;
                Int32 Index_SupplierContactName = 0;
                Int32 Index_SupplierContactEmail = 0;
                Int32 Index_SupplierNotes = 0;
                Int32 Index_SupplierContactPhone = 0;
                Int32 Index_DeliveryAddressWarehouseName = 0;
                Int32 Index_DeliveryAddressStreet = 0;
                Int32 Index_DeliveryAddressZipCode = 0;
                Int32 Index_DeliveryAddressCity = 0;
                Int32 Index_DeliveryAddressState = 0;
                Int32 Index_DeliveryAddressCountry = 0;
                Int32 Index_DeliveryAddressPhone = 0;
                Int32 Index_Remarks = 0;
                Int32 Index_Observations = 0;
                Int32 Index_PurchasingContactName = 0;
                Int32 Index_PurchasingContactEmail = 0;
                Int32 Language = 0;
                Int32 PoCurrency = 0;
                Int32 PO_count = 0;
                Int32 Index_ItemsStartRow = 0;
                string Index_ItemArticleColumn = string.Empty;
                string Index_ItemDescriptionColumn = string.Empty;
                string Index_ItemUnitPriceColumn = string.Empty;
                string Index_ItemQuantityColumn = string.Empty;
                string Index_ItemTotalPriceColumn = string.Empty;
                string POItem_lblColArticle = string.Empty;
                string POItem_lblColDescription = string.Empty;
                string POItem_lblColUnitPrice = string.Empty;
                string POItem_lblColQuantity = string.Empty;
                string POItem_lblColTotalPrice = string.Empty;
                string POItem_lblDiscount = string.Empty;
                #endregion

                #region getItemsValue  
                Int32 PO_counts = 0;
                Int32 Items_ItemsStartRow = 1;
                string Items_ItemArticleColumn = "0";
                string Items_ItemDescriptionColumn = "0";
                string Items_ItemUnitPriceColumn = "0";
                string Items_ItemQuantityColumn = "0";
                string Items_ItemTotalPriceColumn = "0";

                string POItems_lblColArticle = string.Empty;
                string POItems_lblColDescription = string.Empty;
                string POItems_lblColUnitPrice = string.Empty;
                string POItems_lblColQuantity = string.Empty;
                string POItems_lblColTotalPrice = string.Empty;
                string POItems_lblDiscount = string.Empty;

                #endregion

                DevExpress.Spreadsheet.Worksheet wsTranslations = workbook.Worksheets[4];
                #region getIndex
                for (int s = 0; s < 400; s++)
                {
                    #region getValue Using if  

                    if ((wsTranslations.Cells[s, 0].Value.ToString() == "Items_lblColArticle_en"))
                    {
                        POItems_lblColArticle = Convert.ToString(wsTranslations.Cells[s, 1].Value.ToString());
                    }
                    else if ((wsTranslations.Cells[s, 0].Value.ToString() == "Items_lblColDescription_en"))
                    {
                        POItems_lblColDescription = Convert.ToString(wsTranslations.Cells[s, 1].Value.ToString());
                    }
                    else if ((wsTranslations.Cells[s, 0].Value.ToString() == "Items_lblColUnitPrice_en"))
                    {
                        POItems_lblColUnitPrice = Convert.ToString(wsTranslations.Cells[s, 1].Value.ToString());
                    }
                    else if ((wsTranslations.Cells[s, 0].Value.ToString() == "Items_lblColQuantity_en"))
                    {
                        POItems_lblColQuantity = Convert.ToString(wsTranslations.Cells[s, 1].Value.ToString());
                    }
                    else if ((wsTranslations.Cells[s, 0].Value.ToString() == "Items_lblColTotalPrice_en"))
                    {
                        POItems_lblColTotalPrice = Convert.ToString(wsTranslations.Cells[s, 1].Value.ToString());
                    }
                    else if ((wsTranslations.Cells[s, 0].Value.ToString() == "Index_lblDiscount_en"))
                    {
                        POItems_lblDiscount = Convert.ToString(wsTranslations.Cells[s, 1].Value.ToString());
                    }
                    #endregion
                    string case1 = wsDl.Cells[s, 0].Value.ToString();
                    #region getIndex Using switch                           
                    switch (case1)
                    {
                        case "Index_PoCode":
                            Index_PoCode = s;
                            break;
                        case "Index_PoDate":
                            Index_PoDate = s;
                            break;
                        case "Index_PoDeliveryDate":
                            Index_PoDeliveryDate = s;
                            break;
                        case "Index_PoIncoterms":
                            Index_PoIncoterms = s;
                            break;
                        case "Index_PaymentTerms":
                            Index_PaymentTerms = s;
                            break;
                        case "Index_PlantRegisteredName":
                            Index_PlantRegisteredName = s;
                            break;
                        case "Index_PlantStreet":
                            Index_PlantStreet = s;
                            break;
                        case "Index_PlantZipCode":
                            Index_PlantZipCode = s;
                            break;
                        case "Index_PlantCity":
                            Index_PlantCity = s;
                            break;
                        case "Index_PlantState":
                            Index_PlantState = s;
                            break;
                        case "Index_PlantCountry":
                            Index_PlantCountry = s;
                            break;
                        case "Index_PlantPhone":
                            Index_PlantPhone = s;
                            break;
                        case "Index_PlantRegistrationNumber":
                            Index_PlantRegistrationNumber = s;
                            break;
                        case "Index_SupplierName":
                            Index_SupplierName = s;
                            break;
                        case "Index_SupplierStreet":
                            Index_SupplierStreet = s;
                            break;
                        case "Index_SupplierZipCode":
                            Index_SupplierZipCode = s;
                            break;
                        case "Index_SupplierCity":
                            Index_SupplierCity = s;
                            break;
                        case "Index_SupplierState":
                            Index_SupplierState = s;
                            break;
                        case "Index_SupplierCountry":
                            Index_SupplierCountry = s;
                            break;
                        case "Index_SupplierPhone":
                            Index_SupplierPhone = s;
                            break;
                        case "Index_SupplierContactName":
                            Index_SupplierContactName = s;
                            break;
                        case "Index_SupplierContactEmail":
                            Index_SupplierContactEmail = s;
                            break;
                        case "Index_SupplierNotes":
                            Index_SupplierNotes = s;
                            break;
                        case "Index_DeliveryAddressWarehouseName":
                            Index_DeliveryAddressWarehouseName = s;
                            break;
                        case "Index_DeliveryAddressStreet":
                            Index_DeliveryAddressStreet = s;
                            break;
                        case "Index_DeliveryAddressZipCode":
                            Index_DeliveryAddressZipCode = s;
                            break;
                        case "Index_DeliveryAddressCity":
                            Index_DeliveryAddressCity = s;
                            break;
                        case "Index_DeliveryAddressState":
                            Index_DeliveryAddressState = s;
                            break;
                        case "Index_DeliveryAddressCountry":
                            Index_DeliveryAddressCountry = s;
                            break;
                        case "Index_DeliveryAddressPhone":
                            Index_DeliveryAddressPhone = s;
                            break;
                        case "Index_PurchasingContactName":
                            Index_PurchasingContactName = s;
                            break;
                        case "Index_PurchasingContactEmail":
                            Index_PurchasingContactEmail = s;
                            break;
                        case "Index_SupplierContactPhone":
                            Index_SupplierContactPhone = s;
                            break;
                        case "PoCurrency":
                            PoCurrency = s;
                            break;


                        #region Expenses_Items     
                        case "Index_ItemsStartRow":
                            PO_count = Convert.ToInt32(wsDl.Cells[s, 1].Value.ToString());
                            Index_ItemsStartRow = Convert.ToInt32(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Index_ItemArticleColumn":
                            Index_ItemArticleColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Index_ItemDescriptionColumn":
                            Index_ItemDescriptionColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Index_ItemUnitPriceColumn":
                            Index_ItemUnitPriceColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Index_ItemQuantityColumn":
                            Index_ItemQuantityColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Index_ItemTotalPriceColumn":
                            Index_ItemTotalPriceColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        #endregion

                        case "Language":
                            Language = s;
                            break;
                        case "Index_Remarks":
                            Index_Remarks = s;
                            break;
                        case "Index_Observations":
                            Index_Observations = s;
                            break;
                        default:
                            break;

                        #region Expenses_Items_Items    
                        case "Items_ItemsStartRow":                                                       //[001][kshinde][APIGEOS-586][16/08/2022] 
                            PO_counts = Convert.ToInt32(wsDl.Cells[s, 1].Value.ToString());
                            Items_ItemsStartRow = Convert.ToInt32(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Items_ItemArticleColumn":
                            Items_ItemArticleColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Items_ItemDescriptionColumn":
                            Items_ItemDescriptionColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Items_ItemUnitPriceColumn":
                            Items_ItemUnitPriceColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Items_ItemQuantityColumn":
                            Items_ItemQuantityColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                        case "Items_ItemTotalPriceColumn":
                            Items_ItemTotalPriceColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                            break;
                            #endregion
                    }
                    #endregion

                }
                #endregion

                using (stream = new FileStream(Path.Combine(updatedExcelFilePath, filenameExcel), FileMode.Create, FileAccess.ReadWrite))
                {
                    #region Set CultureInfo DeliveryDate
                    if (po.DeliveryDate != null)
                    {
                        //PODate = DateTime.ParseExact(po.DeliveryDate?.Date.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        //wsDl.Cells[Index_PoDeliveryDate, 1].Value = PODate.ToString(myCIintl.DateTimeFormat.ShortDatePattern, myCIintl);
                        PODate = po.DeliveryDate.Value.Date;//chitra.girigosavi[GEOS2-7228][18/03/2025]
                        wsDl.Cells[Index_PoDeliveryDate, 1].Value = PODate.ToString("d", myCIintl);//chitra.girigosavi[GEOS2-7228][18/03/2025]
                    }
                    else
                    {
                        wsDl.Cells[Index_PoDeliveryDate, 1].Value = PODeliveryDate;
                    }
                    #endregion

                    #region Set CultureInfo PoDate
                    //if (!String.IsNullOrEmpty(PODeliveryDate))
                    //{
                    //    DateTime dt = DateTime.ParseExact(PODeliveryDate, "dd-MM-yyyy", null);
                    //    wsDl.Cells[Index_PoDate, 1].Value = dt.ToString(myCIintl.DateTimeFormat.ShortDatePattern, myCIintl);
                    //}
                    //else
                    //{
                    //    wsDl.Cells[Index_PoDate, 1].Value = PODeliveryDate;
                    //}

                    //chitra.girigosavi[GEOS2-7228][18/03/2025]
                    DateTime dt;
                    if (!string.IsNullOrEmpty(PODeliveryDate))
                    {
                        if (DateTime.TryParse(PODeliveryDate, myCIintl, DateTimeStyles.None, out dt))
                        {
                            wsDl.Cells[Index_PoDate, 1].Value = dt.ToString("d", myCIintl); // Culture-specific short date format
                        }
                    }
                    else
                    {
                        wsDl.Cells[Index_PoDate, 1].Value = PODeliveryDate;
                    }

                    #endregion

                    #region cell entry

                    wsDl.Cells[Index_PoCode, 1].Value = po.Code;
                    wsDl.Cells[Index_PoIncoterms, 1].Value = po.Incoterms;
                    wsDl.Cells[Index_PaymentTerms, 1].Value = po.PaymentTerms;
                    wsDl.Cells[Index_PlantRegisteredName, 1].Value = po.Warehouse.Company.RegisteredName;
                    wsDl.Cells[Index_PlantStreet, 1].Value = po.Warehouse.Company.Address;
                    wsDl.Cells[Index_PlantZipCode, 1].Value = po.Warehouse.Company.ZipCode;
                    wsDl.Cells[Index_PlantCity, 1].Value = po.Warehouse.Company.City;
                    wsDl.Cells[Index_PlantState, 1].Value = po.Warehouse.Company.Country.State;
                    wsDl.Cells[Index_PlantCountry, 1].Value = po.Warehouse.Company.Country.Name;
                    wsDl.Cells[Index_PlantPhone, 1].Value = po.Warehouse.Company.Telephone;
                    wsDl.Cells[Index_PlantRegistrationNumber, 1].Value = po.Warehouse.Company.RegisteredNumber;
                    wsDl.Cells[Index_SupplierName, 1].Value = po.ArticleSupplier.Name;
                    wsDl.Cells[Index_SupplierStreet, 1].Value = po.ArticleSupplier.Address;
                    wsDl.Cells[Index_SupplierZipCode, 1].Value = po.ArticleSupplier.PostCode;
                    wsDl.Cells[Index_SupplierCity, 1].Value = po.ArticleSupplier.City;
                    wsDl.Cells[Index_SupplierState, 1].Value = po.ArticleSupplier.Country.State;
                    wsDl.Cells[Index_SupplierCountry, 1].Value = po.ArticleSupplier.Country.Name;
                    wsDl.Cells[Index_SupplierPhone, 1].Value = po.ArticleSupplier.Phone1;
                    wsDl.Cells[Index_SupplierContactName, 1].Value = po.ArticleSupplier.Name;
                    wsDl.Cells[Index_SupplierContactEmail, 1].Value = po.ArticleSupplier.ContactEmail;
                    wsDl.Cells[Index_SupplierNotes, 1].Value = po.SupplierComments;
                    wsDl.Cells[Index_SupplierContactPhone, 1].Value = po.ArticleSupplier.Phone2;
                    wsDl.Cells[PoCurrency, 1].Value = po.Currency.Name;
                    wsDl.Cells[Index_DeliveryAddressWarehouseName, 1].Value = po.WarehouseName;
                    wsDl.Cells[Index_DeliveryAddressStreet, 1].Value = po.DEAddress;
                    wsDl.Cells[Index_DeliveryAddressZipCode, 1].Value = po.DEPostCode;
                    wsDl.Cells[Index_DeliveryAddressCity, 1].Value = po.DECity;
                    wsDl.Cells[Index_DeliveryAddressState, 1].Value = po.DeliveryAddressState;
                    wsDl.Cells[Index_DeliveryAddressCountry, 1].Value = po.DeliveryAddressCountry;
                    wsDl.Cells[Index_DeliveryAddressPhone, 1].Value = po.DETelephone;
                    wsDl.Cells[Index_Remarks, 1].Value = po.Remarks;
                    wsDl.Cells[Index_Observations, 1].Value = po.Comments;
                    wsDl.Cells[Index_PurchasingContactName, 1].Value = po.PurchasingContactName;
                    wsDl.Cells[Index_PurchasingContactEmail, 1].Value = po.PurchasingContactEmail;
                    wsDl.Cells[Language, 1].Value = "en";
                    #endregion

                    #region ExportExpensesListItems       
                    if (po.WarehousePurchaseOrderItems != null)
                    {
                        Int32 Expenses_ItemsHeadingStartRow = Convert.ToInt32(PO_counts) - 1;
                        DevExpress.Spreadsheet.Worksheet wsDlExpensesItems = workbook.Worksheets[1];
                        wsDlExpensesItems.Cells[Items_ItemArticleColumn + (Items_ItemsStartRow - 1)].Value = POItems_lblColArticle;
                        wsDlExpensesItems.Cells[Items_ItemDescriptionColumn + (Items_ItemsStartRow - 1)].Value = POItems_lblColDescription;
                        wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + (Items_ItemsStartRow - 1)].Value = POItems_lblColUnitPrice;
                        wsDlExpensesItems.Cells[Items_ItemQuantityColumn + (Items_ItemsStartRow - 1)].Value = POItems_lblColQuantity;
                        wsDlExpensesItems.Cells[Items_ItemTotalPriceColumn + (Items_ItemsStartRow - 1)].Value = POItems_lblColTotalPrice;

                        if (po.WarehousePurchaseOrderItems.Count > 1)
                            wsDlExpensesItems.Rows.Insert(Expenses_ItemsHeadingStartRow + 1, po.WarehousePurchaseOrderItems.Count() - 1, RowFormatMode.FormatAsPrevious);
                        if (po.WarehousePurchaseOrderItems != null)
                            foreach (WarehousePurchaseOrderItem item in po.WarehousePurchaseOrderItems)
                            {
                                if (item.Description.Equals("Discount"))
                                {
                                    if (item.Article != null)
                                        wsDlExpensesItems.Cells[Items_ItemArticleColumn + Items_ItemsStartRow].Value = item.Article.Reference;
                                    else
                                        wsDlExpensesItems.Cells[Items_ItemArticleColumn + Items_ItemsStartRow].Value = "";
                                    wsDlExpensesItems.Cells[Items_ItemDescriptionColumn + Items_ItemsStartRow].Value = (POItems_lblDiscount + " (" + item.Discount + "%)");

                                    //wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + Items_ItemsStartRow].Value = item.UnitPrice;
                                    //wsDlExpensesItems.Cells[Items_ItemQuantityColumn + Items_ItemsStartRow].Value = item.Quantity;
                                    #region //Added by chitra.girigosavi ON [2/04/2024] GEOS2-5406 Changes in Supplier PO Management(2/3)

                                    wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + Items_ItemsStartRow].Value = item.Quantity * item.UnitPrice;

                                    if (item.Quantity == -1)

                                    {

                                        item.Quantity = 1;

                                        wsDlExpensesItems.Cells[Items_ItemQuantityColumn + Items_ItemsStartRow].Value = item.Quantity;
                                        wsDlExpensesItems.Cells[Items_ItemQuantityColumn + Items_ItemsStartRow].Value = null;
                                    }

                                    //wsDlExpensesItems.Cells[Items_ItemTotalPriceColumn + (Items_ItemsStartRow)].Value = null;

                                    //wsDlExpensesItems.Cells[Items_ItemTotalPriceColumn + (Items_ItemsStartRow - 1)].Value = (item.TotalPrice - item.UnitPrice);

                                    #endregion
                                    wsDlExpensesItems.Rows[(Items_ItemsStartRow - 1)].Font.Bold = true;
                                    if (Items_ItemsStartRow / 3 == 0)
                                    {
                                        wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + Items_ItemsStartRow].Font.Color = System.Drawing.Color.White;
                                        wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + Items_ItemsStartRow].FillColor = System.Drawing.Color.White;
                                        wsDlExpensesItems.Cells[Items_ItemQuantityColumn + Items_ItemsStartRow].Font.Color = System.Drawing.Color.White;
                                        wsDlExpensesItems.Cells[Items_ItemQuantityColumn + Items_ItemsStartRow].FillColor = System.Drawing.Color.White;
                                        #region //Added by chitra.girigosavi ON [2/04/2024] GEOS2-5406 Changes in Supplier PO Management(2/3)

                                        wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + (Items_ItemsStartRow - 0)].Font.Color = System.Drawing.Color.White;

                                        wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + (Items_ItemsStartRow - 0)].FillColor = System.Drawing.Color.White;

                                        wsDlExpensesItems.Cells[Items_ItemTotalPriceColumn + (Items_ItemsStartRow - 1)].Font.Color = System.Drawing.Color.White;

                                        #endregion
                                    }
                                    else
                                    {
                                        wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + Items_ItemsStartRow].Font.Color = System.Drawing.Color.FromArgb(242, 242, 242);
                                        wsDlExpensesItems.Cells[Items_ItemQuantityColumn + Items_ItemsStartRow].Font.Color = System.Drawing.Color.FromArgb(242, 242, 242);
                                        #region //Added by chitra.girigosavi ON [2/04/2024] GEOS2-5406 Changes in Supplier PO Management(2/3)

                                        wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + (Items_ItemsStartRow - 0)].Font.Color = System.Drawing.Color.FromArgb(0, 0, 0);

                                        wsDlExpensesItems.Cells[Items_ItemTotalPriceColumn + (Items_ItemsStartRow - 1)].Font.Color = System.Drawing.Color.FromArgb(0, 0, 0);

                                        wsDlExpensesItems.Cells[Items_ItemTotalPriceColumn + (Items_ItemsStartRow - 0)].Font.Color = System.Drawing.Color.FromArgb(242, 242, 242);

                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (item.IdArticle == 3252)
                                    {
                                        RichTextString richText = new RichTextString();
                                        SpreadsheetFont cellFont = wsDlExpensesItems.Cells[Items_ItemDescriptionColumn + Items_ItemsStartRow].Font;
                                        RichTextRunFont RichTextForSummaryColumn = new RichTextRunFont(cellFont.Name, (cellFont.Size), cellFont.Color);
                                        RichTextRunFont RichText = new RichTextRunFont(cellFont.Name, (cellFont.Size - 3), cellFont.Color);
                                        RichText.Italic = true;
                                        try
                                        {
                                            richText.AddTextRun(item.Description, RichText);
                                            wsDlExpensesItems.Cells[Items_ItemArticleColumn + Items_ItemsStartRow].Value = "";
                                            wsDlExpensesItems.Cells[Items_ItemDescriptionColumn + Items_ItemsStartRow].SetRichText(richText);
                                            wsDlExpensesItems.Cells[Items_ItemTotalPriceColumn + Items_ItemsStartRow].Value = null;
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    else
                                    {
                                        wsDlExpensesItems.Cells[Items_ItemArticleColumn + Items_ItemsStartRow].Value = item.Article.Reference;
                                        wsDlExpensesItems.Cells[Items_ItemDescriptionColumn + Items_ItemsStartRow].Value = item.Description;

                                        if (item.IdArticle == 13339)
                                        {
                                            wsDlExpensesItems.Cells[Items_ItemArticleColumn + Items_ItemsStartRow].Value = null;
                                            wsDlExpensesItems.Cells[Items_ItemDescriptionColumn + Items_ItemsStartRow].Font.Bold = true;
                                            wsDlExpensesItems.Cells[Items_ItemTotalPriceColumn + Items_ItemsStartRow].Font.Bold = true;
                                            wsDlExpensesItems.Cells[Items_ItemQuantityColumn + Items_ItemsStartRow].Font.Color = System.Drawing.Color.FromArgb(248, 249, 250);
                                            wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + Items_ItemsStartRow].Font.Color = System.Drawing.Color.FromArgb(248, 249, 250);
                                        }
                                        wsDlExpensesItems.Cells[Items_ItemQuantityColumn + Items_ItemsStartRow].Value = item.Quantity;
                                        wsDlExpensesItems.Cells[Items_ItemUnitPriceColumn + Items_ItemsStartRow].Value = item.UnitPrice;
                                    }
                                }
                                Items_ItemsStartRow = Items_ItemsStartRow + 1;
                            }
                    }
                    else
                    {
                        workbook.Worksheets[0].Visible = false;
                    }
                    #endregion

                    workbook.Worksheets[2].Visible = false;
                    workbook.Worksheets[4].Visible = false;
                    workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                }
                using (FileStream pdfFileStream = new FileStream(Path.Combine(updatedExcelFilePath, filenamePDF), FileMode.Create))
                {
                    workbook.ExportToPdf(pdfFileStream);
                }
                workbook.Dispose();
                if (stream != null)
                {
                    stream.Dispose();
                }
                #region read
                if (File.Exists(Path.Combine(updatedExcelFilePath, filenameExcel)))
                {
                    using (System.IO.FileStream stream1 = new System.IO.FileStream(Path.Combine(updatedExcelFilePath, filenameExcel), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytesexcel = new byte[stream1.Length];
                        int numBytesToRead = (int)stream1.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream1.Read(bytesexcel, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                if (File.Exists(Path.Combine(updatedExcelFilePath, filenamePDF)))
                {
                    using (System.IO.FileStream stream1 = new System.IO.FileStream(Path.Combine(updatedExcelFilePath, filenamePDF), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytespdf = new byte[stream1.Length];
                        int numBytesToRead = (int)stream1.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream1.Read(bytespdf, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                #endregion

                IsGenerated = SRMService.ExportPurchaseOrderReport(po, bytespdf, bytesexcel);
                if (IsGenerated == true)

                {

                    bool result = SRMService.UpdatePoflag(selectedPurchaseOrder);

                }
                #region Delete Temp created file
                if (File.Exists(Path.Combine(updatedExcelFilePath, filenamePDF)))
                {
                    // Delete PDF the file
                    File.Delete(Path.Combine(updatedExcelFilePath, filenamePDF));
                }
                if (File.Exists(Path.Combine(updatedExcelFilePath, filenameExcel)))
                {
                    // Delete EXCEL the file
                    File.Delete(Path.Combine(updatedExcelFilePath, filenameExcel));
                }
                #endregion
            }
            SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        }
        #endregion
    }
}
