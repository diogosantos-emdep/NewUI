using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Emdep.Geos.Modules.SAM.Reports
{
    public partial class TechnicalPropertiesSubReport : DevExpress.XtraReports.UI.XtraReport
    {
        private DocumentsIdentificationReport _mainReport;

        public TechnicalPropertiesSubReport(DocumentsIdentificationReport mainReport)
        {
            _mainReport = mainReport;
            InitializeComponent();
        }

        private void DetectionCount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if(((DevExpress.XtraReports.UI.XRTableCell)sender).Row.Cells[1].Text == "")
            {
                ((DevExpress.XtraReports.UI.XRTableCell)sender).Row.Cells[2].Borders = DevExpress.XtraPrinting.BorderSide.None;
            }
            else
            {
                ((DevExpress.XtraReports.UI.XRTableCell)sender).Row.Cells[2].Borders = DevExpress.XtraPrinting.BorderSide.All;
            }
        }

        private void TechnicalPropertiesSubReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (_mainReport != null)
            {
                _mainReport.UpdateTitle("Technical Specifications");
            }
        }
    }
}
