using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Helper
{
    public class GroupParentCount_Detection : MarkupExtension, IMultiValueConverter
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

            if (n.Node.Content is Emdep.Geos.Data.Common.PCM.Detections)
            {
                Emdep.Geos.Data.Common.PCM.Detections opt = n.Node.Content as Emdep.Geos.Data.Common.PCM.Detections;
                if (opt.Parent != null)
                    child = true;
            }

            if (n.Node.Nodes.Count == 0 && child == true)
            {
                return ((Emdep.Geos.Data.Common.PCM.Detections)n.Node.Content).Name;
            }
            else if (n.Node.Nodes.Count != 0 && child == false)
            {
                return string.Format("{0} : {1}", ((Emdep.Geos.Data.Common.PCM.Detections)n.Node.Content).Name, n.Node.Nodes.Count);
            }
            else
            {
                return ((Emdep.Geos.Data.Common.PCM.Detections)n.Node.Content).Name;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
