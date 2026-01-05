using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Core;
using System.Collections.ObjectModel;

namespace Emdep.Geos.UI.Helper
{
 public   class MyFlowLayoutControl : FlowLayoutControl
    {
        protected override Size OnMeasure(Size availableSize)
        {
            FrameworkElements children = GetLogicalChildren(true);
            int count = 0;
            foreach (var child in children)
                if (MaximizedElement != child)
                    count++;
            //double height = (availableSize.Height - ItemSpace * count) / count;
            double Width = (availableSize.Width - ItemSpace * count) / count;
            foreach (var child in children)
            {
                //child.MinHeight = height;
                child.MinWidth = Width;
                child.Measure(new Size(Width, availableSize.Height));
            }
            return availableSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }
    }
}
