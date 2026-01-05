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
    public class TransferMaterials : ModelBase, IDisposable
    {
        #region Fields

        Int64 idArticleWarehouseLocation;
        Int64 idWarehouseLocation;
        Int64 idWareHouseDeliveryNoteItem;
        Int64 currentStock;
        Int64 minimumStock;
        Int64 maximumStock;
        Int64 idWarehouse;
        Int64 scannedQty;
        Int64 quantity;
        Int16 idCurrency;
        int modifiedBy;
        double costPrice;
        double unitPrice;
        int idArticle;
        string locationFullName;
        string description;
        string imagePath;
        string reference;
        string articleVisualAidsPath;
        string comments;
        DateTime? uploadedIn;
        byte[] articleImageInBytes;
        Article article;
        WarehouseDeliveryNote warehouseDeliveryNote;
        List<SerialNumber> serialNumbers;
        byte registerSerialNumber;
        List<SerialNumber> scanSerialNumbers;
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

        public Article Article
        {
            get
            {
                return article;
            }

            set
            {
                article = value;
                OnPropertyChanged("Article");
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
        #endregion

        #region Constructor
        public TransferMaterials()
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
