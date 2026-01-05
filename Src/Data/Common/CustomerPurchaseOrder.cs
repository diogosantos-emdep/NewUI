using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("customerpurchaseorders")]
    [DataContract]
    public class CustomerPurchaseOrder : ModelBase, IDisposable
    {
        #region  Fields
        Int64 idCustomerPurchaseOrders;
        string code;
        DateTime receivedIn;
        string receivedBy;
        double value;
        byte idCurrency;
        DateTime createdIn;
        Int32 createdBy;
        DateTime modifiedIn;
        Int32 idmodifiedBy;
        byte isGoAhead;
        Int32 idSite;
        Int64? idShippingAddress;
        byte isPartial;
        string quarter;
        string year;
        User modifiedBy;
        Offer offer;
        Company company;
        Currency currency;
        ShippingAddress shippingAddress;
        bool emailSent;
        string comments;
        OperationDb operationDb;
        People peopleCreatedBy;
        People peopleModifiedBy;
        Int64 idOffer;
        Int32 idPOType;
        Int32 idSender;
        CustomerPurchaseOrderType customerPurchaseOrderType;
        #endregion

        #region Constructor
        public CustomerPurchaseOrder()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdCustomerPurchaseOrders")]

        [DataMember]
        public Int64 IdCustomerPurchaseOrders
        {
            get
            {
                return idCustomerPurchaseOrders;
            }

            set
            {
                idCustomerPurchaseOrders = value;
                OnPropertyChanged("IdCustomerPurchaseOrders");
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

        [Column("ReceivedIn")]
        [DataMember]
        public DateTime ReceivedIn
        {
            get
            {
                return receivedIn;
            }

            set
            {
                receivedIn = value;
                OnPropertyChanged("ReceivedIn");
            }
        }

        [Column("ReceivedBy")]
        [DataMember]
        public string ReceivedBy
        {
            get
            {
                return receivedBy;
            }

            set
            {
                receivedBy = value;
                OnPropertyChanged("ReceivedIn");
            }
        }

        [Column("Value")]
        [DataMember]
        public double Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        [Column("IdCurrency")]
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

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime ModifiedIn
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

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 IdModifiedBy
        {
            get
            {
                return idmodifiedBy;
            }

            set
            {
                idmodifiedBy = value;
                OnPropertyChanged("IdModifiedBy");
            }
        }

        [Column("IsGoAhead")]
        [DataMember]
        public byte IsGoAhead
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

        [Column("IdSite")]
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

        [Column("IsPartial")]
        [DataMember]
        public byte IsPartial
        {
            get
            {
                return isPartial;
            }

            set
            {
                isPartial = value;
                OnPropertyChanged("IsPartial");
            }
        }


        [Column("IdPOType")]
        [DataMember]
        public Int32 IdPOType
        {
            get
            {
                return idPOType;
            }

            set
            {
                idPOType = value;
                OnPropertyChanged("IdPOType");
            }
        }

        [NotMapped]
        [DataMember]
        public string Quarter
        {
            get
            {
                return quarter;
            }

            set
            {
                quarter = value;
                OnPropertyChanged("Quarter");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public People PeopleCreatedBy
        {
            get
            {
                return peopleCreatedBy;
            }

            set
            {
                peopleCreatedBy = value;
                OnPropertyChanged("PeopleCreatedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public People PeopleModifiedBy
        {
            get
            {
                return peopleModifiedBy;
            }

            set
            {
                peopleModifiedBy = value;
                OnPropertyChanged("PeopleModifiedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public OperationDb OperationDB
        {
            get
            {
                return operationDb;
            }

            set
            {
                operationDb = value;
                OnPropertyChanged("OperationDB");
            }
        }

        [NotMapped]
        [DataMember]
        public Company Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        [NotMapped]
        [DataMember]
        public bool EmailSent
        {
            get
            {
                return emailSent;
            }

            set
            {
                emailSent = value;
                OnPropertyChanged("EmailSent");
            }
        }

        [NotMapped]
        [DataMember]
        public ShippingAddress ShippingAddress
        {
            get
            {
                return shippingAddress;
            }

            set
            {
                shippingAddress = value;
                OnPropertyChanged("ShippingAddress");
            }
        }

        [NotMapped]
        [DataMember]
        public string Year
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

        [NotMapped]
        [DataMember]
        public virtual Offer Offer
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

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public User ModifiedBy
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

        [NotMapped]
        [DataMember]
        public Int64 IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [NotMapped]
        [DataMember]
        public CustomerPurchaseOrderType CustomerPurchaseOrderType
        {
            get { return customerPurchaseOrderType; }
            set
            {
                customerPurchaseOrderType = value;
                OnPropertyChanged("CustomerPurchaseOrderType");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdSender
        {
            get { return idSender; }
            set
            {
                idSender = value;
                OnPropertyChanged("IdSender");
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
            return this.MemberwiseClone();
        }


        #endregion
    }
}
