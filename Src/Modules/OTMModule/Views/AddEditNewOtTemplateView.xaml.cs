using DevExpress.Xpf.PdfViewer;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting.Export.Pdf;
using Emdep.Geos.Modules.OTM.CommonClass;

//using PdfiumViewer;
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
using System.Windows.Shapes;
using PdfDocument = DevExpress.XtraPrinting.Export.Pdf.PdfDocument;



namespace Emdep.Geos.Modules.OTM.Views
{
    /// <summary>
    /// Interaction logic for AddEditNewOtTemplateView.xaml
    /// </summary>
    public partial class AddEditNewOtTemplateView : WinUIDialogWindow
    {

        private Point _startPoint; // To store the starting point of the rectangle
        private Rectangle _rectangle; // The rectangle to be drawn
        private bool _isDrawing = false; // To track if drawing is in progress

        public AddEditNewOtTemplateView()
        {
            InitializeComponent();

        }

     
        private void ExcelViewer_DocumentLoaded(object sender, EventArgs e)
        {
            var excelViewer = sender as SpreadsheetControl;
            excelViewer.ActiveViewZoom = 88;
        }

        private void PdfViewer_DocumentLoaded(object sender, RoutedEventArgs e)
        {
            var pdfViewer = sender as PdfViewerControl;

            OTMCommon.Instance.PdfViewer = pdfViewer;


        }

       
    }
}
