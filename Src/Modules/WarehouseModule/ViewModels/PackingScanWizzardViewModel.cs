using System;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using System.Windows.Input;
using System.Windows;
using Emdep.Geos.Services.Contracts;
using System.ComponentModel;
using Prism.Logging;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common;
using System.Collections.Generic;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.Warehouse.Views;
using System.Linq;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class PackingScanWizzardViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
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
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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

        List<Ots> workOrderOtsList;
        public List<Ots> WorkOrderOtsList
        {
            get { return workOrderOtsList; }
            set
            {
                workOrderOtsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderOtsList"));
            }
        }

        TableView tableView;
        public TableView TableView
        {
            get { return tableView; }
            set
            {
                tableView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TableView"));
            }
        }
        #endregion

        #region Command       
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandNextButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandOnLoaded { get; set; }
        public ICommand SkipCancelButton { get; set; }

        #endregion

        #region Constructor 

        public PackingScanWizzardViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);

            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandScanBarcode = new RelayCommand(new Action<object>(ScanBarcodeAction));
            SkipCancelButton = new DelegateCommand<object>(CommandSkipAction);
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region Methods

        /// <summary>
        /// method to initialize
        /// </summary>
        /// <param name="OtsList">List of ots to compare</param>
        public void Init(List<Company> list)
        //public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Init WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);
                WorkOrderBarcodeErrorVisibility = Visibility.Hidden;
                //this.OtsList = OtsList;
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
                        try
                        {
                            System.Windows.Input.KeyEventArgs KeyEventArgs = (System.Windows.Input.KeyEventArgs)obj;
                            System.Windows.Controls.TextBox TextBox = (System.Windows.Controls.TextBox)KeyEventArgs.OriginalSource;
                            string scanCode = TextBox.Text.Trim();
                            ScanWorkOrderByidSites(TableView, scanCode);
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
                    else
                    {
                        try
                        {
                            System.Windows.Input.KeyEventArgs KeyEventArgs = (System.Windows.Input.KeyEventArgs)obj;
                            System.Windows.Controls.TextBox TextBox = (System.Windows.Controls.TextBox)KeyEventArgs.OriginalSource;
                            string scanCode = TextBox.Text.Trim();
                            ScanWorkOrderByWorkOrder(TableView, scanCode);
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

                GeosApplication.Instance.Logger.Log("ScanBarcodeAction ScanBarcodeAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction ScanBarcodeAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void SetViewInErrorState()
        {
            if (SelectedIndex != 0)
                WorkOrderBarcodeError = " Wrong Work Order " + WorkOrderBarcode;
            else
                WorkOrderBarcodeError = " Wrong Customer " + WorkOrderBarcode;
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


        private void CommandSkipAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("CommandCancelAction CommandSkipAction....", category: Category.Info, priority: Priority.Low);
            ScanWorkOrder(TableView);
            GeosApplication.Instance.Logger.Log("CommandCancelAction CommandSkipAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void ScanWorkOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PackingViewModel Method ScanWorkOrder...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                PackingScanView packingScanView = new PackingScanView();
                PackingScanViewModel packingScanViewModel = new PackingScanViewModel();
                packingScanViewModel.Init(WorkOrderOtsList.Select(x => x.Quotation.Site).ToList());
                EventHandler handler = delegate { packingScanView.Close(); };
                packingScanViewModel.RequestClose += handler;
                packingScanView.DataContext = packingScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                packingScanView.Owner = Window.GetWindow(ownerInfo);
                packingScanView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("PackingViewModel Method ScanWorkOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PackingViewModel ScanWorkOrder() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ScanWorkOrderByidSites(object obj,string ShortName)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PackingViewModel Method ScanWorkOrder...", category: Category.Info, priority: Priority.Low);

                List<Company> companylist = WorkOrderOtsList.Where(x => x.Quotation.Site.ShortName.ToUpper().Equals(ShortName.ToUpper())).Select(s => s.Quotation.Site).ToList();
                if (companylist.Count == 0)
                {
                    SetViewInErrorState();
                    return;
                }
                TableView detailView = (TableView)obj;
                PackingScanView packingScanView = new PackingScanView();
                PackingScanViewModel packingScanViewModel = new PackingScanViewModel();
                packingScanViewModel.InitByidSites(companylist);
                EventHandler handler = delegate { packingScanView.Close(); };
                packingScanViewModel.RequestClose += handler;
                packingScanView.DataContext = packingScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                packingScanView.Owner = Window.GetWindow(ownerInfo);
                packingScanView.ShowDialogWindow();
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("PackingViewModel Method ScanWorkOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PackingViewModel ScanWorkOrder() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ScanWorkOrderByWorkOrder(object obj, string WorkOrderCode)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PackingViewModel Method ScanWorkOrder...", category: Category.Info, priority: Priority.Low);

                List<Company> tempCompanyList = WorkOrderOtsList.Select(x => x.Quotation.Site).ToList();
                //WarehouseService = new WarehouseServiceController("localhost:6699");
                ObservableCollection<WOItem> UnPackedItemsList = new ObservableCollection<WOItem>(WarehouseService.GetPackingWorkOrders_V2530(WarehouseCommon.Instance.Selectedwarehouse, WorkOrderCode));
                if (UnPackedItemsList.Count==0)
                {
                    SetViewInErrorState();
                    return;
                }
                // Get the list of company IDs from UnPackedItemsList
                List<int> unPackedCompaniesIdList = UnPackedItemsList.Select(x => x.IdCompany).Distinct().ToList();


                // Filter tempCompanyList to only include companies with IDs in unPackedCompaniesIdList and combine the lists
                List<Company> distinctTempCompanyList = tempCompanyList
                    .Where(company => unPackedCompaniesIdList.Contains(company.IdCompany))
                    .Union(tempCompanyList.Where(company => unPackedCompaniesIdList.Contains(company.IdCompany)))
                    .Distinct()
                    .ToList();

                TableView detailView = (TableView)obj;
                PackingScanView packingScanView = new PackingScanView();
                PackingScanViewModel packingScanViewModel = new PackingScanViewModel();
                packingScanViewModel.UnPackedItemsList = UnPackedItemsList;
                packingScanViewModel.CompanyList = tempCompanyList;
                //packingScanViewModel.InitByWorkOrder(WorkOrderCode);
                packingScanViewModel.InitByidSites(distinctTempCompanyList);
                packingScanViewModel.IsWorkOrder = true;
                packingScanViewModel.WorkOrder = WorkOrderCode;
                packingScanViewModel.IdOT = UnPackedItemsList.FirstOrDefault().IdOT;
                if (packingScanViewModel.UnPackedItemsListForWorkOrder==null)
                {
                    packingScanViewModel.UnPackedItemsListForWorkOrder = new ObservableCollection<WOItem>();
                }
                packingScanViewModel.UnPackedItemsListForWorkOrder.AddRange(UnPackedItemsList);
                EventHandler handler = delegate { packingScanView.Close(); };
                packingScanViewModel.RequestClose += handler;
                packingScanView.DataContext = packingScanViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                packingScanView.Owner = Window.GetWindow(ownerInfo);
                packingScanView.ShowDialogWindow();
                packingScanViewModel.IsWorkOrder = false;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("PackingViewModel Method ScanWorkOrder executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PackingViewModel ScanWorkOrder() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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