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

namespace Emdep.Geos.Modules.PCM.Views
{
    /// <summary>
    /// Interaction logic for AddDetection.xaml
    /// </summary>
    public partial class AddDetectionView : WinUIDialogWindow
    {
        public AddDetectionView()
        {
            InitializeComponent();
        }

        private void layoutImagesItemsSizeChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<Size> e)
        {

        }
        
        private void IncludedDetectionListTableView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }

        private void NotIncludedDetectionItemListTableView_CompleteRecordDragDrop(object sender, DevExpress.Xpf.Core.CompleteRecordDragDropEventArgs e)
        {
            e.Handled = true;
        }
    }
}
