using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "APMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select APMService.svc or APMService.svc.cs at the Solution Explorer and start debugging.
    public class APMService : IAPMService
    {
        public List<APMActionPlan> GetActionPlanDetails_V2550(long selectedPeriod)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2550(connectionString, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [Shweta.thube] [GEOS2-5971]
        public IList<LookupValue> GetLookupValues_V2550(byte key)
        {
            IList<LookupValue> list = null;
            try
            {
                CrmManager mgr = new CrmManager();
                list = mgr.GetLookupValues(key);
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

        //[Sudhir.Jangra][GEOS2-5971]
        public List<Company> GetAuthorizedLocationListByIdUser_V2550(int idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAuthorizedLocationListByIdUser_V2550(connectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5972]
        public List<APMActionPlan> GetActionPlanDetails_V2560(long selectedPeriod)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2560(connectionString, selectedPeriod, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5977]

        public Responsible GetEmployeeCurrentDetail_V2560(Int32 idUser, Int64 selectedPeriod)
        {
            try
            {
                APMManager mgr = new APMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeCurrentDetail_V2560(workbenchConnectionString, idUser, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5977]
        public List<Responsible> GetActiveInactiveResponsibleForActionPlan_V2560(Int32 IdCompany, long selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission)
        {
            try
            {
                APMManager mgr = new APMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActiveInactiveResponsibleForActionPlan_V2560(workbenchConnectionString, IdCompany, selectedPeriod, idsOrganization, idsDepartments, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5977]
        public Responsible GetInactiveResponsibleForActionPlan_V2560(Int32 IdEmployee)
        {
            try
            {
                APMManager mgr = new APMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetInactiveResponsibleForActionPlan_V2560(workbenchConnectionString, IdEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5977]
        public bool UpdateActionPlan_V2560(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateActionPlan_V2560(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        //[Sudhir.Jangra][GEOS2-5978]
        public string GetActionPlanLatestCode_V2560()
        {
            try
            {
                APMManager mgr = new APMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanLatestCode_V2560(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5977]
        public APMActionPlan AddActionPlanDetails_V2560(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddActionPlanDetails_V2560(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-5979]
        public bool AddTaskForActionPlan_V2560(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTaskForActionPlan_V2560(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6397]
        public List<Company> GetAuthorizedLocationListByIdUser_V2560(int idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAuthorizedLocationListByIdUser_V2560(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [shweta.thube][GEOS2-5980]
        public bool UpdateTaskForActionPlan_V2560(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateTaskForActionPlan_V2560(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-6017]
        public bool UpdateTaskDelegatedTo_V2570(APMActionPlanTask task)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateTaskDelegatedTo_V2570(ConnectionString, task);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6017]
        public List<Responsible> GetEmployeeListAsPerLocation_V2570(string IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeListAsPerLocation_V2570(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5982]
        public List<APMActionPlan> GetActionPlanDetails_V2570(string selectedPeriod)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2570(connectionString, selectedPeriod, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-5981]
        public bool DeleteTaskForActionPlan_V2570(Int64 IdActionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.DeleteTaskForActionPlan_V2570(IdActionPlanTask, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-5981]
        public APMActionPlanTask AddTaskForActionPlan_V2570(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTaskForActionPlan_V2570(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [shweta.thube][GEOS2-5981]
        public bool UpdateTaskForActionPlan_V2570(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateTaskForActionPlan_V2570(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6015]
        public List<CommentsByTask> GetTaskCommentsByIdTask_V2570(Int64 idTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskCommentsByIdTask_V2570(connectionString, idTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6017]
        public List<Responsible> GetActiveInactiveResponsibleForActionPlan_V2570(Int32 IdCompany, string selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission)
        {
            try
            {
                APMManager mgr = new APMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActiveInactiveResponsibleForActionPlan_V2570(workbenchConnectionString, IdCompany, selectedPeriod, idsOrganization, idsDepartments, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6017]
        public Responsible GetInactiveDelegatedToForActionPlanTask_V2570(Int32 IdEmployee)
        {
            try
            {
                APMManager mgr = new APMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetInactiveDelegatedToForActionPlanTask_V2570(workbenchConnectionString, IdEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6015]
        public List<CommentsByTask> AddUpdateDeleteCommentsByIdTask_V2570(List<CommentsByTask> taskCommentsList)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddUpdateDeleteCommentsByIdTask_V2570(ConnectionString, taskCommentsList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-5976]
        public bool UpdateStatusByIdTask_V2570(APMActionPlanTask actionPlanTask, Int32 IdLookupChangedValues, string ChangedTaskComment, Int32 CreatedBy)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateStatusByIdTask_V2570(ConnectionString, actionPlanTask, IdLookupChangedValues, ChangedTaskComment, CreatedBy);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5976]
        public Responsible GetEmployeeCurrentDetail_V2570(Int32 idUser, string selectedPeriod)
        {
            try
            {
                APMManager mgr = new APMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeCurrentDetail_V2570(workbenchConnectionString, idUser, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6015]
        public List<Company> GetAuthorizedLocationListByIdUser_V2570(int idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAuthorizedLocationListByIdUser_V2570(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6020]
        public List<LogEntriesByActionPlan> GetLogEntriesByActionPlan_V2580(UInt32 idActionPlan)
        {
            List<LogEntriesByActionPlan> changeLog = new List<LogEntriesByActionPlan>();
            try
            {
                APMManager mgr = new APMManager();
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                changeLog = mgr.GetLogEntriesByActionPlan_V2580(WorkbenchConnectionString, idActionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return changeLog;
        }

        //[shweta.thube][GEOS2-6020]
        public bool UpdateActionPlan_V2580(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateActionPlan_V2580(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][24-10-2024][GEOS2-6018]
        public APMActionPlan AddActionPlanDetails_V2580(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddActionPlanDetails_V2580(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][25-10-2024][GEOS2-6018]
        public List<ActionPlanComment> GetActionPlanCommentsByIdActionPlan(long idActionPlan)
        {

            try
            {
                APMManager mgr = new APMManager();
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanCommentsByIdActionPlan(WorkbenchConnectionString, idActionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-6016]
        public List<AttachmentsByTask> GetTaskAttachmentsByIdTask_V2580(Int64 idTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskAttachmentsByIdTask_V2580(connectionString, idTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6016]
        public AttachmentsByTask DownloadTaskAttachment_V2580(AttachmentsByTask taskAttachment)
        {
            AttachmentsByTask downloadedTaskAttachment = null;

            try
            {
                APMManager mgr = new APMManager();

                downloadedTaskAttachment = mgr.DownloadTaskAttachment_V2580(taskAttachment, Properties.Settings.Default.APMActionPlanTaskAttachments);
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return downloadedTaskAttachment;
        }

        //[Sudhir.Jangra][GEOS2-6016]
        public List<AttachmentsByTask> AddUpdateDeleteAttachments_V2580(List<AttachmentsByTask> attachmentList, string GuidString, Int64 idTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddUpdateDeleteAttachments_V2580(ConnectionString, attachmentList, GuidString, Properties.Settings.Default.APMActionPlanTaskAttachments, idTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6016]
        public List<APMActionPlan> GetActionPlanDetails_V2580(string selectedPeriod, string idLocations)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2580(connectionString, selectedPeriod, idLocations, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6016]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2580(Int64 IdActionPlan, string selectedPeriod)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2580(connectionString, IdActionPlan, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-6020]
        public APMActionPlanTask AddTaskForActionPlan_V2580(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTaskForActionPlan_V2580(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Shweta.thube][GEOS2-6020]
        public List<Responsible> GetEmployeeListAsPerLocation_V2580(string IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeListAsPerLocation_V2580(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Shweta.thube][GEOS2-6020]
        public bool UpdateTaskForActionPlan_V2580(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateTaskForActionPlan_V2580(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6020]
        public bool DeleteTaskForActionPlan_V2580(Int64 IdActionPlanTask, List<LogEntriesByActionPlan> ActionPlanLogEntries)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.DeleteTaskForActionPlan_V2580(connectionString, IdActionPlanTask, ActionPlanLogEntries);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-6019
        //[Sudhir.Jangra][GEOS2-6019]
        public List<AttachmentsByActionPlan> GetActionPlanAttachmentByIdActionPlan_V2580(Int64 IdActionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanAttachmentByIdActionPlan_V2580(connectionString, IdActionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6019]
        public bool AddActionPlanAttachments_V2580(List<AttachmentsByActionPlan> attachmentList)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddActionPlanAttachments_V2580(ConnectionString, attachmentList, Properties.Settings.Default.APMActionPlanAttachments);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6019]
        public AttachmentsByActionPlan DownloadActionPlanAttachment_V2580(AttachmentsByActionPlan actionPlanAttachment)
        {
            AttachmentsByActionPlan downloadedTaskAttachment = null;

            try
            {
                APMManager mgr = new APMManager();

                downloadedTaskAttachment = mgr.DownloadActionPlanAttachment_V2580(actionPlanAttachment, Properties.Settings.Default.APMActionPlanAttachments);
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return downloadedTaskAttachment;
        }

        //[Sudhir.Jangra][GEOS2-6019]
        public bool AddUpdateDeleteAttachmentsForActionPlan_V2580(List<AttachmentsByActionPlan> attachmentList)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddUpdateDeleteAttachmentsForActionPlan_V2580(ConnectionString, attachmentList, Properties.Settings.Default.APMActionPlanAttachments);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion
        //[Shweta.thube][GEOS2-6591]
        public List<Responsible> GetResponsibleListAsPerLocation_V2580(string IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetResponsibleListAsPerLocation_V2580(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-6018]
        public bool UpdateActionPlan_V2590(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateActionPlan_V2590(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-6589]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2590(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2590(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6589]
        public List<APMActionPlan> GetActionPlanDetails_V2590(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2590(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6585]
        public bool UpdateTaskForActionPlan_V2590(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateTaskForActionPlan_V2590(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6585]
        public APMActionPlanTask AddTaskForActionPlan_V2590(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTaskForActionPlan_V2590(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-6586]
        public APMActionPlan AddActionPlanDetails_V2590(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddActionPlanDetails_V2590(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6698]
        public List<Department> GetDepartmentsForActionPlan_V2590()
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetDepartmentsForActionPlan_V2590(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Shweta.thube][GEOS2-6696]
        public List<VisibilityPerBU> GetEmployeeListWithBU_V2590(Int32 IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeListWithBU_V2590(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Shweta.thube][GEOS2-6696]
        public bool AddEmployeeWithBU_V2590(List<VisibilityPerBU> updateBusinessUnitList)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeWithBU_V2590(ConnectionString, updateBusinessUnitList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6698]
        public List<Company> GetUnAuthorizedLocationListByIdCompany_V2590(string idCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUnAuthorizedLocationListByIdCompany_V2590(connectionString, idCompanies, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6023]
        //public List<GlobalReport> GetGlobalReportOpenTasks_V2600(DateTime startDate, DateTime endDate)
        //{
        //    try
        //    {
        //        APMManager mgr = new APMManager();
        //        string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
        //        return mgr.GetGlobalReportOpenTasks_V2600(connectionString, startDate, endDate);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}

        //[Sudhir.Jangra][GEOS2-6787]
        public List<Responsible> GetResponsibleListAsPerLocation_V2600(string IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetResponsibleListAsPerLocation_V2600(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6787]
        public List<Responsible> GetEmployeeListAsPerLocation_V2600(string IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeListAsPerLocation_V2600(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6453]
        public List<APMActionPlan> GetActionPlanDetails_V2600(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2600(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6794]
        public APMActionPlan AddActionPlanDetails_V2600(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddActionPlanDetails_V2600(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6794]
        public bool UpdateActionPlan_V2600(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateActionPlan_V2600(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-6795]
        public bool DeleteActionPlan_V2600(Int64 IdActionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.DeleteActionPlan_V2600(connectionString, IdActionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6453]
        public List<APMActionPlanTask> GetActionPlanTaskDetails_V2600(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanTaskDetails_V2600(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6616]
        public List<CommentsByTask> AddUpdateDeleteCommentsByIdTask_V2610(List<CommentsByTask> taskCommentsList)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddUpdateDeleteCommentsByIdTask_V2610(ConnectionString, taskCommentsList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-6616]
        public List<CommentsByTask> GetTaskCommentsByIdTask_V2610(Int64 idTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskCommentsByIdTask_V2610(connectionString, idTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6616]
        public List<APMActionPlan> GetActionPlanDetails_V2610(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2610(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6616]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2610(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2610(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6616]
        public List<APMActionPlanTask> GetActionPlanTaskDetails_V2610(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanTaskDetails_V2610(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6911]
        public List<APMCustomer> GetCustomersWithSite_V2610()
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCustomersWithSite_V2610(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-6911]
        public APMActionPlan AddActionPlanDetails_V2610(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddActionPlanDetails_V2610(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-6911]
        public bool UpdateActionPlan_V2610(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateActionPlan_V2610(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6911]
        public List<APMCustomer> GetActiveUserIdRegion_V2610(Int32 IdCompany)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActiveUserIdRegion_V2610(connectionString, IdCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-6021][rdixit][17.02.2025]
        public bool AddImportedActionPlanDetails(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddImportedActionPlanDetails(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-6021][rdixit][17.02.2025]
        public byte[] GetActionPlansImportFile()
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;

            fileUploadPath = Properties.Settings.Default.ActionPlansImportFilePath;
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (FileStream stream = new FileStream(fileUploadPath, FileMode.Open, FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6913]
        public DelayedTasks GetDelayedTasks_V2610(DateTime startDate, Int32 idCompany)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetDelayedTasks_V2610(connectionString, startDate, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6913]
        public List<OverdueTaskMail> GetOverdueTOTaskMail_V2610(Int32 IdCompany, string To_Ids)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOverdueTOTaskMail_V2610(connectionString, IdCompany, To_Ids);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-6913]
        public List<OverdueTaskMail> GetOverdueCCTaskMail_V2610(Int32 IdCompany, string To_CC_Ids, string cc_Ids)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOverdueCCTaskMail_V2610(connectionString, IdCompany, To_CC_Ids, cc_Ids);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6913]
        public List<OverdueTaskMail> GetResponsibleOverdueTaskMail_V2610(Int32 IdCompany, string To_CC_Ids, DateTime startDate)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetResponsibleOverdueTaskMail_V2610(connectionString, IdCompany, To_CC_Ids, startDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6913]
        public string ReadOverdueMailTemplate(string templateName)
        {
            try
            {
                return System.IO.File.ReadAllText(string.Format(@"{0}\{1}", Properties.Settings.Default.EmailTemplate, templateName));
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-7006]
        public bool UpdateTaskForActionPlan_V2620(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateTaskForActionPlan_V2620(ConnectionString, actionPlanTask, Properties.Settings.Default.APMActionPlanTaskAttachments);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-6912]
        public List<Company> GetEmdepSitesCompanies_V2620(Int64 IdSite, Int64 IdResponsibleLocation)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEmdepSitesCompanies_V2620(connectionString, IdSite, IdResponsibleLocation);

            }
            catch (Exception ex)
            {
                APMManager mgr = new APMManager();
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist_V2620("LocalGeosContext") == false)
                {
                    exp.ErrorMessage = "LocalGeosContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }
        //[shweta.thube][GEOS2-6912]
        public List<OTs> GetOTsAsPerTaskLocation_V2620(Int32 IdCompany, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["PCMContext"].ConnectionString;
                return mgr.GetOTsAsPerTaskLocation_V2620(IdCompany, connectionString, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6912]
        public APMActionPlanTask AddTaskForActionPlan_V2620(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTaskForActionPlan_V2620(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-6912]
        public List<APMActionPlan> GetActionPlanDetails_V2620(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2620(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6912]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2620(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2620(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-6912]
        public List<APMActionPlanTask> GetActionPlanTaskDetails_V2620(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanTaskDetails_V2620(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public bool UpdateStatusByIdTask_V2620(APMActionPlanTask actionPlanTask, Int32 IdLookupChangedValues, string ChangedTaskComment, Int32 CreatedBy)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateStatusByIdTask_V2620(ConnectionString, actionPlanTask, IdLookupChangedValues, ChangedTaskComment, CreatedBy, Properties.Settings.Default.APMActionPlanTaskAttachments);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-7008]
        public List<Responsible> GetParticipantsByIdTask_V2620(Int64 idTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetParticipantsByIdTask_V2620(connectionString, idTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7008]
        public List<Responsible> AddDeleteParticipantsByIdTask_V2620(List<Responsible> tempParticipantsList)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddDeleteParticipantsByIdTask_V2620(ConnectionString, tempParticipantsList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7008]
        public List<Responsible> GetResponsibleListAsPerLocation_V2620(string IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetResponsibleListAsPerLocation_V2620(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-7209]
        public List<APMActionPlan> GetActionPlanDetails_V2630(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2630(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-7209]
        public List<APMActionPlanTask> GetActionPlanTaskDetails_V2630(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanTaskDetails_V2630(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-7209]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2630(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2630(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-7883][15.04.2025]
        public bool AddImportedActionPlanDetails_V2630(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddImportedActionPlanDetails_V2630(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-7212][23.04.2025]
        public List<APMActionPlan> GetActionPlanDetails_V2640(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2640(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-7212][23.04.2025]
        public List<APMActionPlanTask> GetActionPlanTaskDetails_V2640(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanTaskDetails_V2640(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-7212][23.04.2025]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2640(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2640(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7889]
        public DelayedTasks GetDelayedTasks_V2640(DateTime startDate, Int32 idCompany)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetDelayedTasks_V2640(connectionString, startDate, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8051][15/05/2025]
        public List<APMActionPlanTask> GetTopFiveDelayedTaks_V2640(DateTime startDate, Int32 idCompany)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTopFiveDelayedTaks_V2640(connectionString, startDate, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8061]
        public bool UpdateManualAttachmentSetting_V2640(APMActionPlanTask apmActionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateManualAttachmentSetting_V2640(ConnectionString, apmActionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8061]
        public APMActionPlanTask GetManualAttachmentSetting(Int32 idAppSetting)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetManualAttachmentSetting(connectionString, idAppSetting);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-8063][27/05/2025]
        public List<APMActionPlanTask> GetResponsibleListForEmailNotification_V2650(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetResponsibleListForEmailNotification_V2650(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8066]
        public APMActionPlanTask GetCCPersonMail_V2650(Int64 ccPersonEmailID)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCCPersonMail_V2650(ConnectionString, ccPersonEmailID);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8066]
        public APMActionPlanTask GetToPersonMail_V2650(Int64 toPersonEmailID)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetToPersonMail_V2650(ConnectionString, toPersonEmailID);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7241][27/05/2025]
        public List<VisibilityPerBU> GetEmployeeListWithBU_V2650(Int32 IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeListWithBU_V2650(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8069]
        public APMActionPlan GetActionPlanInfo_V2650(Int64 IdActionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanInfo_V2650(ConnectionString, IdActionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8069]
        public byte[] GetActionPlanExcel()
        {
            byte[] bytes = null;
            string defaultFileName = "APMExcelTemplate.xlsx";

            //string companyFileName = string.Format("{0}{1}",  "OrgChart.xlsm");

            try
            {
                if (File.Exists(string.Format("{0}{1}", Properties.Settings.Default.EmailTemplate, defaultFileName)))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}{1}", Properties.Settings.Default.EmailTemplate, defaultFileName), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }


                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Pallavi.Kale][19/06/2025][GEOS2-8216]
        public bool ValidateCustomerBySites(Int32 CustomerId)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.ValidateCustomerBySites(ConnectionString, CustomerId);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi.kale][GEOS2-7002][19.06.2025]
        public List<APMActionPlan> GetActionPlanDetails_V2650(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2650(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi.kale][GEOS2-7002][19.06.2025]
        public List<APMActionPlanTask> GetActionPlanTaskDetails_V2650(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanTaskDetails_V2650(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi.kale][GEOS2-7002][19.06.2025]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2650(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2650(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<CommentsByTask> GetSubTaskCommentsByIdSubTask_V2650(Int64 idSubTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSubTaskCommentsByIdSubTask_V2650(connectionString, idSubTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7004][25.06.2025]
        public List<CommentsByTask> AddUpdateDeleteCommentsByIdSubTask_V2650(List<CommentsByTask> taskCommentsList)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddUpdateDeleteCommentsByIdSubTask_V2650(ConnectionString, taskCommentsList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7004][25.06.2025]
        public List<AttachmentsByTask> GetTaskAttachmentsByIdSubTask_V2650(Int64 idTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskAttachmentsByIdSubTask_V2650(connectionString, idTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7004][25.06.2025]
        public List<AttachmentsByTask> AddDeleteSubTaskAttachments_V2650(List<AttachmentsByTask> attachmentList, string GuidString, Int64 idTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddDeleteSubTaskAttachments_V2650(ConnectionString, attachmentList, GuidString, Properties.Settings.Default.APMActionPlanTaskAttachments, idTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi.kale][GEOS2-7003]
        public APMActionPlanSubTask AddSubTaskForActionPlan_V2650(APMActionPlanSubTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddSubTaskForActionPlan_V2650(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[pallavi.kale][GEOS2-7003]
        public bool UpdateSubTaskForActionPlan_V2650(APMActionPlanSubTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateSubTaskForActionPlan_V2650(ConnectionString, actionPlanTask, Properties.Settings.Default.APMActionPlanTaskAttachments);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8683][01.07.2025]
        public bool DeleteSubTaskForActionPlan_V2650(Int64 IdActionPlanTask, List<LogEntriesByActionPlan> ActionPlanLogEntries)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.DeleteSubTaskForActionPlan_V2650(connectionString, IdActionPlanTask, ActionPlanLogEntries);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7005][08.07.2025]
        public APMActionPlanSubTask AddSubTaskForActionPlan_V2660(APMActionPlanSubTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddSubTaskForActionPlan_V2660(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7005][08.07.2025]
        public bool UpdateSubTaskForActionPlan_V2660(APMActionPlanSubTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateSubTaskForActionPlan_V2660(ConnectionString, actionPlanTask, Properties.Settings.Default.APMActionPlanTaskAttachments);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7218][23.07.2025]
        public APMActionPlan GetActionPlanDetailsByIdActionPlan_V2660(Int64 IdActionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetailsByIdActionPlan_V2660(ConnectionString, IdActionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8985][18.07.2025]
        public string GetMaxTaskNumberByIdActionPlan_V2660(Int64 IdActionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMaxTaskNumberByIdActionPlan_V2660(workbenchConnectionString, IdActionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-8985][18.07.2025]
        public string GetMaxSubTaskCodeByIdTask_V2660(Int32 IdTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMaxSubTaskCodeByIdTask_V2660(workbenchConnectionString, IdTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7217][23.07.2025]
        public List<APMActionPlan> GetActionPlanDetails_V2660(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2660(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[shweta.thube][GEOS2-9237][14.08.2025]
        public List<APMActionPlan> GetActionPlanDetails_V2670(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2670(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi.kale][GEOS2-8084][04.08.2025]
        public List<APMActionPlanTask> GetActionPlanTaskDetails_V2670(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanTaskDetails_V2670(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-9354][01.09.2025]
        public List<Responsible> GetResponsibleListAsPerLocation_V2670(string IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetResponsibleListAsPerLocation_V2670(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #region  //[shweta.thube][GEOS2-6048][03.09.2025]
        public List<GlobalMailReport> GetGlobalReportTOTaskMail_V2670(Int32 IdCompany, string To_Ids)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetGlobalReportTOTaskMail_V2670(connectionString, IdCompany, To_Ids);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<GlobalMailReport> GetGlobalReportCCTaskMail_V2670(Int32 IdCompany, string cc_Ids)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetGlobalReportCCTaskMail_V2670(connectionString, IdCompany, cc_Ids);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<GlobalMailReport> GetGlobalReportCCIdsTaskMail_V2670(string cc_Ids)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetGlobalReportCCIdsTaskMail_V2670(connectionString, cc_Ids);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<GlobalMailReport> GetMonthlyTaskStatusCountByLocation_V2670(DateTime StartDate, string idLocations)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyTaskStatusCountByLocation_V2670(connectionString, StartDate, idLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<GlobalMailReport> GetMonthlyOpenAndClosedTaskCountByLocation_V2670(DateTime StartDate, string idLocations)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetMonthlyOpenAndClosedTaskCountByLocation_V2670(connectionString, StartDate, idLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion
        //[shweta.thube][GEOS2-9354][03.09.2025]
        public List<Responsible> GetParticipantsListAsPerLocation_V2670(string IdCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetParticipantsListAsPerLocation_V2670(connectionString, IdCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi.kale][GEOS2-8084][03.09.2025]
        public byte[] GetEditActionPlanExcel()
        {
            byte[] bytes = null;
            string defaultFileName = "APMStandardPrintTemplate.xlsx";

            try
            {
                if (File.Exists(string.Format("{0}{1}", Properties.Settings.Default.EmailTemplate, defaultFileName)))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}{1}", Properties.Settings.Default.EmailTemplate, defaultFileName), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }


                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-9273][08.09.2025]
        public bool AddImportedActionPlanDetails_V2670(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddImportedActionPlanDetails_V2670(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-9273][08.09.2025]
        public bool UpdateImportedActionPlan_V2670(APMActionPlan actionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateImportedActionPlan_V2670(ConnectionString, actionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-9273][08.09.2025]
        public APMActionPlan GetActionPlanInfoByIdActionPlan_V2670(Int64 IdActionPlan)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanInfoByIdActionPlan_V2670(ConnectionString, IdActionPlan);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi.kale][GEOS2-8084][08.09.2025]
        public byte[] GetCustomerActionPlanExcel()
        {
            byte[] bytes = null;
            string defaultFileName = "APMCustomerExcelTemplate.xlsx";

            try
            {
                if (File.Exists(string.Format("{0}{1}", Properties.Settings.Default.EmailTemplate, defaultFileName)))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}{1}", Properties.Settings.Default.EmailTemplate, defaultFileName), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }


                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[pallavi.kale][GEOS2-8084][11.09.2025]
        public string GetAPMTemplateFilePath(string fileName)
        {
            try
            {
                string templatePath = Path.Combine(Properties.Settings.Default.EmailTemplate, fileName);
                return templatePath;
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #region  //[shweta.thube][GEOS2-6047][13.09.2025]
        public APMActionPlanTask GetNewMonthlyTasksRegisteredCount_V2670(DateTime startDate, string idLocations)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetNewMonthlyTasksRegisteredCount_V2670(connectionString, startDate, idLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public APMActionPlanTask GetReportPerPlantOpenTaskCount_V2670(DateTime fromDate, string idLocations)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetReportPerPlantOpenTaskCount_V2670(connectionString, fromDate, idLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ReportPerPlant GetReportPerPlantCreatedAndPendingTasks_V2670(DateTime fromDate, string idLocations)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetReportPerPlantCreatedAndPendingTasks_V2670(connectionString, fromDate, idLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<ReportPerPlant> GetReportPerPlantYearlyProgress_V2670(string idLocations)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetReportPerPlantYearlyProgress_V2670(connectionString, idLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ReportPerPlant> GetReportPerPlantMonthWithYearProgress_V2670(DateTime startDate, string idLocations)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetReportPerPlantMonthWithYearProgress_V2670(connectionString, startDate, idLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ReportPerPlant> GetTasksCountByTheme_V2670(DateTime StartDate, string idLocations)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTasksCountByTheme_V2670(connectionString, StartDate, idLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion
        //[shweta.thube][GEOS2-8695][22.09.2025]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2670(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2670(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        public List<APMActionPlanTask> GetResponsibleListForEmailNotification_V2680(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetResponsibleListForEmailNotification_V2680(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        public bool APMEmailSend_V2680(string emailto, string sub, string emailbody, Dictionary<string, byte[]> attachments, string EmailFrom, List<string> ccAddress)
        {
            try
            {
                APMManager mgr = new APMManager();
                return mgr.APMEmailSend_V2680(emailto, sub, emailbody, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, attachments, EmailFrom, ccAddress);
            }
            catch (Exception ex)
            {
                APMManager mgr = new APMManager();
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        public bool UpdateSendDateTime_V2680(APMActionPlanTask apmActionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateSendDateTime_V2680(ConnectionString, apmActionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }       

        //[pallavi.kale][GEOS2-8993][17.10.2025]
        public byte[] GetActionPlansImportFile_V2680(out DateTime? lastModifiedDate)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;
            fileUploadPath = Properties.Settings.Default.ActionPlansImportFilePath; 
            lastModifiedDate =null;
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    lastModifiedDate = File.GetLastWriteTime(fileUploadPath);
                    using (FileStream stream = new FileStream(fileUploadPath, FileMode.Open, FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
       
        //[pallavi.kale][GEOS2-8995][28.10.2025]
        public List<APMActionPlan> GetActionPlanDetails_V2680(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2680(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<APMActionPlanModern> GetActionPlanDetails_WithCounts(string selectedPeriod, int idUser, string filterAlert = null, string filterTheme = null)
        {
            try
            {
                string connString = GetConnectionString();

                string iconPath = GetCountryIconPath();

                return new APMManager().GetActionPlanDetails_WithCounts(connString, selectedPeriod, idUser, iconPath, filterAlert, filterTheme);
            }
            catch (Exception ex)
            {
                var fault = new ServiceException();
                throw new FaultException<ServiceException>(fault, new FaultReason(ex.Message));
            }
        }
        private string GetConnectionString()
        {
            // CORREÇÃO: Atualizado para o IP 10.13.64.6 (Slave/Correto)
            return "Data Source=10.13.64.6;Database=emdep_geos;User ID=GeosUsr;Password=Geos@123;Convert Zero Datetime=True;";
        }
        private string GetCountryIconPath()
        {
            // Retorna o caminho físico da pasta de imagens no servidor
            return System.Web.Hosting.HostingEnvironment.MapPath("~/Images/Countries/");
        }

        //[pallavi.kale][GEOS2-9000][28.10.2025]
        public List<APMActionPlanTask> GetActionPlanTaskDetails_V2680(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanTaskDetails_V2680(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        //[pallavi.kale][GEOS2-8997][28.10.2025]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2680(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2680(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Shweta.Thube][GEOS2-8058][30-10-2025]
        public List<OverdueTaskMail> GetRegionalEmployeelist_V2680(string IdCompany, string To_CC_Ids, string cc_Ids)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetRegionalEmployeelist_V2680(connectionString, IdCompany, To_CC_Ids, cc_Ids);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Shweta.Thube][GEOS2-8058][30-10-2025]
        public List<OverdueTaskMail> GetRegionlistwithCompanies_V2680()
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetRegionlistwithCompanies_V2680(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][06-11-2025][GEOS2-9812]
        public List<OverdueTaskMail> GetPriorityDelayedSettingExceptionMailIDs_V2680(Int32 IdCompany, string To_CC_Ids)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPriorityDelayedSettingExceptionMailIDs_V2680(connectionString, IdCompany, To_CC_Ids);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube] [GEOS2-9868][07-11-2025]
        public List<APMActionPlan> GetActionPlanDetails_V2690(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanDetails_V2690(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube] [GEOS2-9868][19-11-2025]
        public List<APMActionPlanTask> GetActionPlanTaskDetails_V2690(string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActionPlanTaskDetails_V2690(connectionString, selectedPeriod, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube] [GEOS2-9868][19-11-2025]
        public List<APMActionPlanTask> GetTaskListByIdActionPlan_V2690(Int64 IdActionPlan, string selectedPeriod, Int32 idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTaskListByIdActionPlan_V2690(connectionString, IdActionPlan, selectedPeriod, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube] [GEOS2-9870][19-11-2025]
        public List<Company> GetAuthorizedLocationListByIdUser_V2690(int idUser)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAuthorizedLocationListByIdUser_V2690(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube] [GEOS2-9870][19-11-2025]
        public APMActionPlanTask AddTaskForActionPlan_V2690(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTaskForActionPlan_V2690(ConnectionString, actionPlanTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube] [GEOS2-9870][19-11-2025]
        public bool UpdateTaskForActionPlan_V2690(APMActionPlanTask actionPlanTask)
        {
            try
            {
                APMManager mgr = new APMManager();
                string ConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateTaskForActionPlan_V2690(ConnectionString, actionPlanTask, Properties.Settings.Default.APMActionPlanTaskAttachments);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-9547][27-11-2025]
        public List<Company> GetUnAuthorizedLocationListByIdCompany_V2690(string idCompanies)
        {
            try
            {
                APMManager mgr = new APMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUnAuthorizedLocationListByIdCompany_V2690(connectionString, idCompanies, Properties.Settings.Default.CountryFilePath);
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
