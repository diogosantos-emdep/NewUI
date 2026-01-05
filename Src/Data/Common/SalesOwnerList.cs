using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class SalesOwnerList
    {
        public long IdOffer { get; set; }
        public int IdSalesOwner { get; set; }
        public string SalesOwner { get; set; }
        public int IdSite { get; set; }
        public bool IsSiteResponsibleExist { get; set; } 
    }
}
