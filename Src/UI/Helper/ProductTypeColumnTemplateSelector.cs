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
        public DataTemplate ImageTemplate { get; set; }//[Sudhir.Jangra][GEOS2-2922][23/03/2023]
        public DataTemplate LastUpdateTemplate { get; set; }
        public DataTemplate DeleteTemplate { get; set; }
        public DataTemplate CodeTemplate { get; set; }
        public DataTemplate StatusTemplate { get; set; }
        public DataTemplate AbbreviationTamplate { get; set; }
        public DataTemplate DescriptionTemplate { get; set; }//[Sudhir.Jangra][GEOS-4441]
        public DataTemplate CustTemplate { get; set; } //[rdixit][31.12.2024][GEOS2-6574][GEOS2-6575]
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;

            if (ci != null)
            {
                if (ci.ProductTypeSettings == ProductTypeSettingsType.Default)
                {
                    return DefaultTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.Fixed)
                {
                    return FixedTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.Hidden)
                {
                    return HiddenTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.Image)
                {
                    return ImageTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.LastUpdate)
                {
                    return LastUpdateTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.Reference)
                {
                    return CodeTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.Status)
                {
                    return StatusTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.Delete)
                {
                    return DeleteTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.Abbreviation)
                {
                    return AbbreviationTamplate;
                }
                else if (ci.ProductTypeSettings==ProductTypeSettingsType.Description)
                {
                    return DescriptionTemplate;
                }
                else if (ci.ProductTypeSettings == ProductTypeSettingsType.Customer) //[rdixit][31.12.2024][GEOS2-6574][GEOS2-6575]
                {
                    return CustTemplate;
                }               
            }

            return base.SelectTemplate(item, container);
        }

    }

    public enum ProductTypeSettingsType
    {
        Default, Fixed, Hidden, Image, LastUpdate, Delete, Reference, Status, Abbreviation,Description,Customer
    }
}
