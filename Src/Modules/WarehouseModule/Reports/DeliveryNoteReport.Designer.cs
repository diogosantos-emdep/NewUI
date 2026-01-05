namespace Emdep.Geos.Modules.Warehouse.Reports
{
    public partial class DeliveryNoteReport
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
            this.lblTradeGroup = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblReference = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine8 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine16 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine15 = new DevExpress.XtraReports.UI.XRLine();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLine11 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLblWarehouselocation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblMadeIn = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblQuantity = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblDeliveryNoteCode = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblDeliveryNoteDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblProducerName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblSupplierName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine10 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine9 = new DevExpress.XtraReports.UI.XRLine();
            this.lblItem = new DevExpress.XtraReports.UI.XRLabel();
            this.lblMadeIn = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine7 = new DevExpress.XtraReports.UI.XRLine();
            this.lblQuantity = new DevExpress.XtraReports.UI.XRLabel();
            this.lblWarehouselocation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine6 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
            this.lblReference = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDeliveryNoteDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDeliveryNoteCode = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblProducer = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSupplier = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLblPlant = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine14 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine13 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine12 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLblOperatorName = new DevExpress.XtraReports.UI.XRLabel();
            this.imgLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblOperator = new DevExpress.XtraReports.UI.XRLabel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblTradeGroup,
            this.xrLblReference,
            this.xrLine8,
            this.xrLine16,
            this.xrLine15,
            this.xrBarCode1,
            this.xrLine11,
            this.xrLblWarehouselocation,
            this.xrLblMadeIn,
            this.xrLblQuantity,
            this.xrLblDeliveryNoteCode,
            this.xrLblDeliveryNoteDate,
            this.xrLblProducerName,
            this.xrLblSupplierName,
            this.xrLine10,
            this.xrLine1,
            this.xrLine9,
            this.lblItem,
            this.lblMadeIn,
            this.xrLine7,
            this.lblQuantity,
            this.lblWarehouselocation,
            this.xrLine6,
            this.xrLine5,
            this.lblReference,
            this.lblDeliveryNoteDate,
            this.lblDeliveryNoteCode,
            this.xrLine4,
            this.xrLine3,
            this.xrLine2,
            this.lblProducer,
            this.lblSupplier});
            this.Detail.HeightF = 995.9244F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblTradeGroup
            // 
            this.lblTradeGroup.BackColor = System.Drawing.Color.Black;
            this.lblTradeGroup.BorderColor = System.Drawing.Color.Transparent;
            this.lblTradeGroup.CanGrow = false;
            this.lblTradeGroup.Font = new System.Drawing.Font("Calibri", 76.75F, System.Drawing.FontStyle.Bold);
            this.lblTradeGroup.LocationFloat = new DevExpress.Utils.PointFloat(634.4403F, 675.084F);
            this.lblTradeGroup.Name = "lblTradeGroup";
            this.lblTradeGroup.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTradeGroup.SizeF = new System.Drawing.SizeF(172.0415F, 132.0801F);
            this.lblTradeGroup.StylePriority.UseBackColor = false;
            this.lblTradeGroup.StylePriority.UseBorderColor = false;
            this.lblTradeGroup.StylePriority.UseFont = false;
            this.lblTradeGroup.StylePriority.UseTextAlignment = false;
            this.lblTradeGroup.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLblReference
            // 
            this.xrLblReference.CanGrow = false;
            this.xrLblReference.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Reference]")});
            this.xrLblReference.Font = new System.Drawing.Font("Calibri", 70F, System.Drawing.FontStyle.Bold);
            this.xrLblReference.LocationFloat = new DevExpress.Utils.PointFloat(25.50298F, 302.7023F);
            this.xrLblReference.Multiline = true;
            this.xrLblReference.Name = "xrLblReference";
            this.xrLblReference.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblReference.SizeF = new System.Drawing.SizeF(780.2473F, 372.3817F);
            this.xrLblReference.StylePriority.UseFont = false;
            this.xrLblReference.StylePriority.UseTextAlignment = false;
            this.xrLblReference.Text = "xrLblReference";
            this.xrLblReference.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLblReference.TextFitMode = DevExpress.XtraReports.UI.TextFitMode.ShrinkAndGrow;
            // 
            // xrLine8
            // 
            this.xrLine8.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine8.LocationFloat = new DevExpress.Utils.PointFloat(631.7085F, 676.9976F);
            this.xrLine8.Name = "xrLine8";
            this.xrLine8.SizeF = new System.Drawing.SizeF(6.060608F, 130F);
            // 
            // xrLine16
            // 
            this.xrLine16.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine16.LocationFloat = new DevExpress.Utils.PointFloat(419.3784F, 677.1641F);
            this.xrLine16.Name = "xrLine16";
            this.xrLine16.SizeF = new System.Drawing.SizeF(2.083344F, 130F);
            // 
            // xrLine15
            // 
            this.xrLine15.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine15.LocationFloat = new DevExpress.Utils.PointFloat(419.3784F, 809.32F);
            this.xrLine15.Name = "xrLine15";
            this.xrLine15.SizeF = new System.Drawing.SizeF(2.083328F, 155F);
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Barcode]")});
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(421.7984F, 839.6116F);
            this.xrBarCode1.Module = 1.7F;
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode1.ShowText = false;
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(374.9854F, 114.924F);
            this.xrBarCode1.StylePriority.UseTextAlignment = false;
            this.xrBarCode1.Symbology = code128Generator1;
            this.xrBarCode1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine11
            // 
            this.xrLine11.LocationFloat = new DevExpress.Utils.PointFloat(20.90181F, 963.2441F);
            this.xrLine11.Name = "xrLine11";
            this.xrLine11.SizeF = new System.Drawing.SizeF(785.58F, 2.08F);
            // 
            // xrLblWarehouselocation
            // 
            this.xrLblWarehouselocation.CanGrow = false;
            this.xrLblWarehouselocation.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblWarehouselocation.LocationFloat = new DevExpress.Utils.PointFloat(23.42F, 839.6116F);
            this.xrLblWarehouselocation.Name = "xrLblWarehouselocation";
            this.xrLblWarehouselocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblWarehouselocation.SizeF = new System.Drawing.SizeF(395.9584F, 114.9241F);
            this.xrLblWarehouselocation.StylePriority.UseFont = false;
            this.xrLblWarehouselocation.StylePriority.UsePadding = false;
            this.xrLblWarehouselocation.StylePriority.UseTextAlignment = false;
            this.xrLblWarehouselocation.Text = "xrLblWarehouselocation";
            this.xrLblWarehouselocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLblMadeIn
            // 
            this.xrLblMadeIn.CanGrow = false;
            this.xrLblMadeIn.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MadeIn]")});
            this.xrLblMadeIn.Font = new System.Drawing.Font("Calibri", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblMadeIn.LocationFloat = new DevExpress.Utils.PointFloat(421.4617F, 725.4141F);
            this.xrLblMadeIn.Name = "xrLblMadeIn";
            this.xrLblMadeIn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblMadeIn.SizeF = new System.Drawing.SizeF(210.2468F, 67.04163F);
            this.xrLblMadeIn.StylePriority.UseFont = false;
            this.xrLblMadeIn.StylePriority.UseTextAlignment = false;
            this.xrLblMadeIn.Text = "xrLblMadeIn";
            this.xrLblMadeIn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLblQuantity
            // 
            this.xrLblQuantity.AutoWidth = true;
            this.xrLblQuantity.CanGrow = false;
            this.xrLblQuantity.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Quantity]")});
            this.xrLblQuantity.Font = new System.Drawing.Font("Calibri", 70F, System.Drawing.FontStyle.Bold);
            this.xrLblQuantity.LocationFloat = new DevExpress.Utils.PointFloat(23.42F, 707.4557F);
            this.xrLblQuantity.Name = "xrLblQuantity";
            this.xrLblQuantity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblQuantity.SizeF = new System.Drawing.SizeF(395.9584F, 96.87476F);
            this.xrLblQuantity.StylePriority.UseFont = false;
            this.xrLblQuantity.StylePriority.UseTextAlignment = false;
            this.xrLblQuantity.Text = "xrLblQuantity";
            this.xrLblQuantity.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLblDeliveryNoteCode
            // 
            this.xrLblDeliveryNoteCode.AutoWidth = true;
            this.xrLblDeliveryNoteCode.CanGrow = false;
            this.xrLblDeliveryNoteCode.CanShrink = true;
            this.xrLblDeliveryNoteCode.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Code]")});
            this.xrLblDeliveryNoteCode.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblDeliveryNoteCode.LocationFloat = new DevExpress.Utils.PointFloat(23.42F, 189.4751F);
            this.xrLblDeliveryNoteCode.Name = "xrLblDeliveryNoteCode";
            this.xrLblDeliveryNoteCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblDeliveryNoteCode.SizeF = new System.Drawing.SizeF(294.375F, 41.39397F);
            this.xrLblDeliveryNoteCode.StylePriority.UseFont = false;
            this.xrLblDeliveryNoteCode.StylePriority.UseTextAlignment = false;
            this.xrLblDeliveryNoteCode.Text = "xrLblDeliveryNoteCode";
            this.xrLblDeliveryNoteCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLblDeliveryNoteDate
            // 
            this.xrLblDeliveryNoteDate.AutoWidth = true;
            this.xrLblDeliveryNoteDate.CanGrow = false;
            this.xrLblDeliveryNoteDate.CanShrink = true;
            this.xrLblDeliveryNoteDate.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DeliveryNoteDateString]")});
            this.xrLblDeliveryNoteDate.Font = new System.Drawing.Font("Calibri", 55F, System.Drawing.FontStyle.Bold);
            this.xrLblDeliveryNoteDate.LocationFloat = new DevExpress.Utils.PointFloat(319.8784F, 177.0168F);
            this.xrLblDeliveryNoteDate.Name = "xrLblDeliveryNoteDate";
            this.xrLblDeliveryNoteDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblDeliveryNoteDate.SizeF = new System.Drawing.SizeF(476.9055F, 69.35551F);
            this.xrLblDeliveryNoteDate.StylePriority.UseFont = false;
            this.xrLblDeliveryNoteDate.StylePriority.UseTextAlignment = false;
            this.xrLblDeliveryNoteDate.Text = "xrLblDeliveryNoteDate";
            this.xrLblDeliveryNoteDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLblProducerName
            // 
            this.xrLblProducerName.CanGrow = false;
            this.xrLblProducerName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProducerName]")});
            this.xrLblProducerName.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblProducerName.LocationFloat = new DevExpress.Utils.PointFloat(23.42F, 98.55423F);
            this.xrLblProducerName.Name = "xrLblProducerName";
            this.xrLblProducerName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblProducerName.SizeF = new System.Drawing.SizeF(770.9437F, 41.52577F);
            this.xrLblProducerName.StylePriority.UseFont = false;
            this.xrLblProducerName.Text = "xrLblProducerName";
            // 
            // xrLblSupplierName
            // 
            this.xrLblSupplierName.CanGrow = false;
            this.xrLblSupplierName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SupplierName]")});
            this.xrLblSupplierName.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblSupplierName.LocationFloat = new DevExpress.Utils.PointFloat(23.42F, 32.37165F);
            this.xrLblSupplierName.Name = "xrLblSupplierName";
            this.xrLblSupplierName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblSupplierName.SizeF = new System.Drawing.SizeF(773.3638F, 31.62835F);
            this.xrLblSupplierName.StylePriority.UseFont = false;
            this.xrLblSupplierName.Text = "xrLblSupplierName";
            // 
            // xrLine10
            // 
            this.xrLine10.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine10.LocationFloat = new DevExpress.Utils.PointFloat(805.7503F, 0F);
            this.xrLine10.Name = "xrLine10";
            this.xrLine10.SizeF = new System.Drawing.SizeF(2.272705F, 964F);
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(21.25F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(785.58F, 2.08F);
            // 
            // xrLine9
            // 
            this.xrLine9.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine9.LocationFloat = new DevExpress.Utils.PointFloat(19.25329F, 0F);
            this.xrLine9.Name = "xrLine9";
            this.xrLine9.SizeF = new System.Drawing.SizeF(2.272724F, 964F);
            // 
            // lblItem
            // 
            this.lblItem.CanGrow = false;
            this.lblItem.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItem.LocationFloat = new DevExpress.Utils.PointFloat(421.4617F, 809.244F);
            this.lblItem.Name = "lblItem";
            this.lblItem.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 2, 0, 0, 100F);
            this.lblItem.SizeF = new System.Drawing.SizeF(375.3222F, 30.29169F);
            this.lblItem.StylePriority.UseFont = false;
            this.lblItem.StylePriority.UsePadding = false;
            this.lblItem.Text = "ITEM";
            // 
            // lblMadeIn
            // 
            this.lblMadeIn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMadeIn.LocationFloat = new DevExpress.Utils.PointFloat(423.4617F, 677.1641F);
            this.lblMadeIn.Name = "lblMadeIn";
            this.lblMadeIn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblMadeIn.SizeF = new System.Drawing.SizeF(196.7884F, 33.70831F);
            this.lblMadeIn.StylePriority.UseFont = false;
            this.lblMadeIn.Text = "MADE IN";
            // 
            // xrLine7
            // 
            this.xrLine7.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine7.LocationFloat = new DevExpress.Utils.PointFloat(317.795F, 141.16F);
            this.xrLine7.Name = "xrLine7";
            this.xrLine7.SizeF = new System.Drawing.SizeF(2.083344F, 129.1707F);
            // 
            // lblQuantity
            // 
            this.lblQuantity.CanGrow = false;
            this.lblQuantity.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.LocationFloat = new DevExpress.Utils.PointFloat(23.42F, 677.1641F);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblQuantity.SizeF = new System.Drawing.SizeF(395.9584F, 30.29169F);
            this.lblQuantity.StylePriority.UseFont = false;
            this.lblQuantity.Text = "QUANTITY";
            // 
            // lblWarehouselocation
            // 
            this.lblWarehouselocation.CanGrow = false;
            this.lblWarehouselocation.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarehouselocation.LocationFloat = new DevExpress.Utils.PointFloat(23.4164F, 809.3199F);
            this.lblWarehouselocation.Name = "lblWarehouselocation";
            this.lblWarehouselocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblWarehouselocation.SizeF = new System.Drawing.SizeF(395.9584F, 30.29169F);
            this.lblWarehouselocation.StylePriority.UseFont = false;
            this.lblWarehouselocation.StylePriority.UsePadding = false;
            this.lblWarehouselocation.Text = "WAREHOUSE LOCATION";
            // 
            // xrLine6
            // 
            this.xrLine6.LocationFloat = new DevExpress.Utils.PointFloat(21.25333F, 804.6241F);
            this.xrLine6.Name = "xrLine6";
            this.xrLine6.SizeF = new System.Drawing.SizeF(785.5F, 4.62F);
            // 
            // xrLine5
            // 
            this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(20.90181F, 675.084F);
            this.xrLine5.Name = "xrLine5";
            this.xrLine5.SizeF = new System.Drawing.SizeF(785.5F, 2.08F);
            // 
            // lblReference
            // 
            this.lblReference.CanGrow = false;
            this.lblReference.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReference.LocationFloat = new DevExpress.Utils.PointFloat(23.4164F, 272.4106F);
            this.lblReference.Name = "lblReference";
            this.lblReference.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReference.SizeF = new System.Drawing.SizeF(763.6407F, 30.29166F);
            this.lblReference.StylePriority.UseFont = false;
            this.lblReference.Text = "REFERENCE";
            // 
            // lblDeliveryNoteDate
            // 
            this.lblDeliveryNoteDate.CanGrow = false;
            this.lblDeliveryNoteDate.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeliveryNoteDate.LocationFloat = new DevExpress.Utils.PointFloat(319.8784F, 142.16F);
            this.lblDeliveryNoteDate.Name = "lblDeliveryNoteDate";
            this.lblDeliveryNoteDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDeliveryNoteDate.SizeF = new System.Drawing.SizeF(467.1788F, 30.29164F);
            this.lblDeliveryNoteDate.StylePriority.UseFont = false;
            this.lblDeliveryNoteDate.Text = "DELIVERY NOTE DATE";
            // 
            // lblDeliveryNoteCode
            // 
            this.lblDeliveryNoteCode.CanGrow = false;
            this.lblDeliveryNoteCode.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeliveryNoteCode.LocationFloat = new DevExpress.Utils.PointFloat(23.42F, 142.16F);
            this.lblDeliveryNoteCode.Name = "lblDeliveryNoteCode";
            this.lblDeliveryNoteCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDeliveryNoteCode.SizeF = new System.Drawing.SizeF(294.375F, 30.29164F);
            this.lblDeliveryNoteCode.StylePriority.UseFont = false;
            this.lblDeliveryNoteCode.Text = "DELIVERY NOTE CODE";
            // 
            // xrLine4
            // 
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(22.25344F, 270.3307F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(785.5F, 2.08F);
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(21.25335F, 140.08F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(785.58F, 2.08F);
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(20.25348F, 65F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(785.9F, 2.8F);
            // 
            // lblProducer
            // 
            this.lblProducer.CanGrow = false;
            this.lblProducer.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProducer.LocationFloat = new DevExpress.Utils.PointFloat(23F, 69.26254F);
            this.lblProducer.Name = "lblProducer";
            this.lblProducer.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblProducer.SizeF = new System.Drawing.SizeF(764.0571F, 29.29166F);
            this.lblProducer.StylePriority.UseFont = false;
            this.lblProducer.Text = "PRODUCER";
            // 
            // lblSupplier
            // 
            this.lblSupplier.CanGrow = false;
            this.lblSupplier.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupplier.LocationFloat = new DevExpress.Utils.PointFloat(23.42F, 2.079995F);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSupplier.SizeF = new System.Drawing.SizeF(763.6372F, 30.29165F);
            this.lblSupplier.StylePriority.UseFont = false;
            this.lblSupplier.Text = "SUPPLIER";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.TopMargin.Visible = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.BorderWidth = 0F;
            this.BottomMargin.HeightF = 1.557159F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseBorderWidth = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLblPlant,
            this.xrLine14,
            this.xrLine13,
            this.xrLine12,
            this.xrLblOperatorName,
            this.imgLogo,
            this.lblOperator});
            this.ReportHeader.HeightF = 175.4351F;
            this.ReportHeader.Name = "ReportHeader";
            this.ReportHeader.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.ReportHeader.StylePriority.UseBorders = false;
            // 
            // xrLblPlant
            // 
            this.xrLblPlant.CanGrow = false;
            this.xrLblPlant.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblPlant.LocationFloat = new DevExpress.Utils.PointFloat(380.51F, 135.11F);
            this.xrLblPlant.Name = "xrLblPlant";
            this.xrLblPlant.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.xrLblPlant.SizeF = new System.Drawing.SizeF(185.03F, 32.01443F);
            this.xrLblPlant.StylePriority.UseFont = false;
            this.xrLblPlant.StylePriority.UsePadding = false;
            this.xrLblPlant.StylePriority.UseTextAlignment = false;
            this.xrLblPlant.Text = "Plant";
            this.xrLblPlant.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLine14
            // 
            this.xrLine14.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine14.LocationFloat = new DevExpress.Utils.PointFloat(19.44267F, 74.43513F);
            this.xrLine14.Name = "xrLine14";
            this.xrLine14.SizeF = new System.Drawing.SizeF(2.083344F, 101F);
            // 
            // xrLine13
            // 
            this.xrLine13.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine13.LocationFloat = new DevExpress.Utils.PointFloat(805.75F, 74.43513F);
            this.xrLine13.Name = "xrLine13";
            this.xrLine13.SizeF = new System.Drawing.SizeF(2.083344F, 101F);
            // 
            // xrLine12
            // 
            this.xrLine12.LocationFloat = new DevExpress.Utils.PointFloat(21.07225F, 72.43513F);
            this.xrLine12.Name = "xrLine12";
            this.xrLine12.SizeF = new System.Drawing.SizeF(786.9507F, 2.314812F);
            // 
            // xrLblOperatorName
            // 
            this.xrLblOperatorName.CanGrow = false;
            this.xrLblOperatorName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrLblOperatorName.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblOperatorName.LocationFloat = new DevExpress.Utils.PointFloat(569.5371F, 122.4607F);
            this.xrLblOperatorName.Name = "xrLblOperatorName";
            this.xrLblOperatorName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblOperatorName.SizeF = new System.Drawing.SizeF(227.2467F, 52.66F);
            this.xrLblOperatorName.StylePriority.UseFont = false;
            this.xrLblOperatorName.StylePriority.UseTextAlignment = false;
            this.xrLblOperatorName.Text = "xrLblOperatorName";
            this.xrLblOperatorName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // imgLogo
            // 
            this.imgLogo.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.TopLeft;
            this.imgLogo.LocationFloat = new DevExpress.Utils.PointFloat(30.51002F, 72.43513F);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.SizeF = new System.Drawing.SizeF(350F, 101.3708F);
            // 
            // lblOperator
            // 
            this.lblOperator.CanGrow = false;
            this.lblOperator.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperator.LocationFloat = new DevExpress.Utils.PointFloat(569.5371F, 73.74995F);
            this.lblOperator.Name = "lblOperator";
            this.lblOperator.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblOperator.SizeF = new System.Drawing.SizeF(155.2177F, 32.01443F);
            this.lblOperator.StylePriority.UseFont = false;
            this.lblOperator.StylePriority.UseTextAlignment = false;
            this.lblOperator.Text = "OPERATOR";
            this.lblOperator.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(Emdep.Geos.Data.Common.DeliveryNoteItemPrintDetails);
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.xrControlStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrControlStyle1.Name = "xrControlStyle1";
            // 
            // DeliveryNoteReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.bindingSource1});
            this.DataSource = this.bindingSource1;
            this.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 2);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLine xrLine4;
        private DevExpress.XtraReports.UI.XRLine xrLine5;
        private DevExpress.XtraReports.UI.XRLine xrLine6;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        public DevExpress.XtraReports.UI.XRLabel lblOperator;
        private DevExpress.XtraReports.UI.XRLine xrLine11;
        public DevExpress.XtraReports.UI.XRPictureBox imgLogo;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DevExpress.XtraReports.UI.XRLine xrLine15;
        private DevExpress.XtraReports.UI.XRLine xrLine16;
        private DevExpress.XtraReports.UI.XRLine xrLine8;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        public DevExpress.XtraReports.UI.XRLabel lblTradeGroup;
        public DevExpress.XtraReports.UI.XRLabel lblDeliveryNoteDate;
        public DevExpress.XtraReports.UI.XRLabel lblDeliveryNoteCode;
        public DevExpress.XtraReports.UI.XRLine xrLine3;
        public DevExpress.XtraReports.UI.XRLine xrLine2;
        public DevExpress.XtraReports.UI.XRLabel lblProducer;
        public DevExpress.XtraReports.UI.XRLabel lblSupplier;
        public DevExpress.XtraReports.UI.XRLabel lblReference;
        public DevExpress.XtraReports.UI.XRLabel lblItem;
        public DevExpress.XtraReports.UI.XRLabel lblMadeIn;
        public DevExpress.XtraReports.UI.XRLine xrLine7;
        public DevExpress.XtraReports.UI.XRLabel lblQuantity;
        public DevExpress.XtraReports.UI.XRLabel lblWarehouselocation;
        public DevExpress.XtraReports.UI.XRLine xrLine1;
        public DevExpress.XtraReports.UI.XRLabel xrLblMadeIn;
        public DevExpress.XtraReports.UI.XRLabel xrLblQuantity;
        public DevExpress.XtraReports.UI.XRLabel xrLblDeliveryNoteCode;
        public DevExpress.XtraReports.UI.XRLabel xrLblDeliveryNoteDate;
        public DevExpress.XtraReports.UI.XRLabel xrLblProducerName;
        public DevExpress.XtraReports.UI.XRLabel xrLblSupplierName;
        public DevExpress.XtraReports.UI.XRLabel xrLblOperatorName;
        public DevExpress.XtraReports.UI.XRLabel xrLblReference;
        public DevExpress.XtraReports.UI.XRLabel xrLblWarehouselocation;
        public DevExpress.XtraReports.UI.XRBarCode xrBarCode1;
        public DevExpress.XtraReports.UI.XRLine xrLine10;
        public DevExpress.XtraReports.UI.XRLine xrLine9;
        private DevExpress.XtraReports.UI.XRLine xrLine14;
        private DevExpress.XtraReports.UI.XRLine xrLine13;
        public DevExpress.XtraReports.UI.XRLine xrLine12;
        public DevExpress.XtraReports.UI.XRLabel xrLblPlant;
    }
}
