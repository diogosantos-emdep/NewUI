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
    public class CustomerPriceListByItem : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idCustomerPriceListByItems;
        UInt64 idCustomerPriceList;
        UInt64? idArticle;
        UInt32? idCPType;
        UInt32? idDetection;
        string itemCustomerDescription;
        byte visibleToCustomer;
        float? basePriceListValue;
        UInt32 basePriceListCurrency;
        DateTime currencyConversionDate;
        float currencyConversionRate;
        int idRule;
        float ruleValue;
        float sellPrice;
        int version;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;

        Articles article;
        ProductTypes productType;
        Detections detection;

        Currency currencyData;
        string itemCustomerDescription_Old;

        byte linkedDescription;

        DateTime? currencyConversionDate_New;
        float? currencyConversionRate_New;
        float? sellPrice_New;

        int? idRule_New;
        float? ruleValue_New;

        int idPlant;
        UInt32 idCurrency;
        float maxCost;

        #endregion


        #region Properties


        [DataMember]
        public ulong IdCustomerPriceListByItems
        {
            get
            {
                return idCustomerPriceListByItems;
            }

            set
            {
                idCustomerPriceListByItems = value;
                OnPropertyChanged("IdCustomerPriceListByItems");
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
        public string ItemCustomerDescription
        {
            get
            {
                return itemCustomerDescription;
            }

            set
            {
                itemCustomerDescription = value;
                OnPropertyChanged("ItemCustomerDescription");
            }
        }

        [DataMember]
        public byte VisibleToCustomer
        {
            get
            {
                return visibleToCustomer;
            }

            set
            {
                visibleToCustomer = value;
                OnPropertyChanged("VisibleToCustomer");
            }
        }

        [DataMember]
        public float? BasePriceListValue
        {
            get
            {
                return basePriceListValue;
            }

            set
            {
                basePriceListValue = value;
                OnPropertyChanged("BasePriceListValue");
            }
        }

        [DataMember]
        public uint BasePriceListCurrency
        {
            get
            {
                return basePriceListCurrency;
            }

            set
            {
                basePriceListCurrency = value;
                OnPropertyChanged("BasePriceListCurrency");
            }
        }

        [DataMember]
        public string ItemCustomerDescription_Old
        {
            get
            {
                return itemCustomerDescription_Old;
            }

            set
            {
                itemCustomerDescription_Old = value;
                OnPropertyChanged("ItemCustomerDescription_Old");
            }
        }

        [DataMember]
        public byte LinkedDescription
        {
            get
            {
                return linkedDescription;
            }

            set
            {
                linkedDescription = value;
                OnPropertyChanged("LinkedDescription");
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
        public int? IdRule_New
        {
            get
            {
                return idRule_New;
            }

            set
            {
                idRule_New = value;
                OnPropertyChanged("IdRule_New");
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
        #endregion

        #region Constructor

        public CustomerPriceListByItem()
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
            CustomerPriceListByItem customerPriceListByItem = (CustomerPriceListByItem)this.MemberwiseClone();
            if (ArticleData != null)
                customerPriceListByItem.ArticleData = (Articles)this.ArticleData.Clone();

            if (ProductType != null)
                customerPriceListByItem.ProductType = (ProductTypes)this.ProductType.Clone();

            if (Detection != null)
                customerPriceListByItem.Detection = (Detections)this.Detection.Clone();


            return customerPriceListByItem;
        }

        #endregion
    }
}
