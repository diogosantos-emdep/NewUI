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
using DevExpress.Xpf.WindowsUI;

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for frmNewSoftwareUpdate.xaml
    /// </summary>
    public partial class ucNewSoftwareUpdate : UserControl
    {
        public ucNewSoftwareUpdate()
        {

            ThemeManager.ApplicationThemeName = "BlackAndBlue";
            InitializeComponent();
        }
        
        private void Downloadnow_Click(object sender, RoutedEventArgs e)
        {
            downloadpanel.Visibility = Visibility.Visible;
            
            //frmDownloadUpdate frmDownloadUpdate = new frmDownloadUpdate();
            //frmDownloadUpdate.ShowDialogWindow();
            //this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            downloadpanel.Visibility = Visibility.Hidden;
        }

        private void objucNewSoftwareUpdate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Escape")
            {
                //Close();
            }
        }
 
    }
}
