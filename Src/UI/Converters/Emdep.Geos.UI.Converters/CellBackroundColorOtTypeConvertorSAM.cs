using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    public class CellBackroundColorOtTypeConvertorSAM : MarkupExtension, IMultiValueConverter
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
                    var Type = ((System.Data.DataRowView)values[1]);
                    DataRowView dr = (DataRowView)Type;
                    if (dr.Row["OfferType"] != DBNull.Value && dr.Row["DeliveryDate"]!=DBNull.Value)
                    {
                        byte idoffertype = (byte)dr.Row["OfferType"];
                        DateTime DeliveryDate = (DateTime)dr.Row["DeliveryDate"];
                        if (idoffertype == 2 || idoffertype == 3)
                        {
                            htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 17)?.DefaultValue;
                            return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //red
                        }
                        else if (DeliveryDate.Date <= todaysDate)
                        {
                            htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 14)?.DefaultValue;
                            return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //yellow
                        }
                        else if (DeliveryDate.Date > todaysDate && DeliveryDate.Date <= todaysDate.AddDays(6))
                        {
                            htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 15)?.DefaultValue;
                            return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //Green
                        }
                        else if (DeliveryDate.Date >= todaysDate.AddDays(7))
                        {
                            htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 16)?.DefaultValue;
                            return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //Blue
                        }
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
