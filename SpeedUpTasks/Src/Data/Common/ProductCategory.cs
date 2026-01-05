using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace Emdep.Geos.Data.Common
{
    [Table("product_categories")]
    [DataContract]
    public class ProductCategory : ModelBase, IDisposable
    {
        #region Fields

        Int64 idProductCategory;
        string name;
        Int64 idParent;
        Int32 level;
        ProductCategory category;
        Int64 position;
        string mergedCategoryAndProduct;
        private bool isDisabled;
        #endregion

        #region Constructor
        public ProductCategory()
        {
           
        }

        #endregion


        #region Properties

        [Key]
        [Column("IdProductCategory")]
        [DataMember]
        public Int64 IdProductCategory
        {
            get { return idProductCategory; }
            set
            {
                idProductCategory = value;
                OnPropertyChanged("IdProductCategory");
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

        [Column("IdParent")]
        [DataMember]
        public Int64 IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }


        [Column("Level")]
        [DataMember]
        public Int32 Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
        }

       
        [NotMapped]
        [DataMember]
        public ProductCategory Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [NotMapped]
        [DataMember]
        public string MergedCategoryAndProduct
        {
            get { return mergedCategoryAndProduct; }
            set
            {
                mergedCategoryAndProduct = value;
                OnPropertyChanged("MergedCategoryAndProduct");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsDisabled
        {
            get { return isDisabled; }
            set
            {
                isDisabled = value;
                OnPropertyChanged("IsDisabled");
            }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

      
        #endregion
    }
}
