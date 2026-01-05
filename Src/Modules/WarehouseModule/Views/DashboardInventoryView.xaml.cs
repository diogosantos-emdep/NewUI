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

namespace Emdep.Geos.Modules.Warehouse.Views
{
    /// <summary>
    /// Interaction logic for DashboardInventoryView.xaml
    /// </summary>
    public partial class DashboardInventoryView : UserControl
    {
        public DashboardInventoryView()
        {
            InitializeComponent();
        }

        private void ChartInventoryAmountByWeek_Loaded(object sender, RoutedEventArgs e)
        {
            DevExpress.Xpf.Charts.ChartControl obj = (DevExpress.Xpf.Charts.ChartControl)sender;
            //var per = 92.00;
           // var height = (obj.ActualHeight * per) / 100.0;
            obj.Height = obj.ActualHeight;
        }
    }
}
