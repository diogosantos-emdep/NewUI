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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Workbench.ViewModels;

namespace Workbench.Views
{
    /// <summary>
    /// Interaction logic for PersonatePageView.xaml
    /// </summary>
    public partial class PersonatePageView : UserControl
    {
        public PersonatePageView()
        {
            InitializeComponent();
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        private void UserControlImpersonate_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginWindowViewModel vm)
            {
                vm.ShowLoginImpersonateNameLabel = Visibility.Visible;
            }
        }
		//[nsatpute][01.09.2025][GEOS2-9342]
        private void UserControlImpersonate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginWindowViewModel vm)
            {
                if (vm.LoginImpersonateName == null || vm.LoginImpersonateName.Trim() == string.Empty)
                    vm.ShowLoginImpersonateNameLabel = Visibility.Hidden;
                else
                    vm.ShowLoginImpersonateNameLabel = Visibility.Visible;
            }
        }  
    }
}
