using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.UI.Helper
{
    public static class FocusExtension
    {
        #region TaskLog
        // [GEOS2-253] Sprint60 Lost focus in picking scan [adadibathina]
        #endregion

        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusExtension), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnIsFocusedPropertyChanged));

        private static void OnIsFocusedPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if (!((bool)e.NewValue))
                return;
            uie.Focus();
            uie.LostFocus += UieOnLostFocus;
        }

        private static void UieOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            var uie = sender as UIElement;
            if (uie == null)
                return;
            uie.LostFocus -= UieOnLostFocus;
            uie.SetValue(IsFocusedProperty, false);
        }

    }
}
