using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using DevExpress.Data;
using DevExpress.Xpf.Grid;

namespace Emdep.Geos.UI.Helper
{
    public static class TreeListControlBehaviors
    {
        public static readonly DependencyProperty BindableSummaryFromDataTableProperty =
            DependencyProperty.RegisterAttached(
                "BindableSummaryFromDataTable",
                typeof(DataTable),
                typeof(TreeListControlBehaviors),
                new PropertyMetadata(null, OnBindableSummaryFromDataTableChanged));

        public static void SetBindableSummaryFromDataTable(DependencyObject element, DataTable value)
        {
            element.SetValue(BindableSummaryFromDataTableProperty, value);
        }

        public static DataTable GetBindableSummaryFromDataTable(DependencyObject element)
        {
            return (DataTable)element.GetValue(BindableSummaryFromDataTableProperty);
        }

        private static void OnBindableSummaryFromDataTableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeListControl treeListControl && e.NewValue is DataTable dataTable)
            {
                treeListControl.TotalSummary.Clear();

                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName == "FullName")
                    {
                        treeListControl.TotalSummary.Add(new TreeListSummaryItem
                        {
                            FieldName = column.ColumnName,
                            SummaryType = SummaryItemType.Count,
                            DisplayFormat = $"Total Count: {{0}}"
                        });
                    }
                }
            }
        }
    }
}
