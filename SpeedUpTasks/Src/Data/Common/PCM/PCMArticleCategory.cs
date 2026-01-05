using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class PCMArticleCategory : ModelBase, IDisposable
    {

        #region Fields

        uint idPCMArticleCategory;
        string name;
        UInt64? parent;
        Int64 isLeaf;
        uint position;
        string name_es;
        string name_fr;
        string name_pt;

        bool isChecked;

        string keyName;
        string parentName;

        int article_count;

        string nameWithArticleCount;

        string name_ro;
        string name_zh;
        string name_ru;
        string description;
        string description_es;
        string description_fr;
        string description_ro;
        string description_zh;
        string description_pt;
        string description_ru;
        uint? idArticleCategory;

        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;

        ArticleCategories articleCategory;


        UInt32 idArticle;
        string reference;
        int sequence;
        int article_count_original;
        uint idPCMArticle;

        string imagePath;
        byte[] pCMArticleCategoryImageInByte;
        #endregion

        #region Constructor
        public PCMArticleCategory()
        {

        }
        #endregion

        #region Properties

        [DataMember]
        public uint IdPCMArticleCategory
        {
            get { return idPCMArticleCategory; }
            set
            {
                idPCMArticleCategory = value;
                OnPropertyChanged("IdPCMArticleCategory");
            }
        }

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

        [DataMember]
        public UInt64? Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }

        [DataMember]
        public Int64 IsLeaf
        {
            get { return isLeaf; }
            set
            {
                isLeaf = value;
                OnPropertyChanged("IsLeaf");
            }
        }

        [DataMember]
        public uint Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [DataMember]
        public string Name_es
        {
            get { return name_es; }
            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [DataMember]
        public string Name_fr
        {
            get { return name_fr; }
            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [DataMember]
        public string Name_pt
        {
            get { return name_pt; }
            set
            {
                name_pt = value;
                OnPropertyChanged("IdArticleCategory");
            }
        }

        [DataMember]
        public string KeyName
        {
            get
            {
                return keyName;
            }

            set
            {
                keyName = value;
                OnPropertyChanged("KeyName");
            }
        }

        [DataMember]
        public string ParentName
        {
            get
            {
                return parentName;
            }

            set
            {
                parentName = value;
                OnPropertyChanged("ParentName");
            }
        }

        [DataMember]
        public int Article_count
        {
            get
            {
                return article_count;
            }

            set
            {
                article_count = value;
                OnPropertyChanged("Article_count");
            }
        }

        [DataMember]
        public string NameWithArticleCount
        {
            get
            {
                return nameWithArticleCount;
            }

            set
            {
                nameWithArticleCount = value;
                OnPropertyChanged("NameWithArticleCount");
            }
        }

        [DataMember]
        public string Name_ro
        {
            get
            {
                return name_ro;
            }

            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }

        [DataMember]
        public string Name_zh
        {
            get
            {
                return name_zh;
            }

            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [DataMember]
        public string Name_ru
        {
            get
            {
                return name_ru;
            }

            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
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
        public uint? IdArticleCategory
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
        public int Sequence
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

        [DataMember]
        public int Article_count_original
        {
            get
            {
                return article_count_original;
            }

            set
            {
                article_count_original = value;
                OnPropertyChanged("Article_count_original");
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
        public byte[] PCMArticleCategoryImageInByte
        {
            get
            {
                return pCMArticleCategoryImageInByte;
            }

            set
            {
                pCMArticleCategoryImageInByte = value;
                OnPropertyChanged("PCMArticleCategoryImageInByte");
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
            PCMArticleCategory pcmArticleCategory = (PCMArticleCategory)this.MemberwiseClone();

            if (ArticleCategory != null)
                pcmArticleCategory.ArticleCategory = (ArticleCategories)this.ArticleCategory.Clone();

            return pcmArticleCategory;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
