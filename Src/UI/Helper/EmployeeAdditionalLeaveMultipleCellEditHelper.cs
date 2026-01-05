using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class EmployeeAdditionalLeaveMultipleCellEditHelper
    {
        public static readonly DependencyProperty IsValueChangedProperty =
           DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(EmployeeAdditionalLeaveMultipleCellEditHelper), new PropertyMetadata(false));
        public static void SetIsValueChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsValueChangedProperty, value);
        }

        public static string Checkview { get; set; }

        private static bool isChecked;
        public static bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        private static bool isValueChanged;

        public static bool IsValueChanged
        {
            get { return isValueChanged; }
            set { isValueChanged = value; }
        }

        private static TableView viewtableview;
        public static TableView Viewtableview
        {
            get { return viewtableview; }
            set
            {
                viewtableview = value;
            }
        }

        private static int  days;
        public static int Days
        {
            get { return days; }
            set { days = value; }
        }

        private static decimal hours;
        public static decimal Hours
        {
            get { return hours; }
            set { hours = value; }
        }

        private static void IsValueChangedPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TableView view = source as TableView;
            view.CellValueChanging += view_CellValueChanging;
        }

        static void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                IsValueChanged = true;
                TableView view = sender as TableView;
                Checkview = view.Name;
                Viewtableview = view;
                //Table View Name = AdditionalReasonTableView

                List<GridCell> cells = view.GetSelectedCells().ToList<GridCell>();
                List<GridCell> selectedCells = cells.Where(c => c.Column == e.Column).ToList<GridCell>();

                view.PostEditor();
                foreach (GridCell cell in selectedCells)
                {
                    int row = cell.RowHandle;
                    string columnName = cell.Column.FieldName;

                    ((GridControl)view.DataControl).SetCellValue(row, "IsChecked", true);
                    IsChecked = true;

                    if (columnName == "Days")
                    {
                        ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                        ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                        Days = Convert.ToInt32(e.Value);
                    }

                    if (columnName == "Hours")
                    {
                        ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                        ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                        Hours = Convert.ToDecimal(e.Value);
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
