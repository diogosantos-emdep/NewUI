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


namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for frmSetting.xaml
    /// </summary>
    public partial class frmSetting : WinUIDialogWindow
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Escape")
            {
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
