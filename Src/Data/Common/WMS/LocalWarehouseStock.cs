using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
	// [nsatpute][07-04-2025][GEOS2-7015]
    public class LocalWarehouseStock
    {
        public double FourMonthAverage { get; set; }
        public double TwelveMonthAverage { get; set; }

        public double Target { get; set; }
        public List<LocalWarehouseChartData> LocalWarehouseChartDataList { get; set; }
    }
}
