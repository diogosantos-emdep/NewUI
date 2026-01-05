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
    [Table("quotations")]
    [DataContract(IsReference = true)]
    public class Quotation : ModelBase, IDisposable
    {
        #region Fields

        Int32 idCommercial;
        Int64 idQuotation;
        Int32 idCustomer;
        string code;
        Int32 year;
        Int64 number;
        DateTime quotationDate;
        string description;
        string rFQ;
        double price;
        string comments;
        string missingSamples;
        byte idDetectionsTemplate;
        UInt32 idTechnicalTemplate;
        string projectName;
        DateTime createdIn;
        byte isCO;
        Int64 idOffer;
        byte idCurrency;
        Int64? quotQuantity;
        Int32 modifiedBy;
        DateTime? modifiedIn;
        Template template;
        Offer offer;
        List<Revision> revisions;
        List<Ots> ots;
        Company site;

        Int64 idWarehouse;

        TechnicalTemplate technicalTemplate;

        #endregion

        #region Constructor
        public Quotation()
        {
        }

        #endregion

        #region Properties

        [Column("IdCommercial")]
        [DataMember]
        public Int32 IdCommercial
        {
            get
            {
                return idCommercial;
            }

            set
            {
                idCommercial = value;
                OnPropertyChanged("IdCommercial");
            }
        }

        [Key]
        [Column("IdQuotation")]
        [DataMember]
        public Int64 IdQuotation
        {
            get
            {
                return idQuotation;
            }

            set
            {
                idQuotation = value;
                OnPropertyChanged("IdQuotation");
            }
        }

        [Column("IdCustomer")]
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

        [Column("Year")]
        [DataMember]
        public Int32 Year
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

        [Column("QuotationDate")]
        [DataMember]
        public DateTime QuotationDate
        {
            get
            {
                return quotationDate;
            }

            set
            {
                quotationDate = value;
                OnPropertyChanged("QuotationDate");
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

        [Column("RFQ")]
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

        [Column("Price")]
        [DataMember]
        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
                OnPropertyChanged("Price");
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

        [Column("MissingSamples")]
        [DataMember]
        public string MissingSamples
        {
            get
            {
                return missingSamples;
            }

            set
            {
                missingSamples = value;
                OnPropertyChanged("MissingSamples");
            }
        }

        [ForeignKey("Template")]
        [Column("IdDetectionsTemplate")]
        [DataMember]
        public byte IdDetectionsTemplate
        {
            get
            {
                return idDetectionsTemplate;
            }

            set
            {
                idDetectionsTemplate = value;
                OnPropertyChanged("IdDetectionsTemplate");
            }
        }

        [Column("IdTechnicalTemplate")]
        [DataMember]
        public UInt32 IdTechnicalTemplate
        {
            get { return idTechnicalTemplate; }
            set
            {
                idTechnicalTemplate = value;
                OnPropertyChanged("IdTechnicalTemplate");
            }
        }

        [Column("ProjectName")]
        [DataMember]
        public string ProjectName
        {
            get
            {
                return projectName;
            }

            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
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

        [Column("IsCO")]
        [DataMember]
        public byte IsCO
        {
            get
            {
                return isCO;
            }

            set
            {
                isCO = value;
                OnPropertyChanged("IsCO");
            }
        }

        [Column("IdOffer")]
        [ForeignKey("Offer")]
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

        [Column("QuotQuantity")]
        [DataMember]
        public Int64? QuotQuantity
        {
            get
            {
                return quotQuantity;
            }

            set
            {
                quotQuantity = value;
                OnPropertyChanged("QuotQuantity");
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

        [NotMapped]
        [DataMember]
        public List<Revision> Revisions
        {
            get
            {
                return revisions;
            }

            set
            {
                revisions = value;
                OnPropertyChanged("Revisions");
            }
        }


        [NotMapped]
        [DataMember]
        public List<Ots> Ots
        {
            get
            {
                return ots;
            }

            set
            {
                ots = value;
                OnPropertyChanged("Ots");
            }
        }

        [DataMember]
        public virtual Template Template
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
        public Company Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
                OnPropertyChanged("Company");
            }
        }

        [NotMapped]
        [DataMember]
        public long IdWarehouse
        {
            get
            {
                return idWarehouse;
            }

            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [NotMapped]
        [DataMember]
        public TechnicalTemplate TechnicalTemplate
        {
            get { return technicalTemplate; }
            set
            {
                technicalTemplate = value;
                OnPropertyChanged("TechnicalTemplate");
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
