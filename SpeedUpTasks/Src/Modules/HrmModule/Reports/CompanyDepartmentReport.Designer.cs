namespace Emdep.Geos.Modules.Hrm.Reports
{
    partial class CompanyDepartmentReport
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
            DevExpress.DataAccess.ObjectBinding.ObjectConstructorInfo objectConstructorInfo1 = new DevExpress.DataAccess.ObjectBinding.ObjectConstructorInfo();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLbSize = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLbDepartmentSize = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLbWorkstations = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLbNmuberOfWorkstations = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLbWorkers = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLbTotalWorkers = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.xrCrossBandBox1 = new DevExpress.XtraReports.UI.XRCrossBandBox();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.xrPanel1});
            this.Detail.HeightF = 185.9027F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(491F, 25F);
            this.xrTable1.StylePriority.UseBorders = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Department]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "BackColor", "[Department.DepartmentArea.HtmlColor]")});
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 2.1379557784881644D;
            // 
            // xrPanel1
            // 
            this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPanel1.BorderWidth = 1F;
            this.xrPanel1.CanShrink = true;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLbSize,
            this.xrLbDepartmentSize,
            this.xrLbWorkstations,
            this.xrLbNmuberOfWorkstations,
            this.xrLbWorkers,
            this.xrLbTotalWorkers});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(499F, 185.9027F);
            this.xrPanel1.StylePriority.UseBorders = false;
            this.xrPanel1.StylePriority.UseBorderWidth = false;
            // 
            // xrLbSize
            // 
            this.xrLbSize.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLbSize.LocationFloat = new DevExpress.Utils.PointFloat(10.1873F, 40.95841F);
            this.xrLbSize.Name = "xrLbSize";
            this.xrLbSize.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLbSize.SizeF = new System.Drawing.SizeF(150.5833F, 18.83335F);
            this.xrLbSize.StylePriority.UseBorders = false;
            this.xrLbSize.StylePriority.UseTextAlignment = false;
            this.xrLbSize.Text = "Size(m2)";
            this.xrLbSize.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLbDepartmentSize
            // 
            this.xrLbDepartmentSize.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLbDepartmentSize.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Size]")});
            this.xrLbDepartmentSize.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrLbDepartmentSize.LocationFloat = new DevExpress.Utils.PointFloat(266.1873F, 40.9584F);
            this.xrLbDepartmentSize.Name = "xrLbDepartmentSize";
            this.xrLbDepartmentSize.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLbDepartmentSize.SizeF = new System.Drawing.SizeF(152.9167F, 18.83326F);
            this.xrLbDepartmentSize.StylePriority.UseBorders = false;
            this.xrLbDepartmentSize.StylePriority.UseFont = false;
            this.xrLbDepartmentSize.Text = "xrLbDepartmentSize";
            this.xrLbDepartmentSize.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLbWorkstations
            // 
            this.xrLbWorkstations.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLbWorkstations.LocationFloat = new DevExpress.Utils.PointFloat(10.1873F, 65.95841F);
            this.xrLbWorkstations.Name = "xrLbWorkstations";
            this.xrLbWorkstations.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLbWorkstations.SizeF = new System.Drawing.SizeF(150.5833F, 18.83335F);
            this.xrLbWorkstations.StylePriority.UseBorders = false;
            this.xrLbWorkstations.StylePriority.UseTextAlignment = false;
            this.xrLbWorkstations.Text = "Number of Workstations";
            this.xrLbWorkstations.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLbNmuberOfWorkstations
            // 
            this.xrLbNmuberOfWorkstations.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLbNmuberOfWorkstations.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NumberOfWorkstations]")});
            this.xrLbNmuberOfWorkstations.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrLbNmuberOfWorkstations.LocationFloat = new DevExpress.Utils.PointFloat(266.1873F, 65.9584F);
            this.xrLbNmuberOfWorkstations.Name = "xrLbNmuberOfWorkstations";
            this.xrLbNmuberOfWorkstations.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLbNmuberOfWorkstations.SizeF = new System.Drawing.SizeF(152.9167F, 18.83325F);
            this.xrLbNmuberOfWorkstations.StylePriority.UseBorders = false;
            this.xrLbNmuberOfWorkstations.StylePriority.UseFont = false;
            this.xrLbNmuberOfWorkstations.StylePriority.UseTextAlignment = false;
            this.xrLbNmuberOfWorkstations.Text = "xrLbNmuberOfWorkstations";
            this.xrLbNmuberOfWorkstations.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLbWorkers
            // 
            this.xrLbWorkers.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLbWorkers.LocationFloat = new DevExpress.Utils.PointFloat(10.1873F, 90.95841F);
            this.xrLbWorkers.Name = "xrLbWorkers";
            this.xrLbWorkers.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLbWorkers.SizeF = new System.Drawing.SizeF(150.5833F, 18.83335F);
            this.xrLbWorkers.StylePriority.UseBorders = false;
            this.xrLbWorkers.StylePriority.UseTextAlignment = false;
            this.xrLbWorkers.Text = "Total Workers";
            this.xrLbWorkers.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLbTotalWorkers
            // 
            this.xrLbTotalWorkers.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLbTotalWorkers.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmployeesRecordCount]")});
            this.xrLbTotalWorkers.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrLbTotalWorkers.LocationFloat = new DevExpress.Utils.PointFloat(266.1873F, 90.9584F);
            this.xrLbTotalWorkers.Name = "xrLbTotalWorkers";
            this.xrLbTotalWorkers.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLbTotalWorkers.SizeF = new System.Drawing.SizeF(152.9167F, 18.83326F);
            this.xrLbTotalWorkers.StylePriority.UseBorders = false;
            this.xrLbTotalWorkers.StylePriority.UseFont = false;
            this.xrLbTotalWorkers.StylePriority.UseTextAlignment = false;
            this.xrLbTotalWorkers.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 300F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1});
            this.DetailReport.DataMember = "Jobdescriptions";
            this.DetailReport.DataSource = this.objectDataSource1;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail1.HeightF = 20F;
            this.Detail1.Name = "Detail1";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(49.26455F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(449.7354F, 20F);
            this.xrTable2.StylePriority.UseBorders = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell3});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1.15D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[JobDescriptionTitle]")});
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell2.Weight = 1.931312896896384D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmployeeRecordCount]")});
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 1.9999210902661206D;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.Constructor = objectConstructorInfo1;
            this.objectDataSource1.DataSource = typeof(Emdep.Geos.Data.Common.Hrm.CompanyDepartment);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // xrCrossBandBox1
            // 
            this.xrCrossBandBox1.AnchorVertical = ((DevExpress.XtraReports.UI.VerticalAnchorStyles)((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top | DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom)));
            this.xrCrossBandBox1.BorderWidth = 1F;
            this.xrCrossBandBox1.EndBand = this.BottomMargin;
            this.xrCrossBandBox1.EndPointFloat = new DevExpress.Utils.PointFloat(330F, 0F);
            this.xrCrossBandBox1.Name = "xrCrossBandBox1";
            this.xrCrossBandBox1.StartBand = this.Detail;
            this.xrCrossBandBox1.StartPointFloat = new DevExpress.Utils.PointFloat(330F, 0F);
            this.xrCrossBandBox1.WidthF = 480F;
            // 
            // CompanyDepartmentReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReport});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Margins = new System.Drawing.Printing.Margins(0, 349, 100, 300);
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;       
        public DevExpress.XtraReports.UI.XRPanel xrPanel1;     
        public DevExpress.XtraReports.UI.XRLabel xrLbSize;
        public DevExpress.XtraReports.UI.XRLabel xrLbWorkstations;
        public DevExpress.XtraReports.UI.XRLabel xrLbWorkers;
        public DevExpress.XtraReports.UI.XRLabel xrLbDepartmentSize;
        public DevExpress.XtraReports.UI.XRLabel xrLbNmuberOfWorkstations;
        public DevExpress.XtraReports.UI.XRLabel xrLbTotalWorkers;
        private DevExpress.XtraReports.UI.XRCrossBandBox xrCrossBandBox1;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        public DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        public DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.DetailBand Detail1;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        public DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        public DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        public DevExpress.XtraReports.UI.XRTableCell xrTableCell3;

    }
}
