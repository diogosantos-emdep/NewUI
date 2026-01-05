namespace Emdep.Geos.Modules.Warehouse.Reports
{
    partial class WarehoseLocationsLabelReport
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
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator2 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPanel = new DevExpress.XtraReports.UI.XRPanel();
            this.xrImage = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrRefrence = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlblLocation2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRefBarCode = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrlblLocation1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlblLocation3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLocationBarCode = new DevExpress.XtraReports.UI.XRBarCode();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
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
            this.xrImage,
            this.xrRefrence,
            this.xrlblLocation2,
            this.xrRefBarCode,
            this.xrlblLocation1,
            this.xrlblLocation3,
            this.xrLocationBarCode});
            this.xrPanel.LocationFloat = new DevExpress.Utils.PointFloat(8.999999F, 25F);
            this.xrPanel.Name = "xrPanel";
            this.xrPanel.SizeF = new System.Drawing.SizeF(668.97F, 202.9299F);
            this.xrPanel.StylePriority.UseBorders = false;
            this.xrPanel.StylePriority.UseBorderWidth = false;
            // 
            // xrImage
            // 
            this.xrImage.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageSource", "[ArticleImageInBytes]")});
            this.xrImage.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleLeft;
            this.xrImage.LocationFloat = new DevExpress.Utils.PointFloat(495.1946F, 0.7083569F);
            this.xrImage.Name = "xrImage";
            this.xrImage.SizeF = new System.Drawing.SizeF(173.7753F, 202.2216F);
            this.xrImage.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // xrRefrence
            // 
            this.xrRefrence.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrRefrence.BorderWidth = 2F;
            this.xrRefrence.CanGrow = false;
            this.xrRefrence.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Reference]")});
            this.xrRefrence.Font = new System.Drawing.Font("Calibri", 32F, System.Drawing.FontStyle.Bold);
            this.xrRefrence.LocationFloat = new DevExpress.Utils.PointFloat(2.000001F, 52.70837F);
            this.xrRefrence.Name = "xrRefrence";
            this.xrRefrence.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrRefrence.SizeF = new System.Drawing.SizeF(493.1947F, 75.43159F);
            this.xrRefrence.StylePriority.UseBorders = false;
            this.xrRefrence.StylePriority.UseBorderWidth = false;
            this.xrRefrence.StylePriority.UseFont = false;
            this.xrRefrence.StylePriority.UsePadding = false;
            this.xrRefrence.StylePriority.UseTextAlignment = false;
            this.xrRefrence.Text = "xrRefrence";
            this.xrRefrence.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrRefrence.TextFitMode = DevExpress.XtraReports.UI.TextFitMode.ShrinkAndGrow;
            // 
            // xrlblLocation2
            // 
            this.xrlblLocation2.BackColor = System.Drawing.Color.Transparent;
            this.xrlblLocation2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrlblLocation2.BorderWidth = 2F;
            this.xrlblLocation2.CanGrow = false;
            this.xrlblLocation2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ArticleWarehouseLocation].[WarehouseLocation].[LevelSecondName]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ForeColor", "Iif([ArticleWarehouseLocation].[WarehouseLocation].[HtmlColor]==\'#000000\', \'#FFFF" +
                    "FF\', \'#000000\')\n\n\n\n"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "BackColor", " [ArticleWarehouseLocation].[WarehouseLocation].[HtmlColor]")});
            this.xrlblLocation2.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold);
            this.xrlblLocation2.LocationFloat = new DevExpress.Utils.PointFloat(55F, 0.0417099F);
            this.xrlblLocation2.Name = "xrlblLocation2";
            this.xrlblLocation2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrlblLocation2.SizeF = new System.Drawing.SizeF(55F, 52F);
            this.xrlblLocation2.StylePriority.UseBackColor = false;
            this.xrlblLocation2.StylePriority.UseBorders = false;
            this.xrlblLocation2.StylePriority.UseBorderWidth = false;
            this.xrlblLocation2.StylePriority.UseFont = false;
            this.xrlblLocation2.StylePriority.UsePadding = false;
            this.xrlblLocation2.StylePriority.UseTextAlignment = false;
            this.xrlblLocation2.Text = "xrlblLocation2";
            this.xrlblLocation2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrlblLocation2.TextFitMode = DevExpress.XtraReports.UI.TextFitMode.ShrinkAndGrow;
            // 
            // xrRefBarCode
            // 
            this.xrRefBarCode.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrRefBarCode.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrRefBarCode.BorderWidth = 2F;
            this.xrRefBarCode.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Reference]")});
            this.xrRefBarCode.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrRefBarCode.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 128.14F);
            this.xrRefBarCode.Module = 1.7F;
            this.xrRefBarCode.Name = "xrRefBarCode";
            this.xrRefBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 10, 5, 5, 100F);
            this.xrRefBarCode.ShowText = false;
            this.xrRefBarCode.SizeF = new System.Drawing.SizeF(493.1947F, 74.79001F);
            this.xrRefBarCode.StylePriority.UseBorders = false;
            this.xrRefBarCode.StylePriority.UseBorderWidth = false;
            this.xrRefBarCode.StylePriority.UseFont = false;
            this.xrRefBarCode.StylePriority.UsePadding = false;
            this.xrRefBarCode.StylePriority.UseTextAlignment = false;
            this.xrRefBarCode.Symbology = code128Generator1;
            this.xrRefBarCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrlblLocation1
            // 
            this.xrlblLocation1.BackColor = System.Drawing.Color.Transparent;
            this.xrlblLocation1.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrlblLocation1.BorderWidth = 2F;
            this.xrlblLocation1.CanGrow = false;
            this.xrlblLocation1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ArticleWarehouseLocation].[WarehouseLocation].[LevelFirstName]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "BackColor", " [ArticleWarehouseLocation].[WarehouseLocation].[HtmlColor]\n\n"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ForeColor", "Iif([ArticleWarehouseLocation].[WarehouseLocation].[HtmlColor]==\'#000000\', \'#FFFF" +
                    "FF\', \'#000000\')\n\n\n")});
            this.xrlblLocation1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlblLocation1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrlblLocation1.Name = "xrlblLocation1";
            this.xrlblLocation1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrlblLocation1.SizeF = new System.Drawing.SizeF(55F, 52F);
            this.xrlblLocation1.StylePriority.UseBackColor = false;
            this.xrlblLocation1.StylePriority.UseBorders = false;
            this.xrlblLocation1.StylePriority.UseBorderWidth = false;
            this.xrlblLocation1.StylePriority.UseFont = false;
            this.xrlblLocation1.StylePriority.UsePadding = false;
            this.xrlblLocation1.StylePriority.UseTextAlignment = false;
            this.xrlblLocation1.Text = "xrlblLocation1";
            this.xrlblLocation1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrlblLocation1.TextFitMode = DevExpress.XtraReports.UI.TextFitMode.ShrinkAndGrow;
            // 
            // xrlblLocation3
            // 
            this.xrlblLocation3.BackColor = System.Drawing.Color.Black;
            this.xrlblLocation3.BorderColor = System.Drawing.Color.Black;
            this.xrlblLocation3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrlblLocation3.BorderWidth = 2F;
            this.xrlblLocation3.CanGrow = false;
            this.xrlblLocation3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ArticleWarehouseLocation].[WarehouseLocation].[LevelThirdName]")});
            this.xrlblLocation3.Font = new System.Drawing.Font("Calibri", 25F, System.Drawing.FontStyle.Bold);
            this.xrlblLocation3.ForeColor = System.Drawing.Color.White;
            this.xrlblLocation3.LocationFloat = new DevExpress.Utils.PointFloat(110F, 0.7083569F);
            this.xrlblLocation3.Name = "xrlblLocation3";
            this.xrlblLocation3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlblLocation3.SizeF = new System.Drawing.SizeF(150F, 52F);
            this.xrlblLocation3.StylePriority.UseBackColor = false;
            this.xrlblLocation3.StylePriority.UseBorderColor = false;
            this.xrlblLocation3.StylePriority.UseBorders = false;
            this.xrlblLocation3.StylePriority.UseBorderWidth = false;
            this.xrlblLocation3.StylePriority.UseFont = false;
            this.xrlblLocation3.StylePriority.UseForeColor = false;
            this.xrlblLocation3.StylePriority.UseTextAlignment = false;
            this.xrlblLocation3.Text = "xrlblLocation3";
            this.xrlblLocation3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrlblLocation3.TextFitMode = DevExpress.XtraReports.UI.TextFitMode.ShrinkAndGrow;
            this.xrlblLocation3.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrlblLocation3_BeforePrint);
            // 
            // xrLocationBarCode
            // 
            this.xrLocationBarCode.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLocationBarCode.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLocationBarCode.BorderWidth = 2F;
            this.xrLocationBarCode.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ArticleWarehouseLocation].[WarehouseLocation].[FullName]")});
            this.xrLocationBarCode.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLocationBarCode.LocationFloat = new DevExpress.Utils.PointFloat(260F, 0F);
            this.xrLocationBarCode.Module = 1.7F;
            this.xrLocationBarCode.Name = "xrLocationBarCode";
            this.xrLocationBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 4, 4, 100F);
            this.xrLocationBarCode.ShowText = false;
            this.xrLocationBarCode.SizeF = new System.Drawing.SizeF(235.1947F, 52F);
            this.xrLocationBarCode.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 1, 1, 100F);
            this.xrLocationBarCode.StylePriority.UseBorders = false;
            this.xrLocationBarCode.StylePriority.UseBorderWidth = false;
            this.xrLocationBarCode.StylePriority.UseFont = false;
            this.xrLocationBarCode.StylePriority.UseTextAlignment = false;
            this.xrLocationBarCode.Symbology = code128Generator2;
            this.xrLocationBarCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLocationBarCode.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLocationBarCode_BeforePrint);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 21F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 5.291653F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // WarehoseLocationsLabelReport
            // 
            this.BackColor = System.Drawing.Color.Empty;
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(100, 25, 21, 5);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
            this.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        //private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        public DevExpress.XtraReports.UI.XRBarCode xrLocationBarCode;
        public DevExpress.XtraReports.UI.XRLabel xrlblLocation1;
        public DevExpress.XtraReports.UI.XRPanel xrPanel;
        public DevExpress.XtraReports.UI.XRLabel xrlblLocation3;
        public DevExpress.XtraReports.UI.XRBarCode xrRefBarCode;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        public DevExpress.XtraReports.UI.XRLabel xrlblLocation2;
        public DevExpress.XtraReports.UI.XRLabel xrRefrence;
        public DevExpress.XtraReports.UI.XRPictureBox xrImage;
        //private System.Windows.Forms.BindingSource WarehouseLocationSource;
        //private System.Windows.Forms.BindingSource ArticleSource;
        //private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource2;
        //private System.Windows.Forms.BindingSource objectDataSource2;
        //private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        //private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource3;
        //private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource4;
        //private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource3;
    }
}
