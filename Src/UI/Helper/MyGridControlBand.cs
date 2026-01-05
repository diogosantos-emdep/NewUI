using System.Windows;
using System.Collections;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;

using System.Collections.Specialized;
using System;

namespace Emdep.Geos.UI.Helper
{
    public class MyGridControlBand : GridControlBand
    {
        public static readonly DependencyProperty ColumnsSourceProperty =
            DependencyProperty.Register("ColumnsSource", typeof(IList), typeof(MyGridControlBand), new PropertyMetadata(null, OnColumnsSourcePropertyChanged));
        public static readonly DependencyProperty ColumnTemplateProperty =
            DependencyProperty.Register("ColumnTemplate", typeof(DataTemplate), typeof(MyGridControlBand), new PropertyMetadata(null));


        public MyColumnTemplateSelector ColumnTemplateSelector
        {
            get { return (MyColumnTemplateSelector)GetValue(ColumnTemplateSelectorProperty); }
            set { SetValue(ColumnTemplateSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnTemplateSelectorProperty =
            DependencyProperty.Register("ColumnTemplateSelector", typeof(MyColumnTemplateSelector), typeof(MyGridControlBand), new PropertyMetadata(null));





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
            ((MyGridControlBand)d).OnColumnsSourceChanged(e);
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
        private void PopulateColumns()
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
