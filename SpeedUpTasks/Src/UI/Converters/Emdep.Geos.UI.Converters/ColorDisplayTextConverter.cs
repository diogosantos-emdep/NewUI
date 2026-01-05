using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.UI.Converters
{
    public class ColorDisplayTextConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Color)value == Colors.Transparent) return "Automatic";
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ColorDisplayTextConverter();
        }
    }
    public class IsNullColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new IsNullColorConverter();
        }
    }


    public class ColorToBrushConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "hover")
            {
                switch (value.ToString())
                {

                    case "0.2":
                        return new SolidColorBrush(Colors.Red);

                    case "0.4":
                        return new SolidColorBrush(Colors.Orange);

                    case "0.6":
                        return new SolidColorBrush(Colors.Yellow);

                    case "0.8":
                        return new SolidColorBrush(Colors.DeepSkyBlue);

                    case "1":
                        return new SolidColorBrush(Colors.Green);
                    default:
                        return new SolidColorBrush(Colors.Transparent);
                }
            }


            return value == null ? null : new SolidColorBrush((Color)value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ColorToBrushConverter();
        }
    }

    public class ColorToBrushConverterNew : MarkupExtension, IValueConverter
    {   
         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse(value.ToString()) >= 0 && int.Parse(value.ToString()) <= 20)
            {
                return Brushes.Red;
            }
            if (int.Parse(value.ToString()) > 20 && int.Parse(value.ToString()) <= 40)
            {
                return Brushes.Orange;
            }
            if (int.Parse(value.ToString()) > 40 && int.Parse(value.ToString()) <= 60)
            {
                return Brushes.Yellow;
            }
            if (int.Parse(value.ToString()) > 60 && int.Parse(value.ToString()) <= 80)
            {
                return Brushes.SkyBlue;
            }
            if (int.Parse(value.ToString()) > 80 && int.Parse(value.ToString()) <= 100)
            {
                return Brushes.Green;
            }
            return Brushes.Transparent;
           
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ColorToBrushConverterNew();
        }

      
    }











    public class CustConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is int))
                return null;
            var vl = (int)value;
            var dt = (double)vl / 20;
            var vlIt = Math.Ceiling(dt) * 20;
            return (int)vlIt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Int32.Parse(value.ToString());
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
