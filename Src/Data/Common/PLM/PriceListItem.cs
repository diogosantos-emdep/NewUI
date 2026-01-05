using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PLM
{
    //[nsatpute][GEOS2-7894][01.08.2025]
    [DataContract]
    public class PriceListItem : ModelBase, IDisposable
    {
        #region Declaration
        bool isChecked;
        string reference;
        string category;
        string stat;
        string htmlColor;
        string name;
        ulong idArticle;
        int logic;
        double? value;
        double maxCost1;
        DateTime? currencyConversionDate;
        double common_Cur;
        double profit;
        double costMargin;
        string saleCurrencyConversionDate;
        ObservableDictionary<string, double?> plantCosts = new ObservableDictionary<string, double?>();
        ObservableDictionary<string, double?> currencyValues = new ObservableDictionary<string, double?>();
        ObservableDictionary<string, double?> currencyConversionRates = new ObservableDictionary<string, double?>();
        #endregion

        #region Properties
        [DataMember]
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

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

        [DataMember]
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        [DataMember]
        public string STAT
        {
            get { return stat; }
            set
            {
                stat = value;
                OnPropertyChanged("STAT");
            }
        }

        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

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

        [DataMember]
        public ulong IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [DataMember]
        public int Logic
        {
            get { return logic; }
            set
            {
                logic = value;
                OnPropertyChanged("Logic");
            }
        }

        [DataMember]
        public double? Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        [DataMember]
        public double MaxCost_1
        {
            get { return maxCost1; }
            set
            {
                maxCost1 = value;
                OnPropertyChanged("MaxCost1");
            }
        }

        [DataMember]
        public DateTime? CurrencyConversionDate
        {
            get { return currencyConversionDate; }
            set
            {
                currencyConversionDate = value;
                OnPropertyChanged("CurrencyConversionDate");
            }
        }

        [DataMember]
        public double Common_Cur
        {
            get { return common_Cur; }
            set
            {
                common_Cur = value;
                OnPropertyChanged("Common_Cur");
            }
        }

        [DataMember]
        public double Profit
        {
            get { return profit; }
            set
            {
                profit = value;
                OnPropertyChanged("Profit");
            }
        }

        [DataMember]
        public double CostMargin
        {
            get { return costMargin; }
            set
            {
                costMargin = value;
                OnPropertyChanged("CostMargin");
            }
        }

        [DataMember]
        public string SaleCurrencyConversionDate
        {
            get { return saleCurrencyConversionDate; }
            set
            {
                saleCurrencyConversionDate = value;
                OnPropertyChanged("SaleCurrencyConversionDate");
            }
        }

        [DataMember]
        public ObservableDictionary<string, double?> PlantCosts
        {
            get { return plantCosts; }
            set
            {
                plantCosts = value;
                OnPropertyChanged("PlantCosts");
            }
        }

        [DataMember]
        public ObservableDictionary<string, double?> CurrencyValues
        {
            get { return currencyValues; }
            set
            {
                currencyValues = value;
                OnPropertyChanged("CurrencyValues");
            }
        }

        [DataMember]
        public ObservableDictionary<string, double?> CurrencyConversionRates
        {
            get { return currencyConversionRates; }
            set
            {
                currencyConversionRates = value;
                OnPropertyChanged("CurrencyConversionRates");
            }
        }
        #endregion

        #region Constructor
        public PriceListItem()
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
            PriceListItem priceListItem = (PriceListItem)this.MemberwiseClone();
            return priceListItem;
        }
        #endregion
    }
}
