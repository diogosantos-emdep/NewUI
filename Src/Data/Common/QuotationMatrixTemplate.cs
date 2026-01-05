using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    [Table("QuotationMatrixTemplates")]
    [DataContract(IsReference = true)]
    public class QuotationMatrixTemplate : ModelBase, IDisposable
    {
        #region Fields
        Int64 idQuotationMatrixTemplate;
        string name;
        string description;
        LookupValue regionLookupValue;
        Customer customer;
        ProductCategory productCategory;
        string url;
        bool inUse;

        #endregion

        #region Constructor
        public QuotationMatrixTemplate()
        {
            Customer = new Customer();
            ProductCategory = new ProductCategory();
        }

        #endregion

        #region Properties

        [Column("IdQuotationMatrixTemplate")]
        [DataMember]
        public long IdQuotationMatrixTemplate
        {
            get
            {
                return idQuotationMatrixTemplate;
            }

            set
            {
                this.idQuotationMatrixTemplate = value;
                OnPropertyChanged("IdQuotationMatrixTemplate");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.name = value;
                OnPropertyChanged("Name");
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
                this.description = value;
                OnPropertyChanged("Description");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue RegionLookupValue
        {
            get
            {
                return regionLookupValue;
            }

            set
            {
                this.regionLookupValue = value;
                OnPropertyChanged("RegionLookupValue");
            }
        }

        [NotMapped]
        [DataMember]
        public Customer Customer
        {
            get
            {
                return customer;
            }

            set
            {
                this.customer = value;
                OnPropertyChanged("Customer");
            }
        }

        [NotMapped]
        [DataMember]
        public ProductCategory ProductCategory
        {
            get
            {
                return productCategory;
            }

            set
            {
                this.productCategory = value;
                OnPropertyChanged("ProductCategory");
            }
        }

        [Column("Url")]
        [DataMember]
        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                this.url = value;
                OnPropertyChanged("Url");
            }
        }

        [Column("InUse")]
        [DataMember]
        public bool InUse
        {
            get
            {
                return inUse;
            }

            set
            {
                this.inUse = value;
                OnPropertyChanged("InUse");
                OnPropertyChanged("InUseYesOrNo");
            }
        }


        [NotMapped]
        [IgnoreDataMember]
        public string InUseYesOrNo
        {
            get
            {
                if (InUse)
                    return "Yes";
                else
                    return "No";
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
