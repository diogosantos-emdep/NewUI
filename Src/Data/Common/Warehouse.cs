using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("warehouses")]
    [DataContract]
    public class Warehouses : ModelBase, IDisposable
    {
        #region Fields

        Int64 idWarehouse;
        string name;
        Int64 idSite;
        Company company;
        byte idCurrency;
        Int64 idArticleSupplier;
        Int64? idTransitLocation;
        byte isRegional;
        Int64 articleCurrentStock;
        double monthStock;
        Int64 otForecastStock;
        Int64 poForecastStock;
        Int64 articleMinimumStock;
        string customMessage;
        DateTime? importantNoticeStartDate;
        DateTime? importantNoticeEndDate;
        bool isImportantNoticeEnabled;

        Currency currency;

        bool isInUse;//[Sudhir.Jangra][GEOS2-4489][30/05/2023]
        string iso;
        string countryIconUrl;
        #endregion

        #region Properties

        [Key]
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

        [Column("IdSite")]
        [DataMember]
        public long IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("IdCurrency")]
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

        [NotMapped]
        [DataMember]
        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        [Column("IdTransitLocation")]
        [DataMember]
        public Int64? IdTransitLocation
        {
            get { return idTransitLocation; }
            set
            {
                idTransitLocation = value;
                OnPropertyChanged("IdTransitLocation");
            }
        }


        [NotMapped]
        [DataMember]
        public byte IsRegional
        {
            get { return isRegional; }
            set
            {
                isRegional = value;
                OnPropertyChanged("IsRegional");
            }
        }
        
        [NotMapped]
        [DataMember]
        public Int64 ArticleCurrentStock
        {
            get { return articleCurrentStock; }
            set
            {
                articleCurrentStock = value;
                OnPropertyChanged("ArticleCurrentStock");
            }
        }

        [NotMapped]
        [DataMember]
        public double MonthStock
        {
            get { return monthStock; }
            set
            {
                monthStock = value;
                OnPropertyChanged("MonthStock");
            }
        }
        
        [NotMapped]
        [DataMember]
        public Int64 ArticleMinimumStock
        {
            get { return articleMinimumStock; }
            set
            {
                articleMinimumStock = value;
                OnPropertyChanged("ArticleMinimumStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 OTForecastStock
        {
            get { return otForecastStock; }
            set
            {
                otForecastStock = value;
                OnPropertyChanged("OTForecastStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 POForecastStock
        {
            get { return poForecastStock; }
            set
            {
                poForecastStock = value;
                OnPropertyChanged("POForecastStock");
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


        //[Sudhir.Jangra][GEOS2-4489][30/05/2023]
        [NotMapped]
        [DataMember]
        public bool IsInUse
        {
            get { return isInUse; }
            set
            {
                isInUse = value;
                OnPropertyChanged("IsInUse");
            }
        }

        //rajashri GEOS2-5376
        [Column("ImportantNoticeStartDate")]
        [DataMember]
        public DateTime? ImportantNoticeStartDate
        {
            get
            {
                return importantNoticeStartDate;
            }
            set
            {
                importantNoticeStartDate = value;
                OnPropertyChanged("ImportantNoticeStartDate");
            }
        }
        [Column("ImportantNoticeEndDate")]
        [DataMember]
        public DateTime? ImportantNoticeEndDate
        {
            get
            {
                return importantNoticeEndDate;
            }
            set
            {
                importantNoticeEndDate = value;
                OnPropertyChanged("ImportantNoticeEndDate");
            }
        }
        [Column("CustomMessage")]
        [DataMember]
        public string CustomMessage
        {
            get { return customMessage; }
            set
            {
                customMessage = value;
                OnPropertyChanged("CustomMessage");
            }
        }
        [Column("IsImportantNoticeEnabled")]
        [DataMember]
        public bool IsImportantNoticeEnabled
        {
            get { return isImportantNoticeEnabled; }
            set
            {
                isImportantNoticeEnabled = value;
                OnPropertyChanged("IsImportantNoticeEnabled");
            }
        }

        [NotMapped]
        [DataMember]
        public string CountryIconUrl
        {
            get { return countryIconUrl; }
            set
            {
                countryIconUrl = value;
                OnPropertyChanged("CountryIconUrl");
            }
        }


        [NotMapped]
        [DataMember]
        public string Iso
        {
            get { return iso; }
            set
            {
                iso = value;
                OnPropertyChanged("Iso");
            }
        }
        #endregion

        #region Constructor
        public Warehouses()
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
