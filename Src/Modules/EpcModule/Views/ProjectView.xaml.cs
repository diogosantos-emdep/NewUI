using DevExpress.Xpf.Data;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
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
    /// Interaction logic for ProjectView.xaml
    /// </summary>
    public partial class ProjectView : UserControl
    {
        public ProjectView()
        {
            InitializeComponent();
        }

        private void DevelopmentTreeList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            TreeListViewHitInfo hitInfo = ((TreeListView)gcDevelopmentTask.View).CalcHitInfo(e.OriginalSource as DependencyObject);
            if (hitInfo.HitTest == TreeListViewHitTest.RowIndicator)
            {

                //TreeListNode clickedNode = gcDevelopmentTask.View.GetValue(hi.RowHandle);
                object o = gcDevelopmentTask.GetRow(hitInfo.RowHandle);

            }
        }
    }
}
