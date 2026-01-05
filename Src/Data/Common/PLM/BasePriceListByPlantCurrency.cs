using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PLM
{
    [DataContract]
    public class BasePriceListByPlantCurrency : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idBasePriceListByPlantCurrency;
        UInt64 idBasePriceList;
        Int64 idPlant;
        byte idCurrency;
        byte plantbyCurrency;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        string plantName;
        string currencyName;
        string currenciesName;
        string curencyConversionDateForToolTip;
        #endregion

        #region Properties
        [DataMember]
        public ulong IdBasePriceListByPlantCurrency
        {
            get
            {
                return idBasePriceListByPlantCurrency;
            }

            set
            {
                idBasePriceListByPlantCurrency = value;
                OnPropertyChanged("IdBasePriceListByPlantCurrency");
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
        public long IdPlant
        {
            get
            {
                return idPlant;
            }

            set
            {
                idPlant = value;
                OnPropertyChanged("IdPlant");
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
        public byte PlantbyCurrency
        {
            get
            {
                return plantbyCurrency;
            }

            set
            {
                plantbyCurrency = value;
                OnPropertyChanged("PlantbyCurrency");
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
        public string PlantName
        {
            get
            {
                return plantName;
            }

            set
            {
                plantName = value;
                OnPropertyChanged("PlantName");
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
                OnPropertyChanged("CurrencyName");
            }
        }

        [DataMember]
        public string CurrenciesName
        {
            get
            {
                return currenciesName;
            }

            set
            {
                currenciesName = value;
                OnPropertyChanged("CurrenciesName");
            }
        }


        [DataMember]
        public string CurencyConversionDateForToolTip
        {
            get
            {
                return curencyConversionDateForToolTip;
            }

            set
            {
                curencyConversionDateForToolTip = value;
                OnPropertyChanged("CurencyConversionDateForToolTip");
            }
        }
        #endregion

        #region Constructor

        public BasePriceListByPlantCurrency()
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
            BasePriceListByPlantCurrency basePriceListByPlantCurrency = (BasePriceListByPlantCurrency)this.MemberwiseClone();

            return basePriceListByPlantCurrency;
        }

        #endregion
    }
}
