using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.UI.Helper;
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
    /// Interaction logic for EditProductTypeView.xaml
    /// </summary>
    public partial class EditProductTypeView : WinUIDialogWindow
    {
        public EditProductTypeView()
        {
            InitializeComponent();
        }

        private void OptionMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void OptionTreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void DetectionMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void DetectionTreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void ModuleMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void ArticleMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void gridCustomers_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {

        }

        private void CustomerTableView_ColumnHeaderClick(object sender, DevExpress.Xpf.Grid.ColumnHeaderClickEventArgs e)
        {

        }
    }
}
