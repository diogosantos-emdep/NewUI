using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Logging;

namespace Emdep.Geos.UI.Helper
{
    public class MultipleCellEditHelperSAMWorkPlanning
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(MultipleCellEditHelperSAMWorkPlanning), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));
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
            //TreeListView view = source as TreeListView;
            //view.CellValueChanging += view_CellValueChanging;

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
        private static string remarks;
        public static string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
        private static bool isChecked;
        public static bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public static readonly DependencyProperty IsValueChangedProperty =
            DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(MultipleCellEditHelperSAMWorkPlanning), new PropertyMetadata(false));
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

        static void view_CellValueChanging(object sender, CellValueEventArgs e)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method view_CellValueChanging...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 46))
                {
                    IsValueChanged = true;
                    TableView view = sender as TableView;
                    Checkview = view.Name;
                    Viewtableview = view;

                    List<GridCell> cells = view.GetSelectedCells().ToList<GridCell>();
                    List<GridCell> selectedCells = cells.Where(c => c.Column == e.Column).ToList<GridCell>();
                    bool isAnyCellValueUpdated = false;
                   
                    // e.Source.PostEditor();
                    //  view.PostEditor();

                    foreach (GridCell cell in selectedCells)
                    {
                        int row = cell.RowHandle;
                        string columnName = cell.Column.FieldName;
                        bool existingValue = (bool)((GridControl)view.DataControl).GetCellValue(row, "IsChecked");

                        if (existingValue == false)
                        {
                            ((GridControl)view.DataControl).SetCellValue(row, "IsChecked", true);
                            IsChecked = true;
                            isAnyCellValueUpdated = true;
                        }

                        //((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);

                      //  ((GridControl)view.DataControl).RefreshRow(row);
                    }

                    if (isAnyCellValueUpdated) SetIsValueChanged(view, true);
                }
                else
                {
                    //e.Value = ((DevExpress.Xpf.Grid.CellValueChangedEventArgs)e).OldValue;
                    //e.Handled = true;
                }

                GeosApplication.Instance.Logger.Log("Method view_CellValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in view_CellValueChanging() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

    }
}
