using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class DetectionsViewMultipleCellEditHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(DetectionsViewMultipleCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));

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

        private static int idDetection;
        public static int IdDetection
        {
            get { return idDetection; }
            set { idDetection = value; }
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

        private static string htmlColor;
        public static string HtmlColor
        {
            get { return htmlColor; }
            set { htmlColor = value; }
        }

        private static string code;
        public static string Code
        {
            get { return code; }
            set { code = value; }
        }
        private static string testType;
        public static string TestType
        {
            get { return testType; }
            set { testType = value; }
        }

        private static string weldorder;
        public static string WeldOrder
        {
            get { return weldorder; }
            set { weldorder = value; }
        }
        private static string description;
        public static string Description
        {
            get { return description; }
            set { description = value; }
        }
        private static string groupName;
        public static string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }
        private static string detectionName;
        public static string DetectionName
        {
            get { return detectionName; }
            set { detectionName = value; }
        }
        public static readonly DependencyProperty IsValueChangedProperty =
          DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(DetectionsViewMultipleCellEditHelper), new PropertyMetadata(false));

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

                            if (columnName == "ECOSVisibilityValue")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                IdECOS = (int)((Emdep.Geos.Data.Common.PCM.DetectionDetails)e.Cell.Row).ECOSVisibilityList.Where(i => i.Value.ToLower() == e.Value.ToString().ToLower()).FirstOrDefault().IdLookupValue;

                                if (IdECOS == 326)
                                {
                                    foreach (var item in newRowData)
                                    {
                                        ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).ECOSVisibilityHTMLColor = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == IdECOS).HtmlColor;
                                        ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).PurchaseQtyMax = 0;
                                        ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).PurchaseQtyMin = 0;
                                    }
                                    Min = 0;
                                    Max = 0;
                                }
                                else
                                {
                                    foreach (var item in newRowData)
                                    {
                                        ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).ECOSVisibilityHTMLColor = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).ECOSVisibilityList.FirstOrDefault(a => a.IdLookupValue == IdECOS).HtmlColor;
                                        if (((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).PurchaseQtyMax == 0 || ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).PurchaseQtyMin == 0)
                                        {
                                            ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).PurchaseQtyMax = 1;
                                            ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).PurchaseQtyMin = 1;
                                        }
                                    }
                                }
                            }
                            else if (columnName == "PCMStatus")
                            {
                           
                                //((GridControl)view.DataControl).SetCellValue(cell.RowHandle, columnName, cell.Value);
                                ((GridControl)view.DataControl).SetCellValue(cell.RowHandle, "IsUpdatedRow", true);
                                string s = Convert.ToString(e.Value);
                                foreach (var item in newRowData)
                                {
                                    ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).StatusHTMLColor = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).StatusList.FirstOrDefault(a => a.Value == s).HtmlColor;
                                    ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).Status = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).StatusList.FirstOrDefault(a => a.Value == s);
                                    ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).PCMStatus = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)item.Row).StatusList.FirstOrDefault(a => a.Value == s).Value;
                                }
                            }

                            else if (columnName == "Description")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Description = Convert.ToString(e.Value);
                                Description = Description.Trim(' ', '\r');
                            }
                            else if (columnName == "WeldOrder")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                WeldOrder = Convert.ToString(e.Value);
                                WeldOrder = WeldOrder.Trim(' ', '\r');
                            }
                            else if (columnName == "TestTypes.Name")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                TestType = Convert.ToString(e.Value);
                                TestType = TestType.Trim(' ', '\r');
                            }
                            else if (columnName == "DetectionGroup.Name")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                GroupName = Convert.ToString(e.Value);
                                GroupName = GroupName.Trim(' ', '\r');
                                if (((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup == null)
                                    ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup = new DetectionGroup();

                                ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroupList.Where(i=>i.OriginalName == e.Value.ToString()).FirstOrDefault();
                                GroupName = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup.Name;
                            }
                            else if (columnName == "Code")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                Code = Convert.ToString(e.Value);
                                Code = Code.Trim(' ', '\r');
                            }
                            else if (columnName == "DetectionTypes.Name")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                DetectionName = Convert.ToString(e.Value);
                                DetectionName = DetectionName.Trim(' ', '\r');

                                //[rdixit][GEOS2-3970][01.12.2022] 
                                DetectionDetails selectedrow = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem);
                                int detectionId = (int)((Emdep.Geos.Data.Common.PCM.DetectionDetails)e.Cell.Row).TestList.Where(i => i.Name == selectedrow.DetectionTypes.Name).FirstOrDefault().IdDetectionType;

                                ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroupList = selectedrow.DetectionAllGroupList.Where(j => j.IdDetectionType == detectionId).ToList();

                                if (((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup == null)
                                    ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup = new DetectionGroup();

                                if (((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroupList != null && ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroupList.Count > 0)
                                {                                   
                                    ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup.Name = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroupList.FirstOrDefault().OriginalName;
                                    GroupName = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup.Name;
                                }
                                else
                                {
                                    ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup.Name = "";
                                    GroupName = ((Emdep.Geos.Data.Common.PCM.DetectionDetails)((GridControl)view.DataControl).SelectedItem).DetectionGroup.Name;
                                }
                            }
                            else
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                            }                                                        
                        }
                        newRowData.ToList().ForEach(i => ((GridControl)view.DataControl).RefreshRow(i.RowHandle));                        
                        ((GridControl)view.DataControl).UpdateLayout();
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
