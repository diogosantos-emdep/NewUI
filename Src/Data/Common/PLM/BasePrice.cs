using Emdep.Geos.Data.Common.Epc;
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
    public class BasePrice : ModelBase, IDisposable
    {

        #region Declaration
        UInt64 idBasePriceList;
        string code;
        string name;
        string name_es;
        string name_fr;
        string name_ro;
        string name_zh;
        string name_pt;
        string name_ru;
        string description;
        string description_es;
        string description_fr;
        string description_ro;
        string description_zh;
        string description_pt;
        string description_ru;
        DateTime effectiveDate;
        DateTime expiryDate;
        UInt32 idStatus;
        string remark;
        string remark_es;
        string remark_fr;
        string remark_ro;
        string remark_zh;
        string remark_pt;
        string remark_ru;
        byte idCurrency;
        byte isEnabled;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        LookupValue status;
        string idPermission;
        string plants;
        string pVPCurrencies;
        DateTime lastUpdated;
        Currency currency;
        int? articleCount;
        int? moduleCount;
        int? detectionCount;

        List<Site> plantList;
        List<Currency> currencyList;
        List<Currency> currencyList_Cloned;
        List<Site> plantList_Cloned;

        List<BasePriceListByItem> basePriceListItems;

        List<Site> article_PlantList;
        List<Currency> article_CurrencyList;

        List<Currency> article_CurrencyList_Cloned;
        List<Site> article_PlantList_Cloned;
        List<BasePriceListByItem> basePriceListItems_Cloned;
        List<BasePriceListByItem> basePriceListItems_Save;
        List<Site> article_PlantList_Save;
        List<Currency> article_CurrencyList_Save;
        List<BasePriceLogEntry> logList;
        List<PCMArticleCategory> cPLArticleList;

        List<BasePriceListByPlantCurrency> plantCurrencyList;

        List<Currency> article_CurrencyList_ForCPL;

        List<CustomerPriceLogEntry> cPLlogList;

        List<CustomerPriceListByItem> customerPriceListItems;

        List<Articles> deleteArticleList;
        List<BasePriceListByItem> addArticleList;
        List<BasePriceListByItem> updateArticleList;

        List<Site> addPlantList;
        List<Currency> addCurrencyList;

        List<Site> deletePlantList;
        List<Currency> deleteCurrencyList;

        List<Site> updatePlantList;
        List<Currency> updateCurrencyList;


        List<BasePriceListByItem> addDetectionList;
        List<Detections> deleteDetectionList;
        List<BasePriceListByItem> updateDetectionList;

        List<BasePriceListByItem> basePriceListDetections;
        List<Currency> detection_CurrencyList;

        List<Currency> updateDetectionCurrencyList;

        List<CustomerPriceListByItem> basePriceListItems_ForCPL;
        List<CustomerPriceListByItem> basePriceListDetections_ForCPL;

        List<Currency> detection_CurrencyList_ForCPL;
        Int32? idExchangeRateUpdateType;
        LookupValue exchangeRateUpdateType;

        List<ArticleCostPrice> articleCostPriceListForSavedBPL;
        List<Currency> pvpCurrencyWithBytes;

        bool isBPLExistInCPL;
        List<CustomerPriceListByItem> customerPriceListDetections;
        List<CustomerPriceListByItem> customerPriceListSaleCurrencies;
        List<BPLDocument> pLMArticleAttachmentList;
        //List<CurrencyConversion> currencyConversionsForBPLArticlesSaved;
        //List<CurrencyConversion> currencyConversionsForBPLDetectionsSaved;


        List<BasePriceLogEntry> basePriceCommentsList;//[Sudhir.Jangra][GEOS2-4935]
        #endregion

        #region Properties

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
        public string Name_es
        {
            get
            {
                return name_es;
            }

            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [DataMember]
        public string Name_fr
        {
            get
            {
                return name_fr;
            }

            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [DataMember]
        public string Name_ro
        {
            get
            {
                return name_ro;
            }

            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }

        [DataMember]
        public string Name_zh
        {
            get
            {
                return name_zh;
            }

            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [DataMember]
        public string Name_pt
        {
            get
            {
                return name_pt;
            }

            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }

        [DataMember]
        public string Name_ru
        {
            get
            {
                return name_ru;
            }

            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public string Description_es
        {
            get
            {
                return description_es;
            }

            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }

        [DataMember]
        public string Description_fr
        {
            get
            {
                return description_fr;
            }

            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }

        [DataMember]
        public string Description_ro
        {
            get
            {
                return description_ro;
            }

            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }

        [DataMember]
        public string Description_zh
        {
            get
            {
                return description_zh;
            }

            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }

        [DataMember]
        public string Description_pt
        {
            get
            {
                return description_pt;
            }

            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
            }
        }

        [DataMember]
        public string Description_ru
        {
            get
            {
                return description_ru;
            }

            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru");
            }
        }

        [DataMember]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDate;
            }

            set
            {
                effectiveDate = value;
                OnPropertyChanged("EffectiveDate");
            }
        }

        [DataMember]
        public DateTime ExpiryDate
        {
            get
            {
                return expiryDate;
            }

            set
            {
                expiryDate = value;
                OnPropertyChanged("ExpiryDate");
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
        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
                OnPropertyChanged("Remark");
            }
        }

        [DataMember]
        public string Remark_es
        {
            get
            {
                return remark_es;
            }

            set
            {
                remark_es = value;
                OnPropertyChanged("Remark_es");
            }
        }

        [DataMember]
        public string Remark_fr
        {
            get
            {
                return remark_fr;
            }

            set
            {
                remark_fr = value;
                OnPropertyChanged("Remark_fr");
            }
        }

        [DataMember]
        public string Remark_ro
        {
            get
            {
                return remark_ro;
            }

            set
            {
                remark_ro = value;
                OnPropertyChanged("Remark_ro");
            }
        }

        [DataMember]
        public string Remark_zh
        {
            get
            {
                return remark_zh;
            }

            set
            {
                remark_zh = value;
                OnPropertyChanged("Remark_zh");
            }
        }

        [DataMember]
        public string Remark_pt
        {
            get
            {
                return remark_pt;
            }

            set
            {
                remark_pt = value;
                OnPropertyChanged("Remark_pt");
            }
        }

        [DataMember]
        public string Remark_ru
        {
            get
            {
                return remark_ru;
            }

            set
            {
                remark_ru = value;
                OnPropertyChanged("Remark_ru");
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
        public byte IsEnabled
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
        public string Plants
        {
            get
            {
                return plants;
            }

            set
            {
                plants = value;
                OnPropertyChanged("Plants");
            }
        }

        [DataMember]
        public string PVPCurrencies
        {
            get
            {
                return pVPCurrencies;
            }

            set
            {
                pVPCurrencies = value;
                OnPropertyChanged("PVPCurrencies");
            }
        }

        [DataMember]
        public DateTime LastUpdated
        {
            get
            {
                return lastUpdated;
            }

            set
            {
                lastUpdated = value;
                OnPropertyChanged("LastUpdated");
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
        public int? ArticleCount
        {
            get
            {
                return articleCount;
            }

            set
            {
                articleCount = value;
                OnPropertyChanged("ArticleCount");
            }
        }

        [DataMember]
        public int? ModuleCount
        {
            get
            {
                return moduleCount;
            }

            set
            {
                moduleCount = value;
                OnPropertyChanged("ModuleCount");
            }
        }
        [DataMember]
        public int? DetectionCount
        {
            get
            {
                return detectionCount;
            }

            set
            {
                detectionCount = value;
                OnPropertyChanged("DetectionCount");
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
        public List<Currency> CurrencyList
        {
            get
            {
                return currencyList;
            }

            set
            {
                currencyList = value;
                OnPropertyChanged("CurrencyList");
            }
        }

        [DataMember]
        public List<Currency> CurrencyList_Cloned
        {
            get
            {
                return currencyList_Cloned;
            }

            set
            {
                currencyList_Cloned = value;
                OnPropertyChanged("CurrencyList_Cloned");
            }
        }
        [DataMember]
        public List<Site> PlantList_Cloned
        {
            get
            {
                return plantList_Cloned;
            }

            set
            {
                plantList_Cloned = value;
                OnPropertyChanged("PlantList_Cloned");
            }
        }

        [DataMember]
        public List<BasePriceListByItem> BasePriceListItems
        {
            get
            {
                return basePriceListItems;
            }

            set
            {
                basePriceListItems = value;
                OnPropertyChanged("BasePriceListItems");
            }
        }

        [DataMember]
        public List<Site> Article_PlantList
        {
            get
            {
                return article_PlantList;
            }

            set
            {
                article_PlantList = value;
                OnPropertyChanged("Article_PlantList");
            }
        }

        [DataMember]
        public List<Currency> Article_CurrencyList
        {
            get
            {
                return article_CurrencyList;
            }

            set
            {
                article_CurrencyList = value;
                OnPropertyChanged("Article_CurrencyList");
            }
        }

        [DataMember]
        public List<Currency> Article_CurrencyList_Cloned
        {
            get
            {
                return article_CurrencyList_Cloned;
            }

            set
            {
                article_CurrencyList_Cloned = value;
                OnPropertyChanged("Article_CurrencyList_Cloned");
            }
        }
        [DataMember]
        public List<Site> Article_PlantList_Cloned
        {
            get
            {
                return article_PlantList_Cloned;
            }

            set
            {
                article_PlantList_Cloned = value;
                OnPropertyChanged("Article_PlantList_Cloned");
            }
        }

        [DataMember]
        public List<BasePriceListByItem> BasePriceListItems_Cloned
        {
            get
            {
                return basePriceListItems_Cloned;
            }

            set
            {
                basePriceListItems_Cloned = value;
                OnPropertyChanged("BasePriceListItems_Cloned");
            }
        }

        [DataMember]
        public List<BasePriceListByItem> BasePriceListItems_Save
        {
            get
            {
                return basePriceListItems_Save;
            }

            set
            {
                basePriceListItems_Save = value;
                OnPropertyChanged("BasePriceListItems_Save");
            }
        }

        [DataMember]
        public List<Site> Article_PlantList_Save
        {
            get
            {
                return article_PlantList_Save;
            }

            set
            {
                article_PlantList_Save = value;
                OnPropertyChanged("Article_PlantList_Save");
            }
        }

        [DataMember]
        public List<Currency> Article_CurrencyList_Save
        {
            get
            {
                return article_CurrencyList_Save;
            }

            set
            {
                article_CurrencyList_Save = value;
                OnPropertyChanged("Article_CurrencyList_Save");
            }
        }

        [DataMember]
        public List<BasePriceLogEntry> LogList
        {
            get
            {
                return logList;
            }

            set
            {
                logList = value;
                OnPropertyChanged("LogList");
            }
        }

        [DataMember]
        public List<PCMArticleCategory> CPLArticleList
        {
            get
            {
                return cPLArticleList;
            }

            set
            {
                cPLArticleList = value;
                OnPropertyChanged("CPLArticleList");
            }
        }

        [DataMember]
        public List<BasePriceListByPlantCurrency> PlantCurrencyList
        {
            get
            {
                return plantCurrencyList;
            }

            set
            {
                plantCurrencyList = value;
                OnPropertyChanged("PlantCurrencyList");
            }
        }

        [DataMember]
        public List<Currency> Article_CurrencyList_ForCPL
        {
            get
            {
                return article_CurrencyList_ForCPL;
            }

            set
            {
                article_CurrencyList_ForCPL = value;
                OnPropertyChanged("Article_CurrencyList_ForCPL");
            }
        }

        [DataMember]
        public List<CustomerPriceLogEntry> CPLlogList
        {
            get
            {
                return cPLlogList;
            }

            set
            {
                cPLlogList = value;
                OnPropertyChanged("CPLlogList");
            }
        }

        [DataMember]
        public List<CustomerPriceListByItem> CustomerPriceListItems
        {
            get
            {
                return customerPriceListItems;
            }

            set
            {
                customerPriceListItems = value;
                OnPropertyChanged("CustomerPriceListItems");
            }
        }

        [DataMember]
        public List<Articles> DeleteArticleList
        {
            get
            {
                return deleteArticleList;
            }

            set
            {
                deleteArticleList = value;
                OnPropertyChanged("DeleteArticleList");
            }
        }

        [DataMember]
        public List<BasePriceListByItem> AddArticleList
        {
            get
            {
                return addArticleList;
            }

            set
            {
                addArticleList = value;
                OnPropertyChanged("AddArticleList");
            }
        }

        [DataMember]
        public List<BasePriceListByItem> UpdateArticleList
        {
            get
            {
                return updateArticleList;
            }

            set
            {
                updateArticleList = value;
                OnPropertyChanged("UpdateArticleList");
            }
        }

        [DataMember]
        public List<Site> AddPlantList
        {
            get
            {
                return addPlantList;
            }

            set
            {
                addPlantList = value;
                OnPropertyChanged("AddPlantList");
            }
        }

        [DataMember]
        public List<Currency> AddCurrencyList
        {
            get
            {
                return addCurrencyList;
            }

            set
            {
                addCurrencyList = value;
                OnPropertyChanged("AddCurrencyList");
            }
        }

        [DataMember]
        public List<Site> DeletePlantList
        {
            get
            {
                return deletePlantList;
            }

            set
            {
                deletePlantList = value;
                OnPropertyChanged("DeletePlantList");
            }
        }

        [DataMember]
        public List<Currency> DeleteCurrencyList
        {
            get
            {
                return deleteCurrencyList;
            }

            set
            {
                deleteCurrencyList = value;
                OnPropertyChanged("DeleteCurrencyList");
            }
        }

        [DataMember]
        public List<Site> UpdatePlantList
        {
            get
            {
                return updatePlantList;
            }

            set
            {
                updatePlantList = value;
                OnPropertyChanged("UpdatePlantList");
            }
        }

        [DataMember]
        public List<Currency> UpdateCurrencyList
        {
            get
            {
                return updateCurrencyList;
            }

            set
            {
                updateCurrencyList = value;
                OnPropertyChanged("UpdateCurrencyList");
            }
        }

        [DataMember]
        public List<BasePriceListByItem> AddDetectionList
        {
            get
            {
                return addDetectionList;
            }

            set
            {
                addDetectionList = value;
                OnPropertyChanged("AddDetectionList");
            }
        }

        [DataMember]
        public List<Detections> DeleteDetectionList
        {
            get
            {
                return deleteDetectionList;
            }

            set
            {
                deleteDetectionList = value;
                OnPropertyChanged("DeleteDetectionList");
            }
        }

        [DataMember]
        public List<BasePriceListByItem> UpdateDetectionList
        {
            get
            {
                return updateDetectionList;
            }

            set
            {
                updateDetectionList = value;
                OnPropertyChanged("UpdateDetectionList");
            }
        }
        [DataMember]
        public List<BasePriceListByItem> BasePriceListDetections
        {
            get
            {
                return basePriceListDetections;
            }

            set
            {
                basePriceListDetections = value;
                OnPropertyChanged("BasePriceListDetections");
            }
        }

        List<BasePriceListByItem> addModuleList;
        [DataMember]
        public List<BasePriceListByItem> AddModuleList
        {
            get
            {
                return addModuleList;
            }

            set
            {
                addModuleList = value;
                OnPropertyChanged("AddModuleList");
            }
        }
        public List<BPLModule> deleteModuleList;
        [DataMember]
        public List<BPLModule> DeleteModuleList
        {
            get
            {
                return deleteModuleList;
            }

            set
            {
                deleteModuleList = value;
                OnPropertyChanged("DeleteModuleList");
            }
        }

        List<BasePriceListByItem> updateModuleList;
        [DataMember]
        public List<BasePriceListByItem> UpdateModuleList
        {
            get
            {
                return updateModuleList;
            }

            set
            {
                updateModuleList = value;
                OnPropertyChanged("UpdateModuleList");
            }
        }
        List<BasePriceListByItem> basePriceListModules;
        [DataMember]
        public List<BasePriceListByItem> BasePriceListModules
        {
            get
            {
                return basePriceListModules;
            }

            set
            {
                basePriceListModules = value;
                OnPropertyChanged("BasePriceListModules");
            }
        }

        [DataMember]
        public List<Currency> Detection_CurrencyList
        {
            get
            {
                return detection_CurrencyList;
            }

            set
            {
                detection_CurrencyList = value;
                OnPropertyChanged("Detection_CurrencyList");
            }
        }

        [DataMember]
        public List<Currency> UpdateDetectionCurrencyList
        {
            get
            {
                return updateDetectionCurrencyList;
            }

            set
            {
                updateDetectionCurrencyList = value;
                OnPropertyChanged("UpdateDetectionCurrencyList");
            }
        }

        [DataMember]
        public List<CustomerPriceListByItem> BasePriceListItems_ForCPL
        {
            get
            {
                return basePriceListItems_ForCPL;
            }

            set
            {
                basePriceListItems_ForCPL = value;
                OnPropertyChanged("BasePriceListItems_ForCPL");
            }
        }

        [DataMember]
        public List<CustomerPriceListByItem> BasePriceListDetections_ForCPL
        {
            get
            {
                return basePriceListDetections_ForCPL;
            }

            set
            {
                basePriceListDetections_ForCPL = value;
                OnPropertyChanged("BasePriceListDetections_ForCPL");
            }
        }

        List<CustomerPriceListByItem> basePriceListModules_ForCPL;
        [DataMember]
        public List<CustomerPriceListByItem> BasePriceListModules_ForCPL
        {
            get
            {
                return basePriceListModules_ForCPL;
            }

            set
            {
                basePriceListModules_ForCPL = value;
                OnPropertyChanged("BasePriceListModules_ForCPL");
            }
        }
        [DataMember]
        public List<Currency> Detection_CurrencyList_ForCPL
        {
            get
            {
                return detection_CurrencyList_ForCPL;
            }

            set
            {
                detection_CurrencyList_ForCPL = value;
                OnPropertyChanged("Detection_CurrencyList_ForCPL");
            }
        }


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
        public LookupValue ExchangeRateUpdateType
        {
            get
            {
                return exchangeRateUpdateType;
            }

            set
            {
                exchangeRateUpdateType = value;
                OnPropertyChanged("ExchangeRateUpdateType");
            }
        }

        [DataMember]
        public List<ArticleCostPrice> ArticleCostPriceListForSavedBPL
        {
            get
            {
                return articleCostPriceListForSavedBPL;
            }

            set
            {
                articleCostPriceListForSavedBPL = value;
                OnPropertyChanged("ArticleCostPriceListForSavedBPL");
            }
        }

        [DataMember]
        public List<Currency> PVPCurrencyWithBytes
        {
            get
            {
                return pvpCurrencyWithBytes;
            }

            set
            {
                pvpCurrencyWithBytes = value;
                OnPropertyChanged("PVPCurrencyWithBytes");
            }
        }

        [DataMember]
        public bool IsBPLExistInCPL
        {
            get
            {
                return isBPLExistInCPL;
            }

            set
            {
                isBPLExistInCPL = value;
                OnPropertyChanged("IsBPLExistInCPL");
            }
        }

        [DataMember]
        public List<CustomerPriceListByItem> CustomerPriceListDetections
        {
            get
            {
                return customerPriceListDetections;
            }

            set
            {
                customerPriceListDetections = value;
                OnPropertyChanged("CustomerPriceListDetections");
            }
        }

        [DataMember]
        public List<CustomerPriceListByItem> CustomerPriceListSaleCurrencies
        {
            get
            {
                return customerPriceListSaleCurrencies;
            }

            set
            {
                customerPriceListSaleCurrencies = value;
                OnPropertyChanged("CustomerPriceListSaleCurrencies");
            }
        }

        [DataMember]
        public List<BPLDocument> PLMArticleAttachmentList
        {
            get
            {
                return pLMArticleAttachmentList;
            }

            set
            {
                pLMArticleAttachmentList = value;
                OnPropertyChanged("PLMArticleAttachmentList");
            }
        }

        //[DataMember]
        //public List<CurrencyConversion> CurrencyConversionsForBPLArticlesSaved
        //{
        //    get
        //    {
        //        return currencyConversionsForBPLArticlesSaved;
        //    }

        //    set
        //    {
        //        currencyConversionsForBPLArticlesSaved = value;
        //        OnPropertyChanged("CurrencyConversionsForBPLArticlesSaved");
        //    }
        //}

        //[DataMember]
        //public List<CurrencyConversion> CurrencyConversionsForBPLDetectionsSaved
        //{
        //    get
        //    {
        //        return currencyConversionsForBPLDetectionsSaved;
        //    }

        //    set
        //    {
        //        currencyConversionsForBPLDetectionsSaved = value;
        //        OnPropertyChanged("CurrencyConversionsForBPLDetectionsSaved");
        //    }
        //}

        //[Sudhir.Jangra][GEOS2-4935]
        [DataMember]
        public List<BasePriceLogEntry> BasePriceCommentsList
        {
            get { return basePriceCommentsList; }
            set
            {
                basePriceCommentsList = value;
                OnPropertyChanged("BasePriceCommentsList");
            }
        }
        #endregion

        #region Constructor

        public BasePrice()
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
            BasePrice basePrice = (BasePrice)this.MemberwiseClone();
            if (Status != null)
                basePrice.Status = (LookupValue)this.Status.Clone();

            if (Currency != null)
                basePrice.Currency = (Currency)this.Currency.Clone();

            if (PlantList != null)
                basePrice.PlantList = PlantList.Select(x => (Site)x.Clone()).ToList();

            if (CurrencyList != null)
                basePrice.CurrencyList = CurrencyList.Select(x => (Currency)x.Clone()).ToList();

            if (CurrencyList_Cloned != null)
                basePrice.CurrencyList_Cloned = CurrencyList_Cloned.Select(x => (Currency)x.Clone()).ToList();

            if (PlantList_Cloned != null)
                basePrice.PlantList_Cloned = PlantList_Cloned.Select(x => (Site)x.Clone()).ToList();

            if (BasePriceListItems != null)
                basePrice.BasePriceListItems = BasePriceListItems.Select(x => (BasePriceListByItem)x.Clone()).ToList();

            if (Article_PlantList != null)
                basePrice.Article_PlantList = Article_PlantList.Select(x => (Site)x.Clone()).ToList();

            if (Article_CurrencyList != null)
                basePrice.Article_CurrencyList = Article_CurrencyList.Select(x => (Currency)x.Clone()).ToList();

            if (Article_CurrencyList_Cloned != null)
                basePrice.Article_CurrencyList_Cloned = Article_CurrencyList_Cloned.Select(x => (Currency)x.Clone()).ToList();

            if (Article_PlantList_Cloned != null)
                basePrice.Article_PlantList_Cloned = Article_PlantList_Cloned.Select(x => (Site)x.Clone()).ToList();

            if (BasePriceListItems_Cloned != null)
                basePrice.BasePriceListItems_Cloned = BasePriceListItems_Cloned.Select(x => (BasePriceListByItem)x.Clone()).ToList();


            if (LogList != null)
                basePrice.LogList = LogList.Select(x => (BasePriceLogEntry)x.Clone()).ToList();

            if (CPLArticleList != null)
                basePrice.CPLArticleList = CPLArticleList.Select(x => (PCMArticleCategory)x.Clone()).ToList();

            if (PlantCurrencyList != null)
                basePrice.PlantCurrencyList = PlantCurrencyList.Select(x => (BasePriceListByPlantCurrency)x.Clone()).ToList();

            if (Article_CurrencyList_ForCPL != null)
                basePrice.Article_CurrencyList_ForCPL = Article_CurrencyList_ForCPL.Select(x => (Currency)x.Clone()).ToList();

            if (basePriceListItems_ForCPL != null)
                basePrice.basePriceListItems_ForCPL = basePriceListItems_ForCPL.Select(x => (CustomerPriceListByItem)x.Clone()).ToList();

            if (BasePriceListDetections_ForCPL != null)
                basePrice.BasePriceListDetections_ForCPL = BasePriceListDetections_ForCPL.Select(x => (CustomerPriceListByItem)x.Clone()).ToList();

            if (PLMArticleAttachmentList != null)
                basePrice.PLMArticleAttachmentList = PLMArticleAttachmentList.Select(x => (BPLDocument)x.Clone()).ToList();

            if (BasePriceCommentsList!=null)
            {
                basePrice.BasePriceCommentsList = BasePriceCommentsList.Select(x => (BasePriceLogEntry)x.Clone()).ToList();
            }

            //if (CurrencyConversionsForBPLArticlesSaved != null)
            //    basePrice.CurrencyConversionsForBPLArticlesSaved = CurrencyConversionsForBPLArticlesSaved.Select(x => (CurrencyConversion)x.Clone()).ToList();


            //if (CurrencyConversionsForBPLDetectionsSaved != null)
            //    basePrice.CurrencyConversionsForBPLDetectionsSaved = CurrencyConversionsForBPLDetectionsSaved.Select(x => (CurrencyConversion)x.Clone()).ToList();

            return basePrice;
        }

        #endregion
    }
}
