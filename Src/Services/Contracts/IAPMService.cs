using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PLM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAPMService" in both code and config file together.
    [ServiceContract]
    public interface IAPMService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use GetActionPlanDetails_V2560 instead.")]
        List<APMActionPlan> GetActionPlanDetails_V2550(long selectedPeriod);

        // [Shweta.thube] [GEOS2-5971]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupValues_V2550(byte key);

        //[Sudhir.Jangra][GEOS2-5971]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use GetAuthorizedLocationListByIdUser_V2560 instead.")]
        List<Company> GetAuthorizedLocationListByIdUser_V2550(int user);

        //[Sudhir.Jangra][GEOS2-5972]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2570. Use GetAuthorizedLocationListByIdUser_V2570 instead.")]
        List<APMActionPlan> GetActionPlanDetails_V2560(long selectedPeriod);


        //[Sudhir.Jangra][GEOS2-5977]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2570. Use GetEmployeeCurrentDetail_V2570 instead.")]
        Responsible GetEmployeeCurrentDetail_V2560(Int32 idUser, Int64 selectedPeriod);

        //[Sudhir.Jangra][GEOS2-5977]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2570. Use GetActiveInactiveResponsibleForActionPlan_V2570 instead.")]
        List<Responsible> GetActiveInactiveResponsibleForActionPlan_V2560(Int32 IdCompany, long selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission);


        //[Sudhir.Jangra][GEOS2-5977]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Responsible GetInactiveResponsibleForActionPlan_V2560(Int32 IdEmployee);

        //[Sudhir.Jangra][GEOS2-5977]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActionPlan_V2560(APMActionPlan actionPlan);

        //[Sudhir.Jangra][GEOS2-5978]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetActionPlanLatestCode_V2560();

        //[Sudhir.Jangra][GEOS2-5978]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        APMActionPlan AddActionPlanDetails_V2560(APMActionPlan actionPlan);
        //[shweta.thube][GEOS2-5979]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddTaskForActionPlan_V2560(APMActionPlanTask actionPlanTask);

        //[Sudhir.Jangra][GEOS2-6397]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2570. Use GetAuthorizedLocationListByIdUser_V2570 instead.")]
        List<Company> GetAuthorizedLocationListByIdUser_V2560(int user);

        // [shweta.thube][GEOS2-5980]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTaskForActionPlan_V2560(APMActionPlanTask actionPlanTask);

        //[Sudhir.Jangra][GEOS2-6017]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTaskDelegatedTo_V2570(APMActionPlanTask task);

        //[Sudhir.Jangra][GEOS2-6017]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Responsible> GetEmployeeListAsPerLocation_V2570(string IdCompanies);

        //[Sudhir.Jangra][GEOS2-5982]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetActionPlanDetails_V2580 instead.")]
        List<APMActionPlan> GetActionPlanDetails_V2570(string selectedPeriod);

        //[shweta.thube][GEOS2-5981]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteTaskForActionPlan_V2570(Int64 IdActionPlanTask);

        //[shweta.thube][GEOS2-5981]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask AddTaskForActionPlan_V2570(APMActionPlanTask actionPlanTask);

        //[shweta.thube][GEOS2-5981]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTaskForActionPlan_V2570(APMActionPlanTask actionPlanTask);

        //[Sudhir.Jangra][GEOS2-6015]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2610. Use GetTaskCommentsByIdTask_V2610 instead.")]
        List<CommentsByTask> GetTaskCommentsByIdTask_V2570(Int64 idTask);

        //[Sudhir.Jangra][GEOS2-6017]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Responsible> GetActiveInactiveResponsibleForActionPlan_V2570(Int32 IdCompany, string selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission);

        //[Sudhir.Jangra][GEOS2-6017]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Responsible GetInactiveDelegatedToForActionPlanTask_V2570(Int32 IdEmployee);

        //[Sudhir.Jangra][GEOS2-6015]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2610. Use AddUpdateDeleteCommentsByIdTask_V2610 instead.")]
        List<CommentsByTask> AddUpdateDeleteCommentsByIdTask_V2570(List<CommentsByTask> taskCommentsList);

        //[shweta.thube][GEOS2-5976]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use UpdateStatusByIdTask_V2620 instead.")]
        bool UpdateStatusByIdTask_V2570(APMActionPlanTask actionPlanTask, Int32 IdLookupChangedValues, string ChangedTaskComment, Int32 CreatedBy);

        //[Sudhir.Jangra][GEOS2-5976]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Responsible GetEmployeeCurrentDetail_V2570(Int32 idUser, string selectedPeriod);

        //[Sudhir.Jangra][GEOS2-6015]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2570. Use GetAuthorizedLocationListByIdUser_V2690 instead.")]
        List<Company> GetAuthorizedLocationListByIdUser_V2570(int user);

        //[shweta.thube][GEOS2-6020]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByActionPlan> GetLogEntriesByActionPlan_V2580(UInt32 idActionPlan);

        //[shweta.thube][GEOS2-6020]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActionPlan_V2580(APMActionPlan actionPlan);

        //[nsatpute][24-10-2024][GEOS2-6018]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlan AddActionPlanDetails_V2580(APMActionPlan actionPlan);


        //[nsatpute][24-10-2024][GEOS2-6018]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActionPlanComment> GetActionPlanCommentsByIdActionPlan(long idActionPlan);

        //[Sudhir.Jangra][GEOS2-6016]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<AttachmentsByTask> GetTaskAttachmentsByIdTask_V2580(Int64 idTask);

        //[Sudhir.Jangra][GEOS2-6016]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        AttachmentsByTask DownloadTaskAttachment_V2580(AttachmentsByTask taskAttachment);

        //[Sudhir.Jangra][GEOS2-6016]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<AttachmentsByTask> AddUpdateDeleteAttachments_V2580(List<AttachmentsByTask> attachmentList, string GuidString, Int64 idTask);

        //[Sudhir.Jangra][GEOS2-6016]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlan> GetActionPlanDetails_V2580(string selectedPeriod, string idLocations);

        //[Sudhir.Jangra][GEOS2-6016]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2580(Int64 IdActionPlan, string selectionPeriod);

        //[shweta.thube][GEOS2-6020]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask AddTaskForActionPlan_V2580(APMActionPlanTask actionPlanTask);

        //[Shweta.thube][GEOS2-6020]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetEmployeeListAsPerLocation_V2600 instead.")]
        List<Responsible> GetEmployeeListAsPerLocation_V2580(string IdCompanies);

        //[Shweta.thube][GEOS2-6020]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTaskForActionPlan_V2580(APMActionPlanTask actionPlanTask);

        //[shweta.thube][GEOS2-6020]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteTaskForActionPlan_V2580(Int64 IdActionPlanTask, List<LogEntriesByActionPlan> ActionPlanLogEntries);

        #region GEOS2-6019
        //[Sudhir.Jangra][GEOS2-6019]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<AttachmentsByActionPlan> GetActionPlanAttachmentByIdActionPlan_V2580(Int64 idActionPlan);

        //[Sudhir.Jangra][GEOS2-6019]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddActionPlanAttachments_V2580(List<AttachmentsByActionPlan> attachmentList);

        //[Sudhir.jangra][GEOS2-6019]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        AttachmentsByActionPlan DownloadActionPlanAttachment_V2580(AttachmentsByActionPlan actionPlanAttachment);

        //[Sudhir.Jangra][GEOS2-6019]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        bool AddUpdateDeleteAttachmentsForActionPlan_V2580(List<AttachmentsByActionPlan> attachmentList);

        #endregion
        //[Shweta.thube][GEOS2-6591]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetActionPlanDetails_V2600 instead.")]
        List<Responsible> GetResponsibleListAsPerLocation_V2580(string IdCompanies);

        //[shweta.thube][GEOS2-6018]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActionPlan_V2590(APMActionPlan actionPlan);

        //[shweta.thube][GEOS2-6589]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2610. Use GetTaskListByIdActionPlan_V2610 instead.")]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2590(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-6589]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlan> GetActionPlanDetails_V2590(string selectedPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-6585]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use UpdateTaskForActionPlan_V2620 instead.")]
        bool UpdateTaskForActionPlan_V2590(APMActionPlanTask actionPlanTask);

        //[shweta.thube][GEOS2-6585]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask AddTaskForActionPlan_V2590(APMActionPlanTask actionPlanTask);

        //[shweta.thube][GEOS2-6586]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlan AddActionPlanDetails_V2590(APMActionPlan actionPlan);

        //[Sudhir.Jangra][GEOS2-6698]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetDepartmentsForActionPlan_V2590();

        //[Shweta.thube][GEOS2-6696]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VisibilityPerBU> GetEmployeeListWithBU_V2590(Int32 IdCompanies);

        //[Shweta.thube][GEOS2-6696]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEmployeeWithBU_V2590(List<VisibilityPerBU> updateBusinessUnitList);

        //[Sudhir.Jangra][GEOS2-6698]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetUnAuthorizedLocationListByIdCompany_V2590(string idCompanies);

        //[Sudhir.Jangra][GEOS2-6023]
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]

        //List<GlobalReport> GetGlobalReportOpenTasks_V2600(DateTime startDate,DateTime endDate);

        //[Sudhir.Jangra][GEOS2-6787]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Responsible> GetResponsibleListAsPerLocation_V2600(string IdCompanies);

        //[Sudhir.Jangra][GEOS2-6787]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Responsible> GetEmployeeListAsPerLocation_V2600(string IdCompanies);

        //[shweta.thube][GEOS2-6453]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2610. Use GetActionPlanDetails_V2610 instead.")]
        List<APMActionPlan> GetActionPlanDetails_V2600(string selectedPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-6794]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlan AddActionPlanDetails_V2600(APMActionPlan actionPlan);

        //[shweta.thube][GEOS2-6794]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActionPlan_V2600(APMActionPlan actionPlan);

        //[shweta.thube][GEOS2-6795]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteActionPlan_V2600(Int64 IdActionPlan);

        //[Sudhir.Jangra][GEOS2-6453]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2610. Use GetActionPlanTaskDetails_V2610 instead.")]
        List<APMActionPlanTask> GetActionPlanTaskDetails_V2600(string selectedPeriod, Int32 idUser);

        //[Sudhir.Jangra][GEOS2-6616]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CommentsByTask> AddUpdateDeleteCommentsByIdTask_V2610(List<CommentsByTask> taskCommentsList);

        //[Sudhir.Jangra][GEOS2-6616]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CommentsByTask> GetTaskCommentsByIdTask_V2610(Int64 idTask);

        //[Sudhir.Jangra][GEOS2-6616]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlan> GetActionPlanDetails_V2610(string selectedPeriod, Int32 idUser);

        //[Sudhir.Jangra][GEOS2-6616]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2610(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);

        //[Sudhir.Jangra][GEOS2-6616]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetActionPlanTaskDetails_V2610(string selectedPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-6911]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMCustomer> GetCustomersWithSite_V2610();


        //[shweta.thube][GEOS2-6911]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlan AddActionPlanDetails_V2610(APMActionPlan actionPlan);

        //[shweta.thube][GEOS2-6911]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActionPlan_V2610(APMActionPlan actionPlan);

        //[shweta.thube][GEOS2-6911]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMCustomer> GetActiveUserIdRegion_V2610(Int32 IdCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddImportedActionPlanDetails(APMActionPlan actionPlan);

        //[GEOS2-6021][rdixit][17.02.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2680. Use GetActionPlansImportFile_V2680 instead.")]
        byte[] GetActionPlansImportFile();

        //[Sudhir.Jangra][GEOS2-6913]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DelayedTasks GetDelayedTasks_V2610(DateTime startDate, Int32 idCompany);

        //[Sudhir.Jangra][GEOS2-6913]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<OverdueTaskMail> GetOverdueTOTaskMail_V2610(Int32 IdCompany, string To_Ids);

        //[Sudhir.Jangra][GEOS2-6913]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OverdueTaskMail> GetOverdueCCTaskMail_V2610(Int32 IdCompany, string CC_EIds, string cc_Ids);

        //[Sudhir.Jangra][GEOS2-6913]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<OverdueTaskMail> GetResponsibleOverdueTaskMail_V2610(Int32 IdCompany, string To_CC_Ids, DateTime startDate);

        //[Sudhir.Jangra][GEOS2-6913]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string ReadOverdueMailTemplate(string templateName);

        //[Sudhir.Jangra][GEOS2-7006]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTaskForActionPlan_V2620(APMActionPlanTask actionPlanTask);


        //[shweta.thube][GEOS2-6912]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetEmdepSitesCompanies_V2620(Int64 IdSite, Int64 IdResponsibleLocation);

        //[shweta.thube][GEOS2-6912]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTs> GetOTsAsPerTaskLocation_V2620(Int32 IdCompany, DateTime FromDate, DateTime ToDate);

        //[shweta.thube][GEOS2-6912]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask AddTaskForActionPlan_V2620(APMActionPlanTask actionPlanTask);

        //[shweta.thube][GEOS2-6912]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetActionPlanDetails_V2630 instead.")]
        List<APMActionPlan> GetActionPlanDetails_V2620(string selectedPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-6912]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetTaskListByIdActionPlan_V2630 instead.")]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2620(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-6912]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetActionPlanTaskDetails_V2630 instead.")]
        List<APMActionPlanTask> GetActionPlanTaskDetails_V2620(string selectedPeriod, Int32 idUser);

        //[Sudhir.Jangra][GEOS2-7007]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateStatusByIdTask_V2620(APMActionPlanTask actionPlanTask, Int32 IdLookupChangedValues, string ChangedTaskComment, Int32 CreatedBy);


        //[shweta.thube][GEOS2-7008]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Responsible> GetParticipantsByIdTask_V2620(Int64 idTask);

        //[shweta.thube][GEOS2-7008]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Responsible> AddDeleteParticipantsByIdTask_V2620(List<Responsible> tempParticipantsList);

        //[shweta.thube][GEOS2-7008]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Responsible> GetResponsibleListAsPerLocation_V2620(string IdCompanies);

        //[Sudhir.Jangra][GEOS2-7209]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlan> GetActionPlanDetails_V2630(string selectedPeriod, Int32 idUser);

        //[Sudhir.Jangra][GEOS2-7209]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetActionPlanTaskDetails_V2630(string selectedPeriod, Int32 idUser);

        //[Sudhir.Jangra][GEOS2-7209]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2630(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);

        //[rdixit][GEOS2-7883][15.04.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use AddImportedActionPlanDetails_V2670 instead.")]
        bool AddImportedActionPlanDetails_V2630(APMActionPlan actionPlan);

        //[shweta.thube][GEOS2-7212][23.04.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlan> GetActionPlanDetails_V2640(string selectedPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-7212][23.04.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetActionPlanTaskDetails_V2640(string selectedPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-7212][23.04.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2640(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);
        //[shweta.thube][GEOS2-7889]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DelayedTasks GetDelayedTasks_V2640(DateTime startDate, Int32 idCompany);
        //[shweta.thube][GEOS2-8051][15/05/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetTopFiveDelayedTaks_V2640(DateTime startDate, Int32 idCompany);
        //[shweta.thube][GEOS2-8061]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateManualAttachmentSetting_V2640(APMActionPlanTask apmActionPlanTask);
        //[shweta.thube][GEOS2-8061]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask GetManualAttachmentSetting(Int32 idAppSetting);

        //[shweta.thube][GEOS2-8063][27/05/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetResponsibleListForEmailNotification_V2650(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-8066]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask GetCCPersonMail_V2650(Int64 ccPersonEmailID);

        //[shweta.thube][GEOS2-8066]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask GetToPersonMail_V2650(Int64 toPersonEmailID);

        //[shweta.thube][GEOS2-7241][27/05/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<VisibilityPerBU> GetEmployeeListWithBU_V2650(Int32 IdCompanies);

        //[shweta.thube][GEOS2-8069]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlan GetActionPlanInfo_V2650(Int64 IdActionPlan);

        //[shweta.thube][GEOS2-8069]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetActionPlanExcel();

        //[Pallavi.Kale][19/06/2025][GEOS2-8216]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ValidateCustomerBySites(Int32 CustomerId);

        //[pallavi.kale][GEOS2-7002][19.06.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlan> GetActionPlanDetails_V2650(string selectedPeriod, Int32 idUser);

        //[pallavi.kale][GEOS2-7002][19.06.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use GetActionPlanTaskDetails_V2670 instead.")]
        List<APMActionPlanTask> GetActionPlanTaskDetails_V2650(string selectedPeriod, Int32 idUser);

        //[pallavi.kale][GEOS2-7002][19.06.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use GetTaskListByIdActionPlan_V2670 instead.")]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2650(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);
        //[shweta.thube][GEOS2-7004][25.06.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CommentsByTask> GetSubTaskCommentsByIdSubTask_V2650(Int64 idTaskSub);
        //[shweta.thube][GEOS2-7004][25.06.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CommentsByTask> AddUpdateDeleteCommentsByIdSubTask_V2650(List<CommentsByTask> taskCommentsList);
        //[shweta.thube][GEOS2-7004][25.06.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<AttachmentsByTask> GetTaskAttachmentsByIdSubTask_V2650(Int64 idTask);
        //[shweta.thube][GEOS2-7004][25.06.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<AttachmentsByTask> AddDeleteSubTaskAttachments_V2650(List<AttachmentsByTask> attachmentList, string GuidString, Int64 idTask);

        //[pallavi.kale][GEOS2-7003]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanSubTask AddSubTaskForActionPlan_V2650(APMActionPlanSubTask actionPlanTask);

        //[pallavi.kale][GEOS2-7003]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSubTaskForActionPlan_V2650(APMActionPlanSubTask actionPlanTask);

        //[shweta.thube][GEOS2-8683][01.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteSubTaskForActionPlan_V2650(Int64 IdActionPlanTask, List<LogEntriesByActionPlan> ActionPlanLogEntries);

        //[shweta.thube][GEOS2-7005][08.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanSubTask AddSubTaskForActionPlan_V2660(APMActionPlanSubTask actionPlanTask);

        //[shweta.thube][GEOS2-7005][08.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSubTaskForActionPlan_V2660(APMActionPlanSubTask actionPlanTask);

        //[shweta.thube][GEOS2-7218][23.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlan GetActionPlanDetailsByIdActionPlan_V2660(Int64 IdActionPlan);
        //[shweta.thube][GEOS2-8985][18.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetMaxTaskNumberByIdActionPlan_V2660(Int64 IdActionPlan);
        //[shweta.thube][GEOS2-8985][18.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetMaxSubTaskCodeByIdTask_V2660(Int32 IdTask);
        //[shweta.thube][GEOS2-7217][23.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use GetActionPlanDetails_V2670 instead.")]
        List<APMActionPlan> GetActionPlanDetails_V2660(string selectedPeriod, Int32 idUser);

        //[shweta.thube][GEOS2-9237][14.08.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2680. Use GetActionPlanDetails_V2680 instead.")]
        List<APMActionPlan> GetActionPlanDetails_V2670(string selectedPeriod, Int32 idUser);

        //[pallavi.kale][GEOS2-8084][04.08.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2680. Use GetActionPlanTaskDetails_V2680 instead.")]
        List<APMActionPlanTask> GetActionPlanTaskDetails_V2670(string selectedPeriod, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanModern> GetActionPlanDetails_WithCounts(string selectedPeriod, int idUser);

        //[rdixit][GEOS2-9354][01.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Responsible> GetResponsibleListAsPerLocation_V2670(string IdCompanies);

        #region  //[shweta.thube][GEOS2-6048][03.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GlobalMailReport> GetGlobalReportTOTaskMail_V2670(Int32 IdCompany, string To_Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GlobalMailReport> GetGlobalReportCCTaskMail_V2670(Int32 IdCompany, string cc_Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GlobalMailReport> GetGlobalReportCCIdsTaskMail_V2670(string cc_Ids);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GlobalMailReport> GetMonthlyTaskStatusCountByLocation_V2670(DateTime StartDate, string idLocations);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GlobalMailReport> GetMonthlyOpenAndClosedTaskCountByLocation_V2670(DateTime StartDate, string idLocations);
        #endregion

        //[shweta.thube][GEOS2-9354][03.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Responsible> GetParticipantsListAsPerLocation_V2670(string IdCompanies);

        //[pallavi.kale][GEOS2-8084][03.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetEditActionPlanExcel();
        //[shweta.thube][GEOS2-9273][08.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddImportedActionPlanDetails_V2670(APMActionPlan actionPlan);
        //[shweta.thube][GEOS2-9273][08.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateImportedActionPlan_V2670(APMActionPlan actionPlan);
        //[shweta.thube][GEOS2-9273][08.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlan GetActionPlanInfoByIdActionPlan_V2670(Int64 IdActionPlan);

        //[pallavi.kale][GEOS2-8084][08.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetCustomerActionPlanExcel();

        //[pallavi.kale][GEOS2-8084][11.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetAPMTemplateFilePath(string fileName);
        #region  //[shweta.thube][GEOS2-6047][13.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask GetNewMonthlyTasksRegisteredCount_V2670(DateTime startDate, string idLocations);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask GetReportPerPlantOpenTaskCount_V2670(DateTime fromDate, string idLocations);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ReportPerPlant GetReportPerPlantCreatedAndPendingTasks_V2670(DateTime fromDate, string idLocations);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ReportPerPlant> GetReportPerPlantYearlyProgress_V2670(string idLocations);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ReportPerPlant> GetReportPerPlantMonthWithYearProgress_V2670(DateTime startDate, string idLocations);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ReportPerPlant> GetTasksCountByTheme_V2670(DateTime StartDate, string idLocations);
        #endregion

        //[shweta.thube][GEOS2-8695][22.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2680. Use GetTaskListByIdActionPlan_V2680 instead.")]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2670(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetResponsibleListForEmailNotification_V2680(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool APMEmailSend_V2680(string emailto, string sub, string emailbody, Dictionary<string, byte[]> attachments, string EmailFrom, List<string> ccAddress);
        //[Shweta.Thube][GEOS2-9129][09-10-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSendDateTime_V2680(APMActionPlanTask apmActionPlanTask);       
        

        
        //[pallavi.kale][GEOS2-8993][17.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetActionPlansImportFile_V2680(out DateTime? lastModifiedDate);
       
        //[pallavi.kale][GEOS2-8995][28.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlan> GetActionPlanDetails_V2680(string selectedPeriod, Int32 idUser);

        //[pallavi.kale][GEOS2-9000][28.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetActionPlanTaskDetails_V2680(string selectedPeriod, Int32 idUser);
       
        //[pallavi.kale][GEOS2-8997][28.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2680(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);
        //[Shweta.Thube][GEOS2-8058][30-10-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OverdueTaskMail> GetRegionalEmployeelist_V2680(string IdCompany, string CC_EIds, string cc_Ids);
        //[Shweta.Thube][GEOS2-8058][30-10-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OverdueTaskMail> GetRegionlistwithCompanies_V2680();
        //[shweta.thube][GEOS2-9812][06-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OverdueTaskMail> GetPriorityDelayedSettingExceptionMailIDs_V2680(Int32 IdCompany, string CC_EIds);
        //[shweta.thube] [GEOS2-9868][07-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlan> GetActionPlanDetails_V2690(string selectedPeriod, Int32 idUser);
        //[shweta.thube] [GEOS2-9868][07-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetActionPlanTaskDetails_V2690(string selectedPeriod, Int32 idUser);
        //[shweta.thube] [GEOS2-9868][07-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<APMActionPlanTask> GetTaskListByIdActionPlan_V2690(Int64 IdActionPlan, string selectionPeriod, Int32 idUser);
        //[shweta.thube] [GEOS2-9870][19-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAuthorizedLocationListByIdUser_V2690(int user);
        //[shweta.thube] [GEOS2-9870][19-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        APMActionPlanTask AddTaskForActionPlan_V2690(APMActionPlanTask actionPlanTask);
        //[shweta.thube] [GEOS2-9870][19-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTaskForActionPlan_V2690(APMActionPlanTask actionPlanTask);
        //[shweta.thube][GEOS2-9547][27-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetUnAuthorizedLocationListByIdCompany_V2690(string idCompanies);
    }
}
