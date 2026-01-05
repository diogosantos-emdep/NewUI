using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.SRM;

namespace Emdep.Geos.Data.Common
{
    [Table("articlesuppliers")]
    [DataContract]
    public class ArticleSupplier : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idArticleSupplier;
        string name;
        sbyte isStillActive;
        string address;
        string postCode;
        string city;
        string cif;
        double defaultIVA;
        string phone1;
        string phone2;
        string fax;
        string email;
        string web;
        string contactPerson;
        string contactMobile;
        string contactEmail;
        Int64 idPaymentType;
        Int64 deliveryDays;
        Int32 idAgency;
        DateTime createdIn;
        DateTime modifiedIn;
        Int32 createdBy;
        Int32 modifiedBy;
        Int64 idTransportType;
        string observations;
        string accountNumber;
        string iban;
        string swift;
        string bankName;
        string defaultText;
        byte idCurrency;
        Int32 idSite;
        string region;
        string registrationDetails;
        string bankAddress;
        sbyte freeZoneMember;

        char serie;
        Int32 idArticleSupplierType;
        ArticleSupplierType articleSupplierType;
        Int64 idCountry;
        Country country;
        WarehousePurchaseOrder warehousePurchaseOrder;
        SupplierBySite supplierBySite;
        string code;
        double age;

        DateTime? lastOrderDate;
        List<ArticleBySupplier> articleList;
        List<SRM.ArticleSuppliersDoc> documentList;
        double latitude;
        double longitude;
        string coordinates;
        #endregion

        #region Properties

