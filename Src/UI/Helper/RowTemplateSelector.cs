using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class RowTemplateSelector : DataTemplateSelector
    {
        public DataTemplate evenRowTemplate { get; set; }
        public DataTemplate oddRowTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            RowData row = item as RowData;

            if (row != null)
            {
                if (row.EvenRow)
                    return evenRowTemplate;
                else
                    return oddRowTemplate;

            }
            return base.SelectTemplate(item, container);
        }
    }
}