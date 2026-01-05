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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for ucForgottenPassword.xaml
    /// </summary>
    public partial class ucForgottenPassword : UserControl
    {
        public ucForgottenPassword()
        {
            InitializeComponent();
            //gridForgott.RowDefinitions[7].Height = new GridLength(0);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            //gridForgott.RowDefinitions[7].Height = new GridLength(110);
            //// gridForgott.RowDefinitions[1].Height = new GridLength(0);
            //TextBlockmsg.Visibility = Visibility.Visible;

        }

        private void sendmail_Click(object sender, RoutedEventArgs e)
        {
            imagemail.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
