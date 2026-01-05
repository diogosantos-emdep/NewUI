using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.OTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Emdep.Geos.UI.Helper
{
    //[pooja.jadhav][17.02.2025][GEOS2-6724]
    public class ToolTipAttachmentCountflyoutControlBehaviour : Behavior<GridControl>
    {
        private Point _lastMousePosition; // Track the last mouse position
        private DispatcherTimer _closeFlyoutTimer; // Timer to manage delayed flyout closure

        public FlyoutControl FlyoutControl
        {
            get { return (FlyoutControl)GetValue(FlyoutControlProperty); }
            set { SetValue(FlyoutControlProperty, value); }
        }
        public static readonly DependencyProperty FlyoutControlProperty =
            DependencyProperty.Register("FlyoutControl", typeof(FlyoutControl), typeof(ToolTipAttachmentCountflyoutControlBehaviour), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;

            // Initialize the timer with a short delay
            _closeFlyoutTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10) // 300ms delay for flyout closure
            };
            _closeFlyoutTimer.Tick += (s, e) =>
            {
                if (FlyoutControl != null)
                {
                    FlyoutControl.IsOpen = false; // Close the flyout when the timer elapses
                }
                _closeFlyoutTimer.Stop();
            };
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
            base.OnDetaching();
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            GridControl grid = (GridControl)sender;
            var view = grid.View as TableView;
            TableViewHitInfo hitInfo = view.CalcHitInfo(e.OriginalSource as DependencyObject);

            // Get the current mouse position
            Point currentMousePosition = e.GetPosition(AssociatedObject);

            // Check for horizontal movement
            if (Math.Abs(currentMousePosition.X - _lastMousePosition.X) > Math.Abs(currentMousePosition.Y - _lastMousePosition.Y))
            {
                if (FlyoutControl != null)
                {
                    FlyoutControl.IsOpen = false; // Close the flyout on horizontal movement
                }
                _lastMousePosition = currentMousePosition;
                return;
            }

            _lastMousePosition = currentMousePosition; // Update the last mouse position

            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "AttachmentCnt")
            {

                List<Emailattachment> attachemnt = new List<Emailattachment>();
                if (e.OriginalSource is DevExpress.Xpf.Grid.CellEditorBase cellEditor)
                {
                    attachemnt = ((Emdep.Geos.Data.Common.OTM.PORequestDetails)cellEditor.CellData.Row).EmailAttachmentList;
                }
                if (attachemnt != null && attachemnt.Count > 0)
                {
                    var cellElement = view.GetCellElementByRowHandleAndColumn(hitInfo.RowHandle, hitInfo.Column);
                    if (cellElement != null)
                    {
                        FlyoutControl.PlacementTarget = cellElement;
                        FlyoutControl.DataContext = attachemnt;
                        FlyoutControl.IsOpen = true;
                        _closeFlyoutTimer.Stop();
                    }
                }

            }
            else
            {
                // Start the close timer if the mouse leaves the cell or column
                _closeFlyoutTimer.Start();
            }
        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            // Close the flyout if the mouse leaves the grid
            _closeFlyoutTimer.Start();
        }
    }
}
