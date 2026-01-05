using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{//[Sudhir.jangra][GEOS2-5203]
   public  class SampleViewMultipleCellEditHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(SampleViewMultipleCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));

        private static void IsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TableView view = source as TableView;
            view.CellValueChanging += view_CellValueChanging;
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
        public static void SetIsEnabled(UIElement element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(UIElement element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
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
                        foreach (GridCell cell in selectedCells)
                        {
                            int row = cell.RowHandle;
                            string columnName = cell.Column.FieldName;
                            if (columnName == "StatusName")
                            { 
                                ((GridControl)view.DataControl).SetCellValue(cell.RowHandle, "IsUpdatedRow", true);
                                string s = Convert.ToString(e.Value);
                                foreach (var item in newRowData)
                                {
                                    ((Emdep.Geos.Data.Common.SCM.Samples)item.Row).StatusHTMLColor = ((Emdep.Geos.Data.Common.SCM.Samples)item.Row).StatusList.FirstOrDefault(a => a.Name == s).HTMLColor;
                                    ((Emdep.Geos.Data.Common.SCM.Samples)item.Row).Status = ((Emdep.Geos.Data.Common.SCM.Samples)item.Row).StatusList.FirstOrDefault(a => a.Name == s);
                                    ((Emdep.Geos.Data.Common.SCM.Samples)item.Row).StatusName = ((Emdep.Geos.Data.Common.SCM.Samples)item.Row).StatusList.FirstOrDefault(a => a.Name == s).Name;
                                }
                            }
                        newRowData.ToList().ForEach(i => ((GridControl)view.DataControl).RefreshRow(i.RowHandle));
                        ((GridControl)view.DataControl).UpdateLayout();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
