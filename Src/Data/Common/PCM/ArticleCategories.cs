using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ArticleCategories : ModelBase, IDisposable
    {

        #region Fields

        uint idArticleCategory;
        string name;
        UInt64? parent;
        Int64 isLeaf;
        uint position;
        string name_es;
        string name_fr;
        string name_pt;
        string taricCode;
        string ncm_Code;
        string hs_Code;
        bool isChecked;

        string keyName;
        string parentName;

        int article_count;
        int parent_count;
        string nameWithArticleCount;
        int article_count_original;
        #endregion

        #region Constructor
        public ArticleCategories()
        {

        }
        #endregion

        #region Properties

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
        public string TaricCode
        {
            get { return taricCode; }
            set
            {
                taricCode = value;
                OnPropertyChanged("TaricCode");
            }
        }

        [DataMember]
        public string NCM_Code
        {
            get { return ncm_Code; }
            set
            {
                ncm_Code = value;
                OnPropertyChanged("NCM_Code");
            }
        }

        [DataMember]
        public string HS_Code
        {
            get { return hs_Code; }
            set
            {
                hs_Code = value;
                OnPropertyChanged("HS_Code");
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
        public int Parent_count
        {
            get
            {
                return parent_count;
            }

            set
            {
                parent_count = value;
                OnPropertyChanged("Parent_count");
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
