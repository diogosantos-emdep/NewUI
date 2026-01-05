using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for AnnualSalesPerformanceView.xaml
    /// </summary>
    public partial class AnnualSalesPerformanceView : UserControl
    {
        public AnnualSalesPerformanceView()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CultureInfo.CurrentCulture.IetfLanguageTag);
        }

        private void ChartControlBoundDataChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((TableView)sender).FocusedRowHandle = GridControl.InvalidRowHandle;
        }
    }
}
