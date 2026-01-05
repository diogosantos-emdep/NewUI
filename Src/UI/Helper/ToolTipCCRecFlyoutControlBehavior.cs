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
    //[nsatpute][03.03.2025][GEOS2-6726]
    public class ToolTipCCRecFlyoutControlBehavior : Behavior<GridControl>
    {
        private Point _lastMousePosition; // Track the last mouse position
        private DispatcherTimer _closeFlyoutTimer; // Timer for delayed flyout closure

        public FlyoutControl FlyoutControl
        {
            get { return (FlyoutControl)GetValue(FlyoutControlProperty); }
            set { SetValue(FlyoutControlProperty, value); }
        }
        public static readonly DependencyProperty FlyoutControlProperty =
            DependencyProperty.Register("FlyoutControl", typeof(FlyoutControl), typeof(ToolTipCCRecFlyoutControlBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;

            // Initialize the timer with a 300ms delay
            _closeFlyoutTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
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

            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "CCName")
            {
                List<ToCCName> ccNames = new List<ToCCName>();

                if (e.OriginalSource is DevExpress.Xpf.Grid.CellEditorBase cellEditor)
                {
                    ccNames = ((Emdep.Geos.Data.Common.OTM.PORequestDetails)cellEditor.CellData.Row).CCNameList;
                }
                if (ccNames != null && ccNames.Count > 0)
                {
                    var cellElement = view.GetCellElementByRowHandleAndColumn(hitInfo.RowHandle, hitInfo.Column);
                    if (cellElement != null)
                    {
                        FlyoutControl.PlacementTarget = cellElement;
                        FlyoutControl.DataContext = ccNames;
                        FlyoutControl.IsOpen = true;
                        _closeFlyoutTimer.Stop();
                    }
                }
            }
            else
            {
                _closeFlyoutTimer.Start(); // Start the timer for delayed flyout closure
            }
        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            _closeFlyoutTimer.Start(); // Close the flyout when the mouse leaves the grid
        }
    }
}
