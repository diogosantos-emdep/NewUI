using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class VisibilityPerBUViewMultipleCellEditHelper
    {
        #region//[Shweta.thube][GEOS2-6696]
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(VisibilityPerBUViewMultipleCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));
        private static bool isValueChanged;
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
        private static bool isLoad;
        public static bool IsLoad
        {
            get { return isLoad; }
            set { isLoad = value; }
        }
        public static bool IsValueChanged
        {
            get { return isValueChanged; }
            set
            {
                if (!GeosApplication.Instance.IsAPMActionPlanPermission)
                {
                    isValueChanged = false;
                }
                else
                {
                    isValueChanged = value;
                }
            }
        }
        public static string Checkview { get; set; }

        public static TableView viewtableview;
        public static TableView Viewtableview
        {
            get { return viewtableview; }
            set
            {
                viewtableview = value;
                Clonedviewtableview = viewtableview;
            }
        }
        private static TableView clonedviewtableview;
        public static TableView Clonedviewtableview
        {
            get { return clonedviewtableview; }
            set
            {
                clonedviewtableview = value;
            }
        }

        private static string tAU;

        public static string TAU
        {
            get { return tAU; }
            set { tAU = value; }
        }
        private static string electricTestBoards;

        public static string ElectricTestBoards
        {
            get { return electricTestBoards; }
            set { electricTestBoards = value; }
        }
        private static string engineering;

        public static string Engineering
        {
            get { return engineering; }
            set { engineering = value; }
        }
        public static string assembly;

        public static string Assembly
        {
            get { return assembly; }
            set { assembly = value; }
        }
        public static string advanced;

        public static string Advanced
        {
            get { return advanced; }
            set { advanced = value; }
        }
        private static bool isChecked;
        public static bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
        public static readonly DependencyProperty IsValueChangedProperty =
         DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(VisibilityPerBUViewMultipleCellEditHelper), new PropertyMetadata(false));
        public static void SetIsValueChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsValueChangedProperty, value);
        }

        static void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                TableView view = sender as TableView;
                Checkview = view.Name;
                Viewtableview = view;

                List<GridCell> cells = view.GetSelectedCells().ToList<GridCell>();
                List<GridCell> selectedCells = cells.Where(c => c.Column == e.Column).ToList<GridCell>();
                IList<GridRowInfo> newRowData = view.GetSelectedRows();

                view.PostEditor();

                if (e.Cell.Row != null)
                {
                    IsValueChanged = true;
                    if (IsValueChanged)
                    {
                        foreach (GridCell cell in selectedCells)
                        {
                            int row = cell.RowHandle;
                            string columnName = cell.Column.FieldName;
                            ((GridControl)view.DataControl).SetCellValue(row,"IsChecked", true);

                            IsChecked = true;      
                                                                              
                            if (columnName == "TAU")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row,"IsUpdatedRow", true);
                                TAU = Convert.ToString(e.Value);
                                foreach (var item in newRowData)
                                {
                                    var dataRow = (System.Data.DataRowView)item.Row;
                                    var visibilityInfo = dataRow["YesNoList"] as List<string>;
                                    ((System.Data.DataRowView)item.Row)["TAU"] = visibilityInfo.FirstOrDefault(a => a == TAU);                                    
                                }
                            }
                            else if (columnName == "ElectricTestBoards")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                ElectricTestBoards = Convert.ToString(e.Value);
                                foreach (var item in newRowData)
                                {
                                    var dataRow = (System.Data.DataRowView)item.Row;
                                    var visibilityInfo = dataRow["YesNoList"] as List<string>;
                                    ((System.Data.DataRowView)item.Row)["ElectricTestBoards"] = visibilityInfo.FirstOrDefault(a => a == ElectricTestBoards);
                                }
                            }
                            else if (columnName == "Engineering")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Engineering = Convert.ToString(e.Value);
                                foreach (var item in newRowData)
                                {
                                    var dataRow = (System.Data.DataRowView)item.Row;
                                    var visibilityInfo = dataRow["YesNoList"] as List<string>;
                                    ((System.Data.DataRowView)item.Row)["Engineering"] = visibilityInfo.FirstOrDefault(a => a == Engineering);
                                }
                            }
                            else if (columnName == "Assembly")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Assembly = Convert.ToString(e.Value);
                                foreach (var item in newRowData)
                                {
                                    var dataRow = (System.Data.DataRowView)item.Row;
                                    var visibilityInfo = dataRow["YesNoList"] as List<string>;
                                    ((System.Data.DataRowView)item.Row)["Assembly"] = visibilityInfo.FirstOrDefault(a => a == Assembly);
                                }
                            }
                            else if (columnName == "Advanced")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Advanced = Convert.ToString(e.Value);
                                foreach (var item in newRowData)
                                {
                                    var dataRow = (System.Data.DataRowView)item.Row;
                                    var visibilityInfo = dataRow["YesNoList"] as List<string>;
                                    ((System.Data.DataRowView)item.Row)["Advanced"] = visibilityInfo.FirstOrDefault(a => a == Advanced);
                                }
                            }
                            else
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                            }
                            ((GridControl)view.DataControl).RefreshRow(row);
                            ((GridControl)view.DataControl).UpdateLayout();
                        }
                        SetIsValueChanged(view, true);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
