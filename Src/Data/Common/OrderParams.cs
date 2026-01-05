using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Emdep.Geos.Data.Common
{
    public class OrderParams
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
        public string idsSelectedUser { get; set; }

        [DataMember]
        public RoleType Roles { get; set; }
		//[nsatpute][20.11.2025][GEOS2-8367]
        [DataMember]
        public ActiveSite activeSite { get; set; }
    }

}
