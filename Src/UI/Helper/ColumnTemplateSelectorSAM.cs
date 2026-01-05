using System.Windows.Controls;
using System.Windows;
using System.Collections;
using DevExpress.Data;
using DevExpress.Xpf.Grid;
using System;
using System.Windows.Data;

namespace Emdep.Geos.UI.Helper
{
    public class ColumnTemplateSelectorSAM : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnSAM column = (ColumnSAM)item;
            return (DataTemplate)((Control)container).FindResource(column.Settings + "ColumnTemplate");
        }
    }

    //[pramod.misal][GEOS2-5888][20.08.2024]
    public enum SettingsTypeSAM
    {
        Default, Combo, Text,Array,  Amount, InvoiceAmount, Status, CloseDate, SalesOwner,Email,PODate, Delay, PlannedStartDate, PlannedEndDate, PlannedDuration, ProducedModules, RealStartDate, RealEndDate,RealDuration,
        Description, PercentText,  DeliveryDate, DeliveryWeek, OthersFields, Source, BusinessUnit, OEM, Hidden, Fixed,Project,IsChecked, OfferCode, PO,Category1, Category2, OfferOwner,OfferedTo,Type, Modules, Group,Plant,Empty,Remark,Clock
    }
}
