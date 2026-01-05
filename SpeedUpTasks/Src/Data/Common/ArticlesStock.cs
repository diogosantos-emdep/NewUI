using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("articlesstock")]
    [DataContract]
    public class ArticlesStock : ModelBase, IDisposable
    {
        #region Declaration

        UInt64 idArticleStock;
        UInt32 idArticle;
        Int64 quantity;
        double price;
        DateTime uploadedIn;
        string comments;
        UInt32? modifiedBy;
        Int64? idSupplierComplaintItem;
        byte isDamaged;
        UInt64? idOtItem;
        UInt64? idArticleStockGroup;
        Int16 idCurrency;
        UInt64? idWarehouseProductComponent;
        double unitPrice;
        UInt64 idWarehouseDeliveryNoteItem;
        byte regularizationPoint;
        UInt32? idOperator;
        byte isBroken;
        byte isWaste;
        byte isLost;
        Int32? idWorkstation;
        UInt64? idWarehouse;
        UInt64? idWarehouseLocation;
        People peopleModifiedBy;
        WarehouseDeliveryNoteItem warehouseDeliveryNoteItem;
        WarehouseLocation warehouseLocation;
        RevisionItem revisionItem;
        List<SerialNumber> serialNumbers;
        #endregion

        #region Properties

        [Key]
        [Column("IdArticleStock")]
        [DataMember]
        public ulong IdArticleStock
        {
            get { return idArticleStock; }
            set
            {
                idArticleStock = value;
                OnPropertyChanged("IdArticleStock");
            }
        }

        [Column("IdArticle")]
        [DataMember]
        public uint IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [Column("Quantity")]
        [DataMember]
        public long Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [Column("Price")]
        [DataMember]
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        [Column("UploadedIn")]
        [DataMember]
        public DateTime UploadedIn
        {
            get { return uploadedIn; }
            set
            {
                uploadedIn = value;
                OnPropertyChanged("UploadedIn");
            }
        }

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public uint? ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("IdSupplierComplaintItem")]
        [DataMember]
        public long? IdSupplierComplaintItem
        {
            get { return idSupplierComplaintItem; }
            set
            {
                idSupplierComplaintItem = value;
                OnPropertyChanged("IdSupplierComplaintItem");
            }
        }

        [Column("IsDamaged")]
        [DataMember]
        public byte IsDamaged
        {
            get { return isDamaged; }
            set
            {
                isDamaged = value;
                OnPropertyChanged("IsDamaged");
            }
        }

        [Column("IdOtItem")]
        [DataMember]
        public ulong? IdOtItem
        {
            get { return idOtItem; }
            set
            {
                idOtItem = value;
                OnPropertyChanged("IdOtItem");
            }
        }

        [Column("IdArticleStockGroup")]
        [DataMember]
        public ulong? IdArticleStockGroup
        {
            get { return idArticleStockGroup; }
            set
            {
                idArticleStockGroup = value;
                OnPropertyChanged("IdArticleStockGroup");
            }
        }

        [Column("IdCurrency")]
        [DataMember]
        public short IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [Column("IdWarehouseProductComponent")]
        [DataMember]
        public ulong? IdWarehouseProductComponent
        {
            get { return idWarehouseProductComponent; }
            set
            {
                idWarehouseProductComponent = value;
                OnPropertyChanged("IdWarehouseProductComponent");
            }
        }

        [Column("UnitPrice")]
        [DataMember]
        public double UnitPrice
        {
            get { return unitPrice; }
            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }

        [Column("IdWarehouseDeliveryNoteItem")]
        [DataMember]
        public ulong IdWarehouseDeliveryNoteItem
        {
            get { return idWarehouseDeliveryNoteItem; }
            set
            {
                idWarehouseDeliveryNoteItem = value;
                OnPropertyChanged("IdWarehouseDeliveryNoteItem");
            }
        }

        [Column("RegularizationPoint")]
        [DataMember]
        public byte RegularizationPoint
        {
            get { return regularizationPoint; }
            set
            {
                regularizationPoint = value;
                OnPropertyChanged("RegularizationPoint");
            }
        }

        [Column("IdOperator")]
        [DataMember]
        public uint? IdOperator
        {
            get { return idOperator; }
            set
            {
                idOperator = value;
                OnPropertyChanged("IdOperator");
            }
        }

        [Column("IsBroken")]
        [DataMember]
        public byte IsBroken
        {
            get { return isBroken; }
            set
            {
                isBroken = value;
                OnPropertyChanged("IsBroken");
            }
        }

        [Column("IsWaste")]
        [DataMember]
        public byte IsWaste
        {
            get { return isWaste; }
            set
            {
                isWaste = value;
                OnPropertyChanged("IsWaste");
            }
        }

        [Column("IsLost")]
        [DataMember]
        public byte IsLost
        {
            get { return isLost; }
            set
            {
                isLost = value;
                OnPropertyChanged("IsLost");
            }
        }

        [Column("IdWorkstation")]
        [DataMember]
        public int? IdWorkstation
        {
            get { return idWorkstation; }
            set
            {
                idWorkstation = value;
                OnPropertyChanged("IdWorkstation");
            }
        }

        [Column("IdWarehouse")]
        [DataMember]
        public ulong? IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [Column("IdWarehouseLocation")]
        [DataMember]
        public ulong? IdWarehouseLocation
        {
            get { return idWarehouseLocation; }
            set
            {
                idWarehouseLocation = value;
                OnPropertyChanged("IdWarehouseLocation");
            }
        }

        [NotMapped]
        [DataMember]
        public People PeopleModifiedBy
        {
            get { return peopleModifiedBy; }
            set
            {
                peopleModifiedBy = value;
                OnPropertyChanged("PeopleModifiedBy");
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


        [NotMapped]
        [DataMember]
        public WarehouseLocation WarehouseLocation
        {
            get { return warehouseLocation; }
            set
            {
                warehouseLocation = value;
                OnPropertyChanged("WarehouseLocation");
            }
        }

        [NotMapped]
        [DataMember]
        public RevisionItem RevisionItem
        {
            get { return revisionItem; }
            set
            {
                revisionItem = value;
                OnPropertyChanged("RevisionItem");
            }
        }

        [NotMapped]
        [DataMember]
        public List<SerialNumber> SerialNumbers
        {
            get { return serialNumbers; }
            set
            {
                serialNumbers = value;
                OnPropertyChanged("SerialNumbers");
            }
        }

        #endregion


        #region Constructor

        public ArticlesStock()
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
