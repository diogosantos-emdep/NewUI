using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("warehouse_inventory_audits")]
    [DataContract]
    public class WarehouseInventoryAudit : ModelBase, IDisposable
    {
        #region Fields

        Int64 idWarehouseInventoryAudit;
        string name;
        Int64 idWarehouse;
        DateTime? startDate;
        DateTime? endDate;
        Int32 idCreator;
        User creator;
        DateTime creationDate;
        Int32? idModifier;
        User modifier;
        DateTime? modificationDate;

        // Additional Properties
        Int64 totalItems;
        Int64 totalLocations;
        Int64 okItems;
        Int64 nokItems;
        byte successRate;
        Int64 balanceAmount;

        #endregion

        #region Properties

        [Column("IdWarehouseInventoryAudit")]
        [DataMember]
        public long IdWarehouseInventoryAudit
        {
            get { return idWarehouseInventoryAudit; }
            set
            {
                idWarehouseInventoryAudit = value;
                OnPropertyChanged("IdWarehouseInventoryAudit");
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

        [Column("StartDate")]
        [DataMember]
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Column("EndDate")]
        [DataMember]
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [Column("IdCreator")]
        [DataMember]
        public int IdCreator
        {
            get { return idCreator; }
            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [Column("Creator")]
        [DataMember]
        public User Creator
        {
            get { return creator; }
            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [Column("IdModifier")]
        [DataMember]
        public int? IdModifier
        {
            get { return idModifier; }
            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [Column("Modifier")]
        [DataMember]
        public User Modifier
        {
            get { return modifier; }
            set
            {
                modifier = value;
                OnPropertyChanged("Modifier");
            }
        }

        [Column("ModificationDate")]
        [DataMember]
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [NotMapped]
        [DataMember]
        public long TotalItems
        {
            get { return totalItems; }
            set
            {
                totalItems = value;
                OnPropertyChanged("TotalItems");
            }
        }

        [NotMapped]
        [DataMember]
        public long TotalLocations
        {
            get { return totalLocations; }
            set
            {
                totalLocations = value;
                OnPropertyChanged("TotalLocations");
            }
        }

        [NotMapped]
        [DataMember]
        public long OKItems
        {
            get { return okItems; }
            set
            {
                okItems = value;
                OnPropertyChanged("OKItems");
            }
        }

        [NotMapped]
        [DataMember]
        public long NOKItems
        {
            get { return nokItems; }
            set
            {
                nokItems = value;
                OnPropertyChanged("NOKItems");
            }
        }

        [NotMapped]
        [DataMember]
        public byte SuccessRate
        {
            get { return successRate; }
            set
            {
                successRate = value;
                OnPropertyChanged("SuccessRate");
            }
        }

        [NotMapped]
        [DataMember]
        public long BalanceAmount
        {
            get { return balanceAmount; }
            set
            {
                balanceAmount = value;
                OnPropertyChanged("BalanceAmount");
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
