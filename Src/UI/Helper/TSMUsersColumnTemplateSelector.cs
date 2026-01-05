using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace Emdep.Geos.UI.Helper
{
    public class TSMUsersColumnTemplateSelector : DataTemplateSelector
    {
        #region//[pallavi.kale][GEOS2-5388]
        public DataTemplate HiddenTemplate { get; set; }
        public DataTemplate ImageTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate DynamicTemplate { get; set; }
        public DataTemplate CountryTemplate { get; set; } //  [GEOS2-6993][pallavi.kale][19.02.2025]
        public DataTemplate TooltipColumnTemplate { get; set; }
        //  [GEOS2-5388][pallavi.kale][22.01.2025]
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;
            if (ci != null)
            {
                if (ci.TSMUsersSettings == TSMUsersSettingsType.Hidden)
                {
                    return HiddenTemplate;
                }
                else if (ci.TSMUsersSettings == TSMUsersSettingsType.Image)
                {
                    return ImageTemplate;
                }
                else if (ci.TSMUsersSettings == TSMUsersSettingsType.Default)
                {
                    return DefaultTemplate;
                }
                else if (ci.TSMUsersSettings == TSMUsersSettingsType.DynamicColumns)
                {
                    return DynamicTemplate;
                }
                //  [GEOS2-6993][pallavi.kale][19.02.2025]
                else if (ci.TSMUsersSettings == TSMUsersSettingsType.Country)
                {
                    return CountryTemplate;
                }
                else if (ci.TSMUsersSettings == TSMUsersSettingsType.Tooltip)
                {
                    return TooltipColumnTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
    #endregion
    public enum TSMUsersSettingsType
    {
        Image, Default, Hidden , DynamicColumns,Country, Tooltip
    }
}