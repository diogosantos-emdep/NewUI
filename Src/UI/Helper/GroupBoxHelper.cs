using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    //[rdixit][25.07.2024][GEOS2-5680]
    public class GroupBoxHelper
    {
        public static TableView GetInnerTableView(DependencyObject obj)
        {
            return (TableView)obj.GetValue(InnerTableViewProperty);
        }
        public static void SetInnerTableView(DependencyObject obj, TableView value)
        {
            obj.SetValue(InnerTableViewProperty, value);
        }
        public static readonly DependencyProperty InnerTableViewProperty =
            DependencyProperty.RegisterAttached("InnerTableView", typeof(TableView), typeof(GroupBoxHelper), new PropertyMetadata(null));

    }
}
