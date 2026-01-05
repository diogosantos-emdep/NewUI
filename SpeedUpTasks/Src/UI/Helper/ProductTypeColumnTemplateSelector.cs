using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class ProductTypeColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate FixedTemplate { get; set; }
        public DataTemplate HiddenTemplate { get; set; }
        public DataTemplate LastUpdateTemplate { get; set; }
        public DataTemplate DeleteTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;

            if (ci != null)
            {
                if (ci.ProductTypeSettings ==  ProductTypeSettingsType.Default)
                {
                    return DefaultTemplate;
                }
                else if(ci.ProductTypeSettings == ProductTypeSettingsType.Fixed)
                {
                    return FixedTemplate;
                }
                else if(ci.ProductTypeSettings == ProductTypeSettingsType.Hidden)
                {
                    return HiddenTemplate;
                }
                else if(ci.ProductTypeSettings == ProductTypeSettingsType.LastUpdate)
                {
                    return LastUpdateTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.Delete)
                {
                    return DeleteTemplate;
                }

            }

            return base.SelectTemplate(item, container);
        }

    }

    //public class ProductTypeColumnTemplateSelector : DataTemplateSelector
    //{
    //    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    //    {
    //        if (container == null)
    //            return null;

    //        //((System.Windows.FrameworkElement)((System.Windows.FrameworkContentElement)container).Parent).Parent

    //        ColumnItem column = (ColumnItem)item;
    //        return (DataTemplate)((Control)container).FindResource(column.ProductTypeSettings + "ColumnTemplate");
    //    }
    //}

    public enum ProductTypeSettingsType
    {
        Default, Fixed, Hidden, LastUpdate, Delete
    }
}
