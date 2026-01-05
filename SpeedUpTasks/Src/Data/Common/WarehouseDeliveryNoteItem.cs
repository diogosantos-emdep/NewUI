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
    [Table("warehousedeliverynoteitems")]
    [DataContract]
    public class WarehouseDeliveryNoteItem : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idWarehouseDeliveryNoteItem;
        Int64 idWarehousePurchaseOrderItem;
        Int32 quantity;
        Int32 maxQuantity;
        Int64 idWarehouseDeliveryNote;
        byte uploaded;
        double customsDuty;
        Int64 idSupplierComplaintItem;
        Int64 idManufacturerByArticle;

        WarehousePurchaseOrderItem warehousePurchaseOrderItem;
        ArticleBySupplier articleBySupplier;
        List<ManufacturersByArticle> manufacturersByArticle;
        List<SerialNumber> serialNumbers;
        Int64? prevDNIdManufacturerByArticle;
        ManufacturersByArticle producer;

        ArticlesStock articlesStock;
        WarehouseDeliveryNote warehouseDeliveryNote;
        #endregion

        #region Properties

        [Key]
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

        [Column("IdWarehousePurchaseOrderItem")]
        [DataMember]
        public long IdWarehousePurchaseOrderItem
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
        public Int32 Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [Column("MaxQuantity")]
        [DataMember]
        public Int32 MaxQuantity
        {
            get { return maxQuantity; }
            set
            {
                maxQuantity = value;
                OnPropertyChanged("MaxQuantity");
            }
        }

        [Column("IdWarehouseDeliveryNote")]
        [DataMember]
        public long IdWarehouseDeliveryNote
        {
            get { return idWarehouseDeliveryNote; }
            set
            {
                idWarehouseDeliveryNote = value;
                OnPropertyChanged("IdWarehouseDeliveryNote");
            }
        }

        [Column("Uploaded")]
        [DataMember]
        public byte Uploaded
        {
            get { return uploaded; }
            set
            {
                uploaded = value;
                OnPropertyChanged("Uploaded");
            }
        }

        [Column("CustomsDuty")]
        [DataMember]
        public double CustomsDuty
        {
            get { return customsDuty; }
            set
            {
                customsDuty = value;
                OnPropertyChanged("CustomsDuty");
            }
        }

        [Column("IdSupplierComplaintItem")]
        [DataMember]
        public long IdSupplierComplaintItem
        {
            get { return idSupplierComplaintItem; }
            set
            {
                idSupplierComplaintItem = value;
                OnPropertyChanged("IdSupplierComplaintItem");
            }
        }

        [Column("IdManufacturerByArticle")]
        [DataMember]
        public long IdManufacturerByArticle
        {
            get { return idManufacturerByArticle; }
            set
            {
                idManufacturerByArticle = value;
                OnPropertyChanged("IdManufacturerByArticle");
            }
        }

        [NotMapped]
        [DataMember]
        public WarehousePurchaseOrderItem WarehousePurchaseOrderItem
        {
            get { return warehousePurchaseOrderItem; }
            set
            {
                warehousePurchaseOrderItem = value;
                OnPropertyChanged("WarehousePurchaseOrderItem");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ManufacturersByArticle> ManufacturersByArticle
        {
            get { return manufacturersByArticle; }
            set
            {
                manufacturersByArticle = value;
                OnPropertyChanged("ManufacturersByArticle");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleBySupplier ArticleBySupplier
        {
            get { return articleBySupplier; }
            set
            {
                articleBySupplier = value;
                OnPropertyChanged("ArticleBySupplier");
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

        [NotMapped]
        [DataMember]
        public long? PrevDNIdManufacturerByArticle
        {
            get { return prevDNIdManufacturerByArticle; }
            set
            {
                prevDNIdManufacturerByArticle = value;
                OnPropertyChanged("SerialNumbers");
            }
        }

        [NotMapped]
        [DataMember]
        public ManufacturersByArticle Producer
        {
            get { return producer; }
            set
            {
                producer = value;
                OnPropertyChanged("Producer");
            }
        }

        /// <summary>
        /// It is used to download upload stock from view delivery note.
        /// </summary>
        [NotMapped]
        [DataMember]
        public ArticlesStock ArticlesStock
        {
            get { return articlesStock; }
            set
            {
                articlesStock = value;
                OnPropertyChanged("ArticlesStock");
            }
        }

        [NotMapped]
        [DataMember]
        public WarehouseDeliveryNote WarehouseDeliveryNote
        {
            get { return warehouseDeliveryNote; }
            set
            {
                warehouseDeliveryNote = value;
                OnPropertyChanged("WarehouseDeliveryNote");
            }
        }

        #endregion

        #region Constructor

        public WarehouseDeliveryNoteItem()
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
