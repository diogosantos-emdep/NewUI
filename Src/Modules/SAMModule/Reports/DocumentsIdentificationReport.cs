using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Emdep.Geos.Modules.SAM.Reports
{
    public partial class DocumentsIdentificationReport : DevExpress.XtraReports.UI.XtraReport
    {
        bool firstPagePrinted = false;
        bool lastPagePrinted = false;
        public DocumentsIdentificationReport()
        {
            InitializeComponent();
        }
        public void UpdateTitle(string title)
        {
            xrTitleDatasheet.Text = title;
        }

        private void rptIndex_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        }

        private void pbUserGuideImage_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //xrTitleDatasheet.Text = "";
        }

        private void xrTitleDatasheet_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //if (xrTitleDatasheet.Text.ToUpper() == "TECHNICAL SPECIFICATIONS" && !firstPagePrinted)
            //{
            //    xrTitleDatasheet.Text = "";
            //    firstPagePrinted = true;
            //}
            //if(xrTitleDatasheet.Text.ToUpper() == "DECLARATION OF CONFORMITY" && !lastPagePrinted)
            //{
            //    xrTitleDatasheet.Text = "Index Page";
            //    lastPagePrinted = true;
            //}
        }

        private void xrTitleDatasheet_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            XRLabel label = sender as XRLabel;
            if (label != null)
            {
                int pageNumber = e.PageIndex;
                switch (pageNumber)
                {
                    case 0:
                        label.Text = "";
                        break;
                    case 1:
                        label.Text = "Index Page";
                        break;
                }
            }
        }
    }
}
