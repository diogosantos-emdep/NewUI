using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Emdep.Geos.UI.Helper
{
   public class CellDecorator: Decorator
    {
        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewLostKeyboardFocus(e);

            if (LayoutTreeHelper.GetVisualChildren(this).Contains((DependencyObject)e.NewFocus)) return;

            if (DataContext is EditGridCellData)
            {
                var view = ((EditGridCellData)DataContext).View;
                if (view == null)
                    return;
                view.PostEditor();
                view.HideEditor();
            }
        }
    }
}
