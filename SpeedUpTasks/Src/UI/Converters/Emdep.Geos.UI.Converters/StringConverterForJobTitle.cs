using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class StringConverterForJobTitle : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Title = string.Empty;
            if (value != null && (string)value != string.Empty)
            {
                string JobTitle = (string)value;
                string[] JobTitleList = JobTitle.Split('\n');
               
                foreach (var item in JobTitleList)
                {
                    Title = Title + " - " + item;
                    Title = Title + "\n";
                }

                Title = Title.Remove(Title.Length - 1);             
            }
            return Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
