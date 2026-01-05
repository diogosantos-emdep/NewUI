using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class RTMFutureLoadParams
    {
        [DataMember]
        public byte idCurrency { get; set; }
        [DataMember]
        public Int32 idUser { get; set; }
        [DataMember]
        public Int32 idZone { get; set; }
        [DataMember]
        public DateTime accountingYearFrom { get; set; }

        [DataMember]
        public DateTime accountingYearTo { get; set; }

        [DataMember]
        public ConnectSiteDetailParams connectSiteDetailParams { get; set; }

        [DataMember]
        public ActiveSite activeSite { get; set; }

        [DataMember]
        public string idsSelectedUser { get; set; }

        [DataMember]
        public RoleType Roles { get; set; }
    }
}
