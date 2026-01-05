using System.Windows.Controls;
using System.Windows;
using System.Collections;
using DevExpress.Data;
using DevExpress.Xpf.Grid;
using System;
using System.Windows.Data;

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
        Default, Combo, Image, Chart, Text, ArrowImage, Array, ImageWithText, Amount, InvoiceAmount, Status, CloseDate, SalesOwner,Email,
        Description, PercentText, RFQDate, QuoteSentDate, OthersFields, Source, BusinessUnit, OEM, Hidden, Fixed,Project,IsChecked, OfferCode, RFQ,PO,SleepDays,Category1, Category2, OfferOwner,OfferedTo
    }
}
