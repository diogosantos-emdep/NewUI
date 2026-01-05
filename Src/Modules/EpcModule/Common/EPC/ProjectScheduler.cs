using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
  public  class ProjectScheduler
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Task { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public float? Progress { get; set; }
    }
}
