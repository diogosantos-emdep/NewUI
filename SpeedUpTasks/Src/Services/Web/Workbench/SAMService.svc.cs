using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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
                return mgr.GetOTWorkingTimeDetails(idOT,company, Properties.Settings.Default.UserProfileImage);
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

        public List<OTAttachment> GetOTAttachment(Company company,Int64 idOT)
        {
            try
            {
                return mgr.GetOTAttachment(company, idOT,Properties.Settings.Default.WorkingOrdersPath);
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
    }
}

