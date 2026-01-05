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
using System.Net.Mail;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class PendingPurchaseOrderViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {

        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        //ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        public virtual bool IsMailSend { get; set; }
        private bool isBusy;
        private ObservableCollection<TileBarFilters> listofitem;
        private string myFilterString;
        private int selectedTileIndex;
        private List<WarehousePurchaseOrder> mainPurchaseOrderList;
        private ObservableCollection<WarehousePurchaseOrder> listPurchaseOrder;
        private WarehousePurchaseOrder selectedPurchaseOrder;

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
        private EmailAssigneeAndCreatedBy emailAssigneeAndCreatedBy;//[Sudhir.Jangra][GEOS2-4407][10/05/2023]
        private ObservableCollection<SystemSettings> systemSettingsList;//[Sudhir.jangra][GEOS2-4407][10/05/2023]
        private string emailFrom;//[Sudhir.Jangra][GEOS2-4407][11/05/2023]
        private string emailCC;//[Sudhir.Jangra][GEOS2-4407][11/05/2023]

        ObservableCollection<Contacts> toContactList;
        ObservableCollection<Contacts> ccContactList;
        #endregion

        #region Properties

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
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public ObservableCollection<TileBarFilters> Listofitem
        {
            get
            {
                return listofitem;
            }

            set
            {
                listofitem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Listofitem"));
            }
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

        public int SelectedTileIndex
        {
            get
            {
                return selectedTileIndex;
            }

            set
            {
                selectedTileIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileIndex"));
            }
        }

        public List<WarehousePurchaseOrder> MainPurchaseOrderList
        {
            get
            {
                return mainPurchaseOrderList;
            }

            set
            {
                mainPurchaseOrderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainPurchaseOrderList"));
            }
        }

        public ObservableCollection<WarehousePurchaseOrder> ListPurchaseOrder
        {
            get
            {
                return listPurchaseOrder;
            }

            set
            {
                listPurchaseOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListPurchaseOrder"));
            }
        }

        public WarehousePurchaseOrder SelectedPurchaseOrder
        {
            get
            {
                return selectedPurchaseOrder;
            }

            set
            {
                selectedPurchaseOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPurchaseOrder"));
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

        public EmailAssigneeAndCreatedBy EmailAssigneeAndCreatedBy //[Sudhir.jangra][GEOS2-4077][10/05/2023]
        {
            get
            {
                return emailAssigneeAndCreatedBy;
            }
            set
            {
                emailAssigneeAndCreatedBy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmailAssigneeAndCreatedBy"));
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
        public string EmailFrom//[Sudhir.jangra][GEOS2-4077][11/05/2023]
        {
            get { return emailFrom; }
            set
            {
                emailFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmailFrom"));
            }
        }
        public string EmailCC
        {
            get { return emailCC; }
            set
            {
                emailCC = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmailCC"));
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

        public ICommand RefreshPurchaseOrderViewCommand { get; set; }
        public ICommand PrintPurchaseOrderViewCommand { get; set; }
        public ICommand ExportPurchaseOrderViewCommand { get; set; }
        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }

        public ICommand CommandWarehouseEditValueChanged { get; private set; }

        public ICommand CommandTileBarClickDoubleClick { get; set; }


        public ICommand FilterEditorCreatedCommand { get; set; }


        public ICommand SendEmailCommand { get; set; }

        public ICommand UpdateMultipleRowsPendingPurchaseGridCommand { get; set; }

        public ICommand CommandShowFilterPopupForWorkflowStatusTransitionClick { get; set; }
        public ICommand SendPOEmailCommand { get; set; }

        #endregion

        #region Constructor

        public PendingPurchaseOrderViewModel()
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

                RefreshPurchaseOrderViewCommand = new RelayCommand(new Action<object>(RefreshPurchaseOrderList));
                PrintPurchaseOrderViewCommand = new RelayCommand(new Action<object>(PrintPurchaseOrderList));
                ExportPurchaseOrderViewCommand = new RelayCommand(new Action<object>(ExportPurchaseOrderList));
                OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));

                CommandShowFilterPopupClick = new DelegateCommand<object>(LeadsShowFilterValue);
                CommandGridDoubleClick = new DelegateCommand<object>(PendingReceptionItemsWindowShow);
                CommandWarehouseEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(WarehouseEditValueChangedCommandAction);

                CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTileBarClickDoubleClickAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                SendPOEmailCommand = new DelegateCommand<object>(SendPOEmailCommandAction);
                SendEmailCommand = new DelegateCommand<object>(SendEmailCommandAction);
                UpdateMultipleRowsPendingPurchaseGridCommand = new DelegateCommand<object>(UpdateMultipleRowsPendingPurchaseGridCommandAction);
                CommandShowFilterPopupForWorkflowStatusTransitionClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForWorkflowStatusTransition);
                //Fill data as per selected warehouse
                FillMainPurchaseOrderList();
                FillWorkFlowStatusList();
                //Rearrange tile Arrange
                TileBarArrange(MainPurchaseOrderList);

                MyFilterString = string.Empty;

                AddCustomSetting();

                //[pramod.misal][30.06.2023][GEOS2-4448]
                //try
                //{

                //    foreach (var item in Listofitem)
                //    {
                //        item.SortOrder = WorkflowStatusList.FirstOrDefault(s => s.Name == item.DisplayText).SortOrder;
                //    }

                //}
                //catch (Exception ex)
                //{


                //}



                #region GEOS2-4405
                //Shubham[skadam]  GEOS2-4405 (Grid) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
                try
                {
                    WorkflowTransitionList = new List<WorkflowTransition>();
                    //  WorkflowTransitionList = MainPurchaseOrderList.FirstOrDefault().LstAllWorkflowStatusTransitions;
                    WorkflowTransitionList = new List<WorkflowTransition>(SRMService.GetAllWorkflowTransitions_V2400());
                }
                catch (Exception ex)
                {
                }
                #endregion
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

        }

        public void RefreshPurchaseOrderList(object obj)
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

                FillMainPurchaseOrderList();
                //Rearrange tile Arrange
                TileBarArrange(MainPurchaseOrderList);
                detailView.SearchString = null;
                AddCustomSetting();
                MyFilterString = string.Empty;

                IsBusy = false;
                SRMPurchaseOrderCellEditHelper.SetIsValueChanged(view, false);
                SRMPurchaseOrderCellEditHelper.IsValueChanged = false;
                gridControl.RefreshData();
                gridControl.UpdateLayout();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }


                //ViewPurchaseOrderView viewPurchaseOrderView = new ViewPurchaseOrderView();
                //ViewPurchaseOrderViewModel viewPurchaseOrderViewModel = new ViewPurchaseOrderViewModel();
                //EventHandler handle = delegate { viewPurchaseOrderView.Close(); };
                //viewPurchaseOrderViewModel.RequestClose += handle;
                //viewPurchaseOrderView.DataContext = viewPurchaseOrderViewModel;
                //viewPurchaseOrderView.ShowDialog();



                GeosApplication.Instance.Logger.Log("Method RefreshPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintPurchaseOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);

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

                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportPurchaseOrderList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Purchase Order List";
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

                    GeosApplication.Instance.Logger.Log("Method ExportPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();

                documentViewModel.OpenPdf(SelectedPurchaseOrder, obj);
                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void LeadsShowFilterValue(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LeadsShowFilterValue....", category: Category.Info, priority: Priority.Low);

                if (Listofitem.Count > 0)
                {
                    string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                    string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                    CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;

                    if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                        return;

                    if (str == null)
                    {
                        if (!string.IsNullOrEmpty(_FilterString))
                            MyFilterString = _FilterString;
                        else
                            MyFilterString = string.Empty;
                    }
                    else if (str.Equals("All"))
                    {
                        ListPurchaseOrder = new ObservableCollection<WarehousePurchaseOrder>(MainPurchaseOrderList);
                        SelectedPurchaseOrder = ListPurchaseOrder.FirstOrDefault();
                        MyFilterString = string.Empty;
                    }
                    else
                    {
                        MyFilterString = "[WorkflowStatus.Name] In ('" + str + "')";
                    }
                }

                GeosApplication.Instance.Logger.Log("Method LeadsShowFilterValue....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LeadsShowFilterValue...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

                if (viewPurchaseOrderViewModel.IsSaveChanges == true)
                {
                    wpo.IdWorkflowStatus = viewPurchaseOrderViewModel.WorkflowStatus.IdWorkflowStatus;
                    wpo.WorkflowStatus = viewPurchaseOrderViewModel.WorkflowStatus;

                    TileBarArrange(MainPurchaseOrderList);
                    AddCustomSetting();
                    MyFilterString = string.Empty;
                    SelectedPurchaseOrder = ListPurchaseOrder.Where(x => x.IdWarehousePurchaseOrder == wpo.IdWarehousePurchaseOrder).First();
                    if (!viewPurchaseOrderViewModel.IsMoreNeeded)
                    {
                        var item = ListPurchaseOrder.Where(x => x.IdWarehousePurchaseOrder == wpo.IdWarehousePurchaseOrder).First();
                        ListPurchaseOrder.Remove(item);
                        ListPurchaseOrder = new ObservableCollection<WarehousePurchaseOrder>(ListPurchaseOrder);
                    }
                    #region GEOS2-3986
                    // shubham[skadam] GEOS2-3986 Not change the color after change status from view purchase order window  21 OCT 2022
                    WarehousePurchaseOrder WarehousePurchaseOrder = ListPurchaseOrder.Where(w => w.IdWarehousePurchaseOrder == wpo.IdWarehousePurchaseOrder).FirstOrDefault();
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
            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction...", category: Category.Info, priority: Priority.Low);
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
            FillMainPurchaseOrderList();

            //rearrange tile Arrange
            TileBarArrange(MainPurchaseOrderList);
            AddCustomSetting();
            MyFilterString = string.Empty;

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method WarehousePopupClosedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        }

        private void TileBarArrange(List<WarehousePurchaseOrder> MainPurchaseOrderList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TileBarArrange...", category: Category.Info, priority: Priority.Low);

                Listofitem = new ObservableCollection<TileBarFilters>();


                Listofitem.Add(new TileBarFilters()
                {
                    Caption = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
                    DisplayText = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
                    EntitiesCount = MainPurchaseOrderList.Count(),
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                    //ImageUri = "AllTasks.png"
                });
                if (MainPurchaseOrderList == null)
                {
                    MainPurchaseOrderList = new List<WarehousePurchaseOrder>();
                }

                //[pramod.misal][GEOS2-4448][03-07-2023]
                //List<WorkflowStatus> StatusList = MainPurchaseOrderList.Where(a => a.WorkflowStatus != null).Select(b => b.WorkflowStatus).ToList();
                //StatusList.OrderBy(x => x.SortOrder);

                List<WorkflowStatus> StatusList = MainPurchaseOrderList.Where(a => a.WorkflowStatus != null).Select(b => b.WorkflowStatus).ToList();
                StatusList = StatusList.OrderBy(x => x.SortOrder).ToList();


                //foreach (var item in Listofitem)
                //{
                //    item.SortOrder = WorkflowStatusList.FirstOrDefault(s => s.Name == item.DisplayText).SortOrder;
                //}

                if (StatusList != null)
                {
                    foreach (WorkflowStatus Status in StatusList)
                    {


                        if (!Listofitem.Any(a => a.DisplayText == Status.Name))
                        {
                            Listofitem.Add(new TileBarFilters()
                            {
                                Caption = Status.Name,
                                DisplayText = Status.Name,
                                EntitiesCount = MainPurchaseOrderList.Count(x => x.IdWorkflowStatus == Status.IdWorkflowStatus),
                                BackColor = Status.HtmlColor,
                                ForeColor = Status.HtmlColor,
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 80,
                                width = 200
                            });


                        }

                    }
                }

                //if (WorkflowStatusList != null && WorkflowStatusList.Any())
                //{
                //    foreach (var item in Listofitem)
                //    {
                //        var workflowStatus = WorkflowStatusList.FirstOrDefault(s => s.Name == item.DisplayText);
                //        if (workflowStatus != null)
                //        {
                //            item.SortOrder = workflowStatus.SortOrder;
                //        }
                //    }
                //}


                Listofitem.Add(new TileBarFilters()
                {


                    Caption = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });

                // After change index it will automatically redirect to method LeadsShowFilterValue(object obj) and arrange the tile section count.
                if (Listofitem.Count > 0)
                    SelectedTileIndex = 0;
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TileBarArrange() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region old method
        //private void TileBarArrange(List<WarehousePurchaseOrder> MainPurchaseOrderList)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method TileBarArrange...", category: Category.Info, priority: Priority.Low);

        //        Listofitem = new ObservableCollection<TileBarFilters>();


        //        Listofitem.Add(new TileBarFilters()
        //        {
        //            Caption = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
        //            DisplayText = string.Format(System.Windows.Application.Current.FindResource("AllPendingPurchaseOrder").ToString()),
        //            EntitiesCount = MainPurchaseOrderList.Count(),
        //            EntitiesCountVisibility = Visibility.Visible,
        //            Height = 80,
        //            width = 200
        //            //ImageUri = "AllTasks.png"
        //        });
        //        if (MainPurchaseOrderList == null)
        //        {
        //            MainPurchaseOrderList = new List<WarehousePurchaseOrder>();
        //        }

        //        // List<WorkflowStatus> StatusList = MainPurchaseOrderList.Where(a => a.WorkflowStatus != null).Select(b => b.WorkflowStatus).ToList();

        //        //List<WorkflowStatus> StatusList = MainPurchaseOrderList
        //        //.Where(a => a.WorkflowStatus != null)
        //        //.OrderBy(b => b.WorkflowStatus)
        //        //.Select(b => b.WorkflowStatus)
        //        //.ToList();


        //        List<WorkflowStatus> StatusList = MainPurchaseOrderList
        //         .Where(a => a.WorkflowStatus != null)
        //         .Select(b => b.WorkflowStatus)
        //         .OrderBy(b => b.IdWorkflowStatus)
        //         .ToList();


        //        if (StatusList != null)
        //        {
        //            foreach (WorkflowStatus Status in StatusList)
        //            {
        //                if (!Listofitem.Any(a => a.DisplayText == Status.Name))
        //                {
        //                    Listofitem.Add(new TileBarFilters()
        //                    {
        //                        Caption = Status.Name,
        //                        DisplayText = Status.Name,
        //                        EntitiesCount = MainPurchaseOrderList.Count(x => x.IdWorkflowStatus == Status.IdWorkflowStatus),
        //                        BackColor = Status.HtmlColor,
        //                        ForeColor = Status.HtmlColor,
        //                        EntitiesCountVisibility = Visibility.Visible,
        //                        Height = 80,
        //                        width = 200
        //                    });
        //                }
        //            }
        //        }

        //        //Listofitem.Add(new TileBarFilters()
        //        //{
        //        //    Caption = string.Format(System.Windows.Application.Current.FindResource("OnlyPendingPurchaseOrder").ToString()),
        //        //    DisplayText = string.Format(System.Windows.Application.Current.FindResource("OnlyPendingPurchaseOrder").ToString()),
        //        //    EntitiesCount = MainPurchaseOrderList.Count(x => !x.IsPartialPending),
        //        //    ImageUri = "NotStarted.png"
        //        //});

        //        Listofitem.Add(new TileBarFilters()
        //        {


        //            Caption = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
        //            Id = 0,
        //            BackColor = null,
        //            ForeColor = null,
        //            EntitiesCountVisibility = Visibility.Collapsed,
        //            Height = 30,
        //            width = 200,
        //        });

        //        // After change index it will automatically redirect to method LeadsShowFilterValue(object obj) and arrange the tile section count.
        //        if (Listofitem.Count > 0)
        //            SelectedTileIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in TileBarArrange() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        #endregion old method
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            try
            {
                if (e.Value != null && e.Value.ToString() != "Status" && e.ColumnFieldName == "IdWorkflowStatus")
                {
                    e.Value = ListPurchaseOrder.First().LstWorkflowStatus.FirstOrDefault(a => a.IdWorkflowStatus == Convert.ToInt32(e.Value)).Name;
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
        //[001][cpatil][30-10-2023][GEOS2-4902]
        //[002][cpatil][30-04-2024][GEOS2-5618]
        private void FillMainPurchaseOrderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList...", category: Category.Info, priority: Priority.Low);

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

                        MainPurchaseOrderList = new List<WarehousePurchaseOrder>();

                        foreach (var item in plantOwners)
                        {
                            plantOwnersIds = item.IdWarehouse;
                            plantconnection = item.Company.ConnectPlantConstr;
                            #region Service Comments
                            //  TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetPendingPurchaseOrdersByWarehouse_V2390(plantOwnersIds, plantconnection));
                            // Warehouses Warehouse1 = SRM.SRMCommon.Instance.Selectedwarehouse;
                            // var TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetPendingPurchaseOrdersByWarehouse_V2310(Warehouse1));
                            // var TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(WarehouseService.GetPurchaseOrdersPendingReceptionByWarehouse_V2035(Warehouse1));
                            //  var TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetPendingPurchaseOrdersByWarehouse_V2390(plantOwnersIds, plantconnection));
                            //[Sudhir.Jangra][GEOS2-4487][31/05/2023] Updated SP version Wise
                            //Updated some code in V2400 Service by [rdixit][GEOS2-4455][19.06.2023]

                            // TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetPendingPurchaseOrdersByWarehouse_V2400(plantOwnersIds, plantconnection));

                            //[pramod.misal][GEOS2-4448][30-06-2023]

                            //var selectedwarehouse = SRMCommon.Instance.WarehouseList.FirstOrDefault(i => i.IdWarehouse == item.IdWarehouse);
                            //SRMService = new SRMServiceController((selectedwarehouse != null && selectedwarehouse.Company.ServiceProviderUrl != null) ? selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            //TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetPendingPurchaseOrdersByWarehouse_V2410(plantOwnersIds, plantconnection));
                            //Shubham[skadam]  GEOS2-4713 Missing Delivery Dates 31 07 2023 
                            #endregion
                            // SRMService = new SRMServiceController("localhost:6699");
                            // SRMService = new SRMServiceController((item != null && item.Company.ServiceProviderUrl != null) ? item.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            //[001]Changed service method version wise
                            //[002]Changed service method version wise
                            //TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetPendingPurchaseOrdersByWarehouse_V2510(plantOwnersIds, plantconnection));
                            //SRMService = new SRMServiceController("localhost:6699");
                            //TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetPendingPurchaseOrdersByWarehouse_V2620(plantOwnersIds, plantconnection)); // [pallavi.kale][04-03-2025][GEOS2-7012]
                            TempMainPurchaseOrderList = new ObservableCollection<WarehousePurchaseOrder>(SRMService.GetPendingPurchaseOrdersByWarehouse_V2650(plantOwnersIds, plantconnection));//Shubham[skadam] GEOS2-8262 PO reminder Mail send to whom 29 05 2025
                            if (TempMainPurchaseOrderList != null)
                                TempMainPurchaseOrderList.ToList().ForEach(i => i.Warehouse = item);

                            #region[rdixit][28.06.2023][Images now taken from API urls]
                            //if (TempMainPurchaseOrderList != null)
                            //{
                            //    foreach (var otitem in TempMainPurchaseOrderList.GroupBy(tpa => tpa.ArticleSupplier.Country.Iso))
                            //    {
                            //        ImageSource countryFlagImage = ByteArrayToBitmapImage(otitem.ToList().FirstOrDefault().ArticleSupplier.Country.CountryIconBytes);
                            //        otitem.ToList().Where(oti => oti.ArticleSupplier.Country.Iso == otitem.Key).ToList().ForEach(oti => oti.ArticleSupplier.Country.CountryIconImage = countryFlagImage);
                            //    }
                            //}
                            #endregion
                            MainPurchaseOrderList.AddRange(TempMainPurchaseOrderList);
                        }

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    MainPurchaseOrderList = new List<WarehousePurchaseOrder>(MainPurchaseOrderList);
                }
                else
                {
                    MainPurchaseOrderList = new List<WarehousePurchaseOrder>();
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillMainPurchaseOrderList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainPurchaseOrderList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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


        private void CommandTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                List<WorkflowStatus> StatusList = MainPurchaseOrderList.Where(a => a.WorkflowStatus != null).Select(b => b.WorkflowStatus).ToList();
                if (StatusList != null)
                {
                    foreach (var item in StatusList)
                    {
                        if (CustomFilterStringName != null)
                        {
                            if (CustomFilterStringName.Equals(item.Name))
                            {
                                return;
                            }
                        }
                    }
                }

                if (CustomFilterStringName == "CUSTOM FILTERS" || CustomFilterStringName == "All")
                {
                    return;
                }

                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Template"));
                IsEdit = true;
                table.ShowFilterEditor(column);
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();

                if (tempUserSettings != null && tempUserSettings.Count > 0)
                {
                    foreach (var item in tempUserSettings)
                    {
                        ExpressionEvaluator evaluator = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(WarehousePurchaseOrder)), item.Value, false);
                        List<WarehousePurchaseOrder> tempList = new List<WarehousePurchaseOrder>();

                        foreach (var po in MainPurchaseOrderList)
                        {
                            if (evaluator.Fit(po))
                                tempList.Add(po);
                        }

                        MyFilterString = item.Value;
                        Listofitem.Add(new TileBarFilters()
                        {
                            Caption = item.Key.Replace(userSettingsKey, ""),
                            Id = 0,
                            BackColor = null,
                            ForeColor = null,
                            FilterCriteria = item.Value,
                            EntitiesCount = tempList.Count,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200
                        });
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TableView table = (TableView)obj.OriginalSource;
            GridControl gridControl = (table).Grid;
            ShowFilterEditor(obj);
        }

        public void UpdateMultipleRowsPendingPurchaseGridCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsPendingPurchaseGridCommandAction ...", category: Category.Info, priority: Priority.Low);

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

                view = obj as TableView;
                GridControl gridControl = (view).Grid;
                ObservableCollection<object> selectedRows = (ObservableCollection<object>)view.SelectedRows;
                ObservableCollection<WarehousePurchaseOrder> LstWarehousePO = (ObservableCollection<WarehousePurchaseOrder>)(((GridControl)view.DataControl)).ItemsSource;
                IsPurchaseOrderSave = false;
                IsAllSave = false;

                int? cellIdWorkflowStatus = null;

                //[rdixit][GEOS2-4353][16.06.2023]
                cellIdWorkflowStatus = WorkflowStatusList.Where(sl => sl.IdWorkflowStatus == SRMPurchaseOrderCellEditHelper.SelectedWorkflowStatus.IdWorkflowStatus).Select(u => u.IdWorkflowStatus).FirstOrDefault();

                AssigneeList = new ObservableCollection<User>(SRMService.GetPermissionUsers());
                ApproverList = new ObservableCollection<User>(AssigneeList);
                //WarehousePurchaseOrder[] foundRow = MainPurchaseOrderList.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                WarehousePurchaseOrder[] foundRow = LstWarehousePO.AsEnumerable().Where(row => row.IsUpdatedRow == true).ToArray();
                IsPurchaseOrderSave = true;
                int count = 0;
                string POReportGenerated = string.Empty;
                foreach (WarehousePurchaseOrder item in foundRow)
                {
                    //SRMService = new SRMServiceController("localhost:6699");
                    SRMService = new SRMServiceController((item.Warehouse != null && item.Warehouse.Company.ServiceProviderUrl != null) ? item.Warehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    WarehousePurchaseOrder AI = item;
                    ChangeLogsEntry = new List<LogEntriesByWarehousePO>();
                    count = count + 1;

                    WarehousePurchaseOrder _WarehousePurchaseOrder = new WarehousePurchaseOrder();
                    _WarehousePurchaseOrder.IdWarehousePurchaseOrder = item.IdWarehousePurchaseOrder;
                    _WarehousePurchaseOrder.IdWorkflowStatus = item.IdWorkflowStatus;
                    _WarehousePurchaseOrder.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    _WarehousePurchaseOrder.WorkflowStatus = WorkflowStatusList.Where(sl => sl.IdWorkflowStatus == _WarehousePurchaseOrder.IdWorkflowStatus).FirstOrDefault();
                    _WarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>();

                    WarehousePurchaseOrder TempArticleList = new WarehousePurchaseOrder();
                    TempArticleList = SRMService.GetPendingPODetailsForGridUpdate(_WarehousePurchaseOrder.IdWarehousePurchaseOrder);

                    _WarehousePurchaseOrder.IdAssignee = TempArticleList.IdAssignee;
                    _WarehousePurchaseOrder.IdApprover = TempArticleList.IdApprover;


                    if (AI.IdWorkflowStatus != TempArticleList.IdWorkflowStatus)
                    {
                        List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList = new List<LogEntriesByWarehousePO>();
                        LogEntriesByWarehousePO logEntriesByWarehousePO = new LogEntriesByWarehousePO();
                        logEntriesByWarehousePO.IdWarehousePurchaseOrder = _WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                        logEntriesByWarehousePO.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                        //logEntriesByWarehousePO.Comments = string.Format(System.Windows.Application.Current.FindResource("WorkflowStatusLogEntryByPO").ToString(), AI.WorkflowStatus.Name, _WarehousePurchaseOrder.WorkflowStatus.Name);
                        logEntriesByWarehousePO.Comments = string.Format(System.Windows.Application.Current.FindResource("WorkflowStatusLogEntryByPO").ToString(), TempArticleList.WorkflowStatus.Name, _WarehousePurchaseOrder.WorkflowStatus.Name);
                        logEntriesByWarehousePO.IdEntryType = 258;
                        logEntriesByWarehousePO.IsRtfText = false;
                        logEntriesByWarehousePO.Datetime = GeosApplication.Instance.ServerDateTime;
                        logEntriesByWarehousePO.People = new People();
                        logEntriesByWarehousePO.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                        logEntriesByWarehousePO.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                        logEntriesByWarehousePO.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                        LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO);

                        #region Prev Code_ Now Assignee and Approver will save on accept button click event
                        //if (TempArticleList.IdAssignee != GeosApplication.Instance.ActiveUser.IdUser)
                        //{
                        //    LogEntriesByWarehousePO logEntriesByWarehousePO_Assignee = new LogEntriesByWarehousePO();
                        //    logEntriesByWarehousePO_Assignee.IdWarehousePurchaseOrder = _WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                        //    logEntriesByWarehousePO_Assignee.IdUser = GeosApplication.Instance.ActiveUser.IdUser;

                        //    if (TempArticleList.IdAssignee == null)
                        //        logEntriesByWarehousePO_Assignee.Comments = string.Format(System.Windows.Application.Current.FindResource("AssigneeLogEntryByPO").ToString(), "None", GeosApplication.Instance.ActiveUser.FullName);
                        //    else
                        //        logEntriesByWarehousePO_Assignee.Comments = string.Format(System.Windows.Application.Current.FindResource("AssigneeLogEntryByPO").ToString(), AssigneeList.FirstOrDefault(a => a.IdUser == TempArticleList.IdAssignee).FullName, GeosApplication.Instance.ActiveUser.FullName);

                        //    logEntriesByWarehousePO_Assignee.IdLogEntryType = 253;
                        //    logEntriesByWarehousePO_Assignee.IsRtfText = false;
                        //    logEntriesByWarehousePO_Assignee.Datetime = GeosApplication.Instance.ServerDateTime;
                        //    logEntriesByWarehousePO_Assignee.People = new People();
                        //    logEntriesByWarehousePO_Assignee.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                        //    logEntriesByWarehousePO_Assignee.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                        //    logEntriesByWarehousePO_Assignee.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                        //    LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO_Assignee);
                        //    _WarehousePurchaseOrder.IdAssignee = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                        //}

                        //if (TempArticleList.IdApprover != GeosApplication.Instance.ActiveUser.IdUser)
                        //{
                        //    LogEntriesByWarehousePO logEntriesByWarehousePO_Approver = new LogEntriesByWarehousePO();
                        //    logEntriesByWarehousePO_Approver.IdWarehousePurchaseOrder = _WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                        //    logEntriesByWarehousePO_Approver.IdUser = GeosApplication.Instance.ActiveUser.IdUser;

                        //    if (TempArticleList.IdApprover == null)
                        //        logEntriesByWarehousePO_Approver.Comments = string.Format(System.Windows.Application.Current.FindResource("ApproverLogEntryByPO").ToString(), "None", GeosApplication.Instance.ActiveUser.FullName);
                        //    else
                        //        logEntriesByWarehousePO_Approver.Comments = string.Format(System.Windows.Application.Current.FindResource("ApproverLogEntryByPO").ToString(), ApproverList.FirstOrDefault(a => a.IdUser == TempArticleList.IdApprover).FullName, GeosApplication.Instance.ActiveUser.FullName);

                        //    logEntriesByWarehousePO_Approver.IdLogEntryType = 253;
                        //    logEntriesByWarehousePO_Approver.IsRtfText = false;
                        //    logEntriesByWarehousePO_Approver.Datetime = GeosApplication.Instance.ServerDateTime;
                        //    logEntriesByWarehousePO_Approver.People = new People();
                        //    logEntriesByWarehousePO_Approver.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                        //    logEntriesByWarehousePO_Approver.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                        //    logEntriesByWarehousePO_Approver.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                        //    LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO_Approver);

                        //    _WarehousePurchaseOrder.IdApprover = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);


                        //}
                        #endregion
                        if (LogEntriesByWarehousePOList.Count > 0)
                        {
                            _WarehousePurchaseOrder.WarehousePOLogEntries.AddRange(LogEntriesByWarehousePOList);
                        }

                    }
                    //Shubham[skadam] GEOS2-5026 Log not added properly in purchase order 08 11 2023
                    IsPurchaseOrderSave = SRMService.IsUpdateWarehousePurchaseOrderWithStatus_V2450(_WarehousePurchaseOrder);
                    if (count == 1)
                    {
                        WarehousePurchaseOrderForSendPOEmail = _WarehousePurchaseOrder;
                        SendPOEmailNotification(item.IdWorkflowStatus, TempArticleList.WorkflowStatus.IdWorkflowStatus);
                    }
                    WarehousePurchaseOrder WarehousePurchaseOrder = ListPurchaseOrder.Where(w => w.IdWarehousePurchaseOrder == _WarehousePurchaseOrder.IdWarehousePurchaseOrder).FirstOrDefault();
                    //[GEOS2-4453][24.07.2023][rdixit]
                    #region Added code to Generate report when status is changed to Aprroved and regenerate if it exist.
                    if (item.IdWorkflowStatus == 4)
                    {
                        try
                        {
                            // Open PDF in another window
                            DocumentView documentView = new DocumentView();
                            DocumentViewModel documentViewModel = new DocumentViewModel();

                            documentViewModel.GeneratePurchaseOrderReport(item);
                            if (documentViewModel.IsGenerated)
                            {
                                if (string.IsNullOrEmpty(POReportGenerated))
                                    POReportGenerated = item.Code;
                                else
                                    POReportGenerated = POReportGenerated + ", " + item.Code;
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Error in Method UpdateMultipleRowsArticleGridCommandAction() Export Report Code." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    #endregion
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
                    item.IsUpdatedRow = false;
                }


                if (IsPurchaseOrderSave)
                {
                    IsAllSave = true;
                    SRMPurchaseOrderCellEditHelper.SetIsValueChanged(view, false);
                    SRMPurchaseOrderCellEditHelper.IsValueChanged = false;
                    gridControl.RefreshData();
                    gridControl.UpdateLayout();
                    //Added code to show proper status count in left tiles after changes on grid [rdixit][05.07.2023]
                    FillMainPurchaseOrderList();
                    TileBarArrange(MainPurchaseOrderList);
                    ListPurchaseOrder = new ObservableCollection<WarehousePurchaseOrder>(MainPurchaseOrderList);
                }
                else
                    IsAllSave = false;

                if (IsAllSave == null)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }
                else if (IsAllSave.Value == true)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMWarehousePurchaseOrderUpdate").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                else if (IsAllSave.Value == false)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMWarehousePurchaseOrderUpdateUpdatedFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                ProductTypeArticleViewMultipleCellEditHelper.SetIsValueChanged(view, false);
                ProductTypeArticleViewMultipleCellEditHelper.IsValueChanged = false;


                GeosApplication.Instance.Logger.Log("Method UpdateMultipleRowsArticleGridCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsArticleGridCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateMultipleRowsArticleGridCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method UpdateMultipleRowsArticleGridCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomFilterEditorView customFilterEditorView = new CustomFilterEditorView();
                CustomFilterEditorViewModel customFilterEditorViewModel = new CustomFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    customFilterEditorViewModel.FilterName = CustomFilterStringName;
                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;

                customFilterEditorViewModel.Init(e.FilterControl, Listofitem);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        Listofitem.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");

                            if (!key.Equals(tileBarItem.Caption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = ((DevExpress.Xpf.Grid.GridViewBase)e.OriginalSource).Grid.VisibleRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");

                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKey + tileBarItem.Caption), tileBarItem.FilterCriteria));

                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    Listofitem.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = ((DevExpress.Xpf.Grid.GridViewBase)e.OriginalSource).Grid.VisibleRowCount
                    });

                    string filterName = "";

                    filterName = userSettingsKey + customFilterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedFilter = Listofitem.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void SendEmailCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction()...", category: Category.Info, priority: Priority.Low);
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
                List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                List<WarehousePurchaseOrder> SupplierPOList = new List<Data.Common.WarehousePurchaseOrder>();
                Warehouses warehouses = new Warehouses(); ;

                List<WarehousePurchaseOrder> ListPurchaseOrder_Checked = new List<WarehousePurchaseOrder>(ListPurchaseOrder.Where(a => a.IsSendMail == true));
                int count = 0;
                if (ListPurchaseOrder_Checked.Count > 0)
                {
                    List<KeyValuePair<string, string>> MailWithBody = new List<KeyValuePair<string, string>>();
                    foreach (var item in ListPurchaseOrder_Checked)
                    {
                        List<WarehousePurchaseOrder> WPOList = new List<Data.Common.WarehousePurchaseOrder>();
                        WPOList.Add(item);
                        SupplierPOList.Add(item);
                        //SRMService = new SRMServiceController("localhost:6699");
                         SRMService = new SRMServiceController((item.Warehouse != null && item.Warehouse.Company.ServiceProviderUrl != null) ? item.Warehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        //MailWithBody.AddRange(SRMService.GetBodyAndUpdateReminderDateInPO(SRM.SRMCommon.Instance.Selectedwarehouse, WPOList, MainPurchaseOrderList.FirstOrDefault().SupplierEmailId, GeosApplication.Instance.ActiveUser.IdUser));
                        try
                        {
                            //SRMService = new SRMServiceController("localhost:6699");
                            warehouses=plantOwners.FirstOrDefault(f => f.Name.ToLower().Equals(item.WarehouseName.ToLower()));
                            warehouses = !string.IsNullOrEmpty(warehouses.Name) ? warehouses : SRM.SRMCommon.Instance.Selectedwarehouse;
                            Dictionary<string, List<long>> SupplierEmailId = new Dictionary<string, List<long>>();
                            try
                            {
                                var nonNullEmailList = MainPurchaseOrderList .Where(w => w.SupplierEmailId != null).ToList();
                                foreach (var po in nonNullEmailList)
                                {
                                    //Shubham[skadam] GEOS2-8262 PO reminder Mail send to whom 29 05 2025
                                    MailWithBody.AddRange(SRMService.GetBodyAndUpdateReminderDateInPO_V2650(warehouses, WPOList, po.SupplierEmailId, GeosApplication.Instance.ActiveUser.IdUser));
                                }
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in method SendEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method SendEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    ListPurchaseOrder_Checked.ForEach(x => x.ArticleSupplier.Country.CountryIconImage = null);

                    //List<KeyValuePair<string, string>> MailWithBody = 
                    if (MailWithBody.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> val in MailWithBody)
                        {
                            count++;
                            CreateOutlookEmail(val.Key, "EMDEP Delayed Purchase Orders[REMINDER]", val.Value);
                        }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierReminderMail_Success").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    /*
                    if (MainPurchaseOrderList.Count > 0 && MainPurchaseOrderList.FirstOrDefault().Suppliername.Count > 0)
                    {
                        string message = string.Join(",", MainPurchaseOrderList.FirstOrDefault().Suppliername.Select(a => a));
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierReminderMail_Info").ToString(), message), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    }
                    */
                    try
                    {
                        string message = string.Empty;
                        List<string> missingEmailSuppliers = new List<string>();
                        if (MainPurchaseOrderList.Count > 0 && MainPurchaseOrderList.FirstOrDefault().Suppliername.Count > 0)
                        {
                            foreach (var item in SupplierPOList)
                            {
                                if (string.IsNullOrEmpty(item.ArticleSupplier.ContactEmail))
                                {
                                    missingEmailSuppliers.Add(item.ArticleSupplier?.Name);
                                }
                            }
                            message = string.Join(", ", missingEmailSuppliers);
                        }
                        if (!string.IsNullOrEmpty(message))
                        {
                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierReminderMail_Info").ToString(), message), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in method SendEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }

                    if (count == 0)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierReminderMail_NotSend").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierReminderMail_NotChecked").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SendEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CreateOutlookEmail(string to, string subject, string body)
        {
            try
            {
                OutlookApp objApp = new OutlookApp();
                Microsoft.Office.Interop.Outlook.MailItem mailItem = null;
                mailItem = (Microsoft.Office.Interop.Outlook.MailItem)objApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                mailItem.Subject = subject;

                mailItem.To = to;
                mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                mailItem.HTMLBody = body;
                mailItem.Importance = Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh;
                mailItem.Send();
                //mailItem.Display(false);
            }
            catch (SmtpException smtpEx)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SendPOEmailCommandAction() - {0}", smtpEx.Message), category: Category.Exception, priority: Priority.Low);
                if (smtpEx.InnerException != null)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method SendPOEmailCommandAction() - {0}", smtpEx.InnerException.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SendPOEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (ex.InnerException != null)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method SendPOEmailCommandAction() - {0}", ex.InnerException.Message), category: Category.Exception, priority: Priority.Low);
                }               
            }
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
                        foreach (var item in ListPurchaseOrder.First().LstWorkflowStatus)
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
        //[GEOS2-4309][rdixit][10.04.2023]
        private void SendPOEmailCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true; IsMailSend = false;
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
                List<WarehousePurchaseOrder> ListPurchaseOrder_Checked = new List<WarehousePurchaseOrder>(ListPurchaseOrder.Where(a => a.IsSendMail == true));
                int count = 0;
                var temp = SRMService.GetSystemSettings_V2390();
                SystemSettingsList = temp.FirstOrDefault().SystemSettings;
                string senderEmail = GetEmailFrom();
                if (ListPurchaseOrder_Checked.Count > 0)
                {
                    try
                    {
                        ListPurchaseOrder_Checked.ForEach(x => x.ArticleSupplier.Country.CountryIconImage = null);//added code by rdixit to assign countries images for selected po 28.06.2023

                        List<WarehousePurchaseOrder> WarehousePurchaseOrder = new List<Data.Common.WarehousePurchaseOrder>();
                        foreach (var item in ListPurchaseOrder_Checked)
                        {
                            List<WarehousePurchaseOrder> WPOList = new List<Data.Common.WarehousePurchaseOrder>();
                            WPOList.Add(item);
                           //SRMService = new SRMServiceController("localhost:6699");
                           SRMService = new SRMServiceController((item.Warehouse != null && item.Warehouse.Company.ServiceProviderUrl != null) ? item.Warehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            // WarehousePurchaseOrder.AddRange(SRMService.SendPOMailForSelectedPO_V2390(WPOList));
                            //WarehousePurchaseOrder.AddRange(SRMService.SendPOMailForSelectedPO_V2520(WPOList));
                            WarehousePurchaseOrder.AddRange(SRMService.SendPOMailForSelectedPO_V2660(WPOList));
                        }
                        foreach (WarehousePurchaseOrder item in WarehousePurchaseOrder)
                        {
                            ToContactList = new ObservableCollection<Contacts>();
                            CCContactList = new ObservableCollection<Contacts>();
                            //SRMService = new SRMServiceController("localhost:6699");
                            SRMService = new SRMServiceController((item.Warehouse != null && item.Warehouse.Company.ServiceProviderUrl != null) ? item.Warehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            count = count + 1;
                            if (count != 1)
                            {
                                IsSend = true;
                            }
                            if (!(string.IsNullOrEmpty(item.ArticleSupplier.ContactEmail) && string.IsNullOrEmpty(item.ArticleSupplier.ContactPerson)))
                            {
                                #region GEOS-4077 Sudhir.Jangra 10/05/2023
                                //if (temp != null)
                                //{
                                //    List<Warehouses> plantOwner = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                                //    var selectedCC = SystemSettingsList.FirstOrDefault(x => x.Warehouse == plantOwner.FirstOrDefault().Name).CC;
                                //    string differentEmails = string.Join(";", plantOwner.SelectMany(selected =>
                                //    {
                                //        var selectedWarehouseName = selected.Name;
                                //        var selectedWarehouse = SystemSettingsList.Where(x => x.Warehouse == selectedWarehouseName);
                                //        return selectedWarehouse.Where(systemSetting => item.AssigneeEmail != senderEmail || item.CreatorEmail != senderEmail).Select(systemSetting =>
                                //        item.AssigneeEmail != senderEmail && item.CreatorEmail != senderEmail ? $"{item.AssigneeEmail};{item.CreatorEmail}" :
                                //                (item.AssigneeEmail != senderEmail ? item.AssigneeEmail : item.CreatorEmail));
                                //    }));
                                //    var distinctEmails = differentEmails.Split(';').Distinct();
                                //    var ccEmail = selectedCC.Split(',');
                                //    foreach (var email in distinctEmails)
                                //    {
                                //        if (!ccEmail.Contains(email))
                                //        {
                                //            ccEmail = ccEmail.Concat(new[] { email }).ToArray();
                                //        }
                                //    }
                                //    EmailCC = string.Empty;
                                //    EmailCC += string.Join(";", ccEmail);
                                //    //EmailCC = "rajashri.telvekar@emdep.com";
                                //}
                                #endregion

                                #region Updated above code by rdixit 13.05.2024 Incorrect cc email
                                try
                                {
                                    var plantOwner = SRMCommon.Instance.SelectedAuthorizedWarehouseList
                                        .Cast<Warehouses>()?.FirstOrDefault(i => i.IdWarehouse == item.IdWarehouse);

                                    GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction() Selected Warehouse..." + plantOwner?.IdWarehouse, category: Category.Info, priority: Priority.Low);
                                    var selectedCC = SystemSettingsList.FirstOrDefault(x => x.Warehouse == plantOwner?.Name)?.CC ?? "";
                                    GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction() selectedCC..." + selectedCC, category: Category.Info, priority: Priority.Low);

                                    var recipients = (item.AssigneeEmail != senderEmail && item.CreatorEmail != senderEmail)
                                        ? $"{item.AssigneeEmail};{item.CreatorEmail}":(item.AssigneeEmail != senderEmail ? item.AssigneeEmail : item.CreatorEmail);

                                    GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction() AssigneeEmail..." + item.AssigneeEmail, category: Category.Info, priority: Priority.Low);

                                    GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction() CreatorEmail..." + item.CreatorEmail, category: Category.Info, priority: Priority.Low);

                                    GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction() recipients..." + recipients, category: Category.Info, priority: Priority.Low);

                                    List<string> distinctEmails = recipients?.Split(';').Distinct()?.ToList();

                                    if(distinctEmails?.Count>0)
                                    GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction() distinctEmails..." + string.Join(",", distinctEmails), category: Category.Info, priority: Priority.Low);

                                    var ccEmail = new HashSet<string>(selectedCC != null ? selectedCC.Split(',') : new string[0]);

                                    if (ccEmail?.Count>0)
                                    GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction() ccEmail..." + string.Join(",", ccEmail), category: Category.Info, priority: Priority.Low);
                                    if (distinctEmails != null)
                                    {
                                        foreach (var email in distinctEmails)
                                        {
                                            if (!string.IsNullOrEmpty(email))
                                                ccEmail.Add(email.Trim());
                                        }
                                    }
                                    EmailCC = string.Join(";", ccEmail);
                                    GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction() EmailCC..." + EmailCC, category: Category.Info, priority: Priority.Low);
                                }
                                catch (Exception ex)
                                {
                                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                                    GeosApplication.Instance.Logger.Log(string.Format("Error in method SendPOEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                }
                                #endregion

                                GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction()...", category: Category.Info, priority: Priority.Low);
                                if (item.EmailBody != null && item.AttachmentBytes != null)
                                {
                                    CreateOutlookEmailForPO(item, senderEmail);
                                    IsMailSend = true;
                                }
                                else
                                {
                                    if (item.EmailBody == null || item.EmailBody == "")
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_failure1").ToString(), item.Code), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }
                                    else if (item.AttachmentBytes == null)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_failure").ToString(), item.Code), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMFavSupplierMail_failureGrid").ToString(), item.Code), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                            if (IsMailSend)
                                GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction()....executed successfully for PO " + item.Code, category: Category.Info, priority: Priority.Low);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log(string.Format("Error in method SendPOEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }

                    try
                    {
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
                        FillMainPurchaseOrderList();
                        TileBarArrange(MainPurchaseOrderList);
                        ListPurchaseOrder = new ObservableCollection<WarehousePurchaseOrder>(MainPurchaseOrderList);
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in method SendPOEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    }
                    if (IsMailSend)
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_Success").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierReminderMail_NotChecked").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SendPOEmailCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SendPOEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-4309][rdixit][10.04.2023]
        private void CreateOutlookEmailForPO(WarehousePurchaseOrder WarehousePurchaseOrder, string senderEmail)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... Method Started", category: Category.Info, priority: Priority.Low);
                FillToCC(WarehousePurchaseOrder);
                string tempFilePath = Path.GetTempPath() + WarehousePurchaseOrder.Code + ".pdf";
                OutlookApp outlookApp = new OutlookApp();
                Microsoft.Office.Interop.Outlook.MailItem mailItem = outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                mailItem.Subject = "PO " + WarehousePurchaseOrder.Code;
                mailItem.HTMLBody = WarehousePurchaseOrder.EmailBody;
                GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... got HTMLBody", category: Category.Info, priority: Priority.Low);
                mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                using (MemoryStream ms = new MemoryStream(WarehousePurchaseOrder.AttachmentBytes))
                {
                    FileStream fs = new FileStream(tempFilePath, FileMode.Create);
                    ms.CopyTo(fs);
                    fs.Close();
                    mailItem.Attachments.Add(tempFilePath, Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue, 1, WarehousePurchaseOrder.Code + ".pdf");
                }
                GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... got AttachmentBytes", category: Category.Info, priority: Priority.Low);
                //Set a high priority to the message
                mailItem.Importance = Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh;

                List<string> toemails = new List<string>();

                if (!string.IsNullOrEmpty(WarehousePurchaseOrder.ArticleSupplier.ContactEmail))
                {
                    toemails.Add(WarehousePurchaseOrder.ArticleSupplier.ContactEmail);
                    GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... got ArticleSupplier ContactEmail" + WarehousePurchaseOrder.ArticleSupplier.ContactEmail, category: Category.Info, priority: Priority.Low);
                }

                if (ToContactList != null)
                {
                    foreach (Contacts item in ToContactList)
                    {
                        toemails.Add(item.Email);
                    }
                    GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... got ToContactList" + string.Join(",", toemails), category: Category.Info, priority: Priority.Low);
                }
                HashSet<string> distinctToEmails = new HashSet<string>(toemails);

                mailItem.To = string.Join(";", distinctToEmails.ToArray());
                GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... got To Email" + mailItem.To, category: Category.Info, priority: Priority.Low);
                //  mailItem.To =  WarehousePurchaseOrder.ArticleSupplier.ContactEmail;
                #region To take distinct CC emails
                List<string> ccemails = new List<string>();
                if (!string.IsNullOrEmpty(EmailCC))
                {
                    MatchCollection matches = Regex.Matches(EmailCC, @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b");
                    GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... got CC Email" + EmailCC, category: Category.Info, priority: Priority.Low);
                    foreach (Match match in matches)
                    {
                        ccemails.Add(match.Value);
                    }
                }
                //[Sudhir.Jangra][GEOS2-5493]
                if (CCContactList != null)
                {
                    foreach (Contacts item in CCContactList)
                    {
                        ccemails.Add(item.Email);
                    }
                    GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... got CContactList" + string.Join(",", CCContactList.Select(i=>i.Email)), category: Category.Info, priority: Priority.Low);
                }

                HashSet<string> distinctEmails = new HashSet<string>(ccemails);
                #endregion
                mailItem.CC = string.Join("; ", distinctEmails.ToArray());
                GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... CC emails in mail" + mailItem.CC, category: Category.Info, priority: Priority.Low);
                mailItem.Send();
                CCContactList = new ObservableCollection<Contacts>();
                ToContactList = new ObservableCollection<Contacts>();
                //[rdixit][28.06.2023]
                #region Mail send Code using service
                //Dictionary<string, byte[]> Attachment = new Dictionary<string, byte[]>();
                //Attachment.Add(WarehousePurchaseOrder.Code + ".pdf", WarehousePurchaseOrder.AttachmentBytes);
                //IsMailSend = SRMService.SendEmailForPO(WarehousePurchaseOrder.ArticleSupplier.ContactEmail, "PO " + WarehousePurchaseOrder.Code, WarehousePurchaseOrder.EmailBody, Attachment, senderEmail, distinctEmails.ToList());
                #endregion
                #region  GEOS2-4405 
                //Shubham[skadam]  GEOS2-4405 (Grid) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
                try
                {
                    if (ListPurchaseOrder.Any(a => a.IdWarehousePurchaseOrder == WarehousePurchaseOrder.IdWarehousePurchaseOrder))
                    {
                        WarehousePurchaseOrder warehousePurchaseOrder = ListPurchaseOrder.FirstOrDefault(a => a.IdWarehousePurchaseOrder == WarehousePurchaseOrder.IdWarehousePurchaseOrder);
                        if (warehousePurchaseOrder.LstWorkflowStatusTransition.Any(a => a.IdWorkflowStatus == 6))
                        {
                            //SRMService = new SRMServiceController("localhost:6699");
                            bool result = SRMService.UpdatePurchasingOrderStatus(WarehousePurchaseOrder.IdWarehousePurchaseOrder, GeosApplication.Instance.ActiveUser.IdUser);
                            //Shubham[skadam] GEOS2-4406 Send an email notification when the status of the PO changes 21 06 2023
                            WarehousePurchaseOrderForSendPOEmail = WarehousePurchaseOrder;
                            if (IsSend != true)
                            {
                                SendPOEmailNotification(SRMService.GetPurchasingOrderStatus(WarehousePurchaseOrder.IdWarehousePurchaseOrder), WarehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus);
                            }
                             TransitionWorkflowStatus(SRMService.GetPurchasingOrderStatus(WarehousePurchaseOrder.IdWarehousePurchaseOrder), WarehousePurchaseOrder);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion
                EmailCC = string.Empty;
                //mailItem.Display(false);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CreateOutlookEmailForPO() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CreateOutlookEmailForPO() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (SmtpException smtpEx)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CreateOutlookEmailForPO() - {0}", smtpEx.Message), category: Category.Exception, priority: Priority.Low);
                if (smtpEx.InnerException != null)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method CreateOutlookEmailForPO() - {0}", smtpEx.InnerException.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CreateOutlookEmailForPO() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (ex.InnerException != null)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method CreateOutlookEmailForPO() - {0}", ex.InnerException.Message), category: Category.Exception, priority: Priority.Low);
                }
            }           
        }

        //[GEOS2-4407][Sudhir.Jangra][19/05/2023]
        public string GetEmailFrom()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... Method Started", category: Category.Info, priority: Priority.Low);
                OutlookEmailFrom.Application outlookEmail = new OutlookEmailFrom.Application();
                OutlookEmailFrom.Account account = outlookEmail.Session.Accounts.Cast<Microsoft.Office.Interop.Outlook.Account>().FirstOrDefault();

                if (account != null)
                {
                    GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... From Emails Address " + account.SmtpAddress, category: Category.Info, priority: Priority.Low);
                    return account.SmtpAddress;
                }
                GeosApplication.Instance.Logger.Log("Method CreateOutlookEmailForPO()... Method Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (SmtpException smtpEx)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method GetEmailFrom() - {0}", smtpEx.Message), category: Category.Exception, priority: Priority.Low);
                if (smtpEx.InnerException != null)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method GetEmailFrom() - {0}", smtpEx.InnerException.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method GetEmailFrom() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (ex.InnerException != null)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method GetEmailFrom() - {0}", ex.InnerException.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
            return string.Empty;
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
        //Shubham[skadam] GEOS2-4406 Send an email notification when the status of the PO changes 21 06 2023
        private void SendPOEmailNotification(int NewStatusId, int OldStatusId)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendPOEmailNotification ...", category: Category.Info, priority: Priority.Low);
                //SRMService = new SRMServiceController("localhost:6699");
                WorkflowTransition WorkflowTransition = WorkflowTransitionList.FirstOrDefault(f => f.IdWorkflowStatusFrom == OldStatusId && f.IdWorkflowStatusTo == NewStatusId);
                if (WorkflowTransition != null)
                {
                    if (WorkflowTransition.IsNotificationRaised == 1)
                    {
                        POEmailNotification poEmailNotification = (SRMService.GetPurchasingOrderNotificationDetails(WarehousePurchaseOrderForSendPOEmail.IdWarehousePurchaseOrder, GeosApplication.Instance.ActiveUser.IdUser));
                        if (string.IsNullOrEmpty(Comment))
                        {
                            poEmailNotification.Comments = null;
                        }
                        else
                        {
                            poEmailNotification.Comments = Comment;
                        }
                        //Service updated from PurchasingOrderNotificationSend to PurchasingOrderNotificationSend_V2560 by [rdixit][02.09.2024][GEOS2-6383]
                        SRMService.PurchasingOrderNotificationSend_V2560(poEmailNotification);
                        Comment = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendPOEmailNotification() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[Sudhir.Jangra][GEOS2-5493]
        private void FillToCC(WarehousePurchaseOrder wpo)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillToCC()...", category: Category.Info, priority: Priority.Low);
                List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                ObservableCollection<Contacts> temp = new ObservableCollection<Contacts>();
                //temp.AddRange(SRMService.GetArticleSuppliersOrders_V2510(Convert.ToInt32(wpo.ArticleSupplier.IdArticleSupplier), (long)wpo.IdWarehouse));
                //pramod.misal 31.05.2024
                temp.AddRange(SRMService.GetArticleSuppliersOrders_V2520(Convert.ToInt32(wpo.ArticleSupplier.IdArticleSupplier), (long)wpo.IdWarehouse));
                GeosApplication.Instance.Logger.Log("Method FillToCC()... recived to and cc emails in list", category: Category.Info, priority: Priority.Low);
                if (temp?.Count > 0)
                {
                    ToContactList = new ObservableCollection<Contacts>(temp.Where(x => x.IdType == 1918));
                    CCContactList = new ObservableCollection<Contacts>(temp.Where(x => x.IdType == 1919));
                }

                if (ToContactList?.Count > 0)
                {
                    GeosApplication.Instance.Logger.Log("Method FillToCC()... list have at least 1 To email" + string.Join(",", ToContactList.Select(i => i.Email)), category: Category.Info, priority: Priority.Low);
                    ToContactList = new ObservableCollection<Contacts>(ToContactList.OrderByDescending(x => x.IsMainContact).ToList());
                    ToContactList.FirstOrDefault().IsMainContact = true;
                }
                else
                    GeosApplication.Instance.Logger.Log("Method FillToCC()... list dont have at least 1 To email", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.Logger.Log("Method FillToCC()...", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Exception in Method FillToCC()..." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion
    }
}
