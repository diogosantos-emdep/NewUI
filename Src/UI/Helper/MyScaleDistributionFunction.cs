using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public class MyScaleDistributionFunction : DevExpress.Xpf.Carousel.FunctionBase
    {
        protected override double GetValueOverride(double x)
        {
            if (x == 0.5)
                return 1;
            return 0.5;
        }
    }
}
