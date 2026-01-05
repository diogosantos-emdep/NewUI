using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.Warehouse.ViewModels;
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
    /// Interaction logic for Warehouseview.xaml
    /// </summary>
    public partial class ScheduleEventsView : UserControl
    {
        private int _savedTopRowIndex;
        private int _savedFocusedRowHandle;

        public ScheduleEventsView()
        {
            InitializeComponent();
            if (DataContext is ScheduleEventsViewModel viewModel)
            {
                viewModel.RequestScrollPreservation += SaveScrollPosition;
                viewModel.RequestScrollRestoration += RestoreScrollPosition;
            }
        }
        private void SaveScrollPosition()
        {
            var gridView = gridControl.View as TableView;
            if (gridView != null)
            {
                _savedTopRowIndex = gridView.TopRowIndex;
                _savedFocusedRowHandle = gridView.FocusedRowHandle;
            }
        }

        private void RestoreScrollPosition()
        {
            var gridView = gridControl.View as TableView;
            if (gridView != null)
            {
                gridControl.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        // Restore the scroll position and focused row
                        gridView.TopRowIndex = _savedTopRowIndex;
                        gridView.FocusedRowHandle = _savedFocusedRowHandle;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error restoring scroll position: {ex.Message}");
                    }
                }), System.Windows.Threading.DispatcherPriority.Render);
            }
        }
    }
}
