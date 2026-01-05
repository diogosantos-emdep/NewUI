using DevExpress.Xpf.Grid;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class MultipleCellEditHelperSAMWorkOrder
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(MultipleCellEditHelperSAMWorkOrder), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));
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

       
        private static string progress;
        public static string Progress
        {
            get { return progress; }
            set { progress = value; }
        }

        private static DateTime expectedStartDate;
        public static DateTime ExpectedStartDate
        {
            get { return expectedStartDate; }
            set { expectedStartDate = value; }
        }
        private static DateTime expectedEndDate;
        public static DateTime ExpectedEndDate
        {
            get { return expectedEndDate; }
            set { expectedEndDate = value; }
        }
        private static bool isChecked;
        public static bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public static readonly DependencyProperty IsValueChangedProperty =
            DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(MultipleCellEditHelperSAMWorkOrder), new PropertyMetadata(false));
        public static List<int> MmodifiedRowHandles = new List<int>();

     
        private static bool isValueChanged;

        public static bool IsValueChanged
        {
            get { return isValueChanged; }
            set { isValueChanged = value; }
        }

        public static string Checkview { get; set; }

        public static string strviewmodulname { get; set; }
        private static TableView viewtableview;
        public static TableView Viewtableview
        {
            get { return viewtableview; }
            set
            {
                viewtableview = value;
            }
        }

        static void View_CellValueChanged1(object sender, CellValueChangedEventArgs e)
        {
            MmodifiedRowHandles.Add(e.RowHandle);
           
        }

        static void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 46))
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


                        ///----------[Sprint-83] [GEOS2-2372]  [19-06-2020] [sjadhav]---------
                        
                        if (columnName == "Progress")
                        {
                            ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                            ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                            Progress = e.Value.ToString();
                            GeosApplication.Instance.MainOtsList[row].IsUpdatedRow = true;
                            GeosApplication.Instance.MainOtsList[row].Progress = Convert.ToByte(Progress);
                        }
                        else if (columnName == "PlannedStartDate")
                        {

                            ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                            ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                            if (e.Value != null)
                                ExpectedStartDate = Convert.ToDateTime(e.Value);
                            //else
                            //    ExpectedStartDate = null;
                            GeosApplication.Instance.MainOtsList[row].IsUpdatedRow = true;
                            GeosApplication.Instance.MainOtsList[row].ExpectedStartDate = ExpectedStartDate;
                        }
                        else if (columnName == "PlannedEndDate")
                        {
                            ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                            ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                            if (e.Value != null)
                                ExpectedEndDate = Convert.ToDateTime(e.Value);
                            //else
                            //    ExpectedEndDate = null;
                            GeosApplication.Instance.MainOtsList[row].IsUpdatedRow = true;
                            GeosApplication.Instance.MainOtsList[row].ExpectedEndDate = ExpectedEndDate;
                        }
                        else
                        {
                            ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                        }

                        ((GridControl)view.DataControl).RefreshRow(row);
                    }

                    SetIsValueChanged(view, true);
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
