using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using System.Data;

namespace Emdep.Geos.UI.Converters
{
    public class ValidatedCellBackgroundConverter : IMultiValueConverter
    {
        //[nsatpute][12-12-2024][GEOS2-6382] 
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var fieldName = values[1] as string;
            if (fieldName == "Status" && values[0] != null && (((System.Data.DataRowView)values[0]).Row.ItemArray[2]).ToString() == "Audited")
            {
                return Brushes.Green;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
