using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class StockBySupplier : ModelBase, IDisposable
    {
        #region Fields
        Int32 idArticle;
        Int32 idArticleSupplier;
        string reference;
        string description;
        string articleCategoryName;
        string supplierName;
        string supplierGroup;
        Int64 stockQty;
        double price;
        string valueWithSelectedCurrencySymbol;
        #endregion

        #region Constructor
        public StockBySupplier()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdArticle")]
        [DataMember]
        public Int32 IdArticle
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
        [Column("IdArticleSupplier")]
        [DataMember]
        public Int32 IdArticleSupplier
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

        [Column("Reference")]
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

        [Column("Description")]
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

        [Column("ArticleCategoryName")]
        [DataMember]
        public string ArticleCategoryName
        {
            get
            {
                return articleCategoryName;
            }

            set
            {
                articleCategoryName = value;
                OnPropertyChanged("ArticleCategoryName");
            }
        }

        [Column("SupplierName")]
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

        [Column("SupplierGroup")]
        [DataMember]
        public string SupplierGroup
        {
            get
            {
                return supplierGroup;
            }

            set
            {
                supplierGroup = value;
                OnPropertyChanged("SupplierGroup");
            }
        }

        [Column("StockQty")]
        [DataMember]
        public Int64 StockQty
        {
            get
            {
                return stockQty;
            }

            set
            {
                stockQty = value;
                OnPropertyChanged("StockQty");
            }
        }

        [NotMapped]
        [DataMember]
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        [NotMapped]
        [DataMember]
        public string ValueWithSelectedCurrencySymbol
        {
            get { return valueWithSelectedCurrencySymbol; }
            set
            {
                valueWithSelectedCurrencySymbol = value;
                OnPropertyChanged("ValueWithSelectedCurrencySymbol");
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
