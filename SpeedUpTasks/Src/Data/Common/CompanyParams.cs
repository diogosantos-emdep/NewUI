using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Emdep.Geos.Data.Common
{
    public class CompanyParams
    {

        [DataMember]
        public string StringValues { get; set; }
        [DataMember]
        public int idUser { get; set; }
        [DataMember]
        public int idZone { get; set; }
        [DataMember]
        public int idUserPermission { get; set; }
        [DataMember]
        public RoleType Roles { get; set; }
    }
    public enum RoleType
    {
        SalesAssistant,
        SalesPlantManager,
        SalesGlobalManager
    }
}
