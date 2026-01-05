using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class RealTimeMonitorHRResourcesColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate CalenderWeek { get; set; }
        public DataTemplate HRExpected { get; set; }
        public DataTemplate HRPlan { get; set; }
        public DataTemplate ProductionExpectedTime { get; set; }
        public DataTemplate ProductionExpectedTime1 { get; set; }

        public DataTemplate ProductionExpectedTime2 { get; set; }
        public DataTemplate ProductionExpectedTime3 { get; set; }
        public DataTemplate ProductionExpectedTime4 { get; set; }
        public DataTemplate ProductionExpectedTime5 { get; set; }
        public DataTemplate ProductionExpectedTime6 { get; set; }
        public DataTemplate ProductionExpectedTime7 { get; set; }
        public DataTemplate ProductionExpectedTime8 { get; set; }
        public DataTemplate ProductionExpectedTime9 { get; set; }
        public DataTemplate ProductionExpectedTime10 { get; set; }
        public DataTemplate ProductionExpectedTime11 { get; set; }
        public DataTemplate ProductionExpectedTime12 { get; set; }
        public DataTemplate ProductionExpectedTime21 { get; set; }
        public DataTemplate ProductionExpectedTime26 { get; set; }
        public DataTemplate ProductionExpectedTime27 { get; set; }
        public DataTemplate ProductionExpectedTime28 { get; set; }
        public DataTemplate ProductionExpectedTime29 { get; set; }
        public DataTemplate ProductionExpectedTime32 { get; set; }

        public DataTemplate ProductionExpectedTime33 { get; set; }
        public DataTemplate ProductionExpectedTime35 { get; set; }
        public DataTemplate ProductionExpectedTime37 { get; set; }
        public DataTemplate HiddenTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            try
            {

                ColumnItem ci = item as ColumnItem;
                if (ci != null)
                {

                    if (ci.ColumnFieldName == "CalenderWeek")
                    {
                        return CalenderWeek;
                    }
                    else
                    if (ci.ColumnFieldName.Contains("HRExpected_"))
                    {
                        return HRExpected;
                    }
                    else
                    if (ci.ColumnFieldName.Contains("HRPlan_"))
                    {
                        return HRPlan;
                    }
                    else
                    // if (ci.ColumnFieldName.Contains("ProductionExpectedTime_"))
                    //{
                    //    return ProductionExpectedTime;
                    //}
                    if (ci.ColumnFieldName == "ProductionExpectedTime_1")
                    {
                        return ProductionExpectedTime1;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_2")
                    {
                        return ProductionExpectedTime2;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_3")
                    {
                        return ProductionExpectedTime3;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_4")
                    {
                        return ProductionExpectedTime4;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_5")
                    {
                        return ProductionExpectedTime5;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_6")
                    {
                        return ProductionExpectedTime6;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_7")
                    {
                        return ProductionExpectedTime7;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_8")
                    {
                        return ProductionExpectedTime8;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_9")
                    {
                        return ProductionExpectedTime9;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_10")
                    {
                        return ProductionExpectedTime10;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_11")
                    {
                        return ProductionExpectedTime11;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_12")
                    {
                        return ProductionExpectedTime12;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_21")
                    {
                        return ProductionExpectedTime21;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_26")
                    {
                        return ProductionExpectedTime26;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_27")
                    {
                        return ProductionExpectedTime27;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_28")
                    {
                        return ProductionExpectedTime28;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_29")
                    {
                        return ProductionExpectedTime29;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_32")
                    {
                        return ProductionExpectedTime32;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_33")
                    {
                        return ProductionExpectedTime33;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_35")
                    {
                        return ProductionExpectedTime35;
                    }
                    else
                if (ci.ColumnFieldName == "ProductionExpectedTime_37")
                    {
                        return ProductionExpectedTime37;
                    }
                    else if (ci.ColumnFieldName.Contains("IdStage_"))
                    {
                        return HiddenTemplate;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return base.SelectTemplate(item, container);
        }
        public enum RealTimeMonitorHRResourcesSettingType
        {
            DefaultTemplate, CalenderWeek, HRExpected, HRPlan, ProductionExpectedTime, ProductionExpectedTime1,
            ProductionExpectedTime2, ProductionExpectedTime3, ProductionExpectedTime4, ProductionExpectedTime5,
            ProductionExpectedTime6, ProductionExpectedTime7, ProductionExpectedTime8, ProductionExpectedTime9,
            ProductionExpectedTime10, ProductionExpectedTime11, ProductionExpectedTime12, ProductionExpectedTime21,
            ProductionExpectedTime26, ProductionExpectedTime27, ProductionExpectedTime28, ProductionExpectedTime33,
            ProductionExpectedTime29, ProductionExpectedTime32, ProductionExpectedTime35, ProductionExpectedTime37, Hidden
        }
    }
}
