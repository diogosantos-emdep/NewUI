using DevExpress.Data;
using DevExpress.Xpf.Data;
using DevExpress.Xpf.Grid;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Emdep.Geos.UI.Converters
{
    public class SummaryValueToColorConverter : MarkupExtension, IValueConverter
    {
        //double Value1 = 0;
        //double Value2 = 0;
        //bool isMore = false;
        static double Total = 0;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //string stringValue = value.ToString();
            //double Value = 0;
            //int Count=0;


            //if (!string.IsNullOrWhiteSpace(stringValue) && !stringValue.StartsWith("Total Count"))
            //{
            //    Value = double.Parse(stringValue);
            //    Count++;

            //}
            //if (Count == 0)
            //{
            //    if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
            //        return Brushes.Black;
            //    else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
            //        return Brushes.White;
            //}
            //else if (Count == 1)
            //{
            //    if (!isMore)
            //    {
            //        if (Value1 == 0)
            //        {
            //            Value1 = Value;
            //            isMore = true;
            //            if (!isMore)
            //            {
            //                if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
            //                    return Brushes.Black;
            //                else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
            //                    return Brushes.White;
            //            }
            //            else
            //            {
            //                return Brushes.Green;
            //            }
            //        }
            //        else
            //        {

            //            Value2 = Value;
            //            isMore = false;
            //            if (Value1 > Value2)
            //                return Brushes.Red;
            //            else if (Value1 == Value2)
            //                return Brushes.Green;
            //        }
            //    }
            //    else
            //    {
            //        Value2 = Value;
            //        isMore = false;
            //        if (Value1 > Value2)
            //            return Brushes.Red;
            //        else if (Value1 == Value2)
            //            return Brushes.Green;
            //    }

            //}

            List<GridTotalSummaryData> summaryList = (List<GridTotalSummaryData>)value;
            if (summaryList != null && summaryList.Count == 0)
                return null;
            if (summaryList != null)
            {
                if (summaryList[0].Item.FieldName == "Article.Reference")
                {
                    if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                        return Brushes.Black;
                    else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                        return Brushes.White;
                }

                if (summaryList[0].Item.FieldName == "Quantity")
                {
                    int sumValue = System.Convert.ToInt32(summaryList[0].Value);
                    if (sumValue > 0)
                    {
                        Total = sumValue;
                        if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                            return Brushes.Black;
                        else if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "BlackAndBlue")
                            return Brushes.White;
                    }
                }

                if (summaryList[0].Item.FieldName == "ReceivedQuantity")
                {
                    int sumValue = System.Convert.ToInt32(summaryList[0].Value);
                    if (sumValue >= 0)
                    {
                        if(sumValue<Total)
                        return new SolidColorBrush(Colors.Red);
                        if (sumValue == Total)
                            return new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        return new SolidColorBrush(Colors.Red);
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
