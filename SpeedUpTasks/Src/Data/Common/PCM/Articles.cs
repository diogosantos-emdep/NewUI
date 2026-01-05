using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class Articles : ModelBase, IDisposable
    {
        #region Declaration
        UInt32 idArticle;
        string reference;
        string description;
        string description_es;
        string description_fr;
        string description_ro;
        string description_zh;
        string description_pt;
        string description_ru;
        uint idArticleCategory;

        string imagePath;

        string supplierName;

        decimal weight;
        float length;
        float width;
        float height;

        //pcm status
        string pcmStatus;

        //warehouse status
        Int64 isObsolete;
        WarehouseStatus warehouseStatus;

        //delete
        Int64 isEnabled;
        
        ArticleCategories articleCategory;
        uint idPCMArticleCategory;
        PCMArticleCategory pcmArticleCategory;

        UInt32 idArticleSupplier;

        byte[] articleImageInBytes;
        string visibility;

        Int32? idPCMStatus;

        List<ArticleCompatibility> articleCompatibilityList;
        List<PCMArticleLogEntry> pCMArticleLogEntiryList;
        List<PCMArticleImage> pCMArticleImageList;
        List<ArticleDocument> pCMArticleAttachmentList;
        bool isPCMArticle;
        bool isUpdatedRow;

        uint idPCMArticle;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        List<PCMArticleCategory> categoryMenulist;
        PCMArticleCategory selectedCategory;
        bool isRtfText;
        string pCMDescription;
        #endregion

        #region Properties
        [DataMember]
        public bool IsPCMArticle
        {
            get
            {
                return isPCMArticle;
            }

            set
            {
                isPCMArticle = value;
                OnPropertyChanged("IsPCMArticle");
            }
        }

        [DataMember]
        public uint IdArticle
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

        [DataMember]
        public string Description_es
        {
            get
            {
                return description_es;
            }

            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }

        [DataMember]
        public string Description_fr
        {
            get
            {
                return description_fr;
            }

            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }

        [DataMember]
        public string Description_ro
        {
            get
            {
                return description_ro;
            }

            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }

        [DataMember]
        public string Description_zh
        {
            get
            {
                return description_zh;
            }

            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }

        [DataMember]
        public string Description_pt
        {
            get
            {
                return description_pt;
            }

            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
            }
        }

        [DataMember]
        public string Description_ru
        {
            get
            {
                return description_ru;
            }

            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru");
            }
        }

        [DataMember]
        public uint IdArticleCategory
        {
            get
            {
                return idArticleCategory;
            }

            set
            {
                idArticleCategory = value;
                OnPropertyChanged("IdArticleCategory");
            }
        }

        [DataMember]
        public string SupplierName
        {
            get
            {
                return supplierName;
            }

            set
            {
                supplierName = value;
                OnPropertyChanged("SupplierName");
            }
        }

        [DataMember]
        public decimal Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }

        [DataMember]
        public float Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }

        [DataMember]
        public float Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        [DataMember]
        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        [DataMember]
        public string PCMStatus
        {
            get
            {
                return pcmStatus;
            }

            set
            {
                pcmStatus = value;
                OnPropertyChanged("PCMStatus");
            }
        }

        [DataMember]
        public WarehouseStatus WarehouseStatus
        {
            get
            {
                return warehouseStatus;
            }

            set
            {
                warehouseStatus = value;
                OnPropertyChanged("WarehouseStatus");
            }
        }

        [DataMember]
        public long IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        [DataMember]
        public ArticleCategories ArticleCategory
        {
            get
            {
                return articleCategory;
            }

            set
            {
                articleCategory = value;
                OnPropertyChanged("ArticleCategory");
            }
        }

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

        [DataMember]
        public uint IdPCMArticleCategory
        {
            get
            {
                return idPCMArticleCategory;
            }

            set
            {
                idPCMArticleCategory = value;
                OnPropertyChanged("IdPCMArticleCategory");
            }
        }

        [DataMember]
        public PCMArticleCategory PcmArticleCategory
        {
            get
            {
                return pcmArticleCategory;
            }

            set
            {
                pcmArticleCategory = value;
                OnPropertyChanged("PcmArticleCategory");
            }
        }

        [DataMember]
        public uint IdArticleSupplier
        {
            get
            {
                return idArticleSupplier;
            }

            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [DataMember]
        public long IsObsolete
        {
            get
            {
                return isObsolete;
            }

            set
            {
                isObsolete = value;
                OnPropertyChanged("IsObsolete");
            }
        }
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

        [DataMember]
        public string Visibility
        {
            get
            {
                return visibility;
            }

            set
            {
                visibility = value;
                OnPropertyChanged("ArticleImageInBytes");
            }
        }

        [DataMember]
        public int? IdPCMStatus
        {
            get
            {
                return idPCMStatus;
            }

            set
            {
                idPCMStatus = value;
                OnPropertyChanged("IdPCMStatus");
            }
        }

        [DataMember]
        public List<ArticleCompatibility> ArticleCompatibilityList
        {
            get
            {
                return articleCompatibilityList;
            }

            set
            {
                articleCompatibilityList = value;
                OnPropertyChanged("ArticleCompatibilityList");
            }
        }

        [DataMember]
        public List<PCMArticleLogEntry> PCMArticleLogEntiryList
        {
            get
            {
                return pCMArticleLogEntiryList;
            }

            set
            {
                pCMArticleLogEntiryList = value;
                OnPropertyChanged("PCMArticleLogEntiryList");
            }
        }
        [DataMember]
        public List<PCMArticleImage> PCMArticleImageList
        {
            get
            {
                return pCMArticleImageList;
            }

            set
            {
                pCMArticleImageList = value;
                OnPropertyChanged("PCMArticleImageList");
            }
        }

        [DataMember]
        public List<ArticleDocument> PCMArticleAttachmentList
        {
            get
            {
                return pCMArticleAttachmentList;
            }

            set
            {
                pCMArticleAttachmentList = value;
                OnPropertyChanged("PCMArticleAttachmentList");
            }
        }

        [DataMember]
        public bool IsUpdatedRow
        {
            get
            {
                return isUpdatedRow;
            }

            set
            {
                isUpdatedRow = value;
                OnPropertyChanged("IsUpdatedRow");
            }
        }

        [DataMember]
        public uint IdPCMArticle
        {
            get
            {
                return idPCMArticle;
            }

            set
            {
                idPCMArticle = value;
                OnPropertyChanged("IdPCMArticle");
            }
        }

        [DataMember]
        public uint IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public uint? IdModifier
        {
            get
            {
                return idModifier;
            }

            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }

            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public List<PCMArticleCategory> CategoryMenulist
        {
            get
            {
                return categoryMenulist;
            }

            set
            {
                categoryMenulist = value;
                OnPropertyChanged("CategoryMenulist");
            }
        }

        [DataMember]
        public PCMArticleCategory SelectedCategory
        {
            get
            {
                return selectedCategory;
            }

            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        [DataMember]
        public bool IsRtfText
        {
            get
            {
                return isRtfText;
            }

            set
            {
                isRtfText = value;
                OnPropertyChanged("IsRtfText");
            }
        }

        [DataMember]
        public string PCMDescription
        {
            get
            {
                return pCMDescription;
            }

            set
            {
                pCMDescription = value;
                OnPropertyChanged("PCMDescription");
            }
        }
        #endregion

        #region Constructor

        public Articles()
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
            Articles articles = (Articles)this.MemberwiseClone();

            if (ArticleCategory != null)
                articles.ArticleCategory = (ArticleCategories)this.ArticleCategory.Clone();

            if (PcmArticleCategory != null)
                articles.PcmArticleCategory = (PCMArticleCategory)this.PcmArticleCategory.Clone();

            if (ArticleCompatibilityList != null)
                articles.ArticleCompatibilityList = ArticleCompatibilityList.Select(x => (ArticleCompatibility)x.Clone()).ToList();

            if (PCMArticleImageList != null)
                articles.PCMArticleImageList = PCMArticleImageList.Select(x => (PCMArticleImage)x.Clone()).ToList();

            if (PCMArticleLogEntiryList != null)
                articles.PCMArticleLogEntiryList = PCMArticleLogEntiryList.Select(x => (PCMArticleLogEntry)x.Clone()).ToList();

            if (PCMArticleAttachmentList != null)
                articles.PCMArticleAttachmentList = PCMArticleAttachmentList.Select(x => (ArticleDocument)x.Clone()).ToList();

            return articles;
        }

        public override string ToString()
        {
            return Reference + "-" + Description;
        }
        #endregion
    }
}
