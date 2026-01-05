using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Emdep.Geos.UI.Helper
{
    public class ColumnBindingHelper
    {
        public static readonly DependencyProperty BindingPathProperty = DependencyProperty.RegisterAttached("BindingPath", typeof(string), typeof(ColumnBindingHelper), new UIPropertyMetadata(null, new PropertyChangedCallback(OnBindingPathChanged)));

        public static string GetBindingPath(DependencyObject target)
        {
            return (string)target.GetValue(BindingPathProperty);
        }
        public static void SetBindingPath(DependencyObject target, string value)
        {
            target.SetValue(BindingPathProperty, value);
        }
        private static void OnBindingPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            OnBindingPathChanged(o, (string)e.OldValue, (string)e.NewValue);
        }
        private static void OnBindingPathChanged(DependencyObject o, string oldValue, string newValue)
        {
            var column = o as GridColumn;
            column.DisplayMemberBinding = new Binding(newValue);
        }
    }
}
