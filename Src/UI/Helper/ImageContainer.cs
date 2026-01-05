using DevExpress.Xpf.Core;
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
    public class ImageContainer : ContentControlBase
    {
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (Controller.IsMouseLeftButtonDown)
            {
                var layoutControl = Parent as FlowLayoutControl;
                if (layoutControl != null)
                {
                    Controller.IsMouseEntered = false;
                    layoutControl.MaximizedElement = layoutControl.MaximizedElement == this ? null : this;
                }
            }
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            //Did Comment For ImageGallery Size [Sudhir.Jangra][GEOS-2922][24/04/2023]
            //if (e.PreviousSize.Height == 0 || (e.NewSize.Height <= e.PreviousSize.Height))
            //{
            //    Height = 210;
            //    Width = 280;
            //}
            //else
            //{
            //    if (!double.IsNaN(Width) && !double.IsNaN(Height))
            //        if (e.NewSize.Width != e.PreviousSize.Width)
            //            Height = double.NaN;
            //        else
            //            Width = double.NaN;
            //}
        }

    }
}
