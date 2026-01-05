using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class WarehousePurchaseOrderExpectedItem : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idWarehousePurchaseOrderItem;
        Int64 quantity;
        DateTime date;

        #endregion

        #region Properties

        [Column("IdWarehousePurchaseOrderItem")]
        [DataMember]
        public Int64 IdWarehousePurchaseOrderItem
        {
            get { return idWarehousePurchaseOrderItem; }
            set
            {
                idWarehousePurchaseOrderItem = value;
                OnPropertyChanged("IdWarehousePurchaseOrderItem");
            }
        }

        [Column("Quantity")]
        [DataMember]
        public long Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [Column("Date")]
        [DataMember]
        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        #endregion

        #region Constructor

        public WarehousePurchaseOrderExpectedItem()
        {
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
