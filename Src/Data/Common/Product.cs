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
    [Table("Product")]
    [DataContract]
    class Product : ModelBase, IDisposable
    {

        byte idProductType;
        Int32 idProduct;

        [Column("IdProductType")]
        [DataMember]
        public byte IdProductType
        {
            get
            {
                return idProductType;
            }

            set
            {
                idProductType = value;
                OnPropertyChanged("IdProductType");
            }
        }
        [Key]
        [Column("IdProduct")]
        [DataMember]
        public Int32 IdProduct
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
