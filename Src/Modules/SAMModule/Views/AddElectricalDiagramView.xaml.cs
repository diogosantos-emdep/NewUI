using DevExpress.Xpf.PdfViewer;
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

namespace Emdep.Geos.Modules.SAM.Views
{
    /// <summary>
    /// Interaction logic for AddElectricalDiagram.xaml
    /// </summary>
    public partial class AddElectricalDiagramView : WinUIDialogWindow
    {
        public AddElectricalDiagramView()
        {
            InitializeComponent();
        }
        private void PdfViewer_DocumentLoaded(object sender, RoutedEventArgs e)
        {
            var pdfViewer = sender as PdfViewerControl;

            SAMCommon.Instance.PdfViewer = pdfViewer;


        }
    }
}
