using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.ERM
{
    public class TimeTracking : ModelBase, IDisposable
    {

        #region Field
        private string deliveryWeek;
        private DateTime? deliveryDate; 
        private string pOType;
        private string customer;
        private string project;
        private string offer;
        private string oTCode;
        private string originalPlant;
        private string productionPlant;
        private string item;
        private string reference;
        private string template;
        private string type;
        private int? qTY;
        private double? unit;
        private string serialNumber;
        private string itemStatus;
        private string currentWorkStation;
        private UInt64? tRework;
        private double? totalSalePrice;
        private string real;
        private float? observedTime;
        private float? activity;
        private float? supplementValue;
        private float? tempExpectedTime;
        private bool tempcolor;

        private Int64 idOt;
        private Int64 idCP;
        private Int64 idRevisionItem;
        private Int32 idCptypes;
        private Int32 idTemplate;
        private Int64 idCounterpart;
        private Int64 idSOD;
        List<TimeTrackingCurrentStage> timeTrackingStage;
        private Int32 idProductionPlant;
        private Int64 idCPType;
        private Int64? numItem;
        private string htmlColor;
        private string deliveryDateHtmlColor;
        //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        private bool expectedHtmlColorFlag;
        private bool prodcutionHtmlColorFlag;
        private UInt64 idShippingAddress;
        private DateTime? plannedDeliveryDate;
        private string plannedDeliveryDateHtmlColor;
        private string samplesColor;
        #region [GEOS2-4093][Rupali Sarode][26-12-2022]
        private DateTime quoteSendDate;
        private DateTime? goAheadDate;
        private DateTime? poDate;
        private DateTime? availbleForDesignDate;
        #endregion

        #region [GEOS2-4145][Pallavi Jadhav][02-03-2023]
        private string samples;
        private DateTime? samplesDate;
        #endregion

        private string drawingType; //[Gulab Lakade][geso2-4173][02-03-2023]
        private string trayName;     //[Gulab Lakade][geso2-4173][02-03-2023]
        private string trayColor;     //[Gulab Lakade][geso2-4173][02-03-2023]
        private DateTime? firstDeliveryDate;     //[Rupali Sarode][geso2-4173][10-03-2023]

        private Int32 idSite;
        private string stageName;
        private Int32 rework;
        private string stageCode;
        private string customerReference;
        private Int32 itemNumber;
        List<Counterpartstracking> counterpartstrackingList;
        private string validationWeek;//[GEOS2-5002] [gulab lakade][31 10 2023]

        private Int32 productionIdSite; //[][gulab lakade][30 11 2023]
        List<ERM_CounterPartFailurData> counterPartFailurData;//[GEOS2-5127][gulab lakade][20 12 2023]
        private DateTime? validationWeekDate;     //[GEOS2-5324][gulab lakade][09 02 2023]
        private string employeeName;//[GEOS2-5324][gulab lakade][09 02 2023]
        private bool isBatch;    //[GEOS2-5420][Rupali Sarode][15-03-2024]
        private Int64 idOTItem;  //[GEOS2-5420][Rupali Sarode][15-03-2024]
        private Int64 idDrawing; //[GEOS2-5420][Rupali Sarode][15-03-2024]
        #region Aishwarya[Geos2-6034]
        private string workbookdrawing; //[aishwarya ingale][30/7/2024]
        private string designer;//[aishwarya ingale][30/7/2024]
        private DateTime? startDate;
        private DateTime? endDate;
        private string action;
        private string responsible;
        private string priority;
        private Int32 status;
        private string additionalinformation;
        private Int64 idWorkbookOfCpProducts;
        //rajashri GEOS2-5988
        List<TimeTracking> mismatchRecord;
        private Int32 cp_Detection;
        private Int32 cpQty;
        private Int32 detectiondrawing;
        private Int32 draw_quantity;
        private Int32 detection;//[GEOS2-5988][gulab lakade][30 08 2024]
        private UInt64 idStandardOperationsDictionary;
        #endregion

        private string failCode;

        #region [rani dhamankar][GEOS2-6685][21-01-2025]
        private bool powsHtmlColorFlag;
        private bool rowsHtmlColorFlag;
        private bool reworkHtmlColorFlag;
        private bool remainingHtmlColorFlag1;
        private bool remainingHtmlColorFlag2;
        private bool remainingHtmlColorFlag3;
        private bool dynamicProductionHtmlColorFlag;
        #endregion
        #region [GEOS2-6683][rani dhamankar][12 02 2025] 
        private string fQUWeek;
        private DateTime fQUOkDate;
        #endregion
        #region [rani dhamankar][31-03-2025][GEOS2-7097]
        private string designSystem;
        private Int64 startRevision;
        private Int64 lastRevision;
        #endregion

        private string oTNumber; //[pallavi jadhav][GEOS2-7060][25-03-2025]
        List<TimeTrackingCurrentStage> timeTrackingAddingPostServer;//[GEOS2-7880][gulab lakade][15 04 2025]

        private DateTime sampleDate;  // [pallavi.jadhav][20 05 2025]

        #region  // [pallavi.jadhav][20 05 2025]
        private Int32? idDetection_Ways;
        private Int32? idDetection_Detections;
        private Int32? idDetection_Options;
        private string no_Of_Ways;
        private string no_Of_Detections;
        private string no_Of_Options;
        private string way_Name;
        private string detection_Name;
        private string option_Name;
        #endregion
        #endregion

        #region [GEOS2-8005][rani dhamankar][13-06-2025]
        private double finalEquivalentWeight;
        #endregion

        #region Property

        public bool Tempcolor
        {
            get { return tempcolor; }
            set
            {
                tempcolor = value;
                OnPropertyChanged("Tempcolor");
            }
        }
        [DataMember]
        public float? TempExpectedTime
        {
            get
            {
                return tempExpectedTime;
            }

            set
            {
                tempExpectedTime = value;
                OnPropertyChanged("TempExpectedTime");
            }
        }
        [DataMember]
        public float? ObservedTime
        {
            get
            {
                return observedTime;
            }

            set
            {
                observedTime = value;
                OnPropertyChanged("ObservedTime");
            }
        }
        [DataMember]
        public float? Activity
        {
            get
            {
                return activity;
            }

            set
            {
                activity = value;
                OnPropertyChanged("Activity");
            }
        }
        [DataMember]
        public float? SupplementValue
        {
            get
            {
                return supplementValue;
            }

            set
            {
                supplementValue = value;
                OnPropertyChanged("SupplementValue");
            }
        }
        [DataMember]
        public string DeliveryWeek
        {
            get
            {
                return deliveryWeek;
            }

            set
            {
                deliveryWeek = value;
                OnPropertyChanged("DeliveryWeek");
            }
        }
        [DataMember]
        public DateTime? DeliveryDate
        {
            get
            {
                return deliveryDate;
            }

            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }
        #region [GEOS2-4093][Rupali Sarode][26-12-2022]
        [DataMember]
        public DateTime QuoteSendDate
        {
            get
            {
                return quoteSendDate;
            }

            set
            {
                quoteSendDate = value;
                OnPropertyChanged("QuoteSendDate");
            }
        }

        [DataMember]
        public DateTime? GoAheadDate
        {
            get
            {
                return goAheadDate;
            }

            set
            {
                goAheadDate = value;
                OnPropertyChanged("GoAheadDate");
            }
        }

        [DataMember]
        public DateTime? PODate
        {
            get
            {
                return poDate;
            }

            set
            {
                poDate = value;
                OnPropertyChanged("PODate");
            }
        }

        [DataMember]
        public DateTime? AvailbleForDesignDate
        {
            get
            {
                return availbleForDesignDate;
            }

            set
            {
                availbleForDesignDate = value;
                OnPropertyChanged("AvailbleForDesignDate");
            }
        }

        #endregion [GEOS2-4093][Rupali Sarode][26-12-2022]


        [DataMember]
        public string POType
        {
            get
            {
                return pOType;
            }

            set
            {
                pOType = value;
                OnPropertyChanged("POType");
            }
        }
        [DataMember]
        public string Customer
        {
            get
            {
                return customer;
            }

            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }
        [DataMember]
        public string Project
        {
            get
            {
                return project;
            }

            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }
        [DataMember]
        public string Offer
        {
            get
            {
                return offer;
            }

            set
            {
                offer = value;
                OnPropertyChanged("Offer");
            }
        }
        [DataMember]
        public string OTCode
        {
            get
            {
                return oTCode;
            }

            set
            {
                oTCode = value;
                OnPropertyChanged("OTCode");
            }
        }
        [DataMember]
        public string OriginalPlant
        {
            get
            {
                return originalPlant;
            }

            set
            {
                originalPlant = value;
                OnPropertyChanged("OriginalPlant");
            }
        }
        [DataMember]
        public string ProductionPlant
        {
            get
            {
                return productionPlant;
            }

            set
            {
                productionPlant = value;
                OnPropertyChanged("ProductionPlant");
            }
        }
        [DataMember]
        public string Item
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
                OnPropertyChanged("Item");
            }
        }
        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }
        [DataMember]
        public string Template
        {
            get
            {
                return template;
            }

            set
            {
                template = value;
                OnPropertyChanged("Template");
            }
        }
        [DataMember]
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        [DataMember]
        public Int32? QTY
        {
            get
            {
                return qTY;
            }

            set
            {
                qTY = value;
                OnPropertyChanged("QTY");
            }
        }
        [DataMember]
        public double? Unit
        {
            get
            {
                return unit;
            }

            set
            {
                unit = value;
                OnPropertyChanged("Unit");
            }
        }
        [DataMember]
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                serialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }
        [DataMember]
        public string ItemStatus
        {
            get
            {
                return itemStatus;
            }

            set
            {
                itemStatus = value;
                OnPropertyChanged("ItemStatus");
            }
        }
        [DataMember]
        public string CurrentWorkStation
        {
            get
            {
                return currentWorkStation;
            }

            set
            {
                currentWorkStation = value;
                OnPropertyChanged("CurrentWorkStation");
            }
        }
        [DataMember]
        public UInt64? TRework
        {
            get
            {
                return tRework;
            }

            set
            {
                tRework = value;
                OnPropertyChanged("TRework");
            }
        }
        
             [DataMember]
        public double? TotalSalePrice
        {
            get
            {
                return totalSalePrice;
            }

            set
            {
                totalSalePrice = value;
                OnPropertyChanged("TotalSalePrice");
            }
        }
        [DataMember]
        public string Real
        {
            get
            {
                return real;
            }

            set
            {
                real = value;
                OnPropertyChanged("Real");
            }
        }

        [DataMember]
        public Int64 IdOt
        {
            get
            {
                return idOt;
            }

            set
            {
                idOt = value;
                OnPropertyChanged("IdOt");
            }
        }
        [DataMember]
        public Int64 IdCP
        {
            get
            {
                return idCP;
            }

            set
            {
                idCP = value;
                OnPropertyChanged("IdCP");
            }
        }
        [DataMember]
        public Int64 IdRevisionItem
        {
            get
            {
                return idRevisionItem;
            }

            set
            {
                idRevisionItem = value;
                OnPropertyChanged("IdRevisionItem");
            }
        }
        [DataMember]
        public Int32 IdCptypes
        {
            get
            {
                return idCptypes;
            }

            set
            {
                idCptypes = value;
                OnPropertyChanged("IdCptypes");
            }
        }
        [DataMember]
        public Int32 IdTemplate
        {
            get
            {
                return idTemplate;
            }

            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }

        [DataMember]
        public Int64 IdCounterpart
        {
            get
            {
                return idCounterpart;
            }

            set
            {
                idCounterpart = value;
                OnPropertyChanged("IdCounterpart");
            }
        }

        [DataMember]
        public Int64 IdSOD
        {
            get
            {
                return idSOD;
            }

            set
            {
                idSOD = value;
                OnPropertyChanged("IdSOD");
            }
        }

        [DataMember]
        public Int64 IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }


     

        [DataMember]
        public Int32 IdProductionPlant
        {
            get
            {
                return idProductionPlant;
            }

            set
            {
                idProductionPlant = value;
                OnPropertyChanged("IdProductionPlant");
            }
        }


        [DataMember]
        public Int64? NumItem
        {
            get
            {
                return numItem;
            }

            set
            {
                numItem = value;
                OnPropertyChanged("NumItem");
            }
        }
        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }

            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }
        [DataMember]
        public string DeliveryDateHtmlColor
        {
            get
            {
                return deliveryDateHtmlColor;
            }

            set
            {
                deliveryDateHtmlColor = value;
                OnPropertyChanged("DeliveryDateHtmlColor");
            }
        }
        [DataMember]
        public List<TimeTrackingCurrentStage> TimeTrackingStage
        {
            get
            {
                return timeTrackingStage;
            }

            set
            {
                timeTrackingStage = value;
                OnPropertyChanged("TimeTrackingStage");
            }
        }
        //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        
        [DataMember]
        public bool ExpectedHtmlColorFlag
        {
            get
            {
                return expectedHtmlColorFlag;
            }

            set
            {
                expectedHtmlColorFlag = value;
                OnPropertyChanged("ExpectedHtmlColorFlag");
            }
        }
        [DataMember]
        public bool ProductionHtmlColorFlag
        {
            get
            {
                return prodcutionHtmlColorFlag;
            }

            set
            {
                prodcutionHtmlColorFlag = value;
                OnPropertyChanged("ProductionHtmlColorFlag");
            }
        }
        [DataMember]
        public UInt64 IdShippingAddress 
        {
            get
            {
                return idShippingAddress;
            }

            set
            {
                idShippingAddress = value;
                OnPropertyChanged("IdShippingAddress");
            }
        }
        [DataMember]
        public DateTime? PlannedDeliveryDate
        {
            get
            {
                return plannedDeliveryDate;
            }

            set
            {
                plannedDeliveryDate = value;
                OnPropertyChanged("PlannedDeliveryDate");
            }
        }

        [DataMember]
        public string PlannedDeliveryDateHtmlColor
        {
            get
            {
                return plannedDeliveryDateHtmlColor;
            }

            set
            {
                plannedDeliveryDateHtmlColor = value;
                OnPropertyChanged("PlannedDeliveryDateHtmlColor");
            }
        }

        #region [GEOS2-4145][Pallavi Jadhav][02-03-2023]
        [DataMember]
        public string Samples
        {
            get
            {
                return samples;
            }

            set
            {
                samples = value;
                OnPropertyChanged("Samples");
            }
        }
        [DataMember]
        public DateTime? SamplesDate
        {
            get
            {
                return samplesDate;
            }

            set
            {
                samplesDate = value;
                OnPropertyChanged("SamplesDate");
            }
        }
        [DataMember]
        public string SamplesColor
        {
            get
            {
                return samplesColor;
            }

            set
            {
                samplesColor = value;
                OnPropertyChanged("SamplesColor");
            }
        }

        #endregion

        #region [Gulab Lakade][geso2-4173][02-03 -2023]
        [DataMember]
        public string DrawingType
        {
            get
            {
                return drawingType;
            }

            set
            {
                drawingType = value;
                OnPropertyChanged("DrawingType");
            }
        }
        [DataMember]
        public string TrayColor
        {
            get
            {
                return trayColor;
            }

            set
            {
                trayColor = value;
                OnPropertyChanged("TrayColor");
            }
        }
        [DataMember]
        public string TrayName
        {
            get 
            {
                return trayName;
            }

            set
            {
                trayName = value;
                OnPropertyChanged("TrayName");
            }
        }

        //[Rupali Sarode][geso2-4173][10-03-2023]
        [DataMember]
        public DateTime? FirstDeliveryDate
        {
            get
            {
                return firstDeliveryDate;
            }

            set
            {
                firstDeliveryDate = value;
                OnPropertyChanged("FirstDeliveryDate");
            }
        }


        #region [Pallavi Jadhav][Geos-4812][09 06 2023]
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }
        [DataMember]
        public string StageName
        {
            get
            {
                return stageName;
            }

            set
            {
                stageName = value;
                OnPropertyChanged("StageName");
            }
        }
        [DataMember]
        public Int32 Rework
        {
            get
            {
                return rework;
            }

            set
            {
                rework = value;
                OnPropertyChanged("Rework");
            }
        }

        [DataMember]
        public string StageCode
        {
            get
            {
                return stageCode;
            }

            set
            {
                stageCode = value;
                OnPropertyChanged("StageCode");
            }
        }
        [DataMember]
        public string CustomerReference
        {
            get
            {
                return customerReference;
            }

            set
            {
                customerReference = value;
                OnPropertyChanged("CustomerReference");
            }
        }
        [DataMember]
        public Int32 ItemNumber
        {
            get
            {
                return itemNumber;
            }

            set
            {
                itemNumber = value;
                OnPropertyChanged("ItemNumber");
            }
        }

        [DataMember]
        public List<Counterpartstracking> CounterpartstrackingList
        {
            get
            {
                return counterpartstrackingList;
            }

            set
            {
                counterpartstrackingList = value;
                OnPropertyChanged("CounterpartstrackingList");
            }
        }
        #endregion
        #endregion
        //[GEOS2-5002] [gulab lakade][31 10 2023]
        [DataMember]
        public string ValidationWeek
        {
            get
            {
                return validationWeek;
            }

            set
            {
                validationWeek = value;
                OnPropertyChanged("ValidationWeek");
            }
        }

        [DataMember]
        public Int32 ProductionIdSite
        {
            get
            {
                return productionIdSite;
            }

            set
            {
                productionIdSite = value;
                OnPropertyChanged("ProductionIdSite");
            }
        }
        //[GEOS2-5127][gulab lakade][20 12 2023]
        [DataMember]
        public List<ERM_CounterPartFailurData> CounterPartFailurData
        {
            get
            {
                return counterPartFailurData;
            }

            set
            {
                counterPartFailurData = value;
                OnPropertyChanged("CounterPartFailurData");
            }
        }
        //[GEOS2-5324][gulab lakade][09 02 2023]
        [DataMember]
        public DateTime? ValidationWeekDate     
        {
            get
            {
                return validationWeekDate;
            }

            set
            {
                validationWeekDate = value;
                OnPropertyChanged("ValidationWeekDate");
            }
        }
        //[GEOS2-5324][gulab lakade][09 02 2023]
        [DataMember]
        public string EmployeeName
        {
            get
            {
                return employeeName;
            }

            set
            {
                employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }

        [DataMember]
        public bool IsBatch
        {
            get
            {
                return isBatch;
            }

            set
            {
                isBatch = value;
                OnPropertyChanged("IsBatch");
            }
        }

        [DataMember]
        public Int64 IdOTItem
        {
            get
            {
                return idOTItem;
            }

            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }

        [DataMember]
        public Int64 IdDrawing
        {
            get
            {
                return idDrawing;
            }

            set
            {
                idDrawing = value;
                OnPropertyChanged("IdDrawing");
            }
        }

        [DataMember]
        public string Workbookdrawing
        {
            get
            {
                return workbookdrawing;
            }

            set
            {
                workbookdrawing = value;
                OnPropertyChanged("Workbookdrawing");
            }
        }

        [DataMember]
        public string Designer
        {
            get
            {
                return designer;
            }

            set
            {
                designer = value;
                OnPropertyChanged("Designer");
            }
        }


    
        
        [DataMember]
        public Int64 IdWorkbookOfCpProducts
        {
            get
            {
                return idWorkbookOfCpProducts;
            }

            set
            {
                idWorkbookOfCpProducts = value;
                OnPropertyChanged("IdWorkbookOfCpProducts");
            }
        }
        [DataMember]
        public List<TimeTracking> MismatchRecord
        {
            get
            {
                return mismatchRecord;
            }

            set
            {
                mismatchRecord = value;
                OnPropertyChanged("MismatchRecord");
            }
        }
        [DataMember]
        public Int32 Cp_Detection
        {
            get
            {
                return cp_Detection;
            }

            set
            {
                cp_Detection = value;
                OnPropertyChanged("Cp_Detection");
            }
        }
        [DataMember]
        public Int32 CpQty
        {
            get
            {
                return cpQty;
            }

            set
            {
                cpQty = value;
                OnPropertyChanged("CpQty");
            }
        }
        [DataMember]
        public Int32 Detectiondrawing
        {
            get
            {
                return detectiondrawing;
            }

            set
            {
                detectiondrawing = value;
                OnPropertyChanged("Detectiondrawing");
            }
        }
        [DataMember]
        public Int32 Draw_quantity
        {
            get
            {
                return draw_quantity;
            }

            set
            {
                draw_quantity = value;
                OnPropertyChanged("Draw_quantity");
            }
        }
        //[GEOS2-5988][gulab lakade][30 08 2024]
        [DataMember]
        public Int32 Detection
        {
            get
            {
                return detection;
            }

            set
            {
                detection = value;
                OnPropertyChanged("Detection");
            }
        }

        [DataMember]
        public UInt64 IdStandardOperationsDictionary
        {
            get
            {
                return idStandardOperationsDictionary;
            }

            set
            {
                idStandardOperationsDictionary = value;
                OnPropertyChanged("IdStandardOperationsDictionary");
            }
        }

        [DataMember]
        public string FailCode
        {
            get
            {
                return failCode;
            }

            set
            {
                failCode = value;
                OnPropertyChanged("FailCode");
            }
        }
        [DataMember]
        public string FQUWeek
        {
            get
            {
                return fQUWeek;
            }

            set
            {
                fQUWeek = value;
                OnPropertyChanged("FQUWeek");
            }
        }
        [DataMember]
        public DateTime FQUOkDate
        {
            get
            {
                return fQUOkDate;
            }

            set
            {
                fQUOkDate = value;
                OnPropertyChanged("FQUOkDate");
            }
        }

        #region [pallavi jadhav][GEOS2-7060][25-03-2025]
        [DataMember]
        public string OTNumber    
        {
            get
            {
                return oTNumber;
            }

            set
            {
                oTNumber = value;
                OnPropertyChanged("OTNumber");
            }
        }
        #endregion


        [DataMember]
        public DateTime SampleDate // [pallavi.jadhav][20 05 2025]
        {
            get
            {
                return sampleDate;
            }

            set
            {
                sampleDate = value;
                OnPropertyChanged("SampleDate");
            }
        }

        #region // [pallavi.jadhav][21 05 2025]
        [DataMember]
        public Int32? IdDetection_Ways
        {
            get
            {
                return idDetection_Ways;
            }

            set
            {
                idDetection_Ways = value;
                OnPropertyChanged("IdDetection_Ways");
            }
        }

        [DataMember]
        public Int32? IdDetection_Detections
        {
            get
            {
                return idDetection_Detections;
            }

            set
            {
                idDetection_Detections = value;
                OnPropertyChanged("IdDetection_Detections");
            }
        }

        [DataMember]
        public Int32? IdDetection_Options
        {
            get
            {
                return idDetection_Options;
            }

            set
            {
                idDetection_Options = value;
                OnPropertyChanged("IdDetection_Options");
            }
        }

        [DataMember]
        public string No_Of_Ways
        {
            get
            {
                return no_Of_Ways;
            }

            set
            {
                no_Of_Ways = value;
                OnPropertyChanged("No_Of_Ways");
            }
        }
        [DataMember]
        public string No_Of_Detections
        {
            get
            {
                return no_Of_Detections;
            }

            set
            {
                no_Of_Detections = value;
                OnPropertyChanged("No_Of_Detections");
            }
        }
        [DataMember]
        public string No_Of_Options
        {
            get
            {
                return no_Of_Options;
            }

            set
            {
                no_Of_Options = value;
                OnPropertyChanged("No_Of_Options");
            }
        }
        [DataMember]
        public string Way_Name
        {
            get
            {
                return way_Name;
            }

            set
            {
                way_Name = value;
                OnPropertyChanged("Way_Name");
            }
        }
        [DataMember]
        public string Detection_Name
        {
            get
            {
                return detection_Name;
            }

            set
            {
                detection_Name = value;
                OnPropertyChanged("Detection_Name");
            }
        }
        [DataMember]
        public string Option_Name
        {
            get
            {
                return option_Name;
            }

            set
            {
                option_Name = value;
                OnPropertyChanged("Option_Name");
            }
        }
        #endregion
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }


        #endregion
        //start [rani dhamankar][GEOS2-6685][21-01-2025]

        [DataMember]
        public bool POWSHtmlColorFlag
        {
            get
            {
                return powsHtmlColorFlag;
            }

            set
            {
                powsHtmlColorFlag = value;
                OnPropertyChanged("POWSHtmlColorFlag");
            }
        }
        [DataMember]
        public bool ROWSHtmlColorFlag
        {
            get
            {
                return rowsHtmlColorFlag;
            }

            set
            {
                rowsHtmlColorFlag = value;
                OnPropertyChanged("ROWSHtmlColorFlag");
            }
        }
        [DataMember]
        public bool ReworkHtmlColorFlag
        {
            get
            {
                return reworkHtmlColorFlag;
            }

            set
            {
                reworkHtmlColorFlag = value;
                OnPropertyChanged("ReworkHtmlColorFlag");
            }
        }
       
       
        [DataMember]
        public bool DynamicProductionHtmlColorFlag
        {
            get
            {
                return dynamicProductionHtmlColorFlag;
            }

            set
            {
                dynamicProductionHtmlColorFlag = value;
                OnPropertyChanged("DynamicProductionHtmlColorFlag");
            }
        }
        //end [rani dhamankar][GEOS2-6685][21-01-2025]

        #region [rani dhamankar][31-03-2025][GEOS2-7097]
        [DataMember]
        public string DesignSystem
        {
            get
            {
                return designSystem;
            }

            set
            {
                designSystem = value;
                OnPropertyChanged("DesignSystem");
            }
        }
        [DataMember]
        public Int64 StartRevision
        {
            get
            {
                return startRevision;
            }

            set
            {
                startRevision = value;
                OnPropertyChanged("StartRevision");
            }
        }
        [DataMember]
        public Int64 LastRevision
        {
            get
            {
                return lastRevision;
            }
            set
            {
                lastRevision = value;
                OnPropertyChanged("LastRevision");
            }
        }
        #endregion

        #region [GEOS2-8005][rani dhamankar][13-06-2025]
        [DataMember]
        public double FinalEquivalentWeight
        {
            get
            {
                return finalEquivalentWeight;
            }
            set
            {
                finalEquivalentWeight = value;
                OnPropertyChanged("FinalEquivalentWeight");
            }
        }

        #endregion



        //[GEOS2-7880][gulab lakade][15 04 2025]

        [DataMember]
        public List<TimeTrackingCurrentStage> TimeTrackingAddingPostServer
        {
            get
            {
                return timeTrackingAddingPostServer;
            }

            set
            {
                timeTrackingAddingPostServer = value;
                OnPropertyChanged("TimeTrackingAddingPostServer");
            }
        }
        //[GEOS2-7880][gulab lakade][15 04 2025]


    }
}
