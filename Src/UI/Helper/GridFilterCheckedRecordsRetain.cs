using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;

namespace Emdep.Geos.UI.Helper
{
    public class GridFilterCheckedRecordsRetain : DevExpress.Xpf.Grid.GridControl
    {
        ObservableCollectionCore<object> mySelectedItems;
        public IList MySelectedItems { get { return mySelectedItems; } }
        public GridFilterCheckedRecordsRetain()
        {
            SelectionChanged += MyGridControl_SelectionChanged;
            mySelectedItems = new ObservableCollectionCore<object>();
        }

        Hashtable selection = new Hashtable();
        IEnumerable OrderedSelection { get { return selection.Keys.Cast<int>().OrderBy(x => x); } }
        protected override void OnItemsSourceChanged(object oldValue, object newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            selection.Clear();
            IEnumerable itemsSource = newValue as IEnumerable;
            if (itemsSource == null)
                return;
            int i = 0;
            foreach (object item in itemsSource)
                selection[i++] = false;
        }


        Locker updateLocker = new Locker();
        void MyGridControl_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (updateLocker.IsLocked)
                return;
            for (int i = 0; i < VisibleRowCount; i++)
            {
                int rowHandle = GetRowHandleByVisibleIndex(i);
                selection[GetListIndexByRowHandle(rowHandle)] = View.IsRowSelected(rowHandle);
            }
            mySelectedItems.BeginUpdate();
            mySelectedItems.Clear();
            foreach (int index in OrderedSelection)
            {
                if ((bool)selection[index])
                    mySelectedItems.Add(GetRowByListIndex(index));
            }
            mySelectedItems.EndUpdate();
        }
        protected override void ApplyFilter(bool checkFilterEnabled, bool skipIfFilterEquals = false)
        {
            updateLocker.DoLockedAction(() => {
                base.ApplyFilter(checkFilterEnabled, skipIfFilterEquals);
                BeginSelection();
                foreach (int index in OrderedSelection)
                {
                    if ((bool)selection[index])
                        SelectItem(GetRowHandleByListIndex(index));
                }
                EndSelection();
            });
        }
    }
}
