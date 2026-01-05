namespace ActivityReminder
{
    partial class BirthdayReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BirthdayReport));
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.birthdate = new DevExpress.XtraReports.UI.XRLabel();
            this.empname = new DevExpress.XtraReports.UI.XRLabel();
            this.happy = new DevExpress.XtraReports.UI.XRLabel();
            this.birthday = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 9.375F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2,
            this.birthdate,
            this.empname,
            this.happy,
            this.birthday,
            this.xrPictureBox1});
            this.Detail.HeightF = 1091.66F;
            this.Detail.Name = "Detail";
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(650.0834F, 156.0383F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(315.8367F, 308.7567F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // birthdate
            // 
            this.birthdate.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.birthdate.ForeColor = System.Drawing.Color.White;
            this.birthdate.LocationFloat = new DevExpress.Utils.PointFloat(246.9584F, 523.7915F);
            this.birthdate.Multiline = true;
            this.birthdate.Name = "birthdate";
            this.birthdate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.birthdate.SizeF = new System.Drawing.SizeF(586.4583F, 42.79156F);
            this.birthdate.StylePriority.UseFont = false;
            this.birthdate.StylePriority.UseForeColor = false;
            // 
            // empname
            // 
            this.empname.Font = new System.Drawing.Font("Calibri", 39.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.empname.ForeColor = System.Drawing.Color.White;
            this.empname.LocationFloat = new DevExpress.Utils.PointFloat(246.9584F, 475.7916F);
            this.empname.Multiline = true;
            this.empname.Name = "empname";
            this.empname.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.empname.SizeF = new System.Drawing.SizeF(431.25F, 47.99994F);
            this.empname.StylePriority.UseFont = false;
            this.empname.StylePriority.UseForeColor = false;
            // 
            // happy
            // 
            this.happy.Font = new System.Drawing.Font("Calibri", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.happy.ForeColor = System.Drawing.Color.White;
            this.happy.LocationFloat = new DevExpress.Utils.PointFloat(246.9584F, 317.7535F);
            this.happy.Multiline = true;
            this.happy.Name = "happy";
            this.happy.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.happy.SizeF = new System.Drawing.SizeF(403.125F, 66.74994F);
            this.happy.StylePriority.UseFont = false;
            this.happy.StylePriority.UseForeColor = false;
            // 
            // birthday
            // 
            this.birthday.Font = new System.Drawing.Font("Calibri", 60F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.birthday.ForeColor = System.Drawing.Color.White;
            this.birthday.LocationFloat = new DevExpress.Utils.PointFloat(246.9583F, 384.5034F);
            this.birthday.Multiline = true;
            this.birthday.Name = "birthday";
            this.birthday.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.birthday.SizeF = new System.Drawing.SizeF(431.25F, 66.74994F);
            this.birthday.StylePriority.UseFont = false;
            this.birthday.StylePriority.UseForeColor = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(1199F, 991.66F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // BirthdayReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail});
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(2, 0, 9, 100);
            this.PageHeight = 1917;
            this.PageWidth = 1201;
            this.PaperKind = System.Drawing.Printing.PaperKind.BPlus;
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        public DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        public DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        public DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        public DevExpress.XtraReports.UI.XRLabel birthdate;
        public DevExpress.XtraReports.UI.XRLabel empname;
        public DevExpress.XtraReports.UI.XRLabel happy;
        public DevExpress.XtraReports.UI.XRLabel birthday;
        public DevExpress.XtraReports.UI.XRPictureBox xrPictureBox2;
    }
}
