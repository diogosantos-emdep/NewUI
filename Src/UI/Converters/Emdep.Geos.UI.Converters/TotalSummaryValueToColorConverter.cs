using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Globalization;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Grid;

namespace Emdep.Geos.UI.Converters
{
    public class TotalSummaryValueToColorConverter : MarkupExtension, IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
                List<GridTotalSummaryData> summaryList = (List<GridTotalSummaryData>)value;
                if (summaryList != null && summaryList.Count == 0)
                    return null;
            if (summaryList != null)
            {
                if (summaryList[0].Item.FieldName == "RevisionItem.NumItem")
                {
                    if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                        return Brushes.Black;
                    else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                        return Brushes.White;
                }

                if (summaryList[0].Item.FieldName == "RevisionItem.Quantity")
                {
                    int sumValue = System.Convert.ToInt32(summaryList[0].Value);
                    if (sumValue > 0)
                        if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                            return Brushes.Black;
                        else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                            return Brushes.White;
                }

                if (summaryList[0].Item.FieldName == "RevisionItem.RemainingQuantity")
                {
                    int sumValue = System.Convert.ToInt32(summaryList[0].Value);
                    if (sumValue >= 0)
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                            return Brushes.Black;
                        else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                            return Brushes.White;
                    }
                }
                if (summaryList[0].Item.FieldName == "WeightKg")
                {
                    int sumValue = System.Convert.ToInt32(summaryList[0].Value);
                    if (sumValue > 0)
                    {
                        if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                            return Brushes.Black;
                        else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                            return Brushes.White;
                    }
                    else
                    {

                        if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                            return Brushes.Black;
                        else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                            return Brushes.White;

                    }
                }

                if (summaryList[0].Item.FieldName == "ValidatedCount")
                {
                    int sumValue = System.Convert.ToInt32(summaryList[0].Value);
                    if (sumValue > 0)
                    {
                        if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                            return Brushes.Black;
                        else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                            return Brushes.White;
                    }
                    else
                    {

                        if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                            return Brushes.Black;
                        else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                            return Brushes.White;

                    }
                }


            }
            
            return Brushes.Transparent;
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
