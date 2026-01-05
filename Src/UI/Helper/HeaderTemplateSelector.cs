using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
 public class HeaderTemplateSelectorOffice : DataTemplateSelector
    {
        public DataTemplate WhiteBlueTemplate { get; set; }
        public DataTemplate BlackBlueTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            PageViewItem row = item as PageViewItem;
            
            if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() != null) 
            {
                if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                    return WhiteBlueTemplate;
                else
                    return BlackBlueTemplate;

            }
            return base.SelectTemplate(item, container);
        }
    }
 public class HeaderTemplateSelectorProduction : DataTemplateSelector
    {
        public DataTemplate WhiteBlueTemplate { get; set; }
        public DataTemplate BlackBlueTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            PageViewItem row = item as PageViewItem;

            if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() != null)
            {
                if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                    return WhiteBlueTemplate;
                else
                    return BlackBlueTemplate;

            }
            return base.SelectTemplate(item, container);
        }
    }

}
