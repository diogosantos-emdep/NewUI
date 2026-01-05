using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class ProductionTimeLineHelper : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate DateTemplate { get; set; }
        public DataTemplate BreakTimeTemplate { get; set; }
        public DataTemplate DateHTMLColorTemplate { get; set; }

        public DataTemplate ManagementBreakTemplate { get; set; }
        #region [pallavi jadhav][GEOS2-6716][10 12 2024]
        public DataTemplate Details { get; set; }
       

        #endregion
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;
            if (ci != null)
            {
                if (ci.ColumnFieldName == "Plant" || ci.ColumnFieldName == "Employee" || ci.ColumnFieldName == "Stage" || ci.ColumnFieldName == "Week")
                {
                    return StringTemplate;
                }
                else
                    if(ci.ColumnFieldName == "Date")
                {
                    return DateTemplate;
                }
                else
                    if (ci.ColumnFieldName == "DateHTMLColor")
                {
                    return DateHTMLColorTemplate;
                }

                else if (ci.ColumnFieldName.Contains("ManagementValue_"))//Aishwarya Ingale[Geos2-5853]
                {
                    return ManagementBreakTemplate;
                }
                #region [pallavi jadhav][GEOS2-6716][10 12 2024]
                else if (ci.ColumnFieldName.Contains("Details_"))
                {
                    return Details;
                }
               
                #endregion
                else
                {
                    return BreakTimeTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
        public enum ProductionTimeLineSettingType
        {
            StringTemplate, DateTemplate, BreakTimeTemplate, DateHTMLColorTemplate, ManagementBreakTemplate, Details //[pallavi jadhav][GEOS2-6716][10 12 2024]

        }
    }
}
