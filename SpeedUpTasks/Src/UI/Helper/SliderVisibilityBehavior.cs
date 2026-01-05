using DevExpress.Mvvm.UI.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
  public class SliderVisibilityBehavior : Behavior<Slider>
    {

        Slider AssociatedSlider { get { return AssociatedObject; } }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedSlider.Opacity = 0.01;
            AssociatedSlider.MouseEnter += AssociatedSlider_MouseEnter;
            AssociatedSlider.MouseLeave += AssociatedSlider_MouseLeave;
        }

        protected override void OnDetaching()
        {
            AssociatedSlider.Opacity = 0.001;
            AssociatedSlider.MouseEnter -= AssociatedSlider_MouseEnter;
            AssociatedSlider.MouseLeave -= AssociatedSlider_MouseLeave;
            base.OnDetaching();
        }

        void AssociatedSlider_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AssociatedSlider.Opacity = 0.001;
        }

        void AssociatedSlider_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AssociatedSlider.Opacity = 1;
        }
    }
}
