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

    //public class BaseObject : ModelBase, IDisposable
    //{
    //    string id;
    //    string name;
    //    bool isChecked;

    //    public string BaseName
    //    {
    //        get { return name; }
    //        set
    //        {
    //            if (BaseName == value)
    //                return;
    //            name = value;
    //            OnPropertyChanged("BaseName");
    //        }
    //    }

    //    public bool IsChecked
    //    {
    //        get { return isChecked; }
    //        set
    //        {
    //            isChecked = value;
    //            OnPropertyChanged("IsChecked");
    //        }
    //    }

    //    public string Id
    //    {
    //        get { return id; }
    //        set
    //        {
    //            id = value;
    //            OnPropertyChanged("Id");
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        GC.SuppressFinalize(this);
    //    }
    //}


    [Table("articlecategories")]
    [DataContract]
    public class ArticleCategory : ModelBase, IDisposable
    {
        #region Fields

        Int64 idArticleCategory;
        string name;
        Int64 parent;
        byte isLeaf;
        Int64 position;
        string name_es;
        string name_fr;
        string name_pt;
        string taricCode;
        string ncm_Code;
        string hs_Code;
        bool isChecked;

        #endregion

        #region Constructor
        public ArticleCategory()
        {

        }
        #endregion

        #region Properties

        [Key]
        [Column("IdArticleCategory")]
        [DataMember]
        public long IdArticleCategory
        {
            get { return idArticleCategory; }
            set
            {
                idArticleCategory = value;
                OnPropertyChanged("IdArticleCategory");
            }
        }

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

        [Column("Parent")]
        [DataMember]
        public long Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }

        [Column("IsLeaf")]
        [DataMember]
        public byte IsLeaf
        {
            get { return isLeaf; }
            set
            {
                isLeaf = value;
                OnPropertyChanged("IsLeaf");
            }
        }

        [Column("Position")]
        [DataMember]
        public long Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [Column("Name_es")]
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

        [Column("Name_fr")]
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

        [Column("Name_pt")]
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

        [Column("TaricCode")]
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

        [Column("NCM_Code")]
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

        [Column("HS_Code")]
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
