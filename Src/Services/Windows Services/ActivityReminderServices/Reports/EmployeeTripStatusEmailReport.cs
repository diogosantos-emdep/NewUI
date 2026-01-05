using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;

namespace ActivityReminder
{
    public partial class EmployeeTripStatusEmailReport : DevExpress.XtraReports.UI.XtraReport
    {
        public EmployeeTripStatusEmailReport()
        {
            InitializeComponent();
            XRShape pageBorder = new XRShape
            {
                Shape = new ShapeRectangle(),
                LineWidth = 1, // Border thickness
                ForeColor = Color.Black, // Border color
                BoundsF = new RectangleF(0, 0, this.PageWidth - this.Margins.Left - this.Margins.Right, this.PageHeight - this.Margins.Top - this.Margins.Bottom)
            };

            // Add the shape to the report's Detail band or any other band
            this.Bands[BandKind.Detail].Controls.Add(pageBorder);
        }

    }
}
