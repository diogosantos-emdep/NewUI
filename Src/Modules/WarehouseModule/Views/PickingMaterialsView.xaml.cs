using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Modules.Warehouse.ViewModels;
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

namespace Emdep.Geos.Modules.Warehouse.Views
{
    /// <summary>
    /// Interaction logic for PickingMaterialsView.xaml
    /// </summary>
    public partial class PickingMaterialsView : WinUIDialogWindow
    {
        public PickingMaterialsView()
        {
            InitializeComponent();
        }


        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModel = this.DataContext as PickingMaterialsViewModel;

            if (viewModel != null)
            {
                bool canClose = true;
                if (viewModel.IsTimer)
                {
                 
                    // Call the method in the ViewModel
                     canClose = viewModel.CanCloseMethod();
                }
                // Cancel the closing if the ViewModel method returns false
                if (!canClose)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
