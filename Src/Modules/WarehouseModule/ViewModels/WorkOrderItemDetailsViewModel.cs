using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm.POCO;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Warehouse.Reports;
using DevExpress.Xpf.Printing;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraReports.UI;
using Emdep.Geos.UI.Commands;
using Microsoft.Win32;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Data.Common.SAM;
using System.Collections.ObjectModel;

//DevExpress.XtraGrid.Views.Base;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class WorkOrderItemDetailsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Task log
        // [000][skale][14-08-2019][GEOS2-1659]Add work time log in Work Order details
        // [001][avpawar][22-08-2019][GEOS2-1648] Wizzard should not be displayed if there are NO items
        // [002][avpawar][28/08/2019][GEOS2-1655] Add "Stock History" section in Work Order
        // [003][avpawar][11/09/2019][GEOS2-1655] Add Loaction and Delivery Note column in "Stock History" section in Work Order
        // [004][avpawar][09/01/2020][GEOS2-1848] Add Producer Info in Work Order Label On Print button
        #endregion

        #region Services
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
       // IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        #endregion //End Of Services

        #region Declaration

        private Ots oT;
        private Int64 downloadRemainingQuantity;
        long idOtTemp;
        Warehouses objWarehouseTemp;
        private double dialogHeight;
        private double dialogWidth;
        private bool isSaveChanges;
        //[000] added
        private ObservableCollection<OTWorkingTime> workLogItemList;

        private string worklogTotalTime;

        private List<UserShortDetail> userImageList;

        bool isBusy; // [002] added       

        MaximizedElementPosition maximizedElementPosition;
        private List<Shipment> listShipment;
        private List<PackingBox> listBox;
        private bool isBoxControlEnable;
        private Company objCompany;
        public string ViewWorkOrderItemsGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ViewWorkOrderItemsGridSettingFilePath.Xml";
        private bool isViewWorkOrderItemColumnChooserVisible;
        private string mySearchText;
        private List<WorkflowTransition> workflowTransition;
        private List<WorkflowStatus> workflowStatus;
        private List<WorkflowStatus> workflowStatusButtons;
        private WorkflowStatus selectedWorkflowStatusButton;
        WorkflowStatus workflowstatus;

        bool isSave;//[Sudhir.Jangra][GEOS2-4539][24/08/2023]

        private bool isSituationEnabled;//[Sudhir.Jangra][GEOS2-4539][23/08/2023]
        private Company otSite;//[Sudhir.Jangra][GEOS2-5644]
        private bool isRemoveWorkLog;//[Sudhir.Jangra][GEOS2-5644]
        private bool isEditWorkLog;//[Sudhir.Jangra][GEOS2-5644]
        private OTWorkingTime selectedWorklogRow;//[Sudhir.Jangra][GEOS2-5644]
        private ObservableCollection<OTAssignedUser> otAssignedUser;//[Sudhir.Jangra][GEOS2-5644]
        private ObservableCollection<OTAssignedUser> userToAssingedUser;//[Sudhir.Jangra][GEOS2-5644]
        List<OTAssignedUser> cloneOTAssignedUser = new List<OTAssignedUser>();
        #endregion // End Of Declaration

        #region Public Properties
        //[rdixit][02.10.2023]
        List<OtItem> otItemsList;
        public List<OtItem> OtItemsList
        {
            get { return otItemsList; }
            set
            {
                otItemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItemsList"));
            }
        }
        public Bitmap ReportHeaderImage { get; set; }
        public Warehouses WarehouseDetails { get; set; }

        public Ots OT
        {
            get { return oT; }
            set
            {
                oT = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
            }
        }

        public long DownloadRemainingQuantity
        {
            get { return downloadRemainingQuantity; }
            set
            {
                downloadRemainingQuantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadRemainingQuantity"));
            }
        }

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }

        //[000] added
        public ObservableCollection<OTWorkingTime> WorkLogItemList
        {
            get { return workLogItemList; }
            set
            {
                workLogItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogItemList")); //[rdixit][18.06.2024][GEOS2-5644]
            }
        }
        public List<UserShortDetail> UserImageList
        {
            get { return userImageList; }
            set { userImageList = value; }
        }
        public string WorklogTotalTime
        {
            get { return worklogTotalTime; }
            set
            {
                worklogTotalTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorklogTotalTime"));
            }
        }
        // [002] added
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        // [002] End

        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }

        public List<Shipment> ListShipment
        {
            get { return listShipment; }
            set
            {
                listShipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListShipment"));
            }
        }

        public List<PackingBox> ListBox
        {
            get { return listBox; }
            set
            {
                listBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListBox"));
            }
        }

        public bool IsBoxControlEnable
        {
            get { return isBoxControlEnable; }
            set { isBoxControlEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsBoxControlEnable")); }
        }

        public Company ObjCompany
        {
            get { return objCompany; }
            set
            {
                objCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjCompany"));
            }
        }

        public bool IsViewWorkOrderItemColumnChooserVisible
        {
            get
            {
                return isViewWorkOrderItemColumnChooserVisible;
            }

            set
            {
                isViewWorkOrderItemColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsViewWorkOrderItemColumnChooserVisible"));
            }
        }

        public string MySearchText
        {
            get { return mySearchText; }
            set
            {
                mySearchText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MySearchText"));
            }
        }
        public List<WorkflowTransition> WorkflowTransitionList
        {
            get { return workflowTransition; }
            set
            {
                workflowTransition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransitionList"));
            }
        }
        public List<WorkflowStatus> WorkflowStatusList
        {
            get { return workflowStatus; }
            set
            {
                workflowStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusList"));
            }
        }
        public List<WorkflowStatus> WorkflowStatusButtons
        {
            get { return workflowStatusButtons; }
            set
            {
                workflowStatusButtons = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusButtons"));
            }
        }

        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
            }
        }
        public WorkflowStatus SelectedWorkflowStatusButton
        {
            get
            {
                return selectedWorkflowStatusButton;
            }

            set
            {
                selectedWorkflowStatusButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkflowStatusButton"));
            }
        }

        public WorkflowStatus WorkflowStatus
        {
            get { return workflowstatus; }
            set
            {
                workflowstatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatus"));
            }
        }
      
      
        

      
      
        //[Sudhir.Jangra][GEOS2-4539][24/08/2023]
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        //[Sudhir.Jangra][GEOS2-4539][23/08/2023]
        public bool IsSituationEnabled
        {
            get { return isSituationEnabled; }
            set
            {
                isSituationEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSituationEnabled"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5644]
        public Company OtSite
        {
            get { return otSite; }
            set
            {
                otSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtSite"));
            }
        }
        //[Sudhir.Jangra][GEOS2-5644]
        public bool IsRemoveWorkLog
        {
            get { return isRemoveWorkLog; }
            set
            {
                isRemoveWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRemoveWorkLog"));

            }
        }
        //[Sudhir.Jangra][GEOS2-5644]
        public bool IsEditWorkLog
        {
            get { return isEditWorkLog; }
            set
            {
                isEditWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditWorkLog"));

            }
        }

        //[Sudhir.Jangra][GEOS2-5644]
        public OTWorkingTime SelectedWorklogRow
        {
            get { return selectedWorklogRow; }
            set
            {
                selectedWorklogRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorklogRow"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5644]
        public ObservableCollection<OTAssignedUser> OtAssignedUser
        {
            get { return otAssignedUser; }
            set
            {
                otAssignedUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtAssignedUser"));
            }
        }
        //[Sudhir.Jangra][GEOS2-5644]
        public ObservableCollection<OTAssignedUser> UserToAssingedUser
        {
            get { return userToAssingedUser; }
            set { userToAssingedUser = value; OnPropertyChanged(new PropertyChangedEventArgs("UserToAssingedUser")); }
        }


        #endregion

        #region ICommands
        public ICommand CommandPendingWorkOrderItemShowCancelButton { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand ScanMaterialsCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand WorkOrderItemDetailsViewHyperlinkClickCommand { get; set; }
        public ICommand PrintWorkOrderItemCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand DNHyperlinkClickCommand { get; set; } //[003] added
        public ICommand ExportWorkLogCommand { get; set; }
        public ICommand CommentsShipmentGridDoubleClickCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand OpenWorkflowDiagramCommand { get; set; }

        public ICommand WorkflowButtonClickCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-4539][23/08/2023]

        public ICommand DeleteWorkLogCommand { get; set; }//[Sudhir.Jangra][GEOS2-5644]

        public ICommand EditWorkLogDoubleClickCommand { get; set; }//[Sudhir.jangra][GEOS2-5549]

        #endregion // End Of ICommands

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

        #region Constructor
        public WorkOrderItemDetailsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsViewModel()...", category: Category.Info, priority: Priority.Low);
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 95;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
             
                PopupMenuShowingCommand = new DelegateCommand<OpenPopupEventArgs>(OpenPopupDateEditAction);
                CommandPendingWorkOrderItemShowCancelButton = new DelegateCommand<object>(CommandPendingWorkOrderItemShowCancelAction);
                WorkOrderItemDetailsViewHyperlinkClickCommand = new DelegateCommand<object>(WorkOrderItemDetailsViewHyperlinkClickCommandAction);
                ScanMaterialsCommand = new DelegateCommand<object>(ScanAction);
                PrintCommand = new DelegateCommand<object>(PrintCommandAction);
                ReportHeaderImage = ResizeImage(new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.EmdepMonoBlue));
                PrintWorkOrderItemCommand = new DelegateCommand<object>(PrintWorkOrderItem);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportStockLogButtonCommandAction));   // [002] added
                DNHyperlinkClickCommand = new DelegateCommand<object>(DNHyperlinkClickCommandAction);           //[003] Added
                ExportWorkLogCommand = new RelayCommand(new Action<object>(ExportWorkLogCommandAction));
                CommentsShipmentGridDoubleClickCommand = new DelegateCommand<object>(EditShipmentAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                OpenWorkflowDiagramCommand = new DelegateCommand<object>(OpenWorkflowDiagramCommandAction);
                WorkflowButtonClickCommand = new DelegateCommand<object>(WorkflowButtonClickCommandAction);

                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);//[Sudhir.Jangra][GEOS2-4539][23/08/2023]

                DeleteWorkLogCommand = new DelegateCommand<object>(DeleteWorklogRowCommandAction);

                EditWorkLogDoubleClickCommand = new DelegateCommand<object>(EditWorklog);//[Sudhir.Jangra][GEOS2-5644]

                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor WorkOrderItemDetailsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // End Of Constructor

        #region Methods

        /// <summary>
        /// method for resize image.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private Bitmap ResizeImage(Bitmap bitmap)
        {
            GeosApplication.Instance.Logger.Log("Method ResizeImage ...", category: Category.Info, priority: Priority.Low);

            Bitmap resized = new Bitmap(300, 100);

            try
            {
                Graphics g = Graphics.FromImage(resized);

                g.DrawImage(bitmap, new Rectangle(0, 0, resized.Width, resized.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
                g.Dispose();

                //Save picture in users temp folder.
                string myTempFile = Path.Combine(Path.GetTempPath(), "EmdepLogo.jpg");

                //delete if already image exist there.
                if (File.Exists(myTempFile))
                {
                    File.Delete(myTempFile);
                }

                resized.Save(myTempFile, ImageFormat.Jpeg);
                return resized;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ResizeImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return resized;
        }

        /// <summary>
        /// Method for Print work order 
        /// [001][skale][23/01/2020][GEOS2-1937]- Add OT comments in OT label
        /// </summary>
        /// <param name="idOt"></param>
        /// <param name="objWarehouse"></param>
        public void PrintCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PrintCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
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

                WorkorderReport report = new WorkorderReport();
                report.imgLogo.Image = ReportHeaderImage;
                report.lblCarriageType.BackColor = System.Drawing.Color.Black;
                report.lblCarriageType.ForeColor = System.Drawing.Color.White;
                report.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                report.lblTradeGroup.ForeColor = System.Drawing.Color.Black;
                report.lblCustomer.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrLblCustomer.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 40, System.Drawing.FontStyle.Bold);
                report.lblWO.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrLblWO.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 50, System.Drawing.FontStyle.Bold);
                report.lblCarriageMethod.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.lblCarriageType.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 100, System.Drawing.FontStyle.Bold);
                report.lblDeliveryDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrLblDeliveyDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 40, System.Drawing.FontStyle.Bold);
                report.lblTradeGroup.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 75, System.Drawing.FontStyle.Bold);
                report.xrLblWarehouse.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);

                //[004] Added
                report.lblProducerOrigin.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrLblProducerName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 50, System.Drawing.FontStyle.Bold);

                //[001] added
                report.lblComment.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrlblComment.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 50, System.Drawing.FontStyle.Bold);

                List<WorkorderItemPrintDetails> tempList = new List<WorkorderItemPrintDetails>();
                WorkorderItemPrintDetails workOrderItemPrintDetails = new WorkorderItemPrintDetails();
                workOrderItemPrintDetails.Customer = OT.Quotation.Site.Name;
                workOrderItemPrintDetails.Workorder = OT.Code;

                if (OT.Quotation.Site.CountryGroup != null)
                {
                    if (OT.Quotation.Site.CountryGroup.IsFreeTrade == 1)
                    {
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.Black;
                        report.lblTradeGroup.Text = OT.Quotation.Site.CountryGroup.Name;
                        workOrderItemPrintDetails.TradeGroup = OT.Quotation.Site.CountryGroup.Name;
                    }
                    else
                    {
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                        report.lblTradeGroup.Text = OT.Quotation.Site.Country.Iso;
                        workOrderItemPrintDetails.TradeGroup = OT.Quotation.Site.Country.Iso;
                    }
                }
                else
                {
                    report.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                    report.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                    report.lblTradeGroup.Text = OT.Quotation.Site.Country.Iso;
                    workOrderItemPrintDetails.TradeGroup = OT.Quotation.Site.Country.Iso;
                }

                if (OT.DeliveryDate != null)
                    workOrderItemPrintDetails.DeliveryDateString = OT.DeliveryDate.Value.ToShortDateString();

                if (OT.Quotation.Offer.CarriageMethod != null && OT.Quotation.Offer.IdCarriageMethod != null)
                {
                    string typeText = string.Empty;
                    typeText = OT.Quotation.Offer.CarriageMethod.Value.Substring(0, 1);
                    workOrderItemPrintDetails.CarriageText = typeText;
                    if (OT.Quotation.Offer.IdCarriageMethod == 50)
                    {
                        workOrderItemPrintDetails.CarriageType = new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.Ground);
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                    }
                    else if (OT.Quotation.Offer.IdCarriageMethod == 51)
                    {
                        workOrderItemPrintDetails.CarriageType = new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.Sea);
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Black;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.White;
                    }
                    else if (OT.Quotation.Offer.IdCarriageMethod == 52)
                    {
                        workOrderItemPrintDetails.CarriageType = new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.Air);
                        report.lblTradeGroup.BackColor = System.Drawing.Color.Transparent;
                        report.lblTradeGroup.ForeColor = System.Drawing.Color.Black;
                    }
                }
                else
                {
                    report.lblCarriageType.BackColor = System.Drawing.Color.Transparent;
                }

                report.xrLblWarehouse.Text = WarehouseCommon.Instance.Selectedwarehouse.Name;           // [004] Added

                if (OT.CountryGroup != null)                                                            // [004] Added
                {
                    report.xrLblProducerName.Text = OT.CountryGroup.Name;
                    report.xrLine5.LocationF = new DevExpress.Utils.PointFloat(24F, 321.7083F);
                    report.lblWO.LocationF = new DevExpress.Utils.PointFloat(27F, 330.2099F);
                }
                else
                {
                    report.xrProducerPanel.Visible = false;
                    report.xrLine9.Visible = false;
                    report.xrLblCustomer.SizeF = new System.Drawing.SizeF(575.25F, 188.2049F);
                    report.lblTradeGroup.SizeF = new System.Drawing.SizeF(187.75F, 250.7917F);
                    report.xrLine1.SizeF = new System.Drawing.SizeF(2.083313F, 250.7917F);
                    report.xrLine5.LocationF = new DevExpress.Utils.PointFloat(24F, 248.7917F);
                    report.lblWO.LocationF = new DevExpress.Utils.PointFloat(27F, 255.2099F);
                    report.xrLblWO.LocationF = new DevExpress.Utils.PointFloat(27F, 300.2883F);
                }
                //[001] added
                if (string.IsNullOrEmpty(OT.Comments))
                    report.xrlblComment.Text = string.Empty;
                else
                    report.xrlblComment.Text = OT.Comments;

                //report.xrlblComment.Text = OT.Comments.Replace("\r\n","").Trim();


                tempList.Add(workOrderItemPrintDetails);
                report.DataSource = tempList;

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = report;
                report.CreateDocument();
                window.Show();

                ReportPrintTool printTool = new ReportPrintTool(report);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][14-08-2019][GEOS2-1659] Add work time log in Work Order details
        /// [002][cpatil][24-09-2021][GEOS2-2174] [Add new column "CurrentStock" in Planning Simulator]
        /// [003][cpatil][08-09-2023][GEOS2-4417]
        /// </summary>
        /// <param name="idOt"></param>
        /// <param name="objWarehouse"></param>
        public void Init(long idOt, Warehouses objWarehouse, string reference = null)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                idOtTemp = idOt;
                objWarehouseTemp = objWarehouse;
                WarehouseDetails = objWarehouse;
                ObjCompany = objWarehouse.Company;
                #region Service Comments
                //  WarehouseService = new WarehouseServiceController("localhost:6699");
                // OT = WarehouseService.GetWorkOrderByIdOt_V2038(idOt, objWarehouse);
                //   OT = WarehouseService.GetWorkOrderByIdOt_V2320(idOt, objWarehouse);
                //[Sudhir.Jangra][GEOS2-4539][10/08/2023]
                //[003]
                //Service GetWorkOrderByIdOt_V2430 updated with GetWorkOrderByIdOt_V2450 [rdixit][GEOS2-4948][31.10.2023]
                //Service Updated from GetWorkOrderByIdOt_V2450 to GetWorkOrderByIdOt_V2460 by [rdixit][30.11.2023][GEOS2-5068]
                #endregion

                //OT = WarehouseService.GetWorkOrderByIdOt_V2460(idOt, objWarehouse);

                // OT = WarehouseService.GetWorkOrderByIdOt_V2510(idOt, objWarehouse);//rajashri GEOS2-5455 12-04-2024
               // OT = WarehouseService.GetWorkOrderByIdOt_V2540(idOt, objWarehouse);// //[rahul.gadhave] [GEOS2-5676] [16-07-2024]

                //[Sudhir.Jangra][GEOS2-6050]
                OT = WarehouseService.GetWorkOrderByIdOt_V2550(idOt, objWarehouse);

                //FillStatusList();
                // [002]
                if (!String.IsNullOrEmpty(reference))
                    MySearchText = reference;
                else
                    MySearchText = string.Empty;
                
                DownloadRemainingQuantity = OT.OtItems.Sum(oti => oti.RevisionItem.RemainingQuantity);
                OtItem item = OT.OtItems.FirstOrDefault();

                #region GEOS2-4539 
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 86))//[Sudhir.Jangra][GEOS2-4539][23/08/2023]
                {
                    IsSituationEnabled = true;
                }
                else
                {
                    IsSituationEnabled = false;
                }
                #endregion


                //[001] Added
                //WorkLogItemList = new ObservableCollection<OTWorkingTime>();
                UserImageList = new List<UserShortDetail>();

                //WorkLogItemList = WarehouseService.GetOTWorkingTimeDetails(idOtTemp, objWarehouse);
             //   WorkLogItemList =new ObservableCollection<OTWorkingTime>(WarehouseService.GetOTWorkingTimeDetails_V2036(idOtTemp, objWarehouse));
                //[Sudhir.Jangra][GEOS2-6050]
                WorkLogItemList = new ObservableCollection<OTWorkingTime>(WarehouseService.GetOTWorkingTimeDetails_V2550(idOtTemp, objWarehouse));
                List<UserShortDetail> tempList = WorkLogItemList.Select(x => x.UserShortDetail).ToList();
                UserImageList = tempList.GroupBy(x => x.IdUser).Select(x => x.First()).ToList();

                TimeSpan worklogTotalTime = new TimeSpan(WorkLogItemList.Sum(r => r.TotalTime.Ticks));

                WorklogTotalTime = string.Format("{0}H {1}M", worklogTotalTime.Hours, worklogTotalTime.Minutes);
                //End
                SetMaximizedElementPosition();
                ListShipment = new List<Shipment>();
                ListShipment = CrmStartUp.GetAllShipmentsByOfferId(ObjCompany, OT.Quotation.IdOffer);


                WorkflowTransitionList = new List<WorkflowTransition>(WarehouseService.GetAllWorkflowTransitions_V2320());
                WorkflowStatusList = new List<WorkflowStatus>(WarehouseService.GetAllWorkflowStatus_V2320());
                WorkflowStatus = new WorkflowStatus();
                WorkflowStatus = WorkflowStatusList.Where(i => i.IdWorkflowStatus == OT.IdWorkflowStatus).FirstOrDefault();
                OT.WorkflowStatus = WorkflowStatus;
                List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == OT.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                WorkflowStatusButtons = new List<WorkflowStatus>();
                foreach (byte statusbutton in GetCurrentButtons)
                {
                    WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                }
                WorkflowStatusButtons = new List<WorkflowStatus>(WorkflowStatusButtons);

                //[Sudhir.Jangra][GEOS2-5644]
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 35))
                {
                    IsRemoveWorkLog = true;
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 34))
                {
                    IsEditWorkLog = true;
                }
                FillOtAssignUsersList(OtSite, idOt);


                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to restrict dateEdit Popup.
        /// </summary>
        /// <param name="obj"></param>
        private void OpenPopupDateEditAction(OpenPopupEventArgs obj)
        {
            obj.Cancel = true;
            obj.Handled = true;
        }

        //decimal qty = 0; decimal weight = 0;
        //public void UnboundData(object obj)
        //{
        //    if (obj == null) return;
        //    TreeListCustomColumnDisplayTextEventArgs e = obj as TreeListCustomColumnDisplayTextEventArgs;
        //    if (e != null)
        //    {
        //        if (e.Column.FieldName == "RevisionItem.Quantity")
        //        {
        //            qty = 0;
        //            qty = Convert.ToDecimal(e.DisplayText);
        //        }
        //        if (e.Column.FieldName == "RevisionItem.WarehouseProduct.Article.Weight")
        //        {
        //            weight = 0;
        //            weight = Convert.ToDecimal(e.Value);
        //            e.DisplayText = Convert.ToString((qty * weight));
        //            weight = qty * System.Convert.ToDecimal(e.Value);
        //            if (weight < 1)
        //            {
        //                if (Math.Round(weight * 1000, 0) == 1000)
        //                    e.DisplayText = string.Format("{0} Kg", 1);
        //                e.DisplayText = string.Format("{0} gr", Math.Round(weight * 1000, 0));
        //            }
        //            else
        //            {
        //                e.DisplayText = string.Format("{0} Kg", Math.Round(weight, 3));
        //            }
        //        }
        //    }
        //}

        private void PrintWorkOrderItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderItem....", category: Category.Info, priority: Priority.Low);

                TreeListView detailView = (TreeListView)((object[])obj)[0];
                // OtItem otItem = (OtItem)obj;
                OtItem otItem = (OtItem)((object[])obj)[1];
                WorkOrderPrintView workOrderPrintView = new WorkOrderPrintView();
                WorkOrderPrintViewModel workOrderPrintViewModel = new WorkOrderPrintViewModel();
                EventHandler handler = delegate { workOrderPrintView.Close(); };
                workOrderPrintViewModel.RequestClose += handler; //392235
                workOrderPrintView.DataContext = workOrderPrintViewModel;
                workOrderPrintViewModel.Init(otItem, OT);
                var ownerInfo = (detailView as FrameworkElement);
                workOrderPrintView.Owner = Window.GetWindow(ownerInfo);
                workOrderPrintView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method PrintWorkOrderItem....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkOrderItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for open scan picking material window.
        /// [001] [GEOS2-1648][avpawar] Wizzard should not be displayed if there are NO items
        /// [002][cpatil][08-09-2023][GEOS2-4417]
        /// </summary>
        /// <param name="obj"></param>
        private void ScanAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanAction....", category: Category.Info, priority: Priority.Low);
                bool FollowFIFO;
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
                    pickingMaterialsViewModel.PreviousTimerValue = isTimer;
                    pickingMaterialsViewModel.PrintBatchLabelAfterScanCompleted = isBatchLabel;
                    EventHandler handle = delegate { pickingMaterialsView.Close(); };
                    pickingMaterialsViewModel.RequestClose += handle;
                    pickingMaterialsViewModel.SetFollowFIFO(FollowFIFO, FIFOGotoItem);
                    pickingMaterialsViewModel.InIt(OT, WarehouseDetails);

                    // [001] Added //[rdixit][GEOS2-4930][31.10.2023]
                    if (pickingMaterialsViewModel.materialSoredList != null && pickingMaterialsViewModel.materialSoredList.Count > 0)
                        isItemsAvailable = true;

                    if (isItemsAvailable)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        if (pickingMaterialsViewModel.UnspectedMaterialList?.Count > 0)//[rdixit][GEOS2-4930][31.10.2023]
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UnInspectedItemsAvailable").ToString(),
                                string.Join(",", pickingMaterialsViewModel.UnspectedMaterialList.Select(r => r.Reference))), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);

                        pickingMaterialsView.DataContext = pickingMaterialsViewModel;
                        pickingMaterialsView.ShowDialog();
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        if (pickingMaterialsViewModel.UnspectedMaterialList?.Count > 0)//[rdixit][GEOS2-4930][31.10.2023]
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UnInspectedItemsAvailableNoPicking").ToString(),
                                string.Join(",", pickingMaterialsViewModel.UnspectedMaterialList.Select(r => r.Reference))), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                        break;
                    }


                } while (pickingMaterialsViewModel.IsCanceled == false);

                //[001] Added
                if (isItemsAvailable == false && pickingMaterialsViewModel.UnspectedMaterialList?.Count == 0)
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PickingNoItemsAvailable").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                #region Service Comments
                //[001] End
                //OT = WarehouseService.GetWorkOrderByIdOt(idOtTemp, Convert.ToInt32(objWarehouseTemp.IdWarehouse), objWarehouseTemp);
                // OT = WarehouseService.GetWorkOrderByIdOt_V2200(idOtTemp, objWarehouseTemp);
                //[Sudhir.Jangra][GEOS2-4539][17/08/2023] Changed Service Version wise.
                //[002]
                //Service GetWorkOrderByIdOt_V2430 updated with GetWorkOrderByIdOt_V2450 [rdixit][GEOS2-4948][31.10.2023]
                //Service Updated from GetWorkOrderByIdOt_V2450 to GetWorkOrderByIdOt_V2460 by [rdixit][30.11.2023][GEOS2-5068]
                #endregion
                //OT = WarehouseService.GetWorkOrderByIdOt_V2460(idOtTemp, objWarehouseTemp);
                /* OT = WarehouseService.GetWorkOrderByIdOt_V2510(idOtTemp, objWarehouseTemp);*///rajashri GEOS2-5455 12-04-2024
               // OT = WarehouseService.GetWorkOrderByIdOt_V2540(idOtTemp, objWarehouseTemp);//[rahul.gadhave] [GEOS2-5676] [16-07-2024]

                //[Sudhir.Jangra][GEOS2-6050]
                OT = WarehouseService.GetWorkOrderByIdOt_V2550(idOtTemp, objWarehouseTemp);


                //[rdixit][30.11.2023][GEOS2-5068] To update finish status in data base
                List<OtItem> ItemsToFinishStatus = new List<OtItem>();
                bool result = false;
                if (OT.OtItems != null)
                {
                    ItemsToFinishStatus = OT.OtItems.Where(i => i.IsFinish).ToList();                
                    if (ItemsToFinishStatus != null)
                    {
                        result = WarehouseService.UpdateOtItemsStatusToFinish(ItemsToFinishStatus);
                    }
                }
                List<WorkflowStatus> st = new List<WorkflowStatus>();
                //[rajashri][GEOS2-4849][30.10.2023]
                // Added
                if (pickingMaterialsViewModel.IsSaveChanges)
                WorkflowStatusButtons = new List<WorkflowStatus>(pickingMaterialsViewModel.WorkflowStatusButtons);
                WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == pickingMaterialsViewModel.WorkflowStatus.IdWorkflowStatus);
                //End
                DownloadRemainingQuantity = OT.OtItems.Sum(oti => oti.RevisionItem.RemainingQuantity);


                GeosApplication.Instance.Logger.Log("Method ScanAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ScanAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for cloase window.
        /// </summary>
        /// <param name="obj"></param>
        private void CommandPendingWorkOrderItemShowCancelAction(object obj)
        {
            RequestClose(null, null);
        }
        public void Dispose()
        {

        }

        /// <summary>
        /// Method to Open Article View on hyperlink click
        /// [001][skale][2019-03-06][GEOS2-1515][SP-64]Add new column "Stock" in Work Order
        /// </summary>
        /// <param name="obj"></param>
        public void WorkOrderItemDetailsViewHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WorkOrderItemDetailsViewHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
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

                TreeListView detailView = (TreeListView)obj;
                OtItem otItem = (OtItem)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.Init(otItem.RevisionItem.WarehouseProduct.Article.Reference, warehouseId);
                articleDetailsView.DataContext = articleDetailsViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                var ownerInfo = (detailView as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                //[001] added
                if (articleDetailsViewModel.IsResult)
                {
                    if (articleDetailsViewModel.UpdateArticle.MyWarehouse != null)
                        otItem.ArticleMinimumStock = articleDetailsViewModel.UpdateArticle.MyWarehouse.MinimumStock;
                }
                //end
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method WorkOrderItemDetailsViewHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WorkOrderItemDetailsViewHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [002][avpawar][28/08/2019][GEOS2-1655][Add "Stock History" section in Work Order]
        /// Method to export Stock Log on Excel
        /// </summary>
        /// <param name="obj"></param>
        private void ExportStockLogButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportStockLogButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Stock_Log";
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

                    ResultFileName = (saveFile.FileName);

                    TableView tblStockLog = ((TableView)obj);
                    tblStockLog.ShowTotalSummary = false;
                    tblStockLog.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    tblStockLog.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    tblStockLog.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportStockLogButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportStockLogButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method is for [GEOS2-1655][Add "Stock History" section in Work Order]
        /// </summary>
        /// <param name="e"></param>
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }


        /// <summary>
        /// [003][avpawar][11/09/2019][GEOS2-1655][Add Location and Delivery Note in "Stock History" section in Work Order]
        /// Method to Open Delivery Note on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
        private void DNHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DNHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

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

                TableView tblView = (TableView)obj;
                ArticlesStock ac = (ArticlesStock)tblView.DataControl.CurrentItem;
                EditDeliveryNoteView editDeliveryNoteView = new EditDeliveryNoteView();
                EditDeliveryNoteViewModel editDeliveryNoteViewModel = new EditDeliveryNoteViewModel();
                EventHandler handle = delegate { editDeliveryNoteView.Close(); };
                editDeliveryNoteViewModel.RequestClose += handle;
                // WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById(ac.WarehouseDeliveryNoteItem.IdWarehouseDeliveryNote);
                //[pramod.misal][GEOS2-4543][11-10-2023]
                //WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2440(ac.WarehouseDeliveryNoteItem.IdWarehouseDeliveryNote);
                //Shubham[skadam] GEOS2-5226 NO SE PUEDE DESBLOQUEAR UN ALBARÁN DE LA REFERENCIA 02MOTTRONIC 12 01 2024
               // WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2480(ac.WarehouseDeliveryNoteItem.IdWarehouseDeliveryNote);
                //[Sudhir.Jangra][GEOS2-5457]
                WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2510(ac.WarehouseDeliveryNoteItem.IdWarehouseDeliveryNote);


                editDeliveryNoteViewModel.Init(wdn);
                editDeliveryNoteView.DataContext = editDeliveryNoteViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                var ownerInfo = (tblView as FrameworkElement);
                editDeliveryNoteView.Owner = Window.GetWindow(ownerInfo);
                editDeliveryNoteView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method DNHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DNHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        /// </summary>
        public void SetMaximizedElementPosition()
        {

            if (GeosApplication.Instance.UserSettings != null)
            {
                if (GeosApplication.Instance.UserSettings.ContainsKey("Appearance"))
                {
                    if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["Appearance"].ToString()))
                    {
                        MaximizedElementPosition = MaximizedElementPosition.Right;
                    }
                    else
                    {
                        MaximizedElementPosition = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["Appearance"].ToString(), true);
                    }
                }
                else
                {
                    MaximizedElementPosition = MaximizedElementPosition.Right;
                }

            }
        }

        private void ExportWorkLogCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportWorkLogCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Work Log List" + " - " + OT.Code;
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

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportWorkLogCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportWorkLogCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditShipmentAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditShipmentAction ...", category: Category.Info, priority: Priority.Low);

            if (obj == null) return;
            Shipment ship = (Shipment)obj;

            try
            {
                ListBox = new List<PackingBox>();
                ListBox = CrmStartUp.GetAllPackingBoxesByShipmentId(ObjCompany, ship.IdShipment);
                if (ListBox.Count > 0)
                {
                    IsBoxControlEnable = true;
                    string LWH = string.Empty;
                    int i = 0;
                    foreach (PackingBox item in ListBox)
                    {
                        double length = item.Length;
                        double width = item.Width;
                        double height = item.Height;
                        LWH = length + " x " + width + " x " + height;
                        ListBox[i].PackingBoxDimension = LWH;
                        i++;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditShipmentAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditShipmentAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }


        /// <summary>
        /// [001][avpawar][17-09-2021][GEOS2-1768] [Save current configuration of the work order grid]
        /// [002][cpatil][24-09-2021][GEOS2-2174] [Add new column "CurrentStock" in Planning Simulator]
        /// </summary>
        /// <param name="obj"></param>
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;

                if (File.Exists(ViewWorkOrderItemsGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl.RestoreLayoutFromXml(ViewWorkOrderItemsGridSettingFilePath);
                    GridControl gridControlView = (GridControl)((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl;
                    TreeListView tableView = (TreeListView)gridControlView.View;
                    //[002]
                    if (!String.IsNullOrEmpty(MySearchText))
                    {
                        tableView.SearchString = MySearchText;
                    }
                    else
                    {
                        tableView.SearchString = string.Empty;
                    }
                }
                #region GEOS2-4620
                try
                {
                    //Shubham[skadam] GEOS2-4620 Not show proper work log section while open ot 29 06 2023
                    GridControl gridControlViewNew = (GridControl)((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl;
                    List<DevExpress.Xpf.Grid.GridColumn> GridColumnList = new List<DevExpress.Xpf.Grid.GridColumn>();
                    Int32 TotalQuantity = gridControlViewNew.Columns.Where(a => a != null && a.HeaderCaption.ToString().ToLower().Equals("Total Quantity".ToLower())).FirstOrDefault().ActualVisibleIndex;
                    Int32 RemainingQuantity = gridControlViewNew.Columns.Where(a => a.HeaderCaption.ToString().ToLower().Equals("Remaining Quantity".ToLower())).FirstOrDefault().ActualVisibleIndex;
                    if (TotalQuantity < RemainingQuantity)
                    {
                        foreach (DevExpress.Xpf.Grid.GridColumn item in gridControlViewNew.Columns)
                        {
                            if (item.HeaderCaption.ToString().ToLower().Equals("Total Quantity".ToLower()))
                            {
                                item.ActualVisibleIndex = RemainingQuantity;
                                item.VisibleIndex = RemainingQuantity;
                            }
                            else if (item.HeaderCaption.ToString().ToLower().Equals("Remaining Quantity".ToLower()))
                            {
                                item.ActualVisibleIndex = TotalQuantity;
                                item.VisibleIndex = TotalQuantity;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion

                 ((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl.SaveLayoutToXml(ViewWorkOrderItemsGridSettingFilePath);

                GridControl gridControl = (GridControl)((DevExpress.Xpf.Grid.TreeListView)obj.OriginalSource).DataControl;

                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(TreeListColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(TreeListColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 0)
                {
                    IsViewWorkOrderItemColumnChooserVisible = true;
                }
                else
                {
                    IsViewWorkOrderItemColumnChooserVisible = false;
                }

                TreeListView datailView = ((TreeListView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                //datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for remove filter save on grid layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.DependencyProperty == TreeListControl.FilterStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {

            }
            //if (e.Property.Name == "FormatConditions")
            //    e.Allow = false;
        }


        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ViewWorkOrderItemsGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsViewWorkOrderItemColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ViewWorkOrderItemsGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenWorkflowDiagramCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()...", category: Category.Info, priority: Priority.Low);
                WorkflowDiagramsViewModel workflowDiagramViewModel = new WorkflowDiagramsViewModel();
                WorkFlowDiagramsView workflowDiagramView = new WorkFlowDiagramsView();
                EventHandler handle = delegate { workflowDiagramView.Close(); };
                workflowDiagramViewModel.RequestClose += handle;
                workflowDiagramViewModel.WorkflowStatusList = WorkflowStatusList;
                workflowDiagramViewModel.WorkflowTransitionList = WorkflowTransitionList.OrderByDescending(a => a.IdWorkflowTransition).ToList();
                workflowDiagramView.DataContext = workflowDiagramViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                workflowDiagramView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenWorkflowDiagramCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void WorkflowButtonClickCommandAction(object obj)
        {
            try
            {
                int status_id = Convert.ToInt32(obj);

                GeosApplication.Instance.Logger.Log("Method WorkflowButtonClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                TransitionWorkflowStatus(status_id);


                GeosApplication.Instance.Logger.Log("Method WorkflowButtonClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkflowButtonClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkflowButtonClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method WorkflowButtonClickCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void TransitionWorkflowStatus(int currentStatus)
        {
            List<LogEntriesByOT> LogEntriesByOTList = new List<LogEntriesByOT>();
            WorkflowTransition workflowTransition = new WorkflowTransition();
            workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == currentStatus);
            WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == currentStatus);
            LogEntriesByOT logEntriesByOT = new LogEntriesByOT();
            logEntriesByOT.IdOT = OT.IdOT;
            logEntriesByOT.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
            logEntriesByOT.Comments = string.Format(System.Windows.Application.Current.FindResource("WorkflowStatusLogEntryByPO").ToString(), OT.WorkflowStatus.Name, WorkflowStatus.Name);
            logEntriesByOT.IdLogEntryType = 1;
            logEntriesByOT.IsRtfText = false;

            LogEntriesByOTList.Add(logEntriesByOT);

            IsSaveChanges = WarehouseService.UpdateWorkflowStatusInOT_V2320(OT.IdOT, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByOTList);
           
            List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == currentStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
            WorkflowStatusButtons = new List<WorkflowStatus>();

            foreach (byte statusbutton in GetCurrentButtons)
            {
                WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
            }
            WorkflowStatusButtons = new List<WorkflowStatus>(WorkflowStatusButtons);
            
        }

        //[Sudhir.Jangra][GEOS2-4539][23/08/2023]
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
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
                List<OtItem> foundRow = OT.OtItems.Where(x => x.IsUpdatedRow == true).ToList();
                if (foundRow.Count > 0)
                {
                    IsSave = WarehouseService.UpdateEditWorkOrderOTItems(foundRow);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SituationStatusUpdated").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5644]
        private void DeleteWorklogRowCommandAction(object obj)
        {
            try
            {
                bool result = false;
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 35))
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["WorkLogDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (WorkLogItemList != null && WorkLogItemList.Count > 0)
                        {
                            OTWorkingTime OTworktime = (OTWorkingTime)obj;
                            result = WarehouseService.DeleteWorkLog_V2530(OtSite, OTworktime.IdOTWorkingTime);
                            //WorkLogItemList.Remove(OTworktime);
                            //[rdixit][18.06.2024][GEOS2-5644]
                            WorkLogItemList = new ObservableCollection<OTWorkingTime>(WorkLogItemList.Where(i => i.IdOTWorkingTime != OTworktime.IdOTWorkingTime).ToList());
                        }
                        if (result)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkLogDeleteSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log("Method DeletWorklogRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkLogDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeletWorklogRowCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[sudhir.Jangra][GEOS2-5644]
        private void EditWorklog(object obj)
        {

            GeosApplication.Instance.Logger.Log("Method EditWorklog()...", category: Category.Info, priority: Priority.Low);
            TableView detailView = (TableView)obj;
            OTWorkingTime worklog = (OTWorkingTime)detailView.DataControl.CurrentItem;
            SelectedWorklogRow = worklog;

            if (worklog != null)
            {
                WMSEditWorkLogView editWorklLogView = new WMSEditWorkLogView();
                WMSEditWorkLogViewModel editWorkLogViewModel = new WMSEditWorkLogViewModel();
                EventHandler handle = delegate { editWorklLogView.Close(); };
                editWorkLogViewModel.RequestClose += handle;
                editWorklLogView.DataContext = editWorkLogViewModel;
                // ExistWorkLogItemList = new List<OTWorkingTime>(WorkLogItemList.ToList());
                editWorkLogViewModel.EditInit(WorkLogItemList.ToList(), worklog, OtAssignedUser, OtSite);

                var ownerInfo = (obj as FrameworkElement);
                editWorklLogView.Owner = Window.GetWindow(ownerInfo);
                editWorklLogView.ShowDialog();
                if (editWorkLogViewModel.IsSave == true)
                {
                    SelectedWorklogRow.UserShortDetail = editWorkLogViewModel.EditWorkLogItem.UserShortDetail;
                    SelectedWorklogRow.TotalTime = editWorkLogViewModel.EditWorkLogItem.TotalTime;
                    // SelectedWorklogRow.TotalTime = new TimeSpan(SelectedWorklogRow.TotalTime.Days, SelectedWorklogRow.TotalTime.Hours, SelectedWorklogRow.TotalTime.Minutes, 00);

                    if (SelectedWorklogRow.TotalTime.Days > 0)
                    {
                        int Hours = SelectedWorklogRow.TotalTime.Days * 24 + SelectedWorklogRow.TotalTime.Hours;
                        SelectedWorklogRow.Hours = string.Format("{0}H {1}M", Hours, SelectedWorklogRow.TotalTime.Minutes);
                    }
                    else
                    {
                        SelectedWorklogRow.Hours = string.Format("{0}H {1}M", SelectedWorklogRow.TotalTime.Hours, SelectedWorklogRow.TotalTime.Minutes);
                    }
                    if (WorkLogItemList != null && WorkLogItemList.Count > 0)
                    {
                        WorkLogItemList = new ObservableCollection<OTWorkingTime>(WorkLogItemList);
                    }
                }
                //else
                //{
                //    SelectedWorklogRow =  worklog;
                //}
            }
            GeosApplication.Instance.Logger.Log("Method EditWorklog()....executed successfully", category: Category.Info, priority: Priority.Low);

        }

        //[Sudhir.Jangra][GEOS2-5644]
        private void FillOtAssignUsersList(Company company, long idOT)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOtAssignUsersList ...", category: Category.Info, priority: Priority.Low);
                //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
                OtAssignedUser = new ObservableCollection<OTAssignedUser>(WarehouseService.GetOTAssignedUsers(company, idOT).ToList());

                if (OtAssignedUser.Count > 0)
                {
                    foreach (OTAssignedUser item in OtAssignedUser)
                    {
                        OTAssignedUser OTAssignedUser = UserToAssingedUser.FirstOrDefault(x => x.UserShortDetail.IdUser == item.IdUser);
                        UserToAssingedUser.Remove(OTAssignedUser);
                    }

                }

                ///Copy selected record in clone object.
                if (OtAssignedUser != null)
                    cloneOTAssignedUser = OtAssignedUser.Select(x => (OTAssignedUser)x.Clone()).ToList();

                for (int i = 0; i < OtAssignedUser.Count; i++)
                {

                    try
                    {
                        byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(OtAssignedUser[i].UserShortDetail.Login);

                        if (UserProfileImageByte != null)
                        {
                            OtAssignedUser[i].UserShortDetail.UserImage = WarehouseCommon.Instance.ByteArrayToBitmapImage(UserProfileImageByte);
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (OtAssignedUser[i].UserShortDetail.IdUserGender == 1)
                                    OtAssignedUser[i].UserShortDetail.UserImage = WarehouseCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png");
                                else if (OtAssignedUser[i].UserShortDetail.IdUserGender == 2)
                                    OtAssignedUser[i].UserShortDetail.UserImage = WarehouseCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png");
                            }
                            else
                            {
                                if (OtAssignedUser[i].UserShortDetail.IdUserGender == 1)
                                    OtAssignedUser[i].UserShortDetail.UserImage = WarehouseCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png");
                                else if (OtAssignedUser[i].UserShortDetail.IdUserGender == 2)
                                    OtAssignedUser[i].UserShortDetail.UserImage = WarehouseCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillOtAssignUsersList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOtAssignUsersList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOtAssignUsersList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOtAssignUsersList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // End Of Methods
    }
}
