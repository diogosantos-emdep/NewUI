using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common
{
    [Table("warehouse_inventory_audits")]
    [DataContract]
    public class WarehouseInventoryAudit : ModelBase, IDisposable
    {
        #region Fields

        Int64 idWarehouseInventoryAudit;
        string name;
        Int64 idWarehouse;
        DateTime? startDate;
        DateTime? endDate;
        Int32 idCreator;
        User creator;
        DateTime creationDate;
        Int32? idModifier;
        User modifier;
        DateTime? modificationDate;

        // Additional Properties
        Int64 totalItems;
        Int64 totalLocations;
        Int64 totalScopeItems;
        Int64 totalScopeLocations;
        double totalItemsPercentage;
        double totalLocationsPercentage;        
        Int64 okItems;
        Int64 nokItems;
        byte successRate;
        Int64 balanceAmount;
        string balanceAmountwithCurrentSymbol;
        private Visibility isDeleteInventoryAuditVisibility;
        List<WarehouseInventoryAuditItem> warehouseInventoryAuditItemsList;
        List<InventoryAuditArticle> deletedArticles;
        List<InventoryAuditArticle> addedArticles;
        List<InventoryAuditLocation> deletedLocations;
        List<InventoryAuditLocation> addedLocation;
        List<InventoryAuditArticle> updatedArticles;
        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public List<WarehouseInventoryAuditItem> WarehouseInventoryAuditItemsList
        {
            get { return warehouseInventoryAuditItemsList; }
            set
            {
                warehouseInventoryAuditItemsList = value;
                OnPropertyChanged("WarehouseInventoryAuditItemsList");
            }
        }

        [Column("IdWarehouseInventoryAudit")]
        [DataMember]
        public long IdWarehouseInventoryAudit
        {
            get { return idWarehouseInventoryAudit; }
            set
            {
                idWarehouseInventoryAudit = value;
                OnPropertyChanged("IdWarehouseInventoryAudit");
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

        [Column("IdWarehouse")]
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

        [Column("StartDate")]
        [DataMember]
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Column("EndDate")]
        [DataMember]
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [Column("IdCreator")]
        [DataMember]
        public int IdCreator
        {
            get { return idCreator; }
            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [Column("Creator")]
        [DataMember]
        public User Creator
        {
            get { return creator; }
            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [Column("IdModifier")]
        [DataMember]
        public int? IdModifier
        {
            get { return idModifier; }
            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [Column("Modifier")]
        [DataMember]
        public User Modifier
        {
            get { return modifier; }
            set
            {
                modifier = value;
                OnPropertyChanged("Modifier");
            }
        }

        [Column("ModificationDate")]
        [DataMember]
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [NotMapped]
        [DataMember]
        public long TotalItems
        {
            get { return totalItems; }
            set
            {
                totalItems = value;
                OnPropertyChanged("TotalItems");
            }
        }

        [NotMapped]
        [DataMember]
        public long TotalLocations
        {
            get { return totalLocations; }
            set
            {
                totalLocations = value;
                OnPropertyChanged("TotalLocations");
            }
        }

        [NotMapped]
        [DataMember]
        public long TotalScopeItems
        {
            get { return totalScopeItems; }
            set
            {
                totalScopeItems = value;
                OnPropertyChanged("TotalScopeItems");
            }
        }

        [NotMapped]
        [DataMember]
        public long TotalScopeLocations
        {
            get { return totalScopeLocations; }
            set
            {
                totalScopeLocations = value;
                OnPropertyChanged("TotalScopeLocations");
            }
        }
        [NotMapped]
        [DataMember]
        public double TotalItemsPercentage
        {
            get { return totalItemsPercentage; }
            set
            {
                totalItemsPercentage = value;
                OnPropertyChanged("TotalItemsPercentage");
            }
        }

        [NotMapped]
        [DataMember]
        public double TotalLocationsPercentage
        {
            get { return totalLocationsPercentage; }
            set
            {
                totalLocationsPercentage = value;
                OnPropertyChanged("TotalLocationsPercentage");
            }
        }

        [NotMapped]
        [DataMember]
        public long OKItems
        {
            get { return okItems; }
            set
            {
                okItems = value;
                OnPropertyChanged("OKItems");
            }
        }

        [NotMapped]
        [DataMember]
        public long NOKItems
        {
            get { return nokItems; }
            set
            {
                nokItems = value;
                OnPropertyChanged("NOKItems");
            }
        }

        [NotMapped]
        [DataMember]
        public byte SuccessRate
        {
            get { return successRate; }
            set
            {
                successRate = value;
                OnPropertyChanged("SuccessRate");
            }
        }

        [NotMapped]
        [DataMember]
        public long BalanceAmount
        {
            get { return balanceAmount; }
            set
            {
                balanceAmount = value;
                OnPropertyChanged("BalanceAmount");
            }
        }


        [NotMapped]
        [DataMember]
        public string BalanceAmountwithCurrentSymbol
        {
            get { return balanceAmountwithCurrentSymbol; }
            set
            {
                balanceAmountwithCurrentSymbol = value;
                OnPropertyChanged("BalanceAmountwithCurrentSymbol");
            }
        }

        [DataMember]
        public Visibility IsDeleteInventoryAuditVisibility
        {
            get
            {
                return isDeleteInventoryAuditVisibility;
            }

            set
            {
                isDeleteInventoryAuditVisibility = value;
                OnPropertyChanged("IsDeleteInventoryAuditVisibility");
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        [DataMember]
        public List<InventoryAuditArticle> DeletedArticles
        {
            get { return deletedArticles; }
            set
            {
                deletedArticles = value;
                OnPropertyChanged("DeletedArticles");
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        [DataMember]
        public List<InventoryAuditArticle> AddedArticles
        {
            get { return addedArticles; }
            set
            {
                addedArticles = value;
                OnPropertyChanged("AddedArticles");
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        [DataMember]
        public List<InventoryAuditLocation> DeletedLocations
        {
            get { return deletedLocations; }
            set
            {
                deletedLocations = value;
                OnPropertyChanged("DeletedLocations");
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        [DataMember]
        public List<InventoryAuditLocation> AddedLocation
        {
            get { return addedLocation; }
            set
            {
                addedLocation = value;
                OnPropertyChanged("AddedLocation");
            }
        }
        //[rdixit][14.12.2022][GEOS2-3962]
        [DataMember]
        public List<InventoryAuditArticle> UpdatedArticles
        {
            get { return updatedArticles; }
            set
            {
                updatedArticles = value;
                OnPropertyChanged("UpdatedArticles");
            }
        }

        List<PrintInventoryAuditReport> printInventoryAuditReport;
        [DataMember]
        public List<PrintInventoryAuditReport> PrintInventoryAuditReport
        {
            get { return printInventoryAuditReport; }
            set
            {
                printInventoryAuditReport = value;
                OnPropertyChanged("PrintInventoryAuditReport");
            }
        }

        public static List<PrintInventoryAuditReport> PrintInventoryAuditReportNew()
        {
            List<PrintInventoryAuditReport> data = new List<PrintInventoryAuditReport>();
           
            return data;
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
