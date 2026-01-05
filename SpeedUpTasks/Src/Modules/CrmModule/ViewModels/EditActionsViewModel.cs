using DevExpress.DataProcessing;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Crm.Views;
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
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.Validations;
using System.Windows.Documents;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    class EditActionsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        #region Service
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Decleration

        //private ActionPlanItem actionItemData;
        //private IList<ActionPlanItem> selectedActionPlanList;
        //private string salesOwnersIds = "";
        //private IList<ActionPlanItem> selectedCompanyList;
        //private string customerPlantName;
        //private bool inIt = false;
        //private bool showCommentsFlyout;
        private string rowHeight = "35";
        private string subRowHeight = "35";
        private Int32 subRowIndex = 4;

        private Visibility isScopeVisible;
        private Visibility isReporterVisible;
        private Visibility isResponsibleVisible;
        private Visibility isDueDateVisible;
        private Visibility isStatusVisible;
        private LookupValue selectedType;
        private int selectedIndexSalesActivityType;
        private ObservableCollection<Customer> companyGroupList;
        private int selectedIndexCompanyGroup;
        private ObservableCollection<Company> entireCompanyPlantList;
        private ObservableCollection<Company> companyPlantList;
        private int selectedIndexCompanyPlant;
        private Company selectedPlant;
        private ObservableCollection<People> responsibleList;
        private int selectedResponsibleListIndex;
        private int selectedScopeListIndex;
        private ActionPlanItem _actionPlanItem;
        private int selectedReporter;
        private int selectedStatusListIndex;
        private string subject;
        private string description;
        byte[] UserProfileImageByte = null;
        private ImageSource userProfileImage;
        private DateTime dueDate;
        private ObservableCollection<LogEntriesByActionItem> actionPlanItemCommentsList;
        private string commentButtonText;
        private bool isAdd;
        private bool isDescriptionAdd;
        private string oldActionComment;
        private int commentboxHeight;
        private string newActionComment;
        private bool isRtf;
        private bool isNormal;
        private Visibility textboxnormal;
        private Visibility richtextboxrtf;
        private ObservableCollection<LogEntriesByActionItem> actionPlanItemChangeLogList;
        private LogEntriesByActionItem selectedComment;
        private DateTime? modifiedInDate;
        private DateTime createdInDate;
        private string actionCreatedDays;
        private string actionCreatedDaysStr;
        private int actionGenerateDays;
        private bool isBusy;
        private string editInformationError;
        private List<LogEntriesByActionItem> comments;
        

        //private ObservableCollection<People> linkeditemsPeopleList;
        #endregion

        #region Icommand
        public ICommand EditActionsViewCancelButtonCommand { get; set; }
        public ICommand CommandEditTypeValueChanged { get; set; }
        public ICommand CommandEditValueChanged { get; private set; }
        public ICommand CommandOpenAddOrEditActionCommentClick { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand EditActionsViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Properties

        public Visibility IsScopeVisible
        {
            get { return isScopeVisible; }
            set
            {
                isScopeVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsScopeVisible"));
            }
        }
        public String RowHeight
        {
            get
            {
                return rowHeight;
            }

            set
            {
                rowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RowHeight"));
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
        public String SubRowHeight
        {
            get
            {
                return subRowHeight;
            }

            set
            {
                subRowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SubRowHeight"));
            }
        }
        public Int32 SubRowIndex
        {
            get
            {
                return subRowIndex;
            }

            set
            {
                subRowIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SubRowIndex"));
            }
        }
        public Visibility IsReporterVisible
        {
            get { return isReporterVisible; }
            set
            {
                isReporterVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReporterVisible"));
            }
        }

        public Visibility IsResponsibleVisible
        {
            get { return isResponsibleVisible; }
            set
            {
                isResponsibleVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsResponsibleVisible"));
            }
        }

        public Visibility IsDueDateVisible
        {
            get { return isDueDateVisible; }
            set
            {
                isDueDateVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDueDateVisible"));
            }
        }

        public Visibility IsStatusVisible
        {
            get { return isStatusVisible; }
            set
            {
                isStatusVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStatusVisible"));
            }
        }
        public int SelectedIndexSalesActivityType
        {
            get { return selectedIndexSalesActivityType; }
            set { selectedIndexSalesActivityType = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexSalesActivityType")); }
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

        public ObservableCollection<Customer> CompanyGroupList
        {
            get { return companyGroupList; }
            set
            {
                companyGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroupList"));
            }
        }

        public int SelectedIndexCompanyGroup
        {
            get { return selectedIndexCompanyGroup; }
            set
            {
                if (value != SelectedIndexCompanyGroup)
                {
                    selectedIndexCompanyGroup = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));

                    if (SelectedIndexCompanyGroup > 0)
                    {
                        if (EntireCompanyPlantList != null)
                        {
                            CompanyPlantList = new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList());
                            SelectedIndexCompanyPlant = 0;
                        }
                    }
                    else
                    {
                        CompanyPlantList = null;
                        SelectedIndexCompanyPlant = -1;
                    }
                }
            }
        }


        public ObservableCollection<Company> CompanyPlantList
        {
            get { return companyPlantList; }
            set
            {
                companyPlantList = value;
                //if (value == null || companyPlantList?.Count <= 0)
                //    LinkeditemsPeopleList = new ObservableCollection<People>();
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyPlantList"));
            }
        }

        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }

        public int SelectedIndexCompanyPlant
        {
            get { return selectedIndexCompanyPlant; }
            set { selectedIndexCompanyPlant = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyPlant")); }
        }

        public Company SelectedPlant
        {
            get { return selectedPlant; }
            set
            {
                selectedPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }

        public int SelectedResponsibleListIndex
        {
            get { return selectedResponsibleListIndex; }
            set
            {
                selectedResponsibleListIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedResponsibleListIndex"));
            }
        }

        public int SelectedScopeListIndex
        {
            get { return selectedScopeListIndex; }
            set
            {
                selectedScopeListIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedScopeListIndex"));
            }
        }

        public ActionPlanItem _ActionPlanItem
        {
            get
            {
                return _actionPlanItem;
            }
            set
            {
                _actionPlanItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("_ActionPlanItem"));
            }
        }

        public ActionPlanItem ObjActionPlanItem
        {
            get
            {
                return objActionPlanItem;
            }
            set
            {
                objActionPlanItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjActionPlanItem"));
            }
        }

        public int SelectedReporter
        {
            get
            {
                return selectedReporter;
            }

            set
            {
                selectedReporter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReporter"));
            }
        }

        public List<LogEntriesByActionItem> ChangedLogsEntries { get; set; }
        public List<LookupValue> SalesActivityTypeList { get; set; }
        public int SelectedStatusListIndex
        {
            get { return selectedStatusListIndex; }
            set
            {
                selectedStatusListIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatusListIndex"));
            }
        }

        public string Subject
        {
            get { return subject; }
            set
            {
                if (value != null)
                {
                    subject = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("Subject"));
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value != null)
                {
                    description = value.TrimStart();

                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                }
            }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DueDate"));
            }
        }

        public DateTime? ModifiedInDate
        {
            get { return modifiedInDate; }
            set
            {
                modifiedInDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifiedInDate"));
            }
        }

        public DateTime CreatedInDate
        {
            get { return createdInDate; }
            set
            {
                createdInDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatedInDate"));
            }
        }

        public String ActionCreatedDays
        {
            get { return actionCreatedDays; }
            set
            {
                actionCreatedDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionCreatedDays"));
            }
        }

        public int ActionGenerateDays
        {
            get { return actionGenerateDays; }
            set
            {
                actionGenerateDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionGenerateDays"));
            }
        }

        public String ActionCreatedDaysStr
        {
            get { return actionCreatedDaysStr; }
            set
            {
                actionCreatedDaysStr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionCreatedDaysStr"));
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

        //Comments List
        public ObservableCollection<LogEntriesByActionItem> ActionPlanItemCommentsList
        {
            get { return actionPlanItemCommentsList; }
            set
            {
                actionPlanItemCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanItemCommentsList"));
            }
        }

        //public List<LookupValue> ActionPlanItemCommentsList { get; set; }

        // Change Log List
        //Change log
        public ObservableCollection<LogEntriesByActionItem> ActionPlanItemChangeLogList
        {
            get { return actionPlanItemChangeLogList; }
            set
            {
                actionPlanItemChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanItemChangeLogList"));

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

        public bool IsAdd
        {
            get
            {
                return isAdd;
            }

            set
            {
                isAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdd"));
            }
        }

        //public bool ShowCommentsFlyout
        //{
        //    get { return showCommentsFlyout; }
        //    set
        //    {
        //        showCommentsFlyout = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ShowCommentsFlyout"));
        //    }
        //}

        public string OldActionComment
        {
            get { return oldActionComment; }
            set
            {
                oldActionComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldActionComment"));
            }
        }


        public bool IsDescriptionAdd
        {
            get { return isDescriptionAdd; }
            set
            {
                isDescriptionAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDescriptionAdd"));
            }
        }

        public string NewActionComment
        {
            get { return newActionComment; }
            set
            {
                newActionComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewActionComment"));
            }
        }

        public int CommentboxHeight
        {
            get
            {
                return commentboxHeight;
            }

            set
            {
                commentboxHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentboxHeight"));
            }
        }

        public bool IsRtf
        {
            get { return isRtf; }
            set
            {
                isRtf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRtf"));
                if (isRtf == true)
                {

                    Textboxnormal = Visibility.Collapsed;
                    Richtextboxrtf = Visibility.Visible;
                }
                else
                {
                    Textboxnormal = Visibility.Visible;
                    Richtextboxrtf = Visibility.Collapsed;
                }
            }
        }

        public Visibility Textboxnormal
        {
            get
            {
                return textboxnormal;
            }

            set
            {
                textboxnormal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Textboxnormal"));
            }
        }
        public Visibility Richtextboxrtf
        {
            get
            {
                return richtextboxrtf;
            }

            set
            {
                richtextboxrtf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("richtextboxrtf"));
            }
        }

        public bool IsNormal
        {
            get { return isNormal; }
            set
            {
                isNormal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNormal"));
            }
        }

        public LogEntriesByActionItem SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }

        public string EditInformationError
        {
            get { return editInformationError; }
            set { editInformationError = value; OnPropertyChanged(new PropertyChangedEventArgs("EditInformationError")); }
        }

        public List<LookupValue> ScopeList { get; set; }
        public List<LookupValue> StatusList { get; set; }
        public ObservableCollection<User> ReporterList { get; private set; }

        public LookupValue SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                selectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
            }
        }
        public ObservableCollection<People> ResponsibleList
        {
            get { return responsibleList; }

            set
            {
                responsibleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResponsibleList"));

            }
        }
        
        //public ObservableCollection<People> ResponsibleList { get; private set; }

        //public ObservableCollection<People> LinkeditemsPeopleList
        //{
        //    get { return linkeditemsPeopleList; }
        //    set
        //    {
        //        linkeditemsPeopleList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("LinkeditemsPeopleList"));
        //    }
        //}
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
        #endregion

        #region Constructor
        public EditActionsViewModel()
        {
            try
            {
                EditActionsViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                CommandEditValueChanged = new DelegateCommand<object>(EditValueChangedCommandAction);
                EditActionsViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(EditActionViewAcceptAction);
                CommandEditTypeValueChanged = new DelegateCommand<object>(EditTypeValueChangedCommandAction);
                // Commands for comments 
                CommandOpenAddOrEditActionCommentClick = new DelegateCommand<object>(AddOrEditActionCommentViewWindowShow);
                CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion



        #region Methods

        private void EditActionViewAcceptAction(object obj)
        {
            try
            {
                
                GeosApplication.Instance.Logger.Log("Method EditActionViewAcceptAction ...", category: Category.Info, priority: Priority.Low);
                if (SelectedType.IdLookupValue == 272)   // Information
                {
                    SelectedScopeListIndex = ScopeList.FindIndex(x => x.IdLookupValue == 236);              //227
                    SelectedReporter = ReporterList.IndexOf(ReporterList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser));
                    SelectedResponsibleListIndex = ResponsibleList.IndexOf(ResponsibleList.FirstOrDefault(x => x.IdPerson == GeosApplication.Instance.ActiveUser.IdUser));
                    SelectedStatusListIndex = StatusList.IndexOf(StatusList.FirstOrDefault(x => x.IdLookupValue == 266));       //254
                    DueDate = GeosApplication.Instance.ServerDateTime;
                }
                IsBusy = true;
                EditInformationError = null;

                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    EditInformationError = null;

                else
                    EditInformationError = "";

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexSalesActivityType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedScopeListIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("Subject"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedReporter"));
                PropertyChanged(this, new PropertyChangedEventArgs("DueDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                // PropertyChanged(this, new PropertyChangedEventArgs("Action"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedResponsibleListIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("EditInformationError"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatusListIndex"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    AddChangedLogsDetails();


                    ObjActionPlanItem = new ActionPlanItem();
                    ObjActionPlanItem.TransactionOperation = ModelBase.TransactionOperations.Update;
                    ObjActionPlanItem.IdActionPlanItem = _ActionPlanItem.IdActionPlanItem;
                    ObjActionPlanItem.IdActionPlan = 1;
                    ObjActionPlanItem.Number = _ActionPlanItem.Number;
                    ObjActionPlanItem.IdGroup = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;
                    ObjActionPlanItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                    ObjActionPlanItem.IdScope = ScopeList[SelectedScopeListIndex].IdLookupValue;
                    ObjActionPlanItem.IdStatus = StatusList[SelectedStatusListIndex].IdLookupValue;

                    if (SelectedType.IdLookupValue == 272)   // Information
                    {
                        ObjActionPlanItem.CloseDate = GeosApplication.Instance.ServerDateTime;
                    }
                    else
                    {
                        if (_ActionPlanItem.IdStatus == 266 && _ActionPlanItem.IdStatus != ObjActionPlanItem.IdStatus)
                        {
                            ObjActionPlanItem.CloseDate = null;
                        }
                        else if (ObjActionPlanItem.IdStatus == 266 && _ActionPlanItem.IdStatus != ObjActionPlanItem.IdStatus)
                        {
                            ObjActionPlanItem.CloseDate = GeosApplication.Instance.ServerDateTime;
                        }
                        else
                        {
                            ObjActionPlanItem.CloseDate = _ActionPlanItem.CloseDate;
                        }
                    }
                    ObjActionPlanItem.IdReporter = ReporterList[SelectedReporter].IdUser;
                    ObjActionPlanItem.IdAssignee = ResponsibleList[SelectedResponsibleListIndex].IdPerson;
                    ObjActionPlanItem.IdModifier = GeosApplication.Instance.ActiveUser.IdUser;
                    ObjActionPlanItem.Group = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName;
                    ObjActionPlanItem.Plant = CompanyPlantList[SelectedIndexCompanyPlant].SiteNameWithoutCountry;
                    ObjActionPlanItem.Title = Subject;
                    ObjActionPlanItem.IdSalesActivityType = SalesActivityTypeList[SelectedIndexSalesActivityType].IdLookupValue;
                    //objActionPlanItem.SalesActivityType = SelectedType;
                    //ObjActionPlanItem.SalesActivityType.Value = SalesActivityTypeList[SelectedIndexSalesActivityType].Value;
                    ObjActionPlanItem.Scope = ScopeList[SelectedScopeListIndex].Value;
                    ObjActionPlanItem.Reporter = ReporterList[SelectedReporter].FullName;
                    ObjActionPlanItem.Assignee = ResponsibleList[SelectedResponsibleListIndex].FullName;
                    ObjActionPlanItem.Status = StatusList[SelectedStatusListIndex];
                    ObjActionPlanItem.CurrentDueDate = DueDate;

                    if (_ActionPlanItem.CurrentDueDate.Date != ObjActionPlanItem.CurrentDueDate.Date)
                    {
                        ObjActionPlanItem.ExpectedDueDate = _ActionPlanItem.CurrentDueDate.Date;
                        ObjActionPlanItem.DueDateChangeCount = _ActionPlanItem.DueDateChangeCount + 1;
                    }
                    if (!String.IsNullOrEmpty(Description))
                    {
                        if (IsDescriptionAdd)
                        {

                            LogEntriesByActionItem comment = new LogEntriesByActionItem()
                            {

                                Creator = GeosApplication.Instance.ActiveUser.FirstName + ' ' + GeosApplication.Instance.ActiveUser.LastName,
                                IdCreator = GeosApplication.Instance.ActiveUser.IdUser,
                                CreationDate = GeosApplication.Instance.ServerDateTime,
                                Comment = Description,
                                IdLogEntryType = 257,
                                TransactionOperation = ModelBase.TransactionOperations.Add,
                                IsEnabled = false,
                                IsRtfText = false
                            };

                            comment.PeopleCreator = new People();
                            comment.PeopleCreator.OwnerImage = SetUserProfileImage();

                            if (comment != null)
                            {
                                ActionPlanItemCommentsList.Add(comment);
                            }

                            SelectedComment = comment;

                        }
                        else
                        {
                            if (ActionPlanItemCommentsList != null && ActionPlanItemCommentsList.Count() > 0)
                            {
                                if (ActionPlanItemCommentsList.Any(i => i.IdActionPlanItem != 0))
                                {
                                    LogEntriesByActionItem logEntriesByActionItem = ActionPlanItemCommentsList.Where(i => i.IdActionPlanItem != 0).OrderBy(i => i.IdLogEntryByActionItem).FirstOrDefault();
                                    logEntriesByActionItem.Comment = Description;
                                    logEntriesByActionItem.IsEnabled = false;
                                    logEntriesByActionItem.IsRtfText = false;
                                    logEntriesByActionItem.TransactionOperation = ModelBase.TransactionOperations.Update;
                                }
                            }
                        }

                    }
                    ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>(ActionPlanItemCommentsList.OrderByDescending(c => c.CreationDate).ToList());
                    ObjActionPlanItem.LogEntriesByActionItems = new List<LogEntriesByActionItem>();

                    List<LogEntriesByActionItem> AddedComments = ActionPlanItemCommentsList.Where(x => x.IdActionPlanItem == 0).ToList();

                    if (AddedComments != null && AddedComments.Count > 0)
                    {
                        foreach (LogEntriesByActionItem x in AddedComments)
                        {
                            x.PeopleCreator = null;
                            ObjActionPlanItem.LogEntriesByActionItems.Add(x);

                            //Added log.
                            LogEntriesByActionItem log = new LogEntriesByActionItem();
                            log.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                            log.CreationDate = GeosApplication.Instance.ServerDateTime;
                            log.IdLogEntryType = 258;

                            if (x.IsRtfText)
                            {
                                log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentAdded").ToString(), RtfToPlaintext(x));
                            }
                            else
                            {
                                log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentAdded").ToString(), x.Comment);
                            }

                            ChangedLogsEntries.Add(log);
                        }
                    }

                    if (comments != null)
                    {
                        foreach (LogEntriesByActionItem item in comments)
                        {
                            LogEntriesByActionItem logc = ActionPlanItemCommentsList.FirstOrDefault(x => x.IdLogEntryByActionItem == item.IdLogEntryByActionItem);

                            if (logc != null)
                            {
                                if (item.Comment != logc.Comment)
                                {
                                    logc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    logc.PeopleCreator = null;
                                    ObjActionPlanItem.LogEntriesByActionItems.Add(logc);

                                    //Updated log.
                                    LogEntriesByActionItem log = new LogEntriesByActionItem();
                                    log.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                                    log.CreationDate = GeosApplication.Instance.ServerDateTime;
                                    log.IdLogEntryType = 258;

                                    if (item.IsRtfText)
                                    {
                                        log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentUpdated").ToString(), RtfToPlaintext(item), RtfToPlaintext(logc));
                                    }
                                    else
                                    {
                                        log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentUpdated").ToString(), item.Comment, logc.Comment);
                                    }

                                    ChangedLogsEntries.Add(log);
                                }
                            }
                            else
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                logc.PeopleCreator = null;
                                ObjActionPlanItem.LogEntriesByActionItems.Add(item);

                                //Removed log.
                                LogEntriesByActionItem log = new LogEntriesByActionItem();
                                log.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                                log.CreationDate = GeosApplication.Instance.ServerDateTime;
                                log.IdLogEntryType = 258;

                                if (item.IsRtfText)
                                {
                                    log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentRemoved").ToString(), RtfToPlaintext(item));
                                }
                                else
                                {
                                    log.Comment = string.Format(System.Windows.Application.Current.FindResource("ActionChangeLogCommentRemoved").ToString(), item.Comment);
                                }

                                ChangedLogsEntries.Add(log);
                            }
                        }
                    }

                    ObjActionPlanItem.LogEntriesByActionItems.AddRange(ChangedLogsEntries.Where(i => i.IdLogEntryType == 258).ToList());
                    ObjActionPlanItem.LogEntriesByActionItems = ObjActionPlanItem.LogEntriesByActionItems.OrderBy(i => i.IsEnabled).ToList();
                    IsSave = CrmStartUp.UpdateActionPlanItem_V2043(ObjActionPlanItem);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    RequestClose(null, null);
                }

                GeosApplication.Instance.Logger.Log("Method EditActionViewAcceptAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in EditActionViewAcceptAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in EditActionViewAcceptAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in EditActionViewAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(ActionPlanItem actionPlanItem)
        {
            try
            {
                _ActionPlanItem = CrmStartUp.GetActionPlanItem_V2043(actionPlanItem.IdActionPlanItem);


                FillGroupList();
                FillCompanyPlantList();
                FillScopeList();
                FillTypeList();
                FillReporterList();
                FillStatusList();
                //FillResponsiblesList();

                Subject = _ActionPlanItem.Title;
                DueDate = _ActionPlanItem.CurrentDueDate;

                comments = _ActionPlanItem.LogEntriesByActionItems.Where(a => a.IdLogEntryType == 257).ToList();

                if (comments != null)
                {
                    ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>(comments.Select(x => (LogEntriesByActionItem)x.Clone()));   // for retrieve comment
                    if (ActionPlanItemCommentsList != null && ActionPlanItemCommentsList.Count > 0)
                    {
                        Description = ActionPlanItemCommentsList.OrderBy(i => i.IdLogEntryByActionItem).FirstOrDefault().Comment;
                        ActionPlanItemCommentsList.OrderBy(i => i.IdLogEntryByActionItem).FirstOrDefault().IsEnabled = false;
                        SetUserProfileImage(ActionPlanItemCommentsList);
                    }
                }
                ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>(ActionPlanItemCommentsList.OrderByDescending(i => i.CreationDate).ToList());
                if (!String.IsNullOrEmpty(Description))
                {
                    isDescriptionAdd = false;
                }
                else
                {
                    isDescriptionAdd = true;
                }

                List<LogEntriesByActionItem> logEntries = _ActionPlanItem.LogEntriesByActionItems.Where(a => a.IdLogEntryType == 258).ToList();

                if (logEntries != null)
                    ActionPlanItemChangeLogList = new ObservableCollection<LogEntriesByActionItem>(logEntries.OrderByDescending(x => x.CreationDate).Select(x => (LogEntriesByActionItem)x.Clone()));     // for change Log

                // Action Age Calculation
                if (_ActionPlanItem.CreationDate != null)
                {
                    CreatedInDate = _ActionPlanItem.CreationDate;
                    DateCalculateInYearAndMonthHelper dateCalculateInYearAndMonth = new DateCalculateInYearAndMonthHelper(GeosApplication.Instance.ServerDateTime.Date, _ActionPlanItem.CreationDate.Date);

                    if (dateCalculateInYearAndMonth.Years > 0)
                    {
                        ActionCreatedDays = dateCalculateInYearAndMonth.Years.ToString() + "+";
                        ActionCreatedDaysStr = string.Format(System.Windows.Application.Current.FindResource("EditActionViewYears").ToString());
                    }
                    else
                    {
                        ActionGenerateDays = (GeosApplication.Instance.ServerDateTime.Date - _ActionPlanItem.CreationDate.Date).Days;

                        if (ActionGenerateDays > 99)
                            ActionCreatedDays = "99+";
                        else
                            ActionCreatedDays = ActionGenerateDays.ToString();

                        ActionCreatedDaysStr = string.Format(System.Windows.Application.Current.FindResource("EdiActionViewDays").ToString());
                    }
                }

                if (_ActionPlanItem.ModificationDate.HasValue)
                    ModifiedInDate = _ActionPlanItem.ModificationDate.Value.Date;
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditTypeValueChangedCommandAction(object obj)
        {
            if (SelectedType != null)
            {
                if (SelectedType.IdLookupValue == 272)   // Information
                {
                    IsScopeVisible = Visibility.Collapsed;
                    IsReporterVisible = Visibility.Collapsed;
                    IsResponsibleVisible = Visibility.Collapsed;
                    IsDueDateVisible = Visibility.Collapsed;
                    IsStatusVisible = Visibility.Collapsed;
                    SubRowHeight = "70";
                    //TextHeight = "Auto";
                    SubRowIndex = 3;
                }
                else //if (SelectedType.IdLookupValue == 271)
                {
                    IsScopeVisible = Visibility.Visible;
                    IsReporterVisible = Visibility.Visible;
                    IsResponsibleVisible = Visibility.Visible;
                    IsDueDateVisible = Visibility.Visible;
                    IsStatusVisible = Visibility.Visible;
                    RowHeight = "35";
                    SubRowHeight = "35";
                    //TextHeight = "25";
                    SubRowIndex = 4;
                }
            }
        }
        private void FillTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTypeList ...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = CrmStartUp.GetLookupValues(56);
                SalesActivityTypeList = new List<LookupValue>();
                SalesActivityTypeList = new List<LookupValue>(tempTypeList);

                SalesActivityTypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                SelectedIndexSalesActivityType = SalesActivityTypeList.FindIndex(x => x.IdLookupValue == _ActionPlanItem.IdSalesActivityType);
                SalesActivityTypeList.Remove(SalesActivityTypeList.Find(x => x.IdLookupValue == 273));
                //if (SalesActivityTypeList.Count > 0)
                //    SelectedIndexSalesActivityType = 1;                 // Action selected by default
                //else
                //    SelectedIndexSalesActivityType = 0;

                GeosApplication.Instance.Logger.Log("Method FillTypeList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //private void FillActionItemPlanComments(Object Obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillActionItemPlanComments ...", category: Category.Info, priority: Priority.Low);

        //        ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>(_ActionPlanItem.LogEntriesByActionItems.Where(a => a.IdLogEntryType ==1).ToList());   //1 for comment

        //        //ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>(_ActionPlanItem.IdActionPlanItem, 1).OrderByDescending(x => x.CreationDate);
        //        SetUserProfileImage(ActionItemPlanComments);
        //        //RtfToPlaintext();

        //        GeosApplication.Instance.Logger.Log("Method FillActionItemPlanComments() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Error in FillActionItemPlanComments() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Error in FillActionItemPlanComments() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in FillActionItemPlanComments() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void SetUserProfileImage(ObservableCollection<LogEntriesByActionItem> ActivityCommentsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                foreach (LogEntriesByActionItem item in ActionPlanItemCommentsList)
                {
                    byte[] UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(item.PeopleCreator.Login);

                    if (UserProfileImageByte != null)
                        item.PeopleCreator.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (item.PeopleCreator.IdPersonGender == 1)
                                item.PeopleCreator.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (item.PeopleCreator.IdPersonGender == 2)
                                item.PeopleCreator.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                            else if (item.PeopleCreator.IdPersonGender == null)
                                item.PeopleCreator.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                        }
                        else
                        {
                            if (item.PeopleCreator.IdPersonGender == 1)
                                item.PeopleCreator.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (item.PeopleCreator.IdPersonGender == 2)
                                item.PeopleCreator.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                            else if (item.PeopleCreator.IdPersonGender == null)
                                item.PeopleCreator.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
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
                GeosApplication.Instance.Logger.Log("Error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        public string RtfToPlaintext(LogEntriesByActionItem comment)
        {
            if (comment.IsRtfText)
            {
                TextRange range = null;

                var rtb = new RichTextBox();
                var doc = new FlowDocument();
                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(comment.Comment));
                range = new TextRange(doc.ContentStart, doc.ContentEnd);
                range.Load(stream, DataFormats.Rtf);

                if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                    return range.Text.Trim();
            }
            else
            {
                return comment.Comment;
            }

            return null;
        }
        /// <summary>
        /// Method for Set Comment User  image.
        /// </summary>
        public ImageSource SetUserProfileImage()
        {
            User user = new User();
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
                UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(GeosApplication.Instance.ActiveUser.Login);

                if (UserProfileImageByte != null)
                    UserProfileImage = ByteArrayToBitmapImage(UserProfileImageByte);
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueFemale.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        else if (user.IdUserGender == null)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueMale.png");

                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        else if (user.IdUserGender == null)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
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
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                        if (!string.IsNullOrEmpty(salesOwnersIds))

                            if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                            {
                                CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                            }
                            else
                            {
                                CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true).ToList().OrderBy(cust => cust.IdCustomer));
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                            }
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                    {
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                    }
                    else
                    {
                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true).ToList().OrderBy(cust => cust.IdCustomer));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
                    }
                }

                SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(i => i.IdCustomer == _ActionPlanItem.IdGroup));

                if (SelectedIndexCompanyGroup == -1)
                {
                    SelectedIndexCompanyGroup = 0;
                }

                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                        if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT21"))
                            EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT21"];
                        else
                        {
                            EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                            GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                        }
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)(GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"]);
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                    }
                }

                CompanyPlantList = new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList());

                if (CompanyPlantList != null)
                    SelectedIndexCompanyPlant = CompanyPlantList.IndexOf(CompanyPlantList.FirstOrDefault(x => x.IdCompany == _ActionPlanItem.IdSite));

                if (SelectedIndexCompanyPlant == -1)
                    SelectedIndexCompanyPlant = 0;

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillScopeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillScopeList ...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempScopeList = CrmStartUp.GetLookupValues(39);
                ScopeList = new List<LookupValue>(tempScopeList);
                SelectedScopeListIndex = ScopeList.FindIndex(x => x.IdLookupValue == _ActionPlanItem.IdScope);

                GeosApplication.Instance.Logger.Log("Method FillScopeList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillScopeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillScopeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillScopeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillReporterList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillReporterList ...", category: Category.Info, priority: Priority.Low);
                ReporterList = new ObservableCollection<User>(CrmStartUp.GetSalesAndCommericalUsers());
                SelectedReporter = ReporterList.IndexOf(ReporterList.FirstOrDefault(x => x.IdUser == _ActionPlanItem.IdReporter));
                GeosApplication.Instance.Logger.Log("Method FillReporterList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillReporterList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillReporterList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillReporterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditValueChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);
                List<People> responsibles = new List<People>(CrmStartUp.GetActionPlanItemResponsible(SelectedPlant.IdCompany));
                People people = new People();
                people.Name = "---";
                responsibles.Insert(0, people);
                ResponsibleList = new ObservableCollection<People>(responsibles);

                SelectedResponsibleListIndex = ResponsibleList.IndexOf(ResponsibleList.FirstOrDefault(x => x.IdPerson == _ActionPlanItem.IdAssignee));

                if (SelectedResponsibleListIndex == -1)
                {
                    SelectedResponsibleListIndex = 0;
                }
                GeosApplication.Instance.Logger.Log("Method EditValueChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in EditValueChangedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in EditValueChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in EditValueChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList() ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = CrmStartUp.GetLookupValues(48);
                StatusList = new List<LookupValue>(tempStatusList);
                SelectedStatusListIndex = StatusList.FindIndex(x => x.IdLookupValue == _ActionPlanItem.IdStatus);

                GeosApplication.Instance.Logger.Log("Method FillStatusList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddOrEditActionCommentViewWindowShow(object obj)
        {
            try
            {

                AddOrEditActionCommentsViewModel addOrEditActionCommentsViewModel = new AddOrEditActionCommentsViewModel();
                AddOrEditActionCommentsView addOrEditActionCommentsView = new AddOrEditActionCommentsView();
                EventHandler handle = delegate { addOrEditActionCommentsView.Close(); };
                addOrEditActionCommentsViewModel.RequestClose += handle;
                addOrEditActionCommentsView.DataContext = addOrEditActionCommentsViewModel;
                addOrEditActionCommentsViewModel.AddInit();
                addOrEditActionCommentsView.ShowDialog();

                if (addOrEditActionCommentsViewModel.SelectedComment != null)
                {
                    if (ActionPlanItemCommentsList == null)
                        ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>();

                    ActionPlanItemCommentsList.Add(addOrEditActionCommentsViewModel.SelectedComment as LogEntriesByActionItem);
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>    
        /// [001][20200825]][starakh][GEOS2-2336] Allow add a Description to the new Actions
        /// </summary>
        private void CommentDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method convert CommentDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (obj == null) return;

            LogEntriesByActionItem commentOffer = (LogEntriesByActionItem)obj;
            // [001] start
            if (ActionPlanItemCommentsList.OrderBy(apl => apl.IdLogEntryByActionItem).FirstOrDefault().IdLogEntryByActionItem != commentOffer.IdLogEntryByActionItem)
            {
                if (commentOffer.IdCreator == GeosApplication.Instance.ActiveUser.IdUser)
            {
                CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewUpdateComment").ToString();
                IsAdd = false;

                AddOrEditActionCommentsViewModel addOrEditActionCommentsViewModel = new AddOrEditActionCommentsViewModel();
                AddOrEditActionCommentsView addOrEditActionCommentsView = new AddOrEditActionCommentsView();
                EventHandler handle = delegate { addOrEditActionCommentsView.Close(); };
                addOrEditActionCommentsViewModel.RequestClose += handle;
                addOrEditActionCommentsView.DataContext = addOrEditActionCommentsViewModel;
               
                    addOrEditActionCommentsViewModel.EditInit(SelectedComment);
                    addOrEditActionCommentsView.ShowDialog();

                    // [001] end
                    if (addOrEditActionCommentsViewModel.SelectedComment != null)
                    {
                        LogEntriesByActionItem comment = addOrEditActionCommentsViewModel.SelectedComment as LogEntriesByActionItem;
                        Int32 index = ActionPlanItemCommentsList.IndexOf(ActionPlanItemCommentsList.FirstOrDefault(x => x.IdLogEntryByActionItem == comment.IdLogEntryByActionItem));
                        ActionPlanItemCommentsList[index].Comment = comment.Comment;
                        ActionPlanItemCommentsList[index].IsRtfText = comment.IsRtfText;
                    }

                    //// ShowCommentsFlyout = true;
                    //OldActionComment = String.Copy(commentOffer.Comment);
                    //NewActionComment = String.Copy(commentOffer.Comment);

                    //if (commentOffer.IsRtfText == true)
                    //    IsRtf = true;
                    //else
                    //    IsNormal = true;
                }
                else
                {
                    //ShowCommentsFlyout = false;
                    NewActionComment = null;
                    OldActionComment = null;

                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NotAllowedUpdateActionPlanItemComment").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }
            else
            {
                //ShowCommentsFlyout = false;
                NewActionComment = null;
                OldActionComment = null;

                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NotAllowedUpdateFirstActionPlanItemComment").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>    
        /// [001][20200825]][starakh][GEOS2-2336] Allow add a Description to the new Actions
        /// </summary>
        public void DeleteCommentCommandAction(object parameter)
        {

            GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

            LogEntriesByActionItem commentObject = (LogEntriesByActionItem)parameter;
            //[001]start
            if (ActionPlanItemCommentsList.OrderBy(apl => apl.IdLogEntryByActionItem).FirstOrDefault().IdLogEntryByActionItem != commentObject.IdLogEntryByActionItem)
            {

                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    return;
                }


                if (commentObject.IdCreator != GeosApplication.Instance.ActiveUser.IdUser)
                {
                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NotAllowedDeleteActionPlanItemComment").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteComment").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ActionPlanItemCommentsList != null && ActionPlanItemCommentsList.Count > 0)
                    {
                        foreach (LogEntriesByActionItem item in ActionPlanItemCommentsList.ToList())
                        {
                            if (item.Comment == commentObject.Comment)
                            {
                                //item.IsDeleted = true;
                                ActionPlanItemCommentsList.Remove((LogEntriesByActionItem)commentObject);
                            }
                        }
                    }
                }
            }
            else
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NotAllowedDeleteFirstActionPlanItemComment").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }
            //[001]End
            //ShowCommentsFlyout = false;
            NewActionComment = null;

            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        //private void FillGroupList()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

        //        if (CompanyGroupList == null)
        //            CompanyGroupList = new ObservableCollection<Customer>();

        //        if (GeosApplication.Instance.IdUserPermission == 21)
        //        {
        //            //TempCompanyGroupList = CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds);
        //            if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
        //                CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
        //            else
        //            {
        //                CompanyGroupList.AddRange(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
        //                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
        //            }
        //        }
        //        else
        //        {
        //            // TempCompanyGroupList = CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
        //            if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
        //                CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
        //            else
        //            {
        //                CompanyGroupList.AddRange(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
        //                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
        //                //  SelectedIndexCompanyGroup = CompanyGroupList.FindIndex(i => i.IdCustomer == SelectedCompanyList[0].Customers[0].IdCustomer);
        //            }

        //        }

        //        //CompanyGroupList = new List<Customer>();
        //        //CompanyGroupList.Insert(0, new Customer() { CustomerName = "---" });
        //        //CompanyGroupList.AddRange(TempCompanyGroupList);

        //        SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(i => i.IdCustomer == SelectedCompanyList[0].Customers[0].IdCustomer));
        //        if (SelectedIndexCompanyGroup == -1)
        //        {
        //            SelectedIndexCompanyGroup = 0;
        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in FillGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //private void FillCompanyPlantList()
        //{
        //    try
        //    {
        //        //if (!IsPlannedAppointment)
        //        //{
        //        GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);

        //        if (CompanyPlantList == null)
        //            CompanyPlantList = new ObservableCollection<Company>();

        //        if (GeosApplication.Instance.IdUserPermission == 21)
        //        {
        //            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
        //            {
        //                List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
        //                var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
        //                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT21"))
        //                    EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT21"];
        //                else
        //                {
        //                    EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
        //                    GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
        //                EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"];
        //            else
        //            {
        //                EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
        //                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);

        //            }
        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
        //        // }
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void AddChangedLogsDetails()
        {
            GeosApplication.Instance.Logger.Log("Method AddChangedLogsDetails ...", category: Category.Info, priority: Priority.Low);

            if (ChangedLogsEntries == null)
                ChangedLogsEntries = new List<LogEntriesByActionItem>();

            //for Subject
            if (_ActionPlanItem.Title != null && !_ActionPlanItem.Title.Equals(Subject))
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogSubject").ToString(), _ActionPlanItem.Title, Subject), IdLogEntryType = 258 });
            }

            //for DueDate
            if (_ActionPlanItem.CurrentDueDate != null && _ActionPlanItem.CurrentDueDate.Date != DueDate.Date)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogDueDate").ToString(), _ActionPlanItem.CurrentDueDate.ToShortDateString(), DueDate.ToShortDateString()), IdLogEntryType = 258 });
            }

            //for status
            if (_ActionPlanItem.Status != null && _ActionPlanItem.Status.IdLookupValue != StatusList[SelectedStatusListIndex].IdLookupValue)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogStatus").ToString(), _ActionPlanItem.Status.Value, StatusList[SelectedStatusListIndex].Value), IdLogEntryType = 258 });
            }

            //for Scope
            if (_ActionPlanItem.IdScope != ScopeList[SelectedScopeListIndex].IdLookupValue)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogScope").ToString(), _ActionPlanItem.Scope, ScopeList[SelectedScopeListIndex].Value), IdLogEntryType = 258 });
            }

            // for group
            if (_ActionPlanItem.IdGroup != CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogGroup").ToString(), _ActionPlanItem.Group, CompanyGroupList[SelectedIndexCompanyGroup].CustomerName), IdLogEntryType = 258 });
            }

            // for plant
            if (_ActionPlanItem.IdSite != CompanyPlantList[SelectedIndexCompanyPlant].IdCompany)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogPlant").ToString(), _ActionPlanItem.Plant, CompanyPlantList[SelectedIndexCompanyPlant].Name), IdLogEntryType = 258 });
            }

            //for reporter
            if (_ActionPlanItem.IdReporter != ReporterList[SelectedReporter].IdUser)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogReporter").ToString(), _ActionPlanItem.Reporter, ReporterList[SelectedReporter].FullName), IdLogEntryType = 258 });
            }

            // for responsible
            if (_ActionPlanItem.Assignee != null && _ActionPlanItem.IdAssignee != ResponsibleList[SelectedResponsibleListIndex].IdPerson)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogResponsible").ToString(), _ActionPlanItem.Assignee, ResponsibleList[SelectedResponsibleListIndex].FullName), IdLogEntryType = 258 });
            }

            // for type
            if (_ActionPlanItem.IdSalesActivityType != SalesActivityTypeList[SelectedIndexSalesActivityType].IdLookupValue)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogType").ToString(), _ActionPlanItem.SalesActivityType.Value, SalesActivityTypeList[SelectedIndexSalesActivityType].Value), IdLogEntryType = 258 });
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validations

        bool allowValidation = false;
        private ActionPlanItem objActionPlanItem;
        private bool isSave;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup)] +
                    me[BindableBase.GetPropertyName(() => SelectedPlant)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexSalesActivityType)] +
                    me[BindableBase.GetPropertyName(() => SelectedScopeListIndex)] +
                    me[BindableBase.GetPropertyName(() => Subject)] +
                    me[BindableBase.GetPropertyName(() => SelectedReporter)] +
                    me[BindableBase.GetPropertyName(() => DueDate)] +
                    // me[BindableBase.GetPropertyName(() => Action)] +
                    me[BindableBase.GetPropertyName(() => SelectedResponsibleListIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedStatusListIndex)] +
                     me[BindableBase.GetPropertyName(() => Description)] +
                    me[BindableBase.GetPropertyName(() => EditInformationError)];


                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string SelectedIndexCompanyGroupProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup);
                string SelectedIndexCompanyPlantProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant);   // Lead Source
                string SelectedIndexSalesActivityTypeProp = BindableBase.GetPropertyName(() => SelectedIndexSalesActivityType);
                string SelectedScopeListIndexProp = BindableBase.GetPropertyName(() => SelectedScopeListIndex);
                string SubjectProp = BindableBase.GetPropertyName(() => Subject);
                string SelectedReporterProp = BindableBase.GetPropertyName(() => SelectedReporter);
                string DueDateProp = BindableBase.GetPropertyName(() => DueDate);
                string descriptionProp = BindableBase.GetPropertyName(() => Description);
                // string ActionProp = BindableBase.GetPropertyName(() => Action);
                string SelectedResponsibleListIndexProp = BindableBase.GetPropertyName(() => SelectedResponsibleListIndex);

                string SelectedStatusListIndexProp = BindableBase.GetPropertyName(() => SelectedStatusListIndex);

                //string CustomerCityProp = BindableBase.GetPropertyName(() => CustomerCity);

                string EditInformationErrorProp = BindableBase.GetPropertyName(() => EditInformationError);

                if (columnName == SelectedIndexCompanyGroupProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexCompanyGroupProp, SelectedIndexCompanyGroup);

                if (columnName == SelectedIndexCompanyPlantProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexCompanyPlantProp, SelectedIndexCompanyPlant);

                if (columnName == descriptionProp)
                    return RequiredValidationRule.GetErrorMessage(descriptionProp, Description);

                if (columnName == SelectedIndexSalesActivityTypeProp)    //Type
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexSalesActivityTypeProp, SelectedIndexSalesActivityType);

                if (columnName == SelectedScopeListIndexProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedScopeListIndexProp, SelectedScopeListIndex);

                //if (columnName == SubjectProp)
                //    return ActionValidation.GetErrorMessage(SubjectProp, Subject);

                if (columnName == SelectedReporterProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedReporterProp, SelectedReporter);

                //if (columnName == DueDateProp)
                //    return ActionValidation.GetErrorMessage(DueDateProp, DueDate);

                //if (columnName == ActionProp)
                //    return CustomerAddRequiredFieldValidation.GetErrorMessage(ActionProp, Action);

                if (columnName == SelectedResponsibleListIndexProp)
                    return ActionValidation.GetErrorMessage(SelectedResponsibleListIndexProp, SelectedResponsibleListIndex);

                if (columnName == SelectedStatusListIndexProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedStatusListIndexProp, SelectedStatusListIndex);

                if (columnName == EditInformationErrorProp)
                    return RequiredValidationRule.GetErrorMessage(EditInformationErrorProp, EditInformationError);
                return null;
            }
        }
        #endregion
    }
}
