using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
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

namespace Emdep.Geos.Modules.SCM.Views
{
    /// <summary>
    /// Interaction logic for PropertiesManagerView.xaml
    /// </summary>
    public partial class PropertiesManagerView : WinUIDialogWindow
    {
        public PropertiesManagerView()
        {
            InitializeComponent();
        }
        private void View_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                e.Value = e.Node;
            }
        }
    }

}

