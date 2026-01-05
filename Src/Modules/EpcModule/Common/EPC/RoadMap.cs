using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
    public class RoadMap
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public LookupValue Priority { get; set; }
        public RoadMapStatus? Status { get; set; }
        public string Release { get; set; }
        public DateTime? RequestDate { get; set; }


    }
}
