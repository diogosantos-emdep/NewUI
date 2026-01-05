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
    [Table("RevisionItem")]
    [DataContract]
    public class RevisionItem : ModelBase, IDisposable
    {
        #region Fields

        bool validated;
        decimal unitPrice;
        decimal quantity;
        Int16 obsolete;
        string numItem;
        DateTime? modifiedIn;
        Int32 modifiedBy;
        byte marked;
        bool manualPrice;
        string internalComment;
        Int64 idRevisionItem;
        Int64 idRevision;
        Int64 idProduct;
        string customerComment;
        DateTime? createdIn;
        Int32 createdBy;
        String attachedFiles;
        WarehouseProduct warehouseProduct;
        Revision revision;

        Int64 downloadedQuantity;
        Int64 remainingQuantity;
        Int64 status;

        Int64? idDrawing;
        CpType cpType;
        Int32 ways;
        double sellPrice;
        string connectorFamily;
        string reference;
        CPProduct cpProduct;
        decimal downloadedQuantityDecimal;
        decimal remainingQuantityDecimal;
        #endregion

        #region Constructor
        public RevisionItem()
        {
        }

        #endregion

        #region Properties

        [Column("Validated")]
        [DataMember]
        public bool Validated
        {
            get
            {
                return validated;
            }

            set
            {
                validated = value;
                OnPropertyChanged("Validated");
            }
        }

        [Column("UnitPrice")]
        [DataMember]
        public decimal UnitPrice
        {
            get
            {
                return unitPrice;
            }

            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }

        [Column("Quantity")]
        [DataMember]
        public decimal Quantity
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

        [Column("Obsolete")]
        [DataMember]
        public Int16 Obsolete
        {
            get
            {
                return obsolete;
            }

            set
            {
                obsolete = value;
                OnPropertyChanged("Obsolete");
            }
        }

        [Column("NumItem")]
        [DataMember]
        public string NumItem
        {
            get
            {
                return numItem;
            }

            set
            {
                numItem = value;
                OnPropertyChanged("NumItem");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("Marked")]
        [DataMember]
        public byte Marked
        {
            get
            {
                return marked;
            }

            set
            {
                marked = value;
                OnPropertyChanged("Marked");
            }
        }

        [Column("ManualPrice")]
        [DataMember]
        public bool ManualPrice
        {
            get
            {
                return manualPrice;
            }

            set
            {
                manualPrice = value;
                OnPropertyChanged("ManualPrice");
            }
        }

        [Column("InternalComment")]
        [DataMember]
        public string InternalComment
        {
            get
            {
                return internalComment;
            }

            set
            {
                internalComment = value;
                OnPropertyChanged("InternalComment");
            }
        }

        [Key]
        [Column("IdRevisionItem")]
        [DataMember]
        public Int64 IdRevisionItem
        {
            get
            {
                return idRevisionItem;
            }

            set
            {
                idRevisionItem = value;
                OnPropertyChanged("IdRevisionItem");
            }
        }

        [Key]
        [Column("IdRevision")]
        [DataMember]
        public Int64 IdRevision
        {
            get
            {
                return idRevision;
            }

            set
            {
                idRevision = value;
                OnPropertyChanged("IdRevision");
            }
        }

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

        [Column("CustomerComment")]
        [DataMember]
        public string CustomerComment
        {
            get
            {
                return customerComment;
            }

            set
            {
                customerComment = value;
                OnPropertyChanged("CustomerComment");
            }
        }

        [Column("CreatedIn")]
        [DataMember]
        public DateTime? CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("AttachedFiles")]
        [DataMember]
        public string AttachedFiles
        {
            get
            {
                return attachedFiles;
            }

            set
            {
                attachedFiles = value;
                OnPropertyChanged("AttachedFiles");
            }
        }

        [NotMapped]
        [DataMember]
        public WarehouseProduct WarehouseProduct
        {
            get
            {
                return warehouseProduct;
            }

            set
            {
                warehouseProduct = value;
                OnPropertyChanged("WarehouseProduct");
            }
        }


        [NotMapped]
        [DataMember]
        public Revision Revision
        {
            get
            {
                return revision;
            }

            set
            {
                revision = value;
                OnPropertyChanged("Revision");
            }
        }

        [NotMapped]
        [DataMember]
        public long DownloadedQuantity
        {
            get
            {
                return downloadedQuantity;
            }

            set
            {
                downloadedQuantity = value;
                OnPropertyChanged("DownloadedQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public long RemainingQuantity
        {
            get
            {
                return remainingQuantity;
            }

            set
            {
                remainingQuantity = value;
                OnPropertyChanged("RemainingQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public long Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [NotMapped]
        [DataMember]
        public long? IdDrawing
        {
            get { return idDrawing; }
            set
            {
                idDrawing = value;
                OnPropertyChanged("IdDrawing");
            }
        }

        [NotMapped]
        [DataMember]
        public CpType CpType
        {
            get
            {
                return cpType;
            }

            set
            {
                cpType = value;
                OnPropertyChanged("CpType");
            }
        }

        [NotMapped]
        [DataMember]
        public int Ways
        {
            get { return ways; }
            set
            {
                ways = value;
                OnPropertyChanged("Ways");
            }
        }

        [NotMapped]
        [DataMember]
        public double SellPrice
        {
            get { return sellPrice; }
            set
            {
                sellPrice = value;
                OnPropertyChanged("SellPrice");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectorFamily
        {
            get { return connectorFamily; }
            set
            {
                connectorFamily = value;
                OnPropertyChanged("ConnectorFamily");
            }
        }

        [NotMapped]
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }


        [NotMapped]
        [DataMember]
        public CPProduct CPProduct
        {
            get { return cpProduct; }
            set
            {
                cpProduct = value;
                OnPropertyChanged("CPProduct");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal DownloadedQuantityDecimal
        {
            get { return downloadedQuantityDecimal; }
            set
            {
                downloadedQuantityDecimal = value;
                OnPropertyChanged("DownloadedQuantityDecimal");
            }
        }


        [NotMapped]
        [DataMember]
        public decimal RemainingQuantityDecimal
        {
            get { return remainingQuantityDecimal; }
            set
            {
                remainingQuantityDecimal = value;
                OnPropertyChanged("RemainingQuantityDecimal");
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
