using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.Epc.Common.EPC;
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

namespace Emdep.Geos.Modules.Epc.Views
{
    /// <summary>
    /// Interaction logic for WbsView.xaml
    /// </summary>
    public partial class WbsView : UserControl
    {
        public WbsView()
        {
            InitializeComponent();
        }

        private void AddWbsNode_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //GridMenuInfo menuInfo = treeListView1.DataControlMenu.MenuInfo;
            //if (menuInfo != null)
            //{
            //    TreeListNode node = treeListView1.GetNodeByRowHandle(treeListView1.FocusedRowHandle);
            //    node.Nodes.Add(new TreeListNode(new WBS() { Resources = ((WBS)node.Content).Resources, ParentID = ((WBS)node.Content).ParentID }));
            //}

        }     

        private void DeleteWbsNode_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //GridMenuInfo menuInfo = treeListView1.DataControlMenu.MenuInfo;
            //if (menuInfo != null)
            //    treeListView1.DeleteNode(treeListView1.FocusedRowHandle);


        }
    }
}
