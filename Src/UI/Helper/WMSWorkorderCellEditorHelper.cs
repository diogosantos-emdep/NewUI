using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{//[Sudhir.Jangra][GEOS2-4539][23/08/2023]
    public class WMSWorkorderCellEditorHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(WMSWorkorderCellEditorHelper), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));

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
            TreeListView view = source as TreeListView;
            view.CellValueChanging += view_CellValueChanging;
        }
        public static string Checkview { get; set; }

        public static TreeListView viewtableview;
        public static TreeListView Viewtableview
        {
            get { return viewtableview; }
            set
            {
                viewtableview = value;
                Clonedviewtableview = viewtableview;
            }
        }
        private static TreeListView clonedviewtableview;
        public static TreeListView Clonedviewtableview
        {
            get { return clonedviewtableview; }
            set
            {
                clonedviewtableview = value;
            }
        }
        private static bool isValueChanged;

        public static bool IsValueChanged
        {
            get { return isValueChanged; }
            set { isValueChanged = value; }
        }
        private static ItemOTStatusType selectedWorkflowStatus;
        public static ItemOTStatusType SelectedWorkflowStatus
        {
            get { return selectedWorkflowStatus; }
            set { selectedWorkflowStatus = value; }
        }
        public static bool GetIsValueChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsValueChangedProperty);
        }

        public static List<int> MmodifiedRowHandles = new List<int>();
        static void View_CellValueChanged1(object sender, CellValueChangedEventArgs e)
        {
            MmodifiedRowHandles.Add(e.RowHandle);
        }

        public static void SetIsValueChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsValueChangedProperty, value);
        }
        public static readonly DependencyProperty IsValueChangedProperty =
       DependencyProperty.RegisterAttached("IsValueChanged", typeof(bool), typeof(WMSWorkorderCellEditorHelper), new PropertyMetadata(false));
        static void view_CellValueChanging(object sender, TreeListCellValueChangedEventArgs e)
        {
            try
            {
                TreeListView view = sender as TreeListView;
                Checkview = view.Name;
                Viewtableview = view;

                if (e.Cell.Row != null)
                {

                    OtItem wo = (OtItem)e.Cell.Row;
                   
                    if (e.Cell.Property == "Status.Name")
                    {
                        if (e.Cell.Row != null)
                        {
                            string status = Convert.ToString(e.Value);
                            if (wo.StatusList.Any(i => i.Name == status))
                            {
                                view.PostEditor();
                                IsValueChanged = true;
                                wo.IsUpdatedRow = true;
                                SelectedWorkflowStatus = wo.StatusList.FirstOrDefault(a => a.Name == status);
                                wo.Status.HtmlColor = SelectedWorkflowStatus.HtmlColor;
                                wo.Status.IdItemOtStatus= SelectedWorkflowStatus.IdItemOtStatus;
                             
                                
                               
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
