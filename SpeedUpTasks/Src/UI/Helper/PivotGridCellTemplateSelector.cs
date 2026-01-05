using DevExpress.Xpf.PivotGrid.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class PivotGridCellTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            CellsAreaItem cell = (CellsAreaItem)item;
            // Applies the Default template to the Row Grand Total cells.
            if (cell.RowValue == null && cell.Field.ToString() == "Quantity")
                return OddTemplate;

            if (cell.RowValue == null && cell.Field.ToString() == "LastUpdate")
                return ExchangeRateTemplate;

            if (cell.ColumnValueDisplayText.ToString() == "OfferConfidentialLevel")
                return ProgressBarTemplate;

            if (cell.RowValue == null && cell.Field.ToString() == "LastUpdate" || cell.RowValue == null && cell.Field.ToString() == "ExchangeRate")
                return ExchangeDateRateTemplate;

            if (cell.RowValue == null && cell.Field.ToString() == "CurrencyFrom")
                return ImageWithText;
           
            if (cell.ColumnValueDisplayText.ToString() == "TemplateName")
                return ColumnHeaderVerticalAlignmentTemplate;
            else
                return ColumnHeaderVerticalAlignmentTemplate;

           
            


        }
        public DataTemplate OddTemplate { get; set; }

        public DataTemplate ExchangeRateTemplate { get; set; }
   
        public DataTemplate ColumnHeaderVerticalAlignmentTemplate { get; set; }

        public DataTemplate ProgressBarTemplate { get; set; }//HighlightedCellTemplate

        public DataTemplate ImageWithText { get; set; }

        public DataTemplate ExchangeDateRateTemplate { get; set; }
    }
}
