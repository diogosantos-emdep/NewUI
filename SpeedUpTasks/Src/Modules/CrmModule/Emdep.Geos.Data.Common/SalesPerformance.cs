using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Crm.Emdep.Geos.Data.Common
{
    public class SalesPerformance
    {
        private string salesType;
        private string salesPerformanceAmount;


        public string SalesType
        {
            get { return salesType; }
            set { salesType = value; }
        }


        public string SalesPerformanceAmount
        {
            get { return salesPerformanceAmount; }
            set { salesPerformanceAmount = value; }
        }
    }
}
