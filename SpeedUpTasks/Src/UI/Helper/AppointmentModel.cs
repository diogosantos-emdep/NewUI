using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public class AppointmentModel
    { 
        DateTime start;
        DateTime end;
        string subject;
        string description;
        long label;
        //int status;
        //string location;
        //bool allday = true;
        //int eventType;
        //string recurrenceInfo;
        //string reminderInfo;
        public object ResourceId { get; set; }
        public int AppointmentType { get; set; }
        public object Data { get; set; }

        public DateTime StartTime { get { return start; } set { start = value; } }
        public DateTime EndTime { get { return end; } set { end = value; } }
        public string Subject { get { return subject; } set { subject = value; } }  
        public string Description { get { return description; } set { description = value; } }
        public long Label { get { return label; } set { label = value; } }
        //public int Status { get { return status; } set { status = value; } }
        //public string Location { get { return location; } set { location = value; } }
        //public bool AllDay { get { return allday; } set { allday = value; } }
        //public int EventType { get { return eventType; } set { eventType = value; } }
        //public string RecurrenceInfo { get { return recurrenceInfo; } set { recurrenceInfo = value; } }
        //public string ReminderInfo { get { return reminderInfo; } set { reminderInfo = value; } }
    }
}
