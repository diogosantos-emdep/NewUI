using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMDeliveryVisualManagement : ModelBase, IDisposable
    {
        #region Field
       
        private string stageName;
        private string stageCode;
        private int sequence;
        private Int32 iDOT;
        private Int32 idSite;
        private Int32 qTY;
        private int idStage;
        private Int32 idOfferStatusType;
        private Int32 idOfferType;
        private string oTCodeProductionPlant;
        private string counterPSHtmlColor;
        private string name;
        private DateTime? goAheadDate;
        private DateTime? counterPSSateDate;
       // List<ERMDeliveeryVisualManagementStages> eRMDeliveeryVisualManagementStageList;

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
        private Int32 idProductionPlant;
        private Int64 idCPType;
        private Int64? numItem;
        private string htmlColor;
        private string deliveryDateHtmlColor;
        //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        private bool expectedHtmlColorFlag;
        private UInt64 idShippingAddress;
        private DateTime? plannedDeliveryDate;
        private string plannedDeliveryDateHtmlColor;
        private string samplesColor;
        #region [GEOS2-4093][Rupali Sarode][26-12-2022]
        private DateTime quoteSendDate;
        private DateTime? poDate;
        private DateTime? availbleForDesignDate;
        #endregion

        #region [GEOS2-4145][Pallavi Jadhav][02-03-2023]
        private string samples;
        private DateTime? samplesDate;
        #endregion

        private string drawingType; 
        private string trayName;     
        private string trayColor;     
        private DateTime? firstDeliveryDate;

        #region [GEOS2-4606][gulab lakade][30 06 2023]
        private string cPTypeName;
        private string templateName;

        #endregion

       
        private string station;  //[GEOS2-4624][rupali sarode][30-06-2023]

        
        private int newSequence; //[GEOS2-4821][Rupali Sarode][14-09-2023]

        #region [GEOS2-4862][Rupali Sarode][25-09-2023]
        private int idOfferSite; 
        private string offerSiteName;
        private int idCustomer;
        private int idProject;
        #endregion [GEOS2-4862][Rupali Sarode][25-09-2023]

        #endregion
        #region Property
        [DataMember]
        public Int32 IDOT
        {
            get
            {
                return iDOT;
            }

            set
            {
                iDOT = value;
                OnPropertyChanged("IDOT");
            }
        }

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
        public Int32 QTY
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
        public int IdStage
        {
            get
            {
                return idStage;
            }

            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }
        [DataMember]
        public int IdOfferStatusType
        {
            get
            {
                return idOfferStatusType;
            }

            set
            {
                idOfferStatusType = value;
                OnPropertyChanged("IdOfferStatusType");
            }
        }
        [DataMember]
        public int IdOfferType
        {
            get
            {
                return idOfferType;
            }

            set
            {
                idOfferType = value;
                OnPropertyChanged("IdOfferType");
            }
        }
        [DataMember]
        public string OTCodeProductionPlant
        {
            get
            {
                return oTCodeProductionPlant;
            }

            set
            {
                oTCodeProductionPlant = value;
                OnPropertyChanged("OTCodeProductionPlant");
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
        public DateTime? CounterPSSateDate
        {
            get
            {
                return counterPSSateDate;
            }

            set
            {
                counterPSSateDate = value;
                OnPropertyChanged("CounterPSSateDate");
            }
        }
        [DataMember]
        public string CounterPSHtmlColor
        {
            get
            {
                return counterPSHtmlColor;
            }

            set
            {
                counterPSHtmlColor = value;
                OnPropertyChanged("CounterPSHtmlColor");
            }
        }
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
     

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
        //[DataMember]
        //public List<ERMDeliveeryVisualManagementStages> ERMDeliveeryVisualManagementStageList
        //{
        //    get
        //    {
        //        return eRMDeliveeryVisualManagementStageList;
        //    }

        //    set
        //    {
        //        eRMDeliveeryVisualManagementStageList = value;
        //        OnPropertyChanged("ERMDeliveeryVisualManagementStageList");
        //    }
        //}
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
        public int Sequence
        {
            get
            {
                return sequence;
            }

            set
            {
                sequence = value;
                OnPropertyChanged("Sequence");
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


        #endregion

        #region  [GEOS2-4606][gulab lakade][30 06 2023]
        [DataMember]
        public string CPTypeName
        {
            get
            {
                return cPTypeName;
            }

            set
            {
                cPTypeName = value;
                OnPropertyChanged("CPTypeName");
            }
        }
        [DataMember]
        public string TemplateName
        {
            get
            {
                return templateName;
            }

            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
            }
        }


        #endregion

        #region [GEOS2-4624][rupali sarode][30-06-2023]
        [DataMember]
        public string Station
        {
            get
            {
                return station;
            }

            set
            {
                station = value;
                OnPropertyChanged("Station");
            }
        }

        #endregion

        #region [GEOS2-4821][Rupali Sarode][14-09-2023]
        [DataMember]
        public int NewSequence
        {
            get
            {
                return newSequence;
            }

            set
            {
                newSequence = value;
                OnPropertyChanged("NewSequence");
            }
        }
        #endregion [GEOS2-4821][Rupali Sarode][14-09-2023]

        #region [GEOS2-4862][Rupali Sarode][25-09-2023]
        [DataMember]
        public int IdOfferSite
        {
            get
            {
                return idOfferSite;
            }

            set
            {
                idOfferSite = value;
                OnPropertyChanged("IdOfferSite");
            }
        }

        [DataMember]
        public string OfferSiteName
        {
            get
            {
                return offerSiteName;
            }

            set
            {
                offerSiteName = value;
                OnPropertyChanged("OfferSiteName");
            }
        }

        [DataMember]
        public int IdCustomer
        {
            get
            {
                return idCustomer;
            }

            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [DataMember]
        public int IdProject
        {
            get
            {
                return idProject;
            }

            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }

        #endregion

        #endregion



        #region Constructor
        public ERMDeliveryVisualManagement()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
