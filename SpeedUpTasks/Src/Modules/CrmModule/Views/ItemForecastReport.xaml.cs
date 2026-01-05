using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Globalization;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System.Windows.Threading;

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for ItemForecastReport.xaml
    /// </summary>
    public partial class ItemForecastReport : UserControl
    {

        //public static bool GetIsMergedCellProvider(DependencyObject obj)
        //{
        //    return (bool)obj.GetValue(IsMergedCellProviderProperty);
        //}
        //public static void SetIsMergedCellProvider(DependencyObject obj, bool value)
        //{
        //    obj.SetValue(IsMergedCellProviderProperty, value);
        //}
        //public static readonly DependencyProperty IsMergedCellProviderProperty =
        // DependencyProperty.RegisterAttached("IsMergedCellProvider", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public ItemForecastReport()
        {            InitializeComponent();
           // Panel.ZIndexProperty.AddPropertyChangedCallback(typeof(GridRow), OnGridRowPropertyChanged);

        }
        //private void OnGridRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var gridRow = (GridRow)d;
        //    if (GetIsMergedCellProvider(gridRow))
        //        gridRow.SetZIndex(0xFF);
        //    else
        //        gridRow.SetZIndex(-1);
        //}
        private void ItemsForecastGridTableView_CellMerge(object sender, DevExpress.Xpf.Grid.CellMergeEventArgs e)
        {

        }
    }
}
