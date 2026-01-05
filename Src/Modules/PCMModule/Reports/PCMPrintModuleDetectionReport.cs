using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Emdep.Geos.Modules.PCM.Reports
{
    public partial class PCMPrintModuleDetectionReport : DevExpress.XtraReports.UI.XtraReport
    {
        public PCMPrintModuleDetectionReport()
        {
            InitializeComponent();
        }

        private void PCMPrintModuleDetectionReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
