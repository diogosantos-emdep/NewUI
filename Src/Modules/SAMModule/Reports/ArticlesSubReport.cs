using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Emdep.Geos.Modules.SAM.Reports
{
    public partial class ArticlesSubReport : DevExpress.XtraReports.UI.XtraReport
    {
        private DocumentsIdentificationReport _mainReport;

        public ArticlesSubReport(DocumentsIdentificationReport mainReport)
        {
            _mainReport = mainReport;
            InitializeComponent();
        }

        private void ArticlesSubReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (_mainReport != null)
            {
                _mainReport.UpdateTitle("Articles");
            }
        }
    }
}
