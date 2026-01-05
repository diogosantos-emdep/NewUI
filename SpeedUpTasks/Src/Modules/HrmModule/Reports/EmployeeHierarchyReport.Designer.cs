namespace Emdep.Geos.Modules.Hrm.Reports
{
    partial class EmployeeHierarchyReport
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.SideBySideBarSeriesView sideBySideBarSeriesView1 = new DevExpress.XtraCharts.SideBySideBarSeriesView();
            DevExpress.XtraCharts.RectangleGradientFillOptions rectangleGradientFillOptions1 = new DevExpress.XtraCharts.RectangleGradientFillOptions();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel2 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.SideBySideBarSeriesView sideBySideBarSeriesView2 = new DevExpress.XtraCharts.SideBySideBarSeriesView();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.empHierarchyImage = new DevExpress.XtraReports.UI.XRPictureBox();
            this.empHerarchyIsolatedDept = new DevExpress.XtraReports.UI.XRPictureBox();
            this.HeaderSection = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.imgLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrlblPlantTotal = new DevExpress.XtraReports.UI.XRLabel();
            this.xrDepartmentAverageChart = new DevExpress.XtraReports.UI.XRChart();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrGauge1 = new DevExpress.XtraReports.UI.XRGauge();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCrossBandBox1 = new DevExpress.XtraReports.UI.XRCrossBandBox();
            this.xrBandBoxNormal = new DevExpress.XtraReports.UI.XRCrossBandBox();
            this.xrBandBoxIsolated = new DevExpress.XtraReports.UI.XRCrossBandBox();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.xrDepartmentAverageChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.BorderWidth = 2F;
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.empHierarchyImage,
            this.empHerarchyIsolatedDept});
            this.Detail.HeightF = 1350F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseBorderWidth = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // empHierarchyImage
            // 
            this.empHierarchyImage.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.empHierarchyImage.BorderWidth = 1F;
            this.empHierarchyImage.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
            this.empHierarchyImage.LocationFloat = new DevExpress.Utils.PointFloat(21F, 2F);
            this.empHierarchyImage.Name = "empHierarchyImage";
            this.empHierarchyImage.SizeF = new System.Drawing.SizeF(950F, 1330F);
            // 
            // empHerarchyIsolatedDept
            // 
            this.empHerarchyIsolatedDept.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.empHerarchyIsolatedDept.BorderWidth = 1F;
            this.empHerarchyIsolatedDept.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.TopCenter;
            this.empHerarchyIsolatedDept.LocationFloat = new DevExpress.Utils.PointFloat(980F, 0F);
            this.empHerarchyIsolatedDept.Name = "empHerarchyIsolatedDept";
            this.empHerarchyIsolatedDept.SizeF = new System.Drawing.SizeF(165F, 1350F);
            // 
            // HeaderSection
            // 
            this.HeaderSection.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.imgLogo,
            this.xrlblPlantTotal,
            this.xrDepartmentAverageChart,
            this.xrLabel2,
            this.xrGauge1,
            this.xrLabel4,
            this.xrLabel5});
            this.HeaderSection.HeightF = 198F;
            this.HeaderSection.Name = "HeaderSection";
            this.HeaderSection.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.HeaderSection.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(1034F, 41.38F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(111.4583F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Plant Total";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // imgLogo
            // 
            this.imgLogo.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
            this.imgLogo.LocationFloat = new DevExpress.Utils.PointFloat(28.04167F, 41.37501F);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.SizeF = new System.Drawing.SizeF(126.0417F, 36.45833F);
            this.imgLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // xrlblPlantTotal
            // 
            this.xrlblPlantTotal.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlblPlantTotal.LocationFloat = new DevExpress.Utils.PointFloat(1045F, 55.96F);
            this.xrlblPlantTotal.Name = "xrlblPlantTotal";
            this.xrlblPlantTotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlblPlantTotal.SizeF = new System.Drawing.SizeF(109.3751F, 72.83332F);
            this.xrlblPlantTotal.StylePriority.UseFont = false;
            this.xrlblPlantTotal.StylePriority.UseTextAlignment = false;
            this.xrlblPlantTotal.Text = "xrlblPlantTotal";
            this.xrlblPlantTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrDepartmentAverageChart
            // 
            this.xrDepartmentAverageChart.BorderColor = System.Drawing.Color.Black;
            this.xrDepartmentAverageChart.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrDepartmentAverageChart.DataSource = this.objectDataSource1;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.GridLines.Visible = false;
            xyDiagram1.AxisY.Label.Visible = false;
            xyDiagram1.AxisY.Tickmarks.MinorVisible = false;
            xyDiagram1.AxisY.Tickmarks.Visible = false;
            xyDiagram1.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.BorderVisible = false;
            xyDiagram1.DefaultPane.EnableAxisXScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisXZooming = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYZooming = DevExpress.Utils.DefaultBoolean.False;
            this.xrDepartmentAverageChart.Diagram = xyDiagram1;
            this.xrDepartmentAverageChart.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            this.xrDepartmentAverageChart.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.TopOutside;
            this.xrDepartmentAverageChart.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.xrDepartmentAverageChart.Legend.Name = "Default Legend";
            this.xrDepartmentAverageChart.LocationFloat = new DevExpress.Utils.PointFloat(341.2502F, 55.95834F);
            this.xrDepartmentAverageChart.Name = "xrDepartmentAverageChart";
            series1.ArgumentDataMember = "Value";
            sideBySideBarSeriesLabel1.TextPattern = "{V:0}%";
            series1.Label = sideBySideBarSeriesLabel1;
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.LegendName = "Default Legend";
            series1.LegendTextPattern = "Avg. Emdep";
            series1.Name = "Average";
            series1.ValueDataMembersSerializable = "Average";
            sideBySideBarSeriesView1.BarWidth = 0.8D;
            sideBySideBarSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            sideBySideBarSeriesView1.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Gradient;
            rectangleGradientFillOptions1.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            rectangleGradientFillOptions1.GradientMode = DevExpress.XtraCharts.RectangleGradientMode.LeftToRight;
            sideBySideBarSeriesView1.FillStyle.Options = rectangleGradientFillOptions1;
            series1.View = sideBySideBarSeriesView1;
            series2.ArgumentDataMember = "Value";
            series2.ColorDataMember = "HtmlColor";
            sideBySideBarSeriesLabel2.TextPattern = "{V:0}%";
            series2.Label = sideBySideBarSeriesLabel2;
            series2.LegendName = "Default Legend";
            series2.Name = "Percentage";
            series2.ValueDataMembersSerializable = "Percentage";
            sideBySideBarSeriesView2.BarWidth = 0.8D;
            series2.View = sideBySideBarSeriesView2;
            this.xrDepartmentAverageChart.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.xrDepartmentAverageChart.SizeF = new System.Drawing.SizeF(363.2917F, 132.0416F);
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Emdep.Geos.Data.Common.Epc.LookupValue);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(157.9167F, 55.95834F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(165.625F, 47.91666F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "ORGANIZATION CHART";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrGauge1
            // 
            this.xrGauge1.BackColor = System.Drawing.Color.Transparent;
            this.xrGauge1.BorderColor = System.Drawing.Color.Black;
            this.xrGauge1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrGauge1.LocationFloat = new DevExpress.Utils.PointFloat(1046F, 102.12F);
            this.xrGauge1.Name = "xrGauge1";
            this.xrGauge1.SizeF = new System.Drawing.SizeF(109.375F, 52.8333F);
            this.xrGauge1.StylePriority.UseBackColor = false;
            this.xrGauge1.StylePriority.UseBorderColor = false;
            this.xrGauge1.StylePriority.UseBorderDashStyle = false;
            this.xrGauge1.TickmarkCount = 0;
            this.xrGauge1.XlsxFormatString = null;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1088F, 148.25F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.SizeF = new System.Drawing.SizeF(28.125F, 19.87502F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel5
            // 
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(1016F, 168.125F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.SizeF = new System.Drawing.SizeF(137.0001F, 19.87495F);
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Production vs Total";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3});
            this.BottomMargin.HeightF = 30F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(985F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.SizeF = new System.Drawing.SizeF(162.5419F, 23F);
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrCrossBandBox1
            // 
            this.xrCrossBandBox1.AnchorVertical = ((DevExpress.XtraReports.UI.VerticalAnchorStyles)((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top | DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom)));
            this.xrCrossBandBox1.BorderWidth = 1F;
            this.xrCrossBandBox1.EndBand = this.HeaderSection;
            this.xrCrossBandBox1.EndPointFloat = new DevExpress.Utils.PointFloat(16F, 193F);
            this.xrCrossBandBox1.Name = "xrCrossBandBox1";
            this.xrCrossBandBox1.StartBand = this.HeaderSection;
            this.xrCrossBandBox1.StartPointFloat = new DevExpress.Utils.PointFloat(16F, 18F);
            this.xrCrossBandBox1.WidthF = 1129F;
            // 
            // xrBandBoxNormal
            // 
            this.xrBandBoxNormal.AnchorVertical = ((DevExpress.XtraReports.UI.VerticalAnchorStyles)((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top | DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom)));
            this.xrBandBoxNormal.BorderWidth = 1F;
            this.xrBandBoxNormal.EndBand = this.Detail;
            this.xrBandBoxNormal.EndPointFloat = new DevExpress.Utils.PointFloat(16F, 1350F);
            this.xrBandBoxNormal.Name = "xrBandBoxNormal";
            this.xrBandBoxNormal.StartBand = this.Detail;
            this.xrBandBoxNormal.StartPointFloat = new DevExpress.Utils.PointFloat(16F, 0F);
            this.xrBandBoxNormal.WidthF = 960F;
            // 
            // xrBandBoxIsolated
            // 
            this.xrBandBoxIsolated.AnchorVertical = ((DevExpress.XtraReports.UI.VerticalAnchorStyles)((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top | DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom)));
            this.xrBandBoxIsolated.BorderWidth = 1F;
            this.xrBandBoxIsolated.EndBand = this.Detail;
            this.xrBandBoxIsolated.EndPointFloat = new DevExpress.Utils.PointFloat(673.9584F, 795.375F);
            this.xrBandBoxIsolated.Name = "xrBandBoxIsolated";
            this.xrBandBoxIsolated.StartBand = this.Detail;
            this.xrBandBoxIsolated.StartPointFloat = new DevExpress.Utils.PointFloat(673.9584F, 0F);
            this.xrBandBoxIsolated.WidthF = 165.8333F;
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.Name = "xrControlStyle1";
            // 
            // EmployeeHierarchyReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.HeaderSection,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.CrossBandControls.AddRange(new DevExpress.XtraReports.UI.XRCrossBandControl[] {
            this.xrCrossBandBox1,
            this.xrBandBoxNormal});
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 198, 30);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrDepartmentAverageChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand HeaderSection;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.XRPictureBox imgLogo;
        public DevExpress.XtraReports.UI.XRPictureBox empHierarchyImage;
        public DevExpress.XtraReports.UI.XRPictureBox empHerarchyIsolatedDept;
        public DevExpress.XtraReports.UI.XRLabel xrlblPlantTotal;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        public DevExpress.XtraReports.UI.XRChart xrDepartmentAverageChart;
        private DevExpress.XtraReports.UI.XRCrossBandBox xrCrossBandBox1;
        public DevExpress.XtraReports.UI.XRCrossBandBox xrBandBoxNormal;
        public DevExpress.XtraReports.UI.XRCrossBandBox xrBandBoxIsolated;
        public DevExpress.XtraReports.UI.XRLabel xrLabel1;
        public DevExpress.XtraReports.UI.XRLabel xrLabel2;
        public DevExpress.XtraReports.UI.XRGauge xrGauge1;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        public DevExpress.XtraReports.UI.XRLabel xrLabel3;
        public DevExpress.XtraReports.UI.XRLabel xrLabel4;
        public DevExpress.XtraReports.UI.XRLabel xrLabel5;
    }
}