        [Key]
        [Column("IdArticleSupplier")]
        [DataMember]
        public Int64 IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("Serie")]
        [DataMember]
        public char Serie
        {
            get { return serie; }
            set
            {
                serie = value;
                OnPropertyChanged("Serie");
            }
        }

        [Column("IdArticleSupplierType")]
        [DataMember]
        public Int32 IdArticleSupplierType
        {
            get { return idArticleSupplierType; }
            set
            {
                idArticleSupplierType = value;
                OnPropertyChanged("IdArticleSupplierType");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleSupplierType ArticleSupplierType
        {
            get { return articleSupplierType; }
            set
            {
                articleSupplierType = value;
                OnPropertyChanged("ArticleSupplierType");
            }
        }

        [Column("IdCountry")]
        [DataMember]
        public Int64 IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [Column("IsStillActive")]
        [DataMember]
        public sbyte IsStillActive
        {
            get { return isStillActive; }
            set
            {
                isStillActive = value;
                OnPropertyChanged("IsStillActive");
            }
        }

        [Column("Address")]
        [DataMember]
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }


        [Column("PostCode")]
        [DataMember]
        public string PostCode
        {
            get { return postCode; }
            set
            {
                postCode = value;
                OnPropertyChanged("PostCode");
            }
        }


        [Column("City")]
        [DataMember]
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }


        [Column("Cif")]
        [DataMember]
        public string Cif
        {
            get { return cif; }
            set
            {
                cif = value;
                OnPropertyChanged("Cif");
            }
        }


        [Column("DefaultIVA")]
        [DataMember]
        public Double DefaultIVA
        {
            get { return defaultIVA; }
            set
            {
                defaultIVA = value;
                OnPropertyChanged("DefaultIVA");
            }
        }


        [Column("Phone1")]
        [DataMember]
        public string Phone1
        {
            get { return phone1; }
            set
            {
                phone1 = value;
                OnPropertyChanged("Phone1");
            }
        }


        [Column("Phone2")]
        [DataMember]
        public string Phone2
        {
            get { return phone2; }
            set
            {
                phone2 = value;
                OnPropertyChanged("Phone2");
            }
        }


        [Column("Fax")]
        [DataMember]
        public string Fax
        {
            get { return fax; }
            set
            {
                fax = value;
                OnPropertyChanged("Fax");
            }
        }


        [Column("Email")]
        [DataMember]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }


        [Column("Web")]
        [DataMember]
        public string Web
        {
            get { return web; }
            set
            {
                web = value;
                OnPropertyChanged("Web");
            }
        }


        [Column("ContactPerson")]
        [DataMember]
        public string ContactPerson
        {
            get { return contactPerson; }
            set
            {
                contactPerson = value;
                OnPropertyChanged("ContactPerson");
            }
        }


        [Column("ContactMobile")]
        [DataMember]
        public string ContactMobile
        {
            get { return contactMobile; }
            set
            {
                contactMobile = value;
                OnPropertyChanged("ContactMobile");
            }
        }

        [Column("ContactEmail")]
        [DataMember]
        public string ContactEmail
        {
            get { return contactEmail; }
            set
            {
                contactEmail = value;
                OnPropertyChanged("ContactEmail");
            }
        }


        [Column("IdPaymentType")]
        [DataMember]
        public Int64 IdPaymentType
        {
            get { return idPaymentType; }
            set
            {
                idPaymentType = value;
                OnPropertyChanged("IdPaymentType");
            }
        }


        [Column("deliveryDays")]
        [DataMember]
        public Int64 DeliveryDays
        {
            get { return deliveryDays; }
            set
            {
                deliveryDays = value;
                OnPropertyChanged("deliveryDays");
            }
        }



        [Column("IdAgency")]
        [DataMember]
        public Int32 IdAgency
        {
            get { return idAgency; }
            set
            {
                idAgency = value;
                OnPropertyChanged("IdAgency");
            }
        }


        [Column("CreatedIn")]
        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }


        [Column("ModifiedIn")]
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }


        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }


        [Column("idTransportType")]
        [DataMember]
        public Int64 IdTransportType
        {
            get { return idTransportType; }
            set
            {
                idTransportType = value;
                OnPropertyChanged("IdTransportType");
            }
        }


        [Column("Observations")]
        [DataMember]
        public string Observations
        {
            get { return observations; }
            set
            {
                observations = value;
                OnPropertyChanged("Observations");
            }
        }


        [Column("AccountNumber")]
        [DataMember]
        public string AccountNumber
        {
            get { return accountNumber; }
            set
            {
                accountNumber = value;
                OnPropertyChanged("AccountNumber");
            }
        }


        [Column("Iban")]
        [DataMember]
        public string Iban
        {
            get { return iban; }
            set
            {
                iban = value;
                OnPropertyChanged("Iban");
            }
        }


        [Column("Swift")]
        [DataMember]
        public string Swift
        {
            get { return swift; }
            set
            {
                swift = value;
                OnPropertyChanged("Swift");
            }
        }


        [Column("BankName")]
        [DataMember]
        public string BankName
        {
            get { return bankName; }
            set
            {
                bankName = value;
                OnPropertyChanged("BankName");
            }
        }


        [Column("DefaultText")]
        [DataMember]
        public string DefaultText
        {
            get { return defaultText; }
            set
            {
                defaultText = value;
                OnPropertyChanged("DefaultText");
            }
        }


        [Column("idCurrency")]
        [DataMember]
        public byte IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("idCurrency");
            }
        }



        [Column("idSite")]
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }


        [Column("Region")]
        [DataMember]
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }


        [Column("RegistrationDetails")]
        [DataMember]
        public string RegistrationDetails
        {
            get { return registrationDetails; }
            set
            {
                registrationDetails = value;
                OnPropertyChanged("RegistrationDetails");
            }
        }


        [Column("BankAddress")]
        [DataMember]
        public string BankAddress
        {
            get { return bankAddress; }
            set
            {
                bankAddress = value;
                OnPropertyChanged("BankAddress");
            }
        }


        [Column("FreeZoneMember")]
        [DataMember]
        public sbyte FreeZoneMember
        {
            get { return freeZoneMember; }
            set
            {
                freeZoneMember = value;
                OnPropertyChanged("FreeZoneMember");
            }
        }


        [NotMapped]
        [DataMember]
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }


        [NotMapped]
        [DataMember]
        public WarehousePurchaseOrder WarehousePurchaseOrder
        {
            get { return warehousePurchaseOrder; }
            set
            {
                warehousePurchaseOrder = value;
                OnPropertyChanged("WarehousePurchaseOrder");
            }
        }


        [NotMapped]
        [DataMember]
        public SupplierBySite SupplierBySite
        {
            get { return supplierBySite; }
            set
            {
                supplierBySite = value;
                OnPropertyChanged("SupplierBySite");
            }
        }


        [NotMapped]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }
        [NotMapped]
        [DataMember]
        public double Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged("Age");
            }
        }
        [NotMapped]
        [DataMember]
        public DateTime? LastOrderDate
        {
            get
            {
                return lastOrderDate;
            }

            set
            {
                lastOrderDate = value;
                OnPropertyChanged("lastOrderDate");
            }
        }
        [NotMapped]
        [DataMember]
        public List<ArticleBySupplier> ArticleList
        {
            get
            {
                return articleList;
            }

            set
            {
                articleList = value;
                OnPropertyChanged("ArticleList");
            }
        }
        [NotMapped]
        [DataMember]
        public List<ArticleSuppliersDoc> DocumentList
        {
            get
            {
                return documentList;
            }

            set
            {
                documentList = value;
                OnPropertyChanged("DocumentList");
            }
        }
        [NotMapped]
        [DataMember]
        public double Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }
        [NotMapped]
        [DataMember]
        public double Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }
        [NotMapped]
        [DataMember]
        public string Coordinates
        {
            get
            {
                return coordinates;
            }

            set
            {
                coordinates = value;
                OnPropertyChanged("Coordinates");
            }
        }
        #endregion

        #region Constructor

        public ArticleSupplier()
        {
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
