using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class WarehouseInventoryDiffTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            GridCellData data = (GridCellData)item;
            PropertyDescriptor property = TypeDescriptor.GetProperties(data.Data)["RemaningQtySign"];
            if (property == null)
                return null;
            string editorType = property.GetValue(data.Data) as string;
            if (editorType == "Zero")
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("ZeroTemplate");
            }
            if (editorType == "Plus")
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("PlusTemplate");
            }
            if (editorType == "Minus")
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("MinusTemplate");
            }

            return null;
        }
    }
}
