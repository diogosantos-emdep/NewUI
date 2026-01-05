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
using DevExpress.Xpf.Map;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddNewActionsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        #region Tasks
        #endregion

        #region Service
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Decleration
        private Visibility isScopeVisible;
        private Visibility isReporterVisible;
        private Visibility isResponsibleVisible;
        private Visibility isDueDateVisible;
        private Visibility isStatusVisible;
        private string rowHeight = "35";
        private string rowTagHeight = "70";//80
        private string subRowHeight = "50";//35
        private Int32 subRowIndex = 6;
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
        private bool isInternal;
        private Visibility isAccountVisible;
        private List<Tag> tagList;
        private Tag selectedTag;
        public List<Tag> newlyAddedTagList;
        //private string tagsMargine = "0 -7 0 0";
        private List<Object> selectedTagList = new List<object>();
        private string tagsString;

        #region Linked Items chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:
        private ObservableCollection<ActionsLinkedItems> selectedActionLinkedItems = new ObservableCollection<ActionsLinkedItems>();
        private LookupValue selectedActionsType;
        private string actionsAddress;
        private GeoPoint mapLatitudeAndLongitude;
        private bool isCoordinatesNull;
        private double? latitude;
        private double? longitude;
        private ObservableCollection<People> linkeditemsPeopleList;
        private List<People> linkeditemsPeopleListCopy;
        private int selectedAccountActionLinkedItemsCount;
        private ObservableCollection<Offer> listPlantOpportunity;
        private ObservableCollection<People> attendiesList;
        private ObservableCollection<People> selectedattendiesList = new ObservableCollection<People>();
        private ObservableCollection<Offer> listPlantOpportunityCopy;
        private ObservableCollection<Offer> listPlantOpportunityContain;
        private bool isSearchButtonEnable = true;
        private bool isInternalEnable;
        private bool isAddedFromOutSide = false;
        #endregion
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
        public ICommand AddTagButtonCommand { get; set; }

        //Linked items
        public ICommand LinkedItemCancelCommand { get; set; }
        public ICommand OfferRowMouseDoubleClickCommand { get; set; }
        public ICommand LinkedItemDoubleClickCommand { get; set; }
        public ICommand SearchOfferCommand { get; set; }
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
        public String RowTagHeight  //[rgadhave][GEOS2-3801][17.01.2024]
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
            set
            {
                selectedIndexCompanyPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                #region chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:
                if (SelectedIndexCompanyPlant > 0)
                {

                    Company plantObj = new Company();
                    plantObj = CompanyPlantList[SelectedIndexCompanyPlant];
                    Customer groupObj = new Customer();
                    groupObj = CompanyGroupList[SelectedIndexCompanyGroup];
                    //AddGroupPlantAsLinkedItem(plantObj, groupObj);
                    //  FillPlantOpportunityList();


                }
                else
                {
                    if (SelectedIndexCompanyPlant != -1)
                        DeleteLinkedItem();

                }
                #endregion
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
                        CompanyPlantList = new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList().GroupBy(cpl => cpl.IdCompany).Select(group => group.First()).ToList());
                        SelectedIndexCompanyPlant = 0;
                        List<Offer> currentPlantOpportunityList = new List<Offer>();
                        FillPlantOpportunityList(false, currentPlantOpportunityList);
                    }
                    else
                    {
                        selectedIndexCompanyPlant = -1;
                        CompanyPlantList = null;
                        DeleteLinkedItem();
                        ListPlantOpportunity = null;
                    }
                }

                if (GeosApplication.Instance.IsPermissionReadOnly)
                    IsSearchButtonEnable = false;
                else if (SelectedIndexCompanyGroup == 0)
                    IsSearchButtonEnable = false;
                else if (SelectedIndexCompanyGroup > 0)
                    IsSearchButtonEnable = true;
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
                        FillResponsiblesList();
                        if (SelectedActionLinkedItems != null && SelectedActionLinkedItems.Count > 0)
                        {
                            if (SelectedActionLinkedItems[0].IdLinkedItemType == 42)     // Account
                            {
                                if (SelectedActionLinkedItems[0].IsVisible == true)
                                {
                                    SelectedActionLinkedItems.Remove(SelectedActionLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42));
                                    SelectedIndexCompanyGroup = 0;
                                    SelectedIndexCompanyPlant = 0;
                                    ActionsAddress = string.Empty;

                                    CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                                    customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                                    customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();
                                    MapLatitudeAndLongitude = null;
                                    Latitude = null;
                                    Longitude = null;

                                    //Remove contacts related to account.
                                    LinkeditemsPeopleList = new ObservableCollection<People>();
                                    LinkeditemsPeopleListCopy = new List<People>();
                                    SelectedActionLinkedItems.Where(l => l.IdLinkedItemType == 43 && l.IsVisible == true).ToList().All(i => SelectedActionLinkedItems.Remove(i));
                                }
                            }
                        }
                        // IsResponsibleVisible = Visibility.Collapsed;
                    }
                    else
                    {
                        IsAccountVisible = Visibility.Visible;
                        FillResponsiblesList();
                        //IsResponsibleVisible = Visibility.Visible;
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

        public List<Tag> TagList
        {
            get { return tagList; }
            set
            {
                tagList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TagList"));
            }
        }

        public Tag SelectedTag
        {
            get { return selectedTag; }
            set
            {
                selectedTag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTag"));
            }
        }

        public List<Tag> NewlyAddedTagList
        {
            get { return newlyAddedTagList; }
            set { newlyAddedTagList = value; OnPropertyChanged(new PropertyChangedEventArgs("NewlyAddedTagList")); }
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

        public List<Object> SelectedTagList
        {
            get { return selectedTagList; }
            set
            {
                selectedTagList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTagList"));
            }
        }

        public string TagsString
        {
            get
            {
                return tagsString;
            }

            set
            {
                tagsString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("tagsString"));
            }
        }
        //Linked Items
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

        public ObservableCollection<People> LinkeditemsPeopleList
        {
            get { return linkeditemsPeopleList; }
            set
            {
                linkeditemsPeopleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkeditemsPeopleList"));
            }
        }

        public List<People> LinkeditemsPeopleListCopy
        {
            get { return linkeditemsPeopleListCopy; }
            set
            {
                linkeditemsPeopleListCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkeditemsPeopleListCopy"));
            }
        }

        public LookupValue SelectedActionsType
        {
            get { return selectedActionsType; }
            set
            {
                selectedActionsType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivityType"));
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
        public ObservableCollection<People> AttendiesList
        {
            get { return attendiesList; }
            set
            {
                attendiesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendiesList"));
            }
        }
        private List<People> AttendiesListCopy { get; set; }
        public ObservableCollection<People> SelectedAttendiesList
        {
            get { return selectedattendiesList; }
            set
            {
                selectedattendiesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttendiesList"));
            }
        }
        public ObservableCollection<Offer> ListPlantOpportunityCopy
        {
            get
            {
                return listPlantOpportunityCopy;
            }

            set
            {
                listPlantOpportunityCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListPlantOpportunityCopy"));
            }
        }
        public ObservableCollection<Offer> ListPlantOpportunityContain
        {
            get
            {
                return listPlantOpportunityContain;
            }

            set
            {
                listPlantOpportunityContain = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListPlantOpportunityContain"));
            }
        }
        public bool IsSearchButtonEnable
        {
            get { return isSearchButtonEnable; }
            set
            {
                isSearchButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSearchButtonEnable"));
            }
        }
        public bool IsInternalEnable
        {
            get { return isInternalEnable; }
            set { isInternalEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsInternalEnable")); }
        }
        public bool IsAddedFromOutSide
        {
            get
            {
                return isAddedFromOutSide;
            }

            set
            {
                isAddedFromOutSide = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddedFromOutSide"));
            }
        }
        //End Linked Items
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



                if (columnName == SelectedIndexCompanyGroupProp && IsAccountVisible == Visibility.Visible)
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexCompanyGroupProp, SelectedIndexCompanyGroup);

                if (columnName == SelectedIndexCompanyPlantProp && IsAccountVisible == Visibility.Visible)
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
                AddTagButtonCommand = new DelegateCommand<object>(AddNewTagCommandAction);
                //Linked items chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:
                OfferRowMouseDoubleClickCommand = new DelegateCommand<object>(OfferRowMouseDoubleClickCommandAction);
                LinkedItemCancelCommand = new DelegateCommand<object>(LinkedItemCancelCommandAction);
                LinkedItemDoubleClickCommand = new DelegateCommand<object>(LinkedItemDoubleClickCommandAction);
                SearchOfferCommand = new DelegateCommand<object>(SearchOffer);
                FillGroupList();
                FillCompanyPlantList();
                FillResponsiblesList();
                FillScopeList();
                FillTypeList();
                FillStatusList();
                FillReporterList();
                DueDate = DateTime.Today;
                IsInternal = false;
                //FillLinkedItems(); //chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:
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
                //[rdixit][GEOS2-4699][28.08.2023]
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 87) && up.Permission.IdGeosModule == 5))
                    IsAddTagsPermissionCRM = true;
                else
                    IsAddTagsPermissionCRM = false;
                FillTagList();

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
        /// [003][GEOS2-2293][avpawar][22-11-2020][Define Internal Actions]
        /// [004][GEOS2-2976][cpatil][15-04-2022][Completed tasks in AP Must be NOT displayed In RED]  
        /// [005][GEOS2-3760][cpatil][17-05-2022]
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
                    List<LogEntriesByActionItem> logEntriesByActionItem = new List<LogEntriesByActionItem>(); //chitra.girigosavi GEOS2-3799[ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:
                    ActionPlanItem = new ActionPlanItem();

                    ActionPlanItem.IdActionPlan = 1;
                    // ActionPlanItem.IdGroup = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;

                    // [003] Start
                    if (IsInternal == true)
                    {
                        ActionPlanItem.IdGroup = 0;
                        ActionPlanItem.Group = "";
                        ActionPlanItem.IdSite = 0;
                        ActionPlanItem.Plant = "";
                        ActionPlanItem.IdAssignee = ResponsibleList[SelectedResponsibleListIndex].IdPerson;
                        ActionPlanItem.Assignee = ResponsibleList[SelectedResponsibleListIndex].FullName;
                    }
                    else
                    {
                        ActionPlanItem.IdGroup = CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer;
                        ActionPlanItem.Group = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName;
                        ActionPlanItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                        ActionPlanItem.Plant = CompanyPlantList[SelectedIndexCompanyPlant].SiteNameWithoutCountry;
                        ActionPlanItem.IdAssignee = ResponsibleList[SelectedResponsibleListIndex].IdPerson;
                        ActionPlanItem.Assignee = ResponsibleList[SelectedResponsibleListIndex].FullName;
                    }
                    // [003] End

                    //ActionPlanItem.IdSite = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
                    ActionPlanItem.IdScope = ScopeList[SelectedScopeListIndex].IdLookupValue;
                    ActionPlanItem.IdStatus = StatusList[SelectedStatusListIndex].IdLookupValue;
                    //[004] [005]
                       ActionPlanItem.IdReporter = ReporterList[SelectedReporter].IdUser;
                        ActionPlanItem.Reporter = ReporterList[SelectedReporter].FullName;
                    // ActionPlanItem.IdAssignee = ResponsibleList[SelectedResponsibleListIndex].IdPerson;
                    // ActionPlanItem.Group = CompanyGroupList[SelectedIndexCompanyGroup].CustomerName;
                    // ActionPlanItem.Plant = CompanyPlantList[SelectedIndexCompanyPlant].SiteNameWithoutCountry;
                    ActionPlanItem.IdSalesActivityType = SalesActivityTypeList[SelectedIndexSalesActivityType].IdLookupValue;
                    ActionPlanItem.SalesActivityType = SalesActivityTypeList[SelectedIndexSalesActivityType];
                    ActionPlanItem.Title = Subject;
                    ActionPlanItem.Scope = ScopeList[SelectedScopeListIndex].Value;
                
                    // ActionPlanItem.Assignee = ResponsibleList[SelectedResponsibleListIndex].FullName;
                    ActionPlanItem.Status = StatusList[SelectedStatusListIndex];
                    ActionPlanItem.CurrentDueDate = DueDate;
                    ActionPlanItem.ExpectedDueDate = DueDate;
                    ActionPlanItem.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                    ActionPlanItem.OpenDate = GeosApplication.Instance.ServerDateTime.Date;

                    ActionPlanItem.TransactionOperation = ModelBase.TransactionOperations.Add;
                    ActionPlanItem.IsInternal = Convert.ToByte(IsInternal);
                    ActionPlanItem.LstResponsible = ResponsibleList.ToList();//[003] Added

                    #region // For Add  //[rgadhave][GEOS2-3801][17.01.2024]
                    List<Tag> listTag = new List<Tag>();
                    List<string> lstTag = new List<string>();
                    if (SelectedTagList != null && SelectedTagList.Count > 0)
                    {
                        foreach (Tag item in SelectedTagList)
                        {
                            Tag objTag = new Tag();
                            objTag.IdTag = item.IdTag;
                            if (item.IdTag == 0)            // if new tag is added it assign new tag id
                            {
                                try
                                {
                                    //Service IsExistTagName() Changed with IsExistTagName_V2350() by [GEOS2-4120][rdixit][10.01.2022]
                                    bool isExist = CrmStartUp.IsExistTagName_V2350(item.Name.Trim());
                                    if (!isExist)
                                    {
                                        item.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                        Tag tagActivity = CrmStartUp.AddTag(item);
                                        objTag.IdTag = tagActivity.IdTag;
                                        if (selectedTag.Name == item.Name)
                                        {
                                            selectedTag.IdTag = tagActivity.IdTag;
                                        }
                                        //GeosApplication.Instance.TagList.Add(tagActivity);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log(ex.Message, category: Category.Exception, priority: Priority.Low);
                                }
                            }
                            listTag.Add(objTag);
                            //ActionPlanItem.IdTag= Convert.ToUInt64(objTag.IdTag);


                            lstTag.Add(((Tag)item).Name.ToString());

                        }
                        ActionPlanItem.ActionTag = listTag;
                    }
                    #endregion

                    //if (SelectedTag != null)
                    //{
                    //    ActionPlanItem.IdTag = Convert.ToUInt64(SelectedTag.IdTag);
                    //    ActionPlanItem.Tag = SelectedTag; //TagList[SelectedTag].IdTag;
                    //}
                    //else
                    //{
                    //    ActionPlanItem.IdTag = null;
                    //}

                    #region chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:

                    //Linked Items - Add linked item to list.
                    List<ActionsLinkedItems> listActionsLinkedItems = new List<ActionsLinkedItems>();
                    foreach (ActionsLinkedItems item in SelectedActionLinkedItems)
                    {
                        listActionsLinkedItems.Add((ActionsLinkedItems)item.Clone());
                        // item.ActivityLinkedItemImage = LinkedItemImage;

                        if (item.IdLinkedItemType == 44)   // Opportunity
                        {
                            if (item.LinkedItemType.Value == "Order")
                            {
                                logEntriesByActionItem.Add(new LogEntriesByActionItem() { IdActionPlanItem = ActionPlanItem.IdActionPlanItem, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOrder").ToString(), item.Name), IdLogEntryType = 258 });
                            }
                            else
                            {
                                logEntriesByActionItem.Add(new LogEntriesByActionItem() { IdActionPlanItem = ActionPlanItem.IdActionPlanItem, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comment = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOpportunity").ToString(), item.Name), IdLogEntryType = 258 });
                            }

                        }
                    }

                    foreach (ActionsLinkedItems item in listActionsLinkedItems)
                    {
                        item.ActionLinkedItemImage = null;
                        if (item.IdLinkedItemType == 43 && item.People != null)
                            item.People.OwnerImage = null;

                    }
                    ActionPlanItem.ActionsLinkedItems = listActionsLinkedItems;
                    //End Linked Items.

                    #endregion

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
                    //ActionPlanItem = CrmStartUp.AddActionPlanItem_V2380(ActionPlanItem); //service AddActionPlanItem_V2120 updated with AddActionPlanItem_V2380 by [rdixit][GEOS2-4372][27.04.2023]

                    //ActionPlanItem = CrmStartUp.AddActionPlanItem_V2480(ActionPlanItem);     //[rgadhave][GEOS2-3801][17.01.2024]
                    ActionPlanItem = CrmStartUp.AddActionPlanItem_V2500(ActionPlanItem); //chitra.girigosavi GEOS2-3799[ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:
                    ActionPlan ac = CRMCommon.Instance.GetActionPlan();
                    ActionPlanItem.ActionPlan = new ActionPlan();
                    ActionPlanItem.ActionPlan.Code = string.Format("{0} - {1}", ac.Code, ActionPlanItem.Number);
                    IsSave = true;
                    // var ownerInfo = (obj as FrameworkElement);
                    //addIdentificationDocumentView.Owner = Window.GetWindow(ownerInfo);
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
                            //[rdixit][GEOS2-4682][08-08-2023] Service GetSelectedUserCompanyGroup updated with GetSelectedUserCompanyGroup_V2420
                            CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup_V2420(salesOwnersIds, true));
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

                        //CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        //[19.10.2023][GEOS2-4903][rdixit] Service GetCompanyGroup_V2420 updated with GetCompanyGroup_V2450
                        CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup_V2450(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
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
                            //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                            //[pramod.misal][GEOS2-4682][08-08-2023]
                            EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser_V2420(salesOwnersIds, true));
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
                        // EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
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

                //ReporterList = new ObservableCollection<User>(CrmStartUp.GetSalesAndCommericalUsers());

                //[pramod.misal][GEOS2-4690]][29-09-2023]               
                ObservableCollection<User> temp = new ObservableCollection<User>(CrmStartUp.GetSalesAndCommericalUsers_V2440());
                ReporterList = new ObservableCollection<User>(temp.Where(x => x.IsEnabled == 1));             
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
                    RowHeight = "35";
                    SubRowHeight = "70"; //70
                    TextHeight = "Auto";
                    SubRowIndex = 4;
                    RowTagHeight = "70";//[rgadhave][GEOS2-3801][17.01.2024]
                    //TagsMargine = "0 5 0 0";
                }
                else //if (SelectedType.IdLookupValue == 271)
                {
                    IsScopeVisible = Visibility.Visible;
                    IsReporterVisible = Visibility.Visible;
                    IsResponsibleVisible = Visibility.Visible;
                    //if(IsInternal == true)
                    //{
                    //    IsResponsibleVisible = Visibility.Collapsed;
                    //}
                    //else
                    //{
                    //    IsResponsibleVisible = Visibility.Visible;
                    //}
                    IsDueDateVisible = Visibility.Visible;
                    IsStatusVisible = Visibility.Visible;
                    RowHeight = "35";
                    SubRowHeight = "35"; //35
                    TextHeight = "25";
                    SubRowIndex = 6;
                    RowTagHeight = "70";//[rgadhave][GEOS2-3801][17.01.2024]
                    //TagsMargine = "0 5 0 0";
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
                    List<People> responsibles = new List<People>();
                  
                    if (IsInternal)
                    {
                        responsibles = new List<People>(CrmStartUp.GetActionPlanItemResponsible_V2590("0"));
                    }
                    else
                    {
                        responsibles = new List<People>(CrmStartUp.GetActionPlanItemResponsible_V2590(SelectedPlant.IdCompany.ToString()));
                    }
                    People people = new People();
                    people.Name = "---";
                    responsibles.Insert(0, people);
                    ResponsibleList = new ObservableCollection<People>(responsibles);
                }
                else
                {
                    List<People> responsibles = new List<People>(CrmStartUp.GetActionPlanItemResponsible(0));
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

        /// <summary>
        /// Method for Add new tag
        /// </summary>
        /// <param name="obj"></param>
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
                }
                GeosApplication.Instance.Logger.Log("Method AddNewTagCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewTagCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #region Linked Items methods chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:

        public void Init(List<ActionPlanItem> ActionList)
        {
            FillSelectedActivityLinkedItems(ActionList[0].ActionsLinkedItems);
        }

        /// <summary>
        /// Method for set account if it come from EditCustomer.
        /// </summary>
        /// <param name="ActivityLinkedItems"></param>
        private void FillSelectedActivityLinkedItems(List<ActionsLinkedItems> ActionLinkedItems)
        {
            GeosApplication.Instance.Logger.Log("Method FillSelectedActivityLinkedItems ...", category: Category.Info, priority: Priority.Low);
            SelectedActionLinkedItems = new ObservableCollection<ActionsLinkedItems>();
            foreach (ActionsLinkedItems actionLinkedItem in ActionLinkedItems)
            {
                try
                {
                    if (actionLinkedItem.IdLinkedItemType == 44)     // Opportunity
                    {

                        //ListPlantOpportunity = new ObservableCollection<Offer>();

                        //foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                        //{
                        //    ListPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdSiteToLinkedWithActivities(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyPlantList[selectedIndexCompanyPlant].IdCompany));
                        //}
                        //ListPlantOpportunityCopy = new ObservableCollection<Offer>();
                        //foreach (Offer plantOffer in ListPlantOpportunity)
                        //{
                        //    ListPlantOpportunityCopy.Add((Offer)plantOffer.Clone());
                        //}

                        ActionsLinkedItems selectedActionLinkedItem = (ActionsLinkedItems)actionLinkedItem.Clone();
                        SelectedActionLinkedItems.Add(selectedActionLinkedItem);
                        ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == selectedActionLinkedItem.IdOffer && Convert.ToInt16(x.Site.ConnectPlantId) == selectedActionLinkedItem.IdEmdepSite));

                        if (actionLinkedItem.Company != null && actionLinkedItem.Company.Customers.Count > 0)
                        {
                            byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(actionLinkedItem.Company.Customers[0].CustomerName);
                            if (bytes != null)
                            {
                                selectedActionLinkedItem.ActionLinkedItemImage = ByteArrayToBitmapImage(bytes);
                            }
                            else
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                    selectedActionLinkedItem.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wOpportunity.png");
                                else
                                    selectedActionLinkedItem.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueOpportunity.png");
                            }
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
                //if (!IsUserWatcherReadOnly)
                //{

                if (isSearch)
                {
                    foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList.Where(i => i.Alias != GeosApplication.Instance.SiteName).ToList())
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemCompaniesDetails.Alias;
                            TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2490(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer));
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
                    TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2490(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(i => i.Alias == GeosApplication.Instance.SiteName).FirstOrDefault(), GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer));
                }

                if (SelectedActionLinkedItems != null && SelectedActionLinkedItems.Any(i => i.IdLinkedItemType == 44))
                {
                    List<Int64> idsOffers = new List<Int64>(SelectedActionLinkedItems.Where(i => i.IdLinkedItemType == 44).Select(i => i.IdOffer.Value).ToList());
                    TempPlantOpportunity = TempPlantOpportunity.Where(i => !idsOffers.Contains(i.IdOffer)).ToList();
                }

                //}
                ListPlantOpportunity = new ObservableCollection<Offer>(TempPlantOpportunity);

                ListPlantOpportunityCopy = new ObservableCollection<Offer>();
                foreach (Offer plantOffer in ListPlantOpportunity)
                {
                    ListPlantOpportunityCopy.Add((Offer)plantOffer.Clone());
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

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        /// <summary>
        /// Method for opportunity double click and add it in Linked items.
        /// </summary>
        /// <param name="Obj"></param>
        private void OfferRowMouseDoubleClickCommandAction(object Obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OfferRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;


                if (Obj is Offer)
                {
                    Offer offer = Obj as Offer;
                    // if (!SelectedActivityLinkedItems.Any(o => o.LinkedItemType.IdLookupValue == 44))
                    // {
                    ActionsLinkedItems actionLinkedItem = new ActionsLinkedItems();
                    actionLinkedItem.Name = offer.Code;

                    actionLinkedItem.IdOffer = offer.IdOffer;
                    actionLinkedItem.IdEmdepSite = Convert.ToInt32(offer.Site.ConnectPlantId);
                    actionLinkedItem.IdLinkedItemType = 44;
                    actionLinkedItem.LinkedItemType = new LookupValue();
                    actionLinkedItem.LinkedItemType.IdLookupValue = 44;

                    actionLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();
                    //activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/Opportunity.png");
                    SelectedActionLinkedItems.Add(actionLinkedItem);
                    ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == offer.IdOffer));

                    Customer customer = CompanyGroupList[SelectedIndexCompanyGroup];

                    //if (activityLinkedItemoffferDetail.Site.Customers.Count > 0)
                    //{
                    byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customer.CustomerName);
                    if (bytes != null)
                    {
                        actionLinkedItem.ActionLinkedItemImage = ByteArrayToBitmapImage(bytes);
                    }
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            actionLinkedItem.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wOpportunity.png");
                        //activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueOpportunity.png");
                        else
                            actionLinkedItem.ActionLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueOpportunity.png");
                    }
                    //}
                    //   }

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
        /// Method for adding group and plant as a linked item
        /// </summary>
        private void AddGroupPlantAsLinkedItem(object objPlant, object objGroup)
        {

            GeosApplication.Instance.Logger.Log("Method AddGroupPlantAsLinkedItem ...", category: Category.Info, priority: Priority.Low);

            try
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

                            actionLinkedItem.IdLinkedItemType = 42;
                            actionLinkedItem.LinkedItemType = new LookupValue();
                            actionLinkedItem.LinkedItemType.IdLookupValue = 42;
                            actionLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                            // Set account location to action location
                            CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                            customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                            customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();
                            if (SelectedActionsType != null && (SelectedActionsType.IdLookupValue == 37 || SelectedActionsType.IdLookupValue == 96))   // Appointment,PlannedAppointment
                            {
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
                                    if (!string.IsNullOrEmpty(company.Address))
                                    {
                                        if (!ActionsAddress.Contains(company.Address))
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
                                    // activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueAccount.png");
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
                        try
                        {
                            foreach (People item in LinkeditemsPeopleList)
                            {
                                LinkeditemsPeopleListCopy.Add((People)item.Clone());
                            }

                            //Attendees
                            AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030(Convert.ToString(company.IdCompany)).ToList());
                            //Make one main copy.
                            AttendiesListCopy = new List<People>();
                            foreach (People people in AttendiesList)
                            {
                                AttendiesListCopy.Add((People)people.Clone());
                            }

                            //remove attendee if not related to account.
                            for (int i = SelectedAttendiesList.Count - 1; i > -1; i--)
                            {
                                if (!AttendiesList.Any(x => x.IdPerson == SelectedAttendiesList[i].IdPerson))
                                {
                                    //If grid attendies list does not contain idperson then remove it from selected attendies list. 
                                    SelectedAttendiesList.RemoveAt(i);
                                }
                                else
                                {
                                    //If selected attendies list contains idperson then remove it from attendies grid.
                                    AttendiesList.Remove(AttendiesList.FirstOrDefault(x => x.IdPerson == SelectedAttendiesList[i].IdPerson));
                                }
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
                    else
                    {
                        DeleteLinkedItem();
                        Company plantObj = new Company();
                        plantObj = CompanyPlantList[SelectedIndexCompanyPlant];
                        Customer groupObj = new Customer();
                        groupObj = CompanyGroupList[SelectedIndexCompanyGroup];
                        AddGroupPlantAsLinkedItem(plantObj, groupObj);
                    }
                }

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
                if (SelectedActionLinkedItems != null && SelectedActionLinkedItems.Count > 0)
                {
                    SelectedActionLinkedItems.Remove(SelectedActionLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42));
                    SelectedAccountActionLinkedItemsCount = 0;

                    //Remove Opportunity if Plant selected Index set 0 or group selected index set 0
                    //ListPlantOpportunity = new ObservableCollection<Offer>();
                    //ListPlantOpportunityCopy = new ObservableCollection<Offer>();
                    SelectedActionLinkedItems.Where(l => l.IdLinkedItemType == 44 && l.IsVisible == true).ToList().All(i => SelectedActionLinkedItems.Remove(i));

                    //Remove contacts if account is removed.
                    LinkeditemsPeopleList = new ObservableCollection<People>();
                    LinkeditemsPeopleListCopy = new List<People>();
                    SelectedActionLinkedItems.Where(l => l.IdLinkedItemType == 43 && l.IsVisible == true).ToList().All(i => SelectedActionLinkedItems.Remove(i));

                    ///Remove attendees related to account
                    AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030("0").ToList());
                    //Create copy of new list.
                    AttendiesListCopy = new List<People>();
                    foreach (People people in AttendiesList)
                    {
                        AttendiesListCopy.Add((People)people.Clone());
                    }

                    //remove attendee if not related to account.
                    for (int i = SelectedAttendiesList.Count - 1; i > -1; i--)
                    {
                        if (!AttendiesList.Any(x => x.IdPerson == SelectedAttendiesList[i].IdPerson))
                        {
                            SelectedAttendiesList.RemoveAt(i);
                        }
                        else
                        {
                            AttendiesList.Remove(AttendiesList.FirstOrDefault(x => x.IdPerson == SelectedAttendiesList[i].IdPerson));
                        }
                    }
                    // SelectedIndexCompanyGroup = 0;
                    //SelectedIndexCompanyPlant = 0;
                    ActionsAddress = string.Empty;

                    CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                    customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                    customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();
                    MapLatitudeAndLongitude = null;
                    Latitude = null;
                    Longitude = null;
                    //ListPlantOpportunity = null;
                }

                GeosApplication.Instance.Logger.Log("Method DeleteLinkedItem() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteLinkedItem() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                if (obj is ActionsLinkedItems)
                {
                    ActionsLinkedItems linkedItem = obj as ActionsLinkedItems;

                    if (linkedItem.IdLinkedItemType == 44) // Opportunity
                    {
                        SelectedActionLinkedItems.Remove(SelectedActionLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 44 && x.IdEmdepSite == linkedItem.IdEmdepSite && x.IdOffer == linkedItem.IdOffer));
                        ListPlantOpportunity.Add((Offer)ListPlantOpportunityCopy.FirstOrDefault(x => x.IdOffer == linkedItem.IdOffer).Clone());
                    }
                }
                GeosApplication.Instance.Logger.Log("Method LinkedItemCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LinkedItemCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void LinkedItemDoubleClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LinkedItemDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

                ActionsLinkedItems linkedItem = obj as ActionsLinkedItems;
                //if (!IsUserWatcherReadOnly)
                //{
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

                                IList<Offer> TempLeadsList = new List<Offer>();
                                ActiveSite offerActiveSite = new ActiveSite();
                                if (GeosApplication.Instance.CompanyList.Any(cl => cl.IdCompany.ToString() == ConnectPlantId.ToString()))
                                {
                                    Company company = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).FirstOrDefault();
                                    offerActiveSite = new ActiveSite { IdSite = company.IdCompany, SiteAlias = company.Alias, SiteServiceProvider = company.ServiceProviderUrl };
                                }
                                ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(offerActiveSite.SiteServiceProvider);
                                //[001] added change method
                                TempLeadsList.Add(CrmStartUpOfferActiveSite.GetOfferDetailsById_V2090(offerId, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, offerActiveSite));

                                LostReasonsByOffer lostReasonsByOffer = CrmStartUpOfferActiveSite.GetLostReasonsByOffer(offerId, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).Select(x => x.ConnectPlantConstr).FirstOrDefault());

                                if (lostReasonsByOffer != null)
                                {
                                    TempLeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                                }

                                // For open Opportunity
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
                                ListPlantOpportunityContain = new ObservableCollection<Offer>();

                                foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                                {
                                    //ListPlantOpportunityContain.AddRange(CrmStartUp.GetOffersByIdSiteToLinkedWithActivities(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyPlantList[selectedIndexCompanyPlant].IdCompany));
                                    ListPlantOpportunityContain.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2490(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer));
                                }
                                foreach (var linkedItems in SelectedActionLinkedItems)
                                {
                                    Offer offer = ListPlantOpportunityContain.FirstOrDefault(x => x.Code == linkedItem.Name && linkedItem.LinkedItemType.IdLookupValue == 44);
                                    if (offer == null)
                                    {
                                        SelectedActionLinkedItems.Remove(SelectedActionLinkedItems.FirstOrDefault(x => x.Name == linkedItem.Name && linkedItem.LinkedItemType.IdLookupValue == 44));
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
                //}
                GeosApplication.Instance.Logger.Log("Method LinkedItemDoubleClickCommandAction Executed Successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LinkedItemDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

        #endregion
    }

}
