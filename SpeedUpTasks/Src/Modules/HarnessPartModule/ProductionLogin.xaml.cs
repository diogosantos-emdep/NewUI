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
    /// Interaction logic for ProductionLogin.xaml
    /// </summary>
    public partial class ProductionLogin : WinUIDialogWindow
    {
        public ProductionLogin()
        {
            ThemeManager.ApplicationThemeName = "BlackAndBlue";
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void WinUIDialogWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString()=="Escape")
            {
                Close();
            }
        }
    }
}
