using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class VisibilityPerBUColumnTemplateSelector : DataTemplateSelector
    {
        #region//[Shweta.thube][GEOS2-6696]
        public DataTemplate NameTemplate { get; set; }

        public DataTemplate OrganizationTemplate { get; set; }

        public DataTemplate JobDescriptionTemplate { get; set; }

        public DataTemplate JDScopeTemplate { get; set; }

        public DataTemplate DynamicTemplate { get; set; }

        public DataTemplate HiddenTemplate { get; set; }
        //public DataTemplate ElectricTestBoardsTemplate { get; set; }

        //public DataTemplate EngineeringTemplate { get; set; }

        //public DataTemplate AssemblyTemplate { get; set; }

        //public DataTemplate AdvancedTemplate { get; set; }



        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;

            if (ci != null)
            {
                if (ci.VisibilityPerBUSettings == VisibilityPerBUSettingsType.Name)
                {
                    return NameTemplate;
                }
                else if (ci.VisibilityPerBUSettings == VisibilityPerBUSettingsType.Organization)
                {
                    return OrganizationTemplate;
                }
                else if (ci.VisibilityPerBUSettings == VisibilityPerBUSettingsType.JobDescription)
                {
                    return JobDescriptionTemplate;
                }
                else if (ci.VisibilityPerBUSettings == VisibilityPerBUSettingsType.JDScope)
                {
                    return JDScopeTemplate;
                }              
                else if (ci.VisibilityPerBUSettings == VisibilityPerBUSettingsType.Hidden)
                {
                    return HiddenTemplate;
                }
                else if (ci.VisibilityPerBUSettings == VisibilityPerBUSettingsType.DynamicColumns)
                {
                    return DynamicTemplate;
                }
               


            }
            return base.SelectTemplate(item, container);
        }
    }
    #endregion
    public enum VisibilityPerBUSettingsType
    {
        Name, Organization, JobDescription, JDScope,Hidden,DynamicColumns
    }
}
