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

namespace Emdep.Geos.Modules.SCM.Views
{
    /// <summary>
    /// Interaction logic for AddAttachmentsFilesInConnectorView.xaml
    /// </summary>
    public partial class AddAttachmentsFilesInConnectorView : WinUIDialogWindow
    {
        public AddAttachmentsFilesInConnectorView()
        {
            InitializeComponent();
        }

        private void ComboBoxEdit_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }

        private void ComboBoxEdit_PreviewDrop(object sender, DragEventArgs e)
        {
            string[] fileloadup = (string[])e.Data.GetData(DataFormats.FileDrop);//Get the filename including path
            ((Emdep.Geos.Modules.SCM.ViewModels.AddAttachmentsFilesInConnectorViewModel)((System.Windows.FrameworkElement)sender).DataContext).FileUploadDargDropFromFileExplorer = fileloadup;
            e.Handled = true;//This is file being dropped not copied text so we handle it

        }
    }
}
