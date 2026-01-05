using DevExpress.Xpf.Scheduling;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Internal;
using DevExpress.XtraScheduler.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Utils;

namespace Emdep.Geos.UI.Helper
{
    public class Appointment
    {
        public int UniqueID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? QueryStart { get; set; }
        public DateTime? QueryEnd { get; set; }
        public bool? AllDay { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? Label { get; set; }
        public int? ResourceID { get; set; }
        public string ResourceIDs { get; set; }
        public string ReminderInfo { get; set; }
        public string RecurrenceInfo { get; set; }
        public int? Type { get; set; }
        public ulong? IdEmployeeLeave { get; set; }
        public sbyte? IsAllDayEvent { get; set; }
        public string EmployeeCode { get; set; }
        public long? IdEmployeeAttendance { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal DailyHoursCount { get; set; }
        public int IdEmployee { get; set; }
        public double TotalLeaveDurationInHours { get; set; }
        public DateTime? AccountingDate { get; set; }

        public int IsNightShift { get; set; }

        public bool AttendanceIsManual { get; set; }
    }

}
