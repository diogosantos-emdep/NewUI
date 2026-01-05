using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class TimetrackingList
    {
        [DataMember(Order = 1)]
        public string Plants { get; set; }

        [DataMember(Order = 2)]
        public string From { get; set; }
        [DataMember(Order = 3)]
        public string To { get; set; }

        [DataMember(Order = 4)]
        public string Workstage { get; set; }

        [DataMember(Order = 5)]
        public string Currency { get; set; }

        [DataMember(Order = 6)]
        public string ParameterMainConn { get; set; }

        [DataMember(Order = 7)]
        public string ParameterLoginContext { get; set; }

        [DataMember(Order = 8)]
        public string ParameterPlantwiseconnectionstring { get; set; }

        [DataMember(Order = 9)]
        public string Plantwiseconnectionstringslave { get; set; }
        [DataMember(Order = 10)]
        public string login { get; set; }

       
       
    }

  
}
