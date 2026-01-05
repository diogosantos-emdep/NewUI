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
    [Table("product_category_offer_options")]
    [DataContract]
    public class ProductCategoryOfferOption : ModelBase, IDisposable
    {
        #region Fields

        Int64 idProductCategoryOfferOption;
        Int64 idProductCategory;
        Int64 idOfferOption;
        sbyte isPermitted;
       
        #endregion

        #region Constructor
        public ProductCategoryOfferOption()
        {

        }

        #endregion


        #region Properties

        [Key]
        [Column("IdProductCategoryOfferOption")]
        [DataMember]
        public Int64 IdProductCategoryOfferOption
        {
            get { return idProductCategoryOfferOption; }
            set
            {
                idProductCategoryOfferOption = value;
                OnPropertyChanged("IdProductCategoryOfferOption");
            }
        }

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

        [Column("IdOfferOption")]
        [DataMember]
        public Int64 IdOfferOption
        {
            get { return idOfferOption; }
            set
            {
               idOfferOption = value;
                OnPropertyChanged("IdOfferOption");
            }
        }


        [Column("IsPermitted")]
        [DataMember]
        public sbyte IsPermitted
        {
            get { return isPermitted; }
            set
            {
                isPermitted = value;
                OnPropertyChanged("IsPermitted");
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
