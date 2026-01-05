using DevExpress.Xpf.WindowsUI;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for AddEditAttachmentFileInPLMView.xaml
    /// </summary>
    public partial class AddEditAttachmentFileInPLMView : WinUIDialogWindow
    {
        public AddEditAttachmentFileInPLMView()
        {
            InitializeComponent();
        }

        private void BPLFileComboBoxEdit_PreviewDrop(object sender, DragEventArgs e)
        {
            string[] fileloadup = (string[])e.Data.GetData(DataFormats.FileDrop);//Get the filename including path
            ((Emdep.Geos.Modules.PLM.ViewModels.AddEditAttachmentFileInPLMArticleViewModel)((System.Windows.FrameworkElement)sender).DataContext).FileUploadDargDropFromFileExplorer = fileloadup;
           // FileTextBox.Text = File.ReadAllText(fileloadup[0]);//Load data to textBox2
            e.Handled = true;//This is file being dropped not copied text so we handle it
        }

        private void BPLFileComboBoxEdit_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }

        private void CPLFileComboBoxEdit_PreviewDrop(object sender, DragEventArgs e)
        {
            string[] fileloadup = (string[])e.Data.GetData(DataFormats.FileDrop);//Get the filename including path
            ((Emdep.Geos.Modules.PLM.ViewModels.AddEditAttachmentFileInPLMArticleViewModel)((System.Windows.FrameworkElement)sender).DataContext).FileUploadDargDropFromFileExplorer = fileloadup;
            // FileTextBox.Text = File.ReadAllText(fileloadup[0]);//Load data to textBox2
            e.Handled = true;//This is file being dropped not copied text so we handle it
        }

        private void CPLFileComboBoxEdit_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }
    }
}
