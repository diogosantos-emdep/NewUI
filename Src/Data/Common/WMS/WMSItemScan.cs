using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    [Table("OtItem")]
    [DataContract]
    public class WMSItemScan : ModelBase, IDisposable
    {

        Int64 idOT;
        Int64 idOTItem;
        byte isBatch;
        string attachedFiles;
        byte rework;
        //string description;
        //sbyte isGeneric;
        //string warehouseProductDescription;
        //string reference;
        //sbyte isObsolete;
        //string description_es;
        //string description_fr;
        //string imagePath;
        //string imagePathURL;
        WMSItemScanRevisionItem revisionItem;
        Int32 idArticle;
        Int32 assignedTo;
        Int64 idOffer;
        byte idItemOtStatus;
        ItemOTStatusType status;
        WMSItemScanAssignedToUser assignedToUser;

        [Column("IdOT")]
        [DataMember]
        public Int64 IdOT
        {
            get
            {
                return idOT;
            }

            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        [Column("IdOTItem")]
        [DataMember]
        public Int64 IdOTItem
        {
            get
            {
                return idOTItem;
            }

            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }


        [Column("IsBatch")]
        [DataMember]
        public byte IsBatch
        {
            get
            {
                return isBatch;
            }

            set
            {
                isBatch = value;
                OnPropertyChanged("IsBatch");
            }
        }

        [Column("AttachedFiles")]
        [DataMember]
        public string AttachedFiles
        {
            get
            {
                return attachedFiles;
            }

            set
            {
                attachedFiles = value;
                OnPropertyChanged("AttachedFiles");
            }
        }

        [Column("Rework")]
        [DataMember]
        public byte Rework
        {
            get
            {
                return rework;
            }

            set
            {
                rework = value;
                OnPropertyChanged("Rework");
            }
        }

        Int64 idRevisionItem;
        [Column("IdRevisionItem")]
        [DataMember]
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
        
        [Column("RevisionItem")]
        [DataMember]
        public WMSItemScanRevisionItem RevisionItem
        {
            get
            {
                return revisionItem;
            }

            set
            {
                revisionItem = value;
                OnPropertyChanged("RevisionItem");
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
       
        [Column("IdItemOtStatus")]
        [DataMember]
        public byte IdItemOtStatus
        {
            get
            {
                return idItemOtStatus;
            }

            set
            {
                idItemOtStatus = value;
                OnPropertyChanged("IdItemOtStatus");
            }
        }
       
        [Column("IdItemOtStatus")]
        [DataMember]
        public ItemOTStatusType Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [Column("AssignedTo")]
        [DataMember]
        public Int32 AssignedTo
        {
            get
            {
                return assignedTo;
            }

            set
            {
                assignedTo = value;
                OnPropertyChanged("AssignedTo");
            }
        }
       
        [NotMapped]
        [DataMember]
        public WMSItemScanAssignedToUser AssignedToUser
        {
            get { return assignedToUser; }
            set
            {
                assignedToUser = value;
                OnPropertyChanged("AssignedToUser");
            }
        }
       
        [Column("IdOffer")]
        [ForeignKey("Offer")]
        [DataMember]
        public Int64 IdOffer
        {
            get
            {
                return idOffer;
            }

            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        List<WMSItemScan> articleDecomposedList;
        [NotMapped]
        [DataMember]
        public List<WMSItemScan> ArticleDecomposedList
        {
            get { return articleDecomposedList; }
            set
            {
                articleDecomposedList = value;
                OnPropertyChanged("ArticleDecomposedList");
            }
        }

        Int32? parentidArticle;
        [NotMapped]
        [DataMember]
        public Int32? ParentidArticle
        {
            get { return parentidArticle; }
            set
            {
                parentidArticle = value;
                OnPropertyChanged("ParentidArticle");
            }
        }

        Int32 mainParentIdArticle;
        [NotMapped]
        [DataMember]
        public Int32 MainParentIdArticle
        {
            get { return mainParentIdArticle; }
            set
            {
                mainParentIdArticle = value;
                OnPropertyChanged("MainParentIdArticle");
            }
        }

        int keyId;
        [NotMapped]
        [DataMember]
        public int KeyId
        {
            get { return keyId; }
            set
            {
                keyId = value;
                OnPropertyChanged("KeyId");
            }
        }

        int parentId;
        [NotMapped]
        [DataMember]
        public int ParentId
        {
            get { return parentId; }
            set
            {
                parentId = value;
                OnPropertyChanged("ParentId");
            }
        }

        List<WMSPickingMaterials> pickingMaterialsList;
        [NotMapped]
        [DataMember]
        public List<WMSPickingMaterials> PickingMaterialsList
        {
            get
            {
                return pickingMaterialsList;
            }

            set
            {
                pickingMaterialsList = value;
                OnPropertyChanged("PickingMaterialsList");
            }
        }

        List<WMSPickingMaterials> notInspectedpickingMaterialsList;
        [NotMapped]
        [DataMember]
        public List<WMSPickingMaterials> NotInspectedPickingMaterialsList
        {
            get
            {
                return notInspectedpickingMaterialsList;
            }

            set
            {
                notInspectedpickingMaterialsList = value;
                OnPropertyChanged("NotInspectedPickingMaterialsList");
            }
        }
        bool isFinish;
        [NotMapped]
        [DataMember]
        public bool IsFinish
        {
            get
            {
                return isFinish;
            }

            set
            {
                isFinish = value;
                OnPropertyChanged("IsFinish");
            }
        }

        bool isDecomposed;
        [DataMember]
        public bool IsDecomposed
        {
            get
            {
                return isDecomposed;
            }

            set
            {
                isDecomposed = value;
                OnPropertyChanged("IsDecomposed");
            }
        }
        #region Constructor
        public WMSItemScan()
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

    //WMSItemScan
    public class WMSItemScanArticle : ModelBase, IDisposable
    {
        Int32 idArticle;
        string reference;
        sbyte isObsolete;
        string description;
        string description_es;
        string description_fr;
        string imagePath;
        string imagePathURL;
        sbyte isGeneric;

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
        [Key]
        [Column("Reference")]
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        [Column("IsObsolete")]
        [DataMember]
        public sbyte IsObsolete
        {
            get { return isObsolete; }
            set
            {
                isObsolete = value;
                OnPropertyChanged("IsObsolete");
            }
        }

        [Column("IsGeneric")]
        [DataMember]
        public sbyte IsGeneric
        {
            get { return isGeneric; }
            set
            {
                isGeneric = value;
                OnPropertyChanged("IsGeneric");
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

        [Column("Description_es")]
        [DataMember]
        public string Description_es
        {
            get { return description_es; }
            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }

        [Column("Description_fr")]
        [DataMember]
        public string Description_fr
        {
            get { return description_fr; }
            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }

        [Column("ImagePath")]
        [DataMember]
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        [Column("ImagePath")]
        [DataMember]
        public string ImagePathURL
        {
            get { return imagePathURL; }
            set
            {
                imagePathURL = value;
                OnPropertyChanged("ImagePath");
            }
        }

        byte registerSerialNumber;
        [Column("RegisterSerialNumber")]
        [DataMember]
        public byte RegisterSerialNumber
        {
            get { return registerSerialNumber; }
            set
            {
                registerSerialNumber = value;
                OnPropertyChanged("RegisterSerialNumber");
            }
        }

        string location;
        [NotMapped]
        [DataMember]
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        DateTime? uploadedIn;
        [NotMapped]
        [DataMember]
        public DateTime? UploadedIn
        {
            get { return uploadedIn; }
            set
            {
                uploadedIn = value;
                OnPropertyChanged("UploadedIn");
            }
        }

        Int64? idWareHouseDeliveryNoteItem;
        [NotMapped]
        [DataMember]
        public Int64? IdWareHouseDeliveryNoteItem
        {
            get { return idWareHouseDeliveryNoteItem; }
            set
            {
                idWareHouseDeliveryNoteItem = value;
                OnPropertyChanged("IdWareHouseDeliveryNoteItem");
            }
        }

        Int64 idArticleType;
        [Column("IdArticleType")]
        [DataMember]
        public Int64 IdArticleType
        {
            get { return idArticleType; }
            set
            {
                idArticleType = value;
                OnPropertyChanged("IdArticleType");
            }
        }

        long minimumStock;
        [Column("MinimumStock")]
        [DataMember]
        public long MinimumStock
        {
            get { return minimumStock; }
            set
            {
                minimumStock = value;
                OnPropertyChanged("MinimumStock");
            }
        }

        long maximumStock;
        [Column("MaximumStock")]
        [DataMember]
        public long MaximumStock
        {
            get { return maximumStock; }
            set
            {
                maximumStock = value;
                OnPropertyChanged("MaximumStock");
            }
        }

        long currentStock;
        [Column("CurrentStock")]
        [DataMember]
        public long CurrentStock
        {
            get { return currentStock; }
            set
            {
                currentStock = value;
                OnPropertyChanged("CurrentStock");
            }
        }

        sbyte isCountRequiredAfterPicking;
        [Column("IsCountRequiredAfterPicking")]
        [DataMember]
        public sbyte IsCountRequiredAfterPicking
        {
            get { return isCountRequiredAfterPicking; }
            set
            {
                isCountRequiredAfterPicking = value;
                OnPropertyChanged("IsCountRequiredAfterPicking");
            }
        }

        decimal weight;
        [Column("Weight")]
        [DataMember]
        public decimal Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }

        string articleComment;
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

        DateTime? articleCommentDateOfExpiry;
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

        ArticleWarehouseLocations articleWarehouseLocation;
        [NotMapped]
        [DataMember]
        public ArticleWarehouseLocations ArticleWarehouseLocation
        {
            get { return articleWarehouseLocation; }
            set
            {
                articleWarehouseLocation = value;
                OnPropertyChanged("ArticleWarehouseLocations");
            }
        }
        #region Constructor
        public WMSItemScanArticle()
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
    public class WMSItemScanRevisionItem : ModelBase, IDisposable
    {

        Int64 idRevisionItem;
        string numItem;
        decimal quantity;
        decimal unitPrice;
        string internalComment;
        string customerComment;
        Int64 idWarehouseProduct;
        string description;
        Int32 idArticle;

        [Column("IdRevisionItem")]
        [DataMember]
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

        [Column("NumItem")]
        [DataMember]
        public string NumItem
        {
            get
            {
                return numItem;
            }

            set
            {
                numItem = value;
                OnPropertyChanged("NumItem");
            }
        }

        [Column("Quantity")]
        [DataMember]
        public decimal Quantity
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

        [Column("UnitPrice")]
        [DataMember]
        public decimal UnitPrice
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

        [Column("InternalComment")]
        [DataMember]
        public string InternalComment
        {
            get
            {
                return internalComment;
            }

            set
            {
                internalComment = value;
                OnPropertyChanged("InternalComment");
            }
        }

        [Column("CustomerComment")]
        [DataMember]
        public string CustomerComment
        {
            get
            {
                return customerComment;
            }

            set
            {
                customerComment = value;
                OnPropertyChanged("CustomerComment");
            }
        }

        [Key]
        [Column("IdWarehouseProduct")]
        [DataMember]
        public Int64 IdWarehouseProduct
        {
            get { return idWarehouseProduct; }
            set
            {
                idWarehouseProduct = value;
                OnPropertyChanged("IdWarehouseProduct");
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

        WMSItemScanArticle article;
        [NotMapped]
        [DataMember]
        public WMSItemScanArticle Article
        {
            get
            {
                return article;
            }

            set
            {
                article = value;
                OnPropertyChanged("WarehouseProduct");
            }
        }
        long downloadedQuantity;
        [NotMapped]
        [DataMember]
        public long DownloadedQuantity
        {
            get
            {
                return downloadedQuantity;
            }

            set
            {
                downloadedQuantity = value;
                OnPropertyChanged("DownloadedQuantity");
            }
        }

        long remainingQuantity;
        [NotMapped]
        [DataMember]
        public long RemainingQuantity
        {
            get
            {
                return remainingQuantity;
            }

            set
            {
                remainingQuantity = value;
                OnPropertyChanged("RemainingQuantity");
            }
        }
        long status;
        [NotMapped]
        [DataMember]
        public long Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        long idComponent;
        [NotMapped]
        [DataMember]
        public long IdComponent
        {
            get { return idComponent; }
            set
            {
                idComponent = value;
                OnPropertyChanged("IdComponent");
            }
        }
        #region Constructor
        public WMSItemScanRevisionItem()
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
    public class WMSItemScanAssignedToUser : ModelBase, IDisposable
    {
        string name;
        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        string surname;
        [Column("Surname")]
        [DataMember]
        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }
        #region Constructor
        public WMSItemScanAssignedToUser()
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
    public class WMSItemScanItemOTStatusType : ModelBase, IDisposable
    {

        #region Fields
        Int32 idItemOtStatus;
        string name;
        string htmlColor;
        Int32 sequence;
        #endregion

        #region Properties
        [Key]
        [Column("IdItemOtStatus")]
        [DataMember]
        public Int32 IdItemOtStatus
        {
            get
            {
                return idItemOtStatus;
            }

            set
            {
                idItemOtStatus = value;
                OnPropertyChanged("IdItemOtStatus");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("Sequence")]
        [DataMember]
        public Int32 Sequence
        {
            get
            {
                return sequence;
            }

            set
            {
                sequence = value;
                OnPropertyChanged("Sequence");
            }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }

            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        #endregion

        #region Constructor
        public WMSItemScanItemOTStatusType()
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
    public class WMSPickingMaterials : ModelBase, IDisposable
    {
        WMSItemScanArticle article;
        [DataMember]
        [NotMapped]
        public WMSItemScanArticle Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged("Article");
            }
        }
        int idArticle;
        [DataMember]
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

        Int64 articleCurrentStock;
        [DataMember]
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
        string reference;
        [DataMember]
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

        long idWarehouseLocation;
        [DataMember]
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

        string code;
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

        long idWareHouseDeliveryNoteItem;
        [DataMember]
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

        string locationFullName;
        [DataMember]
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

        string articleComment;
        [DataMember]
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

        DateTime? articleCommentDateOfExpiry;
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

        long idOtitem;
        [DataMember]
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

        Int64 idRevisionItem;
        [DataMember]
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

        WMSItemScanRevisionItem revisionItem;
        [DataMember]
        public WMSItemScanRevisionItem RevisionItem
        {
            get
            {
                return revisionItem;
            }

            set
            {
                revisionItem = value;
                OnPropertyChanged("RevisionItem");
            }
        }


        Int64? idWarehouseProductComponent;
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

        Int64 currentStock;
        [DataMember]
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

        double costPrice;
        [DataMember]
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

        double unitPrice;
        [DataMember]
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
        long minimumStock;
        [DataMember]
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
        long maximumStock;
        [DataMember]
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
        string description;
        [DataMember]
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

        string imagePath;
        [DataMember]
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

        string finalImagePath;
        [DataMember]
        public string FinalImagePath
        {
            get
            {
                return finalImagePath;
            }

            set
            {
                finalImagePath = value;
                OnPropertyChanged("FinalImagePath");
            }
        }
        long idWarehouse;
        [DataMember]
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

        string madeIn;
        [DataMember]
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

        Manufacturer manufacturer;
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

        List<SerialNumber> serialNumbers;
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

        DateTime? uploadedIn;
        [DataMember]
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



        WarehouseLocation warehouseLocation;
        [DataMember]
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

        long downloadQuantity;
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
        WMSWarehouseDeliveryNote warehouseDeliveryNote;
        [DataMember]
        [NotMapped]
        public WMSWarehouseDeliveryNote WarehouseDeliveryNote
        {
            get { return warehouseDeliveryNote; }
            set
            {
                warehouseDeliveryNote = value;
                OnPropertyChanged("WarehouseDeliveryNote");
            }
        }

        Byte registerSerialNumber;
        [DataMember]
        public Byte RegisterSerialNumber
        {
            get { return registerSerialNumber; }
            set
            {
                registerSerialNumber = value;
                OnPropertyChanged("RegisterSerialNumber");
            }
        }

        string partNumberCode;
        [DataMember]
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
        Int16 idCurrency;
        [DataMember]
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

        Int64 idCountryGroup;
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

        bool lockedForOrderPicking;
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

        byte[] articleImageInBytes;
        [DataMember]
        public byte[] ArticleImageInBytes
        {
            get { return articleImageInBytes; }
            set
            {
                articleImageInBytes = value;
                OnPropertyChanged("ArticleImageInBytes");
            }
        }

        Int64 batchPrintQuantity;
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

        bool isInventoryReview;
        [DataMember]
        [NotMapped]
        public bool IsInventoryReview
        {
            get
            {
                return isInventoryReview;
            }

            set
            {
                isInventoryReview = value;
                OnPropertyChanged("IsInventoryReview");
            }
        }

        bool isPartialBatchPrinted;
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

        Int64 scannedQty;
        [DataMember]
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


        List<SerialNumber> scanSerialNumbers;
        public List<SerialNumber> ScanSerialNumbers
        {
            get { return scanSerialNumbers; }
            set
            {
                scanSerialNumbers = value;
                OnPropertyChanged("ScanSerialNumbers");
            }
        }

        bool showComment;
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


        string comments;
        [DataMember]
        [NotMapped]
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
        int modifiedBy;
        [DataMember]
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

        Observation observation;
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

        //WMSItemScanRevisionItem
        #region Constructor
        public WMSPickingMaterials()
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
    public class WMSWarehouseDeliveryNote : ModelBase, IDisposable
    {

        DateTime deliveryDate;
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

        string code;
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

        #region Constructor
        public WMSWarehouseDeliveryNote()
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

    public class WMSArticleImageInBytes : ModelBase, IDisposable
    {
        int idArticle;
        [DataMember]
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

        string reference;
        [DataMember]
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


        byte[] articleImageInBytes;
        [DataMember]
        public byte[] ArticleImageInBytes
        {
            get { return articleImageInBytes; }
            set
            {
                articleImageInBytes = value;
                OnPropertyChanged("ArticleImageInBytes");
            }
        }

        string imagePath;
        [Column("ImagePath")]
        [DataMember]
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        string finalImagePath;
        [DataMember]
        public string FinalImagePath
        {
            get
            {
                return finalImagePath;
            }

            set
            {
                finalImagePath = value;
                OnPropertyChanged("FinalImagePath");
            }
        }
        #region Constructor
        public WMSArticleImageInBytes()
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
