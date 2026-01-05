using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class DependencyObjectHelper
    {
        public static object GetDataContext(DependencyObject obj)
        {
            return obj.GetValue(DataContextProperty);
        }
        public static void SetDataContext(DependencyObject obj, object value)
        {
            obj.SetValue(DataContextProperty, value);
        }
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.RegisterAttached("DataContext", typeof(object), typeof(DependencyObjectHelper), new PropertyMetadata(null));
    }
}
