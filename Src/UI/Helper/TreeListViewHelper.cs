using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.UI.Helper
{
    //class TreeListViewHelper
    //{
    //}

    public class LookUpEditEx : LookUpEdit
    {
        public static readonly DependencyProperty AllowClosingProperty =
          DependencyProperty.Register("AllowClosing", typeof(bool), typeof(LookUpEditEx), new UIPropertyMetadata(true));

        public bool AllowClosing
        {
            get { return (bool)GetValue(AllowClosingProperty); }
            set { SetValue(AllowClosingProperty, value); }
        }

        protected override bool RaisePopupClosing(PopupCloseMode mode, object value)
        {
            if (!AllowClosing && Popup.IsMouseOver)
            {
                AllowClosing = true;
                return false;
            }

            return base.RaisePopupClosing(mode, value);
        }
    }

    public delegate void CanceledEventHandler(object sender, CanceledEventArgs e);

    public class MyTreeListView : TreeListView
    {
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            int rowHandle = GetRowHandleByMouseEventArgs(e);
            ProductCategory probl = DataControl.GetRow(rowHandle) as ProductCategory;
            LookUpEditEx lookUp1 = BaseEdit.GetOwnerEdit(this) as LookUpEditEx;

            if (probl != null)
                lookUp1.AllowClosing = (!probl.IsDisabled);
        }

        public event CanceledEventHandler FocusedRowChanging;
        static MyTreeListView()
        {
            FocusedRowHandleProperty.OverrideMetadata(typeof(MyTreeListView), new FrameworkPropertyMetadata(GridControl.InvalidRowHandle, null,
                 (d, e) => ((MyTreeListView)d).CoerceFocusedRowHandle((int)e)));
        }

        object CoerceFocusedRowHandle(int value)
        {
            if (FocusedRowHandle == value)
                return value;
            if (FocusedRowChanging != null)
            {
                CanceledEventArgs e = new CanceledEventArgs(FocusedRowHandle, value);
                FocusedRowChanging(this, e);
                if (e.Cancel)
                    return FocusedRowHandle;
            }
            return value;
        }
    }

    public class CanceledEventArgs : EventArgs
    {
        public int NewRowHandle { get; private set; }
        public int OldRowHandle { get; private set; }
        public bool Cancel { get; set; }

        public CanceledEventArgs(int oldRowHandle, int newRowHandle)
        {
            OldRowHandle = oldRowHandle;
            NewRowHandle = newRowHandle;
            Cancel = false;
        }
    }

}
