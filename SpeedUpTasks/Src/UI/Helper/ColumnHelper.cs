using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public static class ColumnHelper
    {
        public static readonly DependencyProperty EnableNullTextProperty =
            DependencyProperty.RegisterAttached("EnableNullText", typeof(bool), typeof(ColumnHelper),
            new PropertyMetadata(false, OnEnableNullTextChanged));
        public static bool GetEnableNullText(GridControl obj)
        {
            return (bool)obj.GetValue(EnableNullTextProperty);
        }
        public static void SetEnableNullText(GridControl obj, bool value)
        {
            obj.SetValue(EnableNullTextProperty, value);
        }
        static void OnEnableNullTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridControl c = d as GridControl;
            c.Loaded += c_Loaded;
        }
        static void c_Loaded(object sender, RoutedEventArgs e)
        {
            GridControl c = sender as GridControl;
            c.Loaded -= c_Loaded;
            foreach (GridColumn cc in c.Columns)
            {
                cc.ActualEditSettings.NullText = GetNullText(cc);
                cc.ActualEditSettings.NullValue = GetNullValue(cc);
            }
        }


        public static readonly DependencyProperty NullTextProperty =
            DependencyProperty.RegisterAttached("NullText", typeof(string), typeof(ColumnHelper),
            new PropertyMetadata(null, OnNullTextChanged));
        public static string GetNullText(GridColumn obj)
        {
            return (string)obj.GetValue(NullTextProperty);
        }
        public static void SetNullText(GridColumn obj, string value)
        {
            obj.SetValue(NullTextProperty, value);
        }
        static void OnNullTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridColumn c = d as GridColumn;
            if (c.ActualEditSettings != null)
                c.ActualEditSettings.NullText = e.NewValue.ToString();
        }


        public static readonly DependencyProperty NullValueProperty =
            DependencyProperty.RegisterAttached("NullValue", typeof(object), typeof(ColumnHelper), new PropertyMetadata(null));
        public static object GetNullValue(GridColumn obj)
        {
            return (object)obj.GetValue(NullValueProperty);
        }
        public static void SetNullValue(GridColumn obj, object value)
        {
            obj.SetValue(NullValueProperty, value);
        }
        static void OnNullValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridColumn c = d as GridColumn;
            if (c.ActualEditSettings != null)
                c.ActualEditSettings.NullValue = e.NewValue;
        }



    }
}
