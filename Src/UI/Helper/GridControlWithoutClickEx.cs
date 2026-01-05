using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.UI.Helper
{
    public class GridControlWithoutClickEx : GridControl
    {
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
           // e.Handled = true;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
           // e.Handled = true;
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
           // e.Handled = true;
        }
    }


    public class GridControlWithoutClickExtended : GridControl
    {
        public static readonly DependencyProperty IsFocusedPropertyNew =
            DependencyProperty.Register("IsFocusedNew", typeof(bool), typeof(GridControlWithoutClickExtended),
                null);

        /// <summary>
        /// This dependancy property is created for focus on user control.
        /// </summary>
        public bool IsFocusedNew
        {
            get { return (bool)GetValue(IsFocusedPropertyNew); }
            set{ SetValue(IsFocusedPropertyNew, value); }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var hitInfo = ((TableView)View).CalcHitInfo(e.OriginalSource as DependencyObject);

            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "IdReason")
                base.OnPreviewMouseLeftButtonDown(e);

            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "IsOK")
            {
                base.OnPreviewMouseLeftButtonDown(e);

                InventoryMaterial inventoryMaterial = (InventoryMaterial)((TableView)View).DataControl.CurrentItem;

                if (inventoryMaterial != null &&
                    !inventoryMaterial.IsApproved &&
                     inventoryMaterial.IsOK != null)
                {
                    //System.Windows.Application.Current.FindResource("InventoryRegularizeStock").ToString()
                    MessageBoxResult result = CustomMessageBox.Show(string.Format("Are you sure you want to reset quantity?"), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        inventoryMaterial.RemainingQty = (-1) * inventoryMaterial.AvailableQty;
                        inventoryMaterial.ScannedQty = 0;
                        inventoryMaterial.IsOK = null;
                    }
                }

                IsFocusedNew = true;
            }

            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "IdItemStatus")
            {
                ValidateItem validateItem = (ValidateItem)((TableView)View).DataControl.GetRow(hitInfo.RowHandle);
                //original
                if ((validateItem == null) || (validateItem != null && !validateItem.IsEnabled))
                {
                    e.Handled = true;
                    return;
                }

                base.OnPreviewMouseLeftButtonDown(e);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
           // e.Handled = true;
        }
    }
}
