using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.LayoutControl;
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
   public class FlowLayoutControlMinimiseMaximiseStretch : Behavior<FlowLayoutControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MaximizedElementChanged += AssociatedObject_MaximizedElementChanged;
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            var flc = sender as FlowLayoutControl;
            if (flc.Children != null)
            {
                flc.MaximizedElement = flc.Children[0] as FrameworkElement;
            }

            var grid = LayoutTreeHelper.GetVisualParents(AssociatedObject).OfType<Grid>().Where(el => el.HorizontalAlignment == System.Windows.HorizontalAlignment.Center).FirstOrDefault();
            grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        }

        private void AssociatedObject_MaximizedElementChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<System.Windows.FrameworkElement> e)
        {
            if (e.NewValue == null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var flc = sender as FlowLayoutControl;
                    if (flc.Children != null)
                    {
                        flc.MaximizedElement = flc.Children[0] as FrameworkElement;
                    }
                }));
            }
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.MaximizedElementChanged -= AssociatedObject_MaximizedElementChanged;
            base.OnDetaching();
        }
    }
}
