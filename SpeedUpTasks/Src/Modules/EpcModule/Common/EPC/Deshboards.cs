using Emdep.Geos.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
  public class Deshboards
    {
        public string ProcessFamily { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string GroupIndicator { get; set; }
        public string Indicator { get; set; }
        public string Target { get; set; }
        //public string June { get; set; }
        public string Diciembre { get; set; }
        public Emdep.Geos.UI.Helper.CustomCellValue Current { get; set; }
        public string Trend { get; set; }
        public string Update { get; set; }
        public List<int> History { get; set; }

        public List<string> Months { get; set; }
    }
}
