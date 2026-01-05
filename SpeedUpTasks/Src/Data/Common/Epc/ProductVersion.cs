using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("product_versions")]
    [DataContract(IsReference = true)]
    public class ProductVersion : ModelBase ,IDisposable
    {
        #region Fields
        Int64 idProductVersion;
        Int64 idProduct;
        string productVersionNumber;
        Int32 idCreator;
        User creator;
        DateTime? createdDate;
        Product product;
        DateTime? versionBetaDate;
        DateTime? versionReleaseDate;
        IList<ProductVersionItem> productVersionItems;
        #endregion
        #region Constructor
        public ProductVersion()
        {
            this.ProductVersionItems = new List<ProductVersionItem>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdProductVersion")]
        [DataMember]
        public Int64 IdProductVersion
        {
            get
            {
                return idProductVersion;
            }

            set
            {
                idProductVersion = value;
                OnPropertyChanged("IdProductVersion");
            }
        }

        [Column("IdProduct")]
        [ForeignKey("Product")]
        [DataMember]
        public Int64 IdProduct
        {
            get
            {
                return idProduct;
            }

            set
            {
                idProduct = value;
                OnPropertyChanged("IdProduct");
            }
        }

        [Column("ProductVersionNumber")]
        [DataMember]
        public string ProductVersionNumber
        {
            get
            {
                return productVersionNumber;
            }

            set
            {
                productVersionNumber = value;
                OnPropertyChanged("ProductVersionNumber");
            }
        }

        [Column("IdCreator")]
        [ForeignKey("Creator")]
        [DataMember]
        public Int32 IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [Column("CreatedDate")]
        [DataMember]
        public DateTime? CreatedDate
        {
            get
            {
                return createdDate;
            }

            set
            {
                createdDate = value;
                OnPropertyChanged("CreatedDate");
            }
        }

        [Column("VersionBetaDate")]
        [DataMember]
        public DateTime? VersionBetaDate
        {
            get
            {
                return versionBetaDate;
            }

            set
            {
                versionBetaDate = value;
                OnPropertyChanged("VersionBetaDate");
            }
        }

        [Column("VersionReleaseDate")]
        [DataMember]
        public DateTime? VersionReleaseDate
        {
            get
            {
                return versionReleaseDate;
            }

            set
            {
                versionReleaseDate = value;
                OnPropertyChanged("VersionReleaseDate");
            }
        }

        [DataMember]
        public virtual User Creator
        {
            get
            {
                return creator;
            }

            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }

        [DataMember]
        public virtual Product Product
        {
            get
            {
                return product;
            }

            set
            {
                product = value;
                OnPropertyChanged("Product");
            }
        }

        [DataMember]
        public virtual IList<ProductVersionItem> ProductVersionItems
        {
            get
            {
                return productVersionItems;
            }

            set
            {
                productVersionItems = value;
                OnPropertyChanged("ProductVersionItems");
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
