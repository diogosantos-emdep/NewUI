using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class OfferMonthDetail
    {
        #region Fields
     
        Int32 connectPlantId;
       
        double value;
        string currName;
        string currSymbol;
       
        Int64 idStatus;
        string geosStatusName;
        string saleStatusName;
     
        Int32 currentMonth;
        Int32 currentYear;
        Int32 year;
        double salesQuotaAmount;
        byte idSalesQuotaCurrency;
        double quotaAmountWithExchangeRate;
        DateTime? exchangeRateDate;
        #endregion

        #region Properties
      
        [DataMember]
        public int ConnectPlantId
        {
            get
            {
                return connectPlantId;
            }

            set
            {
                connectPlantId = value;
            }
        }

      

        [DataMember]
        public double Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        [DataMember]
        public string CurrName
        {
            get
            {
                return currName;
            }

            set
            {
                currName = value;
            }
        }

        [DataMember]
        public string CurrSymbol
        {
            get
            {
                return currSymbol;
            }

            set
            {
                currSymbol = value;
            }
        }


        [DataMember]
        public long IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
            }
        }

        [DataMember]
        public string GeosStatusName
        {
            get
            {
                return geosStatusName;
            }

            set
            {
                geosStatusName = value;
            }
        }

        [DataMember]
        public string SaleStatusName
        {
            get
            {
                return saleStatusName;
            }

            set
            {
                saleStatusName = value;
            }
        }

    
        [DataMember]
        public Int32 CurrentMonth
        {
            get
            {
                return currentMonth;
            }

            set
            {
                currentMonth = value;
            }
        }

        [DataMember]
        public Int32 CurrentYear
        {
            get
            {
                return currentYear;
            }

            set
            {
                currentYear = value;
            }
        }

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
            }
        }

        [DataMember]
        public double SalesQuotaAmount
        {
            get
            {
                return salesQuotaAmount;
            }

            set
            {
                salesQuotaAmount = value;
            }
        }


        [DataMember]
        public double QuotaAmountWithExchangeRate
        {
            get
            {
                return quotaAmountWithExchangeRate;
            }

            set
            {
                quotaAmountWithExchangeRate = value;
            }
        }


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
            }
        }

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
            }
        }

        #endregion
    }
}
