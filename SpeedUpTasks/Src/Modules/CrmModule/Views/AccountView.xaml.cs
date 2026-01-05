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
using Emdep.Geos.Modules.Crm.ViewModels;


namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for DXWindow1.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmdepSitesWiseMapWindowViewModel emdepSitesWiseMapWindowViewModel = new EmdepSitesWiseMapWindowViewModel("Spain");
            // if (emdepSitesWiseMapWindowViewModel.IsInit == true)
            {
                EmdepSitesMapWindow emdepSitesMapWindow = new EmdepSitesMapWindow();
                EventHandler handle = delegate { emdepSitesMapWindow.Close(); };
                emdepSitesWiseMapWindowViewModel.RequestClose += handle;
                emdepSitesMapWindow.DataContext = emdepSitesWiseMapWindowViewModel;
                // GeosApplication.Instance.Logger.Log("Initialising EmdepSitesMapWindow Successfully", category: Category.Info, priority: Priority.Low);
                emdepSitesMapWindow.ShowDialogWindow();
            }
        }
    }
}
