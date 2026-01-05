using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.TableAttribute("CountryEURO")]
    public class CountryEURO : ModelBase, IDisposable
    {
        #region Fields

        Int64 idOt;
        string code;
        int numOT;
        string otCode;
        string comments;
        int createdBy;
        string createdByName;
        string createdBySurname;
        DateTime creationDate;
        int reviewedBy;
        int modifiedBy;
        string modifiedByName;
        string modifiedBySurname;
        DateTime modifiedIn;
        DateTime deliveryDate;
        int wareHouseLockSession;
        string attachedFiles;
        int idCustomer;
        string customerName;
        int idSite;
        string siteName;
        string customer;
        string shortName;
        int idTemplate;
        string template;
        int idTemplateType;
        int idQuotation;
        int year;
        string quotationCode;
        string description;
        string projectName;
        int idShippingAddress;
        int idOffer;
        int idCarriageMethod;
        string carriageValue;
        int carriageImage;
        DateTime pODate;
        int idCountry;
        string countryName;
        string iso;
        int euroZone;
        int idCountryGroup;
        string countryGroup;
        int isFreeTrade;
        int producerIdCountryGroup;
        string producerCountryGroup;
        string producerCountryGroupColor;
        #endregion

        #region Constructor
        public CountryEURO()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdOt")]
        [DataMember]
        public long IdOt
        {
            get { return idOt; }
            set
            {
                idOt = value;
                OnPropertyChanged("IdOt");
            }
        }

        [Column("Code")]
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


        [Column("NumOT")]
        [DataMember]
        public int  NumOT
        {
            get { return numOT; }
            set
            {
                numOT = value;
                OnPropertyChanged("NumOT");
            }
        }


        [Column("OtCode")]
        [DataMember]
        public string OtCode
        {
            get { return otCode; }
            set
            {
                otCode = value;
                OnPropertyChanged("OtCode");
            }
        }

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public int CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("CreatedByName")]
        [DataMember]
        public string CreatedByName
        {
            get { return createdByName; }
            set
            {
                createdByName = value;
                OnPropertyChanged("CreatedByName");
            }
        }

        [Column("CreatedBySurname")]
        [DataMember]
        public string CreatedBySurname
        {
            get { return createdBySurname; }
            set
            {
                createdBySurname = value;
                OnPropertyChanged("CreatedBySurname");
            }
        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [Column("ReviewedBy")]
        [DataMember]
        public int ReviewedBy
        {
            get { return reviewedBy; }
            set
            {
                reviewedBy = value;
                OnPropertyChanged("ReviewedBy");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public int ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }


        [Column("ModifiedByName")]
        [DataMember]
        public string ModifiedByName
        {
            get { return modifiedByName; }
            set
            {
                modifiedByName = value;
                OnPropertyChanged("ModifiedByName");
            }
        }


        [Column("ModifiedBySurname")]
        [DataMember]
        public string ModifiedBySurname
        {
            get { return modifiedBySurname; }
            set
            {
                modifiedBySurname = value;
                OnPropertyChanged("ModifiedBySurname");
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


        [Column("DeliveryDate")]
        [DataMember]
        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }


        [Column("WareHouseLockSession")]
        [DataMember]
        public int WareHouseLockSession
        {
            get { return wareHouseLockSession; }
            set
            {
                wareHouseLockSession = value;
                OnPropertyChanged("WareHouseLockSession");
            }
        }


        [Column("AttachedFiles")]
        [DataMember]
        public string AttachedFiles
        {
            get { return attachedFiles; }
            set
            {
                attachedFiles = value;
                OnPropertyChanged("AttachedFiles");
            }
        }


        [Column("IdCustomer")]
        [DataMember]
        public int IdCustomer
        {
            get { return idCustomer; }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [Column("CustomerName")]
        [DataMember]
        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public int IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("SiteName")]
        [DataMember]
        public string SiteName
        {
            get { return siteName; }
            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
            }
        }

        [Column("Customer")]
        [DataMember]
        public string Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }


        [Column("ShortName")]
        [DataMember]
        public string ShortName
        {
            get { return shortName; }
            set
            {
                shortName = value;
                OnPropertyChanged("ShortName");
            }
        }

        [Column("IdTemplate")]
        [DataMember]
        public int IdTemplate
        {
            get { return idTemplate; }
            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }

        [Column("Template")]
        [DataMember]
        public string Template
        {
            get { return template; }
            set
            {
                template = value;
                OnPropertyChanged("Template");
            }
        }

        [Column("IdTemplateType")]
        [DataMember]
        public int IdTemplateType
        {
            get { return idTemplateType; }
            set
            {
                idTemplateType = value;
                OnPropertyChanged("IdTemplateType");
            }
        }

        [Column("IdQuotation")]
        [DataMember]
        public int IdQuotation
        {
            get { return idQuotation; }
            set
            {
                idQuotation = value;
                OnPropertyChanged("IdQuotation");
            }
        }

        [Column("Year")]
        [DataMember]
        public int Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        [Column("QuotationCode")]
        [DataMember]
        public string QuotationCode
        {
            get { return quotationCode; }
            set
            {
                quotationCode = value;
                OnPropertyChanged("QuotationCode");
            }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("ProjectName")]
        [DataMember]
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        [Column("IdShippingAddress")]
        [DataMember]
        public int IdShippingAddress
        {
            get { return idShippingAddress; }
            set
            {
                idShippingAddress = value;
                OnPropertyChanged("IdShippingAddress");
            }
        }

        [Column("IdOffer")]
        [DataMember]
        public int IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [Column("IdCarriageMethod")]
        [DataMember]
        public int IdCarriageMethod
        {
            get { return idCarriageMethod; }
            set
            {
                idCarriageMethod = value;
                OnPropertyChanged("IdCarriageMethod");
            }
        }

        [Column("CarriageValue")]
        [DataMember]
        public string CarriageValue
        {
            get { return carriageValue; }
            set
            {
                carriageValue = value;
                OnPropertyChanged("CarriageValue");
            }
        }

        [Column("CarriageImage")]
        [DataMember]
        public int CarriageImage
        {
            get { return carriageImage; }
            set
            {
                carriageImage = value;
                OnPropertyChanged("CarriageImage");
            }
        }

        [Column("PODate")]
        [DataMember]
        public DateTime PODate
        {
            get { return pODate; }
            set
            {
                pODate = value;
                OnPropertyChanged("PODate");
            }
        }

        [Column("IdCountry")]
        [DataMember]
        public int IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }


        [Column("CountryName")]
        [DataMember]
        public string CountryName
        {
            get { return countryName; }
            set
            {
                countryName = value;
                OnPropertyChanged("CountryName");
            }

        }

        [Column("Iso")]
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

        [Column("EuroZone")]
        [DataMember]
        public int EuroZone
        {
            get { return euroZone; }
            set
            {
                euroZone = value;
                OnPropertyChanged("EuroZone");
            }
        }

           [Column("IdCountryGroup")]
            [DataMember]
            public int IdCountryGroup
        {
            get { return idCountryGroup; }
            set
            {
                idCountryGroup = value;
                OnPropertyChanged("IdCountryGroup");
            }
        }

        [Column("CountryGroup")]
        [DataMember]
        public string CountryGroup
        {
            get { return countryGroup; }
            set
            {
                countryGroup = value;
                OnPropertyChanged("CountryGroup");
            }
        }

        [Column("IsFreeTrade")]
        [DataMember]
        public int IsFreeTrade
        {
            get { return isFreeTrade; }
            set
            {
                isFreeTrade = value;
                OnPropertyChanged("IsFreeTrade");
            }
        }


        [Column("ProducerIdCountryGroup")]
        [DataMember]
        public int ProducerIdCountryGroup
        {
            get { return producerIdCountryGroup; }
            set
            {
                producerIdCountryGroup = value;
                OnPropertyChanged("ProducerIdCountryGroup");
            }
        }

        [Column("ProducerCountryGroup")]
        [DataMember]
        public string ProducerCountryGroup
        {
            get { return producerCountryGroup; }
            set
            {
                producerCountryGroup = value;
                OnPropertyChanged("ProducerCountryGroup");
            }
        }

        [Column("ProducerCountryGroupColor")]
        [DataMember]
        public string ProducerCountryGroupColor
        {
            get { return producerCountryGroupColor; }
            set
            {
                producerCountryGroupColor = value;
                OnPropertyChanged("ProducerCountryGroupColor");
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

