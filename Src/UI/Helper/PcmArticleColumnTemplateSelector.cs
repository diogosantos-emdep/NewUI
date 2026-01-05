using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    //[rdixit][15.07.2024][rdixit]
    public class PcmArticleColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HiddenTemplate { get; set; }
        public DataTemplate ImageTemplate { get; set; }
        public DataTemplate LastUpdateTemplate { get; set; }
        public DataTemplate DeleteTemplate { get; set; }       
        public DataTemplate AbbreviationTamplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate CodeTemplate { get; set; }
        public DataTemplate StatusTemplate { get; set; }
        public DataTemplate DescriptionTemplate { get; set; }
        public DataTemplate ArticleCategoryTemplate { get; set; }
        public DataTemplate WeightTemplate { get; set; }
        public DataTemplate ECOSVisibilityTemplate { get; set; }
        public DataTemplate PQtyTemplate { get; set; }
        public DataTemplate CostPriceTemplate { get; set; }
        public DataTemplate BasePriceTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;

            if (ci != null)
            {
                if (ci.ArticleTypeSetting == ArticleTypeSettingsType.Default)
                {
                    return DefaultTemplate;
                }
                if (ci.ArticleTypeSetting == ArticleTypeSettingsType.ArticleCategory)
                {
                    return ArticleCategoryTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.Weight)
                {
                    return WeightTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.Hidden)
                {
                    return HiddenTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.Image)
                {
                    return ImageTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.LastUpdate)
                {
                    return LastUpdateTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.Reference)
                {
                    return CodeTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.Status)
                {
                    return StatusTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.Delete)
                {
                    return DeleteTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.Abbreviation)
                {
                    return AbbreviationTamplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.Description)
                {
                    return DescriptionTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.ECOSVisibility)
                {
                    return ECOSVisibilityTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.PQty)
                {
                    return PQtyTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.CostPrice)
                {
                    return CostPriceTemplate;
                }
                else if (ci.ArticleTypeSetting == ArticleTypeSettingsType.BasePrice)
                {
                    return BasePriceTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
        public enum ArticleTypeSettingsType
        {
            Default, Fixed, Hidden, Image, LastUpdate, Delete, Reference, Status, Abbreviation, Description, ArticleCategory,Weight, ECOSVisibility, PQty, CostPrice,BasePrice, IsValueChanged
        }
    }
}
