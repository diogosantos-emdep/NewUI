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
    public class CellBackroundColorProducedModuleConvertor : MarkupExtension, IMultiValueConverter
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
                    var Type = ((System.Data.DataRowView)values[1]);
                    DataRowView dr = (DataRowView)Type;
                    var data = dr.Row["Modules"].ToString();
                    long Modules = System.Convert.ToInt64(dr.Row["Modules"]);
                    long ProducedModules = System.Convert.ToInt64(dr.Row["ProducedModules"]);
                    if (Modules == ProducedModules && Modules > 0)
                    {
                        htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 15)?.DefaultValue;
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //Green
                    }
                    else if (0 < ProducedModules && ProducedModules < Modules)
                    {
                        htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 14)?.DefaultValue;
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //yellow
                    }
                    else if (ProducedModules==0 && Modules>0)
                    {
                        htmlColor = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 17)?.DefaultValue;
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));  //red
                        
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
