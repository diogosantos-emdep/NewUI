using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    //[DataContract]
    //public class TimeTrackinglist
    //{
    //    [DataMember]
    //    public DateTime DeliveryDate { get; set; }
    //}
    [DataContract]
   public  class TrackingAccordian
    {
        [DataMember]
        public string deliwaryWeek { get; set; }
        [DataMember]
        public string copyDeliwaryWeek { get; set; }
        // public ObservableCollection<Template> TemplatesMenuList { get; set; }

        [DataMember]
        //public List<DateTime?> TimeTracking { get; set; }
        //public List<String> TimeTracking { get; set; }
        public List<String> TimeTracking { get; set; }
        //public string CopyDeliwaryWeek
        //{

        //}
        public override string ToString()
        {
            return deliwaryWeek;
        }

        public TrackingAccordian()
        {
        }
        //public TrackingAccordian(string DeliwaryWeek, IEnumerable<TimeTracking> TimeTracking)
        //{
        //    DeliwaryWeek = deliwaryWeek;
        //    TimeTracking = new List<TimeTracking>(TimeTracking);
        //}
        public TrackingAccordian(string DeliwaryWeek, string CopyDeliwaryWeek, List<string> TimeTracking)
        {
            DeliwaryWeek = deliwaryWeek;
            CopyDeliwaryWeek = copyDeliwaryWeek;
            TimeTracking = new List<string>(TimeTracking);
        }
    }
}
