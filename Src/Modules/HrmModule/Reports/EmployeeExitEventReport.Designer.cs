namespace Emdep.Geos.Modules.Hrm.Reports
{
    partial class EmployeeExitEventReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrExitEventPanel = new DevExpress.XtraReports.UI.XRPanel();
            this.xrExitEventReasonValueLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrExitEventRemarkValueLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrExitEventExitDateValueLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrExitEventReasonLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrExitEventRemarkLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrExitEventDateLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrExitEventExitLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrExitEventPanel,
            this.xrExitEventExitLabel});
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrExitEventPanel
            // 
            this.xrExitEventPanel.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrExitEventPanel.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrExitEventReasonValueLabel,
            this.xrExitEventRemarkValueLabel,
            this.xrExitEventExitDateValueLabel,
            this.xrExitEventReasonLabel,
            this.xrExitEventRemarkLabel,
            this.xrExitEventDateLabel});
            this.xrExitEventPanel.LocationFloat = new DevExpress.Utils.PointFloat(0F, 24.28058F);
            this.xrExitEventPanel.Name = "xrExitEventPanel";
            this.xrExitEventPanel.SizeF = new System.Drawing.SizeF(650F, 75.71942F);
            this.xrExitEventPanel.StylePriority.UseBorders = false;
            // 
            // xrExitEventReasonValueLabel
            // 
            this.xrExitEventReasonValueLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrExitEventReasonValueLabel.LocationFloat = new DevExpress.Utils.PointFloat(49.44751F, 42.71943F);
            this.xrExitEventReasonValueLabel.Multiline = true;
            this.xrExitEventReasonValueLabel.Name = "xrExitEventReasonValueLabel";
            this.xrExitEventReasonValueLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrExitEventReasonValueLabel.SizeF = new System.Drawing.SizeF(225.045F, 16.92986F);
            this.xrExitEventReasonValueLabel.StylePriority.UseBorders = false;
            // 
            // xrExitEventRemarkValueLabel
            // 
            this.xrExitEventRemarkValueLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrExitEventRemarkValueLabel.LocationFloat = new DevExpress.Utils.PointFloat(347.0575F, 9.999964F);
            this.xrExitEventRemarkValueLabel.Multiline = true;
            this.xrExitEventRemarkValueLabel.Name = "xrExitEventRemarkValueLabel";
            this.xrExitEventRemarkValueLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrExitEventRemarkValueLabel.SizeF = new System.Drawing.SizeF(292.9425F, 16.92985F);
            this.xrExitEventRemarkValueLabel.StylePriority.UseBorders = false;
            // 
            // xrExitEventExitDateValueLabel
            // 
            this.xrExitEventExitDateValueLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrExitEventExitDateValueLabel.LocationFloat = new DevExpress.Utils.PointFloat(49.44751F, 10.00002F);
            this.xrExitEventExitDateValueLabel.Multiline = true;
            this.xrExitEventExitDateValueLabel.Name = "xrExitEventExitDateValueLabel";
            this.xrExitEventExitDateValueLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrExitEventExitDateValueLabel.SizeF = new System.Drawing.SizeF(100F, 16.92985F);
            this.xrExitEventExitDateValueLabel.StylePriority.UseBorders = false;
            // 
            // xrExitEventReasonLabel
            // 
            this.xrExitEventReasonLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrExitEventReasonLabel.LocationFloat = new DevExpress.Utils.PointFloat(2.508286F, 42.71943F);
            this.xrExitEventReasonLabel.Multiline = true;
            this.xrExitEventReasonLabel.Name = "xrExitEventReasonLabel";
            this.xrExitEventReasonLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrExitEventReasonLabel.SizeF = new System.Drawing.SizeF(48.93056F, 23F);
            this.xrExitEventReasonLabel.StylePriority.UseBorders = false;
            this.xrExitEventReasonLabel.Text = "Reason:";
            // 
            // xrExitEventRemarkLabel
            // 
            this.xrExitEventRemarkLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrExitEventRemarkLabel.LocationFloat = new DevExpress.Utils.PointFloat(289.6675F, 10.00002F);
            this.xrExitEventRemarkLabel.Multiline = true;
            this.xrExitEventRemarkLabel.Name = "xrExitEventRemarkLabel";
            this.xrExitEventRemarkLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrExitEventRemarkLabel.SizeF = new System.Drawing.SizeF(57.38998F, 16.92985F);
            this.xrExitEventRemarkLabel.StylePriority.UseBorders = false;
            this.xrExitEventRemarkLabel.Text = "Remarks:";
            // 
            // xrExitEventDateLabel
            // 
            this.xrExitEventDateLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrExitEventDateLabel.LocationFloat = new DevExpress.Utils.PointFloat(2.508286F, 9.999964F);
            this.xrExitEventDateLabel.Multiline = true;
            this.xrExitEventDateLabel.Name = "xrExitEventDateLabel";
            this.xrExitEventDateLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrExitEventDateLabel.SizeF = new System.Drawing.SizeF(38.6241F, 16.92985F);
            this.xrExitEventDateLabel.StylePriority.UseBorders = false;
            this.xrExitEventDateLabel.Text = "Date:";
            // 
            // xrExitEventExitLabel
            // 
            this.xrExitEventExitLabel.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrExitEventExitLabel.Multiline = true;
            this.xrExitEventExitLabel.Name = "xrExitEventExitLabel";
            this.xrExitEventExitLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrExitEventExitLabel.SizeF = new System.Drawing.SizeF(41.13239F, 24.28058F);
            this.xrExitEventExitLabel.Text = "EXIT";
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // EmployeeExitEventReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        public DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.XRPanel xrExitEventPanel;
        public DevExpress.XtraReports.UI.XRLabel xrExitEventReasonValueLabel;
        public DevExpress.XtraReports.UI.XRLabel xrExitEventRemarkValueLabel;
        public DevExpress.XtraReports.UI.XRLabel xrExitEventExitDateValueLabel;
        public DevExpress.XtraReports.UI.XRLabel xrExitEventReasonLabel;
        public DevExpress.XtraReports.UI.XRLabel xrExitEventRemarkLabel;
        public DevExpress.XtraReports.UI.XRLabel xrExitEventDateLabel;
        public DevExpress.XtraReports.UI.XRLabel xrExitEventExitLabel;
    }
}
