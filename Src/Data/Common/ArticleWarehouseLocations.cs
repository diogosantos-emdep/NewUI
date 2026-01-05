using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{

    [Table("article_warehouse_locations")]
    [DataContract]
    public class ArticleWarehouseLocations : ModelBase, IDisposable
    {
        #region Fields

        Int64 idArticleWarehouseLocation;
        Int64 idWarehouseLocation;
        Int64 position;
        int idArticle;

        Int64 idWarehouse;
        Int64 parent;
        string name;
        string fullName;

        WarehouseLocation warehouseLocation;
        ArticlesStock articlesStock;
        Int64 minimumStock;
        bool isLocationDeleted;
        List<WarehouseDeliveryNote> warehouseDeliveryNotes;
        Int64 maximumStock;
        Int64 locationStock;
        decimal avgStock;
        Int64 articleStockForAllLocation;

        #endregion

        #region Properties

        [Key]
        [Column("IdArticleWarehouseLocation")]
        [DataMember]
        public long IdArticleWarehouseLocation
        {
            get { return idArticleWarehouseLocation; }
            set
            {
                idArticleWarehouseLocation = value;
                OnPropertyChanged("IdArticleWarehouseLocation");
            }
        }

        [Column("IdWarehouseLocation")]
        [DataMember]
        public long IdWarehouseLocation
        {
            get { return idWarehouseLocation; }
            set
            {
                idWarehouseLocation = value;
                OnPropertyChanged("IdWarehouseLocation");
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

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public long IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        [NotMapped]
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



        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public WarehouseLocation WarehouseLocation
        {
            get { return warehouseLocation; }
            set
            {
                warehouseLocation = value;
                OnPropertyChanged("WarehouseLocation");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticlesStock ArticlesStock
        {
            get { return articlesStock; }
            set
            {
                articlesStock = value;
                OnPropertyChanged("ArticlesStock");
            }
        }

        [Column("MinimumStock")]
        [DataMember]
        public Int64 MinimumStock
        {
            get { return minimumStock; }
            set
            {
                minimumStock = value;
                OnPropertyChanged("MinimumStock");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsLocationDeleted
        {
            get { return isLocationDeleted; }
            set
            {
                isLocationDeleted = value;
                OnPropertyChanged("IsLocationDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WarehouseDeliveryNote> WarehouseDeliveryNotes
        {
            get { return warehouseDeliveryNotes; }
            set
            {
                warehouseDeliveryNotes = value;
                OnPropertyChanged("WarehouseDeliveryNotes");
            }
        }


        [Column("MaximumStock")]
        [DataMember]
        public Int64 MaximumStock
        {
            get { return maximumStock; }
            set
            {
                maximumStock = value;
                OnPropertyChanged("MaximumStock");
            }
        }

        [NotMapped]
        [DataMember]
        public long LocationStock
        {
            get { return locationStock; }
            set
            {
                locationStock = value;
                OnPropertyChanged("LocationStock");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal AvgStock
        {
            get { return avgStock; }
            set
            {
                avgStock = value;
                OnPropertyChanged("AvgStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ArticleStockForAllLocation
        {
            get { return articleStockForAllLocation; }
            set
            {
                articleStockForAllLocation = value;
                OnPropertyChanged("ArticleStockForAllLocation");
            }
        }

        #endregion

        #region Constructor
        public ArticleWarehouseLocations()
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
