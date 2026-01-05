using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.OTM;
using System.Windows;
using System.Windows.Input;
namespace Emdep.Geos.UI.Helper
{
	//[nsatpute][11-02-2025][GEOS2-GEOS2-6726]
    public  class ToolTipFlyoutControlBehavior : Behavior<GridControl>
    {
        
        public FlyoutControl FlyoutControl
        {
            get { return (FlyoutControl)GetValue(FlyoutControlProperty); }
            set { SetValue(FlyoutControlProperty, value); }
        }
        public static readonly DependencyProperty FlyoutControlProperty =
            DependencyProperty.Register("FlyoutControl", typeof(FlyoutControl), typeof(ToolTipFlyoutControlBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            base.OnDetaching();
        }
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            var view = AssociatedObject.View as TableView;
            TableViewHitInfo hitInfo = view.CalcHitInfo(e.OriginalSource as DependencyObject);

            if (hitInfo.InRowCell && (hitInfo.Column.FieldName == "SenderName"))
            {
                if (hitInfo.Column.FieldName == "SenderName")
                {
                    var dataItem = this.AssociatedObject.GetRow(hitInfo.RowHandle) as PORequestDetails;
                    var cellElement = view.GetCellElementByRowHandleAndColumn(hitInfo.RowHandle, hitInfo.Column);
                    var rowElement = view.GetRowElementByRowHandle(hitInfo.RowHandle);
                    FlyoutControl.PlacementTarget = cellElement;
                    FlyoutControl.IsOpen = !string.IsNullOrEmpty(dataItem.SenderName);
                    FlyoutControl.Width = 350;
                    FlyoutControl.Height = 80;


                }
                //if (hitInfo.InRowCell && hitInfo.Column.FieldName == "ToRecipientName")
                //{
                //    var dataItem = this.AssociatedObject.GetRow(hitInfo.RowHandle) as PORequestDetails;
                //    var cellElement = view.GetCellElementByRowHandleAndColumn(hitInfo.RowHandle, hitInfo.Column);
                //    var rowElement = view.GetRowElementByRowHandle(hitInfo.RowHandle);
                //    FlyoutControl.PlacementTarget = cellElement;
                //    FlyoutControl.IsOpen = !string.IsNullOrEmpty(dataItem.ToRecipientName);
                //    FlyoutControl.Width = 350;
                //    FlyoutControl.Height = 80;

                //}
                //if (hitInfo.InRowCell && hitInfo.Column.FieldName == "CCName")
                //{
                //    var dataItem = this.AssociatedObject.GetRow(hitInfo.RowHandle) as PORequestDetails;
                //    var cellElement = view.GetCellElementByRowHandleAndColumn(hitInfo.RowHandle, hitInfo.Column);
                //    var rowElement = view.GetRowElementByRowHandle(hitInfo.RowHandle);
                //    FlyoutControl.PlacementTarget = cellElement;
                //    FlyoutControl.IsOpen = !string.IsNullOrEmpty(dataItem.CCName);
                //    FlyoutControl.Width = 350;
                //    FlyoutControl.Height = 80;

                //}


            }
            else
            {
                if (FlyoutControl!=null)
                {
                    FlyoutControl.IsOpen = false;

                }
                              
            }

        }
    }
}