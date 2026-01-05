using DevExpress.Xpf.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Crm.UI.Helper
{
    public class ColumnTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Column column = (Column)item;
            if (column.Settings == SettingsType.ArrowImage)
            {

            }
            return (DataTemplate)((Control)container).FindResource(column.Settings + "ColumnTemplate");
        }

    }

    public enum SettingsType { Default, Combo, Image, Chart, Text, ArrowImage, Array,ImageWithText }

    //public class Summary
    //{
    //    public SummaryItemType Type { get; set; }
    //    public string FieldName { get; set; }


    //}

    public class Column
    {
        public string FieldName { get; set; }


        public SettingsType Settings { get; set; }
        public bool AllowCellMerge { get; set; }
        public double Width { get; set; }
        public bool AllowEditing { get; set; }

        public double BestFitWidth;
        public bool AllowBestFit;
        public bool FixedWidth { get; set; }
        public int ImageIndex { get; set; }
        public string HeaderText { get; set; }
        public ImageSource ImageSourceFlag { get; set; }

        //public ImageSource Image
        //{
        //    get { return image; }
        //    set { image = value; }
        //}
        public Column()
        {
            Width = 50;
            BestFitWidth = 50;
            AllowBestFit = true;
            AllowEditing = false;
            FixedWidth = false;
            ImageIndex = 0;
           
        }

    }
    public class ComboColumn : Column
    {
        public IList Source { get; set; }
    }
    public class ImageTextColumn : Column
    {
        private ImageSource image;

        public ImageSource Image
        {
            get { return image; }
            set { image = value; }
        }
    }




    public class ColumnBindingHelper
    {
        public static readonly DependencyProperty BindingPathProperty = DependencyProperty.RegisterAttached("BindingPath", typeof(string), typeof(ColumnBindingHelper), new UIPropertyMetadata(null, new PropertyChangedCallback(OnBindingPathChanged)));

        public static string GetBindingPath(DependencyObject target)
        {
            return (string)target.GetValue(BindingPathProperty);
        }
        public static void SetBindingPath(DependencyObject target, string value)
        {
            target.SetValue(BindingPathProperty, value);
        }
        private static void OnBindingPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            OnBindingPathChanged(o, (string)e.OldValue, (string)e.NewValue);
        }
        private static void OnBindingPathChanged(DependencyObject o, string oldValue, string newValue)
        {
            var column = o as GridColumn;
            column.DisplayMemberBinding = new Binding(newValue);
        }

    }
}
