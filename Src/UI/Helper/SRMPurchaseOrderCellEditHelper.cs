using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class SRMPurchaseOrderCellEditHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(SRMPurchaseOrderCellEditHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));

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
            set { isValueChanged = value; }
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

        private static int idWorkflowStatus;
        public static int IdWorkflowStatus
        {
            get { return idWorkflowStatus; }
            set { idWorkflowStatus = value; }
        }
        private static WorkflowStatus selectedWorkflowStatus;
        public static WorkflowStatus SelectedWorkflowStatus
        {
            get { return selectedWorkflowStatus; }
            set { selectedWorkflowStatus = value; }
        }


        public static readonly DependencyProperty IsValueChangedProperty =
          DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(SRMPurchaseOrderCellEditHelper), new PropertyMetadata(false));

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

                if (e.Cell.Row != null)
                {

                    WarehousePurchaseOrder wo = (WarehousePurchaseOrder)e.Cell.Row;
                    //[rdixit][GEOS2-4353][16.06.2023]
                    if (e.Cell.Property == "WorkflowStatus.Name")
                    {
                        if (e.Cell.Row != null)
                        {
                            string status = Convert.ToString(e.Value);
                            if (wo.LstWorkflowStatusTransition.Any(i => i.Name == status))
                            {
                                view.PostEditor();
                                IsValueChanged = true;
                                wo.IsUpdatedRow = true;
                                SelectedWorkflowStatus = wo.LstWorkflowStatus.FirstOrDefault(a => a.Name == status);
                                wo.WorkflowStatus.HtmlColor = SelectedWorkflowStatus.HtmlColor;
                                wo.IdWorkflowStatus = SelectedWorkflowStatus.IdWorkflowStatus;
                                //[rdixit][30.05.2023]
                                ObservableCollection<WarehousePurchaseOrder> LstWarehousePO = (ObservableCollection<WarehousePurchaseOrder>)(((GridControl)view.DataControl)).ItemsSource;
                                if (LstWarehousePO.Any(i => i.IsSendMail == true))
                                {
                                    foreach (WarehousePurchaseOrder item in LstWarehousePO.Where(i => i.IsSendMail == true))
                                    {
                                        if (wo.LstWorkflowStatusTransition.Any(i => i.IdWorkflowStatus == item.IdWorkflowStatus))
                                        {
                                            try
                                            {
                                                item.IsUpdatedRow = true;
                                                //item.IdWorkflowStatus = Convert.ToByte(e.Value);
                                                //GEOS2 - 4353 instead of name show values
                                                item.IdWorkflowStatus = item.LstWorkflowStatus.FirstOrDefault(a => a.Name.ToLower().Equals(e.Value.ToString().ToLower())).IdWorkflowStatus;
                                                item.WorkflowStatus = item.LstWorkflowStatus.FirstOrDefault(a => a.IdWorkflowStatus == item.IdWorkflowStatus);
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                }
                                // ((GridControl)view.DataControl).RefreshRow(row);
                                ((GridControl)view.DataControl).UpdateLayout();
                                SetIsValueChanged(view, true);
                            } 
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
