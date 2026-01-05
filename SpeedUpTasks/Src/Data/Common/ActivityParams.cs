using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class ActivityParams
    {
        [DataMember]
        public Int32 idActiveUser { get; set; }
        [DataMember]
        public string idOwner { get; set; }
        [DataMember]
        public Int32 idPermission { get; set; }
        [DataMember]
        public string idPlant { get; set; }
        [DataMember]
        public DateTime accountingYearFrom { get; set; }
        [DataMember]
        public DateTime accountingYearTo { get; set; }
    }
}
