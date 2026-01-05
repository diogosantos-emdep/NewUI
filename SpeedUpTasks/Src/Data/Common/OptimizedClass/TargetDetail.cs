using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class TargetDetail
    {
        #region  Fields
        Int32 idSite;
        string countryName;
        string customerGroup;
        string customerName;
        decimal targetAmount;
        byte targetIdCurrency;
        decimal targetAmountWithExchangeRate;
        string symbol;
        string currencyName;
        Int32 year;
        #endregion
        #region Properties

        [DataMember]
        public int IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
            }
        }

        [DataMember]
        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                countryName = value;
            }
        }

        [DataMember]
        public string CustomerGroup
        {
            get
            {
                return customerGroup;
            }

            set
            {
                customerGroup = value;
            }
        }

        [DataMember]
        public string CustomerName
        {
            get
            {
                return customerName;
            }

            set
            {
                customerName = value;
            }
        }

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
            }
        }

        [DataMember]
        public byte TargetIdCurrency
        {
            get
            {
                return targetIdCurrency;
            }

            set
            {
                targetIdCurrency = value;
            }
        }

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
            }
        }

        [DataMember]
        public string Symbol
        {
            get
            {
                return symbol;
            }

            set
            {
                symbol = value;
            }
        }

        [DataMember]
        public string CurrencyName
        {
            get
            {
                return currencyName;
            }

            set
            {
                currencyName = value;
            }
        }

        [DataMember]
        public int Year
        {
            get
            {
                return year;
            }

            set
            {
                year = value;
            }
        }

        #endregion
    }
}
