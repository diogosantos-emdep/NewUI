using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    [DataContract]
    public class CurrencyRateListByDate
    {

        private int _IdCurrency = 0;
        [DataMember(Order = 1)]
        public int IdCurrency
        {
            get { return _IdCurrency; }
            set { _IdCurrency = value; }
        }

        private string _Currency = string.Empty;
        [DataMember(Order = 2)]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        private DateTime? _ConversionDate;
        [DataMember(Order=3)]
        public DateTime? ConversionDate
        {
            get { return _ConversionDate; }
            set { _ConversionDate = value; }
        }
        //[jbabhulkar][APIGEOS-547][23/06/2022]
        private double _ConversionRate;
        [DataMember(Order = 4)]
        public double ConversionRate
        {
            get { return _ConversionRate; }
            set { _ConversionRate = value; }
        }

        private string _CurrencyFrom;
        [DataMember(Order = 5)]
        public string CurrencyFrom
        {
            get { return _CurrencyFrom; }
            set { _CurrencyFrom = value; }
        }

        private string _CurrencyTo;
        [DataMember(Order = 6)]
        public string CurrencyTo
        {
            get { return _CurrencyTo; }
            set { _CurrencyTo = value; }
        }

       
    }
}
