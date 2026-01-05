using DevExpress.Data;
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
using DevExpress.Xpf.Data;
using DevExpress.Xpf.Grid;

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for DashboardSale.xaml
    /// </summary>
    public partial class DashboardSale : UserControl
    {
        Dictionary<string, double> CustomSummary = new Dictionary<string, double>();

        public DashboardSale()
        {
            InitializeComponent();
        }

        private void treeListView1_CustomSummary(object sender, DevExpress.Xpf.Grid.TreeList.TreeListCustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == CustomSummaryProcess.Start && e.Node == null)
            {
                CustomSummary[((GridSummaryItem)e.SummaryItem).FieldName] = 0.0;
            }

            if (e.SummaryProcess == CustomSummaryProcess.Calculate && e.Node.Level == 0)
            {
                CustomSummary[((GridSummaryItem)e.SummaryItem).FieldName] = CustomSummary[((GridSummaryItem)e.SummaryItem).FieldName] + (double)e.FieldValue;
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize && e.Node == null)
            {
                e.TotalValue = (double)CustomSummary[((GridSummaryItem)e.SummaryItem).FieldName];
            }

        }
    }
}
