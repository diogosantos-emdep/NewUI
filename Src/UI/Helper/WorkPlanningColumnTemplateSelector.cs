using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class WorkPlanningColumnTemplateSelector : DataTemplateSelector
    {
        //Default, Hidden, OfferCode, Plant, User, TimeTypePlannedOrLogged, Customer, Array, Total
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate HiddenTemplate { get; set; }
        public DataTemplate OfferCodeTemplate { get; set; }
        //public DataTemplate PlantTemplate { get; set; }
        public DataTemplate UserTemplate { get; set; }
        public DataTemplate TimeTypePlannedOrLoggedTemplate { get; set; }
        public DataTemplate CustomerTemplate { get; set; }
        public DataTemplate ArrayTemplate { get; set; }
        public DataTemplate TotalTemplate { get; set; }
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;

            if (ci != null)
            {
                if (ci.WorkPlanningSettingsType == WorkPlanningSettingsType.Default)
                {
                    return DefaultTemplate;
                }
                else if(ci.WorkPlanningSettingsType == WorkPlanningSettingsType.Hidden)
                {
                    return HiddenTemplate;
                }
                else if(ci.WorkPlanningSettingsType == WorkPlanningSettingsType.OfferCode)
                {
                    return OfferCodeTemplate;
                }
                else if(ci.WorkPlanningSettingsType == WorkPlanningSettingsType.User)
                {
                    return UserTemplate;
                }
                else if(ci.WorkPlanningSettingsType == WorkPlanningSettingsType.TimeTypePlannedOrLogged)
                {
                    return TimeTypePlannedOrLoggedTemplate;
                }
                else if (ci.WorkPlanningSettingsType == WorkPlanningSettingsType.Customer)
                {
                    return CustomerTemplate;
                }
                else if (ci.WorkPlanningSettingsType == WorkPlanningSettingsType.Array)
                {
                    return ArrayTemplate;
                }
                else if (ci.WorkPlanningSettingsType == WorkPlanningSettingsType.Total)
                {
                    return TotalTemplate;
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

    //public enum ProductTypeSettingsType
    //{
    //    Default, Fixed, Hidden, LastUpdate, Delete, Reference, Status
    //}
    public enum WorkPlanningSettingsType
    {
        //Combo, Image, Chart, Text, ArrowImage, Idpermission, Name, ALLPriceLists, IdPriceList, ImageWithText, Amount, InvoiceAmount, Status, CloseDate, SalesOwner, Email, Reference, SellPriceValue, Discount,
        //Description, PercentText, RFQDate, QuoteSentDate, OthersFields, Source, BusinessUnit, OEM, Fixed, Project, IsChecked,  RFQ, PO, SleepDays, Category1, Category2, OfferOwner, OfferedTo, MaxDiscount, MaxCost,

        Default, Hidden, OfferCode, Customer, User, TimeTypePlannedOrLogged, Array, Total, IsChecked
    }
}
