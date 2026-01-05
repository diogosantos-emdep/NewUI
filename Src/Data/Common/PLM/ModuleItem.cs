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
    public class ModuleItem : ModelBase, IDisposable
    {
        #region Declaration
        bool isChecked;
        string code;
        string type;
        string stat;
        string htmlColor;
        string name;
        uint? idCPType;
        int logic;
        double? value;
        double maxCost1;
        DateTime? currencyConversionDate;
        double commonCur;
        double profit;
        double costMargin;
        string saleCurrencyConversionDate;
        Dictionary<string, double?> currencyConversionRates;
        Dictionary<string, double?> currencyValues;
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
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [DataMember]
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
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
        public uint? IdCPType
        {
            get { return idCPType; }
            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
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
                OnPropertyChanged("MaxCost_1");
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
            get { return commonCur; }
            set
            {
                commonCur = value;
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
        public Dictionary<string, double?> CurrencyConversionRates
        {
            get { return currencyConversionRates; }
            set
            {
                currencyConversionRates = value;
                OnPropertyChanged("CurrencyConversionRates");
            }
        }

        [DataMember]
        public Dictionary<string, double?> CurrencyValues
        {
            get { return currencyValues; }
            set
            {
                currencyValues = value;
                OnPropertyChanged("CurrencyValues");
            }
        }
        #endregion

        #region Constructor
        public ModuleItem()
        {
            currencyConversionRates = new Dictionary<string, double?>();
            currencyValues = new Dictionary<string, double?>();
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            ModuleItem moduleItem = (ModuleItem)this.MemberwiseClone();
            return moduleItem;
        }
        #endregion
    }
}
