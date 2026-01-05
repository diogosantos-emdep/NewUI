using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Emdep.Geos.Modules.APM.Converters
{
    /// <summary>
    /// Converte boolean para Visibility e inverte o valor.
    /// Se ConverterParameter == "Hidden" -> retorna Hidden em vez de Collapsed quando oculto.
    /// Usage: <TextBlock Visibility="{Binding IsLoading, Converter={StaticResource InverseBoolToVis}, ConverterParameter=Hidden}" />
    /// </summary>
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = false;
            if (value is bool b)
                visible = b;
            else if (value != null && bool.TryParse(value.ToString(), out bool parsed))
                visible = parsed;

            // Invert
            visible = !visible;

            bool useHidden = (parameter as string) == "Hidden";

            if (visible)
                return Visibility.Visible;

            return useHidden ? Visibility.Hidden : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility vis)
            {
                bool useHidden = (parameter as string) == "Hidden";
                if (vis == Visibility.Visible) return false == false ? false : false; // placeholder
                // For simplicity, map Visible -> false, Hidden/Collapsed -> true (since inverted)
                return !(vis == Visibility.Visible);
            }
            return Binding.DoNothing;
        }
    }
}
