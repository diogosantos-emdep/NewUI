using DevExpress.Mvvm;
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
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class WorkOrderScanViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region TaskLog

        /// <summary>
        /// WMS	M056-02	New option of OT scanning for picking [adaibathina]
        /// GEOS2-1550 Option to Print only one label in Workorder item print option [adaibathina]
        /// [001] [GEOS2-1648][avpawar] Wizzard should not be displayed if there are NO items
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
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }
        #endregion //End Of Services

        #region Declaration
        private string workOrderBarcode;
        private string workOrderBarcodeError;
        private Visibility workOrderBarcodeErrorVisibility;
        private string workOrderErrorColour;
        private int selectedIndex = 0;
        private string windowHeader;
        private bool isRefund;
        #endregion

        #region Properties
        public string WorkOrderBarcode
        {
            get { return workOrderBarcode; }
            set
            {
                workOrderBarcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderBarcode"));
            }
        }
        public string WorkOrderBarcodeError
        {
            get { return workOrderBarcodeError; }
            set
            {
                workOrderBarcodeError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderBarcodeError"));
            }
        }
        public Visibility WorkOrderBarcodeErrorVisibility
        {
            get { return workOrderBarcodeErrorVisibility; }
            set
            {
                workOrderBarcodeErrorVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderBarcodeErrorVisibility"));
            }
        }
        public string WorkOrderErrorColour
        {
            get { return workOrderErrorColour; }
            set
            {
                workOrderErrorColour = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderErrorColour"));
            }
        }

        public List<Ots> OtsList { get; set; }

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
            }
        }
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public bool IsRefund
        {
            get
            {
                return isRefund;
            }

            set
            {
                isRefund = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRefund"));
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

        public WorkOrderScanViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);

            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandScanBarcode = new RelayCommand(new Action<object>(ScanBarcodeAction));
            //  CommandOnLoaded = new DelegateCommand<RoutedEventArgs>(LoadedAction);
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region Methods

        /// <summary>
        /// method to initialize
        /// </summary>
        /// <param name="OtsList">List of ots to compare</param>
        public void Init(List<Ots> OtsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Init WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);
                WorkOrderBarcodeErrorVisibility = Visibility.Hidden;
                this.OtsList = OtsList;
                GeosApplication.Instance.Logger.Log("Init WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderScanViewModel Init() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Scaning bare code
        /// [001] [GEOS2-1648][avpawar] Wizzard should not be displayed if there are NO items
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ScanBarcodeAction WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);

                if (((KeyEventArgs)obj).Key == Key.Enter)
                {
                    if (SelectedIndex == 0)
                    {
                        string strNumOt = string.Empty;
                        string workOrderCode = string.Empty;
                        byte numOt;

                        #region Barcode Validation 

                        if (WorkOrderBarcode.Contains('.'))
                        {
                            strNumOt = WorkOrderBarcode.Substring(WorkOrderBarcode.IndexOf('.') + 1, WorkOrderBarcode.Length - ((WorkOrderBarcode.IndexOf('.') + 1)));
                            workOrderCode = WorkOrderBarcode.Substring(0, WorkOrderBarcode.IndexOf('.'));

                            if (!byte.TryParse(strNumOt, out numOt))
                            {
                                SetViewInErrorState();
                                return;
                            }
                        }
                        else
                        {
                            SetViewInErrorState();
                            return;
                        }

                        #endregion

                        if (OtsList.Any(Ot => ((Ot.NumOT.ToString().Length == 1 ? "0" + Ot.NumOT : Ot.NumOT.ToString()) == strNumOt) && (Ot.Code == workOrderCode)))
                        {
                            Ots ot = OtsList.FirstOrDefault(x => x.Code == workOrderCode && x.NumOT == numOt);

                            try
                            {
                                ot = WarehouseService.GetWorkOrderByIdOt(ot.IdOT, Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse), WarehouseCommon.Instance.Selectedwarehouse);
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

                            if (!IsRefund)
                            {
                                bool FollowFIFO = true;
                                string FIFOGotoItem;
                                bool isTimer;
                                bool isBatchLabel;
                                PickingMaterialsViewModel pickingMaterialsViewModel = new PickingMaterialsViewModel();
                                isTimer = WarehouseCommon.Instance.IsPickingTimer;
                                pickingMaterialsViewModel.PreviousTimerValue = isTimer;
                                bool isItemsAvailable = false;

                                do
                                {
                                    PickingMaterialsView pickingMaterialsView = new PickingMaterialsView();

                                    FollowFIFO = pickingMaterialsViewModel.FollowFIFO;
                                    FIFOGotoItem = pickingMaterialsViewModel.FIFOGotoItem;
                                    isTimer = pickingMaterialsViewModel.PreviousTimerValue;
                                    isBatchLabel = pickingMaterialsViewModel.PrintBatchLabelAfterScanCompleted;
                                    pickingMaterialsViewModel = new PickingMaterialsViewModel();
                                    //pickingMaterialsViewModel.FollowFIFO = FollowFIFO;
                                    pickingMaterialsViewModel.PreviousTimerValue = isTimer;
                                    pickingMaterialsViewModel.PrintBatchLabelAfterScanCompleted = isBatchLabel;
                                    EventHandler handle = delegate { pickingMaterialsView.Close(); };
                                    pickingMaterialsViewModel.RequestClose += handle;
                                    pickingMaterialsViewModel.SetFollowFIFO(FollowFIFO, FIFOGotoItem);
                                    pickingMaterialsViewModel.InIt(ot, WarehouseCommon.Instance.Selectedwarehouse);

                                    //[001] Added
                                    if (pickingMaterialsViewModel.materialSoredList != null && pickingMaterialsViewModel.materialSoredList.Count > 0)
                                    {
                                        isItemsAvailable = true;

                                        //foreach (OtItem item in pickingMaterialsViewModel.OtItemList)
                                        //{
                                        //    if (item.PickingMaterialsList != null && item.PickingMaterialsList.Count > 0)
                                        //    {
                                        //        isItemsAvailable = true;
                                        //        break;
                                        //    }
                                        //}
                                    }

                                    if (isItemsAvailable)
                                    {
                                        pickingMaterialsView.DataContext = pickingMaterialsViewModel;
                                        pickingMaterialsView.ShowDialog();
                                    }
                                    else
                                    {
                                        break;
                                    }

                                } while (pickingMaterialsViewModel.IsCanceled == false);

                                //[001] Added
                                if (!isItemsAvailable)
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PickingNoItemsAvailable").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                }


                                RequestClose(null, null);
                            }
                            else
                            {
                                WorkOrderBarcodeErrorVisibility = Visibility.Hidden;
                                RefundWorkOrderView refundWorkOrderView = new RefundWorkOrderView();
                                RefundWorkOrderViewModel refundWorkOrderViewModel = new RefundWorkOrderViewModel();
                                EventHandler handle = delegate { refundWorkOrderView.Close(); };
                                refundWorkOrderViewModel.RequestClose += handle;
                                refundWorkOrderViewModel.Init(ot);
                                refundWorkOrderView.DataContext = refundWorkOrderViewModel;
                                refundWorkOrderView.ShowDialog();

                                RequestClose(null, null);
                            }
                        }
                        else
                        {
                            SetViewInErrorState();
                        }
                    }
                    else
                    {
                        try
                        {
                            Int64 idOT;
                            if (!IsRefund)
                            {
                                idOT = WarehouseService.GetIdOtByBarcode(WorkOrderBarcode);
                            }
                            else
                            {
                                idOT = WarehouseService.GetRefundIdOtByBarcode(WorkOrderBarcode);
                            }

                            // Ots OT = OtsList.FirstOrDefault(x => x.IdOT == idOT);
                            if (idOT > 0)
                            {
                                Ots ot = new Ots();
                                ot = WarehouseService.GetWorkOrderByIdOt(idOT, Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse), WarehouseCommon.Instance.Selectedwarehouse);

                                if (!IsRefund)
                                {
                                    WorkOrderBarcodeErrorVisibility = Visibility.Hidden;
                                    bool FollowFIFO = true;
                                    string FIFOGotoItem;
                                    bool isTimer;
                                    bool isBatchLabel;
                                    PickingMaterialsViewModel pickingMaterialsViewModel = new PickingMaterialsViewModel();
                                    isTimer = WarehouseCommon.Instance.IsPickingTimer;
                                    pickingMaterialsViewModel.PreviousTimerValue = isTimer;
                                    bool isItemsAvailable = false; //[001] Added

                                    do
                                    {
                                        PickingMaterialsView pickingMaterialsView = new PickingMaterialsView();
                                        FollowFIFO = pickingMaterialsViewModel.FollowFIFO;
                                        FIFOGotoItem = pickingMaterialsViewModel.FIFOGotoItem;
                                        isTimer = pickingMaterialsViewModel.PreviousTimerValue;
                                        isBatchLabel = pickingMaterialsViewModel.PrintBatchLabelAfterScanCompleted;
                                        pickingMaterialsViewModel = new PickingMaterialsViewModel();
                                        //pickingMaterialsViewModel.FollowFIFO = FollowFIFO;
                                        pickingMaterialsViewModel.PreviousTimerValue = isTimer;
                                        pickingMaterialsViewModel.PrintBatchLabelAfterScanCompleted = isBatchLabel;
                                        EventHandler handle = delegate { pickingMaterialsView.Close(); };
                                        pickingMaterialsViewModel.RequestClose += handle;
                                        pickingMaterialsViewModel.SetFollowFIFO(FollowFIFO, FIFOGotoItem);
                                        pickingMaterialsViewModel.InIt(ot, WarehouseCommon.Instance.Selectedwarehouse, WorkOrderBarcode);

                                        //[001] Added
                                        if (pickingMaterialsViewModel.materialSoredList != null && pickingMaterialsViewModel.materialSoredList.Count > 0)
                                        {
                                            isItemsAvailable = true;

                                            //foreach (OtItem item in pickingMaterialsViewModel.OtItemList)
                                            //{
                                            //    if (item.PickingMaterialsList != null && item.PickingMaterialsList.Count > 0)
                                            //    {
                                            //        isItemsAvailable = true;
                                            //        break;
                                            //    }
                                            //}
                                        }

                                        if (isItemsAvailable)
                                        {
                                            pickingMaterialsView.DataContext = pickingMaterialsViewModel;
                                            pickingMaterialsView.ShowDialog();
                                            WorkOrderBarcode = string.Empty;
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    } while (pickingMaterialsViewModel.IsCanceled == false);

                                    //[001] Added
                                    if (isItemsAvailable == false)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PickingNoItemsAvailable").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }
                                    // [001] End

                                    RequestClose(null, null);
                                }
                                else
                                {
                                    WorkOrderBarcodeErrorVisibility = Visibility.Hidden;
                                    RefundWorkOrderView refundWorkOrderView = new RefundWorkOrderView();
                                    RefundWorkOrderViewModel refundWorkOrderViewModel = new RefundWorkOrderViewModel();
                                    EventHandler handle = delegate { refundWorkOrderView.Close(); };
                                    refundWorkOrderViewModel.RequestClose += handle;
                                    refundWorkOrderViewModel.Init(ot, WorkOrderBarcode);
                                    refundWorkOrderView.DataContext = refundWorkOrderViewModel;
                                    refundWorkOrderView.ShowDialog();
                                    RequestClose(null, null);
                                }
                            }
                            else
                            {
                                SetViewInErrorState();
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

                    }
                }

                GeosApplication.Instance.Logger.Log("ScanBarcodeAction WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderScanViewModel ScanBarcodeAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void SetViewInErrorState()
        {
            if (SelectedIndex == 0)
                WorkOrderBarcodeError = " Wrong Work Order " + WorkOrderBarcode;
            else
                WorkOrderBarcodeError = " Wrong Item " + WorkOrderBarcode;
            WorkOrderBarcode = string.Empty;
            WorkOrderErrorColour = "#FFFF0000";
            WorkOrderBarcodeErrorVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Cancel Action
        /// </summary>
        /// <param name="obj"></param>
        private void CommandCancelAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("CommandCancelAction WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
            GeosApplication.Instance.Logger.Log("CommandCancelAction WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// obj Dispose
        /// </summary>
        public void Dispose()
        {
            GeosApplication.Instance.Logger.Log("Dispose WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);
            GC.SuppressFinalize(this);
            GeosApplication.Instance.Logger.Log("Dispose WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion
    }
}
