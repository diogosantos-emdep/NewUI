using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.OTM
{
    public class LinkedOffers : ModelBase, IDisposable
    {
        #region Fields
        private string code;
        private string description;
        private string rFQ;
        private string contact;
        private string linkedPO;
        private string status;
        private Double discount;
        private double amount;
        private string currency;
        private byte[] currencyIconbytes;
        Int32 idCurrency;
        Int32 idCarriageMethod;
        Int32 idPO;
        Int32 idCustomer;
        Int32 idStatus;
        private string htmlColor;
        Int32 idOfferType;
        string offersType;
        string confirmation;
        string offerCurrency;
        string category;
        Int32 idProductCategory;
        bool isNew;
        bool isDelete;
        bool isUpdate;
        Int64 idOffer;
        string year;
        string attachmentFileName;
        byte[] commericalAttachementsDocInBytes;
        string name;
        string customerGroup;
        string remark;
        string cutomerName;
        string plant;
        Int32 idsite;
        Int32 idofferby;
        Int32 idShippingAddress;
        DateTime? receivedin;
        Int32 idCustomerPurchaseOrder;
        string potype;
        int idpotype;
        string carriageMethod;
        string offerTypeName;
        string idperson;
        private Int32 idPlant;
        private LookupValue selectedIndexstatus;
        private TransactionOperations transactionOperation;
        private List<LinkedOffers> offerContcat;
        private string shippingAddress;
        Int64 idPORequest;
        private List<LogEntryByPOOffer> offerChangeLog;
        public bool IsNewLinkedOffer = false;
        public bool IsDeletedLinkedOffer = false;
        private ObservableCollection<LinkedOffers> linkedofferlist;
        private ObservableCollection<LinkedOffers> deletedLinkedofferlist;
        string plantfullname;
        private ObservableCollection<Emailattachment> poTypePOAttachementsList;
        private string poNumber;
        private Emailattachment attachment;
        public bool IsNewPoForOffer = false;
        //[Rahul.Gadhave]
        string country;
        string iso;
        string countryIconUrl;
        private string offerCode;
        long idOfferCustomerGroup;
        //[rahul.gadhave][GEOS2-9218][10-11-2025] https://helpdesk.emdep.com/browse/GEOS2-9218
        bool mailFromEditPoScreen;
        #endregion

        #region Constructor
        public LinkedOffers()
        {

        }
        #endregion
        #region Properties
        [DataMember]
        public ObservableCollection<LinkedOffers> DeletedLinkedofferlist
        {
            get
            {
                return deletedLinkedofferlist;
            }

            set
            {
                deletedLinkedofferlist = value;
                OnPropertyChanged("DeletedLinkedofferlist");
            }
        }
        [DataMember]
        public ObservableCollection<LinkedOffers> Linkedofferlist
        {
            get
            {
                return linkedofferlist;
            }

            set
            {
                linkedofferlist = value;
                OnPropertyChanged("Linkedofferlist");
            }
        }
        [DataMember]
        public List<LinkedOffers> OffersContact
        {
            get { return offerContcat; }
            set { offerContcat = value; OnPropertyChanged("OffersContact"); }
        }
        [DataMember]
        public TransactionOperations TransactionOperation
        {
            get
            {
                return this.transactionOperation;
            }
            set
            {
                this.transactionOperation = value;
                this.OnPropertyChanged("TransactionOperation");
            }
        }
        [DataMember]
        public string CutomerName
        {
            get
            {
                return cutomerName;
            }

            set
            {
                cutomerName = value;
                OnPropertyChanged("CutomerName");
            }
        }

        private ImageSource activityLinkedItemImage;
        [DataMember]
        public ImageSource ActivityLinkedItemImage
        {
            get
            {
                return activityLinkedItemImage;
            }
            set
            {
                activityLinkedItemImage = value;
                OnPropertyChanged("ActivityLinkedItemImage");
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

        [DataMember]
        public Int32 IdProductCategory
        {
            get
            {
                return idProductCategory;
            }

            set
            {
                idProductCategory = value;
                OnPropertyChanged("IdProductCategory");
            }
        }

        [DataMember]
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        [DataMember]
        public string OfferCurrency
        {
            get { return offerCurrency; }
            set
            {
                offerCurrency = value;
                OnPropertyChanged("OfferCurrency");
            }
        }
        [DataMember]
        public string Confirmation
        {
            get { return confirmation; }
            set
            {
                confirmation = value;
                OnPropertyChanged("Confirmation");
            }
        }
        [DataMember]
        public string OffersType
        {
            get
            {
                return offersType;
            }

            set
            {
                offersType = value;
                OnPropertyChanged("offersType");
            }
        }


        [DataMember]
        public Int32 IdOfferType
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
        public Int32 IdStatus
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

        [DataMember]
        public string LinkedPO
        {
            get
            {
                return linkedPO;
            }

            set
            {
                linkedPO = value;
                OnPropertyChanged("LinkedPO");
            }
        }

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

        [DataMember]
        public Int32 IdPO
        {
            get
            {
                return idPO;
            }

            set
            {
                idPO = value;
                OnPropertyChanged("IdPO");
            }
        }

        [DataMember]
        public Int32 IdCurrency
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

        [DataMember]
        public string RFQ
        {
            get
            {
                return rFQ;
            }

            set
            {
                rFQ = value;
                OnPropertyChanged("RFQ");
            }
        }

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

        [DataMember]
        public double Discount
        {
            get
            {
                return discount;
            }

            set
            {
                discount = value;
                OnPropertyChanged("Discount");
            }
        }


        [DataMember]
        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        [DataMember]
        public string Currency
        {
            get { return currency; }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }
        [DataMember]
        public byte[] CurrencyIconBytes
        {
            get { return currencyIconbytes; }
            set
            {
                currencyIconbytes = value;
                OnPropertyChanged("CurrencyIconBytes");
            }
        }
        [DataMember]
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged("IsNew");
            }
        }

        [DataMember]
        public bool IsDelete
        {
            get { return isDelete; }
            set
            {
                isDelete = value;
                OnPropertyChanged("IsDelete");
            }
        }
        public Int64 IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        

        public string AttachmentFileName
        {
            get { return attachmentFileName; }
            set
            {
                attachmentFileName = value;
                OnPropertyChanged("AttachmentFileName");
            }
        }

        public byte[] CommericalAttachementsDocInBytes
        {
            get
            {
                return commericalAttachementsDocInBytes;
            }

            set
            {
                commericalAttachementsDocInBytes = value;
                OnPropertyChanged("CommericalAttachementsDocInBytes");
            }
        }

        [DataMember]
        public string CustomerGroup
        {
            get
            {
                return customerGroup;
            }

            set
            {
                customerGroup = value;
                OnPropertyChanged("CustomerGroup");
            }
        }
        [DataMember]
        public bool IsUpdate
        {
            get { return isUpdate; }
            set
            {
                isUpdate = value;
                OnPropertyChanged("IsUpdate");
            }
        }
       [DataMember]
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                OnPropertyChanged("Remark");
            }
        }

        //[Rahul.Gadhave][GEOS2-7246][Date:15-04-2025]
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idsite;
            }

            set
            {
                idsite = value;
                OnPropertyChanged("IdSite");
            }
        }
        [DataMember]
        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }
        public Int32 IdOfferedBy
        {
            get
            {
                return idofferby;
            }

            set
            {
                idofferby = value;
                OnPropertyChanged("IdOfferedBy");
            }
        }
        public Int32 IdShippingAddress
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
        public Int32 IdCustomerPurchaseOrder
        {
            get
            {
                return idCustomerPurchaseOrder;
            }
            set
            {
                idCustomerPurchaseOrder = value;
                OnPropertyChanged("IdCustomerPurchaseOrder");
            }
        }
        [DataMember]
        public DateTime? ReceivedIn
        {
            get { return receivedin; }
            set
            {
                receivedin = value;
                OnPropertyChanged("ReceivedIn");
            }
        }
        [DataMember]
        public string PoType
        {
            get { return potype; }
            set
            {
                potype = value;
                OnPropertyChanged("PoType");
            }
        }
        [DataMember]
        public int IdPOType
        {
            get { return idpotype; }
            set
            {
                idpotype = value;
                OnPropertyChanged("IdPOType");
            }
        }

        [DataMember]
        public Int32 IdCarriageMethod
        {
            get
            {
                return idCarriageMethod;
            }

            set
            {
                idCarriageMethod = value;
                OnPropertyChanged("IdCarriageMethod");
            }
        }
        [DataMember]
        public string CarriageMethod
        {
            get { return carriageMethod; }
            set
            {
                carriageMethod = value;
                OnPropertyChanged("CarriageMethod");
            }
        }
        [DataMember]
        public string OfferTypeName
        {
            get { return offerTypeName; }
            set
            {
                offerTypeName = value;
                OnPropertyChanged("OfferTypeName");
            }
        }
        [DataMember]
        public string IdPerson
        {
            get
            {
                return idperson;
            }

            set
            {
                idperson = value;
                OnPropertyChanged("IdPerson");
            }
        }
        [DataMember]
        public Int32 IdPlant
        {
            get
            {
                return idPlant;
            }

            set
            {
                idPlant = value;
                OnPropertyChanged("IdPlant");
            }
        }
        //[Rahul.gadhave][GEOS2-7246][Date:03-06-2025]
        [DataMember]
        public LookupValue SelectedIndexStatus
        {
            get { return selectedIndexstatus; }
            set
            {
                selectedIndexstatus = value;
                OnPropertyChanged("SelectedIndexStatus");
            }
        }

        //[Rahul.gadhave][GEOS2-7246][Date:03-06-2025]
        [DataMember]
        public Int64 IdPORequest
        {
            get
            {
                return idPORequest;
            }
            set
            {
                idPORequest = value;
                OnPropertyChanged("IdPORequest");
            }
        }

        public string ShippingAddress
        {
            get { return shippingAddress; }
            set { shippingAddress = value; OnPropertyChanged("ShippingAddress"); }
        }
        public List<LogEntryByPOOffer> OfferChangeLog
        {
            get { return offerChangeLog; }
            set { offerChangeLog = value; OnPropertyChanged("OfferChangeLog"); }
        }
        //[Rahul.Gadhave][GEOS2-7253][Date:07/05/2025]
        [DataMember]
        public string PlantFullName
        {
            get
            {
                return plantfullname;
            }
            set
            {
                plantfullname = value;
                OnPropertyChanged("PlantFullName");
            }
        }

        public Emailattachment Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged("Attachment");
            }
        }

        [DataMember]
        public string PONumber
        {
            get
            {
                return poNumber;
            }

            set
            {
                poNumber = value;
                OnPropertyChanged("PONumber");
            }
        }

        [DataMember]
        public ObservableCollection<Emailattachment> PoTypePOAttachementsList
        {
            get { return poTypePOAttachementsList; }
            set
            {
                poTypePOAttachementsList = value;
                OnPropertyChanged("PoTypePOAttachementsList");
            }
        }

        //[Rahul.Gadhave]
        [DataMember]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }
        long idCountry;
        [DataMember]
        public Int64 IdCountry
        {
            get
            {
                return idCountry;
            }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }
        [DataMember]
        public string Iso
        {
            get { return iso; }
            set
            {
                iso = value;
                OnPropertyChanged("Iso");
            }
        }
        [DataMember]
        public string CountryIconUrl
        {
            get { return countryIconUrl; }
            set
            {
                countryIconUrl = value;
                OnPropertyChanged("CountryIconUrl");
            }
        }

        [DataMember]
        public string OfferCode
        {
            get
            {
                return offerCode;
            }

            set
            {
                offerCode = value;
                OnPropertyChanged("OfferCode");
            }
        }

        [DataMember]
        public Int64 IdOfferCustomerGroup
        {
            get
            {
                return idOfferCustomerGroup;
            }
            set
            {
                idOfferCustomerGroup = value;
                OnPropertyChanged(("IdOfferCustomerGroup"));
            }
        }
        //[rahul.gadhave][GEOS2-9218][10-11-2025] https://helpdesk.emdep.com/browse/GEOS2-9218
        [DataMember]
        public bool MailFromEditPoScreen
        {
            get { return mailFromEditPoScreen; }
            set
            {
                mailFromEditPoScreen = value;
                OnPropertyChanged("MailFromEditPoScreen");
            }
        }
        #endregion

        #region Methods
        public enum TransactionOperations
        {
            Add,
            Modify,
            Update,
            Delete
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

    }
}
