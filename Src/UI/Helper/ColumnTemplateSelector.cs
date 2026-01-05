using System.Windows.Controls;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class ColumnTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Column column = (Column)item;
            return (DataTemplate)((Control)container).FindResource(column.Settings + "ColumnTemplate");
        }
    }
    public enum SettingsType
    {
        Default, Combo, Image, Chart, Text, ArrowImage, Idpermission, Name, ALLPriceLists, IdPriceList, Array, ImageWithText, Amount, InvoiceAmount, Status, CloseDate, SalesOwner,Email,Reference, SellPriceValue, Discount,
        Description, PercentText, RFQDate, QuoteSentDate, OthersFields, Source, BusinessUnit, OEM, Hidden, Fixed,Project,IsChecked, OfferCode, RFQ,PO,SleepDays,Category1, Category2, OfferOwner,OfferedTo, MaxDiscount, MaxCost,
        Plant, User, TimeTypePlannedOrLogged, Customer, CountryWithImage, CalendarWeek, ArrayOfferOption, SiteName, OfferDeliveryDateYear, DeliveryWeek, OTCode, Modules, ProducedModules, InProduction, RowLabels,
        ArrayOfferOptionProduction, GrandTotal, DeliveryDate, CalendarWeekPDAWorkstation, CustomerName, QTY, PDAOTCode, IdDrawing,HexColor

    }
}
