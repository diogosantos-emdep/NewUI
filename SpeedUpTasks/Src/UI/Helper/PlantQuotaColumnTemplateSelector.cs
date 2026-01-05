using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
   public class PlantQuotaColumnTemplateSelector: DataTemplateSelector
    {
       
            public DataTemplate template1 { get; set; }
            public DataTemplate template2 { get; set; }

            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                ColumnItem ci = item as ColumnItem;
            if (ci != null)
            {
                if (ci.ColumnFieldName == "PlantName")
                {
                    return template1;
                }
                else
                {
                    return template2;
                }
            }

                return base.SelectTemplate(item, container);
            }
       
    }
}
