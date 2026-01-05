using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Emdep.Geos.UI.Common;

namespace Emdep.Geos.UI.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]

    public class CheckIfDetailsGridContainRecord : MarkupExtension, IValueConverter
    {
        #region IValueConverter Members
        
        public object Convert(object value, System.Type targetType,object parameter, System.Globalization.CultureInfo culture)
        {
            // Obtaining the value to be converted 
            string categoryValue = (string)value;

            // Specifying values for which to show expand buttons
            string[] categories = new string[] { "First", "Third" };
            if (categoryValue!=null)
                return true;

            // Disable expand button if the value isn't in the list
            return false;
        }

        public object ConvertBack(object value, System.Type targetType,object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
