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
    /// Interaction logic for EditProfile.xaml
    /// </summary>
    public partial class EditProfile : WinUIDialogWindow
    {
        public EditProfile()
        {
            //Theme theme2 = new Theme("BlackWithBlue", "DevExpress.Xpf.Themes.BlackWithBlue.v15.1");
            //theme2.AssemblyName = "DevExpress.Xpf.Themes.BlackWithBlue.v15.1";
            //Theme.RegisterTheme(theme2); 
            ThemeManager.ApplicationThemeName = "BlackAndBlue";
            InitializeComponent();


        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void objEditProfile_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Escape")
            {
                Close();
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            frmChangePassword fcp = new frmChangePassword();
            fcp.ShowDialogWindow();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }


        
    }
}
