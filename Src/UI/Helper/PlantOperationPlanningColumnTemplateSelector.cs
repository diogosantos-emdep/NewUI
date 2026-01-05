using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class PlantOperationPlanningColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate CalenderWeek { get; set; }
        public DataTemplate Employee { get; set; }
        public DataTemplate HRExpected { get; set; }
        public DataTemplate HRPlan { get; set; }
        public DataTemplate JobDescription { get; set; }
        public DataTemplate TimeType { get; set; }
        public DataTemplate Current { get; set; }
        public DataTemplate HRResult { get; set; }
        public DataTemplate Real { get; set; }  //[Rupali Sarode][GEOS2-4553][08-06-2023]
        public DataTemplate HiddenTemplate { get; set; }
        public DataTemplate Plant { get; set; }


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
                   if (ci.ColumnFieldName.Contains("Employee_"))
                    {
                        return Employee;
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
                    if (ci.ColumnFieldName.Contains("HRResult_"))
                    {
                        return HRResult;
                    }
                    else
                    if (ci.ColumnFieldName.Contains("JobDescription_"))
                    {
                        return JobDescription;
                    }
                    else
                    if (ci.ColumnFieldName.Contains("Real_")) //[Rupali Sarode][GEOS2-4553][08-06-2023]
                    {
                        return Real;
                    }

                    else
                    if (ci.ColumnFieldName.Contains("TimeType_")) //[Rupali Sarode][GEOS2-4553][09-06-2023]
                    {
                        return TimeType;
                    }
                    else if (ci.ColumnFieldName.Contains("IdStage_"))
                    {
                        return HiddenTemplate;
                    }
                    else if (ci.ColumnFieldName.Contains("IdEmployee_"))
                    {
                        return HiddenTemplate;
                    }
                    else if (ci.ColumnFieldName.Contains("Plant"))  //[GEOS2-4839][gulab lakade][18 09 2023]
                    {
                        return Plant;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return base.SelectTemplate(item, container);
        }
        public enum PlantOperationPlanningSettingType
        {
            DefaultTemplate, CalenderWeek, Employee, HRExpected, HRPlan, JobDescription, TimeType, Current, HRResult, Real, Hidden, Plant

        }
    }
}
