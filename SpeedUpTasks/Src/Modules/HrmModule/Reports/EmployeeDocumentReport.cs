using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Emdep.Geos.Modules.Hrm.Reports
{
    public partial class EmployeeDocumentReport : DevExpress.XtraReports.UI.XtraReport
    {
        public EmployeeDocumentReport()
        {
            InitializeComponent();
        }

        private void xrLabel4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
