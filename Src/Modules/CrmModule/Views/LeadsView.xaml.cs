using DevExpress.Xpf.Grid;
using DevExpress.Xpf.PivotGrid;
using DevExpress.Xpf.PivotGrid.Internal;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Leads.xaml
    /// </summary>
    public partial class LeadsView : UserControl
    {
        public LeadsView()
        {
            InitializeComponent();

          
            this.Loaded += LeadsView_Loaded;

            CRMCommon.Instance.IsTimelineOpen = true;
            Loaded += (s, e) =>
            {
                Window.GetWindow(this).Closing += (s1, e1) =>
                {
                    CRMCommon.Instance.IsTimelineOpen = false;
                };
            };
        }

        private void LeadsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is LeadsViewModel vm)
            {
                vm.InitializeGridControl(this.grid); 
            }
        }
    }
}
