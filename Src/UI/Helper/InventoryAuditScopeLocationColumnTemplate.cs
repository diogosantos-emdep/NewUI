using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
   public class InventoryAuditScopeLocationColumnTemplate : DataTemplateSelector
    {
        public DataTemplate IsChecked { get; set; }
        public DataTemplate Delete { get; set; }
        public DataTemplate Default { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;
            if (ci != null)
            {
                if (ci.ColumnFieldName == "CheckBox")
                {
                    return IsChecked;
                }
                else if (ci.ColumnFieldName == "Delete")
                {
                    return Delete;
                }
                else if (ci.ColumnFieldName == "IdLocation")
                {
                    return Default;
                }
                else
                {
                    return Default;
                }
            }

            return base.SelectTemplate(item, container);
        }  

    }
}
