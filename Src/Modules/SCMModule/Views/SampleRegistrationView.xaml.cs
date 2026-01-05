
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for SampleRegistrationView.xaml
    /// </summary>
    public partial class SampleRegistrationView : UserControl
    {
        public string SCM_Docklayout_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "SCM_Docklayout_SettingFilePath.Xml";
        DevExpress.Xpf.Docking.CaptionLocation CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Top;
        List<Key> textableKeys = new List<Key>
    {
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J, Key.K, Key.L,
        Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z,
        Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
        Key.Oem1, Key.Oem2, Key.Oem3, Key.Oem4, Key.Oem5, Key.Oem6, Key.Oem7, Key.Oem8
    };
        public SampleRegistrationView()
        {
            InitializeComponent();
        }

        private void clearbtn_Click(object sender, RoutedEventArgs e)
        {
        //    Emdep.Geos.Modules.SCM.ViewModels.ConnectorViewModel ConnectorViewModel = new Emdep.Geos.Modules.SCM.ViewModels.ConnectorViewModel();
        //    AllCustomData(SCMCommon.Instance.Grid, ConnectorViewModel.CustomListByIdFamily, ConnectorViewModel.CustomList, (Family)ConnectorViewModel.SelectedFamilyList?.FirstOrDefault(), ConnectorViewModel.StandardPropertiedbyFamily);
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Grid g = (Grid)sender;
            if (g.Visibility == Visibility.Visible)
                dropdownButton.Visibility = Visibility.Hidden;
            else
                dropdownButton.Visibility = Visibility.Visible;

        }
        private void cmbFamily_KeyUp(object sender, KeyEventArgs e)
        {
            if (textableKeys.Contains(e.Key))
            {
                // Open the ComboBox popup
                cmbFamily.IsPopupOpen = true;
            }
        }

        private void cmbSubfamily_KeyUp(object sender, KeyEventArgs e)
        {
            if (textableKeys.Contains(e.Key))
            {
                // Open the ComboBox popup
                cmbSubfamily.IsPopupOpen = true;
            }
        }
        private void WidthAdjust(object sender, RoutedEventArgs e)
        {
            DevExpress.Xpf.Grid.GridControl obj = (DevExpress.Xpf.Grid.GridControl)sender;

            foreach (var column in obj.Columns)
            {
                if (column.FieldName == "Family" || column.FieldName == "Terminal")
                {
                    column.Width = CalculateColumnWidthh(obj, column.FieldName);
                }
            }
        }
        private double CalculateColumnWidthh(DevExpress.Xpf.Grid.GridControl gridControl, string fieldName)
        {
            double maxWidth = 0;

            for (int rowHandle = 0; rowHandle < gridControl.VisibleRowCount; rowHandle++)
            {
                var cellValue = gridControl.GetCellValue(rowHandle, fieldName);
                if (fieldName == "Terminal" || fieldName == "Family")
                {
                    if (cellValue == null)
                    {
                        var cellValue1 = fieldName;
                        var formattedText = new FormattedText(
                            cellValue1?.ToString(),
                            CultureInfo.CurrentUICulture,
                            FlowDirection.LeftToRight,
                            new Typeface("Arial"),
                            12,
                            Brushes.Black
                        );

                        maxWidth = Math.Max(maxWidth, formattedText.Width);
                    }
                }
                if (cellValue != null)
                {
                    var formattedText = new FormattedText(
                        cellValue.ToString(),
                        CultureInfo.CurrentUICulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        12,
                        Brushes.Black
                    );

                    maxWidth = Math.Max(maxWidth, formattedText.Width);
                }
            }

            return maxWidth + 40; // Add some padding or adjust as needed
        }
        private void cmbColor_KeyUp(object sender, KeyEventArgs e)
        {
            if (textableKeys.Contains(e.Key))
            {
                // Open the ComboBox popup
                cmbColor.IsPopupOpen = true;
            }
        }

        private void cmbGender_KeyUp(object sender, KeyEventArgs e)
        {
            if (textableKeys.Contains(e.Key))
            {
                // Open the ComboBox popup
                cmbGender.IsPopupOpen = true;
            }
        }
        private void ScrollUpButton_Click(object sender, RoutedEventArgs e)
        {
            scroll.PageUp();
        }

        private void ScrollDownButton_Click(object sender, RoutedEventArgs e)
        {
            scroll.PageDown();
        }
        private void scroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;

            // Check if the user has scrolled to the top
            if (scrollViewer.VerticalOffset == 0)
            {
                downButton.Visibility = Visibility.Visible;
                upButton.Visibility = Visibility.Hidden;
            }

            // Check if the user has scrolled to the bottom
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                downButton.Visibility = Visibility.Hidden;
                upButton.Visibility = Visibility.Visible;
            }
        }
        //private void bUp_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{           
        //    documentContainer.CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Top;
        //}
        //private void bDown_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{     
        //    documentContainer.CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Bottom;
        //}
        //private void bLeft_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{         
        //    documentContainer.CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Left;
        //}
        //private void bRight_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{         
        //    documentContainer.CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Right;
        //}
    }
}
