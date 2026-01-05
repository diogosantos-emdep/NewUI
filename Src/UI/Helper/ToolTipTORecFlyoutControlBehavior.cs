using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.OTM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Emdep.Geos.UI.Helper
{
    //[pramod.misal][04.02.2025][GEOS2-6726]
    //[rdixit][19.02.2025][GEOS2-6726]
    public class ToolTipTORecFlyoutControlBehavior : Behavior<GridControl>
    {
        private Point _lastMousePosition; // Track the last mouse position
        private DispatcherTimer _closeFlyoutTimer;

        public FlyoutControl FlyoutControl
        {
            get { return (FlyoutControl)GetValue(FlyoutControlProperty); }
            set { SetValue(FlyoutControlProperty, value); }
        }
        public static readonly DependencyProperty FlyoutControlProperty =
            DependencyProperty.Register("FlyoutControl", typeof(FlyoutControl), typeof(ToolTipTORecFlyoutControlBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave; // Handle mouse leave

            _closeFlyoutTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10) // Delay for closing the flyout
            };
            _closeFlyoutTimer.Tick += (s, e) =>
            {
                if (FlyoutControl != null)
                {
                    FlyoutControl.IsOpen = false;
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

            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "ToRecipientName")
            {

                List<ToRecipientName> toRecipientName = new List<ToRecipientName>();

                if (e.OriginalSource is DevExpress.Xpf.Grid.CellEditorBase cellEditor)
                {
                    toRecipientName = ((Emdep.Geos.Data.Common.OTM.PORequestDetails)cellEditor.CellData.Row).ToRecipientNameList;
                }

                if (toRecipientName != null && toRecipientName.Count > 0)
                {
                    var cellElement = view.GetCellElementByRowHandleAndColumn(hitInfo.RowHandle, hitInfo.Column);
                    if (cellElement != null)
                    {
                        FlyoutControl.PlacementTarget = cellElement;
                        FlyoutControl.DataContext = toRecipientName;
                        FlyoutControl.IsOpen = true;
                        _closeFlyoutTimer.Stop();
                    }
                }
            }
            else
            {
                _closeFlyoutTimer.Start(); // Start the timer if mouse moves out
            }
        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            // Close the flyout if the mouse leaves the grid
            _closeFlyoutTimer.Start();
        }
    }
}
