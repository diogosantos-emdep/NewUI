using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Data.Common;
using System.Configuration;

namespace Emdep.Geos.Services.Web.Workbench
{
    /// <summary>
    /// EpcService class use for getting information of Epc Service
    /// </summary>
    public class EpcService : IEpcService
    {


        /// <summary>
        /// Get LookupValues by lookupkey
        /// </summary>
        /// <param name="lookupKey">Get lookup key</param>
        /// <returns>List of loop values</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         IList&lt;LookupValue&gt; list = control.GetLookupValues(8);
        ///     }
        /// }
        /// </code>
        /// </example>

        public IList<LookupValue> GetLookupValues(byte lookupKey)
        {
            IList<LookupValue> list = null;

            try
            {
                EpcManager mgr = new EpcManager();
                list = mgr.GetLookupValues(lookupKey);
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

        /// <summary>
        /// This method is to get all teams
        /// </summary>
        /// <param name="idParentTeam">Get parent team id</param>
        /// <param name="isHierarchicalTeam">Get hierarchical team order or not</param>
        /// <returns>List of team</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         IList&lt;Team&gt; list = control.GetTeams(null,false);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Team> GetTeams(byte? idParentTeam = null, bool isHierarchicalTeam = false)
        {
            IList<Team> list = null;

            try
            {
                EpcManager mgr = new EpcManager();
                list = mgr.GetTeams(idParentTeam, isHierarchicalTeam);
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

        /// <summary>
        /// This method is to get products
        /// </summary>
        /// <param name="idParentProduct">Get product parent id</param>
        /// <param name="isHierarchicalProduct">Get hierarchical product</param>
        /// <returns>List of products</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         IList&lt;Product&gt; list = control.GetProducts(null,false);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Product> GetProducts(Int64? idParentProduct = null, bool isHierarchicalProduct = false)
        {
            IList<Product> list = null;

            try
            {
                EpcManager mgr = new EpcManager();
                list = mgr.GetProducts(idParentProduct, isHierarchicalProduct);
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

        /// <summary>
        /// This method is to get projects related to product id
        /// </summary>
        /// <param name="idProduct">Get product id</param>
        /// <returns>List of project</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          IList&lt;Project&gt; list = control.GetProjects(22);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Project> GetProjectsByProductId(Int64 idProduct)
        {
            IList<Project> list = null;

            try
            {
                EpcManager mgr = new EpcManager();
                list = mgr.GetProjectsByProductId(idProduct);
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

        /// <summary>
        /// This method is to get all open task watcher related to user id
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <returns>List of all open task watcher related to user id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          IList&lt;ProjectTask&gt; list = control.GetOpenTaskWatcher(7);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetOpenTaskWatchersByUserId(Int32 idUser)
        {
            IList<ProjectTask> projectTasks = null;

            try
            {
                EpcManager mgr = new EpcManager();
                projectTasks = mgr.GetOpenTaskWatchersByUserId(idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;

        }

        /// <summary>
        /// This method is to get all open task request assistance related to request from id
        /// </summary>
        /// <param name="idRequestFrom">Get request from</param>
        /// <returns>List of all open task request assistance related to request from id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          IList&lt;ProjectTask&gt; list = control.GetRequestAssistanceByRequestedFrom(7);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetRequestAssistanceByRequestedFrom(Int32 idRequestFrom)
        {
            IList<ProjectTask> projectTasks = null;

            try
            {
                EpcManager mgr = new EpcManager();
                projectTasks = mgr.GetRequestAssistanceByRequestedFrom(idRequestFrom);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;

        }

        /// <summary>
        /// This method is to get all request assistance by task id
        /// </summary>
        /// <param name="idTask">Get task id</param>
        /// <param name="idRequestFrom">Get request from id</param>
        /// <returns>List of all project task</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          IList&lt;ProjectTask&gt; list = control.GetRequestAssistanceByTask(1);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetRequestAssistanceByTask(Int64 idTask, Int32? idRequestFrom = null)
        {
            IList<ProjectTask> projectTasks = null;

            try
            {
                EpcManager mgr = new EpcManager();
                projectTasks = mgr.GetRequestAssistanceByTask(idTask, idRequestFrom);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;

        }

        /// <summary>
        /// This method is to get list of all task working times related to user id and working date
        /// </summary>
        /// <param name="userId">Get Userid</param>
        /// <param name="workingDate">Get WorkingDate</param>
        /// <returns>List of task working times</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          IList&lt;taskWorkingTime&gt; list = control.GetTaskWorkingTimeByDateAndUser(4,DateTime.Now);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<TaskWorkingTime> GetTaskWorkingTimeByDateAndUser(Int32? userId = null, DateTime? workingDate = null)
        {
            IList<TaskWorkingTime> taskWorkingTimes = null;

            try
            {
                EpcManager mgr = new EpcManager();
                taskWorkingTimes = mgr.GetTaskWorkingTimeByDateAndUser(userId, workingDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return taskWorkingTimes;

        }

        /// <summary>
        /// This method is to get offer status type
        /// </summary>
        /// <returns>List of offer status type</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          IList&lt;GeosStatus&gt; list = control.GetGeosOfferStatus();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<GeosStatus> GetGeosOfferStatus()
        {
            IList<GeosStatus> geosStatus = null;

            try
            {
                EpcManager mgr = new EpcManager();
                geosStatus = mgr.GetGeosOfferStatus();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosStatus;
        }


        /// <summary>
        /// This method is to get list of weekly task working time related to user id and working date
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <param name="workingDate">Get working date</param>
        /// <returns>List of task working time</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         DateTime workingDate = new DateTime(2009, 03, 05);
        ///         IList&lt;TaskWorkingTime&gt; list = control.GetWeeklyTaskWorkingTime(4,workingDate);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<TaskWorkingTime> GetWeeklyTaskWorkingTime(Int32 userId, DateTime workingDate)
        {
            IList<TaskWorkingTime> taskWorkingTimes = null;

            try
            {
                EpcManager mgr = new EpcManager();
                taskWorkingTimes = mgr.GetWeeklyTaskWorkingTime(userId, workingDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return taskWorkingTimes;
        }

        /// <summary>
        /// This method is to get list of project task
        /// </summary>
        /// <returns>List of project task</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         IList&lt;ProjectTask&gt; list = control.GetTasksByTaskType();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetTasksByTaskType()
        {
            IList<ProjectTask> projectTasks = new List<ProjectTask>();

            try
            {
                EpcManager mgr = new EpcManager();
                projectTasks = mgr.GetTasksByTaskType();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;
        }

        /// <summary>
        /// This method is to get task details by task id
        /// </summary>
        /// <param name="idTask">Get Task id</param>
        /// <returns>Get task details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         ProjectTask projectTask = control.GetTaskDetailsByTaskId(1);
        ///     }
        /// }
        /// </code>
        /// </example>
        public ProjectTask GetTaskDetailsByTaskId(Int64 idTask)
        {
            ProjectTask projectTask = null;

            try
            {
                EpcManager mgr = new EpcManager();
                projectTask = mgr.GetTaskDetailsByTaskId(idTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTask;

        }

        /// <summary>
        /// This method is to get all users
        /// </summary>
        /// <returns>List of user</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          IList&lt;User&gt; list = control.GetUsers();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<User> GetUsers()
        {
            IList<User> list = null;

            try
            {
                UserManager usermgr = new UserManager();
                list = usermgr.GetUsers();
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

        /// <summary>
        /// This method is to get all customers
        /// </summary>
        /// <returns>List of customer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         IList&lt;Customer&gt; list = control.GetCustomers();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Customer> GetCustomers()
        {
            IList<Customer> list = null;

            try
            {
                CustomerManager customermgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["EpcContext"].ConnectionString;
                list = customermgr.GetCustomers(connectionString);
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

        /// <summary>
        /// This method is to get request assistance related to requested to
        /// </summary>
        /// <param name="idRequestTo">Get request to id</param>
        /// <returns>List of project task</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         IList&lt;ProjectTask&gt; list = control.GetRequestAssistanceByRequestedTo(idRequestTo);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetRequestAssistanceByRequestedTo(Int32 idRequestTo)
        {
            IList<ProjectTask> projectTasks = null;

            try
            {
                EpcManager epcmgr = new EpcManager();
                projectTasks = epcmgr.GetRequestAssistanceByRequestedTo(idRequestTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;

        }

        /// <summary>
        /// This method is to add project followup
        /// </summary>
        /// <param name="projectFollowup">Get project followup details to add</param>
        /// <returns>Get added project followup details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           ProjectFollowup projectFollowup = new ProjectFollowup();
        ///           projectFollowup.Description = "Test";
        ///           projectFollowup.FollowupDate = DateTime.Now;
        ///           projectFollowup.IdUser = 3;
        ///           projectFollowup.IdProject = 1365;
        ///           projectFollowup = epcControl.AddProjectFollowup(projectFollowup);
        ///     }
        /// }
        /// </code>
        /// </example>
        public ProjectFollowup AddProjectFollowup(ProjectFollowup projectFollowup)
        {
            ProjectFollowup updatedProjectFollowup = null;

            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedProjectFollowup = epcmgr.AddProjectFollowup(projectFollowup);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedProjectFollowup;

        }

        /// <summary>
        /// This method is to get product details of related product id
        /// </summary>
        /// <param name="idProduct">Get product id</param>
        /// <returns>Product details of related product id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          Product product = epcControl.GetProductByProductId(12);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Product GetProductByProductId(Int64 idProduct)
        {
            Product product = null;

            try
            {
                EpcManager epcmgr = new EpcManager();
                product = epcmgr.GetProductByProductId(idProduct);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return product;

        }

        /// <summary>
        /// This method is to add task comment
        /// </summary>
        /// <param name="taskComment">Get task comment details to add</param>
        /// <returns>Get added task comment details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///            TaskComment taskComment = new TaskComment();
        ///            taskComment.Comment = "Testing";
        ///            taskComment.CommentDate = DateTime.Now;
        ///            taskComment.IdTask = 389;
        ///            taskComment.IdUser = 3;
        ///            taskComment = epcControl.AddTaskComment(taskComment);
        ///     }
        /// }
        /// </code>
        /// </example>
        public TaskComment AddTaskComment(TaskComment taskComment)
        {
            TaskComment updatedTaskComment = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedTaskComment = epcmgr.AddTaskComment(taskComment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedTaskComment;
        }

        /// <summary>
        /// This method is to get task attachment
        /// </summary>
        /// <param name="idTask">Get task id</param>
        /// <returns>Get task attachment in bytes</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         byte[] fileInBytes = control.GetTaskAttachment(idTask);
        ///     }
        /// }
        /// </code>
        /// </example>
        public byte[] GetTaskAttachment(Int64 idTask)
        {
            byte[] fileInBytes = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
               // fileInBytes = epcmgr.GetTaskAttachment(idTask, Properties.Settings.Default.EpcProjectFolder);
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
            return fileInBytes;
        }

        /// <summary>
        /// This method is to get task attachment details related to id task
        /// </summary>
        /// <param name="idTask">Get id task </param>
        /// <returns>Get task attachment details related to id task</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          IList&lt;TaskAttachment&gt; taskAttachments = epcControl.GetTaskAttachmentByTaskId(8068);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<TaskAttachment> GetTaskAttachmentByTaskId(Int64 idTask)
        {
            IList<TaskAttachment> taskAttachments = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                taskAttachments = epcmgr.GetTaskAttachmentByTaskId(idTask,Properties.Settings.Default.EpcProjectFolder);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return taskAttachments;
        }

        /// <summary>
        /// This method is to add project
        /// </summary>
        /// <param name="project">Get project details to add</param>
        /// <param name="teamIds">Get team Ids</param>
        /// <returns>Added project details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          Project project = new Project();
        ///          project.IdProduct = 1;
        ///          project.ProjectCode = "2016-PR0421";
        ///          project.Description = "This projects belongs to Geos";
        ///          project.ProjectName = "Geos2";
        ///          project.IdCustomer = 2;
        ///          project.IdOwner = 4;
        ///          project.IdPriority = 3;
        ///          project.IdStatus = 8;
        ///          project.IdCategory = 14;
        ///          project.ProjectPath = "";
        ///          project.DueDate = DateTime.Now;
        ///          Project project = control.AddProject();
        ///     }
        /// }
        /// </code>
        /// </example>
        public Project AddProject(Project project,List<byte> teamIds)
        {
            Project updatedProject = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedProject = epcmgr.AddProject(project, Properties.Settings.Default.EpcProjectFolder, teamIds);
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
            return updatedProject;

        }

        /// <summary>
        /// This method is to update project by project id
        /// </summary>
        /// <param name="project">Get project details to update</param>
        /// <returns>Updated project or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          Project project = new Project();
        ///          project.IdProject =23;
        ///          project.IdProduct = 1;
        ///          project.ProjectCode = "2016-PR0421";
        ///          project.Description = "This projects belongs to Geos";
        ///          project.ProjectName = "Geos2";
        ///          project.IdCustomer = 2;
        ///          project.IdOwner = 4;
        ///          project.IdPriority = 3;
        ///          project.IdStatus = 8;
        ///          project.IdCategory = 14;
        ///          project.ProjectPath = "";
        ///          project.DueDate = DateTime.Now;
        ///          bool isUpdated = control.UpdateProjectById(project);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateProjectById(Project project)
        {
            bool isUpdated = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isUpdated = epcmgr.UpdateProjectById(project);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;

        }

        /// <summary>
        /// This method is to update project scope
        /// </summary>
        /// <param name="projectScope">Get project scope details</param>
        /// <returns>isupdated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          Program program = new Program();
        ///          ProjectScope projectScope = new ProjectScope();
        ///          projectScope.IdProjectScope = 1;
        ///          projectScope.IdProductVersion = 16;
        ///          projectScope.IdProject = 1654;
        ///          projectScope.IdOffer = 41;
        ///          projectScope.CreatedBy = 3;
        ///          projectScope.ProductScopeDescription = "Test";
        ///          projectScope.ProjectAcceptanceCriteria = "Test";
        ///          projectScope.ProjectAssumptions = "Test";
        ///          projectScope.ProjectConstraints = "Test";
        ///          projectScope.ProjectDeliverables = "Test";
        ///          projectScope.ProjectExclusions = "Test";
        ///          byte[] filebyte = program.GetCompanyImage();
        ///          projectScope.ScopeFileBytes = filebyte;
        ///          bool isUpdated = epcControl.UpdateProjectScope(projectScope);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateProjectScope(ProjectScope projectScope)
        {
            bool isUpdated = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isUpdated = epcmgr.UpdateProjectScope(projectScope,Properties.Settings.Default.EpcProjectFolder);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;

        }

        /// <summary>
        /// This method is to update working hours in task
        /// </summary>
        /// <param name="taskWorkingTime">Get task working time details</param>
        /// <returns>Get is updated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         bool isUpdated = control.UpdateWorkingHoursInTask(taskWorkingTime);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateWorkingHoursInTask(TaskWorkingTime taskWorkingTime)
        {
            bool isUpdated = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isUpdated = epcmgr.UpdateWorkingHoursInTask(taskWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;

        }
        /// <summary>
        /// This method is to update working hours in task list
        /// </summary>
        /// <param name="taskWorkingTimes">Get list of task working times details</param>
        /// <returns>updated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          List&lt;TaskWorkingTime&gt; taskWorkingTimes = new List&lt;TaskWorkingTime&gt;();
        ///          TaskWorkingTime taskWorkingTime = new TaskWorkingTime();
        ///          taskWorkingTime.IdTaskWorkingTime = 3817;
        ///           taskWorkingTime.IdUser = 4;
        ///           taskWorkingTime.IdTask = 2612;
        ///           taskWorkingTime.Description = "Test EPC89";
        ///           taskWorkingTime.WorkingDate = DateTime.Now;
        ///           taskWorkingTime.WorkingTimeInHours = 20;
        ///           TaskWorkingTime taskWorkingTime1 = new TaskWorkingTime();
        ///           taskWorkingTime1.IdTaskWorkingTime = 3824;
        ///           taskWorkingTime1.IdUser = 4;
        ///           taskWorkingTime1.IdTask = 8126;
        ///           taskWorkingTime1.Description = "Test EPC89";
        ///           taskWorkingTime1.WorkingDate = DateTime.Now;
        ///           taskWorkingTime1.WorkingTimeInHours = 10;
        ///           taskWorkingTimes.Add(taskWorkingTime);
        ///           taskWorkingTimes.Add(taskWorkingTime1);
        ///           bool isUpdated= epcControl.UpdateWorkingHoursInTaskList(taskWorkingTimes);
        ///     }
        /// }
        /// </code>
        /// </example> 
        public bool UpdateWorkingHoursInTaskList(List<TaskWorkingTime> taskWorkingTimes)
        {
            bool isUpdated = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isUpdated = epcmgr.UpdateWorkingHoursInTaskList(taskWorkingTimes);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;

        }

        /// <summary>
        /// This method is to update project status by project id and status id
        /// </summary>
        /// <param name="project">Get project details</param>
        /// <param name="idStatus">Get status id</param>
        /// <returns>isUpdated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          Project project = new Project();
        ///          project.IdProject =23;
        ///          project.IdProduct = 1;
        ///          project.ProjectCode = "2016-PR0421";
        ///          project.Description = "This projects belongs to Geos";
        ///          project.ProjectName = "Geos2";
        ///          project.IdCustomer = 2;
        ///          project.IdOwner = 4;
        ///          project.IdPriority = 3;
        ///          project.IdStatus = 8;
        ///          project.IdCategory = 14;
        ///          project.ProjectPath = "";
        ///          project.DueDate = DateTime.Now;
        ///          bool isUpdated = control.UpdateProjectStatusById(project,4);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateProjectStatusById(Project project, Int32 idStatus)
        {
            bool isUpdated = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isUpdated = epcmgr.UpdateProjectStatusById(project, idStatus);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;

        }

        /// <summary>
        /// This method is to delete project by project id
        /// </summary>
        /// <param name="idProject">Get project id </param>
        /// <returns>isDeleted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          
        ///          bool isDeleted = control.DeleteProjectById(23);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool DeleteProjectById(Int64 idProject)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isDeleted = epcmgr.DeleteProjectById(idProject);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        /// <summary>
        /// This method is to add project roadmap
        /// </summary>
        /// <param name="productRoadmap">Get project roadmap details to add</param>
        /// <returns>Get product roadmap id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          ProductRoadmap productRoadmap = new ProductRoadmap();
        ///          productRoadmap.idProduct =23;
        ///          productRoadmap.idSource = 1;
        ///          productRoadmap.idPriority = 3;
        ///          productRoadmap.idStatus = 8;
        ///          productRoadmap.idType = 14;
        ///          productRoadmap.linkedTo = "";
        ///          productRoadmap.requestDate = DateTime.Now;
        ///          productRoadmap.sourceFrom = "";
        ///          productRoadmap.title = "";
        ///          productRoadmap.description = "";
        ///          Int64 productRoadmapId = control.AddProductRoadmap(productRoadmap);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Int64 AddProductRoadmap(ProductRoadmap productRoadmap)
        {
            Int64 productRoadmapId = 0;
            try
            {
                EpcManager epcmgr = new EpcManager();
                productRoadmapId = epcmgr.AddProductRoadmap(productRoadmap);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return productRoadmapId;

        }

        /// <summary>
        /// This method is to add task attachment
        /// </summary>
        /// <param name="taskAttachment">Get task attachment details to add</param>
        /// <returns>Get added task attachment details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          ProductRoadmap productRoadmap = new ProductRoadmap();
        ///          productRoadmap.idProduct =23;
        ///          productRoadmap.idSource = 1;
        ///          productRoadmap.idPriority = 3;
        ///          productRoadmap.idStatus = 8;
        ///          productRoadmap.idType = 14;
        ///          productRoadmap.linkedTo = "";
        ///          productRoadmap.requestDate = DateTime.Now;
        ///          productRoadmap.sourceFrom = "";
        ///          productRoadmap.title = "";
        ///          productRoadmap.description = "";
        ///          Int64 productRoadmapId = control.AddProductRoadmap(productRoadmap);
        ///     }
        /// }
        /// </code>
        /// </example>
        public TaskAttachment AddTaskAttachment(TaskAttachment taskAttachment)
        {
            TaskAttachment updatedTaskAttachment = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedTaskAttachment = epcmgr.AddTaskAttachment(taskAttachment, Properties.Settings.Default.EpcProjectFolder);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedTaskAttachment;

        }

        /// <summary>
        /// This method is to update project roadmap
        /// </summary>
        /// <param name="productRoadmap">Get product roadmap details</param>
        /// <returns>Updated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          ProductRoadmap productRoadmap = new ProductRoadmap();
        ///          productRoadmap.idProductRoadmap =1;
        ///          productRoadmap.idProduct =23;
        ///          productRoadmap.idSource = 1;
        ///          productRoadmap.idPriority = 3;
        ///          productRoadmap.idStatus = 8;
        ///          productRoadmap.idType = 14;
        ///          productRoadmap.linkedTo = "";
        ///          productRoadmap.requestDate = DateTime.Now;
        ///          productRoadmap.sourceFrom = "";
        ///          productRoadmap.title = "";
        ///          productRoadmap.description = "";
        ///          bool isUpdated = control.UpdateProductRoadmapById(productRoadmap);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateProductRoadmapById(ProductRoadmap productRoadmap)
        {
            bool isUpdated = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isUpdated = epcmgr.UpdateProductRoadmapById(productRoadmap);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;

        }

        /// <summary>
        /// This method is to delete project roadmap by id
        /// </summary>
        /// <param name="idProductRoadmap">Get product roadmap id</param>
        /// <returns>Deleted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///        
        ///          bool isDeleted = control.DeleteProductRoadmapById(1);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool DeleteProductRoadmapById(Int64 idProductRoadmap)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isDeleted = epcmgr.DeleteProductRoadmapById(idProductRoadmap);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        /// <summary>
        /// This method is to delete project followup related to idProjectFollowup
        /// </summary>
        /// <param name="idProjectFollowup">Get idProjectFollowup</param>
        /// <returns>Deleted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///        
        ///          bool isDeleted = control.DeleteProjectFollowupById(1);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool DeleteProjectFollowupById(Int64 idProjectFollowup)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isDeleted = epcmgr.DeleteProjectFollowupById(idProjectFollowup);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        /// <summary>
        /// This method is to get list of open project
        /// </summary>
        /// <returns>List of open project</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///        
        ///          IList&lt;Project&gt; project = control.GetOpenProjectOnBoard();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Project> GetOpenProjectOnBoard()
        {
            IList<Project> project = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                project = epcmgr.GetOpenProjectOnBoard();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return project;

        }

        /// <summary>
        /// This method is to get list of all project roadmap related to product id
        /// </summary>
        /// <param name="idProduct">Get product id</param>
        /// <returns>List of all project roadmap related to product id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///        
        ///         IList&lt;ProductRoadmap&gt; productRoadmaps = control.GetProductRoadmap(23);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProductRoadmap> GetProductRoadmapByProductId(Int64 idProduct)
        {
            IList<ProductRoadmap> productRoadmaps = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                productRoadmaps = epcmgr.GetProductRoadmapByProductId(idProduct);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return productRoadmaps;
        }


        /// <summary>
        /// This method is to get open all tasks related to project id
        /// </summary>
        /// <param name="project">Get project id</param>
        /// <returns>List of project task</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          Project project = new Project();
        ///          project.IdProject =23;
        ///          project.IdProduct = 1;
        ///          project.ProjectCode = "2016-PR0421";
        ///          project.Description = "This projects belongs to Geos";
        ///          project.ProjectName = "Geos2";
        ///          project.IdCustomer = 2;
        ///          project.IdOwner = 4;
        ///          project.IdPriority = 3;
        ///          project.IdStatus = 8;
        ///          project.IdCategory = 14;
        ///          project.ProjectPath = "";
        ///          project.DueDate = DateTime.Now;
        ///         IList&lt;ProjectTask&gt; projectTasks = control.GetOpenTaskByProjectId(project);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetOpenTaskByProjectId(Project project)
        {
            IList<ProjectTask> projectTasks = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projectTasks = epcmgr.GetOpenTaskByProjectId(project);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;
        }

        /// <summary>
        /// This method is to get all open tasks
        /// </summary>
        /// <param name="users">Get list of user id</param>
        /// <returns>List of open task</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///       
        ///         IList&lt;ProjectTask&gt; projectTasks = control.GetOpenTask(users);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetOpenTasks(List<User> users)
        {
            IList<ProjectTask> projectTasks = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projectTasks = epcmgr.GetOpenTasks(users);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;
        }

        /// <summary>
        /// This method is to get list of project teams
        /// </summary>
        /// <param name="idProject">Get project id</param>
        /// <returns>List of project teams related to project id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///       
        ///         IList&lt;ProjectTeam&gt; projectTeams = control.GetProjectTeams(idProject);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTeam> GetProjectTeams(Int64 idProject)
        {
            IList<ProjectTeam> projectTeams = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projectTeams = epcmgr.GetProjectTeams(idProject);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTeams;
        }

        /// <summary>
        /// This method is to get project deatils related to project id
        /// </summary>
        /// <param name="idProject">Get project id</param>
        /// <returns>Project details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///       
        ///         Project project = control.GetProjectByProjectId(idProject);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Project GetProjectByProjectId(Int64 idProject)
        {
            Project project = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                project = epcmgr.GetProjectByProjectId(idProject);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return project;
        }

        /// <summary>
        /// This method is to get total working time related to task id
        /// </summary>
        /// <param name="idTask">Get task id</param>
        /// <returns>Get toatl working time related to task id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///       
        ///         float totalTaskWorkingTime = epcControl.GetTotalTaskWorkingTime(idTask);
        ///     }
        /// }
        /// </code>
        /// </example>
        public float GetTotalTaskWorkingTime(Int64 idTask)
        {
            float totalTaskWorkingTime = 0;
            try
            {
                EpcManager epcmgr = new EpcManager();
                totalTaskWorkingTime = epcmgr.GetTotalTaskWorkingTime(idTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return totalTaskWorkingTime;
        }

        /// <summary>
        /// This method is to add working hours in task
        /// </summary>
        /// <param name="taskWorkingTime">Get task working time details to add</param>
        /// <returns>Get added task working time</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          TaskWorkingTime taskWorkingTime = new TaskWorkingTime();
        ///          taskWorkingTime.IdUser = 4;
        ///          taskWorkingTime.IdTask = 2612;
        ///          taskWorkingTime.Description = "Test EPC89";
        ///          taskWorkingTime.WorkingDate = DateTime.Now;
        ///          taskWorkingTime.WorkingTimeInHours = new TimeSpan(00, 00, 00);
        ///          taskWorkingTime = epcControl.AddWorkingHoursInTask(taskWorkingTime);
        ///        
        ///     }
        /// }
        /// </code>
        /// </example>
        public TaskWorkingTime AddWorkingHoursInTask(TaskWorkingTime taskWorkingTime)
        {
            TaskWorkingTime updatedTaskWorkingTime = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedTaskWorkingTime = epcmgr.AddWorkingHoursInTask(taskWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedTaskWorkingTime;

        }


        /// <summary>
        /// This method is to add task watcher
        /// </summary>
        /// <param name="taskWatcher">Get task watcher to add</param>
        /// <returns>Get added task watcher details </returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          TaskWatcher taskWatcher = new TaskWatcher();
        ///           taskWatcher.IdTask = 4;
        ///           taskWatcher.IdUser = 4;
        ///           taskWatcher =epcControl.AddTaskWatcher(taskWatcher);
        ///     }
        /// }
        /// </code>
        /// </example>
        public TaskWatcher AddTaskWatcher(TaskWatcher taskWatcher)
        {
            TaskWatcher updatedTaskWatcher = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedTaskWatcher = epcmgr.AddTaskWatcher(taskWatcher);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedTaskWatcher;

        }

        /// <summary>
        /// This method is to add task assistance
        /// </summary>
        /// <param name="taskAssistance">Get task assistance details to add</param>
        /// <returns>Get added task assistance details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           TaskAssistance taskAssistance = new TaskAssistance();
        ///           taskAssistance.IdRequestFrom = 3;
        ///           taskAssistance.IdRequestTo = 39;
        ///           taskAssistance.IdTask = 4061;
        ///           taskAssistance.RequestDate = DateTime.Now;
        ///           taskAssistance = epcControl.AddTaskAssistance(taskAssistance);
        ///     }
        /// }
        /// </code>
        /// </example>
        public TaskAssistance AddTaskAssistance(TaskAssistance taskAssistance)
        {
            TaskAssistance updatedTaskAssistance = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedTaskAssistance = epcmgr.AddTaskAssistance(taskAssistance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedTaskAssistance;

        }


        /// <summary>
        /// This method is to add project milestone
        /// </summary>
        /// <param name="projectMilestone">Get project milestone details to add</param>
        /// <returns>Get added project milestone details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           ProjectMilestone projectMilestone = new ProjectMilestone();
        ///            projectMilestone.MilestoneTitle = "Desktop";
        ///            projectMilestone.CreationDate = DateTime.Now
        ///            projectMilestone.Description = "This for Geos Desktop";
        ///            projectMilestone.IdProject = 793;
        ///            projectMilestone.TargetDate = DateTime.Now;
        ///            projectMilestone.Comments = "Test";
        ///            projectMilestone = epcControl.AddProjectMilestone(projectMilestone);
        ///     }
        /// }
        /// </code>
        /// </example>
        public ProjectMilestone AddProjectMilestone(ProjectMilestone projectMilestone)
        {
            ProjectMilestone updatedProjectMilestone = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedProjectMilestone = epcmgr.AddProjectMilestone(projectMilestone);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedProjectMilestone;
        }

        /// <summary>
        /// This method is to add project scope
        /// </summary>
        /// <param name="projectScope">Get project scope details</param>
        /// <returns>Added project scope details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          Program program = new Program();
        ///          ProjectScope projectScope = new ProjectScope();
        ///          projectScope.IdProductVersion = 16;
        ///          projectScope.IdProject = 1654;
        ///          projectScope.IdOffer = 41;
        ///          projectScope.CreatedBy = 3;
        ///          projectScope.ProductScopeDescription = "Test";
        ///          projectScope.ProjectAcceptanceCriteria = "Test";
        ///          projectScope.ProjectAssumptions = "Test";
        ///          projectScope.ProjectConstraints = "Test";
        ///          projectScope.ProjectDeliverables = "Test";
        ///          projectScope.ProjectExclusions = "Test";
        ///          byte[] filebyte = program.GetCompanyImage();
        ///          projectScope.ScopeFileBytes = filebyte;
        ///          ProjectScope updatedProjectScope = epcControl.AddProjectScope(projectScope);
        ///     }
        /// }
        /// </code>
        /// </example>
        public ProjectScope AddProjectScope(ProjectScope projectScope)
        {
            ProjectScope updatedProjectScope = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedProjectScope = epcmgr.AddProjectScope(projectScope, Properties.Settings.Default.EpcProjectFolder);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedProjectScope;
        }

        /// <summary>
        /// This method is to get project scope by project id
        /// </summary>
        /// <param name="idProject">Get id project</param>
        /// <returns>Project scope details related ti project id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         ProjectScope projectScope = epcControl.GetProjectScopeByProjectId(1658);
        ///     }
        /// }
        /// </code>
        /// </example>
        public ProjectScope GetProjectScopeByProjectId(Int64 idProject)
        {
            ProjectScope projectScope = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projectScope = epcmgr.GetProjectScopeByProjectId(idProject);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectScope;
        }

        /// <summary>
        /// This method is to delete task watcher realted to task watcher id
        /// </summary>
        /// <param name="idTaskWatcher">Get id task watcher</param>
        /// <returns>Get isdeleted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           bool isDeleted = control.DeleteTaskWatcherById(idTaskWatcher);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool DeleteTaskWatcherById(Int64 idTaskWatcher)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isDeleted = epcmgr.DeleteTaskWatcherById(idTaskWatcher);
            }
            catch (Exception ex)
            {
                isDeleted = false;
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        /// <summary>
        /// This method is to get task working time
        /// </summary>
        /// <param name="idTaskWorkingTime">Get id task working time</param>
        /// <returns>Deleted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           bool isDeleted = control.DeleteTaskWorkingTimeById(idTaskWorkingTime);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool DeleteTaskWorkingTimeById(Int64 idTaskWorkingTime)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isDeleted = epcmgr.DeleteTaskWorkingTimeById(idTaskWorkingTime);
            }
            catch (Exception ex)
            {
                isDeleted = false;
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        /// <summary>
        /// This method is to delete task working time by id list
        /// </summary>
        /// <param name="taskWorkingTimes">Get list of task working time</param>
        /// <returns>isDeleted or not </returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           bool isDeleted = control.DeleteTaskWorkingTimeByIdList(taskWorkingTimes);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool DeleteTaskWorkingTimeByIdList(List<TaskWorkingTime> taskWorkingTimes)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isDeleted = epcmgr.DeleteTaskWorkingTimeByIdList(taskWorkingTimes);
            }
            catch (Exception ex)
            {
                isDeleted = false;
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        /// <summary>
        /// This method is to delete project milestone date by id
        /// </summary>
        /// <param name="idProjectMilestoneDate">Get project milestone date id</param>
        /// <returns>isDeleted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           bool isDeleted = control.DeleteProjectMilestoneDateById(idProjectMilestoneDate);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool DeleteProjectMilestoneDateById(Int64 idProjectMilestoneDate)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isDeleted = epcmgr.DeleteProjectMilestoneDateById(idProjectMilestoneDate);
            }
            catch (Exception ex)
            {
                isDeleted = false;
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        /// <summary>
        /// This method is to update project milestone date by id
        /// </summary>
        /// <param name="projectMilestoneDate">Get project milestone date details</param>
        /// <returns>isUpdated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           ProjectMilestoneDate projectMilestoneDate = new ProjectMilestoneDate();
        ///           projectMilestoneDate.IdProjectMilestoneDate = 8;
        ///           projectMilestoneDate.IdProjectMilestoneStatus = 91;
        ///           projectMilestoneDate.TargetDate = DateTime.Now;
        ///           bool isUpdated = epcControl.UpdateProjectMilestoneDateById(projectMilestoneDate);
        ///     }
        /// }
        /// </code>
        /// </example>
        public ProjectMilestoneDate UpdateProjectMilestoneDateById(ProjectMilestoneDate projectMilestoneDate)
        {
            ProjectMilestoneDate updatedprojectMilestoneDate = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedprojectMilestoneDate = epcmgr.UpdateProjectMilestoneDateById(projectMilestoneDate);
            }
            catch (Exception ex)
            {
              
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedprojectMilestoneDate;

        }

        /// <summary>
        /// This method is to add project milestone date
        /// </summary>
        /// <param name="projectMilestoneDate">Get project milestone date details to add</param>
        /// <returns>Get added project milestone details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           ProjectMilestoneDate projectMilestoneDate = new ProjectMilestoneDate();
        ///           projectMilestoneDate.IdProjectMilestone = 8;
        ///           projectMilestoneDate.IdProjectMilestoneStatus = 91;
        ///           projectMilestoneDate.TargetDate = DateTime.Now;
        ///           ProjectMilestoneDate addedProjectMilestoneDate = epcControl.AddProjectMilestoneDate(projectMilestoneDate);
        ///     }
        /// }
        /// </code>
        /// </example>
        public ProjectMilestoneDate AddProjectMilestoneDate(ProjectMilestoneDate projectMilestoneDate)
        {
            ProjectMilestoneDate addedProjectMilestoneDate = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                addedProjectMilestoneDate = epcmgr.AddProjectMilestoneDate(projectMilestoneDate);
            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedProjectMilestoneDate;

        }

        /// <summary>
        /// This method is to delete task comment by task comment id
        /// </summary>
        /// <param name="idTaskComment">Get task comment id</param>
        /// <returns>Is deleted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           bool isDeleted = control.DeleteTaskCommentById(idTaskComment);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool DeleteTaskCommentById(Int64 idTaskComment)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isDeleted = epcmgr.DeleteTaskCommentById(idTaskComment);
            }
            catch (Exception ex)
            {
                isDeleted = false;
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        /// <summary>
        /// This method is to delete task attachment related to task attachment id
        /// </summary>
        /// <param name="idTaskAttachment">Get Task attachment id</param>
        /// <returns>Is Deleted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           bool isDeleted = control.DeleteTaskAttachmentById(idTaskAttachment);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool DeleteTaskAttachmentById(Int64 idTaskAttachment)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                string directoryPath = Properties.Settings.Default.EpcProjectFolder;
                isDeleted = epcmgr.DeleteTaskAttachmentById(idTaskAttachment, directoryPath);
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
                isDeleted = false;
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to delete task assistance by id
        /// </summary>
        /// <param name="idTaskAssistance">Get task assistance id</param>
        /// <returns>is deleted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           bool isDeleted = control.DeleteTaskAssistanceById(idTaskAssistance);
        ///     }
        /// }
        /// </code>
        /// </example> 
        public bool DeleteTaskAssistanceById(Int64 idTaskAssistance)
        {
            bool isDeleted = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isDeleted = epcmgr.DeleteTaskAssistanceById(idTaskAssistance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        /// <summary>
        /// This method is to get list of products
        /// </summary>
        /// <returns>List of products</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           IList&lt;Product&gt; products = control.GetProducts();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Product> GetAllProducts()
        {
            IList<Product> products = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                products = epcmgr.GetAllProducts();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return products;
        }

        /// <summary>
        /// This method is to get list of user details in team related to user id
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <returns>List of user</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           IList&lt;User&gt; users = control.GetUserTeams(3);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<User> GetUserTeams(Int32 idUser)
        {
            IList<User> users = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                users = epcmgr.GetUserTeams(idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return users;
        }

        /// <summary>
        /// This method is to get product version related to product id
        /// </summary>
        /// <param name="idProduct">Get product id</param>
        /// <returns>List of product version</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService control = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           IList&lt;ProductVersion&gt; productVersions = control.GetProductVersionByProductId(idProduct);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProductVersion> GetProductVersionByProductId(Int64 idProduct)
        {
            IList<ProductVersion> productVersions = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                productVersions = epcmgr.GetProductVersionByProductId(idProduct);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return productVersions;
        }

        /// <summary>
        /// This method is to get list of code as per condition
        /// </summary>
        /// <param name="code">Get Code</param>
        /// <param name="roadMapType">Get RoadMapType</param>
        /// <param name="roadMapSource">Get RoadMapSource</param>
        /// <returns>List of code</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           IList&lt;String&gt; codeList = epcControl.GetCode("", "Proposals", "PO");
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<String> GetCode(string code, string roadMapType, string roadMapSource)
        {
            IList<String> codeList = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                codeList = epcmgr.GetCode(code, roadMapType, roadMapSource);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return codeList;
        }

        /// <summary>
        /// This method is to get task working time
        /// </summary>
        /// <param name="idProject">Get project id</param>
        /// <returns>List of task working time</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           IList&lt;TaskWorkingTime&gt; taskWorkingTime = epcControl.GetProjectWorkingTimeByProjectId(idProject);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<TaskWorkingTime> GetProjectWorkingTimeByProjectId(Int64 idProject)
        {
            IList<TaskWorkingTime> taskWorkingTime = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                taskWorkingTime = epcmgr.GetProjectWorkingTimeByProjectId(idProject);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return taskWorkingTime;
        }

        /// <summary>
        /// This method is to get list of task  related to working date and user id
        /// </summary>
        /// <param name="workingDate">Get working date </param>
        /// <param name="userId">Get user id</param>
        /// <returns>List of project task</returns>
        ///  <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           IList&lt;ProjectTask&gt; projectTasks = epcControl.GetTaskByWorkingDateAndUser( DateTime.Now,66);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetTaskByWorkingDateAndUser(DateTime? workingDate = null, Int32? userId = null)
        {
            IList<ProjectTask> projectTasks = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projectTasks = epcmgr.GetTaskByWorkingDateAndUser(workingDate, userId);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;

        }

        /// <summary>
        /// This method is to get project delivery related to date and user
        /// </summary>
        /// <param name="fromDate">Get from date</param>
        /// <param name="toDate">Get to date</param>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of project</returns>
        ///  <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           DateTime fromDate = new DateTime(2016,05, 29);
        ///           DateTime toDate = new DateTime(2016, 05, 29);
        ///           IList&lt;Project&gt; projects = epcControl.GetProjectsDeliveryByDateAndUser( fromDate,toDate,66);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Project> GetProjectsDeliveryByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null)
        {
            IList<Project> projects = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projects = epcmgr.GetProjectsDeliveryByDateAndUser(fromDate, toDate, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projects;
        }

        /// <summary>
        /// This method is to get project milestone related to date and user
        /// </summary>
        /// <param name="fromDate">Get from date</param>
        /// <param name="toDate">Get to date</param>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of project milestone</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           DateTime fromDate = new DateTime(2016,05, 29);
        ///           DateTime toDate = new DateTime(2016, 05, 29);
        ///           IList&lt;ProjectMilestone&gt; projectMilestones = epcControl.GetProjectMilestonesByDateAndUser( fromDate,toDate,66);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectMilestone> GetProjectMilestonesByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null)
        {
            IList<ProjectMilestone> projectMilestones = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projectMilestones = epcmgr.GetProjectMilestonesByDateAndUser(fromDate, toDate, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectMilestones;

        }

        /// <summary>
        /// This method is to get project task delivery related to date and user
        /// </summary>
        /// <param name="fromDate">Get from date</param>
        /// <param name="toDate">Get to date</param>
        /// <param name="idUser">Get user id</param>
        /// <returns>List of project task</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           DateTime fromDate = new DateTime(2016,05, 29);
        ///           DateTime toDate = new DateTime(2016, 05, 29);
        ///           IList&lt;ProjectTask&gt; projectTasks = epcControl.GetProjectTasksDeliveryByDateAndUser( fromDate,toDate,66);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetProjectTasksDeliveryByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null)
        {
            IList<ProjectTask> projectTasks = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projectTasks = epcmgr.GetProjectTasksDeliveryByDateAndUser(fromDate, toDate, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;

        }

        /// <summary>
        /// This method is to get project analysis delivery related to date and user id
        /// </summary>
        /// <param name="fromDate">Get from date</param>
        /// <param name="toDate">Get to date</param>
        /// <param name="idUser">Get user id</param>
        /// <returns>List of project analysis</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           DateTime fromDate = new DateTime(2016,05, 29);
        ///           DateTime toDate = new DateTime(2016, 05, 29);
        ///           IList&lt;ProjectAnalysis&gt; projectAnalysis = epcControl.GetProjectAnalysisDeliveryByDateAndUser( fromDate,toDate,66);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectAnalysis> GetProjectAnalysisDeliveryByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null)
        {
            IList<ProjectAnalysis> projectAnalysis = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projectAnalysis = epcmgr.GetProjectAnalysisDeliveryByDateAndUser(fromDate, toDate, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectAnalysis;

        }

        /// <summary>
        /// This method is to get user related to team id
        /// </summary>
        /// <param name="teams">Get list of id team</param>
        /// <returns>List of user</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           List&lt;byte&gt; idTeams = new List&lt;byte&gt;;
        ///           idTeams.Add(1);
        ///            idTeams.Add(2);
        ///           IList&lt;User&gt; users = epcControl.GetUserByTeamId(idTeams);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<User> GetUserByTeamId(List<Team> teams)
        {

            IList<User> users = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                users = epcmgr.GetUserByTeamId(teams);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return users;
        }

        /// <summary>
        /// This method is to get list of task related to users in iduser team
        /// </summary>
        /// <param name="idUser">Get iduser</param>
        /// <returns>List of task related to users in iduser team</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           IList&lt;ProjectTask&gt; projectTasks = epcControl.GetTeamOpenTaskByUserId(idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<ProjectTask> GetTeamOpenTaskByUserId(Int32 idUser)
        {

            IList<ProjectTask> projectTasks = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                projectTasks = epcmgr.GetTeamOpenTaskByUserId(idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return projectTasks;
        }


        /// <summary>
        /// This method is to add working hours in task list
        /// </summary>
        /// <param name="taskWorkingTimes">Get list of task working time details to add</param>
        /// <returns>Added list of task working time details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           List&lt;TaskWorkingTime&gt; taskWorkingTimes = new List&lt;TaskWorkingTime&gt;();
        ///           TaskWorkingTime taskWorkingTime = new TaskWorkingTime();
        ///           taskWorkingTime.IdUser = 4;
        ///           taskWorkingTime.IdTask = 2612;
        ///           taskWorkingTime.Description = "Test EPC89";
        ///           taskWorkingTime.WorkingDate = DateTime.Now;
        ///           taskWorkingTime.WorkingTimeInHours = 20;
        ///           TaskWorkingTime taskWorkingTime1 = new TaskWorkingTime();
        ///           taskWorkingTime1.IdUser = 4;
        ///           taskWorkingTime1.IdTask = 8126;
        ///           taskWorkingTime1.Description = "Test EPC89";
        ///           taskWorkingTime1.WorkingDate = DateTime.Now;
        ///           taskWorkingTime1.WorkingTimeInHours = 10;
        ///           taskWorkingTimes.Add(taskWorkingTime);
        ///           taskWorkingTimes.Add(taskWorkingTime1);
        ///           taskWorkingTimes = epcControl.AddWorkingHoursInTaskList(taskWorkingTimes);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<TaskWorkingTime> AddWorkingHoursInTaskList(List<TaskWorkingTime> taskWorkingTimes)
        {

            List<TaskWorkingTime> updatedTaskWorkingTimes = null;
            try
            {
                EpcManager epcmgr = new EpcManager();
                updatedTaskWorkingTimes = epcmgr.AddWorkingHoursInTaskList(taskWorkingTimes);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedTaskWorkingTimes;
        }

        /// <summary>
        /// This method is to update project product
        /// </summary>
        /// <param name="idProject">Get idproject</param>
        /// <param name="idProduct">Get idProduct</param>
        /// <returns>isUpdated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///           IEpcService epcControl = new EpcServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///           bool isUpdated = epcControl.UpdateProjectProduct(idProject,idProduct);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateProjectProduct(Int64 idProject, Int64 idProduct)
        {

            bool isUpdated = false;
            try
            {
                EpcManager epcmgr = new EpcManager();
                isUpdated = epcmgr.UpdateProjectProduct(idProject, idProduct);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


    }
}
