using DevExpress.Data;
using DevExpress.Data.Summary;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Emdep.Geos.Modules.SRM.Views
{
    /// <summary>
    /// Interaction logic for PendingReviewView.xaml
    /// </summary>
    public partial class PendingReviewView : UserControl
    {
        public PendingReviewView()
        {
            InitializeComponent();
        }

        //private void GridControl_CustomSummary(object sender, CustomSummaryEventArgs e)
        //{
        //    DevExpress.Xpf.Grid.GridCustomSummaryEventArgs eve = (DevExpress.Xpf.Grid.GridCustomSummaryEventArgs)e;
        //    try
        //    {
        //        if (e.SummaryProcess == CustomSummaryProcess.Finalize)
        //        {
        //            if (e.Item == srmProgressSummary)
        //            {
        //                var reviewedTotalCountSummary = gridControl.GetTotalSummaryValue(ReviewedTotalCountSummary);
        //                var summaryReviewed = gridControl.GetTotalSummaryValue(reviewedSummary);

        //                if (reviewedTotalCountSummary != null && summaryReviewed != null)
        //                {
        //                    decimal totalItemSum = Convert.ToDecimal(reviewedTotalCountSummary);
        //                    decimal reviewed = Convert.ToDecimal(summaryReviewed);

        //                    int percentage = totalItemSum != 0 ? (int)((reviewed / totalItemSum) * 100) : 0;
        //                    e.TotalValue = $"{percentage}%";
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
