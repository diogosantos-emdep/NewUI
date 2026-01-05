using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class ShowDetailBehavior : Behavior<TableView>
    {
        public static bool GetShowDetail(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowDetailProperty);
        }
        public static void SetShowDetail(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowDetailProperty, value);
        }
        public static readonly DependencyProperty ShowDetailProperty =
            DependencyProperty.RegisterAttached("ShowDetail", typeof(bool), typeof(ShowDetailBehavior), new PropertyMetadata(true));

        public TableView View { get { return AssociatedObject; } }

        protected override void OnAttached()
        {
            base.OnAttached();

            View.PreviewMouseRightButtonDown += OnAssociatedObjectPreviewMouseRightButtonDown;
        }
        protected override void OnDetaching()
        {
            View.PreviewMouseRightButtonDown -= OnAssociatedObjectPreviewMouseRightButtonDown;
            base.OnDetaching();
        }

        void OnAssociatedObjectPreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnViewPreviewMouseRightButtonDown(e);
        }

        protected virtual void OnViewPreviewMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            var hi = View.CalcHitInfo((DependencyObject)e.OriginalSource);

            if (hi.InRow)
            {
                var state = View.Grid.GetRowState(hi.RowHandle, true);
                SetShowDetail(state, !GetShowDetail(state));
            }
        }
    }
}
