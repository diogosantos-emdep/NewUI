using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.TSM;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TSMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TSMService.svc or TSMService.svc.cs at the Solution Explorer and start debugging.
    
    public class TSMService : ITSMService

    {
        //[GEOS2-5388][pallavi.kale][13.01.2025]
        public List<TSMUsers> GetUserDetailsList_V2600()
        {
            try
            {
                TSMManager mgr = new TSMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUserDetailsList_V2600(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        } 
        //  [GEOS2-5388][pallavi.kale][22.01.2025]
        public IList<LookupValue> GetLookupValues(byte key)
        {
            IList<LookupValue> list = null;
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                TSMManager mgr = new TSMManager();
                list = mgr.GetLookupValues(ConnectionString, key);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return list;
        }
        //  [GEOS2-5388][nsatpute][30.01.2025]
        public bool UpdateUserPermissions(TSMUsers user, List<LookupValue> permissions)
        {
            IList<LookupValue> list = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                TSMManager mgr = new TSMManager();
                return mgr.UpdateUserPermissions(user, permissions, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-6993][pallavi.kale][26.02.2025]
        public List<TSMUsers> GetUserDetailsList_V2610()
        {
            try
            {
                TSMManager mgr = new TSMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUserDetailsList_V2610(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
       
        //[GEOS2-8963][pallavi.kale][28.11.2025]
        public List<Company> GetAllCompaniesDetails_V2690(Int32 idUser)
        {
            List<Company> connectionstrings = null;
            try
            {
                TSMManager mgr = new TSMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompaniesDetails_V2690(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return connectionstrings;
        }
        
        //[GEOS2-8963][pallavi.kale][28.11.2025]
        public ObservableCollection<Ots> GetPendingOrdersByPlant_V2690(Company company)
        {
            try
            {
                TSMManager mgr = new TSMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPendingOrdersByPlant_V2690(company, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-8965][pallavi.kale][28.11.2025]
        public Ots GetWorkOrderByIdOt_V2690(Int64 idOt, Company company)
        {
            try
            {
                TSMManager mgr = new TSMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWorkOrderByIdOt_V2690(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-8973][pallavi.kale][28.11.2025]
        public bool UpdateOTAssignedUser_V2690(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser)
        {
            try
            {
                TSMManager mgr = new TSMManager();
                return mgr.UpdateOTAssignedUser_V2690(company, otAssignedUsers, IdOT, OldRemark, Remark, IdUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-8973][pallavi.kale][28.11.2025]
        public List<OTAssignedUser> GetUsersToAssignedOT_V2690(Company company, Int32 idCompany)
        {
            try
            {
                TSMManager mgr = new TSMManager();
                return mgr.GetUsersToAssignedOT_V2690(company, Properties.Settings.Default.UserProfileImage, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-8973][pallavi.kale][28.11.2025]
        public List<OTAssignedUser> GetOTAssignedUsers_V2690(Company company, Int64 idOT)
        {
            try
            {
                TSMManager mgr = new TSMManager();
                return mgr.GetOTAssignedUsers_V2690(company, idOT, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-8981][pallavi.kale][28.11.2025]
        public List<TSMWorkLogReport> GetOTWorkLogTimesByPeriodAndSite_V2690(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant)
        {
            try
            {
                TSMManager mgr = new TSMManager();
                return mgr.GetOTWorkLogTimesByPeriodAndSite_V2690(FromDate, ToDate, IdSite, Plant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-8965][pallavi.kale][28.11.2025]
        public bool UpdateWorkflowStatusInOT_V2690(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<Data.Common.TSM.TSMLogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                TSMManager mgr = new TSMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateWorkflowStatusInOT_V2690(connectionString, IdOT, IdWorkflowStatus, IdUser, LogEntriesByOTList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
    }
}
