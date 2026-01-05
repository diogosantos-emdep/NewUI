namespace FileReplicator.WindowsService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FileReplicatorInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FileReplicator = new System.ServiceProcess.ServiceInstaller();
            // 
            // FileReplicatorInstaller
            // 
            this.FileReplicatorInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FileReplicatorInstaller.Password = null;
            this.FileReplicatorInstaller.Username = null;
            // 
            // FileReplicator
            // 
            this.FileReplicator.Description = "Geos File Replicator use for upload files from local path tracking in setting fil" +
    "es.";
            this.FileReplicator.DisplayName = "Geos File Replicator";
            this.FileReplicator.ServiceName = "FileReplicator";
            this.FileReplicator.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FileReplicatorInstaller,
            this.FileReplicator});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FileReplicatorInstaller;
        private System.ServiceProcess.ServiceInstaller FileReplicator;
    }
}