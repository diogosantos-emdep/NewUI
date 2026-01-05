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
using Emdep.Geos.Modules.HarnessPart.Views;


namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for AdminLogin.xaml
    /// </summary>
    public partial class AdminLogin : WinUIDialogWindow
    {
        public AdminLogin()
        {
            //Theme theme = new Theme("BlackWithBlue", "DevExpress.Xpf.Themes.BlackWithBlue.v15.1");
            //theme.AssemblyName = "DevExpress.Xpf.Themes.BlackWithBlue.v15.1";
            //Theme.RegisterTheme(theme);
            //Theme theme1 = new Theme("colorBlue", "DevExpress.Xpf.Themes.colorBlue.v15.1");
            //theme1.AssemblyName = "DevExpress.Xpf.Themes.colorBlue.v15.1";
            //Theme.RegisterTheme(theme1);
            ThemeManager.ApplicationThemeName = "BlackAndBlue";
            InitializeComponent();
           
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //objucWorkbench.Visibility = System.Windows.Visibility.Visible;
       
            //objAdminLogin.Visibility = System.Windows.Visibility.Hidden;
            //objAdminLogin.AddChild(objucWorkbench);
           // Window1 w = new Window1(); 
            windowsui objwindowsui = new windowsui();
            objwindowsui.Show();
            //Workbench objw = new Workbench();
            //objw.Show();
            this.Close();
            //objAdminLogin.Content = new Emdep.Geos.Modules.HarnessPart.Views.ucWorkbench();
            //objAdminLogin.Show();
            
        }
    }
}
