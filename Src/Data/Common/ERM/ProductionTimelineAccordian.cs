using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class ProductionTimelineAccordian
    {
        [DataMember]
        public string logWeek { get; set; }
        [DataMember]
        public string copyLogWeek { get; set; }
       

        [DataMember]
       
        public List<String> LogDate { get; set; }
        
        public override string ToString()
        {
            return logWeek;
        }

        public ProductionTimelineAccordian()
        {
        }
        
        public ProductionTimelineAccordian(string LogWeek, string CopyLogWeek, List<string> LogDate)
        {
            LogWeek = logWeek;
            CopyLogWeek = copyLogWeek;
            LogDate = new List<string>();
        }
    }
}
