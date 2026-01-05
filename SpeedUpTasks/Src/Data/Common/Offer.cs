using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.Epc;
using System.Data;
using Emdep.Geos.Data.Common.File;

namespace Emdep.Geos.Data.Common
{
    [Table("offers")]
    [DataContract(IsReference = true)]
    public class Offer : ModelBase, IDisposable
    {
        #region  Fields

        double tempDefaultValue;
        DateTime? assignedIn;
        Int32? assignedTo;
        string code;
        string comments;
        string contact;
        Int32 createdBy;
        DateTime createdIn;
        Int64 deliveryWeeks;
        Company customer;
        string description;
        byte idCurrency;
        Int32 idCustomer;
        Int32 idCustomerToDelivery;
        Int64 idOffer;
        byte? idOfferType;
        OfferType offerType;
        int? idProject;
        Int64? idPurchaseOrder;
        Int64? idServiceRequest;
        Int64? idShippingAddress;
        Int64 idStatus;
        byte isCritical;
        sbyte isGoAhead;
        sbyte isTracked;
        Int32 modifiedBy;
        DateTime? modifiedIn;
        Int64 number;
        Int32? offeredBy;
        Int32 priority;
        int probabilityOfSuccess;
        DateTime? productionFinish;
        DateTime? productionStart;
        string rfq;
        DateTime? rfqReception;
        DateTime? sendIn;
        string title;
        DateTime? deliveryDate;
        DateTime? otsDeliveryDate;
        DateTime? poReceivedInDate;
        double _value;
        double invoiceAmount;
        Int64 year;
        Project project;
        IList<Project> projects;
        GeosStatus geosStatus;
        Currency currency;
        Company site;
        IList<CustomerPurchaseOrdersByOffer> customerPurchaseOrdersByOffers;
        IList<CustomerPurchaseOrder> targetByCustomers;
        List<CustomerPurchaseOrder> salesByCustomers;
        List<CustomerPurchaseOrder> qualifiedByCustomers;
        List<CustomerPurchaseOrder> quotedByCustomers;
        List<CustomerPurchaseOrder> forecastedByCustomers;
        List<CustomerPurchaseOrder> rfqByCustomers;
        DateTime? offerExpectedDate;
        Double? totalQualified;
        SByte? offerExpectedMonth;
        Int16? offerExpectedYear;
        IList<Quotation> quotations;
        IList<string> templates;
        LostReasonsByOffer lostReasonsByOffer;
        decimal amount;
        Int64 numberOfOffers;
        string status;
        double? totalForecasted;
        double? totalQuotated;
        double? totalSales;
        double? totalRFQ;
        Dictionary<string, double> quotationItems;
        Int32 currentWeek;
        Int32 currentMonth;
        Int32 currentYear;
        int probabilityOfSuccessforstar;
        List<LogEntryByOffer> logEntryByOffers;
        List<LogEntryByOffer> commentsByOffers;
        byte? idBusinessUnit;
        List<OptionsByOffer> optionsByOffers;
        List<LostReasonsByOffer> lostReasonsByOffers;
        bool isupdateLeadToOT;
        Int32? idSalesOwner;
        People salesOwner;
        bool isCloseDateExceed;
        Int32? idCarOEM;
        GeosProject geosProject;
        CarOEM carOEM;
        byte? idSource;
        LookupValue source;
        byte? idCarriageMethod;
        LookupValue carriageMethod;

        List<OfferContact> offerContacts;
        LookupValue businessUnit;
        List<OfferStatusTracking> offerStatusTracking;
        Int64? idCarProject;
        CarProject carProject;
        Int64 idSourceOffer;
        string reportGroup;
        PartNumbers partNumber;
        string customerPOCodes;
        List<Activity> newlyLinkedActivities;
        DateTime? lastActivityDate;
        EngineeringAnalysis engineeringAnalysis;

        User modifiedByUser;
        User createdByUser;
        People taskAssigned;//Used for eng analysis task owner(Assigned)

        Int64 idProductCategory;
        ProductCategory productCategory;

        People offeredByPerson;
        People assignedToPerson;

        DateTime? shipmentDate;

