using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class PLMArticlePrice : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idCustomerOrBasePriceList;
        UInt64 idBasePriceList;
        UInt64 idCustomerPriceList;
        string code;
        string name;
        UInt32 idStatus;
        byte idCurrency;
        LookupValue status;
        Currency currency;
        string type;
        float? maxCost;
        int idRule;
        float? ruleValue;
        double? sellPrice;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        LookupValue rule;
        double profit;
        double costMargin;
        string country;
        string idPermission;
        bool isEnabled = false;
        bool isChecked;
        #endregion

        #region Properties
        [DataMember]
        public ulong IdCustomerOrBasePriceList
        {
            get
            {
                return idCustomerOrBasePriceList;
            }

            set
            {
                idCustomerOrBasePriceList = value;
                OnPropertyChanged("IdCustomerOrBasePriceList");
            }
        }

        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [DataMember]
        public uint IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

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

        [DataMember]
        public LookupValue Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public Currency Currency
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

        [DataMember]
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        [DataMember]
        public float? MaxCost
        {
            get
            {
                return maxCost;
            }

            set
            {
                maxCost = value;
                OnPropertyChanged("MaxCost");
            }
        }

        [DataMember]
        public int IdRule
        {
            get
            {
                return idRule;
            }

            set
            {
                idRule = value;
                OnPropertyChanged("IdRule");
            }
        }

        [DataMember]
        public float? RuleValue
        {
            get
            {
                return ruleValue;
            }

            set
            {
                ruleValue = value;
                OnPropertyChanged("RuleValue");
            }
        }

        [DataMember]
        public double? SellPrice
        {
            get
            {
                return sellPrice;
            }

            set
            {
                sellPrice = value;
                OnPropertyChanged("SellPrice");
            }
        }

        [DataMember]
        public uint IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public uint? IdModifier
        {
            get
            {
                return idModifier;
            }

            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }

            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public LookupValue Rule
        {
            get
            {
                return rule;
            }

            set
            {
                rule = value;
                OnPropertyChanged("Rule");
            }
        }

        [DataMember]
        public double Profit
        {
            get
            {
                return profit;
            }

            set
            {
                profit = value;
                OnPropertyChanged("Profit");
            }
        }

        [DataMember]
        public double CostMargin
        {
            get
            {
                return costMargin;
            }

            set
            {
                costMargin = value;
                OnPropertyChanged("CostMargin");
            }
        }

        [DataMember]
        public ulong IdBasePriceList
        {
            get
            {
                return idBasePriceList;
            }

            set
            {
                idBasePriceList = value;
                OnPropertyChanged("IdBasePriceList");
            }
        }

        [DataMember]
        public ulong IdCustomerPriceList
        {
            get
            {
                return idCustomerPriceList;
            }

            set
            {
                idCustomerPriceList = value;
                OnPropertyChanged("IdCustomerPriceList");
            }
        }


        [DataMember]
        public string Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [DataMember]
        public string IdPermission
        {
            get
            {
                return idPermission;
            }

            set
            {
                idPermission = value;
                OnPropertyChanged("IdPermission");
            }
        }

        [DataMember]
        public bool IsEnabledPermission
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        [DataMember]
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        #endregion

        #region Constructor

        public PLMArticlePrice()
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
            PLMArticlePrice pLMArticlePrice = (PLMArticlePrice)this.MemberwiseClone();
            if (Status != null)
                pLMArticlePrice.Status = (LookupValue)this.Status.Clone();

            if (currency != null)
                pLMArticlePrice.currency = (Currency)this.currency.Clone();

            if (Rule != null)
                pLMArticlePrice.Rule = (LookupValue)this.Rule.Clone();


            return pLMArticlePrice;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
