using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace ActivityReminder.Reports
{
    public partial class ERMMonthlyProductionTimelinePdfEmailReport : DevExpress.XtraReports.UI.XtraReport
    {
        public ERMMonthlyProductionTimelinePdfEmailReport()
        {
            InitializeComponent();
        }

        private void xrCHRowOvertime1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRChart chart = (XRChart)cell.Controls[0];

            chart.WidthF = cell.WidthF;
            //chart.HeightF = cell.HeightF;

            chart.CustomDrawAxisLabel += (s, args) =>
            {
                args.Item.Text = args.Item.Text.Replace("'", ""); // remove single quotes
            };


        }
    }
}
