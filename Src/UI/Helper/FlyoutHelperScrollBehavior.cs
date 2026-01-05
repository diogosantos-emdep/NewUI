using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Controls.Primitives;
using DevExpress.Xpf.Editors.Flyout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.UI.Helper
{
    public class FlyoutHelperScrollBehavior : Behavior<ScrollableControlButton>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
        }

        private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ICommand command = sender is ICommandSource ? ((ICommandSource)sender).Command : null;
            if (command == null)
                return;
            command.Execute(null);
            e.Handled = true;
        }


        protected override void OnDetaching()
        {
            this.AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
            base.OnDetaching();
        }
    }
}
