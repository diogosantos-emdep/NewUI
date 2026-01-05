using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Emdep.Geos.UI.Helper
{
   public class FlyoutHelperBehavior:Behavior<TileBarItem>
    {
        static readonly DependencyProperty IsItemSelectedProperty =
             DependencyProperty.Register("IsItemSelected", typeof(bool), typeof(FlyoutHelperBehavior), new PropertyMetadata(false, new PropertyChangedCallback((d, e) =>
             {
                 ((FlyoutHelperBehavior)d).OnIsItemSelectedChanged();
             })));

        protected virtual void OnIsItemSelectedChanged()
        {
            var flyout = (DevExpress.Xpf.Editors.Flyout.Native.FlyoutBase)AssociatedObject.GetValue(DevExpress.Xpf.Editors.Flyout.Native.FlyoutBase.FlyoutProperty);
            if (flyout != null)
                flyout.IsOpen = false;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            BindingOperations.SetBinding(this, IsItemSelectedProperty, new Binding("IsSelected") { Source = AssociatedObject });
        }

        protected override void OnDetaching()
        {
            BindingOperations.ClearBinding(this, IsItemSelectedProperty);
            base.OnDetaching();
        }

    }
}
