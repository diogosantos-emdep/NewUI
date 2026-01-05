using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class RealTimeMonitorColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CalendarWeek { get; set; }
        public DataTemplate CustomerETD { get; set; }
        public DataTemplate ProductionPlan { get; set; }
        public DataTemplate ProductionVision { get; set; }
        public DataTemplate ProductionTightening { get; set; }
        public DataTemplate ProductionO { get; set; }
        public DataTemplate RFQ { get; set; }
        public DataTemplate SpecialEquipments { get; set; }
        public DataTemplate Forecast { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;
            if (ci != null)
            {
                if (ci.ColumnFieldName == "CalendarWeek")
                {
                    return CalendarWeek;
                }
                else
                if (ci.ColumnFieldName == "CustomerETD")
                {
                    return CustomerETD;
                }

                else
                if (ci.ColumnFieldName == "ProductionPlan")
                {
                    return ProductionPlan;
                }
                else
                if (ci.ColumnFieldName == "Vision")
                {
                    return ProductionVision;
                }
                else
                if (ci.ColumnFieldName == "Tightening")
                {
                    return ProductionTightening;
                }
                else
                if (ci.ColumnFieldName == "ProductionO")
                {
                    return ProductionO;
                }
                else
                if (ci.ColumnFieldName == "RFQ")
                {
                    return RFQ;
                }
                else
                if (ci.ColumnFieldName == "SpecialEquipments")
                {
                    return SpecialEquipments;
                }
                else
                if (ci.ColumnFieldName == "Forecast")
                {
                    return Forecast;
                }
            }

            //ColumnItem ci = item as ColumnItem;
            //if (ci != null)
            //    return CalendarWeek;
            return base.SelectTemplate(item, container);
        }
        public enum RealTimeMonitorSettingType
        {
            CalendarWeek, CustomerETD, ProductionPlan, ProductionVision, ProductionTightening, ProductionO, RFQ, SpecialEquipments, Forecast
        }
    }
}
