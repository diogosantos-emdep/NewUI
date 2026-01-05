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
            return string.Format(uiCulture, LongestTextFormat, current);
        }
        protected override string FormatTextInternal(object current, string format)
        {
            
            return string.Format(uiCulture, format, current);
        }
    }
}
