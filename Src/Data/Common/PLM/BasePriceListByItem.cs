using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PLM
{
    [DataContract]
    public class BasePriceListByItem : ModelBase, IDisposable
    {
        #region Declaration
        List<Site> plantList;
        UInt64 idBasePriceListByItems;
        UInt64 idBasePriceList;
        UInt64? idArticle;
        UInt32? idCPType;
        UInt32? idDetection;
        int idPlant;
        float articleCostValue;
        float maxCost;
        UInt32 idCurrency;
        DateTime currencyConversionDate;
        float currencyConversionRate;
        int idRule;
        float ruleValue;
        float sellPrice;
        int version;
        float royalty;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;

        Articles article;
        ProductTypes productType;
        Detections detection;

        Site plantData;
        Currency currencyData;

        DateTime? currencyConversionDate_New;
        float? currencyConversionRate_New;
        float? sellPrice_New;
        float? ruleValue_New;
        Int32? idExchangeRateUpdateType;
        string name;
        #endregion


        #region Properties
        [DataMember]
        public Int32? IdExchangeRateUpdateType
        {
            get
            {
                return idExchangeRateUpdateType;
            }

            set
            {
                idExchangeRateUpdateType = value;
                OnPropertyChanged("IdExchangeRateUpdateType");
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
        public List<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged("PlantList");
            }
        }
        [DataMember]
        public ulong IdBasePriceListByItems
        {
            get
            {
                return idBasePriceListByItems;
            }

            set
            {
                idBasePriceListByItems = value;
                OnPropertyChanged("IdBasePriceListByItems");
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
        public ulong? IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [DataMember]
        public uint? IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }

        [DataMember]
        public uint? IdDetection
        {
            get
            {
                return idDetection;
            }

            set
            {
                idDetection = value;
                OnPropertyChanged("IdDetection");
            }
        }

        [DataMember]
        public int IdPlant
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
        public float ArticleCostValue
        {
            get
            {
                return articleCostValue;
            }

            set
            {
                articleCostValue = value;
                OnPropertyChanged("ArticleCostValue");
            }
        }

        [DataMember]
        public uint IdCurrency
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
        public DateTime CurrencyConversionDate
        {
            get
            {
                return currencyConversionDate;
            }

            set
            {
                currencyConversionDate = value;
                OnPropertyChanged("CurrencyConversionDate");
            }
        }

        [DataMember]
        public float CurrencyConversionRate
        {
            get
            {
                return currencyConversionRate;
            }

            set
            {
                currencyConversionRate = value;
                OnPropertyChanged("CurrencyConversionRate");
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
        public float RuleValue
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
        public float SellPrice
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
        public int Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
                OnPropertyChanged("Version");
            }
        }

        [DataMember]
        public float Royalty
        {
            get
            {
                return royalty;
            }

            set
            {
                royalty = value;
                OnPropertyChanged("Royalty");
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
        public Articles ArticleData
        {
            get
            {
                return article;
            }

            set
            {
                article = value;
                OnPropertyChanged("ArticleData");
            }
        }

        [DataMember]
        public ProductTypes ProductType
        {
            get
            {
                return productType;
            }

            set
            {
                productType = value;
                OnPropertyChanged("ProductType");
            }
        }

        [DataMember]
        public Detections Detection
        {
            get
            {
                return detection;
            }

            set
            {
                detection = value;
                OnPropertyChanged("Detection");
            }
        }

        

        [DataMember]
        public float MaxCost
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
        public Site PlantData
        {
            get
            {
                return plantData;
            }

            set
            {
                plantData = value;
                OnPropertyChanged("PlantData");
            }
        }

        [DataMember]
        public Currency CurrencyData
        {
            get
            {
                return currencyData;
            }

            set
            {
                currencyData = value;
                OnPropertyChanged("CurrencyData");
            }
        }

        [DataMember]
        public DateTime? CurrencyConversionDate_New
        {
            get
            {
                return currencyConversionDate_New;
            }

            set
            {
                currencyConversionDate_New = value;
                OnPropertyChanged("CurrencyConversionDate_New");
            }
        }

        [DataMember]
        public float? CurrencyConversionRate_New
        {
            get
            {
                return currencyConversionRate_New;
            }

            set
            {
                currencyConversionRate_New = value;
                OnPropertyChanged("CurrencyConversionRate_New");
            }
        }

        [DataMember]
        public float? SellPrice_New
        {
            get
            {
                return sellPrice_New;
            }

            set
            {
                sellPrice_New = value;
                OnPropertyChanged("SellPrice_New");
            }
        }

        [DataMember]
        public float? RuleValue_New
        {
            get
            {
                return ruleValue_New;
            }

            set
            {
                ruleValue_New = value;
                OnPropertyChanged("RuleValue_New");
            }
        }
        List<UInt32> plantIdList;
        [DataMember]
        public List<UInt32> PlantIdList
        {
            get
            {
                return plantIdList;
            }

            set
            {
                plantIdList = value;
                OnPropertyChanged("PlantIdList");
            }
        }
        #endregion

        #region Constructor

        public BasePriceListByItem()
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
            BasePriceListByItem basePriceListByItem = (BasePriceListByItem)this.MemberwiseClone();
            if (ArticleData != null)
                basePriceListByItem.ArticleData = (Articles)this.ArticleData.Clone();

            if (ProductType != null)
                basePriceListByItem.ProductType = (ProductTypes)this.ProductType.Clone();

            if (Detection != null)
                basePriceListByItem.Detection = (Detections)this.Detection.Clone();

            
            return basePriceListByItem;
        }

        #endregion
    }
}
