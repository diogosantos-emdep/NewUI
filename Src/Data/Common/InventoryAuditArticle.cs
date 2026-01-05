using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class InventoryAuditArticle : ModelBase, IDisposable
    {
        #region Fields
        bool isArticleChecked;
        int idWarehouseInventoryAuditArticle;
        string isAuditedYesNo;
        int isAudited;
        long idWarehouseInventoryAudit;
        uint idArticleCategory;
        string name;
        UInt64? parent;
        Int64 isLeaf;
        uint position;
        string inuse;
        bool isChecked;
        string keyName;
        string parentName;

        int article_count;
        string nameWithArticleCount;
        string description;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        UInt32 idArticle;
        string reference;
        int sequence;
        int article_count_original;
        string categoryName;
        string statusAbbreviation;
        string statusHtmlColor;
        #endregion

        #region Properties

        [DataMember]
        public int IsAudited
        {
            get
            {
                return isAudited;
            }

            set
            {

                isAudited = value;
                OnPropertyChanged("IsAudited");
            }
        }
        [DataMember]
        public string IsAuditedYesNo
        {
            get
            {
                return isAuditedYesNo;
            }

            set
            {

                isAuditedYesNo = value;
                OnPropertyChanged("IsAuditedYesNo");
            }
        }
        [DataMember]
        public int IdWarehouseInventoryAuditArticle
        {
            get
            {
                return idWarehouseInventoryAuditArticle;
            }

            set
            {

                idWarehouseInventoryAuditArticle = value;
                OnPropertyChanged("IdWarehouseInventoryAuditArticle");
            }
        }
        
       [DataMember]
        public long IdWarehouseInventoryAudit
        {
            get
            {
                return idWarehouseInventoryAudit;
            }

            set
            {

                idWarehouseInventoryAudit = value;
                OnPropertyChanged("IdWarehouseInventoryAudit");
            }
        }
        [DataMember]
        public uint IdArticleCategory
        {
            get { return idArticleCategory; }
            set
            {
                idArticleCategory = value;
                OnPropertyChanged("IdArticleCategory");
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
        public string CategoryName
        {
            get
            {
                return categoryName;
            }

            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }

        [DataMember]
        public string StatusAbbreviation
        {
            get
            {
                return statusAbbreviation;
            }

            set
            {
                statusAbbreviation = value;
                OnPropertyChanged("StatusAbbreviation");
            }
        }
        [DataMember]
        public bool IsArticleChecked
        {
            get
            {
                return isArticleChecked;
            }

            set
            {
                isArticleChecked = value;
                OnPropertyChanged("IsArticleChecked");
            }
        }

        List<InventoryAuditLocation> inventoryAuditLocation;

        [DataMember]
        public List<InventoryAuditLocation> InventoryAuditLocation
        {
            get
            {
                return inventoryAuditLocation;
            }

            set
            {
                inventoryAuditLocation = value;
                OnPropertyChanged("InventoryAuditLocation");
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

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
