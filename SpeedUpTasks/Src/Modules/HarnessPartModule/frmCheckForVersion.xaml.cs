using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using System.Xml;
using System.IO;


namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for frmCheckForVersion.xaml
    /// </summary>
    public partial class frmCheckForVersion : System.Windows.Window
    {
        public frmCheckForVersion()
        {
            InitializeComponent();

            ucsoftwareUpdate.btnRelease.Click += btnRelease_Click;
            ucsoftwareUpdate.BtnDone.Click += BtnDone_Click;
            ucsoftwareUpdate.btnDownloadlater.Click += btnDownloadlater_Click;

            ucReleaseNote.btnBack.Click += btnBack_Click;


        }

        void btnDownloadlater_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
          
            this.Close();
            login.ShowDialog();
           

        }

        void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
           this.Close();
            login.Show();
           
            
          
        }

        void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageView1.SelectedIndex = 0;
        }

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            PageView1.SelectedIndex = 1;
        }

        private void frmCheckForVersion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Escape")
            {
                this.Close();
            }
        }
    }
}