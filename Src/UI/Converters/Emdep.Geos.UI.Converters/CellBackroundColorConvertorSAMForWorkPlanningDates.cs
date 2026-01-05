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
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;

namespace Emdep.Geos.UI.Converters
{
    public class CellBackroundColorConvertorSAMForWorkPlanningDates : MarkupExtension, IMultiValueConverter
    {
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public static List<GeosAppSetting> GeosAppSettingListStatic = null;
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string htmlColor = string.Empty;
            try
            {
                if(GeosAppSettingListStatic == null)
                {
                    GeosAppSettingListStatic = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17,67,68");
                }

                if (values.Length == 1 && values[0] != DependencyProperty.UnsetValue)
                {
                    DateTime resultDate;
                    bool isDateColumn = DateTime.TryParse(values[0].ToString(), out resultDate);
                    if (isDateColumn)
                    {
                        if (resultDate.DayOfWeek == DayOfWeek.Saturday || resultDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            //return new SolidColorBrush(Colors.LightSlateGray);
                            //return new SolidColorBrush(Colors.LightSlateGray);
                            // 68,'Non_Working_Calendar_Day_Color', 0, '(1;#F2F2F2),(2;#F2F2F2)'
                            var setting = GeosAppSettingListStatic.FirstOrDefault(x => x.IdAppSetting == 68)?.DefaultValue;

                            var themewiseSetting = setting.Split(',');
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                htmlColor = themewiseSetting[0].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                            }
                            else
                            {
                                htmlColor = themewiseSetting[1].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                            }

                            return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));
                        }
                    }
                }

                if (values.Length <3 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                {
                    return null;
                }
                else
                {
                    if (values[0] != null && values[1] != null && values[2] != null)
                    {
                        List<GeosAppSetting> GeosAppSettingList = (List<GeosAppSetting>)values[0];
                        DataRowView dr = ((System.Data.DataRowView)((RowData)values[1]).Row);

                        DateTime resultDate;
                        bool isDateColumn = DateTime.TryParse(((DevExpress.Xpf.Grid.BaseColumn)values[2]).Header.ToString(), out resultDate);
                        if (isDateColumn)
                        {
                            if (resultDate.DayOfWeek == DayOfWeek.Saturday || resultDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                //return new SolidColorBrush(Colors.LightSlateGray);
                                // 68,'Non_Working_Calendar_Day_Color', 0, '(1;#F2F2F2),(2;#F2F2F2)'
                                var setting = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 68)?.DefaultValue;

                                var themewiseSetting = setting.Split(',');
                                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                {
                                    htmlColor = themewiseSetting[0].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                                }
                                else
                                {
                                    htmlColor = themewiseSetting[1].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                                }

                                return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));
                            }

                            if (dr != null && dr.Row["OtsObject"] != DBNull.Value && dr.Row["IsItLoggedTimeRow"].ToString() == 0.ToString())
                            {
                                Ots OtsObject = (Ots)dr.Row["OtsObject"];

                                if (OtsObject.ExpectedStartDate != null && OtsObject.ExpectedEndDate != null &&
                                   resultDate >= OtsObject.ExpectedStartDate.Value.Date && resultDate <= OtsObject.ExpectedEndDate.Value.Date &&
                                   resultDate.DayOfWeek != DayOfWeek.Saturday && resultDate.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    // return new SolidColorBrush(Colors.LightSkyBlue);
                                    // 67,'Planned_Calendar_Day_Color', 0, '(1;#DDEBF7),(2;#DDEBF7)'

                                    var setting = GeosAppSettingList.FirstOrDefault(x => x.IdAppSetting == 67)?.DefaultValue;

                                    var themewiseSetting = setting.Split(',');
                                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                                    {
                                        htmlColor = themewiseSetting[0].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                                    }
                                    else
                                    {
                                        htmlColor = themewiseSetting[1].Replace("(", String.Empty).Replace(")", String.Empty).Split(';')[1];
                                    }

                                    return (SolidColorBrush)(new BrushConverter().ConvertFrom(htmlColor));
                                }
                            }
                        }
                        return null;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in CellBackroundColorConvertorSAMForWorkPlanningDates Method Convert()...." 
                    + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
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
