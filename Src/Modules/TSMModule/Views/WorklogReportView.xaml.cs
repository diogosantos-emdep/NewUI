using DevExpress.Data;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.TSM.Views
{
    /// <summary>
    /// Interaction logic for WorklogReportView.xaml
    /// </summary>
    public partial class WorklogReportView : UserControl
    {
        public WorklogReportView()
        {
            InitializeComponent();
        }

        private void GridControl_CustomSummary(object sender, CustomSummaryEventArgs e)
        {
            DevExpress.Xpf.Grid.GridCustomSummaryEventArgs eve = (DevExpress.Xpf.Grid.GridCustomSummaryEventArgs)e;
            try
            {
                if (e.FieldValue != null)
                {
                    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                    {
                        TimeSpan timeSpanValue = TimeSpan.Zero;
                        TimeSpan TotaltimeSpanValue = TimeSpan.Zero;
                        Emdep.Geos.Data.Common.TSM.TSMWorkLogReport CurrentRow = (Emdep.Geos.Data.Common.TSM.TSMWorkLogReport)e.Row;
                        string[] parts = CurrentRow.Hours.Split(' ');

                        foreach (string part in parts)
                        {
                            if (part.Contains("H"))
                            {
                                int hours = int.Parse(part.Replace("H", ""));
                                timeSpanValue = timeSpanValue.Add(TimeSpan.FromHours(hours));
                            }
                            else if (part.Contains("M"))
                            {
                                int minutes = int.Parse(part.Replace("M", ""));
                                timeSpanValue = timeSpanValue.Add(TimeSpan.FromMinutes(minutes));
                            }
                        }
                        if (e.TotalValue != null)
                        {
                            string TotalValue = e.TotalValue.ToString();
                            string[] total = e.TotalValue.ToString().Split(' ');
                            foreach (string totalpart in total)
                            {
                                if (totalpart.Contains("H"))
                                {
                                    int hours = int.Parse(totalpart.Replace("H", ""));
                                    TotaltimeSpanValue = TotaltimeSpanValue.Add(TimeSpan.FromHours(hours));
                                }
                                else if (totalpart.Contains("M"))
                                {
                                    int minutes = int.Parse(totalpart.Replace("M", ""));
                                    TotaltimeSpanValue = TotaltimeSpanValue.Add(TimeSpan.FromMinutes(minutes));
                                }
                            }
                            TotaltimeSpanValue = TotaltimeSpanValue.Add(timeSpanValue);
                            if (TotaltimeSpanValue.Days > 0)
                                e.TotalValue = (TotaltimeSpanValue.Hours + (TotaltimeSpanValue.Days * 24)) + "H" + " " + TotaltimeSpanValue.Minutes + "M";
                            else
                                e.TotalValue = TotaltimeSpanValue.Hours + "H" + " " + TotaltimeSpanValue.Minutes + "M";
                        }
                        else
                        {
                            if (timeSpanValue.Days > 0)
                                e.TotalValue = (timeSpanValue.Hours + (timeSpanValue.Days * 24)) + "H" + " " + timeSpanValue.Minutes + "M";
                            else
                                e.TotalValue = timeSpanValue.Hours + "H" + " " + timeSpanValue.Minutes + "M";
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
