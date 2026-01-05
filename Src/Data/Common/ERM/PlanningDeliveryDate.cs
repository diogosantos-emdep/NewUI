using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class PlanningDeliveryDate
    {
        [DataMember]
        public string DeliveryDate { get; set; }
        [DataMember]
        public string copyDeliveryDate { get; set; }
        [DataMember]
        public List<String> OtCodeList { get; set; }
        public PlanningDeliveryDate()
        {
        }
        public PlanningDeliveryDate(string CopyDeliveryDate, List<string> OtCodeList)
        {
            CopyDeliveryDate = copyDeliveryDate;
            OtCodeList = new List<string>(OtCodeList);
        }
        public override string ToString()
        {
            return DeliveryDate;
        }
    }
}

