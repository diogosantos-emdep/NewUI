using DevExpress.Data;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.WindowsUI;
using System.Linq;

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for ActionsLauncherView.xaml
    /// </summary>
    public partial class ActionsLauncherView : WinUIDialogWindow
    {
        public ActionsLauncherView()
        {
            InitializeComponent();
        }

        //chitra.girigosavi GEOS2-7733 09/04/2025
        private void PlantTreeListView_CustomSummary(object sender, TreeListCustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                var treeListView = sender as TreeListView;
                if (treeListView != null)
                {
                    e.TotalValue = treeListView.Nodes.Count(n => n.ParentNode == null);
                }
            }
        }
    }
}
