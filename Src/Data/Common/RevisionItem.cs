using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.IO;
using Emdep.Geos.Data.Common.File;

namespace Emdep.Geos.Data.Common
{
    [Table("RevisionItem")]
    [DataContract]
    public class RevisionItem : ModelBase, IDisposable
    {
        #region Fields
        Int64 idRevisionItemImported;
        bool validated;
        decimal unitPrice;
        decimal quantity;
        Int16 obsolete;
        string numItem;
        DateTime? modifiedIn;
        Int32 modifiedBy;
        byte marked;
        bool manualPrice;
        string internalComment;
        Int64 idRevisionItem;
        Int64 idRevision;
        Int64 idProduct;
        string customerComment;
        DateTime? createdIn;
        Int32 createdBy;
        String attachedFiles;
        WarehouseProduct warehouseProduct;
        Revision revision;

        Int64 downloadedQuantity;
        Int64 remainingQuantity;
        Int64 status;

        Int64? idDrawing;
        CpType cpType;
        Int32 ways;
        double sellPrice;
        string connectorFamily;
        string reference;
        CPProduct cpProduct;
        decimal downloadedQuantityDecimal;
        decimal remainingQuantityDecimal;
        string cntDetections;
        Int64 cntOptions;
        Int64 cntWays;
        Int64 cntSpareParts;
        Int64 expectedStock;
        string connectorSubFamily;
        List<ArticleForecast> lstArticleForecast;
        string drawingPath;//[pramod.misal][GEOS2-5472][04.07.2024]
        string solidworksDrawingFileName;//[pramod.misal][GEOS2-5472][04.07.2024]
        string itemRemarks;//[Sudhir.Jangra][GEOS2-5637]
        string totalPrice;//[Sudhir.Jangra][GEOS2-5637]
        double itemNum;//[Sudhir.Jangra][GEOS2-6049]
        List<FileDetail> fileDetail; // [GEOS2-6727][pallavi.kale][14-04-2025]
        string electricalDiagramFilePath; // [GEOS2-6727][pallavi.kale][14-04-2025]
        string electricalDigramFileName; // [GEOS2-6727][pallavi.kale][14-04-2025]
        ArticleProduct articlewarehouseProduct;

        #endregion

        #region Constructor
        public RevisionItem()
        {
        }

        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public List<ArticleForecast> LstArticleForecast
        {
            get { return lstArticleForecast; }
            set
            {
                lstArticleForecast = value;
                OnPropertyChanged("LstArticleForecast");
            }
        }
        [Column("Validated")]
        [DataMember]
        public bool Validated
        {
            get
            {
                return validated;
            }

            set
            {
                validated = value;
                OnPropertyChanged("Validated");
            }
        }

        [Column("UnitPrice")]
        [DataMember]
        public decimal UnitPrice
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

        [Column("Quantity")]
        [DataMember]
        public decimal Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [Column("Obsolete")]
        [DataMember]
        public Int16 Obsolete
        {
            get
            {
                return obsolete;
            }

            set
            {
                obsolete = value;
                OnPropertyChanged("Obsolete");
            }
        }

        [Column("NumItem")]
        [DataMember]
        public string NumItem
        {
            get
            {
                return numItem;
            }

            set
            {
                numItem = value;
                OnPropertyChanged("NumItem");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("Marked")]
        [DataMember]
        public byte Marked
        {
            get
            {
                return marked;
            }

            set
            {
                marked = value;
                OnPropertyChanged("Marked");
            }
        }

        [Column("ManualPrice")]
        [DataMember]
        public bool ManualPrice
        {
            get
            {
                return manualPrice;
            }

            set
            {
                manualPrice = value;
                OnPropertyChanged("ManualPrice");
            }
        }

        [Column("InternalComment")]
        [DataMember]
        public string InternalComment
        {
            get
            {
                return internalComment;
            }

            set
            {
                internalComment = value;
                OnPropertyChanged("InternalComment");
            }
        }

        [Key]
        [Column("IdRevisionItem")]
        [DataMember]
        public Int64 IdRevisionItem
        {
            get
            {
                return idRevisionItem;
            }

            set
            {
                idRevisionItem = value;
                OnPropertyChanged("IdRevisionItem");
            }
        }
        [NotMapped]
        [DataMember]
        public Int64 IdRevisionItemImported
        {
            get
            {
                return idRevisionItemImported;
            }

            set
            {
                idRevisionItemImported = value;
                OnPropertyChanged("IdRevisionItemImported");
            }
        }

        [Key]
        [Column("IdRevision")]
        [DataMember]
        public Int64 IdRevision
        {
            get
            {
                return idRevision;
            }

            set
            {
                idRevision = value;
                OnPropertyChanged("IdRevision");
            }
        }

        [Column("IdProduct")]
        [DataMember]
        public Int64 IdProduct
        {
            get
            {
                return idProduct;
            }

            set
            {
                idProduct = value;
                OnPropertyChanged("IdProduct");
            }
        }

        [Column("CustomerComment")]
        [DataMember]
        public string CustomerComment
        {
            get
            {
                return customerComment;
            }

            set
            {
                customerComment = value;
                OnPropertyChanged("CustomerComment");
            }
        }

        [Column("CreatedIn")]
        [DataMember]
        public DateTime? CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("AttachedFiles")]
        [DataMember]
        public string AttachedFiles
        {
            get
            {
                return attachedFiles;
            }

            set
            {
                attachedFiles = value;
                OnPropertyChanged("AttachedFiles");
            }
        }

        [NotMapped]
        [DataMember]
        public WarehouseProduct WarehouseProduct
        {
            get
            {
                return warehouseProduct;
            }

            set
            {
                warehouseProduct = value;
                OnPropertyChanged("WarehouseProduct");
            }
        }


