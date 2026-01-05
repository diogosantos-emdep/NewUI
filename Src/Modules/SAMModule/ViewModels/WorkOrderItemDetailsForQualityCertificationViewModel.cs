using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
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
//using Emdep.Geos.Modules.Warehouse.Reports;
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
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.Helper;
using System.Text.RegularExpressions;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Data.Common.File;
using DevExpress.Compression;
using Emdep.Geos.Utility;
using Emdep.Geos.Modules.SAM.CommonClasses;
namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class WorkOrderItemDetailsForQualityCertificationViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Task log
        //
        #endregion
        #region Services
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceToGetFiles = null;
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services
        #region Declaration
        private bool isSaveChanges;
        private String fileName;
        private string uniqueFileName;
        private OTAttachment attachment;
        private string fileNameString;
        private Ots ot;
        private double dialogHeight;
        private double dialogWidth;
        private List<OTWorkingTime> workLogItemList;
        private string worklogTotalTime;
        private List<UserShortDetail> userImageList;
        private bool isBusy;
        private bool isAssignedUsers;
        private MaximizedElementPosition maximizedElementPosition;
        private OtItem selectedOtItem;
        private List<LogEntryBySite> tempListChangeLog = new List<LogEntryBySite>();
        private ObservableCollection<OTAssignedUser> userToAssingedUser;
        private ObservableCollection<OTAssignedUser> otAssignedUser;
        List<OTAssignedUser> cloneOTAssignedUser = new List<OTAssignedUser>();
        private ObservableCollection<OTAssignedUser> otAddDeleteAssignedUser;
        Ots objOT = new Ots();
        OTAssignedUser OTAssignedUser = new OTAssignedUser();
        //  private bool showAttachmentFlyout;
        private ObservableCollection<OTAttachment> listAttachment;
        private OTAttachment selectedAttachment;
        private ObservableCollection<OTAttachment> listAddedAttachment;
        private List<object> otAttachmentList;
        private Company otSite;
        private OTWorkingTime selectedWorklogRow;
        private bool isRemoveWorkLog;
        private bool isEditWorkLog;
        private string infoTooltipBackColor;
        private WorkflowStatus workflowStatus;
        private List<WorkflowStatus> workflowStatusList;
        private List<WorkflowStatus> workflowStatusButtons;
        private WorkflowStatus selectedWorkflowStatusButton;
        private List<WorkflowTransition> workflowTransitionList;
        private string remark;
        private bool showCommentsFlyout;
        private ObservableCollection<OTAttachment> listUpdateOTAttachment;
        private Visibility isPrintLabelVisible; //[nsatpute][04-09-2024][GEOS2-5415]
        #endregion // End Of Declaration
        #region Public Properties
        public string GuidCode { get; set; }
        public bool ShowCommentsFlyout
        {
            get { return showCommentsFlyout; }
            set
            {
                showCommentsFlyout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowCommentsFlyout"));
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
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged(new PropertyChangedEventArgs("FileName")); }
        }
		//[nsatpute][04-09-2024][GEOS2-5415]
        PrintLabel PrintLabel { get; set; }
        Dictionary<string, string> PrintValues { get; set; }
        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set { uniqueFileName = value; OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName")); }
        }
        public OTAttachment Attachment
        {
            get { return attachment; }
            set { attachment = value; OnPropertyChanged(new PropertyChangedEventArgs("Attachment")); }
        }
        public string FileNameString
        {
            get { return fileNameString; }
            set { fileNameString = value; OnPropertyChanged(new PropertyChangedEventArgs("FileNameString")); }
        }
        public ObservableCollection<OTAttachment> ListUpdateOTAttachment
        {
            get { return listUpdateOTAttachment; }
            set { listUpdateOTAttachment = value; OnPropertyChanged(new PropertyChangedEventArgs("ListUpdateOTAttachment")); }
        }
        public Warehouses WarehouseDetails { get; set; }
        public Ots OT
        {
            get { return ot; }
            set
            {
                ot = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
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
        public List<OTWorkingTime> WorkLogItemList
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
        public bool IsAssignedUsers
        {
            get { return isAssignedUsers; }
            set
            {
                isAssignedUsers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAssignedUsers"));
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
        public OtItem SelectedOtItem
        {
            get { return selectedOtItem; }
            set
            {
                selectedOtItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOtItem"));
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
        public ObservableCollection<OTAssignedUser> OtAddDeleteAssignedUser
        {
            get { return otAddDeleteAssignedUser; }
            set
            {
                otAddDeleteAssignedUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtAddDeleteAssignedUser"));
            }
        }
        public ObservableCollection<OTAttachment> ListAttachment
        {
            get { return listAttachment; }
            set
            {
                listAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAttachment"));
            }
        }
        public OTAttachment SelectedAttachment
        {
            get { return selectedAttachment; }
            set
            {
                selectedAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttachment"));
            }
        }
        public ObservableCollection<OTAttachment> ListAddedAttachment
        {
            get { return listAddedAttachment; }
            set
            {
                listAddedAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAddedAttachment"));
            }
        }
        public List<object> OTAttachmentList
        {
            get { return otAttachmentList; }
            set
            {
                otAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTAttachmentList"));
            }
        }
        public Company OtSite
        {
            get { return otSite; }
            set
            {
                otSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtSite"));
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
		//[nsatpute][04-09-2024][GEOS2-5415]
        public Visibility IsPrintLabelVisible
        {
            get { return isPrintLabelVisible; }
            set { isPrintLabelVisible = value; OnPropertyChanged(new PropertyChangedEventArgs("IsPrintLabelVisible")); }
        }
        public string Remark
        {
            get
            {
                return remark;
            }
            set
            {
                remark = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remark"));
            }
        }
        bool isCancel = false;
        public bool IsCancel
        {
            get { return isCancel; }
            set
            {
                isCancel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancel"));
            }
        }
        #endregion
        #region ICommands
        public ICommand CommandWorkOrderCancel { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand ScanMaterialsCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand PrintWorkOrderItemCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand CurrentItemChangedCommand { get; set; }
        public ICommand GetWorkOrderUserContactCommand { get; set; }
        public ICommand AssignedWorkOrderItemCancelCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand EditWorkOrderViewAcceptButtonCommand { get; set; }
        //Attachment
        public ICommand DownloadFileCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand UploadFileCommand { get; set; }
        public ICommand CustomSortColumnDataCommand { get; set; }
        public ICommand ExportWorkLogCommand { get; set; }
        public ICommand DeleteWorkLogCommand { get; set; }
        public ICommand EditWorkLogDoubleClickCommand { get; set; }
        public ICommand ImageClickCommand { get; set; }
        public ICommand WorkflowButtonClickCommand { get; set; }
        public ICommand OpenWorkflowDiagramCommand { get; set; }
        public ICommand QualityCertificationViewCommand { get; set; }
        public ICommand PrintQcPassLabelCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand EditCommand { get; set; }
        #endregion // End Of ICommands
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public event EventHandler RequestClose;
        #endregion // End Of Events
        #region Constructor
        public WorkOrderItemDetailsForQualityCertificationViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsViewModel()...", category: Category.Info, priority: Priority.Low);
                IsCancel = false;
                //GeosApplication.CurrentCultureInfo
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 95;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                PopupMenuShowingCommand = new DelegateCommand<OpenPopupEventArgs>(OpenPopupDateEditAction);
                CommandWorkOrderCancel = new DelegateCommand<object>(CommandWorkOrderCancelAction);
                CurrentItemChangedCommand = new DelegateCommand<object>(CurrentItemChangedCommandAction);
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                EditWorkOrderViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveWorkOrderDetails));
                DownloadFileCommand = new DelegateCommand<object>(DownloadFileCommandAction);
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFile));
                UploadFileCommand = new DelegateCommand<object>(UploadFileCommandAction);
                CustomSortColumnDataCommand = new DelegateCommand<object>(CustomSortColumnDataAction);
                ImageClickCommand = new DelegateCommand<object>(ImageClickCommandAction);
                OTAttachmentList = new List<object>();
                ListAttachment = new ObservableCollection<OTAttachment>();
                ListUpdateOTAttachment = new ObservableCollection<OTAttachment>();
                ExportWorkLogCommand = new RelayCommand(new Action<object>(ExportWorkLogCommandAction));
                DeleteWorkLogCommand = new DelegateCommand<object>(DeletWorklogRowCommandAction);
                EditWorkLogDoubleClickCommand = new DelegateCommand<object>(EditWorklog);
                WorkflowButtonClickCommand = new DelegateCommand<object>(WorkflowButtonClickCommandAction);
                OpenWorkflowDiagramCommand = new DelegateCommand<object>(OpenWorkflowDiagramCommandAction);
                QualityCertificationViewCommand = new DelegateCommand<object>(QualityCertificationViewCommandAction);
                PrintQcPassLabelCommand = new DelegateCommand<object>(PrintQcPassLabelCommandAction);
                DeleteFileCommand = new DelegateCommand<object>(DeleteAttachmentFile);
                //EditCommand = new RelayCommand(new Action<object>(EditAction));
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor WorkOrderItemDetailsViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // End Of Constructor
        #region Command Actions
        /// <summary>
        /// Method for close window.
        /// </summary>
        /// <param name="obj"></param>
        private void CommandWorkOrderCancelAction(object obj)
        {
            IsCancel = true;
            RequestClose(null, null);
        }
        public void Dispose()
        {
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
                    if (SAMCommon.Instance.Articles.Any(x => x.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle))
                    {
                        Article article = SAMCommon.Instance.Articles.FirstOrDefault(x => x.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                        SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = article.ArticleImage;
                    }
                    else
                    {
                        Article article = SAMService.GetArticleDetails(SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                        if (article.ArticleImageInBytes == null)
                        {
                            article.ArticleImage = new BitmapImage(new Uri("/Emdep.Geos.Modules.SAM;component/Assets/Images/EmptyImage.png", UriKind.Relative));
                        }
                        else if (article.ArticleImageInBytes != null)
                        {
                            article.ArticleImage = SAMCommon.Instance.ByteArrayToBitmapImage(article.ArticleImageInBytes);
                        }
                        SAMCommon.Instance.Articles.Add(article);
                        SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = article.ArticleImage;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CurrentItemChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
        //[001][nsatpute][04-07-2024][GEOS2-5408]
        //[002][nsatpute][04-09-2024][GEOS2-5415]
        // [nsatpute][05-09-2024] [IESD-116556]
		//[004][nsatpute][29-11-2024][GEOS2-6621]
        private void TransitionWorkflowStatus(int currentStatus)
        {
            List<LogEntriesByOT> LogEntriesByOTList = new List<LogEntriesByOT>();
            WorkflowTransition workflowTransition = new WorkflowTransition();
            workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == currentStatus);
            WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == currentStatus);
            if (currentStatus == 14 && GeosApplication.Instance.IsSAMReport)
            {
                DocumentsIdentificationView documentsIdentificationView = new DocumentsIdentificationView();
                EventHandler handle = delegate { documentsIdentificationView.Close(); };
                DocumentsIdentificationViewModel documentsIdentificationViewModel = new DocumentsIdentificationViewModel(ot);
                documentsIdentificationViewModel.RequestClose += handle;
                documentsIdentificationView.DataContext = documentsIdentificationViewModel;
                if (documentsIdentificationViewModel.DocumentsIdentificationList.Count > 0)
                    documentsIdentificationView.ShowDialog();
                else
                    documentsIdentificationViewModel.IsSave = true;
                if (!documentsIdentificationViewModel.IsSave)
                    return;
            }
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
                    LogEntriesByOT CommentByOT = new LogEntriesByOT();
                    CommentByOT.IdOT = ot.IdOT;
                    CommentByOT.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                    CommentByOT.Comments = addWorkflowStatusCommentViewModel.Comment;
                    CommentByOT.IdLogEntryType = 252;
                    CommentByOT.IsRtfText = false;
                    CommentByOT.Datetime = GeosApplication.Instance.ServerDateTime;
                    CommentByOT.People = new People();
                    CommentByOT.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                    CommentByOT.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                    CommentByOT.People.Surname = GeosApplication.Instance.ActiveUser.LastName;
                    LogEntriesByOTList.Add(CommentByOT);
                    if (OT.OTComments == null)
                        OT.OTComments = new List<LogEntriesByOT>();
                    OT.OTComments.Insert(0, CommentByOT);
                    OT.OTComments = new List<LogEntriesByOT>(ot.OTComments);
                    LogEntriesByOT logEntriesByOT_1 = new LogEntriesByOT();
                    logEntriesByOT_1.IdOT = ot.IdOT;
                    logEntriesByOT_1.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                    logEntriesByOT_1.Comments = string.Format(System.Windows.Application.Current.FindResource("POWorkflowStatusCommentChangeLog").ToString(), WorkflowStatus.Name);
                    logEntriesByOT_1.IdLogEntryType = 253;
                    logEntriesByOT_1.IsRtfText = false;
                    logEntriesByOT_1.Datetime = GeosApplication.Instance.ServerDateTime;
                    logEntriesByOT_1.People = new People();
                    logEntriesByOT_1.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
                    logEntriesByOT_1.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
                    logEntriesByOT_1.People.Surname = GeosApplication.Instance.ActiveUser.LastName;
                    LogEntriesByOTList.Add(logEntriesByOT_1);
                    if (OT.OTLogEntries == null)
                        OT.OTLogEntries = new List<LogEntriesByOT>();
                    OT.OTLogEntries.Insert(0, logEntriesByOT_1);
                    OT.OTLogEntries = new List<LogEntriesByOT>(ot.OTLogEntries);
                }
                else
                {
                    workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == OT.IdWorkflowStatus);
                    WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == OT.IdWorkflowStatus);
                    OT.WorkflowStatus = WorkflowStatus;
                    List<byte> GetCurrentButtons1 = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == OT.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                    WorkflowStatusButtons = new List<WorkflowStatus>();
                    foreach (byte statusbutton in GetCurrentButtons1)
                    {
                        WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                    }
                    WorkflowStatusButtons = new List<WorkflowStatus>(WorkflowStatusButtons);
                    return;
                }
            }
            LogEntriesByOT logEntriesByOT = new LogEntriesByOT();
            logEntriesByOT.IdOT = ot.IdOT;
            logEntriesByOT.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
            logEntriesByOT.Comments = string.Format(System.Windows.Application.Current.FindResource("WorkflowStatusLogEntryByPO").ToString(), OT.WorkflowStatus.Name, WorkflowStatus.Name);
            logEntriesByOT.IdLogEntryType = 253;
            logEntriesByOT.IsRtfText = false;
            logEntriesByOT.Datetime = GeosApplication.Instance.ServerDateTime;
            logEntriesByOT.People = new People();
            logEntriesByOT.People.IdPerson = GeosApplication.Instance.ActiveUser.IdUser;
            logEntriesByOT.People.Name = GeosApplication.Instance.ActiveUser.FirstName;
            logEntriesByOT.People.Surname = GeosApplication.Instance.ActiveUser.LastName;
            LogEntriesByOTList.Add(logEntriesByOT);
            if (OT.OTLogEntries == null)
                OT.OTLogEntries = new List<LogEntriesByOT>();
            OT.OTLogEntries.Insert(0, logEntriesByOT);
            OT.OTLogEntries = new List<LogEntriesByOT>(OT.OTLogEntries);
            IsSaveChanges = SAMService.UpdateWorkflowStatusInOTQC_V2580((UInt64)OT.IdOT, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByOTList); // [nsatpute][12-11-2024][GEOS2-5890]
            if(IsSaveChanges)
            {
                OtSite = ot.Site;
                OT = SAMService.GetWorkOrderByIdOt_V2180(ot.IdOT, ot.Site);
                OT.Site = OtSite;
            }
            OT.IdWorkflowStatus = WorkflowStatus.IdWorkflowStatus;
            OT.WorkflowStatus = WorkflowStatus;
            List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == currentStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
            WorkflowStatusButtons = new List<WorkflowStatus>();
            foreach (byte statusbutton in GetCurrentButtons)
            {
                WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
            }
            WorkflowStatusButtons = new List<WorkflowStatus>(WorkflowStatusButtons);
            ShowHidePrintLabel();
            //if(IsSaveChanges==true)
            //{
            //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("POWorkflowStatusMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            //}
            //RequestClose(null, null);
        }
        #region Old DeleteAttachmentFile
        //private void DeleteAttachmentFile(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method DeleteAttachmentFile()...", category: Category.Info, priority: Priority.Low);
        //        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
        //        if (MessageBoxResult == MessageBoxResult.Yes)
        //        {
        //            ListAttachment.Remove(SelectedAttachment);
        //            SelectedAttachment.TransactionOperation = ModelBase.TransactionOperations.Delete;
        //            if (ListAddedAttachment == null)
        //                ListAddedAttachment = new ObservableCollection<OTAttachment>();
        //            //SelectedAttachment.AttachmentImage = null;
        //            ListAddedAttachment.Add(SelectedAttachment);
        //        }
        //        GeosApplication.Instance.Logger.Log("Method DeleteAttachmentFile()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method DeleteAttachmentFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        #endregion DeleteAttachmentFile
        private void DeleteAttachmentFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAttachmentFile()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    SelectedAttachment = (OTAttachment)obj;
                    SelectedAttachment.TransactionOperation = ModelBase.TransactionOperations.Delete;
                    if (ListAddedAttachment == null)
                        ListAddedAttachment = new ObservableCollection<OTAttachment>();
                    //SelectedAttachment.AttachmentImage = null;
                    //ListAddedAttachment.Add(SelectedAttachment);
                    SelectedAttachment = (OTAttachment)obj;
                    ListAddedAttachment.Add(SelectedAttachment);
                    ListAttachment.Remove(SelectedAttachment);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteAttachmentFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteAttachmentFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void QualityCertificationViewCommandAction(object obj)
        {
            try
            {
                DownloadTemplateFileViewModel downloadTemplateFileViewModel = new DownloadTemplateFileViewModel();
                DownloadTemplateFileView downloadTemplateFileView = new DownloadTemplateFileView();
                EventHandler handle1 = delegate { downloadTemplateFileView.Close(); };
                downloadTemplateFileViewModel.RequestClose += handle1;
                downloadTemplateFileViewModel.Init(OT);
                downloadTemplateFileView.DataContext = downloadTemplateFileViewModel;
                downloadTemplateFileView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method QualityCertificationViewCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method QualityCertificationViewCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
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
        private void DeletWorklogRowCommandAction(object obj)
        {
            try
            {
                bool result = false;
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 47))
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["WorkLogDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (WorkLogItemList != null && WorkLogItemList.Count > 0)
                        {
                            OTWorkingTime OTworktime = (OTWorkingTime)obj;
                            result = SAMService.DeleteWorkLog(OtSite, OTworktime.IdOTWorkingTime);
                            WorkLogItemList.Remove(OTworktime);
                            WorkLogItemList = new List<OTWorkingTime>(WorkLogItemList);
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
        /// <summary>
        /// [000][20200727]][srowlo][GEOS2-2374] Add a new permission to Edit work log entries
        /// </summary>
        /// <param name="obj"></param>
        private void EditWorklog(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditWorklog()...", category: Category.Info, priority: Priority.Low);
            TableView detailView = (TableView)obj;
            OTWorkingTime worklog = (OTWorkingTime)detailView.DataControl.CurrentItem;
            SelectedWorklogRow = worklog;
            if (worklog != null)
            {
                EditWorkLogView editWorklLogView = new EditWorkLogView();
                EditWorkLogViewModel editWorkLogViewModel = new EditWorkLogViewModel();
                EventHandler handle = delegate { editWorklLogView.Close(); };
                editWorkLogViewModel.RequestClose += handle;
                editWorklLogView.DataContext = editWorkLogViewModel;
                // ExistWorkLogItemList = new List<OTWorkingTime>(WorkLogItemList.ToList());
                editWorkLogViewModel.EditInit(WorkLogItemList, worklog, OtAssignedUser, OtSite);
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
                        WorkLogItemList = new List<OTWorkingTime>(WorkLogItemList);
                    }
                }
                //else
                //{
                //    SelectedWorklogRow =  worklog;
                //}
            }
            GeosApplication.Instance.Logger.Log("Method EditWorklog()....executed successfully", category: Category.Info, priority: Priority.Low);
        }
		//[nsatpute][03-09-2024][GEOS2-5415]
        private void PrintQcPassLabelCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PrintQcPassLabelCommandAction WorkOrderItemDetailsForQualityCertificationViewModel....", category: Category.Info, priority: Priority.Low);
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
                        //WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                GeosRepositoryServiceToGetFiles = new GeosRepositoryServiceController(ot.Site.ServiceProviderUrl);
                QcPassLabelDetails labeldetails = SAMService.GetQCPassLabelDetails(ot.Site, ot.IdOT);
                CreatePrintValues(labeldetails);
                byte[]  printFile = GeosRepositoryServiceToGetFiles.GetPrintQcPassLabelFile(GeosApplication.Instance.UserSettings["SAM_LabelPrinterModel"]);
                PrintLabel lblPrint = new PrintLabel(PrintValues,printFile);
                lblPrint.Print();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("PrintQcPassLabelCommandAction WorkOrderItemDetailsForQualityCertificationViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintQcPassLabelCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintQcPassLabelCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintQcPassLabelCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        #endregion
        #region Methods
        public void Init(Ots ot)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = MaximizedElementPosition.Right;
                OtSite = ot.Site;
                OT = SAMService.GetWorkOrderByIdOt_V2180(ot.IdOT, ot.Site);
                OT.OfferCode = ot.OfferCode;
                OT.Modules = ot.Modules;
                OT.Site = OtSite;
                Remark = OT.Observations;
                SelectedOtItem = OT.OtItems.FirstOrDefault();
                //set info tooltip back color
                GeosAppSetting GeosAppSetting = WorkbenchService.GetGeosAppSettings(37);
                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;
                FillListAttachment(ot.Site, ot.IdOT);
                objOT.IdOT = ot.IdOT;
                objOT.Site = ot.Site;
                WorkflowStatusList = new List<WorkflowStatus>(SAMService.GetAllWorkflowStatusForQuality());
                WorkflowTransitionList = new List<WorkflowTransition>(SAMService.GetAllWorkflowTransitionsForQuality());
                List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == OT.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                WorkflowStatusButtons = new List<WorkflowStatus>();
                foreach (byte statusbutton in GetCurrentButtons)
                {
                    WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                }
                ShowHidePrintLabel();
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
        //[nsatpute][18-02-2025][GEOS2-6997]
        public void InitRemoteService(string serviceProviderUrl)
        {
            if (!string.IsNullOrEmpty(serviceProviderUrl))
            {
                SAMService = new SAMServiceController(serviceProviderUrl);
            }
        }
        /// <summary>
        /// Method to fill Attachments Section
        /// </summary>
        /// <param name="company"></param>
        /// <param name="idOT"></param>
        private void FillListAttachment(Company company, long idOT)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillListAttachment ...", category: Category.Info, priority: Priority.Low);
                //[pramod.misal][GEOS2-5327][05.03.2024]
                SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == company.IdCompany);
                SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                // SAMService = new SAMServiceController("localhost:6699");
                ListAttachment = new ObservableCollection<OTAttachment>(SAMService.GetQualityOTAttachment(company, idOT).ToList());
                //[pramod.misal][GEOS2-5327][05.03.2024]
                SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == company.IdCompany);
                foreach (OTAttachment items in ListAttachment)
                {
                    ImageSource imageObj = FileExtensionToFileIcon.FindIconForFilename(items.FileName, true);
                    items.AttachmentImage = imageObj;
                }
                GeosApplication.Instance.Logger.Log("Method FillListAttachment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to Set maximized element position.
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
        public bool UploadOTAttachmentFiles(string year, string quotationCode)
        {
            bool isupload = false;
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFile() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                List<FileInfo> FileDetail = new List<FileInfo>();
                FileOTAttachmentUploader otAttachmentFileUploader = new FileOTAttachmentUploader();
                otAttachmentFileUploader.FileUploadName = GUIDCode.GUIDCodeString();
                GuidCode = otAttachmentFileUploader.FileUploadName;
                otAttachmentFileUploader.Year = year;
                otAttachmentFileUploader.QuotationCode = quotationCode;
                if (ListAddedAttachment != null && ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).Count() > 0)
                {
                    foreach (OTAttachment fs in ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).ToList())
                    {
                        FileInfo file = new FileInfo(fs.FilePath);
                        fs.AttachmentImage = null;
                        FileDetail.Add(file);
                    }
                    otAttachmentFileUploader.FileByte = ConvertZipToByte(FileDetail, otAttachmentFileUploader.FileUploadName);
                    GeosApplication.Instance.Logger.Log("Getting Upload ot Attachment FileUploader Zip File ", category: Category.Info, priority: Priority.Low);
                    //[pramod.misal][GEOS2-5327][06-03-2024]
                    SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                    GeosRepositoryServiceController = new GeosRepositoryServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    // GeosRepositoryServiceController = new GeosRepositoryServiceController("localhost:6699");
                    fileUploadReturnMessage = GeosRepositoryServiceController.UploaderOTAttachmentZipFile(otAttachmentFileUploader);
                    GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    GeosApplication.Instance.Logger.Log("Getting Upload ot Attachment FileUploader Zip File successfully", category: Category.Info, priority: Priority.Low);
                }
                if (fileUploadReturnMessage.IsFileUpload == true)
                {
                    isupload = true;
                    IsBusy = false;
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
                    if (!string.IsNullOrEmpty(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    GeosApplication.Instance.Logger.Log("Method UploadFile() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    IsBusy = false;
                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return isupload;
        }
        /// <summary>
        /// This method for Save Work Order Details.
        /// </summary>
        /// <param name="obj"></param>
        private void SaveWorkOrderDetails(object obj)
        {
            try
            {
                string attachmentfiles = null;
                if (!string.IsNullOrEmpty(Remark))
                    Remark = Remark.Trim();
                if (ListAddedAttachment != null && ListAddedAttachment.Count > 0)
                {
                    ListAddedAttachment.ToList().ForEach(x => x.AttachmentImage = null);
                    bool isupload = false;
                    if (ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).Count() > 0)
                        isupload = UploadOTAttachmentFiles(OT.Quotation.Year.ToString(), OT.Quotation.Code);               // Upload attachment on Server method
                    if (isupload == true || ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Delete).Count() > 0 || (OT.Observations != Remark))
                    {
                        if (ListAttachment == null || ListAttachment.Count == 0)
                        {
                            attachmentfiles = null;
                        }
                        else
                        {
                            attachmentfiles = string.Join(";", ListAttachment.Select(i => i.FileName).ToList());
                        }
                        //[pramod.misal][GEOS2-5327][06-03-2024]
                        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                        SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        // SAMService = new SAMServiceController("localhost:6699");
                        SAMService.UpdateOTQuality(objOT.Site, OT.IdOT, GeosApplication.Instance.ActiveUser.IdUser, attachmentfiles, GuidCode, OT.Quotation.Year.ToString(), OT.Quotation.Code, ((ListAddedAttachment == null || ListAddedAttachment.Count == 0) ? null : ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Delete).ToList()), OT.Observations, Remark);
                        //[pramod.misal][GEOS2-5327][05.03.2024]
                        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        ListAddedAttachment = new ObservableCollection<OTAttachment>();
                    }
                }
                else if (OT.Observations != Remark)
                {
                    //[pramod.misal][GEOS2-5327][06-03-2024]
                    SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                    SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    // SAMService = new SAMServiceController("localhost:6699");
                    SAMService.UpdateOTQuality(objOT.Site, OT.IdOT, GeosApplication.Instance.ActiveUser.IdUser, attachmentfiles, GuidCode, OT.Quotation.Year.ToString(), OT.Quotation.Code, ((ListAddedAttachment == null || ListAddedAttachment.Count == 0) ? null : ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Delete).ToList()), OT.Observations, Remark);
                    //[pramod.misal][GEOS2-5327][05.03.2024]
                    SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                }
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOrderEditViewEditedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SaveWorkOrderDetails() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        /// <summary>
        /// This method for Send Mail to Person
        /// </summary>
        /// <param name="obj"></param>
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            IsBusy = false;
        }
        /// <summary>
        /// save Downloaded file
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected bool SaveData(string FileName, byte[] Data)
        {
            BinaryWriter Writer = null;
            string Name = FileName;
            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.OpenWrite(Name));
                // Writer raw data
                Writer.Write(Data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                //...
                return false;
            }
            return true;
        }
        protected ISaveFileDialogService SaveFileDialogService
        {
            get
            {
                return this.GetService<ISaveFileDialogService>();
            }
        }
        /// <summary>
        /// Method for convert zip to byte.
        /// </summary>
        /// <param name="filesDetail"></param>
        /// <param name="GuidCode"></param>
        /// <returns></returns>
        private byte[] ConvertZipToByte(List<FileInfo> filesDetail, string GuidCode)
        {
            byte[] filedetails = null;
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";
            string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolder\";
            if (!Directory.Exists(tempfolderPath))
            {
                System.IO.Directory.CreateDirectory(tempfolderPath);
            }
            try
            {
                GeosApplication.Instance.Logger.Log("add files into zip", category: Category.Info, priority: Priority.Low);
                using (ZipArchive archive = new ZipArchive())
                {
                    if (filesDetail.Count > 0)
                    {
                        for (int i = 0; i < filesDetail.Count; i++)
                        {
                            if (!File.Exists(tempfolderPath + filesDetail[i].Name))
                            {
                                System.IO.File.Copy(filesDetail[i].FullName, tempfolderPath + filesDetail[i].Name);
                            }
                            string s = tempfolderPath + filesDetail[i].Name;
                            archive.AddFile(s, @"/");
                            // filesDetail[i].FilePath = s;
                        }
                        archive.Save(tempPath + GuidCode + ".zip");
                        filedetails = File.ReadAllBytes(tempPath + GuidCode + ".zip");
                    }
                }
                GeosApplication.Instance.Logger.Log("zip created successfully", category: Category.Info, priority: Priority.Low);
                return filedetails;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On ConvertZipToByte Method", category: Category.Exception, priority: Priority.Low);
                DeleteTempFolder();
                return filedetails;
            }
        }
        /// <summary>
        /// Method for delete TempFolder folders.
        /// </summary>
        private void DeleteTempFolder()
        {
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
        /// <summary>
        /// Method to download the file
        /// </summary>
        /// <param name="obj"></param>
        public void DownloadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool isDownload = false;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AttachmentsFileDownloadMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    OTAttachment attachmentObject = (OTAttachment)obj;
                    SaveFileDialogService.DefaultExt = attachmentObject.FileExtension;
                    SaveFileDialogService.DefaultFileName = attachmentObject.FileName;
                    SaveFileDialogService.Filter = "All Files|*.*";
                    SaveFileDialogService.FilterIndex = 1;
                    DialogResult = SaveFileDialogService.ShowDialog();
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
                                //WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                                win.Topmost = false;
                                return win;
                            }, x =>
                            {
                                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                            }, null, null);
                        }
                        //[pramod.misal][GEOS2-5327][05.03.2024]
                        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                        SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        Byte[] byteObj = SAMService.GetOTAttachmentInBytes(attachmentObject.FileName, attachmentObject.QuotationYear, attachmentObject.QuotationCode);
                        //[pramod.misal][GEOS2-5327][05.03.2024]
                        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                        ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                        isDownload = SaveData(ResultFileName, byteObj);
                    }
                    if (isDownload)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        IsBusy = false;
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        IsBusy = false;
                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    IsBusy = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Upload File
        /// </summary>
        public void UploadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                List<FileInfo> FileDetail = new List<FileInfo>();
                if (OTAttachmentList != null)
                {
                    foreach (OTAttachment item in OTAttachmentList)
                    {
                        item.TransactionOperation = ModelBase.TransactionOperations.Add;
                        ListAttachment.Add(item);
                        if (ListAddedAttachment == null)
                            ListAddedAttachment = new ObservableCollection<OTAttachment>();
                        ListAddedAttachment.Add(item);
                    }
                }
                OTAttachmentList = new List<object>();
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for to browse and add document into the list. 
        /// </summary>
        /// <param name="obj"></param>
        private void BrowseFile(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            IsBusy = true;
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".*";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    FileInfo file = new FileInfo(dlg.FileName);
                    FileName = file.FullName;
                    var newFileList = OTAttachmentList != null ? new List<object>(OTAttachmentList) : new List<object>();
                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Attachment = new OTAttachment();
                    Attachment.FileType = file.Extension;
                    Attachment.FilePath = file.FullName;
                    if (file.Name.Contains("."))
                    {
                        string[] a = file.Name.Split('.');
                        FileNameString = a[0];
                    }
                    else
                    {
                        FileNameString = file.Name;
                    }
                    Attachment.FileName = file.Name;
                    Attachment.OriginalFileName = FileNameString;
                    Attachment.SavedFileName = UniqueFileName + file.Extension;
                    Attachment.UploadedIn = GeosApplication.Instance.ServerDateTime;
                    Attachment.FileSizeInInt = file.Length;
                    Attachment.FileType = file.Extension;
                    Attachment.FileUploadName = file.Name;
                    Attachment.IsUploaded = true;
                    var theIcon = IconFromFilePath(FileName);
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\Images\";
                    if (theIcon != null)
                    {
                        // Save it to disk, or do whatever you want with it.
                        if (!Directory.Exists(tempPath))
                        {
                            System.IO.Directory.CreateDirectory(tempPath);
                        }
                        if (!File.Exists(tempPath + UniqueFileName + file.Extension + ".ico"))
                        {
                            using (var stream = new System.IO.FileStream(tempPath + UniqueFileName + file.Extension + ".ico", System.IO.FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                theIcon.Save(stream);
                                stream.Close();
                                stream.Dispose();
                            }
                        }
                        theIcon.Dispose();
                    }
                    // useful to get icon end process of temp. used imgage 
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(tempPath + UniqueFileName + file.Extension + ".ico", UriKind.RelativeOrAbsolute);
                    image.EndInit();
                    Attachment.AttachmentImage = image;
                    // not allow to add same files
                    List<OTAttachment> fooList = newFileList.OfType<OTAttachment>().ToList();
                    if (!fooList.Any(x => x.OriginalFileName == Attachment.OriginalFileName))
                    {
                        newFileList.Add(Attachment);
                    }
                    OTAttachmentList = newFileList;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public static Icon IconFromFilePath(string filePath)
        {
            var result = (Icon)null;
            try
            {
                result = Icon.ExtractAssociatedIcon(filePath);
            }
            catch (System.Exception)
            {
                // swallow and return nothing. You could supply a default Icon here as well
            }
            return result;
        }
        /// <summary>
        /// method to sort FileSize Column
        /// </summary>
        /// <param name="obj"></param>
        private void CustomSortColumnDataAction(object obj)
        {
            CustomColumnSortEventArgs e = obj as CustomColumnSortEventArgs;
            if (e.Column.FieldName == "FileSize")
            {
                string FirstFile = Convert.ToString(e.Value1);
                string SecondFile = Convert.ToString(e.Value2);
                int FirstFileSpaceIndex = FirstFile.IndexOf(" ");
                string FirstFileSizeType = FirstFile.Substring(FirstFileSpaceIndex + 1, 1);
                int SecondFileSpaceIndex = SecondFile.IndexOf(" ");
                string SecondFileSizeType = SecondFile.Substring(SecondFileSpaceIndex + 1, 1);
                if (FirstFileSizeType.Equals("K") && SecondFileSizeType.Equals("K"))
                {
                    int FirstFileSize = Convert.ToInt32(Regex.Match(FirstFile, @"^.*?(?=K)").Value);
                    int SecondFileSize = Convert.ToInt32(Regex.Match(SecondFile, @"^.*?(?=K)").Value);
                    if (FirstFileSize > SecondFileSize)
                    {
                        e.Result = 1;
                    }
                    else if (FirstFileSize < SecondFileSize)
                    {
                        e.Result = -1;
                    }
                }
                if (FirstFileSizeType.Equals("M") && SecondFileSizeType.Equals("M"))
                {
                    int FirstFileSize = Convert.ToInt32(Regex.Match(FirstFile, @"^.*?(?=M)").Value);
                    int SecondFileSize = Convert.ToInt32(Regex.Match(SecondFile, @"^.*?(?=M)").Value);
                    if (FirstFileSize > SecondFileSize)
                    {
                        e.Result = 1;
                    }
                    else if (FirstFileSize < SecondFileSize)
                    {
                        e.Result = -1;
                    }
                }
                if (FirstFileSizeType.Equals("B") && SecondFileSizeType.Equals("B"))
                {
                    int FirstFileSize = Convert.ToInt32(Regex.Match(FirstFile, @"^.*?(?=B)").Value);
                    int SecondFileSize = Convert.ToInt32(Regex.Match(SecondFile, @"^.*?(?=B)").Value);
                    if (FirstFileSize > SecondFileSize)
                    {
                        e.Result = 1;
                    }
                    else if (FirstFileSize < SecondFileSize)
                    {
                        e.Result = -1;
                    }
                }
                if ((FirstFileSizeType.Equals("B") && SecondFileSizeType.Equals("K")))
                {
                    e.Result = -1;
                }
                if (FirstFileSizeType.Equals("K") && SecondFileSizeType.Equals("B"))
                {
                    e.Result = 1;
                }
                if ((FirstFileSizeType.Equals("B") && SecondFileSizeType.Equals("M")))
                {
                    e.Result = -1;
                }
                if ((FirstFileSizeType.Equals("M") && SecondFileSizeType.Equals("B")))
                {
                    e.Result = 1;
                }
                if ((FirstFileSizeType.Equals("K") && SecondFileSizeType.Equals("M")))
                {
                    e.Result = -1;
                }
                if (FirstFileSizeType.Equals("M") && SecondFileSizeType.Equals("K"))
                {
                    e.Result = 1;
                }
                e.Handled = true;
            }
        }
        private void ImageClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                OtItem otItem = (OtItem)obj;
                OT.OtItems.Where(a => a.IdOTItem != otItem.IdOTItem && a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                OT.OtItems.Where(a => a.IdOTItem == otItem.IdOTItem).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ImageClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....executed ", category: Category.Info, priority: Priority.Low);
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
        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        //[nsatpute][03-09-2024][GEOS2-5415]
        private void CreatePrintValues(QcPassLabelDetails labeldetails)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PrintQcPassLabelCommandAction WorkOrderItemDetailsForQualityCertificationViewModel....", category: Category.Info, priority: Priority.Low);
                PrintValues = new Dictionary<string, string>();
                PrintValues.Add("@OFFER", labeldetails.Order);
                PrintValues.Add("@REFERENCE", labeldetails.Reference);
                PrintValues.Add("@PRODUCT", labeldetails.Product);
                PrintValues.Add("@CUSTOMER", labeldetails.CustomerName);
                PrintValues.Add("@AMPERAGE", labeldetails.Amperage);
                PrintValues.Add("@PRESSURE", labeldetails.Pressure);
                PrintValues.Add("@FREQUENCY", labeldetails.Frequency);
                PrintValues.Add("@VOLTAGE", labeldetails.Voltage);
                PrintValues.Add("@MADE_IN", labeldetails.SupplierCountry);
                PrintValues.Add("@PLANT_TAX_NUMBER", labeldetails.TaxNumber);
                PrintValues.Add("@PLANT_ALIAS", labeldetails.SiteShortName);
                PrintValues.Add("@PLANT_NAME", labeldetails.SupplierName);
                PrintValues.Add("@QRCODE", labeldetails.QRCodeData);
                PrintValues.Add("@DATE", $@"{DateTime.Now.Day.ToString().PadLeft(2, '0')}/{DateTime.Now.Month.ToString().PadLeft(2,'0')}/{DateTime.Now.Year.ToString()}");
                GeosApplication.Instance.Logger.Log("PrintQcPassLabelCommandAction WorkOrderItemDetailsForQualityCertificationViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method WorkOrderItemDetailsForQualityCertificationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // End Of Methods
        #region GEOS2-3682
        private void EditAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;
                EditItemView editItemView = new EditItemView();
                EditItemViewModel editItemViewModel = new EditItemViewModel();
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                EventHandler handle = delegate { editItemView.Close(); };
                editItemViewModel.RequestClose += handle;
                editItemViewModel.IsNew = false;
                editItemView.DataContext = editItemViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                DevExpress.Xpf.Grid.TreeListView tableView = (DevExpress.Xpf.Grid.TreeListView)obj;
                var ownerInfo = (tableView as FrameworkElement);
                OtItem tempOtItem = (OtItem)tableView.DataControl.CurrentItem;
                editItemView.Owner = Window.GetWindow(ownerInfo);
                editItemViewModel.EditInitItem(tempOtItem, OT, OtSite);
                editItemView.ShowDialog();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[nsatpute][04-09-2024][GEOS2-5415]
        private void ShowHidePrintLabel()
        {
            if (OT.IdWorkflowStatus == 14)
                IsPrintLabelVisible = Visibility.Visible;
            else
                IsPrintLabelVisible = Visibility.Hidden;
        }
        #endregion
    }
}