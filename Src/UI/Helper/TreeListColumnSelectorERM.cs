using System.Windows.Controls;
using System.Windows;
using System.Collections;
using DevExpress.Data;
using DevExpress.Xpf.Grid;
using System;
using System.Windows.Data;

namespace Emdep.Geos.UI.Helper
{
   public class TreeListColumnSelectorERM : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            TLColumnERM column = (TLColumnERM)item;
            return (DataTemplate)((Control)container).FindResource(column.Settings + "ColumnTemplate");
        }
    }
    public enum SettingsTreeListColumnERM
    {
        Default,Hidden, NameWithWorkOperationCount, ObservedTime, Activity, NormalTime,Array
    }
}
