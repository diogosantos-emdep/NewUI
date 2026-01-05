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
    public class EmployeeDetailsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmployeeDetailsTemplate { get; set; }
        public DataTemplate DepartmentEmployeeDetailsTemplate { get; set; }
        public DataTemplate EmptyDetailsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Employee)
                return DepartmentEmployeeDetailsTemplate;
            if (item is Department)
                return DepartmentEmployeeDetailsTemplate;
            return EmptyDetailsTemplate;
        }
    }
}
