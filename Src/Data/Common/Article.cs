using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Data;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    [Table("Article")]
    [DataContract]
    public class Article : ModelBase, IDisposable
    {
        #region Declaration
        string firstParent;
        string parentSecond;
        Warehouses warehouse;
        Single width;
        decimal weight;
        string taric;
        byte registerSerialNumber;
        string reference;
        sbyte publishOnWeb;
        string nCM_Code;
        DateTime? modifiedIn;
        Int32 modifiedBy;
        Single length;
        sbyte isPublic;
        byte isOrientativeVisualAid;
        sbyte isObsolete;
        sbyte isGlobalArticle;
        sbyte isGeneric;
        sbyte isFrequentlyUsed;
        byte isBatch;
        string internalNotes;
        string imagePath;
        Int32 idReplacementArticle;
        Int32 idCountry;
        Int64 idArticleType;
        Int32 idArticleSubfamily;
        Int64 idArticleCategory;
        Int32 idArticle;
        string hS_Code;
        Single height;
        string documentationComment;
        string description_zh;
        string description_ru;
        string description_ro;
        string description_pt;
        string description_fr;
        string description_es;
        string description;
        DateTime? createdIn;
        Int32 createdBy;
        DateTime? additionsModifiedIn;

        //Not Mapped
        double quantity;
        string location;
        DateTime? uploadedIn;
        Int64? idWareHouseDeliveryNoteItem;
        WarehousePurchaseOrder warehousePurchaseOrder;
        MyWarehouse myWarehouse;

        byte[] articleImageInBytes;
        double sellPrice;

        ArticleCategory articleCategory;
        ArticleBySupplier articleBySupplier;
        ArticlesStock articlesStock;
        ArticleSupplierType articelSupplierType;
        Country country;
        ArticleType articlesType;

        List<ArticleBySupplier> lstArticleBySupplier;
        List<RelatedArticle> lstRelatedArticle;
        List<ArticleDecomposition> lstArticleDecomposition;
        //DataTable dtArticleDecomposition;
        List<ArticlesStock> lstArticlesStock;
        List<ArticleDocument> lstArticleDocument;

        List<ArticleWarehouseLocations> lstArticleWarehouseLocations;
        string replacementArticle;

        ArticleWarehouseLocations articleWarehouseLocation;

        List<LogEntriesByArticle> logEntriesByArticles;
        bool isAddedArticleImage;
        bool isDeletedArticleImage;
        List<ManufacturersByArticle> lstmanufacturersByArticles;
        List<ArticleForecast> lstArticleForecast;
        DateTime? articleSleepDate;
        Int32? articleSleepDays;
        Int64? articleCurrentStock;
        bool isArticleInRefillExistInAnotherLocation;
        WarehouseLocation toWarehouseLocation;
        Int64 materialType;
        ImageSource articleImage;
        Int64 purchasedQty;
        Int64 deliveredQty;

        LookupValue family;
        List<ArticleComment> lstArticleComment;
        string articleComment;
        DateTime? articleCommentDateOfExpiry;

        double price;
        string priceWithSelectedCurrencySymbol;
        private bool isChecked;
        bool isUpdatedArticleFamily;
        bool isUpdatedArticleType;
        double discount;
        sbyte isCountRequiredAfterPicking;//[Sudhir.Jangra][GEOS2-4435][04/08/2023]
		//[nsatpute][29.08.2025][GEOS2-6505]
        Data.Common.Epc.LookupValue transportType;
        bool isSelected;
        #endregion

        #region Properties

        [Column("Width")]
        [DataMember]
        public Single Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

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
        [Column("Taric")]
        [DataMember]
        public string Taric
        {
            get { return taric; }
            set
            {
                taric = value;
                OnPropertyChanged("Taric");
            }
        }

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

        [Column("PublishOnWeb")]
        [DataMember]
        public sbyte PublishOnWeb
        {
            get { return publishOnWeb; }
            set
            {
                publishOnWeb = value;
                OnPropertyChanged("PublishOnWeb");
            }
        }

        [Column("NCM_Code")]
        [DataMember]
        public string NCM_Code
        {
            get { return nCM_Code; }
            set
            {
                nCM_Code = value;
                OnPropertyChanged("NCM_Code");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
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

        [Column("Length")]
        [DataMember]
        public Single Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }

        [Column("IsPublic")]
        [DataMember]
        public sbyte IsPublic
        {
            get { return isPublic; }
            set
            {
                isPublic = value;
                OnPropertyChanged("IsPublic");
            }
        }

        [Column("IsOrientativeVisualAid")]
        [DataMember]
        public byte IsOrientativeVisualAid
        {
            get { return isOrientativeVisualAid; }
            set
            {
                isOrientativeVisualAid = value;
                OnPropertyChanged("IsOrientativeVisualAid");
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

        [Column("IsGlobalArticle")]
        [DataMember]
        public sbyte IsGlobalArticle
        {
            get { return isGlobalArticle; }
            set
            {
                isGlobalArticle = value;
                OnPropertyChanged("IsGlobalArticle");
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

        [Column("IsFrequentlyUsed")]
        [DataMember]
        public sbyte IsFrequentlyUsed
        {
            get { return isFrequentlyUsed; }
            set
            {
                isFrequentlyUsed = value;
                OnPropertyChanged("IsFrequentlyUsed");
            }
        }

        [Column("IsBatch")]
        [DataMember]
        public byte IsBatch
        {
            get { return isBatch; }
            set
            {
                isBatch = value;
                OnPropertyChanged("IsBatch");
            }
        }

        [Column("InternalNotes")]
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

        [Column("IdReplacementArticle")]
        [DataMember]
        public Int32 IdReplacementArticle
        {
            get { return idReplacementArticle; }
            set
            {
                idReplacementArticle = value;
                OnPropertyChanged("IdReplacementArticle");
            }
        }

        [Column("IdCountry")]
        [DataMember]
        public Int32 IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

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

        [Column("IdArticleSubfamily")]
        [DataMember]
        public Int32 IdArticleSubfamily
        {
            get { return idArticleSubfamily; }
            set
            {
                idArticleSubfamily = value;
                OnPropertyChanged("IdArticleSubfamily");
            }
        }

        [Column("IdArticleCategory")]
        [DataMember]
        public Int64 IdArticleCategory
        {
            get { return idArticleCategory; }
            set
            {
                idArticleCategory = value;
                OnPropertyChanged("IdArticleCategory");
            }
        }

        [NotMapped]
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

        [Column("HS_Code")]
        [DataMember]
        public string HS_Code
        {
            get { return hS_Code; }
            set
            {
                hS_Code = value;
                OnPropertyChanged("HS_Code");
            }
        }

        [Column("Height")]
        [DataMember]
        public Single Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        [Column("DocumentationComment")]
        [DataMember]
        public string DocumentationComment
        {
            get { return documentationComment; }
            set
            {
                documentationComment = value;
                OnPropertyChanged("DocumentationComment");
            }
        }

        [Column("Description_zh")]
        [DataMember]
        public string Description_zh
        {
            get { return description_zh; }
            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }

        [Column("Description_ru")]
        [DataMember]
        public string Description_ru
        {
            get { return description_ru; }
            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru"); ;
            }
        }

        [Column("Description_ro")]
        [DataMember]
        public string Description_ro
        {
            get { return description_ro; }
            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }

        [Column("Description_pt")]
        [DataMember]
        public string Description_pt
        {
            get { return description_pt; }
            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
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

        [Column("CreatedIn")]
        [DataMember]
        public DateTime? CreatedIn
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

        [Column("AdditionsModifiedIn")]
        [DataMember]
        public DateTime? AdditionsModifiedIn
        {
            get { return additionsModifiedIn; }
            set
            {
                additionsModifiedIn = value;
                OnPropertyChanged("AdditionsModifiedIn");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public MyWarehouse MyWarehouse
        {
            get { return myWarehouse; }
            set
            {
                myWarehouse = value;
                OnPropertyChanged("MyWarehouse");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public double SellPrice
        {
            get { return sellPrice; }
            set
            {
                sellPrice = value;
                OnPropertyChanged("SellPrice");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleCategory ArticleCategory
        {
            get { return articleCategory; }
            set
            {
                articleCategory = value;
                OnPropertyChanged("ArticleCategory");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleBySupplier ArticleBySupplier
        {
            get { return articleBySupplier; }
            set
            {
                articleBySupplier = value;
                OnPropertyChanged("ArticleBySupplier");
            }
        }


        [NotMapped]
        [DataMember]
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }



        [NotMapped]
        [DataMember]
        public ArticlesStock ArticlesStock
        {
            get { return articlesStock; }
            set
            {
                articlesStock = value;
                OnPropertyChanged("ArticlesStock");
            }
        }


        [NotMapped]
        [DataMember]
        public ArticleType ArticlesType
        {
            get { return articlesType; }
            set
            {
                articlesType = value;
                OnPropertyChanged("ArticlesType");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ArticleBySupplier> LstArticleBySupplier
        {
            get { return lstArticleBySupplier; }
            set
            {
                lstArticleBySupplier = value;
                OnPropertyChanged("LstArticleBySupplier");
            }
        }


        //[NotMapped]
        //[DataMember]
        //public DataTable DtArticleDecomposition
        //{
        //    get { return dtArticleDecomposition; }
        //    set
        //    {
        //        dtArticleDecomposition = value;
        //        OnPropertyChanged("DtArticleDecomposition");
        //    }
        //}

        [NotMapped]
        [DataMember]
        public List<ArticleDecomposition> LstArticleDecomposition
        {
            get { return lstArticleDecomposition; }
            set
            {
                lstArticleDecomposition = value;
                OnPropertyChanged("LstArticleDecomposition");
            }
        }


        [NotMapped]
        [DataMember]
        public List<RelatedArticle> LstRelatedArticle
        {
            get { return lstRelatedArticle; }
            set
            {
                lstRelatedArticle = value;
                OnPropertyChanged("LstRelatedArticle");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ArticlesStock> LstArticlesStock
        {
            get { return lstArticlesStock; }
            set
            {
                lstArticlesStock = value;
                OnPropertyChanged("LstArticlesStock");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ArticleDocument> LstArticleDocument
        {
            get { return lstArticleDocument; }
            set
            {
                lstArticleDocument = value;
                OnPropertyChanged("LstArticleDocument");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ArticleWarehouseLocations> LstArticleWarehouseLocations
        {
            get { return lstArticleWarehouseLocations; }
            set
            {
                lstArticleWarehouseLocations = value;
                OnPropertyChanged("LstArticleWarehouseLocations");
            }
        }

        [NotMapped]
        [DataMember]
        public string ReplacementArticle
        {
            get { return replacementArticle; }
            set
            {
                replacementArticle = value;
                OnPropertyChanged("ReplacementArticle");
            }
        }

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

        [NotMapped]
        [DataMember]
        public List<LogEntriesByArticle> LogEntriesByArticles
        {
            get { return logEntriesByArticles; }
            set
            {
                logEntriesByArticles = value;
                OnPropertyChanged("LogEntriesByArticles");
            }
        }



        [NotMapped]
        [DataMember]
        public bool IsAddedArticleImage
        {
            get { return isAddedArticleImage; }
            set
            {
                isAddedArticleImage = value;
                OnPropertyChanged("IsAddedArticleImage");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsDeletedArticleImage
        {
            get { return isDeletedArticleImage; }
            set
            {
                isDeletedArticleImage = value;
                OnPropertyChanged("IsDeletedArticleImage");
            }
        }

        [NotMapped]
        [DataMember]
        public List<ManufacturersByArticle> LstManufacturersByArticles
        {
            get { return lstmanufacturersByArticles; }
            set
            {
                lstmanufacturersByArticles = value;
                OnPropertyChanged("LstManufacturersByArticles");
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

        [NotMapped]
        [DataMember]
        public DateTime? ArticleSleepDate
        {
            get { return articleSleepDate; }
            set
            {
                articleSleepDate = value;
                OnPropertyChanged("ArticleSleepDate");
            }
        }


        [NotMapped]
        [DataMember]
        public Int32? ArticleSleepDays
        {
            get { return articleSleepDays; }
            set
            {
                articleSleepDays = value;
                OnPropertyChanged("ArticleSleepDays");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64? ArticleCurrentStock
        {
            get { return articleCurrentStock; }
            set
            {
                articleCurrentStock = value;
                OnPropertyChanged("ArticleCurrentStock");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsArticleInRefillExistInAnotherLocation
        {
            get { return isArticleInRefillExistInAnotherLocation; }
            set
            {
                isArticleInRefillExistInAnotherLocation = value;
                OnPropertyChanged("IsArticleInRefillExistInAnotherLocation");
            }
        }

        [NotMapped]
        [DataMember]
        public WarehouseLocation ToWarehouseLocation
        {
            get { return toWarehouseLocation; }
            set
            {
                toWarehouseLocation = value;
                OnPropertyChanged("ToWarehouseLocation");
            }
        }

        [Column("MaterialType")]
        [DataMember]
        public Int64 MaterialType
        {
            get { return materialType; }
            set
            {
                materialType = value;
                OnPropertyChanged("MaterialType");
            }
        }

        [NotMapped]
        [DataMember]
        public ImageSource ArticleImage
        {
            get { return articleImage; }
            set
            {
                articleImage = value;
                OnPropertyChanged("ArticleImage");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 PurchasedQty
        {
            get { return purchasedQty; }
            set
            {
                purchasedQty = value;
                OnPropertyChanged("PurchasedQty");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 DeliveredQty
        {
            get { return deliveredQty; }
            set
            {
                deliveredQty = value;
                OnPropertyChanged("DeliveredQty");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue Family
        {
            get { return family; }
            set
            {
                family = value;
                OnPropertyChanged("Family");
            }
        }



        [NotMapped]
        [DataMember]
        public List<ArticleComment> LstArticleComment
        {
            get { return lstArticleComment; }
            set
            {
                lstArticleComment = value;
                OnPropertyChanged("LstArticleComment");
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
        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        [DataMember]
        [NotMapped]
        public string PriceWithSelectedCurrencySymbol
        {
            get
            {
                return priceWithSelectedCurrencySymbol;
            }

            set
            {
                priceWithSelectedCurrencySymbol = value;
                OnPropertyChanged("PriceWithSelectedCurrencySymbol");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdatedArticleFamily
        {
            get { return isUpdatedArticleFamily; }
            set
            {
                isUpdatedArticleFamily = value;
                OnPropertyChanged("IsUpdatedArticleFamily");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdatedArticleType
        {
            get { return isUpdatedArticleType; }
            set
            {
                isUpdatedArticleType = value;
                OnPropertyChanged("IsUpdatedArticleType");
            }
        }

        [NotMapped]
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

        #region GEOS2-3532
        //Shubham[skadam] GEOS2-3532 [QUALITY_INSPECTION] Ignore the Items in TRANSIT without “Product Inspection” OK 29 12 2022
        sbyte isInspectionRequired;
        [Column("IsInspectionRequired")]
        [DataMember]
        public sbyte IsInspectionRequired
        {
            get { return isInspectionRequired; }
            set
            {
                isInspectionRequired = value;
                OnPropertyChanged("IsInspectionRequired");
            }
        }
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
        #endregion

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
        //[rdixit][29.08.2024][GEOS2-5410]
        [DataMember]
        public string FirstParent
        {
            get { return firstParent; }
            set
            {
                firstParent = value;
                OnPropertyChanged("FirstParent");
            }
        }

        [DataMember]
        public string ParentSecond
        {
            get { return parentSecond; }
            set
            {
                parentSecond = value;
                OnPropertyChanged("ParentSecond");
            }
        }
		//[nsatpute][29.08.2025][GEOS2-6505]
        [NotMapped]
        [DataMember]
        public Data.Common.Epc.LookupValue TransportType
        {
            get { return transportType; }
            set
            {
                transportType = value;
                OnPropertyChanged("TransportType");
            }
        }
		//[nsatpute][29.08.2025][GEOS2-6505]
        [NotMapped]
        [DataMember]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        #endregion

        #region Constructor

        public Article()
        {
        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        //public override object Clone()
        //{
        //    Article article = (Article)this.MemberwiseClone();
        //    article.WarehousePurchaseOrder = (WarehousePurchaseOrder)WarehousePurchaseOrder.Clone();

        //    return article;
        //}
		//[nsatpute][29.08.2025][GEOS2-6505]
        public override object Clone()
        {
            Article article = (Article)this.MemberwiseClone();

            article.WarehousePurchaseOrder = WarehousePurchaseOrder != null
                ? (WarehousePurchaseOrder)WarehousePurchaseOrder.Clone()
                : null;
            if (this.MyWarehouse != null)
            {
                article.MyWarehouse = (MyWarehouse)this.MyWarehouse.Clone();
            }

            return article;
        }


        #endregion
    }
}
