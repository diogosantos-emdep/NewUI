using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.Hrm.Views
{
    /// <summary>
    /// Interaction logic for LeavesView.xaml
    /// </summary>
    public partial class LeavesView : UserControl
    {
        public LeavesView()
        {
            InitializeComponent();
        }
        //[GEOS2-2456][rdixit][12.06.2023]
        private void LeavesGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DevExpress.Xpf.Grid.GridControl grid = (DevExpress.Xpf.Grid.GridControl)sender;
            CriteriaOperator filterCriteria = new BinaryOperator("InUse", "Yes");//[rdixit][GEOS2-6571][14.12.2024]
            grid.FilterCriteria = filterCriteria;
        }

        #region [rdixit][25.07.2024][GEOS2-5680]
        private void Leaves_StateChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<GroupBoxState> e)
        {
            if (e.NewValue == GroupBoxState.Maximized)
                Dispatcher.BeginInvoke(new Action(() => {
                    GroupBoxHelper.SetInnerTableView(Leaves, LayoutTreeHelper.GetVisualChildren(this).OfType<TableView>().FirstOrDefault());
                }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void Plants_StateChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<GroupBoxState> e)
        {
            if (e.NewValue == GroupBoxState.Maximized)
                Dispatcher.BeginInvoke(new Action(() => {
                    GroupBoxHelper.SetInnerTableView(Plants, LayoutTreeHelper.GetVisualChildren(this).OfType<TableView>().FirstOrDefault());
                }), System.Windows.Threading.DispatcherPriority.Render);
        }

        #endregion

        private void LengthOfService_StateChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<GroupBoxState> e)
        {
            if (e.NewValue == GroupBoxState.Maximized)
                Dispatcher.BeginInvoke(new Action(() => {
                    GroupBoxHelper.SetInnerTableView(LengthOfService, LayoutTreeHelper.GetVisualChildren(this).OfType<TableView>().FirstOrDefault());
                }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void ChangeLog_StateChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<GroupBoxState> e)
        {
            if (e.NewValue == GroupBoxState.Maximized)
                Dispatcher.BeginInvoke(new Action(() => {
                    GroupBoxHelper.SetInnerTableView(ChangeLog, LayoutTreeHelper.GetVisualChildren(this).OfType<TableView>().FirstOrDefault());
                }), System.Windows.Threading.DispatcherPriority.Render);
        }
    }
}
