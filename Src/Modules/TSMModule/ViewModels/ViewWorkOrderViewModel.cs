using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.TSM;
using Emdep.Geos.Modules.TSM.Views;
using Emdep.Geos.Modules.TSM.ViewModels;
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
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Editors;
using System.Windows.Media.Imaging;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Modules.TSM.CommonClass;
namespace Emdep.Geos.Modules.TSM.ViewModels
{
    //[GEOS2-8965][pallavi.kale][28.11.2025]
    public class ViewWorkOrderViewModel : NavigationViewModelBase, INotifyPropertyChanged
    { 
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
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ITSMService TSMService = new TSMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ITSMService TSMService = new TSMServiceController("localhost:6699");

        #endregion

        #region Declaration
        private MaximizedElementPosition maximizedElementPosition;
        private double dialogHeight;
        private double dialogWidth;
        private bool isCancel = false;
        private bool isNew;
        private Company otSite;
        private Ots ot;
        private string remark;
        private OtItem selectedOtItem;
        private string infoTooltipBackColor;
        private TSMWorkflowStatus workflowStatus;
        private List<TSMWorkflowStatus> workflowStatusList;
        private List<TSMWorkflowStatus> workflowStatusButtons;
        private TSMWorkflowStatus selectedWorkflowStatusButton;
        private List<TSMWorkflowTransition> workflowTransitionList;
        private bool isBusy;
        private ObservableCollection<OTAssignedUser> otAddDeleteAssignedUser;
        private ObservableCollection<OTAssignedUser> otAssignedUser;
        List<OTAssignedUser> cloneOTAssignedUser = new List<OTAssignedUser>();
        private ObservableCollection<OTAssignedUser> userToAssingedUser;
        Ots objOT = new Ots();
        private bool isSaveChanges;
        #endregion

