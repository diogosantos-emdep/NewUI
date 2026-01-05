using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Emdep.Geos.UI.Helper
{
    public class BestFitBehaviorSimulator : Behavior<GridControl>
    {
        protected override void OnAttached()
        {
            AssociatedObject.ItemsSourceChanged += Grid_ItemsSourceChanged;
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            base.OnAttached();
        }
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.ItemsSourceChanged -= Grid_ItemsSourceChanged;
            base.OnDetaching();
        }

        //Initial Best Fit
        private void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (GridColumn gridColumn in AssociatedObject.Columns)
            {
                gridColumn.AllowResizing = DevExpress.Utils.DefaultBoolean.True;
            }

            ((TableView)AssociatedObject.View).BestFitColumns();

            foreach (GridColumn gridColumn in AssociatedObject.Columns)
            {
                if (gridColumn.FieldName == "Reference" || gridColumn.FieldName == "Supplier")
                {
                    gridColumn.AllowResizing = DevExpress.Utils.DefaultBoolean.True;
                }
                else
                {
                    gridColumn.AllowResizing = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }

        //When a new source is assigned
        private void Grid_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            ((TableView)AssociatedObject.View).AllowResizing = true;

            foreach (GridColumn gridColumn in AssociatedObject.Columns)
            {
                gridColumn.AllowResizing = DevExpress.Utils.DefaultBoolean.Default;
            }

            ((TableView)AssociatedObject.View).BestFitColumns();

            //Dispatcher.BeginInvoke(new Action(() => ((TableView)AssociatedObject.View).BestFitColumns()), DispatcherPriority.Render);

            foreach (GridColumn gridColumn in AssociatedObject.Columns)
            {
                if (gridColumn.FieldName == "Reference" || gridColumn.FieldName == "Supplier")
                {
                    gridColumn.AllowResizing = DevExpress.Utils.DefaultBoolean.True;
                }
                else
                {
                    gridColumn.AllowResizing = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }
    }
}
