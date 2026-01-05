using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.PdfViewer;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ServiceModel;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class DeliveryNotePDFViewModel
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // End Services

        #region Declaration
        private bool isInIt = false;
        private Uri pdfPath;
        private MemoryStream pdfDoc;

        #endregion

        #region Properties
        public Uri PDFPath
        {
            get { return pdfPath; }
            set
            {
                pdfPath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PDFPath"));
            }
        }

        public MemoryStream PdfDoc
        {
            get { return pdfDoc; }
            set
            {
                pdfDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PdfDoc"));
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

        #region Public ICommands

        public ICommand PdfCancelButtonCommand { get; set; }

        #endregion

        #region Constructor
        public DeliveryNotePDFViewModel()
        {
            PdfCancelButtonCommand = new RelayCommand(new Action<object>(PdfCancelButton));
        }

        #endregion

        #region Methods

        private void PdfCancelButton(object obj)
        {
            RequestClose(null, null);
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
                EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdSite));
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
