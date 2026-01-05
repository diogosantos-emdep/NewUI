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
    [Table("warehousepurchaseorderitems")]
    [DataContract]
    public class WarehousePurchaseOrderItem : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idWarehousePurchaseOrderItem;
        Int32 idArticle;
        Article article;
        string description;
        double quantity;
        double receivedQuantity;
        double unitPrice;
        string additionalComments;
        Int64 position;
        Int64 idWarehousePurchaseOrder;
        double discount;
        double iva;
        Int64? idOt;
        DateTime? expectedDate;

        List<WarehouseDeliveryNote> receivedDeliveryNotes;
        List<WarehousePurchaseOrderExpectedItem> warehousePurchaseOrderExpectedItems;
        Int32 status;

        decimal totalAmount;
        #endregion

        #region Properties

        [Key]
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

        [Column("IdArticle")]
        [DataMember]
        public Int32 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("Quantity")]
        [DataMember]
        public double Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
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

        [Column("AdditionalComments")]
        [DataMember]
        public string AdditionalComments
        {
            get { return additionalComments; }
            set
            {
                additionalComments = value;
                OnPropertyChanged("AdditionalComments");
            }
        }

        [Column("Position")]
        [DataMember]
        public Int64 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [Column("IdWarehousePurchaseOrder")]
        [DataMember]
        public Int64 IdWarehousePurchaseOrder
        {
            get { return idWarehousePurchaseOrder; }
            set
            {
                idWarehousePurchaseOrder = value;
                OnPropertyChanged("IdWarehousePurchaseOrder");
            }
        }

        [Column("Discount")]
        [DataMember]
        public double Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                OnPropertyChanged("Discount");
            }
        }

        [Column("IVA")]
        [DataMember]
        public double IVA
        {
            get { return iva; }
            set
            {
                iva = value;
                OnPropertyChanged("IVA");
            }
        }

        [Column("IdOt")]
        [DataMember]
        public Int64? IdOt
        {
            get { return idOt; }
            set
            {
                idOt = value;
                OnPropertyChanged("IdOt");
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

        [NotMapped]
        [DataMember]
        public double ReceivedQuantity
        {
            get { return receivedQuantity; }
            set
            {
                receivedQuantity = value;
                OnPropertyChanged("ReceivedQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? ExpectedDate
        {
            get { return expectedDate; }
            set
            {
                expectedDate = value;
                OnPropertyChanged("ExpectedDate");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WarehouseDeliveryNote> ReceivedDeliveryNotes
        {
            get { return receivedDeliveryNotes; }
            set
            {
                receivedDeliveryNotes = value;
                OnPropertyChanged("ReceivedDeliveryNotes");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WarehousePurchaseOrderExpectedItem> WarehousePurchaseOrderExpectedItems
        {
            get { return warehousePurchaseOrderExpectedItems; }
            set
            {
                warehousePurchaseOrderExpectedItems = value;
                OnPropertyChanged("WarehousePurchaseOrderExpectedItems");
            }
        }

        [NotMapped]
        [DataMember]
        public int Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }


        [NotMapped]
        [DataMember]
        public decimal TotalAmount
        {
            get
            {
                return totalAmount;
            }

            set
            {
                totalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }
        #endregion

        #region Constructor

        public WarehousePurchaseOrderItem()
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
