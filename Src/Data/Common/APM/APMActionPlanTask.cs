using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.TechnicalRestService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Emdep.Geos.Data.Common.APM
{
    [DataContract]
    public class APMActionPlanTask : ModelBase, IDisposable
    {
        #region Declaration
        private Int64 idActionPlanTask;
        private Int32 taskNumber;
        private string title;
        private string description;
        private int idLookupStatus;
        private string status;
        private string statusHTMLColor;
        private Int32 idLookupPriority;
        private string priority;
        private Int32 idLookupOrigin;
        private string origin;
        private Int32 idLookupTheme;
        private string theme;
        private DateTime dueDate;
        private int dueDays;
        private Int32 idDelegated;
        private string delegatedTo;
        private int commentsCount;
        private string comments;
        private int fileCount;
        private int progress;
        private Int32 idEmployee;
        private string responsible;
        private Int32 idLookupBusiness;
        private string businessUnit;
        private string dueColor;
        private Int64 idActionPlan;
        private Int32 idCompany;
        private UInt32 createdBy;
        private string code;
        private string taskLastComment;//[Sudhir.Jangra][GEOS2-6397]
        private string employeeCode;//[Sudhir.Jangra][GEOS2-6397]
        private int idGender;//[Sudhir.Jangra][GEOS2-6397]
        private Int32 modifiedBy;//[Shweta.Thube[GEOS2-5980]
        private Responsible selectedDelegatedTo;//[Sudhir.Jangra][GEOS2-6017]
        private ObservableCollection<Responsible> delegatedToList;//[Sudhir.Jangra][GEOS2-6017]
        private DateTime? openDate;//[Shweta.Thube[GEOS2-5980]
        private int duration;//[Shweta.Thube[GEOS2-5980]
        private DateTime? originalDueDate;//[Shweta.Thube[GEOS2-5981]
        private DateTime? closeDate;//[Shweta.Thube[GEOS2-5981]
        private int changeCount;//[Shweta.Thube[GEOS2-5981]
        private DateTime? lastUpdated;//[Shweta.Thube[GEOS2-5981]        
        private string actionPlanCode;
        private string themeHTMLColor;//[Sudhir.Jangra][GEOS2-5982]
        private System.Drawing.Color taskTabColor;
        private System.Windows.Media.Brush taskTabBrush;
        private string location;//[Sudhir.Jangra][GEOS2-6538]
        private string businessUnitHTMLColor;
        private Int32 idActionPlanResponsible;//[GEOS2-6557][Sudhir.Jangra]
        private string cardDueColor;//[Sudhir.Jangra][GEOS2-6557]
        List<LogEntriesByActionPlan> actionPlanLogEntries;//[Shweta.Thube][GEOS2-6020]
        private bool isShowIcon;
        private Int32 closedBy;//[Sudhir.jangra][GEOS2-6594]
        private string closedByName;//[Sudhir.Jangra][GEOS2-6594]
        private DateTime? createdIn;//[Sudhir.Jangra][GEOS2-6594]
        private string createdByName;//[Sudhir.Jangra][GEOS2-6594]
        private string taskOrigin;//[Shweta.Thube[GEOS2-6453]
        private Country country;//[Shweta.Thube[GEOS2-6453]
        private string actionPlanDescription;
        private Int32 idOrigin;//[Sudhir.Jangra][GEOS2-6453]
        private Int32 idBusinessUnit;//[Sudhir.Jangra][GEOS2-6453]
        private Int32 actionPlanResponsibleIdUser;//[Sudhir.Jangra][GEOS2-6866]
        private string taskResponsibleDisplayName;//[Sudhir.Jangra][GEOS2-6897]
        private string delegatedDisplayName;//[Sudhir.jangra][GEOS2-6897]
        private string taskStatusComment;//[Sudhir.Jangra][GEOS2-7006]
        private string taskStatusDescription;//[Sudhir.jangra][GEOS2-7006]
        private List<AttachmentsByTask> taskAttachmentList;//[Sudhir.jangra][GEOS2-7006]
        private Int32 idOTItem; //[Shweta.Thube][GEOS2-6912]
        private Int32 idSite; //[Shweta.Thube][GEOS2-6912]
        private string codeNumber; //[Shweta.Thube][GEOS2-6912]
        private Int32 idActionPlanLocation; //[Shweta.Thube][GEOS2-6912]
        private string numItem;//[Shweta.Thube][GEOS2-6912]
        private Int64 idOT;//[Shweta.Thube][GEOS2-6912]
        private string oTCode;//[Shweta.Thube][GEOS2-6912]
        private string customerName;//[Shweta.Thube][GEOS2-6912]
        private int participantCount;//[Shweta.Thube][GEOS2-7008]
        private string firstName;//[Shweta.Thube][GEOS2-7238]
        private bool isSelectedRow;
        private bool isSaved;//[Shweta.Thube][GEOS2-7218]
        private string priorityHTMLColor;//[shweta.thube][GEOS2-8051][15/05/2025]
        private byte settingInUse;      //[Shweta.Thube][GEOS2-8061]
        private Int32 idAppSetting;      //[Shweta.Thube][GEOS2-8061]
        private string openTaskCount;//[shweta.thube][GEOS2-8063][27/05/2025]
        private string responsibleWithTaskCount;//[shweta.thube][GEOS2-8063][27/05/2025]
        private Int32 idEmployeeContact;
        private string employeeContactValue;
        List<APMActionPlanTask> emailTaskDetailsList;
        private bool isTaskAdded;//[Shweta.Thube][GEOS2-8394]
        private Int64 idParent;//[pallavi.kale][GEOS2-7002][19.06.2025]
        List<APMActionPlanSubTask> subTaskList;//[pallavi.kale][GEOS2-7002][19.06.2025]
        private Int32 responsibleIdUser;//[pallavi.kale][GEOS2-7003]
        private string groupName;//[pallavi.kale][GEOS2-7003]
        private bool isTaskDeleted; //[shweta.thube][GEOS2-8880]
        private string group; //[pallavi.kale][GEOS2-8084][05.08.2025]
        private string site; //[pallavi.kale][GEOS2-8084][05.08.2025]
        private Int32 totalTaskCount;//[shweta.thube][GEOS2-6047][03.09.2025]
        private Int32 openStatusTaskCount;//[shweta.thube][GEOS2-6047][03.09.2025]
        private Int32 closedTaskCount;//[shweta.thube][GEOS2-6047][03.09.2025]
        private DateTime? sentDateTime;
        private string companyEmail;
        private bool isPreviewEmail;
        private bool sentEmail;
        private string originWeek;//[pallavi.kale][GEOS2-8995][28.10.2025]
        private string taskNumberLable;//[Shweta.Thube][GEOS2-9618][29-10-2025]
        private string openSubtaskCount;//[Shweta.Thube][GEOS2-9618][29-10-2025]
        private System.Windows.Media.ImageSource responsibleImage; // Imagem do responsável
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdActionPlanTask
        {
            get { return idActionPlanTask; }
            set
            {
                idActionPlanTask = value;
                OnPropertyChanged("IdActionPlanTask");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 TaskNumber
        {
            get { return taskNumber; }
            set
            {
                taskNumber = value;
                OnPropertyChanged("TaskNumber");
            }
        }

        [NotMapped]
        [DataMember]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        [NotMapped]
        public System.Windows.Media.ImageSource ResponsibleImage
        {
            get { return responsibleImage; }
            set
            {
                responsibleImage = value;
                OnPropertyChanged("ResponsibleImage");
            }
        }

        [NotMapped]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdLookupStatus
        {
            get { return idLookupStatus; }
            set
            {
                idLookupStatus = value;
                OnPropertyChanged("IdLookupStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdLookupPriority
        {
            get { return idLookupPriority; }
            set
            {
                idLookupPriority = value;
                OnPropertyChanged("IdLookupPriority");
            }
        }

        [NotMapped]
        [DataMember]
        public string Priority
        {
            get { return priority; }
            set
            {
                priority = value;
                OnPropertyChanged("Priority");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdLookupOrigin
        {
            get { return idLookupOrigin; }
            set
            {
                idLookupOrigin = value;
                OnPropertyChanged("IdLookupOrigin");
            }
        }

        [NotMapped]
        [DataMember]
        public string Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                OnPropertyChanged("Origin");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdLookupTheme
        {
            get { return idLookupTheme; }
            set
            {
                idLookupTheme = value;
                OnPropertyChanged("IdLookupTheme");
            }
        }

        [NotMapped]
        [DataMember]
        public string Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                OnPropertyChanged("Theme");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                OnPropertyChanged("DueDate");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IdDelegated
        {
            get { return idDelegated; }
            set
            {
                idDelegated = value;
                OnPropertyChanged("IdDelegated");
            }
        }


        [NotMapped]
        [DataMember]
        public string DelegatedTo
        {
            get { return delegatedTo; }
            set
            {
                delegatedTo = value;
                OnPropertyChanged("DelegatedTo");
            }
        }

        [NotMapped]
        [DataMember]
        public int CommentsCount
        {
            get { return commentsCount; }
            set
            {
                commentsCount = value;
                OnPropertyChanged("CommentsCount");
            }
        }

        [NotMapped]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [NotMapped]
        [DataMember]
        public int FileCount
        {
            get { return fileCount; }
            set
            {
                fileCount = value;
                OnPropertyChanged("FileCount");
            }
        }

        [NotMapped]
        [DataMember]
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [NotMapped]
        [DataMember]
        public string Responsible
        {
            get { return responsible; }
            set
            {
                responsible = value;
                OnPropertyChanged("Responsible");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdLookupBusiness
        {
            get { return idLookupBusiness; }
            set
            {
                idLookupBusiness = value;
                OnPropertyChanged("IdLookupBusiness");
            }
        }

        [NotMapped]
        [DataMember]
        public string BusinessUnit
        {
            get { return businessUnit; }
            set
            {
                businessUnit = value;
                OnPropertyChanged("BusinessUnit");
            }
        }

        [NotMapped]
        [DataMember]
        public int DueDays
        {
            get { return dueDays; }
            set
            {
                dueDays = value;
                OnPropertyChanged("DueDays");
            }
        }

        [NotMapped]
        [DataMember]
        public string DueColor
        {
            get { return dueColor; }
            set
            {
                dueColor = value;
                OnPropertyChanged("DueColor");
            }
        }

        [NotMapped]
        [DataMember]
        public string StatusHTMLColor
        {
            get { return statusHTMLColor; }
            set
            {
                statusHTMLColor = value;
                OnPropertyChanged("StatusHTMLColor");
            }
        }
        [NotMapped]
        [DataMember]
        public Int64 IdActionPlan
        {
            get { return idActionPlan; }
            set
            {
                idActionPlan = value;
                OnPropertyChanged("IdActionPlan");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }
        [NotMapped]
        [DataMember]
        public UInt32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }
        [NotMapped]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        //[Sudhir.Jangra][GEOS2-6397]
        [NotMapped]
        [DataMember]
        public string TaskLastComment
        {
            get { return taskLastComment; }
            set
            {
                taskLastComment = value;
                OnPropertyChanged("TaskLastComment");
            }
        }

        //[Sudhir.Jangra][GEOS2-6397]
        [NotMapped]
        [DataMember]
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }

        //[Sudhir.Jangra][GEOS2-6397]
        [NotMapped]
        [DataMember]
        public int IdGender
        {
            get { return idGender; }
            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
            }
        }
        //[Shweta.Thube[GEOS2-5980]
        [NotMapped]
        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        //[Sudhir.Jangra][GEOS2-6017]
        [NotMapped]
        [DataMember]
        public Responsible SelectedDelegatedTo
        {
            get { return selectedDelegatedTo; }
            set
            {
                selectedDelegatedTo = value;
                OnPropertyChanged("SelectedDelegatedTo");
            }
        }

        //[Sudhir.Jangra][GEOS2-6017]
        [NotMapped]
        [DataMember]
        public ObservableCollection<Responsible> DelegatedToList
        {
            get { return delegatedToList; }
            set
            {
                delegatedToList = value;
                OnPropertyChanged("DelegatedToList");
            }
        }
        //[Shweta.Thube[GEOS2-5981]
        [NotMapped]
        [DataMember]
        public DateTime? OpenDate
        {
            get { return openDate; }
            set
            {
                openDate = value;
                OnPropertyChanged("OpenDate");
            }
        }

        //[Shweta.Thube[GEOS2-5981]
        [NotMapped]
        [DataMember]
        public DateTime? OriginalDueDate
        {
            get { return originalDueDate; }
            set
            {
                originalDueDate = value;
                OnPropertyChanged("OriginalDueDate");
            }
        }
        //[Shweta.Thube[GEOS2-5981]
        [NotMapped]
        [DataMember]
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }
        //[Shweta.Thube[GEOS2-5981]
        [NotMapped]
        [DataMember]
        public DateTime? CloseDate
        {
            get { return closeDate; }
            set
            {
                closeDate = value;
                OnPropertyChanged("CloseDate");
            }
        }

        //[Shweta.Thube[GEOS2-5981]
        [NotMapped]
        [DataMember]
        public int ChangeCount
        {
            get { return changeCount; }
            set
            {
                changeCount = value;
                OnPropertyChanged("ChangeCount");
            }
        }
        //[Shweta.Thube[GEOS2-5981]
        [NotMapped]
        [DataMember]
        public DateTime? LastUpdated
        {
            get { return lastUpdated; }
            set
            {
                lastUpdated = value;
                OnPropertyChanged("LastUpdated");
            }
        }
        // [nsatpute][09-10-2024][GEOS2-5975]
        [NotMapped]
        [DataMember]
        public string ActionPlanCode
        {
            get { return actionPlanCode; }
            set { actionPlanCode = value; OnPropertyChanged("ActionPlanCode"); }
        }

        //[Sudhir.Jangra][GEOS2-5982]
        [NotMapped]
        [DataMember]
        public string ThemeHTMLColor
        {
            get { return themeHTMLColor; }
            set
            {
                themeHTMLColor = value;
                OnPropertyChanged("ThemeHTMLColor");
            }
        }

        [NotMapped]
        [DataMember]
        public System.Drawing.Color TaskTabColor
        {
            get { return taskTabColor; }
            set { taskTabColor = value; OnPropertyChanged("TaskTabColor"); }
        }

        [NotMapped]
        [DataMember]
        public System.Windows.Media.Brush TaskTabBrush
        {
            get { return taskTabBrush; }
            set { taskTabBrush = value; OnPropertyChanged("TaskTabBrush"); }
        }

        //[Sudhir.jangra][GEOS2-6538]
        [NotMapped]
        [DataMember]
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        [NotMapped]
        [DataMember]
        public string BusinessUnitHTMLColor
        {
            get { return businessUnitHTMLColor; }
            set
            {
                businessUnitHTMLColor = value;
                OnPropertyChanged("BusinessUnitHTMLColor");
            }
        }

        //[Sudhir.Jangra][GEOS2-5976]
        [NotMapped]
        [DataMember]
        public string ActionPlanCodeWithTaskNumber
        {
            get { return ActionPlanCode + " - " + TaskNumber; }
            set { }
        }

        //[Sudhir.Jangra][GEOS2-6557]
        [NotMapped]
        [DataMember]
        public Int32 IdActionPlanResponsible
        {
            get { return idActionPlanResponsible; }
            set
            {
                idActionPlanResponsible = value;
                OnPropertyChanged("IdActionPlanResponsible");
            }
        }


        //[Sudhir.Jangra][GEOS2-6557]
        [NotMapped]
        [DataMember]
        public string CardDueColor
        {
            get { return cardDueColor; }
            set
            {
                cardDueColor = value;
                OnPropertyChanged("CardDueColor");
            }
        }

        //[Shweta.Thube][GEOS2-6020]
        [NotMapped]
        [DataMember]
        public List<LogEntriesByActionPlan> ActionPlanLogEntries
        {
            get
            {
                return actionPlanLogEntries;
            }

            set
            {
                actionPlanLogEntries = value;
                OnPropertyChanged("ActionPlanLogEntries");
            }
        }
        [NotMapped]
        [DataMember]
        public bool IsShowIcon
        {
            get { return isShowIcon; }
            set
            {
                isShowIcon = value;
                OnPropertyChanged("IsShowIcon");
            }
        }

        //[Sudhir.jangra][GEOS2-6594]
        [NotMapped]
        [DataMember]
        public Int32 ClosedBy
        {
            get { return closedBy; }
            set
            {
                closedBy = value;
                OnPropertyChanged("ClosedBy");
            }
        }

        //[Sudhir.jangra][GEOS2-6594]
        [NotMapped]
        [DataMember]
        public string ClosedByName
        {
            get { return closedByName; }
            set
            {
                closedByName = value;
                OnPropertyChanged("ClosedByName");
            }
        }

        //[Sudhir.jangra][GEOS2-6594]
        [NotMapped]
        [DataMember]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        //[Sudhir.jangra][GEOS2-6594]
        [NotMapped]
        [DataMember]
        public string CreatedByName
        {
            get { return createdByName; }
            set
            {
                createdByName = value;
                OnPropertyChanged("CreatedByName");
            }
        }
        //[Shweta.Thube[GEOS2-6453]
        [NotMapped]
        [DataMember]
        public string TaskOrigin
        {
            get { return taskOrigin; }
            set
            {
                taskOrigin = value;
                OnPropertyChanged("taskOrigin");
            }
        }
        //[Shweta.Thube[GEOS2-6453]
        [NotMapped]
        [DataMember]
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }
        //[Shweta.Thube[GEOS2-6453]
        [NotMapped]
        [DataMember]
        public string ActionPlanDescription
        {
            get { return actionPlanDescription; }
            set
            {
                actionPlanDescription = value;
                OnPropertyChanged("ActionPlanDescription");
            }
        }

        //[Sudhir.Jangra][GEOS2-6453]
        [NotMapped]
        [DataMember]
        public Int32 IdOrigin
        {
            get { return idOrigin; }
            set
            {
                idOrigin = value;
                OnPropertyChanged("IdOrigin");
            }
        }
        //[Sudhir.Jangra][GEOS2-6453]
        [NotMapped]
        [DataMember]
        public Int32 IdBusinessUnit
        {
            get { return idBusinessUnit; }
            set
            {
                idBusinessUnit = value;
                OnPropertyChanged("IdBusinessUnit");
            }
        }

        //[Sudhir.jangra][GEOS2-6866]
        [NotMapped]
        [DataMember]
        public Int32 ActionPlanResponsibleIdUser
        {
            get { return actionPlanResponsibleIdUser; }
            set
            {
                actionPlanResponsibleIdUser = value;
                OnPropertyChanged("ActionPlanResponsibleIdUser");
            }
        }

        //[Sudhir.jangra][GEOS2-6897]
        [NotMapped]
        [DataMember]
        public string TaskResponsibleDisplayName
        {
            get { return taskResponsibleDisplayName; }
            set
            {
                taskResponsibleDisplayName = value;
                OnPropertyChanged("TaskResponsibleDisplayName");
            }
        }

        //[Sudhir.jangra][GEOS2-6897]
        [NotMapped]
        [DataMember]
        public string DelegatedDisplayName
        {
            get { return delegatedDisplayName; }
            set
            {
                delegatedDisplayName = value;
                OnPropertyChanged("DelegatedDisplayName");
            }
        }

        //[Sudhir.Jangra][GEOS2-7006]
        [NotMapped]
        [DataMember]
        public string TaskStatusComment
        {
            get { return taskStatusComment; }
            set
            {
                taskStatusComment = value;
                OnPropertyChanged("TaskStatusComment");
            }
        }

        //[Sudhir.Jangra][GEOS2-7006]
        [NotMapped]
        [DataMember]
        public string TaskStatusDescription
        {
            get { return taskStatusDescription; }
            set
            {
                taskStatusDescription = value;
                OnPropertyChanged("TaskStatusDescription");
            }
        }

        //[Sudhir.Jangra][GEOS2-7006]
        [NotMapped]
        [DataMember]
        public List<AttachmentsByTask> TaskAttachmentList
        {
            get { return taskAttachmentList; }
            set
            {
                taskAttachmentList = value;
                OnPropertyChanged("TaskAttachmentList");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IdOTItem
        {
            get { return idOTItem; }
            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }
        //[Shweta.Thube][GEOS2-6912]
        [NotMapped]
        [DataMember]
        public string CodeNumber
        {
            get { return codeNumber; }
            set
            {
                codeNumber = value;
                OnPropertyChanged("CodeNumber");
            }
        }
        //[Shweta.Thube][GEOS2-6912]
        [NotMapped]
        [DataMember]
        public Int32 IdActionPlanLocation
        {
            get { return idActionPlanLocation; }
            set
            {
                idActionPlanLocation = value;
                OnPropertyChanged("IdActionPlanLocation");
            }
        }
        //[Shweta.Thube][GEOS2-6912]
        [NotMapped]
        [DataMember]
        public string NumItem
        {
            get { return numItem; }
            set
            {
                numItem = value;
                OnPropertyChanged("NumItem");
            }
        }
        //[Shweta.Thube][GEOS2-6912]
        [NotMapped]
        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        //[Shweta.Thube][GEOS2-6912]
        [NotMapped]
        [DataMember]
        public string OTCode
        {
            get { return oTCode; }
            set
            {
                oTCode = value;
                OnPropertyChanged("OTCode");
            }
        }
        //[Shweta.Thube][GEOS2-6912]
        [NotMapped]
        [DataMember]
        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }
        //[Shweta.Thube][GEOS2-7008]
        [NotMapped]
        [DataMember]
        public int ParticipantCount
        {
            get { return participantCount; }
            set
            {
                participantCount = value;
                OnPropertyChanged("ParticipantCount");
            }
        }
        //[Shweta.Thube][GEOS2-7238]
        [NotMapped]
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsSelectedRow
        {
            get { return isSelectedRow; }
            set
            {
                isSelectedRow = value;
                OnPropertyChanged("IsSelectedRow");
            }
        }

        //[Shweta.Thube][GEOS2-7218]
        [NotMapped]
        [DataMember]
        public bool IsSaved
        {
            get
            {
                return isSaved;
            }
            set
            {
                isSaved = value;
                OnPropertyChanged("IsSaved");
            }
        }
        //[shweta.thube][GEOS2-8051][15/05/2025]
        [NotMapped]
        [DataMember]
        public string PriorityHTMLColor
        {
            get { return priorityHTMLColor; }
            set
            {
                priorityHTMLColor = value;
                OnPropertyChanged("PriorityHTMLColor");
            }
        }
        //[Shweta.Thube][GEOS2-8061]
        [NotMapped]
        [DataMember]
        public byte SettingInUse
        {
            get { return settingInUse; }
            set
            {
                settingInUse = value;
                OnPropertyChanged("SettingInUse");
            }
        }
        //[Shweta.Thube][GEOS2-8061]
        [NotMapped]
        [DataMember]
        public Int32 IdAppSetting
        {
            get { return idAppSetting; }
            set
            {
                idAppSetting = value;
                OnPropertyChanged("IdAppSetting");
            }
        }

        //[shweta.thube][GEOS2-8063][27/05/2025]
        [NotMapped]
        [DataMember]
        public string OpenTaskCount
        {
            get { return openTaskCount; }
            set
            {
                openTaskCount = value;
                OnPropertyChanged("OpenTaskCount");
            }
        }
        //[shweta.thube][GEOS2-8063][27/05/2025]
        [NotMapped]
        [DataMember]
        public string ResponsibleWithTaskCount
        {
            get { return responsibleWithTaskCount; }
            set
            {
                responsibleWithTaskCount = value;
                OnPropertyChanged("ResponsibleWithTaskCount");
            }
        }
        //[shweta.thube][GEOS2-8066]
        [NotMapped]
        [DataMember]
        public Int32 IdEmployeeContact
        {
            get { return idEmployeeContact; }
            set
            {
                idEmployeeContact = value;
                OnPropertyChanged("IdEmployeeContact");
            }
        }
        //[shweta.thube][GEOS2-8066]
        [NotMapped]
        [DataMember]
        public string EmployeeContactValuef
        {
            get { return employeeContactValue; }
            set
            {
                employeeContactValue = value;
                OnPropertyChanged("EmployeeContactValue");
            }
        }
        [NotMapped]
        [DataMember]
        public List<APMActionPlanTask> SubTasks { get; set; } = new List<APMActionPlanTask>();

        [NotMapped]
        [DataMember]
        public string EmployeeContactValue { get; set; }
        //[shweta.thube][GEOS2-8066]
        [NotMapped]
        [DataMember]
        public List<APMActionPlanTask> EmailTaskDetailsList
        {
            get { return emailTaskDetailsList; }
            set
            {
                emailTaskDetailsList = value;
                OnPropertyChanged("EmailTaskDetailsList");
            }
        }
        //[Shweta.Thube][GEOS2-8394]
        [NotMapped]
        [DataMember]
        public bool IsTaskAdded
        {
            get
            {
                return isTaskAdded;
            }
            set
            {
                isTaskAdded = value;
                OnPropertyChanged("IsTaskAdded");
            }
        }

        //[pallavi.kale][GEOS2-7002]
        [NotMapped]
        [DataMember]
        public Int64 IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }
        //[pallavi.kale][GEOS2-7002]
        [NotMapped]
        [DataMember]
        public List<APMActionPlanSubTask> SubTaskList
        {
            get { return subTaskList; }
            set
            {
                subTaskList = value;
                OnPropertyChanged("SubTaskList");
            }
        }

        //[pallavi.kale][GEOS2-7002]
        [NotMapped]
        [DataMember]
        public Int32 ResponsibleIdUser
        {
            get { return responsibleIdUser; }
            set
            {
                responsibleIdUser = value;
                OnPropertyChanged("ResponsibleIdUser");
            }
        }
        //[pallavi.kale][GEOS2-7002]
        [NotMapped]
        [DataMember]
        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }
        //[shweta.thube][GEOS2-8880]
        [NotMapped]
        [DataMember]
        public bool IsTaskDeleted
        {
            get
            {
                return isTaskDeleted;
            }
            set
            {
                isTaskDeleted = value;
                OnPropertyChanged("IsTaskDeleted");
            }
        }

        //[pallavi.kale][GEOS2-8084][05.08.2025]
        [NotMapped]
        [DataMember]
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }
        //[pallavi.kale][GEOS2-8084][05.08.2025]
        [NotMapped]
        [DataMember]
        public string Site
        {
            get { return site; }
            set
            {
                site = value;
                OnPropertyChanged("Site");
            }
        }
        //[shweta.thube][GEOS2-6047][03.09.2025]
        [NotMapped]
        [DataMember]
        public Int32 TotalTaskCount
        {
            get { return totalTaskCount; }
            set
            {
                totalTaskCount = value;
                OnPropertyChanged("TotalTaskCount");
            }
        }
        //[shweta.thube][GEOS2-6047][03.09.2025]
        [NotMapped]
        [DataMember]
        public Int32 OpenStatusTaskCount
        {
            get { return openStatusTaskCount; }
            set
            {
                openStatusTaskCount = value;
                OnPropertyChanged("OpenStatusTaskCount");
            }
        }
        //[shweta.thube][GEOS2-6047][03.09.2025]
        [NotMapped]
        [DataMember]
        public Int32 ClosedTaskCount
        {
            get { return closedTaskCount; }
            set
            {
                closedTaskCount = value;
                OnPropertyChanged("ClosedTaskCount");
            }
        }
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        [NotMapped]
        [DataMember]
        public DateTime? SentDateTime
        {
            get { return sentDateTime; }
            set
            {
                sentDateTime = value;
                OnPropertyChanged("SentDateTime");
            }
        }
        [NotMapped]
        [DataMember]
        public string CompanyEmail
        {
            get { return companyEmail; }
            set
            {
                companyEmail = value;
                OnPropertyChanged("CompanyEmail");
            }
        }
        [NotMapped]
        [DataMember]
        public bool IsPreviewEmail
        {
            get { return isPreviewEmail; }
            set
            {
                isPreviewEmail = value;
                OnPropertyChanged("IsPreviewEmail");
            }
        }
        //[Shweta.Thube][09-10-2025][GEOS2-9131]
        [NotMapped]
        [DataMember]
        public bool SentEmail
        {
            get { return sentEmail; }
            set
            {
                sentEmail = value;
                OnPropertyChanged("SentEmail");
            }
        }

        //[pallavi.kale][GEOS2-8995][28.10.2025]
        [NotMapped]
        [DataMember]
        public string OriginWeek
        {
            get { return originWeek; }
            set
            {
                originWeek = value;
                OnPropertyChanged("OriginWeek");
            }
        }
        //[Shweta.Thube][GEOS2-9618][29-10-2025]
        [NotMapped]
        [DataMember]
        public string TaskNumberLable
        {
            get { return taskNumberLable; }
            set
            {
                taskNumberLable = value;
                OnPropertyChanged("TaskNumberLable");
            }
        }
        //[Shweta.Thube][GEOS2-9618][29-10-2025]
        [NotMapped]
        [DataMember]
        public string OpenSubtaskCount
        {
            get { return openSubtaskCount; }
            set
            {
                openSubtaskCount = value;
                OnPropertyChanged("OpenSubtaskCount");
            }
        }
        #endregion

        #region Constructor
        public APMActionPlanTask()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
