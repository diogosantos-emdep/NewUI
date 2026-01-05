using DevExpress.DataProcessing;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Runtime;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Crm.Views;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddNewActionsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        #region Tasks
        #endregion

        #region Service
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Decleration
        private Visibility isScopeVisible;
        private Visibility isReporterVisible;
        private Visibility isResponsibleVisible;
        private Visibility isDueDateVisible;
        private Visibility isStatusVisible;
        private string rowHeight = "35";
        private string subRowHeight = "35";
        private Int32 subRowIndex = 4;
        private string textHeight = "25";
        private ObservableCollection<Customer> companyGroupList;
        private int selectedIndexCompanyGroup;
        private string selectedCustomerName;
        private int selectedIndexCompanyPlant;
        private int selectedIndexSalesActivityType;

        private ObservableCollection<Company> companyPlantList;
        private ObservableCollection<Type> salesActivityTypeList;
        private ObservableCollection<Company> entireCompanyPlantList;
        private int selectedScopeListIndex;
        private int selectedStatusListIndex;
        private int selectedResponsibleListIndex;

        private ObservableCollection<User> reporterList;
        private int selectedReporter;

        private ObservableCollection<People> responsibleList;

        private People selectedResponsible;
        private Company selectedPlant;
        private LookupValue selectedType;
        private string title;
        private string description;
        byte[] UserProfileImageByte = null;
        private ImageSource userProfileImage;
        private ObservableCollection<LogEntriesByActionItem> actionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>();
        private LogEntriesByActionItem selectedComment;
        private bool showCommentsFlyout;
        private int commentboxHeight;
        // private int screenHeight;
        private bool isBusy;
        private string informationError;
        private string subject;
        private string action;
        bool allowValidation = false;
        private DateTime dueDate;
        private string newActionComment;
        private string oldActionComment;
        private string commentButtonText;
        private bool isAdd;
        private bool isRtf;

        private Visibility textboxnormal;
        private Visibility richtextboxrtf;
        private bool isNormal = true;
        private string commentText;
        private int screenHeight;
        private bool isSave = false;
        private ActionPlanItem actionPlanItem;
        private ObservableCollection<LogEntriesByActionItem> actionPlanItemChangeLogList;

        // private ObservableCollection<People> linkeditemsPeopleList;
        private string visible;
        #endregion

        #region Icommand
        public ICommand AddNewActionsViewCancelButtonCommand { get; set; }
        public ICommand CommandEditValueChanged { get; private set; }
        public ICommand CommandEditTypeValueChanged { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand AddNewActionsViewAcceptButtonCommand { get; set; }
        public ICommand CommentButtonCheckedCommand { get; set; }
        public ICommand CommentButtonUncheckedCommand { get; set; }
        public ICommand IsnormalPreviewMouseRightButtonDown { get; set; }
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        public ICommand CommandOpenAddOrEditActionCommentClick { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<Customer> CompanyGroupList
        {
            get { return companyGroupList; }
            set
            {
                companyGroupList = value; OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroupList"));
            }
        }

        public String TextHeight
        {
            get
            {
                return textHeight;
            }

            set
            {
                textHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextHeight"));
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
        public Visibility IsScopeVisible
        {
            get { return isScopeVisible; }
            set
            {
                isScopeVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsScopeVisible"));
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
        public string SelectedCustomerName
        {
            get { return selectedCustomerName; }
            set { selectedCustomerName = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerName")); }
        }
        public int SelectedIndexSalesActivityType
        {
            get { return selectedIndexSalesActivityType; }
            set { selectedIndexSalesActivityType = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexSalesActivityType")); }
        }
        public int SelectedIndexCompanyPlant
        {
            get { return selectedIndexCompanyPlant; }
            set { selectedIndexCompanyPlant = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyPlant")); }
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
                        CompanyPlantList = new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList());
                    }
                    else
                    {
                        CompanyPlantList = null;
                    }
                }
            }
        }

        //public ObservableCollection<LookupValue> SalesActivityTypeList
        //{
        //    get { return SalesActivityTypeList; }
        //    set
        //    {
        //        SalesActivityTypeList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SalesActivityTypeList"));
        //    }
        //}
        public ObservableCollection<Company> CompanyPlantList
        {
            get { return companyPlantList; }
            set
            {
                companyPlantList = value;
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

        public List<LookupValue> ScopeList { get; set; }
        public List<LookupValue> StatusList { get; set; }
        public List<LookupValue> SalesActivityTypeList { get; set; }

        public int SelectedScopeListIndex
        {
            get { return selectedScopeListIndex; }
            set
            {
                selectedScopeListIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedScopeListIndex"));
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

        public int SelectedStatusListIndex
        {
            get { return selectedStatusListIndex; }
            set
            {
                selectedStatusListIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatusListIndex"));
            }
        }

        public ObservableCollection<User> ReporterList
        {
            get
            {
                return reporterList;
            }

            set
            {
                reporterList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReporterList"));
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

        public ObservableCollection<People> ResponsibleList
        {
            get
            {
                return responsibleList;
            }

            set
            {
                responsibleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResponsibleList"));
            }
        }

        public People SelectedResponsible
        {
            get
            {
                return selectedResponsible;
            }
            set
            {
                selectedResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedResponsible"));
            }
        }

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
        public Company SelectedPlant
        {
            get
            {
                return selectedPlant;
            }
            set
            {
                selectedPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (value != null)
                {
                    title = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("Title"));
                }
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

        public ObservableCollection<LogEntriesByActionItem> ActionPlanItemCommentsList
        {
            get { return actionPlanItemCommentsList; }
            set
            {
                actionPlanItemCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanItemCommentsList"));
            }
        }

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

        public LogEntriesByActionItem SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }

        public bool ShowCommentsFlyout
        {
            get { return showCommentsFlyout; }
            set
            {
                showCommentsFlyout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowCommentsFlyout"));
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

        public string OldActionComment
        {
            get { return oldActionComment; }
            set
            {
                oldActionComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldActionComment"));
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
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public string InformationError
        {
            get { return informationError; }
            set { informationError = value; OnPropertyChanged(new PropertyChangedEventArgs("InformationError")); }
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

        public string Action
        {
            get { return action; }
            set
            {

                action = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Action"));

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

        public bool IsNormal
        {
            get { return isNormal; }
            set
            {
                isNormal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNormal"));
            }
        }

        public string CommentText
        {
            get
            {
                return commentText;
            }

            set
            {
                commentText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentText"));
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

        public ActionPlanItem ActionPlanItem
        {
            get { return actionPlanItem; }
            set
            {
                actionPlanItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanItem"));
            }
        }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }
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

        #region Validation



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
                    me[BindableBase.GetPropertyName(() => InformationError)];


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
                string SelectedIndexSalesActivityTypeProp = BindableBase.GetPropertyName(() => SelectedIndexSalesActivityType);
                string SelectedIndexCompanyPlantProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant);   // Lead Source
                string SelectedScopeListIndexProp = BindableBase.GetPropertyName(() => SelectedScopeListIndex);
                string SubjectProp = BindableBase.GetPropertyName(() => Subject);
                string SelectedReporterProp = BindableBase.GetPropertyName(() => SelectedReporter);
                string DueDateProp = BindableBase.GetPropertyName(() => DueDate);
                // string ActionProp = BindableBase.GetPropertyName(() => Action);
                string SelectedResponsibleListIndexProp = BindableBase.GetPropertyName(() => SelectedResponsibleListIndex);
                string SelectedStatusListIndexProp = BindableBase.GetPropertyName(() => SelectedStatusListIndex);
                string descriptionProp = BindableBase.GetPropertyName(() => Description);

                //string CustomerCityProp = BindableBase.GetPropertyName(() => CustomerCity);

                string informationError = BindableBase.GetPropertyName(() => InformationError);



                if (columnName == SelectedIndexCompanyGroupProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexCompanyGroupProp, SelectedIndexCompanyGroup);

                if (columnName == SelectedIndexCompanyPlantProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexCompanyPlantProp, SelectedIndexCompanyPlant);

                if (columnName == SelectedIndexSalesActivityTypeProp)    //Type
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexSalesActivityTypeProp, SelectedIndexSalesActivityType);

                if (columnName == descriptionProp)    //Description
                    return RequiredValidationRule.GetErrorMessage(descriptionProp, description);

                if (columnName == SelectedScopeListIndexProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedScopeListIndexProp, SelectedScopeListIndex);

                //if (columnName == SubjectProp)
                //    return ActionValidation.GetErrorMessage(SubjectProp, Subject);

                if (columnName == SelectedReporterProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedReporterProp, SelectedReporter);

                //if (columnName == DueDateProp)
                //    return ActionValidation.GetErrorMessage(DueDateProp, DueDate);

                //if (columnName == SelectedResponsibleListIndexProp)
                //    return ActionValidation.GetErrorMessage(SelectedResponsibleListIndexProp, SelectedResponsibleListIndex);


                //if (columnName == ActionProp)
                //    return CustomerAddRequiredFieldValidation.GetErrorMessage(ActionProp, Action);

                if (columnName == SelectedStatusListIndexProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedStatusListIndexProp, SelectedStatusListIndex);

                if (columnName == informationError)
                    return RequiredValidationRule.GetErrorMessage(informationError, InformationError);
                return null;
            }
        }
        #endregion 

        #region Constructor
        public AddNewActionsViewModel()
        {
            try
            {
                screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                CommentboxHeight = screenHeight - 680;

                CRMCommon.Instance.Init();
                PleaseWait();

                AddNewActionsViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                CommandEditValueChanged = new DelegateCommand<object>(EditValueChangedCommandAction);
                CommandEditTypeValueChanged = new DelegateCommand<object>(EditTypeValueChangedCommandAction);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                AddNewActionsViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AddNewActionsAccept);

                //Comments Section
                //CommentButtonCheckedCommand = new DelegateCommand<object>(CommentButtonCheckedCommandAction);
                //CommentButtonUncheckedCommand = new DelegateCommand<object>(CommentButtonUncheckedCommandAction);
                //IsnormalPreviewMouseRightButtonDown = new DelegateCommand<object>(IsnormalCommandAction);
                // AddNewCommentCommand = new DelegateCommand<object>(AddCommentCommandAction);
                CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);
                CommandOpenAddOrEditActionCommentClick = new DelegateCommand<object>(AddOrEditActionCommentViewWindowShow);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                FillGroupList();
                FillCompanyPlantList();
                FillResponsiblesList();
                FillScopeList();
                FillTypeList();
                FillStatusList();
                FillReporterList();
                DueDate = DateTime.Today;

                //set hide/show shortcuts on permissions
                Visible = Visibility.Visible.ToString();
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    Visible = Visibility.Hidden.ToString();
                }
                else
                {
                    Visible = Visibility.Visible.ToString();
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor AddNewActionsViewModel - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method For Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void InitAddin(string senderEmail)
        {
            CRMCommon.Instance.Init();

            PeopleDetails pd = CrmStartUp.GetGroupPlantByMailId(senderEmail);

            if (pd != null)
            {
                SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(x => x.IdCustomer == pd.IdCustomer));

                if (SelectedIndexCompanyGroup != -1)
                    SelectedIndexCompanyPlant = CompanyPlantList.IndexOf(CompanyPlantList.FirstOrDefault(x => x.IdCompany == pd.IdSite));
            }
        }

        /// <summary>    
        /// [001][20200825]][starakh][GEOS2-2336] Allow add a Description to the new Actions
        /// [002][GEOS2-2265][avpawar][20-08-2020][Fields are not editable from columns]
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewActionsAccept(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewActionsAccept ...", category: Category.Info, priority: Priority.Low);

                if (SelectedType.IdLookupValue == 272)   // Information
                {
                    SelectedScopeListIndex = ScopeList.FindIndex(x => x.IdLookupValue == 236);      //227
                    SelectedReporter = ReporterList.IndexOf(ReporterList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser));
                    SelectedResponsibleListIndex = ResponsibleList.IndexOf(ResponsibleList.FirstOrDefault(x => x.IdPerson == GeosApplication.Instance.ActiveUser.IdUser));
                    SelectedStatusListIndex = StatusList.IndexOf(StatusList.FirstOrDefault(x => x.IdLookupValue == 266));       //254
                    DueDate = GeosApplication.Instance.ServerDateTime;
                }

                IsBusy = true;
                InformationError = null;
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexSalesActivityType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedScopeListIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("Subject"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedReporter"));
                PropertyChanged(this, new PropertyChangedEventArgs("DueDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedResponsibleListIndex"));
                // PropertyChanged(this, new PropertyChangedEventArgs("Action")); 
                PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatusListIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    ActionPlanItem = new ActionPlanItem();

                    ActionPlanItem.IdActionPlan = 1;
                    ActionPlanItem.IdGroup = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;
                    ActionPlanItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                    ActionPlanItem.IdScope = ScopeList[SelectedScopeListIndex].IdLookupValue;
                    ActionPlanItem.IdStatus = StatusList[SelectedStatusListIndex].IdLookupValue;
                    ActionPlanItem.IdReporter = ReporterList[SelectedReporter].IdUser;
                    ActionPlanItem.IdAssignee = ResponsibleList[SelectedResponsibleListIndex].IdPerson;
                    ActionPlanItem.Group = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName;
                    ActionPlanItem.Plant = CompanyPlantList[SelectedIndexCompanyPlant].SiteNameWithoutCountry;
                    ActionPlanItem.IdSalesActivityType = SalesActivityTypeList[SelectedIndexSalesActivityType].IdLookupValue;
                    ActionPlanItem.SalesActivityType = SalesActivityTypeList[SelectedIndexSalesActivityType];
                    ActionPlanItem.Title = Subject;
                    ActionPlanItem.Scope = ScopeList[SelectedScopeListIndex].Value;
                    ActionPlanItem.Reporter = ReporterList[SelectedReporter].FullName;
                    ActionPlanItem.Assignee = ResponsibleList[SelectedResponsibleListIndex].FullName;
                    ActionPlanItem.Status = StatusList[SelectedStatusListIndex];
                    ActionPlanItem.CurrentDueDate = DueDate;
                    ActionPlanItem.ExpectedDueDate = DueDate;
                    ActionPlanItem.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                    ActionPlanItem.OpenDate = GeosApplication.Instance.ServerDateTime.Date;
                    ActionPlanItem.LstResponsible = ResponsibleList.ToList();   //[002] Added
                    ActionPlanItem.TransactionOperation = ModelBase.TransactionOperations.Add;

                    if (ActionPlanItem.IdStatus == 266)
                        ActionPlanItem.CloseDate = GeosApplication.Instance.ServerDateTime;
                    if (SelectedType.IdLookupValue == 272)   // Information
                        ActionPlanItem.CloseDate = GeosApplication.Instance.ServerDateTime;
                    //[001] start
                    if (!String.IsNullOrEmpty(Description))
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
                    //[001]End
                    ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>(ActionPlanItemCommentsList.OrderByDescending(c => c.CreationDate).ToList());

                    //Added Change logs.
                    ActionPlanItem.LogEntriesByActionItems = ActionPlanItemCommentsList.ToList();

                    //Add one log when inserting new action item.
                    LogEntriesByActionItem tempLogEntriesByActionItem = new LogEntriesByActionItem();
                    tempLogEntriesByActionItem.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                    tempLogEntriesByActionItem.IdLogEntryType = 258;
                    tempLogEntriesByActionItem.Creator = GeosApplication.Instance.ActiveUser.FirstName + ' ' + GeosApplication.Instance.ActiveUser.LastName;
                    tempLogEntriesByActionItem.CreationDate = GeosApplication.Instance.ServerDateTime;
                    tempLogEntriesByActionItem.Comment = string.Format(System.Windows.Application.Current.FindResource("AddNewActionChangeLog").ToString(), Subject);
                    //tempLogEntriesByActionItem. = GeosApplication.Instance.ServerDateTime;
                    ActionPlanItem.LogEntriesByActionItems.Add(tempLogEntriesByActionItem);

                    //Added normal comments.
                    ActionPlanItem.LogEntriesByActionItems.Where(a => a.PeopleCreator != null).ForEach(x => x.PeopleCreator.OwnerImage = null);
                    ActionPlanItem.LogEntriesByActionItems = ActionPlanItem.LogEntriesByActionItems.OrderBy(i => i.IsEnabled).ToList();
                    ActionPlanItem = CrmStartUp.AddActionPlanItem_V2043(ActionPlanItem);

                    ActionPlan ac = CRMCommon.Instance.GetActionPlan();
                    ActionPlanItem.ActionPlan = new ActionPlan();
                    ActionPlanItem.ActionPlan.Code = string.Format("{0} - {1}", ac.Code, ActionPlanItem.Number);
                    IsSave = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionAddedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method AddNewActionsAccept() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewActionsAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewActionsAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewActionsAccept() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void DeleteCommentCommandAction(object parameter)
        {
            GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

            LogEntriesByActionItem commentObject = (LogEntriesByActionItem)parameter;

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (commentObject.IdCreator != GeosApplication.Instance.ActiveUser.IdUser)
            {
                CustomMessageBox.Show("Not Allowed to delete Comment!", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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

            ShowCommentsFlyout = false;
            NewActionComment = null;

            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                ////Added user permission type 22 for show all group type.
                //if (CompanyGroupList == null)
                //    CompanyGroupList = new ObservableCollection<Customer>();

                //CompanyGroupList.AddRange(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, 22, true));

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                        //TempCompanyGroupList = CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds);

                        if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                            CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];

                        else
                        {

                            CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                            GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                            // SelectedIndexCompanyGroup = 0;
                        }
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                        CompanyGroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                    else
                    {

                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);

                    }

                }

                SelectedIndexCompanyGroup = 0;
                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            #endregion

        }

        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);

                if (CompanyPlantList == null)
                    CompanyPlantList = new ObservableCollection<Company>();

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

                    //TempcompanyPlant = CrmStartUp.GetCompanyPlantByCustomerId(CompanyGroupList[selectedIndexCompanyGroup].IdCustomer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)(GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"]);
                    else
                    {
                        //TempcompanyPlant = new List<Company>(CrmStartUp.GetCompanyPlantByCustomerId(CompanyGroupList[selectedIndexCompanyGroup].IdCustomer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission));
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                        //SelectedIndexCompanyPlant = -1;
                    }
                }
                // ObservableCollection<Company> TempcompanyPlantList = new ObservableCollection<Company>();
                // TempcompanyPlantList.Insert(0, new Company() { Name = "---" });
                // TempcompanyPlantList.AddRange(TempcompanyPlant);
                // CompanyPlantList = new ObservableCollection<Company>();
                // CompanyPlantList = TempcompanyPlantList;
                //SelectedIndexCompanyPlant = 0;
                // if (SelectedIndexCompanyPlant == -1)
                //    SelectedIndexCompanyPlant = 0;

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillScopeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillScopeList ...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempScopeList = CrmStartUp.GetLookupValues(39);
                // ScopeList = new List<LookupValue>();
                ScopeList = new List<LookupValue>(tempScopeList);
                SelectedScopeListIndex = ScopeList.FindIndex(x => x.IdLookupValue == 236);

                GeosApplication.Instance.Logger.Log("Method FillScopeList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillScopeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillScopeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillScopeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                SelectedIndexSalesActivityType = SalesActivityTypeList.FindIndex(x => x.IdLookupValue == 271);
                SalesActivityTypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });
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
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList() ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = CrmStartUp.GetLookupValues(48);
                //StatusList = new List<LookupValue>();
                StatusList = new List<LookupValue>(tempStatusList);

                GeosApplication.Instance.Logger.Log("Method FillStatusList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillReporterList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillReporterList ...", category: Category.Info, priority: Priority.Low);
                ReporterList = new ObservableCollection<User>(CrmStartUp.GetSalesAndCommericalUsers());

                SelectedReporter = ReporterList.IndexOf(ReporterList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser));

                GeosApplication.Instance.Logger.Log("Method FillReporterList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillReporterList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillReporterList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillReporterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    //RowHeight = 0;
                    SubRowHeight = "70";
                    TextHeight = "Auto";
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
                    TextHeight = "25";
                    SubRowIndex = 4;
                }
            }
        }

        private void EditValueChangedCommandAction(object obj)
        {
            FillResponsiblesList();
        }



        private void FillResponsiblesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);

                if (SelectedPlant != null)
                {
                    List<People> responsibles = new List<People>(CrmStartUp.GetActionPlanItemResponsible(SelectedPlant.IdCompany));
                    People people = new People();
                    people.Name = "---";
                    responsibles.Insert(0, people);
                    ResponsibleList = new ObservableCollection<People>(responsibles);
                }
                else
                {
                    List<People> responsibles = new List<People>();
                    People people = new People();
                    people.Name = "---";
                    responsibles.Insert(0, people);
                    ResponsibleList = new ObservableCollection<People>(responsibles);
                }

                GeosApplication.Instance.Logger.Log("Method EditValueChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditValueChangedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditValueChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditValueChangedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //private void CommentButtonCheckedCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method convert CommentButtonCheckedCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    // if (string.IsNullOrEmpty(OldActivityComment))

        //    //CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
        //    //IsAdd = true;
        //    //ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;

        //    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //    {
        //        // DXSplashScreen.Show<SplashScreenView>(); 
        //        DXSplashScreen.Show(x =>
        //        {
        //            Window win = new Window()
        //            {
        //                ShowActivated = false,
        //                WindowStyle = WindowStyle.None,
        //                ResizeMode = ResizeMode.NoResize,
        //                AllowsTransparency = true,
        //                Background = new SolidColorBrush(Colors.Transparent),
        //                ShowInTaskbar = false,
        //                Topmost = true,
        //                SizeToContent = SizeToContent.WidthAndHeight,
        //                WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //            };
        //            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //            win.Topmost = false;
        //            return win;
        //        }, x =>
        //        {
        //            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //        }, null, null);
        //    }

        //    AddOrEditActionCommentsViewModel addOrEditActionCommentsViewModel = new AddOrEditActionCommentsViewModel();
        //    AddOrEditActionCommentsView addOrEditActionCommentsView = new AddOrEditActionCommentsView();
        //    EventHandler handle = delegate { addOrEditActionCommentsView.Close(); };
        //    addOrEditActionCommentsViewModel.RequestClose += handle;
        //    addOrEditActionCommentsView.DataContext = addOrEditActionCommentsViewModel;

        //    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //    {
        //        DXSplashScreen.Close();
        //    }
        //    addOrEditActionCommentsView.ShowDialog();

        //    NewActionComment = "";
        //    OldActionComment = "";
        //    IsRtf = false;
        //    IsNormal = true;
        //    if (IsNormal == true)
        //    {
        //        Textboxnormal = Visibility.Visible;
        //        Richtextboxrtf = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        Textboxnormal = Visibility.Collapsed;
        //        Richtextboxrtf = Visibility.Visible;
        //    }

        //    GeosApplication.Instance.Logger.Log("Method CommentButtonCheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //}

        //private void CommentButtonUncheckedCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method convert CommentButtonUncheckedCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    //if (string.IsNullOrEmpty(OldActivityComment))

        //    //CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
        //    //IsAdd = true;
        //    //ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;

        //    // CloseWindow();

        //    RequestClose(null, null);

        //    NewActionComment = null;
        //    OldActionComment = null;
        //    IsRtf = false;
        //    IsNormal = true;
        //    if (IsNormal == true)
        //    {
        //        Textboxnormal = Visibility.Visible;
        //        Richtextboxrtf = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        Textboxnormal = Visibility.Collapsed;
        //        Richtextboxrtf = Visibility.Visible;
        //    }

        //    GeosApplication.Instance.Logger.Log("Method CommentButtonUncheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //}

        //private void IsnormalCommandAction(object gcComments)
        //{
        //    string convertedText = string.Empty;
        //    if (IsNormal)
        //    {
        //        var document = ((RichTextBox)gcComments).Document;
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
        //            range2.Save(ms, DataFormats.Text);
        //            ms.Seek(0, SeekOrigin.Begin);
        //            using (StreamReader sr = new StreamReader(ms))
        //            {
        //                convertedText = sr.ReadToEnd();
        //            }
        //        }
        //        NewActionComment = convertedText;
        //    }
        //}

        //public void AddCommentCommandAction(object gcComments)
        //{
        //    GeosApplication.Instance.Logger.Log("Method convert AddCommentCommandAction ...", category: Category.Info, priority: Priority.Low);
        //    if (IsRtf)
        //    {
        //        var document = ((RichTextBox)gcComments).Document;
        //        NewActionComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
        //        string convertedText = string.Empty;
        //        if (!string.IsNullOrEmpty(NewActionComment.Trim()))
        //        {
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
        //                range2.Save(ms, DataFormats.Rtf);
        //                ms.Seek(0, SeekOrigin.Begin);
        //                using (StreamReader sr = new StreamReader(ms))
        //                {
        //                    convertedText = sr.ReadToEnd();
        //                }
        //            }
        //        }

        //        NewActionComment = convertedText;
        //    }

        //    if (OldActionComment != null && !string.IsNullOrEmpty(OldActionComment.Trim()) && OldActionComment.Equals(OldActionComment.Trim()))
        //    {
        //        ShowCommentsFlyout = false;
        //        return;
        //    }

        //    //update Comment
        //    //if (CommentButtonText == System.Windows.Application.Current.FindResource("LeadsEditViewUpdateComment").ToString())
        //    //{
        //    //    if (!string.IsNullOrEmpty(NewActionComment) && !string.IsNullOrEmpty(NewActionComment.Trim()))
        //    //    {
        //       LogEntriesByActionItem comment = ActionPlanItemCommentsList.FirstOrDefault(x => x.Comment == OldActionComment);
        //    //        if (comment !=null)
        //    //        {
        //    //            comment.Comment = string.Copy(NewActionComment.Trim());
        //    //            comment.CreationDate = GeosApplication.Instance.ServerDateTime;
        //    //            SelectedComment = comment;
        //    //            if (comment.IdLogEntriesByActionItem != 0)
        //    //            {
        //    //                comment.
        //    //            }

        //    //        }
        //    //    }
        //    //}

        //        GeosApplication.Instance.Logger.Log("Method AddCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //}

        private void CommentDoubleClickCommandAction(object obj)
        {           
                GeosApplication.Instance.Logger.Log("Method convert CommentDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (obj == null) return;

            LogEntriesByActionItem commentOffer = (LogEntriesByActionItem)obj;

            if (commentOffer.IdCreator == GeosApplication.Instance.ActiveUser.IdUser)
            {
                // CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewUpdateComment").ToString();
                IsAdd = false;

                AddOrEditActionCommentsViewModel addOrEditActionCommentsViewModel = new AddOrEditActionCommentsViewModel();
                AddOrEditActionCommentsView addOrEditActionCommentsView = new AddOrEditActionCommentsView();
                EventHandler handle = delegate { addOrEditActionCommentsView.Close(); };
                addOrEditActionCommentsViewModel.RequestClose += handle;
                addOrEditActionCommentsView.DataContext = addOrEditActionCommentsViewModel;
                addOrEditActionCommentsViewModel.EditInit(commentOffer);
                addOrEditActionCommentsView.ShowDialog();

                if (addOrEditActionCommentsViewModel.SelectedComment != null)
                {
                    LogEntriesByActionItem comment = addOrEditActionCommentsViewModel.SelectedComment as LogEntriesByActionItem;
                    commentOffer.Comment = comment.Comment;
                }

                ////ShowCommentsFlyout = true;
                //OldActionComment = String.Copy(commentOffer.Comment);
                //NewActionComment = String.Copy(commentOffer.Comment);
                //if (commentOffer.IsRtfText == true)
                //    IsRtf = true;
                //else
                //    IsNormal = true;
                //addOrEditActionCommentsView.ShowDialog();

            }
            else
            {
                ShowCommentsFlyout = false;
                NewActionComment = null;
                OldActionComment = null;
                CustomMessageBox.Show("Not Allowed to update comment!", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
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

                if (addOrEditActionCommentsViewModel.ActionPlanItemCommentsList != null && addOrEditActionCommentsViewModel.ActionPlanItemCommentsList.Count > 0)
                {
                    if (ActionPlanItemCommentsList == null)
                    {
                        ActionPlanItemCommentsList = new ObservableCollection<LogEntriesByActionItem>();
                    }
                    ActionPlanItemCommentsList.AddRange(addOrEditActionCommentsViewModel.ActionPlanItemCommentsList);
                }
                //NewActionComment = "";
                //OldActionComment = "";
                //IsRtf = false;
                //IsNormal = true;
                //if (IsNormal == true)
                //{
                //    Textboxnormal = Visibility.Visible;
                //    Richtextboxrtf = Visibility.Collapsed;
                //}
                //else
                //{
                //    Textboxnormal = Visibility.Collapsed;
                //    Richtextboxrtf = Visibility.Visible;
                //}



            }
            catch (Exception ex)
            {

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

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new MemoryStream(byteArrayIn))
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
                return image;

                //GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        //private void FillResponsibleList()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillResponsibleList ...", category: Category.Info, priority: Priority.Low);

        //        //List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
        //        //string plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
        //        //Int32 IdSite = Convert.ToInt32(plantOwnersIds);

        //        ResponsibleList = new ObservableCollection<People>();
        //        //    ResponsibleList = new ObservableCollection<People>(CrmStartUp.GetActionPlanItemResponsible(Convert.ToInt32(plantOwnersIds)));
        //        ResponsibleList = new ObservableCollection<People>(CrmStartUp.GetActionPlanItemResponsible(18));


        //        SelectedResponsible = ResponsibleList.FirstOrDefault();

        //        GeosApplication.Instance.Logger.Log("Method FillResponsibleList() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
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

        public void PleaseWait()
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
        }

    }

}
