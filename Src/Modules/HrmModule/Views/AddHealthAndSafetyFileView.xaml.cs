using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Modules.Hrm.ViewModels;
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

namespace Emdep.Geos.Modules.Hrm.Views
{
    /// <summary>
    /// Interaction logic for AddHealthAndSafetyFileView.xaml
    /// </summary>
    public partial class AddHealthAndSafetyFileView : WinUIDialogWindow
    {
        public AddHealthAndSafetyFileView()
        {
            InitializeComponent();
        }

        private void ProductFileComboBoxEdit_PreviewDrop(object sender, DragEventArgs e)
        {
            string[] fileloadup = (string[])e.Data.GetData(DataFormats.FileDrop);//Get the filename including path
            ((AddHealthAndSafetyFileViewModel)((System.Windows.FrameworkElement)sender).DataContext).HealthAndSafetyFileUploadDargDropFromFileExplorer = fileloadup;
            e.Handled = true;//This is file being dropped not copied text so we handle it
        }

        private void ProductFileComboBoxEdit_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }
    }
}
