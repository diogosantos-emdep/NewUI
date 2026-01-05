using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class OfferActivity
    {

        //  public List<ActivityAttachment> ActivityAttachment { get; set; }

        public List<OfferActivityAttendees> ActivityAttendees { get; set; }

        public string ActivityAttendeesString { get; set; }

        public string ActivityGridStatus { get; set; }

        public List<OfferActivityLinkedItem> ActivityLinkedItem { get; set; }

        // public ActivityMail ActivityMail { get; set; }

        public DateTime? ActivityReminderDateTime { get; set; }

        // public LookupValue ActivityStatus { get; set; }

        //  public List<ActivityTag> ActivityTags { get; set; }

        public string ActivityTagsString { get; set; }

        //   public List<ActivityWatcher> ActivityWatchers { get; set; }

        public long? ActualAppointment { get; set; }

        public string BothLongitudeLatitude { get; set; }

        public DateTime? CloseDate { get; set; }

        public List<OfferLogActivity> CommentsByActivity { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedIn { get; set; }

        public string Description { get; set; }

        public DateTime? FromDate { get; set; }

        public string GUIDString { get; set; }

        public long IdActivity { get; set; }

        public int? IdActivityStatus { get; set; }

        public int IdActivityType { get; set; }

        public int IdOwner { get; set; }

        public byte? IsCompleted { get; set; }

        public byte? IsDeleted { get; set; }

        public byte IsInternal { get; set; }

        public byte IsSentMail { get; set; }

        public double? Latitude { get; set; }

        public string Location { get; set; }

        public List<OfferLogActivity> LogEntriesByActivity { get; set; }

        public double? Longitude { get; set; }

        //  public LookupValue LookupValue { get; set; }

        public DateTime? MaxDueDate { get; set; }

        public DateTime? MinDueDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime? ModifiedIn { get; set; }

        // public Notification Notification { get; set; }

        //public People People { get; set; }

        public long? PlannedAppointment { get; set; }

        public string ReportGroup { get; set; }

        public string Subject { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
