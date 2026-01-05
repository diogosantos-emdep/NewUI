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
    public class PickingMaterials : ModelBase, IDisposable
    {
        #region Fields

        Int64 idArticleWarehouseLocation;
        Int64 idWarehouseLocation;
        Int64 idWareHouseDeliveryNoteItem;
        Int64 articleCurrentStock;
        Int64 currentStock;
        Int64 minimumStock;
        Int64 maximumStock;
        Int64 idWarehouse;
        Int64 idOtitem;
        Int64 scannedQty;
        Int16 idCurrency;
        int modifiedBy;
        double costPrice;
        double unitPrice;
        int idArticle;
        string locationFullName;
        string description;
        string imagePath;
        string reference;
        string comments;
        DateTime? uploadedIn;
        Int64 downloadQuantity;
        byte[] articleImageInBytes;
        Int64 idRevisionItem;
        WarehouseLocation warehouseLocation;
        string madeIn;
        string partNumberCode;
        List<SerialNumber> serialNumbers;
        byte registerSerialNumber;
        List<SerialNumber> scanSerialNumbers;
        Observation observation;
        RevisionItem revisionItem;
        Int64? idWarehouseProductComponent;
        WarehouseDeliveryNote warehouseDeliveryNote;
        Int64 idCountryGroup;
        Article article;
        Int64 remainingQtyForLoop;
        Manufacturer manufacturer;
        string productComponentNumber;
        Int64 batchPrintQuantity;
        bool isPartialBatchPrinted;
        string articleComment;
        DateTime? articleCommentDateOfExpiry;
        bool showComment;
        bool lockedForOrderPicking;//[Sudhir.jangra][GEOS2-4544]
        #endregion

        #region Properties

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
        public string LocationFullName
        {
            get
            {
                return locationFullName;
            }

            set
            {
                locationFullName = value;
                OnPropertyChanged("LocationFullName");
            }
        }

        public Int64 CurrentStock
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

        public long IdOtitem
        {
            get
            {
                return idOtitem;
            }

            set
            {
                idOtitem = value;
                OnPropertyChanged("IdOtitem");
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

        public long DownloadQuantity
        {
            get
            {
                return downloadQuantity;
            }

            set
            {
                downloadQuantity = value;
                OnPropertyChanged("DownloadQuantity");
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

        public byte[] ArticleImageInBytes
        {
            get { return articleImageInBytes; }
            set
            {
                articleImageInBytes = value;
                OnPropertyChanged("ArticleImageInBytes");
            }
        }

        public Int64 ScannedQty
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

        public Int64 ArticleCurrentStock
        {
            get
            {
                return articleCurrentStock;
            }

            set
            {
                articleCurrentStock = value;
                OnPropertyChanged("ArticleCurrentStock");
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


        public string MadeIn
        {
            get
            {
                return madeIn;
            }

            set
            {
                madeIn = value;
                OnPropertyChanged("MadeIn");
            }
        }

        public string PartNumberCode
        {
            get
            {
                return partNumberCode;
            }

            set
            {
                partNumberCode = value;
                OnPropertyChanged("PartNumberCode");
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


        [DataMember]
        [NotMapped]
        public Observation Observation
        {
            get { return observation; }
            set
            {
                observation = value;
                OnPropertyChanged("Observation");
            }
        }


        [DataMember]
        [NotMapped]
        public RevisionItem RevisionItem
        {
            get { return revisionItem; }
            set
            {
                revisionItem = value;
                OnPropertyChanged("RevisionItem");
            }
        }


        [DataMember]
        [NotMapped]
        public Int64? IdWarehouseProductComponent
        {
            get { return idWarehouseProductComponent; }
            set
            {
                idWarehouseProductComponent = value;
                OnPropertyChanged("IdWarehouseProductComponent");
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

        [DataMember]
        [NotMapped]
        public Int64 IdCountryGroup
        {
            get { return idCountryGroup; }
            set
            {
                idCountryGroup = value;
                OnPropertyChanged("IdCountryGroup");
            }
        }

        [DataMember]
        [NotMapped]
        public Article Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged("Article");
            }
        }

        [DataMember]
        [NotMapped]
        public Int64 RemainingQtyForLoop
        {
            get { return remainingQtyForLoop; }
            set
            {
                remainingQtyForLoop = value;
                OnPropertyChanged("RemainingQtyForLoop");
            }
        }

        [DataMember]
        [NotMapped]
        public Manufacturer Manufacturer
        {
            get
            {
                return manufacturer;
            }

            set
            {
                manufacturer = value;
                OnPropertyChanged("Manufacturer");
            }
        }

        [DataMember]
        [NotMapped]
        public string ProductComponentNumber
        {
            get
            {
                return productComponentNumber;
            }

            set
            {
                productComponentNumber = value;
                OnPropertyChanged("ProductComponentNumber");
            }
        }


        [DataMember]
        [NotMapped]
        public Int64 BatchPrintQuantity
        {
            get
            {
                return batchPrintQuantity;
            }

            set
            {
                batchPrintQuantity = value;
                OnPropertyChanged("BatchPrintQuantity");
            }
        }

        [DataMember]
        [NotMapped]
        public bool IsPartialBatchPrinted
        {
            get
            {
                return isPartialBatchPrinted;
            }

            set
            {
                isPartialBatchPrinted = value;
                OnPropertyChanged("IsPartialBatchPrinted");
            }
        }


        [DataMember]
        [NotMapped]
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


        [DataMember]
        [NotMapped]
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

        [DataMember]
        [NotMapped]
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

        [DataMember]
        public bool LockedForOrderPicking
        {
            get { return lockedForOrderPicking; }
            set
            {
                lockedForOrderPicking = value;
                OnPropertyChanged("LockedForOrderPicking");
            }
        }

        bool isInventoryReview = false;
        [DataMember]
        public bool IsInventoryReview
        {
            get { return isInventoryReview; }
            set
            {
                isInventoryReview = value;
                OnPropertyChanged("IsInventoryReview");
            }
        }

        #endregion

        #region Constructor
        public PickingMaterials()
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
