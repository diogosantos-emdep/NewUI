using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.ERM;

namespace Emdep.Geos.Data.Common
{
    public class InventoryMaterial : ModelBase, IDisposable, IDataErrorInfo
    {
        #region Fields

        Int64 idWareHouseDeliveryNoteItem;
        int idArticle;
        string description;
        string imagePath;
        string reference;
        string comments;
        DateTime? deliveryDate;
        Int64 availableQty;
        byte[] articleImageInBytes;
        Int64 idWarehouse;
        Int64 idWarehouseLocation;
        Int64 remainingQty;
        Int64 scannedQty;
        string remaningQtySign;
        WarehouseDeliveryNote warehouseDeliveryNote;
        Int64 awlminimumStock;
        Int64 minimumStock;
        Int16 idCurrency;
        int modifiedBy;
        double costPrice;
        double unitPrice;
        Int64 idOtitem;
        Int64? idWarehouseProductComponent;
        //bool isInserted;
        Int64 articleTotalStock;
        Int32 idReason;
        LookupValue reason;
        WarehouseInventoryAuditItem warehouseInventoryAuditItem;
        bool isApproved;
        bool? isOK;
      private bool isChecked;//[rani dhamankar][07-04-2025][GEOS2-6758]
        #endregion

        #region Properties

        public Int64 IdWareHouseDeliveryNoteItem
        {
            get { return idWareHouseDeliveryNoteItem; }
            set
            {
                idWareHouseDeliveryNoteItem = value;
                OnPropertyChanged("IdWareHouseDeliveryNoteItem");
            }
        }

        public int IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        public Int64 IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }


        public long AvailableQty
        {
            get { return availableQty; }
            set
            {
                availableQty = value;
                OnPropertyChanged("AvailableQty");
            }
        }

        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        public byte[] ArticleImageInBytes
        {
            get { return articleImageInBytes; }
            set
            {
                articleImageInBytes = value;
                OnPropertyChanged("ArticleImageInBytes");
            }
        }


        [DataMember]
        [NotMapped]
        public WarehouseDeliveryNote WarehouseDeliveryNote
        {
            get { return warehouseDeliveryNote; }
            set
            {
                warehouseDeliveryNote = value;
                OnPropertyChanged("WarehouseDeliveryNote");
            }
        }


        public Int64 IdWarehouseLocation
        {
            get { return idWarehouseLocation; }
            set
            {
                idWarehouseLocation = value;
                OnPropertyChanged("IdWarehouseLocation");
            }
        }

        public Int64 RemainingQty
        {
            get { return remainingQty; }
            set
            {
                remainingQty = value;
                OnPropertyChanged("RemainingQty");
                OnPropertyChanged("RemaningQtySign");
            }
        }

        public Int64 ScannedQty
        {
            get { return scannedQty; }
            set
            {
                scannedQty = value;
                OnPropertyChanged("ScannedQty");

            }
        }

        public string RemaningQtySign
        {
            get
            {
                if (RemainingQty == 0)
                {
                    return "Zero";
                }
                else if (RemainingQty > 0)
                {
                    return "Plus";
                }
                else if (RemainingQty < 0)
                {
                    return "Minus";
                };
                return null;
            }

        }

        public long AwlMinimumStock
        {
            get { return awlminimumStock; }
            set
            {
                awlminimumStock = value;
                OnPropertyChanged("AwlMinimumStock");
            }
        }

        public long MinimumStock
        {
            get { return minimumStock; }
            set
            {
                minimumStock = value;
                OnPropertyChanged("MinimumStock");
            }
        }

        public long IdOtitem
        {
            get { return idOtitem; }
            set
            {
                idOtitem = value;
                OnPropertyChanged("IdOtitem");
            }
        }

        public double CostPrice
        {
            get { return costPrice; }
            set
            {
                costPrice = value;
                OnPropertyChanged("CostPrice");
            }
        }

        public double UnitPrice
        {
            get { return unitPrice; }
            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }

        public int ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        public Int16 IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        public Int64? IdWarehouseProductComponent
        {
            get { return idWarehouseProductComponent; }
            set
            {
                idWarehouseProductComponent = value;
                OnPropertyChanged("IdWarehouseProductComponent");
            }
        }

        public Int64 ArticleTotalStock
        {
            get { return articleTotalStock; }
            set
            {
                articleTotalStock = value;
                OnPropertyChanged("ArticleTotalStock");
            }
        }

        public int IdReason
        {
            get { return idReason; }
            set
            {
                idReason = value;
                OnPropertyChanged("IdReason");
            }
        }

        public LookupValue Reason
        {
            get { return reason; }
            set
            {
                reason = value;
                OnPropertyChanged("Reason");
            }
        }

        public WarehouseInventoryAuditItem WarehouseInventoryAuditItem
        {
            get { return warehouseInventoryAuditItem; }
            set
            {
                warehouseInventoryAuditItem = value;
                OnPropertyChanged("WarehouseInventoryAuditItem");
            }
        }

        public bool IsApproved
        {
            get { return isApproved; }
            set
            {
                isApproved = value;
                OnPropertyChanged("IsApproved");
            }
        }

        public bool? IsOK
        {
            get { return isOK; }
            set
            {
                isOK = value;
                OnPropertyChanged("IsOK");
            }
        }
        #region [rani dhamankar][07-04-2025][GEOS2-6758]

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }
        private bool isCheckboxEnabled;
        public bool IsCheckboxEnabled
        {
            get => isCheckboxEnabled;
            set
            {
                if (isCheckboxEnabled != value)
                {
                    isCheckboxEnabled = value;
                    OnPropertyChanged("IsCheckboxEnabled");
                }
            }
        }
        #endregion
        #endregion

        #region Constructor
        public InventoryMaterial()
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

        #region Validation

        public string Error
        {
            get { return GetError(); }
        }

        public string this[string columnName]
        {
            get { return GetError(columnName); }
        }

        string GetError(string name = null)
        {
            switch (name)
            {
                case "IdReason":
                    return RemainingQty != 0 && IdReason <= 0 ? "Add reason!" : null;

                default:
                    return null;
            }
        }

        #endregion
        
    }
}
