using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emdep.Geos.Modules.HarnessPart.Class;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.XtraPrinting;
using System.Windows.Markup;
using System.Globalization;

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for ucGridView.xaml
    /// </summary>
    public partial class ucGridView : UserControl
    {


        public ucGridView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //List<clsgrid> f = new List<clsgrid>();
            //dgvsearch.ItemsSource = null;
            //for (int i = 0; i < 10; i++)
            //{
            //    clsgrid h = new clsgrid(50403, 4, "conn", "black", "Mail", "saled", "W443", 050503, 111);
            //    f.Add(h);
            //}
            //dgvsearch.DataContext = this;
            //dgvsearch.ItemsSource = f;
            //tableView1.IsRowSelected
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("hi");
        }

        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (RowData.GetRowData((DependencyObject)e.OriginalSource) != null)
            {

                int rowHandle = RowData.GetRowData((DependencyObject)e.OriginalSource).RowHandle.Value;

                image.DataContext = dgvsearch.GetCellValue(rowHandle, "ConImage1");
                //pup.IsOpen = true;
                image.Visibility = Visibility.Visible;
                image.Width = 300;
                image.Height = 300;

            }
            else
            {
                // pup.IsOpen = false;
                image.Visibility = Visibility.Hidden;
                image.DataContext = null;
            }


        }

        private void image_MouseLeave(object sender, MouseEventArgs e)
        {
            image.Visibility = Visibility.Hidden;
            image.DataContext = null;
        }

       

        private void dgvsearch_MouseMove(object sender, MouseEventArgs e)
        {
            
            TableViewHitInfo tvhi = tableView1.CalcHitInfo(e.OriginalSource as DependencyObject);
            if (tvhi.InRowCell == true && tvhi.Column.FieldName == "ConImage1")
            {

                if (RowData.GetRowData((DependencyObject)e.OriginalSource) != null)
                {

                    int rowHandle = RowData.GetRowData((DependencyObject)e.OriginalSource).RowHandle.Value;
                    var v = dgvsearch.GetCellValue(rowHandle, "ConImage1");
                    image.DataContext = dgvsearch.GetCellValue(rowHandle, "ConImage1");
                    //pup.IsOpen = true;
                    image.Visibility = Visibility.Visible;
                    image.Width = 300;
                    image.Height = 300;

                }
                else
                {
                    // pup.IsOpen = false;
                    image.Visibility = Visibility.Hidden;
                    image.DataContext = null;
                }
            }
            else
            {
                // pup.IsOpen = false;
                image.Visibility = Visibility.Hidden;
                image.DataContext = null;
            }
        }

        private void dgvsearch1_MouseMove(object sender, MouseEventArgs e)
        {
            TableViewHitInfo tvhi = tableView2.CalcHitInfo(e.OriginalSource as DependencyObject);
            if (tvhi.InRowCell == true && tvhi.Column.FieldName == "ConImage1")
            {

                if (RowData.GetRowData((DependencyObject)e.OriginalSource) != null)
                {

                    int rowHandle = RowData.GetRowData((DependencyObject)e.OriginalSource).RowHandle.Value;
                    var v = dgvsearch1.GetCellValue(rowHandle, "ConImage1");
                    image.DataContext = dgvsearch.GetCellValue(rowHandle, "ConImage1");
                    //pup.IsOpen = true;
                    image.Visibility = Visibility.Visible;
                    image.Width = 300;
                    image.Height = 300;

                }
                else
                {
                    // pup.IsOpen = false;
                    image.Visibility = Visibility.Hidden;
                    image.DataContext = null;
                }
            }
            else
            {
                // pup.IsOpen = false;
                image.Visibility = Visibility.Hidden;
                image.DataContext = null;
            }
        }

        private void Gallery_ItemClick(object sender, DevExpress.Xpf.Bars.GalleryItemEventArgs e)
        {
            OpenImageViewPopup(e.Item);
        }

        void OpenImageViewPopup(GalleryItem item)
        {
        //    ShowItemInImageVewer(item);
        //    mainView.Effect = new BlurEffect() { Radius = 3 };
        //    imageViewPopup.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
          
            options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;
           
            options.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
           
            ((TableView)dgvsearch.View).ExportToXls(@"grid.xls", new XlsExportOptionsEx() { ExportType = DevExpress.Export.ExportType.WYSIWYG });


          //view.ExportToXls(@"grid_export.xls", new XlsExportOptionsEx() { ExportType = DevExpress.Export.ExportType.WYSIWYG });
          //Process.Start("grid_export.xls");
         
          //((TableView)dgvsearch.Columns).EditSettings = new TextEditSettings();

            //((TableView)dgvsearch.View).ExportToImage(@"grid.xls", options);
            //DevExpress.XtraPrinting.PdfExportOptions pdfExportOptions = new DevExpress.XtraPrinting.PdfExportOptions();
            //pdfExportOptions.ImageQuality = PdfJpegImageQuality.Highest;
            //pdfExportOptions.ConvertImagesToJpeg = true;
        }

      

        
        //private void dgvsearch_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    var f = dgvsearch.Columns[0].FieldName;
        //      TableViewHitInfo tvhi = tableView1.CalcHitInfo(e.OriginalSource as DependencyObject);
        //      if (tvhi.InRowCell == true && tvhi.Column.FieldName == "ConImage1")
        //      {
        //          tableView1.FocusedRow = dgvsearch.GetRow(tvhi.RowHandle);
        //          var test = tableView1.GetCellElementByRowHandleAndColumn(tvhi.RowHandle, dgvsearch.Columns[0]);
        //      }
        //}

    
    }
    public class ImageContainer1 : ContentControlBase
    {
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (Controller.IsMouseLeftButtonDown)
            {
                var layoutControl = Parent as FlowLayoutControl;
                if (layoutControl != null)
                {
                    Controller.IsMouseEntered = false;
                    layoutControl.MaximizedElement = layoutControl.MaximizedElement == this ? null : this;
                }
            }
        }
        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            if (!double.IsNaN(Width) && !double.IsNaN(Height))
                if (e.NewSize.Width != e.PreviousSize.Width)
                    Height = double.NaN;
                else
                    Width = double.NaN;
        }
    }

    public class MyCellTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            CellEditor cellEditor = container as CellEditor;
            EditGridCellData gridCellData = item as EditGridCellData;
            Border backgroundBorder = cellEditor.Parent as Border;
            bool shouldSetDefaults = true;

            if (gridCellData.Column.FieldName == "Col1")
            {
                string stringValue = gridCellData.Value as string;
                if (stringValue != null && stringValue == "cell_1_4")
                {
                    CustomizeAppearance(backgroundBorder, Brushes.Wheat, Brushes.Black, cellEditor, HorizontalAlignment.Right, 15d);
                    shouldSetDefaults = false;
                }
                if (gridCellData.Value is int)
                {
                    int intValue = (int)gridCellData.Value;
                    if (intValue == 3)
                    {
                        CustomizeAppearance(backgroundBorder, Brushes.Red, Brushes.Black, cellEditor, HorizontalAlignment.Right, 20d);
                        shouldSetDefaults = false;
                    }

                }
            }

            if (gridCellData.Column.FieldName == "Col2")
            {
                string stringValue = gridCellData.Value as string;
                if (stringValue != null && stringValue == "cell_2_1")
                {
                    CustomizeAppearance(backgroundBorder, Brushes.Yellow, Brushes.Black, cellEditor, HorizontalAlignment.Right, 10d);
                    shouldSetDefaults = false;
                }
                if (gridCellData.Value is int)
                {
                    int intValue = (int)gridCellData.Value;
                    if (intValue == 14)
                    {
                        CustomizeAppearance(backgroundBorder, Brushes.Green, Brushes.White, cellEditor, HorizontalAlignment.Center, 6d);
                        shouldSetDefaults = false;
                    }
                }
            }
            if (shouldSetDefaults)
                CustomizeAppearance(backgroundBorder, Brushes.White, Brushes.Black, cellEditor, HorizontalAlignment.Left, 12d);
            return base.SelectTemplate(item, container);
        }
        void CustomizeAppearance(Border backgroundborder, Brush background, Brush foreground, CellEditor cellEditor, HorizontalAlignment alignment, double fontSize)
        {
            background = System.Windows.Media.Brushes.Pink;
            //backgroundborder.Background = System.Windows.Media.Brushes.Black;
            //backgroundborder.SetValue(TextBlock.BackgroundProperty, background);
            //backgroundborder.SetValue(TextBlock.ForegroundProperty, foreground);
            //cellEditor.SetValue(TextEdit.HorizontalAlignmentProperty, alignment);
            cellEditor.SetValue(TextBlock.FontSizeProperty, fontSize);
        }
    }


    public class StateConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string state = (string)value;

            if (state == "open")
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO Not used yet
            return "open";
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }


        public class NameToBrushConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                string input = value as string;
                switch (input)
                {
                    case "John":
                        return Brushes.LightGreen;
                    default:
                        return DependencyProperty.UnsetValue;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotSupportedException();
            }
        }
    }

}
