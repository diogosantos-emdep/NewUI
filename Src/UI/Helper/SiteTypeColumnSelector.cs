using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class SiteTypeColumnTemplateSelector : DataTemplateSelector
    {
		//[nsatpute][04-07-2024][GEOS2-5681]
        public DataTemplate YearsOfServiceTemplate { get; set; }
        public DataTemplate SitesTemplate { get; set; }
        public DataTemplate HiddenTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;

            if (ci != null)
            {
                if (ci.SiteTypeSettings == SiteTypeSettingsType.YearsOfService)
                {
                    return YearsOfServiceTemplate;
                }
                if (ci.SiteTypeSettings == SiteTypeSettingsType.Hidden)
                {
                    return HiddenTemplate;
                }
                else
                {
                   return SitesTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }

    public enum SiteTypeSettingsType
    {
        YearsOfService, Sites, SitesReadOnly, Hidden
    }
}
