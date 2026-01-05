using Emdep.Geos.Modules.ERM.CommonClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.ERM.Views
{
    /// <summary>
    /// Interaction logic for PDAShowPOListView.xaml
    /// </summary>
    public partial class PDAShowPOListView : Window
    {
        public static double amountt, tipt;
        public PDAShowPOListView()
        {
            InitializeComponent();
        }

        private void GridControl_LayoutUpdated(object sender, EventArgs e)
        {
            amountt = 0;
        }

        private void CustomSummaryCommandAction(object obj, EventArgs eve)
        {

            DevExpress.Xpf.Grid.GridCustomSummaryEventArgs e = (DevExpress.Xpf.Grid.GridCustomSummaryEventArgs)eve;

            try
            {
                if (e.FieldValue != null)
                {
                    DevExpress.Xpf.Grid.GridSummaryItem t = (DevExpress.Xpf.Grid.GridSummaryItem)e.Item;
                    Emdep.Geos.Data.Common.ERM.PlantDeliveryAnalysis CurrentRow = (Emdep.Geos.Data.Common.ERM.PlantDeliveryAnalysis)e.Row;

                    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
                        amountt = 0;
                    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                    {

                        //  CultureInfo c = CultureInfo.CurrentCulture;

                        if (t.FieldName == "Amounts")
                        {
                            //if (CurrentRow.ExistAsAttendee != 1)//[GEOS2-4438][rdixit][15.06.2023]
                            //{
                            //if (e.TotalValue !=null)
                            //{
                            //    e.TotalValue = null;
                            //}

                            e.TotalValue = Convert.ToDouble(e.TotalValue) + Convert.ToDouble(e.FieldValue);
                            //if (e.TotalValue is string)
                            //{
                            //    string TotalValue = e.TotalValue.ToString();
                            //    TotalValue = TotalValue.Remove(0, 1);
                            //    amountt = Convert.ToDouble(amountt) + CurrentRow.Amounts;
                            //    e.TotalValue = t;
                            //    if (e.TotalValue != null)
                            //    {
                            //        // e.TotalValue = amountt + CurrentRow.Amount ;
                            //        e.TotalValue = CurrentRow.CurrencySymbol + " " + Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture);
                            //    }
                            //}
                            //else
                            //{
                            //    amountt = Convert.ToDouble(amountt) + CurrentRow.Amounts;
                            //    if (amountt != null)
                            //        e.TotalValue = CurrentRow.CurrencySymbol + " " + Math.Round(amountt, 2).ToString("n", CultureInfo.CurrentCulture); 
                            //}
                            //CurrentRow.AmountsWithCurrency = string.Empty;
                            //    CurrentRow.AmountsWithCurrency = CurrentRow.Amounts.ToString();
                            //  }
                        }

                    }

                    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                    {
                        if (t.FieldName == "Amounts" )
                        {
                            string CurrencySymbolSummary = string.Empty;

                            Emdep.Geos.Data.Common.ERM.PlantDeliveryAnalysis CRow = (Emdep.Geos.Data.Common.ERM.PlantDeliveryAnalysis)e.Row;
                            if (CRow != null && CRow.Amount != "")
                            {
                                CurrencySymbolSummary = Convert.ToString(CRow.Amount).Substring(0, 1);

                            }
                            if (string.IsNullOrEmpty(CurrencySymbolSummary))
                            {
                                e.TotalValue = ERMCommon.Instance.CurrencySymbolFromSetting + " "  + Math.Round(Convert.ToDouble(e.TotalValue), 2).ToString("n", CultureInfo.CurrentCulture);

                            }
                            else
                            {
                                e.TotalValue = CurrencySymbolSummary + Math.Round(Convert.ToDouble(e.TotalValue), 2).ToString("n", CultureInfo.CurrentCulture);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
