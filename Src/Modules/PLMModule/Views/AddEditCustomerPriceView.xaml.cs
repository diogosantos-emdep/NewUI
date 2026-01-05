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

namespace Emdep.Geos.Modules.PLM.Views
{
    /// <summary>
    /// Interaction logic for AddEditCustomerPriceView.xaml
    /// </summary>
    public partial class AddEditCustomerPriceView : WinUIDialogWindow
    {
        public AddEditCustomerPriceView()
        {
            InitializeComponent();
        }

        private void ModuleMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void ArticleMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void DetectionMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private Point mousePoint = new Point(-10000, -10000);
        private int mouseDownRowHandle = GridControl.InvalidRowHandle;
        private void TableView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            TableView view = (TableView)sender;
            if (view.ActiveEditor != null)
                return;

            if (e.LeftButton != MouseButtonState.Pressed || mousePoint == new Point(-10000, -10000) || mouseDownRowHandle == GridControl.InvalidRowHandle)
            {
                mousePoint = new Point(-10000, -10000);
                mouseDownRowHandle = GridControl.InvalidRowHandle;
                return;
            }

            Rect dragRect = new Rect(mousePoint.X - SystemParameters.MinimumHorizontalDragDistance / 2, mousePoint.Y - SystemParameters.MinimumVerticalDragDistance / 2, SystemParameters.MinimumHorizontalDragDistance, SystemParameters.MinimumVerticalDragDistance);
            if (!dragRect.Contains(e.GetPosition(view.Grid)))
                DragDrop.DoDragDrop(view.GetRowElementByRowHandle(mouseDownRowHandle), new DataObject(typeof(TableView), sender), DragDropEffects.Move);
        }


        private void TableView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TableView view = (TableView)sender;
                TableViewHitInfo hi = view.CalcHitInfo((DependencyObject)e.OriginalSource);
                if (hi.InRow)
                {
                    mousePoint = e.GetPosition(view.Grid);
                    mouseDownRowHandle = hi.RowHandle;
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
