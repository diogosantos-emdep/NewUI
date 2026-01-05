using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Emdep.Geos.UI.Helper
{
   public class CellMergeBehavior: Behavior<TableView>
    {
        #region Task Log
        // [000][skale][15-01-2020][GEOS2-1658] The filter LeaveType in Summary Leaves does not work properly
        #endregion
        protected override void OnAttached()
        {
            AssociatedObject.CellMerge += tableView_CellMerge;
            base.OnAttached();
        }
        protected override void OnDetaching()
        {
            AssociatedObject.CellMerge -= tableView_CellMerge;
            base.OnDetaching();
        }

        private void tableView_CellMerge(object sender, CellMergeEventArgs e)
        {
            var view = (TableView)sender;

            if (e.Column.FieldName == "EmployeeCode" || e.Column.FieldName == "FullName" || e.Column.FieldName == "EmployeeJobTitles")
            {
                string CellValue1 = e.CellValue1 as string;
                string CellValue2 = e.CellValue2 as string;

                if (String.Equals(CellValue1, CellValue2, StringComparison.OrdinalIgnoreCase))
                {
                    e.Merge = true;
                    e.Handled = true;
                }
            }
            else if (e.Column.FieldName == "EmployeeCompanyAlias")
            {

                int IdEmployee1 = ((Employee)view.Grid.GetRow(e.RowHandle1)).IdEmployee;
                int IdEmployee2 = ((Employee)view.Grid.GetRow(e.RowHandle2)).IdEmployee;

                if (IdEmployee1 == IdEmployee2)
                {
                    e.Merge = true;
                    e.Handled = true;
                }
                else
                {
                    e.Merge = false;
                    e.Handled = true;

                }

            }
            else if (e.Column.FieldName == "Departments")
            {
                //var view = (TableView)sender;
                var CellValue1 = ((Employee)view.Grid.GetRow(e.RowHandle1));
                var CellValue2 = ((Employee)view.Grid.GetRow(e.RowHandle2));

                if (CellValue1.LstEmployeeDepartments != null && CellValue2.LstEmployeeDepartments != null)
                {
                    List<string> DepartmentList = CellValue2.LstEmployeeDepartments.Select(i=>i.DepartmentName.ToUpper()).ToList();

                    if (!CellValue1.LstEmployeeDepartments.Any(x=> DepartmentList.Contains(x.DepartmentName.ToUpper())))
                    {
                        e.Merge = false;
                        e.Handled = true;
                    }
                    else
                    {
                        if (CellValue2.IdEmployee == CellValue1.IdEmployee)
                        {
                            e.Merge = true;
                            e.Handled = true;
                        }
                        else
                        {
                            e.Merge = false;
                            e.Handled = true;
                        }
                       
                    }
                }

            }
            else if(e.Column.FieldName == "FullName")
            {

                int IdEmployee1 = ((Employee)view.Grid.GetRow(e.RowHandle1)).IdEmployee;
                int IdEmployee2 = ((Employee)view.Grid.GetRow(e.RowHandle2)).IdEmployee;

                if (IdEmployee1 == IdEmployee2)
                {
                    e.Merge = true;
                    e.Handled = true;
                }
                else
                {
                    e.Merge = false;
                    e.Handled = true;

                }
            }
            else if (e.Column.FieldName == "EmployeeJobTitles")
            {
                int IdEmployee1 = ((Employee)view.Grid.GetRow(e.RowHandle1)).IdEmployee;
                int IdEmployee2 = ((Employee)view.Grid.GetRow(e.RowHandle2)).IdEmployee;

                if (IdEmployee1 == IdEmployee2)
                {
                    e.Merge = true;
                    e.Handled = true;
                }
                else
                {
                    e.Merge = false;
                    e.Handled = true;

                }

            }

        }

    }
}
