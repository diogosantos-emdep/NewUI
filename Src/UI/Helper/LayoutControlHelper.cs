using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Mvvm.UI.Interactivity;
using System.Windows.Data;

namespace Emdep.Geos.UI.Helper
{
  public  class LayoutControlHelper : Behavior<LayoutControl>
    {
        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }
        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(LayoutControlHelper), new UIPropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(LayoutControlHelper), new UIPropertyMetadata(null));

        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(LayoutControlHelper), new UIPropertyMetadata(null,
                new PropertyChangedCallback((d, e) =>
                {
                    ((LayoutControlHelper)d).OnItemsSourceChanged(e.OldValue, e.NewValue);
                })));

        protected virtual void OnItemsSourceChanged(object oldValue, object newValue)
        {
            if (newValue is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)newValue).CollectionChanged += OnItemsSourceCollectionChanged;
            }
            ArrangeChildren();
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
                AssociatedObject.Children.Clear();

            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    AddItem(item);
        //    if (e.OldItems != null)
        //        foreach (var item in e.OldItems)
        //            RemoveItem(item as SampleItem);
        }

        protected virtual void ArrangeChildren()
        {
            AssociatedObject.Children.Clear();
            AssociatedObject.Orientation = System.Windows.Controls.Orientation.Vertical;
            if (ItemsSource is IEnumerable)
                foreach (var item in (ItemsSource as IEnumerable))
                    AddItem(item);
        }
        protected virtual void RemoveItem(object current)
        {
            LayoutItem element = AssociatedObject.Children.OfType<LayoutItem>().Where(el => ((LayoutItem)el).DataContext == current).FirstOrDefault();
            AssociatedObject.Children.Remove(element);
        }

        protected virtual void AddItem(object current)
        {
            LayoutItem lItem = new LayoutItem { DataContext = current };

            var itemContent = new ContentControl { Content = current };
            lItem.Content = itemContent;
            itemContent.SetBinding(ContentControl.ContentTemplateProperty, new Binding("ItemTemplate")
            {
                Source = this,
                Mode = BindingMode.OneWay
            });
            itemContent.SetBinding(ContentControl.ContentTemplateSelectorProperty, new Binding("ItemTemplateSelector")
            {
                Source = this,
                Mode = BindingMode.OneWay
            });

            AssociatedObject.Children.Add(lItem);
        }

    }
}
