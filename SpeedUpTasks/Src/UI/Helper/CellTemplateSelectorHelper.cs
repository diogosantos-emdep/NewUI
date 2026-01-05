using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class CellTemplateSelectorHelper : DataTemplateSelector
    {
        public DataTemplate template1 { get; set; }
        public DataTemplate template2 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            EditGridCellData editGridCellData = item as EditGridCellData;

            if (editGridCellData != null && editGridCellData.Column.FieldName == "Month")
            {
                return template1;
            }


            return base.SelectTemplate(item, container);
        }

        //public override DataTemplate SelectTemplate(object item, DependencyObject container)
        //{
        //    GridCellData cellData = item as GridCellData;
        //    TableView view = cellData.View as TableView;
        //    // CellsAreaItem cell = (CellsAreaItem)item;
        //    if (cellData != null && view != null)
        //    {

        //        //return (DataTemplate)view.Grid.FindResource("TextColumnTemplate");
        //        // return (DataTemplate)view.Grid.FindResource("ImageColumnTemplate");

        //    }

        //    return base.SelectTemplate(item, container);

        //}
    }
}
