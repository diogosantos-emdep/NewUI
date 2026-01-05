using DevExpress.Xpf.Editors.RangeControl;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public class MonthIntervalFactoryEx : MonthIntervalFactory
    {
        CultureInfo uiCulture = new CultureInfo(GeosApplication.Instance.UserSettings["Language"]);
        public override string GetLongestText(object current)
        {
            // return string.Format(uiCulture, LongestTextFormat, current);
            //on production application is closing,so fixed the issue using DefaultLongestTextFormat
            //Shubham[skadam] GEOS2-5085 If we use custom date rang and try to open order/opportunity application gets crash  27 11 2023
            return string.Format(uiCulture, DefaultLongestTextFormat, current);

        }
        protected override string FormatTextInternal(object current, string format)
        {
            
            return string.Format(uiCulture, format, current);
        }
    }
}
