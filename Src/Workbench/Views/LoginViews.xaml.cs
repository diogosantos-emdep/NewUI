using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Workbench.ViewModels;

namespace Workbench.Views
{
    /// <summary>
    /// Interaction logic for LoginViews.xaml
    /// </summary>
    public partial class LoginViews : DevExpress.Xpf.Core.DXWindow
    {
        public LoginViews()
        {
            InitializeComponent();
        }

        private void DXWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                this.DataContext = null;
                this.Close();
            }
        }

        // 👇 NEW: Eye toggle handler
        private void ToggleEye_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext is LoginWindowViewModel vm)
            {
                vm.ShowPassword = !vm.ShowPassword;
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginWindowViewModel vm)
            {
                vm.ShowPasswordLabel = Visibility.Visible;
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginWindowViewModel vm)
            {
                if (vm.LoginPassword == null || vm.LoginPassword.Trim() == string.Empty)
                    vm.ShowPasswordLabel = Visibility.Hidden;
                else
                    vm.ShowPasswordLabel = Visibility.Visible;
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        private void Cmblogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginWindowViewModel vm)
            {
                vm.ShowUserNameLabel = Visibility.Visible;
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        private void Cmblogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginWindowViewModel vm)
            {
                if (vm.LoginName == null || vm.LoginName.Trim() == string.Empty)
                    vm.ShowUserNameLabel = Visibility.Hidden;
                else
                    vm.ShowUserNameLabel = Visibility.Visible;
            }
        }
		//[nsatpute][04.09.2025][GEOS2-9342]
        private void visiblePasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginWindowViewModel vm)
            {
                vm.ShowPasswordLabel = Visibility.Visible;
            }
        }
		//[nsatpute][04.09.2025][GEOS2-9342]
        private void visiblePasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginWindowViewModel vm)
            {
                if (vm.LoginPassword == null || vm.LoginPassword.Trim() == string.Empty)
                    vm.ShowPasswordLabel = Visibility.Hidden;
                else
                    vm.ShowPasswordLabel = Visibility.Visible;
            }
        }
    }
}
