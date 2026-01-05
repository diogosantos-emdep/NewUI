using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common.Glpi
{
    [Table("glpi_users")]
    [DataContract]
    public class GlpiUser
    {
        #region Fields
        Int32 id;
        Int32? locationsId;
        Int32? useMode;
        SByte? isActive;
        Int32? authsId;
        Int32? authType;
        SByte? isDeleted;
        Int32? profilesId;
        Int32? entitiesId;
        Int32? userTitlesId;
        Int32? userCategoriesId;
        SByte? isDeletedLdap;
        string name;
        string password;
        string phone;
        string phone2;
        string mobile;
        string realName;
        string firstName;
        char language;
        Int32? listLimit;
        string comment;
        DateTime? lastLogin;
        DateTime? dateMod;
        DateTime? dateSync;
        Int32? dateFormat;
        Int32? numberFormat;
        Int32? namesFormat;
        string csvDelimiter;
        SByte? isIdsVisible;
        Int32? dropdownCharsLimit;
        SByte? useFlatDropdowntree;
        SByte? showJobsAtLogin;
        string priority1;
        string priority2;
        string priority3;
        string priority4;
        string priority5;
        string priority6;
        SByte? followUpPrivate;
        SByte? taskPrivate;
        Int32? defaultRequestTypesId;
        string passwordForgetToken;
        DateTime? passwordForgetTokenDate;
        string userDn;
        string registrationNumber;
        SByte? showCountOnTabs;
        Int32? refreshTicketList;
        SByte? setDefaultTech;
        string personalToken;
        DateTime? personalTokenDate;
        Int32? displayCountOnHome;
        SByte? notificationToMyself;
        string dueDateOkColor;
        string dueDateWarningColor;
        string dueDateCriticalColor;
        Int32? dueDateWarningLess;
        Int32? dueDateCriticalLess;
        string dueDateWarningUnit;
        string dueDateCriticalUnit;
        string displayOptions;
        string pdfFont;
        string picture;
        DateTime? beginDate;
        DateTime? endDate;
        SByte? keepDevicesWhenPurgingItem;
        string privateBookmarkOrder;
        SByte? backCreated;
        Int32? taskState;
        SByte? ticketTimelineKeepReplacedTabs;
        SByte? ticketTimeline;
        char palette;
        char layout;
        #endregion

        #region Properties

        [Key]
        [Column("id")]
        [DataMember]
        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        [Column("locations_id")]
        [DataMember]
        public Int32? LocationsId
        {
            get { return locationsId; }
            set { locationsId = value; }
        }

        [Column("use_mode")]
        [DataMember]
        public Int32? UseMode
        {
            get { return useMode; }
            set { useMode = value; }
        }

        [Column("is_active")]
        [DataMember]
        public SByte? IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        [Column("auths_id")]
        [DataMember]
        public Int32? AuthsId
        {
            get { return authsId; }
            set { authsId = value; }
        }

        [Column("authtype")]
        [DataMember]
        public Int32? AuthType
        {
            get { return authType; }
            set { authType = value; }
        }

        [Column("is_deleted")]
        [DataMember]
        public SByte? IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        [Column("profiles_id")]
        [DataMember]
        public Int32? ProfilesId
        {
            get { return profilesId; }
            set { profilesId = value; }
        }

        [Column("entities_id")]
        [DataMember]
        public Int32? EntitiesId
        {
            get { return entitiesId; }
            set { entitiesId = value; }
        }

        [Column("usertitles_id")]
        [DataMember]
        public Int32? UserTitlesId
        {
            get { return userTitlesId; }
            set { userTitlesId = value; }
        }

        [Column("usercategories_id")]
        [DataMember]
        public Int32? UserCategoriesId
        {
            get { return userCategoriesId; }
            set { userCategoriesId = value; }
        }

        [Column("is_deleted_ldap")]
        [DataMember]
        public SByte? IsDeletedLdap
        {
            get { return isDeletedLdap; }
            set { isDeletedLdap = value; }
        }

        [Column("name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("password")]
        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [Column("phone")]
        [DataMember]
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        [Column("phone2")]
        [DataMember]
        public string Phone2
        {
            get { return phone2; }
            set { phone2 = value; }
        }

        [Column("mobile")]
        [DataMember]
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        [Column("realname")]
        [DataMember]
        public string RealName
        {
            get { return realName; }
            set { realName = value; }
        }

        [Column("firstname")]
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [Column("language")]
        [DataMember]
        public char Language
        {
            get { return language; }
            set { language = value; }
        }

        [Column("list_limit")]
        [DataMember]
        public Int32? ListLimit
        {
            get { return listLimit; }
            set { listLimit = value; }
        }

        [Column("comment")]
        [DataMember]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [Column("last_login")]
        [DataMember]
        public DateTime? LastLogin
        {
            get { return lastLogin; }
            set { lastLogin = value; }
        }

        [Column("date_mod")]
        [DataMember]
        public DateTime? DateMod
        {
            get { return dateMod; }
            set { dateMod = value; }
        }

        [Column("date_sync")]
        [DataMember]
        public DateTime? DateSync
        {
            get { return dateSync; }
            set { dateSync = value; }
        }

        [Column("date_format")]
        [DataMember]
        public Int32? DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        [Column("number_format")]
        [DataMember]
        public Int32? NumberFormat
        {
            get { return numberFormat; }
            set { numberFormat = value; }
        }

        [Column("names_format")]
        [DataMember]
        public Int32? NamesFormat
        {
            get { return namesFormat; }
            set { namesFormat = value; }
        }

        [Column("csv_delimiter")]
        [DataMember]
        public string CsvDelimiter
        {
            get { return csvDelimiter; }
            set { csvDelimiter = value; }
        }

        [Column("is_ids_visible")]
        [DataMember]
        public SByte? IsIdsVisible
        {
            get { return isIdsVisible; }
            set { isIdsVisible = value; }
        }

        [Column("use_flat_dropdowntree")]
        [DataMember]
        public SByte? UseFlatDropdowntree
        {
            get { return useFlatDropdowntree; }
            set { useFlatDropdowntree = value; }
        }

        [Column("show_jobs_at_login")]
        [DataMember]
        public SByte? ShowJobsAtLogin
        {
            get { return showJobsAtLogin; }
            set { showJobsAtLogin = value; }
        }

        [Column("priority_1")]
        [DataMember]
        public string Priority1
        {
            get { return priority1; }
            set { priority1 = value; }
        }

        [Column("priority_2")]
        [DataMember]
        public string Priority2
        {
            get { return priority2; }
            set { priority2 = value; }
        }

        [Column("priority_3")]
        [DataMember]
        public string Priority3
        {
            get { return priority3; }
            set { priority3= value; }
        }

        [Column("priority_4")]
        [DataMember]
        public string Priority4
        {
            get { return priority4; }
            set { priority4 = value; }
        }

        [Column("priority_5")]
        [DataMember]
        public string Priority5
        {
            get { return priority5; }
            set { priority5 = value; }
        }

        [Column("priority_6")]
        [DataMember]
        public string Priority6
        {
            get { return priority6; }
            set { priority6 = value; }
        }

        [Column("followup_private")]
        [DataMember]
        public SByte? FollowUpPrivate
        {
            get { return followUpPrivate; }
            set { followUpPrivate = value; }
        }

        [Column("task_private")]
        [DataMember]
        public SByte? TaskPrivate
        {
            get { return taskPrivate; }
            set { taskPrivate = value; }
        }

        [Column("default_requesttypes_id")]
        [DataMember]
        public Int32? DefaultRequestTypesId
        {
            get { return defaultRequestTypesId; }
            set { defaultRequestTypesId = value; }
        }

        [Column("password_forget_token")]
        [DataMember]
        public string PasswordForgetToken
        {
            get { return passwordForgetToken; }
            set { passwordForgetToken = value; }
        }

        [Column("password_forget_token_date")]
        [DataMember]
        public DateTime? PasswordForgetTokenDate
        {
            get { return passwordForgetTokenDate; }
            set { passwordForgetTokenDate = value; }
        }

        [Column("user_dn")]
        [DataMember]
        public string UserDn
        {
            get { return userDn; }
            set { userDn = value; }
        }

        [Column("registration_number")]
        [DataMember]
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { registrationNumber = value; }
        }

        [Column("show_count_on_tabs")]
        [DataMember]
        public SByte? ShowCountOnTabs
        {
            get { return showCountOnTabs; }
            set { showCountOnTabs = value; }
        }

        [Column("refresh_ticket_list")]
        [DataMember]
        public Int32? RefreshTicketList
        {
            get { return refreshTicketList; }
            set { refreshTicketList = value; }
        }

        [Column("set_default_tech")]
        [DataMember]
        public SByte? SetDefaultTech
        {
            get { return setDefaultTech; }
            set { setDefaultTech = value; }
        }

        [Column("personal_token")]
        [DataMember]
        public string PersonalToken
        {
            get { return personalToken; }
            set { personalToken = value; }
        }

        [Column("personal_token_date")]
        [DataMember]
        public DateTime? PersonalTokenDate
        {
            get { return personalTokenDate; }
            set { personalTokenDate = value; }
        }

        [Column("display_count_on_home")]
        [DataMember]
        public Int32? DisplayCountOnHome
        {
            get { return displayCountOnHome; }
            set { displayCountOnHome = value; }
        }

        [Column("notification_to_myself")]
        [DataMember]
        public SByte? NotificationToMyself
        {
            get { return notificationToMyself; }
            set { notificationToMyself = value; }
        }

        [Column("duedateok_color")]
        [DataMember]
        public string DueDateOkColor
        {
            get { return dueDateOkColor; }
            set { dueDateOkColor = value; }
        }

        [Column("duedatewarning_color")]
        [DataMember]
        public string DueDateWarningColor
        {
            get { return dueDateWarningColor; }
            set { dueDateWarningColor = value; }
        }

        [Column("duedatecritical_color")]
        [DataMember]
        public string DueDateCriticalColor
        {
            get { return dueDateCriticalColor; }
            set { dueDateCriticalColor = value; }
        }

        [Column("duedatewarning_less")]
        [DataMember]
        public Int32? DueDateWarningLess
        {
            get { return dueDateWarningLess; }
            set { dueDateWarningLess = value; }
        }

        [Column("duedatecritical_less")]
        [DataMember]
        public Int32? DueDateCriticalLess
        {
            get { return dueDateCriticalLess; }
            set { dueDateCriticalLess = value; }
        }

        [Column("duedatewarning_unit")]
        [DataMember]
        public string DueDateWarningUnit
        {
            get { return dueDateWarningUnit; }
            set { dueDateWarningUnit = value; }
        }

        [Column("duedatecritical_unit")]
        [DataMember]
        public string DueDateCriticalUnit
        {
            get { return dueDateCriticalUnit; }
            set { dueDateCriticalUnit = value; }
        }

        [Column("display_options")]
        [DataMember]
        public string DisplayOptions
        {
            get { return displayOptions; }
            set { displayOptions = value; }
        }

        [Column("pdffont")]
        [DataMember]
        public string PdfFont
        {
            get { return pdfFont; }
            set { pdfFont = value; }
        }

        [Column("picture")]
        [DataMember]
        public string Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        [Column("begin_date")]
        [DataMember]
        public DateTime? BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }

        [Column("end_date")]
        [DataMember]
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        [Column("keep_devices_when_purging_item")]
        [DataMember]
        public SByte? keep_devices_when_purging_item
        {
            get { return keepDevicesWhenPurgingItem; }
            set { keepDevicesWhenPurgingItem = value; }
        }

        [Column("privatebookmarkorder")]
        [DataMember]
        public string PrivateBookmarkOrder
        {
            get { return privateBookmarkOrder; }
            set { privateBookmarkOrder = value; }
        }

        [Column("backcreated")]
        [DataMember]
        public SByte? BackCreated
        {
            get { return backCreated; }
            set { backCreated = value; }
        }

        [Column("task_state")]
        [DataMember]
        public Int32? TaskState
        {
            get { return taskState; }
            set { taskState = value; }
        }

        [Column("layout")]
        [DataMember]
        public char Layout
        {
            get { return layout; }
            set { layout = value; }
        }

        [Column("palette")]
        [DataMember]
        public char Palette
        {
            get { return palette; }
            set { palette = value; }
        }

        [Column("ticket_timeline")]
        [DataMember]
        public SByte? TicketTimeline
        {
            get { return ticketTimeline; }
            set { ticketTimeline = value; }
        }

        [Column("ticket_timeline_keep_replaced_tabs")]
        [DataMember]
        public SByte? TicketTimelineKeepReplacedTabs
        {
            get { return ticketTimelineKeepReplacedTabs; }
            set { ticketTimelineKeepReplacedTabs = value; }
        }
        #endregion
    }
}
