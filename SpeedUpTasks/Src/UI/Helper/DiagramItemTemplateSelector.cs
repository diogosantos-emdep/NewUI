using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
   public class DiagramItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DepartmentTemplate { get; set; }
        public DataTemplate EmployeeTemplate { get; set; }
        public DataTemplate JDChildParentTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            EmployeeHeirarchy rootItem = item as EmployeeHeirarchy;
            if (rootItem != null)
            {
                if (rootItem.ID.StartsWith("PD") || rootItem.ID.StartsWith("CD") || rootItem.ID.StartsWith("ID") || rootItem.ID.StartsWith("D")|| rootItem.ID.StartsWith("JD"))
                    return DepartmentTemplate;
                if(rootItem.ID.StartsWith("EMP"))
                    return EmployeeTemplate;
                if (rootItem.ID.StartsWith("EG") || rootItem.ID.StartsWith("JG"))
                    return JDChildParentTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
