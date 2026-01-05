using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common.PCM
{
    //[rdixit][22.02.2023][GEOS2-4176]
    [DataContract]
    public class PCMArticleSynchronization : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idCustomerOrBasePriceList;
        UInt64 idBasePriceList;
        UInt64 idCustomerPriceList;
        string code;
        string name;
        byte idCurrency;
        LookupValue status;
        Currency currency;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        string country;
        ObservableCollection<Currency> priceCurrencies;
        ObservableCollection<Site> pricePlants;
        ObservableCollection<Site> selectedPricePlants;
        ObservableCollection<Currency> selectedPriceCurrencies;
		List<object> newSelectedPricePlants;
        List<object> newSelectedPriceCurrencies;
        ObservableCollection<BPLPlantCurrencyDetail> bPLPlantCurrencyList;
        bool isChecked = false;
        #endregion

        #region Properties
        [DataMember]
        public ObservableCollection<Currency> PriceCurrencies
        {
            get
            {
                return priceCurrencies;
            }

            set
            {
                priceCurrencies = value;
                OnPropertyChanged("PriceCurrencies");
            }
        }
        [DataMember]
        public ObservableCollection<Site> PricePlants
        {
            get
            {
                return pricePlants;
            }

            set
            {
                pricePlants = value;
                OnPropertyChanged("PricePlants");
            }
        }

        [DataMember]
        public ObservableCollection<Currency> SelectedPriceCurrencies
        {
            get
            {
                return selectedPriceCurrencies;
            }

            set
            {
                selectedPriceCurrencies = value;
                OnPropertyChanged("SelectedPriceCurrencies");
            }
        }

        [DataMember]
        public ObservableCollection<Site> SelectedPricePlants
        {
            get
            {
                return selectedPricePlants;
            }

            set
            {
                selectedPricePlants = value;
                OnPropertyChanged("SelectedPricePlants");
            }
        }
        [DataMember]
        public List<object> NewSelectedPriceCurrencies
        {
            get
            {
                return newSelectedPriceCurrencies;
            }

            set
            {
                newSelectedPriceCurrencies = value;
                OnPropertyChanged("NewSelectedPriceCurrencies");
            }
        }

        [DataMember]
        public List<object> NewSelectedPricePlants
        {
            get
            {
                return newSelectedPricePlants;
            }

            set
            {
                newSelectedPricePlants = value;
                OnPropertyChanged("NewSelectedPricePlants");
            }
        }
        [DataMember]
        public ObservableCollection<BPLPlantCurrencyDetail> BPLPlantCurrencyList
        {
            get
            {
                return bPLPlantCurrencyList;
            }

            set
            {
                bPLPlantCurrencyList = value;
                OnPropertyChanged("BPLPlantCurrencyList");
            }
        }
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

        public PCMArticleSynchronization()
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
            PCMArticleSynchronization pCMArticleSynchronization = (PCMArticleSynchronization)this.MemberwiseClone();

            if (PriceCurrencies != null)
                pCMArticleSynchronization.PriceCurrencies = new ObservableCollection<Currency>(PriceCurrencies.Select(x => (Currency)x.Clone()).ToList());

            if (PricePlants != null)
                pCMArticleSynchronization.PricePlants = new ObservableCollection<Site>(PricePlants.Select(x => (Site)x.Clone()).ToList());

            return pCMArticleSynchronization;
        }

        #endregion
    }
}
