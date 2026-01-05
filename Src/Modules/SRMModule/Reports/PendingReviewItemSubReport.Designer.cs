namespace Emdep.Geos.Modules.SRM.Reports
{
    partial class PendingReviewItemSubReport
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
            DevExpress.DataAccess.ObjectBinding.ObjectConstructorInfo objectConstructorInfo1 = new DevExpress.DataAccess.ObjectBinding.ObjectConstructorInfo();
            DevExpress.DataAccess.ObjectBinding.ObjectConstructorInfo objectConstructorInfo2 = new DevExpress.DataAccess.ObjectBinding.ObjectConstructorInfo();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrItemTextTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xritemBarCode = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrReferenceTextTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrDescriptionTextTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrUnitPriceTextTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrQtyTextTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalPriceTextTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrRemarksTextTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrItemHeaderTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrReferenceHeaderTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrDescriptionHeaderTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrUnitPriceHeaderTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrQtyHeaderTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalPriceHeaderTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrRemarksHeaderTableCell = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.objectDataSource2 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
            this.Detail.HeightF = 46.66666F;
            this.Detail.Name = "Detail";
            // 
            // xrTable4
            // 
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable4.SizeF = new System.Drawing.SizeF(1128F, 46.66666F);
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrItemTextTableCell,
            this.xrReferenceTextTableCell,
            this.xrDescriptionTextTableCell,
            this.xrUnitPriceTextTableCell,
            this.xrQtyTextTableCell,
            this.xrTotalPriceTextTableCell,
            this.xrRemarksTextTableCell});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrItemTextTableCell
            // 
            this.xrItemTextTableCell.BorderColor = System.Drawing.Color.LightGray;
            this.xrItemTextTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrItemTextTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xritemBarCode});
            this.xrItemTextTableCell.Multiline = true;
            this.xrItemTextTableCell.Name = "xrItemTextTableCell";
            this.xrItemTextTableCell.StylePriority.UseBorderColor = false;
            this.xrItemTextTableCell.StylePriority.UseBorders = false;
            this.xrItemTextTableCell.StylePriority.UseTextAlignment = false;
            this.xrItemTextTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrItemTextTableCell.Weight = 1.2677788914459358D;
            // 
            // xritemBarCode
            // 
            this.xritemBarCode.AutoModule = true;
            this.xritemBarCode.BorderColor = System.Drawing.Color.LightGray;
            this.xritemBarCode.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xritemBarCode.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NumItemBarCodeText]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif(IsNullOrEmpty([NumItemBarCodeText]), false, true)")});
            this.xritemBarCode.Font = new System.Drawing.Font("Arial", 10F);
            this.xritemBarCode.LocationFloat = new DevExpress.Utils.PointFloat(1.999942F, 16.25002F);
            this.xritemBarCode.Name = "xritemBarCode";
            this.xritemBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0, 100F);
            this.xritemBarCode.ShowText = false;
            this.xritemBarCode.SizeF = new System.Drawing.SizeF(189F, 25F);
            this.xritemBarCode.StylePriority.UseBorderColor = false;
            this.xritemBarCode.StylePriority.UseBorders = false;
            this.xritemBarCode.StylePriority.UseFont = false;
            this.xritemBarCode.StylePriority.UsePadding = false;
            this.xritemBarCode.StylePriority.UseTextAlignment = false;
            this.xritemBarCode.Symbology = code128Generator1;
            this.xritemBarCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrReferenceTextTableCell
            // 
            this.xrReferenceTextTableCell.BorderColor = System.Drawing.Color.LightGray;
            this.xrReferenceTextTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrReferenceTextTableCell.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RevisionItem].[WarehouseProduct].[Article].[Reference]")});
            this.xrReferenceTextTableCell.Name = "xrReferenceTextTableCell";
            this.xrReferenceTextTableCell.StylePriority.UseBorderColor = false;
            this.xrReferenceTextTableCell.StylePriority.UseBorders = false;
            this.xrReferenceTextTableCell.StylePriority.UseTextAlignment = false;
            this.xrReferenceTextTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrReferenceTextTableCell.Weight = 0.72381824466502576D;
            // 
            // xrDescriptionTextTableCell
            // 
            this.xrDescriptionTextTableCell.BorderColor = System.Drawing.Color.LightGray;
            this.xrDescriptionTextTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrDescriptionTextTableCell.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RevisionItem].[WarehouseProduct].[Article].[Description]")});
            this.xrDescriptionTextTableCell.Multiline = true;
            this.xrDescriptionTextTableCell.Name = "xrDescriptionTextTableCell";
            this.xrDescriptionTextTableCell.StylePriority.UseBorderColor = false;
            this.xrDescriptionTextTableCell.StylePriority.UseBorders = false;
            this.xrDescriptionTextTableCell.StylePriority.UseTextAlignment = false;
            this.xrDescriptionTextTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrDescriptionTextTableCell.Weight = 1.8586229592188444D;
            // 
            // xrUnitPriceTextTableCell
            // 
            this.xrUnitPriceTextTableCell.BorderColor = System.Drawing.Color.LightGray;
            this.xrUnitPriceTextTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrUnitPriceTextTableCell.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[UnitPriceWithSymbol]")});
            this.xrUnitPriceTextTableCell.Multiline = true;
            this.xrUnitPriceTextTableCell.Name = "xrUnitPriceTextTableCell";
            this.xrUnitPriceTextTableCell.StylePriority.UseBorderColor = false;
            this.xrUnitPriceTextTableCell.StylePriority.UseBorders = false;
            this.xrUnitPriceTextTableCell.StylePriority.UseTextAlignment = false;
            this.xrUnitPriceTextTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrUnitPriceTextTableCell.Weight = 0.68493551793132035D;
            // 
            // xrQtyTextTableCell
            // 
            this.xrQtyTextTableCell.BorderColor = System.Drawing.Color.LightGray;
            this.xrQtyTextTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrQtyTextTableCell.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RevisionItem].[Quantity]")});
            this.xrQtyTextTableCell.Multiline = true;
            this.xrQtyTextTableCell.Name = "xrQtyTextTableCell";
            this.xrQtyTextTableCell.StylePriority.UseBorderColor = false;
            this.xrQtyTextTableCell.StylePriority.UseBorders = false;
            this.xrQtyTextTableCell.StylePriority.UseTextAlignment = false;
            this.xrQtyTextTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrQtyTextTableCell.Weight = 0.47241205408741721D;
            // 
            // xrTotalPriceTextTableCell
            // 
            this.xrTotalPriceTextTableCell.BorderColor = System.Drawing.Color.LightGray;
            this.xrTotalPriceTextTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTotalPriceTextTableCell.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RevisionItem].[TotalPrice]")});
            this.xrTotalPriceTextTableCell.Multiline = true;
            this.xrTotalPriceTextTableCell.Name = "xrTotalPriceTextTableCell";
            this.xrTotalPriceTextTableCell.StylePriority.UseBorderColor = false;
            this.xrTotalPriceTextTableCell.StylePriority.UseBorders = false;
            this.xrTotalPriceTextTableCell.StylePriority.UseTextAlignment = false;
            this.xrTotalPriceTextTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTotalPriceTextTableCell.Weight = 1.0072658166143123D;
            // 
            // xrRemarksTextTableCell
            // 
            this.xrRemarksTextTableCell.BorderColor = System.Drawing.Color.LightGray;
            this.xrRemarksTextTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrRemarksTextTableCell.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RevisionItem].[ItemRemarks]")});
            this.xrRemarksTextTableCell.Multiline = true;
            this.xrRemarksTextTableCell.Name = "xrRemarksTextTableCell";
            this.xrRemarksTextTableCell.StylePriority.UseBorderColor = false;
            this.xrRemarksTextTableCell.StylePriority.UseBorders = false;
            this.xrRemarksTextTableCell.StylePriority.UseTextAlignment = false;
            this.xrRemarksTextTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrRemarksTextTableCell.Weight = 1.4172362773816583D;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.ReportHeader.HeightF = 38.54167F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1128F, 38.54167F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrItemHeaderTableCell,
            this.xrReferenceHeaderTableCell,
            this.xrDescriptionHeaderTableCell,
            this.xrUnitPriceHeaderTableCell,
            this.xrQtyHeaderTableCell,
            this.xrTotalPriceHeaderTableCell,
            this.xrRemarksHeaderTableCell});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrItemHeaderTableCell
            // 
            this.xrItemHeaderTableCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(148)))));
            this.xrItemHeaderTableCell.BorderColor = System.Drawing.Color.White;
            this.xrItemHeaderTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrItemHeaderTableCell.ForeColor = System.Drawing.Color.White;
            this.xrItemHeaderTableCell.Multiline = true;
            this.xrItemHeaderTableCell.Name = "xrItemHeaderTableCell";
            this.xrItemHeaderTableCell.StylePriority.UseBackColor = false;
            this.xrItemHeaderTableCell.StylePriority.UseBorderColor = false;
            this.xrItemHeaderTableCell.StylePriority.UseBorders = false;
            this.xrItemHeaderTableCell.StylePriority.UseForeColor = false;
            this.xrItemHeaderTableCell.StylePriority.UseTextAlignment = false;
            this.xrItemHeaderTableCell.Text = "   Item";
            this.xrItemHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrItemHeaderTableCell.Weight = 1.2305070206427189D;
            // 
            // xrReferenceHeaderTableCell
            // 
            this.xrReferenceHeaderTableCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(148)))));
            this.xrReferenceHeaderTableCell.BorderColor = System.Drawing.Color.White;
            this.xrReferenceHeaderTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrReferenceHeaderTableCell.ForeColor = System.Drawing.Color.White;
            this.xrReferenceHeaderTableCell.Multiline = true;
            this.xrReferenceHeaderTableCell.Name = "xrReferenceHeaderTableCell";
            this.xrReferenceHeaderTableCell.StylePriority.UseBackColor = false;
            this.xrReferenceHeaderTableCell.StylePriority.UseBorderColor = false;
            this.xrReferenceHeaderTableCell.StylePriority.UseBorders = false;
            this.xrReferenceHeaderTableCell.StylePriority.UseForeColor = false;
            this.xrReferenceHeaderTableCell.StylePriority.UseTextAlignment = false;
            this.xrReferenceHeaderTableCell.Text = "   Reference";
            this.xrReferenceHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrReferenceHeaderTableCell.Weight = 0.70254056728123382D;
            // 
            // xrDescriptionHeaderTableCell
            // 
            this.xrDescriptionHeaderTableCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(148)))));
            this.xrDescriptionHeaderTableCell.BorderColor = System.Drawing.Color.White;
            this.xrDescriptionHeaderTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrDescriptionHeaderTableCell.ForeColor = System.Drawing.Color.White;
            this.xrDescriptionHeaderTableCell.Multiline = true;
            this.xrDescriptionHeaderTableCell.Name = "xrDescriptionHeaderTableCell";
            this.xrDescriptionHeaderTableCell.StylePriority.UseBackColor = false;
            this.xrDescriptionHeaderTableCell.StylePriority.UseBorderColor = false;
            this.xrDescriptionHeaderTableCell.StylePriority.UseBorders = false;
            this.xrDescriptionHeaderTableCell.StylePriority.UseForeColor = false;
            this.xrDescriptionHeaderTableCell.StylePriority.UseTextAlignment = false;
            this.xrDescriptionHeaderTableCell.Text = "   Description";
            this.xrDescriptionHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrDescriptionHeaderTableCell.Weight = 1.8039798871409414D;
            // 
            // xrUnitPriceHeaderTableCell
            // 
            this.xrUnitPriceHeaderTableCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(148)))));
            this.xrUnitPriceHeaderTableCell.BorderColor = System.Drawing.Color.White;
            this.xrUnitPriceHeaderTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrUnitPriceHeaderTableCell.ForeColor = System.Drawing.Color.White;
            this.xrUnitPriceHeaderTableCell.Multiline = true;
            this.xrUnitPriceHeaderTableCell.Name = "xrUnitPriceHeaderTableCell";
            this.xrUnitPriceHeaderTableCell.StylePriority.UseBackColor = false;
            this.xrUnitPriceHeaderTableCell.StylePriority.UseBorderColor = false;
            this.xrUnitPriceHeaderTableCell.StylePriority.UseBorders = false;
            this.xrUnitPriceHeaderTableCell.StylePriority.UseForeColor = false;
            this.xrUnitPriceHeaderTableCell.StylePriority.UseTextAlignment = false;
            this.xrUnitPriceHeaderTableCell.Text = "Unit Price";
            this.xrUnitPriceHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrUnitPriceHeaderTableCell.Weight = 0.66746741243673835D;
            // 
            // xrQtyHeaderTableCell
            // 
            this.xrQtyHeaderTableCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(148)))));
            this.xrQtyHeaderTableCell.BorderColor = System.Drawing.Color.White;
            this.xrQtyHeaderTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrQtyHeaderTableCell.ForeColor = System.Drawing.Color.White;
            this.xrQtyHeaderTableCell.Multiline = true;
            this.xrQtyHeaderTableCell.Name = "xrQtyHeaderTableCell";
            this.xrQtyHeaderTableCell.StylePriority.UseBackColor = false;
            this.xrQtyHeaderTableCell.StylePriority.UseBorderColor = false;
            this.xrQtyHeaderTableCell.StylePriority.UseBorders = false;
            this.xrQtyHeaderTableCell.StylePriority.UseForeColor = false;
            this.xrQtyHeaderTableCell.StylePriority.UseTextAlignment = false;
            this.xrQtyHeaderTableCell.Text = "Qty";
            this.xrQtyHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrQtyHeaderTableCell.Weight = 0.45880156103583125D;
            // 
            // xrTotalPriceHeaderTableCell
            // 
            this.xrTotalPriceHeaderTableCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(148)))));
            this.xrTotalPriceHeaderTableCell.BorderColor = System.Drawing.Color.White;
            this.xrTotalPriceHeaderTableCell.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTotalPriceHeaderTableCell.ForeColor = System.Drawing.Color.White;
            this.xrTotalPriceHeaderTableCell.Multiline = true;
            this.xrTotalPriceHeaderTableCell.Name = "xrTotalPriceHeaderTableCell";
            this.xrTotalPriceHeaderTableCell.StylePriority.UseBackColor = false;
            this.xrTotalPriceHeaderTableCell.StylePriority.UseBorderColor = false;
            this.xrTotalPriceHeaderTableCell.StylePriority.UseBorders = false;
            this.xrTotalPriceHeaderTableCell.StylePriority.UseForeColor = false;
            this.xrTotalPriceHeaderTableCell.StylePriority.UseTextAlignment = false;
            this.xrTotalPriceHeaderTableCell.Text = "Total Price";
            this.xrTotalPriceHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTotalPriceHeaderTableCell.Weight = 0.97387411868164853D;
            // 
            // xrRemarksHeaderTableCell
            // 
            this.xrRemarksHeaderTableCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(148)))));
            this.xrRemarksHeaderTableCell.BorderColor = System.Drawing.Color.White;
            this.xrRemarksHeaderTableCell.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrRemarksHeaderTableCell.ForeColor = System.Drawing.Color.White;
            this.xrRemarksHeaderTableCell.Multiline = true;
            this.xrRemarksHeaderTableCell.Name = "xrRemarksHeaderTableCell";
            this.xrRemarksHeaderTableCell.StylePriority.UseBackColor = false;
            this.xrRemarksHeaderTableCell.StylePriority.UseBorderColor = false;
            this.xrRemarksHeaderTableCell.StylePriority.UseBorders = false;
            this.xrRemarksHeaderTableCell.StylePriority.UseForeColor = false;
            this.xrRemarksHeaderTableCell.StylePriority.UseTextAlignment = false;
            this.xrRemarksHeaderTableCell.Text = "Remarks";
            this.xrRemarksHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrRemarksHeaderTableCell.Weight = 1.3764045287987832D;
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(2.000014F, 1.666665F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(189F, 14.58335F);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RevisionItem].[NumItem]")});
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.Text = "xrTableCell1";
            this.xrTableCell1.Weight = 1.7911778996708885D;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.Constructor = objectConstructorInfo1;
            this.objectDataSource1.DataMember = "ArticleDecomposedList";
            this.objectDataSource1.DataSource = typeof(Emdep.Geos.Data.Common.OtItem);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // objectDataSource2
            // 
            this.objectDataSource2.Constructor = objectConstructorInfo2;
            this.objectDataSource2.DataSource = typeof(Emdep.Geos.Data.Common.OtItem);
            this.objectDataSource2.Name = "objectDataSource2";
            // 
            // PendingReviewItemSubReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1,
            this.objectDataSource2});
            this.DataSource = this.objectDataSource2;
            this.DefaultPrinterSettingsUsing.UseLandscape = true;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(17, 9, 0, 0);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        public DevExpress.XtraReports.UI.XRTableCell xrItemHeaderTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrReferenceHeaderTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrDescriptionHeaderTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrUnitPriceHeaderTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrQtyHeaderTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrTotalPriceHeaderTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrRemarksHeaderTableCell;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        public DevExpress.XtraReports.UI.XRTableCell xrReferenceTextTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrDescriptionTextTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrUnitPriceTextTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrQtyTextTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrTotalPriceTextTableCell;
        public DevExpress.XtraReports.UI.XRTableCell xrRemarksTextTableCell;
        public DevExpress.XtraReports.UI.XRBarCode xritemBarCode;
        public DevExpress.XtraReports.UI.XRTable xrTable1;
        public DevExpress.XtraReports.UI.XRTable xrTable4;
        public DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        public DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource2;
        public DevExpress.XtraReports.UI.XRTableCell xrItemTextTableCell;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
    }
}
