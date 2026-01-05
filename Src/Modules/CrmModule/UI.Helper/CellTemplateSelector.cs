using DevExpress.Xpf.Grid;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.Modules.Crm.UI.Helper
{
    public class CellTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            GridCellData cellData = item as GridCellData;
            TableView view = cellData.View as TableView;
            // CellsAreaItem cell = (CellsAreaItem)item;
            if (cellData != null && view != null)
            {

                return (DataTemplate)view.Grid.FindResource(".");
                // return (DataTemplate)view.Grid.FindResource("ImageColumnTemplate");

            }

            return base.SelectTemplate(item, container);

        }
    }

    public class CustomCellValue
    {
        public string ForegroundColor { get; set; }
        public string BackgroundColor { get; set; }
        public string FontValue { get; set; }
        public string TextValue { get; set; }
        public TextAlignment TextAlignment { get; set; }

    }
}
