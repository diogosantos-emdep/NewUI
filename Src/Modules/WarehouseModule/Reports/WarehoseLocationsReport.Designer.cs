namespace Emdep.Geos.Modules.Warehouse.Reports
{
    partial class WarehoseLocationsReport
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            code128Generator1.CharacterSet = DevExpress.XtraPrinting.BarCode.Code128Charset.CharsetAuto;

            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPanel = new DevExpress.XtraReports.UI.XRPanel();
            this.xrlblLocation1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlblLocation2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlblLocation3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBoxDirection = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLocationBarCode = new DevExpress.XtraReports.UI.XRBarCode();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel});
            this.Detail.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Detail.HeightF = 260F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseFont = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel
            // 
            this.xrPanel.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel.BorderWidth = 2F;
            this.xrPanel.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlblLocation1,
            this.xrlblLocation2,
            this.xrlblLocation3,
            this.xrPictureBoxDirection,
            this.xrLocationBarCode});
            this.xrPanel.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 25F);
            this.xrPanel.Name = "xrPanel";
            this.xrPanel.SizeF = new System.Drawing.SizeF(786F, 213.8783F);
            this.xrPanel.StylePriority.UseBorders = false;
            this.xrPanel.StylePriority.UseBorderWidth = false;
            // 
            // xrlblLocation1
            // 
            this.xrlblLocation1.BackColor = System.Drawing.Color.Transparent;
            this.xrlblLocation1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrlblLocation1.BorderWidth = 2F;
            this.xrlblLocation1.CanGrow = false;
            this.xrlblLocation1.CanShrink = true;
            this.xrlblLocation1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LocationParent]")});
            this.xrlblLocation1.Font = new System.Drawing.Font("Calibri", 70F, System.Drawing.FontStyle.Bold);
            this.xrlblLocation1.LocationFloat = new DevExpress.Utils.PointFloat(1.210052F, 0F);
            this.xrlblLocation1.Name = "xrlblLocation1";
            this.xrlblLocation1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlblLocation1.SizeF = new System.Drawing.SizeF(185F, 106.96F);
            this.xrlblLocation1.StylePriority.UseBackColor = false;
            this.xrlblLocation1.StylePriority.UseBorders = false;
            this.xrlblLocation1.StylePriority.UseBorderWidth = false;
            this.xrlblLocation1.StylePriority.UseFont = false;
            this.xrlblLocation1.StylePriority.UseTextAlignment = false;
            this.xrlblLocation1.Text = "xrlblLocation1";
            this.xrlblLocation1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrlblLocation2
            // 
            this.xrlblLocation2.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrlblLocation2.BorderWidth = 2F;
            this.xrlblLocation2.CanGrow = false;
            this.xrlblLocation2.CanShrink = true;
            this.xrlblLocation2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Locationchild1]")});
            this.xrlblLocation2.Font = new System.Drawing.Font("Calibri", 70F, System.Drawing.FontStyle.Bold);
            this.xrlblLocation2.LocationFloat = new DevExpress.Utils.PointFloat(186.2101F, 0F);
            this.xrlblLocation2.Name = "xrlblLocation2";
            this.xrlblLocation2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlblLocation2.SizeF = new System.Drawing.SizeF(185F, 106.96F);
            this.xrlblLocation2.StylePriority.UseBorders = false;
            this.xrlblLocation2.StylePriority.UseBorderWidth = false;
            this.xrlblLocation2.StylePriority.UseFont = false;
            this.xrlblLocation2.StylePriority.UseTextAlignment = false;
            this.xrlblLocation2.Text = "xrlblLocation2";
            this.xrlblLocation2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrlblLocation3
            // 
            this.xrlblLocation3.BackColor = System.Drawing.Color.Black;
            this.xrlblLocation3.BorderColor = System.Drawing.Color.Black;
            this.xrlblLocation3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrlblLocation3.BorderWidth = 2F;
            this.xrlblLocation3.CanGrow = false;
            this.xrlblLocation3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Locationchild2]")});
            this.xrlblLocation3.Font = new System.Drawing.Font("Calibri", 100F, System.Drawing.FontStyle.Bold);
            this.xrlblLocation3.ForeColor = System.Drawing.Color.White;
            this.xrlblLocation3.LocationFloat = new DevExpress.Utils.PointFloat(371.2101F, 0F);
            this.xrlblLocation3.Name = "xrlblLocation3";
            this.xrlblLocation3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlblLocation3.SizeF = new System.Drawing.SizeF(250.72F, 213.92F);
            this.xrlblLocation3.StylePriority.UseBackColor = false;
            this.xrlblLocation3.StylePriority.UseBorderColor = false;
            this.xrlblLocation3.StylePriority.UseBorders = false;
            this.xrlblLocation3.StylePriority.UseBorderWidth = false;
            this.xrlblLocation3.StylePriority.UseFont = false;
            this.xrlblLocation3.StylePriority.UseForeColor = false;
            this.xrlblLocation3.StylePriority.UseTextAlignment = false;
            this.xrlblLocation3.Text = "xrlblLocation3";
            this.xrlblLocation3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrlblLocation3.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrlblLocation3_BeforePrint);
            // 
            // xrPictureBoxDirection
            // 
            this.xrPictureBoxDirection.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPictureBoxDirection.BorderWidth = 2F;
            this.xrPictureBoxDirection.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageSource", "[ImageDirection].[BitmapDirection]")});
            this.xrPictureBoxDirection.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
            this.xrPictureBoxDirection.LocationFloat = new DevExpress.Utils.PointFloat(621.9301F, 0F);
            this.xrPictureBoxDirection.Name = "xrPictureBoxDirection";
            this.xrPictureBoxDirection.SizeF = new System.Drawing.SizeF(164.07F, 213.92F);
            this.xrPictureBoxDirection.StylePriority.UseBorders = false;
            this.xrPictureBoxDirection.StylePriority.UseBorderWidth = false;
            // 
            // xrLocationBarCode
            // 
            this.xrLocationBarCode.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLocationBarCode.BorderWidth = 2F;
            this.xrLocationBarCode.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FullName]")});
            this.xrLocationBarCode.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLocationBarCode.LocationFloat = new DevExpress.Utils.PointFloat(1.210054F, 106.96F);
            this.xrLocationBarCode.Module = 1.7F;
            this.xrLocationBarCode.Name = "xrLocationBarCode";
            this.xrLocationBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 10, 10, 10, 100F);
            this.xrLocationBarCode.ShowText = false;
            this.xrLocationBarCode.SizeF = new System.Drawing.SizeF(369.9999F, 106.96F);
            this.xrLocationBarCode.StylePriority.UseBorders = false;
            this.xrLocationBarCode.StylePriority.UseBorderWidth = false;
            this.xrLocationBarCode.StylePriority.UseFont = false;
            this.xrLocationBarCode.StylePriority.UsePadding = false;
            this.xrLocationBarCode.StylePriority.UseTextAlignment = false;
            this.xrLocationBarCode.Symbology = code128Generator1;
            this.xrLocationBarCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLocationBarCode.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLocationBarCode_BeforePrint);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 2F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Emdep.Geos.Data.Common.WarehouseLocation);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // WarehoseLocationsReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Margins = new System.Drawing.Printing.Margins(14, 30, 0, 2);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
            this.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        public DevExpress.XtraReports.UI.XRBarCode xrLocationBarCode;
        public DevExpress.XtraReports.UI.XRPictureBox xrPictureBoxDirection;
        public DevExpress.XtraReports.UI.XRLabel xrlblLocation1;
        public DevExpress.XtraReports.UI.XRLabel xrlblLocation3;
        public DevExpress.XtraReports.UI.XRLabel xrlblLocation2;
        public DevExpress.XtraReports.UI.XRPanel xrPanel;
        private DevExpress.XtraReports.UI.DetailBand Detail;
    }
}
