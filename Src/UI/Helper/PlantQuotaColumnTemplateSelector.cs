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
            public DataTemplate template3 { get; set; }
            public DataTemplate template4 { get; set; }
            public DataTemplate template5 { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                ColumnItem ci = item as ColumnItem;
            if (ci != null)
            {
                if (ci.ColumnFieldName == "PlantName" || ci.ColumnFieldName == "Region")
                {
                    return template1;
                }

                if(ci.ColumnFieldName.Contains("ExchangeRateDate-"))
                {
                    return template3;
                }
                if (ci.ColumnFieldName.Contains("ExchangeRate-"))
                {
                    return template4;
                }
                if (ci.ColumnFieldName.Contains("ConvertedAmount"))
                {
                    return template5;
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
