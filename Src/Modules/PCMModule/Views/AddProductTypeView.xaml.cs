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
    /// Interaction logic for AddProductTypeView.xaml
    /// </summary>
    public partial class AddProductTypeView : WinUIDialogWindow
    {
        public AddProductTypeView()
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

        private void SparePartMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void DetectionTreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void layoutImagesItemsSizeChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<Size> e)
        {

        }

        private void ModuleMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void ArticleMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void IncludedModuleListTableView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void NotIncludedModuleItemListTableView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

    }
}
