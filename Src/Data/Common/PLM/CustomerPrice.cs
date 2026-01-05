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
    public class CustomerPrice : ModelBase, IDisposable
    {

        #region Declaration
        UInt64 idCustomerPriceList;
        UInt64 idBasePriceList;
        string basePriceName;
        string code;
        string name;
        string group;
        string plant;
        string region;
        string country;
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

        List<CustomerPriceListByItem> customerPriceListItems;

        List<Site> article_PlantList;
        List<Currency> article_CurrencyList;
        List<CPLCustomer> customerList;
        List<CustomerPriceLogEntry> logList;
        string saleCurrencies;

        List<Currency> article_CurrencyList_Cloned;
        List<Site> article_PlantList_Cloned;
        List<CustomerPriceListByItem> customerPriceListItems_Cloned;
        List<CustomerPriceListByItem> customerPriceListItems_Save;
        List<Site> article_PlantList_Save;
        List<Currency> article_CurrencyList_Save;

        UInt64 idBasePriceList_Cloned;
        List<CustomerPriceListByItem> customerPriceListDetections;

        List<Articles> deleteArticleList;
        List<CustomerPriceListByItem> addArticleList;
        List<CustomerPriceListByItem> updateArticleList;

        List<Site> addPlantList;
        List<Currency> addCurrencyList;

        List<Site> deletePlantList;
        List<Currency> deleteCurrencyList;

        List<Site> updatePlantList;
        List<Currency> updateCurrencyList;


        List<CustomerPriceListByItem> addDetectionList;
        List<Detections> deleteDetectionList;
        List<CustomerPriceListByItem> updateDetectionList;

        List<Currency> detection_CurrencyList;

        List<Currency> updateDetectionCurrencyList;
        List<ArticleCostPrice> articleCostPriceListForSavedCPL;

        List<Currency> pvpCurrencyWithBytes;

        List<CPLDocument> pLMCustomerAttachmentList;
        //List<CurrencyConversion> currencyConversionsForCPLArticlesSaved;
        //List<CurrencyConversion> currencyConversionsForCPLDetectionsSaved;

        List<CustomerPriceLogEntry> customerPriceCommentsList;//[Sudhir.Jangra][GEOS2-4935]

        #endregion

        #region Properties

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
        public string BasePriceName
        {
            get
            {
                return basePriceName;
            }

            set
            {
                basePriceName = value;
                OnPropertyChanged("BasePriceName");
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
        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        [DataMember]
        public string Plant
        {
            get
            {
                return plant;
            }

            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [DataMember]
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
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
        public List<CPLCustomer> CustomerList
        {
            get
            {
                return customerList;
            }

            set
            {
                customerList = value;
                OnPropertyChanged("CustomerList");
            }
        }

        [DataMember]
        public List<CustomerPriceLogEntry> LogList
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
        public string SaleCurrencies
        {
            get
            {
                return saleCurrencies;
            }

            set
            {
                saleCurrencies = value;
                OnPropertyChanged("SaleCurrencies");
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
        public List<CustomerPriceListByItem> CustomerPriceListItems_Cloned
        {
            get
            {
                return customerPriceListItems_Cloned;
            }

            set
            {
                customerPriceListItems_Cloned = value;
                OnPropertyChanged("CustomerPriceListItems_Cloned");
            }
        }

        [DataMember]
        public List<CustomerPriceListByItem> CustomerPriceListItems_Save
        {
            get
            {
                return customerPriceListItems_Save;
            }

            set
            {
                customerPriceListItems_Save = value;
                OnPropertyChanged("CustomerPriceListItems_Save");
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
        public ulong IdBasePriceList_Cloned
        {
            get
            {
                return idBasePriceList_Cloned;
            }

            set
            {
                idBasePriceList_Cloned = value;
                OnPropertyChanged("IdBasePriceList_Cloned");
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
        public List<CustomerPriceListByItem> AddArticleList
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
        public List<CustomerPriceListByItem> UpdateArticleList
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
        public List<CustomerPriceListByItem> AddDetectionList
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
        public List<CustomerPriceListByItem> UpdateDetectionList
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
        public List<ArticleCostPrice> ArticleCostPriceListForSavedCPL
        {
            get
            {
                return articleCostPriceListForSavedCPL;
            }

            set
            {
                articleCostPriceListForSavedCPL = value;
                OnPropertyChanged("ArticleCostPriceListForSavedCPL");
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
        public List<CPLDocument> PLMCustomerAttachmentList
        {
            get
            {
                return pLMCustomerAttachmentList;
            }

            set
            {
                pLMCustomerAttachmentList = value;
                OnPropertyChanged("PLMCustomerAttachmentList");
            }
        }

        List<BPLModule> deleteModuleList;
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
        List<CustomerPriceListByItem> updateModuleList;
        [DataMember]
        public List<CustomerPriceListByItem> UpdateModuleList
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

        List<CustomerPriceListByItem> customerPriceListModules;
        [DataMember]
        public List<CustomerPriceListByItem> CustomerPriceListModules
        {
            get
            {
                return customerPriceListModules;
            }

            set
            {
                customerPriceListModules = value;
                OnPropertyChanged("CustomerPriceListModules");
            }
        }

        List<CustomerPriceListByItem> addModuleList;
        [DataMember]
        public List<CustomerPriceListByItem> AddModuleList
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
        List<Currency> updateModuleCurrencyList;
        [DataMember]
        public List<Currency> UpdateModuleCurrencyList
        {
            get
            {
                return updateModuleCurrencyList;
            }

            set
            {
                updateModuleCurrencyList = value;
                OnPropertyChanged("UpdateModuleCurrencyList");
            }
        }

        //[DataMember]
        //public List<CurrencyConversion> CurrencyConversionsForCPLArticlesSaved
        //{
        //    get
        //    {
        //        return currencyConversionsForCPLArticlesSaved;
        //    }

        //    set
        //    {
        //        currencyConversionsForCPLArticlesSaved = value;
        //        OnPropertyChanged("CurrencyConversionsForCPLArticlesSaved");
        //    }
        //}

        //[DataMember]
        //public List<CurrencyConversion> CurrencyConversionsForCPLDetectionsSaved
        //{
        //    get
        //    {
        //        return currencyConversionsForCPLDetectionsSaved;
        //    }

        //    set
        //    {
        //        currencyConversionsForCPLDetectionsSaved = value;
        //        OnPropertyChanged("CurrencyConversionsForCPLDetectionsSaved");
        //    }
        //}

        //[Sudhir.Jangra][GEOS2-4935]
        [DataMember]
        public List<CustomerPriceLogEntry> CustomerPriceCommentsList
        {
            get { return customerPriceCommentsList; }
            set
            {
                customerPriceCommentsList = value;
                OnPropertyChanged("CustomerPriceCommentsList");
            }
        }
        #endregion

        #region Constructor

        public CustomerPrice()
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
            CustomerPrice customerPrice = (CustomerPrice)this.MemberwiseClone();
            if (Status != null)
                customerPrice.Status = (LookupValue)this.Status.Clone();


            if (PlantList != null)
                customerPrice.PlantList = PlantList.Select(x => (Site)x.Clone()).ToList();

            if (CurrencyList != null)
                customerPrice.CurrencyList = CurrencyList.Select(x => (Currency)x.Clone()).ToList();


            if (PlantList_Cloned != null)
                customerPrice.PlantList_Cloned = PlantList_Cloned.Select(x => (Site)x.Clone()).ToList();

            if (CustomerPriceListItems != null)
                customerPrice.CustomerPriceListItems = CustomerPriceListItems.Select(x => (CustomerPriceListByItem)x.Clone()).ToList();

            if (Article_PlantList != null)
                customerPrice.Article_PlantList = Article_PlantList.Select(x => (Site)x.Clone()).ToList();

            if (Article_CurrencyList != null)
                customerPrice.Article_CurrencyList = Article_CurrencyList.Select(x => (Currency)x.Clone()).ToList();

            if (CustomerList != null)
                customerPrice.CustomerList = CustomerList.Select(x => (CPLCustomer)x.Clone()).ToList();

            if (Article_CurrencyList_Cloned != null)
                customerPrice.Article_CurrencyList_Cloned = Article_CurrencyList_Cloned.Select(x => (Currency)x.Clone()).ToList();

            if (Article_PlantList_Cloned != null)
                customerPrice.Article_PlantList_Cloned = Article_PlantList_Cloned.Select(x => (Site)x.Clone()).ToList();

            if (CustomerPriceListItems_Cloned != null)
                customerPrice.CustomerPriceListItems_Cloned = CustomerPriceListItems_Cloned.Select(x => (CustomerPriceListByItem)x.Clone()).ToList();

            if (CustomerPriceListDetections != null)
                customerPrice.CustomerPriceListDetections = CustomerPriceListDetections.Select(x => (CustomerPriceListByItem)x.Clone()).ToList();

            if (Detection_CurrencyList != null)
                customerPrice.Detection_CurrencyList = Detection_CurrencyList.Select(x => (Currency)x.Clone()).ToList();


            if (PLMCustomerAttachmentList != null)
                customerPrice.PLMCustomerAttachmentList = PLMCustomerAttachmentList.Select(x => (CPLDocument)x.Clone()).ToList();

            //if (CurrencyConversionsForCPLArticlesSaved != null)
            //    customerPrice.CurrencyConversionsForCPLArticlesSaved = CurrencyConversionsForCPLArticlesSaved.Select(x => (CurrencyConversion)x.Clone()).ToList();


            //if (CurrencyConversionsForCPLDetectionsSaved != null)
            //    customerPrice.CurrencyConversionsForCPLDetectionsSaved = CurrencyConversionsForCPLDetectionsSaved.Select(x => (CurrencyConversion)x.Clone()).ToList();

            if (CustomerPriceListModules != null)
                customerPrice.CustomerPriceListModules = CustomerPriceListModules.Select(x => (CustomerPriceListByItem)x.Clone()).ToList();

            if(CustomerPriceCommentsList!=null)
                customerPrice.CustomerPriceCommentsList = CustomerPriceCommentsList.Select(x => (CustomerPriceLogEntry)x.Clone()).ToList();

            return customerPrice;
        }

        #endregion
    }
}
