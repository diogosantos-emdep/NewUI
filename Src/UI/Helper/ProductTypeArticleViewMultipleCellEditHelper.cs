using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class ProductTypeArticleViewMultipleCellEditHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ProductTypeArticleViewMultipleCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));

        private static bool isLoad;
        public static bool IsLoad
        {
            get { return isLoad; }
            set { isLoad = value; }
        }
        
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
            set
            {
                if (!GeosApplication.Instance.IsPermissionReadOnlyForPCM)
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

        private static string status;
        public static string Status
        {
            get { return status; }
            set { status = value; }
        }

        private static PCMArticleCategory selectedCategory;
        public static PCMArticleCategory SelectedCategory
        {
            get { return selectedCategory; }
            set { selectedCategory = value; }
        }

        private static int min;
        public static int Min
        {
            get { return min; }
            set { min = value; }
        }

        private static int max;
        public static int Max
        {
            get { return max; }
            set { max = value; }
        }
        
        private static int idECOS;
        public static int IdECOS
        {
            get { return idECOS; }
            set { idECOS = value; }
        }

        private static string eCOSVisibilityValue;
        public static string ECOSVisibilityValue
        {
            get { return eCOSVisibilityValue; }
            set { eCOSVisibilityValue = value; }
        }

        private static string htmlColor;
        public static string HtmlColor
        {
            get { return htmlColor; }
            set { htmlColor = value; }
        }

        private static string name;
        public static string Name
        {
            get { return name; }
            set { name = value; }
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
                            ((GridControl)view.DataControl).SetCellValue(row, "IsChecked", true);
                            IsChecked = true;

                            if (columnName == "PCMStatus")
                            {
                                #region [rdixit][15.07.2024][rdixit]
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Status = Convert.ToString(e.Value);
                                foreach (var item in newRowData)
                                {
                                    var dataRow = (System.Data.DataRowView)item.Row;
                                    var StatusList = dataRow.Row["StatusList"] as List<LookupValue>;
                                    ((System.Data.DataRowView)item.Row)["StatusHTMLColor"] = StatusList.FirstOrDefault(a => a.Value == Status).HtmlColor;
                                }
                                #endregion
                            }
                            else if (columnName == "PcmArticleCategory")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                SelectedCategory = (PCMArticleCategory)e.Value;
                            }

                            else if (columnName == "PurchaseQtyMin")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Min = Convert.ToInt32(e.Value);
                            }

                            else if (columnName == "PurchaseQtyMax")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Max = Convert.ToInt32(e.Value);
                            }
                            else if (columnName == "ECOSVisibilityValue")
                            {
                                #region [rdixit][15.07.2024][rdixit]
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                ECOSVisibilityValue = Convert.ToString(e.Value);
                                foreach (var item in newRowData)
                                {
                                    var dataRow = (System.Data.DataRowView)item.Row;
                                    var visibilityInfo = dataRow["ECOSVisibilityList"] as List<LookupValue>;
                                    ((System.Data.DataRowView)item.Row)["IdECOSVisibility"] = visibilityInfo.FirstOrDefault(a => a.Value == ECOSVisibilityValue).IdLookupValue;
                                    ((System.Data.DataRowView)item.Row)["ECOSVisibilityHTMLColor"] = visibilityInfo.FirstOrDefault(a => a.Value == ECOSVisibilityValue).HtmlColor;
                                }
                                #endregion
                            }
                            else if (columnName == "Description")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Name = Convert.ToString(e.Value);
                                Name = Name.Trim(' ', '\r');
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
    }
}
