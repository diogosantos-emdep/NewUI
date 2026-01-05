using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
  public  class ERMProductionTimelineCellMergeBehavior : Behavior<TableView>
    {
        protected override void OnAttached()
        {
            AssociatedObject.CellMerge += AssociatedObject_CellMerge;

            base.OnAttached();
        }

        private void AssociatedObject_CellMerge(object sender, CellMergeEventArgs e)
        {
            var view = (TableView)sender;



            object Employee1 = view.Grid.GetCellValue(e.RowHandle1, "Employee");
            object Employee2 = view.Grid.GetCellValue(e.RowHandle2, "Employee");

            object Date1 = view.Grid.GetCellValue(e.RowHandle1, "Date");
            object Date2 = view.Grid.GetCellValue(e.RowHandle2, "Date");

            object Week1 = view.Grid.GetCellValue(e.RowHandle1, "Week");
            object Week2 = view.Grid.GetCellValue(e.RowHandle2, "Week");

            object ShiftTime1 = view.Grid.GetCellValue(e.RowHandle1, "Total_Shift");
            object ShiftTime2 = view.Grid.GetCellValue(e.RowHandle2, "Total_Shift");

            object AttedanceTime1 = view.Grid.GetCellValue(e.RowHandle1, "Total_Attedance");
            object AttedanceTime2 = view.Grid.GetCellValue(e.RowHandle2, "Total_Attedance");


            object BreakFast1 = view.Grid.GetCellValue(e.RowHandle1, "Breakfast / Snack Break");
            object BreakFast2 = view.Grid.GetCellValue(e.RowHandle2, "Breakfast / Snack Break");


            object Lunch1 = view.Grid.GetCellValue(e.RowHandle1, "Lunch / Dinner");
            object Lunch2 = view.Grid.GetCellValue(e.RowHandle2, "Lunch / Dinner");




            if (e.Column.FieldName == "Week")
            {

                if (Employee1.Equals(Employee2) && Week1.Equals(Week2))
                {
                    e.Merge = true;

                }
                else
                {
                    e.Merge = false;

                }
                e.Handled = true;
            }

            else if (e.Column.FieldName == "Date")
            {

                if (Employee1.Equals(Employee2) && Date1.Equals(Date2))
                {
                    e.Merge = true;

                }
                else
                {
                    e.Merge = false;

                }
                e.Handled = true;
            }

            else if (e.Column.FieldName == "Employee")
            {

                if (Employee1.Equals(Employee2) && Date1.Equals(Date2))
                {
                    e.Merge = true;

                }
                else
                {
                    e.Merge = false;

                }
                e.Handled = true;
            }
            else if (e.Column.FieldName == "Total_Shift")
            {

                if (Employee1.Equals(Employee2) && Date1.Equals(Date2))
                {
                    e.Merge = true;

                }
                else
                {
                    e.Merge = false;

                }
                e.Handled = true;
            }


            else if (e.Column.FieldName == "Total_Attendance")
            {

                if (Employee1.Equals(Employee2) && Date1.Equals(Date2))
                {
                    e.Merge = true;

                }
                else
                {
                    e.Merge = false;

                }
                e.Handled = true;
            }

            else if (e.Column.FieldName == "Breakfast / Snack Break")
            {

                if (Employee1.Equals(Employee2) && Date1.Equals(Date2))
                {
                    e.Merge = true;

                }
                else
                {
                    e.Merge = false;

                }
                e.Handled = true;
            }
            else if (e.Column.FieldName == "Lunch / Dinner")
            {

                if (Employee1.Equals(Employee2) && Date1.Equals(Date2))
                {
                    e.Merge = true;

                }
                else
                {
                    e.Merge = false;

                }
                e.Handled = true;
            }

            else
            {
                e.Merge = false;
                e.Handled = true;

            }


        }












        protected override void OnDetaching()
        {
            AssociatedObject.CellMerge -= AssociatedObject_CellMerge;
            base.OnDetaching();
        }


    }
}
