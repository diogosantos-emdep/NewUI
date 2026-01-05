using DevExpress.Data;
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

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for ReportDashboard2.xaml
    /// </summary>
    public partial class ReportDashboard2 : UserControl
    {
        public ReportDashboard2()
        {
            InitializeComponent();
        }

        double emptyCellsTotalCount = 0;
        double? sum = 0;

        private void MyGridControl_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (((GridSummaryItem)e.Item).FieldName == "TARGET_Percentage" && e.IsTotalSummary)
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    emptyCellsTotalCount = 0;
                    sum = 0;
                }
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    double? val = (double?)e.FieldValue;
                    if (val > 0)
                    {
                        sum = sum + val;
                        emptyCellsTotalCount++;
                    }
                }

                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (emptyCellsTotalCount == 0)
                    {
                        e.TotalValue = 0;
                    }
                    else
                    {
                        double finalSum = Math.Round(sum.Value / emptyCellsTotalCount, 2);

                        e.TotalValue = finalSum;
                    }
                }
            }
        }
    }
}
