using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class ProductTypeArticleViewMultipleCellEditHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ProductTypeArticleViewMultipleCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));

        public static void SetIsEnabled(UIElement element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(UIElement element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        private static void IsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TableView view = source as TableView;
            view.CellValueChanging += view_CellValueChanging;
        }

        public static bool GetIsValueChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsValueChangedProperty);
        }

        public static void SetIsValueChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsValueChangedProperty, value);
        }

        private static bool isValueChanged;

        public static bool IsValueChanged
        {
            get { return isValueChanged; }
            set { isValueChanged = value; }
        }

        public static string Checkview { get; set; }

        private static TableView viewtableview;
        public static TableView Viewtableview
        {
            get { return viewtableview; }
            set
            {
                viewtableview = value;
            }
        }

        private static bool isChecked;
        public static bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        private static int idStatus;
        public static int IdStatus
        {
            get { return idStatus; }
            set { idStatus = value; }
        }

        public static readonly DependencyProperty IsValueChangedProperty =
          DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(ProductTypeArticleViewMultipleCellEditHelper), new PropertyMetadata(false));

        public static List<int> MmodifiedRowHandles = new List<int>();

        static void View_CellValueChanged1(object sender, CellValueChangedEventArgs e)
        {
            MmodifiedRowHandles.Add(e.RowHandle);

        }

        static void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                IsValueChanged = true;
                TableView view = sender as TableView;
                Checkview = view.Name;
                Viewtableview = view;

                List<GridCell> cells = view.GetSelectedCells().ToList<GridCell>();
                List<GridCell> selectedCells = cells.Where(c => c.Column == e.Column).ToList<GridCell>();

                view.PostEditor();

                foreach (GridCell cell in selectedCells)
                {
                    int row = cell.RowHandle;
                    string columnName = cell.Column.FieldName;

                    ((GridControl)view.DataControl).SetCellValue(row, "IsChecked", true);
                    IsChecked = true;

                    if (columnName == "IdPCMStatus")
                    {
                        ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                        ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                        IdStatus = Convert.ToInt32(e.Value);
                    }

                    else
                    {
                        ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                    }

                    ((GridControl)view.DataControl).RefreshRow(row);
                }

                SetIsValueChanged(view, true);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
