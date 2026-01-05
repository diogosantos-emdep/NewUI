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
    [DataContract(IsReference = true)]
    [Table("products")]
    public class Product : ModelBase,IDisposable
    {
        #region Fields
        Int64 idProduct;
        string productName;
        string description;
        Int64? idParent;
        string htmlColor;
        IList<Product> childrens;
        Product parentProduct;
        IList<ProductRoadmap> productRoadmaps;
        IList<ProductVersion> productVersions;
        #endregion

        #region Constructor
        public Product()
        {
           this.ProductRoadmaps = new List<ProductRoadmap>();
            this.ProductVersions = new List<ProductVersion>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdProduct")]
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

        [Column("ProductName")]
        [DataMember]
        public string ProductName
        {
            get
            {
                return productName;
            }

            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
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

        [Column("HtmlColor")]
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

        [ForeignKey("ParentProduct")]
        [Column("IdParent")]
        [DataMember]
        public Int64? IdParent
        {
            get
            {
                return idParent;
            }

            set
            {
                idParent = value;
                 OnPropertyChanged("IdParent");
            }
        }

         [DataMember]
        public virtual IList<Product> Childrens
        {
            get
            {
                return childrens;
            }

            set
            {
                childrens = value;
                OnPropertyChanged("Childrens");
            }
        }

         [DataMember]
         public virtual Product ParentProduct
         {
             get
             {
                 return parentProduct;
             }

             set
             {
                 parentProduct = value;
                 OnPropertyChanged("ParentProduct");
             }
         }


         [DataMember]
         public virtual IList<ProductRoadmap> ProductRoadmaps
        {
             get
             {
                 return productRoadmaps;
             }

             set
             {
                productRoadmaps = value;
                 OnPropertyChanged("ProductRoadmaps");
             }
         }

        [DataMember]
        public virtual IList<ProductVersion> ProductVersions
        {
            get
            {
                return productVersions;
            }

            set
            {
                productVersions = value;
                OnPropertyChanged("ProductVersions");
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
