using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Emdep.Geos.Data.Common.Hrm;


namespace Emdep.Geos.UI.Helper
{
    public class DataGridCellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmailTemplate { get; set; }
        public DataTemplate SkypeTemplate { get; set; }
        public DataTemplate HomePhoneTemplate { get; set; }
        public DataTemplate MobilePhoneTemplate { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {

            if (((DevExpress.Xpf.Grid.GridCellData)item).RowData.Row != null)
            {
                var Template = ((EmployeeContact)((DevExpress.Xpf.Grid.GridCellData)item).RowData.Row).EmployeeContactIdType;
                
                if (Template == 87)
                    return SkypeTemplate;
                else if (Template == 88)
                    return EmailTemplate;
                else if (Template == 89)
                    return HomePhoneTemplate;
                else
                    return MobilePhoneTemplate;

            }

            return SkypeTemplate;

        }
    }
}