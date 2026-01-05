using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.ERM;
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
    //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
    public class PlanningDateReviewCellEditHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(PlanningDateReviewCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));



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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        
        private static bool isValueChanged;

        public static bool IsValueChanged
        {
            get { return isValueChanged; }
            set
            {

                isValueChanged = value;

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

      

       

        
        private static DateTime planningDeliveryDate;
        public static DateTime PlanningDeliveryDate
        {
            get { return planningDeliveryDate; }
            set { planningDeliveryDate = value; }
        }
     
        public static readonly DependencyProperty IsValueChangedProperty =
          DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(PlanningDateReviewCellEditHelper), new PropertyMetadata(false));

        public static List<int> MmodifiedRowHandles = new List<int>();

        

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

                          
                            if (columnName == "PlanningDeliveryDate")
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                                ((GridControl)view.DataControl).SetCellValue(row, "IsUpdatedRow", true);
                                PlanningDeliveryDate = Convert.ToDateTime(e.Value);
                                //PlanningDeliveryDate = PlanningDeliveryDate.Trim(' ', '\r');
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
