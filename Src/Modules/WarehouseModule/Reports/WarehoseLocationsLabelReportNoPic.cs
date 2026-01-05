using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.BarCode;

namespace Emdep.Geos.Modules.Warehouse.Reports
{
    public partial class WarehoseLocationsLabelReportNoPic : DevExpress.XtraReports.UI.XtraReport
    {
        public WarehoseLocationsLabelReportNoPic()
        {
            InitializeComponent();
        }

        private void xrLocationBarCode_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            // Obtain the current label. 
            XRBarCode label = (XRBarCode)sender;
            label.AutoModule = false;

            BarCodeError berror = label.Validate();
            if (berror == BarCodeError.ControlBoundsTooSmall)
            {
                label.AutoModule = true;
            }
        }

        private void xrlblLocation3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = (XRLabel)sender;

            if (label.Text == null || label.Text == string.Empty)
            {
                return;
            }
            else if (label.WidthF == 118 && label.Text.Length == 4)
            {
                label.Font = new Font(label.Font.FontFamily, 33, label.Font.Style);
            }
            else if (label.WidthF == 118 && label.Text.Length == 5)
            {
                label.Font = new Font(label.Font.FontFamily, 26.8F, label.Font.Style);
            }
            else
            {
                AutoscaleControlText(label);
            }
        }

        public void AutoscaleControlText(XRLabel control)
        {
            float originalHeight = GetTextHeight(control);
            if (originalHeight > control.HeightF)
            {
                do
                {
                    control.Font = new Font(control.Font.FontFamily, control.Font.Size - 0.1f, control.Font.Style);
                } while (control.HeightF < GetTextHeight(control));
            }
            else
            {
                do
                {
                    control.Font = new Font(control.Font.FontFamily, control.Font.Size + 0.1f, System.Drawing.FontStyle.Bold);
                } while (control.HeightF > GetTextHeight(control));
            }
        }

        public float GetTextHeight(XRControl label)
        {
            StringFormat format = (StringFormat)StringFormat.GenericTypographic.Clone();
            format.FormatFlags = System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.LineLimit | System.Drawing.StringFormatFlags.NoClip;

            String text = label.Text;
            SizeF textSize = SizeF.Empty;
            float height = 0.0F;

            switch (this.ReportUnit)
            {
                case DevExpress.XtraReports.UI.ReportUnit.HundredthsOfAnInch:
                    textSize = BrickGraphics.MeasureString(text, label.Font, (int)(label.Width / 100), format, GraphicsUnit.Inch);
                    height = textSize.Height * 100;
                    break;
                case DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter:
                    textSize = BrickGraphics.MeasureString(text, label.Font, (int)(label.Width / 10), format, GraphicsUnit.Inch);
                    height = textSize.Height * 10;
                    break;
                case DevExpress.XtraReports.UI.ReportUnit.Pixels:
                    textSize = BrickGraphics.MeasureString(text, label.Font, (int)(label.Width), format, GraphicsUnit.Pixel);
                    height = textSize.Height;
                    break;
            }

            return height;
        }
    }
}