        sbyte isManualCategory;
        double _offerValue;
        byte idOfferCurrency;
        bool isGoAheadProduction;
        bool isEngAnalysisONAndStatusChangedToQuoted;
        string jiraAssigneeDisplayName;
        string previousOfferStatus;
        User jiraUserReporter;
        string errorFromJira;
        string offerIssueKey;
        bool isUpdated;
        bool isCreateIssueInJira;
        bool isEngAnalysisSetONToOFF;
        ActiveSite offerActiveSite;
        bool isEngAnalysisUnlink;
        FileDetail outlookmailCopy;
        List<OptionsByOfferGrid> optionsByOffersGrid;
        ProductCategoryGrid productCategoryGrid;
        #endregion

        #region Constructor

        public Offer()
        {
            this.projects = new List<Project>();
            this.Quotations = new List<Quotation>();
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdOffer")]
        [DataMember]
        public Int64 IdOffer
        {
            get
            {
                return idOffer;
            }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [Column("IdProject")]
        [DataMember]
        public int? IdProject
        {
            get
            {
                return idProject;
            }
            set
            {
                this.idProject = value;
                OnPropertyChanged("IdProject");
            }
        }
        [Column("AssignedIn")]
        [DataMember]
        public DateTime? AssignedIn
        {
            get
            {
                return assignedIn;
            }
            set
            {
                assignedIn = value;
                OnPropertyChanged("AssignedIn");
            }
        }

        [Column("AssignedTo")]
        [DataMember]
        public Int32? AssignedTo
        {
            get
            {
                return assignedTo;
            }
            set
            {
                assignedTo = value;
                OnPropertyChanged("AssignedTo");
            }
        }


        [Column("Code")]
        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [Column("Contact")]
        [DataMember]
        public string Contact
        {
            get
            {
                return contact;
            }
            set
            {
                contact = value;
                OnPropertyChanged("Contact");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("CreatedIn")]
        [DataMember]
        public DateTime CreatedIn
        {
            get
            {
                return createdIn;
            }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("DeliveryWeeks")]
        [DataMember]
        public Int64 DeliveryWeeks
        {
            get
            {
                return deliveryWeeks;
            }
            set
            {
                deliveryWeeks = value;
                OnPropertyChanged("DeliveryWeeks");
            }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("IdCurrency")]
        [ForeignKey("Currency")]
        [DataMember]
        public byte IdCurrency
        {
            get
            {
                return idCurrency;
            }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }
        [Column("IdCustomer")]
        [ForeignKey("Site")]
        [DataMember]
        public Int32 IdCustomer
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

        [Column("IdCustomerToDelivery")]
        [DataMember]
        public Int32 IdCustomerToDelivery
        {
            get
            {
                return idCustomerToDelivery;
            }
            set
            {
                idCustomerToDelivery = value;
                OnPropertyChanged("IdCustomerToDelivery;");
            }
        }

        [Column("DeliveryDate")]
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
                OnPropertyChanged("DeliveryDate;");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? OTSDeliveryDate
        {
            get
            {
                return otsDeliveryDate;
            }
            set
            {
                otsDeliveryDate = value;
                OnPropertyChanged("OTSDeliveryDate;");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? POReceivedInDate
        {
            get
            {
                return poReceivedInDate;
            }
            set
            {
                poReceivedInDate = value;
                OnPropertyChanged("POReceivedInDate;");
            }
        }


        [Column("IdPurchaseOrder")]
        [DataMember]
        public Int64? IdPurchaseOrder
        {
            get
            {
                return idPurchaseOrder;
            }
            set
            {
                idPurchaseOrder = value;
                OnPropertyChanged("IdPurchaseOrder");
            }
        }

        [Column("IdOfferType")]
        [DataMember]
        public byte? IdOfferType
        {
            get { return idOfferType; }
            set
            {
                idOfferType = value;
                OnPropertyChanged("IdOfferType");
            }
        }

        [Column("IdServiceRequest")]
        [DataMember]
        public Int64? IdServiceRequest
        {
            get
            {
                return idServiceRequest;
            }
            set
            {
                idServiceRequest = value;
                OnPropertyChanged("IdServiceRequest");
            }
        }
        [Column("IdShippingAddress")]
        [DataMember]
        public Int64? IdShippingAddress
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

        [Column("IdStatus")]
        [ForeignKey("GeosStatus")]
        [DataMember]
        public Int64 IdStatus
        {
            get
            {
                return idStatus;
            }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [Column("IsCritical")]
        [DataMember]
        public byte IsCritical
        {
            get
            {
                return isCritical;
            }
            set
            {
                isCritical = value;
                OnPropertyChanged("IsCritical");
            }
        }

        [Column("IsGoAhead")]
        [DataMember]
        public sbyte IsGoAhead
        {
            get
            {
                return isGoAhead;
            }
            set
            {
                isGoAhead = value;
                OnPropertyChanged("IsGoAhead");
            }
        }

        [Column("IsTracked")]
        [DataMember]
        public sbyte IsTracked
        {
            get
            {
                return isTracked;
            }
            set
            {
                isTracked = value;
                OnPropertyChanged("IsTracked");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("Number")]
        [DataMember]
        public Int64 Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        [Column("OfferedBy")]
        [DataMember]
        public Int32? OfferedBy
        {
            get
            {
                return offeredBy;
            }
            set
            {
                offeredBy = value;
                OnPropertyChanged("OfferedBy");
            }
        }

        [Column("Priority")]
        [DataMember]
        public Int32 Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
                OnPropertyChanged("Priority");
            }
        }

        [Column("ProbabilityOfSuccess")]
        [DataMember]
        public int ProbabilityOfSuccess
        {
            get
            {
                return probabilityOfSuccess;
            }
            set
            {
                probabilityOfSuccess = value;
                OnPropertyChanged("ProbabilityOfSuccess");
            }
        }

        [NotMapped]
        [DataMember]
        public int ProbabilityOfSuccessForStar
        {
            get
            {
                return probabilityOfSuccessforstar;
            }
            set
            {
                probabilityOfSuccessforstar = value;
                OnPropertyChanged("ProbabilityOfSuccessForStar");
            }
        }

        [Column("ProductionFinish")]
        [DataMember]
        public DateTime? ProductionFinish
        {
            get
            {
                return productionFinish;
            }
            set
            {
                productionFinish = value;
                OnPropertyChanged("ProductionFinish");
            }
        }

        [Column("ProductionStart")]
        [DataMember]
        public DateTime? ProductionStart
        {
            get
            {
                return productionStart;
            }
            set
            {
                productionStart = value;
                OnPropertyChanged("ProductionStart");
            }
        }

        [Column("Rfq")]
        [DataMember]
        public string Rfq
        {
            get
            {
                return rfq;
            }
            set
            {
                rfq = value;
                OnPropertyChanged("Rfq");
            }
        }

        [Column("RFQReception")]
        [DataMember]
        public DateTime? RFQReception
        {
            get
            {
                return rfqReception;
            }
            set
            {
                rfqReception = value;
                OnPropertyChanged("RFQReception");
            }
        }

        [Column("SendIn")]
        [DataMember]
        public DateTime? SendIn
        {
            get
            {
                return sendIn;
            }
            set
            {
                sendIn = value;
                OnPropertyChanged("SendIn");
            }
        }

        [Column("IdBusinessUnit")]
        [DataMember]
        public byte? IdBusinessUnit
        {
            get
            {
                return idBusinessUnit;
            }
            set
            {
                idBusinessUnit = value;
                OnPropertyChanged("IdBusinessUnit");
            }
        }

        [Column("IdSource")]
        [DataMember]
        public byte? IdSource
        {
            get { return idSource; }
            set
            {
                idSource = value;
                OnPropertyChanged("IdSource");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        [Column("IdCarriageMethod")]
        [DataMember]
        public byte? IdCarriageMethod
        {
            get { return idCarriageMethod; }
            set
            {
                idCarriageMethod = value;
                OnPropertyChanged("IdCarriageMethod");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue CarriageMethod
        {
            get
            {
                return carriageMethod;
            }

            set
            {
                carriageMethod = value;
                OnPropertyChanged("CarriageMethod");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue BusinessUnit
        {
            get
            {
                return businessUnit;
            }
            set
            {
                businessUnit = value;
                OnPropertyChanged("BusinessUnit");
            }
        }

        [NotMapped]
        [DataMember]
        public List<OfferStatusTracking> OfferStatusTracking
        {
            get
            {
                return offerStatusTracking;
            }
            set
            {
                offerStatusTracking = value;
                OnPropertyChanged("OfferStatusTracking");
            }
        }

        [NotMapped]
        [DataMember]
        public LostReasonsByOffer LostReasonsByOffer
        {
            get
            {
                return lostReasonsByOffer;
            }
            set
            {
                lostReasonsByOffer = value;
                OnPropertyChanged("LostReasonsByOffer");
            }
        }

        [Column("Title")]
        [DataMember]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        [Column("Value")]
        [DataMember]
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        [NotMapped]
        [DataMember]
        public double InvoiceAmount
        {
            get
            {
                return invoiceAmount;
            }
            set
            {
                invoiceAmount = value;
                OnPropertyChanged("InvoiceAmount");
            }
        }


        [Column("Year")]
        [DataMember]
        public Int64 Year
        {
            get
            {
                return year;
            }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }



        [Column("OfferExpectedDate")]
        [DataMember]
        public DateTime? OfferExpectedDate
        {
            get
            {
                return offerExpectedDate;
            }
            set
            {
                offerExpectedDate = value;
                OnPropertyChanged("OfferExpectedDate");
            }
        }

        [Column("IdCarOEM")]
        [DataMember]
        public Int32? IdCarOEM
        {
            get
            {
                return idCarOEM;
            }
            set
            {
                idCarOEM = value;
                OnPropertyChanged("IdCarOEM");
            }
        }


        [Column("IdCarProject")]
        [DataMember]
        public Int64? IdCarProject
        {
            get
            {
                return idCarProject;
            }
            set
            {
                idCarProject = value;
                OnPropertyChanged("IdCarProject");
            }
        }


        [NotMapped]
        [DataMember]
        public CarProject CarProject
        {
            get
            {
                return carProject;
            }
            set
            {
                carProject = value;
                OnPropertyChanged("CarProject");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 NumberOfOffers
        {
            get
            {
                return numberOfOffers;
            }
            set
            {
                numberOfOffers = value;
                OnPropertyChanged("NumberOfOffers");
            }
        }

        [NotMapped]
        [DataMember]
        public CarOEM CarOEM
        {
            get
            {
                return carOEM;
            }
            set
            {
                carOEM = value;
                OnPropertyChanged("CarOEM");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdSourceOffer
        {
            get
            {
                return idSourceOffer;
            }
            set
            {
                idSourceOffer = value;
                OnPropertyChanged("IdSourceOffer");
            }
        }

        [NotMapped]
        [DataMember]
        public Company Customer
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

        [NotMapped]
        [DataMember]
        public GeosProject GeosProject
        {
            get
            {
                return geosProject;
            }
            set
            {
                geosProject = value;
                OnPropertyChanged("GeosProject");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 CurrentWeek
        {
            get
            {
                return currentWeek;
            }
            set
            {
                currentWeek = value;
                OnPropertyChanged("CurrentWeek");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 CurrentMonth
        {
            get
            {
                return currentMonth;
            }
            set
            {
                currentMonth = value;
                OnPropertyChanged("CurrentMonth");
            }
        }

        [NotMapped]
        [DataMember]
        public int CurrentYear
        {
            get { return currentYear; }
            set
            {
                currentYear = value;
                OnPropertyChanged("CurrentYear");
            }
        }

        [NotMapped]
        [DataMember]
        public double TempDefaultValue
        {
            get
            {
                return tempDefaultValue;
            }
            set
            {
                tempDefaultValue = value;
                OnPropertyChanged("TempDefaultValue");
            }
        }

        [NotMapped]
        [DataMember]
        public Double? TotalForecasted
        {
            get
            {
                return totalForecasted;
            }
            set
            {
                totalForecasted = value;
                OnPropertyChanged("TotalForecasted");
            }
        }

        [NotMapped]
        [DataMember]
        public Double? TotalQualified
        {
            get
            {
                return totalQualified;
            }
            set
            {
                totalQualified = value;
                OnPropertyChanged("TotalQualified");
            }
        }

        [NotMapped]
        [DataMember]
        public Double? TotalQuotated
        {
            get
            {
                return totalQuotated;
            }
            set
            {
                totalQuotated = value;
                OnPropertyChanged("TotalQuotated");
            }
        }

        [NotMapped]
        [DataMember]
        public Double? TotalRFQ
        {
            get
            {
                return totalRFQ;
            }
            set
            {
                totalRFQ = value;
                OnPropertyChanged("TotalRFQ");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdateLeadToOT
        {
            get
            {
                return isupdateLeadToOT;
            }
            set
            {
                isupdateLeadToOT = value;
                OnPropertyChanged("IsUpdateLeadToOT");
            }
        }

        [NotMapped]
        [DataMember]
        public Double? TotalSales
        {
            get
            {
                return totalSales;
            }
            set
            {
                totalSales = value;
                OnPropertyChanged("TotalSales");
            }
        }

        [Column("IdSalesOwner")]
        [DataMember]
        public Int32? IdSalesOwner
        {
            get
            {
                return idSalesOwner;
            }
            set
            {
                idSalesOwner = value;
                OnPropertyChanged("IdSalesOwner");
            }
        }

        [NotMapped]
        [DataMember]
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }


        [DataMember]
        public virtual IList<Project> Projects
        {
            get
            {
                return projects;
            }

            set
            {
                projects = value;
                OnPropertyChanged("Projects");
            }
        }

        [NotMapped]
        [DataMember]
        public IList<string> Templates
        {
            get
            {
                return templates;
            }

            set
            {
                templates = value;
                OnPropertyChanged("Templates");
            }
        }

        [NotMapped]
        [DataMember]
        public Dictionary<string, double> QuotationItems
        {
            get
            {
                return quotationItems;
            }

            set
            {
                quotationItems = value;
                OnPropertyChanged("QuotationItems");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsCloseDateExceed
        {
            get
            {
                return isCloseDateExceed;
            }
            set
            {
                isCloseDateExceed = value;
                OnPropertyChanged("IsCloseDateExceed");
            }
        }

        [DataMember]
        public virtual IList<CustomerPurchaseOrdersByOffer> CustomerPurchaseOrdersByOffers
        {
            get
            {
                return customerPurchaseOrdersByOffers;
            }

            set
            {
                customerPurchaseOrdersByOffers = value;
                OnPropertyChanged("CustomerPurchaseOrdersByOffers");
            }
        }

        [DataMember]
        public virtual Company Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
                OnPropertyChanged("Site");
            }
        }

        [DataMember]
        public virtual GeosStatus GeosStatus
        {
            get
            {
                return geosStatus;
            }

            set
            {
                geosStatus = value;
                OnPropertyChanged("GeosStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual People SalesOwner
        {
            get
            {
                return salesOwner;
            }

            set
            {
                salesOwner = value;
                OnPropertyChanged("SalesOwner");
            }
        }

        [DataMember]
        public virtual Currency Currency
        {
            get
            {
                return currency;
            }

            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        [DataMember]
        public virtual IList<Quotation> Quotations
        {
            get
            {
                return quotations;
            }

            set
            {
                quotations = value;
                OnPropertyChanged("Quotations");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual IList<CustomerPurchaseOrder> TargetByCustomers
        {
            get
            {
                return targetByCustomers;
            }

            set
            {
                targetByCustomers = value;
                OnPropertyChanged("TargetByCustomers");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual List<CustomerPurchaseOrder> ForecastedByCustomers
        {
            get
            {
                return forecastedByCustomers;
            }

            set
            {
                forecastedByCustomers = value;
                OnPropertyChanged("ForecastedByCustomers");
            }
        }

        [NotMapped]
        [DataMember]
        public List<OfferContact> OfferContacts
        {
            get
            {
                return offerContacts;
            }

            set
            {
                offerContacts = value;
                OnPropertyChanged("OfferContact");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual List<CustomerPurchaseOrder> QuotedByCustomers
        {
            get
            {
                return quotedByCustomers;
            }

            set
            {
                quotedByCustomers = value;
                OnPropertyChanged("QuotedByCustomers");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual List<CustomerPurchaseOrder> RFQByCustomers
        {
            get
            {
                return rfqByCustomers;
            }

            set
            {
                rfqByCustomers = value;
                OnPropertyChanged("RFQByCustomers");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual List<CustomerPurchaseOrder> QualifiedByCustomers
        {
            get
            {
                return qualifiedByCustomers;
            }

            set
            {
                qualifiedByCustomers = value;
                OnPropertyChanged("QualifiedByCustomers");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual List<CustomerPurchaseOrder> SalesByCustomers
        {
            get
            {
                return salesByCustomers;
            }

            set
            {
                salesByCustomers = value;
                OnPropertyChanged("SalesByCustomers");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual List<LogEntryByOffer> LogEntryByOffers
        {
            get
            {
                return logEntryByOffers;
            }

            set
            {
                logEntryByOffers = value;
                OnPropertyChanged("LogEntryByOffers");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntryByOffer> CommentsByOffers
        {
            get { return commentsByOffers; }
            set
            {
                commentsByOffers = value;
                OnPropertyChanged("CommentsByOffers");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual List<OptionsByOffer> OptionsByOffers
        {
            get
            {
                return optionsByOffers;
            }
            set
            {
                optionsByOffers = value;
                OnPropertyChanged("OptionsByOffers");
            }
        }


        [NotMapped]
        [DataMember]
        public virtual List<LostReasonsByOffer> LostReasonsByOffers
        {
            get
            {
                return lostReasonsByOffers;
            }
            set
            {
                lostReasonsByOffers = value;
                OnPropertyChanged("LostReasonsByOffers");
            }
        }

        [NotMapped]
        [DataMember]
        public string ReportGroup
        {
            get
            {
                return reportGroup;
            }

            set
            {
                reportGroup = value;
                OnPropertyChanged("ReportGroup");
            }
        }

        [NotMapped]
        [DataMember]
        public PartNumbers PartNumber
        {
            get { return partNumber; }
            set
            {
                partNumber = value;
                OnPropertyChanged("PartNumber");
            }
        }

        [NotMapped]
        [DataMember]
        public string CustomerPOCodes
        {
            get { return customerPOCodes; }
            set
            {
                customerPOCodes = value;
                OnPropertyChanged("CustomerPOCodes");
            }
        }

        [NotMapped]
        [DataMember]
        public List<Activity> NewlyLinkedActivities
        {
            get { return newlyLinkedActivities; }
            set
            {
                newlyLinkedActivities = value;
                OnPropertyChanged("NewlyLinkedActivities");
            }
        }

        //[NotMapped]
        //[DataMember]
        //public Int32 SleepDays
        //{
        //    get { return sleepDays; }
        //    set
        //    {
        //        sleepDays = value;
        //        OnPropertyChanged("SleepDays");
        //    }
        //}

        [NotMapped]
        [DataMember]
        public DateTime? LastActivityDate
        {
            get { return lastActivityDate; }
            set
            {
                lastActivityDate = value;
                OnPropertyChanged("LastActivityDate");
            }
        }

        [NotMapped]
        [DataMember]
        public EngineeringAnalysis EngineeringAnalysis
        {
            get { return engineeringAnalysis; }
            set
            {
                engineeringAnalysis = value;
                OnPropertyChanged("EngineeringAnalysis");
            }
        }

        [NotMapped]
        [DataMember]
        public OfferType OfferType
        {
            get { return offerType; }
            set
            {
                offerType = value;
                OnPropertyChanged("OfferType");
            }
        }

        [NotMapped]
        [DataMember]
        public User ModifiedByUser
        {
            get { return modifiedByUser; }
            set
            {
                modifiedByUser = value;
                OnPropertyChanged("ModifiedByUser");
            }
        }

        [NotMapped]
        [DataMember]
        public User CreatedByUser
        {
            get { return createdByUser; }
            set
            {
                createdByUser = value;
                OnPropertyChanged("CreatedByUser");
            }
        }

        [NotMapped]
        [DataMember]
        public People TaskAssigned
        {
            get { return taskAssigned; }
            set
            {
                taskAssigned = value;
                OnPropertyChanged("TaskAssigned");
            }
        }

        [Column("IdProductCategory")]
        [DataMember]
        public Int64 IdProductCategory
        {
            get { return idProductCategory; }
            set
            {
                idProductCategory = value;
                OnPropertyChanged("IdProductCategory");
            }
        }


        [NotMapped]
        [DataMember]
        public ProductCategory ProductCategory
        {
            get { return productCategory; }
            set
            {
                productCategory = value;
                OnPropertyChanged("ProductCategory");
            }
        }

        [NotMapped]
        [DataMember]
        public People OfferedByPerson
        {
            get { return offeredByPerson; }
            set
            {
                offeredByPerson = value;
                OnPropertyChanged("OfferedByPerson");
            }
        }

        [NotMapped]
        [DataMember]
        public People AssignedToPerson
        {
            get { return assignedToPerson; }
            set
            {
                assignedToPerson = value;
                OnPropertyChanged("AssignedToPerson");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? ShipmentDate
        {
            get { return shipmentDate; }
            set
            {
                shipmentDate = value;
                OnPropertyChanged("ShipmentDate");
            }
        }

        [Column("IsManualCategory")]
        [DataMember]
        public sbyte IsManualCategory
        {
            get { return isManualCategory; }
            set
            {
                isManualCategory = value;
                OnPropertyChanged("IsManualCategory");
            }
        }

        [NotMapped]
        [DataMember]
        public double OfferValue
        {
            get
            {
                return _offerValue;
            }
            set
            {
                _offerValue = value;
                OnPropertyChanged("OfferValue");
            }
        }


        [NotMapped]
        [DataMember]
        public byte IdOfferCurrency
        {
            get
            {
                return idOfferCurrency;
            }
            set
            {
                idOfferCurrency = value;
                OnPropertyChanged("IdOfferCurrency");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsGoAheadProduction
        {
            get
            {
                return isGoAheadProduction;
            }
            set
            {
                isGoAheadProduction = value;
                OnPropertyChanged("IsGoAheadProduction");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEngAnalysisONAndStatusChangedToQuoted
        {
            get
            {
                return isEngAnalysisONAndStatusChangedToQuoted;
            }
            set
            {
                isEngAnalysisONAndStatusChangedToQuoted = value;
                OnPropertyChanged("IsEngAnalysisONAndStatusChangedToQuoted");
            }
        }

        [NotMapped]
        [DataMember]
        public string JiraAssigneeDisplayName
        {
            get
            {
                return jiraAssigneeDisplayName;
            }
            set
            {
                jiraAssigneeDisplayName = value;
                OnPropertyChanged("JiraAssigneeDisplayName");
            }
        }

        [NotMapped]
        [DataMember]
        public string PreviousOfferStatus
        {
            get
            {
                return previousOfferStatus;
            }
            set
            {
                previousOfferStatus = value;
                OnPropertyChanged("PreviousOfferStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public User JiraUserReporter
        {
            get
            {
                return jiraUserReporter;
            }
            set
            {
                jiraUserReporter = value;
                OnPropertyChanged("JiraUserReporter");
            }
        }


        [NotMapped]
        [DataMember]
        public String ErrorFromJira
        {
            get
            {
                return errorFromJira;
            }
            set
            {
                errorFromJira = value;
                OnPropertyChanged("ErrorFromJira");
            }
        }

        [NotMapped]
        [DataMember]
        public String OfferIssueKey
        {
            get
            {
                return offerIssueKey;
            }
            set
            {
                offerIssueKey = value;
                OnPropertyChanged("OfferIssueKey");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsUpdated
        {
            get
            {
                return isUpdated;
            }
            set
            {
                isUpdated = value;
                OnPropertyChanged("IsUpdated");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsCreateIssueInJira
        {
            get
            {
                return isCreateIssueInJira;
            }
            set
            {
                isCreateIssueInJira = value;
                OnPropertyChanged("IsCreateIssueInJira");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsEngAnalysisSetONToOFF
        {
            get
            {
                return isEngAnalysisSetONToOFF;
            }
            set
            {
                isEngAnalysisSetONToOFF = value;
                OnPropertyChanged("IsEngAnalysisSetONToOFF");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEngAnalysisUnlink
        {
            get
            {
                return isEngAnalysisUnlink;
            }
            set
            {
                isEngAnalysisUnlink = value;
                OnPropertyChanged("IsEngAnalysisUnlink");
            }
        }


        [NotMapped]
        [DataMember]
        public FileDetail OutLookMailCopy
        {
            get
            {
                return outlookmailCopy;
            }
            set
            {
                outlookmailCopy = value;
                OnPropertyChanged("OutLookMailCopy");
            }
        }

        [NotMapped]
        [DataMember]
        public ActiveSite OfferActiveSite
        {
            get
            {
                return offerActiveSite;
            }
            set
            {
                offerActiveSite = value;
                OnPropertyChanged("OfferActiveSite");
            }
        }


        [NotMapped]
        [DataMember]
        public List<OptionsByOfferGrid> OptionsByOffersGrid
        {
            get
            {
                return optionsByOffersGrid;
            }
            set
            {
                optionsByOffersGrid = value;
                OnPropertyChanged("OptionsByOffersGrid");
            }
        }

        [NotMapped]
        [DataMember]
        public ProductCategoryGrid ProductCategoryGrid
        {
            get
            {
                return productCategoryGrid;
            }
            set
            {
                productCategoryGrid = value;
                OnPropertyChanged("ProductCategoryGrid");
            }
        }

        #endregion

        #region Methods

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
