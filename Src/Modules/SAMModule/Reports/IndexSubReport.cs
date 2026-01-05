using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Emdep.Geos.Modules.SAM.Reports
{
    public partial class IndexSubReport : DevExpress.XtraReports.UI.XtraReport
    {
        private DocumentsIdentificationReport _mainReport;
        public IndexSubReport(DocumentsIdentificationReport mainReport)
        {
            _mainReport = mainReport;
            InitializeComponent();
        }

        private void IndexSubReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (_mainReport != null)
            {
                _mainReport.UpdateTitle("Index Page");
            }
        }
    }
}
