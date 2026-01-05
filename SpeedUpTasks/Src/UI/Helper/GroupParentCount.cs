using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Emdep.Geos.UI.Helper
{
    //class GroupParentCount
    //{
    //}

    public class GroupParentCount : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            EditGridCellData cd = values[1] as EditGridCellData;
            TreeListRowData n = cd.RowData as TreeListRowData;
            bool child = false;

            if (n.Node.Content is Emdep.Geos.Data.Common.PCM.Options)
            {
                Emdep.Geos.Data.Common.PCM.Options opt = n.Node.Content as Emdep.Geos.Data.Common.PCM.Options;
                if (opt.Parent != null)
                    child = true;
            }

            if (n.Node.Nodes.Count == 0 && child == true)
            {
                return ((Emdep.Geos.Data.Common.PCM.Options)n.Node.Content).Name;
            }
            else if (n.Node.Nodes.Count != 0 && child == false)
            {
                return string.Format("{0} : {1}", ((Emdep.Geos.Data.Common.PCM.Options)n.Node.Content).Name, n.Node.Nodes.Count);
            }
            else
            {
                return ((Emdep.Geos.Data.Common.PCM.Options)n.Node.Content).Name;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
