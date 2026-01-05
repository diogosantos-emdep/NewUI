using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using NetOffice.OutlookApi.Enums;
using Outlook = NetOffice.OutlookApi;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.Diagnostics;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Spreadsheet;
using Microsoft.Win32;
using System.Data;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.CodeView;
using Emdep.Geos.UI.Validations;
using DevExpress.Xpf.Map;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Utility;
using DevExpress.Compression;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Globalization;
using System.Windows.Documents;
using System.Text.RegularExpressions;
//using System.Windows.Forms;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddActivityViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {

        #region TaskLog

        //[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        //[GEOS2-267]Add all people as possible contact belonging to the group in activities [adadibathina]
        //[GEOS2-257](#70081) Different criteria for calculating Sleep days [adadibathina]
        #endregion

        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration
        int windowHeight1;
        private int screenWidth;
        private int screenHeight;
        private int windowHeight;
        private int commentboxHeight;
        private List<UserManagerDtl> activityOwnerList;
        private List<object> selectedActivityOwnerList;
        //byte[] UserProfileImageByte = null;
        //private List<DateTime> _FromDates;
        private DateTime fromDate;
        //private DateTime startTime;
        //private List<DateTime> _ToDates;
        private DateTime toDate;
        //private DateTime endTime;
        private string description;
        private string subject;
        private int selectedIndexType;
        private bool isBusy;
        private GeoPoint mapLatitudeAndLongitude;
        private bool isCoordinatesNull;
        private double? latitude;
        private double? longitude;
        private string activityAddress;
        private bool isActivitySave;
        private Activity newActivity;
        private LookupValue selectedActivityType;
        private Visibility isAppointmentVisible;
        private Visibility isContactVisible;
        
        private Visibility isCompletedVisible;
        private Visibility isTagVisible;
        private Visibility isEmailorCallVisible;
        private Visibility isTaskVisible;
        private Visibility isDueDateVisible;
        private Visibility isWatcherVisible;
        private Visibility isWatcherUserVisible;
        private Visibility isWatcherRegionVisible;
        private Visibility isInformationVisible;
        private Visibility isCommentsVisible;
        private List<Activity> newCreatedActivityList;
        private Int32 columnMainWidth;
        private Int32 columnWidth;
        private ObservableCollection<Customer> companyGroupList;
        private ObservableCollection<Company> companyPlantList;
        private int selectedIndexCompanyGroup;
        private int selectedIndexCompanyPlant;
        private bool isSearchButtonEnable = true;
        private ObservableCollection<Offer> listPlantOpportunity;
        private ObservableCollection<Offer> listPlantOpportunityCopy;
        private ObservableCollection<Offer> listPlantOpportunityContain;

        private string addActivityError;

        //public int OldPlantIndex;
        //private bool isChange = false;
        private bool isAddedFromOutSide = false;

        // Tags
        private List<Tag> tagList;
        private List<Object> selectedTagList = new List<object>();
        private int selectedIndexTag;
        private string addTagVisibility;
        //private int selectedIndexOwner;
        private ObservableCollection<People> attendiesList;
        private ObservableCollection<People> watchersList;
        private ObservableCollection<People> watchersUserList;
        private ObservableCollection<LookupValue> watchersRegionList;
        private ObservableCollection<People> selectedattendiesList = new ObservableCollection<People>();
        private ObservableCollection<People> selectedWatchersList = new ObservableCollection<People>();
        private ObservableCollection<People> selectedWatchersUserList = new ObservableCollection<People>();
        private ObservableCollection<LookupValue> selectedWatchersRegionList = new ObservableCollection<LookupValue>();
        private object attendeesCount;
        private object watchersCount;
        private List<Object> selectedAttendeesItems;

        private DateTime fromDateMinValue;
        private DateTime toDateMinValue;
        //private DateTime toDateWithTime;
        //private DateTime fromDateWithTime;
        private bool isAllDayEvent;
        private bool invertIsAllDayEvent;

        private bool isCompleted;
        private DateTime? dueDate;
        private int selectedIndexTaskStatus;

        //private ObservableCollection<Company> companiesList;
        //private List<Company> companiesListCopy;
        private ObservableCollection<CarProject> carProjectList;
        private List<CarProject> carProjectListCopy;
        private ObservableCollection<People> linkeditemsPeopleList;
        private List<People> linkeditemsPeopleListCopy;
        private ObservableCollection<Competitor> linkeditemsCompetitorList;
        private List<Competitor> linkeditemsCompetitorListCopy;


        private ObservableCollection<Company> selectedCompaniesList;
        private ObservableCollection<ActivityLinkedItem> selectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>();
        private int selectedAccountActivityLinkedItemsCount;

        private String fileName;
        private List<Object> activityAttachmentList = new List<object>();
        //private List<Object> tempActivityAttachmentList;
        private List<Object> attachmentGridList = new List<object>();
        private ObservableCollection<ActivityAttachment> listAttachment = new ObservableCollection<ActivityAttachment>();
        private ObservableCollection<ActivityAttachment> tempListAttachment = new ObservableCollection<ActivityAttachment>();
        private string uniqueFileName;
        private string fileNameString;
        private ActivityAttachment attachment;

        //Comments
        private ObservableCollection<LogEntriesByActivity> activityCommentsList = new ObservableCollection<LogEntriesByActivity>();
        private Object selectedComment;
        private bool showCommentsFlyout;
        private string oldActivityComment;
        private string newActivityComment;
        private string commentButtonText;
        private string timeEditMask;
        private string tagsString;
        private string attendiesString;
        private bool isInternal;
        private Visibility isInternalVisible;
        private Visibility isAccountVisible;
        private Visibility isOwnerVisible;
        private bool isInternalEnable;
        public int SelectedIndexCompanyGroupAddActivity = 0;

        private bool isRtf;
        private bool isNormal = true;
        private ImageSource userProfileImage;
        byte[] UserProfileImageByte = null;
        private bool isAdd;
        private string commentText;

        private Visibility textboxnormal;
        private Visibility richtextboxrtf;

        private ImageSource linkedItemImage;

        private ObservableCollection<Company> entireCompanyPlantList;


        private string visible;
        #endregion // Declaration

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

        public int WindowHeight1
        {
            get
            {
                return windowHeight1;
            }

            set
            {
                windowHeight1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight1"));
            }
        }
        public string AddActivityError
        {
            get { return addActivityError; }
            set { addActivityError = value; OnPropertyChanged(new PropertyChangedEventArgs("AddActivityError")); }
        }



        public ImageSource LinkedItemImage
        {
            get { return linkedItemImage; }
            set
            {
                linkedItemImage = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("LinkedItemImage"));

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
        public int WindowHeight
        {
            get
            {
                return windowHeight;
            }

            set
            {
                windowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeight"));
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
        public string AddTagVisibility
        {
            get
            {
                return addTagVisibility;
            }

            set
            {
                addTagVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddTagVisibility"));
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
        public ImageSource UserProfileImage
        {
            get { return userProfileImage; }
            set { userProfileImage = value; OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImage")); }
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

        public ObservableCollection<Customer> CompanyGroupList
        {
            get { return companyGroupList; }
            set
            {
                companyGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroupList"));
            }
        }

        public ObservableCollection<Company> CompanyPlantList
        {
            get { return companyPlantList; }
            set
            {
                companyPlantList = value;
                if (value == null || companyPlantList?.Count <= 0)
                    LinkeditemsPeopleList = new ObservableCollection<People>();
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
        public int SelectedIndexCompanyGroup
        {
            get { return selectedIndexCompanyGroup; }
            set
            {
                if (value != SelectedIndexCompanyGroup)
                {
                    selectedIndexCompanyGroup = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));

                    //if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    //{
                    //    //DXSplashScreen.Show(x =>
                    //}

                    if (SelectedIndexCompanyGroup > 0)
                    {

                        //FillCompanyPlantList();


                        CompanyPlantList = new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList().GroupBy(cpl => cpl.IdCompany).Select(group => group.First()).ToList());
                        SelectedIndexCompanyPlant = 0;
                        FillContactsList();
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
                // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
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

        public int SelectedIndexCompanyPlant
        {
            get
            {
                return selectedIndexCompanyPlant;
            }
            set
            {
                selectedIndexCompanyPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                if (SelectedIndexCompanyPlant > 0)
                {

                    Company plantObj = new Company();
                    plantObj = CompanyPlantList[SelectedIndexCompanyPlant];
                    Customer groupObj = new Customer();
                    groupObj = CompanyGroupList[SelectedIndexCompanyGroup];
                    AddGroupPlantAsLinkedItem(plantObj, groupObj);
                    //  FillPlantOpportunityList();


                }
                else
                {
                    if (SelectedIndexCompanyPlant != -1)
                        DeleteLinkedItem();

                }

            }
        }
        public bool IsInternalEnable
        {
            get { return isInternalEnable; }
            set { isInternalEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsInternalEnable")); }
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


        public Visibility IsOwnerVisible
        {
            get
            {
                return isOwnerVisible;
            }

            set
            {
                isOwnerVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOwnerVisible"));
            }
        }


        public Visibility IsInternalVisible
        {
            get
            {
                return isInternalVisible;
            }

            set
            {
                isInternalVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInternalVisible"));
            }
        }


        public Int32 ColumnMainWidth
        {
            get
            {
                return columnMainWidth;
            }

            set
            {
                columnMainWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnMainWidth"));
            }
        }


        public Int32 ColumnWidth
        {
            get
            {
                return columnWidth;
            }

            set
            {
                columnWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnWidth"));
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
                        if (SelectedActivityLinkedItems != null && SelectedActivityLinkedItems.Count > 0)
                        {
                            if (SelectedActivityLinkedItems[0].IdLinkedItemType == 42)     // Account
                            {
                                if (SelectedActivityLinkedItems[0].IsVisible == true)
                                {
                                    //Company comp = CompaniesListCopy.FirstOrDefault(x => x.IdCompany == SelectedActivityLinkedItems[0].IdSite);
                                    //if (comp != null)
                                    //{
                                    //    CompaniesList.Add((Company)comp.Clone());
                                    //}
                                    //else
                                    //{
                                    //    if (SelectedActivityLinkedItems[0].Company != null)
                                    //        CompaniesList.Add(SelectedActivityLinkedItems[0].Company);
                                    //}

                                    SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42));
                                    SelectedIndexCompanyGroup = 0;
                                    SelectedIndexCompanyPlant = 0;
                                    ActivityAddress = string.Empty;

                                    CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                                    customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                                    customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();
                                    MapLatitudeAndLongitude = null;
                                    Latitude = null;
                                    Longitude = null;

                                    //Remove contacts related to account.
                                    LinkeditemsPeopleList = new ObservableCollection<People>();
                                    LinkeditemsPeopleListCopy = new List<People>();
                                    SelectedActivityLinkedItems.Where(l => l.IdLinkedItemType == 43 && l.IsVisible == true).ToList().All(i => SelectedActivityLinkedItems.Remove(i));
                                }
                            }
                            //for (int i = SelectedActivityLinkedItems.Count - 1; i >= 0; i--)
                            //{
                            //    if (SelectedActivityLinkedItems[i].IdLinkedItemType == 43)  // contact
                            //    {
                            //        LinkeditemsPeopleList.Add((People)LinkeditemsPeopleListCopy.FirstOrDefault(x => x.IdPerson == SelectedActivityLinkedItems[i].IdPerson).Clone());
                            //        SelectedActivityLinkedItems.RemoveAt(i);
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        IsAccountVisible = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in IsInternal Property " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
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
        public string AttendiesString
        {
            get
            {
                return attendiesString;
            }

            set
            {
                attendiesString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendiesString"));
            }
        }
        Activity _Activity { get; set; }
        public Activity objActivity { get; set; }

        public string TimeEditMask
        {
            get
            {
                return timeEditMask;
            }

            set
            {
                timeEditMask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeEditMask"));
            }
        }

        //Linked Items
        public ObservableCollection<ActivityLinkedItem> SelectedActivityLinkedItems
        {
            get { return selectedActivityLinkedItems; }
            set
            {
                selectedActivityLinkedItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivityLinkedItems"));
            }
        }

        public int SelectedAccountActivityLinkedItemsCount
        {
            get { return selectedAccountActivityLinkedItemsCount; }
            set
            {
                selectedAccountActivityLinkedItemsCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAccountActivityLinkedItemsCount"));
            }
        }

        public ObservableCollection<Company> SelectedCompaniesList
        {
            get { return selectedCompaniesList; }
            set
            {
                selectedCompaniesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompaniesList"));
            }
        }

        //public ObservableCollection<Company> CompaniesList
        //{
        //    get { return companiesList; }
        //    set
        //    {
        //        companiesList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CompaniesList"));
        //    }
        //}

        //public List<Company> CompaniesListCopy
        //{
        //    get { return companiesListCopy; }
        //    set
        //    {
        //        companiesListCopy = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("CompaniesListCopy"));
        //    }
        //}

        public ObservableCollection<CarProject> CarProjectList
        {
            get { return carProjectList; }
            set
            {
                carProjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarProjectList"));
            }
        }

        public List<CarProject> CarProjectListCopy
        {
            get { return carProjectListCopy; }
            set
            {
                carProjectListCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarProjectListCopy"));
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

        public ObservableCollection<Competitor> LinkeditemsCompetitorList
        {
            get { return linkeditemsCompetitorList; }
            set
            {
                linkeditemsCompetitorList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkeditemsCompetitorList"));
            }
        }

        public List<Competitor> LinkeditemsCompetitorListCopy
        {
            get { return linkeditemsCompetitorListCopy; }
            set
            {
                linkeditemsCompetitorListCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkeditemsCompetitorListCopy"));
            }
        }
        //End Linked Items

        public List<object> SelectedAttendeesItems
        {
            get { return selectedAttendeesItems; }
            set
            {
                selectedAttendeesItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttendeesItems"));
            }
        }

        public object AttendeesCount
        {
            get { return attendeesCount; }
            set
            {
                attendeesCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttendeesCount"));
            }
        }

        public object WatchersCount
        {
            get { return watchersCount; }
            set
            {
                watchersCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WatchersCount"));
            }
        }

        public List<UserManagerDtl> ActivityOwnerList
        {
            get { return activityOwnerList; }
            set
            {
                activityOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityOwnerList"));
            }
        }

        public List<object> SelectedActivityOwnerList
        {
            get { return selectedActivityOwnerList; }
            set
            {
                selectedActivityOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivityOwnerList"));

                if (selectedActivityOwnerList.Count > 0)
                {
                    foreach (UserManagerDtl ownerAttendees in selectedActivityOwnerList)
                    {
                        if (!SelectedAttendiesList.Any(x => x.IdPerson == ownerAttendees.IdUser))
                        {
                            People attendiesFromOwner = AttendiesListCopy.FirstOrDefault(y => y.IdPerson == ownerAttendees.IdUser);
                            if (attendiesFromOwner != null)
                            {
                                SelectedAttendiesList.Add(attendiesFromOwner);
                                AttendiesList.Remove(AttendiesList.FirstOrDefault(x => x.IdPerson == attendiesFromOwner.IdPerson));
                                AttendeesCount = SelectedAttendiesList.Count;
                            }
                        }
                    }
                }
            }
        }

        public bool IsActivitySave
        {
            get { return isActivitySave; }
            set { isActivitySave = value; OnPropertyChanged(new PropertyChangedEventArgs("IsActivitySave")); }
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

        public string ActivityAddress
        {
            get { return activityAddress; }
            set
            {
                if (value != null)
                {
                    activityAddress = value.Trim();
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

        public Activity NewActivity
        {
            get { return newActivity; }
            set
            {
                newActivity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewActivity"));
            }
        }

        public int SelectedIndexType
        {
            get { return selectedIndexType; }
            set
            {
                selectedIndexType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexType"));
            }
        }

        public LookupValue SelectedActivityType
        {
            get { return selectedActivityType; }
            set
            {
                selectedActivityType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivityType"));
            }
        }

        public Visibility IsAppointmentVisible
        {
            get { return isAppointmentVisible; }
            set
            {
                isAppointmentVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAppointmentVisible"));
            }
        }

        public Visibility IsContactVisible
        {
            get { return isContactVisible; }
            set
            {
               isContactVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsContactVisible"));
            }
        }

        public Visibility IsCompletedVisible
        {
            get { return isCompletedVisible; }
            set
            {
                isCompletedVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCompletedVisible"));
            }
        }

        public Visibility IsTagVisible
        {
            get { return isTagVisible; }
            set
            {
                isTagVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTagVisible"));
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

        public Visibility IsCommentsVisible
        {
            get { return isCommentsVisible; }
            set
            {
                isCommentsVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCommentsVisible"));
            }
        }

        public Visibility IsEmailorCallVisible
        {
            get { return isEmailorCallVisible; }
            set
            {
                isEmailorCallVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmailorCallVisible"));
            }
        }

        public Visibility IsTaskVisible
        {
            get { return isTaskVisible; }
            set
            {
                isTaskVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTaskVisible"));
            }
        }

        public Visibility IsWatcherUserVisible
        {
            get { return isWatcherUserVisible; }
            set
            {
                isWatcherUserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWatcherUserVisible"));
            }
        }

        public Visibility IsWatcherRegionVisible
        {
            get { return isWatcherRegionVisible; }
            set
            {
                isWatcherRegionVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWatcherRegionVisible"));
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

        public Visibility IsWatcherVisible
        {
            get { return isWatcherVisible; }
            set
            {
                isWatcherVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWatcherVisible"));
            }
        }


        public List<Activity> NewCreatedActivityList
        {
            get { return newCreatedActivityList; }
            set
            {
                newCreatedActivityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewCreatedActivityList"));
            }
        }

        //public int SelectedIndexOwner
        //{
        //    get { return selectedIndexOwner; }
        //    set
        //    {
        //        selectedIndexOwner = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexOwner"));
        //    }
        //}

        public IList<LookupValue> TypeList { get; set; }

        //public bool IsAllDayEvent
        //{
        //    get { return isAllDayEvent; }
        //    set
        //    {
        //        isAllDayEvent = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsAllDayEvent"));

        //        if (value)
        //        {
        //            InvertIsAllDayEvent = false;
        //            StartTime = FromDates[0];
        //            EndTime = ToDates[0];
        //        }
        //        else
        //        {
        //            InvertIsAllDayEvent = true;
        //        }
        //    }
        //}

        //public bool InvertIsAllDayEvent
        //{
        //    get { return invertIsAllDayEvent; }
        //    set
        //    {
        //        invertIsAllDayEvent = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("InvertIsAllDayEvent"));
        //    }
        //}

        //public List<DateTime> FromDates
        //{
        //    get { return _FromDates; }
        //    set
        //    {
        //        if (value != _FromDates)
        //        {
        //            _FromDates = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("_FromDates"));
        //        }
        //    }
        //}

        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));

                //If ToDate is less than FromDate then assign ToDate same as FromDate.
                //if (ToDate.Date < FromDate.Date)
                //{
                //    DateTime dateTime = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
                //    ToDate = dateTime;
                //}
            }
        }

        public DateTime FromDateMinValue
        {
            get { return fromDateMinValue; }
            set
            {
                fromDateMinValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDateMinValue"));
            }
        }

        //public DateTime StartTime
        //{
        //    get { return startTime; }
        //    set
        //    {
        //        startTime = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("StartTime"));

        //        if (FromDate != null)
        //        {
        //            DateTime dateTime = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
        //            FromDate = dateTime;
        //        }
        //    }
        //}

        //public List<DateTime> ToDates
        //{
        //    get { return _ToDates; }
        //    set
        //    {
        //        if (value != _ToDates)
        //        {
        //            _ToDates = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("_ToDates"));
        //        }
        //    }
        //}

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }

        public DateTime ToDateMinValue
        {
            get { return toDateMinValue; }
            set
            {
                toDateMinValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDateMinValue"));
            }
        }

        //public DateTime EndTime
        //{
        //    get { return endTime; }
        //    set
        //    {
        //        endTime = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("EndTime"));

        //        if (ToDate != null)
        //        {
        //            DateTime dateTime = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
        //            ToDate = dateTime;
        //        }
        //    }
        //}

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

        public List<LookupValue> TaskStatusList { get; set; }

        public int SelectedIndexTaskStatus
        {
            get { return selectedIndexTaskStatus; }
            set
            {
                selectedIndexTaskStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexTaskStatus"));
            }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCompleted"));
            }
        }

        public DateTime? DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DueDate"));
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

        public ObservableCollection<People> WatchersList
        {
            get { return watchersList; }
            set
            {
                watchersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WatchersList"));
            }
        }

        public ObservableCollection<People> WatchersUserList
        {
            get { return watchersUserList; }
            set
            {
                watchersUserList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WatchersUserList"));
            }
        }

        public ObservableCollection<LookupValue> WatchersRegionList
        {
            get { return watchersRegionList; }
            set
            {
                watchersRegionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WatchersRegionList"));
            }
        }
        private List<People> AttendiesListCopy { get; set; }
        private List<People> WatchersListCopy { get; set; }

        private List<People> WatchersUserListCopy { get; set; }
        private List<LookupValue> WatchersRegionListCopy { get; set; }

        public ObservableCollection<People> SelectedAttendiesList
        {
            get { return selectedattendiesList; }
            set
            {
                selectedattendiesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttendiesList"));
                //if (SelectedAttendiesList.Count == 0)
                //{
                //    People defaultAttendies = AttendiesListCopy.FirstOrDefault(y => y.IdPerson == GeosApplication.Instance.ActiveUser.IdUser);
                //    SelectedAttendiesList.Add(defaultAttendies);
                //    AttendiesList.Remove(AttendiesList.FirstOrDefault(x => x.IdPerson == defaultAttendies.IdPerson));
                //    AttendeesCount = SelectedAttendiesList.Count;
                //}
            }
        }

        public ObservableCollection<People> SelectedWatchersList
        {
            get { return selectedWatchersList; }
            set
            {
                selectedWatchersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWatchersList"));
            }
        }

        public ObservableCollection<People> SelectedWatchersUserList
        {
            get { return selectedWatchersUserList; }
            set
            {
                selectedWatchersUserList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWatchersUserList"));
            }
        }

        public ObservableCollection<LookupValue> SelectedWatchersRegionList
        {
            get { return selectedWatchersRegionList; }
            set
            {
                selectedWatchersRegionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWatchersRegionList"));
            }
        }


        public List<object> AttachmentGridList
        {
            get { return attachmentGridList; }
            set
            {
                attachmentGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentGridList"));
            }
        }

        public List<object> ActivityAttachmentList
        {
            get { return activityAttachmentList; }
            set
            {
                activityAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityAttachmentList"));
            }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged(new PropertyChangedEventArgs("FileName")); }
        }

        public string GuidCode { get; set; }

        //Comments
        public ObservableCollection<LogEntriesByActivity> ActivityCommentsList
        {
            get { return activityCommentsList; }
            set
            {
                activityCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityCommentsList"));
            }
        }

        public Object SelectedComment
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

        public string OldActivityComment
        {
            get { return oldActivityComment; }
            set
            {
                oldActivityComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldActivityComment"));
            }
        }

        public string NewActivityComment
        {
            get { return newActivityComment; }
            set
            {
                newActivityComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewActivityComment"));
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

        //private string uniqueFileName;

        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set { uniqueFileName = value; OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName")); }
        }

        //private string fileNameString;

        public string FileNameString
        {
            get { return fileNameString; }
            set { fileNameString = value; OnPropertyChanged(new PropertyChangedEventArgs("FileNameString")); }
        }


        //private ActivityAttachment attachment;

        public ActivityAttachment Attachment
        {
            get { return attachment; }
            set { attachment = value; OnPropertyChanged(new PropertyChangedEventArgs("Attachment")); }
        }

        public ObservableCollection<ActivityAttachment> ListAttachment
        {
            get { return listAttachment; }
            set
            {
                listAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAttachment"));
            }
        }

        public ObservableCollection<ActivityAttachment> TempListAttachment
        {
            get { return tempListAttachment; }
            set
            {
                tempListAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempListAttachment"));
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

        public List<Object> SelectedTagList
        {
            get { return selectedTagList; }
            set
            {
                selectedTagList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTagList"));
            }
        }

        public int SelectedIndexTag
        {
            get { return selectedIndexTag; }
            set
            {
                selectedIndexTag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexTag"));
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
        #endregion // Properties

        #region public Events
        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion // Events

        #region Public Commands

        public ICommand AddActivityViewAcceptButtonCommand { get; set; }
        public ICommand AddActivityViewCancelButtonCommand { get; set; }
        public ICommand SearchButtonClickCommand { get; set; }
        public ICommand AddAttendeeRowMouseDoubleClickCommand { get; set; }
        public ICommand AttendeeCancelCommand { get; set; }
        public ICommand AddWatchersRowMouseDoubleClickCommand { get; set; }

        public ICommand AddWatchersUserRowMouseDoubleClickCommand { get; set; }

        public ICommand AddWatchersRegionRowMouseDoubleClickCommand { get; set; }
        public ICommand WatchersCancelCommand { get; set; }
        public ICommand AddTagButtonCommand { get; set; }

        //Linked items
        public ICommand ProjectRowMouseDoubleClickCommand { get; set; }
        public ICommand ContactRowMouseDoubleClickCommand { get; set; }

        public ICommand CompetitorRowMouseDoubleClickCommand { get; set; }

        //public ICommand AccountRowMouseDoubleClickCommand { get; set; }
        public ICommand LinkedItemCancelCommand { get; set; }

        public ICommand OfferRowMouseDoubleClickCommand { get; set; }
        public ICommand LinkedItemDoubleClickCommand { get; set; }


        //Attachments
        public ICommand ChooseFileCommand { get; set; }
        public ICommand UploadFileCommand { get; set; }
        public ICommand DownloadFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }

        //Comments
        public ICommand TypeSelectedIndexChangedCommand { get; set; }
        public ICommand CommentButtonCheckedCommand { get; set; }
        public ICommand CommentButtonUncheckedCommand { get; set; }
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }

        public ICommand IsnormalPreviewMouseRightButtonDown { get; set; }

        public ICommand SearchOfferCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion // Commands

        #region Constructor

        public AddActivityViewModel()
        {
            CRMCommon.Instance.Init();

            screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 300;
            screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;            
            WindowHeight = screenHeight - 280;
            CommentboxHeight = screenHeight - 480;
            ColumnMainWidth = screenWidth;
            WindowHeight1 = WindowHeight - 100;
            GeosApplication.Instance.Logger.Log("Constructor AddActivityViewModel() ...", category: Category.Info, priority: Priority.Low);
            TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            AddActivityViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AddNewActivityAccept);
            AddActivityViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
            TypeSelectedIndexChangedCommand = new DelegateCommand<object>(TypeSelectedIndexChangedCommandAction);
            AddTagButtonCommand = new DelegateCommand<object>(AddNewTagCommandAction);

            //Linked items.
            ProjectRowMouseDoubleClickCommand = new DelegateCommand<object>(ProjectRowMouseDoubleClickCommandAction);
            ContactRowMouseDoubleClickCommand = new DelegateCommand<object>(ContactRowMouseDoubleClickCommandAction);
            LinkedItemDoubleClickCommand = new DelegateCommand<object>(LinkedItemDoubleClickCommandAction);
            //AccountRowMouseDoubleClickCommand = new DelegateCommand<object>(AccountRowMouseDoubleClickCommandAction);
            OfferRowMouseDoubleClickCommand = new DelegateCommand<object>(OfferRowMouseDoubleClickCommandAction);
            CompetitorRowMouseDoubleClickCommand = new DelegateCommand<object>(CompetitorRowMouseDoubleClickCommandAction);

            LinkedItemCancelCommand = new DelegateCommand<object>(LinkedItemCancelCommandAction);

            SearchButtonClickCommand = new DelegateCommand<object>(new Action<object>((obj) => { SearchButtonClickCommandAction(obj); }));
            AddAttendeeRowMouseDoubleClickCommand = new DelegateCommand<object>(AddAttendeeRowMouseDoubleClickCommandAction);
            AttendeeCancelCommand = new DelegateCommand<object>(AttendeeCancelCommandAction);

            AddWatchersRowMouseDoubleClickCommand = new DelegateCommand<object>(AddWatchersRowMouseDoubleClickCommandAction);
            AddWatchersUserRowMouseDoubleClickCommand = new DelegateCommand<object>(AddWatchersUserRowMouseDoubleClickCommandAction);
            AddWatchersRegionRowMouseDoubleClickCommand = new DelegateCommand<object>(AddWatchersRegionRowMouseDoubleClickCommandAction);
            WatchersCancelCommand = new DelegateCommand<object>(WatchersCancelCommandAction);


            ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFile));
            UploadFileCommand = new DelegateCommand<object>(UploadFileCommandAction);
            DownloadFileCommand = new DelegateCommand<object>(DownloadFileCommandAction);
            DeleteFileCommand = new DelegateCommand<object>(DeleteAttachmentRowCommandAction);

            //Comments
            CommentButtonCheckedCommand = new DelegateCommand<object>(CommentButtonCheckedCommandAction);
            CommentButtonUncheckedCommand = new DelegateCommand<object>(CommentButtonUncheckedCommandAction);
            AddNewCommentCommand = new DelegateCommand<object>(AddCommentCommandAction);
            CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);
            DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);

            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsnormalPreviewMouseRightButtonDown = new DelegateCommand<object>(IsnormalCommandAction);
            SearchOfferCommand = new DelegateCommand<object>(SearchOffer);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

            FromDate = GeosApplication.Instance.ServerDateTime.Date;
            ToDate = GeosApplication.Instance.ServerDateTime.Date;

            FillTypeList();
            FillTagList();
            FillTaskStatusList();
            FillAttendiesList();
            FillWatchersList();
            FillOwnerList();
            FillLinkedItems();
            FillCompanyPlantList();
            FillGroupList();

            // SelectedTagList = new List<object>();
            //SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>();



            //FillFromDates();
            //FillToDates();
            //IsAllDayEvent = true;

            //SelectedAttendiesList = new ObservableCollection<People>();
            AttendeesCount = SelectedAttendiesList.Count;
            //ActivityCommentsList = new ObservableCollectionscreenWidth<LogEntriesByActivity>();

            //ActivityAttachmentList = new List<object>();
            //ListAttachment = new ObservableCollection<ActivityAttachment>();
            //AttachmentGridList = new List<object>();
            //TempListAttachment = new ObservableCollection<ActivityAttachment>();

            if (GeosApplication.Instance.IsPermissionAdminOnly == true)
            {
                AddTagVisibility = "Visible";
            }
            else
            {
                AddTagVisibility = "Hidden";
            }

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


            GeosApplication.Instance.Logger.Log("Constructor AddActivityViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillContactsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContactsList ...", category: Category.Info, priority: Priority.Low);
                string idSites = string.Join(",", CompanyPlantList.Select(x => x.IdCompany));
                LinkeditemsPeopleList = new ObservableCollection<People>(CrmStartUp.GetContactsByLinkedItemAccount_V2030(idSites).ToList().OrderBy(x => x.Company.Name));
                GeosApplication.Instance.Logger.Log("Method FillContactsList() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillContactsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // Constructor

        #region Addins

        public void InitAppointment(string senderEmail) //(Microsoft.Office.Interop.Outlook.MailItem eMailItem)
        {
            CRMCommon.Instance.Init();
            SelectedIndexType = TypeList.IndexOf(TypeList.FirstOrDefault(x => x.IdLookupValue == 37));
            AssignGroupPlant(senderEmail);
        }

        private void AssignGroupPlant(string senderEmail)
        {
            PeopleDetails pd = CrmStartUp.GetGroupPlantByMailId(senderEmail);

            if (pd != null)
            {
                SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(x => x.IdCustomer == pd.IdCustomer));

                if (SelectedIndexCompanyGroup != -1)
                    SelectedIndexCompanyPlant = CompanyPlantList.IndexOf(CompanyPlantList.FirstOrDefault(x => x.IdCompany == pd.IdSite));

                //if (SalesOwnerList.Any(x => x.IdPerson == GeosApplication.Instance.ActiveUser.IdUser))
                //{
                //    SelectedIndexSalesOwner = SalesOwnerList.FindIndex(x => x.IdPerson == GeosApplication.Instance.ActiveUser.IdUser);
                //}
            }
        }

        public void InitEmail(string senderEmail) //(string mailSubject)
        {
            CRMCommon.Instance.Init();
            SelectedIndexType = TypeList.IndexOf(TypeList.FirstOrDefault(x => x.IdLookupValue == 39));
            IsCompleted = true;
            AssignGroupPlant(senderEmail);
        }

        public void InitCall(string senderEmail) //(string mailSubject)
        {
            CRMCommon.Instance.Init();
            SelectedIndexType = TypeList.IndexOf(TypeList.FirstOrDefault(x => x.IdLookupValue == 38));
            AssignGroupPlant(senderEmail);
        }

        public void InitTask(string senderEmail) //(string mailSubject)
        {
            CRMCommon.Instance.Init();
            SelectedIndexType = TypeList.IndexOf(TypeList.FirstOrDefault(x => x.IdLookupValue == 40));
            SelectedIndexTaskStatus = TaskStatusList.IndexOf(TaskStatusList.FirstOrDefault(x => x.IdLookupValue == 48));
            AssignGroupPlant(senderEmail);
        }

        public void AddAttachement(string localMailPath)
        {
            IsBusy = true;
            try
            {
                FileInfo file = new FileInfo(localMailPath);
                FileName = file.FullName;
                var newFileList = ActivityAttachmentList != null ? new List<object>(ActivityAttachmentList) : new List<object>();
                UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                Attachment = new ActivityAttachment();
                Attachment.FileType = file.Extension;
                Attachment.FilePath = file.FullName;
                Attachment.OriginalFileName = file.Name;
                Attachment.SavedFileName = UniqueFileName + file.Extension;
                Attachment.UploadedIn = DateTime.Now;
                Attachment.FileSize = file.Length;
                Attachment.FileType = file.Extension;
                Attachment.FileUploadName = UniqueFileName + file.Extension;
                Attachment.IsDeleted = 0;

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

                //// not allow to add same files
                //List<ActivityAttachment> fooList = newFileList.OfType<ActivityAttachment>().ToList();

                //if (!fooList.Any(x => x.OriginalFileName == Attachment.OriginalFileName))
                //{
                //    newFileList.Add(Attachment);
                //}

                ListAttachment = new ObservableCollection<ActivityAttachment>();
                ListAttachment.Add(Attachment);

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
        }

        #endregion

        #region Methods

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

        private void IsnormalCommandAction(object gcComments)
        {
            string convertedText = string.Empty;
            if (IsNormal)
            {
                var document = ((RichTextBox)gcComments).Document;
                using (MemoryStream ms = new MemoryStream())
                {
                    TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                    range2.Save(ms, DataFormats.Text);
                    ms.Seek(0, SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        convertedText = sr.ReadToEnd();
                    }
                }
                NewActivityComment = convertedText;
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
                        {//1
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemCompaniesDetails.Alias;
                            //TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2150(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer));
                            //[pramod.misal][GEOS2-5347][16.02.2024]
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
                {//2
                    //TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2150(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(i => i.Alias == GeosApplication.Instance.SiteName).FirstOrDefault(), GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer));
                    //[pramod.misal][GEOS2-5347][16.02.2024]
                    TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2490(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(i => i.Alias == GeosApplication.Instance.SiteName).FirstOrDefault(), GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer));
                }


                if (SelectedActivityLinkedItems!=null && SelectedActivityLinkedItems.Any(i=>i.IdLinkedItemType==45))
                {
                    List<Int64> idsCarProject =new List<Int64>(SelectedActivityLinkedItems.Where(i => i.IdLinkedItemType == 45).Select(i => i.IdCarProject.Value).ToList());
                    TempPlantOpportunity = TempPlantOpportunity.Where(i=>i.IdCarProject!=null).ToList().Where(i => idsCarProject.Contains(i.IdCarProject.Value)).ToList();
                }

                if (SelectedActivityLinkedItems != null && SelectedActivityLinkedItems.Any(i => i.IdLinkedItemType == 44))
                {
                    List<Int64> idsOffers = new List<Int64>(SelectedActivityLinkedItems.Where(i => i.IdLinkedItemType == 44).Select(i => i.IdOffer.Value).ToList());
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
        /// Method to fill List of opportunity of selected plant from Plant Combo
        /// </summary>
        //private void FillPlantOpportunityList()
        //{

        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillPlantOpportunityList ...", category: Category.Info, priority: Priority.Low);

        //        ListPlantOpportunity = new ObservableCollection<Offer>();

        //        foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
        //        {
        //            // ListPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdSiteToLinkedWithActivities(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyPlantList[selectedIndexCompanyPlant].IdCompany));
        //            // ListPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdSiteToLinkedWithActivities(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyPlantList[selectedIndexCompanyPlant].IdCompany));
        //            ListPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer));
        //        }

        //        ListPlantOpportunityCopy = new ObservableCollection<Offer>();
        //        foreach (Offer plantOffer in ListPlantOpportunity)
        //        {
        //            ListPlantOpportunityCopy.Add((Offer)plantOffer.Clone());
        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillPlantOpportunityList() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
        //    }

        //}

        /// <summary>
        /// Method for fill emdep Group list.
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                //IList<Customer> TempCompanyGroupList = null;
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

        /// <summary>
        /// Method for fill Company list.
        /// </summary>
        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                // List<Company> TempcompanyPlant = new List<Company>();
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                        //TempcompanyPlant = CrmStartUp.GetSelectedUserCompanyPlantByCustomerId(CompanyGroupList[selectedIndexCompanyGroup].IdCustomer, salesOwnersIds);

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


        /// <summary>
        /// Method for select account and create opportunity.
        /// </summary>
        /// <param name="ActivityList"></param>
        public void Init(List<Activity> ActivityList)
        {
            FillSelectedActivityLinkedItems(ActivityList[0].ActivityLinkedItem);
        }
        public void AddInit()
        {
            if (isAddedFromOutSide)
            {
                SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(x => x.IdCustomer == SelectedIndexCompanyGroupAddActivity);
            }
        }
        /// <summary>
        /// Method for set account if it come from EditCustomer.
        /// </summary>
        /// <param name="ActivityLinkedItems"></param>
        private void FillSelectedActivityLinkedItems(List<ActivityLinkedItem> ActivityLinkedItems)
        {
            GeosApplication.Instance.Logger.Log("Method FillSelectedActivityLinkedItems ...", category: Category.Info, priority: Priority.Low);
            SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>();
            foreach (ActivityLinkedItem activityLinkedItem in ActivityLinkedItems)
            {
                try
                {
                    if (activityLinkedItem.IdLinkedItemType == 42)          // Account
                    {
                        //Account always at first position.
                        ActivityLinkedItem selectedActivityLinkedItem = (ActivityLinkedItem)activityLinkedItem.Clone();
                        SelectedActivityLinkedItems.Insert(0, selectedActivityLinkedItem);

                        // SelectedIndexCompanyGroup = CompanyGroupList.FindIndex(x => x.IdCustomer == activityLinkedItem.IdCustomer);
                        //SelectedIndexCompanyPlant = CompanyPlantList.FindIndex(x => x.IdCompany == activityLinkedItem.IdSite);

                        // to set account location as activity location
                        CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                        customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                        customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();

                        if (!string.IsNullOrEmpty(selectedActivityLinkedItem.Company.Address))
                        {
                            ActivityAddress = selectedActivityLinkedItem.Company.Address;
                            if (selectedActivityLinkedItem.Company.Latitude != null && selectedActivityLinkedItem.Company.Longitude != null)
                            {
                                customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude = Convert.ToDouble(selectedActivityLinkedItem.Company.Latitude);
                                customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude = Convert.ToDouble(selectedActivityLinkedItem.Company.Longitude);
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

                        // CompaniesList.Remove(CompaniesList.FirstOrDefault(x => x.IdCompany == activityLinkedItem.IdSite));
                        if (activityLinkedItem.Company.Customers != null && activityLinkedItem.Company.Customers.Count > 0)
                        {
                            byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(activityLinkedItem.Company.Customers[0].CustomerName);
                            if (bytes != null)
                            {
                                selectedActivityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                            }
                            else
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                    selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wAccount.png");
                                else
                                    selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueAccount.png");
                            }
                        }
                        else if (activityLinkedItem.Customer != null)
                        {
                            byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(activityLinkedItem.Customer.CustomerName);
                            if (bytes != null)
                            {
                                selectedActivityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                            }
                            else
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                    selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wAccount.png");
                                else
                                    selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueAccount.png");
                            }
                        }

                        //LinkeditemsPeopleList = new ObservableCollection<People>(CrmStartUp.GetContactsByLinkedItemAccount_V2030(Convert.ToString(activityLinkedItem.IdSite)).ToList());
                        //LinkeditemsPeopleListCopy = new List<People>();

                        foreach (People item in LinkeditemsPeopleList)
                        {
                            LinkeditemsPeopleListCopy.Add((People)item.Clone());
                        }
                    }
                    else if (activityLinkedItem.IdLinkedItemType == 43)          // Contact
                    {
                        //Account always at first position.
                        if (activityLinkedItem.People.IdPerson > 0)
                        {
                            FillPersonImageByIdPerson(activityLinkedItem, activityLinkedItem.People.IdPerson);
                        }
                        ActivityLinkedItem selectedActivityLinkedItem = (ActivityLinkedItem)activityLinkedItem.Clone();
                        //CompaniesList.Remove(CompaniesList.FirstOrDefault(x => x.IdCompany == activityLinkedItem.IdSite));
                        SelectedActivityLinkedItems.Add(selectedActivityLinkedItem);
                        LinkeditemsPeopleList.Remove(LinkeditemsPeopleList.FirstOrDefault(x => x.IdPerson == selectedActivityLinkedItem.IdPerson));

                        //LinkeditemsPeopleList = new ObservableCollection<People>(CrmStartUp.GetContactsByLinkedItemAccount_V2030(Convert.ToString(activityLinkedItem.IdSite)).ToList());
                        //LinkeditemsPeopleListCopy = new List<People>();

                        //foreach (People item in LinkeditemsPeopleList)
                        //{
                        //    LinkeditemsPeopleListCopy.Add((People)item.Clone());
                        //}
                    }
                    else if (activityLinkedItem.IdLinkedItemType == 44)     // Opportunity
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

                        ActivityLinkedItem selectedActivityLinkedItem = (ActivityLinkedItem)activityLinkedItem.Clone();
                        SelectedActivityLinkedItems.Add(selectedActivityLinkedItem);
                        ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == selectedActivityLinkedItem.IdOffer && Convert.ToInt16(x.Site.ConnectPlantId) == selectedActivityLinkedItem.IdEmdepSite));

                        if (activityLinkedItem.Company != null && activityLinkedItem.Company.Customers.Count > 0)
                        {
                            byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(activityLinkedItem.Company.Customers[0].CustomerName);
                            if (bytes != null)
                            {
                                selectedActivityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                            }
                            else
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                    selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wOpportunity.png");
                                else
                                    selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueOpportunity.png");
                            }
                        }
                    }
                    else if (activityLinkedItem.IdLinkedItemType == 92)     // Competitor
                    {



                        ActivityLinkedItem selectedActivityLinkedItem = (ActivityLinkedItem)activityLinkedItem.Clone();
                        SelectedActivityLinkedItems.Add(selectedActivityLinkedItem);

                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wCompititor.png");
                        else
                            selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueCompititor.png");
                        //ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == selectedActivityLinkedItem.IdOffer && Convert.ToInt16(x.Site.ConnectPlantId) == selectedActivityLinkedItem.IdEmdepSite));

                        //if (activityLinkedItem.Company != null && activityLinkedItem.Company.Customers.Count > 0)
                        //{
                        //    byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(activityLinkedItem.Company.Customers[0].CustomerName);
                        //    if (bytes != null)
                        //    {
                        //        selectedActivityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                        //    }
                        //}
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

            SelectedAccountActivityLinkedItemsCount = SelectedActivityLinkedItems.Where(x => x.IdLinkedItemType == 42).Count();

            IsBusy = false;
            GeosApplication.Instance.Logger.Log("Method FillSelectedActivityLinkedItems() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        ///// <summary>
        ///// Method for fill from date.
        ///// </summary>
        //private void FillFromDates()
        //{
        //    GeosApplication.Instance.Logger.Log("Method FillFromDates ...", category: Category.Info, priority: Priority.Low);

        //    FromDates = new List<DateTime>();

        //    double addMinutes = 0;
        //    for (int i = 0; i < 48; i++)
        //    {
        //        DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
        //        FromDates.Add(otherDate);
        //        addMinutes += 30;
        //    }

        //    StartTime = FromDates[0];

        //    GeosApplication.Instance.Logger.Log("Method FillFromDates() executed successfully", category: Category.Info, priority: Priority.Low);
        //}

        ///// <summary>
        ///// Method for fill to date.
        ///// </summary>
        //private void FillToDates()
        //{
        //    GeosApplication.Instance.Logger.Log("Method FillToDates ...", category: Category.Info, priority: Priority.Low);

        //    ToDates = new List<DateTime>();

        //    double addMinutes = 0;
        //    for (int i = 0; i < 48; i++)
        //    {
        //        DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
        //        ToDates.Add(otherDate);
        //        addMinutes += 30;
        //    }

        //    EndTime = ToDates[0];

        //    GeosApplication.Instance.Logger.Log("Method FillToDates() executed successfully", category: Category.Info, priority: Priority.Low);
        //}

        /// <summary>
        /// This method is used to change visibility of controls as per selection of type.
        /// </summary>
        /// <param name="obj"></param>
        private void TypeSelectedIndexChangedCommandAction(object obj)
        {
            if (SelectedActivityType != null)
            {
                if (SelectedActivityType.IdLookupValue == 37 || SelectedActivityType.IdLookupValue == 96)   // Appointment
                {
                    IsAppointmentVisible = Visibility.Visible;
                    IsContactVisible = Visibility.Collapsed;
                    IsEmailorCallVisible = Visibility.Collapsed;
                    IsTaskVisible = Visibility.Collapsed;
                    IsDueDateVisible = Visibility.Visible;
                    IsInternalVisible = Visibility.Visible;
                    IsOwnerVisible = Visibility.Visible;
                    IsTagVisible = Visibility.Visible;
                    IsWatcherVisible = Visibility.Collapsed;
                    IsCommentsVisible = Visibility.Visible;
                    IsInformationVisible = Visibility.Collapsed;
                    IsCompletedVisible = Visibility.Visible;
                    IsWatcherUserVisible = Visibility.Collapsed;
                    IsWatcherRegionVisible = Visibility.Collapsed;
                    SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>(SelectedActivityLinkedItems.Where(ik => ik.IdLinkedItemType != 92).ToList());
                    ColumnMainWidth = 920;
                    ColumnWidth = 350;
                }
                else if (SelectedActivityType.IdLookupValue == 38 || SelectedActivityType.IdLookupValue == 39)   // Email or Call
                {
                    IsAppointmentVisible = Visibility.Collapsed;
                    IsContactVisible = Visibility.Visible;
                    IsEmailorCallVisible = Visibility.Visible;
                    IsTaskVisible = Visibility.Collapsed;
                    IsDueDateVisible = Visibility.Visible;
                    IsInternalVisible = Visibility.Collapsed;
                    IsOwnerVisible = Visibility.Visible;
                    IsTagVisible = Visibility.Visible;
                    IsWatcherVisible = Visibility.Collapsed;
                    IsCommentsVisible = Visibility.Visible;
                    IsInformationVisible = Visibility.Collapsed;
                    IsCompletedVisible = Visibility.Visible;
                    IsWatcherUserVisible = Visibility.Collapsed;
                    IsWatcherRegionVisible = Visibility.Collapsed;
                    IsInternal = false;
                    ColumnMainWidth = 920;
                    ColumnWidth = 350;
                    SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>(SelectedActivityLinkedItems.Where(ik => ik.IdLinkedItemType != 92).ToList());
                }
                else if (SelectedActivityType.IdLookupValue == 40)  // Task
                {
                    IsAppointmentVisible = Visibility.Collapsed;
                    IsContactVisible = Visibility.Visible;
                    IsEmailorCallVisible = Visibility.Collapsed;
                    IsTaskVisible = Visibility.Visible;
                    IsWatcherVisible = Visibility.Visible;
                    IsDueDateVisible = Visibility.Visible;
                    IsInternalVisible = Visibility.Visible;
                    IsOwnerVisible = Visibility.Visible;
                    IsTagVisible = Visibility.Visible;
                    IsCommentsVisible = Visibility.Visible;
                    IsInformationVisible = Visibility.Collapsed;
                    IsCompletedVisible = Visibility.Collapsed;
                    IsWatcherUserVisible = Visibility.Collapsed;
                    IsWatcherRegionVisible = Visibility.Collapsed;
                    SelectedWatchersUserList.Clear();
                    ColumnMainWidth = 920;
                    ColumnWidth = 350;
                    SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>(SelectedActivityLinkedItems.Where(ik => ik.IdLinkedItemType != 92).ToList());
                }
                else if (SelectedActivityType.IdLookupValue == 91)  // Information
                {
                    IsAppointmentVisible = Visibility.Collapsed;
                    IsEmailorCallVisible = Visibility.Collapsed;
                    IsDueDateVisible = Visibility.Collapsed;
                    IsOwnerVisible = Visibility.Visible;
                    IsTagVisible = Visibility.Collapsed;
                    IsWatcherVisible = Visibility.Visible;
                    IsInternalVisible = Visibility.Visible;
                    IsInformationVisible = Visibility.Visible;
                    IsContactVisible = Visibility.Visible;
                    IsAccountVisible = Visibility.Visible;
                    IsTaskVisible = Visibility.Collapsed;
                    IsCommentsVisible = Visibility.Collapsed;
                    IsCompletedVisible = Visibility.Collapsed;
                    IsWatcherUserVisible = Visibility.Visible;
                    IsWatcherRegionVisible = Visibility.Visible;
                    IsInternal = false;
                    SelectedWatchersUserList.Clear();
                    ColumnMainWidth = 970;
                    ColumnWidth = 460;
                }
            }
        }

        /// <summary>
        /// This Method for Add Attendee
        /// </summary>
        /// <param name="e"></param>
        public void AddAttendeeRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddAttendeeRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (obj is People)
                {
                    People people = obj as People;

                    if (!SelectedAttendiesList.Any(x => x.IdPerson == people.IdPerson))
                    {
                        People selectedPerson = AttendiesListCopy.FirstOrDefault(x => x.IdPerson == people.IdPerson);


                        if (selectedPerson != null)
                        {
                            SelectedAttendiesList.Add(selectedPerson);
                            AttendiesList.Remove(AttendiesList.FirstOrDefault(x => x.IdPerson == people.IdPerson));
                            AttendeesCount = SelectedAttendiesList.Count;

                            //Sprint45 CRM	M045-07	Customer attendees added also as linked items
                            // When adding some attendee from the customer side at th same time add him as a contact in the linked items
                            if (SelectedActivityLinkedItems == null || (SelectedActivityLinkedItems != null && !(SelectedActivityLinkedItems.Any(i => i.IdPerson == selectedPerson.IdPerson && i.IdLinkedItemType == 43))))
                            {
                                if (LinkeditemsPeopleList != null && LinkeditemsPeopleList.Any(x => x.IdPerson == selectedPerson.IdPerson))
                                {
                                    ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();

                                    activityLinkedItem.IdPerson = selectedPerson.IdPerson;
                                    activityLinkedItem.Name = selectedPerson.FullName;

                                    activityLinkedItem.IdLinkedItemType = 43;
                                    activityLinkedItem.LinkedItemType = new LookupValue();
                                    activityLinkedItem.LinkedItemType.IdLookupValue = 43;
                                    activityLinkedItem.LinkedItemType.Value = "Contact";
                                    activityLinkedItem.People = (People)selectedPerson.Clone();

                                    SelectedActivityLinkedItems.Add(activityLinkedItem);

                                    LinkeditemsPeopleList.Remove(LinkeditemsPeopleList.FirstOrDefault(x => x.IdPerson == selectedPerson.IdPerson));

                                    FillPersonImageByIdPerson(activityLinkedItem, selectedPerson.IdPerson);
                                }
                            }
                        }
                    }
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method AddAttendeeRowMouseDoubleClickCommandAction() executed successfully", category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddAttendeeRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method AddAttendeeRowMouseDoubleClickCommandAction() executed successfully", category: Category.Exception, priority: Priority.Low);
        }

        /// <summary>
        /// This Method for Add Watchers.
        /// </summary>
        /// <param name="e"></param>
        public void AddWatchersRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddWatchersRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (obj is People)
                {
                    People people = obj as People;

                    if (!SelectedWatchersList.Any(x => x.IdPerson == people.IdPerson))
                    {
                        People selectedPerson = WatchersListCopy.FirstOrDefault(x => x.IdPerson == people.IdPerson);
                        if (selectedPerson != null)
                        {
                            SelectedWatchersList.Add(selectedPerson);
                            WatchersList.Remove(WatchersList.FirstOrDefault(x => x.IdPerson == people.IdPerson));
                            WatchersCount = SelectedWatchersList.Count;
                        }
                    }
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method AddWatchersRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddWatchersRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method AddWatchersRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void AddWatchersUserRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddWatchersUserRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (obj is People)
                {
                    People people = obj as People;

                    if (!SelectedWatchersList.Any(x => x.IdPerson == people.IdPerson))
                    {
                        People selectedPerson = WatchersListCopy.FirstOrDefault(x => x.IdPerson == people.IdPerson);
                        if (selectedPerson != null)
                        {
                            SelectedWatchersList.Add(selectedPerson);
                            WatchersList.Remove(WatchersList.FirstOrDefault(x => x.IdPerson == people.IdPerson));
                            //WatchersUserList.Remove(WatchersUserList.FirstOrDefault(x => x.IdPerson == people.IdPerson));
                            WatchersCount = SelectedWatchersList.Count;
                        }
                    }
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method AddWatchersUserRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddWatchersUserRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method AddWatchersUserRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void AddWatchersRegionRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddWatchersRegionRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (obj is LookupValue)
                {
                    LookupValue lookupvalue = obj as LookupValue;

                    ObservableCollection<People> TeamSalesUser = new ObservableCollection<People>(CrmStartUp.GetAllSalesUserByIdSalesTeam(lookupvalue.IdLookupValue).ToList());
                    foreach (People item in TeamSalesUser)
                    {
                        if (!SelectedWatchersList.Any(x => x.IdPerson == item.IdPerson))
                        {
                            People selectedPerson = WatchersListCopy.FirstOrDefault(x => x.IdPerson == item.IdPerson);
                            if (selectedPerson != null)
                            {
                                SelectedWatchersList.Add(selectedPerson);
                                WatchersList.Remove(WatchersList.FirstOrDefault(x => x.IdPerson == item.IdPerson));
                                //WatchersUserList.Remove(WatchersUserList.FirstOrDefault(x => x.IdPerson == item.IdPerson));
                                WatchersCount = SelectedWatchersList.Count;
                            }
                        }
                    }

                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method AddWatchersRegionRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddWatchersRegionRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method AddWatchersRegionRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method to delete Attendee from Attendees.
        /// </summary>
        /// <param name="obj"></param>
        private void AttendeeCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AttendeeCancelCommandActionAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (obj is People)
                {
                    People person = obj as People;

                    SelectedAttendiesList.Remove(SelectedAttendiesList.FirstOrDefault(x => x.IdPerson == person.IdPerson));
                    AttendeesCount = SelectedAttendiesList.Count;
                    AttendiesList.Add((People)AttendiesListCopy.FirstOrDefault(x => x.IdPerson == person.IdPerson));
                }

                GeosApplication.Instance.Logger.Log("Method AttendeeCancelCommandActionAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AttendeeCancelCommandActionAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Remove Watcher from Watchers.
        /// </summary>
        /// <param name="obj"></param>
        private void WatchersCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method WatchersCancelCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (obj is People)
                {
                    People person = obj as People;

                    SelectedWatchersList.Remove(SelectedWatchersList.FirstOrDefault(x => x.IdPerson == person.IdPerson));
                    WatchersCount = SelectedWatchersList.Count;
                    WatchersList.Add((People)WatchersListCopy.FirstOrDefault(x => x.IdPerson == person.IdPerson));
                }

                GeosApplication.Instance.Logger.Log("Method WatchersCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WatchersCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to fill linked items e.g. Amount
        /// </summary>
        private void FillLinkedItems()
        {
            GeosApplication.Instance.Logger.Log("Method FillLinkedItems ...", category: Category.Info, priority: Priority.Low);

            try
            {
                CarProjectList = new ObservableCollection<CarProject>(CrmStartUp.GetCarProject(0).ToList());
                LinkeditemsCompetitorList = new ObservableCollection<Competitor>(CrmStartUp.GetAllCompetitor().ToList());

                LinkeditemsCompetitorListCopy = new List<Competitor>();
                foreach (Competitor item in LinkeditemsCompetitorList)
                {
                    LinkeditemsCompetitorListCopy.Add((Competitor)item.Clone());
                }

                //Clone People list.
                LinkeditemsPeopleList = new ObservableCollection<People>();
                LinkeditemsPeopleListCopy = new List<People>();

                //if (GeosApplication.Instance.IdUserPermission == 21)
                //{
                //    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                //    {
                //        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                //        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                //        //Account
                //        CompaniesList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomerPlant(salesOwnersIds).ToList());
                //    }
                //}
                //else if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                //{
                //    //Account
                //    CompaniesList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomerPlant(GeosApplication.Instance.ActiveUser.IdUser.ToString()).ToList());

                //    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                //    string idPlants = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                //    CompaniesList = new ObservableCollection<Company>(CrmStartUp.GetAccountBySelectedPlant(GeosApplication.Instance.ActiveUser.IdUser, idPlants).ToList());
                //}
                //else
                //{
                //    //Account
                //    CompaniesList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomerPlant(GeosApplication.Instance.ActiveUser.IdUser.ToString()).ToList());
                //}

                //Clone Account list.
                //CompaniesListCopy = new List<Company>();
                //foreach (Company item in CompaniesList)
                //{
                //    CompaniesListCopy.Add((Company)item.Clone());
                //}

                //Clone Company list.
                CarProjectListCopy = new List<CarProject>();
                foreach (CarProject item in CarProjectList)
                {
                    CarProjectListCopy.Add((CarProject)item.Clone());
                }

                //ListPlantOpportunity = new ObservableCollection<Offer>();
                //foreach (Offer plantOffer in ListPlantOpportunity)
                //{
                //    ListPlantOpportunityCopy.Add((Offer)plantOffer.Clone());
                //}

                GeosApplication.Instance.Logger.Log("Method FillLinkedItems() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for fill Owner list.
        /// </summary>
        private void FillOwnerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOwnerList ...", category: Category.Info, priority: Priority.Low);
                ActivityOwnerList = new List<UserManagerDtl>();
                SelectedActivityOwnerList = new List<object>();

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    ActivityOwnerList = GeosApplication.Instance.SalesOwnerUsersList.OrderBy(x => x.User.FullName).ToList();
                    SelectedActivityOwnerList.Add(ActivityOwnerList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser));
                }
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    string idPlants = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    //ActivityOwnerList = CrmStartUp.GetSalesUserByPlant(idPlants).OrderBy(x => x.User.FullName).ToList();
                    //[pramod.misal][GEOS2-4688][27.09.2023]
                    ActivityOwnerList = CrmStartUp.GetSalesUserByPlant_V2440(idPlants).OrderBy(x => x.User.FullName).ToList();
                    SelectedActivityOwnerList.Add(ActivityOwnerList.FirstOrDefault(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser));
                }
                else
                {
                    User user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
                    if (user != null)
                    {
                        UserManagerDtl userManagerDtl = new UserManagerDtl();
                        userManagerDtl.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                        userManagerDtl.User = user;
                        ActivityOwnerList.Add(userManagerDtl);
                        SelectedActivityOwnerList.Add(userManagerDtl);
                    }
                }

                ////ActivityOwnerList = new List<User>();
                ////ActivityOwnerList.Add(user);

                ////for (int i = 0; i < ActivityOwnerList.Count; i++)
                ////{
                ////    try
                ////    {
                ////        UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImage(user.Login);
                ////        ActivityOwnerList[i].OwnerImage = byteArrayToImage(UserProfileImageByte);
                ////    }
                ////    catch (Exception ex)
                ////    {
                ////        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                ////        {
                ////            if (user.IdUserGender == 1)
                ////                ActivityOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                ////            else
                ////                ActivityOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                ////        }
                ////        else
                ////        {
                ////            if (user.IdUserGender == 1)
                ////                ActivityOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                ////            else
                ////                ActivityOwnerList[i].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                ////        }
                ////    }
                ////}

                ////if (SelectedIndexOwner == 0)
                ////    SelectedIndexOwner = ActivityOwnerList.FindIndex(i => i.IdUser == GeosApplication.Instance.ActiveUser.IdUser);

                ////if (SelectedIndexOwner < 0)
                ////    SelectedIndexOwner = 0;

                GeosApplication.Instance.Logger.Log("Method FillOwnerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOwnerList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOwnerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOwnerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Type list.
        /// </summary>
        private void FillTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTypeList ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> temptypeList = CrmStartUp.GetLookupValues(9);
                TypeList = new List<LookupValue>();
                TypeList = new List<LookupValue>(temptypeList.Where(inUseOption => inUseOption.InUse == true));
                if (!GeosApplication.Instance.IsPermissionAuditor)
                {
                    TypeList = TypeList.Where(i => i.IdLookupValue != 96).ToList();
                }

                TypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                if (TypeList.Count > 0)
                    SelectedIndexType = 1;                 // appoinment selected by default
                else
                    SelectedIndexType = 0;

                GeosApplication.Instance.Logger.Log("Method FillTypeList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fill Status list for Task type.
        /// </summary>
        private void FillTaskStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTaskStatusList ...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempTaskStatusList = CrmStartUp.GetLookupValues(11);
                TaskStatusList = new List<LookupValue>();
                TaskStatusList = new List<LookupValue>(tempTaskStatusList.Where(inUseOption => inUseOption.InUse == true));
                SelectedIndexTaskStatus = -1;

                GeosApplication.Instance.Logger.Log("Method FillTaskStatusList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fill Attendies List
        /// </summary>
        private void FillAttendiesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAttendiesList ...", category: Category.Info, priority: Priority.Low);

                // "0" to load all sales people without account contacts.
                //AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030("0").ToList());
                //[rajashri.telvekar][GEOS2-4689][28-9-2023]
                AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2440("0").ToList());

                AttendiesListCopy = new List<People>();
                foreach (People people in AttendiesList)
                {
                    AttendiesListCopy.Add((People)people.Clone());
                }

                GeosApplication.Instance.Logger.Log("Method FillAttendiesList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fill Watchers List
        /// </summary>
        private void FillWatchersList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWatchersList ...", category: Category.Info, priority: Priority.Low);

                // "0" to load all sales people without account contacts.

                if (IsInformationVisible == Visibility.Visible)
                {
                    WatchersRegionList = new ObservableCollection<LookupValue>();
                    ObservableCollection<LookupValue> regionList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(8).ToList());
                    if (GeosApplication.Instance.IdUserPermission == 22)
                    {
                        WatchersList = new ObservableCollection<People>(CrmStartUp.GetAllSalesUser().ToList());
                        WatchersRegionList = regionList;
                    }
                    else if (GeosApplication.Instance.IdUserPermission == 20 || GeosApplication.Instance.IdUserPermission == 21)
                    {
                        People people = CrmStartUp.GetIdSalesTeamByIdSalesUser(GeosApplication.Instance.ActiveUser.IdUser);
                        if (people != null)
                        {
                            WatchersList = new ObservableCollection<People>(CrmStartUp.GetAllSalesUserByIdSalesTeam(people.IdSalesTeam).ToList());
                            WatchersRegionList.Add(regionList.Where(wc => wc.IdLookupValue == people.IdSalesTeam).FirstOrDefault());
                        }
                    }

                }
                else
                {
                    WatchersList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030("0").ToList());
                }


                WatchersListCopy = new List<People>();
                foreach (People people in WatchersList)
                {
                    WatchersListCopy.Add((People)people.Clone());
                }
                GeosApplication.Instance.Logger.Log("Method FillWatchersList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillWatchersList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillWatchersList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWatchersList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Linked Items methods

        /// <summary>
        /// Method for Project double click and add it in linked items.
        /// </summary>
        /// <param name="obj"></param>
        private void ProjectRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ProjectRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (obj is CarProject)
                {
                    CarProject carProject = obj as CarProject;

                    if (!SelectedActivityLinkedItems.Any(x => x.IdCarProject == carProject.IdCarProject && x.LinkedItemType.IdLookupValue == 45))
                    {
                        ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();

                        activityLinkedItem.IdCarProject = carProject.IdCarProject;
                        activityLinkedItem.Name = carProject.Name + " - " + carProject.CarOEM.Name;

                        activityLinkedItem.IdLinkedItemType = 45;
                        activityLinkedItem.LinkedItemType = new LookupValue();
                        activityLinkedItem.LinkedItemType.IdLookupValue = 45;
                        activityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityProject").ToString();
                        activityLinkedItem.CarProject = (CarProject)carProject.Clone();
                        SelectedActivityLinkedItems.Add(activityLinkedItem);
                        CarProjectList.Remove(CarProjectList.FirstOrDefault(x => x.IdCarProject == carProject.IdCarProject));
                        if (SelectedActivityLinkedItems != null && SelectedActivityLinkedItems.Any(i => i.IdLinkedItemType == 45))
                        {
                            FillPlantOpportunityList(false, new List<Offer>());
                        }
                        try
                        {
                            byte[] bytes = GeosRepositoryServiceController.GetCaroemIconFileInBytes(carProject.CarOEM.Name);
                            if (bytes != null)
                            {
                                activityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                            }
                            else
                            {
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                    activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wProject.png");
                                //activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueProject.png");
                                else
                                    activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueProject.png");
                            }
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            IsBusy = false;
                            GeosApplication.Instance.Logger.Log("Get an error in ProjectRowMouseDoubleClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            IsBusy = false;
                            GeosApplication.Instance.Logger.Log("Get an error in ProjectRowMouseDoubleClickCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        }
                    }
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method ProjectRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ProjectRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Contact double click and add it in linked items.
        /// </summary>
        /// <param name="obj"></param>
        private void ContactRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ContactRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (obj is People)
                {
                    People people = obj as People;

                    if (!SelectedActivityLinkedItems.Any(x => x.IdPerson == people.IdPerson && x.LinkedItemType.IdLookupValue == 43))
                    {
                        ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();

                        activityLinkedItem.IdPerson = people.IdPerson;
                        activityLinkedItem.Name = people.FullName;

                        activityLinkedItem.IdLinkedItemType = 43;
                        activityLinkedItem.LinkedItemType = new LookupValue();
                        activityLinkedItem.LinkedItemType.IdLookupValue = 43;
                        activityLinkedItem.LinkedItemType.Value = "Contact";
                        activityLinkedItem.People = (People)people.Clone();
                        SelectedActivityLinkedItems.Add(activityLinkedItem);
                        LinkeditemsPeopleList.Remove(LinkeditemsPeopleList.FirstOrDefault(x => x.IdPerson == people.IdPerson));

                        FillPersonImageByIdPerson(activityLinkedItem, people.IdPerson);
                    }
                }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method ContactRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in ContactRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void CompetitorRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CompetitorRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (obj is Competitor)
                {
                    Competitor competitor = obj as Competitor;

                    if (!SelectedActivityLinkedItems.Any(x => x.IdCompetitor == competitor.IdCompetitor && x.LinkedItemType.IdLookupValue == 92))
                    {
                        ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();

                        activityLinkedItem.IdCompetitor = competitor.IdCompetitor;
                        activityLinkedItem.Name = competitor.Name;

                        activityLinkedItem.IdLinkedItemType = 92;
                        activityLinkedItem.LinkedItemType = new LookupValue();
                        activityLinkedItem.LinkedItemType.IdLookupValue = 92;
                        activityLinkedItem.LinkedItemType.Value = "Competitor";
                        activityLinkedItem.Competitor = (Competitor)competitor.Clone();
                        SelectedActivityLinkedItems.Add(activityLinkedItem);
                        LinkeditemsCompetitorList.Remove(LinkeditemsCompetitorList.FirstOrDefault(x => x.IdCompetitor == competitor.IdCompetitor));

                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wCompititor.png");
                        //activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueCompititor.png");
                        else
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueCompititor.png");

                        //FillPersonImageByIdPerson(activityLinkedItem, people.IdPerson);
                    }
                }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method CompetitorRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in CompetitorRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillPersonImageByIdPerson(ActivityLinkedItem activityLinkedItem, int IdPerson)
        {
            try
            {
                //User user = WorkbenchStartUp.GetUserById(IdPerson);
                //if (user != null)
                //{
                //    byte[] bytes = GeosRepositoryServiceController.GetUserProfileImageWithoutException(user.Login);
                //    if (bytes != null)
                //    {
                //        activityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                //    }
                //    else
                //    {
                //        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                //        {
                //            if (user.IdUserGender == 1)
                //                activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                //            else if (user.IdUserGender == 2)
                //                activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                //        }
                //        else
                //        {
                //            if (user.IdUserGender == 1)
                //                activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                //            else if (user.IdUserGender == 2)
                //                activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                //        }
                //    }
                //}
                if (!string.IsNullOrEmpty(activityLinkedItem.People.ImageText))
                {
                    byte[] imageBytes = Convert.FromBase64String(activityLinkedItem.People.ImageText);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    activityLinkedItem.ActivityLinkedItemImage = byteArrayToImage(imageBytes);
                }
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (activityLinkedItem.People.IdPersonGender == 1)
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        //activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueFemale.png");
                        else if (activityLinkedItem.People.IdPersonGender == 2)
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        //activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueMale.png");
                        else if (activityLinkedItem.People.IdPersonGender == null)
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");

                    }
                    else
                    {
                        if (activityLinkedItem.People.IdPersonGender == 1)
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else if (activityLinkedItem.People.IdPersonGender == 2)
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        else if (activityLinkedItem.People.IdPersonGender == null)
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
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
                    ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();
                    activityLinkedItem.Name = offer.Code;

                    activityLinkedItem.IdOffer = offer.IdOffer;
                    activityLinkedItem.IdEmdepSite = Convert.ToInt32(offer.Site.ConnectPlantId);
                    activityLinkedItem.IdLinkedItemType = 44;
                    activityLinkedItem.LinkedItemType = new LookupValue();
                    activityLinkedItem.LinkedItemType.IdLookupValue = 44;

                    activityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();
                    //activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/Opportunity.png");
                    SelectedActivityLinkedItems.Add(activityLinkedItem);
                    ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == offer.IdOffer));

                    Customer customer = CompanyGroupList[SelectedIndexCompanyGroup];

                    //if (activityLinkedItemoffferDetail.Site.Customers.Count > 0)
                    //{
                    byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customer.CustomerName);
                    if (bytes != null)
                    {
                        activityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                    }
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wOpportunity.png");
                        //activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueOpportunity.png");
                        else
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueOpportunity.png");
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
        /// Method for account double click and add it in linked items.
        /// </summary>
        /// <param name="obj"></param>
        //private void AccountRowMouseDoubleClickCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method AccountRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {
        //        IsBusy = true;
        //        if (obj is Company)
        //        {
        //            Company company = obj as Company;

        //            if (!SelectedActivityLinkedItems.Any(x => x.LinkedItemType.IdLookupValue == 42))
        //            {
        //                if (!SelectedActivityLinkedItems.Any(x => x.IdSite == company.IdCompany && x.LinkedItemType.IdLookupValue == 42))
        //                {
        //                    ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();
        //                    activityLinkedItem.IdSite = company.IdCompany;
        //                    activityLinkedItem.Name = company.Customers[0].CustomerName + " - " + company.Name;
        //                    activityLinkedItem.Company = company;
        //                    activityLinkedItem.Customer = company.Customers[0];

        //                    activityLinkedItem.IdLinkedItemType = 42;
        //                    activityLinkedItem.LinkedItemType = new LookupValue();
        //                    activityLinkedItem.LinkedItemType.IdLookupValue = 42;
        //                    activityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

        //                    // Set account location to activity location
        //                    CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
        //                    customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
        //                    if (SelectedActivityType.IdLookupValue == 37)   // Appointment
        //                    {
        //                        if (string.IsNullOrEmpty(ActivityAddress))
        //                        {
        //                            ActivityAddress = company.Address;
        //                            if (company.Latitude != null && company.Longitude != null)
        //                            {
        //                                customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude = Convert.ToDouble(company.Latitude);
        //                                customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude = Convert.ToDouble(company.Longitude);
        //                                MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

        //                                Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
        //                                Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
        //                            }
        //                            else
        //                            {
        //                                MapLatitudeAndLongitude = null;
        //                                Latitude = null;
        //                                Longitude = null;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (!string.IsNullOrEmpty(company.Address))
        //                            {
        //                                if (!ActivityAddress.Contains(company.Address))
        //                                {
        //                                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["CustomerEditViewChangeLocationDetails"].ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
        //                                    if (MessageBoxResult == MessageBoxResult.Yes)
        //                                    {
        //                                        ActivityAddress = company.Address;
        //                                        if (company.Latitude != null && company.Longitude != null)
        //                                        {
        //                                            customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude = Convert.ToDouble(company.Latitude);
        //                                            customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude = Convert.ToDouble(company.Longitude);
        //                                            MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

        //                                            Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
        //                                            Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
        //                                        }
        //                                        else
        //                                        {
        //                                            MapLatitudeAndLongitude = null;
        //                                            Latitude = null;
        //                                            Longitude = null;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    SelectedActivityLinkedItems.Insert(0, activityLinkedItem);
        //                    SelectedAccountActivityLinkedItemsCount = 1;
        //                    CompaniesList.Remove(CompaniesList.FirstOrDefault(x => x.IdCompany == company.IdCompany));

        //                    try
        //                    {
        //                        byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(company.Customers[0].CustomerName);
        //                        if (bytes != null)
        //                        {
        //                            activityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
        //                        }
        //                    }
        //                    catch (FaultException<ServiceException> ex)
        //                    {
        //                        IsBusy = false;
        //                        GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //                    }
        //                    catch (ServiceUnexceptedException ex)
        //                    {
        //                        IsBusy = false;
        //                        GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //                    }

        //                }
        //                try
        //                {
        //                    //Contacts
        //                    LinkeditemsPeopleList = new ObservableCollection<People>(CrmStartUp.GetContactsByLinkedItemAccount_V2030(Convert.ToString(company.IdCompany)).ToList());
        //                    LinkeditemsPeopleListCopy = new List<People>();
        //                    foreach (People item in LinkeditemsPeopleList)
        //                    {
        //                        LinkeditemsPeopleListCopy.Add((People)item.Clone());
        //                    }

        //                    //Attendees
        //                    AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030(Convert.ToString(company.IdCompany)).ToList());
        //                    //Make one main copy.
        //                    AttendiesListCopy = new List<People>();
        //                    foreach (People people in AttendiesList)
        //                    {
        //                        AttendiesListCopy.Add((People)people.Clone());
        //                    }

        //                    //remove attendee if not related to account.
        //                    for (int i = SelectedAttendiesList.Count - 1; i > -1; i--)
        //                    {
        //                        if (!AttendiesList.Any(x => x.IdPerson == SelectedAttendiesList[i].IdPerson))
        //                        {
        //                            //If grid attendies list does not contain idperson then remove it from selected attendies list. 
        //                            SelectedAttendiesList.RemoveAt(i);
        //                        }
        //                        else
        //                        {
        //                            //If selected attendies list contains idperson then remove it from attendies grid.
        //                            AttendiesList.Remove(AttendiesList.FirstOrDefault(x => x.IdPerson == SelectedAttendiesList[i].IdPerson));
        //                        }
        //                    }
        //                }
        //                catch (FaultException<ServiceException> ex)
        //                {
        //                    IsBusy = false;
        //                    GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //                }
        //                catch (ServiceUnexceptedException ex)
        //                {
        //                    IsBusy = false;
        //                    GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //                }
        //            }
        //        }
        //        IsBusy = false;
        //        GeosApplication.Instance.Logger.Log("Method AccountRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        IsBusy = false;
        //        GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}


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


                    if (!SelectedActivityLinkedItems.Any(x => x.LinkedItemType.IdLookupValue == 42))
                    {
                        if (!SelectedActivityLinkedItems.Any(x => x.IdSite == company.IdCompany && x.LinkedItemType.IdLookupValue == 42))
                        {
                            ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();
                            activityLinkedItem.IdSite = company.IdCompany;
                            activityLinkedItem.Name = Group.CustomerName + " - " + company.Name;
                            activityLinkedItem.Company = company;
                            activityLinkedItem.Customer = Group;

                            activityLinkedItem.IdLinkedItemType = 42;
                            activityLinkedItem.LinkedItemType = new LookupValue();
                            activityLinkedItem.LinkedItemType.IdLookupValue = 42;
                            activityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                            // Set account location to activity location
                            CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                            customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                            customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();
                            if (SelectedActivityType != null && (SelectedActivityType.IdLookupValue == 37 || SelectedActivityType.IdLookupValue == 96))   // Appointment,PlannedAppointment
                            {
                                if (string.IsNullOrEmpty(ActivityAddress))
                                {
                                    ActivityAddress = company.Address;
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
                                        if (!ActivityAddress.Contains(company.Address))
                                        {
                                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["CustomerEditViewChangeLocationDetails"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                            if (MessageBoxResult == MessageBoxResult.Yes)
                                            {
                                                ActivityAddress = company.Address;
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
                            SelectedActivityLinkedItems.Insert(0, activityLinkedItem);
                            SelectedAccountActivityLinkedItemsCount = 1;
                            // CompaniesList.Remove(CompaniesList.FirstOrDefault(x => x.IdCompany == company.IdCompany));

                            try
                            {
                                byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(Group.CustomerName);
                                if (bytes != null)
                                {
                                    activityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                                }
                                else
                                {
                                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                        activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wAccount.png");
                                    // activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueAccount.png");
                                    else
                                        activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueAccount.png");
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
                            //Contacts
                            // LinkeditemsPeopleList = new ObservableCollection<People>(CrmStartUp.GetContactsByLinkedItemAccount_V2030(Convert.ToString(company.IdCompany)).ToList());
                            // LinkeditemsPeopleListCopy = new List<People>();
                            foreach (People item in LinkeditemsPeopleList)
                            {
                                LinkeditemsPeopleListCopy.Add((People)item.Clone());
                            }

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
                if (SelectedActivityLinkedItems != null && SelectedActivityLinkedItems.Count > 0)
                {
                    SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42));
                    SelectedAccountActivityLinkedItemsCount = 0;

                    //Remove Opportunity if Plant selected Index set 0 or group selected index set 0
                    //ListPlantOpportunity = new ObservableCollection<Offer>();
                    //ListPlantOpportunityCopy = new ObservableCollection<Offer>();
                    SelectedActivityLinkedItems.Where(l => l.IdLinkedItemType == 44 && l.IsVisible == true).ToList().All(i => SelectedActivityLinkedItems.Remove(i));

                    //Remove contacts if account is removed.
                    LinkeditemsPeopleList = new ObservableCollection<People>();
                    LinkeditemsPeopleListCopy = new List<People>();
                    SelectedActivityLinkedItems.Where(l => l.IdLinkedItemType == 43 && l.IsVisible == true).ToList().All(i => SelectedActivityLinkedItems.Remove(i));

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
                    ActivityAddress = string.Empty;

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
                if (obj is ActivityLinkedItem)
                {
                    ActivityLinkedItem linkedItem = obj as ActivityLinkedItem;

                    if (linkedItem.IdLinkedItemType == 42)      //Account
                    {
                        SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42 && x.IdSite == linkedItem.IdSite));
                        SelectedAccountActivityLinkedItemsCount = 0;
                        //Company comp = CompaniesListCopy.FirstOrDefault(x => x.IdCompany == linkedItem.IdSite);

                        //if (comp != null)
                        //{
                        //    CompaniesList.Add((Company)comp.Clone());
                        //}
                        //else
                        //{
                        //    if (linkedItem.Company != null)
                        //        CompaniesList.Add(linkedItem.Company);
                        //}

                        //Remove contacts if account is removed.
                        LinkeditemsPeopleList = new ObservableCollection<People>();
                        LinkeditemsPeopleListCopy = new List<People>();
                        SelectedActivityLinkedItems.Where(l => l.IdLinkedItemType == 43 && l.IsVisible == true).ToList().All(i => SelectedActivityLinkedItems.Remove(i));

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
                        SelectedIndexCompanyGroup = 0;
                        SelectedIndexCompanyPlant = 0;
                        ActivityAddress = string.Empty;
                        IsCoordinatesNull = false;


                    }
                    else if (linkedItem.IdLinkedItemType == 43) //Contact
                    {
                        SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 43 && x.IdPerson == linkedItem.IdPerson && linkedItem.IsVisible == true));
                        LinkeditemsPeopleList.Add((People)LinkeditemsPeopleListCopy.FirstOrDefault(x => x.IdPerson == linkedItem.IdPerson).Clone());
                    }
                    else if (linkedItem.IdLinkedItemType == 44) // Opportunity
                    {
                        SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 44 && x.IdEmdepSite == linkedItem.IdEmdepSite && x.IdOffer == linkedItem.IdOffer));
                        ListPlantOpportunity.Add((Offer)ListPlantOpportunityCopy.FirstOrDefault(x => x.IdOffer == linkedItem.IdOffer).Clone());
                    }
                    else if (linkedItem.IdLinkedItemType == 45) //Car project
                    {
                        SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 45 && x.IdCarProject == linkedItem.IdCarProject));
                        CarProjectList.Add((CarProject)CarProjectListCopy.FirstOrDefault(x => x.IdCarProject == linkedItem.IdCarProject).Clone());
                        FillPlantOpportunityList(false, new List<Offer>());
                    }
                    else if (linkedItem.IdLinkedItemType == 92) //Competitor
                    {
                        SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 92 && x.IdCompetitor == linkedItem.IdCompetitor));
                        LinkeditemsCompetitorList.Add((Competitor)LinkeditemsCompetitorListCopy.FirstOrDefault(x => x.IdCompetitor == linkedItem.IdCompetitor).Clone());
                    }
                }
                GeosApplication.Instance.Logger.Log("Method LinkedItemCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LinkedItemCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion //Linked items



        #region All Tags

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

        #endregion


        /// <summary>
        /// SearchButtonClickCommandAction
        /// </summary>
        /// <param name="obj"></param>
        public void SearchButtonClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchButtonClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            //BingSearchDataProvider bingSearchDataProvider = (BingSearchDataProvider)obj;
            //bingSearchDataProvider.Search(CustomerCity);
            CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
            CustomerSetLocationMapView customerSetLocationMapView = new CustomerSetLocationMapView();
            EventHandler handle = delegate { customerSetLocationMapView.Close(); };
            customerSetLocationMapViewModel.RequestClose += handle;

            if (MapLatitudeAndLongitude != null)
            {
                customerSetLocationMapViewModel.MapLatitudeAndLongitude = MapLatitudeAndLongitude;
                customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = MapLatitudeAndLongitude;
            }

            customerSetLocationMapView.DataContext = customerSetLocationMapViewModel;
            customerSetLocationMapView.ShowDialog();

            if (customerSetLocationMapViewModel.MapLatitudeAndLongitude != null
                && !string.IsNullOrEmpty(customerSetLocationMapViewModel.LocationAddress))
            {
                if (!string.IsNullOrEmpty(ActivityAddress))
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["CustomerEditViewChangeLocationDetails"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                        Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                        MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;
                        ActivityAddress = customerSetLocationMapViewModel.LocationAddress;
                    }
                }
                else
                {
                    Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
                    Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
                    MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;
                    ActivityAddress = customerSetLocationMapViewModel.LocationAddress;
                }
            }

            GeosApplication.Instance.Logger.Log("Method SearchButtonClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for add new activity save.
        /////[GEOS2-257][001]
        /// </summary>
        /// <param name="obj"></param>
        public void AddNewActivityAccept(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewActivityAccept ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                AddActivityError = null;
                AttendeesCount = SelectedAttendiesList.Count;
                SelectedAccountActivityLinkedItemsCount = SelectedActivityLinkedItems.Where(x => x.IdLinkedItemType == 42).Count();

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                #region ToDate, FromDate &StartTime,EndTime Commented
                //PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                //PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));
                //PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                //PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexOwner"));
                #endregion
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedActivityOwnerList"));
                PropertyChanged(this, new PropertyChangedEventArgs("ActivityAddress"));
                PropertyChanged(this, new PropertyChangedEventArgs("Subject"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexAttendies"));
                PropertyChanged(this, new PropertyChangedEventArgs("AttendeesCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("ToDateWithTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("FromDateWithTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("DueDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexTaskStatus"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedAccountActivityLinkedItemsCount"));
                if (string.IsNullOrEmpty(error))
                    AddActivityError = null;
                else
                    AddActivityError = "";
                if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    NewCreatedActivityList = new List<Activity>();

                    // For Add tag
                    List<ActivityTag> listTag = new List<ActivityTag>();
                    List<string> lstTag = new List<string>();
                    if (SelectedTagList != null)
                    {
                        foreach (Tag item in SelectedTagList)
                        {
                            ActivityTag objTag = new ActivityTag();
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
                                        GeosApplication.Instance.TagList.Add(tagActivity);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            listTag.Add(objTag);
                            lstTag.Add(((Tag)item).Name.ToString());
                        }
                        TagsString = string.Join(" , ", lstTag.ToList());
                    }

                    foreach (UserManagerDtl user in SelectedActivityOwnerList)
                    {
                        NewActivity = new Activity();

                        NewActivity.IdActivityType = TypeList[SelectedIndexType].IdLookupValue;
                        if (!string.IsNullOrEmpty(Subject))
                            NewActivity.Subject = Subject.Trim();
                        if (!string.IsNullOrEmpty(Description))
                            NewActivity.Description = Description.Trim();
                        NewActivity.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        NewActivity.IsCompleted = (byte)(IsCompleted ? 1 : 0);
                        NewActivity.IdOwner = user.IdUser;               // ActivityOwnerList[SelectedIndexOwner].IdUser;
                        NewActivity.ActivityTags = listTag;

                        // If selected user is not equal to current user then send notification and email.
                        if (user.IdUser != GeosApplication.Instance.ActiveUser.IdUser)
                        {
                            Notification notification = new Notification();
                            notification.FromUser = GeosApplication.Instance.ActiveUser.IdUser;
                            notification.ToUser = user.IdUser;                  // 1356;
                            notification.Title = "New Activity Created";

                            if (TypeList[SelectedIndexType].IdLookupValue == 37 || TypeList[SelectedIndexType].IdLookupValue == 39)
                                notification.Message = string.Format("An {0} activity named \"{1}\" has been created and assigned to you by {2}.", TypeList[SelectedIndexType].Value, NewActivity.Subject, GeosApplication.Instance.ActiveUser.FullName);
                            else
                                notification.Message = string.Format("A {0} activity named \"{1}\" has been created and assigned to you by {2}.", TypeList[SelectedIndexType].Value, NewActivity.Subject, GeosApplication.Instance.ActiveUser.FullName);

                            notification.IdModule = 5;
                            notification.Status = "Unread";
                            notification.IsNew = 1;
                            NewActivity.Notification = notification;

                            ActivityMail activityMail = new ActivityMail();
                            activityMail.SendToUserName = user.User.FullName;                               // "Sagar Khade";
                            activityMail.CreatedByUserName = GeosApplication.Instance.ActiveUser.FullName;  // "Suvarna Kulkarni";
                            activityMail.ActivityType = TypeList[SelectedIndexType].Value;                  // "Appointment";
                            activityMail.ActivitySubject = NewActivity.Subject;                             // "Visit to HQ";
                            activityMail.ActivityDescription = NewActivity.Description;                     // "bla bla bla bla";

                            if (IsAppointmentVisible == Visibility.Visible)
                            {
                                activityMail.ActivityDueDate = DueDate.Value.ToString("dd/MM/yyyy");
                            }
                            else if (IsDueDateVisible == Visibility.Visible)
                            {
                                activityMail.ActivityDueDate = DueDate.Value.ToString("dd/MM/yyyy");
                            }

                            activityMail.ActivitySentToMail = user.User.CompanyEmail;   // "skhade@emdep.com";
                            NewActivity.ActivityMail = activityMail;
                        }

                        List<LogEntriesByActivity> logEntriesByActivity = new List<LogEntriesByActivity>();

                        if (IsAppointmentVisible == Visibility.Visible)
                        {
                            //NewActivity.FromDate = DueDate;
                            //NewActivity.ToDate = DueDate;
                            if (!string.IsNullOrEmpty(ActivityAddress))
                                NewActivity.Location = ActivityAddress.Trim();
                            NewActivity.Longitude = Longitude;
                            NewActivity.Latitude = Latitude;
                            NewActivity.IsInternal = Convert.ToByte(IsInternal);

                            // For Add Attendies
                            List<ActivityAttendees> listattendees = new List<ActivityAttendees>();
                            List<string> lstAttendies = new List<string>();
                            foreach (People item in SelectedAttendiesList)
                            {
                                ActivityAttendees objAttendies = new ActivityAttendees();
                                objAttendies.IdUser = item.IdPerson;
                                listattendees.Add(objAttendies);
                                lstAttendies.Add(((People)item).FullName.ToString());
                            }
                            AttendiesString = string.Join(", ", lstAttendies.ToList());
                            NewActivity.ActivityAttendees = listattendees;

                            // For attendies add log entries.
                            if (NewActivity.ActivityAttendees != null)
                            {
                                foreach (var item in NewActivity.ActivityAttendees)
                                {
                                    LogEntriesByActivity objLog = new LogEntriesByActivity();
                                    objLog.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                    objLog.Datetime = GeosApplication.Instance.ServerDateTime;
                                    objLog.IdLogEntryType = 2;
                                    People person = AttendiesListCopy.FirstOrDefault(x => x.IdPerson == item.IdUser);
                                    if (person != null)
                                    {
                                        objLog.Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAttendees").ToString(), person.FullName);
                                    }
                                    logEntriesByActivity.Add(objLog);
                                }
                            }
                            if (IsCompleted)
                                AddAutoComment(AttendiesString);

                        }
                        else if (IsTaskVisible == Visibility.Visible)
                        {
                            NewActivity.IdActivityStatus = TaskStatusList[SelectedIndexTaskStatus].IdLookupValue;
                            NewActivity.ActivityStatus = TaskStatusList[SelectedIndexTaskStatus];
                            NewActivity.IsCompleted = (byte)(NewActivity.IdActivityStatus == 48 ? 1 : 0);
                            NewActivity.IsInternal = Convert.ToByte(IsInternal);
                            // For Add Watchers
                            List<ActivityWatcher> listWatchers = new List<ActivityWatcher>();
                            foreach (People item in SelectedWatchersList)
                            {
                                ActivityWatcher objWatchers = new ActivityWatcher();
                                objWatchers.IdUser = item.IdPerson;
                                listWatchers.Add(objWatchers);
                            }

                            NewActivity.ActivityWatchers = listWatchers;

                            // For attendies add log entries.
                            if (NewActivity.ActivityWatchers != null)
                            {
                                foreach (var item in NewActivity.ActivityWatchers)
                                {
                                    LogEntriesByActivity objLog = new LogEntriesByActivity();
                                    objLog.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                    objLog.Datetime = GeosApplication.Instance.ServerDateTime;
                                    objLog.IdLogEntryType = 2;
                                    People person = WatchersListCopy.FirstOrDefault(x => x.IdPerson == item.IdUser);
                                    if (person != null)
                                    {
                                        objLog.Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddWatchers").ToString(), person.FullName);
                                    }
                                    logEntriesByActivity.Add(objLog);
                                }
                            }

                        }
                        else if (IsInformationVisible == Visibility.Visible)
                        {
                            if (!string.IsNullOrEmpty(ActivityAddress))
                                NewActivity.Location = ActivityAddress.Trim();
                            NewActivity.Longitude = Longitude;
                            NewActivity.Latitude = Latitude;
                            NewActivity.IsInternal = Convert.ToByte(IsInternal);

                            NewActivity.FromDate = GeosApplication.Instance.ServerDateTime;
                            NewActivity.ToDate = GeosApplication.Instance.ServerDateTime;
                            NewActivity.IsCompleted = 1;

                            // For Add Watchers
                            List<ActivityWatcher> listWatchers = new List<ActivityWatcher>();
                            foreach (People item in SelectedWatchersList)
                            {
                                ActivityWatcher objWatchers = new ActivityWatcher();
                                objWatchers.IdUser = item.IdPerson;
                                listWatchers.Add(objWatchers);
                            }

                            NewActivity.ActivityWatchers = listWatchers;

                            // For attendies add log entries.
                            if (NewActivity.ActivityWatchers != null)
                            {
                                foreach (var item in NewActivity.ActivityWatchers)
                                {
                                    LogEntriesByActivity objLog = new LogEntriesByActivity();
                                    objLog.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                                    objLog.Datetime = GeosApplication.Instance.ServerDateTime;
                                    objLog.IdLogEntryType = 2;
                                    People person = WatchersListCopy.FirstOrDefault(x => x.IdPerson == item.IdUser);
                                    if (person != null)
                                    {
                                        objLog.Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddWatchers").ToString(), person.FullName);
                                    }
                                    logEntriesByActivity.Add(objLog);
                                }
                            }

                        }
                        if (IsDueDateVisible == Visibility.Visible)
                        {
                            NewActivity.FromDate = DueDate;
                            NewActivity.ToDate = DueDate;
                        }

                        NewActivity.LogEntriesByActivity = logEntriesByActivity;

                        // For Comments

                        //NewActivity.CommentsByActivity = ActivityCommentsList.ToList();
                        //NewActivity.CommentsByActivity.ForEach(it => it.People.OwnerImage = null);

                        NewActivity.CommentsByActivity = new List<LogEntriesByActivity>();
                        List<LogEntriesByActivity> newList = ActivityCommentsList.Where(x => x.IdLogEntryByActivity == 0 || x.IsDeleted || x.IsUpdated).ToList();
                        foreach (LogEntriesByActivity item in newList)
                        {
                            string oldTempComment = item.Comments;
                            string newTempComment = string.Empty;
                            string pattern = "[^\\w]";
                            string[] words = null;
                            int i = 0;
                            int count = 0;

                            LogEntriesByActivity newobj = new LogEntriesByActivity();
                            newobj.IdLogEntryByActivity = item.IdLogEntryByActivity;
                            newobj.IdActivity = item.IdActivity;
                            newobj.IdUser = item.IdUser;
                            newobj.Comments = item.Comments;
                            newobj.IsRtfText = item.IsRtfText;
                            newobj.Datetime = item.Datetime;
                            newobj.IdLogEntryType = item.IdLogEntryType;
                            newobj.IsDeleted = item.IsDeleted;
                            newobj.IsUpdated = item.IsUpdated;
                            NewActivity.CommentsByActivity.Add(newobj);

                            if (item.IdLogEntryByActivity == 0)
                            {
                                if (item.IsRtfText)
                                {
                                    TextRange range = null;
                                    var rtb = new RichTextBox();
                                    var doc = new FlowDocument();
                                    MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(item.Comments.ToString()));
                                    range = new TextRange(doc.ContentStart, doc.ContentEnd);
                                    range.Load(stream, DataFormats.Rtf);

                                    if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                                        oldTempComment = range.Text;


                                    words = Regex.Split(oldTempComment, pattern, RegexOptions.IgnoreCase);
                                    for (i = words.GetLowerBound(0); i <= words.GetUpperBound(0); i++)
                                    {
                                        if (words[i].ToString() == string.Empty)
                                            count = count - 1;
                                        count = count + 1;
                                    }
                                    if (count > 10)
                                    {
                                        for (int j = 0; j <= 10; j++)
                                        {
                                            newTempComment += words[j] + " ";
                                        }

                                        newTempComment = newTempComment.TrimEnd();
                                        newTempComment += "...";
                                    }
                                    else
                                    {
                                        newTempComment = oldTempComment;
                                        newTempComment = newTempComment.TrimEnd();
                                    }
                                    logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });
                                }
                                else
                                {
                                    words = Regex.Split(oldTempComment, pattern, RegexOptions.IgnoreCase);
                                    for (i = words.GetLowerBound(0); i <= words.GetUpperBound(0); i++)
                                    {
                                        if (words[i].ToString() == string.Empty)
                                            count = count - 1;
                                        count = count + 1;
                                    }
                                    if (count > 10)
                                    {
                                        for (int j = 0; j <= 10; j++)
                                        {
                                            newTempComment += words[j] + " ";
                                        }

                                        newTempComment = newTempComment.TrimEnd();
                                        newTempComment += "...";
                                    }
                                    else
                                    {
                                        newTempComment = oldTempComment;
                                        newTempComment = newTempComment.TrimEnd();
                                    }

                                    logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });
                                }
                            }
                        }

                        // End Comments


                        //Linked Items - Add linked item to list.
                        List<ActivityLinkedItem> listActivityLinkedItems = new List<ActivityLinkedItem>();
                        foreach (ActivityLinkedItem item in SelectedActivityLinkedItems)
                        {

                            //ActivityLinkedItem addedItem = new ActivityLinkedItem();
                            //item.Clone();
                            //LinkedItemImage = item.ActivityLinkedItemImage;
                            // item.ActivityLinkedItemImage = null;

                            // code for remove ownerimage for contact
                            // if (item.IdLinkedItemType == 43 && item.People != null)
                            //    item.People.OwnerImage = null;

                            listActivityLinkedItems.Add((ActivityLinkedItem)item.Clone());
                            // item.ActivityLinkedItemImage = LinkedItemImage;

                            if (item.IdLinkedItemType == 42)        //Account
                            {
                                logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAccount").ToString(), item.Name), IdLogEntryType = 2 });
                            }
                            else if (item.IdLinkedItemType == 43)   // Contact
                            {
                                logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddContact").ToString(), item.Name), IdLogEntryType = 2 });
                            }
                            else if (item.IdLinkedItemType == 44)   // Opportunity
                            {
                                if (item.LinkedItemType.Value == "Order")
                                {
                                    logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOrder").ToString(), item.Name), IdLogEntryType = 2 });
                                }
                                else
                                {
                                    logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOpportunity").ToString(), item.Name), IdLogEntryType = 2 });
                                }

                            }

                            else if (item.IdLinkedItemType == 45)   // Project
                            {
                                logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddProject").ToString(), item.Name), IdLogEntryType = 2 });
                            }
                            else if (item.IdLinkedItemType == 92)   // Competitor
                            {
                                logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddCompetitor").ToString(), item.Name), IdLogEntryType = 2 });
                            }
                        }



                        foreach (ActivityLinkedItem item in listActivityLinkedItems)
                        {
                            item.ActivityLinkedItemImage = null;
                            if (item.IdLinkedItemType == 43 && item.People != null)
                                item.People.OwnerImage = null;

                        }
                        NewActivity.ActivityLinkedItem = listActivityLinkedItems;
                        //End Linked Items.

                        //Subject
                        //if (NewActivity.Subject != null)
                        //{
                        //   // logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogSubject").ToString(), NewActivity.Subject), IdLogEntryType = 2 });
                        //}


                        // For add activity attachment
                        if (ListAttachment.Count > 0)
                        {
                            bool isupload = UploadActivityAttachment();               // Upload attachment on Server method

                            if (isupload == true)
                            {
                                NewActivity.ActivityAttachment = new List<ActivityAttachment>();
                                NewActivity.ActivityAttachment = ListAttachment.ToList();
                                NewActivity.GUIDString = GuidCode;
                                foreach (var item in ListAttachment)
                                {
                                    item.AttachmentImage = null;
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                NewActivity.ActivityAttachment = new List<ActivityAttachment>();
                            }
                        }
                        else
                        {
                            NewActivity.ActivityAttachment = new List<ActivityAttachment>();
                        }

                        // For add activity
                        NewActivity.IsDeleted = 0;
                        //[001]
                        NewActivity = CrmStartUp.AddActivity_V2031(NewActivity);

                        //Add this properties to show into grid.
                        if (NewActivity != null)
                        {
                            NewActivity.LookupValue = TypeList[SelectedIndexType];

                            NewActivity.People = new People();
                            NewActivity.People.Name = user.User.FirstName;      // ActivityOwnerList[SelectedIndexOwner].FirstName;
                            NewActivity.People.Surname = user.User.LastName;    // ActivityOwnerList[SelectedIndexOwner].LastName;
                            NewActivity.ActivityTagsString = TagsString;

                            if (!string.IsNullOrEmpty(AttendiesString))
                            {
                                AttendiesString = AttendiesString.Replace(",", "\n");
                                NewActivity.ActivityAttendeesString = AttendiesString;
                            }
                        }

                        NewCreatedActivityList.Add((Activity)NewActivity.Clone());
                    }

                    if (NewCreatedActivityList != null && NewCreatedActivityList.Count > 0)
                    {
                        IsActivitySave = true;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivitySaveSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivitySaveFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddNewActivityAccept() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                NewActivity = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewActivityAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                NewActivity = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewActivityAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                NewActivity = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewActivityAccept() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                string tempimagePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\Images\";

                if (File.Exists(tempimagePath))
                {
                    string[] filePaths = Directory.GetFiles(tempimagePath);
                    foreach (string filePath in filePaths)
                    {
                        File.Delete(filePath);
                    }
                }
                if (ListAttachment.Count > 0)
                {
                    string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolder\";
                    if (File.Exists(tempfolderPath))
                    {
                        string[] filePath1 = Directory.GetFiles(tempfolderPath);
                        foreach (string filePath2 in filePath1)
                        {
                            File.Delete(filePath2);

                        }
                    }
                }
            }
        }

        public void AddAutoComment(string AttendiesString)
        {
            try
            {
                LogEntriesByActivity comment = new LogEntriesByActivity()
                {
                    //People = new People { IdPerson = GeosApplication.Instance.ActiveUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                    IdUser = 164,//GeosApplication.Instance.ActiveUser.IdUser,
                    Datetime = GeosApplication.Instance.ServerDateTime,
                    //Comments = System.Windows.Application.Current.FindResource("AutoComment").ToString() + AttendiesString, //string.Copy(NewActivityComment.Trim()),                    
                    Comments = string.Format(System.Windows.Application.Current.FindResource("AutoComment").ToString(), AttendiesString),
                    IdLogEntryType = 1,
                    IsUpdated = false,
                    IsDeleted = false,
                    IsRtfText = false
                };
                //comment.People.OwnerImage = SetUserProfileImage();
                ActivityCommentsList.Add(comment);
                //ActivityCommentsList = new ObservableCollection<LogEntriesByActivity>(ActivityCommentsList.OrderByDescending(c => c.Datetime).ToList());
                //SelectedComment = comment;
            }
            catch (Exception ex)
            {

            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  This method is for to get image in bitmap by path.
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
        private ImageSource SetUserProfileImage()
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

        /// <summary>
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// </summary>
        /// <param name="obj"></param>
        private void LinkedItemDoubleClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LinkedItemDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

                ActivityLinkedItem linkedItem = obj as ActivityLinkedItem;
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
                                {//
                                    //ListPlantOpportunityContain.AddRange(CrmStartUp.GetOffersByIdSiteToLinkedWithActivities(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyPlantList[selectedIndexCompanyPlant].IdCompany));
                                    //ListPlantOpportunityContain.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2150(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer));
                                    //[pramod.misal][GEOS2-5347][16.02.2024]
                                    ListPlantOpportunityContain.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2490(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer));
                                }
                                foreach (var linkedItems in SelectedActivityLinkedItems)
                                {
                                    Offer offer = ListPlantOpportunityContain.FirstOrDefault(x => x.Code == linkedItem.Name && linkedItem.LinkedItemType.IdLookupValue == 44);
                                    if (offer == null)
                                    {
                                        SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.Name == linkedItem.Name && linkedItem.LinkedItemType.IdLookupValue == 44));
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

        #region Attachments

        /// <summary>
        /// Method For Deleting file
        /// </summary>
        public void DeleteAttachmentRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAttachmentRowCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool isDelete = false;
                ActivityAttachment attachmentObject = (ActivityAttachment)parameter;

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ActivityFileDeleteMessage"].ToString(), "#33B4FB", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ListAttachment != null && ListAttachment.Count > 0)
                    {
                        ListAttachment.Remove((ActivityAttachment)attachmentObject);
                        isDelete = true;
                    }

                    if (isDelete)
                    {
                        IsBusy = false;
                        GeosApplication.Instance.Logger.Log("Method DeleteAttachmentRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    IsBusy = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteAttachmentRowCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteAttachmentRowCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DeleteAttachmentRowCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Upload File
        /// </summary>
        public bool UploadActivityAttachment()
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
                FileUploader activityAttachmentFileUploader = new FileUploader();
                activityAttachmentFileUploader.FileUploadName = GUIDCode.GUIDCodeString();
                GuidCode = activityAttachmentFileUploader.FileUploadName;

                if (ListAttachment != null && ListAttachment.Count > 0)
                {
                    foreach (ActivityAttachment fs in ListAttachment)
                    {
                        FileInfo file = new FileInfo(fs.FilePath);
                        fs.AttachmentImage = null;
                        FileDetail.Add(file);
                    }
                    activityAttachmentFileUploader.FileByte = ConvertZipToByte(FileDetail, activityAttachmentFileUploader.FileUploadName);
                    GeosApplication.Instance.Logger.Log("Getting Upload activity Attachment FileUploader Zip File ", category: Category.Info, priority: Priority.Low);
                    fileUploadReturnMessage = GeosRepositoryServiceController.UploaderActivityAttachmentZipFile(activityAttachmentFileUploader);
                    GeosApplication.Instance.Logger.Log("Getting Upload activity Attachment FileUploader Zip File successfully", category: Category.Info, priority: Priority.Low);
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
        /// Method for Upload File
        /// </summary>
        public void UploadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                //List<FileInfo> FileDetail = new List<FileInfo>();
                if (ActivityAttachmentList != null)
                {
                    foreach (ActivityAttachment item in ActivityAttachmentList)
                    {
                        //string add = item.OriginalFileName + item.FileType;
                        //item.FilePath = add;
                        string[] noextension = item.OriginalFileName.Split('.');
                        item.OriginalFileName = noextension[0].ToString();
                        ListAttachment.Add(item);
                    }
                }
                ActivityAttachmentList = new List<object>();
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
        /// Method for Download File
        /// </summary>
        public void DownloadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                bool isDownload = true;
                if (isDownload)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDownloadSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDownloadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                    var newFileList = ActivityAttachmentList != null ? new List<object>(ActivityAttachmentList) : new List<object>();
                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                    Attachment = new ActivityAttachment();
                    Attachment.FileType = file.Extension;
                    Attachment.FilePath = file.FullName;
                    Attachment.OriginalFileName = file.Name;
                    Attachment.SavedFileName = UniqueFileName + file.Extension;
                    Attachment.UploadedIn = DateTime.Now;
                    Attachment.FileSize = file.Length;
                    Attachment.FileType = file.Extension;
                    Attachment.FileUploadName = UniqueFileName + file.Extension;
                    Attachment.IsDeleted = 0;
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
                    List<ActivityAttachment> fooList = newFileList.OfType<ActivityAttachment>().ToList();

                    if (!fooList.Any(x => x.OriginalFileName == Attachment.OriginalFileName))
                    {
                        newFileList.Add(Attachment);
                    }
                    ActivityAttachmentList = newFileList;
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
                            if (!File.Exists(tempfolderPath + ListAttachment[i].FileUploadName))
                            {
                                System.IO.File.Copy(filesDetail[i].FullName, tempfolderPath + ListAttachment[i].FileUploadName);
                            }

                            string s = tempfolderPath + ListAttachment[i].FileUploadName;
                            archive.AddFile(s, @"/");
                            ListAttachment[i].FilePath = s;
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

        #endregion

        #region Comments

        private void CommentButtonCheckedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method convert CommentButtonCheckedCommandAction ...", category: Category.Info, priority: Priority.Low);

            // if (string.IsNullOrEmpty(OldActivityComment))
            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            NewActivityComment = "";
            OldActivityComment = "";
            IsRtf = false;
            IsNormal = true;
            if (IsNormal == true)
            {
                Textboxnormal = Visibility.Visible;
                Richtextboxrtf = Visibility.Collapsed;
            }
            else
            {
                Textboxnormal = Visibility.Collapsed;
                Richtextboxrtf = Visibility.Visible;
            }

            GeosApplication.Instance.Logger.Log("Method CommentButtonCheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void CommentButtonUncheckedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method convert CommentButtonCheckedCommandAction ...", category: Category.Info, priority: Priority.Low);

            //if (string.IsNullOrEmpty(OldActivityComment))
            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            NewActivityComment = null;
            OldActivityComment = null;
            IsRtf = false;
            IsNormal = true;
            if (IsNormal == true)
            {
                Textboxnormal = Visibility.Visible;
                Richtextboxrtf = Visibility.Collapsed;
            }
            else
            {
                Textboxnormal = Visibility.Collapsed;
                Richtextboxrtf = Visibility.Visible;
            }

            GeosApplication.Instance.Logger.Log("Method CommentButtonCheckedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void CommentDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method convert CommentDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (obj == null) return;

            LogEntriesByActivity commentOffer = (LogEntriesByActivity)obj;

            if (commentOffer.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
            {
                CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewUpdateComment").ToString();
                IsAdd = false;
                ShowCommentsFlyout = true;
                OldActivityComment = String.Copy(commentOffer.Comments);
                NewActivityComment = String.Copy(commentOffer.Comments);
                if (commentOffer.IsRtfText == true)
                    IsRtf = true;
                else
                    IsNormal = true;

            }
            else
            {
                ShowCommentsFlyout = false;
                NewActivityComment = null;
                OldActivityComment = null;
                CustomMessageBox.Show("Not Allowed to update comment!", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for Add Comment
        /// </summary>
        /// <param name="leadComment"></param>
        public void AddCommentCommandAction(object gcComments)
        {
            GeosApplication.Instance.Logger.Log("Method convert AddCommentCommandAction ...", category: Category.Info, priority: Priority.Low);
            //IsBusy = true;   
            if (IsRtf)
            {

                var document = ((RichTextBox)gcComments).Document;
                NewActivityComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
                string convertedText = string.Empty;
                if (!string.IsNullOrEmpty(NewActivityComment.Trim()))
                {
                    //if (IsRtf)
                    //{
                    using (MemoryStream ms = new MemoryStream())
                    {
                        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                        range2.Save(ms, DataFormats.Rtf);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            convertedText = sr.ReadToEnd();
                        }
                    }

                }
                //else if (IsNormal)
                //{
                //    using (MemoryStream ms = new MemoryStream())
                //    {
                //        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                //        range2.Save(ms, DataFormats.Text);
                //        ms.Seek(0, SeekOrigin.Begin);
                //        using (StreamReader sr = new StreamReader(ms))
                //        {
                //            convertedText = sr.ReadToEnd();
                //        }
                //    }
                //}
                // }
                NewActivityComment = convertedText;
            }
            //NewActivityComment = NewActivityComment;
            if (OldActivityComment != null && !string.IsNullOrEmpty(OldActivityComment.Trim()) && OldActivityComment.Equals(NewActivityComment.Trim()))
            {
                ShowCommentsFlyout = false;
                return;
            }

            // Update comment.
            if (CommentButtonText == System.Windows.Application.Current.FindResource("LeadsEditViewUpdateComment").ToString())
            {
                if (!string.IsNullOrEmpty(NewActivityComment) && !string.IsNullOrEmpty(NewActivityComment.Trim()))
                {
                    LogEntriesByActivity comment = ActivityCommentsList.FirstOrDefault(x => x.Comments == OldActivityComment);
                    //comment.Comments = string.Copy(NewActivityComment.Trim());
                    //SelectedComment = comment;
                    if (comment != null)
                    {
                        comment.Comments = string.Copy(NewActivityComment.Trim());
                        comment.Datetime = GeosApplication.Instance.ServerDateTime;
                        SelectedComment = comment;
                        if (comment.IdLogEntryByActivity != 0)
                            comment.IsUpdated = true;
                        else
                            comment.IsUpdated = false;
                        comment.IsDeleted = false;
                        comment.IsRtfText = comment.IsRtfText;
                        if (IsRtf)
                        {
                            comment.IsRtfText = true;
                            RtfToPlaintext();
                        }
                        else if (IsNormal)
                        {
                            comment.IsRtfText = false;
                            CommentText = comment.Comments;
                        }
                    }

                    OldActivityComment = null;
                    NewActivityComment = null;

                    // ActivityCommentsList = new ObservableCollection<LogEntriesByActivity>(ActivityCommentsList);
                }
                ActivityCommentsList = new ObservableCollection<LogEntriesByActivity>(ActivityCommentsList.OrderByDescending(c => c.Datetime).ToList());
            }
            else if (CommentButtonText == System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString()) //Add comment.
            {
                if (!string.IsNullOrEmpty(NewActivityComment) && !string.IsNullOrEmpty(NewActivityComment.Trim())) // Add Comment
                {
                    //LogEntriesByActivity comment = new LogEntriesByActivity()
                    //{
                    //    People = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                    //    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                    //    Datetime = GeosApplication.Instance.ServerDateTime,
                    //    Comments = string.Copy(NewActivityComment.Trim()),
                    //    IdLogEntryType = 1
                    //};

                    //ActivityCommentsList.Add(comment);
                    if (IsRtf)
                    {
                        LogEntriesByActivity comment = new LogEntriesByActivity()
                        {
                            People = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Copy(NewActivityComment.Trim()),
                            IdLogEntryType = 1,
                            IsUpdated = false,
                            IsDeleted = false,
                            IsRtfText = true
                        };
                        comment.People.OwnerImage = SetUserProfileImage();
                        ActivityCommentsList.Add(comment);

                        SelectedComment = comment;
                        RtfToPlaintext();
                        ActivityCommentsList = new ObservableCollection<LogEntriesByActivity>(ActivityCommentsList.OrderByDescending(c => c.Datetime).ToList());

                    }
                    else if (IsNormal)
                    {
                        LogEntriesByActivity comment = new LogEntriesByActivity()
                        {
                            People = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Copy(NewActivityComment.Trim()),
                            IdLogEntryType = 1,
                            IsUpdated = false,
                            IsDeleted = false,
                            IsRtfText = false
                        };
                        comment.People.OwnerImage = SetUserProfileImage();
                        ActivityCommentsList.Add(comment);
                        SelectedComment = comment;
                        ActivityCommentsList = new ObservableCollection<LogEntriesByActivity>(ActivityCommentsList.OrderByDescending(c => c.Datetime).ToList());
                    }

                    OldActivityComment = null;
                    NewActivityComment = null;
                }
            }
            //document.Blocks.Clear();
            ShowCommentsFlyout = false;
            IsRtf = false;
            IsNormal = true;
            //IsBusy = false;
            //((GridControl)gcComments).Focus();

            GeosApplication.Instance.Logger.Log("Method AddCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Deleting Comments
        /// </summary>
        /// <param name="obj"></param>
        public void DeleteCommentCommandAction(object parameter)
        {
            GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

            LogEntriesByActivity commentObject = (LogEntriesByActivity)parameter;

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (commentObject.IdUser != GeosApplication.Instance.ActiveUser.IdUser)
            {
                CustomMessageBox.Show("Not Allowed to delete Comment!", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteComment").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                if (ActivityCommentsList != null && ActivityCommentsList.Count > 0)
                {
                    foreach (LogEntriesByActivity item in ActivityCommentsList.ToList())
                    {
                        if (item.Comments == commentObject.Comments)
                        {
                            item.IsDeleted = true;
                            ActivityCommentsList.Remove((LogEntriesByActivity)commentObject);
                        }
                    }
                }
            }

            ShowCommentsFlyout = false;
            NewActivityComment = null;

            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void RtfToPlaintext()
        {
            TextRange range = null;
            if (ActivityCommentsList.Count > 0)
            {
                if (ActivityCommentsList[0].IsRtfText)
                {
                    var rtb = new RichTextBox();
                    var doc = new FlowDocument();
                    MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(ActivityCommentsList[0].Comments.ToString()));
                    range = new TextRange(doc.ContentStart, doc.ContentEnd);
                    range.Load(stream, DataFormats.Rtf);
                }
                else
                {
                    CommentText = ActivityCommentsList[0].Comments.ToString();
                }
            }

            if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                CommentText = range.Text;
        }

        #endregion


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
        #endregion // Methods

        #region validation

        bool allowValidation = false;
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
                string error = null;
                if (IsInformationVisible == Visibility.Visible)
                {
                    IDataErrorInfo me = (IDataErrorInfo)this;
                    error =
                       me[BindableBase.GetPropertyName(() => Description)] +
                       me[BindableBase.GetPropertyName(() => SelectedIndexType)] +

                    #region ToDate, FromDate &StartTime,EndTime Commented
                     //me[BindableBase.GetPropertyName(() => FromDate)] +
                     //me[BindableBase.GetPropertyName(() => ToDate)] +
                     //me[BindableBase.GetPropertyName(() => StartTime)] +
                     //me[BindableBase.GetPropertyName(() => EndTime)] +
                     //me[BindableBase.GetPropertyName(() => SelectedIndexOwner)] +
                    #endregion
                     me[BindableBase.GetPropertyName(() => SelectedActivityOwnerList)] +
                       me[BindableBase.GetPropertyName(() => ActivityAddress)] +
                       me[BindableBase.GetPropertyName(() => Subject)] +
                       me[BindableBase.GetPropertyName(() => AttendeesCount)] +
                       me[BindableBase.GetPropertyName(() => DueDate)] +
                       me[BindableBase.GetPropertyName(() => SelectedIndexTaskStatus)] +
                       me[BindableBase.GetPropertyName(() => AddActivityError)] +
                       me[BindableBase.GetPropertyName(() => SelectedAccountActivityLinkedItemsCount)];
                }
                else
                {
                    IDataErrorInfo me = (IDataErrorInfo)this;
                    error =
                       me[BindableBase.GetPropertyName(() => Description)] +
                       me[BindableBase.GetPropertyName(() => SelectedIndexType)] +
                       me[BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup)] +
                       me[BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant)] +
                    #region ToDate, FromDate &StartTime,EndTime Commented
                     //me[BindableBase.GetPropertyName(() => FromDate)] +
                     //me[BindableBase.GetPropertyName(() => ToDate)] +
                     //me[BindableBase.GetPropertyName(() => StartTime)] +
                     //me[BindableBase.GetPropertyName(() => EndTime)] +
                     //me[BindableBase.GetPropertyName(() => SelectedIndexOwner)] +
                    #endregion
                     me[BindableBase.GetPropertyName(() => SelectedActivityOwnerList)] +
                       me[BindableBase.GetPropertyName(() => ActivityAddress)] +
                       me[BindableBase.GetPropertyName(() => Subject)] +
                       me[BindableBase.GetPropertyName(() => AttendeesCount)] +
                       me[BindableBase.GetPropertyName(() => DueDate)] +
                       me[BindableBase.GetPropertyName(() => SelectedIndexTaskStatus)] +
                       me[BindableBase.GetPropertyName(() => AddActivityError)] +
                       me[BindableBase.GetPropertyName(() => SelectedAccountActivityLinkedItemsCount)];
                }


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

                string descriptionProp = BindableBase.GetPropertyName(() => Description);
                string selectedIndexType = BindableBase.GetPropertyName(() => SelectedIndexType);   // Lead Source
                string selectedIndexCompanyGroup = BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup);
                string selectedIndexCompanyPlant = BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant);
                #region ToDate, FromDate &StartTime,EndTime Commented
                //string fromdateProp = BindableBase.GetPropertyName(() => FromDate);
                //string toDateProp = BindableBase.GetPropertyName(() => ToDate);
                //string startTimeProp = BindableBase.GetPropertyName(() => StartTime);
                //string endTimeProp = BindableBase.GetPropertyName(() => EndTime);
                //string selectedIndexOwnerProp = BindableBase.GetPropertyName(() => SelectedIndexOwner);
                #endregion
                string selectedActivityOwnerListProp = BindableBase.GetPropertyName(() => SelectedActivityOwnerList);
                string subjectProp = BindableBase.GetPropertyName(() => Subject);
                string activityAddressProp = BindableBase.GetPropertyName(() => ActivityAddress);
                string attendeesCountProp = BindableBase.GetPropertyName(() => AttendeesCount);
                string dueDateProp = BindableBase.GetPropertyName(() => DueDate);
                string selectedIndexTaskStatusProp = BindableBase.GetPropertyName(() => SelectedIndexTaskStatus);
                string selectedAccountActivityLinkedItemsCountProp = BindableBase.GetPropertyName(() => SelectedAccountActivityLinkedItemsCount);
                string addActivityErrorProp = BindableBase.GetPropertyName(() => AddActivityError);

                if (columnName == selectedIndexType)    //Type
                {
                    return ActivityValidation.GetErrorMessage(selectedIndexType, SelectedIndexType);
                }
                else if (columnName == selectedIndexCompanyGroup && IsAccountVisible == Visibility.Visible && IsInformationVisible == Visibility.Collapsed)     //Group
                {
                    return ActivityValidation.GetErrorMessage(selectedIndexCompanyGroup, SelectedIndexCompanyGroup);
                }
                else if (columnName == selectedIndexCompanyPlant && IsAccountVisible == Visibility.Visible && IsInformationVisible == Visibility.Collapsed)     //Plant
                {
                    return ActivityValidation.GetErrorMessage(selectedIndexCompanyPlant, SelectedIndexCompanyPlant);
                }
                else if (columnName == subjectProp)     //Subject
                {
                    return ActivityValidation.GetErrorMessage(subjectProp, Subject);
                }
                else if (columnName == descriptionProp) //Description
                {
                    return ActivityValidation.GetErrorMessage(descriptionProp, Description);
                }
                #region ToDate, FromDate &StartTime,EndTime Commented
                //else if (columnName == fromdateProp && IsAppointmentVisible == Visibility.Visible)  //From date
                //{
                //    int result = DateTime.Compare(FromDate.Date, ToDate.Date);

                //    if (result > 0)
                //        return "The date you entered occurs after the end date.";
                //    //else if (result == 0)
                //    //    return "From Date is the same time as To Date";
                //}
                //else if (columnName == startTimeProp && IsAppointmentVisible == Visibility.Visible) //Start Time.
                //{
                //    if (DateTime.Compare(FromDate.Date, ToDate.Date) == 0) //Same date.
                //    {
                //        int result = DateTime.Compare(StartTime, EndTime);
                //        if (result > 0)
                //            return "The time you entered occurs after the end time.";
                //    }
                //    //return ActivityValidation.GetErrorMessage(startTimeProp, StartTime);
                //}
                //else if (columnName == toDateProp && IsAppointmentVisible == Visibility.Visible)    //To date.
                //{
                //    int result = DateTime.Compare(ToDate.Date, FromDate.Date);

                //    if (result < 0)
                //        return "The date you entered occurs before the start date.";
                //    //else if (result == 0)
                //    //    return "To Date is the same time as From";
                //}
                //else if (columnName == endTimeProp && IsAppointmentVisible == Visibility.Visible)   //End time.
                //{
                //    if (DateTime.Compare(ToDate.Date, FromDate.Date) == 0) //Same date.
                //    {
                //        int result = DateTime.Compare(EndTime, StartTime);
                //        if (result < 0)
                //            return "The time you entered occurs before the start time.";
                //    }
                //    //return ActivityValidation.GetErrorMessage(endTimeProp, EndTime);
                //}
                #endregion
                else if (columnName == activityAddressProp && IsAppointmentVisible == Visibility.Visible)   //Address.
                {
                    return ActivityValidation.GetErrorMessage(activityAddressProp, ActivityAddress);
                }
                else if (columnName == attendeesCountProp && IsAppointmentVisible == Visibility.Visible)    //Attendies.
                {
                    return ActivityValidation.GetErrorMessage(attendeesCountProp, AttendeesCount);
                }
                else if (columnName == dueDateProp && IsDueDateVisible == Visibility.Visible)               //Due date.
                {
                    return ActivityValidation.GetErrorMessage(dueDateProp, DueDate);
                }
                else if (columnName == selectedIndexTaskStatusProp && IsTaskVisible == Visibility.Visible)  //Status.
                {
                    return ActivityValidation.GetErrorMessage(selectedIndexTaskStatusProp, SelectedIndexTaskStatus);
                }
                //else if (columnName == selectedIndexOwnerProp)  //Owner.
                //{
                //    return ActivityValidation.GetErrorMessage(selectedIndexOwnerProp, SelectedIndexOwner);
                //}
                else if (columnName == selectedActivityOwnerListProp)  //Owner.
                {
                    return ActivityValidation.GetErrorMessage(selectedActivityOwnerListProp, SelectedActivityOwnerList);
                }
                else if (columnName == selectedAccountActivityLinkedItemsCountProp && IsInternal == false && IsInformationVisible == Visibility.Collapsed)  //linked items.
                {
                    return ActivityValidation.GetErrorMessage(selectedAccountActivityLinkedItemsCountProp, SelectedAccountActivityLinkedItemsCount);
                }
                else if (columnName == addActivityErrorProp)
                {
                    return ActivityValidation.GetErrorMessage(addActivityErrorProp, AddActivityError);
                }

                return null;
            }
        }

        #endregion
    }
}
