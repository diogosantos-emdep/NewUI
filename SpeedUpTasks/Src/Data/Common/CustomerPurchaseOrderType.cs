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
    [Table("customerpurchaseordertypes")]
    [DataContract]
    public class CustomerPurchaseOrderType : ModelBase, IDisposable
    {
        #region  Fields
        Int32 idPOType;
        string poType;
         #endregion

        #region Constructor
        public CustomerPurchaseOrderType()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("idPOType")]

        [DataMember]
        public Int32 IdPOType
        {
            get
            {
                return idPOType;
            }

            set
            {
                idPOType = value;
                OnPropertyChanged("IdPOType");
            }
        }

        [Column("POType")]
        [DataMember]
        public string POType
        {
            get
            {
                return poType;
            }

            set
            {
                poType = value;
                OnPropertyChanged("POType");
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