        [NotMapped]
        [DataMember]
        public Revision Revision
        {
            get
            {
                return revision;
            }

            set
            {
                revision = value;
                OnPropertyChanged("Revision");
            }
        }

        [NotMapped]
        [DataMember]
        public long DownloadedQuantity
        {
            get
            {
                return downloadedQuantity;
            }

            set
            {
                downloadedQuantity = value;
                OnPropertyChanged("DownloadedQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public long RemainingQuantity
        {
            get
            {
                return remainingQuantity;
            }

            set
            {
                remainingQuantity = value;
                OnPropertyChanged("RemainingQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public long Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [NotMapped]
        [DataMember]
        public long? IdDrawing
        {
            get { return idDrawing; }
            set
            {
                idDrawing = value;
                OnPropertyChanged("IdDrawing");
            }
        }

        [NotMapped]
        [DataMember]
        public CpType CpType
        {
            get
            {
                return cpType;
            }

            set
            {
                cpType = value;
                OnPropertyChanged("CpType");
            }
        }

        [NotMapped]
        [DataMember]
        public int Ways
        {
            get { return ways; }
            set
            {
                ways = value;
                OnPropertyChanged("Ways");
            }
        }

        [NotMapped]
        [DataMember]
        public double SellPrice
        {
            get { return sellPrice; }
            set
            {
                sellPrice = value;
                OnPropertyChanged("SellPrice");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectorFamily
        {
            get { return connectorFamily; }
            set
            {
                connectorFamily = value;
                OnPropertyChanged("ConnectorFamily");
            }
        }

        [NotMapped]
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


        [NotMapped]
        [DataMember]
        public CPProduct CPProduct
        {
            get { return cpProduct; }
            set
            {
                cpProduct = value;
                OnPropertyChanged("CPProduct");
            }
        }



        [NotMapped]
        [DataMember]
        public decimal DownloadedQuantityDecimal
        {
            get { return downloadedQuantityDecimal; }
            set
            {
                downloadedQuantityDecimal = value;
                OnPropertyChanged("DownloadedQuantityDecimal");
            }
        }


        [NotMapped]
        [DataMember]
        public decimal RemainingQuantityDecimal
        {
            get { return remainingQuantityDecimal; }
            set
            {
                remainingQuantityDecimal = value;
                OnPropertyChanged("RemainingQuantityDecimal");
            }
        }

        [NotMapped]
        [DataMember]
        public string CntDetections
        {
            get { return cntDetections; }
            set
            {
                cntDetections = value;
                OnPropertyChanged("ParentArticleType");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 CntWays
        {
            get { return cntWays; }
            set
            {
                cntWays = value;
                OnPropertyChanged("ParentArticleType");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 CntOptions
        {
            get { return cntOptions; }
            set
            {
                cntOptions = value;
                OnPropertyChanged("ParentArticleType");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 CntSpareParts
        {
            get { return cntSpareParts; }
            set
            {
                cntSpareParts = value;
                OnPropertyChanged("CntSpareParts");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ExpectedStock
        {
            get { return expectedStock; }
            set
            {
                expectedStock = value;
                OnPropertyChanged("ExpectedStock");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectorSubFamily
        {
            get { return connectorSubFamily; }
            set
            {
                connectorSubFamily = value;
                OnPropertyChanged("ConnectorSubFamily");
            }
        }
       
        [NotMapped]
        [DataMember]
        public string DrawingPath
        {
            get { return drawingPath; }
            set
            {
                drawingPath = value;
                OnPropertyChanged("DrawingPath");
            }
        }

        [NotMapped]
        [DataMember]
        public string SolidworksDrawingFileName
        {
            get { return solidworksDrawingFileName; }
            set
            {
                solidworksDrawingFileName = value;
                OnPropertyChanged("SolidworksDrawingFileName");
            }
        }

        //[Sudhir.Jangra][GEOS2-5637]
        [NotMapped]
        [DataMember]
        public string ItemRemarks
        {
            get { return itemRemarks; }
            set
            {
                itemRemarks = value;
                OnPropertyChanged("ItemRemarks");
            }
        }

        //[Sudhir.Jangra][GEOS2-5637]
        [NotMapped]
        [DataMember]

        public string TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        //[Sudhir.Jangra][GEOS2-6391]
        [NotMapped]
        [DataMember]
        public double ItemNum
        {
            get { return itemNum; }
            set
            {
                itemNum = value;
                OnPropertyChanged("ItemNum");
            }
        }
          // [GEOS2-6727][pallavi.kale][14-04-2025]
        [NotMapped]
        [DataMember]
        public List<FileDetail> Files
        {
            get { return fileDetail; }
            set
            {
                fileDetail = value;
                OnPropertyChanged("FileDetail");
            }
        }

       // [GEOS2-6727][pallavi.kale][14-04-2025]
            [NotMapped]
            [DataMember]
        public string ElectricalDiagramFilePath
        {
            get { return electricalDiagramFilePath; }
            set
            {
                electricalDiagramFilePath = value;
                OnPropertyChanged("ElectricalDiagramFilePath");
            }
        }
	 // [GEOS2-6727][pallavi.kale][14-04-2025]
        [NotMapped]
        [DataMember]
        public string ElectricalDigramFileName
        {
            get { return electricalDigramFileName; }
            set
            {
                electricalDigramFileName = value;
                OnPropertyChanged("ElectricalDigramFileName");
            }
        }

        bool isCritical;
        [Column("IsCritical")]
        [DataMember]
        public bool IsCritical
        {
            get
            {
                return isCritical;
            }

            set
            {
                isCritical = value;
                OnPropertyChanged("IsCritical");
            }
        }
        [Column("ArticlewarehouseProduct")]
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
