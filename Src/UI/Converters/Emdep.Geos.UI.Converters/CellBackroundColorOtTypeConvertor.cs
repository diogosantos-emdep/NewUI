using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class CellBackroundColorOtTypeConvertor : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string htmlColor = string.Empty;

            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return null;
            }
            else
            {
                if (values[0] != null && values[1] != null)
                {
                    List<GeosAppSetting> GeosAppSettingList = (List<GeosAppSetting>)values[0];
                    DateTime todaysDate = DateTime.Now.Date;
                    Ots Ots = (Ots)values[1];

                    if (Ots.Quotation.Offer.OfferType.IdOfferType == 2 || Ots.Quotation.Offer.OfferType.IdOfferType == 3)
                    {
                        htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 17)?.DefaultValue;
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //red
                    }
                    else if (Ots.DeliveryDate.Value.Date <= todaysDate)
                    {
                        htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 14)?.DefaultValue;
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //yellow
                    }
                    else if (Ots.DeliveryDate.Value.Date > todaysDate && Ots.DeliveryDate.Value.Date <= todaysDate.AddDays(6))
                    {
                        htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 15)?.DefaultValue;
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //Green
                    }
                    else if (Ots.DeliveryDate.Value.Date >= todaysDate.AddDays(7))
                    {
                        htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 16)?.DefaultValue;
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //Blue
                    }
                    return null;
                }
                else
                    return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
