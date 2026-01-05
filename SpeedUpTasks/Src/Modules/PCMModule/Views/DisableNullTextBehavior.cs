using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors;

namespace Emdep.Geos.Modules.PCM.Views
{
    class DisableNullTextBehavior : Behavior<TextEdit>
    {
        bool isNullTextEnabled;
        bool isAllowDropEnabled;
        protected override void OnAttached()
        {
            base.OnAttached();
            isAllowDropEnabled = AssociatedObject.AllowDrop;
            AssociatedObject.AllowDrop = true;
            AssociatedObject.PreviewDragEnter += AssociatedObject_PreviewDragEnter;
            AssociatedObject.PreviewDragLeave += AssociatedObject_PreviewDragLeave;
            AssociatedObject.EditValueChanged += AssociatedObject_EditValueChanged;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.AllowDrop = isAllowDropEnabled;
            AssociatedObject.PreviewDragEnter -= AssociatedObject_PreviewDragEnter;
            AssociatedObject.PreviewDragLeave -= AssociatedObject_PreviewDragLeave;
            AssociatedObject.EditValueChanged -= AssociatedObject_EditValueChanged;
            base.OnDetaching();
        }
        private void AssociatedObject_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            AssociatedObject.ShowNullText = isNullTextEnabled;
        }
        private void AssociatedObject_PreviewDragLeave(object sender, System.Windows.DragEventArgs e)
        {
            AssociatedObject.ShowNullText = isNullTextEnabled;
        }
        private void AssociatedObject_PreviewDragEnter(object sender, System.Windows.DragEventArgs e)
        {
            isNullTextEnabled = AssociatedObject.ShowNullText;
            if (isNullTextEnabled)
            {
                AssociatedObject.ShowNullText = false;
            }
        }
    }
}
