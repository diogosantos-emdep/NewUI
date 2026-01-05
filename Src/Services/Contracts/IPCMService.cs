using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Data.Common.SynchronizationClass;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPCMService" in both code and config file together.
    [ServiceContract]
    public interface IPCMService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CatalogueItem> GetAllCatalogueItems();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetAllProductTypes_V2110 instead.")]
        List<ProductTypes> GetAllProductTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Template> GetAllTemplates();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorFamilies> GetAllFamilies();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionTypes> GetAllDetectionTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetAllWayList_V2300 instead.")]
        List<Ways> GetAllWayList();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Detections> GetAllDetectionList();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Options> GetAllOptionList();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SpareParts> GetAllSparePartList();

       
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupValues(byte key);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CatalogueItem AddCatalogueItem(CatalogueItem catalogueItemModel);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCatalogueItem(CatalogueItem catalogueItemModel);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedCatalogueItem(UInt32 idCatalogueItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestCatalogueItemCode();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetProductTypesByTemplate(UInt64 idTemplate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CatalogueItem GetCatalogueItemByIdCatalogueItem(UInt32 IdCatalogueItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TestTypes> GetAllTestTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use AddProductType_V2140 instead.")]
        ProductTypes AddProductType(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use UpdateProductType_V2140 instead.")]
        bool UpdateProductType(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use AddDetection_V2140 instead.")]
        DetectionDetails AddDetection(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use UpdateDetection_V2140 instead.")]
        bool UpdateDetection(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetProductTypeByIdCpType_V2140 instead.")]
        ProductTypes GetProductTypeByIdCpType(UInt64 IdCpType, UInt64 IdTemplate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetDetectionByIdDetection_V2140 instead.")]
        DetectionDetails GetDetectionByIdDetection(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Language> GetAllLanguages();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeleteProductTypeImage(ProductTypeImage productTypeImage);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypeImage> GetProductTypeImagesByIdProductType(UInt64 IdProductType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypeImage GetProductTypeImagesByIdProductTypeImage(UInt64 IdProductTypeImage);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeleteProductTypeAttachedDoc(ProductTypeAttachedDoc productTypeAttachedDoc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypeAttachedDoc> GetProductTypeAttachedDocsByIdProductType(UInt64 IdProductType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypeAttachedDoc GetProductTypeAttachedDocsByIdProductTypeAttachedDoc(Int32 IdProductTypeAttachedDoc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CatalogueItemAttachedDoc> GetCatalogueItemAttachedDocsByIdCatalogueItem(UInt32 IdCatalogueItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CatalogueItemAttachedDoc GetCatalogueItemAttachedDocsById(UInt32 IdCatalogueItemAttachedDoc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionAttachedDoc> GetDetectionAttachedDocsByIdDetection(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionAttachedDoc GetDetectionAttachedDocsById(UInt32 IdDetectionAttachedDoc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CatalogueItemAttachedLink> GetCatalogueItemAttachedLinksByIdCatalogueItem(UInt32 IdCatalogueItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CatalogueItemAttachedLink GetCatalogueItemAttachedLinkById(UInt32 IdCatalogueItemAttachedLink);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypeAttachedLink> GetProductTypeAttachedLinksByIdProductType(UInt64 IdCPType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypeAttachedLink GetProductTypeAttachedLinkById(UInt32 IdCPTypeAttachedLink);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionAttachedLink> GetDetectionAttachedLinksByIdDetection(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionAttachedLink GetDetectionAttachedLinkById(UInt32 IdDetectionAttachedLink);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestProuductTypeReference();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedProductType(UInt64 IdCPType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DefaultWayType> GetAllDefaultWayTypeList();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionImage> GetDetectionImagesByIdDetection(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionImage GetDetectionImagesByIdDetectionImage(UInt32 IdDetectionImage);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionGroup> GetDetectionGroupsByDetectionType(UInt32 IdDetectionType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionOrderGroup> GetDetectionOrderGroupsWithDetections(UInt32 IdDetectionType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionGroup> GetDetectionGroupsList(UInt32 IdDetectionType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionGroup GetDetectionGroupsByIdGroup(UInt32 IdGroup);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetDetectionsConcatByIdGroup(UInt32 IdGroup);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionOrderGroup> GetDetectionOrderGroup(UInt32 IdDetectionType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Detections> GetDetectionGroups(UInt32 IdDetectionType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Options> GetOptionGroups(UInt32 IdDetectionType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetAllDetectionsWithGroups_V2300 instead.")]
        List<Detections> GetAllDetectionsWithGroups();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetAllOptionsWithGroups_V2300 instead.")]
        List<Options> GetAllOptionsWithGroups();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCategories> GetArticleCategories();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetAllArticles();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetArticlesByCategory(uint IdArticleCategory);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use GetPCMArticleCategories_V2060 instead.")]
        List<PCMArticleCategory> GetPCMArticleCategories();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use GetAllPCMArticles_V2060 instead.")]
        List<Articles> GetAllPCMArticles();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetPCMArticlesByCategory(uint IdArticleCategory);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticleCategoryInArticle(uint IdPCMArticleCategory, List<Articles> ArticleList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCategories> GetActiveArticleCategories();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetAllActiveArticles();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetActiveArticlesByCategory(uint IdArticleCategory);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetActivePCMArticleCategories();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetAllActivePCMArticles();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetActivePCMArticlesByCategory(uint IdArticleCategory);

    
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use GetArticleByIdArticle_V2060 instead.")]
        Articles GetArticleByIdArticle(UInt32 IdArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use IsUpdatePCMArticleCategoryInArticleWithStatus_V2060 instead.")]
        bool IsUpdatePCMArticleCategoryInArticleWithStatus(uint IdPCMArticleCategory, Articles Article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypesTemplate> GetProductTypesWithTemplate();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use GetPCMArticlesWithCategory_V2060 instead.")]
        List<PCMArticleCategory> GetPCMArticlesWithCategory();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use IsDeletedDetection_V2500 instead.")]
        bool IsDeletedDetection(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetAllDetectionsWaysOptionsSpareParts_V2110 instead.")]
        List<DetectionDetails> GetAllDetectionsWaysOptionsSpareParts();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleDocument> GetArticleAttachmentByIdArticle(UInt32 idArticle);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatedPCMArticleCategoryOrder(List<PCMArticleCategory> pcmArticleCategoryList, uint? IdModifier);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use GetPCMArticlesWithCategoryForReference_V2060 instead.")]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletePCMArticleCategory(List<PCMArticleCategory> pcmArticleCategoryList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2070. Use IsUpdatePCMArticleCategoryInArticleWithStatus_V2070 instead.")]
        bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2060(uint IdPCMArticleCategory, Articles Article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2070. Use GetAllPCMArticles_V2070 instead.")]
        List<Articles> GetAllPCMArticles_V2060();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2070. Use GetArticleByIdArticle_V2070 instead.")]
        Articles GetArticleByIdArticle_V2060(UInt32 IdArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategory_V2060();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2060();

      
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use IsUpdatePCMArticleCategoryInArticleWithStatus_V2090 instead.")]
        bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2070(uint IdPCMArticleCategory, Articles Article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2100. Use GetAllPCMArticles_V2100 instead.")]
        List<Articles> GetAllPCMArticles_V2070();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetArticleByIdArticle_V2090 instead.")]
        Articles GetArticleByIdArticle_V2070(UInt32 IdArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2100. Use IsUpdatePCMArticleCategoryInArticleWithStatus_V2100 instead.")]
        bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2090(uint IdPCMArticleCategory, Articles Article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2100. Use GetArticleByIdArticle_V2100 instead.")]
        Articles GetArticleByIdArticle_V2090(UInt32 IdArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetArticleByIdArticle_V2120 instead.")]
        Articles GetArticleByIdArticle_V2100(UInt32 IdArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use IsUpdatePCMArticleCategoryInArticleWithStatus_V2110 instead.")]
        bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2100(uint IdPCMArticleCategory, Articles Article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DocumentType> GetDocumentTypes();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetArticleByIdArticle_V2110 instead.")]
        List<Articles> GetAllPCMArticles_V2100();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use GetArticleByIdArticle_V2290 instead.")]
        List<Articles> GetAllPCMArticles_V2110();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2110(uint IdPCMArticleCategory, Articles Article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetAllDetectionsWaysOptionsSpareParts_V2160 instead.")]
        List<DetectionDetails> GetAllDetectionsWaysOptionsSpareParts_V2110();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2270. Use GetAllProductTypes_V2270 instead.")]
        List<ProductTypes> GetAllProductTypes_V2110();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int CheckPCMArticleExist(UInt32 IdArticle);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Articles GetArticleByIdArticleForInformationData(uint IdArticle);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticleFromGrid(uint IdPCMArticleCategory, Articles Article);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetPCMArticleExistNames_V2120 instead.")]
        string GetPCMArticleExistNames(UInt32 IdArticle);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetPCMArticleExistNames_V2120(UInt32 IdArticle);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use AddDeletePCMArticle_V2120 instead.")]
        bool AddDeletePCMArticle(uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use AddDeletePCMArticle_V2140 instead.")]
        bool AddDeletePCMArticle_V2120(uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use UpdateDetection_V2160 instead.")]
        bool UpdateDetection_V2140(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use AddDetection_V2160 instead.")]
        DetectionDetails AddDetection_V2140(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use AddDeletePCMArticle_V2160 instead.")]
        DetectionDetails GetDetectionByIdDetection_V2140(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetAllSparepartsWithGroups_V2300 instead.")]
        List<SpareParts> GetAllSparepartsWithGroups();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddDeletePCMArticle_V2140(uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use AddDeletePCMArticle_V2180 instead.")]
        DetectionDetails GetDetectionByIdDetection_V2160(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionDetails> GetAllDetectionsWaysOptionsSpareParts_V2160();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use AddDetection_V2180 instead.")]
        DetectionDetails AddDetection_V2160(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use UpdateDetection_V2180 instead.")]
        bool UpdateDetection_V2160(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCM_DetectionECOSVisibility_Update_V2160(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionDetails> PCM_GetshortDetectionDetails_V2160();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use IsUpdatePCM_MicroSigaInformation_V2180 instead.")]
        bool IsUpdatePCM_MicroSigaInformation(List<MicroSigainformation> microSigainformation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use IsUpdatePCMArticle_V2170 instead.")]
        bool IsUpdatePCMArticle(uint IdPCMArticleCategory, Articles Article);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetArticleByIdArticle_V2170 instead.")]
        Articles GetArticleByIdArticle_V2120(UInt32 IdArticle);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetArticleByIdArticle_V2240 instead.")]
        Articles GetArticleByIdArticle_V2170(UInt32 IdArticle);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use IsUpdatePCMArticle_V2260 instead.")]
        bool IsUpdatePCMArticle_V2170(uint IdPCMArticleCategory, Articles Article);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetDetectionByIdDetection_V2240 instead.")]
        DetectionDetails GetDetectionByIdDetection_V2180(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetNotIncludedPLMDetectionPrices_V2240 instead.")]
        List<PLMDetectionPrice> GetNotIncludedPLMDetectionPrices(
      string IdBasePriceListCommaSeparated, string IdCustomerPriceListCommaSeparated,
      UInt64 IdDetection);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetEmdepSitesCompanies_V2490 instead.")]
        IList<Company> GetEmdepSitesCompanies();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ImportArticleCostPriceCalculate(Company company, UInt64 itemArticle, List<PODetail> EWHQArticlesByArticleComponentpoDetailLst, List<ArticlesByArticle> LstAllArticlesByArticle, List<PODetail> ArticlesByArticleComponentpoDetailLst);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PODetail> GetAllArticlesByArticleComponentMaxPOFromEWHQ(string plantconnectionstring);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticlesByArticle> GetAllArticlesByArticle(string connectionstring);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PODetail> GetAllArticlesByArticleComponentMaxPOFromPlant(string connectionstring);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetAllStatusPCMArticles();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2310. Use IsPCMArticlesSynchronization_V2310 instead.")]
        Task<List<ErrorDetails>> IsPCMArticlesSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, Articles UpdatedArticle);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<List<ErrorDetails>> IsPCMAddDetectionSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, DetectionDetails NewDetection);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<APIErrorDetailForErrorFalse> IsPCMDetectionSynchronization(List<GeosAppSetting> GeosAppSettingList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<List<ErrorDetails>> IsPCMEditDetectionSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, DetectionDetails UpdatedItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<APIErrorDetailForErrorFalse> IsPCMAddEditCategorySynchronization(List<GeosAppSetting> GeosAppSettingList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<APIErrorDetailForErrorFalse> IsPCMProductTypeArticleSynchronization(List<GeosAppSetting> GeosAppSettingList, Articles[] foundRow);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<List<ErrorDetails>> IsPCMSynchronization(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, string Details, string Name);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetProductTypeByIdCpType_V2250 instead.")]
        ProductTypes GetProductTypeByIdCpType_V2140(UInt64 IdCpType, UInt64 IdTemplate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetProductTypeByIdCpType_V2350 instead.")]
        ProductTypes GetProductTypeByIdCpType_V2250(UInt64 IdCpType, UInt64 IdTemplate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use AddProductType_V2250 instead.")]
        ProductTypes AddProductType_V2140(ProductTypes productTypes);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use UpdateProductType_V2250 instead.")]
        bool UpdateProductType_V2140(ProductTypes productTypes);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetArticleByIdArticle_V2250 instead.")]
        Articles GetArticleByIdArticle_V2240(UInt32 IdArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use GetArticleByIdArticle_V2290 instead.")]
        Articles GetArticleByIdArticle_V2250(UInt32 IdArticle, int IdUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetDetectionByIdDetection_V2250 instead.")]
        DetectionDetails GetDetectionByIdDetection_V2240(UInt32 IdDetection);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetNotIncludedPLMDetectionPrices_V2250 instead.")]
        List<PLMDetectionPrice> GetNotIncludedPLMDetectionPrices_V2240(
        string IdBasePriceListCommaSeparated, string IdCustomerPriceListCommaSeparated, UInt64 IdDetection);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetDetectionByIdDetection_V2330 instead.")]
        DetectionDetails GetDetectionByIdDetection_V2250(UInt32 IdDetection, int IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PLMDetectionPrice> GetNotIncludedPLMDetectionPrices_V2250(
        string IdBasePriceListCommaSeparated, string IdCustomerPriceListCommaSeparated, UInt64 IdDetection, int IdUser);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticle_V2260(uint IdPCMArticleCategory, Articles Article);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use AddProductType_V2260 instead.")]
        ProductTypes AddProductType_V2250(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2270. Use AddProductType_V2270 instead.")]
        ProductTypes AddProductType_V2260(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use GetCustomersWithRegions_V2260 instead.")]
        List<RegionsByCustomer> GetCustomersWithRegions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use GetCustomersWithRegions_V2280 instead.")]
        List<CPLCustomer> GetCustomersWithRegions_V2260(UInt64 IdCpType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use UpdateProductTypes_V2260 instead.")]
        bool UpdateProductType_V2250(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use UpdateProductTypes_V2280 instead.")]
        bool UpdateProductType_V2260(ProductTypes productTypes);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version _V2260. Use AddDetection_V2260 instead.")]
        DetectionDetails AddDetection_V2180(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version _V2280. Use GetCustomersWithRegionsByIdDetection_V2280 instead.")]
        List<CPLCustomer> GetCustomersWithRegionsByIdDetection_V2260(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version _V2260. Use UpdateDetection_V2260 instead.")]
        bool UpdateDetection_V2180(DetectionDetails detectionDetails);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use IsUpdatePCM_MicroSigaInformation_V2260 instead.")]
        bool IsUpdatePCM_MicroSigaInformation_V2180(List<MicroSigainformation> microSigainformation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetDetectionTypeMaxWelOrder();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistDetectionTypeWelOrder(UInt32 welOrder);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        [ObsoleteAttribute("This method will be removed after version V2360. Use GetAllProductTypes_V2360 instead.")]
        List<ProductTypes> GetAllProductTypes_V2270();

        //[sdeshpande][GEOS2-3759]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetAllProductTypes_V2360();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes AddProductType_V2270(ProductTypes productTypes);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetCustomersWithRegions_V2530 instead.")]

        List<CPLCustomer> GetCustomersWithRegions_V2280(UInt64 IdCpType);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes AddProductType_V2280(ProductTypes productTypes);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProductType_V2280(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use UpdateDetection_V2280 instead.")]
        bool UpdateDetection_V2260(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version _V2280. Use UpdateDetection_V2330 instead.")]
        bool UpdateDetection_V2280(DetectionDetails detectionDetails);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use AddDetection_V2260 instead.")]
        DetectionDetails AddDetection_V2260(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use AddDetection_V2330 instead.")]
        DetectionDetails AddDetection_V2280(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CPLCustomer> GetCustomersWithRegionsByIdDetection_V2280(UInt32 IdDetection);


        //[rdixit][GEOS2- 2571][04.07.2022][added field pcmCategoryInUse]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticleCategories_V2290();

        //[rdixit][GEOS2-GEOS2-2571][06.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetAllPCMArticles_V2290();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleDecomposition> GetArticleDeCompostionByIdArticle(UInt32 idArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use GetPCMArticleCategories_V2290 instead.")]
        List<PCMArticleCategory> GetPCMArticleCategories_V2060();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Articles GetArticleByIdArticle_V2290(UInt32 IdArticle, int IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2290();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategory_V2290();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PCMArticleCategory GetPCMArticleCategoryById_V2290(uint idPCMArticleCategory);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticleCategory_V2290(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PCMArticleCategory AddPCMArticleCategory_V2290(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use AddPCMArticleCategory_V2290 instead.")]
        PCMArticleCategory AddPCMArticleCategory(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use IsUpdatePCMArticleCategory_V2290 instead.")]
        bool IsUpdatePCMArticleCategory(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use GetPCMArticleCategoryById_V2290 instead.")]
        PCMArticleCategory GetPCMArticleCategoryById(uint idPCMArticleCategory);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use IsUpdatePCM_MicroSigaInformation_V2300 instead.")]
        bool IsUpdatePCM_MicroSigaInformation_V2260(List<MicroSigainformation> microSigainformation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCM_MicroSigaInformation_V2300(List<MicroSigainformation> microSigainformation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Options> GetAllOptionsWithGroups_V2300();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ways> GetAllWayList_V2300();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Detections> GetAllDetectionsWithGroups_V2300();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SpareParts> GetAllSparepartsWithGroups_V2300();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetDiscounts_V2320 instead.")]
        List<Discounts> GetDiscounts();

        //[rdixit][12.09.2022][GEOS2-3100]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use AddDiscount_V2470 instead.")]
        Discounts AddDiscount(Discounts Discount);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Task<List<ErrorDetails>> IsPCMArticlesSynchronization_V2310(List<GeosAppSetting> GeosAppSettingList, BPLPlantCurrencyDetail itemBPLPlantCurrency, Articles UpdatedArticle, bool IsArticleSynchronization);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Discounts> GetDiscounts_V2320();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use UpdateDiscount_V2470 instead.")]
        Discounts UpdateDiscount(Discounts Discount, Discounts PrevDiscount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DiscountCustomers> GetDiscountCustomers(int customer_DiscountId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetDiscountLogEntriesByDiscountstring_V2470 instead.")]

        List<DiscountLogEntry> GetDiscountLogEntriesByDiscountstring(int IdCustomerDiscount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use AddDetection_V2340 instead.")]
        DetectionDetails AddDetection_V2330(DetectionDetails detectionDetails);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        bool UpdateDetection_V2330(DetectionDetails detectionDetails);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionDetails> GetAllDetectionsWaysOptionsSpareParts_V2330();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetDetectionByIdDetection_V2340 instead.")]
        DetectionDetails GetDetectionByIdDetection_V2330(UInt32 IdDetection, int IdUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetAllModuleDetectionsWaysOptionsSpareParts_V2340 instead.")]
        List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2330();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetAllStructureDetectionsWaysOptionsSpareParts_V2340 instead.")]
        List<DetectionDetails> GetAllStructureDetectionsWaysOptionsSpareParts_V2330();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use IsUpdatePCM_DetectionECOSVisibility_Update_V2560 instead.")]
        bool IsUpdatePCM_DetectionECOSVisibility_Update_V2340(DetectionDetails detectionDetails);
        //[rdixit][GEOS2-3970][01.12.2022] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2340();
        //[rdixit][GEOS2-3970][01.12.2022] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionDetails> GetAllStructureDetectionsWaysOptionsSpareParts_V2340();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        bool UpdateDetection_V2340(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetDetectionByIdDetection_V2360 instead.")]
        DetectionDetails GetDetectionByIdDetection_V2340(UInt32 IdDetection, int IdUser);

        //[rdixit][GEOS2-4074][12.12.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use AddDetection_V2470 instead.")]
        DetectionDetails AddDetection_V2340(DetectionDetails detectionDetails);

        //[Sudhir.jangra][Geos-4072][12/12/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use UpdateProductType_V2470 instead.")]
        bool UpdateProductType_V2340(ProductTypes productTypes);
        //[Sudhir.jangra][Geos-4072][12/12/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetDetectionByIdDetection_V2470 instead.")]
        ProductTypes AddProductType_V2340(ProductTypes productTypes);

        //[Sudhir.Jangra][Geos2-4072][13/12/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypeAttachedDoc> GetProductTypeAttachedDocsByIdProductType_V2340(UInt64 IdProductType);

        // Shubham[skadam] GEOS2-2596 Add option in PCM to print a datasheet of a Module [1 of 3] 06 01 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes GetProductTypeByIdCpType_V2350(UInt64 IdCpType, UInt64 IdTemplate);
        // Shubham[skadam] GEOS2-2596 Add option in PCM to print a datasheet of a Module [1 of 3] 06 01 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectiontypesInformations> GetDetectionTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetArticleByIdArticle_V2660 instead.")]
        Articles GetArticleByIdArticle_V2350(UInt32 IdArticle, int IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCustomer> GetArticleCustomers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticle_V2350(uint IdPCMArticleCategory, Articles Article);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCustomer> GetCustomersByIdArticleCustomerReferences(UInt64 IdArticleList);

        //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetDetectionByIdDetection_V2470 instead.")]
        DetectionDetails GetDetectionByIdDetection_V2360(UInt32 IdDetection, int IdUser);

        //Shubham[skadam] GEOS2-3890 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 2 23 02 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2360(string idCompany);

        //[plahange][GEOS2-2544][22.02.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConfigurationCategories> GetAllPCMCategories_V2360();

        //[rdixit][22.02.2023][GEOS2-4176]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleSynchronization> GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Int32 IdArticle, string IdsBPL, string IdsCPL, string FilterString);

        //[Sudhir.Jangra][GEOS2-3891][27/02/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PCMAnnouncementEmailDetails GetEmailForDows_V2360(DateTime startDate, DateTime endDate, Boolean NewChangeType, Boolean UpdateChangeType, string EmployeeContactEmail);
        //Shubham[skadam] GEOS2-3891 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 3 01 03 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string ReadMailTemplate(string templateName);
        //Shubham[skadam] GEOS2-3891 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 3 01 03 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsPCMAnnouncementEmailSend(string EmployeeContactEmail, string PCMAnnouncementEmailTemplate);
        //Shubham[skadam] GEOS2-3891 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 3 01 03 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use IsPCMAnnouncementEmailSend_V2420 instead.")]
        bool IsPCMAnnouncementEmailSend_V2360(string EmployeeContactEmail, string PCMAnnouncementEmailTemplate, Dictionary<string, byte[]> AnnouncementFilebyte);
        //Shubham[skadam] GEOS2-3891 Add a new option to send "Announcement" email for new and reviewed modules in PCM - 3 01 03 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version _V2420. Use GetEmailForDows_V2420 instead.")]
        PCMAnnouncementEmailDetails GetEmailForDows_V2360New(DateTime startDate, DateTime endDate, Boolean NewChangeType, Boolean UpdateChangeType, string EmployeeContactEmail);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, byte[]> GetCountryIconFileInBytes();

        //[Sudhir.Jangra][GEOS2-2922][17/03/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetAllPCMArticles_V2380 instead.")]
        List<Articles> GetAllPCMArticles_V2370();

        //[Sudhir.Jangra][GEOS2-2922][20/03/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetAllModuleDetectionsWaysOptionsSpareParts_V2380 instead.")]
        List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2370();


        //[Sudhir.Jangra][GEOS2-2922][20/03/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetDetectionImagesByIdDetection_V2380 instead.")]
        ObservableCollection<DetectionImage> GetDetectionImagesByIdDetection_V2370(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetAllProductTypes_V2380 instead.")]
        List<ProductTypes> GetAllProductTypes_V2370();

        ////[Sudhir.Jangra][GEOS2-2922][23/03/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetArticleImage_V2380 instead.")]
        List<PCMArticleImage> GetArticleImage_V2370(UInt32 IdArticle, string articleReference);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BPLModule> GetProductTypesWithTemplate_V2370();


        //[Sudhir.Jangra][GEOS2-2922][29/03/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetAllStructureDetectionsWaysOptionsSpareParts_V2380 instead.")]
        List<DetectionDetails> GetAllStructureDetectionsWaysOptionsSpareParts_V2370();

        #region V2380
        //[Sudhir.jangra][GEOS2-4221][12/04/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes AddProductType_V2380(ProductTypes productTypes);
        //[Sudhir.Jangra][GEOS2-4221][12/04/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProductType_V2380(ProductTypes productTypes);
        //[Sudhir.Jangra][GEOS2-4221][12/04/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes GetProductTypeByIdCpType_V2380(UInt64 IdCpType, UInt64 IdTemplate);

        //[rdixit][GEOS2-2922][24.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetAllPCMArticles_V2440 instead.")]
        List<Articles> GetAllPCMArticles_V2380();

        //[rdixit][GEOS2-2922][24.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionDetails> GetAllStructureDetectionsWaysOptionsSpareParts_V2380();

        //[rdixit][GEOS2-2922][24.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use GetAllModuleDetectionsWaysOptionsSpareParts_V2590 instead.")]
        List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2380();

        //[rdixit][GEOS2-2922][24.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use GetAllProductTypes_V2590 instead.")]
        List<ProductTypes> GetAllProductTypes_V2380();

        //[rdixit][GEOS2-2922][24.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleImage> GetArticleImage_V2380(Articles article);

        //[rdixit][GEOS2-2922][24.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ObservableCollection<DetectionImage> GetDetectionImagesByIdDetection_V2380(UInt32 IdDetection);

        //[rdixit][GEOS2-2922][24.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypeImage> GetProductTypeImagesByIdProductTypeForGrid_V2380(UInt64 IdProductType);


        //[Sudhir.Jangra][GEOS2-4221]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetProductTypeByIdCpType_V2470 instead.")]
        ProductTypes GetProductTypeByIdCpType_V2390(UInt64 IdCpType, UInt64 IdTemplate);

        //[Sudhir.Jangra][GEOS2-4468][01/06/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use AddDetection_V2560 instead.")]
        DetectionDetails AddDetection_V2400(DetectionDetails detectionDetails);

        //[Sudhir.Jangra][GEOS2-4460][26/06/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetProductTypesByDetection_V2410();

        //[Sudhir.Jangra][GEOS2-4460][28/06/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use UpdateDetection_V2470 instead.")]
        bool UpdateDetection_V2410(DetectionDetails detectionDetails);
        #endregion

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsPCMAnnouncementEmailSend_V2420(string EmployeeContactEmail, string PCMAnnouncementEmailTemplate, Dictionary<string, byte[]> AnnouncementFilebyte);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PCMAnnouncementEmailDetails GetEmailForDows_V2420(DateTime startDate, DateTime endDate, Boolean NewChangeType, Boolean UpdateChangeType, string EmployeeContactEmail);


        //[Sudhir.Jangra][GEOS2-4733][22/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Options> GetAllOptionsWithGroups_V2430(int IdScope);


        //[Sudhir.Jangra][GEOS2-4733][22/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Detections> GetAllDetectionsWithGroups_V2430(int IdScope);


        //[PRAMOD.MISAL][GEOS2-4442][29-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<FreePlugins> GetAllFreePlugins_byPermission();

        //[pramod.misal][GEOS2-4443][01-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        FreePlugins AddUpdateFreePlugins(FreePlugins freePlugins);

        //[Rahul.Gadhave][GEOS2-4442][31-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<FreePlugins> GetHardlockFreePluginNames();

        //[Sudhir.Jangra][GEOS2-4441][21/09/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<HardLockLicenses> GetAllHardLockLicenses_V2440();

        //[Sudhir.jangra][GEOS2-4441]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetArticleByIdArticle_V2660 instead.")]
        Articles GetArticleByIdArticle_V2440(UInt32 IdArticle, UInt32 IdPCMArticle, int IdUser);


        //[Sudhir.Jangra][GEOS2-4809]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetAllPCMArticles_V2460 instead.")]
        List<Articles> GetAllPCMArticles_V2440();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdateFreePlugins_V2440(List<FreePlugins> freePlugins);

        //[Sudhir.Jangra][GEOS2-4901]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<HardLockLicenses> GetArticlesForAddEditHardLockLicense_V2450();

        //[Sudhir.Jangra][GEOS2-4901]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<HardLockPlugins> GetAllHardLockPluginForAddEditHardLockLicense_V2450();

        //[Sudhir.jangra][GEOS2-4901]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        bool AddHardLockLicense_V2450(UInt32 IdArticle, List<HardLockPlugins> pluginList);

        //[Sudhir.Jangra][GEOS2-4901]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetSupportedPluginByIdArticle_V2470 instead.")]
        List<HardLockPlugins> GetSupportedPluginByIdArticle_V2450(UInt32 idArticle);

        //[Sudhir.Jangra][GEOS2-4901]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteSupportedPluginForHardLockLicense_V2450(UInt32 idPlugin, UInt32 idArticle);

        //[Sudhir.Jangra][GEOS2-4915]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetHardLockPluginId_V2470 instead.")]
        HardLockPlugins GetHardLockPluginId_V2450();

        //[Sudhir.jangra][GEOS2-4915]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddHardLockPlugin_V2450(UInt32 idPlugin, string Name);

        //[rdixit][GEOS2-4897][01.12.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2540. Use GetAllPCMArticles_V2540 instead.")]
        List<Articles> GetAllPCMArticles_V2460(UInt32 idCurruncy);

        //[rdixit][GEOS2-4897][01.12.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetEmdepSites();

        //[Sudhir.Jangra][GEOS2-4874]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCategory> GetWMSArticlesWithCategoryForReference();

        //[pramod.misal][GEOS2-5134][18-12-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<HardLockPlugins> GetSupportedPluginByIdArticle_V2470(UInt32 idArticle);
        //Shubham[skadam] GEOS2-5133 Add flag in country column loaded through url service 20 12 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        HardLockPlugins GetHardLockPluginId_V2470();



        //[Sudhir.jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use UpdateProductType_V2490 instead.")]
        bool UpdateProductType_V2470(ProductTypes productTypes);


        //[Sudhir.jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use AddProductType_V2490 instead.")]
        ProductTypes AddProductType_V2470(ProductTypes productTypes);

        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetProductTypeByIdCpType_V2490 instead.")]
        ProductTypes GetProductTypeByIdCpType_V2470(UInt64 IdCpType, UInt64 IdTemplate);

        //[GEOS2-4874][25.12.2023][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCategorieMapping> GetWMS_PCMCategoryMapping();

        //[GEOS2-4874][25.12.2023][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddDeleteArticleCategoryMapping(List<ArticleCategorieMapping> ArticleCategoryMappingList);

        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Discounts AddDiscount_V2470(Discounts Discount);


        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DiscountLogEntry> GetDiscountLogEntriesByDiscountstring_V2470(int IdCustomerDiscount);

        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DiscountLogEntry> GetDiscountCommentsByDiscountstring_V2470(int IdCustomerDiscount);

        //[Sudhir.jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Discounts UpdateDiscount_V2470(Discounts Discount, Discounts PrevDiscount);

        //[rdixit][27.12.2023][GEOS2-4875][GEOS2-48756]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCategorieMapping> GetWMS_PCMCategoryMappingOfToday();

        //[rdixit][27.12.2023][GEOS2-4875][GEOS2-48756]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> AddWMSTOPCMArticlesByCategories(List<ArticleCategorieMapping> ArticleCategoryMappingList);

        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use AddDetectionForAddDetectionViewModel_V2560 instead.")]
        DetectionDetails AddDetection_V2470(DetectionDetails detectionDetails);

        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionDetails GetDetectionByIdDetection_V2470(UInt32 IdDetection, int IdUser);


        //[Sudhir.Jangra][GEOS2-4935]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateDetection_V2470(DetectionDetails detectionDetails);

        //[cpatil][GEOS2-5299][26-02-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetEmdepSitesCompanies_V2500 instead.")]
        IList<Company> GetEmdepSitesCompanies_V2490();

        //Shubham[skadam] GEOS2-5307 Modules Window 23 02 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetProductTypeByIdCpType_V2530 instead.")]
        ProductTypes GetProductTypeByIdCpType_V2490(UInt64 IdCpType, UInt64 IdTemplate, int IdUser);

        //[Sudhir.jangra][GEOS2-4935]
        //Shubham[skadam] GEOS2-5307 Modules Window 23 02 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use UpdateProductType_V2530 instead.")]
        bool UpdateProductType_V2490(ProductTypes productTypes);

        //[Sudhir.jangra][GEOS2-4935]
        //Shubham[skadam] GEOS2-5307 Modules Window 23 02 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use AddProductType_V2530 instead.")]
        ProductTypes AddProductType_V2490(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PLMModulePrice> GetNotIncludedPLMModulePrices_V2490(string IdBasePriceListCommaSeparated, string IdCustomerPriceListCommaSeparated, UInt64 IdCPType, int IdUser);
        //rajashri GEOS2-5464
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedDetection_V2500(UInt32 IdDetection, string detectionName);

        //[rdixit][27.03.2024][GEOS2-5556]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Company> GetEmdepSitesCompanies_V2500();

        //[rushikesh.gaikwad][GEOS2-5583][19.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use GetProductTypeByIdCpType_V2590 instead.")]
        ProductTypes GetProductTypeByIdCpType_V2530(UInt64 IdCpType, UInt64 IdTemplate, int IdUser);


        //[rushikesh.gaikwad][GEOS2-5583][19.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use AddProductType_V2590 instead.")]
        ProductTypes AddProductType_V2530(ProductTypes productTypes);

        //[rushikesh.gaikwad][GEOS2-5583][19.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use UpdateProductType_V2590 instead.")]
        bool UpdateProductType_V2530(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CPLCustomer> GetCustomersWithRegions_V2530(UInt64 IdCpType);

        //[rdixit][15.07.2024][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCostPrice> GetArticleCostPricesByCurrency_V2540(UInt32 idCurrency);
        //[rdixit][15.07.2024][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetAllPCMArticles_V2540();
        //[rdixit][15.07.2024][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CurrencyConversion> GetCurrencyConversionsDetailsByLatestDate();
        //[rdixit][15.07.2024][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BasePriceListByItem> GetSalesPriceForPCMArticleByBPL();
        //[Rahul.Gadhave][GEOS2-5898][Date:21-08-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionChangeDetails> GetDailyUpdateChanges_V2550(DateTime date);
        //[Rahul.Gadhave][GEOS2-5898][Date:21-08-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionChangeDetails> GetDailyAddedChanges_V2550(DateTime date);
        //[Rahul.Gadhave][GEOS2-5896][Date:29/08/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionDetails AddDetection_V2560(DetectionDetails detectionDetails);
        //[Rahul.Gadhave][GEOS2-5896][Date:05-09-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionDetails AddDetectionForAddDetectionViewModel_V2560(DetectionDetails detectionDetails);
        //[RGadhave][GEOS2-5896][25.09.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCM_DetectionECOSVisibility_Update_V2560(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BasePriceListByItem> GetSalesPriceForPCMArticleByBPL_V2590();

        //[rdixit][GEOS2-6522][29.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tuple<ulong, string>> GetArticleCostPricesPlantByCurrency_V2590();

        //[rdixit][GEOS2-6522][29.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tuple<ulong, string, uint>> GetSalesPriceNameList_V2590();

        //[rdixit][GEOS2-6522][29.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CurrencyConversion> GetCurrencyConversionsByLatestDate_V2590(string idCurrencyFrom, uint idCurrencyTo);

        //[rdixit][GEOS2-6522][29.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCostPrice> GetArticleCostPricesByCompany_V2590(string alias, uint idCountry);

        //[rdixit][GEOS2-6522][29.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BasePriceListByItem> GetSalesPriceforArticleByIdBasePrice_V2590(ulong idBasePrice);

        //[rdixit][GEOS2-6624][10.12.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version 2640. Use GetProductTypeByIdCpType_V2640 instead.")]
        ProductTypes GetProductTypeByIdCpType_V2590(UInt64 IdCpType, UInt64 IdTemplate, int IdUser);

        //[rdixit][GEOS2-6624][10.12.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProductType_V2590(ProductTypes productTypes);

        //[rdixit][GEOS2-6624][10.12.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ConnectorFamilies> GetAllFamiliesWithSubFamily_V2590();


        //[rdixit][GEOS2-6624][10.12.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes AddProductType_V2590(ProductTypes productTypes);

        //[rdixit][31.12.2024][GEOS2-6574][GEOS2-6575]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetAllProductTypes_V2590();

        //[rdixit][31.12.2024][GEOS2-6575]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DetectionDetails> GetAllModuleDetectionsWaysOptionsSpareParts_V2590();

        //[nsatpute][19-05-2025][GEOS2-6691]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes GetProductTypeByIdCpType_V2640(UInt64 IdCpType, UInt64 IdTemplate, int IdUser);

        //[pooja.jadhav][19-05-2025][GEOS2-6691]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetAllProductTypes_V2640();
        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Articles GetArticleByIdArticle_V2660(UInt32 IdArticle, UInt32 IdPCMArticle, int IdUser);
        //[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Articles GetArticleByIdArticle_V2660_temp(UInt32 IdArticle, int IdUser);

        //[pramod.misal][GEOS2-8321][Date:27-06-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Articles GetPLMArticleByIdArticle_V2660(UInt32 IdArticle, int IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticle_V2660(uint IdPCMArticleCategory, Articles Article, bool IsDetailsChecked, bool IsPricesChecked);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleImage> GetLinkedArticleImage_V2660(uint IdArticle);//GetArticleImage_V2370

    }
}
