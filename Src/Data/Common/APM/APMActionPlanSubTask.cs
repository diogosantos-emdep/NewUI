using Emdep.Geos.Data.Common.Hrm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Emdep.Geos.Data.Common.APM
{
    [DataContract]
    //[pallavi.kale][GEOS2-7002]
    public class APMActionPlanSubTask : ModelBase, IDisposable
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
        private string taskLastComment;
        private string employeeCode;
        private int idGender;
        private Int32 modifiedBy;
        private Responsible selectedDelegatedTo;
        private ObservableCollection<Responsible> delegatedToList;
        private DateTime? openDate;
        private int duration;
        private DateTime? originalDueDate;
        private DateTime? closeDate;
        private int changeCount;
        private DateTime? lastUpdated;      
        private string actionPlanCode;
        private string themeHTMLColor;
        private System.Drawing.Color taskTabColor;
        private System.Windows.Media.Brush taskTabBrush;
        private string location;
        private string businessUnitHTMLColor;
        private Int32 idActionPlanResponsible;
        private string cardDueColor;
        List<LogEntriesByActionPlan> actionPlanLogEntries;
        private bool isShowIcon;
        private Int32 closedBy;
        private string closedByName;
        private DateTime? createdIn;
        private string createdByName;
        private string taskOrigin;
        private Country country;
        private string actionPlanDescription;
        private Int32 idOrigin;
        private Int32 idBusinessUnit;
        private Int32 actionPlanResponsibleIdUser;
        private string taskResponsibleDisplayName;
        private string delegatedDisplayName;
        private string taskStatusComment;
        private string taskStatusDescription;
        private List<AttachmentsByTask> taskAttachmentList;
        private Int32 idOTItem; 
        private Int32 idSite;
        private string codeNumber; 
        private Int32 idActionPlanLocation; 
        private string numItem;
        private Int64 idOT;
        private string oTCode;
        private string customerName;
        private int participantCount;
        private string firstName;
        private bool isSelectedRow;
        private bool isSaved;
        private string priorityHTMLColor;
        private byte settingInUse;    
        private Int32 idAppSetting;      
        private string openTaskCount;
        private string responsibleWithTaskCount;
        private Int32 idEmployeeContact;
        private string employeeContactValue;
        List<APMActionPlanTask> emailTaskDetailsList;
        private Int64 idParent;
        private Int64 subTaskNumber;
        private Int64 parentTaskNumber;
        public string subTaskCode;
        private bool isSubTaskAdded;
        private bool isSubTaskDeleted; //[shweta.thube][GEOS2-8880]
        private string group; //[pallavi.kale][GEOS2-8084][05.08.2025]
        private string site; //[pallavi.kale][GEOS2-8084][05.08.2025]
        private string groupName; //[pallavi.kale][GEOS2-8084][05.08.2025]
        private string originWeek;//[pallavi.kale][GEOS2-8995][28.10.2025]
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
     
        [NotMapped]
        [DataMember]
        public string ActionPlanCode
        {
            get { return actionPlanCode; }
            set { actionPlanCode = value; OnPropertyChanged("ActionPlanCode"); }
        }

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

        [NotMapped]
        [DataMember]
        public string ActionPlanCodeWithTaskNumber
        {
            get { return ActionPlanCode + " - " + TaskNumber; }
            set { }
        }

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
    
        [NotMapped]
        [DataMember]
        public string EmployeeContactValue
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
        public List<APMActionPlanTask> EmailTaskDetailsList
        {
            get { return emailTaskDetailsList; }
            set
            {
                emailTaskDetailsList = value;
                OnPropertyChanged("EmailTaskDetailsList");
            }
        }
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

        [NotMapped]
        [DataMember]
        public Int64 SubTaskNumber
        {
            get { return subTaskNumber; }
            set
            {
                subTaskNumber = value;
                OnPropertyChanged("SubTaskNumber");
            }
        }
        [NotMapped]
        [DataMember]
        public Int64 ParentTaskNumber
        {
            get { return parentTaskNumber; }
            set
            {
                parentTaskNumber = value;
                OnPropertyChanged("ParentTaskNumber");
            }
        }

        [NotMapped]
        [DataMember]
        public string SubTaskCode
        {
            get { return subTaskCode; }
            set
            {
                subTaskCode = value;
                OnPropertyChanged("SubTaskCode");
            }
        }
        [NotMapped]
        [DataMember]
        public bool IsSubTaskAdded
        {
            get
            {
                return isSubTaskAdded;
            }
            set
            {
                isSubTaskAdded = value;
                OnPropertyChanged("IsSubTaskAdded");
            }
        }
       //[shweta.thube][GEOS2-8880]
        [NotMapped]
        [DataMember]
        public bool IsSubTaskDeleted
        {
            get
            {
                return isSubTaskDeleted;
            }
            set
            {
                isSubTaskDeleted = value;
                OnPropertyChanged("IsSubTaskDeleted");
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
        //[pallavi.kale][GEOS2-8084][05.08.2025]
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
        #endregion

        #region Constructor
        public APMActionPlanSubTask()
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
