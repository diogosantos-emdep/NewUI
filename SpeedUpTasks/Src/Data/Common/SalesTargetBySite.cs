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
    [Table("salestargetbysite")]
    [DataContract(IsReference = true)]
    public class SalesTargetBySite : ModelBase, IDisposable
    {
        #region Fields
        Int32 idSite;
        decimal targetAmount;
        byte idCurrency;
        Int32 year;
        CurrencyConversion currencyConversion;
        Currency currency;
        double? targetPercentage;
        List<Currency> currencies;
        DateTime? exchangeRateDate;
        decimal targetAmountWithExchangeRate;
        byte targetCurrencyId;
        #endregion
        #region Constructor
        public SalesTargetBySite()
        {
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdSite")]
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

       
        [Column("Year")]
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

        [Column("TargetAmount")]
        [DataMember]
        public decimal TargetAmount
        {
            get
            {
                return targetAmount;
            }

            set
            {
                targetAmount = value;
                OnPropertyChanged("TargetAmount");
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
        public double? TargetPercentage
        {
            get
            {
                return targetPercentage;
            }

            set
            {
                targetPercentage = value;
                OnPropertyChanged("TargetPercentage");
            }
        }

        [Column("IdCurrency")]
        [DataMember]
        public byte IdCurrency
        {
            get
            {
                return idCurrency;
            }

            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual Currency Currency
        {
            get
            {
                return currency;
            }

            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        [NotMapped]
        [DataMember]
        public List<Currency> Currencies
        {
            get
            {
                return currencies;
            }

            set
            {
                currencies = value;
                OnPropertyChanged("Currencies");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual CurrencyConversion CurrencyConversion
        {
            get
            {
                return currencyConversion;
            }

            set
            {
                currencyConversion = value;
                OnPropertyChanged("CurrencyConversion");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal TargetAmountWithExchangeRate
        {
            get
            {
                return targetAmountWithExchangeRate;
            }

            set
            {
                targetAmountWithExchangeRate = value;
                OnPropertyChanged("TargetAmountWithExchangeRate");
            }
        }


        [NotMapped]
        [DataMember]
        public byte TargetCurrencyId
        {
            get
            {
                return targetCurrencyId;
            }

            set
            {
                targetCurrencyId = value;
                OnPropertyChanged("TargetCurrencyId");
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
