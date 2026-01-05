using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using System.Collections.ObjectModel;
using Emdep.Geos.Modules.HarnessPart.Class;
using DevExpress.Xpf.Grid;
using DevExpress.XtraGrid;


namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for Attachement.xaml
    /// </summary>
    public partial class Attachement : DXWindow
    {
       // Dictionary<string, bool> selectedValues = new Dictionary<string, bool>();

        public bool? IsAllRowsSelected
        {
            get { return (bool?)GetValue(IsAllRowsSelectedProperty); }
            set { SetValue(IsAllRowsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsAllRowsSelectedProperty =
            DependencyProperty.Register("IsAllRowsSelected", typeof(bool?), typeof(Attachement), new PropertyMetadata(false, (d, e) => ((Attachement)d).OnIsAllRowsSelectedChanged()));
        Dictionary<Guid, bool> selectedValues = new Dictionary<Guid, bool>();
        List<clsHarnessPartVisualAidsDoc> list;
        public Attachement()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<clsHarnessPartVisualAidsDoc> listclsAccessories = new ObservableCollection<clsHarnessPartVisualAidsDoc>();

            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { Id = Guid.NewGuid(), IdHarnessPart =true, CategoryName = "01:Commercial Visual Aid", FileName = "000011.bmp" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { Id = Guid.NewGuid(), IdHarnessPart = true, CategoryName = "02:Wintestem  Visual Aid", FileName = "05551.wtg" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { Id = Guid.NewGuid(), IdHarnessPart = true, CategoryName = "03:Documentation", FileName = "01.bmp" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { Id = Guid.NewGuid(), IdHarnessPart = true, CategoryName = "03:Documentation", FileName = "01.bmp" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { Id = Guid.NewGuid(), IdHarnessPart = true, CategoryName = "03:Documentation", FileName = "01.bmp" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { Id = Guid.NewGuid(), IdHarnessPart = true, CategoryName = "03:Documentation", FileName = "01.pdf" });
            //listclsAccessories.Add(new clsType() { ID = 1, Name = "Way" });
            //listclsAccessories.Add(new clsType() { ID = 2, Name = "Family" });
            gridAttachments.ItemsSource = listclsAccessories;
            list = listclsAccessories.ToList();
          
           // gridAttachments.Columns["CategoryName"].SortOrder = DevExpress.Data.ColumnSortOrder.None;
            gridAttachments.AllowLiveDataShaping=false;
        
            //gridView1.Columns["Country"].SortOrder = DevExpress.Data.ColumnSortOrder.None;

         
        }

        private void chkEdt_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkEdt_Unchecked(object sender, RoutedEventArgs e)
        {

        }

     
        private void gridAttachments_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            //if (e.Column.FieldName == "IdHarnessPart")
            //{
            //    string key = (string)e.GetListSourceFieldValue("CategoryName");
            //    if (e.IsGetData)
            //    {
            //        e.Value = GetIsSelected(key);
            //    }
            //    if (e.IsSetData)
            //    {
            //        SetIsSelected(key, (bool)e.Value);
            //    }
            //}
        }

        //bool GetIsSelected(string key)
        //{
        //    bool isSelected;
        //    if (selectedValues.TryGetValue(key, out isSelected))
        //        return isSelected;
        //    return false;
        //}
        //void SetIsSelected(string key, bool value)
        //{
        //    if (value)
        //        selectedValues[key] = value;
        //    else
        //        selectedValues.Remove(key);
        //}

        //private bool GetSelectedAllChkEdt()
        //{
        //    return false;
        //    //for (int i = 0; i < unbndChk.Count; i++)
        //    //    if (!unbndChk[i])
        //    //        return false;
        //    //return true;
        //}
        //private void SelectAllChkEdt(bool selectAll)
        //{
        //    for (int i = 0; i < gridAttachments.VisibleRowCount; i++)
        //        gridAttachments.SetCellValue(i, "IdHarnessPart", selectAll);


        //    //for (int i = 0; i < unbndChk.Count; i++)
        //    //    unbndChk[i] = selectAll;
        //    //grdCrtriaDtl.RefreshData();
        //}

        private void chkEdt_Checked_1(object sender, RoutedEventArgs e)
        {
           // SelectAllChkEdt(true);
        }

        private void chkEdt_Unchecked_1(object sender, RoutedEventArgs e)
        {
            //SelectAllChkEdt(false);
        }

        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Selected")
            {
                Guid key = (Guid)e.GetListSourceFieldValue("Id");
                if (e.IsGetData)
                {
                    e.Value = GetIsSelected(key);
                }
                if (e.IsSetData)
                {
                    SetIsSelected(key, (bool)e.Value);
                }
            }
        }
        bool GetIsSelected(Guid key)
        {
            bool isSelected;
            if (selectedValues.TryGetValue(key, out isSelected))
                return isSelected;
            return false;
        }
        void SetIsSelected(Guid key, bool value)
        {
            if (value)
                selectedValues[key] = value;
            else
                selectedValues.Remove(key);

            bool allSelected = true;
            bool allUnselected = true;
            foreach (clsHarnessPartVisualAidsDoc item in list)
            {
                if (GetIsSelected(item.Id))
                {
                    allUnselected = false;
                }
                else
                {
                    allSelected = false;
                }
            }
            if (!allSelected && !allUnselected)
                IsAllRowsSelected = null;
            if (allSelected)
                IsAllRowsSelected = true;
            if (allUnselected)
                IsAllRowsSelected = false;

        }
        void OnIsAllRowsSelectedChanged()
        {
            if (IsAllRowsSelected == null)
            {
                return;
            }
            if (IsAllRowsSelected.Value)
            {
                List<int> list = GetDataRowHandles();
                bool newIsSelected = true;
                for (int i = 0; i < list.Count; i++)
                {
                    int rowHandle = gridAttachments.GetRowHandleByListIndex(i);
                    gridAttachments.SetCellValue(rowHandle, "Selected", newIsSelected);
                }
            }
            else
            {
                List<int> list = GetDataRowHandles();
                bool newIsSelected = false;
                for (int i = 0; i < list.Count; i++)
                {
                    int rowHandle = gridAttachments.GetRowHandleByListIndex(i);
                    gridAttachments.SetCellValue(rowHandle, "Selected", newIsSelected);
                }
            }
        }
        private List<int> GetDataRowHandles()
        {
            List<int> rowHandles = new List<int>();
            for (int i = 0; i < gridAttachments.VisibleRowCount; i++)
            {
                int rowHandle = gridAttachments.GetRowHandleByVisibleIndex(i);
                if (gridAttachments.IsGroupRowHandle(rowHandle))
                {
                    if (!gridAttachments.IsGroupRowExpanded(rowHandle))
                    {
                        rowHandles.AddRange(GetDataRowHandlesInGroup(rowHandle));
                    }
                }
                else
                    rowHandles.Add(rowHandle);
            }
            return rowHandles;
        }
        private List<int> GetDataRowHandlesInGroup(int groupRowHandle)
        {
            List<int> rowHandles = new List<int>();
            for (int i = 0; i < gridAttachments.GetChildRowCount(groupRowHandle); i++)
            {
                int rowHandle = gridAttachments.GetChildRowHandle(groupRowHandle, i);
                if (gridAttachments.IsGroupRowHandle(rowHandle))
                {
                    rowHandles.AddRange(GetDataRowHandlesInGroup(rowHandle));
                }
                else
                    rowHandles.Add(rowHandle);
            }
            return rowHandles;
        }
    }
}
