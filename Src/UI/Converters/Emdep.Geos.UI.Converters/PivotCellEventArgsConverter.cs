using DevExpress.Mvvm.UI;
using DevExpress.Xpf.PivotGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Converters
{
    public class PivotCellEventArgsConverter : EventArgsConverterBase<PivotCellEventArgs>
    {
        protected override object Convert(object sender, PivotCellEventArgs args)
        {
            return args.GetRowFields().Select(f => args.GetFieldValue(f)).ToArray();
        }
    }
}
