using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
   public class ComboBoxEditEnabledItemHelper
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(WinUIDialogWindow), new FrameworkPropertyMetadata(IsEnabledPropertyChanged));
        public static void SetIsEnabled(UIElement element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }
        public static bool GetIsEnabled(UIElement element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        private static void IsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ListBoxEdit listBox = source as ListBoxEdit;
            listBox.EditValueChanged += listBox_EditValueChanged;
        }

        static void listBox_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            ListBoxEdit listBox = sender as ListBoxEdit;
            List<LookupValue> itemToRemove = new List<LookupValue>();
            for (int i = 0; i < listBox.SelectedItems.Count; i++)
            {
                IList<LookupValue> lookup = listBox.ItemsSource as IList<LookupValue>;
                LookupValue item = lookup[i];
                if (item != null && item.InUse == Convert.ToBoolean(0))
                    itemToRemove.Add(item);
              
            }
            foreach (LookupValue c in itemToRemove)
                listBox.SelectedItems.Remove(c);

            listBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                listBox = listBox;

            }));
         
        }
    }
}
