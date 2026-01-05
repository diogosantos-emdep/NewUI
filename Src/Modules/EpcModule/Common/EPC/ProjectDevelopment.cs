using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
    public class ProjectDevelopment
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Items { get; set; }
        public string Duration { get; set; }
        public string Worked { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public float? Progress { get; set; }
        public string Team { get; set; }

    }
}
