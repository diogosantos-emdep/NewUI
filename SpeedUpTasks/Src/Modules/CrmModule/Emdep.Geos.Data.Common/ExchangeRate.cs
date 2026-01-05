using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Crm.Emdep.Geos.Data.Common
{
    public class ExchangeRate
    {
        private string fielcurrencyName;


        private string headerText;


        private Dictionary<string, string> rateCurrency;
        private string rateCurrency1;

        public string RateCurrency1
        {
            get { return rateCurrency1; }
            set { rateCurrency1 = value; }
        }


        public string FielcurrencyName
        {
            get { return fielcurrencyName; }
            set { fielcurrencyName = value; }
        }
        public string HeaderText
        {
            get { return headerText; }
            set { headerText = value; }
        }
        public Dictionary<string, string> RateCurrency
        {
            get { return rateCurrency; }
            set { rateCurrency = value; }
        }
        private ObservableCollection<ExchangeRateAndDate> exchangeRateAndDateList;

        public ObservableCollection<ExchangeRateAndDate> ExchangeRateAndDateList
        {
            get { return exchangeRateAndDateList; }
            set { exchangeRateAndDateList = value; }
        }

        private ObservableCollection<string> exchange;

        public ObservableCollection<string> Exchange
        {
            get { return exchange; }
            set { exchange = value; }
        }

        public string Trend { get; set; }


    }

    public class ExchangeRateAndDate
    {
        private string rate;
        public string Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

    }

}
