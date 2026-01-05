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
          
        }

        private void DXWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                this.DataContext = null;
                this.Close();
            }
        }
    }
}