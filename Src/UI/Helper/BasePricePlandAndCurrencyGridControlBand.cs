using DevExpress.Xpf.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class BasePricePlandAndCurrencyGridControlBand : GridControlBand
    {
        public static readonly DependencyProperty ColumnsSourceProperty =
            DependencyProperty.Register("ColumnsSource", typeof(IList), typeof(BasePricePlandAndCurrencyGridControlBand), new PropertyMetadata(null, OnColumnsSourcePropertyChanged));
        public static readonly DependencyProperty ColumnTemplateProperty =
            DependencyProperty.Register("ColumnTemplate", typeof(DataTemplate), typeof(BasePricePlandAndCurrencyGridControlBand), new PropertyMetadata(null));


        public BasePricePlantListColumnTemplateSelector ColumnTemplateSelector
        {
            get { return (BasePricePlantListColumnTemplateSelector)GetValue(ColumnTemplateSelectorProperty); }
            set { SetValue(ColumnTemplateSelectorProperty, value); }
        }



        // Using a DependencyProperty as the backing store for ColumnTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnTemplateSelectorProperty =
            DependencyProperty.Register("ColumnTemplateSelector", typeof(BasePricePlantListColumnTemplateSelector), typeof(BasePricePlandAndCurrencyGridControlBand), new PropertyMetadata(null));


        public IList ColumnsSource
        {
            get { return (IList)GetValue(ColumnsSourceProperty); }
            set { SetValue(ColumnsSourceProperty, value); }
        }
        public DataTemplate ColumnTemplate
        {
            get { return (DataTemplate)GetValue(ColumnTemplateProperty); }
            set { SetValue(ColumnTemplateProperty, value); }
        }

        private static void OnColumnsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BasePricePlandAndCurrencyGridControlBand)d).OnColumnsSourceChanged(e);
            //((BasePriceArticleGridControlBand)d).PopulateColumns(e);

            //NotifyCollectionChangedEventHandler myDelegate = delegate (object sender, NotifyCollectionChangedEventArgs par) { ((BasePriceArticleGridControlBand)d).PopulateColumns(e); };

            //if (e.OldValue != null && e.OldValue is INotifyCollectionChanged)
            //{
            //    ((INotifyCollectionChanged)e.NewValue).CollectionChanged -= myDelegate;
            //}

            //if (e.NewValue != null && e.NewValue is INotifyCollectionChanged)
            //{
            //    ((INotifyCollectionChanged)e.NewValue).CollectionChanged += myDelegate;
            //}
        }
        private void OnColumnsSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            Columns.Clear();
            foreach (var b in ColumnsSource)
            {
                DataTemplate custColumnTemplate = null;
                if (ColumnTemplateSelector != null)
                {
                    custColumnTemplate = ColumnTemplateSelector.SelectTemplate(b, null);
                }
                if (ColumnTemplate != null)
                {
                    custColumnTemplate = ColumnTemplate;
                }
                if (custColumnTemplate != null)
                {
                    ContentControl cc = custColumnTemplate.LoadContent() as ContentControl;
                    if (cc == null)
                        continue;
                    GridColumn column = cc.Content as GridColumn;
                    cc.Content = null;
                    if (column == null)
                        continue;
                    column.DataContext = b;
                    Columns.Add(column);
                }
            }
        }
        private void PopulateColumns(DependencyPropertyChangedEventArgs e)
        {
            Columns.Clear();
            foreach (var b in ColumnsSource)
            {
                DataTemplate custColumnTemplate = null;
                if (ColumnTemplateSelector != null) custColumnTemplate = ColumnTemplateSelector.SelectTemplate(b, null);
                if (ColumnTemplate != null) custColumnTemplate = ColumnTemplate;
                if (custColumnTemplate != null)
                {
                    ContentControl cc = custColumnTemplate.LoadContent() as ContentControl;
                    if (cc == null) continue;
                    GridColumn column = cc.Content as GridColumn;
                    cc.Content = null;
                    if (column == null) continue;
                    column.DataContext = b;
                    DependencyObjectHelper.SetDataContext(column, column.DataContext);
                    Columns.Add(column);
                }
            }
        }
    }
}
