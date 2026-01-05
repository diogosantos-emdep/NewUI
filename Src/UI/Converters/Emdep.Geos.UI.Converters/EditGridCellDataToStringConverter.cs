using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
namespace Emdep.Geos.UI.Converters
{
	//[nsatpute][19-02-2025][GEOS2-6722]
    public class EditGridCellDataToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EditGridCellData editGridCellData)
            {
                // Assuming the List<string> is bound to the Cell's Value property
                return editGridCellData.Value?.ToString();
            }
            return value?.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}