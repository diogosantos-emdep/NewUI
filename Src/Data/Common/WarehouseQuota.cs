using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    ////[pramod.misal][GEOS2-4228][15/05/2023]
    [Table("warehouseQuota")]
    [DataContract]
    public class WarehouseQuota : ModelBase, IDisposable
    {
        #region Fields

         UInt32 idWarehouseStockTarget; 
         UInt32 idWarehouse;
         Int32 idPlant;
        //Int32 idWarehouse;
        byte idSalesQuotaCurrency;
         UInt32 targetIdCurrency;
         string name;
         double targetAmount; 
         double salesQuotaAmount;
         double currencyConversionRate;
         DateTime? exchangeRateDate;
         List<LookupValue> lookupValue;
        LookupValue selectedLookupValue;
        string symbol;
         int year;
         string shortname;
        Dictionary<string, double> TargetAmountsByCulture = new Dictionary<string, double>();
        string amountHeader;
        //pmisal
        private string warehouseQuotaEUR;
        string amountHeaderApplicationPrefer;
        string targetAmountwithSymbol; 
        string salesQuotaAmountwithSymbol; 


        #endregion

        #region Properties


         [Key]
        [Column("IdWarehouseStockTarget")]
        [DataMember]
        public UInt32 IdWarehouseStockTarget
        {
            get { return idWarehouseStockTarget; }
            set
            {
                idWarehouseStockTarget = value;
                OnPropertyChanged("IdWarehouseStockTarget");
            }
        }

        [Key]
        [Column("IdWarehouse")]
        [DataMember]
        public UInt32 IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse"); 
            }
        }

        [Key]
        [Column("TargetIdCurrency")]
        [DataMember]
        public UInt32 TargetIdCurrency
        {
            get { return targetIdCurrency; }
            set
            {
                targetIdCurrency = value;
                OnPropertyChanged("TargetIdCurrency"); 
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

        
        //pmisal
        public string WarehouseQuotaEUR
        {
            get { return warehouseQuotaEUR; }
            set
            {
                warehouseQuotaEUR = value;
                OnPropertyChanged("WarehouseQuotaEUR");
            }
        }

        [Column("TargetAmount")]
        [DataMember]
        public double TargetAmount
        {
            get { return targetAmount; }
            set
            {
                targetAmount = value;
                OnPropertyChanged("TargetAmount");
            }
        }

        [Column("SalesQuotaAmount")]
        [DataMember]
        public double SalesQuotaAmount
        {
            get { return salesQuotaAmount; }
            set
            {
                salesQuotaAmount = value;
                OnPropertyChanged("SalesQuotaAmount");
            }
        }

        [Column("CurrencyConversionRate")]
        [DataMember]
        public double CurrencyConversionRate
        {
            get { return currencyConversionRate; }
            set
            {
                currencyConversionRate = value;
                OnPropertyChanged("CurrencyConversionRate");
            }
        }


        [Column("ExchangeRateDate")]
        [DataMember]
        public DateTime? ExchangeRateDate
        {
            get { return exchangeRateDate; }
            set
            {
                exchangeRateDate = value;
                OnPropertyChanged("ExchangeRateDate");
            }
        }

        [Column("Symbol")]
        [DataMember]
        public string Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                OnPropertyChanged("Symbol");
            }
        }

        [Column("ShortName")]
        [DataMember]
        public string ShortName
        {
            get { return shortname; }
            set
            {
                shortname = value;
                OnPropertyChanged("ShortName");
            }
        }

        [Key]
        [Column("Year")]
        [DataMember]
        public int Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LookupValue> LookupValue
        {
            get
            {
                return lookupValue;
            }

            set
            {
                lookupValue = value;
                OnPropertyChanged("LookupValue");
            }
        }

        public byte IdSalesQuotaCurrency
        {
            get
            {
                return idSalesQuotaCurrency;
            }

            set
            {
                idSalesQuotaCurrency = value;
                OnPropertyChanged("IdSalesQuotaCurrency");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue SelectedLookupValue
        {
            get
            {
                return selectedLookupValue;
            }

            set
            {
                selectedLookupValue = value;
                OnPropertyChanged("SelectedLookupValue");
            }
        }


        [NotMapped]
        [DataMember]
        public string AmountHeader
        {
            get
            {
                return amountHeader;
            }

            set
            {
                amountHeader = value;
                OnPropertyChanged("AmountHeader");
            }
        }

        [NotMapped]
        [DataMember]
        public string AmountHeaderApplicationPrefer
        {
            get
            {
                return amountHeaderApplicationPrefer;
            }

            set
            {
                amountHeaderApplicationPrefer = value;
                OnPropertyChanged("AmountHeaderApplicationPrefer");
            }
        }


        [Column("IdPlant")]
        [DataMember]
        public Int32 IdPlant
        {
            get
            {
                return idPlant;
            }

            set
            {
                idPlant = value;
                OnPropertyChanged("IdPlant");
            }
        }

        ////[pramod.misal][GEOS2-4228][24/05/2023]

        [DataMember]
        public string TargetAmountwithSymbol
        {
            get { return targetAmountwithSymbol; }
            set
            {
                targetAmountwithSymbol = value;
                OnPropertyChanged("TargetAmountwithSymbol");
            }
        }

        ////[pramod.misal][GEOS2-4228][24/05/2023]
        [DataMember]
        public string SalesQuotaAmountwithSymbol
        {
            get { return salesQuotaAmountwithSymbol; }
            set
            {
                salesQuotaAmountwithSymbol = value;
                OnPropertyChanged("SalesQuotaAmountwithSymbol");
            }
        }
        #endregion

        #region Constructor
        public WarehouseQuota()
        {

        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
