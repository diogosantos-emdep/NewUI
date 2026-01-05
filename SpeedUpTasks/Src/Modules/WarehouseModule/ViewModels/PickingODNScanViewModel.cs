using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
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
using System.Collections.ObjectModel;
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
    class PickingODNScanViewModel : INotifyPropertyChanged, IDisposable
    {
        #region TaskLog

        /// <summary>
        /// WMS	M057-14	Add new section pending ODN [adaibathina]
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

        #endregion //End Of Services

        #region Declaration
        private string odnCode;
        private string odnCodeError;
        private Visibility odnCodeErrorVisibility;
        private string odnCodeErrorColour;

        #endregion

        #region Properties
        private ObservableCollection<SupplierComplaint> SupplierComplaintsMainList { get; set; }
        private List<SupplierComplaintItem> SupplierComplaintsList { get; set; }
        public string OdnCode
        {
            get { return odnCode; }
            set
            {
                odnCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OdnCode"));
            }
        }
        public string OdnCodeError
        {
            get { return odnCodeError; }
            set
            {
                odnCodeError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OdnCodeError"));
            }
        }
        public Visibility OdnCodeErrorVisibility
        {
            get { return odnCodeErrorVisibility; }
            set
            {
                odnCodeErrorVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OdnCodeErrorVisibility"));
            }
        }
        public string OdnCodeErrorColour
        {
            get { return odnCodeErrorColour; }
            set
            {
                odnCodeErrorColour = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OdnCodeErrorColour"));
            }
        }




        #endregion

        #region Command       
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandNextButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandOnLoaded { get; set; }

        #endregion

        #region Constructor 
        /// <summary>
        /// Constructor
        /// </summary>
        public PickingODNScanViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor PickingODNScanViewModel....", category: Category.Info, priority: Priority.Low);

            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandScanBarcode = new RelayCommand(new Action<object>(ScanBarcodeAction));

            GeosApplication.Instance.Logger.Log("Constructor PickingODNScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        #endregion

        #region Methods
        /// <summary>
        /// initialization method
        /// </summary>
        /// <param name="SupplierComplaintsList">All SupplierComplaints</param>
        public void Init(ObservableCollection<SupplierComplaint> SupplierComplaintsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PickingODNScanViewModel Init()", category: Category.Info, priority: Priority.Low);

                this.SupplierComplaintsMainList = SupplierComplaintsList;
                OdnCodeErrorVisibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingODNScanViewModel Init() method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
            GeosApplication.Instance.Logger.Log("PickingODNScanViewModel Init() executed successfully", category: Category.Info, priority: Priority.Low);

        }
        /// <summary>
        /// Bar code scan action
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PickingODNScanViewModel ScanBarcodeAction()", category: Category.Info, priority: Priority.Low);
                if (((KeyEventArgs)obj).Key == Key.Enter)
                {
                    if (SupplierComplaintsMainList.Any(x => x.Code == OdnCode))
                    {
                        SupplierComplaint tempSupplierComplaint = SupplierComplaintsMainList.FirstOrDefault(x => x.Code == OdnCode);
                        //GetSupplierComplaints(tempSupplierComplaint);
                        PickingODNView pickingOdnView = new PickingODNView();
                        PickingODNViewModel pickingOdnViewModel = new PickingODNViewModel();
                        pickingOdnViewModel.Init(tempSupplierComplaint);
                        EventHandler handler = delegate { pickingOdnView.Close(); };
                        pickingOdnViewModel.RequestClose += handler;
                        pickingOdnView.DataContext = pickingOdnViewModel;
                        pickingOdnView.ShowDialogWindow();
                        CommandCancelAction(null);

                        List<SupplierComplaintItem> SupplierComplaintItemList = WarehouseService.GetRemainingSCItemsByIdSC(tempSupplierComplaint.IdSupplierComplaint, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse).ToList();
                        if (SupplierComplaintItemList.Count == 0)
                        {
                            SupplierComplaintsMainList.Remove(SupplierComplaintsMainList.FirstOrDefault(x => x.IdSupplierComplaint == tempSupplierComplaint.IdSupplierComplaint));
                        }
                    }
                    else
                    {
                        SetViewInErrorState();
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PickingODNScanViewModel ScanBarcodeAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("PickingODNScanViewModel ScanBarcodeAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// Display Error on view
        /// </summary>
        private void SetViewInErrorState()
        {
            GeosApplication.Instance.Logger.Log("PickingODNScanViewModel SetViewInErrorState()", category: Category.Info, priority: Priority.Low);

            OdnCodeError = " Wrong Item " + OdnCode;
            OdnCode = string.Empty;
            OdnCodeErrorColour = "#FFFF0000";
            OdnCodeErrorVisibility = Visibility.Visible;
            GeosApplication.Instance.Logger.Log("PickingODNScanViewModel SetViewInErrorState() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// To Get SupplierComplaintItems
        /// </summary>
        /// <param name = "supplierComplaint" ></ param >
        //private void GetSupplierComplaints(SupplierComplaint supplierComplaint)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("PickingODNScanViewModel GetSupplierComplaints()", category: Category.Info, priority: Priority.Low);

        //        if (!DXSplashScreen.IsActive)
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
        //                return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //            }, null, null);
        //        }

        //        SupplierComplaintsList = WarehouseService.GetRemainingSCItemsByIdSC(supplierComplaint.IdSupplierComplaint, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse).ToList();

        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in GetSupplierComplaints() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in GetSupplierComplaints() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in GetSupplierComplaints() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //    GeosApplication.Instance.Logger.Log("PickingODNScanViewModel GetSupplierComplaints() executed successfully", category: Category.Info, priority: Priority.Low);
        //}


        /// <summary>
        /// Cancel Action
        /// </summary>
        /// <param name="obj"></param>
        private void CommandCancelAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("CommandCancelAction PickingODNScanViewModel....", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
            GeosApplication.Instance.Logger.Log("CommandCancelAction PickingODNScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// obj Dispose
        /// </summary>
        public void Dispose()
        {
            GeosApplication.Instance.Logger.Log("Dispose PickingODNScanViewModel....", category: Category.Info, priority: Priority.Low);
            GC.SuppressFinalize(this);
            GeosApplication.Instance.Logger.Log("Dispose PickingODNScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

    }
}
