using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("warehouse_inventory_audit_items")]
    [DataContract]
    public class WarehouseInventoryAuditItem : ModelBase, IDisposable
    {

        #region Fields

        Int64 idWarehouseInventoryAuditItem;
        Int64 idWarehouseInventoryAudit;
        Int64 idWarehouseLocation;
        Int32 idArticle;
        Article article;
        Int64 idWarehouseDeliveryNoteItem;
        WarehouseDeliveryNoteItem warehouseDeliveryNoteItem;
        Int64 expectedQuantity;
        Int64 currentQuantity;
        byte isOK;
        Int32 idReason;
        LookupValue reason;
        Int32 idReporter;
        User reporter;
        Int32 idApprover;
        User approver;
        Int32 idCreator;
        User creator;
        DateTime creationDate;
        Int32? idModifier;
        User modifier;
        DateTime? modificationDate;

        [Column("IdWarehouseInventoryAuditItem")]
        [DataMember]
        public long IdWarehouseInventoryAuditItem
        {
            get { return idWarehouseInventoryAuditItem; }
            set
            {
                idWarehouseInventoryAuditItem = value;
                OnPropertyChanged("IdWarehouseInventoryAuditItem");
            }
        }

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

        [Column("IdWarehouseLocation")]
        [DataMember]
        public long IdWarehouseLocation
        {
            get { return idWarehouseLocation; }
            set
            {
                idWarehouseLocation = value;
                OnPropertyChanged("IdWarehouseLocation");
            }
        }

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

        [NotMapped]
        [DataMember]
        public Article Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged("Article");
            }
        }

        [Column("IdWarehouseDeliveryNoteItem")]
        [DataMember]
        public long IdWarehouseDeliveryNoteItem
        {
            get { return idWarehouseDeliveryNoteItem; }
            set
            {
                idWarehouseDeliveryNoteItem = value;
                OnPropertyChanged("IdWarehouseDeliveryNoteItem");
            }
        }

        [NotMapped]
        [DataMember]
        public WarehouseDeliveryNoteItem WarehouseDeliveryNoteItem
        {
            get { return warehouseDeliveryNoteItem; }
            set
            {
                warehouseDeliveryNoteItem = value;
                OnPropertyChanged("WarehouseDeliveryNoteItem");
            }
        }

        [Column("ExpectedQuantity")]
        [DataMember]
        public long ExpectedQuantity
        {
            get { return expectedQuantity; }
            set
            {
                expectedQuantity = value;
                OnPropertyChanged("ExpectedQuantity");
            }
        }

        [Column("CurrentQuantity")]
        [DataMember]
        public long CurrentQuantity
        {
            get { return currentQuantity; }
            set
            {
                currentQuantity = value;
                OnPropertyChanged("CurrentQuantity");
            }
        }

        [Column("IsOK")]
        [DataMember]
        public byte IsOK
        {
            get { return isOK; }
            set
            {
                isOK = value;
                OnPropertyChanged("IsOK");
            }
        }

        [Column("IdReason")]
        [DataMember]
        public int IdReason
        {
            get { return idReason; }
            set
            {
                idReason = value;
                OnPropertyChanged("IdReason");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue Reason
        {
            get { return reason; }
            set
            {
                reason = value;
                OnPropertyChanged("Reason");
            }
        }

        [Column("IdReporter")]
        [DataMember]
        public int IdReporter
        {
            get { return idReporter; }
            set
            {
                idReporter = value;
                OnPropertyChanged("IdReporter");
            }
        }

        [NotMapped]
        [DataMember]
        public User Reporter
        {
            get { return reporter; }
            set
            {
                reporter = value;
                OnPropertyChanged("Reporter");
            }
        }

        [Column("IdApprover")]
        [DataMember]
        public int IdApprover
        {
            get { return idApprover; }
            set
            {
                idApprover = value;
                OnPropertyChanged("IdApprover");
            }
        }

        [NotMapped]
        [DataMember]
        public User Approver
        {
            get { return approver; }
            set
            {
                approver = value;
                OnPropertyChanged("Approver");
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

        [NotMapped]
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

        [NotMapped]
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
