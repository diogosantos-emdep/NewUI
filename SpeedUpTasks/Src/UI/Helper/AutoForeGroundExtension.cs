using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Emdep.Geos.UI.Helper
{
    public static class AutoForeGroundExtension
    {
        private static readonly DependencyProperty[] _backGrounds = new[] { Control.BackgroundProperty, TextBlock.BackgroundProperty, Page.BackgroundProperty, TextElement.BackgroundProperty };
        private static readonly DependencyProperty[] _foreGrounds = new[] { Control.ForegroundProperty, TextBlock.ForegroundProperty, Page.ForegroundProperty, TextElement.ForegroundProperty };
        private static readonly Type[] _ownerTypes = new[] { typeof(Control), typeof(TextBlock), typeof(Page), typeof(TextElement) };

        public static readonly DependencyProperty AutoForeGroundProperty = DependencyProperty.RegisterAttached("AutoForeGround", typeof(bool), typeof(AutoForeGroundExtension), new PropertyMetadata(false, OnAutoForeGroundPropertyChangedCallback));
        public static void SetAutoForeGround(DependencyObject element, bool value)
        {
            element.SetValue(AutoForeGroundProperty, value);
        }
        public static bool GetAutoForeGround(DependencyObject element)
        {
            return (bool)element.GetValue(AutoForeGroundProperty);
        }
        private static void OnAutoForeGroundPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (ReferenceEquals(d, null))
                return;

            if (!(e.NewValue is bool) || !(bool)e.NewValue)
                return;

            for (int i = 0; i < _backGrounds.Length; i++)
            {
                var dp = _backGrounds[i];
                var ownerType = _ownerTypes[i];
                if (!ownerType.IsInstanceOfType(d))
                    continue;

                var brush = d.GetValue(dp) as SolidColorBrush;
                if (ReferenceEquals(brush, null))
                    continue;

                var brightness = Brightness(brush.Color);
                var foreGround = brightness > 150 ? Brushes.Black : Brushes.White;

                d.SetValue(_foreGrounds[i], foreGround);
            }
        }

        private static int Brightness(Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }
    }
}
