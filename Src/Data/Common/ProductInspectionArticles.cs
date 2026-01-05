using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    public class ProductInspectionArticles : ModelBase, IDisposable
    {
        #region Fields

        Int64 idWarehouse;
        Int64 idOperator;
        Int64 idArticleWarehouseInspection;
      //  Int64 idArticleWarehouseInspection;
        Int64 idWareHouseDeliveryNote;
        Int64 idWareHouseDeliveryNoteItem;
        Int64 quantity;
        Int64 quantityInspected;
        Int64 currentStock;
        Int64 minimumStock;
        Int64 maximumStock;
        Int64 scannedQty;
        Int16 idCurrency;
        Warehouses warehouses;
        double costPrice;
        double unitPrice;
        LookupValue inspectionStatus;
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
        DateTime? inspectionDate;
        byte[] articleImageInBytes;
        WarehouseDeliveryNote warehouseDeliveryNote;
        WarehouseLocation warehouseLocation;
        Int64 awlminimumStock;
        bool showComment;
        DateTime? articleCommentDateOfExpiry;

        List<SerialNumber> serialNumbers;
        byte registerSerialNumber;
        List<SerialNumber> scanSerialNumbers;
        string stageComments;
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
        public long IdOperator
        {
            get
            {
                return idOperator;
            }

            set
            {
                idOperator = value;
                OnPropertyChanged("IdOperator");
            }
        }

        public long IdArticleWarehouseInspection
        {
            get
            {
                return idArticleWarehouseInspection;
            }

            set
            {
                idArticleWarehouseInspection = value;
                OnPropertyChanged("IdArticleWarehouseInspection");
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

        public long QuantityInspected
        {
            get
            {
                return quantityInspected;
            }

            set
            {
                quantityInspected = value;
                OnPropertyChanged("QuantityInspected");
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
        public DateTime? InspectionDate
        {
            get
            {
                return inspectionDate;
            }

            set
            {
                inspectionDate = value;
                OnPropertyChanged("InspectionDate");
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

        [NotMapped]
        [DataMember]
        public Warehouses Warehouses
        {
            get
            {
                return warehouses;
            }

            set
            {
                warehouses = value;
                OnPropertyChanged("Warehouses");
            }
        }
        [NotMapped]
        [DataMember]
        public LookupValue InspectionStatus
        {
            get
            {
                return inspectionStatus;
            }

            set
            {
                inspectionStatus = value;
                OnPropertyChanged("InspectionStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public string StageComments
        {
            get { return stageComments; }
            set
            {
                stageComments = value;
                OnPropertyChanged("StageComments");
            }
        }

        #endregion

        #region Constructor
        public ProductInspectionArticles()
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
