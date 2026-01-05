using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEpcService" in both code and config file together.
    [ServiceContract]
    public interface IEpcService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupValues(byte lookupKey);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Product> GetProducts(Int64? idParentProduct = null, bool isHierechialProduct = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Team> GetTeams(byte? idParentTeam = null, bool isHierarchicalTeam = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Project> GetProjectsByProductId(Int64 idProduct);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<User> GetUsers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Customer> GetCustomers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Project AddProject(Project project, List<byte> teamIds);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProjectById(Project project);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteProjectById(Int64 idProject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Project> GetOpenProjectOnBoard();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProductRoadmap> GetProductRoadmapByProductId(Int64 idProduct);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProjectStatusById(Project project, Int32 idStatus);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 AddProductRoadmap(ProductRoadmap productRoadmap);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProductRoadmapById(ProductRoadmap productRoadmap);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteProductRoadmapById(Int64 idProductRoadmap);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetOpenTaskByProjectId(Project project);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetOpenTaskWatchersByUserId(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProjectTask GetTaskDetailsByTaskId(Int64 idTask);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetOpenTasks(List<User> users);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetRequestAssistanceByRequestedFrom(Int32 idRequestFrom);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetRequestAssistanceByTask(Int64 idTask, Int32? idRequestFrom = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<GeosStatus> GetGeosOfferStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetTaskByWorkingDateAndUser(DateTime? workingDate = null, Int32? userId = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<TaskWorkingTime> GetWeeklyTaskWorkingTime(Int32 userId, DateTime workingDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetTasksByTaskType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TaskWorkingTime AddWorkingHoursInTask(TaskWorkingTime taskWorkingTime);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Project GetProjectByProjectId(Int64 idProject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkingHoursInTask(TaskWorkingTime taskWorkingTime);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<TaskWorkingTime> GetTaskWorkingTimeByDateAndUser(Int32? userId = null, DateTime? workingDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectMilestone> GetProjectMilestonesByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetProjectTasksDeliveryByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Project> GetProjectsDeliveryByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectAnalysis> GetProjectAnalysisDeliveryByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<TaskWorkingTime> GetProjectWorkingTimeByProjectId(Int64 idProject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProductVersion> GetProductVersionByProductId(Int64 idProduct);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteTaskWatcherById(Int64 idTaskWatcher);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TaskWatcher AddTaskWatcher(TaskWatcher taskWatcher);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<String> GetCode(string code, string roadMapType, string roadMapSource);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        float GetTotalTaskWorkingTime(Int64 idTask);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetTaskAttachment(Int64 idTask);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetRequestAssistanceByRequestedTo(Int32 idRequestTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProjectMilestone AddProjectMilestone(ProjectMilestone projectMilestone);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TaskAssistance AddTaskAssistance(TaskAssistance taskAssistance);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteTaskAssistanceById(Int64 idTaskAssistance);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Product> GetAllProducts();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<User> GetUserTeams(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Product GetProductByProductId(Int64 idProduct);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TaskComment AddTaskComment(TaskComment taskComment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteTaskCommentById(Int64 idTaskComment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TaskAttachment AddTaskAttachment(TaskAttachment taskAttachment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<TaskAttachment> GetTaskAttachmentByTaskId(Int64 idTask);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProjectFollowup AddProjectFollowup(ProjectFollowup projectFollowup);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteTaskAttachmentById(Int64 idTaskAttachment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<User> GetUserByTeamId(List<Team> teams);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTask> GetTeamOpenTaskByUserId(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProjectProduct(Int64 idProject, Int64 idProduct);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteProjectFollowupById(Int64 idProjectFollowup);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteTaskWorkingTimeById(Int64 idTaskWorkingTime);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TaskWorkingTime> AddWorkingHoursInTaskList(List<TaskWorkingTime> taskWorkingTime);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkingHoursInTaskList(List<TaskWorkingTime> taskWorkingTimes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteTaskWorkingTimeByIdList(List<TaskWorkingTime> taskWorkingTimes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteProjectMilestoneDateById(Int64 idProjectMilestoneDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProjectMilestoneDate UpdateProjectMilestoneDateById(ProjectMilestoneDate projectMilestoneDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<ProjectTeam> GetProjectTeams(Int64 idProject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProjectMilestoneDate AddProjectMilestoneDate(ProjectMilestoneDate projectMilestoneDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProjectScope GetProjectScopeByProjectId(Int64 idProject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProjectScope(ProjectScope projectScope);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProjectScope AddProjectScope(ProjectScope projectScope);

    }
}
