using DevExpress.Mvvm.UI;
using DevExpress.Xpf.PdfViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class CustomPdfViewerControlHelper : PdfViewerControl
    {
        public static readonly DependencyProperty DefaultFileNameProperty = DependencyProperty.Register(
                    "DefaultFileName",
                    typeof(string),
                    typeof(CustomPdfViewerControlHelper),
                    new FrameworkPropertyMetadata(string.Empty)
               );
        public string DefaultFileName
        {
            get { return (string)GetValue(DefaultFileNameProperty); }
            set { SetValue(DefaultFileNameProperty, value); }
        }

        protected override DevExpress.Mvvm.UI.SaveFileDialogService CreateDefaultSaveFileDialogService()
        {
            return new SaveFileDialogService()
            {
                DefaultExt = PdfViewerLocalizer.GetString(PdfViewerStringId.PdfFileExtension),
                Filter = PdfViewerLocalizer.GetString(PdfViewerStringId.PdfFileFilter),
                DefaultFileName = DefaultFileName
            };
        }
    }
}
