using DevExpress.Xpf.Grid;
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
    public class MultipleCellEditHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(MultipleCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));
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

        private static string leadSource;
        public static string LeadSource
        {
            get { return leadSource; }
            set { leadSource = value; }
        }

        private static string leadDicount;
        public static string LeadDicount
        {
            get { return leadDicount; }
            set { leadDicount = value; }
        }

        private static string businessUnit;
        public static string BusinessUnit
        {
            get { return businessUnit; }
            set { businessUnit = value; }
        }

        private static int businessUnitId;
        public static int BusinessUnitId
        {
            get { return businessUnitId; }
            set { businessUnitId = value; }
        }

        private static string oEM;
        public static string OEM
        {
            get { return oEM; }
            set { oEM = value; }
        }

        private static string salesOwner;

        public static string SalesOwner
        {
            get { return salesOwner; }
            set { salesOwner = value; }
        }

        private static string project;
        public static string Project
        {
            get { return project; }
            set { project = value; }
        }

        private static bool isChecked;
        public static bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public static readonly DependencyProperty IsValueChangedProperty =
            DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(MultipleCellEditHelper), new PropertyMetadata(false));
        public static List<int> MmodifiedRowHandles = new List<int>();

        //private static List<int> modifiedRowHandles;
        //public static List<int> MmodifiedRowHandles
        //{
        //    get { return modifiedRowHandles; }
        //    set { modifiedRowHandles = value; }
        //}

        private static bool isValueChanged;

        public static bool IsValueChanged
        {
            get { return isValueChanged; }
            set { isValueChanged = value; }
        }

        public static string Checkview { get; set; }

        public static string strviewmodulname { get; set; }
        private static TableView viewtableview;
        public static TableView Viewtableview
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
            //TableViewHitInfo hi = ((TableView)grid.View).CalcHitInfo(e.OriginalSource as DependencyObject);
        }

       /// [001][GEOS2-3157][cpatil][11-10-2021]Improvement in the discount option. (Set the discount null by default)
        static void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                IsValueChanged = true;
                TableView view = sender as TableView;
                Checkview = view.Name;
                Viewtableview = view;

                List<GridCell> cells = view.GetSelectedCells().ToList<GridCell>();
                List<GridCell> selectedCells = cells.Where(c => c.Column == e.Column).ToList<GridCell>();

                view.PostEditor();
                foreach (GridCell cell in selectedCells)
                {
                    int row = cell.RowHandle;
                    string columnName = cell.Column.FieldName;

                    ((GridControl)view.DataControl).SetCellValue(row, "IsChecked", true);
                    IsChecked = true;

                    if (columnName == "Source")
                    {
                        ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                        LeadSource = e.Value.ToString();
                    }
                    else if (columnName == "BusinessUnit")
                    {
                        ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                        BusinessUnit = e.Value.ToString();
                    }
                    else if (columnName == "OEM")
                    {
                        ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                        OEM = e.Value.ToString();
                    }
                    else if (columnName == "Project")
                    {
                        if (e.Value != null)
                        {
                            ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                            Project = e.Value.ToString();

                            OEM = GeosApplication.Instance.GeosCarProjectsList.Where(b => b.Name == e.Value.ToString()).Select(u => u.CarOEM.Name).FirstOrDefault();
                            ((GridControl)view.DataControl).SetCellValue(row, "OEM", OEM);
                            OEM = OEM;
                        }
                        else
                        {
                            Project = null;
                            ((GridControl)view.DataControl).SetCellValue(row, "Project", Project);
                            OEM = null;
                            ((GridControl)view.DataControl).SetCellValue(row, "OEM", OEM);
                        }
                    }
                    else if (columnName == "SalesOwner")
                    {
                        Int32 idSite = Convert.ToInt32(((System.Data.DataRowView)((DevExpress.Xpf.Grid.RowData)(view.GetRowElementByRowHandle(row).DataContext)).Row).Row["IdSite"]);
                        var SalesOwners = GeosApplication.Instance.SalesOwnerList.Where(x => x.IdSite == idSite);

                        foreach (var item in SalesOwners)
                        {
                            if (item.SalesOwner == Convert.ToString(e.Value))
                            {
                                ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                            }
                        }

                        //((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                        //SalesOwner = e.Value.ToString();
                    }

                    else if (columnName == "Discount")
                    {
                        ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                       //[001]
                        if (e.Value != null)
                        {
                            LeadDicount = e.Value.ToString();
                        }
                        else
                        {
                            LeadDicount = null;
                          
                        }
                    }

                    else
                    {
                        ((GridControl)view.DataControl).SetCellValue(row, columnName, e.Value);
                    }

                    ((GridControl)view.DataControl).RefreshRow(row);
                }

                SetIsValueChanged(view, true);
            }
            catch (Exception ex)
            {
            }
        }

    }
}
