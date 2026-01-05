using DevExpress.Data;
using DevExpress.Xpf.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.Warehouse.Views
{
    /// <summary>
    /// Interaction logic for PendingWorkOrdersPreparationView.xaml
    /// </summary>
    public partial class WorkOrdersPreparationView : UserControl
    {
        public static TimeSpan TotalTimePickingOT = new TimeSpan(0, 0, 0);
        public static TimeSpan TotalTimeShipmentCreated = new TimeSpan(0, 0, 0);
        public static TimeSpan TotalTimeShipmentDelivery= new TimeSpan(0, 0, 0);
        public WorkOrdersPreparationView()
        {
            InitializeComponent();
        }
        //[GEOS2-4268][rdixit][22.06.2023]
        private void GridControl_CustomSummary(object sender, CustomSummaryEventArgs e)
        {
            DevExpress.Xpf.Grid.GridCustomSummaryEventArgs eve = (DevExpress.Xpf.Grid.GridCustomSummaryEventArgs)e;
            DevExpress.Xpf.Grid.GridSummaryItem summery = (DevExpress.Xpf.Grid.GridSummaryItem)eve.Item;
            #region TotalTimeForPickingOT
            try
            {
                if (summery.FieldName == "TotalTimeForPickingOT")
                {
                    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                    {
                        Emdep.Geos.Data.Common.Ots CurrentRow = (Emdep.Geos.Data.Common.Ots)e.Row;
                        if (e.TotalValue != null)
                        {
                            if (CurrentRow.TotalTimeForPickingOT != null)
                                TotalTimePickingOT = TotalTimePickingOT.Add((TimeSpan)CurrentRow.TotalTimeForPickingOT);

                            if (TotalTimePickingOT.Days > 0)
                                e.TotalValue = (TotalTimePickingOT.Hours + (TotalTimePickingOT.Days * 24)) + "H" + " " + TotalTimePickingOT.Minutes + "M";
                            else
                                e.TotalValue = TotalTimePickingOT.Hours + "H" + " " + TotalTimePickingOT.Minutes + "M";
                        }
                        else
                        {
                            if (CurrentRow.TotalTimeForPickingOT != null)
                            {
                                TotalTimePickingOT = (TimeSpan)CurrentRow.TotalTimeForPickingOT;

                                if (TotalTimePickingOT.Days > 0)
                                    e.TotalValue = (TotalTimePickingOT.Hours + (TotalTimePickingOT.Days * 24)) + "H" + " " + TotalTimePickingOT.Minutes + "M";
                                else
                                    e.TotalValue = TotalTimePickingOT.Hours + "H" + " " + TotalTimePickingOT.Minutes + "M";
                            }
                            else
                            {
                                e.TotalValue = "0H 0M";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            #endregion
            #region TotalTimeForShipmentCreatedIn
            try
            {
                if (summery.FieldName == "TotalTimeForShipmentCreatedIn")
                {
                    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                    {                    
                        Emdep.Geos.Data.Common.Ots CurrentRow = (Emdep.Geos.Data.Common.Ots)e.Row;
                        if (e.TotalValue != null)
                        {
                            
                            if (CurrentRow.TotalTimeForShipmentCreatedIn != null)
                                TotalTimeShipmentCreated = TotalTimeShipmentCreated.Add((TimeSpan)CurrentRow.TotalTimeForShipmentCreatedIn);
                            if (TotalTimeShipmentCreated.Days > 0)
                                e.TotalValue = (TotalTimeShipmentCreated.Hours + (TotalTimeShipmentCreated.Days * 24)) + "H" + " " + TotalTimeShipmentCreated.Minutes + "M";
                            else
                                e.TotalValue = TotalTimeShipmentCreated.Hours + "H" + " " + TotalTimeShipmentCreated.Minutes + "M";
                        }
                        else
                        {
                            if (CurrentRow.TotalTimeForShipmentCreatedIn != null)
                            {
                                TotalTimeShipmentCreated = (TimeSpan)CurrentRow.TotalTimeForShipmentCreatedIn;
                                if (TotalTimeShipmentCreated.Days > 0)
                                    e.TotalValue = (TotalTimeShipmentCreated.Hours + (TotalTimeShipmentCreated.Days * 24)) + "H" + " " + TotalTimeShipmentCreated.Minutes + "M";
                                else
                                    e.TotalValue = TotalTimeShipmentCreated.Hours + "H" + " " + TotalTimeShipmentCreated.Minutes + "M";
                            }
                            else
                            {
                                e.TotalValue ="0H 0M";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            #endregion
            #region TotalTimeForShipmentDelivery
            try
            {
                if (summery.FieldName == "TotalTimeForShipmentDelivery")
                {
                    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
                    {
                        Emdep.Geos.Data.Common.Ots CurrentRow = (Emdep.Geos.Data.Common.Ots)e.Row;
                        if (e.TotalValue != null)
                        {

                            if (CurrentRow.TotalTimeForShipmentDelivery != null)                            
                                TotalTimeShipmentDelivery = TotalTimeShipmentDelivery.Add((TimeSpan)CurrentRow.TotalTimeForShipmentDelivery);
                            
                            if (TotalTimeShipmentDelivery.Days > 0)
                                e.TotalValue = (TotalTimeShipmentDelivery.Hours + (TotalTimeShipmentDelivery.Days * 24)) + "H" + " " + TotalTimeShipmentDelivery.Minutes + "M";
                            else
                                e.TotalValue = TotalTimeShipmentDelivery.Hours + "H" + " " + TotalTimeShipmentDelivery.Minutes + "M";
                        }
                        else
                        {
                            if (CurrentRow.TotalTimeForShipmentDelivery != null)
                            {
                                TotalTimeShipmentDelivery = (TimeSpan)CurrentRow.TotalTimeForShipmentDelivery;

                                if (TotalTimeShipmentDelivery.Days > 0)
                                    e.TotalValue = (TotalTimeShipmentDelivery.Hours + (TotalTimeShipmentDelivery.Days * 24)) + "H" + " " + TotalTimeShipmentDelivery.Minutes + "M";
                                else
                                    e.TotalValue = TotalTimeShipmentDelivery.Hours + "H" + " " + TotalTimeShipmentDelivery.Minutes + "M";
                            }
                            else
                            {
                                e.TotalValue = "0H 0M";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        private void GridControl_LayoutUpdated(object sender, EventArgs e)
        {
            TotalTimePickingOT = new TimeSpan(0, 0, 0);
            TotalTimeShipmentCreated = new TimeSpan(0, 0, 0);
            TotalTimeShipmentDelivery = new TimeSpan(0, 0, 0);
        }
        //[rdixit][GEOS2-4269][28.06.2023]
        private void Changed_Event(object sender, CustomDrawCrosshairEventArgs e)
        {
            if (e != null)
            {           
                if (e.CrosshairElementGroups.Count > 0)
                {
                    #region Picking
                    string PickingString = e.CrosshairElementGroups[0].CrosshairElements[0].AxisLabelElement.Text;
                    string[] PickingParts = PickingString.Split(':');

                    if (PickingParts.Length == 3)
                    {
                        int hours, minutes, seconds;
                        if (int.TryParse(PickingParts[0], out hours) && int.TryParse(PickingParts[1], out minutes) && int.TryParse(PickingParts[2], out seconds))
                        {
                            e.CrosshairElementGroups[0].CrosshairElements[0].LabelElement.Text = "Total Picking Time : " + hours + "H" + " " + minutes + "M";
                        }

                    }
                    else if (PickingParts.Length == 4)
                    {
                        int days, hours, minutes, seconds;
                        if (int.TryParse(PickingParts[0], out days) && int.TryParse(PickingParts[1], out hours) && int.TryParse(PickingParts[2], out minutes) && int.TryParse(PickingParts[3], out seconds))
                        {
                            double Totalhours = (days * 24) + hours;
                            e.CrosshairElementGroups[0].CrosshairElements[0].LabelElement.Text = "Total Picking Time : " + Totalhours + "H" + " " + minutes + "M";
                        }
                    }
                    #endregion

                    #region Packing
                    string Packingtring = e.CrosshairElementGroups[0].CrosshairElements[1].AxisLabelElement.Text;
                    string[] PackingParts = Packingtring.Split(':');
                    if (PackingParts.Length == 3)
                    {
                        int hours, minutes, seconds;
                        if (int.TryParse(PackingParts[0], out hours) && int.TryParse(PackingParts[1], out minutes) && int.TryParse(PackingParts[2], out seconds))
                        {
                            e.CrosshairElementGroups[0].CrosshairElements[1].LabelElement.Text = "Total Packing Time : " + hours + "H" + " " + minutes + "M";
                        }
                    }
                    else if (PackingParts.Length == 4)
                    {
                        int days, hours, minutes, seconds;
                        if (int.TryParse(PackingParts[0], out days) && int.TryParse(PackingParts[1], out hours) && int.TryParse(PackingParts[2], out minutes) && int.TryParse(PackingParts[3], out seconds))
                        {
                            double Totalhours = (days * 24) + hours;
                            e.CrosshairElementGroups[0].CrosshairElements[1].LabelElement.Text = "Total Packing Time : " + Totalhours + "H" + " " + minutes + "M";
                        }
                    }
                    #endregion

                    #region Shipping
                    string Shippingtring = e.CrosshairElementGroups[0].CrosshairElements[2].AxisLabelElement.Text;
                    string[] ShippingtimeParts = Shippingtring.Split(':');
                    if (ShippingtimeParts.Length == 3)
                    {
                        int hours, minutes, seconds;
                        if (int.TryParse(ShippingtimeParts[0], out hours) && int.TryParse(ShippingtimeParts[1], out minutes) && int.TryParse(ShippingtimeParts[2], out seconds))
                        {
                            e.CrosshairElementGroups[0].CrosshairElements[2].LabelElement.Text = "Total Shipping Time : " + hours + "H" + " " + minutes + "M";
                        }
                    }
                    else if (ShippingtimeParts.Length == 4)
                    {
                        int days, hours, minutes, seconds;
                        if (int.TryParse(ShippingtimeParts[0], out days) && int.TryParse(ShippingtimeParts[1], out hours) && int.TryParse(ShippingtimeParts[2], out minutes) && int.TryParse(ShippingtimeParts[3], out seconds))
                        {
                            double Totalhours = (days * 24) + hours;
                            e.CrosshairElementGroups[0].CrosshairElements[2].LabelElement.Text = "Total Shipping Time : " + Totalhours + "H" + " " + minutes + "M";
                        }
                    }
                    #endregion
                }
            }
        }
    }
}
