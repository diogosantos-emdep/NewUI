using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.APM.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.APM.CommonClasses
{
    public class GridExportService : ServiceBase, IGridExportService
    { //[rdixit][GEOS2-9316][26.08.2025]
        GridControl GridControl => AssociatedObject as GridControl;

        protected override void OnAttached()
        {
            base.OnAttached();
        }

        public void Export(string filePath)
        {
            var gridControl = AssociatedObject as GridControl;
            var detailView = gridControl?.View as TableView;

            if (detailView == null)
            {
                System.Windows.MessageBox.Show("TableView is NULL!");
                return;
            }

            detailView.ExportToXlsx(filePath);
            
        }
        public TableView GetView()
        {
            return GridControl?.View as TableView;
        }


        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
