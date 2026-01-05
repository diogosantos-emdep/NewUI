using DevExpress.Data;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.Warehouse.ViewModels;
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

namespace Emdep.Geos.Modules.Warehouse.Views
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

        //[rdixit][15.06.2023][GEOS2-4271]
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
                        Emdep.Geos.Data.Common.WarehouseWorklogReport CurrentRow = (Emdep.Geos.Data.Common.WarehouseWorklogReport)e.Row;
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
                            if(TotaltimeSpanValue.Days>0)
                                e.TotalValue =  (TotaltimeSpanValue.Hours+ (TotaltimeSpanValue.Days*24)) + "H" + " " + TotaltimeSpanValue.Minutes + "M";
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
        private void Changed_Event(object sender, CustomDrawCrosshairEventArgs e)
        {
            if (e != null)
            {
                //ChartControl chartcontrol = (ChartControl)sender;
                //DevExpress.Xpf.Charts.XYDiagram2D Diagram = (XYDiagram2D)chartcontrol.Diagram;
                //Diagram.ActualAxisY.Label = new AxisLabel();
                //Diagram.ActualAxisY.Label.Formatter = new YAxisLabelFormatterNew();

                #region DevExpress.Xpf.Charts.XYDiagram2D
                /// Diagram.AxisY.Label = new AxisLabel();
                // Diagram.AxisY.Label.TextPattern = "{V:HH:mm:ss}";
                // Diagram.AxisY.DateTimeOptions = new DateTimeOptions();
                // Diagram.AxisY.DateTimeOptions.Format = DateTimeFormat.Custom;
                // Diagram.AxisY.DateTimeOptions.FormatString = "HH:mm";


                //Diagram.ActualAxisX.Title = new AxisTitle { Content = "Worklog Week" };
                //Diagram.ActualAxisY.Title = new AxisTitle {  Content = "Total Time (h m)" };
                //Diagram.ActualAxisY.GridLinesMinorVisible = true;
                //Diagram.ActualAxisX.LabelVisibilityMode = AxisLabelVisibilityMode.AutoGeneratedAndCustom;
                //Diagram.ActualAxisX.LabelPosition = AxisLabelPosition.Outside;
                //Diagram.ActualAxisX.LabelAlignment = AxisLabelAlignment.Auto;
                //Diagram.ActualAxisX.DateTimeScaleOptions = new ManualDateTimeScaleOptions()
                //{
                //    MeasureUnit = DateTimeMeasureUnit.Week,
                //    GridAlignment = DateTimeGridAlignment.Week,
                //    AutoGrid = false
                //};
                //Diagram.ActualAxisY.DateTimeScaleOptions = new ContinuousDateTimeScaleOptions()
                //{
                //    AutoGrid = false,
                //    GridAlignment = DateTimeGridAlignment.Hour
                //};
                #endregion
                if (e.CrosshairElementGroups.Count > 0)
                {
                    for (int i = 0; i < e.CrosshairElementGroups[0].CrosshairElements.Count(); i++)
                    {
                        try
                        {
                            if (e.CrosshairElementGroups[0].CrosshairElements[i].AxisLabelElement != null)
                            {
                                string PickingString = e.CrosshairElementGroups[0].CrosshairElements[i].AxisLabelElement.Text;
                                TimeSpan span = TimeSpan.Parse(PickingString); 
                                string time = string.Format("{0}H {1}M", (int)span.TotalHours, span.Minutes);
                                string LabelElement = e.CrosshairElementGroups[0].CrosshairElements[i].LabelElement.Text.Replace(PickingString, time);
                                e.CrosshairElementGroups[0].CrosshairElements[i].LabelElement.Text = LabelElement;
                            }

                            #region CrosshairElementGroups
                            //if (e.CrosshairElementGroups[0].CrosshairElements[i].AxisLabelElement != null)
                            //{
                            //    string PickingString = e.CrosshairElementGroups[0].CrosshairElements[i].AxisLabelElement.Text;
                            //    string[] PickingParts = PickingString.Split(':');
                            //    TimeSpan span = TimeSpan.FromMinutes(Convert.ToDouble(PickingString));
                            //    string time = string.Format("{0}H {1}M", (int)span.TotalHours, span.Minutes);
                            //    string LabelElement = e.CrosshairElementGroups[0].CrosshairElements[i].LabelElement.Text.Replace(PickingString, time);
                            //    e.CrosshairElementGroups[0].CrosshairElements[i].LabelElement.Text = LabelElement;
                            //    //string label = span.ToString(@"hh\:mm\:ss");
                            //    //string labelspan= span.ToString("hh:mm");
                            //    //string strTime = $"{((int)span.TotalHours).ToString("D3")}H:{span.Minutes.ToString("D2")}M";
                            //    // e.CrosshairElementGroups[0].CrosshairElements[0].LabelElement.Text = "Total : " + label;
                            //}
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }
    }
}
