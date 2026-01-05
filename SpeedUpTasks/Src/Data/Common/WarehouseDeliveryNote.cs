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
    [Table("warehousedeliverynotes")]
    [DataContract]
    public class WarehouseDeliveryNote : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idWarehouseDeliveryNote;
        Int64 number;
        string code;
        DateTime deliveryDate;
        DateTime createdIn;
        Int32 createdBy;
        DateTime modifiedIn;
        Int32 modifiedBy;
        Int64 idWarehousePurchaseOrder;
        WarehousePurchaseOrder warehousePurchaseOrder;
        string supplierCode;
        byte idCurrency;
        double exchangeRate;
        byte attachedDeliveryNotes;
        Int64? idSupplierComplaint;
        Int32? currentlyAccessedBy;
        string zf01;
        double? importExpenses;
        Int64? idCurrencyImportExpenses;
        byte? isValidated;

        Int64 quantity;
        string pdfFilePath;
        byte[] pdfFileInBytes;
        List<WarehouseDeliveryNoteItem> warehouseDeliveryNoteItems;
        ArticleWarehouseLocations masterItem;
        SupplierComplaint supplierComplaint;
        List<String> serialCodes;
        #endregion

        #region Properties

        [Key]
        [Column("IdWarehouseDeliveryNote")]
        [DataMember]
        public Int64 IdWarehouseDeliveryNote
        {
            get { return idWarehouseDeliveryNote; }
            set
            {
                idWarehouseDeliveryNote = value;
                OnPropertyChanged("IdWarehouseDeliveryNote");
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

        [Column("DeliveryDate")]
        [DataMember]
        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
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
        public Int32 CreatedBy
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
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
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

        [NotMapped]
        [DataMember]
        public WarehousePurchaseOrder WarehousePurchaseOrder
        {
            get { return warehousePurchaseOrder; }
            set
            {
                warehousePurchaseOrder = value;
                OnPropertyChanged("WarehousePurchaseOrder");
            }
        }

        [Column("SupplierCode")]
        [DataMember]
        public string SupplierCode
        {
            get { return supplierCode; }
            set
            {
                supplierCode = value;
                OnPropertyChanged("SupplierCode");
            }
        }

        [Column("IdCurrency")]
        [DataMember]
        public byte IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [Column("ExchangeRate")]
        [DataMember]
        public double ExchangeRate
        {
            get { return exchangeRate; }
            set
            {
                exchangeRate = value;
                OnPropertyChanged("ExchangeRate");
            }
        }

        [Column("AttachedDeliveryNotes")]
        [DataMember]
        public byte AttachedDeliveryNotes
        {
            get { return attachedDeliveryNotes; }
            set
            {
                attachedDeliveryNotes = value;
                OnPropertyChanged("AttachedDeliveryNotes");
            }
        }

        [Column("IdSupplierComplaint")]
        [DataMember]

        public Int64? IdSupplierComplaint
        {
            get { return idSupplierComplaint; }
            set
            {
                idSupplierComplaint = value;
                OnPropertyChanged("IdSupplierComplaint");
            }
        }

        [Column("CurrentlyAccessedBy")]
        [DataMember]
        public Int32? CurrentlyAccessedBy
        {
            get { return currentlyAccessedBy; }
            set
            {
                currentlyAccessedBy = value;
                OnPropertyChanged("CurrentlyAccessedBy");
            }
        }

        [Column("ZF01")]
        [DataMember]
        public string ZF01
        {
            get { return zf01; }
            set
            {
                zf01 = value;
                OnPropertyChanged("ZF01");
            }
        }

        [Column("ImportExpenses")]
        [DataMember]
        public double? ImportExpenses
        {
            get { return importExpenses; }
            set
            {
                importExpenses = value;
                OnPropertyChanged("ImportExpenses");
            }
        }

        [Column("IdCurrencyImportExpenses")]
        [DataMember]
        public Int64? IdCurrencyImportExpenses
        {
            get { return idCurrencyImportExpenses; }
            set
            {
                idCurrencyImportExpenses = value;
                OnPropertyChanged("IdCurrencyImportExpenses");
            }
        }

        [Column("IsValidated")]
        [DataMember]
        public byte? IsValidated
        {
            get { return isValidated; }
            set
            {
                isValidated = value;
                OnPropertyChanged("IsValidated");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public string PdfFilePath
        {
            get { return pdfFilePath; }
            set
            {
                pdfFilePath = value;
                OnPropertyChanged("PdfFilePath");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WarehouseDeliveryNoteItem> WarehouseDeliveryNoteItems
        {
            get { return warehouseDeliveryNoteItems; }
            set
            {
                warehouseDeliveryNoteItems = value;
                OnPropertyChanged("WarehouseDeliveryNoteItems");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] PdfFileInBytes
        {
            get { return pdfFileInBytes; }
            set
            {
                pdfFileInBytes = value;
                OnPropertyChanged("PdfFileInBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleWarehouseLocations MasterItem
        {
            get { return masterItem; }
            set
            {
                masterItem = value;
                OnPropertyChanged("MasterItem");
            }
        }


        [NotMapped]
        [DataMember]
        public SupplierComplaint SupplierComplaint
        {
            get { return supplierComplaint; }
            set
            {
                supplierComplaint = value;
                OnPropertyChanged("SupplierComplaint");
            }
        }


        [NotMapped]
        [DataMember]
        public List<String> SerialCodes
        {
            get { return serialCodes; }
            set
            {
                serialCodes = value;
                OnPropertyChanged("SerialCodes");
            }
        }


        //[NotMapped]
        //[DataMember]

        #endregion

        #region Constructor

        public WarehouseDeliveryNote()
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
