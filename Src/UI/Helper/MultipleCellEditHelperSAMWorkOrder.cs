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
            TreeListView view = source as TreeListView;
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
        private static TreeListView viewtableview;
        public static TreeListView Viewtableview
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

        static void view_CellValueChanging(object sender, TreeListCellValueEventArgs e)
        {
            try
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 46))
                {
                    IsValueChanged = true;
                    TreeListView view = sender as TreeListView;
                    Checkview = view.Name;
                    Viewtableview = view;

                    List <TreeListCell> cells = view.GetSelectedCells().ToList<TreeListCell>();
                    List<TreeListCell> selectedCells = cells.Where(c => c.Column == e.Column).ToList<TreeListCell>();
                    TreeListNode cells1 = view.FocusedNode;

                    view.PostEditor();
                    foreach (TreeListCell cell in selectedCells)
                    {
                        int row = cell.RowHandle;
                        string columnName = cell.Column.FieldName;

                        ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), "IsChecked", true);
                        IsChecked = true;


                        ///----------[Sprint-83] [GEOS2-2372]  [19-06-2020] [sjadhav]---------
                        
                        if (columnName == "Progress")
                        {
                            ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), columnName, e.Value);
                            ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), "IsUpdatedRow", true);
                            Progress = e.Value.ToString();
                            GeosApplication.Instance.MainOtsList.Where(x => x.IdOT == Convert.ToInt64(((System.Data.DataRowView)e.Cell.Row).Row.ItemArray[0])).ToList().ForEach(data => { data.IsUpdatedRow = true; data.Progress = Convert.ToByte(Progress); });
                        }
                        else if (columnName == "PlannedStartDate")
                        {
                            ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), columnName, e.Value);
                            ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), "IsUpdatedRow", true);
                            if (e.Value != null && e.Value != DBNull.Value)
                                ExpectedStartDate = Convert.ToDateTime(e.Value);
                            else
                                ExpectedStartDate = DateTime.MinValue;
                            GeosApplication.Instance.MainOtsList.Where(x => x.IdOT == Convert.ToInt64(((System.Data.DataRowView)e.Cell.Row).Row.ItemArray[0])).ToList().ForEach(data => { data.IsUpdatedRow = true; data.ExpectedStartDate = ExpectedStartDate; });
                        }
                        else if (columnName == "PlannedEndDate")
                        {
                            ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), columnName, e.Value);
                            ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), "IsUpdatedRow", true);
                            if (e.Value != null && e.Value != DBNull.Value)
                                ExpectedEndDate = Convert.ToDateTime(e.Value);
                            else
                                ExpectedEndDate = DateTime.MinValue;
                            GeosApplication.Instance.MainOtsList.Where(x => x.IdOT == Convert.ToInt64(((System.Data.DataRowView)e.Cell.Row).Row.ItemArray[0])).ToList().ForEach(data => { data.IsUpdatedRow = true; data.ExpectedEndDate = ExpectedEndDate; });
                        }
                        else if (columnName == "Remarks")
                        {
                            ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), columnName, e.Value);
                            ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), "IsUpdatedRow", true);
                            if (e.Value != null && e.Value != DBNull.Value)
                                Remarks = Convert.ToString(e.Value);
                            else
                                Remarks = string.Empty;
                            GeosApplication.Instance.MainOtsList.Where(x => x.IdOT == Convert.ToInt64(((System.Data.DataRowView)e.Cell.Row).Row.ItemArray[0])).ToList().ForEach(data => { data.IsUpdatedRow = true; data.Observations = Remarks; });
                        }
                        else
                        {
                            ((TreeListControl)view.DataControl).View.SetNodeValue(view.GetNodeByRowHandle(row), columnName, e.Value);
                        }

                        ((TreeListControl)view.DataControl).RefreshRow(row);
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
