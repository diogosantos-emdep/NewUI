using DevExpress.Xpf.Grid;
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
    public class TotalSummaryValueToWeightUnitConverter : MarkupExtension , IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            List<GridTotalSummaryData> summaryList = (List<GridTotalSummaryData>)value;
            if (summaryList != null && summaryList.Count == 0)
                return null;
            if (summaryList != null)
            {
                if (summaryList[0].Item.FieldName == "Weight")
                {
                    double TotalWeight = Math.Round(System.Convert.ToDouble(summaryList[0].Value), 3);
                    if (TotalWeight != 0)
                    {
                        return TotalWeight.ToString() + " Kg";
                    }
                    else
                        return "0.000 Kg";

                }

            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
