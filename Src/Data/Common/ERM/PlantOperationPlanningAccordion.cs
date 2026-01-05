using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class PlantOperationPlanningAccordion
    {
        [DataMember]
        public string calendarWeek { get; set; }
        [DataMember]
        public string copyCalendarWeek { get; set; }
        public override string ToString()
        {
            return calendarWeek;
        }
        public List<string> CalendarWeekList { get; set; }
        public PlantOperationPlanningAccordion()
        {
        }
        public PlantOperationPlanningAccordion(string CalendarWeek, string CopyCalendarWeek, List<string> CalendarWeekList)
        {
            CalendarWeek = calendarWeek;
            CopyCalendarWeek = copyCalendarWeek;
            CalendarWeekList = new List<string>(CalendarWeekList);

        }
    }
}
