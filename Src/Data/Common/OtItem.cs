using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.SAM;

namespace Emdep.Geos.Data.Common
{
    [Table("OtItem")]
    [DataContract]
    public class OtItem : ModelBase, IDisposable
    {
        #region Fields
        bool showCombobox;
        byte rework;
        DateTime? modifiedIn;
        Int32 modifiedBy;
        DateTime? latestStatusChange;
        byte isBatch;
        Int64 idRevisionItem;
        Int64 idRevisionItemImported;
        Int64 idOTItem;
        Int64 idOT;
        byte idItemOtStatus;
        DateTime? docGeneratedIn;
        byte customerCommentRead;
        string customerComment;
        DateTime? createdIn;
        Int32 createdBy;
        string attachedFiles;
        Int32 assignedTo;
        People assignedToUser;
        RevisionItem revisionItem;
        ItemOTStatusType itemOTStatusType;
        Ots ot;
        Int64 expectedStock;//rajashri
        Quotation quotation;

        Int32 keyId;
        Int32 parentId;

        List<PickingMaterials> pickingMaterialsList;

        DateTime? shippingDate;

        List<Counterpart> counterparts;

        List<OtItem> articleDecomposedList;
        Int64 articleStock;
        Int64 articleMinimumStock;
        Int64 parentArticleType;
        bool showComment;
        List<Detection> lstDetection;
        DateTime? poDate;

        List<ItemOTStatusType> statusList;
        bool isUpdatedRow;//[Sudhir.Jangra][GEOS2-4539]
        bool isBulkArticle;//[cpatil][GEOS2-4417]
        Int32? parentidArticle = null;
        Int32 mainParentIdArticle = 0;
        List<PickingMaterials> notInspectedpickingMaterialsList;
        bool isFinish;
        List<ArticleForecast> lstArticleForecast;
        Int64 articleMaximumStock;//[Sudhir.Jangra][GEOS2-5635]
        long articleIncoming;//[Sudhir.Jangra][GEOS2-5635]
        long articleOutcoming;//[Sudhir.Jangra][GEOS2-5635]
        bool isReviewValidated;//[Sudhir.Jangra][GEOS2-5635]
        int validatedCount;//[Sudhir.Jangra][GEOS2-5635]
        Int64 idRevision;//[pramod.misal][GEOS2-5473][18.07.2024]
        string unitPriceWithSymbol;//[Sudhir.Jangra][GEOS2-5637]
        string numItemBarcodeText;//[Sudhir.Jangra][GEOS2-5637]

        // [nsatpute][17-01-2025][GEOS2-6445]
        string internalNotes;
        double costPrice;
        long purchaseQuantity;
        bool forcePurchaseQuantity;
        Currency currency;
        int packingSize;
        byte[] currencyIconBytes;
        private string workflowStatusName; //[GEOS2-8965][pallavi.kale][28.11.2025]
        private string workflowStatusHtmlColor; //[GEOS2-8965][pallavi.kale][28.11.2025]
        #endregion

        #region Constructor
        public OtItem()
        {

        }
        #endregion

