using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;

namespace Emdep.Geos.Modules.Hrm.CommonClass
{
    /// <summary>
    /// [001][skale][31-12-2019][GEOS2-1831]Import attendance (all data sources) with night shifts values. [IES15]
    /// </summary>
    public class EmployeeAttandance
    {
        public string Employee { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public bool In { get; set; }
        public bool Out { get; set; }
        public LookupValue Type { get; set; }
        public string EmployeeClockTimeID { get; set; }

        public bool IsRowError {get;set;}
        //[001] added
        public Int32 idEmployee { get; set; }

        public CompanyShift CompanyShift { get; set; }

        public DateTime? AccountingDate { get; set; }

    }

}
