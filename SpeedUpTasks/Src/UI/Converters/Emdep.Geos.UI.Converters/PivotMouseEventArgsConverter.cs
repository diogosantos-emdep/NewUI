using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.PivotGrid;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm;
using DevExpress.Xpf.PivotGrid.Internal;

namespace Emdep.Geos.UI.Converters
{
    public class PivotMouseEventArgsConverter : EventArgsConverterBase<MouseEventArgs>
    {
        protected override object Convert(object sender, MouseEventArgs args)
        {
            var valueElement = LayoutTreeHelper.GetVisualParents((DependencyObject)args.OriginalSource).OfType<FrameworkElement>().FirstOrDefault(fe => fe.DataContext is FieldValueItem);
            if (valueElement != null)
            {
                FieldValueItem valueItem = valueElement.DataContext as FieldValueItem;
                var values = valueItem.PivotGrid.GetFieldsByArea(valueItem.IsColumn ? FieldArea.ColumnArea : FieldArea.RowArea).
                    Select(f => valueItem.PivotGrid.GetFieldValue(f, valueItem.MaxLastLevelIndex)).ToArray();
                return values;
            }
            return new object[0];
        }
    }
}

