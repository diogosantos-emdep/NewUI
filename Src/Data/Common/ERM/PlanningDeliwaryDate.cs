using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class PlanningDeliwaryDate
    {
        [DataMember]
        public string DeliwaryDate { get; set; }
        [DataMember]
        public List<String> OtCodeList { get; set; }
        public PlanningDeliwaryDate()
        {
        }
        public PlanningDeliwaryDate(string CopyDeliwaryWeek, List<string> OtCodeList)
        {
            CopyDeliwaryWeek = DeliwaryDate;
            OtCodeList = new List<string>(OtCodeList);
        }
    }
}
