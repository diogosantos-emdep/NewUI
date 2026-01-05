using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IERMService" in both code and config file together.
    [ServiceContract]
    public interface IERMService
    {
        //[GEOS2-3235]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedWorkOperationList(UInt32 idWorkOperation);
        [OperationContract]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetWorkOperationByIdWorkOperation_V2240 instead.")]
        [FaultContract(typeof(ServiceException))]
        WorkOperation GetWorkOperationByIdWorkOperation(Int32 idWorkOperation);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Stages> GetAllStages();
        [OperationContract]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetparentAndOrder_V2240 instead.")]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperation> GetparentAndOrder();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use UpdateWorkOperation_V2240 instead.")]
        bool UpdateWorkOperation(WorkOperation workOperation, List<WorkOperation> workOperationList);
        //[GEOS2-3242]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardTime> GetStandardTimeList();
        //[GEOS2-3242]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedStandardTimeList(UInt64 idStandardTime, uint IdModifier);
        //[GEOS2-3240]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestWorkOperationCode();
        //[GEOS2-3240]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use AddWorkOperation_V2240 instead.")]
        WorkOperation AddWorkOperation(WorkOperation workOperation, List<WorkOperation> workOperationList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetAllWorkOperations_V2200 instead.")]
        List<WorkOperation> GetAllWorkOperations();
        //[GEOS2-3241]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetParentListByIdParentAndCode_V2240 instead.")]
        List<WorkOperation> GetParentListByIdParentAndCode(Int32 IdParent, string Code);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetLatestWorkOperationCodeByCode_V2350 instead.")]
        string GetLatestWorkOperationCodeByCode(string Code);
        //GEOS2-3535
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetAllWorkOperationStages_V2240 instead.")]
        List<WorkOperationByStages> GetAllWorkOperationStages(int IdStage);
        //GEOS2-3535
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Stages> GetStages();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use GetWorkOperationByIdWorkOperation_V2280 instead.")]
        WorkOperation GetWorkOperationByIdWorkOperation_V2240(Int32 idWorkOperation);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use UpdateWorkOperation_V2280 instead.")]
        bool UpdateWorkOperation_V2240(WorkOperation workOperation, List<WorkOperation> workOperationList);
        //[GEOS2-3240]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use AddWorkOperation_V2280 instead.")]
        WorkOperation AddWorkOperation_V2240(WorkOperation workOperation, List<WorkOperation> workOperationList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetAllWorkOperations_V2240 instead.")]
        List<WorkOperation> GetAllWorkOperations_V2200();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use GetAllWorkOperations_V2280 instead.")]
        List<WorkOperation> GetAllWorkOperations_V2240();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperation> GetparentAndOrder_V2240();
        //[GEOS2-3241]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperation> GetParentListByIdParentAndCode_V2240(Int32 IdParent, string Code);
        //GEOS2-3535
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2270. Use GetAllWorkOperationStages_V2270 instead.")]
        List<WorkOperationByStages> GetAllWorkOperationStages_V2240(int IdStage);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionary> GetStandardOperationsDictionaryList_V2260();
        //[GEOS2-3242]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteOperationFromStandardOperationsDictionary_V2260(UInt64 idStandardOperationsDictionary, uint IdModifier);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetLatestSODCode_V2350 instead.")]
        string GetLatestSODCode_V2260();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2270. Use AddStandardOperationsDictionary_V2270 instead.")]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2260(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2270. Use UpdateStandardOperationsDictionary_V2270 instead.")]
        bool UpdateStandardOperationsDictionary_V2260(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetSODSupplementsCategoryName();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use GetStandardOperationsDictionaryDetail_V2280 instead.")]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail(UInt64 idStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use AddStandardOperationsDictionary_V2280 instead.")]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2270(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use UpdateStandardOperationsDictionary_V2280 instead.")]
        bool UpdateStandardOperationsDictionary_V2270(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Template> GetAllTemplates();
        //GEOS2-3660
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperationByStages> GetAllWorkOperationStages_V2270(int IdStage);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryModules> GetAllStandardOperationsDictionaryModulesById_V2280(UInt64 idStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedstandard_operations_dictionary_modulesList(UInt32 idWorkOperation, UInt32 IdStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkOperation_V2280(WorkOperation workOperation, List<WorkOperation> workOperationList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkOperation_V2320(WorkOperation workOperation, List<WorkOperation> workOperationList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WorkOperation AddWorkOperation_V2280(WorkOperation workOperation, List<WorkOperation> workOperationList);
        //[Rupali Sarode][19/09/2022][GEOS2-3933]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WorkOperation AddWorkOperation_V2320(WorkOperation workOperation, List<WorkOperation> workOperationList);
        //[001][kshinde][08/06/2022][GEOS2-3711]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperation> GetAllWorkOperations_V2280();
        //[GEOS2-3933][Rupali Sarode][20/09/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperation> GetAllWorkOperations_V2320();
        //[GEOS2-3954][Gulab Lakade][01/11/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperation> GetAllWorkOperations_V2330();
        //[001][kshinde][08/06/2022][GEOS2-3711]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WorkOperation GetWorkOperationByIdWorkOperation_V2280(Int32 idWorkOperation);
        //[Rupali Sarode][19/09/2022][GEOS2-3933]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WorkOperation GetWorkOperationByIdWorkOperation_V2320(Int32 idWorkOperation);
        //[001][rdixit][GEOS2-3710][15/06/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkOperationMultipleRecords_V2280(WorkOperation workOperation, uint IdModifier);
        //[GEOS2-3933][Rupali Sarode][20/09/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkOperationMultipleRecords_V2320(WorkOperation workOperation, uint IdModifier);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMDetectionsGroups> GetAllDetectionsGroups_V2280();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMDetections> GetAllDetections_V2280();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use GetStandardOperationsDictionaryDetail_V2290 instead.")]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2280(UInt64 idStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2290(UInt64 idStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use UpdateStandardOperationsDictionary_V2280 instead.")]
        bool UpdateStandardOperationsDictionary_V2280(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use AddStandardOperationsDictionary_V2280 instead.")]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2280(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use UpdateStandardOperationsDictionary_V2300 instead.")]
        bool UpdateStandardOperationsDictionary_V2290(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use AddStandardOperationsDictionary_V2300 instead.")]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2290(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetAllProductTypes_V2300 instead.")]
        List<ProductTypes> GetAllProductTypes_V2270();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetAllProductTypes_V2300();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use ERM_IsWOExistInSOD_V2350 instead.")]
        bool ERM_IsWOExistInSOD(UInt32 idWorkOperation);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperationByStages> GetAllWorkOperationStages_V2300(int IdStage);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperationByStages> GetAllWorkOperationStages_V2320(int IdStage);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetAllWorkOperationStages_V2300 instead.")]
        List<WorkOperationByStages> GetAllWorkOperationStages_V2280(int IdStage);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMOptionsGroups> GetAllOptionGroups_V2300();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMOptions> GetAllOptions_V2300();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2300(UInt64 idStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateStandardOperationsDictionary_V2300(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2300(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2301(UInt64 idStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2320(UInt64 idStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateStandardOperationsDictionary_V2301(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateStandardOperationsDictionary_V2320(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2301(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2320(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMWaysGroups> GetAllWaysGroups_V2301();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMStructures> GetAllStructures_V2320();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMWays> GetAllWays_V2301();
        //GEOS2-3974
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetAllTimeTracking_V2330();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetAllTimeTracking_V2340(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetAllTimeTracking_V2350(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTrackingProductionStage> GetAllTimeTrackingProductioStage_V2320();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTrackingProductionStage> GetAllTimeTrackingProductioStage_V2340();
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<WorkOperation> GetWorkOperationsByCode_V2300(string Code);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMSparePartsGroups> GetAllSparePartGroups_V2340();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMSpareParts> GetAllSparePart_V2340();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2340(UInt64 idStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2340(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateStandardOperationsDictionary_V2340(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2340(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2350(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-3840] [sdeshpande] [03-01-2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkStages> GetAllWorkStages_V2350();
        //[GEOS2-3841] [Gulab Lakade] [06-01-2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WorkStages GetWorkStagesByIdStages_V2350(UInt64 IdStage);
        //[GEOS2-3841] [Gulab Lakade] [09-01-2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WorkStages AddWorkStage_V2350(WorkStages workStages);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkStage_V2350(WorkStages workStages);
        //[rdixit][10.01.2022][GEOS2-4121]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestWorkOperationCodeByCode_V2350(string Code);
        //[rdixit][10.01.2022][GEOS2-4121]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestSODCode_V2350();
        //[rdixit][10.01.2022][GEOS2-4121]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ERM_IsWOExistInSOD_V2350(UInt32 idWorkOperation);
        //[GEOS2-3841] [Rupali Sarode] [10-01-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkStages> GetAllWorkStageSequence_V2350();
        //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTracking_V2360(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2360(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-4153] [Gulab Lakade] [02 02 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReview_V2360(string OriginPlant, DateTime fromDate, DateTime toDate);
        //[GEOS2-4159] [pallavi jadhav] [02 14 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Holidays> GetCompanyHolidaysBySelectedIdCompany(string OriginPlant, string year);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdatePlanningDateReview(List<PlanningDateReview> PlanningDateReviewList);
        //[GEOS2-4217] [Pallavi Jadhav] [24 02 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2360(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-4145][Pallavi Jadhav][03-03-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2370(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-4145][Pallavi Jadhav][03-03-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2370(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList);
        // [GEOS2-4212][gulab lakade][07 03 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetPlantModuleProductionDelay_V2370(List<Company> AllPlant, Company company, DateTime Date);
        //[GEOS2-4163] [Pallavi Jadhav] [07 03 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReview_V2370(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdatePlanningDateReview_V2370(ProductionPlanningReview PlanningDateReviewList);
        //[GEOS2-4212][gulab lakade][07 03 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string ReadMailTemplate(string templateName);
        //[GEOS2-4241][Rupali Sarode][16 03 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReportMail> GetWeeklyProductionReportMailIDs_V2370(UInt32 IdCompany);
        //[GEOS2-4244] [Gulab lakade] [21 03 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2370(Company company, DateTime StartDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAuthorizedPlantsByIdUser_V2031(Int32 idUser);
        //[GOES2-4242][Rupali Sarode][03-04-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ModulesEquivalencyWeight> GetAllProductTypesEquivalencyWeight_V2380();
        // [GEOS2-4212][gulab lakade][07 03 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantProductionDelay> GetEquipmentproductionDelay_V2370(Company company, DateTime Date);
        //[Pallavi Jadhav] [GEOS2-4329] [06-04-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ModulesEquivalencyWeight> GetAllStructuresEquivalencyWeight_V2380();
        //[GOES2-4330][Rupali Sarode][06-04-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ModulesEquivalencyWeight GetProductTypesEquivalencyWeightByCPType_V2380(Int32 IdCpType);
        //[GEOS2-4330][Rupali Sarode][12-04-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SaveEquivalencyWeight_V2380(ModulesEquivalencyWeight modulesEquivalencyWeight);
        //[GEOS2-4330][Rupali Sarode][14-04-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ModulesEquivalencyWeight GetStructuresEquivalencyWeightByCPType_V2380(Int32 IdCpType);
        //[Origin and Production][gulab lakade][17 04 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2380(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        //[Origin and Production][gulab lakade][17 04 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2380(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList);
        // [gulab lakade][17 04 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetPlantModuleProductionDelay_V2380(List<Company> AllPlant, Company company, DateTime Date);
        //[GEOS2-4241][gulab lakade][18 04 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReportMail> GetWeeklyProductionReportMailIDsByAppSettingTemp_V2380(UInt32 IdCompany);
        //[GEOS2-4336][gulab lakade][18 04 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantOperationProductionStage> GetAllPlantOperationProductioStage_V2380(string IdStage);
        //[GEOS2-4336][gulab lakade][21 04 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2380(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetAllProductTypes_V2390();
        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMDetections> GetAllDetections_V2390();
        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMOptions> GetAllOptions_V2390();
        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMWays> GetAllWays_V2390();
        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMSpareParts> GetAllSparePart_V2390();
        //[GEOS2-4355][Rupali Sarode][3-05-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMStructures> GetAllStructures_V2390();
        //[missmatch QTY for time tracking and planning Date review] [gulab lakade] [03 025 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReview_V2380(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-4425] [Pallavi Jadhav] [04 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2390(Company company, DateTime StartDate);
        //[Rupali Sarode][GEOS2-4347][05-05-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTrackingProductionStage> GetAllTimeTrackingProductioStage_V2390();
        //[GEOS2-4338][gulab lakade][15 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2390(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        //[GEOS2-4341][Pallavi Jadhav][05 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMDeliveryVisualManagement> GetDVManagementProduction_V2390(string IdSite, string CurrencyName, DateTime fromDate, DateTime toDate);
        //[GEOS2-4475][Gulab Lakade][17-05-2023][ERM - Holiday]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WindowsServicesHolidays> GetAllWindowsServicesHolidays_V2390(Company company, DateTime StartDate);
        // [gulab lakade][24 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetPlantModuleProductionDelay_V2390(List<Company> AllPlant, Company company, DateTime Date);
        // [GEOS2-4491][Pallavi Jadhav][24 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantProductionDelay> GetEquipmentproductionDelay_V2390(Company company, DateTime Date);
        //[gulab lakade][GEOS2-4494-batch][26 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2400(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        //[gulab lakade][GEOS2-4494-batch][26 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2400(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList);
        //[gulab lakade][GEOS2-4494-batch][26 05 2023]
        //[GEOS2-4481][Pallavi Jadhav][25 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReview_V2400(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-4343][Pallavi Jadhav][25 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DeliveryVisualManagementStages> GetDVManagementProductionStage_V2400();
        //[GEOS2-4481][Pallavi Jadhav][29 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlanningDateReviewStages> GetProductionPlanningReviewStage_V2400();
        //[GEOS2-4483][gulab lakade][31 05 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2400(Company company, DateTime StartDate);
        ////[GEOS2-4517][Rupali Sarode][05-06-2023]
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<ERMFollowingTwelveWeeksPlan> GetFollowingTwelveWeeksPlan_V2400(string IdSites);
        //[GEOS2-4553][Rupali Sarode][09-06-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2400(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        //[GEOS2-4553][Rupali Sarode][09-06-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMNonOTItemType> GetAllNonOTTimeType_V2400();
        //[][gulab lakade][14 06 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReportMail> GetPlantProductionDelayReportMailIDsByAppSettingTemp_V2400(UInt32 IdCompany);
        //[GEOS2-4590][rupali sarode][22-06-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyReworksReportSummary> GetPlantWeeklyReworksSummary_V2410(Company company, DateTime StartDate);
        //[GEOS2-4590][rupali sarode][26-06-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyReworksReportMail> GetWeeklyReworksReportMailIDs_V2410(UInt32 IdCompany);
        //[GEOS2-4590][rupali sarode][26-06-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyReworksReportMail> GetPlantReworksReportMailIDsByAppSetting_V2410(UInt32 IdCompany);
        //[gulab lakade][GEOS2-4605][26 06 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2410(Company company, DateTime StartDate);
        //[GEOS2-4619][rupali sarode][28-06-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Site> GetPlants_V2410(int IdUser);
        //[GEOS2-4548][pallavi][23-06-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReportMail> GetWeeklyProductionReportMailIDs_V2410(UInt32 IdCompany, string TO_CC_JobDescription, string TO_JobDescription, string CC_JobDescription);
        //[GEOS2-4548][pallavi][23-06-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReportMail> GetWeeklyProductionReportMailIDsByAppSettingTemp_V2410(UInt32 IdCompany, string CC_EmployeeId);
        //[GEOS2-4606][gulab lakade][30 06 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMDeliveryVisualManagement> GetDVManagementProduction_V2410(string IdSite, DateTime fromDate, DateTime toDate);
        //[GEOS2-4481][Pallavi Jadhav][04 07 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReview_V2410(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-4624][rupali sarode][03-07-2023]
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //string GetDVMFolderPath();
        //[GEOS2-4626][pallavi Jadhav][03 07 2023]
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<TimeTracking> GetAllTimeTrackingRepor_V2410(string CurrencyName, UInt32 IdSite, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        //[GEOS2-4626][pallavi Jadhav][03 07 2023]
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //string SaveReportOfTimeTracking(string TimeTrackingReport);
        //[GEOS2-4617][gulab lakade][04-07-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2410(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        //[GEOS2-4617][gulab lakade][04-07-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMNonOTItemType> GetAllNonOTTimeType_V2410();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetDeliveryDateForTimeTrackingReport_V2410(Int32 IdSite);
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<PlantWeeklyReworksMail> GetPlantWeeklyReworksMail_V2410();
        //[GEOS2-4626][pallavi jadhav][03 07 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingRepor_V2410(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2410(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        //[GEOS2-4639][pallavi jadhav][25-07-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMNonOTItemType> GetAllNonOTTimeType_V2420();
        //[GEOS2-4707][rupali sarode][25-07-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2420(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        //[rupali sarode][28/07/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReview_V2420(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        //[pallavi jadhav][16 08 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2420(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        //[pallavi jadhav][16 08 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2420(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList);
        //[gulab lakade][GEOS2-4767][22 08 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2420(Company company, DateTime StartDate);
        //[GEOS2-4730][rupali sarode][08-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetAllRTM_HRResources_V2420(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        //[GEOS2-4730][rupali sarode][18-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<RTMHRResourcesExpectedTime> GetRTM_HRResourcesExpectedTime_V2420(DateTime StartDate, DateTime EndDate);
        //[Pallavi Jadhav][GEOS2-4591][22 06 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantWeeklyReworksMailStage> GetPlantWeeklyReworksMailStage_V2430(Company company);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantWeeklyReworksMail> GetPlantWeeklyReworksMail_V2430(Company company, DateTime StartDate);
        //[GEOS2-4626][pallavi jadhav][31 08 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingRepor_V2430(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2430(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        //[GEOS2-4752][gulab lakade][01 09 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionEmployeeTime> GetWeeklyProductionEmployeeTime_V2430(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetPlantModuleProductionDelay_V2430(List<Company> AllPlant, Company company, DateTime Date);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetPlantModuleProductionDelayByPlant_V2430(List<Company> AllPlant, UInt32 IdSite, DateTime Date, UInt32 OriginalPlantIdSite);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetALLPlantWeeklyReworksMail_V2430(Company company, DateTime Date);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetPlantWeeklyReworksMailByPlant_V2430(UInt32 IdSite, DateTime Date, List<Company> allPlants);
        //[GEOS2-4799][rupali sarode][05-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<RTMFutureLoad> GetRTMFutureLoadDetails_V2430(RTMFutureLoadParams FutureLoadParams);
        //[GEOS2-4813][Aishwarya Ingale][11-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetPlantOperationPlanning_V2500 instead.")]
        List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2430(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyReworksReportSummary> GetPlantWeeklyReworksSummary_V2430(Company company, DateTime StartDate);
        //[Pallavi Jadhav][14-09-2023][GEOS2-4818]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2430(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        //[Pallavi Jadhav][14-09-2023][GEOS2-4818]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2430(UInt32 PlantID, string OriginPlant, string Currency, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-4821][Rupali Sarode][14-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DeliveryVisualManagementStages> GetDVManagementProductionStage_V2440();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMDeliveryVisualManagement> GetDVManagementProduction_V2440(string IdSite, DateTime fromDate, DateTime toDate);
        //[GEOS2-4820][Pallavi Jadhav][13-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllReworkReport_V2440(DateTime fromDate, DateTime toDate, UInt32 IdSite);
        //[pallavi jadhav][GEOS2-4869][9 27 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DeliveryVisualManagementStages> GetDVManagementRTMHRResourcesStage_V2440(Int32 IdSite);
        //[GEOS2-4800][Pallavi Jadhav][02-10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<RTMFutureLoad> GetRTMFutureLoadDetails_V2440(RTMFutureLoadParams FutureLoadParams);
        //[GEOS2-4862][Rupali Sarode][04-10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<RTMHRResourcesExpectedTime> GetRTM_HRResourcesExpectedTime_V2440(DateTime StartDate, DateTime EndDate);
        //[GEOS2-4908][pallavi Jadhav][25-10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantDeliveryAnalysis> GetPlantDeliveryAnalysis_V2450(string CurrencyNameFromSetting, string CurrencySymbolFromSetting, string IdSite, DateTime fromDate, DateTime toDate);
        //[GEOS2-4996][gulab lakade][27 10 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionEmployeeTime> GetWeeklyProductionEmployeeTime_V2450(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription);
        //[GEOS2-5001][rupali sarode][28-10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetAllRTM_HRResources_V2500 instead.")]
        List<ERMPlantOperationalPlanning> GetAllRTM_HRResources_V2450(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        //[GEOS2-4909][Aishwarya Ingale][27-10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMProductionTime> GetRTMTestBoardsInProduction_V2450(uint IdSite, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        //[gulab lakade][GEOS2-4921][30 10 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetProductionOutputReworksMail_V2450(Company company, DateTime StartDate);
        //[GEOS2-5002] [gulab lakade][31 10 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetALLPlantWeeklyReworksMail_V2450(Company company, DateTime Date);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2450(Int32 IdCompany, DateTime StartDate, DateTime EndDate);
        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetAllTimeTrackings_V2500 instead.")]
        TimeTrackingWithSites GetAllTimeTrackings_V2460(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetTimeTrackingBYPlant_V2500 instead.")]
        List<TimeTracking> GetTimeTrackingBYPlant_V2460(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList);
        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetAllTimeTrackingReport_V2500 instead.")]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2460(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetTimeTrackingReportBYPlant_V2500 instead.")]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2460(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetAllReworkReport_V2500 instead.")]
        TimeTrackingWithSites GetAllReworkReport_V2460(DateTime fromDate, DateTime toDate, UInt32 IdSite);
        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetALLPlantWeeklyReworksMail_V2460(Company company, DateTime Date);
        // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantDeliveryAnalysis> GetPlantDeliveryAnalysis_V2460(string CurrencyNameFromSetting, string CurrencySymbolFromSetting, string IdSite, DateTime fromDate, DateTime toDate);
        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetAllProductTypes_V2460();
        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMDetectionsGroups> GetAllDetectionsGroups_V2460();
        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMDetections> GetAllDetections_V2460();
        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMOptionsGroups> GetAllOptionGroups_V2460();
        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMOptions> GetAllOptions_V2460();
        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMWays> GetAllWays_V2460();
        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMSparePartsGroups> GetAllSparePartGroups_V2460();
        //[GEOS2-5006][Rupali Sarode][27-11-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMSpareParts> GetAllSparePart_V2460();
        //[GEOS2-5097][gulab lakade][12 04 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReview_V2460(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        //[gulab lakade][miss match unit prize][05 12 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2460(Company company, DateTime StartDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2460(Int32 IdCompany, DateTime StartDate, DateTime EndDate);
        //[GEOS2-5135][Rupali Sarode][19-12-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2460(UInt64 idStandardOperationsDictionary);
        ////[GEOS2-5127][gulab lakade][20 12 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2470(Company company, DateTime Date);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetProductionOutputReworksMail_V2470(Company company, DateTime StartDate);
        // [Pallavi Jadhav][19-12-2023][GEOS2-5035]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantLoadAnalysis> GetAllPlantLoadAnalysis_V2470(Int32 idSite, DateTime FromDate, DateTime ToDate);
        //[pallavi jadhav] [GEOS2-5197] [02 01 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2470(Int32 IdCompany, DateTime StartDate, DateTime EndDate);
        //[wrong week][gulab lakade][096 01 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetAllWeeklyproductionReportByPlant_V2500 instead.")]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2470(Company company, DateTime StartDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionEmployeeTime> GetWeeklyProductionEmployeeTime_V2470(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WindowsServicesHolidays> GetAllWindowsServicesHolidays_V2470(Company company, DateTime StartDate);
        //[pallavi jadhav] [GEOS2-5197] [02 01 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionTimeReportLegend> GetAllProductionTimeReportLegend_V2480();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2480(Int32 IdCompany, DateTime StartDate, DateTime EndDate);
        // [GEOS2-5037][Rupali Sarode][31-01-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantLoadAnalysis> GetAllPlantLoadAnalysis_V2480(Int32 idSite, DateTime FromDate, DateTime ToDate);
        //[Geos2-5270] [Aishwarya Ingale][5/02/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DeliveryTimeDistribution> GetDeliveryTimeDistributionList_V2480();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteOperationFromDeliveryTimeDistribution_V2480(UInt64 iddeliverytimedistribution, uint IdModifier);
        //// start[GEOS2-5324][gulab lakade][09 02 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetALLPlantWeeklyReworksMail_V2500 instead.")]
        ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2480(Company company, DateTime Date);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetProductionOutputReworksMail_V2500 instead.")]
        ERM_ReworkReport GetProductionOutputReworksMail_V2480(Company company, DateTime StartDate);
        // end [GEOS2-5324][gulab lakade][09 02 2023]
        //[Rupali Sarode][07-02-2024][GEOS2-5353]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Template> GetAllTemplates_DTD_V2490();
        //[Rupali Sarode][07-02-2024][GEOS2-5353]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetAllProductTypes_DTD_V2490(string IdCptypes);
        //[Rupali Sarode][13-02-2024][GEOS2-5271]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestDTDCode_V2490();
        //[Rupali Sarode][14-02-2024][GEOS2-5271]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetDeliveryTimeDistributionDetail_V2530 instead.")]
        DeliveryTimeDistribution GetDeliveryTimeDistributionDetail_V2490(UInt64 idDeliveryTimeDistribution);
        //[Geos2-5356] [Aishwarya Ingale][21/02/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use AddDeliveryTimeDistribution_V2530 instead.")]
        DeliveryTimeDistribution AddDeliveryTimeDistribution_V2490(DeliveryTimeDistribution DTD);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use UpdateDeliveryTimeDistribution_V2530 instead.")]
        bool UpdateDeliveryTimeDistribution_V2490(DeliveryTimeDistribution DTD);
        //[GEOS2-5418] [gulab lakade] [23 02 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionTimeReportLegend> GetAllProductionTimeReportLegend_V2490();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetProductionTimeline_V2500 instead.")]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2490(Int32 IdCompany, DateTime StartDate, DateTime EndDate);
        //[gulab lakade][11 03 2024][GEOS2-5466]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2500(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        //[gulab lakade][11 03 2024][GEOS2-5466]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2500(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList);
        //// start[GEOS2-5324][gulab lakade][14 03 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2500(Company company, DateTime Date);
        //[Aishwarya Ingale][18 03 2024][GEOS2-5424]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2500(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        // [gulab lakade][11 03 2024][GEOS2-5466]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2500(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        // [gulab lakade][11 03 2024][GEOS2-5466]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2500(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        //[GEOS2-5420][Rupali Sarode][15-03-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetProductionOutputReworksMail_V2500(Company company, DateTime StartDate);
        //[GEOS2-5420][Rupali Sarode][18-03-2024]
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //ReworksMailStageByOTItemAndIDDrawing GetPlantWeeklyReworksMailStage_V2500(Company company);
        // [Rupali Sarode][GEOS2-5522][21-03-2024]
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //TimeTrackingStageByOTItemAndIDDrawing GetAllTimeTrackingProductionStage_V2500();
        // [Rupali Sarode][GEOS2-5523][26-03-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllReworkReport_V2500(DateTime fromDate, DateTime toDate, UInt32 IdSite);
        // [Rupali Sarode][GEOS2-5521][27-03-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2500(Company company, DateTime StartDate);
        // [Rupali Sarode][GEOS2-5521][28-03-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StageByOTItemAndIDDrawing GetAllStagesPerIDOTItemAndIDDrawing_V2500();
        //[GEOS2-5546][pallavi jadhav][29 03 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetAllRTM_HRResources_V2500(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2500(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        //[GEOS2-5558][gulab lakade]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2510(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        //[GEOS2-5520][Aishwarya Ingale]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2510(Company company, DateTime StartDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMothlyProductionTimeLine_V2510(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2510( string PlantName,  DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetAllTimeTrackingProductionTimelineByPlant_V2510(UInt32 PlantID, UInt32 ProductionIdSite, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantLoadAnalysis> GetAllPlantLoadAnalysis_V2520(Int32 idSite, DateTime FromDate, DateTime ToDate);
        //[GEOS2-5750][gulab lakade][21052024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_WorkOrder_Other_ProductionTimeline GetWorkorder_Other_MonthlyProduction_V2520();
        //[GEOS2-5742][pallavi jadhav]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2520(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2530(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        //[GEOS2-5849][gulab lakade][13 06 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantORG_EmployeeDetails> GetWeeklyProd_TimeLine_Plant_ORG_V2530(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        #region Aishwarya [Geos2-5629][18/06/2024]
        //Aishwarya Ingale[Geos2-5629][18/06/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2530(UInt64 idStandardOperationsDictionary);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateStandardOperationsDictionary_V2530(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2530(StandardOperationsDictionary sod);
        //[rgadhave][GEOS2-5583][20-06-2024] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DeliveryTimeDistribution GetDeliveryTimeDistributionDetail_V2530(UInt64 idDeliveryTimeDistribution);
        //[rgadhave][GEOS2-5583][20-06-2024] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DeliveryTimeDistribution AddDeliveryTimeDistribution_V2530(DeliveryTimeDistribution DTD);
        //[rgadhave][GEOS2-5583][20-06-2024] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateDeliveryTimeDistribution_V2530(DeliveryTimeDistribution DTD);
        #endregion
        //[GEOS2-5856][gulab lakade][20 06 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionEmployeeTime> GetWeeklyProductionEmployeeTime_V2530(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription);
        //[GEOS2-5856][gulab lakade][25 06 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2530(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductTypes> GetAllProductTypes_DTD_V2530(string IdCptypes);
        #region Aishwarya[Geos2-5629]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary AddStandardOperationsDictionary_V2540(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateStandardOperationsDictionary_V2540(StandardOperationsDictionary sod);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        StandardOperationsDictionary GetStandardOperationsDictionaryDetail_V2540(UInt64 idStandardOperationsDictionary);
        #endregion
        //[GEOS2-5853] [Aishwarya Ingale] [08 07 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionTimeReportLegend> GetAllProductionTimeReportLegend_V2540();
        //[GEOS2-5853] [Aishwarya Ingale] [08 07 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2540(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        //[pallavi jadhav] [11 07 2024][GEOS2-5901]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2540(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        //[pallavi jadhav] [15 07 2024][GEOS2-5917]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantDeliveryAnalysis> GetPlantDeliveryAnalysis_V2540(string CurrencyNameFromSetting, string CurrencySymbolFromSetting, string IdSite, DateTime fromDate, DateTime toDate);
        #region [pallavi jadhav][GEOS2-5907][17 07 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<RTMFutureLoad> GetRTMFutureLoadDetails_V2540(RTMFutureLoadParams FutureLoadParams);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetAllRTM_HRResources_V2540(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<RTMHRResourcesExpectedTime> GetRTM_HRResourcesExpectedTime_V2540(DateTime StartDate, DateTime EndDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMProductionTime> GetRTMTestBoardsInProduction_V2540(uint IdSite, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2540(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2540(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReview_V2540(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMPlantOperationalPlanning> GetPlantOperationPlanning_V2540(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMDeliveryVisualManagement> GetDVManagementProduction_V2540(string IdSite, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2540(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2540(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllReworkReport_V2540(DateTime fromDate, DateTime toDate, UInt32 IdSite);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetPlantModuleProductionDelayByPlant_V2540(List<Company> AllPlant, UInt32 IdSite, DateTime Date, UInt32 OriginalPlantIdSite);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2540(string PlantName, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsActiveERMPlant();
        #region Aishwarya[Geos2-6034]
        //[GEOS2-6034][Aishwarya Ingale][01-08-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2550(Company company, DateTime Date);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllReworkReport_V2550(DateTime fromDate, DateTime toDate, UInt32 IdSite);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2550(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2550(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2550(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2550(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        #endregion
        #endregion
        //[GEOS2-6040][gulab lakade] [13 08 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2550(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        //[GEOS2-5760][pallavi jadhav] [19 08 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetProductionOutputReworksMail_V2550(Company company, DateTime StartDate);
        //[GEOS2-6038][gulab lakade] [16 08 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2550(string PlantName, DateTime fromDate, DateTime toDate);
        //[GEOS2-6069][pallavi jadhav][21 08 2024]   
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2550(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        //rajashri GEOS2-5988[22-08-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetMisMatchDataOFIddrawing_V2550(string iddrawing, string serialnumber, Int32 Idsite);
        //[GEOS2-6058][gulab lakade][27 08 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_WeeklyProd_EmployeeTime GetWeeklyProductionEmployeeTime_V2560(Company company, DateTime StartDate, DateTime EndDate, string IdJobDeescription, List<ERM_IDStage_JobDescription> IDStage_JobDescription);
        //[GEOS2-5520][Aishwarya Ingale]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2560(Company company, DateTime StartDate);
        //Aishwarya Ingale[Geos2-6431]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantDeliveryAnalysis> GetPlantDeliveryAnalysis_V2560(string CurrencyNameFromSetting, string CurrencySymbolFromSetting, string IdSite, DateTime fromDate, DateTime toDate);
        #region [pallavi jadhav][GEOS2-6081][16 09 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2560(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2560(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryModules> GetAllStandardOperationsDictionaryModulesById_V2560(UInt64 IdStandardOperationsDictionary, UInt64 IdSite, UInt64 IdCPType,UInt64 IdCP);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryDetection> GetStandardOperationDictionaryDetectionById_V2560(UInt64 IdCP);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryOption> GetStandardOperationDictionaryOptionById_V2560(UInt64 IdCP);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryWays> GetStandardOperationDictionaryWayById_V2560(UInt64 IdCP);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryModules> GetAllStandardOperationsDictionaryModulesById_V2570(UInt64 IdStandardOperationsDictionary, string IdStage, string IdCPType,string IdCP);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryDetection> GetStandardOperationDictionaryDetectionById_V2570(string IdCP);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryOption> GetStandardOperationDictionaryOptionById_V2570(string IdCP);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryWays> GetStandardOperationDictionaryWayById_V2570(string IdCP);
        #endregion
        // start[GEOS2-6058][gulab lakade][24 09 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2570(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_WeeklyProduction GetGetWeeklyInProductionData_V2570(Company company, DateTime StartDate);
        // end[GEOS2-6058][gulab lakade][24 09 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdatePlanningDeliveryDateGridByStage_V2580( UInt32 IDCompany, ProductionPlanningReview PlanningDateReviewList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReview_V2580(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdatePlanningDateReviewByStage_V2580(UInt32 IDCompany, List<PlanningDateReview> PlanningDateReviewList);
        //[GEOS2-6529][gulab lakade] [22 10 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2580(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2580(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2580(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2580(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-6579][Aishwarya Ingale][07 11 2024]   
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2580(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        #region [pallavi jadhav][GEOS2-5465][06 11 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2580(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2580(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        #endregion
        //[Geos2-6611][Aishwarya Ingale][07 11 2024]   
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2590(DateTime StartDate, DateTime EndDate, int RecordLimit, string Idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option);
        //[GEOS2-5319][gulab lakade][18 11 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdatePlanningDateReview_V2580(List<PlanningDateReview> PlanningDateReviewList);
        //[GEOS2-4143] [Pallavi Jadhav] [15 03 2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdatePlanningDateGrid_V2580(ProductionPlanningReview PlanningDateReviewList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductionPlanningReview> GetProductionPlanningReviewByStage_V2580(string OriginPlant, DateTime fromDate, DateTime toDate, List<GeosAppSetting> GeosAppSettingList);
        //[GEOS2-6554][gulab lakade][28 11 2024]   
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2590(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        //rajashri GEOS 2-6713  [7-12-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GlobalTop> GetAllGlobalTopValues_V2590();
        #region [[GEOS2-6646]][Daivshala Vighne][10-12-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2590(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2590(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList);
        #endregion
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2590(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        #region [pallavi jadhav][24 12 2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Templates> GetAllTemplates_V2590();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CPType> GetAllCPTypes_V2590();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Region> GetAllRegion_V2590(string IdSite);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DesignSystemTypeStatus> GetAllDesigns_V2590();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Data.Common.ERM.Detections> GetAllDetections_V2590();
        #endregion
        #region [GEOS2-6759][Daivshala Vighne][13-01-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2600(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        #endregion
        #region [GEOS2-6759][Daivshala Vighne][13-01-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_WorkOrder_Other_ProductionTimeline GetWorkorder_Other_MonthlyProduction_V2600();
        #endregion
        #region //[GEOS2-6818][pallavi jadhav] [20 01 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2600(Company company, DateTime Date);
        #endregion
        //[GEOS2-6868][gulab lakade][27 01 2025]   
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2600(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        //[GEOS2-6900][gulab lakade][28 01 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_WeeklyProduction GetGetWeeklyInProductionData_V2610(Company company, DateTime StartDate);
        #region[GEOS2-6885][Daivshala Vighne][30 01 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_Warehouses> GetWarehousesByIDSite_V2610(Int32 IdSite);
        #endregion
        //[GEOS2-6900][gulab lakade][30 01 2025]   
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use GetGlobalComparisonTimesResults_V2650 instead.")]
        List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2610(DateTime StartDate, DateTime EndDate, int RecordLimit, string Idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DesignSystemTypeStatus> GetAllDesigns_V2610();
        #region [GEOS2-6646][pallavi jadhav][04-02-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2600(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2600(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList);
        #endregion
        #region [GEOS2-6771][gulab lakade][05 04 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2610(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        #endregion
        #region [GEOS2-6771][gulab lakade][05 04 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2610(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        #endregion
        #region [GEOS2-6683][rani dhamankar][06 02 2025] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2610(string PlantName, DateTime fromDate, DateTime toDate);
        #endregion
        #region [GEOS2-6891][pallavi jadhav][13 02 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2610(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2610(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        #endregion
        #region[GEOS2-6886][Pallavi jadhav][14 02 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMArticleStockPlanning> GetArticleStockPlanningList_V2610(Int32 IdWarehouse, DateTime StartDate, DateTime EndDate);
        #endregion
        //[GEOS2-6949][gulab lakade][17 02 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2610(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        #region[rani dhamankar][17-02-2025][GEOS2-6887]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMOutgoingArticleStockPlanning> GetOutgoingArticleStockPlanningList_V2610(Int32 IdArticle, Int32 IdWarehouse, DateTime StartDate, DateTime EndDate);
        #endregion
        #region [GEOS2-7031][gulab lakade][25 02 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestWorkOperationCodeByCode_V2620(string Code);
        #endregion 

        #region[rani dhamankar][24-02-2025][GEOS2-6889]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMIncomingArticleStockPlanning> GetIncomingArticleStockPlanningList_V2620(Int32 IdArticle, Int32 IdWarehouse, DateTime StartDate, DateTime EndDate);
        #endregion

        #region[rani dhamankar][27-02-2025][GEOS2-6888]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMOutgoingArticleStockPlanning> GetOutgoingArticleStockPlanningList_V2620(Int32 IdArticle, Int32 IdWarehouse, DateTime StartDate, DateTime EndDate);
        #endregion

        #region [GEOS2-6965][rani dhamankar][10-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2620(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion

        #region // [pallavi jadhav][GEOS2-7060][25-03-2025] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2630(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2630(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<MaxMinDate> GetDeliveryDateANDPlannedDeliveryDate_V2630(Int32 IdSite);

        #endregion

        #region [GEOS2-6836][rani dhamankar][26-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2630(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2630(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        #endregion
        #region [GEOS2-7642][gulab lakade][27 03 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2630(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion
        #region [GEOS2-7099][gulab lakade][07 04 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2630(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion

        #region [GEOS2-6835][dhawal bhalerao][08 04 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkOperationByStages> GetAllWorkOperationStages_V2630(int IdStage, string IdCP);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StandardOperationsDictionaryModules> GetAllStandardOperationsDictionaryModulesById_V2630(UInt64 IdStandardOperationsDictionary, string IdStage, string IdCPType, string IdCP);
        #endregion
        #region [GEOS2-7902][gulab lakade][28 04 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2640(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion
        
        #region [GEOS2-7098][rani dhamankar][23 04 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2640(string PlantName, DateTime fromDate, DateTime toDate);
        #endregion
        
        #region [GEOS2-7094][dhawal bhalerao][28 04 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetCamCadTimeTrackings_V2640(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetCamCadTimeTrackingBYPlant_V2640(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        #endregion

        #region [GEOS2-6573][rani dhamankar][07-05-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2640(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion
        //[GEOS2-7908][gulab lakade][13 05 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2640(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);

        #region [pallavi.jadhav][29 04 2025][GEOS2-7066]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2640(Company company, DateTime StartDate);
        #endregion

        #region //[Pallavi.jadhav][16 05 2025][GEOS2-8124]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2640(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2640(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use GetAllTimeTrackingReport_V2650 instead.")]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2640(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2640(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        #endregion

        #region [GEOS2-8189][rani dhamankar][29-05-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2650(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion
        #region [GEOS2-8187][gulab lakade][09 06 2025][add logic for machine time for CNC Stage]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WeeklyProductionReport> GetAllWeeklyproductionReportByPlant_V2650(Company company, DateTime StartDate);
        #endregion

        //[nsatpute][25-06-2025][GEOS2-8641]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2650(string CurrencyName, string PlantName, List<SitesByShippingAddress> sitesByShippingAddressList, List<GeosAppSetting> GeosAppSettingList, GeosAppSetting timeTrackingAppSetting, DateTime fromDate, DateTime toDate);

        //[nsatpute][25-06-2025][GEOS2-8641]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SitesByShippingAddress> GetAllSitesByShippingAddress();

        //[nsatpute][02.07.2025][GEOS2-8172]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2650(DateTime StartDate, DateTime EndDate, int RecordLimit, string Idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option);
        #region [GEOS2-8868][gulab lakade][10 07 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetCamCadTimeTrackings_V2660(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetCamCadTimeTrackingBYPlant_V2660(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        #endregion

        #region [pallavi.jadhav][11 07 2025][GEOS2-8868] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2660(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2660(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2660(string CurrencyName, string PlantName, List<SitesByShippingAddress> sitesByShippingAddressList, List<GeosAppSetting> GeosAppSettingList, GeosAppSetting timeTrackingAppSetting, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2660(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        #endregion

        #region [GEOS2-8698][rani dhamankar][15-07-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Site> GetPlants_V2660(int IdUser);

        // [GEOS2-8698][rani dhamankar][16-07-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompaniesDetails_V2660(Int32 idUser);

        #endregion
        #region [GEOS2-8005][rani dhamankar][13-06-2025]
       
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingCADCAMDetailsPdfEmail_V2660(string PlantName, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingProductionTimeline_V2660(string PlantName, DateTime fromDate, DateTime toDate);

       
        #endregion

        #region [pallavi.jadhav][21 07 2025][GEOS2-8814]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackingReport_V2660V1(string CurrencyName, string PlantName, List<SitesByShippingAddress> sitesByShippingAddressList, List<GeosAppSetting> GeosAppSettingList, GeosAppSetting timeTrackingAppSetting, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingReportBYPlant_V2660V1(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);
        #endregion

        #region  [GEOS2-8189][pallavi jadhav][28-07-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_ProductionTimeline> GetProductionTimeline_V2660(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion

        #region [GEOS2-8907][pallavi jadhav][30-07-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2660(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2660(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        #endregion
        #region [GEOS2-9119][gulab lakade][05 08 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetProductionOutputReworksMail_V2660(Company company, DateTime StartDate);
        #endregion
        #region[GEOS2-9201][rani dhamankar][12-08-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2660(DateTime StartDate, DateTime EndDate, int RecordLimit, string Idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option);
        #endregion
        #region[GEOS2-9233][rani dhamankar][13-08-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2670(DateTime StartDate, DateTime EndDate, int RecordLimit, string Idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option);
        #endregion
        #region  [GEOS2-9220][gulab lakade][08 08 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_Main_productiontimeline GetProductionTimeline_V2660_V1(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion
        #region  [GEOS2-9220][gulab lakade][08 08 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_Main_productiontimeline GetProductionTimeline_V2670(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion
        #region [GEOS2-8309][rani dhamankar][29 08 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetCamCadTimeTrackings_V2670(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetCamCadTimeTrackingBYPlant_V2670(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeTrackingWithSites GetAllTimeTrackings_V2670(string CurrencyName, string PlantName, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimeTracking> GetTimeTrackingBYPlant_V2670(UInt32 PlantID, UInt32 ProductionIdSite, string Currency, List<GeosAppSetting> GeosAppSettingList, DateTime fromDate, DateTime toDate);

        #endregion
        #region [GEOS2-7091][rani dhamankar][11 09 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Counterpartstracking> GetOperatorDesignSahredItemTimeTrackingDetails_V2670(UInt32 PlantID,  DateTime fromDate, DateTime toDate, UInt32 IdSiteOwnersPlant);
        #endregion

        #region [pallavi.jadhav][GEOS2-8550][23 09 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetMonthlyProductionTimeLine_V2670(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_MonthlyProductionTimeline GetWeeklyProductionEmployeeTime_V2670(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDeescription);
        #endregion
        #region // [suggestion of yuvraj sir][gulab lakade][03 10 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetProductionOutputReworksMail_V2670(Company company, DateTime StartDate);

        #endregion
        #region  // [suggestion of yuvraj sir][gulab lakade][03 10 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_ReworkReport GetALLPlantWeeklyReworksMail_V2670(Company company, DateTime Date);
        #endregion
        #region [GEOS2-9443] [gulab lakade][2025 10 16]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_WorkOrder_Other_ProductionTimeline GetWorkorder_Other_MonthlyProduction_V2680();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_Main_productiontimeline GetProductionTimeline_V2680(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion

        #region [GEOS2-10146][pallavi jadhav][11-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Site> GetPlants_V2680(int IdUser);

       
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompaniesDetails_V2680(Int32 idUser);

        #endregion
        #region[GEOS2-9554][rajashri telvekar][12-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_GlobalComparisonTimes> GetGlobalComparisonTimesResults_V2690(DateTime StartDate, DateTime EndDate, int RecordLimit, string Idsite, string Region, string Stage, string Template, string CPType, string DesignSystem, string Type, string DSAStatus, string numofways, string numofDetection, string numofOption, string ways, string Detection, string Option, string Currentidstage);
        #endregion

        #region [GEOS2-9393][pallavi jadhav][12 11 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ERM_Main_productiontimeline GetProductionTimeline_V2690(Int32 IdCompany, DateTime StartDate, DateTime EndDate, string IdJobDescription);

        #endregion
        #region [GEOS2-9404][gulab lakade][18 11 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERM_Warehouses> GetWarehousesByIDSite_V2690(Int32 IdSite);
        #endregion
        #region [GEOS2-9123][gulab lakade][20 11 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMArticleStockPlanning> GetArticleStockPlanningList_V2690(Int32 IdWarehouse, DateTime StartDate, DateTime EndDate);
        #endregion
        #region [GEOS2-9398][gulab lakade][20 11 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ERMOutgoingArticleStockPlanning> GetOutgoingArticleStockPlanningList_V2690(Int32 IdArticle, Int32 IdWarehouse, DateTime StartDate, DateTime EndDate);
        #endregion
    }
}