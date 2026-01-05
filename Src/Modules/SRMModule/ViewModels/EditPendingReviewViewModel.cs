using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
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
using Emdep.Geos.Data.Common.SAM;
using System.Collections.ObjectModel;
using Emdep.Geos.Modules.SRM.Views;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse.Views;
using System.Windows.Media.Imaging;
using Emdep.Geos.Modules.SRM.Reports;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows.Controls;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class EditPendingReviewViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // ISRMService SRMService = new SRMServiceController("localhost:6699");

        #endregion

        #region Declaration

        private Ots oT;
        private Int64 downloadRemainingQuantity;
        long idOtTemp;
        Warehouses objWarehouseTemp;
        private double dialogHeight;
        private double dialogWidth;
        private bool isSaveChanges;

        private ObservableCollection<OTWorkingTime> workLogItemList;

        private string worklogTotalTime;

        private List<UserShortDetail> userImageList;

        bool isBusy;

        MaximizedElementPosition maximizedElementPosition;
        private List<Shipment> listShipment;
        private List<PackingBox> listBox;
        private bool isBoxControlEnable;
        private Company objCompany;
        public string ViewWorkOrderItemsGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "EditPendingReviewItemsGridSettingFilePath.Xml";
        private bool isViewWorkOrderItemColumnChooserVisible;
        private string mySearchText;
        private List<WorkflowTransition> workflowTransition;
        private List<WorkflowStatus> workflowStatus;
        private List<WorkflowStatus> workflowStatusButtons;
        private WorkflowStatus selectedWorkflowStatusButton;
        WorkflowStatus workflowstatus;

        bool isSave;

        private bool isSituationEnabled;
        private List<Company> otSite;
        private bool isRemoveWorkLog;
        private bool isEditWorkLog;
        private OTWorkingTime selectedWorklogRow;
        private ObservableCollection<OTAssignedUser> otAssignedUser;
        private ObservableCollection<OTAssignedUser> userToAssingedUser;
        List<OTAssignedUser> cloneOTAssignedUser = new List<OTAssignedUser>();

        private string infoTooltipBackColor;

        private OtItem selectedOtItem;
        private List<OtItem> clonedOTItems;//[Sudhir.Jangra][GEOS2-5635]
        private ObservableCollection<Ots> pendingReviewList;
        private Ots selectedPendingReview;
        private ObservableCollection<Ots> articleReviewChangedList;
        private object geosAppSettingList;
        #endregion

        #region Public Properties

        public string InfoTooltipBackColor
        {
            get
            {
                return infoTooltipBackColor;
            }

            set
            {
                infoTooltipBackColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InfoTooltipBackColor"));
            }
        }


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
        public Bitmap ReportBMCHeaderImage { get; set; }
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


        public ObservableCollection<OTWorkingTime> WorkLogItemList
        {
            get { return workLogItemList; }
            set
            {
                workLogItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkLogItemList"));
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







        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public bool IsSituationEnabled
        {
            get { return isSituationEnabled; }
            set
            {
                isSituationEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSituationEnabled"));
            }
        }


        public List<Company> OtSite
        {
            get { return otSite; }
            set
            {
                otSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtSite"));
            }
        }

        public bool IsRemoveWorkLog
        {
            get { return isRemoveWorkLog; }
            set
            {
                isRemoveWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRemoveWorkLog"));

            }
        }

        public bool IsEditWorkLog
        {
            get { return isEditWorkLog; }
            set
            {
                isEditWorkLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditWorkLog"));

            }
        }


        public OTWorkingTime SelectedWorklogRow
        {
            get { return selectedWorklogRow; }
            set
            {
                selectedWorklogRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorklogRow"));
            }
        }


        public ObservableCollection<OTAssignedUser> OtAssignedUser
        {
            get { return otAssignedUser; }
            set
            {
                otAssignedUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtAssignedUser"));
            }
        }

        public ObservableCollection<OTAssignedUser> UserToAssingedUser
        {
            get { return userToAssingedUser; }
            set { userToAssingedUser = value; OnPropertyChanged(new PropertyChangedEventArgs("UserToAssingedUser")); }
        }


        public OtItem SelectedOtItem
        {
            get { return selectedOtItem; }
            set
            {
                selectedOtItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOtItem"));
            }
        }


        public List<OtItem> ClonedOTItems
        {
            get { return clonedOTItems; }
            set
            {
                clonedOTItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedOTItems"));
            }
        }

        //[Sudhir.Jangra][GEOs2-5636]
        public ObservableCollection<Ots> PendingReviewList
        {
            get { return pendingReviewList; }
            set
            {
                pendingReviewList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PendingReviewList"));
            }
        }

        public Ots SelectedPendingReview
        {
            get { return selectedPendingReview; }
            set
            {
                selectedPendingReview = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPendingReview"));
            }
        }

        public ObservableCollection<Ots> ArticleReviewChangedList
        {
            get { return articleReviewChangedList; }
            set
            {
                articleReviewChangedList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleReviewChangedList"));
            }
        }

        public object GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        #endregion

        #region ICommands
        public ICommand CommandPendingWorkOrderItemShowCancelButton { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }

        public ICommand PrintCommand { get; set; }
        public ICommand WorkOrderItemDetailsViewHyperlinkClickCommand { get; set; }
        public ICommand PrintWorkOrderItemCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand DNHyperlinkClickCommand { get; set; }
        public ICommand ExportWorkLogCommand { get; set; }
        public ICommand CommentsShipmentGridDoubleClickCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        //  public ICommand OpenWorkflowDiagramCommand { get; set; }

        public ICommand WorkflowButtonClickCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }

        public ICommand DeleteWorkLogCommand { get; set; }

        public ICommand EditWorkLogDoubleClickCommand { get; set; }

        public ICommand ImageClickCommand { get; set; }

        public ICommand CurrentItemChangedCommand { get; set; }

        public ICommand ReviewedButtonCommand { get; set; }

        public ICommand PendingReviewHyperLinkCommand { get; set; }//[Sudhir.jangra][GEOS2-5636]

        public ICommand ArticlesReviewedButtonCommand { get; set; }

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

        #region Constructor
		// [nsatpute][13-01-2025][GEOS2-6443]
        public EditPendingReviewViewModel(Object geosAppSettingList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsViewModel()...", category: Category.Info, priority: Priority.Low);
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 86;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;

                PopupMenuShowingCommand = new DelegateCommand<OpenPopupEventArgs>(OpenPopupDateEditAction);
                CommandPendingWorkOrderItemShowCancelButton = new DelegateCommand<object>(CommandPendingWorkOrderItemShowCancelAction);
                WorkOrderItemDetailsViewHyperlinkClickCommand = new DelegateCommand<object>(WorkOrderItemDetailsViewHyperlinkClickCommandAction);

                PrintCommand = new DelegateCommand<object>(PrintCommandAction);

                ExportButtonCommand = new RelayCommand(new Action<object>(ExportStockLogButtonCommandAction));
                DNHyperlinkClickCommand = new DelegateCommand<object>(DNHyperlinkClickCommandAction);
                ExportWorkLogCommand = new RelayCommand(new Action<object>(ExportWorkLogCommandAction));
                CommentsShipmentGridDoubleClickCommand = new DelegateCommand<object>(EditShipmentAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                //  OpenWorkflowDiagramCommand = new DelegateCommand<object>(OpenWorkflowDiagramCommandAction);
                WorkflowButtonClickCommand = new DelegateCommand<object>(WorkflowButtonClickCommandAction);

                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);

                DeleteWorkLogCommand = new DelegateCommand<object>(DeleteWorklogRowCommandAction);

                ImageClickCommand = new DelegateCommand<object>(ImageClickCommandAction);

                CurrentItemChangedCommand = new DelegateCommand<object>(CurrentItemChangedCommandAction);

                ReviewedButtonCommand = new RelayCommand(new Action<object>(ReviewedButtonCommandAction));

                ArticlesReviewedButtonCommand = new RelayCommand(new Action<object>(ArticlesReviewedButtonCommandAction));

                EditWorkLogDoubleClickCommand = new DelegateCommand<object>(EditWorklog);

                PendingReviewHyperLinkCommand = new DelegateCommand<object>(ShowSelectedGridRowItemWindowAction);

                ReportHeaderImage = new Bitmap(Emdep.Geos.Modules.SRM.Properties.Resources.Emdep_logo_mini);
                ReportBMCHeaderImage = new Bitmap(Emdep.Geos.Modules.SRM.Properties.Resources.TripExpenseReportLogo);

                if (geosAppSettingList == null)
                    GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");
                else
                    GeosAppSettingList = geosAppSettingList;

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

                string myTempFile = Path.Combine(Path.GetTempPath(), "EmdepLogo.jpg");

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
        public static string ModifyOTCode(string otCode)
        {
            // Replace "OT" with "."
            string result = otCode.Replace("OT", ".");

            // Trim spaces around the period
            result = result.Replace(" .", ".").Replace(". ", ".");

            // Use a regular expression to find and format numbers from 1 to 99 with leading zero
            result = Regex.Replace(result, @"\b(\d{1,2})\b", match =>
            {
                int number = int.Parse(match.Value);
                return number >= 1 && number <= 99 ? number.ToString("D2") : match.Value;
            });

            return result;
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                PendingReviewReport report = new PendingReviewReport();


                //[Header Image]
                report.xrEmdepTestboardPicture.Image = ReportHeaderImage;
                report.xrEmdepTestboardPicture.Height = ReportHeaderImage.Height;
                report.xrEmdepTestboardPicture.WidthF = ReportHeaderImage.Width;
                report.xrPictureBox1.Image = ReportBMCHeaderImage;
                report.xrPictureBox1.Height = ReportBMCHeaderImage.Height;
                report.xrPictureBox1.WidthF = ReportBMCHeaderImage.Width;
                //[Header Title]
                report.xrPendingReviewReportHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 14, System.Drawing.FontStyle.Bold);
                report.xrPendingReviewReportHeaderLabel.Text = OT.Quotation.Template.Name;
                report.xrPendingReviewReportHeaderLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                //[OfferNumber]
                report.xrOfferNumberHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrOfferNumberTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrOfferNumberTextLabel.Text = OT.OfferCode;
                //[OT CODE]
                report.xrOTCodeHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrOTCodeTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrOTCodeTextLabel.Text = OT.Code;
                report.xrOTCodeBarCode.Symbology = new DevExpress.XtraPrinting.BarCode.Code128Generator();
                report.xrOTCodeBarCode.Width = 450;
                report.xrOTCodeBarCode.Height = 40;
                report.xrOTCodeBarCode.Text = ModifyOTCode(OT.Code);
                report.xrOTCodeBarCode.ShowText = false;


                //[Customer]
                report.xrCustomerHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrCustomerTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrCustomerTextLabel.Text = OT.CustomerNameForReport;
                //[PO Number]
                report.xrPONumberHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrPONumberTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrPONumberTextLabel.Text = OT.PoCode;

                //[PO Date]
                report.xrPODateHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrPODateTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrPODateTextLabel.Text = OT.PoDate?.ToString("dd/MM/yyyy");

                //[Creation Date]
                report.xrCreationDateHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrCreationDateTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrCreationDateTextLabel.Text = OT.CreationDate?.ToString("dd/MM/yyyy");

                //[Offer Owner]
                report.xrOfferOwnerHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrOfferOwnerTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrOfferOwnerTextLabel.Text = OT.OfferOwner;

                //[Expected Delivery]
                report.xrExpectedDeliveryHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrExpectedDeliveryTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrExpectedDeliveryTextLabel.Text = OT.DeliveryDate?.ToString("dd/MM/yyyy");


                //[Item List]
                //[nsatpute][29.10.2025][GEOS2-8876]
                PendingReviewItemSubReport pendingReviewItemSubReport = new PendingReviewItemSubReport();
                pendingReviewItemSubReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                if (pendingReviewItemSubReport.xrTable1 != null)
                    pendingReviewItemSubReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                //  pendingReviewItemSubReport.xrTable2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                //  pendingReviewItemSubReport.xrTable3.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                if (pendingReviewItemSubReport.xrTable4 != null)
                    pendingReviewItemSubReport.xrTable4.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                //pendingReviewItemSubReport.objectDataSource2.DataSource = OT.OtItems.Where(x => x.ParentId == -1);
                if (pendingReviewItemSubReport.objectDataSource2 != null)
                    pendingReviewItemSubReport.objectDataSource2.DataSource = OT.OtItems;
                report.xrPendingReviewItemsSubreport.ReportSource = pendingReviewItemSubReport;



                string subTotal = string.Empty;
                decimal total = 0;
                foreach (var item in OT.OtItems.Where(x => x.ParentId == -1))
                {
                    total += (item.RevisionItem.UnitPrice * item.RevisionItem.Quantity);
                }
                if (OT.Currency != null)
                {
                    subTotal = String.Format("{0}  {1}", total.ToString("n4", CultureInfo.CurrentCulture), OT.Currency.Symbol);
                }
                else
                {
                    subTotal = String.Format("{0}  {1}", total.ToString("n4", CultureInfo.CurrentCulture), "");
                }



                //[Sub Total]
                report.xrSubTotalHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrSubTotalTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrSubTotalTextLabel.Text = subTotal;

                //[Emdep Site]
                report.xrEmdepSiteLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrEmdepSiteLabel.Text = "www.emdep.com";
                //[Footer Date]
                report.xrFooterDateTimeLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                report.xrFooterDateTimeLabel.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");


                //report.Margins = new System.Drawing.Printing.Margins(10, 10, 10, 10); // Example margins
                //report.PageHeight = 1169;
                // report.PageWidth = 1200;
                report.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
                report.Landscape = true;

                report.ExportOptions.PrintPreview.DefaultFileName = OT.Code;
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = report;



                report.CreateDocument();
                window.Show();




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
                ArticleReviewChangedList = new ObservableCollection<Ots>();
                idOtTemp = idOt;
                objWarehouseTemp = objWarehouse;
                WarehouseDetails = objWarehouse;
                ObjCompany = objWarehouse.Company;

                //[Sudhir.Jangra][GEOS2-6049]
                // [nsatpute][13-01-2025][GEOS2-6443]
                OT = SRMService.GetPendingReviewByIdOt_V2620(idOt, objWarehouse); // [nsatpute][13-01-2025][GEOS2-6443]
                OT.Quotation.Site.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + OT?.Quotation?.Site?.Country?.Iso + ".png";
                //[Sudhir.Jangra][GEOS2-6391]
                // OT = SRMService.GetPendingReviewByIdOt_V2560(idOt, objWarehouse);


                if (!String.IsNullOrEmpty(reference))
                    MySearchText = reference;
                else
                    MySearchText = string.Empty;

                DownloadRemainingQuantity = OT.OtItems.Sum(oti => oti.RevisionItem.RemainingQuantity);
                OtItem item = OT.OtItems.FirstOrDefault();
                SelectedOtItem = OT.OtItems.FirstOrDefault();
                if (ClonedOTItems == null)
                {
                    ClonedOTItems = new List<OtItem>();
                }
                foreach (var ot in OT.OtItems)
                {
                    ClonedOTItems.Add((OtItem)ot.Clone());
                }

                #region GEOS2-4539 
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 86))
                {
                    IsSituationEnabled = true;
                }
                else
                {
                    IsSituationEnabled = false;
                }
                #endregion
                UserImageList = new List<UserShortDetail>();

                // [nsatpute][13-01-2025][GEOS2-6443]
                //WorkLogItemList = new ObservableCollection<OTWorkingTime>(SRMService.GetOTWorkingTimeDetails_V2540(idOtTemp, objWarehouse));
                WorkLogItemList = new ObservableCollection<OTWorkingTime>();
                List<UserShortDetail> tempList = WorkLogItemList.Select(x => x.UserShortDetail).ToList();
                UserImageList = tempList.GroupBy(x => x.IdUser).Select(x => x.First()).ToList();

                TimeSpan worklogTotalTime = new TimeSpan(WorkLogItemList.Sum(r => r.TotalTime.Ticks));

                WorklogTotalTime = string.Format("{0}H {1}M", worklogTotalTime.Hours, worklogTotalTime.Minutes);
                //End
                SetMaximizedElementPosition();
                ListShipment = new List<Shipment>();
                ListShipment = CrmStartUp.GetAllShipmentsByOfferId(ObjCompany, OT.Quotation.IdOffer);
                WorkflowTransitionList = new List<WorkflowTransition>(SRMService.GetAllWorkflowTransitions_V2540());
                WorkflowStatusList = new List<WorkflowStatus>(SRMService.GetAllWorkflowStatus_V2540());
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

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 35))
                {
                    IsRemoveWorkLog = true;
                }

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 34))
                {
                    IsEditWorkLog = true;
                }

                FillOtAssignUsersList(OtSite.FirstOrDefault(), idOt);

                GeosAppSetting GeosAppSetting = WorkbenchService.GetGeosAppSettings(37);
                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;

                foreach (var ite in OT.OtItems.Where(x => x.ParentId == -1))
                {
                    if (OT.Currency != null)
                    {
                        ite.UnitPriceWithSymbol = String.Format("{0} {1}", ite.RevisionItem.UnitPrice.ToString("n4", CultureInfo.CurrentCulture), OT.Currency.Symbol);

                        ite.RevisionItem.TotalPrice = String.Format("{0} {1}", Convert.ToDecimal(ite.RevisionItem.UnitPrice * ite.RevisionItem.Quantity).ToString("n4", CultureInfo.CurrentCulture), OT.Currency.Symbol);
                    }
                    else
                    {
                        ite.UnitPriceWithSymbol = String.Format("{0} {1}", ite.RevisionItem.UnitPrice.ToString("n4", CultureInfo.CurrentCulture), "");
                        ite.RevisionItem.TotalPrice = String.Format("{0} {1}", Convert.ToDecimal(ite.RevisionItem.UnitPrice * ite.RevisionItem.Quantity).ToString("n4", CultureInfo.CurrentCulture), "");
                    }

                    ite.NumItemBarCodeText = GetFormattedCode(OT.Code, ite.RevisionItem.NumItem);
                }

                foreach (var data in OT.OtItems.Where(x => x.ParentId == -1))
                {
                    data.RevisionItem.RemainingQuantity = (Int64)(data.RevisionItem.Quantity - Math.Abs(data.RevisionItem.DownloadedQuantity));
                }

                foreach (var num in OT.OtItems)
                {
                    double result;
                    if (double.TryParse(num.RevisionItem.NumItem, out result))
                    {
                        num.RevisionItem.ItemNum = result;
                    }
                    else
                    {
                        // Handle the case where NumItem is not a valid double
                        // For example, log an error or assign a default value
                        // num.RevisionItem.ItemNum = 0; // Default value or appropriate action
                    }
                }


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
        public static string GetFormattedCode(string otCode, string revisionNumItem)
        {
            string result;

            if (char.IsDigit(otCode[0]))
            {
                string yearLastTwoDigits = otCode.Substring(2, 2);
                string[] parts = otCode.Split(new[] { '-' }, 2);
                if (parts.Length < 2)
                {
                    throw new ArgumentException("Input code must contain a hyphen.");
                }
                string afterHyphen = parts[1].Split(new[] { " OT " }, StringSplitOptions.None)[0].Trim();
                string formattedCode = afterHyphen.Replace(" ", "").Replace(".", "");
                int revisionNumber = int.Parse(revisionNumItem);
                string formattedRevisionNumItem = revisionNumber.ToString("D3");
                result = yearLastTwoDigits + formattedCode + formattedRevisionNumItem;
            }
            else
            {
                int index = otCode.IndexOfAny("0123456789".ToCharArray());
                if (index == -1)
                {
                    throw new ArgumentException("Input code must contain digits.");
                }
                string prefix = otCode.Substring(0, index).Trim();
                string remaining = otCode.Substring(index);
                string digitsPart = remaining.Split(new[] { " OT " }, StringSplitOptions.None)[0].Trim();
                string suffix = remaining.Split(new[] { " OT " }, StringSplitOptions.None)[1].Trim();
                string formattedCode = digitsPart.Replace(" ", "").Replace(".", "");
                int revisionNumber = int.Parse(revisionNumItem);
                string formattedRevisionNumItem = revisionNumber.ToString("D3");
                string firstTwoDigits = formattedCode.Substring(0, 2);
                result = firstTwoDigits + prefix + formattedCode + formattedRevisionNumItem;
            }

            return result;
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

        //private void PrintWorkOrderItem(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method PrintWorkOrderItem....", category: Category.Info, priority: Priority.Low);

        //        TreeListView detailView = (TreeListView)((object[])obj)[0];
        //        // OtItem otItem = (OtItem)obj;
        //        OtItem otItem = (OtItem)((object[])obj)[1];
        //        WorkOrderPrintView workOrderPrintView = new WorkOrderPrintView();
        //        WorkOrderPrintViewModel workOrderPrintViewModel = new WorkOrderPrintViewModel();
        //        EventHandler handler = delegate { workOrderPrintView.Close(); };
        //        workOrderPrintViewModel.RequestClose += handler; //392235
        //        workOrderPrintView.DataContext = workOrderPrintViewModel;
        //        workOrderPrintViewModel.Init(otItem, OT);
        //        var ownerInfo = (detailView as FrameworkElement);
        //        workOrderPrintView.Owner = Window.GetWindow(ownerInfo);
        //        workOrderPrintView.ShowDialogWindow();

        //        GeosApplication.Instance.Logger.Log("Method PrintWorkOrderItem....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method PrintWorkOrderItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}




        /// <summary>
        /// Method for cloase window.
        /// </summary>
        /// <param name="obj"></param>
        private void CommandPendingWorkOrderItemShowCancelAction(object obj)
        {
            try
            {
                List<OtItem> tempList = new List<OtItem>();
                List<SRMOtItemsComment> changeLog = new List<SRMOtItemsComment>();
                List<OtItem> otList = OT.OtItems.Where(x => x.ParentId == -1).ToList();
                List<OtItem> clonedOTList = ClonedOTItems.Where(x => x.ParentId == -1).ToList();
                foreach (var item in clonedOTList)
                {
                    if (otList.Any(x => x.IdRevisionItem == item.IdRevisionItem && x.ParentId == -1))
                    {
                        OtItem updated = otList.FirstOrDefault(x => x.IdRevisionItem == item.IdRevisionItem && x.ParentId == -1);
                        if (updated.IsReviewValidated != item.IsReviewValidated)
                        {
                            OtItem data = (OtItem)updated.Clone();
                            tempList.Add(data);
                            if (data.IsReviewValidated)
                            {
                                changeLog.Add(new SRMOtItemsComment()
                                {
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    CommentDate = GeosApplication.Instance.ServerDateTime,
                                    Idrevisionitem = data.IdRevisionItem,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("OtItemValidatedChangeLog").ToString(), data.RevisionItem.WarehouseProduct.Article.Reference)
                                });
                            }
                            else
                            {
                                changeLog.Add(new SRMOtItemsComment()
                                {
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    CommentDate = GeosApplication.Instance.ServerDateTime,
                                    Idrevisionitem = data.IdRevisionItem,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("OtItemNotValidatedChangeLog").ToString(), data.RevisionItem.WarehouseProduct.Article.Reference)
                                });
                            }


                        }
                    }
                }

                if ((tempList != null && tempList.Count > 0 && changeLog != null && changeLog.Count > 0) || (ArticleReviewChangedList != null && ArticleReviewChangedList.Count > 0))
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["PendingReviewGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        AcceptButtonCommandAction(null);
                    }
                   // if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                //if (IsSave)
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ReviewedStatusUpdated").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                //}
                 if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandPendingWorkOrderItemShowCancelAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TreeListView detailView = (TreeListView)obj;
                OtItem otItem = (OtItem)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                articleDetailsViewModel.IsSRMViewClicked = true;
                if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                    Warehouses warehouse = plantOwners.FirstOrDefault(x => x.IdWarehouse == objWarehouseTemp.IdWarehouse);
                    if (articleDetailsViewModel.SRMWarehouse == null)
                    {
                        articleDetailsViewModel.SRMWarehouse = new Warehouses();
                    }
                    articleDetailsViewModel.SRMWarehouse = warehouse;
                    long warehouseId = warehouse.IdWarehouse;
                    articleDetailsViewModel.Init(otItem.RevisionItem.WarehouseProduct.Article.Reference, warehouseId);
                }

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
                            return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TableView tblView = (TableView)obj;
                ArticlesStock ac = (ArticlesStock)tblView.DataControl.CurrentItem;
                EditDeliveryNoteView editDeliveryNoteView = new EditDeliveryNoteView();
                EditDeliveryNoteViewModel editDeliveryNoteViewModel = new EditDeliveryNoteViewModel();
                EventHandler handle = delegate { editDeliveryNoteView.Close(); };
                editDeliveryNoteViewModel.RequestClose += handle;
                WarehouseDeliveryNote wdn = SRMService.GetWarehouseDeliveryNoteById_V2540(ac.WarehouseDeliveryNoteItem.IdWarehouseDeliveryNote);


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
                            return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
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
		/// [003][nsatpute][13-01-2025][GEOS2-6443][Some modifications in OT validation (1/3)]
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
                    // [nsatpute][22-01-2025][GEOS2-6445]
                    //Int32 RemainingQuantity = gridControlViewNew.Columns.Where(a => a.HeaderCaption.ToString().ToLower().Equals("Remaining Quantity".ToLower())).FirstOrDefault().ActualVisibleIndex;

                    //if (TotalQuantity < RemainingQuantity)
                    //{
                    foreach (DevExpress.Xpf.Grid.GridColumn item in gridControlViewNew.Columns)
                        {
                            //if (item.HeaderCaption.ToString().ToLower().Equals("Total Quantity".ToLower()))
                            //{
                            //    item.ActualVisibleIndex = RemainingQuantity;
                            //    item.VisibleIndex = RemainingQuantity;
                            //}
                            //else 
                            if (item.HeaderCaption.ToString().ToLower().Equals("Remaining Quantity".ToLower()))
                            {
                                item.ActualVisibleIndex = TotalQuantity;
                                item.VisibleIndex = TotalQuantity;
                            }
                        }
                    //}

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

        //private void OpenWorkflowDiagramCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()...", category: Category.Info, priority: Priority.Low);
        //        WorkflowDiagramsViewModel workflowDiagramViewModel = new WorkflowDiagramsViewModel();
        //        WorkFlowDiagramsView workflowDiagramView = new WorkFlowDiagramsView();
        //        EventHandler handle = delegate { workflowDiagramView.Close(); };
        //        workflowDiagramViewModel.RequestClose += handle;
        //        workflowDiagramViewModel.WorkflowStatusList = WorkflowStatusList;
        //        workflowDiagramViewModel.WorkflowTransitionList = WorkflowTransitionList.OrderByDescending(a => a.IdWorkflowTransition).ToList();
        //        workflowDiagramView.DataContext = workflowDiagramViewModel;

        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        workflowDiagramView.ShowDialog();

        //        GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenWorkflowDiagramCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}

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

            //  IsSaveChanges = WarehouseService.UpdateWorkflowStatusInOT_V2320(OT.IdOT, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByOTList);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                // List<OtItem> foundRow = OT.OtItems.Where(x => x.IsUpdatedRow == true).ToList();
                //if (foundRow.Count > 0)
                //{
                List<OtItem> tempList = new List<OtItem>();
                List<SRMOtItemsComment> changeLog = new List<SRMOtItemsComment>();


                foreach (var item in ClonedOTItems.Where(x => x.ParentId == -1))
                {
                    if (OT.OtItems.Any(x => x.IdRevisionItem == item.IdRevisionItem && x.ParentId == -1))
                    {
                        OtItem updated = OT.OtItems.FirstOrDefault(x => x.IdRevisionItem == item.IdRevisionItem && x.ParentId == -1);
                        if (updated.IsReviewValidated != item.IsReviewValidated)
                        {
                            OtItem data = (OtItem)updated.Clone();
                            tempList.Add(data);
                            if (data.IsReviewValidated)
                            {
                                changeLog.Add(new SRMOtItemsComment()
                                {
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    CommentDate = GeosApplication.Instance.ServerDateTime,
                                    Idrevisionitem = data.IdRevisionItem,
                                    //  Comments = string.Format(System.Windows.Application.Current.FindResource("OtItemValidatedChangeLog").ToString(), data.RevisionItem.WarehouseProduct.Article.Reference)
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleOtItemValidatedChangeLog").ToString(), OT.Code, data.RevisionItem.WarehouseProduct.Article.Reference)

                                });
                            }
                            else
                            {
                                changeLog.Add(new SRMOtItemsComment()
                                {
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    CommentDate = GeosApplication.Instance.ServerDateTime,
                                    Idrevisionitem = data.IdRevisionItem,
                                    //  Comments = string.Format(System.Windows.Application.Current.FindResource("OtItemNotValidatedChangeLog").ToString(), data.RevisionItem.WarehouseProduct.Article.Reference)
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleOtItemNotValidatedChangeLog").ToString(), OT.Code, data.RevisionItem.WarehouseProduct.Article.Reference)

                                });
                            }


                        }
                    }
                }

                if (ArticleReviewChangedList != null && ArticleReviewChangedList.Count > 0)
                {
                    foreach (var item in ArticleReviewChangedList)
                    {
                        List<SRMOtItemsComment> articleChangeLog = new List<SRMOtItemsComment>();
                        if (item.IsReviewValidated)
                        {
                            articleChangeLog.Add(new SRMOtItemsComment()
                            {
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                CommentDate = GeosApplication.Instance.ServerDateTime,
                                Idrevisionitem = item.IdRevisionItem,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleOtItemValidatedChangeLog").ToString(), item.MergeCode, SelectedOtItem.RevisionItem.WarehouseProduct.Article.Reference)
                            });
                        }
                        else
                        {
                            articleChangeLog.Add(new SRMOtItemsComment()
                            {
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                CommentDate = GeosApplication.Instance.ServerDateTime,
                                Idrevisionitem = item.IdRevisionItem,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleOtItemNotValidatedChangeLog").ToString(), item.MergeCode, SelectedOtItem.RevisionItem.WarehouseProduct.Article.Reference)
                            });
                        }
                        //List<OtItem> otList = new List<OtItem>(item.OtItems.Where(x => x.RevisionItem.WarehouseProduct.Article.Reference == SelectedOtItem.RevisionItem.WarehouseProduct.Article.Reference));
                        //otList.ToList().ForEach(x => x.IsReviewValidated = item.IsReviewValidated);
                        IsSave = SRMService.UpdateEditWorkOrderOTItemsForArticles_V2540(item, articleChangeLog);
                    }

                }

                if (tempList != null && tempList.Count > 0 && changeLog != null && changeLog.Count > 0)
                {
                    tempList.ToList().ForEach(x => x.RevisionItem.WarehouseProduct.Article.ArticleImage = null);
                    IsSave = SRMService.UpdateEditWorkOrderOTItems_V2540(tempList, changeLog);

                    //  CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ReviewedStatusUpdated").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                if (IsSave)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ReviewedStatusUpdated").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                // }
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
                            // result = WarehouseService.DeleteWorkLog_V2530(OtSite, OTworktime.IdOTWorkingTime);
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
                SRMEditWorkLogView editWorklLogView = new SRMEditWorkLogView();
                SRMEditWorkLogViewModel editWorkLogViewModel = new SRMEditWorkLogViewModel();
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
                OtAssignedUser = new ObservableCollection<OTAssignedUser>(SRMService.GetOTAssignedUsers_V2540(company, idOT).ToList());

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
                            OtAssignedUser[i].UserShortDetail.UserImage = SRMCommon.Instance.ByteArrayToBitmapImage(UserProfileImageByte);
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (OtAssignedUser[i].UserShortDetail.IdUserGender == 1)
                                    OtAssignedUser[i].UserShortDetail.UserImage = SRMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png");
                                else if (OtAssignedUser[i].UserShortDetail.IdUserGender == 2)
                                    OtAssignedUser[i].UserShortDetail.UserImage = SRMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png");
                            }
                            else
                            {
                                if (OtAssignedUser[i].UserShortDetail.IdUserGender == 1)
                                    OtAssignedUser[i].UserShortDetail.UserImage = SRMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png");
                                else if (OtAssignedUser[i].UserShortDetail.IdUserGender == 2)
                                    OtAssignedUser[i].UserShortDetail.UserImage = SRMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png");
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

        private void ImageClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....", category: Category.Info, priority: Priority.Low);

            try
            {
                OtItem otItem = (OtItem)obj;

                OT.OtItems.Where(a => a.IdOTItem != otItem.IdOTItem && a.ShowComment == true && a.ParentId == -1).ToList().ForEach(a => { a.ShowComment = false; });
                OT.OtItems.Where(a => a.IdOTItem == otItem.IdOTItem && a.ParentId == -1).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ImageClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }


        private void CurrentItemChangedCommandAction(object obj)
        {
            try
            {
                if (SelectedOtItem != null &&
                    SelectedOtItem.RevisionItem != null &&
                    SelectedOtItem.RevisionItem.WarehouseProduct != null
                    && SelectedOtItem.RevisionItem.WarehouseProduct.Article != null)
                {




                    if (SRMCommon.Instance.Articles != null)
                    {
                        if (SRMCommon.Instance.Articles.Any(x => x.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle))
                        {
                            Article articleImage = SRMService.GetArticleDetails_V2550(SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle, objWarehouseTemp.IdSite);
                            Article article = SRMCommon.Instance.Articles.FirstOrDefault(x => x.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle);


                            SelectedOtItem.RevisionItem.WarehouseProduct.Article = article;
                            if (articleImage.ArticleImageInBytes == null)
                            {
                                articleImage.ArticleImage = new BitmapImage(new Uri("/Emdep.Geos.Modules.SRM;component/Assets/Images/EmptyImage.png", UriKind.Relative));
                            }
                            else if (article.ArticleImageInBytes != null)
                            {
                                articleImage.ArticleImage = null;
                                articleImage.ArticleImage =SRMCommon.Instance.ByteArrayToBitmapImage(article.ArticleImageInBytes);
                            }


                            SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = articleImage.ArticleImage;


                            //PendingReviewList = new ObservableCollection<Ots>(SRMCommon.Instance.PendingWorkOrdersList.Where(workOrder => workOrder.OtItems
                            // .Any(otItem => otItem.RevisionItem.WarehouseProduct.Article.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle))
                            //.ToList());
                            PendingReviewList = new ObservableCollection<Ots>(SRMService.GetPendingReviewListForArticle_V2540(idOtTemp, SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle, objWarehouseTemp));

                            foreach (var item in PendingReviewList)
                            {
                                if (ArticleReviewChangedList != null)
                                {
                                    if (ArticleReviewChangedList.Any(x => x.IdRevisionItem == item.IdRevisionItem))
                                    {
                                        item.IsReviewValidated = ArticleReviewChangedList.FirstOrDefault(x => x.IdRevisionItem == item.IdRevisionItem).IsReviewValidated;
                                    }
                                }
                            }


                        }
                        else
                        {
                            Article article = SRMService.GetArticleDetails_V2550(SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle, objWarehouseTemp.IdSite);
                            if (article.ArticleImageInBytes == null)
                            {
                                article.ArticleImage = new BitmapImage(new Uri("/Emdep.Geos.Modules.SRM;component/Assets/Images/EmptyImage.png", UriKind.Relative));
                            }
                            else if (article.ArticleImageInBytes != null)
                            {
                                article.ArticleImage = SRMCommon.Instance.ByteArrayToBitmapImage(article.ArticleImageInBytes);
                            }
                            if (SRMCommon.Instance.Articles == null)
                            {
                                SRMCommon.Instance.Articles = new List<Article>();
                            }
                            SRMCommon.Instance.Articles.Add(article);
                            SelectedOtItem.RevisionItem.WarehouseProduct.Article = article;
                            SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = article.ArticleImage;

                            PendingReviewList = new ObservableCollection<Ots>(SRMService.GetPendingReviewListForArticle_V2540(idOtTemp, SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle, objWarehouseTemp));
                        }
                    }
                    else
                    {

                        Article article = SRMService.GetArticleDetails_V2550(SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle, objWarehouseTemp.IdSite);


                        if (article.ArticleImageInBytes == null)
                        {
                            article.ArticleImage = new BitmapImage(new Uri("/Emdep.Geos.Modules.SRM;component/Assets/Images/EmptyImage.png", UriKind.Relative));
                        }
                        else if (article.ArticleImageInBytes != null)
                        {
                            article.ArticleImage = SRMCommon.Instance.ByteArrayToBitmapImage(article.ArticleImageInBytes);
                        }

                        if (SRMCommon.Instance.Articles == null)
                        {
                            SRMCommon.Instance.Articles = new List<Article>();
                        }
                        SRMCommon.Instance.Articles.Add(article);
                        SelectedOtItem.RevisionItem.WarehouseProduct.Article = article;
                        SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = article.ArticleImage;


                        PendingReviewList = new ObservableCollection<Ots>(SRMService.GetPendingReviewListForArticle_V2540(idOtTemp, SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle, objWarehouseTemp));

                    }

                    if (SelectedOtItem.ParentId != -1)
                    {
                        PendingReviewList = new ObservableCollection<Ots>();
                    }


                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CurrentItemChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        private void ReviewedButtonCommandAction(object obj)
        {
            if (SelectedOtItem != null)
            {
                if (SelectedOtItem.IsReviewValidated)
                {
                    OT.OtItems.Where(x => x.IdRevisionItem == SelectedOtItem.IdRevisionItem && x.ParentId == -1).ToList().ForEach(a => a.IsReviewValidated = false);
                    OT.OtItems.Where(x => x.IdRevisionItem == SelectedOtItem.IdRevisionItem && x.ParentId == -1).ToList().ForEach(a => a.ValidatedCount = 0);
                }
                else if (!SelectedOtItem.IsReviewValidated)
                {
                    OT.OtItems.Where(x => x.IdRevisionItem == SelectedOtItem.IdRevisionItem && x.ParentId == -1).ToList().ForEach(a => a.IsReviewValidated = true);
                    OT.OtItems.Where(x => x.IdRevisionItem == SelectedOtItem.IdRevisionItem && x.ParentId == -1).ToList().ForEach(a => a.ValidatedCount = 1);
                }
            }
        }

        private void ArticlesReviewedButtonCommandAction(object obj)
        {
            try
            {
                if (SelectedPendingReview != null)
                {
                    bool isChanged = false;
                    bool isOriginalState = SelectedPendingReview.IsReviewValidated;

                    if (isOriginalState)
                    {
                        PendingReviewList.Where(c => c.IdOT == SelectedPendingReview.IdOT).ToList().ForEach(b => b.IsReviewValidated = false);
                        PendingReviewList.Where(c => c.IdOT == SelectedPendingReview.IdOT).ToList().ForEach(b => b.ValidatedCount = 0);
                        isChanged = true;
                    }
                    else
                    {
                        PendingReviewList.Where(c => c.IdOT == SelectedPendingReview.IdOT).ToList().ForEach(b => b.IsReviewValidated = true);
                        PendingReviewList.Where(c => c.IdOT == SelectedPendingReview.IdOT).ToList().ForEach(b => b.ValidatedCount = 1);
                        isChanged = true;
                    }


                    if (isChanged)
                    {
                        isChanged = false;
                        var existingReview = ArticleReviewChangedList.FirstOrDefault(r => r.IdRevisionItem == SelectedPendingReview.IdRevisionItem);
                        if (existingReview != null)
                        {
                            ArticleReviewChangedList.Remove(existingReview);
                        }
                        if (SelectedPendingReview.IsReviewValidated != isOriginalState)
                        {
                            ArticleReviewChangedList.Add(SelectedPendingReview);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ArticlesReviewedButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        private void ShowSelectedGridRowItemWindowAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TableView detailView = (TableView)obj;
                EditPendingReviewViewModel editPendingReviewViewModel = new EditPendingReviewViewModel(GeosAppSettingList);
                EditPendingReviewView editPendingReviewView = new EditPendingReviewView();

                EventHandler handle = delegate { editPendingReviewView.Close(); };
                editPendingReviewViewModel.RequestClose += handle;

                Ots ot = (Ots)detailView.Grid.SelectedItem;
                if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                    if (editPendingReviewViewModel.OtSite == null)
                    {
                        editPendingReviewViewModel.OtSite = new List<Company>();
                    }
                    foreach (var item in plantOwners)
                    {
                        editPendingReviewViewModel.OtSite.Add(item.Company);
                    }
                    editPendingReviewViewModel.Init(ot.IdOT, plantOwners.FirstOrDefault(x => x.IdWarehouse == objWarehouseTemp.IdWarehouse));
                }


                editPendingReviewView.DataContext = editPendingReviewViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (detailView as FrameworkElement);
                editPendingReviewView.Owner = Window.GetWindow(ownerInfo);
                editPendingReviewView.ShowDialogWindow();


                if (editPendingReviewViewModel.IsSave == true)
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
                            return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }


                    if (SelectedPendingReview.IsReviewValidated)
                    {
                        PendingReviewList.Where(c => c.IdOT == SelectedPendingReview.IdOT).ToList().ForEach(b => b.IsReviewValidated = false);
                    }
                    else
                    {
                        PendingReviewList.Where(c => c.IdOT == SelectedPendingReview.IdOT).ToList().ForEach(b => b.IsReviewValidated = true);
                    }


                    // Article article = SRMService.GetArticleDetails_V2540(SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                    //[Sudhir.Jangra][GEOS2-5636]
                    Article article = SRMService.GetArticleDetails_V2550(SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle, objWarehouseTemp.IdSite);


                    if (article.ArticleImageInBytes == null)
                    {
                        article.ArticleImage = new BitmapImage(new Uri("/Emdep.Geos.Modules.SRM;component/Assets/Images/EmptyImage.png", UriKind.Relative));
                    }
                    else if (article.ArticleImageInBytes != null)
                    {
                        article.ArticleImage = SRMCommon.Instance.ByteArrayToBitmapImage(article.ArticleImageInBytes);
                    }

                    if (SRMCommon.Instance.Articles == null)
                    {
                        SRMCommon.Instance.Articles = new List<Article>();
                    }
                    SRMCommon.Instance.Articles.Add(article);
                    SelectedOtItem.RevisionItem.WarehouseProduct.Article = article;
                    SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = article.ArticleImage;

                    //foreach (var item in PendingReviewList)
                    //{
                    //    if (item.OtItems.Any(a => a.IdOTItem == SelectedOtItem.IdOTItem))
                    //    {
                    //        item.IsReviewValidated = SelectedOtItem.IsReviewValidated;
                    //    }
                    //    else
                    //    {
                    //        item.IsReviewValidated = item.OtItems.FirstOrDefault(x => x.RevisionItem.WarehouseProduct.Article.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle).IsReviewValidated;
                    //    }
                    //}

                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                GeosApplication.Instance.Logger.Log("Method ShowSelectedGridRowItemWindowAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedGridRowItemWindowAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }




        #endregion // End Of Methods
    }
}
