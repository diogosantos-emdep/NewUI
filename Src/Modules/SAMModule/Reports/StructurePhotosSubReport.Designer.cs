namespace Emdep.Geos.Modules.SAM.Reports
{
    partial class StructurePhotosSubReport
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
            this.tblStructurePhotos = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tblCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tblCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.objectDataSource2 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.objectDataSource3 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tblStructurePhotos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 5F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tblStructurePhotos});
            this.Detail.HeightF = 307.5001F;
            this.Detail.Name = "Detail";
            // 
            // tblStructurePhotos
            // 
            this.tblStructurePhotos.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tblStructurePhotos.LocationFloat = new DevExpress.Utils.PointFloat(10.00031F, 0F);
            this.tblStructurePhotos.Name = "tblStructurePhotos";
            this.tblStructurePhotos.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tblStructurePhotos.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.tblStructurePhotos.SizeF = new System.Drawing.SizeF(786.9998F, 307.5001F);
            this.tblStructurePhotos.StylePriority.UseBorders = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tblCell1,
            this.tblCell2});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // tblCell1
            // 
            this.tblCell1.Multiline = true;
            this.tblCell1.Name = "tblCell1";
            this.tblCell1.Text = "tblCell1";
            this.tblCell1.Weight = 1.5744125378641609D;
            // 
            // tblCell2
            // 
            this.tblCell2.CanPublish = false;
            this.tblCell2.Multiline = true;
            this.tblCell2.Name = "tblCell2";
            this.tblCell2.Text = "tblCell2";
            this.tblCell2.Weight = 1.5078312032745029D;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.Constructor = objectConstructorInfo1;
            this.objectDataSource1.DataSource = typeof(Emdep.Geos.Data.Common.SAM.TechnicalSpecifications);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // objectDataSource2
            // 
            this.objectDataSource2.Constructor = objectConstructorInfo2;
            this.objectDataSource2.DataMember = "StructurePhotos";
            this.objectDataSource2.DataSource = typeof(Emdep.Geos.Data.Common.SAM.TechnicalSpecifications);
            this.objectDataSource2.Name = "objectDataSource2";
            // 
            // objectDataSource3
            // 
            this.objectDataSource3.Constructor = objectConstructorInfo2;
            this.objectDataSource3.DataSource = typeof(Emdep.Geos.Data.Common.SAM.TechnicalSpecifications);
            this.objectDataSource3.Name = "objectDataSource3";
            // 
            // StructurePhotosSubReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1,
            this.objectDataSource2,
            this.objectDataSource3});
            this.DataMember = "StructurePhotos";
            this.DataSource = this.objectDataSource3;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(0, 53, 0, 5);
            this.Version = "19.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.StructurePhotosSubReport_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.tblStructurePhotos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource2;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tblCell1;
        private DevExpress.XtraReports.UI.XRTableCell tblCell2;
        public DevExpress.XtraReports.UI.XRTable tblStructurePhotos;
        public DevExpress.XtraReports.UI.DetailBand Detail;
    }
}