        #region Properties
        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
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
        public bool IsCancel
        {
            get { return isCancel; }
            set
            {
                isCancel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancel"));
            }
        }
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
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
        public Ots OT
        {
            get { return ot; }
            set
            {
                ot = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
            }
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
        public OtItem SelectedOtItem
        {
            get { return selectedOtItem; }
            set
            {
                selectedOtItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOtItem"));
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
        public TSMWorkflowStatus WorkflowStatus
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
        public List<TSMWorkflowStatus> WorkflowStatusList
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
        public List<TSMWorkflowStatus> WorkflowStatusButtons
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
        public TSMWorkflowStatus SelectedWorkflowStatusButton
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
        public List<TSMWorkflowTransition> WorkflowTransitionList
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
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
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
            set
            {
                userToAssingedUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserToAssingedUser")); 
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
        #endregion

        #region ICommand
        public ICommand CommandWorkOrderCancel { get; set; }
        public ICommand CommandWorkOrderAcceptCommand { get; set; }
        public ICommand PopupMenuShowingCommand { get; set; }
        public ICommand OpenWorkflowDiagramCommand { get; set; }
        public ICommand WorkflowButtonClickCommand { get; set; }
        public ICommand CurrentItemChangedCommand { get; set; }
        public ICommand ImageClickCommand { get; set; }
        public ICommand CustomSortColumnDataCommand { get; set; }
        public ICommand DownloadFileCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand UploadFileCommand { get; set; }
        public ICommand DeleteWorkLogCommand { get; set; }
        public ICommand AssignedWorkOrderItemCancelCommand { get; set; }
        public ICommand GetWorkOrderUserContactCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand AddCommentsCommand { get; set; }
        #endregion

        #region Constructor

        public ViewWorkOrderViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ViewWorkOrderViewModel()...", category: Category.Info, priority: Priority.Low);
                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 130;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 50;
                CommandWorkOrderAcceptCommand = new RelayCommand(new Action<object>(CommandWorkOrderAcceptAction));
                CommandWorkOrderCancel = new RelayCommand(new Action<object>(CommandWorkOrderCancelAction));
                PopupMenuShowingCommand = new DelegateCommand<OpenPopupEventArgs>(OpenPopupDateEditAction);
                OpenWorkflowDiagramCommand = new DelegateCommand<object>(OpenWorkflowDiagramCommandAction);
                WorkflowButtonClickCommand = new DelegateCommand<object>(WorkflowButtonClickCommandAction);
                CurrentItemChangedCommand = new DelegateCommand<object>(CurrentItemChangedCommandAction);
                //ImageClickCommand = new DelegateCommand<object>(ImageClickCommandAction);
                //CustomSortColumnDataCommand = new DelegateCommand<object>(CustomSortColumnDataAction);
                //DownloadFileCommand = new DelegateCommand<object>(DownloadFileCommandAction);
                //ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFile));
                //UploadFileCommand = new DelegateCommand<object>(UploadFileCommandAction);
                DeleteWorkLogCommand = new DelegateCommand<object>(DeletWorklogRowCommandAction);
                AssignedWorkOrderItemCancelCommand = new DelegateCommand<object>(AssignedUserWorkOrderItemCancelCommandAction);
                GetWorkOrderUserContactCommand = new DelegateCommand<object>(AssignedUserWorkOrderUserContactCheckedAction);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                AddCommentsCommand = new DelegateCommand<object>(AddCommentsCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor ViewWorkOrderViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewWorkOrderViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods
        private void CommandWorkOrderCancelAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandWorkOrderCancelAction()...", category: Category.Info, priority: Priority.Low);
                IsCancel = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CommandWorkOrderCancelAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandWorkOrderCancelAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CommandWorkOrderAcceptAction(object obj)
        {
            try
            {
                    GeosApplication.Instance.Logger.Log("Method CancelButton()...", category: Category.Info, priority: Priority.Low);
                
                    OtAddDeleteAssignedUser = new ObservableCollection<OTAssignedUser>();
                    foreach (OTAssignedUser otAssignedUser in OtAssignedUser)
                    {
                        if (!cloneOTAssignedUser.Any(x => x.IdOT == otAssignedUser.IdOT && x.IdUser == otAssignedUser.IdUser && x.IdStage == otAssignedUser.IdStage))
                        {
                            OTAssignedUser OTAssignedUser = (OTAssignedUser)otAssignedUser.Clone();
                            OTAssignedUser.TransactionOperation = ModelBase.TransactionOperations.Add;
                            OTAssignedUser.UserShortDetail.UserImage = null;
                            OtAddDeleteAssignedUser.Add(OTAssignedUser);
                        }
                    }
                    foreach (OTAssignedUser otAssignedUser in cloneOTAssignedUser)
                    {
                        if (!OtAssignedUser.Any(x => x.IdOT == otAssignedUser.IdOT && x.IdUser == otAssignedUser.IdUser && x.IdStage == otAssignedUser.IdStage))
                        {
                            OTAssignedUser OTAssignedUser = (OTAssignedUser)otAssignedUser.Clone();
                            OTAssignedUser.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            OTAssignedUser.UserShortDetail.UserImage = null;
                            OtAddDeleteAssignedUser.Add(OTAssignedUser);
                        }
                    }
                    if (!string.IsNullOrEmpty(Remark))
                        Remark = Remark.Trim();
                      TSMService.UpdateOTAssignedUser_V2690(objOT.Site, OtAddDeleteAssignedUser.ToList(), OT.IdOT, ot.Observations, Remark, GeosApplication.Instance.ActiveUser.IdUser);

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOrderEditViewEditedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                
               
                GeosApplication.Instance.Logger.Log("Method CommandWorkOrderAcceptAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandWorkOrderAcceptAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
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

        public void Init(Ots ot)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);

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
                MaximizedElementPosition = MaximizedElementPosition.Right;
                OtSite = ot.Site;
                //TSMService = new TSMServiceController("localhost:6699");
                OT = TSMService.GetWorkOrderByIdOt_V2690(ot.IdOT, ot.Site);
                OT.OfferCode = ot.OfferCode;
                OT.Modules = ot.Modules;
                OT.Site = OtSite;
                Remark = OT.Observations;
                SelectedOtItem = OT.OtItems.FirstOrDefault();
                //WorkLogItemList = new List<OTWorkingTime>();
                //UserImageList = new List<UserShortDetail>();
                //set info tooltip back color
                GeosAppSetting GeosAppSetting = WorkbenchStartUp.GetGeosAppSettings(37);
                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;
                FillListAttachment(ot.Site, ot.IdOT);
                objOT.IdOT = ot.IdOT;
                objOT.Site = ot.Site;
                //WorkLogItemList = SAMService.GetOTWorkingTimeDetails(ot.IdOT, ot.Site);
                //List<UserShortDetail> tempList = WorkLogItemList.Select(x => x.UserShortDetail).ToList();
                //UserImageList = tempList.GroupBy(x => x.IdUser).Select(x => x.First()).ToList();
                //TimeSpan worklogTotalTime = new TimeSpan(WorkLogItemList.Sum(r => r.TotalTime.Ticks));
                ////[001] changed the date formate
                //int Hours = worklogTotalTime.Days * 24 + worklogTotalTime.Hours;
                //WorklogTotalTime = string.Format("{0}H {1}M", Hours, worklogTotalTime.Minutes);
                ////set info tooltip back color
                //GeosAppSetting GeosAppSetting = WorkbenchService.GetGeosAppSettings(37);
                //if (GeosAppSetting != null)
                //    InfoTooltipBackColor = GeosAppSetting.DefaultValue;
      
                //FillListAttachment(ot.Site, ot.IdOT);
                //objOT.IdOT = ot.IdOT;
                //objOT.Site = ot.Site;
                //// [002] Add code for task GEOS2-2902
                //WorkflowStatusList = new List<WorkflowStatus>(SAMService.GetAllWorkflowStatus());
                //WorkflowTransitionList = new List<WorkflowTransition>(SAMService.GetAllWorkflowTransitions());
                //List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == OT.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                //WorkflowStatusButtons = new List<WorkflowStatus>();
                //foreach (byte statusbutton in GetCurrentButtons)
                //{
                //    WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                //}
                FillAssignedUsers(ot.Site, ot.IdSite);
                FillAssignedUsers(ot.Site, ot.Site.IdCompany);
                FillOtAssignUsersList(ot.Site, ot.IdOT);
                objOT.IdOT = ot.IdOT;
                objOT.Site = ot.Site;
                // [002] Add code for task GEOS2-2902
               // WorkflowStatusList = new List<WorkflowStatus>(TSMService.GetAllWorkflowStatus());
               // WorkflowTransitionList = new List<WorkflowTransition>(SAMService.GetAllWorkflowTransitions());
                //List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == OT.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                //WorkflowStatusButtons = new List<WorkflowStatus>();
                //foreach (byte statusbutton in GetCurrentButtons)
                //{
                //    WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                //}
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OpenPopupDateEditAction(OpenPopupEventArgs obj)
        {
            obj.Cancel = true;
            obj.Handled = true;
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

        private void TransitionWorkflowStatus(int currentStatus)
        {
            List<TSMLogEntriesByOT> LogEntriesByOTList = new List<TSMLogEntriesByOT>();
            TSMWorkflowTransition workflowTransition = new TSMWorkflowTransition();
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
                    TSMLogEntriesByOT CommentByOT = new TSMLogEntriesByOT();
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
                    if (OT.TSMOTComments == null)
                        OT.TSMOTComments = new List<TSMLogEntriesByOT>();
                    OT.TSMOTComments.Insert(0, CommentByOT);
                    OT.TSMOTComments = new List<TSMLogEntriesByOT>(ot.TSMOTComments);
                    TSMLogEntriesByOT logEntriesByOT_1 = new TSMLogEntriesByOT();
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
                    if (OT.TSMOTLogEntries == null)
                        OT.TSMOTLogEntries = new List<TSMLogEntriesByOT>();
                    OT.TSMOTLogEntries.Insert(0, logEntriesByOT_1);
                    OT.TSMOTLogEntries = new List<TSMLogEntriesByOT>(ot.TSMOTLogEntries);
                }
                else
                {
                    workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == OT.IdWorkflowStatus);
                    WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == OT.IdWorkflowStatus);
                    OT.TSMWorkflowStatus = WorkflowStatus;
                    List<byte> GetCurrentButtons1 = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == OT.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                    WorkflowStatusButtons = new List<TSMWorkflowStatus>();
                    foreach (byte statusbutton in GetCurrentButtons1)
                    {
                        WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                    }
                    WorkflowStatusButtons = new List<TSMWorkflowStatus>(WorkflowStatusButtons);
                    return;
                }
            }
            TSMLogEntriesByOT logEntriesByOT = new TSMLogEntriesByOT();
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
            if (OT.TSMOTLogEntries == null)
                OT.TSMOTLogEntries = new List<TSMLogEntriesByOT>();
            OT.TSMOTLogEntries.Insert(0, logEntriesByOT);
            OT.TSMOTLogEntries = new List<TSMLogEntriesByOT>(OT.TSMOTLogEntries);
            //TSMService = new TSMServiceController("localhost:6699");
            IsSaveChanges = TSMService.UpdateWorkflowStatusInOT_V2690((UInt64)OT.IdOT, (uint)currentStatus, GeosApplication.Instance.ActiveUser.IdUser, LogEntriesByOTList);
            OT.IdWorkflowStatus = WorkflowStatus.IdWorkflowStatus;
            OT.TSMWorkflowStatus = WorkflowStatus;
            List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == currentStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
            WorkflowStatusButtons = new List<TSMWorkflowStatus>();
            foreach (byte statusbutton in GetCurrentButtons)
            {
                WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
            }
            WorkflowStatusButtons = new List<TSMWorkflowStatus>(WorkflowStatusButtons);

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
        private void CurrentItemChangedCommandAction(object obj)
        {
            try
            {
                if (SelectedOtItem != null &&
                    SelectedOtItem.RevisionItem != null &&
                    SelectedOtItem.RevisionItem.WarehouseProduct != null
                    && SelectedOtItem.RevisionItem.WarehouseProduct.Article != null)
                {
                    //if (SAMCommon.Instance.Articles.Any(x => x.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle))
                    //{
                    //    Article article = SAMCommon.Instance.Articles.FirstOrDefault(x => x.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                    //    SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = article.ArticleImage;
                    //}
                    //else
                    //{
                    //    Article article = SAMService.GetArticleDetails(SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                    //    if (article.ArticleImageInBytes == null)
                    //    {
                    //        article.ArticleImage = new BitmapImage(new Uri("/Emdep.Geos.Modules.SAM;component/Assets/Images/EmptyImage.png", UriKind.Relative));
                    //    }
                    //    else if (article.ArticleImageInBytes != null)
                    //    {
                    //        article.ArticleImage = SAMCommon.Instance.ByteArrayToBitmapImage(article.ArticleImageInBytes);
                    //    }
                    //    SAMCommon.Instance.Articles.Add(article);
                    //    SelectedOtItem.RevisionItem.WarehouseProduct.Article.ArticleImage = article.ArticleImage;
                    //}
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CurrentItemChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                //if (MessageBoxResult == MessageBoxResult.Yes)
                //{
                //    OTAttachment attachmentObject = (OTAttachment)obj;
                //    SaveFileDialogService.DefaultExt = attachmentObject.FileExtension;
                //    SaveFileDialogService.DefaultFileName = attachmentObject.FileName;
                //    SaveFileDialogService.Filter = "All Files|*.*";
                //    SaveFileDialogService.FilterIndex = 1;
                //    DialogResult = SaveFileDialogService.ShowDialog();
                //    if (!DialogResult)
                //    {
                //        ResultFileName = string.Empty;
                //    }
                //    else
                //    {
                //        IsBusy = true;
                //        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                //                //WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                //                win.Topmost = false;
                //                return win;
                //            }, x =>
                //            {
                //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                //            }, null, null);
                //        }
                //        //[pramod.misal][GEOS2-5327][05.03.2024]
                //        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                //        SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //        Byte[] byteObj = SAMService.GetOTAttachmentInBytes(attachmentObject.FileName, attachmentObject.QuotationYear, attachmentObject.QuotationCode);
                //        //[pramod.misal][GEOS2-5327][05.03.2024]
                //        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                //        ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                //        isDownload = SaveData(ResultFileName, byteObj);
                //    }
                //    if (isDownload)
                //    {
                //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                //        GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                //        IsBusy = false;
                //    }
                //    else
                //    {
                //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //        IsBusy = false;
                //    }
                //}
                //else
                //{
                //    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                //    IsBusy = false;
                //}
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
        public void UploadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                List<FileInfo> FileDetail = new List<FileInfo>();
                //if (OTAttachmentList != null)
                //{
                //    foreach (OTAttachment item in OTAttachmentList)
                //    {
                //        item.TransactionOperation = ModelBase.TransactionOperations.Add;
                //        ListAttachment.Add(item);
                //        if (ListAddedAttachment == null)
                //            ListAddedAttachment = new ObservableCollection<OTAttachment>();
                //        ListAddedAttachment.Add(item);
                //    }
                //}
                //OTAttachmentList = new List<object>();
                //IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //public void UploadFileCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
        //        DXSplashScreen.Show<SplashScreenView>();
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        List<FileInfo> FileDetail = new List<FileInfo>();
        //        //if (OTAttachmentList != null)
        //        //{
        //        //    foreach (OTAttachment item in OTAttachmentList)
        //        //    {
        //        //        item.TransactionOperation = ModelBase.TransactionOperations.Add;
        //        //        ListAttachment.Add(item);
        //        //        if (ListAddedAttachment == null)
        //        //            ListAddedAttachment = new ObservableCollection<OTAttachment>();
        //        //        ListAddedAttachment.Add(item);
        //        //    }
        //        //}
        //        //OTAttachmentList = new List<object>();
        //        //IsBusy = false;
        //        GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        IsBusy = false;
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
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
                    //    if (WorkLogItemList != null && WorkLogItemList.Count > 0)
                    //    {
                    //        OTWorkingTime OTworktime = (OTWorkingTime)obj;
                    //        result = SAMService.DeleteWorkLog(OtSite, OTworktime.IdOTWorkingTime);
                    //        WorkLogItemList.Remove(OTworktime);
                    //        WorkLogItemList = new List<OTWorkingTime>(WorkLogItemList);
                    //    }
                    //    if (result)
                    //    {
                    //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkLogDeleteSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    //        GeosApplication.Instance.Logger.Log("Method DeletWorklogRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                    //    }
                    //    else
                    //    {
                    //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkLogDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //    }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeletWorklogRowCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AssignedUserWorkOrderItemCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AssignedUserWorkOrderItemCancelCommandAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (obj is OTAssignedUser)
                {
                    //OTAssignedUser = obj as OTAssignedUser;  // (OTAssignedUser)obj;
                    //OtAssignedUser.Remove(OTAssignedUser);
                    //UserToAssingedUser.Add(OTAssignedUser);
                    //UserToAssingedUser.OrderBy(i => i.UserShortDetail.UserName);
                }
                GeosApplication.Instance.Logger.Log("Method AssignedUserWorkOrderItemCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AssignedUserWorkOrderItemCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void AssignedUserWorkOrderUserContactCheckedAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AssignedUserWorkOrderUserContactCheckedAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                //if (obj is OTAssignedUser)
                //{
                //    OTAssignedUser = obj as OTAssignedUser;  // (OTAssignedUser)obj;
                //    OTAssignedUser.IdOT = objOT.IdOT;
                //    OTAssignedUser.IdStage = 4;
                //    OTAssignedUser.IdUser = OTAssignedUser.UserShortDetail.IdUser;
                //    try
                //    {
                //        byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(OTAssignedUser.UserShortDetail.Login);
                //        if (UserProfileImageByte == null)
                //        {
                //            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                //            {
                //                if (OTAssignedUser.UserShortDetail.IdUserGender == 1)
                //                    OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wFemaleUser.png");
                //                else if (OTAssignedUser.UserShortDetail.IdUserGender == 2)
                //                    OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/wMaleUser.png");
                //            }
                //            else
                //            {
                //                if (OTAssignedUser.UserShortDetail.IdUserGender == 1)
                //                    OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bFemaleUser.png");
                //                else if (OTAssignedUser.UserShortDetail.IdUserGender == 2)
                //                    OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.GetImage("/Emdep.Geos.Modules.SAM;component/Assets/Images/bMaleUser.png");
                //            }
                //        }
                //        else
                //        {
                //            OTAssignedUser.UserShortDetail.UserImage = SAMCommon.Instance.ByteArrayToBitmapImage(UserProfileImageByte);
                //        }
                //    }
                //    catch (FaultException<ServiceException> ex)
                //    {
                //        GeosApplication.Instance.Logger.Log("Get an error in AssignedUserWorkOrderUserContactCheckedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                //    }
                //    catch (ServiceUnexceptedException ex)
                //    {
                //        GeosApplication.Instance.Logger.Log("Get an error in AssignedUserWorkOrderUserContactCheckedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                //    }
                //    OtAssignedUser.Add(OTAssignedUser);
                //    UserToAssingedUser.Remove(OTAssignedUser);
                //    GeosApplication.Instance.Logger.Log("Method AssignedUserWorkOrderUserContactCheckedAction() executed successfully", category: Category.Info, priority: Priority.Low);
                //}
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AssignedUserWorkOrderUserContactCheckedAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        public void DeleteCommentCommandAction(object parameter)
        {

            GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

            TSMLogEntries commentObject = (TSMLogEntries)parameter;


            bool result = false;
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 47))
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                //if (MessageBoxResult == MessageBoxResult.Yes)
                //{
                //    if (CommentsList != null && CommentsList.Count > 0)
                //    {
                //        SAMLogEntries Comment = (SAMLogEntries)commentObject;
                //        //result = SAMService.DeleteComment_V2340(Comment.IdComment,Site);
                //        CommentsList.Remove(Comment);

                //        if (DeleteCommentsList == null)
                //            DeleteCommentsList = new ObservableCollection<SAMLogEntries>();

                //        DeleteCommentsList.Add(Comment);
                //        CommentsList = new ObservableCollection<SAMLogEntries>(CommentsList);
                //        SamComments = Comment;
                //        IsDeleted = true;
                //    }


                //}
            }



            //NewItemComment = null;

            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void AddCommentsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);

                //AddCommentsView addCommentsView = new AddCommentsView();
                //AddCommentsViewModel addCommentsViewModel = new AddCommentsViewModel();
                //EventHandler handle = delegate { addCommentsView.Close(); };
                //addCommentsViewModel.RequestClose += handle;
                //addCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCommentsHeader").ToString();
                ////addCommentsViewModel.IsNew = true;
                ////addCommentsViewModel.Init();

                //addCommentsView.DataContext = addCommentsViewModel;
                //addCommentsView.ShowDialog();
                //if (addCommentsViewModel.SelectedComment != null)
                //{
                //    if (CommentsList == null)
                //        CommentsList = new ObservableCollection<SAMLogEntries>();

                //    CommentsList.Add(addCommentsViewModel.SelectedComment as SAMLogEntries);
                //    SelectedComment = addCommentsViewModel.SelectedComment;
                //}

                GeosApplication.Instance.Logger.Log("Method AddFile()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
               
        private void FillListAttachment(Company company, long idOT)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillListAttachment ...", category: Category.Info, priority: Priority.Low);
                //[pramod.misal][GEOS2-5327][05.03.2024]
                //SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == company.IdCompany);
                //SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //ListAttachment = new ObservableCollection<OTAttachment>(SAMService.GetOTAttachment(company, idOT).ToList());
                ////[pramod.misal][GEOS2-5327][05.03.2024]
                //SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == company.IdCompany);
                //foreach (OTAttachment items in ListAttachment)
                //{
                //    ImageSource imageObj = FileExtensionToFileIcon.FindIconForFilename(items.FileName, true);
                //    items.AttachmentImage = imageObj;
                //}
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
        private void FillAssignedUsers(Company company, Int32 idSite)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAssignedUsers ...", category: Category.Info, priority: Priority.Low);
                //TSMService = new TSMServiceController("localhost:6699");
                UserToAssingedUser = new ObservableCollection<OTAssignedUser>(TSMService.GetUsersToAssignedOT_V2690(company, idSite));

                GeosApplication.Instance.Logger.Log("Method FillAssignedUsers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAssignedUsers() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAssignedUsers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }
        private void FillOtAssignUsersList(Company company, long idOT)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOtAssignUsersList ...", category: Category.Info, priority: Priority.Low);
               // TSMService = new TSMServiceController("localhost:6699");
                OtAssignedUser = new ObservableCollection<OTAssignedUser>(TSMService.GetOTAssignedUsers_V2690(company, idOT).ToList());
                if (OtAssignedUser.Count > 0)
                {
                    foreach (OTAssignedUser item in OtAssignedUser)
                    {
                        OTAssignedUser OTAssignedUser = UserToAssingedUser.FirstOrDefault(x => x.UserShortDetail.IdUser == item.IdUser);
                        UserToAssingedUser.Remove(OTAssignedUser);
                    }
                }
                if (OtAssignedUser != null)
                    cloneOTAssignedUser = OtAssignedUser.Select(x => (OTAssignedUser)x.Clone()).ToList();
                for (int i = 0; i < OtAssignedUser.Count; i++)
                {
                    try
                    {
                        byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(OtAssignedUser[i].UserShortDetail.Login);
                        if (UserProfileImageByte != null)
                        {
                            OtAssignedUser[i].UserShortDetail.UserImage = TSMCommon.Instance.ByteArrayToBitmapImage(UserProfileImageByte);
                        }
                        else
                        {
                            
                                if (OtAssignedUser[i].UserShortDetail.IdUserGender == 1)
                                    OtAssignedUser[i].UserShortDetail.UserImage = TSMCommon.Instance.GetImage("/Emdep.Geos.Modules.TSM;component/Assets/Images/FemaleUser_Blue.png");
                                else if (OtAssignedUser[i].UserShortDetail.IdUserGender == 2)
                                    OtAssignedUser[i].UserShortDetail.UserImage = TSMCommon.Instance.GetImage("/Emdep.Geos.Modules.TSM;component/Assets/Images/MaleUser_Blue.png");
                            
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
        #endregion
    }
}
