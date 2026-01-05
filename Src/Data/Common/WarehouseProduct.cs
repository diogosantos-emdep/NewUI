using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
namespace Emdep.Geos.Data.Common
{

    [Table("WarehouseProduct")]
    [DataContract]
    public class WarehouseProduct : ModelBase, IDisposable
    {
        Int64 parentMultiplier;
        Int64 parent;
        Int64 idWarehouseProduct;
        Int32 idArticle;
        Int64 downloadedQty;
        string description;
        Article article;
        ArticleRefillDetail refill;//[sudhir.jangra][GEOS2-3959][07/11/2022]
        Int64 idComponent;
        Int64? parentIdArticleType;
        Int32 idParentOfComponent;

        #region Constructor
        public WarehouseProduct()
        {

        }
        #endregion

        #region Properties


        [Column("ParentMultiplier")]
        [DataMember]
        public Int64 ParentMultiplier
        {
            get { return parentMultiplier; }
            set
            {
                parentMultiplier = value;
                OnPropertyChanged("ParentMultiplier");
            }
        }


        [Column("Parent")]
        [DataMember]
        public Int64 Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                OnPropertyChanged("Parent");
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


        [Column("DownloadedQty")]
        [DataMember]
        public Int64 DownloadedQty
        {
            get { return downloadedQty; }
            set
            {
                downloadedQty = value;
                OnPropertyChanged("DownloadedQty");
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

        [NotMapped]
        [DataMember]
        public Article Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged("Article");
            }
        }

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


        [NotMapped]
        [DataMember]
        public Int64? ParentIdArticleType
        {
            get { return parentIdArticleType; }
            set
            {
                parentIdArticleType = value;
                OnPropertyChanged("ParentIdArticleType");
            }
        }
        //[sudhir.jangra][GEOS2-3959][07/11/2022]
        [NotMapped]
        [DataMember]
        public ArticleRefillDetail Refill
        {
            get { return refill; }
            set
            {
                refill = value;
                OnPropertyChanged("Refill");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdParentOfComponent
        {
            get { return idParentOfComponent; }
            set
            {
                idParentOfComponent = value;
                OnPropertyChanged("IdParentOfComponent");
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
