using System;
using System.Collections.Generic;
using DevExpress.Xpf.WindowsUI;
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
    /// Interaction logic for AddEditDiscountsListGridView.xaml
    /// </summary>
    public partial class AddEditDiscountsListGridView : WinUIDialogWindow
    {
        public AddEditDiscountsListGridView()
        {
            InitializeComponent();
        }

        private void ArticleMenutreeListView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void ArticleTableView_ContinueRecordDrag(object sender, DevExpress.Xpf.Core.ContinueRecordDragEventArgs e)
        {
            e.Action = DragAction.Cancel;
            e.Handled = true;
        }
    }
}
