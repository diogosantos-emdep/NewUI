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
    public class ProductionPlanningReview: ModelBase, IDisposable
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
        private string serialNumber;
        private string itemStatus;
        private UInt64? tRework;
        private double? totalSalePrice;
        private string real;
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
        private Int32 idProductionPlant;
        private Int64 idCPType;
        private Int64? numItem;
        private string htmlColor;
        private string deliveryDateHtmlColor;
        //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        private bool expectedHtmlColorFlag;
        private DateTime quoteSendDate;
        private DateTime? goAheadDate;
        private DateTime? poDate;
        private DateTime? availbleForDesignDate;
        private DateTime? planningDeliveryDate;
        private DateTime? lastUpdateDate;
        private Int32 quantity;
        private bool isUpdatedRow;
        private UInt32 createdBy;
        private DateTime? createdIn;
        private UInt32 modifiedBy;
        private DateTime? modifiedIn;
        private string currentWorkStation; //[pallavi jadhav] [GEOS2-4481] [26 05 2023]
        private List<PlanningDateReviewStages> planningDateReviewStages;
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
        public Int32 Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [DataMember]
        public bool IsUpdatedRow
        {
            get
            {
                return isUpdatedRow;
            }

            set
            {
                isUpdatedRow = value;
                OnPropertyChanged("IsUpdatedRow");
            }
        }

        [DataMember]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [DataMember]
        public UInt32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }
        [DataMember]
        public UInt32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }
        [DataMember]
        public DateTime? PlanningDeliveryDate
        {
            get
            {
                return planningDeliveryDate;
            }

            set
            {
                planningDeliveryDate = value;
                OnPropertyChanged("PlanningDeliveryDate");
            }
        }
        [DataMember]
        public DateTime? LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }

            set
            {
                lastUpdateDate = value;
                OnPropertyChanged("LastUpdateDate");
            }
        }

        [DataMember]        public string CurrentWorkStation //[pallavi jadhav] [GEOS2-4481] [26 05 2023] 
        {            get            {                return currentWorkStation;            }            set            {                currentWorkStation = value;                OnPropertyChanged("CurrentWorkStation");            }        }
        public List<PlanningDateReviewStages> PlanningDateReviewStages
        {
            get
            {
                return planningDateReviewStages;
            }

            set
            {
                planningDateReviewStages = value;
                OnPropertyChanged("PlanningDateReviewStages");
            }
        }
        #endregion
        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            var newMEWone = (ProductionPlanningReview)this.MemberwiseClone();
            return this.MemberwiseClone();
        }

       
        #endregion
    }
}
