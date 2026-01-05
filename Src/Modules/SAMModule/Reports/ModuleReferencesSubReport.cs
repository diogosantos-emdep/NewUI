using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Emdep.Geos.Modules.SAM.Reports
{
    public partial class ModuleReferencesSubReport : DevExpress.XtraReports.UI.XtraReport
    {
        private DocumentsIdentificationReport _mainReport;

        public ModuleReferencesSubReport(DocumentsIdentificationReport mainReport)
        {
            _mainReport = mainReport;
            InitializeComponent();
        }

        private void pbEcos_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRPictureBox pictureBox = sender as XRPictureBox;
            if (pictureBox != null)
            {
                string navigateUrl = GetCurrentColumnValue("EcosNavigateUrl") as string;
                
                if (!string.IsNullOrEmpty(navigateUrl))
                {
                    pictureBox.NavigateUrl = navigateUrl; 
                }
            }
        }

        private void ModuleReferencesSubReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (_mainReport != null)
            {
                _mainReport.UpdateTitle("Pneumatic Modules");
            }
        }
    }
    
}
