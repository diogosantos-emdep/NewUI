namespace Emdep.Geos.Modules.Warehouse.Reports
{
    partial class ArticlesLabelReport
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrMainPanel = new DevExpress.XtraReports.UI.XRPanel();
            this.xrArticleImage = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrArticleBarcode = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrlblArticleReference = new DevExpress.XtraReports.UI.XRLabel();
            this.ArticleSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ArticleSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 20.75F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 4F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrMainPanel});
            this.Detail.HeightF = 241.0967F;
            this.Detail.Name = "Detail";
            // 
            // xrMainPanel
            // 
            this.xrMainPanel.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrMainPanel.BorderWidth = 2F;
            this.xrMainPanel.CanGrow = false;
            this.xrMainPanel.CanShrink = true;
            this.xrMainPanel.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrArticleImage,
            this.xrArticleBarcode,
            this.xrlblArticleReference});
            this.xrMainPanel.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 13.41667F);
            this.xrMainPanel.Name = "xrMainPanel";
            this.xrMainPanel.SizeF = new System.Drawing.SizeF(668.973F, 215.43F);
            this.xrMainPanel.StylePriority.UseBorders = false;
            this.xrMainPanel.StylePriority.UseBorderWidth = false;
            // 
            // xrArticleImage
            // 
            this.xrArticleImage.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrArticleImage.BorderWidth = 2F;
            this.xrArticleImage.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageSource", "[ArticleImageInBytes]")});
            this.xrArticleImage.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
            this.xrArticleImage.LocationFloat = new DevExpress.Utils.PointFloat(536.6929F, 0.4299927F);
            this.xrArticleImage.Name = "xrArticleImage";
            this.xrArticleImage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrArticleImage.SizeF = new System.Drawing.SizeF(132.28F, 215F);
            this.xrArticleImage.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.xrArticleImage.StylePriority.UseBorders = false;
            this.xrArticleImage.StylePriority.UseBorderWidth = false;
            this.xrArticleImage.StylePriority.UsePadding = false;
            // 
            // xrArticleBarcode
            // 
            this.xrArticleBarcode.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrArticleBarcode.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrArticleBarcode.BorderWidth = 0F;
            this.xrArticleBarcode.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Reference]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Reference]")});
            this.xrArticleBarcode.Font = new System.Drawing.Font("Calibri", 15.75F);
            this.xrArticleBarcode.LocationFloat = new DevExpress.Utils.PointFloat(0F, 107.7165F);
            this.xrArticleBarcode.Name = "xrArticleBarcode";
            this.xrArticleBarcode.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 10, 10, 100F);
            this.xrArticleBarcode.ShowText = false;
            this.xrArticleBarcode.SizeF = new System.Drawing.SizeF(536.69F, 107.72F);
            this.xrArticleBarcode.StylePriority.UseBorders = false;
            this.xrArticleBarcode.StylePriority.UseBorderWidth = false;
            this.xrArticleBarcode.StylePriority.UseFont = false;
            this.xrArticleBarcode.StylePriority.UsePadding = false;
            this.xrArticleBarcode.StylePriority.UseTextAlignment = false;
            this.xrArticleBarcode.Symbology = code128Generator1;
            this.xrArticleBarcode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlblArticleReference
            // 
            this.xrlblArticleReference.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrlblArticleReference.BorderWidth = 2F;
            this.xrlblArticleReference.CanGrow = false;
            this.xrlblArticleReference.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Reference]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Reference]")});
            this.xrlblArticleReference.Font = new System.Drawing.Font("Calibri", 70F, System.Drawing.FontStyle.Bold);
            this.xrlblArticleReference.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrlblArticleReference.Multiline = true;
            this.xrlblArticleReference.Name = "xrlblArticleReference";
            this.xrlblArticleReference.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrlblArticleReference.SizeF = new System.Drawing.SizeF(536.6929F, 107.7165F);
            this.xrlblArticleReference.StylePriority.UseBackColor = false;
            this.xrlblArticleReference.StylePriority.UseBorders = false;
            this.xrlblArticleReference.StylePriority.UseBorderWidth = false;
            this.xrlblArticleReference.StylePriority.UseFont = false;
            this.xrlblArticleReference.StylePriority.UsePadding = false;
            this.xrlblArticleReference.StylePriority.UseTextAlignment = false;
            this.xrlblArticleReference.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrlblArticleReference.TextFitMode = DevExpress.XtraReports.UI.TextFitMode.ShrinkAndGrow;
            // 
            // ArticleSource
            // 
            this.ArticleSource.DataSource = typeof(Emdep.Geos.Data.Common.Article);
            // 
            // ArticlesLabelReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.ArticleSource});
            this.DataSource = this.ArticleSource;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(100, 25, 21, 4);
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this.ArticleSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.XRPanel xrMainPanel;
        public DevExpress.XtraReports.UI.XRBarCode xrArticleBarcode;
        public DevExpress.XtraReports.UI.XRLabel xrlblArticleReference;
        private System.Windows.Forms.BindingSource ArticleSource;
        public DevExpress.XtraReports.UI.XRPictureBox xrArticleImage;
        public DevExpress.XtraReports.UI.DetailBand Detail;
    }
}
