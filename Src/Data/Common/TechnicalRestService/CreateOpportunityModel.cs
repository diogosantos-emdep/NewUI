using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class CreateOpportunityModel
    {
        [Display(Order = 1)]
        public string title { get; set; }

        [Display(Order = 2)]
        public string type { get; set; }

        [Display(Order = 3)]
        public string company_group { get; set; }

        [Display(Order = 4)]
        public string company_plant { get; set; }

        [Display(Order = 5)]
        public string company_country { get; set; }


        [Display(Order = 6)]
        public string status { get; set; }


        [Display(Order = 7)]
        public string source { get; set; }

        [Display(Order = 8)]
        public double? amount_value { get; set; }

        [Display(Order = 9)]
        public string amount_currency { get; set; }

        [Display(Order = 10)]
        public double? discount { get; set; }

        //[Display(Order = 11)]
        //public DateTime close_date { get; set; }
        [Display(Order = 11)]
        public string close_date { get; set; }

        //[Display(Order = 12)]
        //public DateTime rfq_reception_date { get; set; }
        [Display(Order = 12)]
        public string rfq_reception_date { get; set; }

        [Display(Order = 13)]
        public string rfq { get; set; }


        //[Display(Order = 14)]
        //public DateTime quote_sent_date { get; set; }
        [Display(Order = 14)]
        public string quote_sent_date { get; set; }

        [Display(Order = 15)]
        public List<OfferedToModel> offered_to { get; set; }

        [Display(Order = 16)]
        public string business_unit { get; set; }

        [Display(Order = 17)]
        public byte? confidence_level { get; set; }

        [Display(Order = 18)]
        public string offer_owner { get; set; }

        [Display(Order = 19)]
        public string created_by { get; set; }

        //[Display(Order = 20)]
        //public string comments { get; set; }
        private string _comments = string.Empty;
        [DataMember(Order = 20)]
        public string comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
        [DataMember(Order = 21)]//added by chitra.girigosavi[APIGEOS-1279][26/12/2024]
        public string validity_weeks { get; set; }
        public string incoterm { get; set; }
        public Int32 Idincoterm { get; set; }
        public Int32 IdReporterDepartment { get; set; }

        public string reporter_device { get; set; }
        public Int32 IdReporterDevice { get; set; }

        [Display(Order = 21)]
        public List<ProductsList> products_list { get; set; }


        [Display(Order = 22)]
        public List<CreateQuotation> quotations { get; set; }

        public Int64 IdOffer { get; set; }

        public string Description { get; set; }
        public Int32? IdCustomer { get; set; }
        public byte? IdOfferType { get; set; }
        public Int32 IdCreatedBy { get; set; }
        public Int64 OfferNumber { get; set; }
        public string OfferCode { get; set; }
        public byte? IdCurrency { get; set; }
        public string Contact { get; set; }
        public List<OptionByOffer> OptionByOffers { get; set; }

        public Int64? idShippingAddress { get; set; }

        public Int64? idSiteWhereCreated { get; set; }
        public Int64? idStatus { get; set; }
        public Int32 idSaleOwner { get; set; }
        public SiteDetail siteDetail { get; set; }
        public List<LogEntryByOffers> LstLogEntryByOffers { get; set; }
        public List<Attachments> attachments { get; set; } //chitra.girigosavi APIGEOS-1190 5/08/2024 Add support for adding attachments in items and orders (1/4)

        [IgnoreDataMember]
        public List<FileDetails> file_details { get; set; } //chitra.girigosavi APIGEOS-1190 5/08/2024 Add support for adding attachments in items and orders (1/4)

        [IgnoreDataMember]
        public Int32 IdBusinessUnit { get; set; }

        [IgnoreDataMember]
        public Int32 IdSource { get; set; }

        [IgnoreDataMember]
        public Int32 IdOfferOwner { get; set; }

        [IgnoreDataMember]
        public List<Quotation> Quotations_New { get; set; }

        [IgnoreDataMember]
        public List<Revision> Revisions { get; set; }

        [IgnoreDataMember]
        public List<RevisionItem> RevisionItems { get; set; }

        [IgnoreDataMember]
        public List<CreateRevisionItem> CreateRevision { get; set; }

        public string modified_by { get; set; }

        [IgnoreDataMember]
        public Int32 IdModifiedBy { get; set; }


        [IgnoreDataMember]
        public string CustomerName { get; set; }


        [IgnoreDataMember]
        public string StatusName { get; set; }

        [IgnoreDataMember]
        public string CurrencyName { get; set; }


        [IgnoreDataMember]
        public string BusinessUnit { get; set; }

        [IgnoreDataMember]
        public string SalesOwner { get; set; }

        [IgnoreDataMember]
        public string OfferOwner { get; set; }

        [IgnoreDataMember]
        public string SiteName { get; set; }

        [IgnoreDataMember]
        public UInt64 IdCountry { get; set; }

        [IgnoreDataMember]
        public UInt64 IdGroup { get; set; }

        [Display(Order = 23)]
        public List<CustomerPurchaseOrder> purchase_orders { get; set; }

        [DataMember(Order = 24)]
        public string ParameterPlantOwner { get; set; }

        [DataMember(Order = 25)]
        public string ParameterMainConn { get; set; }

        [DataMember(Order = 26)]
        public string ParameterLoginContext { get; set; }

        [DataMember(Order = 27)]
        public string ParameterPlantwiseconnectionstring { get; set; }

        [DataMember(Order = 28)]
        public string ParameterCommercialQuotationsPath { get; set; }

        [DataMember(Order = 29)]
        public string ParameterWorkingOrdersPath { get; set; }
        [DataMember(Order = 30)]
        public string login { get; set; }
        [DataMember(Order = 31)]
        public string OfferId { get; set; }
        [DataMember(Order = 32)]
        public string Lang { get; set; }

        [DataMember(Order = 33)]
        public string ParameterCommercialPath { get; set; }

        [DataMember(Order = 34)]
        public string ParameterCommercialOfferTemplatePath { get; set; }
        [IgnoreDataMember]
        public string CommercialOffersPath { get; set; }
        [IgnoreDataMember]
        public string FileServerIP { get; set; }

    }

    //chitra.girigosavi APIGEOS-1190 5/08/2024 Add support for adding attachments in items and orders (1/4)
    public class FileDetails
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
    }

    public class OfferedToModel
    {
        public string contact_email
        {
            get;
            set;
        }
        public bool? is_primary
        {
            get;
            set;
        }

        [IgnoreDataMember]
        public UInt32 IdContact
        {
            get;
            set;
        }

        [IgnoreDataMember]
        public Int64 IdOfferContact
        {
            get;
            set;
        }
    }

    public class ProductsList
    {
        [Display(Order = 1)]
        public string product_type { get; set; }

        [Display(Order = 2)]
        public Int64 product_quantity { get; set; }
    }

    public class CreateQuotation
    {
        [Display(Order = 1)]
        public string template { get; set; }

        [Display(Order = 2)]
        public byte IdTemplate { get; set; }

        [Display(Order = 3)]
        public List<CreateRevision> revisions { get; set; }

        [Display(Order = 4)]
        public string QuotationCode { get; set; }
    }

    public class CreateRevision
    {
        [Display(Order = 1)]
        public string number { get; set; }

        [Display(Order = 2)]
        public List<CreateRevisionItem> items { get; set; }


    }

    public class CreateRevisionItem
    {
        [Display(Order = 1)]
        public string itemNumber { get; set; }

        [Display(Order = 2)]
        public string reference { get; set; }

        [Display(Order = 3)]
        public string name { get; set; }

        [Display(Order = 4)]
        public List<Createitemsfeatures> features { get; set; }
        [Display(Order = 6)]
        public Int64? drawing_ID { get; set; }
        [Display(Order = 7)]
        public double? sale_unit_price { get; set; }

        [Display(Order = 8)]
        public Int64? quantity { get; set; }

        [Display(Order = 9)]
        public string comments { get; set; }
        [Display(Order = 10)]
        public double? discount { get; set; }
        //public Int32? discount { get; set; }
        [Display(Order = 11)]
        public Int64 Ways { get; set; }

        public Int32? idArticle { get; set; }
        public Int32? IdCP { get; set; }
        public Int32? IdCPType { get; set; }
        public Int32? DetectionID { get; set; }
        public Int32? IdTemplate { get; set; }
        public Int32? Type { get; set; }
        public string TemplateName { get; set; }
        public List<Attachments> attachments { get; set; } //chitra.girigosavi APIGEOS-1190 5/08/2024 Add support for adding attachments in items and orders (1/4)

        [IgnoreDataMember]
        public List<FileDetails> file_details { get; set; } //chitra.girigosavi APIGEOS-1190 5/08/2024 Add support for adding attachments in items and orders (1/4)
        public string Revisionattachments { get; set; }
    }
    public class Attachments
    {
        public string file_name { get; set; }
    }
    public class Createitemsfeatures
    {
        [Display(Order = 1)]
        public string Name { get; set; }
        [Display(Order = 2)]
        public Int64? Quantity { get; set; }

        [Display(Order = 3)]
        public Int32 SortOrder { get; set; }
    }
    public class OptionByOffer
    {
        private Int64 _IdOffer = 0;
        private Int64 _IdOption;
        private Int32? _Quantity;
        private string _ProductType;

        public Int64 IdOffer
        {
            get { return _IdOffer; }
            set { _IdOffer = value; }
        }

        public Int64 IdOption
        {
            get { return _IdOption; }
            set { _IdOption = value; }
        }

        public Int32? Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }

        [IgnoreDataMember]
        public string ProductType
        {
            get { return _ProductType; }
            set { _ProductType = value; }
        }
    }

    [DataContract]
    public class SiteDetail
    {
        #region Properties
        public Int32 IdCustomer { get; set; }
        public Int64 IdCountry { get; set; }
        public Int32 IdSite { get; set; }
        public string siteName;
        public string customerName;
        public Double? latitude;
        public Double? longitude;
        public string address;
        #endregion
    }
    public class LogEntryByOffers
    {
        public Int64 idLogEntry;
        public Int64 idOffer;
        public Int32 idUser;
        public DateTime? dateTime;
        public string comments;
        public byte idLogEntryType;
        // public Offer offer;
        //People people;
        //LogEntryByOfferType logEntryByOfferType;
        public bool isDeleted;
        public bool isUpdate;
        public bool isRtfText;
        public string realText;
    }
    public class CreateOfferReturn
    {
        public Int64 id { get; set; }

        public string code { get; set; }
    }

    public class OfferCurrency
    {
        public byte idCurrency;
        public string name;
        public string symbol;
        public string description;
        public Int64 codeN;
        //  public IList<Offer> offers;
        public IList<OfferCurrencyConversion> currencyConversions;
    }

    public class OfferCurrencyConversion
    {
        public Int64 idcurrencyconversion;
        public byte idcurrencyfrom;
        public byte idcurrencyto;
        public DateTime lastUpdate;
        public Single exchangeRate;
        //  public IList<SalesTargetBySite> salesTargetBySites;
        public Currency currencyTo;
        public Currency currencyFrom;

    }

    public class OfferActivityAttendees
    {
        public Int64 idActivityAttendees;
        public Int64 idActivity;
        public Int32 idUser;
        public bool isDeleted;
        // public People people;
    }
    public class GeosAppSettings
    {
        public Int16 idAppSetting;
        public string appSettingName;
        public byte? isUserModify;
        public string defaultValue;
    }

    public class WarehouseProductComponents
    {
        private Int64 _IdComponent = 0;
        [DataMember(Order = 1)]
        public Int64 IdComponent
        {
            get { return _IdComponent; }
            set { _IdComponent = value; }
        }
        private Int64 _IdWarehouseProduct = 0;
        [DataMember(Order = 2)]
        public Int64 IdWarehouseProduct
        {
            get { return _IdWarehouseProduct; }
            set { _IdWarehouseProduct = value; }
        }
        private Int32 _IdArticle = 0;
        [DataMember(Order = 3)]
        public Int32 IdArticle
        {
            get { return _IdArticle; }
            set { _IdArticle = value; }
        }
        private Int64? _Parent = 0;
        [DataMember(Order = 4)]
        public Int64? Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }
        private double? _Quantity = 0;
        [DataMember(Order = 5)]
        public double? Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }

        [IgnoreDataMember]
        public string Number { get; set; }
    }
    public class OfferActivityLinkedItem
    {
        public Int64 idActivityLinkedItem;
        public Int64 idActivity;
        public Int32? idLinkedItemType;
        public Int32? idCustomer;
        public Int32? idSite;
        //public LookupValue linkedItemType;
        //public Customer customer;
        //Company company;
        public string name;
        public bool isUpdated;
        public bool isDeleted;

        bool isVisible = true;
        public Int64? idOffer;
        public Int32? idEmdepSite;


        public DateTime creationDate;
    }

    public class OfferLogActivity
    {
        public string Comments { get; set; }

        public DateTime? Datetime { get; set; }

        public long IdActivity { get; set; }

        public long IdLogEntryByActivity { get; set; }

        public byte? IdLogEntryType { get; set; }

        public int IdUser { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsRtfText { get; set; }

        public bool IsUpdated { get; set; }

        //  public People People { get; set; }

        public string RealText { get; set; }
    }

    public class OfferAddtionalComponents
    {

        [DataMember(Order = 1)]
        public Int64 IdAdditionalArticle { get; set; }
        [DataMember(Order = 2)]
        public string Reference { get; set; }
        [DataMember(Order = 3)]
        public string Description { get; set; }
        [DataMember(Order = 4)]
        public string ImagePath { get; set; }
        [DataMember(Order = 5)]
        public bool isConfigurable { get; set; }
        [DataMember(Order = 6)]
        public bool isObligatory { get; set; }
        [DataMember(Order = 7)]
        public Int32 Quantity { get; set; }
        [DataMember(Order = 8)]
        public Int32 Articlefrom { get; set; }
        [DataMember(Order = 9)]
        public Int32 Articleto { get; set; }
        [DataMember(Order = 9)]
        public double Price { get; set; }
    }

    public class GEOSAPIRevisionItem
    {
        public bool validated;
        public decimal? unitPrice;
        public decimal? quantity;
        public Int16 obsolete;
        public string numItem;
        public DateTime? modifiedIn;
        public Int32 modifiedBy;
        public byte marked;
        public bool manualPrice;
        public string internalComment;
        public Int64 idRevisionItem;
        public Int64 idRevision;
        public Int64 idProduct;
        public string customerComment;
        public DateTime? createdIn;
        public Int32 createdBy;
        public String attachedFiles;
        public GEOSAPIWarehouseProduct warehouseProduct;
        public GEOSAPIRevision revision;

        public Int64 downloadedQuantity;
        public Int64 remainingQuantity;
        public Int64 status;

        public Int64? idDrawing;
        //  CpType cpType;
        public Int32 ways;
        public double sellPrice;
        public string connectorFamily;
        public string reference;
        // CPProduct cpProduct;
        public decimal downloadedQuantityDecimal;
        public decimal remainingQuantityDecimal;

        public decimal? discount;

        public CreateRevisionItem createRevisionItem;

        public Int32? IdTemplate { get; set; }
        public Int32? Type { get; set; }
        public string TemplateName { get; set; }
        public string Revisionattachments { get; set; }

    }

    public class GEOSAPIWarehouseProduct
    {

        public Int64 parentMultiplier;
        public Int64 parent;
        public Int64 idWarehouseProduct;
        public Int32 idArticle;
        public Int64 downloadedQty;
        public string description;
        public WarehouseProductArticle article;

        public Int64 idComponent;
        public Int64? parentIdArticleType;
    }

    public class WarehouseProductArticle
    {
        public Int32? idArticle;
        public string description_fr;
        public string description_es;
        public string description;
        public double? quantity;
        public double? saleUnitPrice;
        public double? discount;
        public Int32? Parent;
        public Int32? ParentMultiplier;
    }

    public class GEOSAPIOts
    {
        public Int32 year;
        public string wareHouseLockSession;
        public Int32 reviewedBy;
        public string observations;
        public byte numOT;
        public Int32 number;
        public DateTime? modifiedIn;
        public Int32 modifiedBy;
        // public People modifiedByPerson;
        public byte isClosed;
        public byte idTemplate;
        public Int32 idSite;
        public Int64? idShippingAddress;
        public Int64 idQuotation;
        public Int64 idOT;
        public DateTime? deliveryDate;
        public DateTime? prevDeliveryDate;

        public Byte delivered;
        public Int32 delay;
        public DateTime? creationDate;
        public Int32 createdBy;
        //   public People createdByPerson;
        public string comments;
        public string code;
        public string attachedFiles;
        public GEOSAPIQuotation quotation;
        public Dictionary<string, OTItem> items;
        public Company site;

        public List<OTItem> otItems;

        public DateTime? poDate;
        public double actualQuantity;
        public double downloadedQuantity;
        public double remainingQuantity;
        public Int16 status;

        public string mergeCode;
        public Int64? producerIdCountryGroup;
        //  public CountryGroup countryGroup;
        public Int64 otitemCount;
        // public List<ArticlesStock> articleStocks;
        public Int64 outOfStockItemCount;
        //  public List<UserShortDetail> userShortDetails;
        public string operators;
        public string operatorNames;
        public DateTime? expectedStartDate;
        public DateTime? expectedEndDate;
        public byte progress;
        public Int64 modules;
        //  object geosAppSettings;
        public bool isSendReadyExpeditionEmail;
        public bool isUpdatedRow;
        public DateTime? realStartDate;
        public DateTime? realEndDate;
        public string realDuration;
        public Int64 producedModules;
    }

    public class GEOSAPIRevision
    {
        public DateTime approvedIn;
        public string attachedFiles;
        public string comments;
        public Int32 createdBy;
        public DateTime createdIn;
        public decimal discount;
        public DateTime expireDate;
        public Int64 id;
        public bool itemModified;
        public bool modified;
        public Int32 numRevision;
        public Int32 reviewedBy;
        public bool sentToClient;
        public string sentToComments;
        public Dictionary<string, GEOSAPIRevisionItem> items;
        public GEOSAPIQuotation quotation;
        public Int64 maxNumItem;

        [IgnoreDataMember]
        public Int64 IdQuotation { get; set; }
    }

    public class GEOSAPIQuotation
    {
        public Int64 IdOffer { get; set; }
        public Int64 IdQuotation { get; set; }
        public Int64 Year { get; set; }

        public Int64 Number { get; set; }

        public string Code { get; set; }

        public DateTime CreatedIn { get; set; }
        public Template Template { get; set; }

        public List<GEOSAPIOts> Ots { get; set; }

        public List<GEOSAPIRevision> Revisions { get; set; }

        [IgnoreDataMember]
        public Int32 IdCustomer { get; set; }
        [IgnoreDataMember]
        public byte IdDetectionsTemplate { get; set; }
        [IgnoreDataMember]
        public string Description { get; set; }
    }

    public class ExportOfferComponent
    {
        #region Fields
        string _Item = string.Empty;
        string _Reference = string.Empty;
        string _Description = string.Empty;
        Int64 _Det1 = 0;
        Int64 _Det2 = 0;
        Int64 _Det3 = 0;
        Int64 _Det4 = 0;
        Int64 _Det5 = 0;
        Int64 _Det6 = 0;
        Int64 _Det7 = 0;
        string _Remarks = string.Empty;
        double _UnitPrice = 0;
        Int64 _Qty = 0;
        double _totalPrice = 0;
        #endregion

        #region Properties
        [DataMember(Order = 1)]
        public string Item
        {
            get { return _Item; }
            set { _Item = value; }
        }

        [DataMember(Order = 2)]
        public string Reference
        {
            get { return _Reference; }
            set { _Reference = value; }
        }

        [DataMember(Order = 3)]
        public string Name
        {
            get { return _Description; }
            set { _Description = value; }
        }

        [DataMember(Order = 4)]
        public Int64 Det1
        {
            get { return _Det1; }
            set { _Det1 = value; }
        }
        [DataMember(Order = 5)]
        public Int64 Det2
        {
            get { return _Det2; }
            set { _Det2 = value; }
        }
        [DataMember(Order = 6)]
        public Int64 Det3
        {
            get { return _Det3; }
            set { _Det3 = value; }
        }
        [DataMember(Order = 7)]
        public Int64 Det4
        {
            get { return _Det4; }
            set { _Det4 = value; }
        }
        [DataMember(Order = 8)]
        public Int64 Det5
        {
            get { return _Det5; }
            set { _Det5 = value; }
        }
        [DataMember(Order = 9)]
        public Int64 Det6
        {
            get { return _Det6; }
            set { _Det6 = value; }
        }
        [DataMember(Order = 10)]
        public Int64 Det7
        {
            get { return _Det7; }
            set { _Det7 = value; }
        }

        [DataMember(Order = 11)]
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        [DataMember(Order = 12)]
        public double UnitPrice
        {
            get { return _UnitPrice; }
            set { _UnitPrice = value; }
        }

        [DataMember(Order = 13)]
        public Int64 Qty
        {
            get { return _Qty; }
            set { _Qty = value; }
        }

        [DataMember(Order = 14)]
        public double TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }

        string _comments = string.Empty;
        [DataMember(Order = 15)]
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
        [DataMember(Order = 16)]
        public List<Createitemsfeatures> features { get; set; }

        public double _discount = 0;
        [DataMember(Order = 17)]
        public double Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }
        //chitra APIGEOS-1132 Add support for creating offers including modules in ECOS(3 / 3)
        public string _TemplateName = string.Empty;
        [DataMember(Order = 18)]
        public string TemplateName
        {
            get { return _TemplateName; }
            set { _TemplateName = value; }
        }
        #endregion
    }
    [DataContract]
    public class ExportOffer
    {
        #region Fields
        public string _OfferDate = string.Empty;
        public string _OfferYear = string.Empty;
        public string _OfferGroupPlant = string.Empty;
        public int _RevNumber = 0;
        public string _OfferNumberFile = string.Empty;
        public string _OfferTitle = string.Empty;
        public string _OfferNumber = string.Empty;
        public string _OfferExpiryDate = string.Empty;
        public string _CloseDate = string.Empty;
        public string _CurrencyName = string.Empty;
        public string _OfferIncoTerms = string.Empty;
        public string _OfferPaymentConditions = string.Empty;
        public string _OfferOwnerGender = string.Empty;
        public string _OfferOwnerFullName = string.Empty;
        public string _OfferOwnerPhone = string.Empty;
        public string _OfferOwnerMobile = string.Empty;
        public string _OfferOwnerEmail = string.Empty;
        public string _OfferOwnerCompanyName = string.Empty;
        public string _OfferOwnerCompanyAlias = string.Empty;
        public string _OfferOwnerCompanyStreet = string.Empty;
        public string _OfferOwnerCompanyZipCode = string.Empty;
        public string _OfferOwnerCompanyCity = string.Empty;
        public string _OfferOwnerCompanyState = string.Empty;
        public string _OfferOwnerCompanyCountry = string.Empty;
        public string _OfferContactGender = string.Empty;
        public string _OfferContactFullName = string.Empty;
        public string _OfferContactPhone = string.Empty;
        public string _OfferContactEmail = string.Empty;
        public string _OfferContactCompanyName = string.Empty;
        public string _OfferContactCompanyAlias = string.Empty;
        public string _OfferContactCompanyStreet = string.Empty;
        public string _OfferContactCompanyZipCode = string.Empty;
        public string _OfferContactCompanyCity = string.Empty;
        public string _OfferContactCompanyState = string.Empty;
        public string _OfferContactCompanyCountry = string.Empty;
        public string _OfferComments = string.Empty;
        public string _OfferSalesOwnerFullName = string.Empty;
        public string _OfferSalesOwnerEmail = string.Empty;
        public List<ExportOfferOptions> _ExportOfferOptionLst = null;
        public List<ExportOfferQuotation> _ExportOfferQuotationsLst = null;
        public List<ExportOfferQuotation> _ExportOfferInvoiceLst = null;
        public List<ExportOfferQuotation> _ExportOfferTechnicalAssistanceLst = null;
        #region chitra.girigosavi APIGEOS-1132 Add support for creating offers including modules in ECOS(3/3)
        public List<ExportOfferComponent> _ExportOfferModuleLst = null;
        #endregion
        #region chitra.girigosavi chitra.girigosavi APIGEOS-1171 Changes when generating the offer for Counterpart type items
        public List<ExportOfferComponent> _ExportOfferPneumaticLst = null;
        public List<ExportOfferComponent> _ExportOfferAssemblyLst = null;
        public List<ExportOfferComponent> _ExportOfferElectric_ControlLst = null;
        public List<ExportOfferComponent> _ExportOfferHybridLst = null;
        public List<ExportOfferComponent> _ExportOfferJapaneseLst = null;
        public List<ExportOfferComponent> _ExportOfferPullLst = null;
        public List<ExportOfferComponent> _ExportOfferVisionLst = null;
        public List<ExportOfferComponent> _ExportOfferWirelessLst = null;
        public List<ExportOfferComponent> _ExportOfferWell_InsertionLst = null;
        #endregion

        public List<ExportOfferComponent> _ExportOfferComponentLst = null;

        public string _OfferRfqNumber = string.Empty;
        public string _RfqDate = string.Empty;
        public string _OfferBusinessUnit = string.Empty;
        public string _OfferProject = string.Empty;
        public string _OfferCarOEM = string.Empty;
        public string _OfferCategory1 = string.Empty;
        public string _OfferCategory2 = string.Empty;
        //public string _OfferShippingAddress = string.Empty;
        public Int32 _IdProductSubCategory = 0;

        public string _OfferShippingAddressName = string.Empty;
        public string _OfferContactCompanyGroup = string.Empty;
        public string _OfferContactCompanySite = string.Empty;
        public string _OfferShippingAddressFiscalNumber = string.Empty;
        public string _OfferShippingAddressStreet = string.Empty;
        public string _OfferShippingAddressZipCode = string.Empty;
        public string _OfferShippingAddressCity = string.Empty;
        public string _OfferShippingAddressState = string.Empty;
        public string _OfferShippingAddressCountry = string.Empty;
        public string _OfferShippingAddressPhone = string.Empty;

        public double _OfferDiscount = 0;
        public Int32 _OfferRevisionNumber = 0;

        public string _OfferSource = string.Empty; //chitra.girigosavi[APIGEOS-1276][15/11/2024]
        #endregion


    }

    [DataContract]
    public class ExportOfferOptions
    {
        #region Fields
        string _Name = string.Empty;

        #endregion

        #region Properties
        [DataMember(Order = 1)]
        public string Name { get { return _Name; } set { _Name = value; } }

        #endregion
    }

    [DataContract]
    public class ExportOfferQuotation
    {
        #region Fields
        string _Item = string.Empty;
        string _Reference = string.Empty;
        string _Description = string.Empty;
        double _UnitPrice = 0;
        Int64 _Qty = 0;
        double _totalPrice = 0;
        double _discount = 0;
        string _comments = string.Empty;
        #endregion

        #region Properties
        [DataMember(Order = 1)]
        public string Item { get { return _Item; } set { _Item = value; } }

        [DataMember(Order = 2)]
        public string Reference { get { return _Reference; } set { _Reference = value; } }

        [DataMember(Order = 3)]
        public string Description { get { return _Description; } set { _Description = value; } }

        [DataMember(Order = 4)]
        public double UnitPrice { get { return _UnitPrice; } set { _UnitPrice = value; } }

        [DataMember(Order = 5)]
        public Int64 Qty { get { return _Qty; } set { _Qty = value; } }

        [DataMember(Order = 5)]
        public double TotalPrice { get { return _totalPrice; } set { _totalPrice = value; } }
        [DataMember(Order = 6)]
        public double Discount { get { return _discount; } set { _discount = value; } }

        [DataMember(Order = 7)]
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
        #endregion
    }
    public class Currency_conversions
    {
        private Int32 _IdCurrencyConversionFrom;
        [DataMember(Order = 1)]
        public Int32 IdCurrencyConversionFrom
        {
            get { return _IdCurrencyConversionFrom; }
            set { _IdCurrencyConversionFrom = value; }
        }
        private Int32 _IdCurrencyConversionTo;
        [DataMember(Order = 2)]
        public Int32 IdCurrencyConversionTo
        {
            get { return _IdCurrencyConversionTo; }
            set { _IdCurrencyConversionTo = value; }
        }
        private string _LastMonthAVGRate;
        [DataMember(Order = 3)]
        public string LastMonthAVGRate
        {
            get { return _LastMonthAVGRate; }
            set { _LastMonthAVGRate = value; }
        }
        private string _CurrencyConversionRate;
        [DataMember(Order = 4)]
        public string CurrencyConversionRate
        {
            get { return _CurrencyConversionRate; }
            set { _CurrencyConversionRate = value; }
        }
    }

    public class SendMailOffer
    {
        [Display(Order = 1)]
        public string Source { get; set; }

        [Display(Order = 2)]
        public string Subject { get; set; }

        [Display(Order = 3)]
        public string TO { get; set; }

        [Display(Order = 4)]
        public string CC { get; set; }

        [Display(Order = 5)]
        public string Body { get; set; }

        [Display(Order = 6)]
        public bool IsHtmlBody { get; set; }


        [DataMember(Order = 7)]
        public string ParameterPlantOwner { get; set; }

        [DataMember(Order = 8)]
        public string ParameterMainConn { get; set; }

        [DataMember(Order = 9)]
        public string ParameterLoginContext { get; set; }

        [DataMember(Order = 10)]
        public string ParameterPlantwiseconnectionstring { get; set; }

        [DataMember(Order = 11)]
        public string login { get; set; }

        [DataMember(Order = 12)]
        public string OfferId { get; set; }

        [DataMember(Order = 13)]
        public string ParameterMailServerName { get; set; }
        [DataMember(Order = 14)]
        public string ParameterMailServerPort { get; set; }
        [DataMember(Order = 15)]
        public string ParameterMailFrom { get; set; }
        [DataMember(Order = 16)]
        public string ParameterMailpassword { get; set; }
        [DataMember(Order = 17)]
        public string ParameterMailUser { get; set; }
        [DataMember(Order = 18)]
        public string ParameterCommercialPath { get; set; }

        [DataMember(Order = 19)]
        public string IdTechnicalAssistanceReport { get; set; }

        [DataMember(Order = 20)]
        public string TechnicalAssistanceReportPath { get; set; }
    }

    public class SendEmailModel
    {
        bool _success = false;
        string _company = "EMDEP";
        string _terms = "This output has been generated automatically by GEOS. All the containing data is property of EMDEP. All rights reserved.";

        #region Properties
        [Display(Order = 1)]
        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }
        [Display(Order = 2)]
        public string company
        {
            get { return _company; }
            set { _company = value; }
        }
        [Display(Order = 3)]
        public string terms
        {
            get { return _terms; }
            set { _terms = value; }
        }

        [Display(Order = 5)]
        public ErrorMessage error { get; set; }

        [Display(Order = 6)]
        public ErrorMessage warning { get; set; }

        //[Display(Order = 4)]
        //public SendEmail SendEmail { get; set; }


        //public ErrorMessage warning { get; set; }

        #endregion
    }

    public class SendEmail
    {
        [Display(Order = 1)]
        public ErrorMessage error { get; set; }
        [Display(Order = 2)]
        public ErrorMessage warning { get; set; }
    }


}
