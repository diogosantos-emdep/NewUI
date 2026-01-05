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
    [Table("product_version_items")]
    [DataContract(IsReference =true)]
    public class ProductVersionItem : ModelBase,IDisposable
    {
        #region Fields
        Int64 idProductVersionItem;
        Int64 idProductVersion;
        Int64 idProductRoadmap;
        ProductRoadmap productRoadmap;
        ProductVersion productVersion;
        IList<ProductVersionValidation> productVersionValidations;
        #endregion

        #region Constructor
        public ProductVersionItem()
        {
            this.ProductVersionValidations = new List<ProductVersionValidation>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdProductVersionItem")]
        [DataMember]
        public Int64 IdProductVersionItem
        {
            get
            {
                return idProductVersionItem;
            }

            set
            {
                idProductVersionItem = value;
                OnPropertyChanged("IdProductVersionItem");
            }
        }

        [Column("IdProductVersion")]
        [ForeignKey("ProductVersion")]
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

        [Column("IdProductRoadmap")]
        [ForeignKey("ProductRoadmap")]
        [DataMember]
        public Int64 IdProductRoadmap
        {
            get
            {
                return idProductRoadmap;
            }

            set
            {
                idProductRoadmap = value;
                OnPropertyChanged("IdProductRoadmap");
            }
        }

        [DataMember]
        public virtual ProductRoadmap ProductRoadmap
        {
            get
            {
                return productRoadmap;
            }

            set
            {
                productRoadmap = value;
                OnPropertyChanged("ProductRoadmap");
            }
        }

        [DataMember]
        public virtual ProductVersion ProductVersion
        {
            get
            {
                return productVersion;
            }

            set
            {
                productVersion = value;
                OnPropertyChanged("ProductVersion");
            }
        }

        [DataMember]
        public virtual IList<ProductVersionValidation> ProductVersionValidations
        {
            get
            {
                return productVersionValidations;
            }

            set
            {
                productVersionValidations = value;
                OnPropertyChanged("ProductVersionValidations");
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
