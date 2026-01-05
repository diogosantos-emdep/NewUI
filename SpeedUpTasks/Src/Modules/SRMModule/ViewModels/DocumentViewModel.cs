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

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    class DocumentViewModel
    {

        #region Service
        //IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());


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
                }
                else
                {
                    CustomMessageBox.Show(string.Format("Could not find file '{0}'.", selectedPurchaseOrder.AttachPdf), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    IsPresent = false;
                }

                isInIt = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdf()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPdf() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", selectedPurchaseOrder.AttachPdf), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByCode()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

        

        #endregion
    }
}
