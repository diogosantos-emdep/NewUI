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
using Emdep.Geos.Modules.Epc.Views;



namespace Workbench.Views
{
    /// <summary>
    /// Interaction logic for WorkbenchWindow.xaml
    /// </summary>
    public partial class WorkbenchWindow : DXWindow
    {
        const string _name=null ;
        public WorkbenchWindow()
        {
            InitializeComponent();
            this.PreviewMouseRightButtonDown += DXWindow_PreviewMouseRightButtonDown;

        }

        private void DXWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                this.DataContext = null;
                this.Close();
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {

        }

        //private void AppBar_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{

        //}

        private void DXWindow_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true; // disables right-click for this window only
        }
    }
}