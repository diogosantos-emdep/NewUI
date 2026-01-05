using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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
        List<RegionsByCustomer> GetCustomersWithRegions();

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
        ProductTypes AddProductType(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProductType(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionDetails AddDetection(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateDetection(DetectionDetails detectionDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes GetProductTypeByIdCpType(UInt64 IdCpType, UInt64 IdTemplate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
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
        List<Detections> GetAllDetectionsWithGroups();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
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
        PCMArticleCategory AddPCMArticleCategory(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticleCategory(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PCMArticleCategory GetPCMArticleCategoryById(uint idPCMArticleCategory);


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
        bool IsDeletedDetection(UInt32 IdDetection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
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
        bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2060(uint IdPCMArticleCategory, Articles Article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticleCategories_V2060();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> GetAllPCMArticles_V2060();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Articles GetArticleByIdArticle_V2060(UInt32 IdArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategory_V2060();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2060();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddDeletePCMArticle(uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser);
    }
}
