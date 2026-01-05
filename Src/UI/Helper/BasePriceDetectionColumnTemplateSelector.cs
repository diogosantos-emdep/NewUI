using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class BasePriceDetectionColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate template1 { get; set; }
        public DataTemplate template2 { get; set; }
        public DataTemplate template3 { get; set; }
        public DataTemplate template4 { get; set; }
        public DataTemplate template5 { get; set; }
        public DataTemplate template6 { get; set; }
        public DataTemplate template7 { get; set; }
        public DataTemplate template8 { get; set; }
        public DataTemplate template9 { get; set; }
        public DataTemplate template10 { get; set; }
        public DataTemplate template11 { get; set; }
        public DataTemplate template12 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;
            if(ci != null)
            {
                if(ci.ColumnFieldName == "Name" || ci.ColumnFieldName == "Type")
                {
                    return template1;
                }
                else if (ci.ColumnFieldName == "Code")
                {
                    return template12;
                }
                else if (ci.ColumnFieldName == "IsChecked")
                {
                    return template3;
                }
                else if (ci.ColumnFieldName == "Delete")
                {
                    return template4;
                }
                else if (ci.ColumnFieldName == "Logic")
                {
                    return template5;
                }
                else if (ci.ColumnFieldName == "Value")
                {
                    return template6;
                }
                else if (ci.ColumnFieldName == "Profit")
                {
                    return template7;
                }
                else if (ci.ColumnFieldName == "CostMargin")
                {
                    return template8;
                }
                else if (ci.ColumnFieldName == "STAT")
                {
                    return template11;
                }
                else if (ci.Settings == SettingsType.MaxCost)
                {
                    return template9;
                }
                else if (ci.Settings == SettingsType.SellPriceValue)
                {
                    return template10;
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
