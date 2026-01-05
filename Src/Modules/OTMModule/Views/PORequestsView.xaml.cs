using DevExpress.Xpf.Core;
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

namespace Emdep.Geos.Modules.OTM.Views
{

    /// <summary>
    /// [001][pramod.misal][GEOS2-6520][8.10.2024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
    /// </summary>
    ///
    /// <returns></returns>
    public partial class PORequestsView : UserControl
    {
        DevExpress.Xpf.Docking.CaptionLocation CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Top;
        public PORequestsView()
        {
            InitializeComponent();
            CaptionLocation = DevExpress.Xpf.Docking.CaptionLocation.Top;
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }
    }
}
