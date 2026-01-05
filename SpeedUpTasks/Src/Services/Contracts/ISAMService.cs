using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SAM;
using System;
using System.Collections.Generic;
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
        List<Ots> GetPendingWorkorders_V2044(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
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
    }
}

