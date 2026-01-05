using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    [Table("activities")]
    [DataContract]
    public class Activity : ModelBase, IDisposable
    {
        #region Fields
        Int64 idActivity;
        Int32 idActivityType;
        string subject;
        string description;
        DateTime? fromDate;
        DateTime? toDate;
        string location;
        Int32 idOwner;
        DateTime? createdIn;
        Int32 createdBy;
        DateTime? modifiedIn;
        Int32 modifiedBy;
        LookupValue lookupValue;
        People people;
        List<ActivityAttendees> activityAttendees;
        string activityAttendeesString;
        List<ActivityWatcher> activityWatchers;
        List<LogEntriesByActivity> logEntriesByActivity;
        List<LogEntriesByActivity> commentsByActivity;
        string bothLongitudeLatitude;
        Double? latitude;
        Double? longitude;
        List<ActivityLinkedItem> activityLinkedItem;
        List<ActivityAttachment> activityAttachment;
        Int32? idActivityStatus;
        byte? isCompleted;
        LookupValue activityStatus;
        Int64? plannedAppointment;
        Int64? actualAppointment;
        string guidString;
        string reportGroup;
        byte? isDeleted;
        string activityGridStatus;
        DateTime? closeDate;
        List<ActivityTag> activityTags;
        string activityTagsString;
        Notification notification;
        ActivityMail activityMail;
        DateTime? activityReminderDateTime;
        byte isSentMail;
        byte isInternal;
        DateTime? minDueDate;
        DateTime? maxDueDate;

        #region chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        List<ActionsLinkedItems> actionsLinkedItems;
        Int64 idActionPlanItem;
        DateTime? creationDate;
        string category;
        #endregion
        #endregion

        #region Constructor
        public Activity()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdActivity")]
        [DataMember]
        public Int64 IdActivity
        {
            get
            {
                return idActivity;
            }

            set
            {
                idActivity = value;
                OnPropertyChanged("IdActivity");
            }
        }


        [Column("Subject")]
        [DataMember]
        public string Subject
        {
            get
            {
                return subject;
            }

            set
            {
                subject = value;
                OnPropertyChanged("Subject");
            }
        }


        [Column("IdActivityType")]
        [DataMember]
        public Int32 IdActivityType
        {
            get
            {
                return idActivityType;
            }

            set
            {
                idActivityType = value;
                OnPropertyChanged("IdActivityType");
            }
        }


        [Column("Description")]
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("FromDate")]
        [DataMember]
        public DateTime? FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        [Column("ToDate")]
        [DataMember]
        public DateTime? ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        [Column("Location")]
        [DataMember]
        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        [Column("IdOwner")]
        [DataMember]
        public Int32 IdOwner
        {
            get
            {
                return idOwner;
            }

            set
            {
                idOwner = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("CreatedIn")]
        [DataMember]
        public DateTime? CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
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

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue LookupValue
        {
            get
            {
                return lookupValue;
            }

            set
            {
                lookupValue = value;
                OnPropertyChanged("LookupValue");
            }
        }

        [NotMapped]
        [DataMember]
        public People People
        {
            get
            {
                return people;
            }

            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ActivityAttendees> ActivityAttendees
        {
            get
            {
                return activityAttendees;
            }

            set
            {
                activityAttendees = value;
                OnPropertyChanged("ActivityAttendees");
            }
        }

        [NotMapped]
        [DataMember]
        public string ActivityAttendeesString
        {
            get { return activityAttendeesString; }
            set
            {
                activityAttendeesString = value;
                OnPropertyChanged("ActivityAttendeesString");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ActivityWatcher> ActivityWatchers
        {
            get { return activityWatchers; }
            set
            {
                activityWatchers = value;
                OnPropertyChanged("ActivityWatchers");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByActivity> LogEntriesByActivity
        {
            get
            {
                return logEntriesByActivity;
            }

            set
            {
                logEntriesByActivity = value;
                OnPropertyChanged("LogEntriesByActivity");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByActivity> CommentsByActivity
        {
            get
            {
                return commentsByActivity;
            }

            set
            {
                commentsByActivity = value;
                OnPropertyChanged("CommentsByActivity");
            }
        }

        [NotMapped]
        [DataMember]
        public string BothLongitudeLatitude
        {
            get { return bothLongitudeLatitude; }
            set
            {
                bothLongitudeLatitude = value;
                OnPropertyChanged("BothLongitudeLatitude");
            }
        }
        [Column("Latitude")]
        [DataMember]
        public Double? Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        [Column("Longitude")]
        [DataMember]
        public Double? Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ActivityLinkedItem> ActivityLinkedItem
        {
            get
            {
                return activityLinkedItem;
            }

            set
            {
                activityLinkedItem = value;
                OnPropertyChanged("ActivityLinkedItem");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ActivityAttachment> ActivityAttachment
        {
            get
            {
                return activityAttachment;
            }

            set
            {
                activityAttachment = value;
                OnPropertyChanged("ActivityAttachment");
            }
        }

        [Column("IdActivityStatus")]
        [DataMember]
        public Int32? IdActivityStatus
        {
            get
            {
                return idActivityStatus;
            }

            set
            {
                idActivityStatus = value;
                OnPropertyChanged("IdActivityStatus");
            }
        }

        [Column("IsCompleted")]
        [DataMember]
        public byte? IsCompleted
        {
            get
            {
                return isCompleted;
            }

            set
            {
                isCompleted = value;
                OnPropertyChanged("IsCompleted");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue ActivityStatus
        {
            get
            {
                return activityStatus;
            }

            set
            {
                activityStatus = value;
                OnPropertyChanged("ActivityStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64? PlannedAppointment
        {
            get
            {
                return plannedAppointment;
            }

            set
            {
                plannedAppointment = value;
                OnPropertyChanged("PlannedAppointment");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64? ActualAppointment
        {
            get
            {
                return actualAppointment;
            }

            set
            {
                actualAppointment = value;
                OnPropertyChanged("ActualAppointment");
            }
        }


        [NotMapped]
        [DataMember]
        public string GUIDString
        {
            get
            {
                return guidString;
            }

            set
            {
                guidString = value;
                OnPropertyChanged("GUIDString");
            }
        }

        [NotMapped]
        [DataMember]
        public string ReportGroup
        {
            get
            {
                return reportGroup;
            }

            set
            {
                reportGroup = value;
                OnPropertyChanged("ReportGroup");
            }
        }

        [NotMapped]
        [DataMember]
        public byte? IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public string ActivityGridStatus
        {
            get { return activityGridStatus; }
            set
            {
                activityGridStatus = value;
                OnPropertyChanged("ActivityGridStatus");
            }
        }

        [Column("CloseDate")]
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
        public List<ActivityTag> ActivityTags
        {
            get { return activityTags; }
            set
            {
                activityTags = value;
                OnPropertyChanged("ActivityTags");
            }
        }

        [NotMapped]
        [DataMember]
        public string ActivityTagsString
        {
            get { return activityTagsString; }
            set
            {
                activityTagsString = value;
                OnPropertyChanged("ActivityTagsString");
            }
        }

        [NotMapped]
        [DataMember]
        public Notification Notification
        {
            get { return notification; }
            set
            {
                notification = value;
                OnPropertyChanged("Notification");
            }
        }

        [NotMapped]
        [DataMember]
        public ActivityMail ActivityMail
        {
            get { return activityMail; }
            set
            {
                activityMail = value;
                OnPropertyChanged("ActivityMail");
            }
        }

        [Column("ReminderDateTime")]
        [DataMember]
        public DateTime? ActivityReminderDateTime
        {
            get { return activityReminderDateTime; }
            set
            {
                activityReminderDateTime = value;
                OnPropertyChanged("ActivityReminderDateTime");
            }
        }

        [Column("IsSentMail")]
        [DataMember]
        public byte IsSentMail
        {
            get { return isSentMail; }
            set
            {
                isSentMail = value;
                OnPropertyChanged("IsSentMail");
            }
        }

        [Column("IsInternal")]
        [DataMember]
        public byte IsInternal
        {
            get { return isInternal; }
            set
            {
                isInternal = value;
                OnPropertyChanged("IsInternal");
            }
        }

        [Column("MinDueDate")]
        [DataMember]
        public DateTime? MinDueDate
        {
            get { return minDueDate; }
            set
            {
                minDueDate = value;
                OnPropertyChanged("MinDueDate");
            }
        }


        [Column("MaxDueDate")]
        [DataMember]
        public DateTime? MaxDueDate
        {
            get { return maxDueDate; }
            set
            {
                maxDueDate = value;
                OnPropertyChanged("MaxDueDate");
            }
        }

        #region chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        [NotMapped]
        [DataMember]
        public List<ActionsLinkedItems> ActionsLinkedItems
        {
            get
            {
                return actionsLinkedItems;
            }

            set
            {
                actionsLinkedItems = value;
                OnPropertyChanged("ActionsLinkedItems");
            }
        }
        [DataMember]
        public Int64 IdActionPlanItem
        {
            get { return idActionPlanItem; }
            set
            {
                idActionPlanItem = value;
                OnPropertyChanged("IdActionPlanItem");
            }
        }
        [Column("CreationDate")]
        [DataMember]
        public DateTime? CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }
        [Column("Category")]
        [DataMember]
        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }
        #endregion
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
