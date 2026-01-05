using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;

using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.SRM.Views;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OutlookApp = Microsoft.Office.Interop.Outlook.Application;
using OutlookEmailFrom = Microsoft.Office.Interop.Outlook;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    class ViewPurchaseOrderViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {

        }

        #region Service

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        //ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SRMService = new SRMServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());//[pramod.misal][GEOS2-4451][10-08-2023]
                                                                                                                                  
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

        #region Declaration
        bool isUpdated;
        private bool isSaveChanges;
        private double dialogHeight;
        private double dialogWidth;
        bool isMoreNeeded = true;
        private WarehousePurchaseOrder warehousePurchaseOrder;
        private WorkflowStatus workflowStatus;
        private List<WorkflowStatus> workflowStatusList;
        private List<WorkflowStatus> workflowStatusButtons;
        private WorkflowStatus selectedWorkflowStatusButton;
        private List<WorkflowTransition> workflowTransitionList;

        private ObservableCollection<User> assigneeList;
        private ObservableCollection<User> approverList;
        private Visibility addressvisibility;

        private User selectedAssignee;
        private User selectedApprover;
        private EmailAssigneeAndCreatedBy emailAssigneeAndCreatedBy;//[Sudhir.Jangra][GEOS2-4407][10/05/2023]
        private ObservableCollection<SystemSettings> systemSettingsList;//[Sudhir.jangra][GEOS2-4407][10/05/2023]
        private string emailFrom;//[Sudhir.Jangra][GEOS2-4407][11/05/2023]
        private string emailCC;//[Sudhir.Jangra][GEOS2-4407][11/05/2023]
        private ArticleSupplier selectedArticleSupplier;  // [pramod.misal][GEOS2-4408][20.06.2023]

        ObservableCollection<LogEntriesByWarehousePO> commentsList;//[pramod.misal][GEOS2-4450][27.06.2023]

        List<LogEntriesByWarehousePO> addcommentsList;//[pramod.misal][GEOS2-4450][27.06.2023]
        List<LogEntriesByWarehousePO> updatedCommentsList;
        ObservableCollection<LogEntriesByWarehousePO> deleteCommentsList;//[pramod.misal][GEOS2-4450][27.06.2023]                                                              
        LogEntriesByWarehousePO samComments;//[pramod.misal][GEOS2-4450][27.06.2023]
        private bool isDeleted;//[pramod.misal][GEOS2-4450][27.06.2023]
        private LogEntriesByWarehousePO selectedComment;//[pramod.misal][GEOS2-4450][27.06.2023]
        byte[] userProfileImageByte = null;//[pramod.misal][GEOS2-4450][27.06.2023]
        private ImageSource userProfileImage;//[pramod.misal][GEOS2-4450][27.06.2023]
        private string name;

        ObservableCollection<LogEntriesByWarehousePO> newcommentsList;
        private OtItem tempOtItem;//[pramod.misal][GEOS2-4450][31.06.2023]
        private SRMOtItemsComment commentItem;
        private bool isBusy;
        private string commentButtonText;


        private string contentcountryname;
        string contentcountryimgurl = null;

        private ObservableCollection<WarehouseShippingAddress> shippingAddressList;
        public List<LogEntriesByWarehousePO> ShippinggAddressList { get; set; }

        private Int32 IdArticleSupplierForTOAndCC { get; set; }//[Sudhir.Jangra][GEOS2-5493]

        ObservableCollection<Contacts> toContactList;//[Sudhir.Jangra][GEOS2-5493]
        ObservableCollection<Contacts> ccContactList;//[Sudhir.Jangra][GEOS2-5493]
        #endregion

        #region Properties

        public string Contentcountryname
        {
            get { return contentcountryname; }
            set
            {
                contentcountryname = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Contentcountryname"));
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


        public string CommentButtonText
        {
            get { return commentButtonText; }
            set
            {
                commentButtonText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentButtonText"));
            }
        }


        public Visibility Addressvisibility
        {
            get { return addressvisibility; }
            set
            {
                addressvisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Addressvisibility"));
            }
        }




        public string Contentcountryimgurl
        {
            get { return contentcountryimgurl; }
            set
            {
                contentcountryimgurl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Contentcountryimgurl"));
            }
        }

        public byte[] UserProfileImageByte
        {
            get { return userProfileImageByte; }
            set
            {
                userProfileImageByte = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImageByte"));
            }
        }

        public bool IsUpdated
        {
            get { return isUpdated; }
            set
            {
                isUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdated"));
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

        public bool IsMoreNeeded
        {
            get
            {
                return isMoreNeeded;
            }
            set
            {
                isMoreNeeded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMoreNeeded"));
            }
        }

        public WarehousePurchaseOrder WarehousePurchaseOrder
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

        public List<WorkflowStatus> WorkflowStatusList
        {
            get
            {
                return workflowStatusList;
            }

            set
            {
                workflowStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusList"));
            }
        }
        public List<WorkflowStatus> WorkflowStatusButtons
        {
            get
            {
                return workflowStatusButtons;
            }

            set
            {
                workflowStatusButtons = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusButtons"));
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
        public ObservableCollection<SystemSettings> SystemSettingsList //[Sudhir.jangra][GEOS2-4077][10/05/2023]
        {
            get { return systemSettingsList; }
            set
            {
                systemSettingsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SystemSettingsList"));
            }
        }
        public string EmailFrom  //[Sudhir.jangra][GEOS2-4077][11/05/2023]
        {
            get { return emailFrom; }
            set
            {
                emailFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmailFrom"));
            }
        }
        public string EmailCC  //[Sudhir.jangra][GEOS2-4077][11/05/2023]
        {
            get { return emailCC; }
            set
            {
                emailCC = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmailCC"));
            }
        }



        // [pramod.misal][GEOS2-4408][20.06.2023]
        public ArticleSupplier SelectedArticleSupplier
        {
            get
            {
                return selectedArticleSupplier;
            }

            set
            {
                selectedArticleSupplier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleSupplier"));
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
        public GeosAppSetting Setting { get; set; } //[pramod.misal][GEOS2-4551][10-08-2023]

        //[pramod.misal][GEOS2-4450][27.06.2023]
        public ObservableCollection<LogEntriesByWarehousePO> DeleteCommentsList
        {
            get { return deleteCommentsList; }
            set
            {
                deleteCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommentsList"));
            }
        }

        public List<LogEntriesByWarehousePO> UpdatedCommentsList
        {
            get { return updatedCommentsList; }
            set
            {
                updatedCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedCommentsList"));
            }
        }

        public ObservableCollection<LogEntriesByWarehousePO> CommentsList
        {
            get { return commentsList; }
            set
            {
                commentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsList"));
            }
        }

        public List<LogEntriesByWarehousePO> AddCommentsList
        {
            get { return addcommentsList; }
            set
            {
                addcommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddCommentsList"));
            }
        }


        //[pramod.misal][GEOS2-4450][27.06.2023]
        public LogEntriesByWarehousePO SamComments
        {
            get { return samComments; }
            set
            {
                samComments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SamComments"));
            }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }


        public LogEntriesByWarehousePO SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }

        public ObservableCollection<WarehouseShippingAddress> ShippingAddressList
        {
            get { return shippingAddressList; }
            set
            {
                shippingAddressList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ShippingAddressList"));
            }
        }

        public ImageSource UserProfileImage
        {
            get { return userProfileImage; }
            set
            {
                userProfileImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImage"));
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public ObservableCollection<LogEntriesByWarehousePO> NewcommentsList
        {
            get { return newcommentsList; }
            set
            {
                newcommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewcommentsList"));
            }
        }

        public OtItem TempOtItem
        {
            get { return tempOtItem; }
            set
            {
                tempOtItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempOtItem"));
            }
        }


        public SRMOtItemsComment CommentItem
        {
            get { return commentItem; }
            set
            {
                commentItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentItem"));
            }
        }

        string contentAddress = string.Empty;
        public string ContentAddress
        {
            get { return contentAddress; }
            set
            {
                contentAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentAddress"));
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

        #region ICommands
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand OpenPdfFileCommand { get; set; }
        public ICommand POItemDetailsViewHyperlinkClickCommand { get; set; }
        public ICommand WorkflowButtonClickCommand { get; set; }
        public ICommand OpenWorkflowDiagramCommand { get; set; }
        public ICommand OpenViewSupplierCommand { get; set; }  // [pramod.misal][GEOS2-4408][20.06.2023]
        public ICommand SendEmailCommand { get; set; }
        public ICommand CustomSummaryCommand { get; set; }
        public List<ArticleSupplier> ArticleSupplierList { get; private set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand SetMainContactCommand { get; set; }// [pramod.misal][GEOS2-4451][20.06.2023]
        public ICommand AddCommentsCommand { get; set; }// [pramod.misal][GEOS2-4450][27.06.2023]
        public ICommand CommentsGridDoubleClickCommand { get; set; }// [pramod.misal][GEOS2-4450][17.08.2023]

        #endregion

        #region Constructor

        public ViewPurchaseOrderViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ViewPurchaseOrderViewModel()...", category: Category.Info, priority: Priority.Low);

                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 130;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 50;
                AcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AcceptButtonAction);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonAction);
                OpenPdfFileCommand = new DelegateCommand<object>(OpenPDFDocument);
                POItemDetailsViewHyperlinkClickCommand = new DelegateCommand<object>(POItemDetailsViewHyperlinkClickCommandAction);
                WorkflowButtonClickCommand = new DelegateCommand<object>(WorkflowButtonClickCommandAction);
                OpenWorkflowDiagramCommand = new DelegateCommand<object>(OpenWorkflowDiagramCommandAction);
                OpenViewSupplierCommand = new DelegateCommand<object>(OpenViewSupplierCommandCommandAction);  // [pramod.misal][GEOS2-4408][20.06.2023]
                SendEmailCommand = new DelegateCommand<object>(SendEmailCommandAction);
                CustomSummaryCommand = new DelegateCommand<object>(CustomSummaryCommandAction);
                //SetMainContactCommand = new DelegateCommand<WarehouseShippingAddress>(SetMainContactCommandAction);// [pramod.misal][GEOS2-4451][27.06.2023]
                //SetMainContactCommand = new DelegateCommand<object>(SetMainContactCommandAction);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);// [pramod.misal][GEOS2-4450][27.06.2023]
                AddCommentsCommand = new DelegateCommand<object>(AddCommentsCommandAction);// [pramod.misal][GEOS2-4450][27.06.2023]
                CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);
                Setting = CrmStartUp.GetGeosAppSettings(104);//[pramod.misal][GEOS2-4451][10-08-2023]

                GeosApplication.Instance.Logger.Log("Constructor ViewPurchaseOrderViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewPurchaseOrderViewModel ViewPurchaseOrderViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods

        private void CustomSummaryCommandAction(object obj)
        {
            try
            {
                GridCustomSummaryEventArgs e = (GridCustomSummaryEventArgs)obj;
                if (e.Item != null)
                {
                    GridSummaryItem Item = (GridSummaryItem)e.Item;

                    if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "IdArticle")
                    {
                        e.TotalValueReady = true;
                        e.TotalValue = WarehousePurchaseOrder.WarehousePurchaseOrderItemWithComments.Where(w => w.IdArticle != 3252).ToList().Count();
                    }
                    if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "Article.Reference")
                    {
                        e.TotalValueReady = true;
                        e.TotalValue = WarehousePurchaseOrder.WarehousePurchaseOrderItemWithComments.Where(w => w.IdArticle != 3252).ToList().Count();
                        if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                        {
                            if (e.FieldValue != null)
                            {
                            }

                        }
                        if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                        {
                            e.TotalValueReady = true;
                            e.TotalValue = WarehousePurchaseOrder.WarehousePurchaseOrderItemWithComments.Where(w => w.IdArticle != 3252).ToList().Count();
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }
        //[001][cpatil][14-09-2022][GEOS2-3864]
        public void Init(WarehousePurchaseOrder WarehousePO, Warehouses objWarehouse)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
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

                #region Service comments
                // shubham[skadam]GEOS2-3833 Add comment rows in PO details view  09 Aug 2022
                //WarehousePurchaseOrder = SRMService.GetPendingPOByIdWarehousePurchaseOrder_V2110(idWarehousePurchaseOrder);
                // Service Method GetPendingPOByIdWarehousePurchaseOrder_V2300 updated with GetPendingPOByIdWarehousePurchaseOrder_V2380 by [GEOS2-4310][rdixit][05.04.2023]
                // Service Method GetPendingPOByIdWarehousePurchaseOrder_V2380 updated with GetPendingPOByIdWarehousePurchaseOrder_V2390 by [rdixit][12.06.2023]

                // WarehousePurchaseOrder = SRMService.GetPendingPOByIdWarehousePurchaseOrder_V2390(idWarehousePurchaseOrder);

                //[pramod.misal][GEOS2-4431][16/06/2023]
                //WarehousePurchaseOrder = SRMService.GetPendingPOByIdWarehousePurchaseOrder_V2400(idWarehousePurchaseOrder);

                //[pramod.misal][GEOS2-4449][04/06/2023]GetPendingPOByIdWarehousePurchaseOrder_V2410
                //[pramod.misal][GEOS2-4451][24/06/2023]Updated Service = GetPendingPOByIdWarehousePurchaseOrder_V2410
                // Service Method GetPendingPOByIdWarehousePurchaseOrder_V2420 updated with GetPendingPOByIdWarehousePurchaseOrder_V2430 by  [rdixit][GEOS2-4822][13-09-2023]
                // Service Method GetPendingPOByIdWarehousePurchaseOrder_V2430 updated with GetPendingPOByIdWarehousePurchaseOrder_V2440 by  [rdixit][GEOS2-4589][09-10-2023]
                #endregion

                #region [rdixit][GEOS2-4451][21.08.2023]
                string WarehouseAddressids = string.Empty;
                try
                {
                    SRM.SRMCommon.Instance.Selectedwarehouse = objWarehouse;
                    //SRMService = new SRMServiceController("localhost:6699");
                    SRMService = new SRMServiceController((objWarehouse != null && objWarehouse.Company.ServiceProviderUrl != null) ? objWarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    GeosAppSetting AppSetting = CrmStartUp.GetGeosAppSettings(105);
                    char[] RemoveChar = { '(', ')' };
                    if (AppSetting != null)
                    {
                        string[] AllAppSettingValues = AppSetting.DefaultValue.Split(',');
                        foreach (var item in AllAppSettingValues.Where(i => Convert.ToInt32(i.Trim(RemoveChar).Split(';').ToArray()[0]) == Convert.ToInt32(objWarehouse.IdWarehouse)))
                        {
                            if (string.IsNullOrEmpty(WarehouseAddressids))
                                WarehouseAddressids = item.Trim(RemoveChar).Split(';').ToArray()[1];
                            else
                                WarehouseAddressids += "," + item.Trim(RemoveChar).Split(';').ToArray()[1];
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
                //WarehousePurchaseOrder = SRMService.GetPendingPOByIdWarehousePurchaseOrder_V2440(WarehousePO.IdWarehousePurchaseOrder, WarehouseAddressids);
                //Shubham[skadam] GEOS2-4965 General discount row is not added in the PO 24 11 2023
                //SRMService = new SRMServiceController("localhost:6699");

                //SRMService.GetPendingPOByIdWarehousePurchaseOrder_V2460(WarehousePO.IdWarehousePurchaseOrder, WarehouseAddressids);

                //WarehousePurchaseOrder = SRMService.GetPendingPOByIdWarehousePurchaseOrder_V2500(WarehousePO.IdWarehousePurchaseOrder, WarehouseAddressids);
                //SRMService = new SRMServiceController("localhost:6699");
               //WarehousePurchaseOrder = SRMService.GetPendingPOByIdWarehousePurchaseOrder_V2520(WarehousePO.IdWarehousePurchaseOrder, WarehouseAddressids);
                WarehousePurchaseOrder = SRMService.GetPendingPOByIdWarehousePurchaseOrder_V2620(WarehousePO.IdWarehousePurchaseOrder, WarehouseAddressids);
                try
                {
                    if (WarehousePO.ArticleSupplier != null)
                    {
                        //[Sudhir.Jangra][GEOS2-5493]
                        IdArticleSupplierForTOAndCC = (Int32)WarehousePO.ArticleSupplier.IdArticleSupplier;
                    }
                    else
                    {
                        if (WarehousePurchaseOrder.ArticleSupplier != null)
                        {
                            IdArticleSupplierForTOAndCC = (Int32)WarehousePurchaseOrder.ArticleSupplier.IdArticleSupplier;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                if (WarehousePurchaseOrder.WarehouseShippingAddress?.Count > 1 && WarehousePurchaseOrder.IdShippingAddress != 0)
                {
                    WarehousePurchaseOrder.WarehouseShippingAddress.FirstOrDefault(x => x.IdShippingAddress == warehousePurchaseOrder.IdShippingAddress).IsMainContact = true;
                    WarehouseShippingAddress MainContact = WarehousePurchaseOrder.WarehouseShippingAddress.FirstOrDefault(x => x.IdShippingAddress == warehousePurchaseOrder.IdShippingAddress);
                    WarehousePurchaseOrder.WarehouseShippingAddress.Remove(MainContact);
                    WarehousePurchaseOrder.WarehouseShippingAddress.Insert(0, MainContact);
                }
                if (WarehousePurchaseOrder != null)
                {
                    if (WarehousePurchaseOrder.WarehouseShippingAddress?.Count != 0)
                    {
                        WarehousePurchaseOrder.WarehouseShippingAddress.ForEach(i => { i.Address = i.Address.Replace("\n", " ").Replace("\r", " "); i.Zipcityregion = i.ZipCode + " - " + i.City + " - " + i.Region; });
                        Addressvisibility = Visibility.Visible;
                        var WPO = WarehousePurchaseOrder.WarehouseShippingAddress.FirstOrDefault();
                        if (WPO != null)
                        {
                            Contentcountryname = WPO.CountryName;
                            Contentcountryimgurl = WPO.Country.CountryIconUrl;
                            //[rdixit][GEOS2-4451][21.08.2023]
                            ContentAddress = WarehousePurchaseOrder.WarehouseShippingAddress[0].ZipCode + " - " + WarehousePurchaseOrder.WarehouseShippingAddress[0].City + " - " + WarehousePurchaseOrder.WarehouseShippingAddress[0].Region;
                        }
                    }
                    else
                    {
                        Addressvisibility = Visibility.Hidden;
                        ContentAddress = string.Empty;
                    }
                }
                //Shubham[skadam]  GEOS2-4713 Missing Delivery Dates 31 07 2023 
                //if (WarehousePurchaseOrder.DeliveryDate==null || WarehousePurchaseOrder.DeliveryDate == DateTime.MinValue)
                if (WarehousePurchaseOrder.DeliveryDate == null)
                {
                    WarehousePurchaseOrder.DeliveryDate = WarehousePO.DeliveryDate;
                }
                CommentsList = new ObservableCollection<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOComments.Where(x => x.IdWarehousePurchaseOrder == WarehousePO.IdWarehousePurchaseOrder).ToList());

                if (CommentsList!=null)//[Sudhir.Jangra][GEOS2-6694]
                {

                    foreach (var item in CommentsList)
                    {
                        item.LogDatetime = item.Datetime.Value.ToString("g");
                        item.People.OwnerImage = SetUserProfileImage(item);
                    }
                }

               
                if (WarehousePurchaseOrder.WarehousePOLogEntries != null)
                    WarehousePurchaseOrder.WarehousePOLogEntries.ForEach(i => i.LogDatetime = i.Datetime.Value.ToString("g"));


                if (WarehousePurchaseOrder.WarehousePurchaseOrderItems == null)
                {
                    WarehousePurchaseOrder.WarehousePurchaseOrderItems = new List<WarehousePurchaseOrderItem>();
                }
                if (WarehousePurchaseOrder.WarehousePurchaseOrderItemWithComments != null)
                {
                    WarehousePurchaseOrder.WarehousePurchaseOrderItemWithComments.ForEach(f => f.Summary = WarehousePurchaseOrder.WarehousePurchaseOrderItemWithComments.Where(w => w.IdArticle != 3252).ToList().Count());
                    foreach (WarehousePurchaseOrderItemWithComments purchaseOrderItems in WarehousePurchaseOrder.WarehousePurchaseOrderItemWithComments)
                    {

                        if (purchaseOrderItems.IdArticle == 3252)
                        {
                            purchaseOrderItems.Quantity = null;
                            purchaseOrderItems.UnitPrice = null;
                            purchaseOrderItems.Discount = null;
                            purchaseOrderItems.TotalAmount_New = null;
                            purchaseOrderItems.ReceivedQuantity = null;
                            //purchaseOrderItems.Status = null;
                        }
                        //Shubham[skadam] GEOS2-4965 General discount row is not added in the PO 28 11 2023
                        else if (purchaseOrderItems.IdArticle == 13339)
                        {
                            purchaseOrderItems.Quantity = null;
                            //purchaseOrderItems.UnitPrice = null;
                            purchaseOrderItems.Discount = null;
                            //purchaseOrderItems.TotalAmount_New = null;
                            purchaseOrderItems.ReceivedQuantity = null;
                            //purchaseOrderItems.Status = null;
                        }
                        else
                        {
                            purchaseOrderItems.Article_New = new Article();
                            purchaseOrderItems.Article_New.Reference = purchaseOrderItems.Article.Reference;
                        }
                    }
                }
                WarehousePurchaseOrder.Warehouse = objWarehouse;
                //[001] changes service method
                WorkflowStatusList = new List<WorkflowStatus>(SRMService.GetAllWorkflowStatus_V2301());
                //SRMService = new SRMServiceController("localhost:6699");
                //Shubham[skadam] GEOS2-4406 Send an email notification when the status of the PO changes 21 06 2023

               // SRMService = new SRMServiceController("localhost:6699");
                WorkflowTransitionList = new List<WorkflowTransition>(SRMService.GetAllWorkflowTransitions_V2400());

                List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == WarehousePurchaseOrder.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                WorkflowStatusButtons = new List<WorkflowStatus>();

                foreach (byte statusbutton in GetCurrentButtons)
                {
                    WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                }
                //GEOS2-2044 adhatkar
                FillAssigneeAndApprovarList();

                ImageSource countryFlagImage = ByteArrayToBitmapImage(warehousePurchaseOrder.ArticleSupplier.Country.CountryIconBytes);
                warehousePurchaseOrder.ArticleSupplier.Country.CountryIconImage = countryFlagImage;

                // [pallavi.kale][04-03-2025][GEOS2-7013]
                ImageSource currencyImage = ByteArrayToBitmapImage(warehousePurchaseOrder.Currency.CurrencyIconbytes);
                warehousePurchaseOrder.Currency.CurrencyIconImage = currencyImage;
                try
                {
                    //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                    CommentsList = new ObservableCollection<LogEntriesByWarehousePO>(CommentsList.OrderByDescending(x => x.Datetime));
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in CommentDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PoCancelButton()...", category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method PoCancelButton()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PoCancelButton()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OpenPDFDocument(object obj)
        {
            GridControl deliveryNoteGrid = (GridControl)obj;
            WarehouseDeliveryNote warehouseDeliveryNote = (WarehouseDeliveryNote)deliveryNoteGrid.CurrentItem;
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                documentViewModel.OpenPdfByCode(WarehousePurchaseOrder, warehouseDeliveryNote);

                documentView.DataContext = documentViewModel;
                documentView.Show();

                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //warehouseDeliveryNote.PdfFilePath
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                CustomMessageBox.Show(string.Format("Could not find file '{0}'.", warehouseDeliveryNote.PdfFilePath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Open Article View on hyperlink click
        /// [001][adhatkar][2020-05-21][GEOS2-2042]
        /// </summary>
        /// <param name="obj"></param>
        public void POItemDetailsViewHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method POItemDetailsViewHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
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
                WarehousePurchaseOrderItemWithComments otItem = (WarehousePurchaseOrderItemWithComments)detailView.DataControl.CurrentItem;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = SRMCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                int ArticleSleepDays = SRMCommon.Instance.ArticleSleepDays;
                articleDetailsViewModel.Init_SRM(otItem.Article.Reference, warehouseId, warehouse, ArticleSleepDays);
                articleDetailsView.DataContext = articleDetailsViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (obj as FrameworkElement);
                articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                //[001] added
                if (articleDetailsViewModel.IsResult)
                {
                    //if (articleDetailsViewModel.UpdateArticle.MyWarehouse != null)
                    //otItem.ArticleMinimumStock = articleDetailsViewModel.UpdateArticle.MyWarehouse.MinimumStock;
                }
                //end
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method POItemDetailsViewHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method POItemDetailsViewHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {

        }
        private void WorkflowButtonClickCommandAction(object obj)
        {
            try
            {
                int status_id = Convert.ToInt32(obj);
                int IdWorkflowStatus = WarehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus;
                GeosApplication.Instance.Logger.Log("Method WorkflowButtonClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                TransitionWorkflowStatus(status_id);

                //Shubham[skadam] GEOS2-4406 Send an email notification when the status of the PO changes 21 06 2023
                SendPOEmailNotification(status_id, IdWorkflowStatus);

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
        private void TransitionWorkflowStatus(int currentStatus)
        {
            List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList = new List<LogEntriesByWarehousePO>();
            WorkflowTransition workflowTransition = new WorkflowTransition();
            workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == currentStatus);
            WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == currentStatus);

            // [Rahul Gadhave][GEOS2-5496 Allow to re-open Closed POs (3/6)][Date:12/06/2024]
            var items = WarehousePurchaseOrder.WarehousePurchaseOrderItemWithComments;
            if (items != null && items.Count > 0)
            {
                // Calculate the total Quantity and ReceivedQuantity
                var totalQuantity = items.Sum(item => item.Quantity);
                var totalReceivedQuantity = items.Sum(item => item.ReceivedQuantity);
                if (totalQuantity == totalReceivedQuantity && currentStatus == 46)
                {
                    //SRMService = new SRMServiceController("localhost:6699");
                    IsSaveChanges = SRMService.UpdateWorkflowStatusInPO_V2530((uint)WarehousePurchaseOrder.IdWarehousePurchaseOrder, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByWarehousePOList, (uint)SelectedAssignee.IdUser, (uint)SelectedApprover.IdUser, Comment);

                }
                else
                {
                    if (currentStatus == 46)
                    {


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
                                //logEntriesByWarehousePO_1.LogDatetime = GeosApplication.Instance.ServerDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                                //logEntriesByWarehousePO_1.LogDatetime = GeosApplication.Instance.ServerDateTime.ToString(CultureInfo.CurrentCulture.DateTimeFormat);
                                logEntriesByWarehousePO_1.LogDatetime = GeosApplication.Instance.ServerDateTime.ToString("g");
                                logEntriesByWarehousePO_1.People = new People();
                                logEntriesByWarehousePO_1.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                                logEntriesByWarehousePO_1.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                                logEntriesByWarehousePO_1.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                                LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO_1);
                                WarehousePurchaseOrder.WarehousePOLogEntries.Insert(0, logEntriesByWarehousePO_1);
                                WarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOLogEntries);
                                if (CommentsList == null)
                                    CommentsList = new ObservableCollection<LogEntriesByWarehousePO>();
                                CommentsList.Insert(0, CommentByWarehousePO);


                                Comment = addWorkflowStatusCommentViewModel.Comment;
                            }
                            else
                            {
                                workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == WarehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus);
                                WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == WarehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus);
                                return;
                            }
                        }
                    }
                }
            }
            if (workflowTransition != null && workflowTransition.IsCommentRequired == 1 && currentStatus!=46)
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
                    //logEntriesByWarehousePO_1.LogDatetime = GeosApplication.Instance.ServerDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                    //logEntriesByWarehousePO_1.LogDatetime = GeosApplication.Instance.ServerDateTime.ToString(CultureInfo.CurrentCulture.DateTimeFormat);
                    logEntriesByWarehousePO_1.LogDatetime = GeosApplication.Instance.ServerDateTime.ToString("g");
                    logEntriesByWarehousePO_1.People = new People();
                    logEntriesByWarehousePO_1.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                    logEntriesByWarehousePO_1.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                    logEntriesByWarehousePO_1.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                    LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO_1);
                    WarehousePurchaseOrder.WarehousePOLogEntries.Insert(0, logEntriesByWarehousePO_1);
                    WarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOLogEntries);
                    if (CommentsList == null)
                        CommentsList = new ObservableCollection<LogEntriesByWarehousePO>();
                    CommentsList.Insert(0, CommentByWarehousePO);


                    Comment = addWorkflowStatusCommentViewModel.Comment;
                }
                else
                {
                    workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == WarehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus);
                    WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == WarehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus);
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
            //logEntriesByWarehousePO.LogDatetime = GeosApplication.Instance.ServerDateTime.ToString("dd-MM-yyyy HH:mm:ss");
            //logEntriesByWarehousePO.LogDatetime = GeosApplication.Instance.ServerDateTime.ToString(CultureInfo.CurrentCulture.DateTimeFormat);
            logEntriesByWarehousePO.LogDatetime = GeosApplication.Instance.ServerDateTime.ToString("g");
            logEntriesByWarehousePO.People = new People();
            logEntriesByWarehousePO.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
            logEntriesByWarehousePO.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
            logEntriesByWarehousePO.People.Surname = GeosApplication.Instance.ActiveUser.LastName;



            LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO);

            WarehousePurchaseOrder.WarehousePOLogEntries.Insert(0, logEntriesByWarehousePO);

            #region Prev Code_ Now Assignee and Approver will save on accept button click event
            /*if (warehousePurchaseOrder.IdAssignee != SelectedAssignee.IdUser)
            {
                LogEntriesByWarehousePO logEntriesByWarehousePO_Assignee = new LogEntriesByWarehousePO();
                logEntriesByWarehousePO_Assignee.IdWarehousePurchaseOrder = WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                logEntriesByWarehousePO_Assignee.IdUser = GeosApplication.Instance.ActiveUser.IdUser;

                if(warehousePurchaseOrder.IdAssignee==null)
                    logEntriesByWarehousePO_Assignee.Comments = string.Format(System.Windows.Application.Current.FindResource("AssigneeLogEntryByPO").ToString(), "None", SelectedAssignee.FullName);
                else
                    logEntriesByWarehousePO_Assignee.Comments = string.Format(System.Windows.Application.Current.FindResource("AssigneeLogEntryByPO").ToString(), AssigneeList.FirstOrDefault(a=>a.IdUser== warehousePurchaseOrder.IdAssignee).FullName, SelectedAssignee.FullName);

                logEntriesByWarehousePO_Assignee.IdLogEntryType = 253;
                logEntriesByWarehousePO_Assignee.IsRtfText = false;
                logEntriesByWarehousePO_Assignee.Datetime = GeosApplication.Instance.ServerDateTime;
                logEntriesByWarehousePO_Assignee.People = new People();
                logEntriesByWarehousePO_Assignee.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                logEntriesByWarehousePO_Assignee.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                logEntriesByWarehousePO_Assignee.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO_Assignee);
                warehousePurchaseOrder.IdAssignee = Convert.ToUInt32(SelectedAssignee.IdUser);

                WarehousePurchaseOrder.WarehousePOLogEntries.Insert(0, logEntriesByWarehousePO_Assignee);
            }

            if (warehousePurchaseOrder.IdApprover != SelectedApprover.IdUser)
            {
                LogEntriesByWarehousePO logEntriesByWarehousePO_Approver = new LogEntriesByWarehousePO();
                logEntriesByWarehousePO_Approver.IdWarehousePurchaseOrder = WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                logEntriesByWarehousePO_Approver.IdUser = GeosApplication.Instance.ActiveUser.IdUser;

                if (warehousePurchaseOrder.IdApprover == null)
                    logEntriesByWarehousePO_Approver.Comments = string.Format(System.Windows.Application.Current.FindResource("ApproverLogEntryByPO").ToString(), "None", SelectedApprover.FullName);
                else
                    logEntriesByWarehousePO_Approver.Comments = string.Format(System.Windows.Application.Current.FindResource("ApproverLogEntryByPO").ToString(), ApproverList.FirstOrDefault(a => a.IdUser == warehousePurchaseOrder.IdApprover).FullName, SelectedApprover.FullName);

                logEntriesByWarehousePO_Approver.IdLogEntryType = 253;
                logEntriesByWarehousePO_Approver.IsRtfText = false;
                logEntriesByWarehousePO_Approver.Datetime = GeosApplication.Instance.ServerDateTime;
                logEntriesByWarehousePO_Approver.People = new People();
                logEntriesByWarehousePO_Approver.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                logEntriesByWarehousePO_Approver.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                logEntriesByWarehousePO_Approver.People.Surname = GeosApplication.Instance.ActiveUser.LastName;

                LogEntriesByWarehousePOList.Add(logEntriesByWarehousePO_Approver);

                warehousePurchaseOrder.IdApprover = Convert.ToUInt32(SelectedApprover.IdUser);

                WarehousePurchaseOrder.WarehousePOLogEntries.Insert(0, logEntriesByWarehousePO_Approver);
            }*/
            #endregion

            WarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>(WarehousePurchaseOrder.WarehousePOLogEntries);

            //ISRMService SRMService = new SRMServiceController("localhost:6699");
            //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");


            //Service Method updated from UpdateWorkflowStatusInPO_V2110 to UpdateWorkflowStatusInPO_V2380 by [rdixit][GEOS2-4313][17.04.2023]
            //IsSaveChanges = SRMService.UpdateWorkflowStatusInPO_V2380((uint)WarehousePurchaseOrder.IdWarehousePurchaseOrder, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByWarehousePOList, (uint)SelectedAssignee.IdUser, (uint)SelectedApprover.IdUser);

            //[pramod.misal][GEOS2-5495][16.08.2024]
            //IsSaveChanges = SRMService.UpdateWorkflowStatusInPO_V2420((uint)WarehousePurchaseOrder.IdWarehousePurchaseOrder, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByWarehousePOList, (uint)SelectedAssignee.IdUser, (uint)SelectedApprover.IdUser);
            //SRMService = new SRMServiceController("localhost:6699");
            IsSaveChanges = SRMService.UpdateWorkflowStatusInPO_V2530((uint)WarehousePurchaseOrder.IdWarehousePurchaseOrder, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByWarehousePOList, (uint)SelectedAssignee.IdUser, (uint)SelectedApprover.IdUser, Comment);

            //try
            //{
            //    //Shubham[skadam] GEOS2-4406 Send an email notification when the status of the PO changes 21 06 2023
            //    if (!string.IsNullOrEmpty(Comment))
            //    {
            //        SendPOEmailNotification(WorkflowStatus.IdWorkflowStatus, WarehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus);
            //        Comment = string.Empty;
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
            WarehousePurchaseOrder.IdWorkflowStatus = WorkflowStatus.IdWorkflowStatus;
            WarehousePurchaseOrder.WorkflowStatus = WorkflowStatus;
            List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == currentStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
            WorkflowStatusButtons = new List<WorkflowStatus>();

            foreach (byte statusbutton in GetCurrentButtons)
            {
                WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
            }
            WorkflowStatusButtons = new List<WorkflowStatus>(WorkflowStatusButtons);
            //if(IsSaveChanges==true)
            //{
            //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("POWorkflowStatusMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            //}
            //RequestClose(null, null);
        }
        private void OpenWorkflowDiagramCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()...", category: Category.Info, priority: Priority.Low);
                WorkflowDiagramViewModel workflowDiagramViewModel = new WorkflowDiagramViewModel();
                WorkflowDiagramView workflowDiagramView = new WorkflowDiagramView();
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
        private void SendEmailCommandAction(object obj)
        {
            //[rdixit][21.05.2024][GEOS2-5762]
            ToContactList = new ObservableCollection<Contacts>();
            CCContactList = new ObservableCollection<Contacts>();        
            FillToCC(WarehousePurchaseOrder);
            string tempFilePath = Path.GetTempPath() + warehousePurchaseOrder.Code + ".pdf";
            try
            {
                if (!(string.IsNullOrEmpty(warehousePurchaseOrder.ArticleSupplier.ContactEmail) && string.IsNullOrEmpty(warehousePurchaseOrder.ArticleSupplier.ContactPerson)))
                {
                    GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction()...", category: Category.Info, priority: Priority.Low);
                    #region GEOS-4077 Sudhir.Jangra 10/05/2023 Commented by rdixit
                    /*string senderEmail = GetEmailFrom();
                    //string senderEmail = "rajashri.telvekar@emdep.com";
                    var temp = SRMService.GetSystemSettings_V2390();
                    if (temp != null)
                    {
                        SystemSettingsList = temp.FirstOrDefault().SystemSettings;
                        List<Warehouses> plantOwner = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                        var selectedCC = SystemSettingsList.FirstOrDefault(x => x.Warehouse == plantOwner.FirstOrDefault().Name).CC;
                        //var selectedCC = "rajashri.telvekar@emdep.com";
                        string differentEmails = string.Join(";", plantOwner.SelectMany(selected =>
                        {
                            var selectedWarehouseName = selected.Name;
                            var selectedWarehouse = SystemSettingsList.Where(x => x.Warehouse == selectedWarehouseName);

                            return selectedWarehouse.Where(systemSetting => warehousePurchaseOrder.AssigneeEmail != senderEmail || warehousePurchaseOrder.CreatorEmail != senderEmail).Select(systemSetting =>

                            warehousePurchaseOrder.AssigneeEmail != senderEmail && warehousePurchaseOrder.CreatorEmail != senderEmail ? $"{warehousePurchaseOrder.AssigneeEmail};{warehousePurchaseOrder.CreatorEmail}" :
                                    (warehousePurchaseOrder.AssigneeEmail != senderEmail ? warehousePurchaseOrder.AssigneeEmail : warehousePurchaseOrder.CreatorEmail));
                        }));
                        var distinctEmails = differentEmails.Split(';').Distinct();
                        var ccEmail = selectedCC.Split(',');
                        foreach (var email in distinctEmails)
                        {
                            if (!ccEmail.Contains(email))
                            {
                                ccEmail = ccEmail.Concat(new[] { email }).ToArray();
                            }
                        }
                        EmailCC += string.Join(";", ccEmail);
                         //EmailCC += "rajashri.telvekar@emdep.com";
                    }*/
                    #endregion

                    #region [rdixit][21.05.2024][GEOS2-5762]
                    string senderEmail = GetEmailFrom();
                    var temp = SRMService.GetSystemSettings_V2390();
                    try
                    {
                        if (temp != null)
                        {

                            SystemSettingsList = temp.FirstOrDefault().SystemSettings;
                            Warehouses plantOwner = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>()?.FirstOrDefault(i => i.IdWarehouse == warehousePurchaseOrder.IdWarehouse);
                            GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction() Selected Warehouse..." + plantOwner?.IdWarehouse, category: Category.Info, priority: Priority.Low);

                            string selectedCC =  SystemSettingsList.FirstOrDefault(x => x.Warehouse == plantOwner?.Name)?.CC ?? "";
                            GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction() selectedCC..." + selectedCC, category: Category.Info, priority: Priority.Low);

                            string recipients = (warehousePurchaseOrder.AssigneeEmail != senderEmail && warehousePurchaseOrder.CreatorEmail != senderEmail)
                                           ? $"{warehousePurchaseOrder.AssigneeEmail};{warehousePurchaseOrder.CreatorEmail}" :
                                           (warehousePurchaseOrder.AssigneeEmail != senderEmail ? warehousePurchaseOrder.AssigneeEmail : warehousePurchaseOrder.CreatorEmail);

                            GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction() AssigneeEmail..." + warehousePurchaseOrder.AssigneeEmail, category: Category.Info, priority: Priority.Low);

                            GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction() CreatorEmail..." + warehousePurchaseOrder.CreatorEmail, category: Category.Info, priority: Priority.Low);

                            GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction() recipients..." + recipients, category: Category.Info, priority: Priority.Low);

                            List<string> distinctEmails = recipients?.Split(';').Distinct()?.ToList();


                            if (distinctEmails?.Count > 0)
                                GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction() distinctEmails..." + string.Join(",", distinctEmails), category: Category.Info, priority: Priority.Low);

                            HashSet<string> ccEmail = new HashSet<string>(selectedCC != null ? selectedCC.Split(',') : new string[0]);

                            if (ccEmail?.Count > 0)
                                GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction() ccEmail..." + string.Join(",", ccEmail), category: Category.Info, priority: Priority.Low);

                            if (distinctEmails?.Count > 0)
                            {
                                foreach (var email in distinctEmails)
                                {
                                    if (!string.IsNullOrEmpty(email))
                                        ccEmail.Add(email.Trim());
                                }
                            }
                            EmailCC = string.Join(";", ccEmail);
                            GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction() EmailCC..." + EmailCC, category: Category.Info, priority: Priority.Low);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in method SendEmailCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                    #endregion

                    if (WarehousePurchaseOrder.EmailBody != null && WarehousePurchaseOrder.AttachmentBytes != null)
                    {
                        GeosApplication.Instance.Logger.Log("Method OutlookApp process Started ", category: Category.Info, priority: Priority.Low);
                        OutlookApp outlookApp = new OutlookApp();
                        Microsoft.Office.Interop.Outlook.MailItem mailItem = outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                        mailItem.Subject = "PO " + WarehousePurchaseOrder.Code;
                        mailItem.HTMLBody = WarehousePurchaseOrder.EmailBody;
                        GeosApplication.Instance.Logger.Log("Got HTMLBody... ", category: Category.Info, priority: Priority.Low);
                        mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                        using (MemoryStream ms = new MemoryStream(WarehousePurchaseOrder.AttachmentBytes))
                        {
                            FileStream fs = new FileStream(tempFilePath, FileMode.Create);
                            ms.CopyTo(fs);
                            fs.Close();
                            mailItem.Attachments.Add(tempFilePath, Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue, 1, warehousePurchaseOrder.Code + ".pdf");
                        }
                        GeosApplication.Instance.Logger.Log("Got AttachmentBytes... ", category: Category.Info, priority: Priority.Low);
                        //Set a high priority to the message
                        mailItem.Importance = Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh;

                        List<string> toemails = new List<string>();

                        if (!string.IsNullOrEmpty(WarehousePurchaseOrder.ArticleSupplier.ContactEmail))
                        {
                            toemails.Add(WarehousePurchaseOrder.ArticleSupplier.ContactEmail);
                            GeosApplication.Instance.Logger.Log("Got ArticleSupplier ContactEmail..." + WarehousePurchaseOrder.ArticleSupplier.ContactEmail, category: Category.Info, priority: Priority.Low);
                        }

                        if (ToContactList != null)
                        {
                            foreach (Contacts item in ToContactList)
                            {
                                toemails.Add(item.Email);
                            }
                            GeosApplication.Instance.Logger.Log("Got ToContactList..." + string.Join(",", toemails), category: Category.Info, priority: Priority.Low);
                        }
                        HashSet<string> distinctToEmails = new HashSet<string>(toemails);

                        mailItem.To = string.Join(";", distinctToEmails.ToArray());
                        GeosApplication.Instance.Logger.Log("Got To Email..." + mailItem.To, category: Category.Info, priority: Priority.Low);
                        #region To take distinct CC emails 
                        List<string> ccemails = new List<string>();
                        if (!string.IsNullOrEmpty(EmailCC))
                        {
                            MatchCollection matches = Regex.Matches(EmailCC, @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b");
                            GeosApplication.Instance.Logger.Log("Got CC Email..." + EmailCC, category: Category.Info, priority: Priority.Low);
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
                            GeosApplication.Instance.Logger.Log("Got CContactList..." + string.Join(",", CCContactList.Select(i => i.Email)), category: Category.Info, priority: Priority.Low);
                        }

                        HashSet<string> distinctEmails = new HashSet<string>(ccemails);
                        #endregion
                        mailItem.CC = string.Join(";", distinctEmails.ToArray());
                        GeosApplication.Instance.Logger.Log("CC emails in mail..." + mailItem.CC, category: Category.Info, priority: Priority.Low);
                        mailItem.Send();
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_Success").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        #region GEOS2-4404
                        //Shubham[skadam]  GEOS2-4404 (View Supplier) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
                        try
                        {
                            if (WorkflowStatusButtons.Any(a => a.IdWorkflowStatus == 6))
                            {
                                bool result = SRMService.UpdatePurchasingOrderStatus(WarehousePurchaseOrder.IdWarehousePurchaseOrder, GeosApplication.Instance.ActiveUser.IdUser);
                                //SRMService = new SRMServiceController("localhost:6699");
                                //Shubham[skadam] GEOS2-4406 Send an email notification when the status of the PO changes 21 06 2023
                                SendPOEmailNotification(SRMService.GetPurchasingOrderStatus(WarehousePurchaseOrder.IdWarehousePurchaseOrder), WarehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus);
                                TransitionWorkflowStatus(SRMService.GetPurchasingOrderStatus(WarehousePurchaseOrder.IdWarehousePurchaseOrder));
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        #endregion
                        EmailCC = string.Empty;
                        //mailItem.Display(false);
                    }
                    else
                    {
                        if (WarehousePurchaseOrder.EmailBody == null || warehousePurchaseOrder.EmailBody == "")
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_failure1").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        else if (WarehousePurchaseOrder.AttachmentBytes == null)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_failure").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMFavSupplierMail_failure").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method SendEmailCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                ToContactList = new ObservableCollection<Contacts>();
                CCContactList = new ObservableCollection<Contacts>();
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMSupplierMail_Success").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            }
            finally
            {
                if (WarehousePurchaseOrder.AttachmentBytes != null && File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        //[GEOS2-2044] adhatkar
        private void FillAssigneeAndApprovarList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAssigneeAndApprovarList()...", category: Category.Info, priority: Priority.Low);
                //SRMService = new SRMServiceController("localhost:6699");
                AssigneeList = new ObservableCollection<User>(SRMService.GetPermissionUsers());

                if (WarehousePurchaseOrder.IdAssignee == null)
                    SelectedAssignee = AssigneeList.FirstOrDefault(a => a.IdUser == GeosApplication.Instance.ActiveUser.IdUser);
                else
                    SelectedAssignee = AssigneeList.FirstOrDefault(a => a.IdUser == WarehousePurchaseOrder.IdAssignee);


                ApproverList = new ObservableCollection<User>(AssigneeList);

                if (WarehousePurchaseOrder.IdApprover == null)
                    SelectedApprover = ApproverList.FirstOrDefault(a => a.IdUser == GeosApplication.Instance.ActiveUser.IdUser);
                else
                    SelectedApprover = ApproverList.FirstOrDefault(a => a.IdUser == WarehousePurchaseOrder.IdApprover);


                GeosApplication.Instance.Logger.Log("Method FillAssigneeAndApprovarList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAssigneeAndApprovarList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAssigneeAndApprovarList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillAssigneeAndApprovarList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
        //[rdixit][GEOS2-4313][17.04.2023]
        private void AcceptButtonAction(object obj)
        {
            try
            {
                //SRMService = new SRMServiceController("localhost:6699");
                GeosApplication.Instance.Logger.Log("Method AcceptButtonAction()...", category: Category.Info, priority: Priority.Low);
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
                WarehousePurchaseOrder UpdatedWarehousePurchaseOrder = new WarehousePurchaseOrder();
                UpdatedWarehousePurchaseOrder.IdAssignee = WarehousePurchaseOrder.IdAssignee;
                UpdatedWarehousePurchaseOrder.IdApprover = WarehousePurchaseOrder.IdApprover;
                UpdatedWarehousePurchaseOrder.ModifiedBy = WarehousePurchaseOrder.ModifiedBy;
                UpdatedWarehousePurchaseOrder.IdWarehousePurchaseOrder = WarehousePurchaseOrder.IdWarehousePurchaseOrder;
                UpdatedWarehousePurchaseOrder.WarehousePOComments = WarehousePurchaseOrder.WarehousePOComments;
                if (UpdatedWarehousePurchaseOrder.IdAssignee == null)
                    UpdatedWarehousePurchaseOrder.IdAssignee = (uint?)GeosApplication.Instance.ActiveUser.IdUser;


                UpdatedWarehousePurchaseOrder.WarehousePOComments = new List<LogEntriesByWarehousePO>();

                #region [rdixit][23.08.2023]
                if (AddCommentsList != null && AddCommentsList.Count > 0)
                {
                    foreach (var item in AddCommentsList)
                    {
                        item.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdatedWarehousePurchaseOrder.WarehousePOComments.Add((LogEntriesByWarehousePO)item.Clone());
                        item.IdWarehousePurchaseOrder = item.IdWarehousePurchaseOrder;
                        item.Datetime = Convert.ToDateTime(item.Datetime);
                    }
                }

                if (DeleteCommentsList != null && DeleteCommentsList.Count > 0)
                {
                    foreach (var item in DeleteCommentsList)
                    {
                        item.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        UpdatedWarehousePurchaseOrder.WarehousePOComments.Add((LogEntriesByWarehousePO)item.Clone());
                        item.IdWarehousePurchaseOrder = item.IdWarehousePurchaseOrder;
                        item.Datetime = Convert.ToDateTime(item.Datetime);
                    }
                }
                if (UpdatedCommentsList != null && UpdatedCommentsList.Count > 0)
                {
                    foreach (var item in UpdatedCommentsList)
                    {
                        item.TransactionOperation = ModelBase.TransactionOperations.Update;
                        UpdatedWarehousePurchaseOrder.WarehousePOComments.Add((LogEntriesByWarehousePO)item.Clone());
                        item.IdWarehousePurchaseOrder = item.IdWarehousePurchaseOrder;
                        item.Datetime = Convert.ToDateTime(item.Datetime);
                    }
                }
                #endregion

                if ((UpdatedWarehousePurchaseOrder.IdAssignee != SelectedAssignee.IdUser) || (UpdatedWarehousePurchaseOrder.IdApprover != SelectedApprover.IdUser) || (UpdatedWarehousePurchaseOrder.WarehousePOComments != null && UpdatedWarehousePurchaseOrder.WarehousePOComments.Count > 0))
                {

                    UpdatedWarehousePurchaseOrder.WarehousePOLogEntries = new List<LogEntriesByWarehousePO>();

                    if ((UpdatedWarehousePurchaseOrder.IdAssignee != SelectedAssignee.IdUser))
                    {
                        string OldAssignee = AssigneeList.FirstOrDefault(i => i.IdUser == UpdatedWarehousePurchaseOrder.IdAssignee).FullName;
                        UpdatedWarehousePurchaseOrder.IdAssignee = (uint?)SelectedAssignee.IdUser;
                        UpdatedWarehousePurchaseOrder.WarehousePOLogEntries.Add(new LogEntriesByWarehousePO()
                        {
                            IdWarehousePurchaseOrder = UpdatedWarehousePurchaseOrder.IdWarehousePurchaseOrder,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            IdEntryType = 258,
                            IsRtfText = false,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            People = new People()
                            {
                                IdPerson = GeosApplication.Instance.ActiveUser.IdUser,
                                Name = GeosApplication.Instance.ActiveUser.FirstName,
                                Surname = GeosApplication.Instance.ActiveUser.LastName
                            },
                            Comments = string.Format(System.Windows.Application.Current.FindResource("POAssigneeCommentChangeLog").ToString(), OldAssignee, SelectedAssignee.FullName)
                        });
                    }

                    if ((UpdatedWarehousePurchaseOrder.IdApprover != SelectedApprover.IdUser))
                    {
                        string OldApprover = ApproverList.FirstOrDefault(i => i.IdUser == UpdatedWarehousePurchaseOrder.IdApprover).FullName;
                        UpdatedWarehousePurchaseOrder.IdApprover = (uint?)SelectedApprover.IdUser;
                        UpdatedWarehousePurchaseOrder.WarehousePOLogEntries.Add(new LogEntriesByWarehousePO()
                        {
                            IdWarehousePurchaseOrder = UpdatedWarehousePurchaseOrder.IdWarehousePurchaseOrder,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            IdEntryType = 258,
                            IsRtfText = false,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            People = new People()
                            {
                                IdPerson = GeosApplication.Instance.ActiveUser.IdUser,
                                Name = GeosApplication.Instance.ActiveUser.FirstName,
                                Surname = GeosApplication.Instance.ActiveUser.LastName
                            },
                            Comments = string.Format(System.Windows.Application.Current.FindResource("POApproverCommentChangeLog").ToString(), OldApprover, SelectedApprover.FullName)
                        });
                    }

                    //IsUpdated = SRMService.UpdateAssigneeAndApproverInPO_V2380(UpdatedWarehousePurchaseOrder);    
                    //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                    IsUpdated = SRMService.UpdateAssigneeAndApproverInPO_V2480(UpdatedWarehousePurchaseOrder);
                }

                if (IsUpdated)
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMWarehousePurchaseOrderUpdate").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonActionNew(object obj)
        {
            try
            {

                if (CommentsList != null && CommentsList.Count > 0)
                {
                    foreach (var item in CommentsList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add))
                    {
                        SRMOtItemsComment OtComment = new SRMOtItemsComment();
                        OtComment.Comments = item.Comments;
                        OtComment.CommentDate = Convert.ToDateTime(item.Datetime);
                        OtComment.IdUser = Convert.ToInt32(item.IdUser);
                        //OtComment.Idrevisionitem = Convert.ToInt32(TempOtItem.IdRevisionItem);
                        //OtComment.IdEntryType = 1719;
                        //CommentItem = SRMService.AddObservationCommentItem(OtComment, Site);
                    }
                }

                if (DeleteCommentsList != null && DeleteCommentsList.Count > 0)
                {
                    foreach (var item in DeleteCommentsList)
                    {
                        // bool result = SRMService.DeleteComment_V2340(item.IdComment, Site);
                    }
                }

                //if (WorkLogItemList != null && WorkLogItemList.Count > 0)
                //{
                //    foreach (var item in WorkLogItemList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add))
                //    {
                //        OTWorkingTime OTWorkingTime = new OTWorkingTime();
                //        OTWorkingTime.StartTime = item.StartTime;
                //        OTWorkingTime.EndTime = item.EndTime;
                //        OTWorkingTime.TotalTime = item.TotalTime;
                //        OTWorkingTime.IdOperator = item.UserShortDetail.IdUser;
                //        //OTWorkingTime.IdOTItem = item.IdOTItem;
                //        OTWorkingTime.IdOTItem = OtItems;
                //        OTWorkingTimeItem = SAMService.AddOtWorkingTimeWorkLogItem(IdOT, OtItems, OTWorkingTime, Site);
                //    }
                //}

                //IsSave = true;

                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ItemsAddedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);



                RequestClose(null, null);


                GeosApplication.Instance.Logger.Log("Method AddNewActionsAccept() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                //IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewActionsAccept() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-4407][Sudhir.Jangra][19/05/2023]
        public string GetEmailFrom()
        {
            OutlookEmailFrom.Application outlookEmail = new OutlookEmailFrom.Application();
            OutlookEmailFrom.Account account = outlookEmail.Session.Accounts.Cast<Microsoft.Office.Interop.Outlook.Account>().FirstOrDefault();

            if (account != null)
            {
                return account.SmtpAddress;
            }
            return string.Empty;
        }
        // [pramod.misal][GEOS2-4408][20.06.2023]
        private void OpenViewSupplierCommandCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenViewSupplierCommandCommandAction....", category: Category.Info, priority: Priority.Low);


                TableView detailView = (TableView)obj;

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

                EditArticleSupplierViewModel editArticleSupplierViewModel = new EditArticleSupplierViewModel();
                EditArticleSupplierView editArticleSupplierView = new EditArticleSupplierView();

                EventHandler handle = delegate { editArticleSupplierView.Close(); };
                editArticleSupplierViewModel.RequestClose += handle;

                //ArticleSupplier wpo = new ArticleSupplier();

                Warehouses warehouse = SRM.SRMCommon.Instance.Selectedwarehouse;

                warehouse.IdArticleSupplier = WarehousePurchaseOrder.IdArticleSupplier;
                editArticleSupplierViewModel.Init((ulong)warehouse.IdArticleSupplier, warehouse);
                editArticleSupplierView.DataContext = editArticleSupplierViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                // editArticleSupplierView.Owner = Window.GetWindow(ownerInfo);

                editArticleSupplierView.ShowDialog();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                if (editArticleSupplierViewModel.IsSaveChanges)
                {
                    Init(WarehousePurchaseOrder, SRMCommon.Instance.Selectedwarehouse); //[rdixit][GEOS2-4789][06.10.2023]
                    FillArticleSupplierList();
                }

                GeosApplication.Instance.Logger.Log("Method OpenViewSupplierCommandCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenViewSupplierCommandCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }
        // [pramod.misal][GEOS2-4408][20.06.2023]
        private void FillArticleSupplierList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleSupplierList...", category: Category.Info, priority: Priority.Low);

                if (SRM.SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    try
                    {
                        #region GEOS2-4403 Sudhir.Jangra 03/05/2023
                        List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                        long plantOwnerIds;
                        string plantConnection;
                        List<ArticleSupplier> tempMainSupplierList = new List<ArticleSupplier>();
                        ArticleSupplierList = new List<ArticleSupplier>();
                        foreach (var item in plantOwners)
                        {
                            plantOwnerIds = item.IdWarehouse;
                            plantConnection = item.Company.ConnectPlantConstr;
                            tempMainSupplierList = new List<ArticleSupplier>(SRMService.GetArticleSuppliersByWarehouse_V2390(plantOwnerIds, plantConnection));

                            if (tempMainSupplierList != null)
                            {
                                foreach (var articleSupplieritem in tempMainSupplierList.GroupBy(tpa => tpa.Country.Iso))
                                {

                                    ImageSource countryFlagImage = ByteArrayToBitmapImage(articleSupplieritem.ToList().FirstOrDefault().Country.CountryIconBytes);
                                    articleSupplieritem.ToList().Where(ari => ari.Country.Iso == articleSupplieritem.Key).ToList().ForEach(ari => ari.Country.CountryIconImage = countryFlagImage);
                                }
                            }
                            ArticleSupplierList.AddRange(tempMainSupplierList);
                        }
                        SelectedArticleSupplier = ArticleSupplierList.FirstOrDefault();

                        #endregion

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillArticleSupplierList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in FillArticleSupplierList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    ArticleSupplierList = new List<ArticleSupplier>(ArticleSupplierList);
                }
                else
                {
                    ArticleSupplierList = new List<ArticleSupplier>();
                }
                FillAccountAge();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillArticleSupplierList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleSupplierList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        // [pramod.misal][GEOS2-4408][20.06.2023]
        private void FillAccountAge()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountAge ...", category: Category.Info, priority: Priority.Low);
                int i = 0;
                if (ArticleSupplierList != null)
                {
                    foreach (ArticleSupplier age in ArticleSupplierList)
                    {
                        ArticleSupplierList[i].Age = Math.Round((double)((GeosApplication.Instance.ServerDateTime.Month - ArticleSupplierList[i].CreatedIn.Month) + 12 * (GeosApplication.Instance.ServerDateTime.Year - ArticleSupplierList[i].CreatedIn.Year)) / 12, 1);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountAge() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                        POEmailNotification poEmailNotification = (SRMService.GetPurchasingOrderNotificationDetails(WarehousePurchaseOrder.IdWarehousePurchaseOrder, GeosApplication.Instance.ActiveUser.IdUser));
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
        //[pramod.misal][GEOS2-4450][31.06.2023]
        public void DeleteCommentCommandAction(object parameter)
        {

            GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

            LogEntriesByWarehousePO commentObject = (LogEntriesByWarehousePO)parameter;


            bool result = false;
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 47))
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (CommentsList != null && CommentsList.Count > 0)
                    {
                        LogEntriesByWarehousePO Comment = (LogEntriesByWarehousePO)commentObject;
                        //result = SAMService.DeleteComment_V2340(Comment.IdComment,Site);
                        CommentsList.Remove(Comment);

                        if (DeleteCommentsList == null)
                            DeleteCommentsList = new ObservableCollection<LogEntriesByWarehousePO>();

                        DeleteCommentsList.Add(new LogEntriesByWarehousePO()
                        {
                            IdUser = Comment.IdUser,
                            IdWarehousePurchaseOrder = Comment.IdWarehousePurchaseOrder,
                            Comments = Comment.Comments,
                            IsRtfText = Comment.IsRtfText,
                            IdLogEntryByPurchaseOrder = Comment.IdLogEntryByPurchaseOrder
                        });
                        CommentsList = new ObservableCollection<LogEntriesByWarehousePO>(CommentsList);
                        SamComments = Comment;
                        IsDeleted = true;
                    }


                }
            }



            //NewItemComment = null;

            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        //[pramod.misal][GEOS2-4450][31.06.2023]
        //PRM
        private void AddCommentsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);

                AddSRMCommentsView addCommentsView = new AddSRMCommentsView();
                AddSRMCommentsViewModel addCommentsViewModel = new AddSRMCommentsViewModel();
                EventHandler handle = delegate { addCommentsView.Close(); };
                addCommentsViewModel.RequestClose += handle;
                addCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCommentsHeader").ToString();
                //addCommentsViewModel.IsNew = true;
                //addCommentsViewModel.Init();

                addCommentsView.DataContext = addCommentsViewModel;
                addCommentsView.ShowDialog();
                if (addCommentsViewModel.SelectedComment != null)
                {

                    if (CommentsList == null)
                        CommentsList = new ObservableCollection<LogEntriesByWarehousePO>();

                    addCommentsViewModel.SelectedComment.IdWarehousePurchaseOrder = warehousePurchaseOrder.IdWarehousePurchaseOrder;

                    if (AddCommentsList == null)
                        AddCommentsList = new List<LogEntriesByWarehousePO>();

                    AddCommentsList.Add(new LogEntriesByWarehousePO()
                    {
                        IdUser = addCommentsViewModel.SelectedComment.IdUser,
                        IdWarehousePurchaseOrder = addCommentsViewModel.SelectedComment.IdWarehousePurchaseOrder,
                        Comments = addCommentsViewModel.SelectedComment.Comments,
                        IsRtfText = addCommentsViewModel.SelectedComment.IsRtfText
                    });
                    CommentsList.Add(addCommentsViewModel.SelectedComment);
                    try
                    {
                        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                        CommentsList = new ObservableCollection<LogEntriesByWarehousePO>(CommentsList.OrderByDescending(x => x.Datetime));
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in AddCommentsCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    SelectedComment = addCommentsViewModel.SelectedComment;
                }
                GeosApplication.Instance.Logger.Log("Method AddCommentsCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddCommentsCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pramod.misal][GEOS2-4450][31.06.2023]
        public ImageSource SetUserProfileImage(LogEntriesByWarehousePO temp)
        {
            User user = new User();
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                user = WorkbenchStartUp.GetUserById(temp.IdUser);
                UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);

                if (UserProfileImageByte != null)
                    UserProfileImage = ByteArrayToBitmapImage(UserProfileImageByte);
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SRM;component/Assets/Images/FemaleUser_White.png");//
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueFemale.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SRM;component/Assets/Images/MaleUser_White.png");//
                        else if (user.IdUserGender == null)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SRM;component/Assets/Images/wUnknownGender.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueMale.png");

                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SRM;component/Assets/Images/FemaleUser_Blue.png");//
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SRM;component/Assets/Images/MaleUser_Blue.png");//
                        else if (user.IdUserGender == null)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.SRM;component/Assets/Images/blueUnknownGender.png");//
                    }
                }


                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return UserProfileImage;
        }
        //[pramod.misal][GEOS2-4450][31.06.2023]
        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }
        //[pramod.misal][GEOS2-4450][17.08.2023]
        private void CommentDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction()...", category: Category.Info, priority: Priority.Low);

            AddSRMCommentsView editCommentsView = new AddSRMCommentsView();
            AddSRMCommentsViewModel editCommentsViewModel = new AddSRMCommentsViewModel();
            EventHandler handle = delegate { editCommentsView.Close(); };
            editCommentsViewModel.RequestClose += handle;
            editCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCommentsHeader").ToString();
            editCommentsViewModel.NewItemComment = SelectedComment.Comments;
            string oldComments = SelectedComment.Comments;
            editCommentsView.DataContext = editCommentsViewModel;
            editCommentsView.ShowDialog();

            if (editCommentsViewModel.SelectedComment != null)
            {
                SelectedComment.Comments = editCommentsViewModel.NewItemComment;

                if (UpdatedCommentsList == null)
                    UpdatedCommentsList = new List<LogEntriesByWarehousePO>();

                editCommentsViewModel.SelectedComment.IdWarehousePurchaseOrder = warehousePurchaseOrder.IdWarehousePurchaseOrder;
                //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                if (!oldComments.ToLower().Equals(editCommentsViewModel.NewItemComment.ToLower()))
                {
                    UpdatedCommentsList.Add(new LogEntriesByWarehousePO()
                    {
                        IdUser = SelectedComment.IdUser,
                        IdWarehousePurchaseOrder = SelectedComment.IdWarehousePurchaseOrder,
                        Comments = SelectedComment.Comments,
                        IsRtfText = SelectedComment.IsRtfText,
                        IdLogEntryByPurchaseOrder = SelectedComment.IdLogEntryByPurchaseOrder,
                        Datetime = DateTime.Now

                    });
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByPurchaseOrder == SelectedComment.IdLogEntryByPurchaseOrder).Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByPurchaseOrder == SelectedComment.IdLogEntryByPurchaseOrder).Datetime = DateTime.Now;
                    try
                    {
                        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                        CommentsList = new ObservableCollection<LogEntriesByWarehousePO>(CommentsList.OrderByDescending(x => x.Datetime));
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in CommentDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }

            }
        }

        //[Sudhir.Jangra][GEOS2-5493]
        private void FillToCC(WarehousePurchaseOrder wpo)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillToCC()...", category: Category.Info, priority: Priority.Low);
                //[rdixit][21.05.2024][GEOS2-5762]
                List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                ObservableCollection<Contacts> temp = new ObservableCollection<Contacts>();
                //temp.AddRange(SRMService.GetArticleSuppliersOrders_V2510(Convert.ToInt32(IdArticleSupplierForTOAndCC), (long)wpo.IdWarehouse));
                //pramod.misal 31.05.2024
                temp.AddRange(SRMService.GetArticleSuppliersOrders_V2520(Convert.ToInt32(IdArticleSupplierForTOAndCC), (long)wpo.IdWarehouse));
                GeosApplication.Instance.Logger.Log("Method FillToCC()... recived to and cc emails in list ", category: Category.Info, priority: Priority.Low);

                if (temp?.Count > 0)
                {
                    ToContactList = new ObservableCollection<Contacts>(temp.Where(x => x.IdType == 1918));
                    CCContactList = new ObservableCollection<Contacts>(temp.Where(x => x.IdType == 1919));
                }

                if (ToContactList?.Count > 0)
                {
                    GeosApplication.Instance.Logger.Log("Method FillToCC()... list have at least 1 To email " + string.Join(",", ToContactList.Select(i => i.Email)), category: Category.Info, priority: Priority.Low);
                    ToContactList = new ObservableCollection<Contacts>(ToContactList.OrderByDescending(x => x.IsMainContact).ToList());
                    ToContactList.FirstOrDefault().IsMainContact = true;
                }
                else
                    GeosApplication.Instance.Logger.Log("Method FillToCC()... list dont have at least 1 To email ", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Exception in Method FillToCC()..." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        #endregion
    }
}
