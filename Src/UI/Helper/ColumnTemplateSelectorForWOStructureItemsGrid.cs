using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class ColumnTemplateSelectorForWOStructureItemsGrid : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnSAM column = (ColumnSAM)item;
            return (DataTemplate)((Control)container).FindResource(column.SettingsWOStructureItemGrid + "ColumnTemplate");
        }
    }

    public enum SettingsTypeWOStructureItemsGrid
    {
        Default, Array, Description,Comment, Drawing
    }
}
