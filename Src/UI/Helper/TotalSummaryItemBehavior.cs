using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public class TotalSummaryItemBehavior : Behavior<TreeListView>
    {
        protected override void OnAttached()
        {
            AssociatedObject.CustomSummary += AssociatedObject_CustomSummary; ;
            base.OnAttached();
        }

        private void AssociatedObject_CustomSummary(object sender, DevExpress.Xpf.Grid.TreeList.TreeListCustomSummaryEventArgs e)
        {
            switch (e.SummaryProcess)
            {
                case DevExpress.Data.CustomSummaryProcess.Start:
                    e.TotalValue = 0d;
                    break;
                case DevExpress.Data.CustomSummaryProcess.Calculate:
                    if (e.Node.ActualLevel == 0)
                        e.TotalValue = (double)e.TotalValue + ((double)((decimal)e.FieldValue));
                    break;
                case DevExpress.Data.CustomSummaryProcess.Finalize:
                    break;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
