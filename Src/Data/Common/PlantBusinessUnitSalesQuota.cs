using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    [Table("plant_business_unit_sales_quotas")]
    [DataContract]
    public class PlantBusinessUnitSalesQuota : ModelBase, IDisposable
    {
        #region Fields
        Int32 idPlant;
        Int32 idBusinessUnit;
        double salesQuotaAmount;
        byte idSalesQuotaCurrency;
        string shortName;
        List<LookupValue> lookupValue;
        Int32 year;
        DateTime? exchangeRateDate;
        double quotaAmountWithExchangeRate;
        LookupValue selectedLookupValue;
        string amountHeader;
        string amountHeaderApplicationPrefer;

        Int32 idRegion;
        string region;
        double currencyConversionRate;
        double convertedAmount;
        #endregion

        #region Constructor
        public PlantBusinessUnitSalesQuota()
        {
           
        }
        #endregion

        #region Properties
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


        [Column("IdBusinessUnit")]
        [DataMember]
        public Int32 IdBusinessUnit
        {
            get
            {
                return idBusinessUnit;
            }

            set
            {
                idBusinessUnit = value;
                OnPropertyChanged("IdBusinessUnit");
            }
        }

        [Column("SalesQuotaAmount")]
        [DataMember]
        public Double SalesQuotaAmount
        {
            get
            {
                return salesQuotaAmount;
            }

            set
            {
                salesQuotaAmount = value;
                OnPropertyChanged("SalesQuotaAmount");
            }
        }

        [Column("IdSalesQuotaCurrency")]
        [DataMember]
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

        [Column("ExchangeRateDate")]
        [DataMember]
        public DateTime? ExchangeRateDate
        {
            get
            {
                return exchangeRateDate;
            }

            set
            {
                exchangeRateDate = value;
                OnPropertyChanged("ExchangeRateDate");
            }
        }

        [NotMapped]
        [DataMember]
        public Double QuotaAmountWithExchangeRate
        {
            get
            {
                return quotaAmountWithExchangeRate;
            }

            set
            {
                quotaAmountWithExchangeRate = value;
                OnPropertyChanged("QuotaAmountWithExchangeRate");
            }
        }

        [NotMapped]
        [DataMember]
        public string ShortName
        {
            get
            {
                return shortName;
            }

            set
            {
                shortName = value;
                OnPropertyChanged("ShortName");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 Year
        {
            get
            {
                return year;
            }

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

        [NotMapped]
        [DataMember]
        public int IdRegion
        {
            get
            {
                return idRegion;
            }

            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        [NotMapped]
        [DataMember]
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [NotMapped]
        [DataMember]
        public double CurrencyConversionRate
        {
            get
            {
                return currencyConversionRate;
            }

            set
            {
                currencyConversionRate = value;
                OnPropertyChanged("CurrencyConversionRate");
            }
        }

        [NotMapped]
        [DataMember]
        public double ConvertedAmount
        {
            get
            {
                return convertedAmount;
            }

            set
            {
                convertedAmount = value;
                OnPropertyChanged("ConvertedAmount");
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
