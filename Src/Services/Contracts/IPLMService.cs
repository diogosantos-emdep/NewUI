using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPLMService" in both code and config file together.
    [ServiceContract]
    public interface IPLMService
    {
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use IsDeletedBasePriceList_V2180 instead.")]
        bool IsDeletedBasePriceList(UInt64 idBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Language> GetLanguages();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Site> GetPlants();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestBasePriceCode();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use AddBasePrice_V2090 instead.")]
        BasePrice AddBasePrice(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use UpdateBasePrice_V2090 instead.")]
        bool UpdateBasePrice(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetBasePriceDetailById_V2090 instead.")]
        BasePrice GetBasePriceDetailById(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetPCMArticlesWithCategory_V2090 instead.")]
        List<PCMArticleCategory> GetPCMArticlesWithCategory();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCostPrice> GetArticleCostPrices();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetPCMArticlesWithCategoryForReference_V2090 instead.")]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference();

      
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2100. Use GetCurrencyConversionsByCurrency_V2100 instead.")]
        List<CurrencyConversion> GetCurrencyConversionsByCurrency(UInt32 IdCurrency);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetCustomerPrices_V2090 instead.")]
        List<CustomerPrice> GetCustomerPrices();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use IsDeletedCustomerPriceList_V2180 instead.")]
        bool IsDeletedCustomerPriceList(UInt64 idCustomerPriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2100. Use GetCustomerPricesById_V2100 instead.")]
        CustomerPrice GetCustomerPricesById(UInt64 idCustomerPriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestCustomerPriceCode();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BasePrice> GetBasePriceNames();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2100. Use AddCustomerPrice_V2100 instead.")]
        CustomerPrice AddCustomerPrice(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2100. Use UpdateCustomerPrice_V2100 instead.")]
        bool UpdateCustomerPrice(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use AddBasePrice_V2110 instead.")]
        BasePrice AddBasePrice_V2090(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use UpdateBasePrice_V2110 instead.")]
        bool UpdateBasePrice_V2090(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetBasePriceDetailById_V2110 instead.")]
        BasePrice GetBasePriceDetailById_V2090(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CPLCustomer> GetCPLCustomers();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetPCMArticlesWithCategory_V2160 instead.")]
        List<PCMArticleCategory> GetPCMArticlesWithCategory_V2090();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetPCMArticlesWithCategoryForReference_V2160 instead.")]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2090();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetCustomerPrices_V2110 instead.")]
        List<CustomerPrice> GetCustomerPrices_V2090();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryByIdBasePriceList(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryForReferenceByIdBasePriceList(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetBasePriceDetailById_ForCPL_V2110 instead.")]
        BasePrice GetBasePriceDetailById_ForCPL(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use AddCustomerPrice_V2110 instead.")]
        CustomerPrice AddCustomerPrice_V2100(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use UpdateCustomerPrice_V2110 instead.")]
        bool UpdateCustomerPrice_V2100(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetCustomerPricesById_V2110 instead.")]
        CustomerPrice GetCustomerPricesById_V2100(UInt64 idCustomerPriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Group> GetGroups();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetRegionsByGroupAndCountryAndSites_V2110 instead.")]
        List<Region> GetRegionsByGroupAndCountryAndSites(int IdGroup, string CountryNames, string SiteNames);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetCountriesByGroupAndRegionAndSites_V2110 instead.")]
        List<Country> GetCountriesByGroupAndRegionAndSites(int IdGroup, string RegionNames, string SiteNames);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetPlantsByGroupAndRegionAndCountry_V2110 instead.")]
        List<Site> GetPlantsByGroupAndRegionAndCountry(int IdGroup, string RegionNames, string CountryNames);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetCurrencyConversionsByCurrency_V2140 instead.")]
        List<CurrencyConversion> GetCurrencyConversionsByCurrency_V2100(UInt32 IdCurrency, DateTime CurrencyConversionDate);

     
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetBasePriceDetailById_V2120 instead.")]
        BasePrice GetBasePriceDetailById_V2110(UInt64 IdBasePriceList);

      
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetBasePriceDetailById_ForCPL_V2120 instead.")]
        BasePrice GetBasePriceDetailById_ForCPL_V2110(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteCustomerPriceListItem(List<CustomerPriceListByItem> customerPriceListByItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use CalculateArticleCostPrice_V2160 instead.")]
        bool CalculateArticleCostPrice();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Region> GetRegionsByGroupAndCountryAndSites_V2110(int IdGroup, string CountryNames, string SiteNames);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetCountriesByGroupAndRegionAndSites_V2110(int IdGroup, string RegionNames, string SiteNames);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Site> GetPlantsByGroupAndRegionAndCountry_V2110(int IdGroup, string RegionNames, string CountryNames);

       
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetCurrencyConversionsMaxRate_V2140 instead.")]
        List<CurrencyConversion> GetCurrencyConversionsMaxRate();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CurrencyConversion> GetCurrencyConversionsMaxRate_V2110(DateTime MinDate, DateTime MaxDate);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DateTime> GetMinMaxArticleExchangeRateDate();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetAllDetections_V2160 instead.")]
        List<BPLDetection> GetAllDetections();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use AddBasePrice_V2120 instead.")]
        BasePrice AddBasePrice_V2110(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use UpdateBasePrice_V2120 instead.")]
        bool UpdateBasePrice_V2110(BasePrice basePrice);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BasePrice AddBasePrice_V2120(BasePrice basePrice);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateBasePrice_V2120(BasePrice basePrice);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BasePrice GetBasePriceDetailById_V2120(UInt64 IdBasePriceList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Site> GetPlants_V2120();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategory_V2120();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2120();


         [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetBasePrices_V2140 instead.")]
        List<CurrencyConversion> GetCurrencyConversionsMaxRate_ByPreviousDate();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetBasePrices_V2120 instead.")]
        List<BasePrice> GetBasePrices();



       
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetCustomerPriceIFExistItem_V2120 instead.")]
        List<CustomerPrice> GetCustomerPriceIFExistItem(UInt64 IdBasePriceList, UInt64 IdArticle);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerPrice> GetCustomerPriceIFExistItem_V2120(UInt64 IdBasePriceList, string IdArticles);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use GetBasePriceDetailById_ForCPL_V2130 instead.")]
        BasePrice GetBasePriceDetailById_ForCPL_V2120(UInt64 IdBasePriceList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetBasePriceDetailById_ForCPL_V2140 instead.")]
        BasePrice GetBasePriceDetailById_ForCPL_V2130(UInt64 IdBasePriceList);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use AddCustomerPrice_V2130 instead.")]
        CustomerPrice AddCustomerPrice_V2110(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use UpdateCustomerPrice_V2130 instead.")]
        bool UpdateCustomerPrice_V2110(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use GetCustomerPricesById_V2130 instead.")]
        CustomerPrice GetCustomerPricesById_V2110(UInt64 idCustomerPriceList);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use AddCustomerPrice_V2150 instead.")]
        CustomerPrice AddCustomerPrice_V2130(CustomerPrice customerPrice);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetCustomerPricesById_V2150 instead.")]
        CustomerPrice GetCustomerPricesById_V2130(UInt64 idCustomerPriceList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use UpdateCustomerPrice_V2150 instead.")]
        bool UpdateCustomerPrice_V2130(CustomerPrice customerPrice);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CurrencyConversion> GetCurrencyConversionsByCurrency_V2140(UInt32 IdCurrency, DateTime CurrencyConversionDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetBasePriceDetailById_ForCPL_V2150 instead.")]
        BasePrice GetBasePriceDetailById_ForCPL_V2140(UInt64 IdBasePriceList);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CurrencyConversion> GetCurrencyConversionsMaxRate_V2140();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CurrencyConversion> GetCurrencyConversionsMaxRate_ByPreviousDate_V2140();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CurrencyConversion> GetCurrencyConversionsDetailsByDate(DateTime currencyConversionDate);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use AddBasePrice_V2150 instead.")]
        BasePrice AddBasePrice_V2140(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use UpdateBasePrice_V2150 instead.")]
        bool UpdateBasePrice_V2140(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetBasePriceDetailById_V2150 instead.")]
        BasePrice GetBasePriceDetailById_V2140(UInt64 IdBasePriceList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BasePrice AddBasePrice_V2150(BasePrice basePrice);
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetBasePriceDetailById_V2160 instead.")]
        BasePrice GetBasePriceDetailById_V2150(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetBasePrices_V2150 instead.")]
        List<BasePrice> GetBasePrices_V2120();

      

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CustomerPrice AddCustomerPrice_V2150(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCustomerPrice_V2150(CustomerPrice customerPrice);


      
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetBasePriceDetailById_ForCPL_V2160 instead.")]
        BasePrice GetBasePriceDetailById_ForCPL_V2150(UInt64 IdBasePriceList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetCustomerPrices_V2150 instead.")]
        List<CustomerPrice> GetCustomerPrices_V2110();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategory_V2160();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2160();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2220. Use GetAllDetections_V2220 instead.")]
        List<BPLDetection> GetAllDetections_V2160();
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetBasePriceDetailById_ForCPL_V2380 instead.")]
        BasePrice GetBasePriceDetailById_ForCPL_V2160(UInt64 IdBasePriceList);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetCustomerPricesById_V2160 instead.")]
        CustomerPrice GetCustomerPricesById_V2150(UInt64 idCustomerPriceList);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CustomerPrice GetCustomerPricesById_V2160(UInt64 idCustomerPriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetArticleCostPricesByCurrency_V2160 instead.")]
        List<ArticleCostPrice> GetArticleCostPricesByCurrency(UInt32 IdCurrency);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetArticleCostPricesByCurrency_V2650 instead.")]
        List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2160(UInt32 IdCurrency);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2640(UInt32 IdCurrency);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2640New(UInt32 IdCurrency);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetCurrencies_V2160 instead.")]

        List<Currency> GetCurrencies();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Currency> GetCurrencies_V2160();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetBasePrices_V2160 instead.")]

        List<BasePrice> GetBasePrices_V2150();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetBasePrices_V2180 instead.")]
        List<BasePrice> GetBasePrices_V2160();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetCustomerPrices_V2160 instead.")]

        List<CustomerPrice> GetCustomerPrices_V2150();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use GetCustomerPriceListItem_User_ById instead.")]
        List<CustomerPrice> GetCustomerPrices_V2160();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CurrencyConversion> GetCurrencyConversionsDetailsByLatestDate();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetEmdepSitesCompanies_V2490 instead.")]
        IList<Company> GetEmdepSitesCompanies();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UInt32> GetAllPCMIdArticles();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PODetail> GetAllArticleMaxPODeliveryDateDetailFromEWHQ();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PODetail> GetAllArticlesByArticleComponentMaxPOFromEWHQ();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticlesByArticle> GetAllArticlesByArticle();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BasePrice GetBasePriceDetailsAfterSavedDataById(UInt64 IdBasePriceList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CustomerPrice GetCustomerPriceDetailsAfterSavedDataById(UInt64 idCustomerPriceList);
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use GetBasePriceListItem_User_ById instead.")]
        List<BasePrice> GetBasePrices_V2180();
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedBasePriceList_V2180(UInt64 idBasePriceList, uint IdModifier);
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedCustomerPriceList_V2180(UInt64 idCustomerPriceList, uint IdModifier);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use UpdateBasePrice_V2180 instead.")]
        bool UpdateBasePrice_V2150(BasePrice basePrice);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetBasePriceDetailById_V2180 instead.")]
        BasePrice GetBasePriceDetailById_V2160(UInt64 IdBasePriceList);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerPrice> GetCustomerPriceIFExistDetections(UInt64 IdBasePriceList, string IdDetections);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateBasePrice_V2180(BasePrice basePrice);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BasePrice GetBasePriceDetailById_V2180(UInt64 IdBasePriceList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetCustomerPriceCodesByBPL(UInt64 IdBasePriceList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerPrice> GetCustomerPriceIFExistCurrencies(UInt64 IdBasePriceList, string IdCurrencies);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BPLPlantCurrencyDetail> GetBPLPlantCurrencyDetail(string IdArticles, string filtertext);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BPLPlantCurrencyDetail> GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Int32 IdArticle, string IdsBPL, string IdsCPL, string filtertext);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2210. Use CalculateArticleCostPrice_V2210 instead.")]
        bool CalculateArticleCostPrice_V2160(Company company);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2220. Use CalculateArticleCostPrice_V2220 instead.")]
        bool CalculateArticleCostPrice_V2210(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BPLDetection> GetAllDetections_V2220();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<List<ErrorDetails>> IsPLMAddEditBasePriceSynchronization(List<GeosAppSetting> GeosAppSettingList, BasePriceListByPlantCurrency itemPlantCurrency, DataTable DtArticle);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<List<ErrorDetails>> IsPLMUpdateEditBasePriceSynchronization(List<GeosAppSetting> GeosAppSettingList, BasePriceListByPlantCurrency itemBasePriceListByPlantCurrency, List<Articles> LstArticles);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<List<ErrorDetails>> IsPLMAddEditCPLCustomerSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrencyDetail, DataColumn[] columns, DataTable DtArticle);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<List<ErrorDetails>> IsPLMUpdateEditCPLCustomerSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBasePriceListByPlantCurrency, List<Articles> LstArticles);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<List<ErrorDetails>> IsPLMSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail BPLPlantCurrencyDetail, string Details, string Name);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<AdditionalArticleCost> GetAllAddedAdditionalArticleCost();


        //GEOS2-2999

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use CalculateArticleCostPrice_V2300 instead.")]
        bool CalculateArticleCostPrice_V2220(Company itemCompany);


     
        //GEOS2-3511

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Users> GetAllUsersList();

      

        //GEOS2-3511


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UserPermissionByBPLPriceList> GetAllUserPermissionsByBPLPriceList(DateTime dtFrom, DateTime dtTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UserPermissionByCPLPriceList> GetAllUserPermissionsByCPLPriceList(DateTime dtFrom, DateTime dtTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertUpdateUserPermissionByBPLForParticularColumn(UserPermissionByBPLPriceList userPermissionByBPLPriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertUpdateUserPermissionByCPLForParticularColumn(UserPermissionByCPLPriceList userPermissionByCPLPriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2370. Use GetBasePriceListItem_User_ById_V2370 instead.")]

        List<BasePrice> GetBasePriceListItem_User_ById(int IdUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetCustomerPriceListItem_User_ById_V2380 instead.")]
        List<CustomerPrice> GetCustomerPriceListItem_User_ById(int IdUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BasePrice> GetBasePriceListByDates(DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerPrice> GetCustomerPriceListByDates(DateTime fromDate, DateTime toDate);

      
        //GEOS2-3511


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteUserPermissionByBPLForParticularColumn(Int32 idUser, UInt64 idBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteUserPermissionByCPLForParticularColumn(Int32 idUser, UInt64 idCustomerPriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteUserPermissionByForParticularUser(Int32 idUser);

        // shubham[skadam] GEOS2-3851 Article_Cost_Price -> PK idarticle + idPlant  10 Aug 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2400. Use CalculateArticleCostPrice_V2400 instead.")]
        bool CalculateArticleCostPrice_V2300(Company itemCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2370. Use AddBasePrice_V2370 instead.")]
        BasePrice AddBasePrice_V2340(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BPLDocument> GetBPLAttachmentByIdBasePriceList(UInt32 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetBasePriceDetailById_V2340 instead.")]
        BasePrice GetBasePriceDetailById_V2340(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2370. Use UpdateBasePrice_V2370 instead.")]
        bool UpdateBasePrice_V2340(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use AddCustomerPrice_V2380 instead.")]
        CustomerPrice AddCustomerPrice_V2340(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use UpdateBasePrice_V2380 instead.")]
        bool UpdateCustomerPrice_V2340(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetCustomerPricesById_V2380 instead.")]
        CustomerPrice GetCustomerPricesById_V2340(UInt64 idCustomerPriceList);
        //[pjadhav][GEOS2-4015][10-01-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetCountriesByGroupAndRegionAndSites_V2350(int IdGroup, string RegionNames, string CountryNames);

        //Shubham[skadam] GEOS2-2886 [Only Modules ] - Able to  add and access to the tab Modules/Detection /Articles a Base Price List [3/3] [#PLM07] 28 03 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerPrice> GetCustomerPriceIFExistModules(UInt64 IdBasePriceList, string IdCPType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use UpdateBasePrice_V2470 instead.")]
        bool UpdateBasePrice_V2370(BasePrice basePrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use AddBasePrice_V2470 instead.")]
        BasePrice AddBasePrice_V2370(BasePrice basePrice);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetBasePriceDetailById_V2370 instead.")]
        BasePrice GetBasePriceDetailById_V2370(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BasePrice> GetBasePriceListItem_User_ById_V2370(int IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetBasePriceDetailById_ForCPL_V2650 instead.")]
        BasePrice GetBasePriceDetailById_ForCPL_V2380(UInt64 IdBasePriceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerPrice> GetCustomerPriceListItem_User_ById_V2380(int IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use UpdateCustomerPrice_V2470 instead.")]

        bool UpdateCustomerPrice_V2380(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use AddCustomerPrice_V2470 instead.")]

        CustomerPrice AddCustomerPrice_V2380(CustomerPrice customerPrice);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetCustomerPricesById_V2470 instead.")]
        CustomerPrice GetCustomerPricesById_V2380(UInt64 idCustomerPriceList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetEmdepSitesCompaniesWithServiceURL_V2490 instead.")]
        IList<Company> GetEmdepSitesCompaniesWithServiceURL();

        //[rdixit][GEOS2-4474][19.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CalculateArticleCostPrice_V2400(Company itemCompany, List<PODetail> EWHQpoDetailLst, List<PODetail> EWHQArticlesByArticleComponentpoDetailLst, List<ArticlePOAVG> GetPOAverageByAllPCMArticleEWHQLst);

        //[rdixit][GEOS2-4474][19.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PODetail> GetAllArticleMaxPODeliveryDateDetailFromEWHQ_V2210();

        //[rdixit][GEOS2-4474][19.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PODetail> GetAllArticlesByArticleComponentMaxPOFromEWHQ_V2210();

        //[rdixit][GEOS2-4474][19.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use WS_GetPOAverageByAllPCMArticleEWHQ_V2490 instead.")]
        List<ArticlePOAVG> WS_GetPOAverageByAllPCMArticleEWHQ(Int32 NumberPOAvg);

        //[rdixit][GEOS2-4520][05.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BasePriceListByPlantCurrency> GetPlantCurrencyByIdBasePrice(UInt64 IdBasePriceList);


        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
       
        BasePrice AddBasePrice_V2470(BasePrice basePrice);

        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use WS_GetPOAverageByAllPCMArticleEWHQ_V2640 instead.")]
        BasePrice GetBasePriceDetailById_V2470(UInt64 IdBasePriceList);

        //[Sudhir.Jangra][GEOS2-4935]
        //GEOS2-6688 PCM- Improve performance to open BPL grid and any BPL
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BasePrice GetBasePriceDetailById_V2640(UInt64 IdBasePriceList);

        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateBasePrice_V2470(BasePrice basePrice);

        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CustomerPrice AddCustomerPrice_V2470(CustomerPrice customerPrice);


        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetCustomerPricesById_V2650 instead.")]
        CustomerPrice GetCustomerPricesById_V2470(UInt64 idCustomerPriceList);

        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCustomerPrice_V2470(CustomerPrice customerPrice);

        //[cpatil][GEOS2-5299]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetEmdepSitesCompanies_V2500 instead.")]
        IList<Company> GetEmdepSitesCompanies_V2490();


        //[cpatil][GEOS2-5299][27.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticlePOAVG> WS_GetPOAverageByAllPCMArticleEWHQ_V2490(Int32 NumberPOAvg);

        //[cpatil][GEOS2-5299][27.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Company> GetEmdepSitesCompaniesWithServiceURL_V2490();

        //[rdixit][27.03.2024][GEOS2-5556]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        IList<Company> GetEmdepSitesCompanies_V2500();

        //[nsatpute][28-05-2025][GEOS2-6689]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CustomerPrice GetCustomerPricesById_V2650(UInt64 idCustomerPriceList);

        //[nsatpute][28-05-2025][GEOS2-6689]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2650(UInt32 IdCurrency);

        //[nsatpute][28-05-2025][GEOS2-6689]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BasePrice GetBasePriceDetailById_ForCPL_V2650(UInt64 IdBasePriceList);
    }
}
