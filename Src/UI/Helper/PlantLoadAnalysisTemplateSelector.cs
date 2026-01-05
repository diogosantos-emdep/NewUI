using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
   public class PlantLoadAnalysisTemplateSelector: DataTemplateSelector
    {

        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate CalenderWeek { get; set; }
        public DataTemplate Plant { get; set; }
        public DataTemplate OTCode { get; set; }

        public DataTemplate CPType { get; set; }
        
        public DataTemplate Total { get; set; }
        public DataTemplate HiddenTemplate { get; set; }
        public DataTemplate CalendarWeekEquipment { get; set; }
        public DataTemplate otCodeEquipment { get; set; }

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
                    if (ci.ColumnFieldName.Contains("OTCode"))
                    {
                        return OTCode;
                    }
                    else
                    if (ci.ColumnFieldName.Contains("Plant_"))
                    {
                        return Plant;
                    }
                    else
                    if (ci.ColumnFieldName.Contains("Total"))
                    {
                        return Total;
                    }
                    
                    else if (ci.ColumnFieldName.Contains("CPType"))
                    {
                        return CPType;
                    }
                    else if (ci.ColumnFieldName.Contains("IdStage_"))
                    {
                        return HiddenTemplate;
                    }
                    else if (ci.ColumnFieldName.Contains("CalendarWeekEquipment"))
                    {
                        return CalendarWeekEquipment;
                    }
                    else if (ci.ColumnFieldName.Contains("otCodeEquipment"))
                    {
                        return otCodeEquipment;
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return base.SelectTemplate(item, container);
        }
        public enum PlantLoadAnalysisSettingType
        {
            DefaultTemplate, CalenderWeek, Plant, OTCode, CPType, Total, Hidden, CalendarWeekEquipment, otCodeEquipment

        }
    }
}
