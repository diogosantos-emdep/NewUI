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
    /// Interaction logic for AddEditBasePriceView.xaml
    /// </summary>
    public partial class AddEditBasePriceView : WinUIDialogWindow
    {
        public AddEditBasePriceView()
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

        private void GridColumn_ToolTipOpening(object sender, ToolTipEventArgs e)
        {

        }

        private void GridColumn_ToolTipClosing(object sender, ToolTipEventArgs e)
        {

        }

        private void ArticleTableView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = false;
        }

        private void ArticleTableView_ContinueRecordDrag(object sender, DevExpress.Xpf.Core.ContinueRecordDragEventArgs e)
        {
            e.Action = DragAction.Cancel;
            e.Handled = true;
        }

        private void DetectionTableView_ContinueRecordDrag(object sender, DevExpress.Xpf.Core.ContinueRecordDragEventArgs e)
        {
            e.Action = DragAction.Cancel;
            e.Handled = true;
        }
        private void ModuleTableView_ContinueRecordDrag(object sender, DevExpress.Xpf.Core.ContinueRecordDragEventArgs e)
        {
            e.Action = DragAction.Cancel;
            e.Handled = true;
        }
    }
}
