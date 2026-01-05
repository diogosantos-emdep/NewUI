using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.UI.Helper
{
    public class JDLevelTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;

            LookupValue JdLevel = null;

            if (item is GridCellData)
            {
                GridCellData data = (GridCellData)item;

                if ((Emdep.Geos.Data.Common.Hrm.JobDescription)(data.RowData.Row) != null)
                    JdLevel = ((Emdep.Geos.Data.Common.Hrm.JobDescription)data.Row).JDLevel;
            }
            else if (item is LookupValue)
            {
                JdLevel = item as LookupValue;
            }

            if (JdLevel == null)
                return null;

            if (JdLevel.IdLookupValue == 212 || JdLevel.IdLookupValue == 213 || JdLevel.IdLookupValue == 214)
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("TemplateDiamondWithCircle");
            }
            else
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("TemplateDiamond");
            }

        }
    }
}

