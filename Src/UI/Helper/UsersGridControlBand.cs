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
    public class UsersGridControlBand : GridControlBand
    {
        public static readonly DependencyProperty ColumnsSourceProperty =
           DependencyProperty.Register("ColumnsSource", typeof(IList), typeof(UsersGridControlBand), new PropertyMetadata(null, OnColumnsSourcePropertyChanged));
        public static readonly DependencyProperty ColumnTemplateProperty =
            DependencyProperty.Register("ColumnTemplate", typeof(DataTemplate), typeof(UsersGridControlBand), new PropertyMetadata(null));

        public UsersColumnTemplateSelector ColumnTemplateSelector
        {
            get { return (UsersColumnTemplateSelector)GetValue(ColumnTemplateSelectorProperty); }
            set { SetValue(ColumnTemplateSelectorProperty, value); }
        }



        // Using a DependencyProperty as the backing store for ColumnTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnTemplateSelectorProperty =
            DependencyProperty.Register("ColumnTemplateSelector", typeof(UsersColumnTemplateSelector), typeof(UsersGridControlBand), new PropertyMetadata(null));

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
            ((UsersGridControlBand)d).OnColumnsSourceChanged(e);
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
                    if (b  is Emdep.Geos.UI.Helper.ColumnItem)
                    {
                        Emdep.Geos.UI.Helper.ColumnItem ColumnItem = (Emdep.Geos.UI.Helper.ColumnItem)b;
                        if (ColumnItem!=null)
                        {
                            if (ColumnItem.ColumnFieldName == "PlantName" ||  ColumnItem.ColumnFieldName == "Region" || ColumnItem.ColumnFieldName == "CustomerName" ||
                                ColumnItem.ColumnFieldName == "SiteName" || ColumnItem.ColumnFieldName == "CountryName" || ColumnItem.ColumnFieldName == "ZoneName" ||
                                 ColumnItem.ColumnFieldName == "FirstName" || ColumnItem.ColumnFieldName == "LastName" || ColumnItem.ColumnFieldName == "Telephone" ||
                                  ColumnItem.ColumnFieldName == "Email" || ColumnItem.ColumnFieldName == "Office" || ColumnItem.ColumnFieldName == "Plant"
                                )
                            {
                                column.HorizontalHeaderContentAlignment = System.Windows.HorizontalAlignment.Left;
                            }
                        }
                    }
                   
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
