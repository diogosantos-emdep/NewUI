using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
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

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for ContactView.xaml
    /// </summary>
    public partial class ContactView : UserControl
    {
        public ContactView()
        {
            InitializeComponent();
            mainGrid.Focus();
            ((GridControl)mainGrid.Children[1]).Focus();
        }

        //[rdixit][05.11.2025][GEOS2-9792]
        private void grid_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var gridControl = mainGrid.Children[1] as GridControl;
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

                if (hitInfo.InRowCell && hitInfo.Column?.FieldName == "SalesOwner")
                {
                    var rowData = grid.GetRow(hitInfo.RowHandle) as People;
                    var salesOwnerList = rowData?.Company?.SalesOwnerListForImages;
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
