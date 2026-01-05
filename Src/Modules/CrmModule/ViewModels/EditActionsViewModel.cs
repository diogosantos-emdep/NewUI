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
using Emdep.Geos.Data.Common.Crm;
using DevExpress.Xpf.Map;
using DevExpress.Mvvm.UI;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    class EditActionsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        #region Service
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController("localhost:6699");
        //IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController("localhost:6699");
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
        private string rowTagHeight = "70";//chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        private string rowDesHeight = "90";
        private string subRowHeight = "50";//35
        private Int32 subRowIndex = 6;//5

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
        private string type;//chitra.girigosavi[20/03/2024][GEOS2-3804][ACTIONS_REVIEW] Display “ACTIONS” in the “Orders"
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
        private bool isEdit;
        private string editInformationError;
        private List<LogEntriesByActionItem> comments;
        //private string tagsMargine;
        private Tag selectedTag;
        private List<Tag> tagList;//chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        private string tagsString;  //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:

        //private ObservableCollection<People> linkeditemsPeopleList;

        #region chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
        private bool isSearchButtonEnable = true;
        private ObservableCollection<Offer> listPlantOpportunity;
        private bool isUserWatcherReadOnly;
        public bool isInit = false;
        private string actionsAddress;
        private double? latitude;
        private double? longitude;
        private GeoPoint mapLatitudeAndLongitude;
        private bool isCoordinatesNull;
        private bool isEditedFromOutSide = false;
        private ObservableCollection<ActionsLinkedItems> selectedActionLinkedItems = new ObservableCollection<ActionsLinkedItems>();
        private int selectedAccountActionLinkedItemsCount;
        private LookupValue selectedActionType;
        private Visibility isInformationVisible;
        private string editActionError;
        private ObservableCollection<People> linkeditemsPeopleList;
        private bool isPlannedAppointment;
        private bool isInfoAccountRemove = false;
        private bool isPODone;

        #endregion

        #region chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        private bool isInternalEnable;
        private List<User> actionOwnerList;
        private int selectedIndexOwner;
        #endregion
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
        public ICommand AddTagButtonCommand { get; set; }
        #region Linked Item chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
        public ICommand OfferRowMouseDoubleClickCommand { get; set; }
        public ICommand LinkedItemCancelCommand { get; set; }
        public ICommand LinkedItemDoubleClickCommand { get; set; }
        public ICommand SearchOfferCommand { get; set; }

        #endregion Linked Item End
        #endregion

        #region Properties
        bool isAddTagsPermissionCRM;
        public bool IsAddTagsPermissionCRM
        {
            get
            {
                return isAddTagsPermissionCRM;
            }

            set
            {
                isAddTagsPermissionCRM = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddTagsPermissionCRM"));
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
        public String RowTagHeight  //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        {
            get
            {
                return rowTagHeight;
            }

            set
            {
                rowTagHeight = value;
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
                    if (IsUserWatcherReadOnly == false)
                    {

                        if (SelectedIndexCompanyGroup > 0)
                        {
                            List<Offer> currentPlantOpportunityList = new List<Offer>();
                            FillPlantOpportunityList(false, currentPlantOpportunityList);

                            if (EntireCompanyPlantList != null)
                            {
                                CompanyPlantList = new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList().GroupBy(cpl => cpl.IdCompany).Select(group => group.First()).ToList());
                                SelectedIndexCompanyPlant = 0;
                            }
                        }
                        else
                        {
                            CompanyPlantList = null;
                            SelectedIndexCompanyPlant = -1;
                            ListPlantOpportunity = null;
                            if (!isInit)
                            {
                                if (IsInternal)
                                {
                                }
                                else
                                {
                                    ActionsAddress = string.Empty;
                                    MapLatitudeAndLongitude = null;
                                    Latitude = null;
                                    Longitude = null;
                                }
                            }
                            if (!IsEditedFromOutSide)
                                DeleteLinkedItem();
                            if (_ActionPlanItem.IdOwner != GeosApplication.Instance.ActiveUser.IdUser)
                            {
                                IsSearchButtonEnable = false;
                            }
                            else
                            {
                                if (GeosApplication.Instance.IsPermissionReadOnly)
                                    IsSearchButtonEnable = false;
                                else if (SelectedIndexCompanyGroup == 0)
                                    IsSearchButtonEnable = false;
                                else if (SelectedIndexCompanyGroup > 0)
                                    IsSearchButtonEnable = true;
                            }
                        }
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
        public string Type //chitra.girigosavi[20/03/2024][GEOS2-3804][ACTIONS_REVIEW] Display “ACTIONS” in the “Orders"
        {
            get { return type; }
            set
            {
                if (value != null)
                {
                    type = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Type"));
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

        //[pramod.misal][GEOS2-4690]][29-09-2023]       
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
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

        //public String TagsMargine
        //{
        //    get
        //    {
        //        return tagsMargine;
        //    }

        //    set
        //    {
        //        tagsMargine = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("TagsMargine"));
        //    }
        //}


        public Tag SelectedTag
        {
            get { return selectedTag; }
            set
            {
                selectedTag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTag"));
            }
        }

        public List<Tag> TagList
        {
            get { return tagList; }
            set { tagList = value; OnPropertyChanged(new PropertyChangedEventArgs("TagList")); }
        }

        public List<Object> SelectedTagList
        {
            get { return selectedTagList; }
            set
            {
                selectedTagList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTagList"));
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

        //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        public string TagsString
        {
            get { return tagsString; }
            set { tagsString = value; OnPropertyChanged(new PropertyChangedEventArgs("TagsString")); }
        }


        #region chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
        public bool IsSearchButtonEnable
        {
            get { return isSearchButtonEnable; }
            set
            {
                isSearchButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSearchButtonEnable"));
            }
        }
        public ObservableCollection<Offer> ListPlantOpportunity
        {
            get
            {
                return listPlantOpportunity;
            }

            set
            {
                listPlantOpportunity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListPlantOpportunity"));
            }
        }
        public bool IsUserWatcherReadOnly
        {
            get
            {
                return isUserWatcherReadOnly;
            }

            set
            {
                isUserWatcherReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUserWatcherReadOnly"));
            }
        }
        public string ActionsAddress
        {
            get { return actionsAddress; }
            set
            {
                if (value != null)
                {
                    actionsAddress = value.Trim();
                    OnPropertyChanged(new PropertyChangedEventArgs("ActivityAddress"));
                }
            }
        }
        public double? Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Latitude"));
            }
        }

        public double? Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Longitude"));
            }
        }
        public GeoPoint MapLatitudeAndLongitude
        {
            get { return mapLatitudeAndLongitude; }
            set
            {
                mapLatitudeAndLongitude = value;

                if (mapLatitudeAndLongitude != null)
                    IsCoordinatesNull = true;
                else
                    IsCoordinatesNull = false;

                OnPropertyChanged(new PropertyChangedEventArgs("MapLatitudeAndLongitude"));
            }
        }
        public bool IsCoordinatesNull
        {
            get { return isCoordinatesNull; }
            set
            {
                isCoordinatesNull = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCoordinatesNull"));
            }
        }
        public bool IsEditedFromOutSide
        {
            get
            {
                return isEditedFromOutSide;
            }

            set
            {
                isEditedFromOutSide = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditedFromOutSide"));
            }
        }
        public ObservableCollection<ActionsLinkedItems> SelectedActionLinkedItems
        {
            get { return selectedActionLinkedItems; }
            set
            {
                selectedActionLinkedItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionLinkedItems"));
            }
        }

        public int SelectedAccountActionLinkedItemsCount
        {
            get { return selectedAccountActionLinkedItemsCount; }
            set
            {
                selectedAccountActionLinkedItemsCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAccountActionLinkedItemsCount"));
            }
        }
        public LookupValue SelectedActionType
        {
            get { return selectedActionType; }
            set
            {
                selectedActionType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
            }
        }
        public Visibility IsInformationVisible
        {
            get { return isInformationVisible; }
            set
            {
                isInformationVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInformationVisible"));
            }
        }
        public string EditActionError
        {
            get { return editActionError; }
            set { editActionError = value; OnPropertyChanged(new PropertyChangedEventArgs("EditActionError")); }
        }
        public ObservableCollection<People> LinkeditemsPeopleList
        {
            get { return linkeditemsPeopleList; }
            set
            {
                linkeditemsPeopleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkeditemsPeopleList"));
            }
        }

        public bool IsPlannedAppointment
        {
            get
            {
                return isPlannedAppointment;
            }

            set
            {
                isPlannedAppointment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPlannedAppointment"));
            }
        }
        public bool IsPODone
        {
            get { return isPODone; }
            set { isPODone = value; OnPropertyChanged(new PropertyChangedEventArgs("IsPODone")); }
        }

        #endregion

        #region
        public bool IsInternalEnable
        {
            get { return isInternalEnable; }
            set { isInternalEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsInternalEnable")); }
        }

        ///ActionPlanItem Owner
        public List<User> ActionOwnerList
        {
            get { return actionOwnerList; }
            set
            {
                actionOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionOwnerList"));
            }
        }

        /// Selected Index Activity Owner
        public int SelectedIndexOwner
        {
            get { return selectedIndexOwner; }
            set
            {
                selectedIndexOwner = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexOwner"));
            }
        }
        #endregion
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

        public bool IsInternal
        {
            get
            {
                return isInternal;
            }
            set
            {
                isInternal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInternal"));
                try
                {
                    if (value)
                    {
                        IsAccountVisible = Visibility.Collapsed;
                        if (SelectedActionLinkedItems != null && SelectedActionLinkedItems.Count > 0)
                        {
                            if (SelectedActionLinkedItems[0].IdLinkedItemType == 42)     // Account
                            {
                                if (SelectedActionLinkedItems[0].IsVisible == true)
                                {

                                    SelectedIndexCompanyGroup = 0;
                                    // SelectedIndexCompanyPlant = 0;
                                    ActionsAddress = string.Empty;
                                    DeleteLinkedItem();
                                    //CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                                    //customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                                    //customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();
                                    //MapLatitudeAndLongitude = null;
                                    //Latitude = null;
                                    //Longitude = null;


                                }
                            }
                        }
                        FillResponsiblesList();
                        // IsResponsibleVisible = Visibility.Collapsed;
                    }
                    else
                    {
                        IsAccountVisible = Visibility.Visible;
                        FillResponsiblesList();
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in IsInternal Property " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }
        public Visibility IsAccountVisible
        {
            get
            {
                return isAccountVisible;
            }

            set
            {
                isAccountVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccountVisible"));
            }
        }
        public String RowDesHeight  //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        {
            get
            {
                return rowDesHeight;
            }

            set
            {
                rowDesHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RowHeight"));
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
                AddTagButtonCommand = new DelegateCommand<object>(AddNewTagCommandAction);
                #region Linked items chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
                OfferRowMouseDoubleClickCommand = new DelegateCommand<object>(OfferRowMouseDoubleClickCommandAction);
                LinkedItemCancelCommand = new DelegateCommand<object>(LinkedItemCancelCommandAction);
                LinkedItemDoubleClickCommand = new DelegateCommand<object>(LinkedItemDoubleClickCommandAction);
                SearchOfferCommand = new DelegateCommand<object>(SearchOffer);
                #endregion
                //[rdixit][GEOS2-4699][28.08.2023]
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 87) && up.Permission.IdGeosModule == 5))
                    IsAddTagsPermissionCRM = true;
                else
                    IsAddTagsPermissionCRM = false;

                //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
                SelectedTagList = new List<object>();

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// [001][GEOS2-2293][avpawar][22-11-2020][Define Internal Actions]
        /// </summary>
        /// <param name="obj"></param>
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
                    ObjActionPlanItem.IdOwner = ActionOwnerList[SelectedIndexOwner].IdUser;
                    // [001] Start
                    if (IsInternal == true)
                    {
                        ObjActionPlanItem.IdGroup = 0;
                        ObjActionPlanItem.Group = "";
                        ObjActionPlanItem.IdSite = 0;
                        ObjActionPlanItem.Plant = "";
                        ObjActionPlanItem.IdAssignee = ResponsibleList[SelectedResponsibleListIndex].IdPerson;
                        ObjActionPlanItem.Assignee = ResponsibleList[SelectedResponsibleListIndex].FullName;
                    }
                    else
                    {
                        ObjActionPlanItem.IdGroup = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;
                        ObjActionPlanItem.Group = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName;
                        ObjActionPlanItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                        ObjActionPlanItem.Plant = CompanyPlantList[SelectedIndexCompanyPlant].SiteNameWithoutCountry;
                        ObjActionPlanItem.IdAssignee = ResponsibleList[SelectedResponsibleListIndex].IdPerson;
                        ObjActionPlanItem.Assignee = ResponsibleList[SelectedResponsibleListIndex].FullName;
                    }
                    // [001] End

                    //ObjActionPlanItem.IdGroup = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;
                    //ObjActionPlanItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
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
                    //ObjActionPlanItem.IdAssignee = ResponsibleList[SelectedResponsibleListIndex].IdPerson;
                    ObjActionPlanItem.IdModifier = GeosApplication.Instance.ActiveUser.IdUser;
                    //ObjActionPlanItem.Group = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName;
                    //ObjActionPlanItem.Plant = CompanyPlantList[SelectedIndexCompanyPlant].SiteNameWithoutCountry;
                    ObjActionPlanItem.Title = Subject;
                    ObjActionPlanItem.IdSalesActivityType = SalesActivityTypeList[SelectedIndexSalesActivityType].IdLookupValue;
                    //objActionPlanItem.SalesActivityType = SelectedType;
                    //ObjActionPlanItem.SalesActivityType.Value = _ActionPlanItem.SalesActivityType.Value;

                    ObjActionPlanItem.Type = SalesActivityTypeList[SelectedIndexSalesActivityType].Value;
                    ObjActionPlanItem.Scope = ScopeList[SelectedScopeListIndex].Value;
                    ObjActionPlanItem.Reporter = ReporterList[SelectedReporter].FullName;
                    // ObjActionPlanItem.Assignee = ResponsibleList[SelectedResponsibleListIndex].FullName;
                    ObjActionPlanItem.Status = StatusList[SelectedStatusListIndex];
                    ObjActionPlanItem.CurrentDueDate = DueDate;
                    ObjActionPlanItem.IsInternal = Convert.ToByte(IsInternal);
                    ObjActionPlanItem.LstResponsible = ResponsibleList.ToList();
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
                    #region Linked Items chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”

                    List<ActionsLinkedItems> listActivityLinkedItems = new List<ActionsLinkedItems>();

                    //Delete linked item from list
                    foreach (ActionsLinkedItems item in _ActionPlanItem.ActionsLinkedItems)
                    {
                        if (!SelectedActionLinkedItems.Any(x => x.IdActionsLinkedItem == item.IdActionsLinkedItem))
                        {
                            ActionsLinkedItems actionLinkedItem = new ActionsLinkedItems();
                            actionLinkedItem = (ActionsLinkedItems)item.Clone();

                            actionLinkedItem.IsDeleted = true;
                            actionLinkedItem.IdActionPlanItem = _ActionPlanItem.IdActionPlanItem;
                            actionLinkedItem.IdActionsLinkedItem = item.IdActionsLinkedItem;
                            actionLinkedItem.ActionLinkedItemImage = null;
                            actionLinkedItem.IdSite = ObjActionPlanItem.IdSite;
                            if (item.IdLinkedItemType == 44)   // Opportunity
                            {
                                actionLinkedItem.IdLinkedItemType = 44;
                                actionLinkedItem.IdOffer = item.IdOffer;
                                actionLinkedItem.IdEmdepSite = item.IdEmdepSite;
                                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdActionPlanItem = _ActionPlanItem.IdActionPlanItem, IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveOpportunity").ToString(), item.Name), IdLogEntryType = 258 });
                            }
                            listActivityLinkedItems.Add(actionLinkedItem);
                        }
                    }

                    //Add linked item to list.
                    foreach (ActionsLinkedItems item in SelectedActionLinkedItems)
                    {
                        if (!_ActionPlanItem.ActionsLinkedItems.Any(x => x.IdActionsLinkedItem == item.IdActionsLinkedItem))
                        {
                            ActionsLinkedItems actionLinkedItem = new ActionsLinkedItems();
                            actionLinkedItem = (ActionsLinkedItems)item.Clone();
                            actionLinkedItem.IdActionPlanItem = _ActionPlanItem.IdActionPlanItem;
                            actionLinkedItem.ActionLinkedItemImage = null;
                            actionLinkedItem.IdSite = ObjActionPlanItem.IdSite;
                            if (item.IdLinkedItemType == 44)
                            {
                                actionLinkedItem.IdLinkedItemType = 44;
                                actionLinkedItem.IdOffer = item.IdOffer;
                                actionLinkedItem.IdEmdepSite = item.IdEmdepSite;
                                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdActionPlanItem = _ActionPlanItem.IdActionPlanItem, IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOpportunity").ToString(), item.Name), IdLogEntryType = 258 });
                                listActivityLinkedItems.Add(actionLinkedItem);
                            }
                        }
                    }

                    objActionPlanItem.ActionsLinkedItems = listActivityLinkedItems;

                    #endregion End Linked Items.
                    #region
                    // For Add 
                    //if (SelectedTagList != null && SelectedTagList.Count > 0)
                    //{
                    //    foreach (Tag item in SelectedTagList)
                    //    {
                    //        ActivityTag objTag = new ActivityTag();
                    //        objTag.IdTag = item.IdTag;
                    //        if (item.IdTag == 0)            // if new tag is added it assign new tag id
                    //        {
                    //            try
                    //            {
                    //                //Service IsExistTagName() Changed with IsExistTagName_V2350() by [GEOS2-4120][rdixit][10.01.2022]
                    //                bool isExist = CrmStartUp.IsExistTagName_V2350(item.Name.Trim());
                    //                if (!isExist)
                    //                {
                    //                    item.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    //                    Tag tagActivity = CrmStartUp.AddTag(item);
                    //                    objTag.IdTag = tagActivity.IdTag;
                    //                    if (selectedTag.Name == item.Name)
                    //                    {
                    //                        selectedTag.IdTag = tagActivity.IdTag;
                    //                    }
                    //                     GeosApplication.Instance.TagList.Add(tagActivity);
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                GeosApplication.Instance.Logger.Log(ex.Message, category: Category.Exception, priority: Priority.Low);
                    //            }
                    //        }
                    //        //listTag.Add(objTag);
                    //        //lstTag.Add(((Tag)item).Name.ToString());
                    //    }
                    //}

                    //if (SelectedTag != null)
                    //{
                    //    ObjActionPlanItem.IdTag = Convert.ToUInt64(SelectedTag.IdTag);
                    //    ObjActionPlanItem.Tag = SelectedTag; //TagList[SelectedTag].IdTag;
                    //}
                    //else
                    //{
                    //    ObjActionPlanItem.IdTag = null;
                    //}
                    #endregion
                    #region chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
                    // Delete Tag
                    if (SelectedTagList == null) { SelectedTagList = new List<object>(); }
                    if (SelectedTagList != null)
                    {
                        List<Tag> temptaglist = SelectedTagList.Cast<Tag>().ToList();
                        List<Tag> listtag = new List<Tag>();
                        List<string> lstTag = new List<string>();
                        foreach (Tag item in _ActionPlanItem.ActionTag)
                        {
                            if (!temptaglist.Any(x => x.IdTag == item.IdTag))
                            {
                                Tag actionTag = new Tag();
                                actionTag.IsDeleted = true;
                                actionTag.IdActionPlanItem = _ActionPlanItem.IdActionPlanItem;
                                actionTag.IdTag = item.IdTag;
                                actionTag.IdActionTag = item.IdActionTag;
                                //[rdixit][GEOS2-5317][02.02.2024]
                                ChangedLogsEntries.Add(new  LogEntriesByActionItem() { IdActionPlanItem = actionTag.IdActionPlanItem, IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveTag").ToString(), item.Name), IdLogEntryType = 258 });
                                listtag.Add(actionTag);
                            }
                        }

                        // For Add tag
                        foreach (Tag tagitem in temptaglist)
                        {
                            if (!_ActionPlanItem.ActionTag.Any(x => x.IdTag == tagitem.IdTag))
                            {
                                if (tagitem.IdTag == 0)            // if new tag is added it assign new tag id
                                {
                                    try
                                    {
                                        //Service IsExistTagName() Changed with IsExistTagName_V2350() by [GEOS2-4120][rdixit][10.01.2022]
                                        bool isExist = CrmStartUp.IsExistTagName_V2350(tagitem.Name.Trim());
                                        if (!isExist)
                                        {
                                            tagitem.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                            Tag tagActivity = CrmStartUp.AddTag(tagitem);
                                            tagitem.IdTag = tagActivity.IdTag;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                Tag actionTag = new Tag();
                                actionTag.IsDeleted = false;
                                actionTag.IdActionPlanItem = _ActionPlanItem.IdActionPlanItem;
                                actionTag.IdTag = tagitem.IdTag;
                                //[rdixit][GEOS2-5317][02.02.2024]
                                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdActionPlanItem = actionTag.IdActionPlanItem, IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddTag").ToString(), tagitem.Name), IdLogEntryType = 258 });
                                listtag.Add(actionTag);
                            }
                            lstTag.Add(((Tag)tagitem).Name.ToString());
                        }

                        TagsString = string.Join(" , ", lstTag.ToList());
                        ObjActionPlanItem.ActionTag = listtag;
                    }


                    #endregion
                    ObjActionPlanItem.LogEntriesByActionItems.AddRange(ChangedLogsEntries.Where(i => i.IdLogEntryType == 258).ToList());
                    ObjActionPlanItem.LogEntriesByActionItems = ObjActionPlanItem.LogEntriesByActionItems.OrderBy(i => i.IsEnabled).ToList();
                    //IsSave = CrmStartUp.UpdateActionPlanItem_V2380(ObjActionPlanItem); //service UpdateActionPlanItem_V2120 updated with UpdateActionPlanItem_V2380 by [rdixit][GEOS2-4372][27.04.2023]
                    //IsSave = CrmStartUp.UpdateActionPlanItem_V2480(ObjActionPlanItem); //service UpdateActionPlanItem_V2380 updated with UpdateActionPlanItem_V2480 by //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
                    IsSave = CrmStartUp.UpdateActionPlanItem_V2500(ObjActionPlanItem); //chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    #region GEOS2-3804 [ACTIONS_REVIEW] Display “ACTIONS” in the “Orders" 
                    if (IsSave)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        ObjActionPlanItem.LookupValue = SalesActivityTypeList[SelectedIndexSalesActivityType];
                        ObjActionPlanItem.People = new People();
                        ObjActionPlanItem.People.Name = ActionOwnerList[SelectedIndexOwner].FirstName;
                        ObjActionPlanItem.People.Surname = ActionOwnerList[SelectedIndexOwner].LastName;
                    }
                    #endregion
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
        private void FillResponsiblesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);
                #region [rdixit][13.12.2024][GEOS2-6686]
                //if (SelectedPlant != null)
                //{
                //    List<People> responsibles = new List<People>();
                //    if (IsInternal)
                //    {
                //        responsibles = new List<People>(CrmStartUp.GetActionPlanItemResponsible(0));
                //    }
                //    else
                //    {
                //        responsibles = new List<People>(CrmStartUp.GetActionPlanItemResponsible(SelectedPlant.IdCompany));
                //    }
                //    People people = new People();
                //    people.Name = "---";
                //    responsibles.Insert(0, people);
                //    ResponsibleList = new ObservableCollection<People>(responsibles);
                //    if(ResponsibleList.Any(i => i.IdPerson == GeosApplication.Instance.ActiveUser.IdUser))
                //    SelectedResponsibleListIndex = ResponsibleList.IndexOf(ResponsibleList.Where(i => i.IdPerson == GeosApplication.Instance.ActiveUser.IdUser).FirstOrDefault());
                //}
                //else
                //{
                //    List<People> responsibles = new List<People>(CrmStartUp.GetActionPlanItemResponsible(0));
                //    People people = new People();
                //    people.Name = "---";
                //    responsibles.Insert(0, people);
                //    ResponsibleList = new ObservableCollection<People>(responsibles);
                //}
                #endregion

                if (_ActionPlanItem.LstResponsible != null)
                    ResponsibleList = new ObservableCollection<People>(_ActionPlanItem.LstResponsible.ToList());//[rdixit][13.12.2024][GEOS2-6686]

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

        public void Init(ActionPlanItem actionPlanItem)
        {
            try
            {
                //_ActionPlanItem = CrmStartUp.GetActionPlanItem_V2120(actionPlanItem.IdActionPlanItem);
                //CheckAllPermissions();
                // FillTagsList();
                #region chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
                //_ActionPlanItem = new ActionPlanItem();
                _ActionPlanItem = actionPlanItem;
                FillResponsiblesList();
                #endregion

                //_ActionPlanItem = CrmStartUp.GetActionPlanItem_V2480(actionPlanItem.IdActionPlanItem);  //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
                //_ActionPlanItem = CrmStartUp.GetActionPlanItem_V2500(actionPlanItem.IdActionPlanItem);
                _ActionPlanItem = CrmStartUp.GetActionPlanItem_V2520(actionPlanItem.IdActionPlanItem);
                FillGroupList();
                FillCompanyPlantList();
                FillScopeList();
                FillTypeList();
                FillReporterList();
                FillStatusList();
             
                FillTagList();
                FillOwnerList(_ActionPlanItem.IdOwner);//chitra.girigosavi[20/03/2024][GEOS2-3804][ACTIONS_REVIEW] Display “ACTIONS” in the “Orders"
               // SelectedIndexOwner = ActionOwnerList.FindIndex(i => i.IdUser == _ActionPlanItem.IdOwner);//chitra.girigosavi[20/03/2024][GEOS2-3804][ACTIONS_REVIEW] Display “ACTIONS” in the “Orders"
                SelectedIndexOwner = ActionOwnerList?.FindIndex(i => i?.IdUser == _ActionPlanItem?.IdOwner) ?? -1;//[rdixit][13.12.2024][GEOS2-6686]
                Type = _ActionPlanItem.SalesActivityType.Value;
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

                if (_ActionPlanItem.IsInternal == 0)
                {
                    IsInternal = false;
                }
                else
                {
                    IsInternal = true;
                }
                FillSelectedActionLinkedItems(_ActionPlanItem.ActionsLinkedItems);
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
                    SubRowHeight = "49";
                    //TextHeight = "Auto";
                    SubRowIndex = 4;//4
                    RowHeight = "35";
                    RowTagHeight = "70";//chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
                    RowDesHeight = "90";
                    //TagsMargine = "0 5 0 0";

                }
                else //if (SelectedType.IdLookupValue == 271)
                {
                    IsScopeVisible = Visibility.Visible;
                    IsReporterVisible = Visibility.Visible;
                    IsResponsibleVisible = Visibility.Visible;
                    IsDueDateVisible = Visibility.Visible;
                    IsStatusVisible = Visibility.Visible;
                    RowHeight = "35";
                    SubRowHeight = "35";//35
                    //TextHeight = "25";
                    SubRowIndex = 6;//5
                  //  TagsMargine = "0 1 0 0";
                    RowTagHeight = "70";//chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
                    RowDesHeight = "90";
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
                                //[rdixit][GEOS2-4682][08-08-2023] Service GetSelectedUserCompanyGroup updated with GetSelectedUserCompanyGroup_V2420
                                CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup_V2420(salesOwnersIds, true).ToList().OrderBy(cust => cust.IdCustomer));
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
                        //CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true).ToList().OrderBy(cust => cust.IdCustomer));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        //[19.10.2023][GEOS2-4903][rdixit] Service GetCompanyGroup_V2420 updated with GetCompanyGroup_V2450
                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup_V2450(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true).ToList().OrderBy(cust => cust.IdCustomer));
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
                            // EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                            //[pramod.misal][GEOS2-4682][08-08-2023]
                            EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser_V2420(salesOwnersIds, true));
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
                        //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
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
                //ReporterList = new ObservableCollection<User>(CrmStartUp.GetSalesAndCommericalUsers());


                //[pramod.misal][GEOS2-4690]][29-09-2023]
                          
                if (IsEdit)
                {
                    ObservableCollection<User> temp = new ObservableCollection<User>(CrmStartUp.GetSalesAndCommericalUsers_V2440());

                    ObservableCollection<User> tempwithIsEnbledOnly = new ObservableCollection<User>(temp.Where(x => x.IsEnabled == 1));

              
                    ObservableCollection<User> tempDataInIsEnabledList = new ObservableCollection<User>(tempwithIsEnbledOnly.Where(a => a.IdUser == _ActionPlanItem.IdReporter));

                    if (tempDataInIsEnabledList.Count == 0)
                    {
                        var temp1 = temp.Where(x => x.IdUser == _ActionPlanItem.IdReporter);
                        ReporterList = tempwithIsEnbledOnly;
                        ReporterList.AddRange(temp1);
                        SelectedReporter = ReporterList.IndexOf(ReporterList.FirstOrDefault(x => x.IdUser == _ActionPlanItem.IdReporter));
                    }
                    else
                    {                      
                        ReporterList = tempDataInIsEnabledList;
                        ReporterList = tempwithIsEnbledOnly;
                        SelectedReporter = ReporterList.IndexOf(ReporterList.FirstOrDefault(x => x.IdUser == _ActionPlanItem.IdReporter));
                    }
                }

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
                if (string.IsNullOrEmpty(_ActionPlanItem.Group) && !string.IsNullOrEmpty(CompanyGroupList[SelectedIndexCompanyGroup].CustomerName))
                {
                    ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogGroup").ToString(), "None", CompanyGroupList[SelectedIndexCompanyGroup].CustomerName), IdLogEntryType = 258 });
                }
                else
                {
                    ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogGroup").ToString(), _ActionPlanItem.Group, CompanyGroupList[SelectedIndexCompanyGroup].CustomerName), IdLogEntryType = 258 });
                }
            }

            

            // for plant
            if (_ActionPlanItem.IdSite != CompanyPlantList[SelectedIndexCompanyPlant].IdCompany)
            {
                if (string.IsNullOrEmpty(_ActionPlanItem.Plant) && !string.IsNullOrEmpty(CompanyPlantList[SelectedIndexCompanyPlant].Name))
                {
                    ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogPlant").ToString(), "None", CompanyPlantList[SelectedIndexCompanyPlant].Name), IdLogEntryType = 258 });
                }
                else
                {
                    ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogPlant").ToString(), _ActionPlanItem.Plant, CompanyPlantList[SelectedIndexCompanyPlant].Name), IdLogEntryType = 258 });
                }
            }

            //for reporter
            if (_ActionPlanItem.IdReporter != ReporterList[SelectedReporter].IdUser)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogReporter").ToString(), _ActionPlanItem.Reporter, ReporterList[SelectedReporter].FullName), IdLogEntryType = 258 });
            }

            // for responsible
            if (_ActionPlanItem.Assignee != null && _ActionPlanItem.IdAssignee != ResponsibleList[SelectedResponsibleListIndex].IdPerson)
            {
                if (string.IsNullOrEmpty(_ActionPlanItem.Assignee) && !string.IsNullOrEmpty(ResponsibleList[SelectedResponsibleListIndex].FullName))
                {
                    ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogResponsible").ToString(), "None", ResponsibleList[SelectedResponsibleListIndex].FullName), IdLogEntryType = 258 });
                }
                else
                {
                    ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogResponsible").ToString(), _ActionPlanItem.Assignee, ResponsibleList[SelectedResponsibleListIndex].FullName), IdLogEntryType = 258 });
                }
            }

            // for type
            if (_ActionPlanItem.IdSalesActivityType != SalesActivityTypeList[SelectedIndexSalesActivityType].IdLookupValue)
            {
                ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogType").ToString(), _ActionPlanItem.SalesActivityType.Value, SalesActivityTypeList[SelectedIndexSalesActivityType].Value), IdLogEntryType = 258 });
            }

            // For IsInternal
            if (_ActionPlanItem.IsInternal != null && !_ActionPlanItem.IsInternal.Equals(Convert.ToByte(IsInternal)))
            {
                if (Convert.ToByte(IsInternal) == 1)
                {
                    ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogInternal").ToString()), IdLogEntryType = 258 });
                }
                else
                {
                    ChangedLogsEntries.Add(new LogEntriesByActionItem() { IdCreator = GeosApplication.Instance.ActiveUser.IdUser, CreationDate = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("EditActionChangeLogNotInternal").ToString()), IdLogEntryType = 258 });
                }
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


        /// <summary>
        /// method for fill TagList
        /// </summary>
        private void FillTagList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTagList ...", category: Category.Info, priority: Priority.Low);
                TagList = new List<Tag>();
                TagList = CrmStartUp.GetAllTags().OrderBy(r => r.Name).ToList();

                //if(_actionPlanItem.IdTag != null)
                //long abc = Convert.ToInt64(_ActionPlanItem.IdTag);
                //SelectedTag.IdTag = TagList.Select(x=> x.IdTag == abc).FirstOrDefault();
                // SelectedTag.Name = TagList.Where(x => x.IdTag == abc).Select(y => y.Name).FirstOrDefault();
                //SelectedTag = TagList.FirstOrDefault(x => x.IdTag == abc);
                SelectedTagList = new List<object>();
                foreach (Tag item in _ActionPlanItem.ActionTag)
                {
                    SelectedTagList.Add(TagList.FirstOrDefault(x => x.IdTag == item.IdTag));
                }
                GeosApplication.Instance.Logger.Log("Method FillTagList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillTagList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillTagList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTagList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddNewTagCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewTagCommandAction ...", category: Category.Info, priority: Priority.Low);

                AddNewTagViewModel addNewTagViewModel = new AddNewTagViewModel();
                AddNewTagView addNewTagView = new AddNewTagView();

                EventHandler handle = delegate { addNewTagView.Close(); };
                addNewTagViewModel.RequestClose += handle;
                addNewTagViewModel.TempTagList = TagList;
                addNewTagView.DataContext = addNewTagViewModel;
                addNewTagView.ShowDialogWindow();

                if (addNewTagViewModel.IsSave)
                {
                    TagList = new List<Tag>(addNewTagViewModel.TempTagList);

                    if (SelectedTagList == null) { SelectedTagList = new List<object>(); }

                    SelectedTagList.Add(TagList[0]);
                    SelectedTagList = new List<object>(SelectedTagList.ToList());
                    //SelectedTag = TagList[0];
                }
                    GeosApplication.Instance.Logger.Log("Method AddNewTagCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewTagCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }



        //void CheckAllPermissions()
        //{
        //    if (GeosApplication.Instance.IsPermissionAdminOnly == true)
        //    {
        //        AddTagVisibility = "Visible";
        //    }
        //    else
        //    {
        //        AddTagVisibility = "Hidden";
        //    }
        //}

        //private void FillTagsList()
        //{
        //    try
        //    {
        //        if (_ActionPlanItem.IdOwner != GeosApplication.Instance.ActiveUser.IdUser && GeosApplication.Instance.IsPermissionAuditor == false)
        //        {
        //            SelectedTagList = new List<object>();
        //            foreach (ActivityTag item in _Activity.ActivityTags)
        //            {
        //                SelectedTagList.Add(item.Tag);
        //            }
        //        }
        //        else {
        //            if (!IsPlannedAppointment)
        //            {
        //                GeosApplication.Instance.Logger.Log("Method FillTagList ...", category: Category.Info, priority: Priority.Low);
        //                TagList = new List<Tag>();
        //                if (GeosApplication.Instance.TagList == null)
        //                {
        //                    GeosApplication.Instance.TagList = CrmStartUp.GetAllTags().OrderBy(r => r.Name).ToList();
        //                }
        //                TagList = GeosApplication.Instance.TagList;

        //                SelectedTagList = new List<object>();
        //                foreach (ActivityTag item in _Activity.ActivityTags)
        //                {
        //                    SelectedTagList.Add(TagList.FirstOrDefault(x => x.IdTag == item.IdTag));
        //                }
        //            }
        //            GeosApplication.Instance.Logger.Log("Method FillTagList() executed successfully", category: Category.Info, priority: Priority.Low);
        //        }
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillTagList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillTagList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillTagList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}


        #region chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource byteArrayToImage(byte[] byteArrayIn)
        {
            ImageSource imgSrc = null;
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(byteArrayIn);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                biImg.DecodePixelHeight = 10;
                biImg.DecodePixelWidth = 10;

                imgSrc = biImg as ImageSource;

                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return imgSrc;
        }
        private void FillPersonImageByIdPerson(ActionsLinkedItems actionsLinkedItems, int IdPerson)
        {
            try
            {

                if (!string.IsNullOrEmpty(actionsLinkedItems.People.ImageText))
                {
                    byte[] imageBytes = Convert.FromBase64String(actionsLinkedItems.People.ImageText);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    actionsLinkedItems.ActionLinkedItemImage = byteArrayToImage(imageBytes);
                }
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (actionsLinkedItems.People.IdPersonGender == 1)
                            actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        else if (actionsLinkedItems.People.IdPersonGender == 2)
                            actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        else if (actionsLinkedItems.People.IdPersonGender == null)
                            actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                    }
                    else
                    {
                        if (actionsLinkedItems.People.IdPersonGender == 1)
                            actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else if (actionsLinkedItems.People.IdPersonGender == 2)
                            actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        else if (actionsLinkedItems.People.IdPersonGender == null)
                            actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FillPersonImageByIdPerson() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FillPersonImageByIdPerson() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }
        public void SearchOffer(object obj)
        {
            if (GeosApplication.Instance.IsPermissionReadOnly)
                IsSearchButtonEnable = false;
            else if (SelectedIndexCompanyGroup == 0)
                IsSearchButtonEnable = false;
            else if (SelectedIndexCompanyGroup > 0)
            {
                if (ListPlantOpportunity == null)
                    ListPlantOpportunity = new ObservableCollection<Offer>();

                FillPlantOpportunityList(true, ListPlantOpportunity.ToList());
                IsSearchButtonEnable = false;
            }
        }

        /// <summary>
        /// Method to fill List of opportunity as per Selected Group for current plant or if Search for all plants
        /// </summary>
        private void FillPlantOpportunityList(bool isSearch, List<Offer> TempPlantOpportunity)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPlantOpportunityList ...", category: Category.Info, priority: Priority.Low);

                if (isSearch)
                {
                    TempPlantOpportunity = new List<Offer>();
                    TempPlantOpportunity = ListPlantOpportunity.ToList();
                }
                else
                {
                    TempPlantOpportunity = new List<Offer>();
                }

                ListPlantOpportunity = new ObservableCollection<Offer>();
                if (!IsUserWatcherReadOnly)
                {
                    if (isSearch)
                    {
                        foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList.Where(i => i.Alias != GeosApplication.Instance.SiteName).ToList())
                        {
                            List<Int64> idOffers = new List<long>(); ;
                            if (TempPlantOpportunity != null && TempPlantOpportunity.Count > 0)
                                idOffers = TempPlantOpportunity.Where(i => i.Site.ConnectPlantId == itemCompaniesDetails.ConnectPlantId).Select(i => i.IdOffer).ToList();

                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemCompaniesDetails.Alias;
                                TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2490(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer).ToList().Where(i => !idOffers.Contains(i.IdOffer)).ToList());
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }

                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    }
                    else
                    {
                        List<Int64> idOffers = new List<long>();
                        if (TempPlantOpportunity != null && TempPlantOpportunity.Count > 0)
                            idOffers = TempPlantOpportunity.Where(i => i.Site.ConnectPlantId == GeosApplication.Instance.CompanyList.Where(j => j.Alias == GeosApplication.Instance.SiteName).FirstOrDefault().ConnectPlantId).Select(i => i.IdOffer).ToList();

                        TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2490(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(i => i.Alias == GeosApplication.Instance.SiteName).FirstOrDefault(), GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer).ToList().Where(i => !idOffers.Contains(i.IdOffer)).ToList());
                    }
                }


                ListPlantOpportunity = new ObservableCollection<Offer>(TempPlantOpportunity);
                if (ListPlantOpportunity != null && ListPlantOpportunity.Count > 0)
                {
                    foreach (ActionsLinkedItems item in SelectedActionLinkedItems.Where(i => i.IdLinkedItemType == 44))
                    {
                        if (ListPlantOpportunity.Any(x => x.IdOffer == item.IdOffer && Convert.ToInt16(x.Site.ConnectPlantId) == item.IdEmdepSite))
                            ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == item.IdOffer && Convert.ToInt16(x.Site.ConnectPlantId) == item.IdEmdepSite));
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillPlantOpportunityList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for adding group and plant as a linked item
        /// </summary>
        private void AddGroupPlantAsLinkedItem(object objPlant, object objGroup)
        {

            GeosApplication.Instance.Logger.Log("Method AddGroupPlantAsLinkedItem ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!IsUserWatcherReadOnly)
                {
                    IsBusy = true;
                    if (objPlant is Company)
                    {
                        Company company = objPlant as Company;
                        Customer Group = objGroup as Customer;


                        if (!SelectedActionLinkedItems.Any(x => x.LinkedItemType.IdLookupValue == 42))
                        {
                            if (!SelectedActionLinkedItems.Any(x => x.IdSite == company.IdCompany && x.LinkedItemType.IdLookupValue == 42))
                            {
                                ActionsLinkedItems actionLinkedItem = new ActionsLinkedItems();
                                actionLinkedItem.IdSite = company.IdCompany;
                                actionLinkedItem.Name = Group.CustomerName + " - " + company.Name;

                                actionLinkedItem.Company = company;
                                actionLinkedItem.Customer = Group;
                                //activityLinkedItem.Company.Name = company.SiteNameWithoutCountry;
                                actionLinkedItem.IdLinkedItemType = 42;
                                actionLinkedItem.LinkedItemType = new LookupValue();
                                actionLinkedItem.LinkedItemType.IdLookupValue = 42;
                                actionLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                                //On init edit activity don't add address from group and plant forcefully. 

                                if ((SelectedActionType.IdLookupValue == 37 || SelectedActionType.IdLookupValue == 96) && !isInit)   // Appointment And PlannedAppointment
                                {
                                    //// Set account location to activity location
                                    CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                                    customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                                    customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();

                                    if (string.IsNullOrEmpty(ActionsAddress))
                                    {

                                        ActionsAddress = company.Address;

                                        if (company.Latitude != null && company.Longitude != null)
                                        {
                                            customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude = Convert.ToDouble(company.Latitude);
                                            customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude = Convert.ToDouble(company.Longitude);

                                            customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

                                            MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

                                            Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                                            Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                                        }
                                        else
                                        {
                                            MapLatitudeAndLongitude = null;
                                            Latitude = null;
                                            Longitude = null;
                                        }
                                    }
                                    else
                                    {
                                        if (!isInit)
                                        {
                                            if (!string.IsNullOrEmpty(company.Address))
                                            {
                                                //if (!ActivityAddress.Contains(company.Address))
                                                if (ActionsAddress != company.Address)
                                                {
                                                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["CustomerEditViewChangeLocationDetails"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                                    if (MessageBoxResult == MessageBoxResult.Yes)
                                                    {

                                                        ActionsAddress = company.Address;

                                                        if (company.Latitude != null && company.Longitude != null)
                                                        {
                                                            customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude = Convert.ToDouble(company.Latitude);
                                                            customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude = Convert.ToDouble(company.Longitude);
                                                            customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

                                                            MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

                                                            Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                                                            Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                                                        }
                                                        else
                                                        {
                                                            MapLatitudeAndLongitude = null;
                                                            Latitude = null;
                                                            Longitude = null;
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }

                                SelectedActionLinkedItems.Insert(0, actionLinkedItem);
                                SelectedAccountActionLinkedItemsCount = 1;
                                // CompaniesList.Remove(CompaniesList.FirstOrDefault(x => x.IdCompany == company.IdCompany));

                                try
                                {
                                    byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(Group.CustomerName);
                                    if (bytes != null)
                                    {
                                        actionLinkedItem.ActionLinkedItemImage = ByteArrayToBitmapImage(bytes);
                                    }
                                    else
                                    {
                                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                            actionLinkedItem.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wAccount.png");
                                        else
                                            actionLinkedItem.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueAccount.png");
                                    }
                                }
                                catch (FaultException<ServiceException> ex)
                                {
                                    IsBusy = false;
                                    GeosApplication.Instance.Logger.Log("Get an error in AddGroupPlantAsLinkedItem() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                }
                                catch (ServiceUnexceptedException ex)
                                {
                                    IsBusy = false;
                                    GeosApplication.Instance.Logger.Log("Get an error in AddGroupPlantAsLinkedItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                                }
                            }
                        }
                        else
                        {
                            if (!IsEditedFromOutSide)
                            {
                                DeleteLinkedItem();
                                Company plantObj = new Company();
                                plantObj = companyPlantList[SelectedIndexCompanyPlant];
                                Customer groupObj = new Customer();
                                groupObj = CompanyGroupList[SelectedIndexCompanyGroup];
                                AddGroupPlantAsLinkedItem(plantObj, groupObj);
                            }
                        }
                    }
                }
                //if (GeosApplication.Instance.IsPermissionAuditor == true)
                //{
                //    GetPlannedActivitiesOfOwnerForSelectedIndex();
                //}
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method AddGroupPlantAsLinkedItem() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddGroupPlantAsLinkedItem() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to delete linkeditem item on seleted Index changed.
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteLinkedItem()
        {
            GeosApplication.Instance.Logger.Log("Method DeleteLinkedItem ...", category: Category.Info, priority: Priority.Low);

            try
            {

                SelectedActionLinkedItems.Remove(SelectedActionLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42));
                SelectedAccountActionLinkedItemsCount = 0;
                SelectedActionLinkedItems.Where(l => l.IdLinkedItemType == 44 && l.IsVisible == true).ToList().All(i => SelectedActionLinkedItems.Remove(i));

                SelectedActionLinkedItems.Where(l => l.IdLinkedItemType == 43 && l.IsVisible == true).ToList().All(i => SelectedActionLinkedItems.Remove(i));

                if (string.IsNullOrEmpty(ActionsAddress) && !isInit)
                {
                    ActionsAddress = string.Empty;
                    CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                    customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                    customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();
                    MapLatitudeAndLongitude = null;
                    Latitude = null;
                    Longitude = null;
                }

                GeosApplication.Instance.Logger.Log("Method DeleteLinkedItem() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteLinkedItem() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillSelectedActionLinkedItems(List<ActionsLinkedItems> ActionsLinkedItems)
        {
            Offer activityLinkedItemoffferDetail = new Offer();
            GeosApplication.Instance.Logger.Log("Method FillSelectedActivityLinkedItems ...", category: Category.Info, priority: Priority.Low);
            SelectedActionLinkedItems = new ObservableCollection<ActionsLinkedItems>();
            ActionsLinkedItems = ActionsLinkedItems.OrderBy(x => x.IdLinkedItemType).ToList();

            foreach (ActionsLinkedItems actionLinkedItem in ActionsLinkedItems)
            {
                try
                {
                    if (actionLinkedItem.IdLinkedItemType == 44)     // Opportunity
                    {
                        // to check order or opportunity
                        try
                        {
                            ///TODO: Changed by Ravi to fixed the bug on 19 June
                            string conn = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == actionLinkedItem.IdEmdepSite.Value.ToString()).Select(x => x.ConnectPlantConstr).FirstOrDefault();
                            if (!string.IsNullOrEmpty(conn))
                                activityLinkedItemoffferDetail = CrmStartUp.GetOfferByIdOfferAndEmdepSite(actionLinkedItem.IdOffer.Value, conn);

                            actionLinkedItem.Name = activityLinkedItemoffferDetail.Code;
                            actionLinkedItem.IdOffer = activityLinkedItemoffferDetail.IdOffer;
                            actionLinkedItem.IdEmdepSite = actionLinkedItem.IdEmdepSite.Value;
                            actionLinkedItem.IdLinkedItemType = 44;

                            actionLinkedItem.Offer = new Offer();
                            actionLinkedItem.Offer.Code = activityLinkedItemoffferDetail.Code;
                            actionLinkedItem.Offer.Description = activityLinkedItemoffferDetail.Description;
                            actionLinkedItem.Offer.IdOffer = activityLinkedItemoffferDetail.IdOffer;
                            actionLinkedItem.Offer.Site = new Company();
                            actionLinkedItem.Offer.Site.SiteNameWithoutCountry = activityLinkedItemoffferDetail.Site.SiteNameWithoutCountry;
                            actionLinkedItem.Offer.Site.Name = activityLinkedItemoffferDetail.Site.Name;

                            actionLinkedItem.Offer.Site.ConnectPlantId = actionLinkedItem.IdEmdepSite.Value.ToString();
                            actionLinkedItem.Offer.Site.ConnectPlantConstr = conn;

                            ActionsLinkedItems selectedActivityLinkedItem = (ActionsLinkedItems)actionLinkedItem.Clone();
                            SelectedActionLinkedItems.Add(selectedActivityLinkedItem);

                            if (ListPlantOpportunity != null && ListPlantOpportunity.Count > 0)
                            {
                                if (ListPlantOpportunity.Any(x => x.IdOffer == selectedActivityLinkedItem.IdOffer && Convert.ToInt16(x.Site.ConnectPlantId) == selectedActivityLinkedItem.IdEmdepSite))
                                    ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == selectedActivityLinkedItem.IdOffer && Convert.ToInt16(x.Site.ConnectPlantId) == selectedActivityLinkedItem.IdEmdepSite));
                            }

                            if (activityLinkedItemoffferDetail.Site.Customers.Count > 0)
                            {
                                byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(activityLinkedItemoffferDetail.Site.Customers[0].CustomerName);
                                if (bytes != null)
                                {
                                    selectedActivityLinkedItem.ActionLinkedItemImage = ByteArrayToBitmapImage(bytes);
                                }
                                else
                                {
                                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                        selectedActivityLinkedItem.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wOpportunity.png");
                                    else
                                        selectedActivityLinkedItem.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueOpportunity.png");
                                }
                            }
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            IsBusy = false;
                            GeosApplication.Instance.Logger.Log("Get an error in FillSelectedActivityLinkedItems() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            IsBusy = false;
                            GeosApplication.Instance.Logger.Log("Get an error in FillSelectedActivityLinkedItems() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        }
                        catch (Exception ex)
                        {
                            IsBusy = false;
                            GeosApplication.Instance.Logger.Log("Get an error in FillSelectedActivityLinkedItems() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                catch (FaultException<ServiceException> ex)
                {
                    IsBusy = false;
                    GeosApplication.Instance.Logger.Log("Get an error in FillSelectedActivityLinkedItems() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    IsBusy = false;
                    GeosApplication.Instance.Logger.Log("Get an error in FillSelectedActivityLinkedItems() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    GeosApplication.Instance.Logger.Log("Get an error in FillSelectedActivityLinkedItems() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }

            SelectedAccountActionLinkedItemsCount = SelectedActionLinkedItems.Where(x => x.IdLinkedItemType == 42).Count();
            IsBusy = false;
            GeosApplication.Instance.Logger.Log("Method FillSelectedActivityLinkedItems() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for opportunity double click and add it in Linked items.
        /// </summary>
        /// <param name="Obj"></param>
        private void OfferRowMouseDoubleClickCommandAction(object Obj)
        {
            GeosApplication.Instance.Logger.Log("Method OfferRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (IsPlannedAppointment)
                {
                    IsBusy = true;
                    if (Obj is Offer)
                    {
                        Offer offer = Obj as Offer;

                        ActionsLinkedItems actionsLinkedItems = new ActionsLinkedItems();
                        actionsLinkedItems.Name = offer.Code;

                        actionsLinkedItems.IdOffer = offer.IdOffer;
                        actionsLinkedItems.IdEmdepSite = Convert.ToInt32(offer.Site.ConnectPlantId);
                        actionsLinkedItems.IdLinkedItemType = 44;
                        actionsLinkedItems.LinkedItemType = new LookupValue();
                        actionsLinkedItems.LinkedItemType.IdLookupValue = 44;
                        actionsLinkedItems.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();
                        actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/Opportunity.png");

                        actionsLinkedItems.Offer = new Offer();
                        actionsLinkedItems.Offer.Code = offer.Code;
                        actionsLinkedItems.Offer.Description = offer.Description;
                        actionsLinkedItems.Offer.IdOffer = offer.IdOffer;
                        actionsLinkedItems.Offer.Site = new Company();
                        actionsLinkedItems.Offer.Site.SiteNameWithoutCountry = offer.Site.SiteNameWithoutCountry;
                        actionsLinkedItems.Offer.Site.Name = offer.Site.Name;

                        actionsLinkedItems.Offer.Site.ConnectPlantId = actionsLinkedItems.IdEmdepSite.Value.ToString();
                        // activityLinkedItem.Offer.Site.ConnectPlantConstr = conn;

                        SelectedActionLinkedItems.Add(actionsLinkedItems);
                        ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == offer.IdOffer && x.Site.ConnectPlantId == actionsLinkedItems.IdEmdepSite.Value.ToString()));

                        Customer customer = CompanyGroupList[SelectedIndexCompanyGroup];
                        byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customer.CustomerName);
                        if (bytes != null)
                        {
                            actionsLinkedItems.ActionLinkedItemImage = ByteArrayToBitmapImage(bytes);
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wOpportunity.png");
                            else
                                actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueOpportunity.png");
                        }
                        //   }

                    }
                }
                else if (!IsUserWatcherReadOnly)
                {
                    IsBusy = true;
                    if (Obj is Offer)
                    {
                        Offer offer = Obj as Offer;
                        // if (!SelectedActivityLinkedItems.Any(o => o.LinkedItemType.IdLookupValue == 44))
                        // {
                        ActionsLinkedItems actionsLinkedItems = new ActionsLinkedItems();
                        actionsLinkedItems.Name = offer.Code;
                        actionsLinkedItems.IdOffer = offer.IdOffer;
                        actionsLinkedItems.IdEmdepSite = Convert.ToInt32(offer.Site.ConnectPlantId);
                        actionsLinkedItems.IdLinkedItemType = 44;
                        actionsLinkedItems.LinkedItemType = new LookupValue();
                        actionsLinkedItems.LinkedItemType.IdLookupValue = 44;
                        actionsLinkedItems.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();
                        actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/Opportunity.png");

                        actionsLinkedItems.Offer = new Offer();
                        actionsLinkedItems.Offer.IdOffer = offer.IdOffer;
                        actionsLinkedItems.Offer.Code = offer.Code;
                        actionsLinkedItems.Offer.Description = offer.Description;

                        actionsLinkedItems.Offer.Site = new Company();
                        actionsLinkedItems.Offer.Site.SiteNameWithoutCountry = offer.Site.SiteNameWithoutCountry;
                        actionsLinkedItems.Offer.Site.Name = offer.Site.Name;


                        actionsLinkedItems.Offer.Site.ConnectPlantId = actionsLinkedItems.IdEmdepSite.Value.ToString();

                        SelectedActionLinkedItems.Add(actionsLinkedItems);
                        ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == offer.IdOffer && x.Site.ConnectPlantId == actionsLinkedItems.IdEmdepSite.Value.ToString()));

                        Customer customer = CompanyGroupList[SelectedIndexCompanyGroup];
                        byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customer.CustomerName);
                        if (bytes != null)
                        {
                            actionsLinkedItems.ActionLinkedItemImage = ByteArrayToBitmapImage(bytes);
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wOpportunity.png");
                            else
                                actionsLinkedItems.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueOpportunity.png");
                        }
                        //   }

                    }
                }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method OfferRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OfferRowMouseDoubleClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OfferRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OfferRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to delete linkeditem from linked items.
        /// </summary>
        /// <param name="obj"></param>
        private void LinkedItemCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method LinkedItemCancelCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (!IsPlannedAppointment)
                {
                    if (!IsUserWatcherReadOnly)
                    {
                        if (obj is ActionsLinkedItems)
                        {
                            ActionsLinkedItems linkedItem = obj as ActionsLinkedItems;
                            if (linkedItem != null && linkedItem.IdLinkedItemType == 44)  // opportunity
                            {
                                ListPlantOpportunity.Add((Offer)SelectedActionLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 44 && x.IdEmdepSite == linkedItem.IdEmdepSite && x.IdOffer == linkedItem.IdOffer).Offer.Clone());
                                SelectedActionLinkedItems.Remove(SelectedActionLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 44 && x.IdEmdepSite == linkedItem.IdEmdepSite && x.IdOffer == linkedItem.IdOffer));
                                FillPlantOpportunityList(false, new List<Offer>());
                            }
                        }
                    }

                }

                GeosApplication.Instance.Logger.Log("Method LinkedItemCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LinkedItemCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// LinkedItemCheckCommandAction
        /// </summary>
        /// <param name="obj"></param>
        private void LinkedItemDoubleClickCommandAction(object obj)
        {
            ActionsLinkedItems linkedItem = obj as ActionsLinkedItems;

            if (!IsUserWatcherReadOnly)
            {
                if (linkedItem != null)
                {
                    #region Opportunity
                    if (linkedItem.IdLinkedItemType == 44)     // Opportunity
                    {
                        if (linkedItem.IsVisible)
                        {
                            try
                            {
                                GeosApplication.Instance.Logger.Log("Method LeadsEditViewWindowShow...", category: Category.Info, priority: Priority.Low);

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


                                long offerId = Convert.ToInt32(linkedItem.IdOffer);
                                Int32 ConnectPlantId = Convert.ToInt32(linkedItem.IdEmdepSite);
                                try
                                {
                                    IsPODone = CrmStartUp.IsPurchaseOrderDoneByIdOffer(offerId, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).FirstOrDefault());
                                }
                                catch (FaultException<ServiceException> ex)
                                {
                                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                    {
                                        DXSplashScreen.Close();
                                    }
                                    GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                }
                                catch (ServiceUnexceptedException ex)
                                {
                                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                    {
                                        DXSplashScreen.Close();
                                    }
                                    GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                                }
                                catch (Exception ex)
                                {
                                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                    {
                                        DXSplashScreen.Close();
                                    }
                                    GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }

                                IList<Offer> TempLeadsList = new List<Offer>();
                                ActiveSite offerActiveSite = new ActiveSite();
                                if (GeosApplication.Instance.CompanyList.Any(cl => cl.IdCompany.ToString() == ConnectPlantId.ToString()))
                                {
                                    Company company = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).FirstOrDefault();
                                    offerActiveSite = new ActiveSite { IdSite = company.IdCompany, SiteAlias = company.Alias, SiteServiceProvider = company.ServiceProviderUrl };
                                }

                                ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(offerActiveSite.SiteServiceProvider);
                                //[001] added Change Method
                                TempLeadsList.Add(CrmStartUpOfferActiveSite.GetOfferDetailsById_V2090(offerId, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, offerActiveSite));

                                LostReasonsByOffer lostReasonsByOffer = CrmStartUpOfferActiveSite.GetLostReasonsByOffer(offerId, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).Select(x => x.ConnectPlantConstr).FirstOrDefault());

                                if (lostReasonsByOffer != null)
                                {
                                    TempLeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                                }

                                if (IsPODone)           // For open Order
                                {
                                    OrderEditViewModel orderEditViewModel = new OrderEditViewModel();
                                    OrderEditView orderEditView = new OrderEditView();
                                    orderEditViewModel.IsControlEnable = true;
                                    orderEditViewModel.IsControlEnableorder = false;
                                    orderEditViewModel.IsStatusChangeAction = true;
                                    orderEditViewModel.IsAcceptControlEnableorder = false;
                                    orderEditViewModel.InItOrder(TempLeadsList);

                                    EventHandler handle = delegate { orderEditView.Close(); };
                                    orderEditViewModel.RequestClose += handle;
                                    orderEditView.DataContext = orderEditViewModel;

                                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                    {
                                        DXSplashScreen.Close();
                                    }

                                    orderEditView.ShowDialogWindow();
                                }
                                else        // For open Opportunity
                                {
                                    LeadsEditViewModel leadsEditViewModel = new LeadsEditViewModel();
                                    LeadsEditView leadsEditView = new LeadsEditView();


                                    if (GeosApplication.Instance.IsPermissionReadOnly)
                                    {
                                        leadsEditViewModel.IsControlEnable = true;
                                        //leadsEditViewModel.IsControlEnableorder = false;
                                        leadsEditViewModel.IsStatusChangeAction = true;
                                        leadsEditViewModel.IsAcceptControlEnableorder = false;

                                        //[CRM-M040-02] (#49016) Validate Eng. Analysis 
                                        //If user has engineering permission then he can edit eng analysis IsCompleted not other fields and save
                                        leadsEditViewModel.IsAcceptEnable = false;
                                        if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27))
                                        {
                                            leadsEditViewModel.IsAcceptEnable = true;
                                        }

                                        leadsEditViewModel.InItLeadsEditReadonly(TempLeadsList);
                                    }
                                    else
                                    {
                                        leadsEditViewModel.ForLeadOpen = true;

                                        leadsEditViewModel.InIt(TempLeadsList);
                                    }

                                    EventHandler handle = delegate { leadsEditView.Close(); };
                                    leadsEditViewModel.RequestClose += handle;
                                    leadsEditView.DataContext = leadsEditViewModel;

                                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                    {
                                        DXSplashScreen.Close();
                                    }

                                    leadsEditView.ShowDialogWindow();
                                    if (leadsEditViewModel.OfferData != null)
                                    {

                                    }
                                }
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                {
                                    DXSplashScreen.Close();
                                }
                                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                {
                                    DXSplashScreen.Close();
                                }
                                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                            }
                            catch (Exception ex)
                            {
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                {
                                    DXSplashScreen.Close();
                                }
                                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        private void FillActionDetailsForPlannedAppointment()
        {
            if (_ActionPlanItem.ActionsLinkedItems.Any(i => i.IdLinkedItemType == 42))
            {
                CompanyGroupList = new ObservableCollection<Customer>();
                CompanyGroupList.Insert(0, new Customer() { IdCustomer = 0, CustomerName = "---" });
                CompanyGroupList.Add(_ActionPlanItem.ActionsLinkedItems.Where(i => i.Customer.IdCustomer != 0).Select(i => i.Customer).FirstOrDefault());

                SelectedIndexCompanyGroup = 1;

                CompanyPlantList = new ObservableCollection<Company>();
                CompanyPlantList.Insert(0, new Company() { IdCompany = 0, Name = "---" });
                Company tempCompany = new Company();
                tempCompany = _ActionPlanItem.ActionsLinkedItems.Where(i => i.Company.IdCompany != 0).Select(i => i.Company).FirstOrDefault();
                if (tempCompany != null)
                {
                    tempCompany.Name = tempCompany.SiteNameWithoutCountry;
                    tempCompany.SiteNameWithoutCountry = tempCompany.Name;
                    CompanyPlantList.Add(tempCompany);
                }

                SelectedIndexCompanyPlant = 1;
            }

        }
        #endregion
        #region chitra.girigosavi[20/03/2024][GEOS2-3804][ACTIONS_REVIEW] Display “ACTIONS” in the “Orders"
        /// <summary>
        /// Method for fill action Owner list.
        /// </summary>
        private void FillOwnerList(Int32 idActionOwner)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOwnerList ...", category: Category.Info, priority: Priority.Low);

                User user = new User();
                user = WorkbenchStartUp.GetUserById(idActionOwner);

                if (user.IsEnabled == 1)
                {
                    ActionOwnerList = new List<User>();
                    ActionOwnerList.Add(user);
                }
                //Owner = user.FullName;

                UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);

                if (UserProfileImageByte != null)
                {
                    user.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                }
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (user.IdUserGender == 1)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        else if (user.IdUserGender == 2)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        else if (user.IdUserGender == null)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else if (user.IdUserGender == 2)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        else if (user.IdUserGender == null)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillOwnerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOwnerList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOwnerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOwnerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #endregion

        #region Validations

        bool allowValidation = false;
        private ActionPlanItem objActionPlanItem;
        private bool isSave;
        private bool isInternal;
        private Visibility isAccountVisible;
        private List<object> selectedTagList;

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
                    me[BindableBase.GetPropertyName(() => SelectedIndexOwner)] +
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
                     // me[BindableBase.GetPropertyName(() => Description)] +
                     me[BindableBase.GetPropertyName(() => EditInformationError)] +
                     me[BindableBase.GetPropertyName(() => Description)] +
                    me[BindableBase.GetPropertyName(() => SelectedAccountActionLinkedItemsCount)]; //chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”

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
                string selectedIndexOwnerProp = BindableBase.GetPropertyName(() => SelectedIndexOwner);

                // string ActionProp = BindableBase.GetPropertyName(() => Action);
                string SelectedResponsibleListIndexProp = BindableBase.GetPropertyName(() => SelectedResponsibleListIndex);

                string SelectedStatusListIndexProp = BindableBase.GetPropertyName(() => SelectedStatusListIndex);

                //string CustomerCityProp = BindableBase.GetPropertyName(() => CustomerCity);

                string EditInformationErrorProp = BindableBase.GetPropertyName(() => EditInformationError);
                string selectedAccountActionLinkedItemsCountProp = BindableBase.GetPropertyName(() => SelectedAccountActionLinkedItemsCount); //chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
                string editActionErrorProp = BindableBase.GetPropertyName(() => EditActionError);
                if (columnName == SelectedIndexCompanyGroupProp && IsAccountVisible == Visibility.Visible)
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexCompanyGroupProp, SelectedIndexCompanyGroup);

                if (columnName == SelectedIndexCompanyPlantProp && IsAccountVisible == Visibility.Visible)
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

                if (columnName == SelectedResponsibleListIndexProp )
                    return ActionValidation.GetErrorMessage(SelectedResponsibleListIndexProp, SelectedResponsibleListIndex);

                if (columnName == SelectedStatusListIndexProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedStatusListIndexProp, SelectedStatusListIndex);

                if (columnName == EditInformationErrorProp)
                    return RequiredValidationRule.GetErrorMessage(EditInformationErrorProp, EditInformationError);

                else if (columnName == selectedAccountActionLinkedItemsCountProp && IsInternal == false && IsInformationVisible == Visibility.Collapsed)  //linked items.
                {
                    return ActivityValidation.GetErrorMessage(selectedAccountActionLinkedItemsCountProp, SelectedAccountActionLinkedItemsCount);
                }

                else if (columnName == editActionErrorProp)  //linked items.
                {
                    return ActivityValidation.GetErrorMessage(editActionErrorProp, EditActionError);
                }
                else if (columnName == selectedIndexOwnerProp)      //Owner
                {
                    return ActivityValidation.GetErrorMessage(selectedIndexOwnerProp, SelectedIndexOwner);
                }
                return null;
            }
        }
        #endregion
    }
}
