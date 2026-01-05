using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Emdep.Geos.UI.Helper
{
    public class TimeTrackingColumnTemplateSelector: DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate DeliveryWeek { get; set; }
        public DataTemplate DeliveryDate { get; set; }

        //[GEOS2-4217][Pallavi Jadhav][24-02-2023]
        public DataTemplate PlannedDeliveryDate { get; set; }

        //[GEOS2-4093][Rupali Sarode][27-12-2022]
        public DataTemplate DatesTemplate { get; set; }

        //public DataTemplate POType { get; set; }
        //public DataTemplate Customer { get; set; }
        //public DataTemplate Project { get; set; }

        public DataTemplate OTCode { get; set; }
        public DataTemplate OTNumber { get; set; }
        public DataTemplate Plant { get; set; }
        public DataTemplate Item { get; set; }
        public DataTemplate Reference { get; set; }
        public DataTemplate ReferenceTemplate { get; set; }
        public DataTemplate Type { get; set; }
        public DataTemplate QTY { get; set; }
        public DataTemplate SalePrice { get; set; }
        public DataTemplate ItemInformation { get; set; }
        public DataTemplate ItemStatus { get; set; }
        public DataTemplate DynamicProduction { get; set; }
        public DataTemplate DynamicProductionWithoutCOMCADCAM { get; set; }
        public DataTemplate CurrentWorkStation { get; set; }
        public DataTemplate TotalRework { get; set; }

        public DataTemplate RemainingTime { get; set; }
        public DataTemplate RemainingTime1 { get; set; }
        public DataTemplate RemainingTime2 { get; set; }
        public DataTemplate RemainingTime3 { get; set; }
        public DataTemplate RemainingTime4 { get; set; }
        public DataTemplate RemainingTime5 { get; set; }
        public DataTemplate RemainingTime6 { get; set; }
        public DataTemplate RemainingTime7 { get; set; }
        public DataTemplate RemainingTime8 { get; set; }
        public DataTemplate RemainingTime9 { get; set; }
        public DataTemplate RemainingTime10 { get; set; }
        public DataTemplate RemainingTime11 { get; set; }
        public DataTemplate RemainingTime12 { get; set; }
        public DataTemplate RemainingTime21 { get; set; }
        public DataTemplate RemainingTime26 { get; set; }
        public DataTemplate RemainingTime27 { get; set; }
        public DataTemplate RemainingTime28 { get; set; }
        public DataTemplate RemainingTime33 { get; set; }
        public DataTemplate RemainingTime35 { get; set; }
        public DataTemplate RemainingTime37 { get; set; }
        public DataTemplate ExpectedTime { get; set; }  //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        public DataTemplate SamplesTemplate { get; set; } //[GEOS2-4145][Pallavi Jadhav][02-03-2023]
        public DataTemplate SamplesDateTemplate { get; set; }
        public DataTemplate DrawingType { get; set; }      //[Gulab Lakade][geso2-4173][02-03 -2023]
        public DataTemplate TrayName { get; set; }      //[Gulab Lakade][geso2-4173][02-03 -2023]

        public DataTemplate FirstDeliveryDate { get; set; }      //[Rupali Sarode][geso2-4173][10-03-2023]
        public DataTemplate Production { get; set; }//[gulab lakade][11 03 2024][GEOS2-5466]
        public DataTemplate Rework { get; set; }//[gulab lakade][11 03 2024][GEOS2-5466]
        public DataTemplate IdDrawing { get; set; }//[Rupali Sarode][04-04-2024][GEOS2-5577]

        public DataTemplate workbook_drawing { get; set; }//[Aishwarya Ingale][09-08-2024][GEOS2-6034]
        public DataTemplate PrOWS { get; set; }//[rajashri] GEOS2-6054
        public DataTemplate ReOWS { get; set; }//[rajashri] GEOS2-6054
        public DataTemplate DynamicProductionExpected { get; set; }  //[GEOS2-6081] [pallavi jadhav] [19 09 2024]
        public DataTemplate DynamicProductionTotalExpected { get; set; }  //[GEOS2-6081] [pallavi jadhav] [19 09 2024]
        public DataTemplate Days { get; set; }  //[GEOS2-5320] [pallavi jadhav] [15 10 2024]
        public DataTemplate PlannedDeliveryDate1 { get; set; }
        public DataTemplate PlannedDeliveryDate2 { get; set; }
        public DataTemplate PlannedDeliveryDate3 { get; set; }
        public DataTemplate PlannedDeliveryDate4 { get; set; }
        public DataTemplate PlannedDeliveryDate5 { get; set; }
        public DataTemplate PlannedDeliveryDate6 { get; set; }
        public DataTemplate PlannedDeliveryDate7 { get; set; }
        public DataTemplate PlannedDeliveryDate8 { get; set; }
        public DataTemplate PlannedDeliveryDate9 { get; set; }
        public DataTemplate PlannedDeliveryDate10 { get; set; }
        public DataTemplate PlannedDeliveryDate11 { get; set; }
        public DataTemplate PlannedDeliveryDate12 { get; set; }
        public DataTemplate PlannedDeliveryDate21 { get; set; }
        public DataTemplate PlannedDeliveryDate26 { get; set; }
        public DataTemplate PlannedDeliveryDate27 { get; set; }
        public DataTemplate PlannedDeliveryDate28 { get; set; }
        public DataTemplate PlannedDeliveryDate33 { get; set; }
        public DataTemplate PlannedDeliveryDate35 { get; set; }
        public DataTemplate PlannedDeliveryDate37 { get; set; }

        #region [rani dhamankar][20-02-2025][GEOS2-6685]
        public DataTemplate DynamicPOWSWithoutCOMCADCAM { get; set; }
        public DataTemplate DynamicROWSWithoutCOMCADCAM { get; set; }
        public DataTemplate DynamicReworkWithoutCOMCADCAM { get; set; }
        public DataTemplate DynamicRealWithoutCOMCADCAM { get; set; }
        public DataTemplate RemainingTime1WithoutCOMCADCA { get; set; }
        #endregion
        #region [dhawal bhalerao][17-04-2025][GEOS2-7094]
        public DataTemplate DesignSystem { get; set; }
        public DataTemplate Designer { get; set; }
        public DataTemplate StartRev { get; set; }
        public DataTemplate LastRev { get; set; }
        public DataTemplate DesignTime { get; set; }
        public DataTemplate Download { get; set; }
        public DataTemplate Transferred { get; set; }
        public DataTemplate AddIn { get; set; }
        public DataTemplate PostServer { get; set; }
        public DataTemplate EDS { get; set; }

        #endregion
        #region //[GEOS2-8309] [Rajashri Telvekar] [06 11 2025]
        public DataTemplate ExpectedTime1 { get; set; }  
        public DataTemplate Production1 { get; set; }
        public DataTemplate PrOWS1 { get; set; }
        public DataTemplate Rework1 { get; set; }
        public DataTemplate ReOWS1 { get; set; }
        public DataTemplate DynamicProduction1 { get; set; }
        #endregion
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;
            if (ci != null)
            {
                if ( ci.ColumnFieldName == "POType" || ci.ColumnFieldName == "Customer" || ci.ColumnFieldName == "Project" || ci.ColumnFieldName == "Offer" )
                {
                    return DefaultTemplate;
                }else
                if (ci.ColumnFieldName == "DeliveryWeek")
                {
                    return DeliveryWeek;
                }
                else
                if (ci.ColumnFieldName == "DeliveryDate")
                {
                    return DeliveryDate;
                }
                //[GEOS2-4217][Pallavi Jadhav][24-02-2023]
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate")
                {
                    return PlannedDeliveryDate;
                }
                //[GEOS2-4093][Rupali Sarode][27-12-2022]
                else
                if (ci.ColumnFieldName == "QuoteSendDate" || ci.ColumnFieldName == "GoAheadDate" || ci.ColumnFieldName == "PODate" || ci.ColumnFieldName == "AvailbleForDesignDate")
                {
                    return DatesTemplate;
                }
                else
                if (ci.ColumnFieldName == "SamplesTemplate") //[GEOS2-4145][Pallavi Jadhav][02-03-2023]
                {
                    return SamplesTemplate;
                }
                else
                if (ci.ColumnFieldName == "SamplesDateTemplate") //[GEOS2-4145][Pallavi Jadhav][02-03-2023]
                {
                    return SamplesDateTemplate;
                }
                else
                if (ci.ColumnFieldName == "OTCode") 
                {
                    return OTCode;
                }
                else
                if (ci.ColumnFieldName == "OTNumber")
                {
                    return OTNumber;
                }
                else
                if (ci.ColumnFieldName == "OriginPlant"|| ci.ColumnFieldName == "ProductionPlant")
                {
                    return Plant;
                }
                else
                if (ci.ColumnFieldName == "Reference")
                {
                    return Reference;
                }
                else
                if (ci.ColumnFieldName == "ReferenceTemplate")
                {
                    return ReferenceTemplate;
                }
                else
                if (ci.ColumnFieldName == "Type")
                {
                    return Type;
                }
                else
                if (ci.ColumnFieldName == "QTY")
                {
                    return QTY;
                }
                
                else
                if (ci.ColumnFieldName == "UnitPrice"|| ci.ColumnFieldName == "TotalPrice")//ci.ColumnFieldName == "Total"
                {
                    return SalePrice;
                }else if(ci.ColumnFieldName == "ItemNumber" || ci.ColumnFieldName == "TRework")
                {
                    return TotalRework;
                }
                else
                if ( ci.ColumnFieldName == "SerialNumber"|| ci.ColumnFieldName == "CurrentWorkStation")
                {
                    return ItemInformation;
                }
                else
                if (ci.ColumnFieldName == "DesignSystem")
                {
                    return DesignSystem;
                }
                else
                if (ci.ColumnFieldName == "Designer")
                {
                    return Designer;
                }
                else
                if (ci.ColumnFieldName == "StartRev")
                {
                    return StartRev;
                }
                else
                if (ci.ColumnFieldName == "LastRev")
                {
                    return LastRev;
                }
                else
                if (ci.ColumnFieldName == "ItemStatus")
                {
                    return ItemStatus;
                }
                else
                if (ci.ColumnFieldName == "Remaining_1")
                {
                    return RemainingTime1;
                }
                else
                if (ci.ColumnFieldName == "Remaining_2")
                {
                    return RemainingTime2;
                }
                else
                if (ci.ColumnFieldName == "Remaining_3")
                {
                    return RemainingTime3;
                }
                else
                if (ci.ColumnFieldName == "Remaining_4")
                {
                    return RemainingTime4;
                }
                else
                if (ci.ColumnFieldName == "Remaining_5")
                {
                    return RemainingTime5;
                }
                else
                if (ci.ColumnFieldName == "Remaining_6")
                {
                    return RemainingTime6;
                }
                else
                if (ci.ColumnFieldName == "Remaining_7")
                {
                    return RemainingTime7;
                }
                else
                if (ci.ColumnFieldName == "Remaining_8")
                {
                    return RemainingTime8;
                }
                else
                if (ci.ColumnFieldName == "Remaining_9")
                {
                    return RemainingTime9;
                }
                else
                if (ci.ColumnFieldName == "Remaining_10")
                {
                    return RemainingTime10;
                }
                else
                if (ci.ColumnFieldName == "Remaining_11")
                {
                    return RemainingTime11;
                }
                else
                if (ci.ColumnFieldName == "Remaining_12")
                {
                    return RemainingTime12;
                }
                else
                if (ci.ColumnFieldName == "Remaining_21")
                {
                    return RemainingTime21;
                }
                else
                if (ci.ColumnFieldName == "Remaining_26")
                {
                    return RemainingTime26;
                }
                else
                if (ci.ColumnFieldName == "Remaining_27")
                {
                    return RemainingTime27;
                }
                else
                if (ci.ColumnFieldName == "Remaining_28")
                {
                    return RemainingTime28;
                }
                else
                if (ci.ColumnFieldName == "Remaining_33")
                {
                    return RemainingTime33;
                }
                else
                if (ci.ColumnFieldName == "Remaining_35")
                {
                    return RemainingTime35;
                }
                else
                if (ci.ColumnFieldName == "Remaining_37")
                {
                    return RemainingTime37;
                }
                #region [Gulab Lakade][geso2-4173][02-03 -2023]

                else
                   if (ci.ColumnFieldName == "DrawingType")
                {
                    return DrawingType;
                }
                else
                   if (ci.ColumnFieldName == "TrayName")
                {
                    return TrayName;
                }
                #endregion

                #region [Rupali Sarode][geso2-4173][10-03-2023]
                else
                   if (ci.ColumnFieldName == "FirstDeliveryDate")
                {
                    return FirstDeliveryDate;
                }
                #endregion
                #region [gulab lakade][11 03 2024][GEOS2-5466]
                else
                if (ci.ColumnFieldName.Contains("Production_"))
                {
                    if (Convert.ToString(ci.TimetrackingSetting) == "Production")
                    {
                        return Production;
                    }
                    else if (Convert.ToString(ci.TimetrackingSetting) == "Production1")
                    {
                        return Production1;
                    }
                    else if (Convert.ToString(ci.TimetrackingSetting) == "DynamicProductionWithoutCOMCADCAM" || ci.ColumnFieldName.Contains("Production"))
                    {
                        return DynamicProductionWithoutCOMCADCAM;
                    }
                }
                else if(ci.ColumnFieldName.Contains("Production"))
                 {
                    if (Convert.ToString(ci.TimetrackingSetting) == "DynamicProductionWithoutCOMCADCAM" )
                    {
                        return DynamicProductionWithoutCOMCADCAM;
                    }
                }
                #region [rajashri][26-08-2024][GEOS2-6054]
                #region [rani dhamankar][20-02-2025][GEOS2-6685]
                else
                if (ci.ColumnFieldName.Contains("POWS_") || ci.ColumnFieldName.Contains("ProductionOWS"))
                {
                    if (Convert.ToString(ci.TimetrackingSetting) == "PrOWS")
                    {
                        return PrOWS;
                    }
                    else if (Convert.ToString(ci.TimetrackingSetting) == "PrOWS1")
                    {
                        return PrOWS1;
                    }
                    else if (Convert.ToString(ci.TimetrackingSetting) == "DynamicPOWSWithoutCOMCADCAM")
                    {
                        return DynamicPOWSWithoutCOMCADCAM;
                    }
                }
                else if (ci.ColumnFieldName.Contains("POWS") )
                {
                    if (Convert.ToString(ci.TimetrackingSetting) == "DynamicPOWSWithoutCOMCADCAM")
                    {
                        return DynamicPOWSWithoutCOMCADCAM;
                    }
                }
                else
                if (ci.ColumnFieldName.Contains("ROWS_") || ci.ColumnFieldName.Contains("ProductionOWS"))
                {
                    if (Convert.ToString(ci.TimetrackingSetting) == "ReOWS")
                    {
                        return ReOWS;
                    }
                    else if (Convert.ToString(ci.TimetrackingSetting) == "ReOWS1")
                    {
                        return ReOWS1;
                    }
                    else if (Convert.ToString(ci.TimetrackingSetting) == "DynamicROWSWithoutCOMCADCAM")
                    {
                        return DynamicROWSWithoutCOMCADCAM;
                    }
                }
                else if (ci.ColumnFieldName.Contains("ROWS"))
                {
                    if (Convert.ToString(ci.TimetrackingSetting) == "DynamicROWSWithoutCOMCADCAM")
                    {
                        return DynamicROWSWithoutCOMCADCAM;
                    }
                }
                else
                if (ci.ColumnFieldName.Contains("Rework_") || ci.ColumnFieldName.Contains("ProductionOWS"))
                {
                    if (Convert.ToString(ci.TimetrackingSetting) == "Rework")
                    {
                        return Rework;
                    }
                    else if (Convert.ToString(ci.TimetrackingSetting) == "Rework1")
                    {
                        return Rework1;
                    }
                    else if (Convert.ToString(ci.TimetrackingSetting) == "DynamicReworkWithoutCOMCADCAM")
                    {
                        return DynamicReworkWithoutCOMCADCAM;
                    }
                }
                else if (ci.ColumnFieldName.Contains("Rework"))
                {
                    if (Convert.ToString(ci.TimetrackingSetting) == "DynamicReworkWithoutCOMCADCAM")
                    {
                        return DynamicReworkWithoutCOMCADCAM;
                    }
                }
                #endregion
                //else
                //if (ci.ColumnFieldName.Contains("POWS_") || ci.ColumnFieldName.Contains("ProductionOWS"))
                //{
                //    return PrOWS;
                //}
                //else
                //if (ci.ColumnFieldName.Contains("ROWS_") || ci.ColumnFieldName.Contains("ProductionOWS"))
                //{
                //    return ReOWS;
                //}
                #endregion
                //else
                //if (ci.ColumnFieldName.Contains("Rework_") || ci.ColumnFieldName.Contains("Rework"))
                //{
                //    return Rework;
                //}

                #region [Rupali Sarode]GEOS2-5577][04-04-2024]
                else
                if (ci.ColumnFieldName == "IdDrawing")
                {
                    return IdDrawing;
                }
                #endregion

                #region [Aishwarya Ingale][09-08-2024][GEOS2-6034]
                else
                if (ci.ColumnFieldName == "workbook_drawing")
                {
                    return workbook_drawing;
                }
                #endregion

                #endregion
                #region [pallavi jadhav][15 10 2024][GEOS2-5320]
                else
                if (ci.ColumnFieldName.Contains("Days_") || ci.ColumnFieldName.Contains("Days"))
                {
                    return Days;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_1")
                {
                    return PlannedDeliveryDate1;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_2")
                {
                    return PlannedDeliveryDate2;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_3")
                {
                    return PlannedDeliveryDate3;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_4")
                {
                    return PlannedDeliveryDate4;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_5")
                {
                    return PlannedDeliveryDate5;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_6")
                {
                    return PlannedDeliveryDate6;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_7")
                {
                    return PlannedDeliveryDate7;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_8")
                {
                    return PlannedDeliveryDate8;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_9")
                {
                    return PlannedDeliveryDate9;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_10")
                {
                    return PlannedDeliveryDate10;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_11")
                {
                    return PlannedDeliveryDate11;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_12")
                {
                    return PlannedDeliveryDate12;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_21")
                {
                    return PlannedDeliveryDate21;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_26")
                {
                    return PlannedDeliveryDate26;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_27")
                {
                    return PlannedDeliveryDate27;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_28")
                {
                    return PlannedDeliveryDate28;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_33")
                {
                    return PlannedDeliveryDate33;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_35")
                {
                    return PlannedDeliveryDate35;
                }
                else
                if (ci.ColumnFieldName == "PlannedDeliveryDate_37")
                {
                    return PlannedDeliveryDate37;
                }
                #endregion                
                else
                {
                    if(ci.ColumnFieldName.Contains("Remaining"))
                    {
                        return RemainingTime;
                    }
                    #region [GEOS2-4252] [gulab lakade][06 03 2023]
                    else if (ci.ColumnFieldName.Contains("Expected")|| ci.ColumnFieldName.Contains("Expected_"))
                    {
                       if(Convert.ToString(ci.TimetrackingSetting)== "ExpectedTime")
                        {
                            return ExpectedTime;
                        }
                        else if (Convert.ToString(ci.TimetrackingSetting) == "ExpectedTime1")
                        {
                            return ExpectedTime1;
                        }
                        else if (Convert.ToString(ci.TimetrackingSetting) == "DynamicProductionTotalExpected")
                        {
                            return DynamicProductionTotalExpected;
                        }
                        else if (Convert.ToString(ci.TimetrackingSetting) == "DynamicProductionExpected")
                        {
                            return DynamicProductionExpected;
                        }

                    }
                    #endregion//[GEOS2-4150] [Gulab Lakade] [30 01 2023]
                    //else if ( ci.ColumnFieldName.Contains("Real_")  || ci.ColumnFieldName.Contains("Real") )
                    //{
                    //    return DynamicProduction;
                    //}
                    //else
                    //{
                    //    return DefaultTemplate;
                    //}
                    #region [rani dhamankar][20-02-2025][GEOS2-6685]
                    else
                   if (ci.ColumnFieldName.Contains("Real_") || ci.ColumnFieldName.Contains("Real"))
                    {
                        if (Convert.ToString(ci.TimetrackingSetting) == "DynamicProduction")
                        {
                            return DynamicProduction;
                        }
                        else if (Convert.ToString(ci.TimetrackingSetting) == "DynamicProduction1")
                        {
                            return DynamicProduction1;
                        }
                        else if (Convert.ToString(ci.TimetrackingSetting) == "DynamicRealWithoutCOMCADCAM")
                        {
                            return DynamicRealWithoutCOMCADCAM;
                        }
                        else
                        {
                            return DefaultTemplate;
                        }
                    }
                    else if (ci.ColumnFieldName.Contains("Real"))
                    {
                        if (Convert.ToString(ci.TimetrackingSetting) == "DynamicRealWithoutCOMCADCAM")
                        {
                            return DynamicRealWithoutCOMCADCAM;
                        }
                    }
                    #endregion

                    #region [dhawal bhalerao][18-04-2025][GEOS2-7094]
                    else if (ci.ColumnFieldName.Contains("DesignTime_") || ci.ColumnFieldName.Contains("DesignTime"))
                    {
                        if (Convert.ToString(ci.TimetrackingSetting) == "DesignTime")
                        {
                            return DesignTime;
                        }
                    }
                    else if (ci.ColumnFieldName.Contains("Download") || ci.ColumnFieldName.Contains("Download_"))
                    {
                        if (Convert.ToString(ci.TimetrackingSetting) == "Download")
                        {
                            return Download;
                        }
                    }
                    else if (ci.ColumnFieldName.Contains("Transferred") || ci.ColumnFieldName.Contains("Transferred_"))
                    {
                        if (Convert.ToString(ci.TimetrackingSetting) == "Transferred")
                        {
                            return Transferred;
                        }
                    }
                    else if (ci.ColumnFieldName.Contains("AddIn") || ci.ColumnFieldName.Contains("AddIn_"))
                    {
                        if (Convert.ToString(ci.TimetrackingSetting) == "AddIn")
                        {
                            return AddIn;
                        }
                    }
                    else if (ci.ColumnFieldName.Contains("PostServer") || ci.ColumnFieldName.Contains("PostServer_"))
                    {
                        if (Convert.ToString(ci.TimetrackingSetting) == "PostServer")
                        {
                            return PostServer;
                        }
                    }

                    else if (ci.ColumnFieldName.Contains("EDS") || ci.ColumnFieldName.Contains("EDS_"))
                    {
                        if (Convert.ToString(ci.TimetrackingSetting) == "EDS")
                        {
                            return PostServer;
                        }
                    }
                    #endregion

                }


                //if (ci.ColumnFieldName.Contains("ExchangeRateDate-"))
                //{
                //    return template3;
                //}
                //if (ci.ColumnFieldName.Contains("ExchangeRate-"))
                //{
                //    return template4;
                //}
                //if (ci.ColumnFieldName.Contains("ConvertedAmount"))
                //{
                //    return template5;
                //}
                //else
                //{
                //    return template2;
                //}


            }

            return base.SelectTemplate(item, container);
        }

        //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        public enum TimetrackingSettingType
        {
            DefaultTemplate, DeliveryWeek,  DeliveryDate, PlannedDeliveryDate,  OTCode, OTNumber, Plant, Item, Reference, ReferenceTemplate, Type, QTY, SalePrice, ItemInformation, RemainingTime, RemainingTime1, RemainingTime2
               , RemainingTime3, RemainingTime4, RemainingTime5, RemainingTime6, RemainingTime7, RemainingTime8, RemainingTime9, RemainingTime10, RemainingTime11, RemainingTime12, RemainingTime21, RemainingTime26, RemainingTime27,
            RemainingTime28, RemainingTime33, RemainingTime35, RemainingTime37, DynamicProduction,TotalRework, ItemStatus, DatesTemplate, ExpectedTime, SamplesTemplate, SamplesDateTemplate, DrawingType, TrayName, FirstDeliveryDate,
            IdDrawing, Production, Rework, DynamicProductionWithoutCOMCADCAM, workbook_drawing, PrOWS, ReOWS, DynamicProductionExpected, DynamicProductionTotalExpected,Days, PlannedDeliveryDate1, PlannedDeliveryDate2
               , PlannedDeliveryDate3, PlannedDeliveryDate4, PlannedDeliveryDate5, PlannedDeliveryDate6, PlannedDeliveryDate7, PlannedDeliveryDate8, PlannedDeliveryDate9, PlannedDeliveryDate10, PlannedDeliveryDate11, PlannedDeliveryDate12, PlannedDeliveryDate21, PlannedDeliveryDate26, PlannedDeliveryDate27,
            PlannedDeliveryDate28, PlannedDeliveryDate33, PlannedDeliveryDate35, PlannedDeliveryDate37, DynamicPOWSWithoutCOMCADCAM, DynamicROWSWithoutCOMCADCAM, DynamicReworkWithoutCOMCADCAM, DynamicRealWithoutCOMCADCAM, DesignSystem, Designer, StartRev, LastRev, DesignTime, Download, Transferred, AddIn, PostServer, EDS ,// [dhawal bhalerao] [17/04/2025][GEOS2-7094] // [rani dhamankar] [20/02/2025][GEOS2-6685]//[Gulab Lakade][geso2-4173][02-03-2023]
                ExpectedTime1, Production1, PrOWS1, Rework1, ReOWS1, DynamicProduction1
        }
    }
}
