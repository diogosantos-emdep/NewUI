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
using Emdep.Geos.Modules.Crm.ViewModels;
using DevExpress.Xpf.Grid;                
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Editors.Flyout;

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for DXWindow1.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();
            mainGrid.Focus();
            ((GridControl)mainGrid.Children[0]).Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmdepSitesWiseMapWindowViewModel emdepSitesWiseMapWindowViewModel = new EmdepSitesWiseMapWindowViewModel("Spain");
            // if (emdepSitesWiseMapWindowViewModel.IsInit == true)
            {
                EmdepSitesMapWindow emdepSitesMapWindow = new EmdepSitesMapWindow();
                EventHandler handle = delegate { emdepSitesMapWindow.Close(); };
                emdepSitesWiseMapWindowViewModel.RequestClose += handle;
                emdepSitesMapWindow.DataContext = emdepSitesWiseMapWindowViewModel;
                // GeosApplication.Instance.Logger.Log("Initialising EmdepSitesMapWindow Successfully", category: Category.Info, priority: Priority.Low);
                emdepSitesMapWindow.ShowDialogWindow();
            }
        }
        //[pallavi.kale][05.11.2025][GEOS2-9792]
        private void grid_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var gridControl = mainGrid.Children[0] as GridControl;
                if (gridControl == null)
                    return;

                var view = gridControl.View as TableView;
                if (view == null)
                    return;

                if (view != null && !view.IsKeyboardFocusWithin)
                {
                    view.Focus();
                }


                var hitInfo = view.CalcHitInfo(e.OriginalSource as DependencyObject);

                if (hitInfo.InRowCell && hitInfo.Column?.FieldName == "SalesOwnerUnbound")
                {
                    var rowData = Accountgrid.GetRow(hitInfo.RowHandle) as Company;
                    var salesOwnerList = rowData?.SalesOwnerListForImages;
                    if (salesOwnerList != null && salesOwnerList.Count > 0)
                    {
                        var cellElement = view.GetCellElementByRowHandleAndColumn(hitInfo.RowHandle, hitInfo.Column);
                        if (cellElement != null)
                        {
                            SalesOwnerflyoutControl.PlacementTarget = cellElement;
                            SalesOwnerflyoutControl.DataContext = salesOwnerList;
                            SalesOwnerflyoutControl.IsOpen = true;
                            SalesOwnerflyoutControl.Visibility = Visibility.Visible;

                            var flyout = SalesOwnerflyoutControl as FlyoutControl;
                            if (flyout != null)
                                flyout.Visibility = Visibility.Visible;
                        }
                    }

                }
                else
                {
                    SalesOwnerflyoutControl.IsOpen = false;
                    var flyout = SalesOwnerflyoutControl as FlyoutControl;
                    if (flyout != null)
                        flyout.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                // Handle exceptions if necessary
            }
        }


    }
}
