using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ServiceModel;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SamService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SamService.svc or SamService.svc.cs at the Solution Explorer and start debugging.
    public class SAMService : ISAMService
    {
        SAMManager mgr = new SAMManager();

        /// <summary>
        /// This method is to get pending workorders details
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <returns></returns>
        public List<Ots> GetPendingWorkorders(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders(company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get ot working time details related to idot
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <param name="idOT">Get idot</param>
        /// <returns>List of ot working time deatils</returns>
        public List<OTWorkingTime> GetOTWorkingTimeByIdOT(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetOTWorkingTimeByIdOT(company, idOT, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to add ot working time
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <param name="otWorkingTime">Get ot working time details</param>
        /// <returns>ot working time details</returns>
        public OTWorkingTime AddOTWorkingTime(Company company, OTWorkingTime otWorkingTime)
        {
            try
            {
                return mgr.AddOTWorkingTime(company, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to update ot working time 
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <param name="otWorkingTime">Get ot working time details</param>
        /// <returns>Is updated or not</returns>
        public bool UpdateOTWorkingTime(Company company, OTWorkingTime otWorkingTime)
        {
            try
            {
                return mgr.UpdateOTWorkingTime(company, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get pending workorders details
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <returns></returns>
        public List<Ots> GetPendingWorkorders_V2038(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2038(company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get list of ot working time details
        /// </summary>
        /// <param name="idOT">Get idot</param>
        /// <param name="company">Get company details</param>
        /// <returns>List of ot working time details</returns>
        public List<OTWorkingTime> GetOTWorkingTimeDetails(Int64 idOT, Company company)
        {
            try
            {
                return mgr.GetOTWorkingTimeDetails(idOT, company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get article details by article reference
        /// </summary>
        /// <param name="reference">Get article reference</param>
        /// <returns>Article Details</returns>
        public Article GetArticleDetails(Int32 idArticle)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleDetails(idArticle, Properties.Settings.Default.ArticleVisualAidsPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Ots GetWorkOrderByIdOt(Int64 idOt, Company company)
        {
            try
            {
                return mgr.GetWorkOrderByIdOt(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OTAssignedUser> GetOTAssignedUsers(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetOTAssignedUsers(company, idOT, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OTAssignedUser> GetUsersToAssignedOT(Company company)
        {
            try
            {
                return mgr.GetUsersToAssignedOT(company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateOTAssignedUser(Company company, List<OTAssignedUser> otAssignedUsers)
        {
            try
            {
                return mgr.UpdateOTAssignedUser(company, otAssignedUsers);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get ot working time details related to idot
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <param name="idOT">Get idot</param>
        /// <returns>List of ot working time deatils</returns>
        public List<OTWorkingTime> GetOTWorkingTimeByIdOT_V2040(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetOTWorkingTimeByIdOT_V2040(company, idOT, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ValidateItem> GetWorkOrderItemsToValidate(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetWorkOrderItemsToValidate(company, idOT);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateTestBoardPartNumberTracking(Company company, Int64 idPartNumberTracking, Int32 status, Int32 idOperator)
        {
            try
            {
                return mgr.UpdateTestBoardPartNumberTracking(company, idPartNumberTracking, status, idOperator);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get pending workorders details
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <returns></returns>
        public List<Ots> GetPendingWorkorders_V2043(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2043(company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OTAttachment> GetOTAttachment(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetOTAttachment(company, idOT, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public byte[] GetOTAttachmentInBytes(string fileName, string quotationYear, string quotationCode)
        {
            try
            {
                return mgr.GetOTAttachmentInBytes(fileName, Properties.Settings.Default.WorkingOrdersPath, quotationYear, quotationCode);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OTAssignedUser> GetUsersToAssignedOT_V2044(Company company, Int32 idCompany)
        {
            try
            {
                return mgr.GetUsersToAssignedOT_V2044(company, Properties.Settings.Default.UserProfileImage, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Ots> GetPendingWorkorders_V2044(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2044(company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateOTFromGrid(Company company, Ots ot)
        {
            try
            {
                return mgr.UpdateOTFromGrid(company, ot);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<WorklogUser> GetWorkLogUserListByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant)
        {
            try
            {
                return mgr.GetWorkLogUserListByPeriodAndSite(FromDate, ToDate, IdSite, Properties.Settings.Default.UserProfileImage, Plant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TempWorklog> GetWorkLogHoursByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWorkLogHoursByPeriodAndSite(connectionString, FromDate, ToDate, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TempWorklog> GetWorkLogOTWithHoursByPeriodAndSiteAndUser(DateTime FromDate, DateTime ToDate, int IdSite, int IdUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWorkLogOTWithHoursByPeriodAndSiteAndUser(connectionString, FromDate, ToDate, IdSite, IdUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TempWorklog> GetWorkLogOTWithHoursAndUserByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant)
        {
            try
            {
                return mgr.GetWorkLogOTWithHoursAndUserByPeriodAndSite(FromDate, ToDate, IdSite, Plant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TempWorklog> GetOTWorkLogTimesByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company Plant)
        {
            try
            {
                return mgr.GetOTWorkLogTimesByPeriodAndSite(FromDate, ToDate, IdSite, Plant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool DeleteWorkLog(Company company, Int64 idOTWorkingTime)
        {
            try
            {
                return mgr.DeleteWorkLog(company, idOTWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateWorkLog(Company company, OTWorkingTime otWorkingTime)
        {
            try
            {
                return mgr.UpdateWorkLog(company, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Ots> GetPendingWorkorders_V2090(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2090(company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2906]
        public List<Ots> GetPendingWorkorders_V2170(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2170(company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool UpdateWorkflowStatusInOT(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateWorkflowStatusInOT(connectionString, IdOT, IdWorkflowStatus, IdUser, LogEntriesByOTList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowStatus> GetAllWorkflowStatus()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWorkflowStatus(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowTransition> GetAllWorkflowTransitions()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWorkflowTransitions(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        public Ots GetWorkOrderByIdOt_V2170(Int64 idOt, Company company)
        {
            try
            {
                return mgr.GetWorkOrderByIdOt_V2170(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Ots GetStructureWorkOrderByIdOt_V2170(Int64 idOt, Company company)
        {
            try
            {
                return mgr.GetStructureWorkOrderByIdOt_V2170(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Ots GetWorkOrderByIdOt_V2180(Int64 idOt, Company company)
        {
            try
            {
                return mgr.GetWorkOrderByIdOt_V2180(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateOTAssignedUser_V2180(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser)
        {
            try
            {
                return mgr.UpdateOTAssignedUser_V2180(company, otAssignedUsers, IdOT, OldRemark, Remark, IdUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2961]
        public List<Ots> GetPendingWorkorders_V2180(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2180(company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateOTFromGrid_V2180(Company company, Ots ot)
        {
            try
            {
                return mgr.UpdateOTFromGrid_V2180(company, ot);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2959]
        public List<Ots> GetPendingWorkordersForDashboard_V2180(Company company)
        {
            try
            {
                return mgr.GetPendingWorkordersForDashboard_V2180(company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Ots> GetAllAssignedWorkordersForPlanning_V2250(Company company,
            out List<OTWorkingTime> listLoggedHoursForOT_User_Date, out List<PlannedHoursForOT_User_Date> listPlannedHoursForOT_User_Date)
        {
            try
            {
                return mgr.GetAllAssignedWorkordersForPlanning_V2250(company, Properties.Settings.Default.UserProfileImage,
                        out listLoggedHoursForOT_User_Date, out listPlannedHoursForOT_User_Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateOTUserPlanningsFromGrid_V2250(Company company, List<PlannedHoursForOT_User_Date> listLoggedHoursForOT_User_Date)
        {
            try
            {
                return mgr.UpdateOTUserPlanningsFromGrid_V2250(company, listLoggedHoursForOT_User_Date);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }




        public List<Ots> GetSAMWorkOrdersReport_V2260(Company company, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return mgr.GetSAMWorkOrdersReport_V2260(company, Properties.Settings.Default.UserProfileImage,
                        fromDate, toDate, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3417]
        public List<Ots> GetPendingWorkorders_V2270(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2270(company, Properties.Settings.Default.UserProfileImage, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-3585]
        public List<Ots> GetPendingWorkorders_V2280(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2280(company, Properties.Settings.Default.UserProfileImage, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowStatus> GetAllWorkflowStatusForQuality()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWorkflowStatusForQuality(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowTransition> GetAllWorkflowTransitionsForQuality()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWorkflowTransitionsForQuality(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateOTQuality(Company company, Int64 IdOT, int IdUser, string otAttachment, string GUIDString, string year, string quotationCode, List<OTAttachment> lstAttachmentDeleted, string OldRemark, string Remark)
        {
            try
            {
                return mgr.UpdateOTQuality(company, IdOT, IdUser, otAttachment, GUIDString, year, quotationCode, Properties.Settings.Default.WorkingOrdersPath, lstAttachmentDeleted, OldRemark, Remark);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OTAttachment> GetQualityOTAttachment(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetQualityOTAttachment(company, idOT, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateWorkflowStatusInOTQC(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateWorkflowStatusInOTQC(connectionString, IdOT, IdWorkflowStatus, IdUser, LogEntriesByOTList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Ots GetWorkOrderByIdOtForQC(Int64 idOt, Company company)
        {
            try
            {
                return mgr.GetWorkOrderByIdOtForQC(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3915]
        public List<Ots> GetPendingWorkorders_V2301(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2301(company, Properties.Settings.Default.UserProfileImage, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pjadhav][GEOS2-3681][12/09/2022]
        public List<OTs> GetAllOrderItemsList(Company company)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllOrderItemsList(connectionString, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-3682 Implement in SAM the items in the WO that must be passed in Bancos Stage (3/9) 09 12 2022
        public OTs GetSAMOrderItemsInformationByIdOt_V2340(Int64 idOt, Company company, UInt32 IdArticle)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetSAMOrderItemsInformationByIdOt_V2340(idOt, company, IdArticle, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<Data.Common.PCM.PCMArticleImage> GetPCMArticleImagesByIdPCMArticle(Int32 IdArticle, string ArticleReference)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetPCMArticleImagesByIdPCMArticle(PCMConnectionString, Properties.Settings.Default.ArticleVisualAidsPath, IdArticle, Properties.Settings.Default.ArticleImages, ArticleReference);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public OtItemsComment AddObservationCommentItem(OtItemsComment ItemsComment, Company company)
        {
            try
            {
                // CrmManager mgr = new CrmManager();
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddObservationCommentItem(ItemsComment, company, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool DeleteComment_V2340(Int64 idComment, Company company)
        {
            try
            {
                return mgr.DeleteComment_V2340(idComment, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pjadhav][GEOS2-3686][12/26/2022]
        public List<OTWorkingTime> GetOTWorkingTimeDetails_V2350(Int64 otItems, Company company)
        {
            try
            {
                return mgr.GetOTWorkingTimeDetails_V2350(otItems, company, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public OTWorkingTime AddOtWorkingTimeWorkLogItem(Int64 IdOT, Int64 idOTItems, OTWorkingTime Items, Company company)
        {
            try
            {
                string PCMConnectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.AddOtWorkingTimeWorkLogItem(IdOT, idOTItems, Items, company, PCMConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        public List<OTs> GetAllOrderItemsList_V2350(Company company)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllOrderItemsList_V2350(connectionString, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][28.08.2023][GEOS2-4754]
        public List<Ots> GetPendingWorkorders_V2430(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2430(company, Properties.Settings.Default.UserProfileImage, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][28.08.2023][GEOS2-4754]
        public List<OTs> GetAllOrderItemsList_V2430(Company company)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetAllOrderItemsList_V2430(connectionString, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][12.03.2024][GEOS2-5361]
        public List<Ots> GetPendingWorkorders_V2500(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2500(company, Properties.Settings.Default.UserProfileImage, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        /// [rgadhave][18-06-2024][GEOS2-5583]
        public Ots GetStructureWorkOrderByIdOt_V2530(Int64 idOt, Company company)
        {
            try
            {
                return mgr.GetStructureWorkOrderByIdOt_V2530(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [GEOS2-5472][nsatpute][04-07-2024]
        public Ots GetStructureWorkOrderByIdOt_V2540(Int64 idOt, Company company)
        {
            try
            {
                return mgr.GetStructureWorkOrderByIdOt_V2540(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5472][nsatpute][04-07-2024]
        public byte[] GetSolidworksDrawingFileImageInBytes(string darwingPath, string SolidworksDrawingFileName)
        {
            try
            {
                return mgr.GetSolidworksDrawingFileImageInBytes(darwingPath, SolidworksDrawingFileName, Properties.Settings.Default.SolidWorksPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][04-07-2024][GEOS2-5408]
        public IList<LookupValue> GetLookupValues(byte key)
        {
            try
            {
                return mgr.GetLookupValues(key);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5473][09.07.2024]
        public List<Article> GetAllReference(bool seeAllArticles)
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllReference(seeAllArticles,connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5473][15.07.2024]
        public Ots GetWorkOrderByIdOt_V2540(Int64 idOt, Company company)
        {
            try
            {
                return mgr.GetWorkOrderByIdOt_V2540(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateOTAssignedUser_V2540(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser, List<OtItem> DeletedOtItemList, List<OtItem> AddedOtItemList, List<OtItem> UpdatedOtItemList, List<LogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                return mgr.UpdateOTAssignedUser_V2540(company, otAssignedUsers, IdOT, OldRemark, Remark, IdUser, DeletedOtItemList, AddedOtItemList, UpdatedOtItemList, LogEntriesByOTList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][18-07-2024] [GEOS2-5409] 
        public void StartSavingFile(string saveDirectorPath, string fileName)
        {
            try
            {
                mgr.StartSavingFile(Path.Combine(Properties.Settings.Default.WorkingOrdersPath, saveDirectorPath), fileName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][18-07-2024] [GEOS2-5409] 
        public void SavePartData(string saveDirectorPath, string fileName, byte[] partData)
        {
            try
            {
                mgr.SavePartData(Path.Combine(Properties.Settings.Default.WorkingOrdersPath, saveDirectorPath), fileName, partData);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][19-07-2024] [GEOS2-5409] 
        public List<OTAttachment> GetQualityOTAndItemAttachments(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetQualityOTAndItemAttachments(company, idOT, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][26.07.2024][GEOS2-5473]
        public List<Article> GetComponentArticlesByIdArticle(int idArticle)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetComponentArticlesByIdArticle(idArticle, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][02-08-2024][GEOS2-5410]
        public TechnicalSpecifications GetTechnicalSpecificationForReport(Company company, long idOt)
        {
            try
            {
                return mgr.GetTechnicalSpecificationForReport(company, idOt);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [GEOS2-5410][06-08-2024][nsatpute]
        public List<ModuleReference> GetModuleReferencesForReport(Company company, long idOt)
        {
            try
            {
                return mgr.GetModuleReferencesForReport(company, idOt);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [GEOS2-5410][06-08-2024][nsatpute]
        public List<OTAttachment> GetStructureOTAttachment(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetStructureOTAttachment(company, idOT, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][GEOS2-5410][08-08-2024]
        public List<Article> GetTestboardElectrificationOtArticles(Company company, Int64 idOt)
        {
            try
            {
                return mgr.GetTestboardElectrificationOtArticles(company, idOt);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][12-08-2024][GEOS2-5412]
        public List<OTAttachment> GetItemAttachments(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetItemAttachments(company, idOT, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][13-08-2024][GEOS2-5411]
        public byte[] GetstructureOtPlanolInBytes(Company company, Int64 idOt)
        {
            try
            {
                return mgr.GetstructureOtPlanolInBytes(company, idOt, Properties.Settings.Default.SolidWorksPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][14-08-2024][GEOS2-5411]
        public List<byte[]> GetstructureOtElectricalDiagramsInBytes(Company company, Int64 idOt)
        {
            try
            {
                return mgr.GetstructureOtElectricalDiagramsInBytes(company, idOt, Properties.Settings.Default.ElectricalDiagrams);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][14-08-2024][GEOS2-5411]
        public ProductTypes GetstructureOtProductDetails(Company company, Int64 idOt)
        {
            try
            {
                return mgr.GetstructureOtProductDetails(company, idOt, Properties.Settings.Default.ProductTypeImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		// [nsatpute][21-08-2024][GEOS2-5412]
        public byte[] GenerateAndGetDeclarationOfConformity(Company company, Int64 idOt)
        {
            try
            {
                return mgr.GenerateAndGetDeclarationOfConformity(company, idOt, Properties.Settings.Default.StructureTemplatePath, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][22-08-2024][GEOS2-5412]
        public byte[] GetCustomerLogoById(int idCustomer)
        {
            try
            {
                return mgr.GetCustomerLogoById(idCustomer, Properties.Settings.Default.CustomerImagePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][03-09-2024][GEOS2-5415]
        public QcPassLabelDetails GetQCPassLabelDetails(Company company, long idOt)
        {
            try
            {
                return mgr.GetQCPassLabelDetails(company, idOt);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][12-11-2024][GEOS2-5890]
        public bool UpdateWorkflowStatusInOTQC_V2580(UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateWorkflowStatusInOTQC_V2580(connectionString, IdOT, IdWorkflowStatus, IdUser, LogEntriesByOTList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][13-11-2024][GEOS2-5889]
        public List<Ots> GetPendingWorkorders_V2580(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2580(company, Properties.Settings.Default.UserProfileImage, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public Ots GetStructureWorkOrderByIdOt_V2640(Int64 idOt, Company company)
        {
            try
            {
                return mgr.GetStructureWorkOrderByIdOt_V2640(idOt, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public List<ElectricalDiagram> GetElectricalDiagram()
        {
            try
            {

                string ConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetElectricalDiagram(ConnectionString, Properties.Settings.Default.ElectricalDiagrams);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public byte[] GetElectricalDiagramsFileImageInBytes(string ElectricalDiagramFileName)
        {
            try
            {
                return mgr.GetElectricalDiagramsFileImageInBytes(ElectricalDiagramFileName, Properties.Settings.Default.ElectricalDiagrams);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
       
        // [GEOS2-6728][pallavi.kale][14-04-2025]
		// [nsatpute][13-05-2025][GEOS2-6728]
        public bool AddEditDeleteElectricalDiagramForIdDrawing(List<OtItem> otItems)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string localConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.AddEditDeleteElectricalDiagramForIdDrawing(mainConnectionString, localConnectionString, otItems);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][12.08.2025][GEOS2-9163]
        public List<Ots> GetPendingWorkorders_V2660(Company company)
        {
            try
            {
                return mgr.GetPendingWorkorders_V2660(company, Properties.Settings.Default.UserProfileImage, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [Rahul.Gadhave][GEOS2-8713][Date:03/11/2025]
        public List<Company> GetAllCompaniesDetails_V2680(Int32 idUser)
        {
            List<Company> connectionstrings = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompaniesDetails_V2680(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                //exp.Logger.Log("Get an error in GetAllCompaniesDetails() Method " + exp.ErrorMessage, category: Category.Info, priority: Priority.Low);
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return connectionstrings;
        }
    }
}

