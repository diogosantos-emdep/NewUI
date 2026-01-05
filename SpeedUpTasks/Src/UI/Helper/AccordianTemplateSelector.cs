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
    public class AccordianTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmployeeDataTemplate { get; set; }
        public DataTemplate DepartmentDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Employee)
                return EmployeeDataTemplate;

            if (item is Department)
                return DepartmentDataTemplate;

            return DepartmentDataTemplate;
        }
    }
}
