using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.UI.Helper
{
    public class FlyoutControlBehavior : Behavior<FlyoutControl>
    {
        protected GroupBox Group;

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(FlyoutControlBehavior), new PropertyMetadata(false,
                (d, e) => { (d as FlyoutControlBehavior).UpdateIsOpen(); }));

        protected DependencyPropertyDescriptor desc = DependencyPropertyDescriptor.FromProperty(GroupBox.RenderTransformProperty, typeof(GroupBox));

        protected override void OnAttached()
        {
            base.OnAttached();
            Group = LayoutTreeHelper.GetVisualParents(this.AssociatedObject).Where(c => c is GroupBox).FirstOrDefault() as GroupBox;
            if (Group != null)
            {
                desc.AddValueChanged(Group, RenderTransform_Changed);
            }
        }

        protected override void OnDetaching()
        {
            if (Group != null)
            {
                desc.RemoveValueChanged(Group, RenderTransform_Changed);
            }

            base.OnDetaching();
        }

        protected void UpdateIsOpen()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (Group != null && Group.State == GroupBoxState.Maximized && Group.RenderTransform == Transform.Identity)
                {
                    this.AssociatedObject.SetCurrentValue(FlyoutControl.IsOpenProperty, this.IsOpen);
                }
            }));

        }

        private void RenderTransform_Changed(object sender, EventArgs e)
        {
            var group = sender as GroupBox;
            if (group.RenderTransform == Transform.Identity && group.State == GroupBoxState.Maximized)
            {
                this.AssociatedObject.SetCurrentValue(FlyoutControl.IsOpenProperty, this.IsOpen);
                this.AssociatedObject.UpdateLocation();
            }
        }
    }
}