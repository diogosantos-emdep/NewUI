using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.PCM.Views
{
    /// <summary>
    /// Interaction logic for ProductTypeViewGrid.xaml
    /// </summary>
    public partial class ProductTypeGridView : UserControl
    {
        public ProductTypeGridView()
        {
            InitializeComponent();
        }

        private void OnColumnsGenerated(object sender, RoutedEventArgs e)
        {
            GridColumn c = new GridColumn()
            {
                VisibleIndex = 0,
                CellTemplate = (DataTemplate)this.Resources["temp"],
            };
            rootGridControl_Grid.Columns.Add(c);

            //foreach (GridColumn column in rootGridControl_Grid.Columns)
            //{
            //    switch (column.FieldName)
            //    {
            //        case " ":
            //            column.CellTemplate = (DataTemplate)this.Resources["temp"] as DataTemplate;
            //           // column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            //            break;
            //    }
            //}

        }

        private void ItemListTableView_Grid_DragLeave(object sender, DragEventArgs e)
        {

        }

        //private void rootGridControl_Grid_ColumnsPopulated(object sender, RoutedEventArgs e)
        //{
        //    GridColumn c = new GridColumn()
        //    {
        //        VisibleIndex = 0,
        //        CellTemplate = (DataTemplate)this.Resources["temp"],
        //    };
        //    rootGridControl_Grid.Columns.Add(c);
        //}


    }
}
