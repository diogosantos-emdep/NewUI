using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    // [nsatpute][07-04-2025][GEOS2-7015]
    public class LocalWarehouseChartData : ICloneable
    {
        public string Category { get; set; }
        public double BarValue { get; set; }
        public double LineValue { get; set; }
        public double TargetLineValue { get; set; }
        public int MonthNumber { get; set; }

        public double GlobalSixMonthAverage { get; set; }
        public double GlobalTwelveMonthAverage { get; set; }


        public double BarValueScaleDown
        {
            get
            {
                return Math.Round(BarValue / 1000.0, 0);
            }
        }
        public double LineValueScaleDown
        {
            get
            {
                return Math.Round(LineValue / 1000.0, 0);
            }
        }

        public double TargetLineValueScaleDown
        {
            get
            {
                return Math.Round(TargetLineValue / 1000.0, 0);
            }
        }


        public double GlobalSixMonthAverageScaleDown
        {
            get
            {                
                    return Math.Round(GlobalSixMonthAverage / 1000.0, 0);                
            }
        }

        public double GlobalTwelveMonthAverageScaleDown
        {
            get
            {
                return Math.Round(GlobalTwelveMonthAverage / 1000.0, 0);
            }
        }


        public bool IsRegional { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
