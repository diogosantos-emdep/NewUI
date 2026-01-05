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



namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for Workbench.xaml
    /// </summary>
    public partial class Workbench : DXWindow
    {
        public Workbench()
        {
            InitializeComponent();
        }

        private void Gom_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GSm_Click(object sender, RoutedEventArgs e)
        {

        }

        

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
 MessageBox.Show("Hi");
        }

      

        private void NewGeos_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //MainWindow objMainWindow = new MainWindow();
            //objMainWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://10.0.0.3/front/helpdesk.public.php?create_ticket=1");
        }
    }
}
