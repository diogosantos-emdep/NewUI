using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class MyColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate template1 { get; set; }
        public DataTemplate template2 { get; set; }
        public DataTemplate template3 { get; set; }
        public DataTemplate totalTemplate { get; set; }
        public DataTemplate totalTemplate1 { get; set; }
        public DataTemplate totalTemplate2 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Column)//[rdixit][GEOS2-5389]08.05.2024] To avoid resource not found for column template error
            {
                Column ci = item as Column;
                if (ci.Settings == SettingsType.IdDrawing)
                {
                    return template1;
                }
                else if (ci.Settings == SettingsType.Name)
                {
                    return template2;
                }
                else if (ci.Settings == SettingsType.Image)
                {
                    return totalTemplate;
                }
                else if (ci.Settings == SettingsType.IsChecked)
                {
                    return totalTemplate1;
                }
                else
                {
                    return template3;
                }
            }
            else
            {
                ColumnItem ci = item as ColumnItem;
                if (ci != null)
                {
                    if (ci.ColumnFieldName == "CountryName" || ci.ColumnFieldName == "CustomerName" || ci.ColumnFieldName == "SiteName" || ci.ColumnFieldName == "ZoneName" ||
                        ci.ColumnFieldName == "Month" || ci.ColumnFieldName == "Week" || ci.ColumnFieldName == "GROUP" || ci.ColumnFieldName == "Plant" ||
                        ci.ColumnFieldName == "Country" || ci.ColumnFieldName == "Group")
                    {
                        return template2;
                    }
                    else if (ci.ColumnFieldName == "TARGET_Percentage")
                    {
                        return template3;
                    }
                    else if (ci.ColumnFieldName.Contains("_Total"))
                    {
                        if (ci.ColumnFieldName == "WON_Total")
                            return totalTemplate1;
                        else if (ci.ColumnFieldName == "QUOTED_Total")
                            return totalTemplate2;
                        return totalTemplate;
                    }
                    else
                    {
                        return template1;
                    }
                }
            }

            return base.SelectTemplate(item, container);
        }
    }

}
