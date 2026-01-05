
using DevExpress.Xpf.PivotGrid;
using DevExpress.Xpf.PivotGrid.Internal;

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
    public class CellContentConverter : MarkupExtension, IValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    string output = null;
        //    try
        //    {
        //        CellsAreaItem cellItem = (CellsAreaItem)value;
        //        PivotDrillDownDataSource source = ((PivotGridControl)cellItem.Field.Parent).CreateDrillDownDataSource(cellItem.ColumnIndex, cellItem.RowIndex);

        //        output = System.Convert.ToString(source[0]["ExchangeRate"]);
        //        //string.Format("{0}\r\n{1}", System.Convert.ToString(cellItem.Value), System.Convert.ToString(source[0]["ExchangeRate"]));
        //    }
        //    catch
        //    {
        //        // output = "#Err";
        //    }
        //    return output;
        //}


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = null;
            try
            {
                CellsAreaItem cellItem = (CellsAreaItem)value;
                PivotDrillDownDataSource source = ((PivotGridControl)cellItem.Field.Parent).CreateDrillDownDataSource(cellItem.ColumnIndex, cellItem.RowIndex);

                output = System.Convert.ToString(source[0]["ExchangeRate"]);
                //string.Format("{0}\r\n{1}", System.Convert.ToString(cellItem.Value), System.Convert.ToString(source[0]["ExchangeRate"]));
            }
            catch
            {
                // output = "#Err";
            }
            return output;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
