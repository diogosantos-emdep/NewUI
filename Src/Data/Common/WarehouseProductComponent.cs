using Emdep.Geos.Data.Common.SCM;
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
    [Table("WarehouseProduct")]
    [DataContract]
    public class WarehouseProductComponent : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idComponent;
        Int64 idWarehouseProduct;
        int idArticle;
        Int64 parent;
        double quantity;
        Article article;
        string number;
        ArticleProduct articlewarehouseProduct;
        private List<WarehouseProductComponent> components;
        #endregion

        #region Constructor

        public WarehouseProductComponent()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdComponent")]
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

        [Column("IdWarehouseProduct")]
        [DataMember]
        public long IdWarehouseProduct
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
        public int IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
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

        [Column("Quantity")]
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
        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleProduct ArticlewarehouseProduct
        {
            get
            {
                return articlewarehouseProduct;
            }
            set
            {
                articlewarehouseProduct = value;
                OnPropertyChanged("ArticlewarehouseProduct");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WarehouseProductComponent> Components
        {
            get
            {
                return components;
            }
            set
            {
                components = value;
                OnPropertyChanged("Components");
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
