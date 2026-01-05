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
    [Table("sites_by_businessproduct")]
    [DataContract]
    public class SitesByBusinessProduct : ModelBase, IDisposable
    {
        #region Fields
        Int32 idSite;
        Int32 idBusinessProduct;
        string businessProduct;
        bool isDeleted;
        bool isAdded;
        #endregion

        #region Constructor
        public SitesByBusinessProduct()
        {

        }
        #endregion

        #region Properties
        [Key]
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

        [Column("IdBusinessProduct")]
        [DataMember]
        public Int32 IdBusinessProduct
        {
            get
            {
                return idBusinessProduct;
            }

            set
            {
                idBusinessProduct = value;
                OnPropertyChanged("IdBusinessProduct");
            }
        }

        [NotMapped]
        [DataMember]
        public string BusinessProduct
        {
            get
            {
                return businessProduct;
            }

            set
            {
                businessProduct = value;
                OnPropertyChanged("BusinessProduct");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsAdded
        {
            get
            {
                return isAdded;
            }

            set
            {
                isAdded = value;
                OnPropertyChanged("IsAdded");
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
