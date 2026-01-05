namespace GEOS_Workbench_Outlook_Addin
{
    partial class RibbonToOutLook : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RibbonToOutLook()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RibbonToOutLook));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.CRMMenu = this.Factory.CreateRibbonMenu();
            this.tbOpportunity = this.Factory.CreateRibbonToggleButton();
            this.toggleButton2 = this.Factory.CreateRibbonToggleButton();
            this.toggleButton3 = this.Factory.CreateRibbonToggleButton();
            this.toggleButton4 = this.Factory.CreateRibbonToggleButton();
            this.toggleButton5 = this.Factory.CreateRibbonToggleButton();
            this.tbAction = this.Factory.CreateRibbonToggleButton();
            this.toggleButton6 = this.Factory.CreateRibbonToggleButton();
            this.toggleButton7 = this.Factory.CreateRibbonToggleButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "GEOS";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.CRMMenu);
            this.group1.Name = "group1";
            // 
            // CRMMenu
            // 
            this.CRMMenu.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CRMMenu.Description = "Add";
            this.CRMMenu.Dynamic = true;
            this.CRMMenu.Image = ((System.Drawing.Image)(resources.GetObject("CRMMenu.Image")));
            this.CRMMenu.Items.Add(this.tbOpportunity);
            this.CRMMenu.Items.Add(this.toggleButton2);
            this.CRMMenu.Items.Add(this.toggleButton3);
            this.CRMMenu.Items.Add(this.toggleButton4);
            this.CRMMenu.Items.Add(this.toggleButton5);
            this.CRMMenu.Items.Add(this.tbAction);
            this.CRMMenu.Items.Add(this.toggleButton6);
            this.CRMMenu.Items.Add(this.toggleButton7);
            this.CRMMenu.Label = "CRM";
            this.CRMMenu.Name = "CRMMenu";
            this.CRMMenu.ShowImage = true;
            // 
            // tbOpportunity
            // 
            this.tbOpportunity.Image = ((System.Drawing.Image)(resources.GetObject("tbOpportunity.Image")));
            this.tbOpportunity.Label = "New Opportunity";
            this.tbOpportunity.Name = "tbOpportunity";
            this.tbOpportunity.ShowImage = true;
            this.tbOpportunity.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewOpportunity_Click);
            // 
            // toggleButton2
            // 
            this.toggleButton2.Image = ((System.Drawing.Image)(resources.GetObject("toggleButton2.Image")));
            this.toggleButton2.Label = "New Appointment";
            this.toggleButton2.Name = "toggleButton2";
            this.toggleButton2.ShowImage = true;
            this.toggleButton2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewAppointment_Click);
            // 
            // toggleButton3
            // 
            this.toggleButton3.Image = ((System.Drawing.Image)(resources.GetObject("toggleButton3.Image")));
            this.toggleButton3.Label = "New Call";
            this.toggleButton3.Name = "toggleButton3";
            this.toggleButton3.ShowImage = true;
            this.toggleButton3.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewCall_Click);
            // 
            // toggleButton4
            // 
            this.toggleButton4.Image = ((System.Drawing.Image)(resources.GetObject("toggleButton4.Image")));
            this.toggleButton4.Label = "New Email";
            this.toggleButton4.Name = "toggleButton4";
            this.toggleButton4.ShowImage = true;
            this.toggleButton4.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewEmail_Click);
            // 
            // toggleButton5
            // 
            this.toggleButton5.Image = ((System.Drawing.Image)(resources.GetObject("toggleButton5.Image")));
            this.toggleButton5.Label = "New Task";
            this.toggleButton5.Name = "toggleButton5";
            this.toggleButton5.ShowImage = true;
            this.toggleButton5.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewTask_Click);
            // 
            // tbAction
            // 
            this.tbAction.Image = ((System.Drawing.Image)(resources.GetObject("tbAction.Image")));
            this.tbAction.Label = "New Action";
            this.tbAction.Name = "tbAction";
            this.tbAction.ShowImage = true;
            this.tbAction.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewAction_Click);
            // 
            // toggleButton6
            // 
            this.toggleButton6.Image = ((System.Drawing.Image)(resources.GetObject("toggleButton6.Image")));
            this.toggleButton6.Label = "Attach to Opportunity";
            this.toggleButton6.Name = "toggleButton6";
            this.toggleButton6.ShowImage = true;
            this.toggleButton6.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.AttachToOpportunity_Click);
            // 
            // toggleButton7
            // 
            this.toggleButton7.Image = ((System.Drawing.Image)(resources.GetObject("toggleButton7.Image")));
            this.toggleButton7.Label = "Attach to Activity";
            this.toggleButton7.Name = "toggleButton7";
            this.toggleButton7.ShowImage = true;
            this.toggleButton7.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.AttachToActivity_Click);
            // 
            // RibbonToOutLook
            // 
            this.Name = "RibbonToOutLook";
            this.RibbonType = "Microsoft.Outlook.Explorer, Microsoft.Outlook.Mail.Compose, Microsoft.Outlook.Mai" +
    "l.Read, Microsoft.Outlook.Response.Read";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonToOutLook_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu CRMMenu;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton tbOpportunity;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton2;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton3;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton4;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton5;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton6;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton7;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton tbAction;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonToOutLook RibbonToOutLook
        {
            get { return this.GetRibbon<RibbonToOutLook>(); }
        }
    }
}
