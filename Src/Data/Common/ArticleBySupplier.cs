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
    [Table("articlesbysupplier")]
    [DataContract]
    public class ArticleBySupplier : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idArticle;
        Int64 idArticleSupplier;
        string reference;
        byte isTheFavorite;
        double basePrice;
        double costPrice;
        double discount;
        double iva;
        Int64 purchaseQty;
        string description;
        byte idCurrency;
        byte fixedPrice;
        byte forcePurchaseQty;
        double overCost;

        Article article;
        ArticleSupplier articleSupplier;

        Int64 idChild;
        Int64 idParent;
        bool isChecked;
        Currency currency;
        string curSymbol;    //[GEOS2-3441][sshegaonkar][24.01.2023]

        #endregion

        #region Properties

        [Key]
        [Column("IdArticle")]
        [DataMember]
        public long IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [Column("IdArticleSupplier")]
        [DataMember]
        public long IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

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

        [Column("IsTheFavorite")]
        [DataMember]
        public byte IsTheFavorite
        {
            get { return isTheFavorite; }
            set
            {
                isTheFavorite = value;
                OnPropertyChanged("IsTheFavorite");

            }
        }

        [Column("BasePrice")]
        [DataMember]
        public double BasePrice
        {
            get { return basePrice; }
            set
            {
                basePrice = value;
                OnPropertyChanged("BasePrice");
            }
        }

        [Column("CostPrice")]
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

        [Column("Discount")]
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

        [Column("IVA")]
        [DataMember]
        public double IVA
        {
            get { return iva; }
            set
            {
                iva = value;
                OnPropertyChanged("IVA");
            }
        }

        [Column("PurchaseQty")]
        [DataMember]
        public long PurchaseQty
        {
            get { return purchaseQty; }
            set
            {
                purchaseQty = value;
                OnPropertyChanged("PurchaseQty");
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

        [Column("idCurrency")]
        [DataMember]
        public byte IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [Column("FixedPrice")]
        [DataMember]
        public byte FixedPrice
        {
            get { return fixedPrice; }
            set
            {
                fixedPrice = value;
                OnPropertyChanged("FixedPrice");
            }
        }

        [Column("ForcePurchaseQty")]
        [DataMember]
        public byte ForcePurchaseQty
        {
            get { return forcePurchaseQty; }
            set
            {
                forcePurchaseQty = value;
                OnPropertyChanged("ForcePurchaseQty");
            }
        }

        [Column("OverCost")]
        [DataMember]
        public double OverCost
        {
            get { return overCost; }
            set
            {
                overCost = value;
                OnPropertyChanged("OverCost");
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
        public ArticleSupplier ArticleSupplier
        {
            get { return articleSupplier; }
            set
            {
                articleSupplier = value;
                OnPropertyChanged("ArticleSupplier");
            }
        }

        [NotMapped]
        [DataMember]
        public long IdChild
        {
            get { return idChild; }
            set
            {
                idChild = value;
                OnPropertyChanged("IdChild");
            }
        }

        [NotMapped]
        [DataMember]
        public long IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
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
        public Currency Currency
        {
            get { return currency; }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }


        //[GEOS2-3441][sshegaonkar][24.01.2023]
        [DataMember]
        public string CurSymbol
        {
            get { return curSymbol; }
            set
            {
                curSymbol = value;
                OnPropertyChanged("CurSymbol");
            }
        }
        #endregion

        #region Constructor

        public ArticleBySupplier()
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
