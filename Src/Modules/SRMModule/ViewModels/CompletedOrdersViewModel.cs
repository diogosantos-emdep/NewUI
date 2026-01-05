using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Logging;
using Emdep.Geos.UI.Helper;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Printing;
using Microsoft.Win32;
using Emdep.Geos.Modules.SRM.Views;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Utility;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using OutlookApp = Microsoft.Office.Interop.Outlook.Application;
using Emdep.Geos.Data.Common.Epc;
using System.IO;
using OutlookEmailFrom = Microsoft.Office.Interop.Outlook;
using System.Text.RegularExpressions;
using System.Globalization;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Editors.Flyout;
using System.Windows.Controls;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class CompletedOrdersViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {

        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        #endregion // Services

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

        #endregion // End Of Events 

        #region Declaration
        
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        private bool isBusy;
        private string myFilterString;

        private List<WarehousePurchaseOrder> closedPurchaseOrderList;
        private ObservableCollection<WarehousePurchaseOrder> listclosedPurchaseOrder;
        private WarehousePurchaseOrder selectedclosedPurchaseOrder;

        private bool isEdit;

        private string userSettingsKey = "SRM_PO_";
        private TileBarFilters selectedFilter;
        private int visibleRowCount;

        private List<WorkflowStatus> workflowStatusList;
        private TableView view;
        private bool isPurchaseOrderSave;
        private bool? isAllSave;
        private List<LogEntriesByWarehousePO> changeLogsEntry;
        private ObservableCollection<User> assigneeList;
        private ObservableCollection<User> approverList;
        private ObservableCollection<SystemSettings> systemSettingsList;//[Sudhir.jangra][GEOS2-4407][10/05/2023]

        ObservableCollection<Contacts> toContactList;
        ObservableCollection<Contacts> ccContactList;

        string fromDate;
        string toDate;
        int isButtonStatus;
        Visibility isCalendarVisible;
        private Duration _currentDuration;
        const string shortDateFormat = "dd/MM/yyyy";

        DateTime startDate;
        DateTime endDate;
        #endregion

        #region Properties
        public string FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
            }
        }

        public string ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }
        public int IsButtonStatus
        {
            get
            {
                return isButtonStatus;
            }

            set
            {
                isButtonStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonStatus"));
            }
        }

        public Visibility IsCalendarVisible
        {
            get
            {
                return isCalendarVisible;
            }

            set
            {
                isCalendarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalendarVisible"));
            }
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


        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        public ObservableCollection<User> AssigneeList
        {
            get
            {
                return assigneeList;
            }

            set
            {
                assigneeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AssigneeList"));
            }
        }

        public ObservableCollection<User> ApproverList
        {
            get
            {
                return approverList;
            }

            set
            {
                approverList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ApproverList"));
            }
        }


        public List<LogEntriesByWarehousePO> ChangeLogsEntry
        {
            get { return changeLogsEntry; }
            set { changeLogsEntry = value; }
        }
     


        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }



        public List<WarehousePurchaseOrder> ClosedPurchaseOrderList
        {
            get
            {
                return closedPurchaseOrderList;
            }

            set
            {
                closedPurchaseOrderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClosedPurchaseOrderList"));
            }
        }

        public ObservableCollection<WarehousePurchaseOrder> ListClosedPurchaseOrder
        {
            get
            {
                return listclosedPurchaseOrder;
            }

            set
            {
                listclosedPurchaseOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListClosedPurchaseOrder"));
            }
        }

        public WarehousePurchaseOrder SelectedClosedPurchaseOrder
        {
            get
            {
                return selectedclosedPurchaseOrder;
            }

            set
            {
                selectedclosedPurchaseOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedClosedPurchaseOrder"));
            }
        }
        public string CustomFilterStringName { get; set; }

        private List<GridColumn> GridColumnList;

        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }
        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }

        public TileBarFilters SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFilter"));
            }
        }


        public List<WorkflowStatus> WorkflowStatusList
        {
            get { return workflowStatusList; }
            set
            {
                workflowStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusList"));
            }
        }


        public bool IsPurchaseOrderSave
        {
            get { return isPurchaseOrderSave; }
            set
            {
                isPurchaseOrderSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPurchaseOrderSave"));
            }
        }

        public bool? IsAllSave
        {
            get { return isAllSave; }
            set
            {
                isAllSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllSave"));
            }
        }

        private User selectedAssignee;
        private User selectedApprover;
        public User SelectedAssignee
        {
            get
            {
                return selectedAssignee;
            }

            set
            {
                selectedAssignee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAssignee"));
            }
        }
        public User SelectedApprover
        {
            get
            {
                return selectedApprover;
            }

            set
            {
                selectedApprover = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedApprover"));
            }
        }


        public ObservableCollection<SystemSettings> SystemSettingsList//[Sudhir.jangra][GEOS2-4077][10/05/2023]
        {
            get { return systemSettingsList; }
            set
            {
                systemSettingsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SystemSettingsList"));
            }
        }



        //Shubham[skadam]  GEOS2-4405 (Grid) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
        WorkflowStatus workflowStatus;
        public WorkflowStatus WorkflowStatus
        {
            get
            {
                return workflowStatus;
            }

            set
            {
                workflowStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatus"));
            }
        }
        List<WorkflowTransition> workflowTransitionList;
        public List<WorkflowTransition> WorkflowTransitionList
        {
            get
            {
                return workflowTransitionList;
            }

            set
            {
                workflowTransitionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransitionList"));
            }
        }
        //Shubham[skadam] GEOS2-4406 Send an email notification when the status of the PO changes 21 06 2023
        string comment = string.Empty;
        public string Comment
        {
            get
            {
                return comment;
            }

            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
            }
        }
        bool isSend = false;
        public bool IsSend
        {
            get
            {
                return isSend;
            }

            set
            {
                isSend = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSend"));
            }
        }
        WarehousePurchaseOrder warehousePurchaseOrder;
        public WarehousePurchaseOrder WarehousePurchaseOrderForSendPOEmail
        {
            get
            {
                return warehousePurchaseOrder;
            }

            set
            {
                warehousePurchaseOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehousePurchaseOrder"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5493]
        public ObservableCollection<Contacts> ToContactList
        {
            get { return toContactList; }
            set
            {
                toContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToContactList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5493]
        public ObservableCollection<Contacts> CCContactList
        {
            get { return ccContactList; }
            set
            {
                ccContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CCContactList"));
            }
        }

        #endregion

        #region ICommand
        public ICommand PeriodCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ICommand RefreshCompletedOrderViewCommand { get; set; }
        public ICommand PrintCompletedOrderViewCommand { get; set; }
        public ICommand ExportCompletedOrderViewCommand { get; set; }
        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }

        public ICommand CommandWarehouseEditValueChanged { get; private set; }


        public ICommand CommandShowFilterPopupForWorkflowStatusTransitionClick { get; set; }

        #endregion

        #region Constructor

        public CompletedOrdersViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PendingPurchaseOrderViewModel....", category: Category.Info, priority: Priority.Low);

                //isInIt = true;

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
                PeriodCommand = new DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DelegateCommand<object>(CancelCommandAction);

                RefreshCompletedOrderViewCommand = new RelayCommand(new Action<object>(RefreshCompletedOrderList));
                PrintCompletedOrderViewCommand = new RelayCommand(new Action<object>(PrintCompletedOrderList));
                ExportCompletedOrderViewCommand = new RelayCommand(new Action<object>(ExportCompletedOrderList));
                OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));

                CommandGridDoubleClick = new DelegateCommand<object>(PendingReceptionItemsWindowShow);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);

                CommandShowFilterPopupForWorkflowStatusTransitionClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForWorkflowStatusTransition);

              
                
                MyFilterString = string.Empty;

                setDefaultPeriod();
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;

                

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor PendingPurchaseOrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PendingPurchaseOrderViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

        #region Methods

        public void Init()
        {
            FillCompletedPurchaseOrderList();
            FillWorkFlowStatusList();
        }
        private void setDefaultPeriod()
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                FromDate = StartFromDate.ToString(shortDateFormat);
                ToDate = EndToDate.ToString(shortDateFormat);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method setDefaultPeriod()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void RefreshCompletedOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                IsBusy = true;
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

                FillCompletedPurchaseOrderList();

                detailView.SearchString = null;

                MyFilterString = string.Empty;

                IsBusy = false;
                SRMPurchaseOrderCellEditHelper.SetIsValueChanged(view, false);
                SRMPurchaseOrderCellEditHelper.IsValueChanged = false;
                gridControl.RefreshData();
                gridControl.UpdateLayout();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshCompletedOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshCompletedOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintCompletedOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintCompletedOrderList()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PurchaseOrderListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PurchaseOrderListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintCompletedOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintCompletedOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportCompletedOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportCompletedOrderList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Closed Order List";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
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

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportCompletedOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportCompletedOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocumentCompletedOrder()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();

                documentViewModel.OpenPdf(SelectedClosedPurchaseOrder, obj);
                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocumentCompletedOrder()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocumentCompletedOrder() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocumentCompletedOrder() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocumentCompletedOrder()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void PendingReceptionItemsWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PendingReceptionItemsWindowShow....", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                ViewPurchaseOrderViewModel viewPurchaseOrderViewModel = new ViewPurchaseOrderViewModel();
                ViewPurchaseOrderView ViewPurchaseOrderView = new ViewPurchaseOrderView();

                EventHandler handle = delegate { ViewPurchaseOrderView.Close(); };
                viewPurchaseOrderViewModel.RequestClose += handle;

                WarehousePurchaseOrder wpo = (WarehousePurchaseOrder)detailView.FocusedRow;
                SRM.SRMCommon.Instance.Selectedwarehouse = SRMCommon.Instance.WarehouseList.FirstOrDefault(i => i.IdWarehouse == wpo.IdWarehouse);
                Warehouses warehouse = SRM.SRMCommon.Instance.Selectedwarehouse;
                viewPurchaseOrderViewModel.Init(wpo, warehouse);
                ViewPurchaseOrderView.DataContext = viewPurchaseOrderViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                ViewPurchaseOrderView.Owner = Window.GetWindow(ownerInfo);
                ViewPurchaseOrderView.ShowDialog();
                ListClosedPurchaseOrder = new ObservableCollection<WarehousePurchaseOrder>(ClosedPurchaseOrderList);
                if (viewPurchaseOrderViewModel.IsSaveChanges == true)
                {
                    wpo.IdWorkflowStatus = viewPurchaseOrderViewModel.WorkflowStatus.IdWorkflowStatus;
                    wpo.WorkflowStatus = viewPurchaseOrderViewModel.WorkflowStatus;



                    MyFilterString = string.Empty;
                    SelectedClosedPurchaseOrder = ListClosedPurchaseOrder.Where(x => x.IdWarehousePurchaseOrder == wpo.IdWarehousePurchaseOrder).First();
                }

                if (!viewPurchaseOrderViewModel.IsMoreNeeded)
                {
                    var item = ListClosedPurchaseOrder.Where(x => x.IdWarehousePurchaseOrder == wpo.IdWarehousePurchaseOrder).First();
                    ListClosedPurchaseOrder.Remove(item);
                    ListClosedPurchaseOrder = new ObservableCollection<WarehousePurchaseOrder>(ListClosedPurchaseOrder);
                }
                #region GEOS2-3986
                // shubham[skadam] GEOS2-3986 Not change the color after change status from view purchase order window  21 OCT 2022
                if (ListClosedPurchaseOrder != null)
                {
                    WarehousePurchaseOrder WarehousePurchaseOrder = ListClosedPurchaseOrder.Where(w => w.IdWarehousePurchaseOrder == wpo.IdWarehousePurchaseOrder).FirstOrDefault();
                    List<WorkflowStatus> workflowStatusList = new List<WorkflowStatus>();
                    List<WorkflowTransition> workflowTransition = WarehousePurchaseOrder.LstAllWorkflowStatusTransitions.Where(w => w.IdWorkflowStatusFrom == WarehousePurchaseOrder.IdWorkflowStatus).ToList();
                    workflowStatusList.Add(WarehousePurchaseOrder.LstWorkflowStatus.Where(w => w.IdWorkflowStatus == WarehousePurchaseOrder.IdWorkflowStatus).FirstOrDefault());
                    if (workflowTransition != null)
                    {
                        foreach (WorkflowTransition WorkflowTransition in workflowTransition)
                        {
                            workflowStatusList.AddRange(WarehousePurchaseOrder.LstWorkflowStatus.Where(w => w.IdWorkflowStatus == WorkflowTransition.IdWorkflowStatusTo));
                        }
                        WarehousePurchaseOrder.LstWorkflowStatusTransition = workflowStatusList;
                    }
                }
                #endregion
                GeosApplication.Instance.Logger.Log("Method PendingReceptionItemsWindowShow....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PendingReceptionItemsWindowShow...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// After warehouse selection  fill list as per it.
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseEditValueChangedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);
            //When setting the warehouse from default the data should not be refreshed
            if (!SRMCommon.Instance.IsWarehouseChangedEventCanOccur)
                return;

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

            //fill data as per selected warehouse
            FillCompletedPurchaseOrderList();

            //rearrange tile Arrange


            MyFilterString = string.Empty;

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehouseEditValueChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }



        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            try
            {
                if (e.Value != null && e.Value.ToString() != "Status" && e.ColumnFieldName == "IdWorkflowStatus")
                {
                    e.Value = ListClosedPurchaseOrder.First().LstWorkflowStatus.FirstOrDefault(a => a.IdWorkflowStatus == Convert.ToInt32(e.Value)).Name;
                }
                e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
                //Shubham[skadam] GEOS2-5084 Apply same format always for amounts or quantities 26 12 2023
                if (e.Value != null && e.Value.ToString() != "Amount" && e.ColumnFieldName == "TotalAmount")
                {
                    try
                    {
                        string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        string formattedNumber = Convert.ToDouble(e.Value).ToString("N2", System.Globalization.CultureInfo.CurrentCulture);

                        e.Value = formattedNumber;
                        e.Formatting.Alignment.HorizontalAlignment = XlHorizontalAlignment.Right;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Options_CustomizeCell() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                e.Handled = true;

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Options_CustomizeCell() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCompletedPurchaseOrderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompletedPurchaseOrderList...", category: Category.Info, priority: Priority.Low);

                if (SRM.SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    try
                    {
                        List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList(); //[001] added
                                                                                                                                       //  var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdWarehouse));
                                                                                                                                       // var plantconnection = plantOwners.FirstOrDefault().Company.ConnectPlantConstr;
                        long plantOwnersIds;
                        string plantconnection;

                        ObservableCollection<WarehousePurchaseOrder> TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>();

                        ClosedPurchaseOrderList = new List<WarehousePurchaseOrder>();

                        foreach (var item in plantOwners)
                        {
                            plantOwnersIds = item.IdWarehouse;
                            plantconnection = item.Company.ConnectPlantConstr;
                            // TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetCompletedPurchaseOrdersByWarehouse_V2520(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), plantOwnersIds, plantconnection));
                            // [pallavi.kale][05-03-2025][GEOS2-7013]
                            TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetCompletedPurchaseOrdersByWarehouse_V2620(DateTime.ParseExact(FromDate, "dd/MM/yyyy", null), DateTime.ParseExact(ToDate, "dd/MM/yyyy", null), plantOwnersIds, plantconnection));

                            if (TempMainPurchaseOrderList != null)
                                TempMainPurchaseOrderList.ToList().ForEach(i => i.Warehouse = item);
                          
                            //TempMainPurchaseOrderList.Add(count);
                            ClosedPurchaseOrderList.AddRange(TempMainPurchaseOrderList);
                          
                        }

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillCompletedPurchaseOrderList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillCompletedPurchaseOrderList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    ClosedPurchaseOrderList = new List<WarehousePurchaseOrder>(ClosedPurchaseOrderList);
                }
                else
                {
                    ClosedPurchaseOrderList = new List<WarehousePurchaseOrder>();
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillCompletedPurchaseOrderList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompletedPurchaseOrderList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void FillWorkFlowStatusList()
        {

            GeosApplication.Instance.Logger.Log("Method FillWorkFlowStatusList...", category: Category.Info, priority: Priority.Low);


            try
            {
                WorkflowStatusList = new List<WorkflowStatus>();
                WorkflowStatusList = SRMService.GetWorkFlowStatus();


            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkFlowStatusList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkFlowStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }


        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
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
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }







        private void CustomShowFilterPopupForWorkflowStatusTransition(FilterPopupEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupForWorkflowStatusTransition ...", category: Category.Info, priority: Priority.Low);
                if (e.Column.FieldName == "IdWorkflowStatus")
                {
                    if (e.Column.FieldName != "IdWorkflowStatus")
                    {
                        return;
                    }

                    try
                    {
                        List<object> filterItem = new List<object>();
                        foreach (var item in ListClosedPurchaseOrder.First().LstWorkflowStatus)
                        {
                            string StatusValue = item.Name;

                            if (StatusValue == null)
                            {
                                continue;
                            }

                            if (!filterItem.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == StatusValue))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = StatusValue;
                                customComboBoxItem.EditValue = item.IdWorkflowStatus;
                                filterItem.Add(customComboBoxItem);
                            }
                        }
                        e.ComboBoxEdit.ItemsSource = filterItem.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).EditValue)).ToList();
                        GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForWorkflowStatusTransition() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupForWorkflowStatusTransition() method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForWorkflowStatusTransition() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CustomShowFilterPopupForWorkflowStatusTransition()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void TransitionWorkflowStatus(int currentStatus, WarehousePurchaseOrder WarehousePurchaseOrder)
        {
            try
            {
                List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList = new List<LogEntriesByWarehousePO>();
                WorkflowTransition workflowTransition = new WorkflowTransition();
                workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == currentStatus);
                WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == currentStatus);

                if (workflowTransition != null && workflowTransition.IsCommentRequired == 1)
                {
                    AddWorkflowStatusCommentView addWorkflowStatusCommentView = new AddWorkflowStatusCommentView();
                    AddWorkflowStatusCommentViewModel addWorkflowStatusCommentViewModel = new AddWorkflowStatusCommentViewModel();
                    EventHandler handle = delegate { addWorkflowStatusCommentView.Close(); };
                    addWorkflowStatusCommentViewModel.RequestClose += handle;
                    addWorkflowStatusCommentView.DataContext = addWorkflowStatusCommentViewModel;
                    addWorkflowStatusCommentView.ShowDialogWindow();

                    if (addWorkflowStatusCommentViewModel.IsSaveChanges == true)
                    {
                        LogEntriesByWarehousePO CommentByWarehousePO = new LogEntriesByWarehousePO();
                        CommentByWarehousePO.IdWarehousePurchaseOrder = WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                        CommentByWarehousePO.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                        CommentByWarehousePO.Comments = addWorkflowStatusCommentViewModel.Comment;
                        CommentByWarehousePO.IdEntryType = 257;
                        CommentByWarehousePO.IsRtfText = false;
                        CommentByWarehousePO.Datetime = GeosApplication.Instance.ServerDateTime;
                        CommentByWarehousePO.People = new People();
                        CommentByWarehousePO.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                        CommentByWarehousePO.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                        CommentByWarehousePO.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                        LogEntriesByWarehousePOList.Add(CommentByWarehousePO);
                        WarehousePurchaseOrder.WarehousePOComments.Insert(0, CommentByWarehousePO);
                        WarehousePurchaseOrder.WarehousePOComments = new List<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOComments);


                        LogEntriesByWarehousePO logEntriesByWarehousePO_1 = new LogEntriesByWarehousePO();
                        logEntriesByWarehousePO_1.IdWarehousePurchaseOrder = WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                        logEntriesByWarehousePO_1.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                        logEntriesByWarehousePO_1.Comments = string.Format(System.Windows.Application.Current.FindResource("POWorkflowStatusCommentChangeLog").ToString(), WorkflowStatus.Name);
                        logEntriesByWarehousePO_1.IdEntryType = 258;
                        logEntriesByWarehousePO_1.IsRtfText = false;
                        logEntriesByWarehousePO_1.Datetime = GeosApplication.Instance.ServerDateTime;
                        logEntriesByWarehousePO_1.People = new People();
                        logEntriesByWarehousePO_1.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                        logEntriesByWarehousePO_1.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                        logEntriesByWarehousePO_1.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                        LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO_1);
                        WarehousePurchaseOrder.WarehousePOLogEntries.Insert(0, logEntriesByWarehousePO_1);
                        WarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOLogEntries);

                        Comment = addWorkflowStatusCommentViewModel.Comment;
                    }
                    else
                    {
                        return;
                    }
                }

                LogEntriesByWarehousePO logEntriesByWarehousePO = new LogEntriesByWarehousePO();
                logEntriesByWarehousePO.IdWarehousePurchaseOrder = WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                logEntriesByWarehousePO.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                logEntriesByWarehousePO.Comments = string.Format(System.Windows.Application.Current.FindResource("WorkflowStatusLogEntryByPO").ToString(), WarehousePurchaseOrder.WorkflowStatus.Name, WorkflowStatus.Name);
                logEntriesByWarehousePO.IdEntryType = 258;
                logEntriesByWarehousePO.IsRtfText = false;
                logEntriesByWarehousePO.Datetime = GeosApplication.Instance.ServerDateTime;
                logEntriesByWarehousePO.People = new People();
                logEntriesByWarehousePO.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                logEntriesByWarehousePO.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                logEntriesByWarehousePO.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO);
                if (WarehousePurchaseOrder.WarehousePOLogEntries == null)
                {
                    WarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>();
                }
                WarehousePurchaseOrder.WarehousePOLogEntries.Insert(0, logEntriesByWarehousePO);
                WarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOLogEntries);
                //bool IsSaveChanges = SRMService.UpdateWorkflowStatusInPO_V2380((uint)WarehousePurchaseOrder.IdWarehousePurchaseOrder, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByWarehousePOList, (uint)0, (uint)0);
                //Shubham[skadam] GEOS2-5026 Log not added properly in purchase order 08 11 2023
                //[pramod.misal][GEOS2-4755][16-08-2023]
                bool IsSaveChanges = SRMService.UpdateWorkflowStatusInPO_V2420((uint)WarehousePurchaseOrder.IdWarehousePurchaseOrder, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByWarehousePOList, (uint)0, (uint)0);

                //           try
                //           {
                ////Shubham[skadam] GEOS2-4406 Send an email notification when the status of the PO changes 21 06 2023
                //               if (IsSend != true)
                //               {
                //                   if (!string.IsNullOrEmpty(Comment))
                //                   {
                //                       SendPOEmailNotification(WorkflowStatus.IdWorkflowStatus, WarehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus);
                //                       Comment = string.Empty;
                //                   }
                //               }
                //           }
                //           catch (Exception ex)
                //           {
                //           }
                WarehousePurchaseOrder.IdWorkflowStatus = WorkflowStatus.IdWorkflowStatus;
                WarehousePurchaseOrder.WorkflowStatus = WorkflowStatus;
            }
            catch (Exception ex)
            {

            }
        }

            private void PeriodCommandAction(object obj)
        {
            if (obj == null)
                return;

            Button button = (Button)obj;
            if (button.Name == "ThisMonth")
            {
                IsButtonStatus = 1;
            }
            else if (button.Name == "LastOneMonth")
            {
                IsButtonStatus = 2;
            }
            else if (button.Name == "LastMonth")
            {
                IsButtonStatus = 3;
            }
            else if (button.Name == "ThisWeek")
            {
                IsButtonStatus = 4;
            }
            else if (button.Name == "LastOneWeek")
            {
                IsButtonStatus = 5;
            }
            else if (button.Name == "LastWeek")
            {
                IsButtonStatus = 6;
            }
            else if (button.Name == "CustomRange")
            {
                IsButtonStatus = 7;
            }
            else if (button.Name == "ThisYear")
            {
                IsButtonStatus = 8;
            }
            else if (button.Name == "LastYear")
            {
                IsButtonStatus = 9;
            }
            else if (button.Name == "Last12Months")
            {
                IsButtonStatus = 10;
            }
            IsCalendarVisible = Visibility.Collapsed;

        }

        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
        }

        private void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);

                MenuFlyout menu = (MenuFlyout)obj;
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                menu.FlyoutControl.Closed += FlyoutControl_Closed;
                menu.IsOpen = false;
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ApplyCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }

        private void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();

                DateTime baseDate = DateTime.Today;
                var today = baseDate;
                //this week
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                //Last week
                var lastWeekStart = thisWeekStart.AddDays(-7);
                var lastWeekEnd = thisWeekStart.AddSeconds(-1);
                //this month
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                //last month
                var lastMonthStart = thisMonthStart.AddMonths(-1);
                var lastMonthEnd = thisMonthStart.AddSeconds(-1);
                //last one month
                var lastOneMonthStart = baseDate.AddMonths(-1);
                var lastOneMonthEnd = baseDate;
                //Last one week
                var lastOneWeekStart = baseDate.AddDays(-7);
                var lastOneWeekEnd = baseDate;

                //Last Year
                int year = DateTime.Now.Year - 1;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                if (IsButtonStatus == 0)
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 1)//this month
                {
                    FromDate = thisMonthStart.ToString(shortDateFormat);
                    ToDate = thisMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    FromDate = lastOneMonthStart.ToString(shortDateFormat);
                    ToDate = lastOneMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 3) //last month
                {
                    FromDate = lastMonthStart.ToString(shortDateFormat);
                    ToDate = lastMonthEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 4) //this week
                {
                    FromDate = thisWeekStart.ToString(shortDateFormat);
                    ToDate = thisWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    FromDate = lastOneWeekStart.ToString(shortDateFormat);
                    ToDate = lastOneWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 6) //last week
                {
                    FromDate = lastWeekStart.ToString(shortDateFormat);
                    ToDate = lastWeekEnd.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    FromDate = StartDate.ToString(shortDateFormat);
                    ToDate = EndDate.ToString(shortDateFormat);
                }
                else if (IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    FromDate = StartFromDate.ToString(shortDateFormat);
                    ToDate = EndToDate.ToString(shortDateFormat);
                }

                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    FromDate = Date_F.ToShortDateString();
                    ToDate = Date_T.ToShortDateString();
                }

                Init();

                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FlyoutControl_Closed....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private Action Processing()
        {
            IsBusy = true;
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
            return null;
        }


        #endregion
    }
}
