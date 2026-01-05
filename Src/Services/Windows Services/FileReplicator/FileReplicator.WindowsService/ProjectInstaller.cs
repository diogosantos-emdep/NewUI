using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace FileReplicator.WindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }
    }
}
