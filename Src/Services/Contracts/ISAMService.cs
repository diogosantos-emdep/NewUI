using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Data.Common.SRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISamService" in both code and config file together.
    [ServiceContract]
    public interface ISAMService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use GetPendingWorkorders_V2038 instead.")]
        List<Ots> GetPendingWorkorders(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetOTWorkingTimeByIdOT_V2040 instead.")]
        List<OTWorkingTime> GetOTWorkingTimeByIdOT(Company company, Int64 idOT);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        OTWorkingTime AddOTWorkingTime(Company company, OTWorkingTime otWorkingTime);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTWorkingTime(Company company, OTWorkingTime otWorkingTime);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2043. Use GetPendingWorkorders_V2043 instead.")]
        List<Ots> GetPendingWorkorders_V2038(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTWorkingTime> GetOTWorkingTimeDetails(Int64 idOT, Company company);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Article GetArticleDetails(Int32 idArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetWorkOrderByIdOt_V2170 instead.")]
        Ots GetWorkOrderByIdOt(Int64 idOt, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAssignedUser> GetOTAssignedUsers(Company company, Int64 idOT);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2044. Use GetUsersToAssignedOT_V2044 instead.")]
        List<OTAssignedUser> GetUsersToAssignedOT(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use UpdateOTAssignedUser_V2180 instead.")]
        bool UpdateOTAssignedUser(Company company, List<OTAssignedUser> otAssignedUsers);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTWorkingTime> GetOTWorkingTimeByIdOT_V2040(Company company, Int64 idOT);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ValidateItem> GetWorkOrderItemsToValidate(Company company, Int64 idOT);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTestBoardPartNumberTracking(Company company, Int64 idPartNumberTracking, Int32 status, Int32 idOperator);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2044. Use GetPendingWorkorders_V2044 instead.")]
        List<Ots> GetPendingWorkorders_V2043(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAttachment> GetOTAttachment(Company company,Int64 idOT);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetOTAttachmentInBytes(string fileName, string quotationYear, string quotationCode);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAssignedUser> GetUsersToAssignedOT_V2044(Company company,Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetPendingWorkorders_V2090 instead.")]
        List<Ots> GetPendingWorkorders_V2044(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use UpdateOTFromGrid_V2180 instead.")]
        bool UpdateOTFromGrid(Company company, Ots ot);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorklogUser> GetWorkLogUserListByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TempWorklog> GetWorkLogHoursByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TempWorklog> GetWorkLogOTWithHoursByPeriodAndSiteAndUser(DateTime FromDate, DateTime ToDate, int IdSite, int IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TempWorklog> GetWorkLogOTWithHoursAndUserByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TempWorklog> GetOTWorkLogTimesByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteWorkLog(Company company, Int64 idOTWorkingTime);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkLog(Company company, OTWorkingTime otWorkingTime);

      
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetPendingWorkorders_V2170 instead.")]
        List<Ots> GetPendingWorkorders_V2090(Company company);


        //[GEOS2-2906]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetPendingWorkorders_V2180 instead.")]
        List<Ots> GetPendingWorkorders_V2170(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowStatus> GetAllWorkflowStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowTransition> GetAllWorkflowTransitions();


      

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkflowStatusInOT(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetWorkOrderByIdOt_V2180 instead.")]
        Ots GetWorkOrderByIdOt_V2170(Int64 idOt, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetWorkOrderByIdOt_V2530 instead.")]
        Ots GetStructureWorkOrderByIdOt_V2170(Int64 idOt, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetWorkOrderByIdOt_V2180(Int64 idOt, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTAssignedUser_V2180(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser);


        //[GEOS2-2961]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2270. Use GetPendingWorkorders_V2270 instead.")]
        List<Ots> GetPendingWorkorders_V2180(Company company);

        //[GEOS2-2961]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTFromGrid_V2180(Company company, Ots ot);

        //[GEOS2-2959]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPendingWorkordersForDashboard_V2180(Company company);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetAllAssignedWorkordersForPlanning_V2250(Company company,
            out List<OTWorkingTime> listLoggedHoursForOT_User_Date, out List<PlannedHoursForOT_User_Date> listPlannedHoursForOT_User_Date);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTUserPlanningsFromGrid_V2250(Company company, List<PlannedHoursForOT_User_Date> listLoggedHoursForOT_User_Date);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetSAMWorkOrdersReport_V2260(Company company, DateTime fromDate, DateTime toDate);

        //[GEOS2-3417]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2280. Use GetPendingWorkorders_V2280 instead.")]
        List<Ots> GetPendingWorkorders_V2270(Company company);

        //[GEOS2-3585]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetPendingWorkorders_V2301 instead.")]
        List<Ots> GetPendingWorkorders_V2280(Company company);

        //[GEOS2-3586]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowStatus> GetAllWorkflowStatusForQuality();

        //[GEOS2-3586]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowTransition> GetAllWorkflowTransitionsForQuality();

        //[GEOS2-3588]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTQuality(Company company, Int64 IdOT, int IdUser,string otAttachment, string GUIDString, string year, string quotationCode, List<OTAttachment> lstAttachmentDeleted, string OldRemark, string Remark);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAttachment> GetQualityOTAttachment(Company company, Int64 idOT);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkflowStatusInOTQC(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetWorkOrderByIdOtForQC(Int64 idOt, Company company);

        //[GEOS2-3915]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPendingWorkorders_V2301(Company company);

        //[pjadhav][GEOS2-3681][12/09/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetAllOrderItemsList_V2350 instead.")]
        List<OTs> GetAllOrderItemsList(Company company);

        //Shubham[skadam] GEOS2-3682 Implement in SAM the items in the WO that must be passed in Bancos Stage (3/9) 09 12 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        OTs GetSAMOrderItemsInformationByIdOt_V2340(Int64 idOt, Company company, UInt32 IdArticle);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Data.Common.PCM.PCMArticleImage> GetPCMArticleImagesByIdPCMArticle(Int32 IdArticle, string ArticleReference);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        OtItemsComment AddObservationCommentItem(OtItemsComment ItemsComment ,Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteComment_V2340(Int64 idComment, Company company);

        //[pjadhav][GEOS2-3686][12/26/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTWorkingTime> GetOTWorkingTimeDetails_V2350(Int64 OtItems, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        OTWorkingTime AddOtWorkingTimeWorkLogItem(Int64 IdOT , Int64 idOTItems, OTWorkingTime Items, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTs> GetAllOrderItemsList_V2350(Company company);

        //[rdixit][28.08.2023][GEOS2-4754]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetPendingWorkorders_V2500 instead.")]
        List<Ots> GetPendingWorkorders_V2430(Company company);

        //[rdixit][28.08.2023][GEOS2-4754]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTs> GetAllOrderItemsList_V2430(Company company);

        //[rdixit][12.03.2024][GEOS2-5361]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPendingWorkorders_V2500(Company company);

        /// [rgadhave][18-06-2024][GEOS2-5583]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2540. Use GetStructureWorkOrderByIdOt_V2540 instead.")]
        Ots GetStructureWorkOrderByIdOt_V2530(Int64 idOt, Company company);

        // [GEOS2-5472][nsatpute][04-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetStructureWorkOrderByIdOt_V2540(Int64 idOt, Company company);

        //[GEOS2-5472][nsatpute][04-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetSolidworksDrawingFileImageInBytes(string darwingPath, string SolidworksDrawingFileName);

        //[nsatpute][04-07-2024][GEOS2-5408]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupValues(byte key);

        //[pramod.misal][GEOS2-5473][09.07.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetAllReference(bool seeAllArticles);

        //[pramod.misal][GEOS2-5473][15.07.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetWorkOrderByIdOt_V2540(Int64 idOt, Company company);

        //[pramod.misal][GEOS2-5473][15.07.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTAssignedUser_V2540(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser, List<OtItem> DeletedOtItemList, List<OtItem> AddedOtItemList, List<OtItem> UpdatedOtItemList, List<LogEntriesByOT> LogEntriesByOTList);


        // [nsatpute][18-07-2024] [GEOS2-5409] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void StartSavingFile(string saveDirectorPath, string fileName);

        // [nsatpute][18-07-2024] [GEOS2-5409] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SavePartData(string saveDirectorPath, string fileName, byte[] partData);

        // [nsatpute][19-07-2024] [GEOS2-5409] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAttachment> GetQualityOTAndItemAttachments(Company company, Int64 idOT);

        //[GEOS2-5473][nsatpute][26.07.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetComponentArticlesByIdArticle(int idArticle);

        //[nsatpute][02-08-2024][GEOS2-5410]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TechnicalSpecifications GetTechnicalSpecificationForReport(Company company, long idOt);

        // [GEOS2-5410][06-08-2024][nsatpute]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ModuleReference> GetModuleReferencesForReport(Company company, long idOt);

        // [GEOS2-5410][06-08-2024][nsatpute]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAttachment> GetStructureOTAttachment(Company company, Int64 idOT);

        //[nsatpute][GEOS2-5410][08-08-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetTestboardElectrificationOtArticles(Company company, long idOt);

        // [nsatpute][12-08-2024][GEOS2-5412]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAttachment> GetItemAttachments(Company company, Int64 idOT);

        // [nsatpute][13-08-2024][GEOS2-5411]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetstructureOtPlanolInBytes(Company company, Int64 idOt);

        // [nsatpute][14-08-2024][GEOS2-5411]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<byte[]> GetstructureOtElectricalDiagramsInBytes(Company company, Int64 idOt);

        // [nsatpute][19-08-2024][GEOS2-5412]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductTypes GetstructureOtProductDetails(Company company, Int64 idOt);

        // [nsatpute][21-08-2024][GEOS2-5412]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GenerateAndGetDeclarationOfConformity(Company company, Int64 idOt);

        // [nsatpute][22-08-2024][GEOS2-5412]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetCustomerLogoById(int idCustomer);

        //[nsatpute][03-09-2024][GEOS2-5415]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        QcPassLabelDetails GetQCPassLabelDetails(Company company, Int64 idOt);

        // [nsatpute][12-11-2024][GEOS2-5890]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkflowStatusInOTQC_V2580(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList);

        // [nsatpute][13-11-2024][GEOS2-5889]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use GetPendingWorkorders_V2660 instead.")]
        List<Ots> GetPendingWorkorders_V2580(Company company);

        // [GEOS2-6727][pallavi.kale][14-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetStructureWorkOrderByIdOt_V2640(Int64 idOt, Company company);

        // [GEOS2-6727][pallavi.kale][14-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ElectricalDiagram> GetElectricalDiagram();

        // [GEOS2-6727][pallavi.kale][14-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetElectricalDiagramsFileImageInBytes(string ElectricalDiagramFileName);

        // [GEOS2-6728][pallavi.kale][14-04-2025]
		// [nsatpute][13-05-2025][GEOS2-6728]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEditDeleteElectricalDiagramForIdDrawing(List<OtItem> otItems);

        //[nsatpute][12.08.2025][GEOS2-9163]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPendingWorkorders_V2660(Company company);

        // [Rahul.Gadhave][GEOS2-8713][Date:03/11/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompaniesDetails_V2680(Int32 idUser);
    }
}

