using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DevExpress.Xpf.Core.Native;


namespace Emdep.Geos.UI.Converters
{
    public class ButtonToParametersConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is Button button && values[1] is string columnName)
            {
                // Find the GridControl first
                var gridControl = LayoutHelper.FindParentObject<GridControl>(button);
                if (gridControl != null)
                {
                    // Get the row handle from the button's context
                    var rowData = GetRowDataFromButton(button, gridControl);
                    if (rowData != null && rowData.Row != null)
                    {
                        return new object[] { rowData.Row, columnName };
                    }
                }
            }

            return null;
        }
        private RowData GetRowDataFromButton(Button button, GridControl gridControl)
        {
            // Try to find the row data through visual tree
            DependencyObject parent = button;
            while (parent != null)
            {
                if (parent is RowData rowData)
                {
                    return rowData;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }    
    }
}
