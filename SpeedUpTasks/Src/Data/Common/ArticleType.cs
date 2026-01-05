using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("articletype")]
    [DataContract]
    public class ArticleType : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idArticleType;
        string articleTypeName;
        string name;
        string name_fr;
        string name_es;
        string name_pt;
        string name_zh;
        string name_ro;
        string name_ru;
        Int64 sortOrder;

        #endregion

        #region Properties

        [Key]
        [Column("idArticleType")]
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

        [Column("Name")]
        [DataMember]
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("ArticleTypeName")]
        [DataMember]
        public String ArticleTypeName
        {
            get { return articleTypeName; }
            set
            {
                articleTypeName = value;
                OnPropertyChanged("ArticleTypeName");
            }
        }

        [Column("Name_es")]
        [DataMember]
        public String Name_es
        {
            get { return name_es; }
            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [Column("Name_fr")]
        [DataMember]
        public String Name_fr
        {
            get { return name_fr; }
            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [Column("Name_pt")]
        [DataMember]
        public String Name_pt
        {
            get { return name_pt; }
            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }

        [Column("Name_zh")]
        [DataMember]
        public String Name_zh
        {
            get { return name_zh; }
            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [Column("Name_ro")]
        [DataMember]
        public String Name_ro
        {
            get { return name_ro; }
            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }



        [Column("Name_ru")]
        [DataMember]
        public String Name_ru
        {
            get { return name_ru; }
            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
            }
        }

        [Column("SortOrder")]
        [DataMember]
        public Int64 SortOrder
        {
            get { return sortOrder; }
            set
            {
                sortOrder = value;
                OnPropertyChanged("SortOrder");
            }
        }

        #endregion

        #region Constructor

        public ArticleType()
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
