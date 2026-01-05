using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.TSM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;

using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITSMService" in both code and config file together.
    [ServiceContract]
    public interface ITSMService
    {
        //[GEOS2-5388][pallavi.kale][13.01.2025]
      

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TSMUsers> GetUserDetailsList_V2600();

        //  [GEOS2-5388][pallavi.kale][22.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupValues(byte key);

        //[GEOS2-5388][nsatpute][30.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateUserPermissions(TSMUsers user, List<LookupValue> permissions);

        //[GEOS2-6993][pallavi.kale][26.02.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TSMUsers> GetUserDetailsList_V2610();
        
        //[GEOS2-8963][pallavi.kale][28.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompaniesDetails_V2690(Int32 idUser);

        //[GEOS2-8963][pallavi.kale][28.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ObservableCollection<Ots> GetPendingOrdersByPlant_V2690(Company company);

        //[GEOS2-8965][pallavi.kale][28.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetWorkOrderByIdOt_V2690(Int64 idOt, Company company);

        //[GEOS2-8973][pallavi.kale][28.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTAssignedUser_V2690(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser);

        //[GEOS2-8973][pallavi.kale][28.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAssignedUser> GetUsersToAssignedOT_V2690(Company company, Int32 idCompany);

        //[GEOS2-8973][pallavi.kale][28.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAssignedUser> GetOTAssignedUsers_V2690(Company company, Int64 idOT);

        //[GEOS2-8981][pallavi.kale][28.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TSMWorkLogReport> GetOTWorkLogTimesByPeriodAndSite_V2690(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant);

        //[GEOS2-8965][pallavi.kale][28.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkflowStatusInOT_V2690(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<TSMLogEntriesByOT> LogEntriesByOTList);
    }
}
