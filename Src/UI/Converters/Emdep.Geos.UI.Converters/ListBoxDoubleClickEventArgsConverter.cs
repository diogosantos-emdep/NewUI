using DevExpress.Mvvm.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Emdep.Geos.UI.Converters
{
    public class ListBoxDoubleClickEventArgsConverter : EventArgsConverterBase<MouseEventArgs>
    {
        protected override object Convert(object sender, MouseEventArgs args)
        {
            var element = LayoutTreeHelper.GetVisualParents((DependencyObject)args.OriginalSource, (DependencyObject)sender).OfType<ListBoxItem>().FirstOrDefault();
            return element != null ? element.DataContext : null;
        }
    }
}
