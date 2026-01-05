using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    public class AuditLocation
    {
        public bool CheckBox { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public string Delete { get; set; }
        public long IdWarehouseLocation { get; set; }
        public long IdParent { get; set; }
        public int Child { get; set; }
    }
}
