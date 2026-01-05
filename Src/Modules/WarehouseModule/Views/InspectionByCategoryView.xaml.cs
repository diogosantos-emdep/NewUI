using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
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

namespace Emdep.Geos.Modules.Warehouse.Views
{
    /// <summary>
    /// Interaction logic for InspectionByCategoryView.xaml
    /// </summary>
    public partial class InspectionByCategoryView : WinUIDialogWindow
    {
        public InspectionByCategoryView()
        {
            InitializeComponent();
            GridControl.AllowInfiniteGridSize = true;
        }

        private void View_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void reworkcausesgrid_Loaded(object sender, RoutedEventArgs e)
        {
            reworkcausesgrid.Height = this.ActualHeight - 170;
            ReworkCausesByCatGrid.Height = this.ActualHeight - 170;   
        }
    }
}
