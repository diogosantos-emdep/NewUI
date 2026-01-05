using DevExpress.Compression;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Map;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.UI.Helper;
using System.Globalization;
using DevExpress.Mvvm.UI;
using System.Data;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Xpf.RichEdit;
using DevExpress.Xpf.Spreadsheet;
using System.Text.RegularExpressions;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class EditActivityViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {
        #region TaskLog

        //[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        //[GEOS2-267] Add all people as possible contact belonging to the group in activities [adadibathina]
        //[GEOS2-257](#70081) Different criteria for calculating Sleep days [adadibathina]
        #endregion

        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        #endregion // Services

        #region Declaration

        int windowHeight1;
        private int screenHeight;
        private int windowHeight;
        private int commentboxHeight;
        IServiceContainer serviceContainer = null;
        byte[] UserProfileImageByte = null;
        private DateTime fromDate = DateTime.Now;
        private DateTime toDate = DateTime.Now;
        private List<DateTime> _FromDates;
        private DateTime startTime;
        private List<DateTime> _ToDates;
        private DateTime endTime;
        private bool isAllDayEvent;
        private bool invertIsAllDayEvent;
        private string reminderButtonText;
        private string reminderCalenderString;
        private TimeSpan reminderTimeMinValue;
        private List<DateTime> reminderDates;
        private DateTime reminderTime;
        private DateTime? reminderDate;
        private bool isCompleted;
        private DateTime? dueDate;

        private int selectedIndexTaskStatus;
        private string addTagVisibility;
        private int selectedIndexType;
        private LookupValue selectedActivityType;
        private Visibility isAppointmentVisible;
        private Visibility isTagVisible;
        private Visibility isEmailorCallVisible;
        private Visibility isTaskVisible;
        private Visibility isDueDateVisible;
        private Visibility isCompletedVisible;
        private Visibility isWatcherVisible;
        private Visibility isWatcherUserVisible;
        private Visibility isWatcherRegionVisible;
        private Visibility isInformationVisible;
        private Visibility isCommentsVisible;
        private Int32 columnMainWidth;
        private Int32 columnWidth;
        private string description;
        private string subject;
        private bool isBusy;
        private GeoPoint mapLatitudeAndLongitude;
        private bool isCoordinatesNull;
        private double? latitude;
        private double? longitude;
        private string activityAddress;
        private Activity activity;

        private ObservableCollection<People> attendiesList;
        private ObservableCollection<People> selectedAttendiesList;
        private ObservableCollection<People> watchersUserList;
        private ObservableCollection<LookupValue> watchersRegionList;
        private ObservableCollection<People> watchersList;
        private ObservableCollection<People> selectedWatchersList;
        private ObservableCollection<People> selectedWatchersUserList = new ObservableCollection<People>();
        private ObservableCollection<LookupValue> selectedWatchersRegionList = new ObservableCollection<LookupValue>();
        private object attendeesCount;
        private object watchersCount;
        private List<User> activityOwnerList;
        private int selectedIndexOwner;

        private ObservableCollection<Customer> companyGroupList;
        private ObservableCollection<Company> companyPlantList;
        private int selectedIndexCompanyGroup;
        private int selectedIndexCompanyPlant;
        private ObservableCollection<Offer> listPlantOpportunity;
        private ObservableCollection<Offer> listPlantOpportunityCopy;
        public bool isInit = false;
        private bool isEditedFromOutSide = false;
        private bool isConfirm = false;

        //Comments
        private ObservableCollection<LogEntriesByActivity> activityCommentsList;
        private Object selectedComment;
        private bool showCommentsFlyout;
        private string oldActivityComment;
        private string newActivityComment;
        private string commentButtonText;

        private bool isRtf;
        private bool isNormal = true;
        private ImageSource userProfileImage;
        private bool isAdd;
        private string commentText;

        //End comments

        //Change log
        private ObservableCollection<LogEntriesByActivity> activityChangeLogList;


        private ObservableCollection<CarProject> carProjectList;
        //   private List<CarProject> carProjectListCopy;
        private ObservableCollection<People> linkeditemsPeopleList;
        // private List<People> linkeditemsPeopleListCopy;
        private ObservableCollection<Competitor> linkeditemsCompetitorList;
        // private List<Competitor> linkeditemsCompetitorListCopy;
        private ObservableCollection<ActivityLinkedItem> selectedActivityLinkedItems;
        private int selectedAccountActivityLinkedItemsCount;
        private bool isPODone;
        //End Linked Items

        // Attachment
        private string uniqueFileName;
        private string fileNameString;
        private ActivityAttachment attachment;
        private String fileName;
        private List<Object> activityAttachmentList;
        //private List<Object> attachmentGridList;
        private ObservableCollection<ActivityAttachment> listAttachment;
        private ObservableCollection<ActivityAttachment> tempListAttachment;
        private string timeEditMask;
        private DateTime reminderDateMinValue;

        //tag
        private int selectedIndexTag;
        private List<Tag> tagList;
        private List<Object> selectedTagList;
        private string tagsString;
        private bool isUserWatcherReadOnly;
        private bool isGroupPlantReadOnly;
        private bool isUserWatcherEnabled;
        private string attendiesString;
        private bool isInternal;
        private Visibility isInternalVisible;
        private Visibility isAccountVisible;
        private Visibility isOwnerVisible;
        private bool isInternalEnable;
        private bool isInfoAccountRemove = false;
        private List<LogEntriesByActivity> listOfChangeLogUpdateComments;
        private bool isPlannedAppointment;
        private bool isPlannedAppointmentReadOnly;

        private Visibility textboxnormal;
        private Visibility richtextboxrtf;
        private ImageSource linkedItemImage;
        private Visibility isCompletedCheckVisible;
        private ObservableCollection<Company> entireCompanyPlantList;

        private bool isSearchButtonEnable = true;


        private string editActivityError;
        private bool isActivityAddressReadOnly = true;
        private Visibility isContactVisible;
        private bool isPermissionCustomerEdit=true;//[pallavi.kale][GEOS2-8961][07.11.2025]
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
        public Visibility IsContactVisible
        {
            get { return isContactVisible; }
            set
            {
                isContactVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsContactVisible"));
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
        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }
        public Visibility IsCompletedCheckVisible
        {
            get
            {
                return isCompletedCheckVisible;
            }

            set
            {
                isCompletedCheckVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCompletedCheckVisible"));
            }
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

        public bool IsConfirm
        {
            get
            {
                return isConfirm;
            }

            set
            {
                isConfirm = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsConfirm"));
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

        public bool IsPlannedAppointmentReadOnly
        {
            get
            {
                return isPlannedAppointmentReadOnly;
            }

            set
            {
                isPlannedAppointmentReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPlannedAppointmentReadOnly"));
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

        public int SelectedIndexCompanyGroup
        {

            get { return selectedIndexCompanyGroup; }
            set
            {
                //Thread BackgroundThread = new Thread();
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
                                FillContactsList();
                                SelectedIndexCompanyPlant = 0;
                            }

                        }
                        else
                        {
                            SelectedIndexCompanyPlant = -1;
                            CompanyPlantList = null;
                            ListPlantOpportunity = null;
                            if (!isInit)
                            {
                                if (IsInternal)
                                {
                                }
                                else
                                {
                                    ActivityAddress = string.Empty;
                                    MapLatitudeAndLongitude = null;
                                    Latitude = null;
                                    Longitude = null;
                                }
                            }
                            if (!IsEditedFromOutSide)
                                DeleteLinkedItem();
                        }
                    }

                    if (_Activity.IdOwner != GeosApplication.Instance.ActiveUser.IdUser)
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
                    // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }
            }
        }

        public int SelectedIndexCompanyPlant
        {
            get { return selectedIndexCompanyPlant; }
            set
            {
                selectedIndexCompanyPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                if (IsUserWatcherReadOnly == false)
                {

                    if (SelectedIndexCompanyPlant > 0)
                    {
                        Company plantObj = new Company();
                        plantObj = CompanyPlantList[SelectedIndexCompanyPlant];
                        Customer groupObj = new Customer();
                        groupObj = CompanyGroupList[SelectedIndexCompanyGroup];
                        AddGroupPlantAsLinkedItem(plantObj, groupObj);

                    }
                    else
                    {
                        if (!isEditedFromOutSide)
                            DeleteLinkedItem();

                        if (!isInit)
                        {
                            if (IsInternal)
                            {
                            }
                            else
                            {
                                ActivityAddress = string.Empty;
                                MapLatitudeAndLongitude = null;
                                Latitude = null;
                                Longitude = null;
                            }
                        }
                    }
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

                                    SelectedIndexCompanyGroup = 0;
                                    // SelectedIndexCompanyPlant = 0;
                                    ActivityAddress = string.Empty;
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
                    }
                    else
                    {
                        IsAccountVisible = Visibility.Visible;
                    }
                    //if (GeosApplication.Instance.IsPermissionAuditor)
                    //{

                    //    GetPlannedActivitiesOfOwnerForSelectedIndex();
                    //}
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in IsInternal Property " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }
        public string AttendiesString
        {
            get { return attendiesString; }
            set { attendiesString = value; OnPropertyChanged(new PropertyChangedEventArgs("AttendiesString")); }
        }
        public string TagsString
        {
            get { return tagsString; }
            set { tagsString = value; OnPropertyChanged(new PropertyChangedEventArgs("TagsString")); }
        }
        public bool IsPODone
        {
            get { return isPODone; }
            set { isPODone = value; OnPropertyChanged(new PropertyChangedEventArgs("IsPODone")); }
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

        public bool IsGroupPlantReadOnly
        {
            get
            {
                return isGroupPlantReadOnly;
            }

            set
            {
                isGroupPlantReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGroupPlantReadOnly"));
            }
        }

        public bool IsUserWatcherEnabled
        {
            get
            {
                return isUserWatcherEnabled;
            }

            set
            {
                isUserWatcherEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUserWatcherEnabled"));
            }
        }

        public TimeSpan ReminderTimeMinValue
        {
            get { return reminderTimeMinValue; }
            set { reminderTimeMinValue = value; OnPropertyChanged(new PropertyChangedEventArgs("ReminderTimeMinValue")); }
        }
        public DateTime ReminderDateMinValue
        {
            get { return reminderDateMinValue; }
            set { reminderDateMinValue = value; OnPropertyChanged(new PropertyChangedEventArgs("ReminderDateMinValue")); }
        }
        public string ReminderCalenderString
        {
            get { return reminderCalenderString; }
            set { reminderCalenderString = value; OnPropertyChanged(new PropertyChangedEventArgs("ReminderCalenderString")); }
        }

        public List<DateTime> ReminderDates
        {
            get { return reminderDates; }
            set { reminderDates = value; OnPropertyChanged(new PropertyChangedEventArgs("ReminderDates")); }
        }

        public DateTime? ReminderDate
        {
            get { return reminderDate; }
            set
            {
                reminderDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReminderDate"));

                //if (ToDate.Date < ReminderDate.Value.Date)
                //{
                //    DateTime dateTime = new DateTime(ReminderDate.Value.Year, ReminderDate.Value.Month, ReminderDate.Value.Day, ReminderTime.Hour, ReminderTime.Minute, ReminderTime.Second);
                //    ToDate = dateTime;
                //}
            }
        }

        public DateTime ReminderTime
        {
            get { return reminderTime; }
            set
            {
                reminderTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReminderTime"));
                if (ReminderDate != null)
                {
                    DateTime dateTime = new DateTime(ReminderDate.Value.Year, ReminderDate.Value.Month, ReminderDate.Value.Day, ReminderTime.Hour, ReminderTime.Minute, ReminderTime.Second);
                    ReminderDate = dateTime;
                }
            }
        }
        public string ReminderButtonText
        {
            get { return reminderButtonText; }
            set { reminderButtonText = value; OnPropertyChanged(new PropertyChangedEventArgs("ReminderButtonText")); }
        }

        public ObservableCollection<CarProject> CarProjectList
        {
            get { return carProjectList; }
            set
            {
                carProjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarProjectList"));
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


        public ObservableCollection<Competitor> LinkeditemsCompetitorList
        {
            get { return linkeditemsCompetitorList; }
            set
            {
                linkeditemsCompetitorList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkeditemsCompetitorList"));
            }
        }

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

        public Activity _Activity { get; set; }
        public Activity objActivity { get; set; }

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
                activityAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityAddress"));
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

        public Activity Activities
        {
            get { return activity; }
            set
            {
                activity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Activities"));
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

        public List<LookupValue> TypeList { get; set; }

        public LookupValue SelectedActivityType
        {
            get { return selectedActivityType; }
            set
            {
                selectedActivityType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
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

        public Visibility IsDueDateVisible
        {
            get { return isDueDateVisible; }
            set
            {
                isDueDateVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDueDateVisible"));
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

        public Visibility IsWatcherVisible
        {
            get { return isWatcherVisible; }
            set
            {
                isWatcherVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWatcherVisible"));
            }
        }


        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));


            }
        }

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));


            }
        }
        //CRM-M052-17
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                if (!string.IsNullOrEmpty(value))
                {
                    description = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                }
            }
        }
        //CRM-M052-17
        public string Subject
        {
            get { return subject; }
            set
            {


                subject = value;
                if (value != null)
                {
                    subject = value.TrimStart();

                }
                OnPropertyChanged(new PropertyChangedEventArgs("Subject"));

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

        public static List<LookupValue> TaskStatusList { get; set; }

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

        //private List<People> AttendiesListCopy { get; set; }

        //private List<People> WatchersListCopy { get; set; }

        public ObservableCollection<People> SelectedAttendiesList
        {
            get { return selectedAttendiesList; }
            set
            {
                selectedAttendiesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttendiesList"));
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
        //End Comment

        //Change log
        public ObservableCollection<LogEntriesByActivity> ActivityChangeLogList
        {
            get { return activityChangeLogList; }
            set { activityChangeLogList = value; }
        }

        public List<LogEntriesByActivity> ChangedLogsEntries { get; set; }
        //End Change log

        ///Activity Owner
        public List<User> ActivityOwnerList
        {
            get { return activityOwnerList; }
            set
            {
                activityOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityOwnerList"));
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

        //Activity Open
        public string ModifiedORCompleted { get; set; }
        public string OpenORCompleted { get; set; }
        public DateTime? ModifiedInORCompletedIn { get; set; }
        public DateTime? CreatedIn { get; set; }
        public int ActivitySleepDays { get; set; }
        private DateTime? previousPlannedDate;
        private DateTime? nextPlannedDate;

        // Attachment
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
        public DateTime? PreviousPlannedDate
        {
            get { return previousPlannedDate; }
            set
            {
                previousPlannedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousPlannedDate"));
            }
        }


        public DateTime? NextPlannedDate
        {
            get { return nextPlannedDate; }
            set
            {
                nextPlannedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NextPlannedDate"));
            }
        }


        public string GuidCode { get; set; }


        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set { uniqueFileName = value; OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName")); }
        }

        public string FileNameString
        {
            get { return fileNameString; }
            set { fileNameString = value; OnPropertyChanged(new PropertyChangedEventArgs("FileNameString")); }
        }

        public ActivityAttachment Attachment
        {
            get { return attachment; }
            set { attachment = value; OnPropertyChanged(new PropertyChangedEventArgs("Attachment")); }
        }

        private ObservableCollection<ActivityAttachment> listUpdateActivityAttachment;

        public ObservableCollection<ActivityAttachment> ListUpdateActivityAttachment
        {
            get { return listUpdateActivityAttachment; }
            set { listUpdateActivityAttachment = value; OnPropertyChanged(new PropertyChangedEventArgs("ListAttachment")); }
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

        private bool isUpdateAttachment;

        public bool IsUpdateAttachment
        {
            get { return isUpdateAttachment; }
            set { isUpdateAttachment = value; OnPropertyChanged(new PropertyChangedEventArgs("IsUpdateAttachment")); }
        }

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



        public List<Object> SelectedTagList
        {
            get { return selectedTagList; }
            set
            {
                selectedTagList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTagList"));
            }
        }


        public List<Tag> TagList
        {
            get { return tagList; }
            set { tagList = value; OnPropertyChanged(new PropertyChangedEventArgs("TagList")); }
        }


        public int SelectedIndexTag
        {
            get { return selectedIndexTag; }
            set
            {
                selectedIndexTag = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexTag"));
            }
        }

        private string currentTimePatternString;

        public string CurrentTimePatternString
        {
            get { return currentTimePatternString; }
            set { currentTimePatternString = value; OnPropertyChanged(new PropertyChangedEventArgs("CurrentTimePatternString")); }
        }

        public List<LogEntriesByActivity> ListOfChangeLogUpdateComments
        {
            get
            {
                return listOfChangeLogUpdateComments;
            }

            set
            {
                listOfChangeLogUpdateComments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfChangeLogUpdateComments"));
            }
        }

        public string EditActivityError
        {
            get { return editActivityError; }
            set { editActivityError = value; OnPropertyChanged(new PropertyChangedEventArgs("EditActivityError")); }
        }

        public bool IsActivityAddressReadOnly
        {
            get
            {
                return isActivityAddressReadOnly;
            }

            set
            {
                isActivityAddressReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActivityAddressReadOnly"));
            }
        }

        //[pallavi.kale][GEOS2-8961][07.11.2025]
        public bool IsPermissionCustomerEdit
        {
            get { return isPermissionCustomerEdit; }
            set
            {
                isPermissionCustomerEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPermissionCustomerEdit"));
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


        public ICommand LoadedCommand { get; set; }
        public ICommand UpdateActivityViewAcceptButtonCommand { get; set; }
        public ICommand UpdateActivityViewCancelButtonCommand { get; set; }
        public ICommand SearchButtonClickCommand { get; set; }
        public ICommand SetPrimaryContactCommand { get; set; }
        public ICommand AddAttendeeRowMouseDoubleClickCommand { get; set; }
        public ICommand AttendeeCancelCommand { get; set; }
        public ICommand AddWatchersRowMouseDoubleClickCommand { get; set; }

        public ICommand AddWatchersUserRowMouseDoubleClickCommand { get; set; }

        public ICommand AddWatchersRegionRowMouseDoubleClickCommand { get; set; }

        public ICommand WatchersCancelCommand { get; set; }
        public ICommand AddTagButtonCommand { get; set; }
        public ICommand ActivityReminderAcceptCommand { get; set; }
        public ICommand ActivityReminderRemoveCommand { get; set; }

        public ICommand RichTextResizingCommand { get; set; }

        //public ICommand AccountCheckCellValueChangedCommand { get; set; }  // Not used
        //public ICommand HyperlinkForEmail { get; set; }

        //Linked items
        public ICommand ProjectRowMouseDoubleClickCommand { get; set; }
        public ICommand ContactRowMouseDoubleClickCommand { get; set; }
        public ICommand CompetitorRowMouseDoubleClickCommand { get; set; }

        //public ICommand AccountRowMouseDoubleClickCommand { get; set; }
        public ICommand OfferRowMouseDoubleClickCommand { get; set; }
        public ICommand LinkedItemCancelCommand { get; set; }
        public ICommand LinkedItemDoubleClickCommand { get; set; }

        //Comments
        public ICommand TypeSelectedIndexChangedCommand { get; set; }
        public ICommand CommentButtonCheckedCommand { get; set; }
        public ICommand CommentButtonUncheckedCommand { get; set; }
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand ExcelexportButtonCheckedCommand { get; set; }


        //Attachments
        public ICommand ChooseFileCommand { get; set; }
        public ICommand UploadFileCommand { get; set; }
        public ICommand DownloadFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand AttachmentGridDoubleClickCommand { get; set; }

        public ICommand IsnormalPreviewMouseRightButtonDown { get; set; }

        public ICommand SearchOfferCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion // Commands

        #region Constructor

        public EditActivityViewModel()
        {
            screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height-100;          
            WindowHeight = screenHeight - 340;
            CommentboxHeight = screenHeight - 550;
            ColumnMainWidth = 920;
            ColumnWidth = 350;
            WindowHeight1 = screenHeight - 350;
            GeosApplication.Instance.Logger.Log("Constructor EditActivityViewModel() ...", category: Category.Info, priority: Priority.Low);
            TimeEditMask = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

            //LoadedCommand = new DelegateCommand<object>(LoadedCommandAction);
            UpdateActivityViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(UpdateActivitytAcceptCommandAction);
            UpdateActivityViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
            TypeSelectedIndexChangedCommand = new DelegateCommand<object>(TypeSelectedIndexChangedCommandAction);
            SearchButtonClickCommand = new DelegateCommand<object>(new Action<object>((obj) => { SearchButtonClickCommandAction(obj); }));
            AddAttendeeRowMouseDoubleClickCommand = new DelegateCommand<object>(AddAttendeeRowMouseDoubleClickCommandAction);
            AttendeeCancelCommand = new DelegateCommand<object>(AttendeeCancelCommandAction);
            AddTagButtonCommand = new DelegateCommand<object>(AddNewTagCommandAction);

            AddWatchersRowMouseDoubleClickCommand = new DelegateCommand<object>(AddWatchersRowMouseDoubleClickCommandAction);
            AddWatchersUserRowMouseDoubleClickCommand = new DelegateCommand<object>(AddWatchersUserRowMouseDoubleClickCommandAction);
            AddWatchersRegionRowMouseDoubleClickCommand = new DelegateCommand<object>(AddWatchersRegionRowMouseDoubleClickCommandAction);
            WatchersCancelCommand = new DelegateCommand<object>(WatchersCancelCommandAction);

            // Activity Reminder
            ActivityReminderAcceptCommand = new RelayCommand(new Action<object>(ActivityReminderOkCommandAction));
            ActivityReminderRemoveCommand = new RelayCommand(new Action<object>(ActivityReminderRemoveCommandAction));

            //Linked items.
            ProjectRowMouseDoubleClickCommand = new DelegateCommand<object>(ProjectRowMouseDoubleClickCommandAction);
            ContactRowMouseDoubleClickCommand = new DelegateCommand<object>(ContactRowMouseDoubleClickCommandAction);
            CompetitorRowMouseDoubleClickCommand = new DelegateCommand<object>(CompetitorRowMouseDoubleClickCommandAction);
            // AccountRowMouseDoubleClickCommand = new DelegateCommand<object>(AccountRowMouseDoubleClickCommandAction);
            OfferRowMouseDoubleClickCommand = new DelegateCommand<object>(OfferRowMouseDoubleClickCommandAction);
            LinkedItemCancelCommand = new DelegateCommand<object>(LinkedItemCancelCommandAction);
            LinkedItemDoubleClickCommand = new DelegateCommand<object>(LinkedItemDoubleClickCommandAction);

            //Comments
            CommentButtonCheckedCommand = new DelegateCommand<object>(CommentButtonCheckedCommandAction);
            CommentButtonUncheckedCommand = new DelegateCommand<object>(CommentButtonUncheckedCommandAction);
            AddNewCommentCommand = new DelegateCommand<object>(AddCommentCommandAction);
            CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);
            DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
            ExcelexportButtonCheckedCommand = new DelegateCommand<object>(ExporttoExcel);

            // Attachment
            ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFile));
            UploadFileCommand = new DelegateCommand<object>(UploadFileCommandAction);
            DownloadFileCommand = new DelegateCommand<object>(DownloadFileCommandAction);
            DeleteFileCommand = new DelegateCommand<object>(DeleteAttachmentRowCommandAction);
            SelectedTagList = new List<object>();
            ActivityAttachmentList = new List<object>();
            ListAttachment = new ObservableCollection<ActivityAttachment>();
            TempListAttachment = new ObservableCollection<ActivityAttachment>();
            ListUpdateActivityAttachment = new ObservableCollection<ActivityAttachment>();
            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();

            RichTextResizingCommand = new DelegateCommand<object>(ResizeRichTextEditor);

            IsnormalPreviewMouseRightButtonDown = new DelegateCommand<object>(IsnormalCommandAction);
            SearchOfferCommand = new DelegateCommand<object>(SearchOffer);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            //[rdixit][GEOS2-4699][28.08.2023]
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => (up.IdPermission == 87) && up.Permission.IdGeosModule == 5))
                IsAddTagsPermissionCRM = true;
            else
                IsAddTagsPermissionCRM = false;
            GeosApplication.Instance.Logger.Log("Constructor EditActivityViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion // Constructor

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

        private void GetPlannedActivitiesOfOwner()
        {
           // List<Activity> ActivityList = new List<Activity>();
            PreviousPlannedDate = DateTime.MinValue;
            NextPlannedDate = DateTime.MinValue;

            #region PreviousPlannedDate and NextPlannedDate logic

            //if (Convert.ToBoolean(_Activity.IsInternal))
            //{
            //    ActivityList = CrmStartUp.GetPlannedAppiontmentByUserIdIsInternal(_Activity.IdOwner, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate).ToList();

            //}
            //else
            //{
            //    ActivityList = CrmStartUp.GetPlannedAppiontmentByUserId(_Activity.IdOwner, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate).ToList();
            //}

            //if (_Activity.LookupValue.IdLookupValue == 96)
            //{
            //    List<Activity> LstActivity = new List<Activity>();
            //    if (_Activity.ActivityLinkedItem.Any(ik => ik.IdLinkedItemType == 42) && Convert.ToBoolean(_Activity.IsInternal) == false)
            //    {
            //        if (_Activity.ActivityLinkedItem.Where(ik => ik.IdLinkedItemType == 42).FirstOrDefault().IdSite > 0)
            //            LstActivity = ActivityList.Where(al => al.IdActivityType == 96 && al.IdOwner == _Activity.IdOwner && al.ActivityLinkedItem.Where(ik => ik.IdLinkedItemType == 42).FirstOrDefault().IdSite == _Activity.ActivityLinkedItem.Where(ik => ik.IdLinkedItemType == 42).FirstOrDefault().IdSite).ToList().OrderBy(d => d.ToDate).ToList();
            //        else
            //            LstActivity = ActivityList.Where(al => al.IdActivityType == 96 && al.IdOwner == _Activity.IdOwner).ToList().OrderBy(d => d.ToDate).ToList();
            //    }
            //    else
            //    {
            //        LstActivity = ActivityList.Where(al => al.IdActivityType == 96 && al.IdOwner == _Activity.IdOwner).ToList().OrderBy(d => d.ToDate).ToList();
            //    }

            //    if (LstActivity.Count == 0)
            //    {
            //        PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
            //        NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
            //    }
            //    else if (LstActivity.Count == 1)
            //    {
            //        PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
            //        NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
            //    }
            //    else if (LstActivity.Count > 1)
            //    {
            //        var groupbydates = LstActivity.GroupBy(cv => cv.ToDate).Select(grp => grp.ToList()).ToList();

            //        List<DateTime> dates = new List<DateTime>();
            //        foreach (var item in groupbydates)
            //        {
            //            DateTime dt = new DateTime();
            //            dt = Convert.ToDateTime(item[0].ToDate);
            //            dates.Add(dt);
            //        }

            //        int currentactivityindex = dates.FindIndex(indexdt => indexdt == _Activity.ToDate.Value);

            //        if (currentactivityindex == 0 && dates.Count == 2)
            //        {
            //            PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
            //            NextPlannedDate = dates[currentactivityindex + 1];
            //        }
            //        else if (currentactivityindex == 1 && dates.Count == 2)
            //        {
            //            PreviousPlannedDate = dates[currentactivityindex - 1];
            //            NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
            //        }
            //        else if (dates.Count > 2)
            //        {
            //            if (currentactivityindex == dates.Count - 1)
            //            {
            //                PreviousPlannedDate = dates[currentactivityindex - 1];
            //                NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
            //            }
            //            else if (currentactivityindex > 0)
            //            {
            //                PreviousPlannedDate = dates[currentactivityindex - 1];
            //                NextPlannedDate = dates[currentactivityindex + 1];
            //            }
            //            else
            //            {
            //                PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
            //                NextPlannedDate = dates[currentactivityindex + 1];
            //            }
            //        }
            //    }
            //}

            #endregion

            if (_Activity.LookupValue.IdLookupValue == 96)
            {
                if(_Activity.MinDueDate != null)
                    PreviousPlannedDate = _Activity.MinDueDate;

                if (_Activity.MaxDueDate != null)
                    NextPlannedDate = _Activity.MaxDueDate;
            }

            if (PreviousPlannedDate == DateTime.MinValue && NextPlannedDate == DateTime.MinValue)
            {
                PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
                NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
            }

        }

        //private void GetPlannedActivitiesOfOwnerForSelectedIndex()
        //{
        //    List<Activity> ActivityList = new List<Activity>();
        //    PreviousPlannedDate = DateTime.MinValue;
        //    NextPlannedDate = DateTime.MinValue;
        //    if (IsInternal)
        //    {
        //        ActivityList = CrmStartUp.GetPlannedAppiontmentByUserIdIsInternal(_Activity.IdOwner, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate).ToList();

        //    }
        //    else
        //    {
        //        ActivityList = CrmStartUp.GetPlannedAppiontmentByUserId(_Activity.IdOwner, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate).ToList();
        //    }

        //    if (SelectedActivityType == null)
        //        SelectedActivityType = _Activity.LookupValue;

        //    if (SelectedActivityType != null && SelectedActivityType.IdLookupValue == 96)
        //    {
        //        List<Activity> LstActivity = new List<Activity>();
        //        if (SelectedActivityLinkedItems.Any(ik => ik.IdLinkedItemType == 42) && IsInternal == false)
        //        {
        //            if (SelectedActivityLinkedItems.Where(ik => ik.IdLinkedItemType == 42).FirstOrDefault().IdSite > 0)
        //                LstActivity = ActivityList.Where(al => al.IdActivityType == 96 && al.IdOwner == _Activity.IdOwner && al.ActivityLinkedItem.Where(ik => ik.IdLinkedItemType == 42).FirstOrDefault().IdSite == SelectedActivityLinkedItems.Where(ik => ik.IdLinkedItemType == 42).FirstOrDefault().IdSite).ToList().OrderBy(d => d.ToDate).ToList();
        //            else
        //                LstActivity = ActivityList.Where(al => al.IdActivityType == 96 && al.IdOwner == _Activity.IdOwner).ToList().OrderBy(d => d.ToDate).ToList();
        //        }
        //        else
        //        {
        //            LstActivity = ActivityList.Where(al => al.IdActivityType == 96 && al.IdOwner == _Activity.IdOwner).ToList().OrderBy(d => d.ToDate).ToList();
        //        }

        //        if (LstActivity.Count == 0)
        //        {
        //            PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
        //            NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
        //        }
        //        else if (LstActivity.Count == 1)
        //        {
        //            PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
        //            NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
        //        }
        //        else if (LstActivity.Count > 1)
        //        {
        //            var groupbydates = LstActivity.GroupBy(cv => cv.ToDate).Select(grp => grp.ToList()).ToList();

        //            List<DateTime> dates = new List<DateTime>();
        //            foreach (var item in groupbydates)
        //            {
        //                DateTime dt = new DateTime();
        //                dt = Convert.ToDateTime(item[0].ToDate);
        //                dates.Add(dt);
        //            }
        //            if (!dates.Any(i => i == Convert.ToDateTime(DueDate.Value)))
        //            {
        //                DateTime dt = new DateTime();
        //                dates.Add(Convert.ToDateTime(DueDate));
        //                dates = dates.OrderBy(i => i).ToList();
        //            }


        //            int currentactivityindex = dates.FindIndex(indexdt => indexdt == DueDate);

        //            if (currentactivityindex == 0 && dates.Count == 2)
        //            {
        //                PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
        //                NextPlannedDate = dates[currentactivityindex + 1];
        //            }
        //            else if (currentactivityindex == 1 && dates.Count == 2)
        //            {
        //                PreviousPlannedDate = dates[currentactivityindex - 1];
        //                NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
        //            }
        //            else if (dates.Count > 2)
        //            {
        //                if (currentactivityindex == dates.Count - 1)
        //                {
        //                    PreviousPlannedDate = dates[currentactivityindex - 1];
        //                    NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
        //                }
        //                else if (currentactivityindex > 0)
        //                {
        //                    PreviousPlannedDate = dates[currentactivityindex - 1];
        //                    NextPlannedDate = dates[currentactivityindex + 1];
        //                }
        //                else
        //                {
        //                    PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
        //                    NextPlannedDate = dates[currentactivityindex + 1];
        //                }
        //            }
        //        }
        //    }

        //    if (PreviousPlannedDate == DateTime.MinValue && NextPlannedDate == DateTime.MinValue)
        //    {
        //        PreviousPlannedDate = GeosApplication.Instance.SelectedyearStarDate;
        //        NextPlannedDate = GeosApplication.Instance.SelectedyearEndDate;
        //    }

        //}
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

        private void FillActivityDetailsForPlannedAppointment()
        {
            FromDate = _Activity.FromDate.Value;
            ToDate = _Activity.ToDate.Value;

            TaskStatusList = new List<LookupValue>();
            TaskStatusList.Add(_Activity.ActivityStatus);

            TagList = new List<Tag>();
            TagList.AddRange(_Activity.ActivityTags.Select(tag => tag.Tag).ToList());
            SelectedTagList = new List<object>();
            foreach (ActivityTag item in _Activity.ActivityTags)
            {
                SelectedTagList.Add(TagList.FirstOrDefault(x => x.IdTag == item.IdTag));
            }

            if (_Activity.ActivityLinkedItem.Any(i => i.IdLinkedItemType == 42))
            {
                CompanyGroupList = new ObservableCollection<Customer>();
                CompanyGroupList.Insert(0, new Customer() { IdCustomer = 0, CustomerName = "---" });
                CompanyGroupList.Add(_Activity.ActivityLinkedItem.Where(i => i.Customer.IdCustomer != 0).Select(i => i.Customer).FirstOrDefault());

                SelectedIndexCompanyGroup = 1;

                CompanyPlantList = new ObservableCollection<Company>();
                CompanyPlantList.Insert(0, new Company() { IdCompany = 0, Name = "---" });
                Company tempCompany = new Company();
                tempCompany = _Activity.ActivityLinkedItem.Where(i => i.Company.IdCompany != 0).Select(i => i.Company).FirstOrDefault();
                if (tempCompany != null)
                {
                    tempCompany.Name = tempCompany.SiteNameWithoutCountry;
                    tempCompany.SiteNameWithoutCountry = tempCompany.Name;
                    CompanyPlantList.Add(tempCompany);
                }

                SelectedIndexCompanyPlant = 1;
                //ReminderCalenderString = _Activity.ActivityReminderDateTime.Value.ToString();
            }

            SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>(_Activity.ActivityLinkedItem.ToList());
            if (ActivityCommentsList != null)
            {
                SetUserProfileImage(ActivityCommentsList);
                RtfToPlaintext();
            }
        }

        private void FillActivityDetails()
        {
            // FillReminderDates();
            //// CheckReminder();
            // ReminderDateMinValue = GeosApplication.Instance.ServerDateTime.Date;
            // ReminderTimeMinValue = GeosApplication.Instance.ServerDateTime.TimeOfDay;
            // ReminderTime = DateTime.Now;
            // CurrentTimePatternString = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern;
           
            if (_Activity.ActivityReminderDateTime != null)
            {

                // Get current time zone.
                TimeZone zone = TimeZone.CurrentTimeZone;
                string standard = zone.StandardName;
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(standard);
                TimeZoneInfo otherTimezone = TimeZoneInfo.FindSystemTimeZoneById(GeosApplication.Instance.CurrentSiteTimeZone);//[rdixit][GEOS2-3871][09.01.2023]
                DateTime nzDateTime = TimeZoneInfo.ConvertTime(_Activity.ActivityReminderDateTime.Value, otherTimezone, tzi);
                ReminderDate = nzDateTime;
                ReminderCalenderString = ReminderDate.ToString();
                ReminderTime = ReminderDate.Value;
                CheckReminder();
            }


            Subject = _Activity.Subject;
            Description = _Activity.Description;
            IsInternal = Convert.ToBoolean(_Activity.IsInternal);
            if (TypeList != null)
            {
                SelectedIndexType = TypeList.FindIndex(x => x.IdLookupValue == _Activity.LookupValue.IdLookupValue);
                SelectedActivityType = TypeList.FirstOrDefault(x => x.IdLookupValue == _Activity.LookupValue.IdLookupValue);
            }

            //Location
            Latitude = _Activity.Latitude;
            Longitude = _Activity.Longitude;
            if (Latitude != null && Longitude != null) { MapLatitudeAndLongitude = new GeoPoint(Latitude.Value, Longitude.Value); }
            ActivityAddress = _Activity.Location;

            //Due date.
            if (_Activity.ToDate != null)
            {
                DueDate = _Activity.ToDate;
            }

            //Task Status
            if (_Activity.IdActivityStatus != null && TaskStatusList != null)
            {
                SelectedIndexTaskStatus = TaskStatusList.FindIndex(x => x.IdLookupValue == _Activity.IdActivityStatus);
            }
            else
            {
                SelectedIndexTaskStatus = -1;
            }

            //Is Completed.
            if (_Activity.IsCompleted == 1)
            {
                IsCompleted = true;
                IsCompletedVisible = Visibility.Visible;
                ModifiedORCompleted = System.Windows.Application.Current.FindResource("LeadsEditViewCompletedIn").ToString();
                OpenORCompleted = System.Windows.Application.Current.FindResource("ActivityViewActivityCompleted").ToString();
                ModifiedInORCompletedIn = _Activity.CloseDate;
            }
            else
            {
                IsCompleted = false;
                IsCompletedVisible = Visibility.Collapsed;
                ModifiedORCompleted = System.Windows.Application.Current.FindResource("LeadsEditViewModifyIn").ToString();
                OpenORCompleted = System.Windows.Application.Current.FindResource("ActivityViewActivityOpen").ToString();
                ModifiedInORCompletedIn = _Activity.ModifiedIn;
            }
            
            
            FillOwnerList(_Activity.IdOwner);
            SelectedIndexOwner = ActivityOwnerList.FindIndex(i => i.IdUser == _Activity.IdOwner);
            CreatedIn = _Activity.CreatedIn;

            //IsInternalEnable = true;
            //this condition is for Attendees and Watcher make for readoly only they can add and delete their comments.

            if (ModifiedInORCompletedIn != null)
            {
                ActivitySleepDays = (GeosApplication.Instance.ServerDateTime.Date - ModifiedInORCompletedIn.Value.Date).Days;
            }
            if (_Activity.ModifiedIn == null)
            {
                ActivitySleepDays = (GeosApplication.Instance.ServerDateTime.Date - CreatedIn.Value.Date).Days;
            }

            FillSelectedActivityLinkedItems(_Activity.ActivityLinkedItem);
            FillAttendiesList();


            //if ((ActivityOwnerList[SelectedIndexOwner].IdUser != GeosApplication.Instance.ActiveUser.IdUser
            //    || GeosApplication.Instance.IsPermissionReadOnly) && GeosApplication.Instance.IsPermissionAuditor==false)
            //{
            //    IsUserWatcherReadOnly = true;
            //    IsUserWatcherEnabled = false;
            //    IsInternalEnable = false;
            //}

            //if (!IsEditedFromOutSide)
            //{
            //    if (IsUserWatcherReadOnly)
            //        IsConfirm = true;
            //    else
            //        IsConfirm = false;
            //}
            //else
            //{
            //    IsConfirm = true;
            //};

            //FillGroupList();
            //Attendies
            //if (IsUserWatcherReadOnly == false)
            //{
            //    FillAttendiesList();
            //    FillWatchersList();
            //}
            //FillSelectedAttendiesList(_Activity.ActivityAttendees);
            //AttendeesCount = SelectedAttendiesList.Count;

            FillActivityComments(_Activity.IdActivity);
            FillActivityChangeLog(_Activity.IdActivity);

            // show activity attachment
            _Activity.ActivityAttachment = _Activity.ActivityAttachment.Where(x => x.IsDeleted == 0 || x.IsDeleted == null).ToList();

            foreach (ActivityAttachment item in _Activity.ActivityAttachment)
            {
                ImageSource objimage = FileExtensionToFileIcon.FindIconForFilename(item.SavedFileName + item.FileType, true);
                item.AttachmentImage = objimage;
                ListAttachment.Add((ActivityAttachment)item.Clone());
            }
        }

        /// <summary>
        /// Method For Resizing Rich Text Box on Load and Content Changed
        /// </summary>
        /// <param name="obj"></param>
        public void ResizeRichTextEditor(object obj)
        {
            RichEditControl edit = (RichEditControl)obj;
            Document currentDocument = edit.Document;
            currentDocument.DefaultCharacterProperties.FontName = GeosApplication.Instance.FontFamilyAsPerTheme.ToString();
            DocumentLayout currentDocumentLayout = edit.DocumentLayout;

            edit.BeginInvoke(() =>
            {
                SubDocument subDocument = currentDocument.CaretPosition.BeginUpdateDocument();
                DocumentPosition docPosition = subDocument.CreatePosition(((currentDocument.CaretPosition.ToInt() == 0) ? 0 : currentDocument.CaretPosition.ToInt() - 1));

                double height = 0;
                System.Drawing.Point pos = PageLayoutHelper.GetInformationAboutCurrentPage(currentDocumentLayout, docPosition);
                height = DevExpress.Office.Utils.Units.TwipsToPixels(pos, edit.DpiX, edit.DpiY).Y;

                edit.Height = height + 50;
                edit.VerticalScrollValue = 0;
                currentDocument.CaretPosition.EndUpdateDocument(subDocument);
            });
        }

        /// <summary>
        /// Method to check reminder is set or not
        /// </summary>
        private void CheckReminder()
        {
            if (string.IsNullOrEmpty(ReminderCalenderString))
                ReminderButtonText = System.Windows.Application.Current.FindResource("ActivityReminderSetReminderButtonHeader").ToString();
            else
                ReminderButtonText = System.Windows.Application.Current.FindResource("ActivityReminderEditButtonHeader").ToString();
        }

        /// <summary>
        /// Method for Reminder Ok Button
        /// </summary>
        /// <param name="obj"></param>
        private void ActivityReminderOkCommandAction(object obj)
        {
            if (ReminderDate != null)
            {
                GeosApplication.Instance.ServerDateTime = WorkbenchStartUp.GetServerDateTime();
                DateTime dateTime = new DateTime(ReminderDate.Value.Year, ReminderDate.Value.Month, ReminderDate.Value.Day, ReminderTime.Hour, ReminderTime.Minute, ReminderTime.Second);

                if (dateTime >= GeosApplication.Instance.ServerDateTime)
                {
                    ReminderDate = dateTime;
                    ReminderCalenderString = ReminderDate.ToString();
                    CheckReminder();
                    ((DropDownButton)obj).IsPopupOpen = false;
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityReminderTimeFailedMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    ((DropDownButton)obj).IsPopupOpen = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Method for Reminder Remove Button 
        /// </summary>
        /// <param name="obj"></param>
        private void ActivityReminderRemoveCommandAction(object obj)
        {
            ReminderDateMinValue = GeosApplication.Instance.ServerDateTime.Date;
            ReminderTime = DateTime.Now;
            ReminderCalenderString = "";
            CheckReminder();
            ((DropDownButton)obj).IsPopupOpen = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewTagCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddNewTagCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (!IsUserWatcherReadOnly)
            {
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
            }

            GeosApplication.Instance.Logger.Log("Method AddNewTagCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// LinkedItemCheckCommandAction
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// </summary>
        /// <param name="obj"></param>
        private void LinkedItemDoubleClickCommandAction(object obj)
        {
            ActivityLinkedItem linkedItem = obj as ActivityLinkedItem;

            if (!IsUserWatcherReadOnly)
            {
                if (linkedItem != null)
                {
                    #region Account
                    if (linkedItem.IdLinkedItemType == 42)      //Account
                    {
                        if (linkedItem.IsVisible)
                        {
                            GeosApplication.Instance.Logger.Log("Method EditCustomerAction...", category: Category.Info, priority: Priority.Low);
                            try
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

                                List<Company> TempCompany = new List<Company>();
                                // TempCompany.Add(CrmStartUp.GetCompanyDetailsById(Convert.ToInt32((linkedItem.Company.IdCompany))));
                                //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
                                TempCompany.Add(CrmStartUp.GetCompanyDetailsById_V2340(Convert.ToInt32((linkedItem.Company.IdCompany))));



                                EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel();
                                EditCustomerView editCustomerView = new EditCustomerView();
                                editCustomerViewModel.InIt(TempCompany);
                                EventHandler handle = delegate { editCustomerView.Close(); };
                                editCustomerViewModel.RequestClose += handle;
                                editCustomerView.DataContext = editCustomerViewModel;

                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                                {
                                    DXSplashScreen.Close();
                                }

                                editCustomerView.ShowDialog();
                                if (editCustomerViewModel.CustomerData != null)
                                {

                                    linkedItem.Company.Name = editCustomerViewModel.CustomerData.Name;
                                    linkedItem.Company.Customers = editCustomerViewModel.CustomerData.Customers;
                                }
                                GeosApplication.Instance.Logger.Log("Method EditCustomerAction...executed successfully", category: Category.Info, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in EditCustomerAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                    #endregion

                    #region Contact
                    else if (linkedItem.IdLinkedItemType == 43) //Contact
                    {
                        if (linkedItem.IsVisible)
                        {
                            try
                            {
                                GeosApplication.Instance.Logger.Log("Method EditContactAction...", category: Category.Info, priority: Priority.Low);
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

                                People peopleData = CrmStartUp.GetContactsByIdPerson(linkedItem.People.IdPerson);

                                EditContactViewModel editContactViewModel = new EditContactViewModel();
                                EditContactView editContactView = new EditContactView();
                                editContactViewModel.InIt(peopleData);
                                editContactViewModel.InItPermisssion(IsPermissionCustomerEdit);  //[pallavi.kale][GEOS2-8961][07.11.2025]
                                EventHandler handle = delegate { editContactView.Close(); };
                                editContactViewModel.RequestClose += handle;
                                editContactView.DataContext = editContactViewModel;
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                                editContactView.ShowDialogWindow();
                                if (editContactViewModel.IsSave && editContactViewModel.SelectedContact[0] != null)
                                {
                                    linkedItem.People.Name = editContactViewModel.SelectedContact[0].Name;
                                    linkedItem.People.Surname = editContactViewModel.SelectedContact[0].Surname;
                                }
                                GeosApplication.Instance.Logger.Log("Method EditContactAction...executed successfully", category: Category.Info, priority: Priority.Low);
                            }

                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in EditContactAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                    #endregion

                    #region Opportunity
                    else if (linkedItem.IdLinkedItemType == 44)     // Opportunity
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
        private void FillCompetitor()
        {

            if (GeosApplication.Instance.CompetitorList == null)
            {
                GeosApplication.Instance.CompetitorList = CrmStartUp.GetAllCompetitor().ToList();
            }

            LinkeditemsCompetitorList = new ObservableCollection<Competitor>(GeosApplication.Instance.CompetitorList.ToList());

            if (_Activity.ActivityLinkedItem != null)
            {
                if (SelectedActivityLinkedItems == null)
                    SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>();

                foreach (ActivityLinkedItem activityLinkedItem in _Activity.ActivityLinkedItem)
                {
                    SelectedActivityLinkedItems.Add(activityLinkedItem);
                    LinkeditemsCompetitorList.Remove(LinkeditemsCompetitorList.FirstOrDefault(x => x.IdCompetitor == activityLinkedItem.IdCompetitor));
                }
            }
        }

        public void Init(Activity activity)
        {
            _Activity = new Activity();
            _Activity = activity;

            FillReminderDates();
            CheckReminder();
            ReminderDateMinValue = GeosApplication.Instance.ServerDateTime.Date;
            ReminderTimeMinValue = GeosApplication.Instance.ServerDateTime.TimeOfDay;
            ReminderTime = DateTime.Now;
            CurrentTimePatternString = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern;

            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            isInit = true;
            try
            {
                IsUserWatcherEnabled = true;

                if (_Activity != null)
                {
                    ///TODO: Changed by Ravi on 19 June to optimized the code.
                    CheckAllPermissions();
                    if (_Activity.IdOwner != GeosApplication.Instance.ActiveUser.IdUser)
                    {
                        FillTypeList();
                        FillTagsList();
                        FillAttendiesList();
                        FillWatchersList();
                        IsSearchButtonEnable = false;
                    }

                    if (IsUserWatcherReadOnly == false)
                    {
                        FillAllMasters();
                        if (GeosApplication.Instance.IsPermissionAuditor)
                        {
                            FillGroupList();
                            FillCompanyPlantList();

                        }
                        else if (!IsConfirm && _Activity.LookupValue.IdLookupValue != 96)
                        {
                            FillGroupList();
                            FillCompanyPlantList();

                        }
                        else
                        {
                            FillCompanyGroupListWhenIsConfirm();

                        }

                    }

                    //SetReminders();
                    if (IsPlannedAppointment || IsUserWatcherReadOnly)
                    {
                        FillActivityDetailsForPlannedAppointment();
                        GetPlannedActivitiesOfOwner();
                        FillTypeList();
                        FillActivityDetails();
                    }
                    else
                    {
                        GetPlannedActivitiesOfOwner();
                        FillActivityDetails();
                    }
                    if (IsUserWatcherReadOnly == false)
                        FillContactsList();
                }

                isInit = false;
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void SetReminders()
        {
            ReminderDates = new List<DateTime>();
            double addMinutes = 0;

            for (int i = 0; i < 48; i++)
            {
                DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
                ReminderDates.Add(otherDate);
                addMinutes += 30;
            }

            ReminderDateMinValue = GeosApplication.Instance.ServerDateTime.Date;
            ReminderTimeMinValue = GeosApplication.Instance.ServerDateTime.TimeOfDay;
            ReminderTime = DateTime.Now;
            CurrentTimePatternString = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern;

            if (_Activity.ActivityReminderDateTime != null)
            {
                // Get current time zone.
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(TimeZone.CurrentTimeZone.StandardName);
                TimeZoneInfo otherTimezone = TimeZoneInfo.FindSystemTimeZoneById(GeosApplication.Instance.CurrentSiteTimeZone);//[rdixit][GEOS2-3871][09.01.2023]
                DateTime nzDateTime = TimeZoneInfo.ConvertTime(_Activity.ActivityReminderDateTime.Value, otherTimezone, tzi);
                ReminderDate = nzDateTime;
                ReminderCalenderString = ReminderDate.ToString();
                ReminderTime = ReminderDate.Value;
                if (string.IsNullOrEmpty(ReminderCalenderString))
                    ReminderButtonText = System.Windows.Application.Current.FindResource("ActivityReminderSetReminderButtonHeader").ToString();
                else
                    ReminderButtonText = System.Windows.Application.Current.FindResource("ActivityReminderEditButtonHeader").ToString();
            }
        }

        /// <summary>
        /// Check all permissions
        /// </summary>
        void CheckAllPermissions()
        {
            if (_Activity.LookupValue.IdLookupValue == 96 && _Activity.IdOwner == GeosApplication.Instance.ActiveUser.IdUser)
            {

                //IsPlannedAppointmentReadOnly = true;
                //IsGroupPlantReadOnly = true;
                if (GeosApplication.Instance.IsPermissionAuditor == true && GeosApplication.Instance.IsPermissionReadOnly == false)
                {
                    IsGroupPlantReadOnly = false;
                    IsPlannedAppointmentReadOnly = false;
                    IsPlannedAppointment = false;
                    IsUserWatcherReadOnly = false;
                }
                else
                {
                    IsPlannedAppointment = true;
                    IsGroupPlantReadOnly = true;
                    IsPlannedAppointmentReadOnly = true;
                    IsUserWatcherReadOnly = false;
                }

                if (!GeosApplication.Instance.IsPermissionReadOnly)
                    IsActivityAddressReadOnly = false;
                //  FillAttendiesList();
                FillActivityDetails();
            }
            else if (_Activity.IdOwner == GeosApplication.Instance.ActiveUser.IdUser && !GeosApplication.Instance.IsPermissionReadOnly)
            {
                IsActivityAddressReadOnly = false;
            }

            if (_Activity.IdOwner != GeosApplication.Instance.ActiveUser.IdUser
                       || GeosApplication.Instance.IsPermissionReadOnly)
            {

                if (GeosApplication.Instance.IsPermissionAuditor == true && GeosApplication.Instance.IsPermissionReadOnly == false)
                {
                    IsGroupPlantReadOnly = false;
                    IsPlannedAppointmentReadOnly = false;
                    IsUserWatcherReadOnly = false;
                    IsActivityAddressReadOnly = false;
                }
                else
                {
                    IsGroupPlantReadOnly = true;
                    IsPlannedAppointmentReadOnly = true;
                    IsUserWatcherReadOnly = true;
                    IsUserWatcherEnabled = false;
                    IsInternalEnable = false;
                    IsActivityAddressReadOnly = true;
                }


            }

            if (!IsEditedFromOutSide)
            {
                if (IsUserWatcherReadOnly)
                {
                    IsConfirm = true;
                    IsGroupPlantReadOnly = true;
                }
                else IsConfirm = false;
            }
            else
            {
                IsGroupPlantReadOnly = true;
                IsConfirm = true;
            }
            if (GeosApplication.Instance.IsPermissionAdminOnly == true)
            {
                AddTagVisibility = "Visible";
            }
            else
            {
                AddTagVisibility = "Hidden";
            }

        }

        private void FillTagsList()
        {
            try
            {
                if (_Activity.IdOwner != GeosApplication.Instance.ActiveUser.IdUser && GeosApplication.Instance.IsPermissionAuditor == false)
                {
                    SelectedTagList = new List<object>();
                    foreach (ActivityTag item in _Activity.ActivityTags)
                    {
                        SelectedTagList.Add(item.Tag);
                    }
                }
                else {
                    if (!IsPlannedAppointment)
                    {
                        GeosApplication.Instance.Logger.Log("Method FillTagList ...", category: Category.Info, priority: Priority.Low);
                        TagList = new List<Tag>();
                        if (GeosApplication.Instance.TagList == null)
                        {
                            GeosApplication.Instance.TagList = CrmStartUp.GetAllTags().OrderBy(r => r.Name).ToList();
                        }
                        TagList = GeosApplication.Instance.TagList;

                        SelectedTagList = new List<object>();
                        foreach (ActivityTag item in _Activity.ActivityTags)
                        {
                            SelectedTagList.Add(TagList.FirstOrDefault(x => x.IdTag == item.IdTag));
                        }
                    }
                    GeosApplication.Instance.Logger.Log("Method FillTagList() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTagList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTagList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTagList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to change visibility of controls as per selection of type.
        /// </summary>
        /// <param name="obj"></param>
        private void TypeSelectedIndexChangedCommandAction(object obj)
        {
            //int previousTypeId = _Activity.LookupValue.IdLookupValue;
            //int currentTypeId = (int)((DevExpress.Xpf.Editors.BaseEdit)((System.Windows.RoutedEventArgs)obj).OriginalSource).EditValue;
            //if (previousTypeId != currentTypeId)
            if (SelectedActivityType != null)
            {
                if (SelectedActivityType.IdLookupValue == 37)   // Appointment
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
                    //IsCompletedVisible = Visibility.Visible;
                    IsCompletedCheckVisible = Visibility.Visible;
                    IsWatcherUserVisible = Visibility.Collapsed;
                    IsWatcherRegionVisible = Visibility.Collapsed;
                    ColumnMainWidth = 920;
                    ColumnWidth = 350;

                    if (SelectedActivityLinkedItems != null)
                        SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>(SelectedActivityLinkedItems.Where(ik => ik.IdLinkedItemType != 92).ToList());
                }
                else if (SelectedActivityType.IdLookupValue == 96)   // Planned Appointment
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
                    //IsCompletedVisible = Visibility.Visible;
                    IsCompletedCheckVisible = Visibility.Visible;
                    IsWatcherUserVisible = Visibility.Collapsed;
                    IsWatcherRegionVisible = Visibility.Collapsed;
                    ColumnMainWidth = 920;
                    ColumnWidth = 350;

                    if (SelectedActivityLinkedItems != null)
                        SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>(SelectedActivityLinkedItems.Where(ik => ik.IdLinkedItemType != 92).ToList());

                    //GetPlannedActivitiesOfOwnerForSelectedIndex();
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
                    //IsCompletedVisible = Visibility.Visible;
                    IsCompletedCheckVisible = Visibility.Visible;
                    IsWatcherUserVisible = Visibility.Collapsed;
                    IsWatcherRegionVisible = Visibility.Collapsed;
                    IsInternal = false;
                    ColumnMainWidth = 920;
                    ColumnWidth = 350;

                    if (SelectedActivityLinkedItems != null)
                        SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>(SelectedActivityLinkedItems.Where(ik => ik.IdLinkedItemType != 92).ToList());
                }
                else if (SelectedActivityType.IdLookupValue == 40)  // Task
                {
                    IsAppointmentVisible = Visibility.Collapsed;
                    IsEmailorCallVisible = Visibility.Collapsed;
                    IsContactVisible = Visibility.Visible;
                    IsTaskVisible = Visibility.Visible;
                    IsDueDateVisible = Visibility.Visible;
                    IsInternalVisible = Visibility.Visible;
                    IsWatcherVisible = Visibility.Visible;
                    IsDueDateVisible = Visibility.Visible;
                    IsInternalVisible = Visibility.Visible;
                    IsOwnerVisible = Visibility.Visible;
                    IsTagVisible = Visibility.Visible;
                    IsCommentsVisible = Visibility.Visible;
                    IsInformationVisible = Visibility.Collapsed;
                    //IsCompletedVisible = Visibility.Collapsed;
                    IsCompletedCheckVisible = Visibility.Collapsed;
                    IsWatcherUserVisible = Visibility.Collapsed;
                    IsWatcherRegionVisible = Visibility.Collapsed;
                    SelectedWatchersUserList.Clear();
                    ColumnMainWidth = 920;
                    ColumnWidth = 350;

                    if (SelectedActivityLinkedItems != null)
                        SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>(SelectedActivityLinkedItems.Where(ik => ik.IdLinkedItemType != 92).ToList());
                }
                else if (SelectedActivityType.IdLookupValue == 91)  // Information
                {
                    IsAppointmentVisible = Visibility.Collapsed;
                    IsContactVisible = Visibility.Visible;
                    IsEmailorCallVisible = Visibility.Collapsed;
                    IsDueDateVisible = Visibility.Collapsed;
                    IsOwnerVisible = Visibility.Visible;
                    IsTagVisible = Visibility.Collapsed;
                    IsWatcherVisible = Visibility.Visible;
                    IsInternalVisible = Visibility.Visible;
                    IsInformationVisible = Visibility.Visible;
                    IsAccountVisible = Visibility.Visible;
                    IsTaskVisible = Visibility.Collapsed;
                    IsCommentsVisible = Visibility.Collapsed;
                    //IsCompletedVisible = Visibility.Collapsed;
                    IsCompletedCheckVisible = Visibility.Collapsed;
                    IsWatcherUserVisible = Visibility.Visible;
                    IsWatcherRegionVisible = Visibility.Visible;
                    SelectedWatchersUserList.Clear();
                    ColumnMainWidth = 970;
                    ColumnWidth = 460;

                    if (IsInternal)
                    {
                        IsAccountVisible = Visibility.Collapsed;
                    }
                    // IsInternal = false;
                }
            }
        }

        private void FillSelectedActivityLinkedItems(List<ActivityLinkedItem> ActivityLinkedItems)
        {
            Offer activityLinkedItemoffferDetail = new Offer();
            GeosApplication.Instance.Logger.Log("Method FillSelectedActivityLinkedItems ...", category: Category.Info, priority: Priority.Low);
            SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>();
            ActivityLinkedItems = ActivityLinkedItems.OrderBy(x => x.IdLinkedItemType).ToList();

            foreach (ActivityLinkedItem activityLinkedItem in ActivityLinkedItems)
            {
                try
                {
                    if (activityLinkedItem.IdLinkedItemType == 42)          // Account
                    {
                        //Account always at first position.

                        ActivityLinkedItem selectedActivityLinkedItem = (ActivityLinkedItem)activityLinkedItem.Clone();
                        SelectedActivityLinkedItems.Insert(0, selectedActivityLinkedItem);
                        if (CompanyGroupList != null && CompanyGroupList.Count > 0)
                            SelectedIndexCompanyGroup = CompanyGroupList.IndexOf(CompanyGroupList.FirstOrDefault(x => x.IdCustomer == activityLinkedItem.IdCustomer));
                        if (CompanyPlantList != null && CompanyPlantList.Count > 0)
                            SelectedIndexCompanyPlant = CompanyPlantList.IndexOf(CompanyPlantList.FirstOrDefault(x => x.IdCompany == activityLinkedItem.IdSite));


                        if (SelectedActivityType.IdLookupValue == 37 && !isInit)   // Appointment
                        {
                            // Set account location to activity location
                            CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                            customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                            customerSetLocationMapViewModel.LocalMapLatitudeAndLongitude = new GeoPoint();

                            if (!string.IsNullOrEmpty(selectedActivityLinkedItem.Company.Address))
                            {
                                if (!ActivityAddress.Contains(selectedActivityLinkedItem.Company.Address))
                                {
                                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["CustomerEditViewChangeLocationDetails"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                    if (MessageBoxResult == MessageBoxResult.Yes)
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
                                        }
                                    }
                                }
                            }
                        }

                        //CompaniesList.Remove(CompaniesList.FirstOrDefault(x => x.IdCompany == activityLinkedItem.IdSite));
                        if (activityLinkedItem.Company.Customers.Count > 0)
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
                        //Removed
                    }
                    else if (activityLinkedItem.IdLinkedItemType == 43)     // User
                    {
                        ActivityLinkedItem selectedActivityLinkedItem = (ActivityLinkedItem)activityLinkedItem.Clone();
                        SelectedActivityLinkedItems.Add(selectedActivityLinkedItem);
                        if (LinkeditemsPeopleList != null && LinkeditemsPeopleList.Count > 0)
                            LinkeditemsPeopleList.Remove(LinkeditemsPeopleList.FirstOrDefault(x => x.IdPerson == activityLinkedItem.IdPerson));
                        if (activityLinkedItem.IdPerson != null)
                        {
                            FillPersonImageByIdPerson(selectedActivityLinkedItem, activityLinkedItem.IdPerson.GetValueOrDefault());
                        }
                    }

                    else if (activityLinkedItem.IdLinkedItemType == 44)     // Opportunity
                    {
                        // to check order or opportunity
                        try
                        {
                            ///TODO: Changed by Ravi to fixed the bug on 19 June
                            string conn = GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == activityLinkedItem.IdEmdepSite.Value.ToString()).Select(x => x.ConnectPlantConstr).FirstOrDefault();
                            if (!string.IsNullOrEmpty(conn))
                                activityLinkedItemoffferDetail = CrmStartUp.GetOfferByIdOfferAndEmdepSite(activityLinkedItem.IdOffer.Value, conn);

                            activityLinkedItem.Name = activityLinkedItemoffferDetail.Code;
                            activityLinkedItem.IdOffer = activityLinkedItemoffferDetail.IdOffer;
                            activityLinkedItem.IdEmdepSite = activityLinkedItem.IdEmdepSite.Value;
                            activityLinkedItem.IdLinkedItemType = 44;

                            activityLinkedItem.Offer = new Offer();
                            activityLinkedItem.Offer.Code = activityLinkedItemoffferDetail.Code;
                            activityLinkedItem.Offer.Description = activityLinkedItemoffferDetail.Description;
                            activityLinkedItem.Offer.IdOffer = activityLinkedItemoffferDetail.IdOffer;
                            activityLinkedItem.Offer.Site = new Company();
                            activityLinkedItem.Offer.Site.SiteNameWithoutCountry = activityLinkedItemoffferDetail.Site.SiteNameWithoutCountry;
                            activityLinkedItem.Offer.Site.Name = activityLinkedItemoffferDetail.Site.Name;

                            activityLinkedItem.Offer.Site.ConnectPlantId = activityLinkedItem.IdEmdepSite.Value.ToString();
                            activityLinkedItem.Offer.Site.ConnectPlantConstr = conn;

                            ActivityLinkedItem selectedActivityLinkedItem = (ActivityLinkedItem)activityLinkedItem.Clone();
                            SelectedActivityLinkedItems.Add(selectedActivityLinkedItem);

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

                    else if (activityLinkedItem.IdLinkedItemType == 45)     // Car project
                    {
                        ActivityLinkedItem selectedActivityLinkedItem = (ActivityLinkedItem)activityLinkedItem.Clone();
                        SelectedActivityLinkedItems.Add(selectedActivityLinkedItem);

                        if (CarProjectList != null && CarProjectList.Count > 0)
                            CarProjectList.Remove(CarProjectList.FirstOrDefault(x => x.IdCarProject == activityLinkedItem.IdCarProject));
                        if (SelectedActivityLinkedItems != null && SelectedActivityLinkedItems.Any(i => i.IdLinkedItemType == 45))
                        {
                            FillPlantOpportunityList(false, new List<Offer>());
                        }
                        byte[] bytes = GeosRepositoryServiceController.GetCaroemIconFileInBytes(activityLinkedItem.CarProject.CarOEM.Name);
                        if (bytes != null)
                        {
                            selectedActivityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wProject.png");
                            else
                                selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueProject.png");
                        }
                    }
                    else if (activityLinkedItem.IdLinkedItemType == 92)     // Competitor
                    {
                        ActivityLinkedItem selectedActivityLinkedItem = (ActivityLinkedItem)activityLinkedItem.Clone();
                        SelectedActivityLinkedItems.Add(selectedActivityLinkedItem);

                        if (LinkeditemsCompetitorList != null && LinkeditemsCompetitorList.Count > 0)
                            LinkeditemsCompetitorList.Remove(LinkeditemsCompetitorList.FirstOrDefault(x => x.IdCompetitor == selectedActivityLinkedItem.IdCompetitor));

                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wCompititor.png");
                        else
                            selectedActivityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueCompititor.png");
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

        private void FillReminderDates()
        {
            ReminderDates = new List<DateTime>();

            double addMinutes = 0;
            for (int i = 0; i < 48; i++)
            {
                DateTime otherDate = DateTime.MinValue.AddMinutes(addMinutes);
                ReminderDates.Add(otherDate);
                addMinutes += 30;
            }
        }

        /// <summary>
        /// This Method for Add Attendee
        /// </summary>
        /// <param name="e"></param>
        private void AddAttendeeRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddAttendeeRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                IsBusy = true;
                if (IsPlannedAppointment)
                {
                    if (obj is People)
                    {
                        People people = obj as People;

                        if (!SelectedAttendiesList.Any(x => x.IdPerson == people.IdPerson))
                        {
                            People selectedPerson = new People();

                            selectedPerson.IdPerson = people.IdPerson;
                            selectedPerson.Name = people.Name;
                            selectedPerson.Surname = people.Surname;
                            selectedPerson.ImageText = people.ImageText;
                            selectedPerson.IdPersonGender = people.IdPersonGender;

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
                else if (!IsUserWatcherReadOnly)
                {
                    if (obj is People)
                    {
                        People people = obj as People;

                        if (!SelectedAttendiesList.Any(x => x.IdPerson == people.IdPerson))
                        {
                            People selectedPerson = new People();

                            selectedPerson.IdPerson = people.IdPerson;
                            selectedPerson.Name = people.Name;
                            selectedPerson.Surname = people.Surname;
                            selectedPerson.ImageText = people.ImageText;
                            selectedPerson.IdPersonGender = people.IdPersonGender;

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
                GeosApplication.Instance.Logger.Log("Method AddAttendeeRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddAttendeeRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                if (!IsUserWatcherReadOnly)
                {
                    if (obj is People)
                    {
                        People people = obj as People;

                        if (!SelectedWatchersList.Any(x => x.IdPerson == people.IdPerson))
                        {
                            People selectedPerson = WatchersList.FirstOrDefault(x => x.IdPerson == people.IdPerson);
                            if (selectedPerson != null)
                            {
                                SelectedWatchersList.Add(selectedPerson);
                                WatchersList.Remove(WatchersList.FirstOrDefault(x => x.IdPerson == people.IdPerson));
                                WatchersCount = SelectedWatchersList.Count;
                            }
                        }
                    }
                }
                IsBusy = false;
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
                        People selectedPerson = WatchersList.FirstOrDefault(x => x.IdPerson == people.IdPerson);
                        if (selectedPerson != null)
                        {
                            SelectedWatchersList.Add(selectedPerson);
                            WatchersList.Remove(WatchersList.FirstOrDefault(x => x.IdPerson == people.IdPerson));
                            WatchersCount = SelectedWatchersList.Count;
                        }
                    }
                }
                IsBusy = false;
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
                            People selectedPerson = WatchersList.FirstOrDefault(x => x.IdPerson == item.IdPerson);
                            if (selectedPerson != null)
                            {
                                SelectedWatchersList.Add(selectedPerson);
                                WatchersList.Remove(WatchersList.FirstOrDefault(x => x.IdPerson == item.IdPerson));
                                WatchersCount = SelectedWatchersList.Count;
                            }
                        }
                    }

                }
                IsBusy = false;
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
                if (!IsPlannedAppointment)
                {
                    if (!IsUserWatcherReadOnly)
                    {
                        if (obj is People)
                        {
                            People person = obj as People;
                            AttendiesList.Add((People)SelectedAttendiesList.FirstOrDefault(x => x.IdPerson == person.IdPerson));
                            SelectedAttendiesList.Remove(SelectedAttendiesList.FirstOrDefault(x => x.IdPerson == person.IdPerson));
                            AttendeesCount = SelectedAttendiesList.Count;

                        }
                    }
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
                if (!IsUserWatcherReadOnly)
                {
                    if (obj is People)
                    {
                        People person = obj as People;
                        WatchersList.Add((People)SelectedWatchersList.FirstOrDefault(x => x.IdPerson == person.IdPerson));
                        SelectedWatchersList.Remove(SelectedWatchersList.FirstOrDefault(x => x.IdPerson == person.IdPerson));
                        WatchersCount = SelectedWatchersList.Count;

                    }
                }
                GeosApplication.Instance.Logger.Log("Method WatchersCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WatchersCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Attachment

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

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ActivityFileDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ListAttachment != null && ListAttachment.Count > 0)
                    {
                        ListAttachment.Remove((ActivityAttachment)attachmentObject);
                        isDelete = true;
                    }

                    if (isDelete)
                    {
                        //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDeleteSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
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
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

                FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                List<FileInfo> FileDetail = new List<FileInfo>();
                FileUploader activityAttachmentFileUploader = new FileUploader();
                activityAttachmentFileUploader.FileUploadName = GUIDCode.GUIDCodeString();
                GuidCode = activityAttachmentFileUploader.FileUploadName;

                if (ListAttachment != null && ListAttachment.Count > 0)
                {
                    foreach (ActivityAttachment fs in ListUpdateActivityAttachment)
                    {
                        if (fs.IsUploaded == true)
                        {
                            FileInfo file = new FileInfo(fs.FilePath);
                            FileDetail.Add(file);
                        }
                    }
                    activityAttachmentFileUploader.FileByte = ConvertZipToByte(FileDetail, activityAttachmentFileUploader.FileUploadName);
                    GeosApplication.Instance.Logger.Log("Getting Upload activity Attachment FileUploader Zip File ", category: Category.Info, priority: Priority.Low);
                    fileUploadReturnMessage = GeosRepositoryServiceController.UploaderActivityAttachmentZipFile(activityAttachmentFileUploader);
                    GeosApplication.Instance.Logger.Log("Getting Upload activity Attachment FileUploader Zip File successfully", category: Category.Info, priority: Priority.Low);
                }

                if (fileUploadReturnMessage.IsFileUpload == true)
                {
                    isupload = true;
                    //IsBusy = false;

                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
                    if (!string.IsNullOrEmpty(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    GeosApplication.Instance.Logger.Log("Method UploadFile() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    //IsBusy = false;
                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
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
                List<FileInfo> FileDetail = new List<FileInfo>();
                if (ActivityAttachmentList != null)
                {
                    foreach (ActivityAttachment item in ActivityAttachmentList)
                    {
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

        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        protected ISaveFileDialogService SaveFileDialogService
        {
            get
            {
                return this.GetService<ISaveFileDialogService>();
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
                IsBusy = true;
                bool isDownload = false;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ActivityFileDownloadMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    ActivityAttachment attachmentObject = (ActivityAttachment)obj;

                    SaveFileDialogService.DefaultExt = attachmentObject.FileType;
                    SaveFileDialogService.DefaultFileName = attachmentObject.OriginalFileName;
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

                        attachmentObject.FileUploadName = attachmentObject.SavedFileName;
                        attachmentObject.AttachmentImage = null;

                        attachmentObject = CrmStartUp.DownloadActivityAttachment(attachmentObject);

                        ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                        isDownload = SaveData(ResultFileName, attachmentObject.FileByte);
                    }
                    if (isDownload)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDownloadSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        IsBusy = false;
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDownloadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Attachment = new ActivityAttachment();
                    Attachment.FileType = file.Extension;
                    if (file.Name.Contains("."))
                    {
                        string[] a = file.Name.Split('.');
                        FileNameString = a[0];
                    }
                    else
                    {
                        FileNameString = file.Name;
                    }
                    Attachment.FilePath = file.FullName;
                    Attachment.OriginalFileName = FileNameString;
                    Attachment.SavedFileName = UniqueFileName + file.Extension;
                    Attachment.UploadedIn = GeosApplication.Instance.ServerDateTime;
                    Attachment.FileSize = file.Length;
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
        /// 
        /// </summary>
        /// <returns></returns>
        private byte[] ConvertZipToByte(List<FileInfo> filesDetail, string GuidCode)
        {
            byte[] filedetails = null;

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";
            string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolderForEdit\";
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
                            ListUpdateActivityAttachment[i].FileUploadName = ListUpdateActivityAttachment[i].SavedFileName;
                            System.IO.File.Copy(filesDetail[i].FullName, tempfolderPath + ListUpdateActivityAttachment[i].SavedFileName);
                            string s = tempfolderPath + ListUpdateActivityAttachment[i].SavedFileName;
                            archive.AddFile(s, @"/");
                            ListUpdateActivityAttachment[i].FilePath = s;
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
                if (IsPlannedAppointment)
                {
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
                }
                else if (!IsUserWatcherReadOnly)
                {
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
                if (IsPlannedAppointment)
                {
                    if (!IsUserWatcherReadOnly)
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
                    }
                }
                else if (!IsUserWatcherReadOnly)
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
                }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method ContactRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ContactRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompetitorRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CompetitorRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (!IsUserWatcherReadOnly)
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
                            else
                                activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueCompititor.png");
                            //FillPersonImageByIdPerson(activityLinkedItem, people.IdPerson);
                        }
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
                        else if (activityLinkedItem.People.IdPersonGender == 2)
                            activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
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
            GeosApplication.Instance.Logger.Log("Method OfferRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (IsPlannedAppointment)
                {
                    IsBusy = true;
                    if (Obj is Offer)
                    {
                        Offer offer = Obj as Offer;

                        ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();
                        activityLinkedItem.Name = offer.Code;

                        activityLinkedItem.IdOffer = offer.IdOffer;
                        activityLinkedItem.IdEmdepSite = Convert.ToInt32(offer.Site.ConnectPlantId);
                        activityLinkedItem.IdLinkedItemType = 44;
                        activityLinkedItem.LinkedItemType = new LookupValue();
                        activityLinkedItem.LinkedItemType.IdLookupValue = 44;
                        activityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();
                        activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/Opportunity.png");

                        activityLinkedItem.Offer = new Offer();
                        activityLinkedItem.Offer.Code = offer.Code;
                        activityLinkedItem.Offer.Description = offer.Description;
                        activityLinkedItem.Offer.IdOffer = offer.IdOffer;
                        activityLinkedItem.Offer.Site = new Company();
                        activityLinkedItem.Offer.Site.SiteNameWithoutCountry = offer.Site.SiteNameWithoutCountry;
                        activityLinkedItem.Offer.Site.Name = offer.Site.Name;

                        activityLinkedItem.Offer.Site.ConnectPlantId = activityLinkedItem.IdEmdepSite.Value.ToString();
                        // activityLinkedItem.Offer.Site.ConnectPlantConstr = conn;

                        SelectedActivityLinkedItems.Add(activityLinkedItem);
                        ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == offer.IdOffer && x.Site.ConnectPlantId == activityLinkedItem.IdEmdepSite.Value.ToString()));

                        Customer customer = CompanyGroupList[SelectedIndexCompanyGroup];
                        byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customer.CustomerName);
                        if (bytes != null)
                        {
                            activityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wOpportunity.png");
                            else
                                activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueOpportunity.png");
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
                        ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();
                        activityLinkedItem.Name = offer.Code;
                        activityLinkedItem.IdOffer = offer.IdOffer;
                        activityLinkedItem.IdEmdepSite = Convert.ToInt32(offer.Site.ConnectPlantId);
                        activityLinkedItem.IdLinkedItemType = 44;
                        activityLinkedItem.LinkedItemType = new LookupValue();
                        activityLinkedItem.LinkedItemType.IdLookupValue = 44;
                        activityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityOpportunity").ToString();
                        activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/Opportunity.png");

                        activityLinkedItem.Offer = new Offer();
                        activityLinkedItem.Offer.IdOffer = offer.IdOffer;
                        activityLinkedItem.Offer.Code = offer.Code;
                        activityLinkedItem.Offer.Description = offer.Description;

                        activityLinkedItem.Offer.Site = new Company();
                        activityLinkedItem.Offer.Site.SiteNameWithoutCountry = offer.Site.SiteNameWithoutCountry;
                        activityLinkedItem.Offer.Site.Name = offer.Site.Name;


                        activityLinkedItem.Offer.Site.ConnectPlantId = activityLinkedItem.IdEmdepSite.Value.ToString();

                        SelectedActivityLinkedItems.Add(activityLinkedItem);
                        ListPlantOpportunity.Remove(ListPlantOpportunity.FirstOrDefault(x => x.IdOffer == offer.IdOffer && x.Site.ConnectPlantId == activityLinkedItem.IdEmdepSite.Value.ToString()));

                        Customer customer = CompanyGroupList[SelectedIndexCompanyGroup];
                        byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customer.CustomerName);
                        if (bytes != null)
                        {
                            activityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wOpportunity.png");
                            else
                                activityLinkedItem.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueOpportunity.png");
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
        /// Method for account double click and add it in linked items.
        /// </summary>
        /// <param name="obj"></param>
        //private void AccountRowMouseDoubleClickCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method AccountRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {
        //        if (!IsUserWatcherReadOnly)
        //        {
        //            if (obj is Company)
        //            {
        //                IsBusy = true;
        //                Company company = obj as Company;

        //                if (!SelectedActivityLinkedItems.Any(x => x.LinkedItemType.IdLookupValue == 42))
        //                {
        //                    if (!SelectedActivityLinkedItems.Any(x => x.IdSite == company.IdCompany && x.LinkedItemType.IdLookupValue == 42))
        //                    {
        //                        ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();

        //                        activityLinkedItem.IdSite = company.IdCompany;
        //                        activityLinkedItem.Name = company.Customers[0].CustomerName + " - " + company.Name;

        //                        activityLinkedItem.IdLinkedItemType = 42;
        //                        activityLinkedItem.LinkedItemType = new LookupValue();
        //                        activityLinkedItem.LinkedItemType.IdLookupValue = 42;
        //                        activityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

        //                        // Set account location to activity location
        //                        CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
        //                        customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
        //                        if (SelectedActivityType.IdLookupValue == 37)   // Appointment
        //                        {
        //                            if (string.IsNullOrEmpty(ActivityAddress))
        //                            {
        //                                ActivityAddress = company.Address;
        //                                if (company.Latitude != null && company.Longitude != null)
        //                                {
        //                                    customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude = Convert.ToDouble(company.Latitude);
        //                                    customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude = Convert.ToDouble(company.Longitude);
        //                                    MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

        //                                    Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
        //                                    Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
        //                                }
        //                                else
        //                                {
        //                                    MapLatitudeAndLongitude = null;
        //                                    Latitude = null;
        //                                    Longitude = null;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (!string.IsNullOrEmpty(company.Address))
        //                                {
        //                                    if (!ActivityAddress.Contains(company.Address))
        //                                    {
        //                                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["CustomerEditViewChangeLocationDetails"].ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
        //                                        if (MessageBoxResult == MessageBoxResult.Yes)
        //                                        {
        //                                            ActivityAddress = company.Address;
        //                                            if (company.Latitude != null && company.Longitude != null)
        //                                            {
        //                                                customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude = Convert.ToDouble(company.Latitude);
        //                                                customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude = Convert.ToDouble(company.Longitude);
        //                                                MapLatitudeAndLongitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude;

        //                                                Latitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Latitude;
        //                                                Longitude = customerSetLocationMapViewModel.MapLatitudeAndLongitude.Longitude;
        //                                            }
        //                                            else
        //                                            {
        //                                                MapLatitudeAndLongitude = null;
        //                                                Latitude = null;
        //                                                Longitude = null;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        activityLinkedItem.Company = company;
        //                        activityLinkedItem.Customer = company.Customers[0];

        //                        SelectedActivityLinkedItems.Insert(0, activityLinkedItem);
        //                        SelectedAccountActivityLinkedItemsCount = 1;
        //                        CompaniesList.Remove(CompaniesList.FirstOrDefault(x => x.IdCompany == company.IdCompany));

        //                        //Display image for account.
        //                        try
        //                        {
        //                            byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(company.Customers[0].CustomerName);
        //                            if (bytes != null)
        //                            {
        //                                activityLinkedItem.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
        //                            }
        //                        }
        //                        catch (FaultException<ServiceException> ex)
        //                        {
        //                            IsBusy = false;
        //                            GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //                        }
        //                        catch (ServiceUnexceptedException ex)
        //                        {
        //                            IsBusy = false;
        //                            GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //                            GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //                        }

        //                        //Add contacts as per account.
        //                        try
        //                        {
        //                            LinkeditemsPeopleList = new ObservableCollection<People>(CrmStartUp.GetContactsByLinkedItemAccount(Convert.ToString(company.IdCompany)).ToList());
        //                            LinkeditemsPeopleListCopy = new List<People>();

        //                            foreach (People item in LinkeditemsPeopleList)
        //                            {
        //                                LinkeditemsPeopleListCopy.Add((People)item.Clone());
        //                            }
        //                        }
        //                        catch (FaultException<ServiceException> ex)
        //                        {
        //                            IsBusy = false;
        //                            GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //                            CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //                        }
        //                        catch (ServiceUnexceptedException ex)
        //                        {
        //                            IsBusy = false;
        //                            GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //                            GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //                        }

        //                        //Attendees
        //                        AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList(Convert.ToString(company.IdCompany)).ToList());
        //                        //Make one main copy.
        //                        AttendiesListCopy = new List<People>();
        //                        foreach (People people in AttendiesList)
        //                        {
        //                            AttendiesListCopy.Add((People)people.Clone());
        //                        }

        //                        //remove attendee if not related to account.
        //                        for (int i = SelectedAttendiesList.Count - 1; i > -1; i--)
        //                        {
        //                            if (!AttendiesList.Any(x => x.IdPerson == SelectedAttendiesList[i].IdPerson))
        //                            {
        //                                //If grid attendies list does not contain idperson then remove it from selected attendies list. 
        //                                SelectedAttendiesList.RemoveAt(i);
        //                            }
        //                            else
        //                            {
        //                                //If selected attendies list contains idperson then remove it from attendies grid.
        //                                AttendiesList.Remove(AttendiesList.FirstOrDefault(x => x.IdPerson == SelectedAttendiesList[i].IdPerson));
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        IsBusy = false;
        //        GeosApplication.Instance.Logger.Log("Method AccountRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in AccountRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

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
                        if (obj is ActivityLinkedItem)
                        {
                            ActivityLinkedItem linkedItem = obj as ActivityLinkedItem;

                            if (linkedItem != null && linkedItem.IdLinkedItemType == 42)     //Account
                            {
                                SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42 && x.IdSite == linkedItem.IdSite));
                                if (IsInformationVisible == Visibility.Visible)
                                    isInfoAccountRemove = true;
                                else
                                    isInfoAccountRemove = false;
                                SelectedAccountActivityLinkedItemsCount = 0;


                                //Remove contacts if account is removed.

                                SelectedActivityLinkedItems.Where(l => l.IdLinkedItemType == 43 && l.IsVisible == true).ToList().All(i => SelectedActivityLinkedItems.Remove(i));

                                ///Remove attendees related to account - 0 means load all sales people.
                                AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030("0").ToList());
                                //Create copy of new list.


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
                            else if (linkedItem != null && linkedItem.IdLinkedItemType == 43)  // contact
                            {
                                LinkeditemsPeopleList.Add((People)SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 43 && x.IdPerson == linkedItem.IdPerson).People.Clone());
                                SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 43 && x.IdPerson == linkedItem.IdPerson));

                            }
                            else if (linkedItem != null && linkedItem.IdLinkedItemType == 44)  // opportunity
                            {
                                ListPlantOpportunity.Add((Offer)SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 44 && x.IdEmdepSite == linkedItem.IdEmdepSite && x.IdOffer == linkedItem.IdOffer).Offer.Clone());
                                SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 44 && x.IdEmdepSite == linkedItem.IdEmdepSite && x.IdOffer == linkedItem.IdOffer));
                                FillPlantOpportunityList(false, new List<Offer>());
                            }
                            else if (linkedItem != null && linkedItem.IdLinkedItemType == 45)  // car project
                            {
                                CarProjectList.Add((CarProject)SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 45 && x.IdCarProject == linkedItem.IdCarProject).CarProject.Clone());
                                SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 45 && x.IdCarProject == linkedItem.IdCarProject));
                                FillPlantOpportunityList(false, new List<Offer>());
                            }
                            else if (linkedItem != null && linkedItem.IdLinkedItemType == 92)  // Competitor
                            {
                                LinkeditemsCompetitorList.Add((Competitor)SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 92 && x.IdCompetitor == linkedItem.IdCompetitor).Competitor.Clone());
                                SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 92 && x.IdCompetitor == linkedItem.IdCompetitor));
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

        #endregion //Linked items

        /// <summary>
        /// Method for fill activity Owner list.
        /// </summary>
        private void FillOwnerList(Int32 idActivityOwner)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOwnerList ...", category: Category.Info, priority: Priority.Low);

                User user = new User();
                user = WorkbenchStartUp.GetUserById(idActivityOwner);             //(GeosApplication.Instance.ActiveUser.IdUser);

                //[pramod.misal][GEOS2-4688][27.09.2023]
               
                if (user.IsEnabled==1)
                {
                    ActivityOwnerList = new List<User>();
                    ActivityOwnerList.Add(user);
                }
               

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

        private void FillActivityComments(long idActivity)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActivityComments ...", category: Category.Info, priority: Priority.Low);

                ActivityCommentsList = new ObservableCollection<LogEntriesByActivity>(CrmStartUp.GetlogEntriesByActivity(idActivity, 1).OrderByDescending(acl => acl.Datetime));   //1 for comment

                SetUserProfileImage(ActivityCommentsList);
                RtfToPlaintext();

                GeosApplication.Instance.Logger.Log("Method FillActivityComments() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityComments() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityComments() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityComments() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillActivityChangeLog(long idActivity)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActivityChangeLog ...", category: Category.Info, priority: Priority.Low);

                ActivityChangeLogList = new ObservableCollection<LogEntriesByActivity>(CrmStartUp.GetlogEntriesByActivity(idActivity, 2).OrderByDescending(i => i.IdLogEntryByActivity).ToList());   //1 for comment

                GeosApplication.Instance.Logger.Log("Method FillActivityChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityChangeLog() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityChangeLog() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityChangeLog() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to fill All Masters
        /// </summary>
        void FillAllMasters()
        {

            if (IsUserWatcherReadOnly == false)
            {
                FillTypeList();
                FillTaskStatusList();
                FillCarProject();
                FillCompetitor();

                FillTagsList();
                FillAttendiesList();
                FillWatchersList();
            }
        }

        private void FillCarProject()
        {

            if (GeosApplication.Instance.GeosCarProjectsList == null)
            {
                GeosApplication.Instance.GeosCarProjectsList = CrmStartUp.GetCarProject(0).ToList();
            }

            CarProjectList = new ObservableCollection<CarProject>(GeosApplication.Instance.GeosCarProjectsList);
            //CarProjectListCopy = new List<CarProject>();
            //foreach (CarProject item in CarProjectList)
            //{
            //    CarProjectListCopy.Add((CarProject)item.Clone());
            //}

            if (_Activity.ActivityLinkedItem != null)
            {
                if (SelectedActivityLinkedItems == null)
                    SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>();

                foreach (ActivityLinkedItem activityLinkedItem in _Activity.ActivityLinkedItem)
                {
                    SelectedActivityLinkedItems.Add(activityLinkedItem);
                    CarProjectList.Remove(CarProjectList.FirstOrDefault(x => x.IdCarProject == activityLinkedItem.IdCarProject));
                }
            }
        }

        /// <summary>
        /// This method is used to fill linked items e.g. Amount
        /// </summary>
        //private void FillLinkedItems()
        //{
        //    GeosApplication.Instance.Logger.Log("Method FillLinkedItems ...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {
        //        if (SelectedActivityLinkedItems == null)
        //            SelectedActivityLinkedItems = new ObservableCollection<ActivityLinkedItem>();
        //        if (_Activity.ActivityLinkedItem != null)
        //        {
        //            foreach (ActivityLinkedItem activityLinkedItem in _Activity.ActivityLinkedItem)
        //            {
        //                SelectedActivityLinkedItems.Add(activityLinkedItem);
        //            //    CarProjectList.Remove(CarProjectList.FirstOrDefault(x => x.IdCarProject == activityLinkedItem.IdCarProject));
        //            }
        //        }
        //        GeosApplication.Instance.Logger.Log("Method FillLinkedItems() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        IsBusy = false;
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        IsBusy = false;
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        IsBusy = false;
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillLinkedItems() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        void FillCompanyGroupListWhenIsConfirm()
        {
            CompanyGroupList = new ObservableCollection<Customer>();
            CompanyGroupList.Insert(0, new Customer() { IdCustomer = 0, CustomerName = "---" });
            CompanyGroupList.Add(_Activity.ActivityLinkedItem.Where(i => i.Customer.IdCustomer != 0).Select(i => i.Customer).FirstOrDefault());

            SelectedIndexCompanyGroup = 1;

            CompanyPlantList = new ObservableCollection<Company>();
            CompanyPlantList.Insert(0, new Company() { IdCompany = 0, Name = "---" });
            Company tempCompany = new Company();
            tempCompany = _Activity.ActivityLinkedItem.Where(i => i.Company.IdCompany != 0).Select(i => i.Company).FirstOrDefault();

            if (tempCompany != null)
            {
                tempCompany.Name = tempCompany.SiteNameWithoutCountry;
                tempCompany.SiteNameWithoutCountry = tempCompany.Name;
                CompanyPlantList.Add(tempCompany);
            }

            SelectedIndexCompanyPlant = 1;
        }

        /// <summary>
        /// Method for fill Type list.
        /// </summary>
        private void FillTypeList()
        {
            try
            {
                //TODO:Ravi- Filled TypeList only when its empty
                if (_Activity.IdOwner != GeosApplication.Instance.ActiveUser.IdUser && GeosApplication.Instance.IsPermissionAuditor == false)
                {
                    TypeList = new List<LookupValue>();
                    TypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                    TypeList.Add(_Activity.LookupValue);
                    if (GeosApplication.Instance.IsPermissionAuditor == true && _Activity.LookupValue.IdLookupValue == 37)
                        TypeList.Add(CrmStartUp.GetLookupValues(9).Where(i => i.IdLookupValue == 96).FirstOrDefault());
                    SelectedIndexType = 1;
                    return;
                }
                else
                {
                    if (_Activity.LookupValue.IdLookupValue == 96)
                    {
                        if (isPlannedAppointment || GeosApplication.Instance.IsPermissionAuditor == true)
                            TypeList = new List<LookupValue>();
                        TypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                        TypeList.Add(_Activity.LookupValue);
                        SelectedIndexType = 1;
                        return;

                    }

                    //if (_Activity.LookupValue.IdLookupValue==37)
                    //{
                    //    TypeList = new List<LookupValue>();
                    //    TypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                    //    TypeList.Add(_Activity.LookupValue);

                    //    if (GeosApplication.Instance.IsPermissionAuditor == true)
                    //    TypeList.Add(CrmStartUp.GetLookupValues(9).Where(i=>i.IdLookupValue==96).FirstOrDefault());

                    //    SelectedIndexType = 1;
                    //    return;
                    //}

                    if (TypeList == null || TypeList.Count < 3)
                    {
                        GeosApplication.Instance.Logger.Log("Method FillTypeList ...", category: Category.Info, priority: Priority.Low);

                        List<LookupValue> temptypeList = CrmStartUp.GetLookupValues(9).ToList();
                        TypeList = new List<LookupValue>();
                        TypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                        if (GeosApplication.Instance.IsPermissionAuditor == false)
                            temptypeList = temptypeList.Where(i => i.IdLookupValue != 96).ToList();


                        TypeList.AddRange(temptypeList);

                        GeosApplication.Instance.Logger.Log("Method FillTypeList() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                }
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

        /// <summary>
        /// Fill Status list for Task type.
        /// </summary>
        private void FillTaskStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTaskStatusList ...", category: Category.Info, priority: Priority.Low);
                if (_Activity.IdOwner != GeosApplication.Instance.ActiveUser.IdUser)
                {
                    TaskStatusList = new List<LookupValue>();
                    TaskStatusList.Add(_Activity.ActivityStatus);
                }
                else
                {
                    if (TaskStatusList == null || TaskStatusList.Count < 3)
                    {
                        TaskStatusList = new List<LookupValue>();
                        TaskStatusList = CrmStartUp.GetLookupValues(11).ToList();
                    }
                }
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
        /// Fill attendies list by selected Account in linked items.
        /// </summary>
        private void FillAttendiesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAttendiesList..", category: Category.Info, priority: Priority.Low);

                if (_Activity.IdOwner != GeosApplication.Instance.ActiveUser.IdUser && GeosApplication.Instance.IsPermissionAuditor == false)
                {
                    //Add items in selected items.
                    SelectedAttendiesList = new ObservableCollection<People>();
                    foreach (ActivityAttendees activityAttendee in _Activity.ActivityAttendees)
                    {

                        if (activityAttendee.People != null)
                            SelectedAttendiesList.Add(activityAttendee.People);
                    }
                }
                else
                {
                    ActivityLinkedItem activityLinkedItem = _Activity.ActivityLinkedItem.FirstOrDefault(x => x.IdLinkedItemType == 42 && !x.IsDeleted);



                    if (activityLinkedItem != null && activityLinkedItem.IdSite != null)
                    {
                       //AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030(activityLinkedItem.IdSite.ToString()).ToList());
                        //[rajashri.telvekar][GEOS2-4689][28-9-2023]
                        AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2440("0").ToList());
                    }
                    else
                    {
                        if (AttendiesList == null)
                            AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030("0").ToList());
                    }

                    //Add items in selected items.
                    SelectedAttendiesList = new ObservableCollection<People>();
                    foreach (ActivityAttendees activityAttendee in _Activity.ActivityAttendees)
                    {
                        SelectedAttendiesList.Add(activityAttendee.People);

                        if (AttendiesList.Any(x => x.IdPerson == activityAttendee.IdUser))
                        {
                            AttendiesList.Remove(AttendiesList.Where(x => x.IdPerson == activityAttendee.IdUser).FirstOrDefault());
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillAttendiesList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fill attendies list by selected Account in linked items.
        /// </summary>
        private void FillWatchersList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWatchersList..", category: Category.Info, priority: Priority.Low);
                if (_Activity.IdOwner != GeosApplication.Instance.ActiveUser.IdUser)
                {
                    // Add items in selected items.
                    SelectedWatchersList = new ObservableCollection<People>();
                    foreach (ActivityWatcher activityAttendee in _Activity.ActivityWatchers)
                    {
                        if (activityAttendee.People != null)
                        {
                            SelectedWatchersList.Add(activityAttendee.People);
                        }
                    }
                }
                else
                {
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

                    // Add items in selected items.
                    SelectedWatchersList = new ObservableCollection<People>();
                    foreach (ActivityWatcher activityAttendee in _Activity.ActivityWatchers)
                    {
                        People people = WatchersList.FirstOrDefault(x => x.IdPerson == activityAttendee.IdUser);
                        if (people != null)
                        {
                            SelectedWatchersList.Add(people);
                            WatchersList.Remove(people);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillWatchersList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttendiesList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWatchersList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWatchersList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


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

            //if(!IsUserWatcherEnabled)
            if (IsPlannedAppointment)
            {
                if (!IsActivityAddressReadOnly)
                    customerSetLocationMapViewModel.IsAcceptButtonEnabled = true;
                else
                    customerSetLocationMapViewModel.IsAcceptButtonEnabled = false;
            }
            else
                customerSetLocationMapViewModel.IsAcceptButtonEnabled = IsUserWatcherEnabled;
            //else
            //customerSetLocationMapViewModel.IsAcceptButtonEnabled = true;

            customerSetLocationMapView.DataContext = customerSetLocationMapViewModel;
            var ownerInfo = (obj as FrameworkElement);
            customerSetLocationMapView.Owner = Window.GetWindow(ownerInfo);
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

        #region Comments

        private void CommentButtonCheckedCommandAction(object obj)
        {
            // if (string.IsNullOrEmpty(OldActivityComment))
            // {
            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            NewActivityComment = "";
            OldActivityComment = "";
            // }

            //ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            //  NewActivityComment  = "";
            //  OldActivityComment = "";
        }

        private void CommentButtonUncheckedCommandAction(object obj)
        {
            //if (string.IsNullOrEmpty(OldActivityComment))
            // {
            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            NewActivityComment = null;
            OldActivityComment = null;
            IsRtf = false;
            IsNormal = true;

            // }

            //IsAdd = true;
            //ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            //NewActivityComment = null;
            //OldActivityComment = null;
        }

        private void CommentDoubleClickCommandAction(object obj)
        {
            IsBusy = true;
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
                OldActivityComment = String.Copy(commentOffer.Comments);
                NewActivityComment = String.Copy(commentOffer.Comments);

                if (commentOffer.IsRtfText == true)
                    IsRtf = true;
                else
                    IsNormal = true;

                ShowCommentsFlyout = true;
            }
            else
            {
                NewActivityComment = null;
                OldActivityComment = null;
                ShowCommentsFlyout = false;
                CustomMessageBox.Show("Not Allowed to update comment!", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            IsBusy = false;
        }

        /// <summary>
        /// Method for Add Comment
        /// </summary>
        /// <param name="leadComment"></param>
        public void AddCommentCommandAction(object gcComments)
        {
            //IsBusy = true;
            string TempOldActivityComment = string.Empty;
            string TempNewActivityComment = string.Empty;
            if (IsRtf)
            {
                var document = ((RichTextBox)gcComments).Document;
                NewActivityComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
                TempNewActivityComment = NewActivityComment;
                string convertedText = string.Empty;
                //string TempOldActivityComment = string.Empty;
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
                    //}
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
                }
                NewActivityComment = convertedText;
            }
            else
            {
                TempNewActivityComment = NewActivityComment;
            }

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

                        if (comment.IsRtfText)
                        {
                            TextRange range = null;
                            var rtb = new RichTextBox();
                            var doc = new FlowDocument();
                            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(OldActivityComment.ToString()));
                            range = new TextRange(doc.ContentStart, doc.ContentEnd);
                            range.Load(stream, DataFormats.Rtf);

                            if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                                TempOldActivityComment = range.Text;
                        }
                        else
                        {
                            TempOldActivityComment = OldActivityComment;
                        }

                        if (IsRtf)
                        {
                            comment.IsRtfText = true;
                            IsRtf = false;

                        }
                        else if (IsNormal)
                        {
                            comment.IsRtfText = false;
                        }

                        TempOldActivityComment = "'" + TempOldActivityComment + "'";
                        TempOldActivityComment = TempOldActivityComment.Replace("\r\n", "").TrimEnd();

                        TempNewActivityComment = "'" + TempNewActivityComment + "'";
                        TempNewActivityComment = TempNewActivityComment.TrimEnd();

                        ChangedLogsEntries = new List<LogEntriesByActivity>();
                        ChangedLogsEntries.Add(new LogEntriesByActivity() { IdLogEntryByActivity = comment.IdLogEntryByActivity, IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentUpdated").ToString(), TempOldActivityComment, TempNewActivityComment), IdLogEntryType = 2 });
                    }

                    OldActivityComment = null;
                    NewActivityComment = null;
                }

                ActivityCommentsList = new ObservableCollection<LogEntriesByActivity>(ActivityCommentsList.OrderByDescending(c => c.Datetime).ToList());
            }
            else if (CommentButtonText == System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString()) //Add comment.
            {
                if (!string.IsNullOrEmpty(NewActivityComment) && !string.IsNullOrEmpty(NewActivityComment.Trim())) // Add Comment
                {
                    if (IsRtf)
                    {
                        LogEntriesByActivity comment = new LogEntriesByActivity()
                        {
                            People = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Copy(NewActivityComment.Trim()),
                            IdActivity = _Activity.IdActivity,
                            IdLogEntryType = 1,
                            IsUpdated = false,
                            IsDeleted = false,
                            IsRtfText = true
                        };
                        comment.People.OwnerImage = SetUserProfileImage();
                        ActivityCommentsList.Add(comment);
                        ActivityCommentsList = new ObservableCollection<LogEntriesByActivity>(ActivityCommentsList.OrderByDescending(c => c.Datetime).ToList());
                        SelectedComment = comment;
                    }
                    else if (IsNormal)
                    {
                        LogEntriesByActivity comment = new LogEntriesByActivity()
                        {
                            People = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Copy(NewActivityComment.Trim()),
                            IdActivity = _Activity.IdActivity,
                            IdLogEntryType = 1,
                            IsUpdated = false,
                            IsDeleted = false,
                            IsRtfText = false
                        };
                        comment.People.OwnerImage = SetUserProfileImage();
                        ActivityCommentsList.Add(comment);
                        ActivityCommentsList = new ObservableCollection<LogEntriesByActivity>(ActivityCommentsList.OrderByDescending(c => c.Datetime).ToList());
                        SelectedComment = comment;
                    }

                    OldActivityComment = null;
                    NewActivityComment = null;
                }
            }

            //document.Blocks.Clear();
            ShowCommentsFlyout = false;
            NewActivityComment = "";
            IsRtf = false;
            IsNormal = true;
            if (ChangedLogsEntries != null)
            {
                ListOfChangeLogUpdateComments = new List<LogEntriesByActivity>();
                ListOfChangeLogUpdateComments.AddRange(ChangedLogsEntries.Where(j => j.IsUpdated = true).ToList());
            }
            // IsBusy = false;
            //((GridControl)gcComments).Focus();
        }

        public void AddAutoComment(string AttendiesString)
        {
            GeosApplication.Instance.Logger.Log("Method AddAutoComment ...", category: Category.Info, priority: Priority.Low);
            try
            {
                LogEntriesByActivity comment = new LogEntriesByActivity()
                {
                    //People = new People { IdPerson = GeosApplication.Instance.ActiveUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                    IdUser = 164,//GeosApplication.Instance.ActiveUser.IdUser,
                    Datetime = GeosApplication.Instance.ServerDateTime,                    
                    Comments = string.Format(System.Windows.Application.Current.FindResource("AutoComment").ToString(), AttendiesString),// + AttendiesString, //string.Copy(NewActivityComment.Trim()),
                    IdActivity = _Activity.IdActivity,
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
            GeosApplication.Instance.Logger.Log("Method AddAutoComment Completed...", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Deleting Comments
        /// </summary>
        /// <param name="obj"></param>
        public void DeleteCommentCommandAction(object parameter)
        {
            LogEntriesByActivity commentObject = (LogEntriesByActivity)parameter;

            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (commentObject.IdUser != GeosApplication.Instance.ActiveUser.IdUser)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("EditActivityDeleteComment").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteComment").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                if (ActivityCommentsList != null && ActivityCommentsList.Count > 0)
                {
                    ActivityCommentsList.Remove(ActivityCommentsList.FirstOrDefault(x => x.IdLogEntryByActivity == commentObject.IdLogEntryByActivity && x.Comments == commentObject.Comments));
                }
            }

            ShowCommentsFlyout = false;
            NewActivityComment = null;
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

        // Export Excel .xlsx
        // public virtual string ResultFileName { get; set; }
        //public virtual bool DialogResult { get; set; }
        private void ExporttoExcel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportActivityCommentGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                //saveFile.DefaultExt = "xlsx";
                //saveFile.FileName = "ActivityComments";
                //saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                //saveFile.FilterIndex = 1;
                //saveFile.Title = "Save Excel Report";
                //DialogResult = (Boolean)saveFile.ShowDialog();

                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "ActivityComments";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                DialogResult = SaveFileDialogService.ShowDialog();

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

                    // ResultFileName = (saveFile.FileName);
                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;

                    TextRange range = null;
                    //Workbook workbook = new Workbook();                
                    //workbook.Worksheets.Insert(0, "ActivityComments");
                    //Worksheet ws = workbook.Worksheets["ActivityComments"];
                    SpreadsheetControl control = new SpreadsheetControl();
                    Worksheet ws = control.ActiveWorksheet;
                    ws.Name = "ActivityComments";
                    ws.Cells[0, 0].Value = "User";
                    ws.Cells[0, 0].Font.Bold = true;
                    ws.Cells[0, 0].ColumnWidth = 400;
                    ws.Cells[0, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[0, 1].Value = "Comments Date";
                    ws.Cells[0, 1].Font.Bold = true;
                    ws.Cells[0, 1].ColumnWidth = 400;
                    ws.Cells[0, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    ws.Cells[0, 2].Value = "Comments";
                    ws.Cells[0, 2].Font.Bold = true;
                    ws.Cells[0, 2].ColumnWidth = 1000;
                    ws.Cells[0, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                    int counter = 1;
                    if (ActivityCommentsList.Count > 0)
                    {
                        for (int i = 0; i < ActivityCommentsList.Count; i++)
                        {
                            var rtb = new RichTextBox();
                            var doc = new FlowDocument();
                            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(ActivityCommentsList[i].Comments.ToString()));
                            range = new TextRange(doc.ContentStart, doc.ContentEnd);
                            range.Load(stream, DataFormats.Rtf);
                            //ListLogComments[i].Comments = range.Text;
                            ws.Cells[counter, 0].Value = ActivityCommentsList[i].People.FullName;
                            ws.Cells[counter, 1].Value = ActivityCommentsList[i].Datetime;
                            ws.Cells[counter, 2].Value = range.Text;
                            ws.Cells[counter, 2].Alignment.WrapText = true;
                            counter++;
                        }
                    }

                    //using (FileStream stream = new FileStream(ResultFileName, FileMode.Create, FileAccess.ReadWrite))
                    //{
                    //    workbook.SaveDocument(stream, DocumentFormat.OpenXml);
                    //}
                    //
                    //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    //System.Diagnostics.Process.Start(ResultFileName);

                    control.SaveDocument(ResultFileName);
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AboutChangelogExportSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportActivityCommentGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportActivityCommentGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        private bool UpdateActivityReadOnly(Int64 idActivity)
        {
            List<LogEntriesByActivity> newList = new List<LogEntriesByActivity>();
            if (IsUserWatcherReadOnly == true)
            {
                ChangedLogsEntries = new List<LogEntriesByActivity>();
                // objActivity.CommentsByActivity = new List<LogEntriesByActivity>();
                foreach (LogEntriesByActivity item in _Activity.CommentsByActivity)
                {
                    if (!ActivityCommentsList.Any(x => x.IdLogEntryByActivity == item.IdLogEntryByActivity))
                    {
                        item.IsDeleted = true;
                        item.People = new People();
                        item.People.OwnerImage = null;
                        //objActivity.CommentsByActivity.Add(item);
                        ActivityCommentsList.Add(item);
                    }
                }
                newList = ActivityCommentsList.Where(x => x.IdLogEntryByActivity == 0 || x.IsDeleted || x.IsUpdated).Distinct().ToList();
                foreach (LogEntriesByActivity item in newList)
                {
                    string oldTempComment = item.Comments;
                    string newTempComment = string.Empty;
                    string pattern = "[^\\w]";
                    string[] words = null;
                    int i = 0;
                    int count = 0;
                    item.People.OwnerImage = null;
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
                            ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = idActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });

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
                            ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = idActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });
                        }
                    }

                    if (item.IsDeleted)
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

                            ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = idActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentRemoved").ToString(), newTempComment), IdLogEntryType = 2 });
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
                            ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = idActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentRemoved").ToString(), newTempComment), IdLogEntryType = 2 });
                        }
                    }
                }
                //CrmStartUp.AddLogEntriesByActivity(_Activity. );
            }

            if (ActivityCommentsList != null)
                newList.AddRange(ActivityCommentsList.Where(x => x.IdLogEntryByActivity != 0 && (x.IsDeleted)).ToList().Distinct());

            if (ListOfChangeLogUpdateComments != null)
            {
                ChangedLogsEntries.AddRange(ListOfChangeLogUpdateComments.Where(l => l.IdLogEntryByActivity != 0 && (l.IsUpdated)).ToList());
            }

            Activity objActivityReadOnly = new Activity();
            // Add attachment
            if (ListAttachment.Count > 0)
            {
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                foreach (ActivityAttachment attach in ListAttachment)
                {
                    if (!_Activity.ActivityAttachment.Any(x => x.IdActivityAttachment == attach.IdActivityAttachment))
                    {
                        ListUpdateActivityAttachment.Add(attach);
                        attach.AttachmentImage = null;
                    }
                }

                if (ListUpdateActivityAttachment.Count > 0)
                {
                    bool IsUpdateAttachment = UploadActivityAttachment();               // Upload Server  attachment method

                    if (IsUpdateAttachment == true)
                    {
                        foreach (var item in ListUpdateActivityAttachment)
                        {
                            ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAttachment").ToString(), item.OriginalFileName + item.FileType), IdLogEntryType = 2 });
                            if (objActivityReadOnly.ActivityAttachment == null)
                                objActivityReadOnly.ActivityAttachment = new List<ActivityAttachment>();
                            objActivityReadOnly.ActivityAttachment.Add(item);

                        }
                        objActivityReadOnly.GUIDString = GuidCode;
                    }
                }
                IsBusy = false;
            }
            //End attachments.

            bool changelogResult = false;
            bool commentsResult = false;

            //Activity objActivityReadOnly = new Activity();
            objActivityReadOnly.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
            objActivityReadOnly.IdActivity = idActivity;
            objActivityReadOnly.LogEntriesByActivity = new List<LogEntriesByActivity>();
            objActivityReadOnly.LogEntriesByActivity = ChangedLogsEntries;
            objActivityReadOnly.CommentsByActivity = new List<LogEntriesByActivity>();
            objActivityReadOnly.CommentsByActivity = newList;

            bool result = CrmStartUp.UpdateReadOnlyActivityLogs(objActivityReadOnly);

            if (ActivityCommentsList != null)
            {
                SetUserProfileImage(ActivityCommentsList);
                RtfToPlaintext();
            }
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
            // return true;
        }

        /// <summary>
        /// AddNewActivitytAccept
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateActivitytAcceptCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateActivitytAcceptCommandAction ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                EditActivityError = null;
                if (SelectedAttendiesList != null)
                {
                    AttendeesCount = SelectedAttendiesList.Count;
                    SelectedAccountActivityLinkedItemsCount = SelectedActivityLinkedItems.Where(x => x.IdLinkedItemType == 42).Count();
                }

                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    EditActivityError = null;
                else
                    EditActivityError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));
                //PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                //PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));
                //PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                //PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexOwner"));
                PropertyChanged(this, new PropertyChangedEventArgs("ActivityAddress"));
                PropertyChanged(this, new PropertyChangedEventArgs("Subject"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexAttendies"));
                PropertyChanged(this, new PropertyChangedEventArgs("AttendeesCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("DueDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexTaskStatus"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedAccountActivityLinkedItemsCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                PropertyChanged(this, new PropertyChangedEventArgs("EditActivityError"));
                bool result = false;

                if (IsUserWatcherReadOnly == true)
                {
                    result = UpdateActivityReadOnly(_Activity.IdActivity);
                    if (result)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditActivityOwnerUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditActivityOwnerUpdateFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    AddChangedLogsDetails();

                    objActivity = new Activity();
                    objActivity.IdActivity = _Activity.IdActivity;
                    objActivity.IdActivityType = TypeList[SelectedIndexType].IdLookupValue;
                    if (!string.IsNullOrEmpty(Subject))
                        objActivity.Subject = Subject.Trim();
                    if (!string.IsNullOrEmpty(Description))
                        objActivity.Description = Description.Trim();
                    objActivity.IdOwner = ActivityOwnerList[SelectedIndexOwner].IdUser;
                    objActivity.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    objActivity.IsCompleted = (byte)(IsCompleted ? 1 : 0);

                    //If appointment is visible then only add below properties.
                    if (IsAppointmentVisible == Visibility.Visible)
                    {
                        //objActivity.FromDate = DueDate;
                        //objActivity.ToDate = DueDate;
                        if (!string.IsNullOrEmpty(ActivityAddress))
                            objActivity.Location = ActivityAddress.Trim();
                        objActivity.Latitude = Latitude;
                        objActivity.Longitude = Longitude;
                        objActivity.IsInternal = Convert.ToByte(IsInternal);

                        // For Attendies Add/Delete.
                        List<ActivityAttendees> listAttendees = new List<ActivityAttendees>();
                        List<string> lstAttendies = new List<string>();
                        foreach (People item in SelectedAttendiesList)
                        {
                            if (!_Activity.ActivityAttendees.Any(x => x.IdUser == item.IdPerson))
                            {
                                ActivityAttendees objAttendies = new ActivityAttendees();
                                objAttendies.IdUser = item.IdPerson;
                                objAttendies.IdActivity = objActivity.IdActivity;
                                listAttendees.Add(objAttendies);
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAttendees").ToString(), item.FullName), IdLogEntryType = 2 });
                            }
                            lstAttendies.Add(((People)item).FullName.ToString());
                        }

                        AttendiesString = string.Join(", ", lstAttendies.ToList());
                        foreach (ActivityAttendees item in _Activity.ActivityAttendees)
                        {
                            if (!SelectedAttendiesList.Any(x => x.IdPerson == item.IdUser))
                            {
                                ActivityAttendees objAttendies = new ActivityAttendees();
                                objAttendies.IdUser = item.IdUser;
                                objAttendies.IdActivity = objActivity.IdActivity;
                                objAttendies.IsDeleted = true;
                                listAttendees.Add(objAttendies);

                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveAttendees").ToString(), item.People.FullName), IdLogEntryType = 2 });
                            }
                        }
                        if (IsCompleted)
                        {
                            if (_Activity.IsCompleted != objActivity.IsCompleted)
                                AddAutoComment(AttendiesString);
                        }

                        objActivity.ActivityAttendees = listAttendees;
                        //End for Attendies Add/Delete.
                    }
                    else if (IsEmailorCallVisible == Visibility.Visible)
                    {
                    }
                    else if (IsTaskVisible == Visibility.Visible)
                    {
                        objActivity.IdActivityStatus = TaskStatusList[SelectedIndexTaskStatus].IdLookupValue;
                        objActivity.ActivityStatus = TaskStatusList[SelectedIndexTaskStatus];
                        objActivity.IsCompleted = (byte)(objActivity.IdActivityStatus == 48 ? 1 : 0);
                        objActivity.IsInternal = Convert.ToByte(IsInternal);
                        // For Watchers Add/Delete.
                        List<ActivityWatcher> listWatchers = new List<ActivityWatcher>();

                        foreach (People item in SelectedWatchersList)
                        {
                            if (!_Activity.ActivityWatchers.Any(x => x.IdUser == item.IdPerson))
                            {
                                ActivityWatcher objWatchers = new ActivityWatcher();
                                objWatchers.IdUser = item.IdPerson;
                                objWatchers.IdActivity = objActivity.IdActivity;
                                listWatchers.Add(objWatchers);
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddWatchers").ToString(), item.FullName), IdLogEntryType = 2 });
                            }
                        }

                        foreach (ActivityWatcher item in _Activity.ActivityWatchers)
                        {
                            if (!SelectedWatchersList.Any(x => x.IdPerson == item.IdUser))
                            {
                                ActivityWatcher objWatchers = new ActivityWatcher();
                                objWatchers.IdUser = item.IdUser;
                                objWatchers.IdActivity = objActivity.IdActivity;
                                objWatchers.IsDeleted = true;
                                listWatchers.Add(objWatchers);

                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveWatchers").ToString(), item.People.FullName), IdLogEntryType = 2 });
                            }
                        }

                        objActivity.ActivityWatchers = listWatchers;
                        //End for Watchers Add/Delete.
                    }
                    else if (IsInformationVisible == Visibility.Visible)
                    {
                        if (!string.IsNullOrEmpty(ActivityAddress))
                            objActivity.Location = ActivityAddress.Trim();
                        objActivity.Latitude = Latitude;
                        objActivity.Longitude = Longitude;
                        objActivity.IsInternal = Convert.ToByte(IsInternal);

                        objActivity.FromDate = GeosApplication.Instance.ServerDateTime;
                        objActivity.ToDate = GeosApplication.Instance.ServerDateTime;
                        objActivity.IsCompleted = 1;

                        List<ActivityWatcher> listWatchers = new List<ActivityWatcher>();

                        foreach (People item in SelectedWatchersList)
                        {
                            if (!_Activity.ActivityWatchers.Any(x => x.IdUser == item.IdPerson))
                            {
                                ActivityWatcher objWatchers = new ActivityWatcher();
                                objWatchers.IdUser = item.IdPerson;
                                objWatchers.IdActivity = objActivity.IdActivity;
                                listWatchers.Add(objWatchers);
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddWatchers").ToString(), item.FullName), IdLogEntryType = 2 });
                            }
                        }

                        foreach (ActivityWatcher item in _Activity.ActivityWatchers)
                        {
                            if (!SelectedWatchersList.Any(x => x.IdPerson == item.IdUser))
                            {
                                ActivityWatcher objWatchers = new ActivityWatcher();
                                objWatchers.IdUser = item.IdUser;
                                objWatchers.IdActivity = objActivity.IdActivity;
                                objWatchers.IsDeleted = true;
                                listWatchers.Add(objWatchers);

                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveWatchers").ToString(), item.People.FullName), IdLogEntryType = 2 });
                            }
                        }
                        objActivity.ActivityWatchers = listWatchers;
                    }

                    if (IsDueDateVisible == Visibility.Visible)
                    {
                        objActivity.FromDate = DueDate;
                        objActivity.ToDate = DueDate;
                    }

                    //Comments
                    //objActivity.CommentsByActivity = new List<LogEntriesByActivity>();
                    //objActivity.CommentsByActivity = ActivityCommentsList.Where(x => x.IdLogEntryByActivity == 0 || x.IsDeleted || x.IsUpdated).ToList();
                    //objActivity.CommentsByActivity.ForEach(it => it.People.OwnerImage = null);

                    objActivity.CommentsByActivity = new List<LogEntriesByActivity>();

                    foreach (LogEntriesByActivity item in _Activity.CommentsByActivity)
                    {
                        if (!ActivityCommentsList.Any(x => x.IdLogEntryByActivity == item.IdLogEntryByActivity))
                        {
                            item.IsDeleted = true;
                            //objActivity.CommentsByActivity.Add(item);
                            ActivityCommentsList.Add(item);
                        }
                    }

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
                        objActivity.CommentsByActivity.Add(newobj);

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
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });
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
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });
                            }
                        }

                        if (item.IsDeleted)
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

                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentRemoved").ToString(), newTempComment), IdLogEntryType = 2 });
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
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCommentRemoved").ToString(), newTempComment), IdLogEntryType = 2 });
                            }
                        }
                    }

                    //Comments

                    //foreach (LogEntriesByActivity item in _Activity.CommentsByActivity)
                    //{
                    //    if (!ActivityCommentsList.Any(x => x.IdLogEntryByActivity == item.IdLogEntryByActivity))
                    //    {
                    //        item.IsDeleted = true;
                    //        objActivity.CommentsByActivity.Add(item);
                    //    }
                    //}

                    objActivity.LogEntriesByActivity = ChangedLogsEntries;

                    //Linked Items
                    List<ActivityLinkedItem> listActivityLinkedItems = new List<ActivityLinkedItem>();

                    //Delete linked item from list
                    foreach (ActivityLinkedItem item in _Activity.ActivityLinkedItem)
                    {
                        if (!SelectedActivityLinkedItems.Any(x => x.IdActivityLinkedItem == item.IdActivityLinkedItem))
                        {
                            ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();
                            activityLinkedItem = (ActivityLinkedItem)item.Clone();

                            activityLinkedItem.IsDeleted = true;
                            activityLinkedItem.IdActivity = _Activity.IdActivity;
                            activityLinkedItem.IdActivityLinkedItem = item.IdActivityLinkedItem;
                            activityLinkedItem.ActivityLinkedItemImage = null;

                            if (item.IdLinkedItemType == 42)        //Account
                            {
                                if (isInfoAccountRemove)
                                {
                                    activityLinkedItem.IdLinkedItemType = 42;
                                    activityLinkedItem.IdSite = item.IdSite;

                                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveAccount").ToString(), item.Name), IdLogEntryType = 2 });
                                }
                                else
                                {
                                    activityLinkedItem.IdLinkedItemType = 42;
                                    activityLinkedItem.IdSite = item.IdSite;

                                    foreach (var item1 in SelectedActivityLinkedItems)
                                    {
                                        if (item1.IdLinkedItemType == 42)
                                        {
                                            if (item1.Customer.IdCustomer != item.IdCustomer && item1.IdSite != item.IdSite)
                                            {
                                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveAccount").ToString(), item.Name), IdLogEntryType = 2 });
                                            }
                                        }
                                    }
                                }

                                if (IsInternal)
                                {
                                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveAccount").ToString(), item.Name), IdLogEntryType = 2 });
                                }
                            }
                            else if (item.IdLinkedItemType == 43)   // Contact
                            {
                                activityLinkedItem.IdLinkedItemType = 43;
                                activityLinkedItem.IdPerson = item.IdPerson;
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveContact").ToString(), item.Name), IdLogEntryType = 2 });
                            }
                            else if (item.IdLinkedItemType == 44)   // Opportunity
                            {
                                activityLinkedItem.IdLinkedItemType = 44;
                                activityLinkedItem.IdOffer = item.IdOffer;
                                activityLinkedItem.IdEmdepSite = item.IdEmdepSite;
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveOpportunity").ToString(), item.Name), IdLogEntryType = 2 });
                                //if (item.LinkedItemType.Value == "Order")
                                //{
                                //    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveOrder").ToString(), item.Name), IdLogEntryType = 2 });
                                //}
                                //else
                                //{
                                //    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveOpportunity").ToString(), item.Name), IdLogEntryType = 2 });
                                //}
                            }
                            else if (item.IdLinkedItemType == 45)   // Project
                            {
                                activityLinkedItem.IdLinkedItemType = 45;
                                activityLinkedItem.IdCarProject = item.IdCarProject;
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveProject").ToString(), item.Name), IdLogEntryType = 2 });
                            }
                            else if (item.IdLinkedItemType == 92)   // Competitor
                            {
                                activityLinkedItem.IdLinkedItemType = 92;
                                activityLinkedItem.IdCompetitor = item.IdCompetitor;
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveCompetitor").ToString(), item.Name), IdLogEntryType = 2 });
                            }

                            //activityLinkedItem.Name = item.Name;
                            listActivityLinkedItems.Add(activityLinkedItem);
                        }
                    }

                    //Add linked item to list.
                    foreach (ActivityLinkedItem item in SelectedActivityLinkedItems)
                    {
                        if (!_Activity.ActivityLinkedItem.Any(x => x.IdActivityLinkedItem == item.IdActivityLinkedItem))
                        {
                            ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();
                            activityLinkedItem = (ActivityLinkedItem)item.Clone();
                            activityLinkedItem.IdActivity = _Activity.IdActivity;
                            activityLinkedItem.ActivityLinkedItemImage = null;

                            if (item.IdLinkedItemType == 42)        //Account
                            {
                                activityLinkedItem.IdLinkedItemType = 42;
                                activityLinkedItem.IdSite = item.IdSite;
                                //if (item.Name != _Activity.ActivityLinkedItem)
                                bool isProceed = false;
                                int count = 0;
                                bool isAccount = true;

                                foreach (var item1 in _Activity.ActivityLinkedItem)
                                {
                                    if (item1.IdLinkedItemType == 42)
                                    {
                                        if (item1.Customer.IdCustomer != item.IdCustomer && item1.IdSite != item.IdSite)
                                        {
                                            ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAccount").ToString(), item.Name), IdLogEntryType = 2 });
                                            isProceed = true;
                                            isAccount = false;
                                            count++;
                                        }
                                        isAccount = false;
                                    }
                                }

                                if (!IsInternal)
                                {
                                    if (isProceed && count != 1)
                                        ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAccount").ToString(), item.Name), IdLogEntryType = 2 });
                                    if (isAccount)
                                        ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAccount").ToString(), item.Name), IdLogEntryType = 2 });
                                }

                                listActivityLinkedItems.Insert(0, activityLinkedItem);
                            }
                            else if (item.IdLinkedItemType == 43)   // Contact
                            {
                                activityLinkedItem.IdLinkedItemType = 43;
                                activityLinkedItem.IdPerson = item.IdPerson;
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddContact").ToString(), item.Name), IdLogEntryType = 2 });
                                listActivityLinkedItems.Add(activityLinkedItem);
                            }
                            else if (item.IdLinkedItemType == 44)
                            {
                                activityLinkedItem.IdLinkedItemType = 44;
                                activityLinkedItem.IdOffer = item.IdOffer;
                                activityLinkedItem.IdEmdepSite = item.IdEmdepSite;
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOpportunity").ToString(), item.Name), IdLogEntryType = 2 });
                                listActivityLinkedItems.Add(activityLinkedItem);
                            }
                            else if (item.IdLinkedItemType == 45)   // Project
                            {
                                activityLinkedItem.IdLinkedItemType = 45;
                                activityLinkedItem.IdCarProject = item.IdCarProject;
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddProject").ToString(), item.Name), IdLogEntryType = 2 });
                                listActivityLinkedItems.Add(activityLinkedItem);
                            }
                            else if (item.IdLinkedItemType == 92)   // Competitor
                            {
                                activityLinkedItem.IdLinkedItemType = 92;
                                activityLinkedItem.IdCompetitor = item.IdCompetitor;
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddCompetitor").ToString(), item.Name), IdLogEntryType = 2 });
                                listActivityLinkedItems.Add(activityLinkedItem);
                            }

                            //activityLinkedItem.Name = item.Name;
                        }
                    }

                    objActivity.ActivityLinkedItem = listActivityLinkedItems;

                    //End Linked Items.

                    // Delete attachment
                    List<ActivityAttachment> listActivityAttachmentItems = new List<ActivityAttachment>();
                    foreach (ActivityAttachment attach in _Activity.ActivityAttachment)
                    {
                        if (!ListAttachment.Any(x => x.IdActivityAttachment == attach.IdActivityAttachment))
                        {
                            ActivityAttachment activityAttachment = new ActivityAttachment();
                            activityAttachment.IsDeleted = 1;
                            activityAttachment.IdActivity = _Activity.IdActivity;
                            activityAttachment.IdActivityAttachment = attach.IdActivityAttachment;
                            activityAttachment.FileType = attach.FileType;
                            if (attach.SavedFileName.Contains("."))
                            {
                                string[] a = attach.SavedFileName.Split('.');
                                FileNameString = a[0];
                            }
                            else
                            {
                                FileNameString = attach.SavedFileName;
                            }
                            activityAttachment.FilePath = attach.FilePath;
                            activityAttachment.OriginalFileName = attach.OriginalFileName;
                            activityAttachment.SavedFileName = attach.SavedFileName;
                            activityAttachment.UploadedIn = GeosApplication.Instance.ServerDateTime;
                            activityAttachment.FileSize = attach.FileSize;
                            activityAttachment.FileType = attach.FileType;
                            activityAttachment.FileUploadName = attach.OriginalFileName + attach.FileType;
                            ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveAttachment").ToString(), attach.OriginalFileName + attach.FileType), IdLogEntryType = 2 });
                            listActivityAttachmentItems.Add(activityAttachment);
                        }
                    }

                    objActivity.ActivityAttachment = listActivityAttachmentItems;

                    if (ListAttachment.Count > 0)                   // For add activity attachment
                    {
                        DXSplashScreen.Show<SplashScreenView>();
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        IsBusy = true;
                        foreach (ActivityAttachment attach in ListAttachment)
                        {
                            if (!_Activity.ActivityAttachment.Any(x => x.IdActivityAttachment == attach.IdActivityAttachment))
                            {
                                ListUpdateActivityAttachment.Add(attach);
                                attach.AttachmentImage = null;
                            }
                        }

                        if (ListUpdateActivityAttachment.Count > 0)
                        {

                            bool IsUpdateAttachment = UploadActivityAttachment();               // Upload Server  attachment method

                            if (IsUpdateAttachment == true)
                            {
                                foreach (var item in ListUpdateActivityAttachment)
                                {
                                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAttachment").ToString(), item.OriginalFileName + item.FileType), IdLogEntryType = 2 });
                                    objActivity.ActivityAttachment.Add(item);
                                }
                                objActivity.GUIDString = GuidCode;
                            }
                        }
                        IsBusy = false;
                    }
                    //End attachments.

                    // Delete Tag
                    if (SelectedTagList == null) { SelectedTagList = new List<object>(); }
                    if (SelectedTagList != null)
                    {
                        List<Tag> temptaglist = SelectedTagList.Cast<Tag>().ToList();
                        List<ActivityTag> listtag = new List<ActivityTag>();
                        List<string> lstTag = new List<string>();
                        foreach (ActivityTag item in _Activity.ActivityTags)
                        {
                            if (!temptaglist.Any(x => x.IdTag == item.IdTag))
                            {
                                ActivityTag activitytag = new ActivityTag();
                                activitytag.IsDeleted = true;
                                activitytag.IdActivity = _Activity.IdActivity;
                                activitytag.IdTag = item.IdTag;
                                activitytag.IdActivityTag = item.IdActivityTag;
                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogRemoveTag").ToString(), item.Tag.Name), IdLogEntryType = 2 });
                                listtag.Add(activitytag);
                            }
                        }

                        // For Add tag
                        foreach (Tag tagitem in temptaglist)
                        {
                            if (!_Activity.ActivityTags.Any(x => x.IdTag == tagitem.IdTag))
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

                                ActivityTag activitytag = new ActivityTag();
                                activitytag.IsDeleted = false;
                                activitytag.IdActivity = _Activity.IdActivity;
                                activitytag.IdTag = tagitem.IdTag;

                                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddTag").ToString(), tagitem.Name), IdLogEntryType = 2 });
                                listtag.Add(activitytag);
                            }
                            lstTag.Add(((Tag)tagitem).Name.ToString());
                        }

                        TagsString = string.Join(" , ", lstTag.ToList());
                        objActivity.ActivityTags = listtag;
                    }
                    objActivity.IsDeleted = 0;

                    if (!string.IsNullOrEmpty(ReminderCalenderString))
                    {
                        // Get current time zone.
                        TimeZone zone = TimeZone.CurrentTimeZone;
                        string standard = zone.StandardName;
                        string daylight = zone.DaylightName;

                        var utcNow = ReminderDate.Value.ToUniversalTime(); // Converted utc time
                        var otherTimezone = TimeZoneInfo.FindSystemTimeZoneById(GeosApplication.Instance.CurrentSiteTimeZone); // Get other timezone  [rdixit][GEOS2-3871][09.01.2023]
                        var newTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, otherTimezone);
                        ReminderDate = newTime;
                        objActivity.ActivityReminderDateTime = ReminderDate;
                    }

                    result = CrmStartUp.UpdateActivity_V2031(objActivity);
                    if (result)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditActivityOwnerUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                        //Filled data to show on the grid/update grid
                        objActivity.LookupValue = TypeList[SelectedIndexType];
                        objActivity.People = new People();
                        objActivity.People.Name = ActivityOwnerList[SelectedIndexOwner].FirstName;
                        objActivity.People.Surname = ActivityOwnerList[SelectedIndexOwner].LastName;
                        objActivity.ActivityTagsString = TagsString;

                        if (!string.IsNullOrEmpty(AttendiesString))
                        {
                            AttendiesString = AttendiesString.Replace(",", "\n");
                            objActivity.ActivityAttendeesString = AttendiesString;
                        }
                        objActivity.ActivityLinkedItem = new List<ActivityLinkedItem>();
                        objActivity.ActivityLinkedItem.Add(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42 && !x.IsDeleted));

                        //if (objActivity.ActivityLinkedItem != null && objActivity.ActivityLinkedItem.Count == 0)
                        //{
                        //    // added this because edit and save without any modification then selected linked items are zero
                        //    objActivity.ActivityLinkedItem.Add(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42 && !x.IsDeleted));
                        //}
                    }
                    else
                    {
                        objActivity = null;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditActivityOwnerUpdateFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                IsBusy = false;

                GeosApplication.Instance.Logger.Log("Method UpdateActivitytAcceptCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                objActivity = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateActivitytAcceptCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                objActivity = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateActivitytAcceptCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                objActivity = null;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateActivitytAcceptCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                if (ListUpdateActivityAttachment.Count > 0)
                {
                    string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolderForEdit\";
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

        private void AddChangedLogsDetails()
        {
            GeosApplication.Instance.Logger.Log("Method AddChangedLogsDetails ...", category: Category.Info, priority: Priority.Low);

            if (ChangedLogsEntries == null)
                ChangedLogsEntries = new List<LogEntriesByActivity>();

            //Internal
            if (_Activity.IsInternal != null && !_Activity.IsInternal.Equals(Convert.ToByte(IsInternal)))
            {
                if (Convert.ToByte(IsInternal) == 1)
                {
                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogInternal").ToString()), IdLogEntryType = 2 });
                }
                else
                {
                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogNotInternal").ToString()), IdLogEntryType = 2 });
                }
            }

            //Type
            if (_Activity.LookupValue != null && _Activity.LookupValue.IdLookupValue != TypeList[SelectedIndexType].IdLookupValue)
            {
                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogType").ToString(), _Activity.LookupValue.Value, TypeList[SelectedIndexType].Value), IdLogEntryType = 2 });
            }

            //Subject
            if (_Activity.Subject != null && !_Activity.Subject.Equals(Subject))
            {
                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogSubject").ToString(), _Activity.Subject, Subject), IdLogEntryType = 2 });
            }

            //Description
            if (_Activity.Description != null && !_Activity.Description.Equals(Description))
            {
                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogDescription").ToString(), _Activity.Description, Description), IdLogEntryType = 2 });
            }

            //Completed
            if ((bool)(_Activity.IsCompleted == 1 ? true : false) != IsCompleted)
            {
                ChangedLogsEntries.Add(new LogEntriesByActivity()
                {
                    IdActivity = _Activity.IdActivity,
                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                    Datetime = GeosApplication.Instance.ServerDateTime,
                    Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogCompleted").ToString(), _Activity.IsCompleted == 1 ? "complete" : "incomplete", IsCompleted ? "complete" : "incomplete"),
                    IdLogEntryType = 2
                });
            }

            //If Appointment is visible then only add 
            if (IsAppointmentVisible == Visibility.Visible)
            {
                #region ToDate,FromDate & StartTime,EndTime Commented
                ////From date
                //if (_Activity.FromDate != null && _Activity.FromDate.Value.Date != FromDate.Date)
                //{
                //    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogFromDate").ToString(), _Activity.FromDate.Value.ToShortDateString(), FromDate.ToShortDateString()), IdLogEntryType = 2 });
                //}

                ////Start time
                //if (_Activity.FromDate != null && _Activity.FromDate.Value.TimeOfDay != StartTime.TimeOfDay)
                //{
                //    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogStartTime").ToString(), _Activity.FromDate.Value.ToString("hh\\:mm\\ tt"), StartTime.ToString("hh\\:mm\\ tt")), IdLogEntryType = 2 });
                //}

                ////To date
                //if (_Activity.ToDate != null && _Activity.ToDate.Value.Date != ToDate.Date)
                //{
                //    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogToDate").ToString(), _Activity.ToDate.Value.ToShortDateString(), ToDate.ToShortDateString()), IdLogEntryType = 2 });
                //}

                ////End time
                //if (_Activity.ToDate != null && _Activity.ToDate.Value.TimeOfDay != EndTime.TimeOfDay)
                //{
                //    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogEndTime").ToString(), _Activity.ToDate.Value.ToString("hh\\:mm\\ tt"), EndTime.ToString("hh\\:mm\\ tt")), IdLogEntryType = 2 });
                //}
                #endregion

                //Location
                if (string.IsNullOrEmpty(_Activity.Location))
                {
                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogLocationAdd").ToString(), ActivityAddress), IdLogEntryType = 2 });
                }
                else if (_Activity.Location != null && !_Activity.Location.Equals(ActivityAddress))
                {
                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogLocation").ToString(), _Activity.Location, ActivityAddress), IdLogEntryType = 2 });
                }
            }

            //Due Date -- Change fromdate to duedate.
            if (IsDueDateVisible == Visibility.Visible)
            {
                if (_Activity.FromDate == null)
                {
                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddDueDate").ToString(), DueDate.Value.ToShortDateString()), IdLogEntryType = 2 });
                }
                else if (_Activity.FromDate != null && _Activity.FromDate.Value.Date != DueDate.Value.Date)
                {
                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogChangeDueDate").ToString(), _Activity.FromDate.Value.ToShortDateString(), DueDate.Value.ToShortDateString()), IdLogEntryType = 2 });
                }
            }

            //Status -- Change 
            if (IsTaskVisible == Visibility.Visible)
            {
                if (_Activity.ActivityStatus != null && _Activity.ActivityStatus.IdLookupValue != TaskStatusList[SelectedIndexTaskStatus].IdLookupValue)
                {
                    ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogChangeStatus").ToString(), _Activity.ActivityStatus.Value, TaskStatusList[SelectedIndexTaskStatus].Value), IdLogEntryType = 2 });
                }
            }

            //Owner
            if (_Activity.People != null && _Activity.People.IdPerson != ActivityOwnerList[SelectedIndexOwner].IdUser)
            {
                ChangedLogsEntries.Add(new LogEntriesByActivity() { IdActivity = _Activity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogOwner").ToString(), _Activity.People.FullName, ActivityOwnerList[SelectedIndexOwner].FullName), IdLogEntryType = 2 });
            }

            GeosApplication.Instance.Logger.Log("Method AddChangedLogsDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

       

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
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


                        if (!SelectedActivityLinkedItems.Any(x => x.LinkedItemType.IdLookupValue == 42))
                        {
                            if (!SelectedActivityLinkedItems.Any(x => x.IdSite == company.IdCompany && x.LinkedItemType.IdLookupValue == 42))
                            {
                                ActivityLinkedItem activityLinkedItem = new ActivityLinkedItem();
                                activityLinkedItem.IdSite = company.IdCompany;
                                activityLinkedItem.Name = Group.CustomerName + " - " + company.Name;

                                activityLinkedItem.Company = company;
                                activityLinkedItem.Customer = Group;
                                //activityLinkedItem.Company.Name = company.SiteNameWithoutCountry;
                                activityLinkedItem.IdLinkedItemType = 42;
                                activityLinkedItem.LinkedItemType = new LookupValue();
                                activityLinkedItem.LinkedItemType.IdLookupValue = 42;
                                activityLinkedItem.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                                //On init edit activity don't add address from group and plant forcefully. 

                                if ((SelectedActivityType.IdLookupValue == 37 || SelectedActivityType.IdLookupValue == 96) && !isInit)   // Appointment And PlannedAppointment
                                {
                                    //// Set account location to activity location
                                    CustomerSetLocationMapViewModel customerSetLocationMapViewModel = new CustomerSetLocationMapViewModel();
                                    customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();
                                    customerSetLocationMapViewModel.MapLatitudeAndLongitude = new GeoPoint();

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
                                        if (!isInit)
                                        {
                                            if (!string.IsNullOrEmpty(company.Address))
                                            {
                                                //if (!ActivityAddress.Contains(company.Address))
                                                if (ActivityAddress != company.Address)
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
                                //  LinkeditemsPeopleList = new ObservableCollection<People>(CrmStartUp.GetContactsByLinkedItemAccount_V2030(Convert.ToString(company.IdCompany)).ToList());
                                //LinkeditemsPeopleListCopy = new List<People>();
                                //foreach (People item in LinkeditemsPeopleList)
                                //{
                                //    LinkeditemsPeopleListCopy.Add((People)item.Clone());
                                //}

                                //Attendees
                                AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030(Convert.ToString(company.IdCompany)).ToList());


                                //remove attendee if not related to account.
                                if (SelectedAttendiesList != null && SelectedAttendiesList.Count > 0)
                                {
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

                SelectedActivityLinkedItems.Remove(SelectedActivityLinkedItems.FirstOrDefault(x => x.IdLinkedItemType == 42));
                SelectedAccountActivityLinkedItemsCount = 0;
                SelectedActivityLinkedItems.Where(l => l.IdLinkedItemType == 44 && l.IsVisible == true).ToList().All(i => SelectedActivityLinkedItems.Remove(i));

                SelectedActivityLinkedItems.Where(l => l.IdLinkedItemType == 43 && l.IsVisible == true).ToList().All(i => SelectedActivityLinkedItems.Remove(i));

                ///Remove attendees related to account
                AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList_V2030("0").ToList());
                //if (_Activity.ActivityLinkedItem != null && _Activity.ActivityLinkedItem.Where(i=>i.IdLinkedItemType==42).FirstOrDefault().IdSite != null)
                //{
                //    AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList(_Activity.ActivityLinkedItem.Where(i => i.IdLinkedItemType == 42).FirstOrDefault().IdSite.ToString()).ToList());
                //}
                //else
                //{
                //    if (AttendiesList == null)
                //        AttendiesList = new ObservableCollection<People>(CrmStartUp.GetAllAttendesList("0").ToList());
                //}
                //remove attendee if not related to account.
                if (SelectedAttendiesList != null && SelectedAttendiesList.Count > 0)
                {
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
                }

                if (string.IsNullOrEmpty(ActivityAddress) && !isInit)
                {
                    ActivityAddress = string.Empty;
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
                                //TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2150(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemCompaniesDetails, GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer).ToList().Where(i => !idOffers.Contains(i.IdOffer)).ToList());
                                //[pramod.misal][GEOS2-5347][16.02.2024]
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
                      
                        //TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2150(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(i => i.Alias == GeosApplication.Instance.SiteName).FirstOrDefault(), GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer).ToList().Where(i => !idOffers.Contains(i.IdOffer)).ToList());
                        //[pramod.misal][GEOS2-5347][16.02.2024]
                        TempPlantOpportunity.AddRange(CrmStartUp.GetOffersByIdCustomerToLinkedWithActivities_V2490(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(i => i.Alias == GeosApplication.Instance.SiteName).FirstOrDefault(), GeosApplication.Instance.IdUserPermission, CompanyGroupList[SelectedIndexCompanyGroup].IdCustomer).ToList().Where(i => !idOffers.Contains(i.IdOffer)).ToList());
                    }
                }

                
                if (SelectedActivityLinkedItems != null && SelectedActivityLinkedItems.Any(i => i.IdLinkedItemType == 45))
                {
                    List<Int64> idsCarProject = new List<Int64>(SelectedActivityLinkedItems.Where(i => i.IdLinkedItemType == 45).Select(i => i.IdCarProject.Value).ToList());
                    TempPlantOpportunity = TempPlantOpportunity.Where(i=>i.IdCarProject!=null).ToList().Where(i => idsCarProject.Contains(i.IdCarProject.Value)).ToList();
                }
                ListPlantOpportunity = new ObservableCollection<Offer>(TempPlantOpportunity);
                if (ListPlantOpportunity != null && ListPlantOpportunity.Count > 0)
                {
                    foreach (ActivityLinkedItem item in SelectedActivityLinkedItems.Where(i => i.IdLinkedItemType == 44))
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
        /// Method for fill emdep Group list.
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                if (!IsPlannedAppointment)
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

                    GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
                }
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
                if (!IsPlannedAppointment)
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
                                //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                                //[pramod.misal][GEOS2-4682][08-08-2023]
                                EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser_V2420(salesOwnersIds, true));
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                            }
                        }
                    }
                    else
                    {
                        if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                            EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"];
                        else
                        {
                            // EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                            //[pramod.misal][GEOS2-4682][08-08-2023]
                            EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                            GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);

                        }
                    }

                    GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
                }
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
        /// Method for fill Contacts list.
        /// </summary>

        private void FillContactsList()
        {
            try
            {
                if (IsUserWatcherReadOnly == false)
                {
                    GeosApplication.Instance.Logger.Log("Method FillContactsList ...", category: Category.Info, priority: Priority.Low);
                    string idSites = string.Join(",", CompanyPlantList.Select(x => x.IdCompany));
                    LinkeditemsPeopleList = new ObservableCollection<People>(CrmStartUp.GetContactsByLinkedItemAccount_V2030(idSites).ToList().OrderBy(x => x.Company.Name));
                    GeosApplication.Instance.Logger.Log("Method FillContactsList() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillContactsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        else if (user.IdUserGender == null)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else if (user.IdUserGender == 2)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        else if (user.IdUserGender == null)
                            user.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
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
        /// Method for Set Comment User  image.
        /// </summary>
        private void SetUserProfileImage(ObservableCollection<LogEntriesByActivity> ActivityCommentsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in ActivityCommentsList)
                {
                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(item.People.Login);

                    if (UserProfileImageByte != null)
                        item.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (item.People.IdPersonGender == 2)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                            else if (item.People.IdPersonGender == null)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");

                        }
                        else
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (item.People.IdPersonGender == 2)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                            else if (item.People.IdPersonGender == null)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
                        }
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
        //[GEOS2-1931][28.12.2022][rdixit]
        //private void LoadedCommandAction(object obj)
        //{

        //    GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
        //    try
        //    {
        //        System.Windows.RoutedEventArgs arg = (System.Windows.RoutedEventArgs)obj;
        //        System.Windows.Controls.Grid Grid = (System.Windows.Controls.Grid)arg.OriginalSource;
        //        Grid.Width = Grid.ActualWidth;
        //        Grid.Height = screenHeight;
        //        GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
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
                   me[BindableBase.GetPropertyName(() => Subject)] +
                    me[BindableBase.GetPropertyName(() => Description)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexType)] +

                    #region ToDate,FromDate & StartTime,EndTime Commented
                    //me[BindableBase.GetPropertyName(() => FromDate)] +
                    //me[BindableBase.GetPropertyName(() => ToDate)] +
                    //me[BindableBase.GetPropertyName(() => StartTime)] +
                    //me[BindableBase.GetPropertyName(() => EndTime)] +
                    #endregion
                    me[BindableBase.GetPropertyName(() => SelectedIndexOwner)] +
                    me[BindableBase.GetPropertyName(() => ActivityAddress)] +
                    me[BindableBase.GetPropertyName(() => AttendeesCount)] +
                    me[BindableBase.GetPropertyName(() => DueDate)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexTaskStatus)] +
                    me[BindableBase.GetPropertyName(() => SelectedAccountActivityLinkedItemsCount)] +
                    me[BindableBase.GetPropertyName(() => EditActivityError)]
                  ;
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
                     me[BindableBase.GetPropertyName(() => SelectedIndexOwner)] +
                       me[BindableBase.GetPropertyName(() => ActivityAddress)] +
                       me[BindableBase.GetPropertyName(() => Subject)] +
                       me[BindableBase.GetPropertyName(() => AttendeesCount)] +
                       me[BindableBase.GetPropertyName(() => DueDate)] +
                       me[BindableBase.GetPropertyName(() => SelectedIndexTaskStatus)] +
                       me[BindableBase.GetPropertyName(() => SelectedAccountActivityLinkedItemsCount)] +
                       me[BindableBase.GetPropertyName(() => EditActivityError)]
                      ;
                }

                //me[BindableBase.GetPropertyName(() => SelectedAttendiesList)];

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
                string subjectProp = BindableBase.GetPropertyName(() => Subject);
                string descriptionProp = BindableBase.GetPropertyName(() => Description);
                string selectedIndexType = BindableBase.GetPropertyName(() => SelectedIndexType);   // Lead Source
                string selectedIndexCompanyGroup = BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup);
                string selectedIndexCompanyPlant = BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant);
                #region ToDate,FromDate & StartTime,EndTime Commented
                //string fromdateProp = BindableBase.GetPropertyName(() => FromDate);
                //string toDateProp = BindableBase.GetPropertyName(() => ToDate);
                //string startTimeProp = BindableBase.GetPropertyName(() => StartTime);
                //string endTimeProp = BindableBase.GetPropertyName(() => EndTime);
                #endregion
                string selectedIndexOwnerProp = BindableBase.GetPropertyName(() => SelectedIndexOwner);
                string customerAddressProp = BindableBase.GetPropertyName(() => ActivityAddress);
                string attendeesCountProp = BindableBase.GetPropertyName(() => AttendeesCount);
                string dueDateProp = BindableBase.GetPropertyName(() => DueDate);
                string selectedIndexTaskStatusProp = BindableBase.GetPropertyName(() => SelectedIndexTaskStatus);
                string selectedAccountActivityLinkedItemsCountProp = BindableBase.GetPropertyName(() => SelectedAccountActivityLinkedItemsCount);

                string editActivityErrorProp = BindableBase.GetPropertyName(() => EditActivityError);

                //string selectedAttendiesListProp = BindableBase.GetPropertyName(() => SelectedAttendiesList);
                //  string errorMessage = string.Empty; ;
                if (columnName == subjectProp)
                {
                    return ActivityValidation.GetErrorMessage(subjectProp, Subject);

                }
                else if (columnName == descriptionProp)
                    return ActivityValidation.GetErrorMessage(descriptionProp, Description);
                else if (columnName == selectedIndexType)
                    return ActivityValidation.GetErrorMessage(selectedIndexType, SelectedIndexType);
                else if (columnName == selectedIndexCompanyGroup && IsAccountVisible == Visibility.Visible && IsInformationVisible == Visibility.Collapsed)     //Group
                {
                    return ActivityValidation.GetErrorMessage(selectedIndexCompanyGroup, SelectedIndexCompanyGroup);
                }
                else if (columnName == selectedIndexCompanyPlant && IsAccountVisible == Visibility.Visible && IsInformationVisible == Visibility.Collapsed)     //Plant
                {
                    return ActivityValidation.GetErrorMessage(selectedIndexCompanyPlant, SelectedIndexCompanyPlant);
                }
                #region ToDate,FromDate & StartTime,EndTime Commented
                //else if (columnName == fromdateProp && IsAppointmentVisible == Visibility.Visible)      //From date
                //{
                //    int result = DateTime.Compare(FromDate.Date, ToDate.Date);
                //    if (result > 0)
                //        return "The date you entered occurs after the end date.";

                //    //return ActivityValidation.GetErrorMessage(fromdateProp, FromDate);
                //}
                //else if (columnName == startTimeProp && IsAppointmentVisible == Visibility.Visible)     //Start time.
                //{
                //    if (DateTime.Compare(FromDate.Date, ToDate.Date) == 0) //Same date.
                //    {
                //        int result = DateTime.Compare(StartTime, EndTime);
                //        if (result > 0)
                //            return "The time you entered occurs after the end time.";
                //    }
                //    //return ActivityValidation.GetErrorMessage(startTimeProp, StartTime);
                //}
                //else if (columnName == toDateProp && IsAppointmentVisible == Visibility.Visible)        //TO date
                //{
                //    int result = DateTime.Compare(ToDate.Date, FromDate.Date);
                //    if (result < 0)
                //        return "The date you entered occurs before the start date.";

                //    //return ActivityValidation.GetErrorMessage(toDateProp, ToDate);
                //}
                //else if (columnName == endTimeProp && IsAppointmentVisible == Visibility.Visible)       //End time
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
                else if (columnName == selectedIndexOwnerProp)      //Owner
                {
                    return ActivityValidation.GetErrorMessage(selectedIndexOwnerProp, SelectedIndexOwner);
                }
                else if (columnName == customerAddressProp && IsAppointmentVisible == Visibility.Visible)   // Address
                {
                    return ActivityValidation.GetErrorMessage(customerAddressProp, ActivityAddress);
                }
                else if (columnName == attendeesCountProp && IsAppointmentVisible == Visibility.Visible)    //Attendies
                {
                    return ActivityValidation.GetErrorMessage(attendeesCountProp, AttendeesCount);
                }
                else if (columnName == dueDateProp && IsDueDateVisible == Visibility.Visible)               //Due Date
                {
                    return ActivityValidation.GetErrorMessage(dueDateProp, DueDate);
                }
                else if (columnName == selectedIndexTaskStatusProp && IsTaskVisible == Visibility.Visible)  //Status
                {
                    return ActivityValidation.GetErrorMessage(selectedIndexTaskStatusProp, SelectedIndexTaskStatus);
                }
                else if (columnName == selectedAccountActivityLinkedItemsCountProp && IsInternal == false && IsInformationVisible == Visibility.Collapsed)  //linked items.
                {
                    return ActivityValidation.GetErrorMessage(selectedAccountActivityLinkedItemsCountProp, SelectedAccountActivityLinkedItemsCount);
                }

                else if (columnName == editActivityErrorProp)  //linked items.
                {
                    return ActivityValidation.GetErrorMessage(editActivityErrorProp, EditActivityError);
                }



                //else if (columnName == selectedAccountActivityLinkedItemsCountProp && SelectedAccountActivityLinkedItemsCount > 0 && IsInternal == true)  //linked items.
                //{
                //    return ActivityValidation.GetErrorMessage(selectedAccountActivityLinkedItemsCountProp, SelectedAccountActivityLinkedItemsCount);
                //}

                return null;
            }
        }

        #endregion
    }


}
