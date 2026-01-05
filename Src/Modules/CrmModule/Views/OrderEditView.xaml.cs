using DevExpress.Xpf.WindowsUI;
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

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for OrderEditView.xaml
    /// </summary>
    public partial class OrderEditView : WinUIDialogWindow
    {
        public OrderEditView()
        {
            InitializeComponent();
        }

        //[GEOS2-1931][28.12.2022][rdixit]
        private void GridGroup_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Grid Grid = (System.Windows.Controls.Grid)sender;
            Grid.Width = Grid.ActualWidth;
          
        }
    }
}
