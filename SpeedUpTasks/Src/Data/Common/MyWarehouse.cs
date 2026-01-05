using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("mywarehouse")]
    [DataContract]
    public class MyWarehouse : ModelBase, IDisposable
    {
        #region Declaration

        Int32 idArticle;
        string location;
        Int64 minimumStock;
        Int64 maximumStock;
        double basePrice;
        Int64 quantityPurchaseOrder;
        Int64 idWarehouse;
        Int32? modifiedBy;
        DateTime? modifiedIn;
        string internalNotes;
        Int64 currentStock;
        Warehouses warehouse;

        #endregion

        #region Properties

        [Column("IdArticle")]
        [DataMember]
        public int IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [Column("Location")]
        [DataMember]
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        [Column("MinimumStock")]
        [DataMember]
        public long MinimumStock
        {
            get { return minimumStock; }
            set
            {
                minimumStock = value;
                OnPropertyChanged("MinimumStock");
            }
        }

        [Column("MaximumStock")]
        [DataMember]
        public long MaximumStock
        {
            get { return maximumStock; }
            set
            {
                maximumStock = value;
                OnPropertyChanged("MaximumStock");
            }
        }

        [Column("BasePrice")]
        [DataMember]
        public double BasePrice
        {
            get { return basePrice; }
            set
            {
                basePrice = value;
                OnPropertyChanged("BasePrice");
            }
        }

        [Column("QuantityPurchaseOrder")]
        [DataMember]
        public long QuantityPurchaseOrder
        {
            get { return quantityPurchaseOrder; }
            set
            {
                quantityPurchaseOrder = value;
                OnPropertyChanged("QuantityPurchaseOrder");
            }
        }

        [Column("IdWarehouse")]
        [DataMember]
        public long IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public int? ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("InternalNotes")]
        [DataMember]
        public string InternalNotes
        {
            get { return internalNotes; }
            set
            {
                internalNotes = value;
                OnPropertyChanged("InternalNotes");
            }
        }

        [Column("CurrentStock")]
        [DataMember]
        public long CurrentStock
        {
            get { return currentStock; }
            set
            {
                currentStock = value;
                OnPropertyChanged("CurrentStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Warehouses Warehouse
        {
            get { return warehouse; }
            set
            {
                warehouse = value;
                OnPropertyChanged("Warehouse");
            }
        }

        #endregion

        #region Constructor

        public MyWarehouse()
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
            MyWarehouse myWarehouse = (MyWarehouse)this.MemberwiseClone();
            return myWarehouse;
        }

        #endregion
    }
}
