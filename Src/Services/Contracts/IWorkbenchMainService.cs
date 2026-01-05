using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
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
    [ServiceContract]
    public interface IWorkbenchMainService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionDetails AddDetection_V2550(DetectionDetails detectionDetails);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateDetection_V2550(DetectionDetails detectionDetails);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedProductType_V2550(UInt64 IdCPType);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatedPCMArticleCategoryOrder_V2550(List<PCMArticleCategory> pcmArticleCategoryList, uint? IdModifier);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletePCMArticleCategory_V2550(List<PCMArticleCategory> pcmArticleCategoryList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2550(uint IdPCMArticleCategory, Articles Article);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddDeletePCMArticle_V2550(uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateDetectionForAddDetectionViewModel_V2550(DetectionDetails detectionDetails);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ImportArticleCostPriceCalculate_V2550(Data.Common.Company company, UInt64 itemArticle, List<PODetail> EWHQArticlesByArticleComponentpoDetailLst, List<ArticlesByArticle> LstAllArticlesByArticle, List<PODetail> ArticlesByArticleComponentpoDetailLst);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticleCategory_V2550(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PCMArticleCategory AddPCMArticleCategory_V2550(PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCM_MicroSigaInformation_V2550(List<MicroSigainformation> microSigainformation);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateDetection_ForAddDetectionViewModeln_V2550(DetectionDetails detectionDetails);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCM_DetectionECOSVisibility_Update_V2550(DetectionDetails detectionDetails);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatePCMArticle_V2550(uint IdPCMArticleCategory, Articles Article);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionDetails AddDetectionForEditDetectionViewModel_V2550(DetectionDetails detectionDetails);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        FreePlugins AddUpdateFreePlugins_V2550(FreePlugins freePlugins);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdateFreePluginsForAddPluginsViewModel_V2550(List<FreePlugins> freePlugins);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddHardLockLicense_V2550(UInt32 IdArticle, List<HardLockPlugins> pluginList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteSupportedPluginForHardLockLicense_V2550(UInt32 idPlugin, UInt32 idArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddHardLockPlugin_V2550(UInt32 idPlugin, string Name);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddDeleteArticleCategoryMapping_V2550(List<ArticleCategorieMapping> ArticleCategoryMappingList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Discounts AddDiscount_V2550(Discounts Discount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Discounts UpdateDiscount_V2550(Discounts Discount, Discounts PrevDiscount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Articles> AddWMSTOPCMArticlesByCategories_V2550(List<ArticleCategorieMapping> ArticleCategoryMappingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DetectionDetails AddDetectionForNewAddDetectionViewModel_V2550(DetectionDetails detectionDetails);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateDetectionForEditDetectionViewModelNew_V2550(DetectionDetails detectionDetails);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedDetection_V2550(UInt32 IdDetection, string detectionName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes AddProductType_V2550(ProductTypes productTypes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> AddEmployeeImportAttendance_V2550(List<EmployeeAttendance> employeeAttendanceList);
        
    }
}
