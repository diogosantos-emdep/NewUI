namespace Emdep.Geos.Modules.SAM.Reports
{
    partial class TechnicalPropertiesSubReport
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
            DevExpress.DataAccess.ObjectBinding.ObjectConstructorInfo objectConstructorInfo2 = new DevExpress.DataAccess.ObjectBinding.ObjectConstructorInfo();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.tblProperties = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.GroupName = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetectionName = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetectionCount = new DevExpress.XtraReports.UI.XRTableCell();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.objectDataSource2 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tblProperties)).BeginInit();
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
            this.tblProperties});
            this.Detail.HeightF = 20F;
            this.Detail.Name = "Detail";
            // 
            // tblProperties
            // 
            this.tblProperties.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblProperties.LocationFloat = new DevExpress.Utils.PointFloat(57.00043F, 0F);
            this.tblProperties.Name = "tblProperties";
            this.tblProperties.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tblProperties.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.tblProperties.SizeF = new System.Drawing.SizeF(629.8332F, 20F);
            this.tblProperties.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.GroupName,
            this.DetectionName,
            this.DetectionCount});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // GroupName
            // 
            this.GroupName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[GroupName]")});
            this.GroupName.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupName.Multiline = true;
            this.GroupName.Name = "GroupName";
            this.GroupName.StylePriority.UseFont = false;
            this.GroupName.Weight = 0.55013009509487265D;
            // 
            // DetectionName
            // 
            this.DetectionName.CanGrow = false;
            this.DetectionName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.DetectionName.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DetectionName.Multiline = true;
            this.DetectionName.Name = "DetectionName";
            this.DetectionName.StylePriority.UseFont = false;
            this.DetectionName.StylePriority.UseTextAlignment = false;
            this.DetectionName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.DetectionName.Weight = 2.05678691473126D;
            this.DetectionName.WordWrap = false;
            // 
            // DetectionCount
            // 
            this.DetectionCount.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.DetectionCount.CanGrow = false;
            this.DetectionCount.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[QuantityInString]")});
            this.DetectionCount.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DetectionCount.Multiline = true;
            this.DetectionCount.Name = "DetectionCount";
            this.DetectionCount.StylePriority.UseBorders = false;
            this.DetectionCount.StylePriority.UseFont = false;
            this.DetectionCount.StylePriority.UseTextAlignment = false;
            this.DetectionCount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.DetectionCount.Weight = 0.26219892415095247D;
            this.DetectionCount.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DetectionCount_BeforePrint);
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.Constructor = objectConstructorInfo1;
            this.objectDataSource1.DataSource = typeof(Emdep.Geos.Data.Common.Detection);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // objectDataSource2
            // 
            this.objectDataSource2.Constructor = objectConstructorInfo2;
            this.objectDataSource2.DataSource = typeof(Emdep.Geos.Data.Common.Detection);
            this.objectDataSource2.Name = "objectDataSource2";
            // 
            // TechnicalPropertiesSubReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1,
            this.objectDataSource2});
            this.DataSource = this.objectDataSource2;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(2, 17, 0, 0);
            this.Version = "19.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.TechnicalPropertiesSubReport_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.tblProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        public DevExpress.XtraReports.UI.XRTable tblProperties;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        public DevExpress.XtraReports.UI.XRTableCell GroupName;
        public DevExpress.XtraReports.UI.XRTableCell DetectionName;
        public DevExpress.XtraReports.UI.XRTableCell DetectionCount;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        public DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        public DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource2;
    }
}
