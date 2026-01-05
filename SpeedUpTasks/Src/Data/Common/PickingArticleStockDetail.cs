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
   public class PickingArticleStockDetail : ModelBase, IDisposable
    {
        #region Fields

      
        Int64 idWarehouseLocation;
     
        Int64? articleCurrentStock;
        Int64? currentStock;
        Int64? minimumStock;
        Int64? maximumStock;
        Int64? idWareHouseDeliveryNoteItem;
        Int16 idCurrency;
        Int64? articleLocationMinimumStock;
        Int64? articleLocationMaximumStock;
        double? costPrice;
        double? unitPrice;
        int idArticle;
      
        DateTime? uploadedIn;
      
        WarehouseLocation warehouseLocation;
       
        WarehouseDeliveryNote warehouseDeliveryNote;
        Int64? idCountryGroup;
        Article article;
        long? articleLocationPosition;
        
        #endregion

        #region Properties


        public long IdWarehouseLocation
        {
            get
            {
                return idWarehouseLocation;
            }

            set
            {
                idWarehouseLocation = value;
                OnPropertyChanged("IdWarehouseLocation");
            }
        }

        public long? IdWareHouseDeliveryNoteItem
        {
            get
            {
                return idWareHouseDeliveryNoteItem;
            }

            set
            {
                idWareHouseDeliveryNoteItem = value;
                OnPropertyChanged("IdWareHouseDeliveryNoteItem");
            }
        }

        public int IdArticle
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
       
        public Int64? CurrentStock
        {
            get
            {
                return currentStock;
            }

            set
            {
                currentStock = value;
                OnPropertyChanged("CurrentStock");
            }
        }

      
        public double? CostPrice
        {
            get
            {
                return costPrice;
            }

            set
            {
                costPrice = value;
                OnPropertyChanged("CostPrice");
            }
        }

        public double? UnitPrice
        {
            get
            {
                return unitPrice;
            }

            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }

        public long? MinimumStock
        {
            get
            {
                return minimumStock;
            }

            set
            {
                minimumStock = value;
                OnPropertyChanged("MinimumStock");
            }
        }

        public long? MaximumStock
        {
            get
            {
                return maximumStock;
            }

            set
            {
                maximumStock = value;
                OnPropertyChanged("MaximumStock");
            }
        }

        public long? ArticleLocationMinimumStock
        {
            get
            {
                return articleLocationMinimumStock;
            }

            set
            {
                articleLocationMinimumStock = value;
                OnPropertyChanged("ArticleLocationMinimumStock");
            }
        }

        public long? ArticleLocationMaximumStock
        {
            get
            {
                return articleLocationMaximumStock;
            }

            set
            {
                articleLocationMaximumStock = value;
                OnPropertyChanged("ArticleLocationMaximumStock");
            }
        }

        public DateTime? UploadedIn
        {
            get
            {
                return uploadedIn;
            }

            set
            {
                uploadedIn = value;
                OnPropertyChanged("UploadedIn");
            }
        }

      
        public Int64? ArticleCurrentStock
        {
            get
            {
                return articleCurrentStock;
            }

            set
            {
                articleCurrentStock = value;
                OnPropertyChanged("ArticleCurrentStock");
            }
        }

    

        public WarehouseLocation WarehouseLocation
        {
            get
            {
                return warehouseLocation;
            }

            set
            {
                warehouseLocation = value;
                OnPropertyChanged("WarehouseLocation");
            }
        }


        [DataMember]
        [NotMapped]
        public WarehouseDeliveryNote WarehouseDeliveryNote
        {
            get { return warehouseDeliveryNote; }
            set
            {
                warehouseDeliveryNote = value;
                OnPropertyChanged("WarehouseDeliveryNote");
            }
        }

        [DataMember]
        [NotMapped]
        public Int64? IdCountryGroup
        {
            get { return idCountryGroup; }
            set
            {
                idCountryGroup = value;
                OnPropertyChanged("IdCountryGroup");
            }
        }

        [DataMember]
        [NotMapped]
        public Article Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged("Article");
            }
        }

        [DataMember]
        [NotMapped]
        public Int16 IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [DataMember]
        [NotMapped]
        public long? ArticleLocationPosition
        {
            get
            {
                return articleLocationPosition;
            }

            set
            {
                articleLocationPosition = value;
                OnPropertyChanged("ArticleLocationPosition");
            }
        }

      


        #endregion

        #region Constructor
        public PickingArticleStockDetail()
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
