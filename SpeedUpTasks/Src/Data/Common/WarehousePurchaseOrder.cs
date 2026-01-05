using Emdep.Geos.Data.Common.SRM;
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
    [Table("warehousepurchaseorders")]
    [DataContract]
    public class WarehousePurchaseOrder : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idWarehousePurchaseOrder;
        string serie;
        Int64 number;
        string code;
        Int64 idArticleSupplier;
        ArticleSupplier articleSupplier;
        DateTime? deliveryDate;
        Int64 idPaymentType;
        Int32 idTransportAgency;
        string comments;
        DateTime createdIn;
        Int64 createdBy;
        DateTime modifiedIn;
        Int64 modifiedBy;
        Byte isClosed;
        Int64? idWarehouse;
        string reasonClosed;
        Byte idCurrency;
        Byte attachedPO;
        int? delay;

        //Not Mapped
        Int32 deliveries;
        DateTime? reminderEmailDate;
        DateTime? latestDeliveryDate;
        Int32 status;
        List<WarehousePurchaseOrderItem> warehousePurchaseOrderItems;
        List<WarehouseDeliveryNote> warehouseDeliveryNotes;
        bool isPartialPending;
        Warehouses warehouse;


        decimal totalAmount;
        string attachPdf;
        People creator;
        People modifier;

        List<LogEntriesByWarehousePO> warehousePOLogEntries;
        List<LogEntriesByWarehousePO> warehousePOComments;

        Currency currency;

        WorkflowStatus workflowStatus;
        byte idWorkflowStatus;

        string emailBody;
        byte[] attachmentBytes;
        #endregion

        #region Properties

       [Key]
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

        [Column("Serie")]
        [DataMember]
        public string Serie
        {
            get { return serie; }
            set
            {
                serie = value;
                OnPropertyChanged("Serie");
            }
        }

        [Column("Number")]
        [DataMember]
        public Int64 Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        [Column("Code")]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [Column("IdArticleSupplier")]
        [DataMember]
        public Int64 IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleSupplier ArticleSupplier
        {
            get { return articleSupplier; }
            set
            {
                articleSupplier = value;
                OnPropertyChanged("ArticleSupplier");
            }
        }

        [Column("DeliveryDate")]
        [DataMember]
        public DateTime? DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }

        [Column("IdPaymentType")]
        [DataMember]
        public Int64 IdPaymentType
        {
            get { return idPaymentType; }
            set
            {
                idPaymentType = value;
                OnPropertyChanged("IdPaymentType");
            }
        }

        [Column("IdTransportAgency")]
        [DataMember]
        public Int32 IdTransportAgency
        {
            get { return idTransportAgency; }
            set
            {
                idTransportAgency = value;
                OnPropertyChanged("IdTransportAgency");
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

        [Column("CreatedIn")]
        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int64 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int64 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("IsClosed")]
        [DataMember]
        public Byte IsClosed
        {
            get { return isClosed; }
            set
            {
                isClosed = value;
                OnPropertyChanged("IsClosed");
            }
        }

        [Column("IdWarehouse")]
        [DataMember]
        public Int64? IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [Column("ReasonClosed")]
        [DataMember]
        public string ReasonClosed
        {
            get { return reasonClosed; }
            set
            {
                reasonClosed = value;
                OnPropertyChanged("ReasonClosed");
            }
        }

        [Column("idCurrency")]
        [DataMember]
        public Byte IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [Column("AttachedPO")]
        [DataMember]
        public Byte AttachedPO
        {
            get { return attachedPO; }
            set
            {
                attachedPO = value;
                OnPropertyChanged("AttachedPO");
            }
        }

        [Column("ReminderEmailDate")]
        [DataMember]
        public DateTime? ReminderEmailDate
        {
            get { return reminderEmailDate; }
            set
            {
                reminderEmailDate = value;
                OnPropertyChanged("ReminderEmailDate");
            }
        }

        [NotMapped]
        [DataMember]
        public int? Delay
        {
            get { return delay; }
            set
            {
                delay = value;
                OnPropertyChanged("Delay");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 Deliveries
        {
            get { return deliveries; }
            set
            {
                deliveries = value;
                OnPropertyChanged("Deliveries");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? LatestDeliveryDate
        {
            get { return latestDeliveryDate; }
            set
            {
                latestDeliveryDate = value;
                OnPropertyChanged("LatestDeliveryDate");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 Status
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
        public List<WarehousePurchaseOrderItem> WarehousePurchaseOrderItems
        {
            get { return warehousePurchaseOrderItems; }
            set
            {
                warehousePurchaseOrderItems = value;
                OnPropertyChanged("WarehousePurchaseOrderItems");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WarehouseDeliveryNote> WarehouseDeliveryNotes
        {
            get { return warehouseDeliveryNotes; }
            set
            {
                warehouseDeliveryNotes = value;
                OnPropertyChanged("WarehouseDeliveryNotes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsPartialPending
        {
            get { return isPartialPending; }
            set
            {
                isPartialPending = value;
                OnPropertyChanged("IsPartialPending");
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

        [NotMapped]
        [DataMember]
        public string AttachPdf
        {
            get
            {
                return attachPdf;
            }

            set
            {
                attachPdf = value;
                OnPropertyChanged("AttachPdf");
            }
        }

        [NotMapped]
        [DataMember]
        public People Creator
        {
            get
            {
                return creator;
            }

            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }

        [NotMapped]
        [DataMember]
        public People Modifier
        {
            get
            {
                return modifier;
            }

            set
            {
                modifier = value;
                OnPropertyChanged("Modifier");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByWarehousePO> WarehousePOLogEntries
        {
            get
            {
                return warehousePOLogEntries;
            }

            set
            {
                warehousePOLogEntries = value;
                OnPropertyChanged("WarehousePOLogEntries");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByWarehousePO> WarehousePOComments
        {
            get
            {
                return warehousePOComments;
            }

            set
            {
                warehousePOComments = value;
                OnPropertyChanged("WarehousePOComments");
            }
        }

        [NotMapped]
        [DataMember]
        public Currency Currency
        {
            get
            {
                return currency;
            }

            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        [NotMapped]
        [DataMember]
        public WorkflowStatus WorkflowStatus
        {
            get
            {
                return workflowStatus;
            }

            set
            {
                workflowStatus = value;
                OnPropertyChanged("WorkflowStatus");
            }
        }
        [NotMapped]
        [DataMember]
        public byte IdWorkflowStatus
        {
            get
            {
                return idWorkflowStatus;
            }

            set
            {
                idWorkflowStatus = value;
                OnPropertyChanged("IdWorkflowStatus");
            }
        }
        [NotMapped]
        [DataMember]
        public string EmailBody
        {
            get
            {
                return emailBody;
            }

            set
            {
                emailBody = value;
                OnPropertyChanged("EmailBody");
            }
        }
        [NotMapped]
        [DataMember]
        public byte[] AttachmentBytes
        {
            get
            {
                return attachmentBytes;
            }

            set
            {
                attachmentBytes = value;
                OnPropertyChanged("AttachmentBytes");
            }
        }

        #endregion

        #region Constructor

        public WarehousePurchaseOrder()
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
