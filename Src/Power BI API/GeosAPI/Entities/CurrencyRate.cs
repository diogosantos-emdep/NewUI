using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class CurrencyRate
    {
        private string _ConversionDate;
        [DataMember(Order=1)]
        public string ConversionDate
        {
            get { return _ConversionDate; }
            set { _ConversionDate = value; }
        }

        private string _ConversionRate;
        [DataMember(Order = 2)]
        public string ConversionRate
        {
            get { return _ConversionRate; }
            set { _ConversionRate = value; }
        }

        private string _CurrencyFrom;
        [DataMember(Order = 3)]
        public string CurrencyFrom
        {
            get { return _CurrencyFrom; }
            set { _CurrencyFrom = value; }
        }

        private string _CurrencyTo;
        [DataMember(Order = 4)]
        public string CurrencyTo
        {
            get { return _CurrencyTo; }
            set { _CurrencyTo = value; }
        }

        private bool _IsCorrect;
        //[DataMember(Order = 5)]
        public bool IsCorrect
        {
            get { return _IsCorrect; }
            set { _IsCorrect = value; }
        }

    }
}
