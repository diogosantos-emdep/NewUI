using System;
using System.Collections.Generic;

namespace Emdep.Geos.Data.Common
{
    public class PendingStorageArticles : ModelBase, IDisposable
    {
        #region Fields

        Int64 idWarehouse;
        Int64 idWarehouseLocation;
        Int64 idArticleWarehouseLocation;
        Int64 idWareHouseDeliveryNote;
        Int64 idWareHouseDeliveryNoteItem;
        Int64 quantity;
        Int64 currentStock;
        Int64 minimumStock;
        Int64 maximumStock;
        Int64 scannedQty;
        Int16 idCurrency;

        double costPrice;
        double unitPrice;

        int modifiedBy;
        int idArticle;

        string description;
        string reference;
        string code;
        string imagePath;
        string fullName;
        string articleVisualAidsPath;
        string comments;
        string articleComment;

        DateTime? uploadedIn;
        byte[] articleImageInBytes;
        WarehouseDeliveryNote warehouseDeliveryNote;
        WarehouseLocation warehouseLocation;
        Int64 awlminimumStock;
        bool showComment;
        DateTime? articleCommentDateOfExpiry;

        List<SerialNumber> serialNumbers;
        byte registerSerialNumber;
        List<SerialNumber> scanSerialNumbers;
        Int64? remainingQtyAfterScan;
        bool? isRemainingQtyEqualScannedQty;
        #endregion

        #region Properties

        public long IdWarehouse
        {
            get
            {
                return idWarehouse;
            }

            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        public long IdWarehouseLocation
        {
            get
            {
                return idWarehouseLocation;
            }

            set
            {
                idWarehouseLocation = value;
                OnPropertyChanged("IdWarehouseLocation");
            }
        }

        public long IdArticleWarehouseLocation
        {
            get
            {
                return idArticleWarehouseLocation;
            }

            set
            {
                idArticleWarehouseLocation = value;
                OnPropertyChanged("IdArticleWarehouseLocation");
            }
        }

        public long IdWareHouseDeliveryNote
        {
            get
            {
                return idWareHouseDeliveryNote;
            }

            set
            {
                idWareHouseDeliveryNote = value;
                OnPropertyChanged("IdWareHouseDeliveryNote");
            }
        }

        public long IdWareHouseDeliveryNoteItem
        {
            get
            {
                return idWareHouseDeliveryNoteItem;
            }

            set
            {
                idWareHouseDeliveryNoteItem = value;
                OnPropertyChanged("IdWareHouseDeliveryNoteItem");
            }
        }

        public long Quantity
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

        public long MinimumStock
        {
            get
            {
                return minimumStock;
            }

            set
            {
                minimumStock = value;
                OnPropertyChanged("MinimumStock");
            }
        }

        public long MaximumStock
        {
            get
            {
                return maximumStock;
            }

            set
            {
                maximumStock = value;
                OnPropertyChanged("MaximumStock");
            }
        }

        public long CurrentStock
        {
            get
            {
                return currentStock;
            }

            set
            {
                currentStock = value;
                OnPropertyChanged("CurrentStock");
            }
        }

        public long ScannedQty
        {
            get
            {
                return scannedQty;
            }

            set
            {
                scannedQty = value;
                OnPropertyChanged("ScannedQty");
            }
        }

        public int IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        public double CostPrice
        {
            get
            {
                return costPrice;
            }

            set
            {
                costPrice = value;
                OnPropertyChanged("CostPrice");
            }
        }

        public double UnitPrice
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

        public int ModifiedBy
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

        public DateTime? UploadedIn
        {
            get
            {
                return uploadedIn;
            }

            set
            {
                uploadedIn = value;
                OnPropertyChanged("UploadedIn");
            }
        }

        public string ImagePath
        {
            get
            {
                return imagePath;
            }

            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
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

        public string FullName
        {
            get
            {
                return fullName;
            }

            set
            {
                fullName = value;
                OnPropertyChanged("UploadedIn");
            }
        }

        public string ArticleVisualAidsPath
        {
            get
            {
                return articleVisualAidsPath;
            }

            set
            {
                articleVisualAidsPath = value;
                OnPropertyChanged("ArticleVisualAidsPath");
            }
        }

        public Int16 IdCurrency
        {
            get
            {
                return idCurrency;
            }

            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        public WarehouseDeliveryNote WarehouseDeliveryNote
        {
            get
            {
                return warehouseDeliveryNote;
            }

            set
            {
                warehouseDeliveryNote = value;
                OnPropertyChanged("WarehouseDeliveryNote");
            }
        }

        public WarehouseLocation WarehouseLocation
        {
            get
            {
                return warehouseLocation;
            }

            set
            {
                 warehouseLocation = value;
                OnPropertyChanged("WarehouseLocation");
            }
        }

        public long AwlMinimumStock
        {
            get
            {
                return awlminimumStock;
            }

            set
            {
                awlminimumStock = value;
                OnPropertyChanged("AwlMinimumStock");
            }
        }


        public string ArticleComment
        {
            get
            {
                return articleComment;
            }

            set
            {
                articleComment = value;
                OnPropertyChanged("ArticleComment");
            }
        }

        public bool ShowComment
        {
            get
            {
                return showComment;
            }

            set
            {
                showComment = value;
                OnPropertyChanged("ShowComment");
            }
        }

        public DateTime? ArticleCommentDateOfExpiry
        {
            get
            {
                return articleCommentDateOfExpiry;
            }

            set
            {
                articleCommentDateOfExpiry = value;
                OnPropertyChanged("ArticleCommentDateOfExpiry");
            }
        }


        public List<SerialNumber> SerialNumbers
        {
            get { return serialNumbers; }
            set
            {
                serialNumbers = value;
                OnPropertyChanged("SerialNumbers");
            }
        }

        public Byte RegisterSerialNumber
        {
            get { return registerSerialNumber; }
            set
            {
                registerSerialNumber = value;
                OnPropertyChanged("RegisterSerialNumber");
            }
        }


        public List<SerialNumber> ScanSerialNumbers
        {
            get { return scanSerialNumbers; }
            set
            {
                scanSerialNumbers = value;
                OnPropertyChanged("ScanSerialNumbers");
            }
        }

        public Int64? RemainingQtyAfterScan
        {
            get
            {
                return remainingQtyAfterScan;
            }

            set
            {
                remainingQtyAfterScan = value;
                OnPropertyChanged("RemainingQtyAfterScan");
            }
        }
        public bool? IsRemainingQtyEqualScannedQty
        {
            get
            {
                return isRemainingQtyEqualScannedQty;
            }

            set
            {
                isRemainingQtyEqualScannedQty = value;
                OnPropertyChanged("IsRemainingQtyEqualScannedQty");
            }
        }
        #endregion

        #region Constructor
        public PendingStorageArticles()
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