        #region Properties
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

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

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
        [Column("LatestStatusChange")]
        [DataMember]
        public DateTime? LatestStatusChange
        {
            get
            {
                return latestStatusChange;
            }

            set
            {
                latestStatusChange = value;
                OnPropertyChanged("LatestStatusChange");
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

        [NotMapped]
        [DataMember]
        public Int64 IdRevisionItemImported
        {
            get
            {
                return idRevisionItemImported;
            }

            set
            {
                idRevisionItemImported = value;
                OnPropertyChanged("IdRevisionItemImported");
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

        [Column("DocGeneratedIn")]
        [DataMember]
        public DateTime? DocGeneratedIn
        {
            get
            {
                return docGeneratedIn;
            }

            set
            {
                docGeneratedIn = value;
                OnPropertyChanged("DocGeneratedIn");
            }
        }

        [Column("CustomerCommentRead")]
        [DataMember]
        public byte CustomerCommentRead
        {
            get
            {
                return customerCommentRead;
            }

            set
            {
                customerCommentRead = value;
                OnPropertyChanged("CustomerCommentRead");
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

        [Column("CreatedIn")]
        [DataMember]
        public DateTime? CreatedIn
        {
            get
            {
                return createdIn;
            }

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
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
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
        public People AssignedToUser
        {
            get { return assignedToUser; }
            set
            {
                assignedToUser = value;
                OnPropertyChanged("AssignedToUser");
            }
        }

        [NotMapped]
        [DataMember]
        public RevisionItem RevisionItem
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

        [NotMapped]
        [DataMember]
        public ItemOTStatusType Status
        {
            get
            {
                return itemOTStatusType;
            }

            set
            {
                itemOTStatusType = value;
                OnPropertyChanged("Status");
            }
        }

        [NotMapped]
        [DataMember]
        public Ots Ot
        {
            get
            {
                return ot;
            }

            set
            {
                ot = value;
                OnPropertyChanged("Ot");
            }
        }

        [NotMapped]
        [DataMember]
        public Quotation Quotation
        {
            get
            {
                return quotation;
            }

            set
            {
                quotation = value;
                OnPropertyChanged("Quotation");
            }
        }

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

        [NotMapped]
        [DataMember]
        public List<PickingMaterials> PickingMaterialsList
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

        [NotMapped]
        [DataMember]
        public DateTime? ShippingDate
        {
            get { return shippingDate; }
            set
            {
                shippingDate = value;
                OnPropertyChanged("ShippingDate");
            }
        }

        [NotMapped]
        [DataMember]
        public List<Counterpart> Counterparts
        {
            get { return counterparts; }
            set
            {
                counterparts = value;
                OnPropertyChanged("Counterparts");
            }
        }


        [NotMapped]
        [DataMember]
        public List<OtItem> ArticleDecomposedList
        {
            get { return articleDecomposedList; }
            set
            {
                articleDecomposedList = value;
                OnPropertyChanged("ArticleDecomposedList");
            }
        }
        [NotMapped]
        [DataMember]
        public Int64 ExpectedStock
        {
            get { return expectedStock; }
            set
            {
                expectedStock = value;
                OnPropertyChanged("ExpectedStock");
            }
        }
        [NotMapped]
        [DataMember]
        public Int64 ArticleStock
        {
            get { return articleStock; }
            set
            {
                articleStock = value;
                OnPropertyChanged("ArticleStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ArticleMinimumStock
        {
            get { return articleMinimumStock; }
            set
            {
                articleMinimumStock = value;
                OnPropertyChanged("ArticleMinimumStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ParentArticleType
        {
            get { return parentArticleType; }
            set
            {
                parentArticleType = value;
                OnPropertyChanged("ParentArticleType");
            }
        }

        [NotMapped]
        [DataMember]
        public bool ShowComment
        {
            get { return showComment; }
            set
            {
                showComment = value;
                OnPropertyChanged("ShowComment");
            }
        }


        [NotMapped]
        [DataMember]
        public List<Detection> LstDetection
        {
            get { return lstDetection; }
            set
            {
                lstDetection = value;
                OnPropertyChanged("LstDetection");
            }
        }
        [NotMapped]
        [DataMember]
        public List<ArticleForecast> LstArticleForecast
        {
            get { return lstArticleForecast; }
            set
            {
                lstArticleForecast = value;
                OnPropertyChanged("LstArticleForecast");
            }
        }

        [Column("PoDate")]
        [DataMember]
        public DateTime? PoDate
        {
            get
            {
                return poDate;
            }

            set
            {
                poDate = value;
                OnPropertyChanged("PoDate");
            }
        }

        List<SAMLogEntries> logEntriesByOT;
        [DataMember]
        public List<SAMLogEntries> LogEntriesByOT
        {
            get
            {
                return logEntriesByOT;
            }

            set
            {
                logEntriesByOT = value;
                OnPropertyChanged("LogEntriesByOT");
            }
        }

        List<SAMLogEntries> comments;
        [DataMember]
        public List<SAMLogEntries> Comments
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

        [DataMember]
        [NotMapped]
        public List<ItemOTStatusType> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged("StatusList");
            }
        }

        public bool IsUpdatedRow
        {
            get { return isUpdatedRow; }
            set
            {
                isUpdatedRow = value;
                OnPropertyChanged("IsUpdatedRow");
            }
        }

        public bool IsBulkArticle
        {
            get { return isBulkArticle; }
            set
            {
                isBulkArticle = value;
                OnPropertyChanged("IsBulkArticle");
            }
        }

        [DataMember]
        public bool ShowCombobox
        {
            get { return showCombobox; }
            set
            {
                showCombobox = value;
                OnPropertyChanged("ShowCombobox");
            }
        }

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


        [NotMapped]
        [DataMember]
        public List<PickingMaterials> NotInspectedPickingMaterialsList
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

        //[Sudhir.Jangra][GEOS2-5635]
        [NotMapped]
        [DataMember]
        public Int64 ArticleMaximumStock
        {
            get { return articleMaximumStock; }
            set
            {
                articleMaximumStock = value;
                OnPropertyChanged("ArticleMaximumStock");
            }
        }

        //[Sudhir.Jangra][GEOS2-5635]
        [NotMapped]
        [DataMember]
        public long ArticleIncoming
        {
            get { return articleIncoming; }
            set
            {
                articleIncoming = value;
                OnPropertyChanged("ArticleIncoming");
            }
        }

        //[Sudhir.Jangra][GEOS2-5635]
        [NotMapped]
        [DataMember]
        public long ArticleOutcoming
        {
            get { return articleOutcoming; }
            set
            {
                articleOutcoming = value;
                OnPropertyChanged("ArticleOutcoming");
            }
        }

        //[Sudhir.Jangra][GEOS2-5635]
        [NotMapped]
        [DataMember]
        public bool IsReviewValidated
        {
            get { return isReviewValidated; }
            set
            {
                isReviewValidated = value;
                OnPropertyChanged("IsReviewValidated");
            }
        }

        //[Sudhir.Jangra][GEOS2-5635]
        [NotMapped]
        [DataMember]
        public int ValidatedCount
        {
            get { return validatedCount; }
            set
            {
                validatedCount = value;
                OnPropertyChanged("ValidatedCount");
            }
        }

        //[pramod.misal][GEOS2-5473][18.07.2024]
        [Column("IdRevision")]
        [DataMember]
        public Int64 IdRevision
        {
            get
            {
                return idRevision;
            }

            set
            {
                idRevision = value;
                OnPropertyChanged("IdRevision");
            }
        }

        //[Sudhir.Jangra][GEOS2-5637]
        [NotMapped]
        [DataMember]
        public string UnitPriceWithSymbol
        {
            get { return unitPriceWithSymbol; }
            set
            {
                unitPriceWithSymbol = value;
                OnPropertyChanged("UnitPriceWithSymbol");
            }
        }
        //[Sudhir.Jangra][GEOS2-5637]
        [NotMapped]
        [DataMember]
        public string NumItemBarCodeText
        {
            get { return numItemBarcodeText; }
            set
            {
                numItemBarcodeText = value;
                OnPropertyChanged("NumItemBarCodeText");
            }
        }

        // [nsatpute][17-01-2025][GEOS2-6445]


        [NotMapped]
        [DataMember]
        public string InternalNotes
        {
            get { return internalNotes; }
            set
            {
                internalNotes = value;
                OnPropertyChanged("InternalNotes");
            }
        }

        [NotMapped]
        [DataMember]
        public double CostPrice
        {
            get { return costPrice; }
            set
            {
                costPrice = value;
                OnPropertyChanged("CostPrice");
            }
        }

        [NotMapped]
        [DataMember]
        public long PurchaseQuantity
        {
            get { return purchaseQuantity; }
            set
            {
                purchaseQuantity = value;
                OnPropertyChanged("PurchaseQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public bool ForcePurchaseQuantity
        {
            get { return forcePurchaseQuantity; }
            set
            {
                forcePurchaseQuantity = value;
                OnPropertyChanged("ForcePurchaseQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public Currency Currency
        {
            get { return currency; }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        [NotMapped]
        [DataMember]
        public int PackingSize
        {
            get { return packingSize; }
            set
            {
                packingSize = value;
                OnPropertyChanged("PackingSize");
            }
        }



        [NotMapped]
        [DataMember]
        public byte[] CurrencyIconBytes
        {
            get { return currencyIconBytes; }
            set
            {
                currencyIconBytes = value;
                OnPropertyChanged("CurrencyIconBytes");
            }
        }
        //[GEOS2-8965][pallavi.kale][28.11.2025]
        [NotMapped]
        [DataMember]
        public string WorkflowStatusName
        {
            get
            {
                return workflowStatusName;
            }
            set
            {
                workflowStatusName = value;
                OnPropertyChanged("WorkflowStatusName");
            }
        }
        //[GEOS2-8965][pallavi.kale][28.11.2025]
        [NotMapped]
        [DataMember]
        public string WorkflowStatusHtmlColor
        {
            get
            {
                return workflowStatusHtmlColor;
            }
            set
            {
                workflowStatusHtmlColor = value;
                OnPropertyChanged("WorkflowStatusHtmlColor");
            }
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
