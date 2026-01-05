using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.DataAccess;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Drawing;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.BusinessLogic.Logging;
using Prism.Logging;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class EpcManager
    {
        #region [GEOS2-5404][rdixit][13.08.2024]
        public EpcManager()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = System.Web.Hosting.HostingEnvironment.MapPath("/") + "log4net.config";
                    CreateIfNotExists(ApplicationLogFilePath);
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                    Log4NetLogger.Logger.Log(string.Format("EpcManager()..... Constructor Executed"), category: Category.Info, priority: Priority.Low);
                }
            }
            catch
            {
                throw;
            }
        }
        void CreateIfNotExists(string config_path)
        {
            string log4netConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <configuration>
                                          <configSections>
                                            <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net"" />
                                          </configSections>
                                          <log4net debug=""true"">
                                            <appender name=""RollingLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">
                                              <file value=""C:\Temp\LogsService.txt""/>
                                              <appendToFile value=""true"" />
                                              <rollingStyle value=""Size"" />
                                              <maxSizeRollBackups value=""10"" />
                                              <maximumFileSize value=""10MB"" />
                                              <staticLogFileName value=""true"" />
                                              <layout type=""log4net.Layout.PatternLayout"">
                                                <conversionPattern value=""%-5p %d %5rms - %m%n"" />
                                              </layout>
                                            </appender>
                                            <root>
                                              <level value=""Info"" />
                                              <appender-ref ref=""RollingLogFileAppender"" />
                                            </root>
                                          </log4net>
                                        </configuration>";

            if (!File.Exists(config_path))
            {
                File.WriteAllText(config_path, log4netConfig);
            }
        }
        #endregion

        /// <summary>
        /// Get LookupValues by lookupkey 
        /// </summary>
        /// <param name="lookupKey">Get lookup key</param>
        /// <returns>List of loop values</returns>
        public IList<LookupValue> GetLookupValues(byte key)
        {
            IList<LookupValue> list = null;

            using (var context = new EpcContext())
            {

                list = (from records in context.LookupValues where records.IdLookupKey == key select records).ToList();
            }

            return list;
        }

        /// <summary>
        /// This method is to get customer details by customer id
        /// </summary>
        /// <param name="idCustomer">Get customer id</param>
        /// <returns>Customer details</returns>
        public Customer GetCustomerByCustomerId(Int32 idCustomer)
        {
            Customer customer = null;

            using (var context = new EpcContext())
            {
                var idParameter = new MySqlParameter("@_customerId", idCustomer);
                customer = context.Customers.SqlQuery("GeosCustomersByCustomerId(@_customerId)", idParameter).FirstOrDefault();
            }


            return customer;
        }

        /// <summary>
        /// This method is to get list of code as per condition
        /// </summary>
        /// <param name="code">Get Code</param>
        /// <param name="roadMapType">Get RoadMapType</param>
        /// <param name="roadMapSource">Get RoadMapSource</param>
        /// <returns>List of code</returns>
        public IList<String> GetCode(string code,string roadMapType,string roadMapSource)
        {
            IList<String> codeList = null;

            using (var context = new EpcContext())
            {
                var paramCode = new MySqlParameter("@_Code", code);
                var paramRoadMapType = new MySqlParameter("@_RoadMapType", roadMapType);
                var paramRoadMapSource = new MySqlParameter("@_RoadMapSource", roadMapSource);
                codeList = context.Database.SqlQuery<String>("GetCode(@_Code,@_RoadMapType,@_RoadMapSource)", paramCode, paramRoadMapType, paramRoadMapSource).ToList();
            }

            return codeList;
        }

        /// <summary>
        /// This method is to get offer status type
        /// </summary>
        /// <param name="idOffer">Get id offer</param>
        /// <returns>List of offer status type</returns>
        public IList<GeosStatus> GetGeosOfferStatus()
        {
            IList<GeosStatus> geosStatus = null;
            using (var epcContext = new EpcContext())
            {
               geosStatus = epcContext.GeosStatus.SqlQuery("GeosOfferStatus").ToList();
            }
            return geosStatus;
        }

        /// <summary>
        /// This method is to get all teams
        /// </summary>
        /// <param name="idParentTeam">Get parent team id</param>
        /// <param name="isHierarchicalTeam">Get hierarchical team order or not</param>
        /// <returns>List of team</returns>
        public IList<Team> GetTeams(byte? idParentTeam = null, bool isHierarchicalTeam = false)
        {
            IList<Team> list = null;
            IList<Team> hieararchy=null;
            using (var context = new EpcContext())
            {
                if (idParentTeam == null)
                {
                    if (isHierarchicalTeam == true)
                    {
                        list = (from records in context.Teams select records).ToList();

                        hieararchy = list.Where(c => c.IdParent == idParentTeam)
                                    .Select(c => new Team()
                                    {
                                        IdTeam = c.IdTeam,
                                        Name = c.Name,
                                        IdParent = c.IdParent,
                                        UserTeams = c.UserTeams,
                                        ProjectTeams = c.ProjectTeams,
                                        ParentTeam = (from records in context.Teams where records.IdTeam == c.IdParent select records).FirstOrDefault(),
                                        Childrens = GetTeamChildren(list, c.IdTeam)
                                    })
                                    .ToList();

                        TeamHieararchyWalk(hieararchy);
                    }
                    else
                    {
                        hieararchy = (from records in context.Teams.Include("UserTeams").Include("ProjectTeams") select records).ToList();
                       // hieararchy = (from records in context.Teams.Include("UserTeams").Include("ProjectTeams") where records.IdParent == idParentTeam select records).ToList();
                    }
                }
                else
                {
                    if (isHierarchicalTeam == true)
                    {
                        list = (from records in context.Teams select records).ToList();

                        hieararchy = list.Where(c => c.IdParent == idParentTeam)
                                    .Select(c => new Team()
                                    {
                                        IdTeam = c.IdTeam,
                                        Name = c.Name,
                                        IdParent = c.IdParent,
                                        UserTeams = c.UserTeams,
                                        ProjectTeams = c.ProjectTeams,
                                        ParentTeam = (from records in context.Teams where records.IdTeam == c.IdParent select records).FirstOrDefault(),
                                        Childrens = GetTeamChildren(list, c.IdTeam)
                                    })
                                    .ToList();

                        TeamHieararchyWalk(hieararchy);
                    }
                    else
                    {
                        hieararchy = (from records in context.Teams.Include("UserTeams").Include("ProjectTeams") where records.IdParent == idParentTeam select records).ToList();
                    }
                }
            }
            return hieararchy;
        }

        /// <summary>
        /// This method is to get list of team children
        /// </summary>
        /// <param name="teams">Get list of team</param>
        /// <param name="parentId">Get parent team id in list of team</param>
        /// <returns>List of all team</returns>
        private IList<Team> GetTeamChildren(IList<Team> teams, byte parentId)
        {
            return teams
                    .Where(c => c.IdParent == parentId)
                    .Select(c => new Team
                    {
                        IdTeam = c.IdTeam,
                        Name = c.Name,
                        IdParent = c.IdParent,
                        ParentTeam = c.ParentTeam,
                        Childrens = GetTeamChildren(teams, c.IdTeam)
                    })
                    .ToList();
        }

      
        private void TeamHieararchyWalk(IList<Team> hierarchy)
        {
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    TeamHieararchyWalk(item.Childrens);
                }
            }
        }

        /// <summary>
        /// This method is to get projects related to product id
        /// </summary>
        /// <param name="idProduct">Get product id</param>
        /// <returns>List of project</returns>
        public IList<Project> GetProjectsByProductId(Int64 idProduct)
        {
            IList<Project> list = null;
            List<Customer> customers=new List<Customer>();
            using (var epccontext = new EpcContext())
            {
                    using (var geoscontext = new GeosContext())
                    {
                        list = epccontext.Projects.Include("Category").Where(project => project.IdProduct == idProduct).ToList();
                    if(list!=null || list.Count>0)
                    list = list.Select(projectRecord => 
                    {
                        projectRecord.GeosStatus = (from records in geoscontext.GeosStatus where records.IdOfferStatusType == projectRecord.IdGeosStatus select records).SingleOrDefault();
                        projectRecord.Customer = GetCustomerByCustomerId(projectRecord.IdCustomer);
                        return projectRecord;
                    }).ToList(); 
                   
                }

            }
            return list;
        }

        /// <summary>
        /// This method is to get list of all task working times related to user id and working date
        /// </summary>
        /// <param name="userId">Get Userid</param>
        /// <param name="workingDate">Get WorkingDate</param>
        /// <returns>List of task working times</returns>
        public IList<TaskWorkingTime> GetTaskWorkingTimeByDateAndUser(Int32? userId=null, DateTime? workingDate=null)
        {
            IList<TaskWorkingTime> taskWorkingTimes = null;
            using (var epccontext = new EpcContext())
            {
                if(userId==null && workingDate==null)
                  taskWorkingTimes = (from record in epccontext.TaskWorkingTimes.Include("ProjectTask") select record).ToList();
                else if (userId == null)
                    taskWorkingTimes = (from record in epccontext.TaskWorkingTimes.Include("ProjectTask") where EntityFunctions.TruncateTime(record.WorkingDate) == EntityFunctions.TruncateTime(workingDate) select record).ToList();
                else if (workingDate == null)
                    taskWorkingTimes = (from record in epccontext.TaskWorkingTimes.Include("ProjectTask") where record.IdUser == userId select record).ToList();
                else
                    taskWorkingTimes = (from record in epccontext.TaskWorkingTimes.Include("ProjectTask") where EntityFunctions.TruncateTime(record.WorkingDate) == EntityFunctions.TruncateTime(workingDate) && record.IdUser == userId select record).ToList();
            }
            return taskWorkingTimes;
        }

        /// <summary>
        /// This method is to get list of weekly task working time related to user id and working date
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <param name="workingDate">Get working date</param>
        /// <returns>List of task working time</returns>
        public IList<TaskWorkingTime> GetWeeklyTaskWorkingTime(Int32 userId, DateTime workingDate)
        {
            IList<TaskWorkingTime> taskWorkingTimes = null;
            using (var epccontext = new EpcContext())
            {
                DateTime start = workingDate.Date.AddDays(-(int)workingDate.DayOfWeek), // prev sunday 00:00
                         end = start.AddDays(7); // next sunday 00:00
              
                taskWorkingTimes = (from record in epccontext.TaskWorkingTimes
                          where (record.WorkingDate > start // include start
                           && record.WorkingDate <= end) && record.IdUser == userId 
                          select record).ToList();
           
            }
            return taskWorkingTimes;
        }

        /// <summary>
        /// This method is to get list of project task
        /// </summary>
        /// <returns>List of project task</returns>
        public IList<ProjectTask> GetTasksByTaskType()
        {
            IList<Int32> lookupvalues = null;
            IList<ProjectTask> projectTasks = null;
            using (var epccontext = new EpcContext())
            {
                using (var workbenchcontext = new WorkbenchContext())
                {
                    lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 12 select records.IdLookupValue).ToList();

                    projectTasks = (from record in epccontext.ProjectTasks.Include("TaskUsers")
                                    where lookupvalues.Contains(record.IdTaskType)
                                    select record).ToList();

                if(projectTasks!=null|| projectTasks.Count >0)
                     
                    projectTasks = projectTasks.Select(projecttask => 
                    {
                        projecttask.TaskUsers.Select(taskuser => 
                    {
                        taskuser.User = ((from records in workbenchcontext.Users where records.IdUser == taskuser.IdUser select records).SingleOrDefault());
                        return taskuser;
                    }).ToList();
                        return projecttask;
                    }).ToList();
                }
            }
            return projectTasks;
        }

        /// <summary>
        /// This method is to get list of task  related to working date and user id
        /// </summary>
        /// <param name="workingDate">Get working date </param>
        /// <param name="userId">Get user id</param>
        /// <returns>List of project task</returns>
        public IList<ProjectTask> GetTaskByWorkingDateAndUser(DateTime? workingDate = null,Int32? userId=null)
        {
            IList<ProjectTask> projectTasks = null;
            List<Int64> taskOfUsers =null;
            using (var epccontext = new EpcContext())
            {
              
                        if(workingDate == null && userId==null) 
                        {
                            taskOfUsers = epccontext.TaskWorkingTimes.Select(taskWorkingTime => taskWorkingTime.IdTask).Distinct().ToList();
                        }
                        else if(workingDate==null)
                        {
                            taskOfUsers = epccontext.TaskWorkingTimes.Where(taskWorkingTime => taskWorkingTime.IdUser == userId).Select(taskWorkingTime => taskWorkingTime.IdTask).Distinct().ToList();
                        }
                        else if(userId==null)
                        {
                            taskOfUsers = epccontext.TaskWorkingTimes.Where(taskWorkingTime => EntityFunctions.TruncateTime(taskWorkingTime.WorkingDate) == EntityFunctions.TruncateTime(workingDate)).Select(taskWorkingTime => taskWorkingTime.IdTask).Distinct().ToList();
                        }
                        else
                        {
                            taskOfUsers = epccontext.TaskWorkingTimes.Where(taskWorkingTime => taskWorkingTime.IdUser == userId && EntityFunctions.TruncateTime(taskWorkingTime.WorkingDate) == EntityFunctions.TruncateTime(workingDate)).Select(taskWorkingTime => taskWorkingTime.IdTask).Distinct().ToList();
                        }
                        projectTasks = epccontext.ProjectTasks.Include("Project").Where(projectTask => taskOfUsers.Contains(projectTask.IdTask)).ToList();
                   
            }
            return projectTasks;
        }

        /// <summary>
        /// This method is to get project details by project id
        /// </summary>
        /// <param name="idProject">Get project id</param>
        /// <returns>Get project details</returns>
        public Project GetProjectDetailByProjectId(Int32 idProject)
        {
            Project project = null;
            List<Customer> customers = null;
            using (var epccontext = new EpcContext())
            {
                using (var workbenchcontext = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        project = epccontext.Projects.Where(projectRecord => projectRecord.IdProject == idProject).SingleOrDefault();
                        if (project != null)
                        {
                            project.Category = (from records in epccontext.LookupValues where records.IdLookupValue == project.IdCategory select records).SingleOrDefault();
                            project.ProjectStatus = (from records in epccontext.LookupValues where records.IdLookupValue == project.IdProjectStatus select records).SingleOrDefault();
                            project.ProjectPriority = (from records in epccontext.LookupValues where records.IdLookupValue == project.IdProjectPriority select records).SingleOrDefault();
                            project.Team = (from records in epccontext.Teams where records.IdTeam == project.IdTeam select records).SingleOrDefault();
                            project.ProjectTeams = (from records in epccontext.ProjectTeams where records.IdProject == project.IdProject select records).ToList();
                            project.GeosStatus = (from records in geoscontext.GeosStatus where records.IdOfferStatusType == project.IdGeosStatus select records).SingleOrDefault();
                            project.Customer = GetCustomerByCustomerId(project.IdCustomer);
                            project.Offer = (from records in geoscontext.Offers where records.IdOffer == project.IdOffer select records).SingleOrDefault();
                            project.ProjectFollowups =new System.Collections.ObjectModel.ObservableCollection<ProjectFollowup>( (from records in epccontext.ProjectFollowups where records.IdProject == project.IdProject select records).ToList());
                        }
                    }
                }
            }
            return project;
        }

        /// <summary>
        /// This method is to get task working time
        /// </summary>
        /// <param name="idProject">Get project id</param>
        /// <returns>List of task working time</returns>
        public IList<TaskWorkingTime> GetProjectWorkingTimeByProjectId(Int64 idProject)
        {
            IList<TaskWorkingTime> taskWorkingTime = null;
         
            using (var epccontext = new EpcContext())
            {
                using (var workbenchcontext = new WorkbenchContext())
                {
                    var idParameter = new MySqlParameter("@_projectId", idProject);
                    taskWorkingTime = epccontext.TaskWorkingTimes.SqlQuery("GetProjectWorkingTimeByProjectId(@_projectId)", idParameter).ToList();
                    if(taskWorkingTime.Count>0 || taskWorkingTime!=null)
                    taskWorkingTime = taskWorkingTime.Select(taskworktime => { taskworktime.User = ((from records in workbenchcontext.Users where records.IdUser == taskworktime.IdUser select records).SingleOrDefault()); taskworktime.ProjectTask = ((from records in epccontext.ProjectTasks where records.IdTask == taskworktime.IdTask select records).SingleOrDefault()); return taskworktime; }).ToList();
                }
            }
            return taskWorkingTime;
        }

        /// <summary>
        /// This method is to get project deatils related to project id
        /// </summary>
        /// <param name="idProject">Get project id</param>
        /// <returns>Project details</returns>
        public Project GetProjectByProjectId(Int64 idProject)
        {
            Project project = null;
            using (var epccontext = new EpcContext())
            {
                using (var workbenchcontext = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        List<Int32> lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 12 select records.IdLookupValue).ToList();
                      
                        project = (from records in epccontext.Projects
                                   where records.IdProject == idProject
                                   select records).SingleOrDefault();
                        if (project != null)
                        {
                            project.ProjectMilestones = new System.Collections.ObjectModel.ObservableCollection<ProjectMilestone>((from records in epccontext.ProjectMilestones where records.IdProject == project.IdProject select records).ToList());

                            project.ProjectMilestones =new System.Collections.ObjectModel.ObservableCollection<ProjectMilestone>( project.ProjectMilestones.Select(projectMilestone => { projectMilestone.ProjectMilestoneDates = new System.Collections.ObjectModel.ObservableCollection<ProjectMilestoneDate>((from records in epccontext.ProjectMilestoneDates.Include("ProjectMilestoneStatus") orderby records.IdProjectMilestoneDate ascending where projectMilestone.IdProjectMilestone == records.IdProjectMilestone select records).ToList()); return projectMilestone; }).ToList());
                            project.ProjectAnalysis = (from records in epccontext.ProjectAnalysis.Include("AnalysisPriority").Include("AnalysisStatus") where records.IdProject == project.IdProject select records).ToList();
                            project.Category = (from records in epccontext.LookupValues where records.IdLookupValue == project.IdCategory select records).SingleOrDefault();
                            project.ProjectStatus = (from records in epccontext.LookupValues where records.IdLookupValue == project.IdProjectStatus select records).SingleOrDefault();
                            project.ProjectPriority = (from records in epccontext.LookupValues where records.IdLookupValue == project.IdProjectPriority select records).SingleOrDefault();
                            project.Team = (from records in epccontext.Teams where records.IdTeam == project.IdTeam select records).SingleOrDefault();
                            project.ProjectTeams = (from records in epccontext.ProjectTeams.Include("Team.ParentTeam") where records.IdProject == project.IdProject select records).ToList();
                            project.GeosStatus = (from records in geoscontext.GeosStatus where records.IdOfferStatusType == project.IdGeosStatus select records).SingleOrDefault();
                            project.Customer = GetCustomerByCustomerId(project.IdCustomer);
                            project.Offer = (from records in geoscontext.Offers where records.IdOffer == project.IdOffer select records).SingleOrDefault();
                            project.ProjectTasks = new List<ProjectTask> (from record in epccontext.ProjectTasks
                                                    where lookupvalues.Contains(record.IdTaskType) && record.IdProject == idProject
                                                    select record).ToList();
                            project.ProjectTasks = project.ProjectTasks.Select(projectTask => { projectTask.TaskUsers = new System.Collections.ObjectModel.ObservableCollection<TaskUser>( (from records in epccontext.TaskUsers where records.IdTask == projectTask.IdTask select records).ToList()); return projectTask; }).ToList();
                            project.ProjectTasks = project.ProjectTasks.Select(projectTask => { projectTask.TaskWatchers =new System.Collections.ObjectModel.ObservableCollection<TaskWatcher>( (from records in epccontext.TaskWatchers where records.IdTask == projectTask.IdTask select records).ToList()); return projectTask; }).ToList();
                            project.ProjectTasks.Select(projecttask =>
                            {
                                projecttask.TaskUsers.Select(taskuser =>
                                {
                                    taskuser.User = ((from records in workbenchcontext.Users where records.IdUser == taskuser.IdUser select records).SingleOrDefault());
                                    return taskuser;
                                }).ToList();
                                return projecttask;
                            }).ToList();
                           project.ProjectFollowups =new System.Collections.ObjectModel.ObservableCollection<ProjectFollowup> ((from records in epccontext.ProjectFollowups where records.IdProject == project.IdProject select records).ToList());
                            if(project.ProjectFollowups.Count>0 || project.ProjectFollowups!=null)
                            project.ProjectFollowups = new System.Collections.ObjectModel.ObservableCollection<ProjectFollowup>(project.ProjectFollowups.Select(projectfollowup => { projectfollowup.User = (from records in workbenchcontext.Users where records.IdUser == projectfollowup.IdUser select records).SingleOrDefault(); return projectfollowup; }).ToList());
                        }
                    }
                }
            }
            return project;
        }

        /// <summary>
        /// This method is to get project code
        /// </summary>
        /// <returns>Get project code</returns>
        public string GetProjectCode()
        {
            Config config = null;
            string projectCode = null;
            try
            {
                using (var db = new EpcContext())
                {
                    config = (from records in db.Configs select records).FirstOrDefault();
                    int code = Convert.ToInt32(config.ConfigValue.ToString()) + 1;
                    projectCode = DateTime.Today.Year + "-" + "PR" + code;
                    config.ConfigValue = code.ToString();
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                
               
            }
         
            return projectCode;
        }

        /// <summary>
        /// This method is to add project
        /// </summary>
        /// <param name="project">Get project details to add</param>
        /// <returns>Added project details</returns>
        public Project AddProject(Project project,string projectDirectoryPath, List<byte> teamIds)
        {
            string projectType = "";
            try
            {
                project.ProjectCode = GetProjectCode();
                project.ProjectPath = projectDirectoryPath + DateTime.Now.Year.ToString() + @"\" + project.ProjectCode;
                using (var db = new EpcContext())
                {
                    projectType = (from records in db.LookupValues where records.IdLookupValue == project.IdProjectType select records.Value).SingleOrDefault();
                    project = db.Projects.Add(project);
                    db.SaveChanges();
                    foreach (byte item in teamIds)
                    {
                        ProjectTeam projectTeam = new ProjectTeam();
                        projectTeam.IdProject = project.IdProject;
                        projectTeam.IdTeam = item;
                        db.ProjectTeams.Add(projectTeam);
                    }
                    db.SaveChanges();
                }
                CreateProjectDirectory(project.ProjectPath, projectType, project);

            }
            catch (Exception ex)
            {
            }
            return project;
        }
       
        /// <summary>
        /// This method is to add task comment
        /// </summary>
        /// <param name="taskComment">Get task comment details to add</param>
        /// <returns>Get added task comment details</returns>
        public TaskComment AddTaskComment(TaskComment taskComment)
        {
            try
            {
                using (var db = new EpcContext())
                {
                    db.TaskComments.Add(taskComment);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
            return taskComment;
        }

        /// <summary>
        /// This method is to add task assistance
        /// </summary>
        /// <param name="taskAssistance">Get task assistance details to add</param>
        /// <returns>Get added task assistance details</returns>
        public TaskAssistance AddTaskAssistance(TaskAssistance taskAssistance)
        {
            try
            {
               
                using (var db = new EpcContext())
                {
                    db.TaskAssistances.Add(taskAssistance);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
              
            return taskAssistance;
        }

        /// <summary>
        /// This method is to add task attachment
        /// </summary>
        /// <param name="taskAttachment">Get task attachment details to add</param>
        /// <returns>Get added task attachment details</returns>
        public TaskAttachment AddTaskAttachment(TaskAttachment taskAttachment,string projectDirectoryPath)
        {
            string projectType = "";
            bool isUpload = false;
            try
            {
                using (var db = new EpcContext())
                {
                    ProjectTask projectTask = (from records in db.ProjectTasks.Include("Project") where records.IdTask == taskAttachment.IdTask select records).SingleOrDefault();
                    if(projectTask!=null)
                        projectType = (from records in db.LookupValues where records.IdLookupValue == projectTask.Project.IdProjectType select records.Value).SingleOrDefault();

                    string[] splitProjectCode = projectTask.Project.ProjectCode.Split('-');
                    string folderPath = projectDirectoryPath + splitProjectCode[0] + @"\" + projectTask.Project.ProjectCode + @"\" + projectType + @"\Development";
                    string fileName = taskAttachment.IdTask + " - " + taskAttachment.FileName;
                    if (Directory.Exists(folderPath))
                    {
                        folderPath += @"\" + fileName;
                        isUpload=SaveTaskAttachmentInProjectFolder(folderPath);
                    }
                    if (isUpload)
                    {
                        taskAttachment.FilePath = splitProjectCode[0] + @"\" + projectTask.Project.ProjectCode + @"\" + projectType + @"\Development" + @"\" + taskAttachment.IdTask + " - " + taskAttachment.FileName;
                        taskAttachment.FileName = taskAttachment.IdTask + " - " + taskAttachment.FileName;
                        db.TaskAttachments.Add(taskAttachment);
                        db.SaveChanges();
                    }
                   
                }
            }
            catch (Exception ex)
            {
            }

            return taskAttachment;
        }

        private bool SaveTaskAttachmentInProjectFolder(string folderPath)
        {
            bool isUpload = false;
            try
            {
                if (File.Exists(folderPath))
                  File.Delete(folderPath);
                 File.Create(folderPath);
                isUpload = true;
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                isUpload = false;
            }
            catch (Exception ex)
            {
                isUpload = false;
            }

            return isUpload;
        }
        public ProjectScope AddProjectScope(ProjectScope projectScope, string projectDirectoryPath)
        {
            bool isUpload = false;
            try
            {
                isUpload = CreateProjectScope(projectScope, projectDirectoryPath);
                if (isUpload)
                {
                    using (var db = new EpcContext())
                    {
                    
                        projectScope.CreationDate = DateTime.Now;
                        db.ProjectScopes.Add(projectScope);
                      
                        ProjectAnalysis projectAnalysis = new ProjectAnalysis();
                        projectAnalysis = db.ProjectAnalysis.Where(projectanalysis => projectanalysis.IdProject == projectScope.IdProject).FirstOrDefault();
                        if(projectAnalysis!=null)
                        projectAnalysis.IsScope = true;
                        db.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
            }

            return projectScope;
        }

        private byte[] GetOldProjectScopeBytes(string filePath)
        {
            byte[] bytes = null;
        
            try
            {
                using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
           
            catch (Exception ex)
            {
             
            }

            return bytes;
        }

        private bool UploaderProjectScopeFile(byte[] fileBytes,string filePath)
        {
            bool isUpload = false;
            FileStream fileStream = null;
         
                try
                {
                    if (fileBytes.Length > 0 && fileBytes != null)
                    {
                        if (!string.IsNullOrEmpty(filePath))
                        {

                            fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                            // write file stream into the specified file  
                            using (System.IO.FileStream fs = fileStream)
                            {
                                fs.Write(fileBytes, 0, fileBytes.Length);
                                isUpload = true;
                            }

                        }
                    }
                }

            catch (Exception ex)
            {
                isUpload = false;
            }

            return isUpload;
        }

        public bool CreateProjectScope(ProjectScope projectScope, string projectDirectoryPath)
        {
            bool isUpload = false;
            string projectType = "";
            try
            {
                using (var db = new EpcContext())
                {
                    Project project = (from records in db.Projects where records.IdProject == projectScope.IdProject select records).SingleOrDefault();
                    if (project != null)
                        projectType = (from records in db.LookupValues where records.IdLookupValue == project.IdProjectType select records.Value).SingleOrDefault();

                    string[] splitProjectCode = project.ProjectCode.Split('-');
                    string folderPath = projectDirectoryPath + splitProjectCode[0] + @"\" + project.ProjectCode + @"\" + projectType + @"\Analysis";
                    if (!Directory.Exists(folderPath + @"\ProjectScope"))
                        Directory.CreateDirectory(folderPath + @"\ProjectScope");
                    string createProjectScopeFile = folderPath + @"\ProjectScope\" + ((projectScope.Offer != null) ? projectScope.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + ".pdf";
                    string fileUploadPath = "";
                    if (System.IO.File.Exists(createProjectScopeFile))
                    {
                       byte[] getOldFileBytes= GetOldProjectScopeBytes(createProjectScopeFile);
                        if (!System.IO.Directory.Exists(folderPath + @"\ProjectScope" + @"\Old"))
                        {
                            Directory.CreateDirectory(folderPath + @"\ProjectScope" + @"\Old");
                        }
                         fileUploadPath = folderPath + @"\ProjectScope" + @"\Old\" + ((projectScope.Offer != null) ? projectScope.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
                         isUpload= UploaderProjectScopeFile(getOldFileBytes, fileUploadPath);
                        File.Delete(createProjectScopeFile);
                        isUpload = UploaderProjectScopeFile(projectScope.ScopeFileBytes, createProjectScopeFile);
                    }
                    else
                    {
                        fileUploadPath = createProjectScopeFile;
                        isUpload = UploaderProjectScopeFile(projectScope.ScopeFileBytes, fileUploadPath);
                    }
                  
                    //System.IO.File.Move(Path.Combine(folderPath + @"\ProjectScope", ((projectScope.Offer != null) ? projectScope.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + ".pdf"),
                    //                    Path.Combine(folderPath + @"\Old\", ((projectScope.Offer != null) ? projectScope.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"));
                }

                }
         
            catch (Exception ex)
            {

            }
            return isUpload;
        }

        public bool UpdateProjectScope(ProjectScope projectScope,string projectDirectoryPath)
        {
            bool isUpdated = false;

            bool isUpload = false;
            try
            {
                isUpload = CreateProjectScope(projectScope, projectDirectoryPath);
                if (isUpload)
                {
                    using (var db = new EpcContext())
                    {
                        ProjectScope UpdatedProjectScope = (from records in db.ProjectScopes
                                                            where records.IdProjectScope == projectScope.IdProjectScope
                                                            select records).SingleOrDefault();
                        UpdatedProjectScope.ModificationDate = DateTime.Now;
                        UpdatedProjectScope.ProductScopeDescription = projectScope.ProductScopeDescription;
                        UpdatedProjectScope.ProjectAcceptanceCriteria = projectScope.ProjectAcceptanceCriteria;
                        UpdatedProjectScope.ProjectAssumptions = projectScope.ProjectAssumptions;
                        UpdatedProjectScope.ProjectConstraints = projectScope.ProjectConstraints;
                        UpdatedProjectScope.ProjectDeliverables = projectScope.ProjectDeliverables;
                        UpdatedProjectScope.ProjectExclusions = projectScope.ProjectExclusions;
                        UpdatedProjectScope.IdProductVersion = projectScope.IdProductVersion;
                        db.SaveChanges();
                        isUpdated = true;
                    }
                }
            }
            catch (Exception)
            {
                isUpdated = false;
            }

            return isUpdated;
        }

        public ProjectScope GetProjectScopeByProjectId(Int64 idProject)
        {
            ProjectScope projectScope = null;
            try
            {
                using (var db = new EpcContext())
                {
                    using (var dbWorkbench = new WorkbenchContext())
                    {
                        using (var dbGeos = new GeosContext())
                        {
                            projectScope = (from records in db.ProjectScopes where records.IdProject == idProject select records).FirstOrDefault();
                            if (projectScope != null)
                            {
                                projectScope.Offer = (from records in dbGeos.Offers where records.IdOffer == projectScope.IdOffer select records).SingleOrDefault();
                                projectScope.Project = (from records in db.Projects where records.IdProject == projectScope.IdProject select records).SingleOrDefault();
                                projectScope.User = (from records in dbWorkbench.Users where records.IdUser == projectScope.CreatedBy select records).SingleOrDefault();
                                projectScope.ProductVersion = (from records in db.ProductVersions where records.IdProductVersion == projectScope.IdProductVersion select records).SingleOrDefault();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return projectScope;
        }

        /// <summary>
        /// This method is to add project milestone
        /// </summary>
        /// <param name="projectMilestone">Get project milestone details to add</param>
        /// <returns>Get added project milestone details</returns>
        public ProjectMilestone AddProjectMilestone(ProjectMilestone projectMilestone)
        {
            try
            {
                using (var db = new EpcContext())
                {
                    projectMilestone=db.ProjectMilestones.Add(projectMilestone);
                    db.SaveChanges();
                }

                using (var db = new EpcContext())
                {
                    ProjectMilestoneDate projectMilestoneDate = new ProjectMilestoneDate();
                    projectMilestoneDate.IdProjectMilestone = projectMilestone.IdProjectMilestone;
                    projectMilestoneDate.IdProjectMilestoneDate = 0;
                    projectMilestoneDate.TargetDate = projectMilestone.TargetDate;
                    projectMilestoneDate.IdProjectMilestoneStatus = projectMilestone.IdProjectMilestoneStatus;
                    projectMilestoneDate.Comments = projectMilestone.Comments;
                    db.ProjectMilestoneDates.Add(projectMilestoneDate);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
            return projectMilestone;
        }

        /// <summary>
        /// This method is to add project milestone date
        /// </summary>
        /// <param name="projectMilestoneDate">Get project milestone date details to add</param>
        /// <returns>Get added project milestone details</returns>
        public ProjectMilestoneDate AddProjectMilestoneDate(ProjectMilestoneDate projectMilestoneDate)
        {
            try
            {
                using (var db = new EpcContext())
                {
                    projectMilestoneDate = db.ProjectMilestoneDates.Add(projectMilestoneDate);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
            }
            return projectMilestoneDate;
        }

        /// <summary>
        /// This method is to get task attachment
        /// </summary>
        /// <param name="idTask">Get task id</param>
        /// <param name="directoryPath">Get directory path</param>
        /// <returns>Get task attachment in bytes</returns>
        public byte[] GetTaskAttachment(string taskAttachmentFilePath,string directoryPath)
        {
            byte[] bytes = null;

            if (taskAttachmentFilePath != null)
            {
                string filepath;
                filepath = directoryPath + taskAttachmentFilePath;

                // open stream
                using ( System.IO.FileStream stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

        /// <summary>
        /// This method is to get task attachment details related to id task
        /// </summary>
        /// <param name="idTask">Get id task </param>
        /// <returns>Get task attachment details related to id task</returns>
        public IList<TaskAttachment> GetTaskAttachmentByTaskId(Int64 idTask, string directoryPath)
        {
            IList<TaskAttachment> taskAttachments = null;
            try
            {
                using (var db = new EpcContext())
                {
                    taskAttachments= db.TaskAttachments.Where(taskattachment => taskattachment.IdTask == idTask).ToList();
                    foreach (TaskAttachment taskAttachment in taskAttachments)
                    {
                        Icon iconForFile = SystemIcons.WinLogo;
                        try
                        {
                            if (taskAttachment.FilePath != null)
                            {
                                iconForFile = Icon.ExtractAssociatedIcon(directoryPath + taskAttachment.FilePath);
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    iconForFile.Save(ms);
                                    taskAttachment.FileByte = ms.ToArray();
                                }
                            }
                        }
                        catch (Exception)
                        {

                           
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
       
            return taskAttachments;
        }

        /// <summary>
        /// This method is to create project directory 
        /// </summary>
        /// <param name="projectDirectoryPath">Get project directory path to create project directory</param>
        public void CreateProjectDirectory(string projectDirectoryPath,string ProjectType, Project project)
        {
       
            List<LookupValue> lookupvaluetasktypes = null;
            try
            {
                if (!Directory.Exists(projectDirectoryPath))
                {
                    string projectPath = projectDirectoryPath+ @"\"+ ProjectType;
                    Directory.CreateDirectory(projectPath);
                    using (var epcContext = new EpcContext())
                    {
                        lookupvaluetasktypes = (from records in epcContext.LookupValues where records.IdLookupKey == 12 select records).ToList();
                        foreach (LookupValue tasktype in lookupvaluetasktypes)
                        {
                            string projectsubtypepath = projectPath + @"\" + tasktype.Value;
                            Directory.CreateDirectory(Path.Combine(projectsubtypepath));

                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// This method is to add task watcher
        /// </summary>
        /// <param name="taskWatcher">Get task watcher to add</param>
        /// <returns>Get added task watcher details </returns>
        public TaskWatcher AddTaskWatcher(TaskWatcher taskWatcher)
        {
            try
            {
                using (var db = new EpcContext())
                {
                    db.TaskWatchers.Add(taskWatcher);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
            }
            return taskWatcher;
        }

        /// <summary>
        /// This method is to delete task watcher realted to task watcher id
        /// </summary>
        /// <param name="idTaskWatcher">Get id task watcher</param>
        /// <returns>Get isdeleted or not</returns>
        public bool DeleteTaskWatcherById(Int64 idTaskWatcher)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    TaskWatcher taskwatcher = (from records in db.TaskWatchers
                                              where records.IdTaskWatcher == idTaskWatcher
                                               select records).SingleOrDefault();
                    db.TaskWatchers.Remove(taskwatcher);
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception)
            {

                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to get task working time
        /// </summary>
        /// <param name="idTaskWorkingTime">Get id task working time</param>
        /// <returns>Deleted or not</returns>
        public bool DeleteTaskWorkingTimeById(Int64 idTaskWorkingTime)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    TaskWorkingTime taskWorkingTime = (from records in db.TaskWorkingTimes
                                               where records.IdTaskWorkingTime == idTaskWorkingTime
                                               select records).SingleOrDefault();
                    db.TaskWorkingTimes.Remove(taskWorkingTime);
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception)
            {

                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to delete task working time by id list
        /// </summary>
        /// <param name="taskWorkingTimes">Get list of task working time</param>
        /// <returns>isDeleted or not </returns>
        public bool DeleteTaskWorkingTimeByIdList(List<TaskWorkingTime> taskWorkingTimes)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    foreach (var item in taskWorkingTimes)
                    {
                        TaskWorkingTime taskWorkingTime = (from records in db.TaskWorkingTimes
                                                           where records.IdTaskWorkingTime == item.IdTaskWorkingTime
                                                           select records).SingleOrDefault();
                        db.TaskWorkingTimes.Remove(taskWorkingTime);
                    }
                
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception)
            {

                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to delete task comment by task comment id
        /// </summary>
        /// <param name="idTaskComment">Get task comment id</param>
        /// <returns>Is deleted or not</returns>
        public bool DeleteTaskCommentById(Int64 idTaskComment)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    TaskComment taskcomment = (from records in db.TaskComments
                                               where records.IdTaskComment == idTaskComment
                                               select records).SingleOrDefault();
                    db.TaskComments.Remove(taskcomment);
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception)
            {

                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to delete task attachment related to task attachment id
        /// </summary>
        /// <param name="idTaskAttachment">Get Task attachment id</param>
        /// <param name="directoryPath">Get directoryPath</param>
        /// <returns>Is Deleted or not</returns>
        public bool DeleteTaskAttachmentById(Int64 idTaskAttachment,string directoryPath)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    TaskAttachment taskattachment = (from records in db.TaskAttachments
                                                     where records.IdTaskAttachment == idTaskAttachment
                                                     select records).SingleOrDefault();
                    if (taskattachment != null)
                    {
                        if (File.Exists(Path.Combine(directoryPath , taskattachment.FileName)))
                        {
                            File.Delete(Path.Combine(directoryPath , taskattachment.FileName));
                        }

                        db.TaskAttachments.Remove(taskattachment);
                        db.SaveChanges();
                        isDeleted = true;
                    }
                }
            }
            catch (Exception)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

     
    /// <summary>
    /// This method is to delete task assistance by id
    /// </summary>
    /// <param name="idTaskAssistance">Get task assistance id</param>
    /// <returns>is deleted or not</returns>
    public bool DeleteTaskAssistanceById(Int64 idTaskAssistance)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    TaskAssistance taskassistance = (from records in db.TaskAssistances
                                               where records.IdTaskAssistance  == idTaskAssistance
                                                     select records).SingleOrDefault();
                    db.TaskAssistances.Remove(taskassistance);
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to add project task
        /// </summary>
        /// <param name="projectTask">Get project task deatils to add</param>
        /// <returns>Get added project task details</returns>
        public ProjectTask AddProjectTask(ProjectTask projectTask)
        {
            try
            {
                using (var db = new EpcContext())
                {
                    db.ProjectTasks.Add(projectTask);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
            return projectTask;
        }

        /// <summary>
        /// This method is to add project followup
        /// </summary>
        /// <param name="projectFollowup">Get project followup details to add</param>
        /// <returns>Get added project followup details</returns>
        public ProjectFollowup AddProjectFollowup(ProjectFollowup projectFollowup)
        {
            try
            {
                using (var db = new EpcContext())
                {
                    db.ProjectFollowups.Add(projectFollowup);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
            return projectFollowup;
        }

        /// <summary>
        /// This method is to add working hours in task
        /// </summary>
        /// <param name="taskWorkingTime">Get task working time details to add</param>
        /// <returns>Get added task working time</returns>
        public TaskWorkingTime AddWorkingHoursInTask(TaskWorkingTime taskWorkingTime)
        {
            try
            {
                using (var db = new EpcContext())
                {
                    db.TaskWorkingTimes.Add(taskWorkingTime);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
            }
            return taskWorkingTime;
        }

        /// <summary>
        /// This method is to add working hours in task list
        /// </summary>
        /// <param name="taskWorkingTime">Get list of task working time details to add</param>
        /// <returns>Added list of task working time details</returns>
        public List<TaskWorkingTime> AddWorkingHoursInTaskList(List<TaskWorkingTime> taskWorkingTime)
        {
            try
            {
                using (var db = new EpcContext())
                {
                    db.TaskWorkingTimes.AddRange(taskWorkingTime);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
            }
            return taskWorkingTime;
        }

        /// <summary>
        /// This method is to update working hours in task
        /// </summary>
        /// <returns>Get is updated or not</returns>
        public bool UpdateWorkingHoursInTask(TaskWorkingTime taskWorkingTime)
        {
            bool isUpdated = false;
            try
            {
                using (var db = new EpcContext())
                {

                    var workingTimeInHours = new MySqlParameter("@_WorkingTimeInHours", taskWorkingTime.WorkingTimeInHours);
                    var datetime = new MySqlParameter("@_WorkingTimedatetime", taskWorkingTime.WorkingDate);
                    var idTaskWorkingTime = new MySqlParameter("@_IdTaskWorkingTime", taskWorkingTime.IdTaskWorkingTime);

                    db.Database.ExecuteSqlCommand("UpdateWorkingHoursInTask(@_WorkingTimeInHours,@_IdTaskWorkingTime,@_WorkingTimedatetime)", workingTimeInHours, idTaskWorkingTime, datetime);

                    isUpdated = true;
                }
            }
            catch (Exception)
            {
                isUpdated = false; 
            }
            return isUpdated;
        }


        /// <summary>
        /// This method is to update working hours in task list
        /// </summary>
        /// <param name="taskWorkingTimes">Get list of task working times details</param>
        /// <returns>updated or not</returns>
        public bool UpdateWorkingHoursInTaskList(List<TaskWorkingTime> taskWorkingTimes)
        {
            bool isUpdated = false;
            try
            {
                using (var db = new EpcContext())
                {
                    foreach (TaskWorkingTime item in taskWorkingTimes)
                    {
                        var workingTimeInHours = new MySqlParameter("@_WorkingTimeInHours", item.WorkingTimeInHours);
                        var datetime = new MySqlParameter("@_WorkingTimedatetime", item.WorkingDate);
                        var idTaskWorkingTime = new MySqlParameter("@_IdTaskWorkingTime", item.IdTaskWorkingTime);

                        db.Database.ExecuteSqlCommand("UpdateWorkingHoursInTask(@_WorkingTimeInHours,@_IdTaskWorkingTime,@_WorkingTimedatetime)", workingTimeInHours, idTaskWorkingTime, datetime);
                    }

                    isUpdated = true;
                }
            }
            catch (Exception)
            {
                isUpdated = false;
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to update project by project id
        /// </summary>
        /// <param name="project">Get project details to update</param>
        /// <returns>Updated project or not</returns>
        public bool UpdateProjectById(Project project)
        {
            bool isUpdated = false;
            List<LookupValue> lookupvaluetasktypes = new List<LookupValue>();
            try
            {
                using (var dbepccontext = new EpcContext())
                {

                    Project projectById = (from records in dbepccontext.Projects
                                        where records.IdProject == project.IdProject
                                        select records).SingleOrDefault();
                   
                    projectById.IdProject = project.IdProject;
                    projectById.IdCategory = project.IdCategory;
                    projectById.Description = project.Description;
                    projectById.ProjectPath = project.ProjectPath;
                    projectById.ProjectName = project.ProjectName;
                    projectById.IdProjectPriority = project.IdProjectPriority;
                    projectById.IdProduct = project.IdProduct;
                    projectById.IdOwner = project.IdOwner;
                    projectById.IdProjectStatus = project.IdProjectStatus;
                    projectById.GeosPath = project.GeosPath;
                    projectById.IdGeosStatus = project.IdGeosStatus;
                    projectById.IdCustomer = project.IdCustomer;
                    projectById.DueDate = project.DueDate;
                    projectById.StartDate = project.StartDate;
                    dbepccontext.SaveChanges();

                    if (project.IdProjectStatus == 7)
                    {
                        lookupvaluetasktypes = (from records in dbepccontext.LookupValues where records.IdLookupKey == 12 select records).ToList();
                        foreach (LookupValue tasktype in lookupvaluetasktypes)
                        {
                            ProjectTask projectTask = new ProjectTask();
                            projectTask.IdProject = project.IdProject;
                            projectTask.TaskTitle = tasktype.Value;
                            projectTask.Description = tasktype.Value;
                            projectTask.StartDate = DateTime.Now;
                            projectTask.IdTaskType = tasktype.IdLookupValue;
                            projectTask.IdTaskPriority = 38;
                            projectTask.IdTaskStatus = 42;
                            projectTask.PlannedHours = 1;
                            projectTask.WorkedHours = 0;
                            projectTask.IdProductRoadmap = 0;
                            projectTask.EffortPoints = 0;
                            projectTask.IdOwner = 0;
                            dbepccontext.ProjectTasks.Add(projectTask);
                            dbepccontext.SaveChanges();
                        }
                    }

                    isUpdated = true;
                }
            }
            catch (Exception)
            {
                isUpdated = false;
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to delete project by project id
        /// </summary>
        /// <param name="idProject">Get project id </param>
        /// <returns>isDeleted or not</returns>
        public bool DeleteProjectById(Int64 idProject)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    Project project = (from records in db.Projects
                                        where records.IdProject == idProject
                                        select records).SingleOrDefault();
                    db.Projects.Remove(project);
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to delete project milestone date by id
        /// </summary>
        /// <param name="idProjectMilestoneDate">Get project milestone date id</param>
        /// <returns>isDeleted or not</returns>
        public bool DeleteProjectMilestoneDateById(Int64 idProjectMilestoneDate)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    ProjectMilestoneDate projectMilestoneDate = (from records in db.ProjectMilestoneDates
                                       where records.IdProjectMilestoneDate == idProjectMilestoneDate
                                       select records).SingleOrDefault();
                    db.ProjectMilestoneDates.Remove(projectMilestoneDate);
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to update project milestone date by id
        /// </summary>
        /// <param name="projectMilestoneDate">Get project milestone date details</param>
        /// <returns>isUpdated or not</returns>
        public ProjectMilestoneDate UpdateProjectMilestoneDateById(ProjectMilestoneDate projectMilestoneDate)
        {
            ProjectMilestoneDate updateprojectMilestoneDate = null;
            try
            {
                using (var db = new EpcContext())
                {
                    updateprojectMilestoneDate = (from records in db.ProjectMilestoneDates
                                                                 where records.IdProjectMilestoneDate == projectMilestoneDate.IdProjectMilestoneDate
                                                                       select records).SingleOrDefault();
                    updateprojectMilestoneDate.IdProjectMilestoneStatus = projectMilestoneDate.IdProjectMilestoneStatus;
                    db.SaveChanges();
                    updateprojectMilestoneDate.ProjectMilestoneStatus = (from records in db.LookupValues where records.IdLookupValue == projectMilestoneDate.IdProjectMilestoneStatus select records).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
               
            }
            return updateprojectMilestoneDate;
        }

        /// <summary>
        /// This method is to delete project followup related to idProjectFollowup
        /// </summary>
        /// <param name="idProjectFollowup">Get idProjectFollowup</param>
        /// <returns>isDeleted or not</returns>
        public bool DeleteProjectFollowupById(Int64 idProjectFollowup)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    ProjectFollowup projectFollowup = (from records in db.ProjectFollowups
                                       where records.IdProjectFollowup == idProjectFollowup
                                       select records).SingleOrDefault();
                    db.ProjectFollowups.Remove(projectFollowup);
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to update project status by project id and status id
        /// </summary>
        /// <param name="project">Get project details</param>
        /// <param name="idStatus">Get status id</param>
        /// <returns>isUpdated or not</returns>
        public bool UpdateProjectStatusById(Project project, Int32 idStatus)
        {
            bool isUpdated = false;
            try
            {
                using (var db = new EpcContext())
                {
                    Project projectById = (from records in db.Projects
                                        where records.IdProject == project.IdProject
                                        select records).SingleOrDefault();

                    projectById.IdProjectStatus = idStatus;
                    db.SaveChanges();
                    isUpdated = true;
                }
            }
            catch (Exception)
            {

                isUpdated = false;
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to update project product
        /// </summary>
        /// <param name="idProject">Get idproject</param>
        /// <param name="idProduct">Get idProduct</param>
        /// <returns>isUpdated or not</returns>
        public bool UpdateProjectProduct(Int64 idProject, Int64 idProduct)
        {
            bool isUpdated = false;
            try
            {
                using (var db = new EpcContext())
                {
                    db.Database.ExecuteSqlCommand("update projects set IdProduct=" + idProduct + " where IdProject=" + idProject + ";");

                    isUpdated = true;
                }
            }
            catch (Exception)
            {

                isUpdated = false;
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to add project roadmap
        /// </summary>
        /// <param name="productRoadmap">Get product roadmap details to add</param>
        /// <returns>Get product roadmap id</returns>
        public Int64 AddProductRoadmap(ProductRoadmap productRoadmap)
        {
            try
            {
                using (var db = new EpcContext())
                {
                    db.ProductRoadmaps.Add(productRoadmap);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
            }
            return productRoadmap.IdProductRoadmap;
        }

        /// <summary>
        /// This method is to update product roadmap
        /// </summary>
        /// <param name="productRoadmap">Get product roadmap details</param>
        /// <returns>Updated or not</returns>
        public bool UpdateProductRoadmapById(ProductRoadmap productRoadmap)
        {
            bool isUpdated = false;
            try
            {
                using (var db = new EpcContext())
                {
                    ProductRoadmap productRoadmapById = (from records in db.ProductRoadmaps
                                                         where records.IdProductRoadmap == productRoadmap.IdProductRoadmap
                                                         select records).SingleOrDefault();
                    productRoadmapById.IdProductRoadmap = productRoadmap.IdProductRoadmap;
                    productRoadmapById.IdRoadmapPriority = productRoadmap.IdRoadmapPriority;
                    productRoadmapById.Description = productRoadmap.Description;
                    productRoadmapById.IdProduct = productRoadmap.IdProduct;
                    productRoadmapById.IdRoadmapSource = productRoadmap.IdRoadmapSource;
                    productRoadmapById.IdRoadmapStatus = productRoadmap.IdRoadmapStatus;
                    productRoadmapById.IdProductRoadmapType = productRoadmap.IdProductRoadmapType;
                    productRoadmapById.LinkedTo = productRoadmap.LinkedTo;
                    productRoadmapById.RequestDate = productRoadmap.RequestDate;
                    productRoadmapById.SourceFrom = productRoadmap.SourceFrom;
                    productRoadmapById.Title = productRoadmap.Title;
                    db.SaveChanges();
                    isUpdated = true;
                }
            }
            catch (Exception)
            {
                isUpdated = false;
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to delete product roadmap by id
        /// </summary>
        /// <param name="idProductRoadmap">Get product roadmap id</param>
        /// <returns>Deleted or not</returns>
        public bool DeleteProductRoadmapById(Int64 idProductRoadmap)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new EpcContext())
                {
                    ProductRoadmap productRoadmap = (from records in db.ProductRoadmaps
                                                     where records.IdProductRoadmap == idProductRoadmap
                                                     select records).SingleOrDefault();
                    db.ProductRoadmaps.Remove(productRoadmap);
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception)
            {

                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to get list of open project
        /// </summary>
        /// <returns>List of open project</returns>
        public IList<Project> GetOpenProjectOnBoard()
        {
            IList<string > lookupvalues = null;
            IList<Project> project = null;
            using (var epccontext = new EpcContext())
            {

                using (var workbenchcontext = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 4 select records.Value).ToList();
                        IList<int> convertstringtointArray = lookupvalues.Select((s, i) => int.TryParse(s, out i) ? i : 0).ToArray();

                        project = (from records in epccontext.Projects.Include("ProjectStatus").Include("Category") where convertstringtointArray.Contains(records.IdProjectStatus) select records).ToList();
                        if(project!=null || project.Count>0)
                        project = project.Select(projectRecord => {
                            projectRecord.GeosStatus = (from records in geoscontext.GeosStatus where records.IdOfferStatusType == projectRecord.IdGeosStatus select records).SingleOrDefault();
                            projectRecord.Customer = GetCustomerByCustomerId(projectRecord.IdCustomer);
                            projectRecord.Offer = (from records in geoscontext.Offers where records.IdOffer == projectRecord.IdOffer select records).SingleOrDefault();
                            projectRecord.Owner = (from records in workbenchcontext.Users where records.IdUser == projectRecord.IdOwner select records).SingleOrDefault();
                            return projectRecord;
                        }).ToList();
                    
                        }
                }
 
            }
            return project;
        }

        /// <summary>
        /// This method is to get project milestone related to date and user
        /// </summary>
        /// <param name="fromDate">Get from date</param>
        /// <param name="toDate">Get to date</param>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of project milestone</returns>
        public IList<ProjectMilestone> GetProjectMilestonesByDateAndUser(DateTime fromDate,DateTime toDate,Int32? idUser=null)
        {
           IList<ProjectMilestone> projectMilestones = null;
            List<Int64> userProjects = null;
            List<Int64> projectMilestonesId = null;
            using (var epccontext = new EpcContext())
            {

                if (idUser != null)
                    userProjects = (from records in epccontext.Projects where records.IdOwner == idUser select records.IdProject).ToList();
                else
                    userProjects = (from records in epccontext.Projects select records.IdProject).ToList();

                projectMilestonesId = (from records in epccontext.ProjectMilestoneDates where (EntityFunctions.TruncateTime(records.TargetDate) >= EntityFunctions.TruncateTime(fromDate) && EntityFunctions.TruncateTime(records.TargetDate) <= EntityFunctions.TruncateTime(toDate)) select records.IdProjectMilestone).ToList();
                projectMilestones = (from records in epccontext.ProjectMilestones.Include("Project") where userProjects.Contains(records.IdProject) && projectMilestonesId.Contains(records.IdProjectMilestone) select records).ToList();
                if(projectMilestones.Count>0|| projectMilestones!=null)
                projectMilestones = projectMilestones.Select(projectmilestone => { projectmilestone.ProjectMilestoneDates = new System.Collections.ObjectModel.ObservableCollection<ProjectMilestoneDate>( (from records in epccontext.ProjectMilestoneDates.Include("ProjectMilestoneStatus") where records.IdProjectMilestone == projectmilestone.IdProjectMilestone select records).ToList()); return projectmilestone; }).ToList();

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
        public IList<ProjectTask> GetProjectTasksDeliveryByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null)
        {
            IList<ProjectTask> projectTasks = null;
            List<Int64> userProjectTasks = new List<Int64>();
            using (var epccontext = new EpcContext())
            {
                if (idUser != null)
                    userProjectTasks = (from records in epccontext.ProjectTasks where records.IdOwner == idUser select records.IdProject).ToList();
                else
                    userProjectTasks = (from records in epccontext.ProjectTasks select records.IdProject).ToList();


                projectTasks = (from records in epccontext.ProjectTasks.Include("Project") where userProjectTasks.Contains(records.IdProject) && (EntityFunctions.TruncateTime(records.DueDate) >= EntityFunctions.TruncateTime(fromDate) && EntityFunctions.TruncateTime(records.DueDate) <= EntityFunctions.TruncateTime(toDate)) select records).ToList();

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
        public IList<Project> GetProjectsDeliveryByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null)
        {
            IList<Project> projects = null;
            List<Int64> userProjects = null;
            using (var epccontext = new EpcContext())
            {

                if (idUser != null)
                    userProjects = (from records in epccontext.Projects where records.IdOwner == idUser select records.IdProject).ToList();
                else
                    userProjects = (from records in epccontext.Projects select records.IdProject).ToList();

                projects = (from records in epccontext.Projects where userProjects.Contains(records.IdProject) && (EntityFunctions.TruncateTime(records.DueDate) >= EntityFunctions.TruncateTime(fromDate) && EntityFunctions.TruncateTime(records.DueDate) <= EntityFunctions.TruncateTime(toDate)) select records).ToList();
            }
            return projects;
        }

        /// <summary>
        /// This method is to get list of products
        /// </summary>
        /// <returns>List of products</returns>
        public IList<Product> GetAllProducts()
        {
            IList<Product> products = null;
            using (var epccontext = new EpcContext())
            {
                using (var workbenchContext = new WorkbenchContext())
                {
                    products = (from records in epccontext.Products select records).ToList();
                    if (products != null || products.Count > 0)
                    {
                        products = products.Select(product => { product.ProductVersions = (from record in epccontext.ProductVersions where record.IdProduct == product.IdProduct select record).ToList(); return product; }).ToList();
                        products = products.Select(product => { product.ProductVersions.Select(productversion => { productversion.Creator = (from record in workbenchContext.Users where record.IdUser == productversion.IdCreator select record).SingleOrDefault(); productversion.ProductVersionItems = (from record in epccontext.ProductVersionItems.Include("ProductRoadmap") where record.IdProductVersion == productversion.IdProductVersion select record).ToList(); return productversion; }).ToList(); return product; }).ToList();
                        products = products.Select(product => { product.ProductVersions.Select(productversion => { productversion.ProductVersionItems.Select(productversionitem => { productversionitem.ProductVersionValidations = (from record in epccontext.ProductVersionValidations where record.IdProductVersionItem == productversionitem.IdProductVersionItem select record).ToList(); return productversionitem; }).ToList(); return productversion; }).ToList(); return product; }).ToList();
                        products = products.Select(product => { product.ProductVersions.Select(productversion => { productversion.ProductVersionItems.Select(productversionitem => { productversionitem.ProductVersionValidations.Select(productversionvalidator => { productversionvalidator.Validator = (from record in workbenchContext.Users where record.IdUser == productversionvalidator.IdValidator select record).SingleOrDefault(); return productversionvalidator; }).ToList(); return productversionitem; }).ToList(); return productversion; }).ToList(); return product; }).ToList();
                    }
                }
            }
            return products;
        }

        /// <summary>
        /// This method is to get product version related to product id
        /// </summary>
        /// <param name="idProduct">Get product id</param>
        /// <returns>List of product version</returns>
        public IList<ProductVersion> GetProductVersionByProductId(Int64 idProduct)
        {
            IList<ProductVersion> productVersions = null;
            using (var epccontext = new EpcContext())
            {
                using (var workbenchContext = new WorkbenchContext())
                {
                    productVersions = (from records in epccontext.ProductVersions.Include("ProductVersionItems.ProductRoadmap") where records.IdProduct == idProduct select records).ToList();
                    if(productVersions.Count>0 || productVersions!=null)
                    productVersions = productVersions.Select(productversion => { productversion.Creator = (from records in workbenchContext.Users where records.IdUser == productversion.IdCreator select records).SingleOrDefault();  return productversion; }).ToList();

                  
                }
            }
            return productVersions;
        }

        /// <summary>
        /// This method is to get product details of related product id
        /// </summary>
        /// <param name="idProduct">Get product id</param>
        /// <returns>Product details of related product id</returns>
        public Product GetProductByProductId(Int64 idProduct)
        {
            Product product = null;
            using (var epccontext = new EpcContext())
            {
                using (var workbenchContext = new WorkbenchContext())
                {
                    product = (from records in epccontext.Products where records.IdProduct == idProduct select records).SingleOrDefault();
                    if (product != null)
                    {
                        product.ProductRoadmaps = (from records in epccontext.ProductRoadmaps where records.IdProduct == idProduct select records).ToList();
                        product.ProductVersions = (from records in epccontext.ProductVersions where records.IdProduct == idProduct select records).ToList();
                        if (product.ProductVersions.Count > 0 || product.ProductVersions != null)
                            product.ProductVersions = product.ProductVersions.Select(productversion => { productversion.Creator = (from records in workbenchContext.Users where records.IdUser == productversion.IdCreator select records).SingleOrDefault(); productversion.ProductVersionItems = (from records in epccontext.ProductVersionItems.Include("ProductRoadmap") where records.IdProductVersion == productversion.IdProductVersion select records).ToList(); return productversion; }).ToList();

                    }
                   
                }
            }
            return product;
        }

        /// <summary>
        /// This method is to get project analysis delivery related to date and user id
        /// </summary>
        /// <param name="fromDate">Get from date</param>
        /// <param name="toDate">Get to date</param>
        /// <param name="idUser">Get user id</param>
        /// <returns>List of project analysis</returns>
        public IList<ProjectAnalysis> GetProjectAnalysisDeliveryByDateAndUser(DateTime fromDate, DateTime toDate, Int32? idUser = null)
        {
            IList<ProjectAnalysis> projectAnalysis = null;
            List<Int64> userProjects = null;
            using (var epccontext = new EpcContext())
            {
                if (idUser != null)
                    userProjects = (from records in epccontext.Projects where records.IdOwner == idUser select records.IdProject).ToList();
                else
                    userProjects = (from records in epccontext.Projects select records.IdProject).ToList();


                projectAnalysis = (from records in epccontext.ProjectAnalysis.Include("Project") where userProjects.Contains(records.IdProject) && (EntityFunctions.TruncateTime(records.EngDeliveryDate) >= EntityFunctions.TruncateTime(fromDate) && EntityFunctions.TruncateTime(records.EngDeliveryDate) <= EntityFunctions.TruncateTime(toDate)) select records).ToList();

            }
            return projectAnalysis;
        }

        /// <summary>
        /// This method is to get open all tasks related to project id
        /// </summary>
        /// <param name="project">Get project id</param>
        /// <returns>List of project task</returns>
        public IList<ProjectTask> GetOpenTaskByProjectId(Project project)
        {
            IList<string> lookupvalues = null;
            IList<ProjectTask> projectTask = null;
            using (var epccontext = new EpcContext())
            {
                using (var geos2context = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 5 select records.Value).ToList();
                        IList<int> convertstringtointArray = lookupvalues.Select((s, i) => int.TryParse(s, out i) ? i : 0).ToArray();

                        projectTask = (from records in epccontext.ProjectTasks.Include("Project").Include("Project.Product").Include("TaskPriority").Include("TaskType") where convertstringtointArray.Contains(records.IdTaskStatus) && records.IdProject == project.IdProject select records).ToList();
                        if(projectTask!=null || projectTask.Count>0)
                        projectTask = projectTask.Select(c => { c.Project.Offer = (from records in geoscontext.Offers where records.IdOffer == c.Project.IdOffer select records).SingleOrDefault();c.Owner = (from records in geos2context.Users where records.IdUser == c.IdOwner select records).SingleOrDefault(); return c; }).ToList();
                    
                    }
                }

            }
            return projectTask;
        }


        /// <summary>
        /// This method is to get all open tasks
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <returns>List of open task</returns>
        public IList<ProjectTask> GetOpenTasks(List<User> users)
        {
            IList<string> lookupvalues = null;

            IList<ProjectTask> projectTask = null;
            using (var epccontext = new EpcContext())
            {
                using (var geos2context = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 5 select records.Value).ToList();
                        IList<Int32> convertstringtointArrayOfOpenTask = lookupvalues.Select((s, i) => Int32.TryParse(s, out i) ? i : 0).ToArray();
                        List <Int32> idUsers= users.Select(i=>i.IdUser).ToList();
                        if (idUsers != null || idUsers.Count > 0)
                            {
                                IList<Int64> taskUsers = (from records in epccontext.TaskUsers where idUsers.Contains(records.IdUser) select records.IdTask).ToList();
                                projectTask = (from records in epccontext.ProjectTasks.Include("Project").Include("ProductRoadmap").Include("TaskPriority").Include("TaskType") where taskUsers.Contains(records.IdTask) select records).ToList();
                                if (projectTask != null || projectTask.Count > 0)
                                    projectTask = projectTask.Where(c => convertstringtointArrayOfOpenTask.Contains(c.IdTaskStatus)).ToList();
                            }
                            else
                            {
                                projectTask = (from records in epccontext.ProjectTasks.Include("Project").Include("Project.Product").Include("TaskPriority").Include("TaskType") where convertstringtointArrayOfOpenTask.Contains(records.IdTaskStatus) select records).ToList();
                            }
                        if (projectTask != null || projectTask.Count > 0)
                        {
                           
                            projectTask = projectTask.Select(projecttask => { projecttask.TaskAssistances = new System.Collections.ObjectModel.ObservableCollection<TaskAssistance>(epccontext.TaskAssistances.Where(taskassistance => taskassistance.IdTask == projecttask.IdTask).ToList()); projecttask.Project.Customer = GetCustomerByCustomerId(projecttask.Project.IdCustomer); projecttask.Project.Offer = (from records in geoscontext.Offers where records.IdOffer == projecttask.Project.IdOffer select records).SingleOrDefault(); projecttask.Owner = (from records in geos2context.Users where records.IdUser == projecttask.IdOwner select records).SingleOrDefault(); return projecttask; }).ToList();

                            projectTask = projectTask.Select(projecttask => { projecttask.TaskAssistances.Select(taskAssistance => { taskAssistance.RequestTo = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestTo select records).SingleOrDefault(); taskAssistance.RequestFrom = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestFrom select records).SingleOrDefault(); return taskAssistance; }).ToList(); return projecttask; }).ToList();
                        }

                    }
                }

         }
            return projectTask;
        }

        /// <summary>
        /// This method is to get list of project teams
        /// </summary>
        /// <param name="idProject">Get project id</param>
        /// <returns>List of project teams related to project id</returns>
        public IList<ProjectTeam> GetProjectTeams(Int64 idProject)
        {

            IList<ProjectTeam> projectTeams = null;
            using (var epccontext = new EpcContext())
            {
                projectTeams = (from records in epccontext.ProjectTeams.Include("Team.ParentTeam") where records.IdProject == idProject select records).ToList();
            }

            return projectTeams;
        }

        /// <summary>
        /// This method is to get list of task related to users in iduser team
        /// </summary>
        /// <param name="idUser">Get iduser</param>
        /// <returns>List of task related to users in iduser team</returns>
        public IList<ProjectTask> GetTeamOpenTaskByUserId(Int32 idUser)
        {
            IList<string> lookupvalues = null;
            List<Int32> userids = null;
            IList<ProjectTask> projectTask = null;
            using (var epccontext = new EpcContext())
            {
                using (var geos2context = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 5 select records.Value).ToList();
                        IList<Int32> convertstringtointArrayOfOpenTask = lookupvalues.Select((s, i) => Int32.TryParse(s, out i) ? i : 0).ToArray();
                        byte? idTeam = (from record in epccontext.UserTeams where record.IdUser == idUser select record.IdTeam).FirstOrDefault();
                        if (idTeam != null)
                        {
                            userids = (from record in epccontext.UserTeams where record.IdTeam == idTeam select record.IdUser).ToList();
                            if (userids != null || userids.Count > 0)
                            {
                                IList<Int64> taskUsers = (from records in epccontext.TaskUsers where userids.Contains(records.IdUser) select records.IdTask).ToList();
                                projectTask = (from records in epccontext.ProjectTasks.Include("Project").Include("ProductRoadmap").Include("TaskPriority").Include("TaskType") where taskUsers.Contains(records.IdTask) select records).ToList();
                                if (projectTask != null || projectTask.Count > 0)
                                { 
                                    projectTask = projectTask.Where(c => convertstringtointArrayOfOpenTask.Contains(c.IdTaskStatus)).ToList();
                                    
                                    projectTask = projectTask.Select(projecttask => { projecttask.TaskAssistances = new System.Collections.ObjectModel.ObservableCollection<TaskAssistance>( epccontext.TaskAssistances.Where(taskassistance=>taskassistance.IdTask==projecttask.IdTask).ToList()); projecttask.Project.Customer = GetCustomerByCustomerId(projecttask.Project.IdCustomer); projecttask.Project.Offer = (from records in geoscontext.Offers where records.IdOffer == projecttask.Project.IdOffer select records).SingleOrDefault(); projecttask.Owner = (from records in geos2context.Users where records.IdUser == projecttask.IdOwner select records).SingleOrDefault(); return projecttask; }).ToList();

                                    projectTask = projectTask.Select(projecttask => { projecttask.TaskAssistances.Select(taskAssistance => { taskAssistance.RequestTo = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestTo select records).SingleOrDefault(); taskAssistance.RequestFrom = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestFrom select records).SingleOrDefault(); return taskAssistance; }).ToList();   return projecttask; }).ToList();
                                }
                            }

                        }
                    }
                }

            }
            return projectTask;
        }

        /// <summary>
        /// This method is to get list of all product roadmap related to product id
        /// </summary>
        /// <param name="idProduct">Get product id</param>
        /// <returns>List of all product roadmap related to product id</returns>
        public IList<ProductRoadmap> GetProductRoadmapByProductId(Int64 idProduct)
        {
            IList<ProductRoadmap> productRoadmaps = null;
            using (var db = new EpcContext())
            {
                productRoadmaps = (from records in db.ProductRoadmaps.Include("Product").Include("RoadmapStatus").Include("RoadmapPriority").Include("RoadmapSource").Include("ProductRoadmapType") where records.IdProduct == idProduct select records).ToList();

              
            }
            return productRoadmaps;
        }

       
        /// <summary>
        /// This method is to get task details related to task id
        /// </summary>
        /// <param name="idTask">Get task id</param>
        /// <returns>Get task details</returns>
        public ProjectTask GetTaskDetailsByTaskId(Int64 idTask)
        {
            ProjectTask projectTask = null;


            using (var epccontext = new EpcContext())
            {
                using (var geos2context = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {

                        projectTask = (from records in epccontext.ProjectTasks.Include("Project").Include("Project.Product").Include("TaskPriority").Include("TaskStatus").Include("TaskType")
                                       where records.IdTask == idTask
                                       select records).SingleOrDefault();
                        if (projectTask != null)
                        {
                            projectTask.TaskAssistances = new System.Collections.ObjectModel.ObservableCollection<TaskAssistance>((from records in epccontext.TaskAssistances.Include("TaskAssistanceStatus") where records.IdTask == projectTask.IdTask select records).ToList());
                            projectTask.TaskUsers = new System.Collections.ObjectModel.ObservableCollection<TaskUser>((from records in epccontext.TaskUsers where records.IdTask == projectTask.IdTask select records).ToList());
                            projectTask.TaskWatchers = new System.Collections.ObjectModel.ObservableCollection<TaskWatcher>((from records in epccontext.TaskWatchers where records.IdTask == projectTask.IdTask select records).ToList());
                            projectTask.TaskAttachments =new System.Collections.ObjectModel.ObservableCollection<TaskAttachment>( (from records in epccontext.TaskAttachments where records.IdTask == projectTask.IdTask select records).ToList());
                            projectTask.TaskLogs = new System.Collections.ObjectModel.ObservableCollection<TaskLog>((from records in epccontext.TaskLogs where records.IdTask == projectTask.IdTask select records).ToList());
                            projectTask.TaskComments = new System.Collections.ObjectModel.ObservableCollection<TaskComment>((from records in epccontext.TaskComments where records.IdTask == projectTask.IdTask select records).ToList());
                            if (projectTask.TaskAssistances.Count > 0 || projectTask.TaskAssistances != null)
                                projectTask.TaskAssistances = new System.Collections.ObjectModel.ObservableCollection<TaskAssistance>(projectTask.TaskAssistances.Select(taskAssistance => { taskAssistance.RequestFrom = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestFrom select records).SingleOrDefault(); taskAssistance.RequestTo = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestTo select records).SingleOrDefault(); return taskAssistance; }).ToList());
                            if (projectTask.TaskComments.Count > 0 || projectTask.TaskComments != null)
                                projectTask.TaskComments = new System.Collections.ObjectModel.ObservableCollection<TaskComment>( projectTask.TaskComments.Select(taskComment => { taskComment.User = (from records in geos2context.Users where records.IdUser == taskComment.IdUser select records).SingleOrDefault(); return taskComment; }).ToList());
                            if (projectTask.TaskWatchers.Count > 0 || projectTask.TaskWatchers != null)
                                projectTask.TaskWatchers = new System.Collections.ObjectModel.ObservableCollection<TaskWatcher>( projectTask.TaskWatchers.Select(taskWatcher => { taskWatcher.User = (from records in geos2context.Users where records.IdUser == taskWatcher.IdUser select records).SingleOrDefault(); return taskWatcher; }).ToList());
                            if (projectTask.TaskUsers.Count > 0 || projectTask.TaskUsers != null)
                                projectTask.TaskUsers = new System.Collections.ObjectModel.ObservableCollection<TaskUser>( projectTask.TaskUsers.Select(taskUser => { taskUser.User = (from records in geos2context.Users where records.IdUser == taskUser.IdUser select records).SingleOrDefault(); return taskUser; }).ToList());
                            if (projectTask.TaskLogs.Count > 0 || projectTask.TaskLogs != null)
                                projectTask.TaskLogs =new System.Collections.ObjectModel.ObservableCollection<TaskLog>( projectTask.TaskLogs.Select(tasklog => { tasklog.User = (from records in geos2context.Users where records.IdUser == tasklog.IdUser select records).SingleOrDefault(); return tasklog; }).ToList());
                            projectTask.Owner = (from records in geos2context.Users where records.IdUser == projectTask.IdOwner select records).SingleOrDefault();
                            if(projectTask.Project!= null)
                             projectTask.Project.Offer = (from records in geoscontext.Offers where records.IdOffer == projectTask.Project.IdOffer select records).SingleOrDefault();

                            projectTask.TaskWorkingTimes =new System.Collections.ObjectModel.ObservableCollection<TaskWorkingTime>( (from records in epccontext.TaskWorkingTimes
                                                            where records.IdTask == idTask
                                                            select records).ToList());
                            if (projectTask.TaskWorkingTimes.Count > 0 || projectTask.TaskWorkingTimes != null)
                                projectTask.TaskWorkingTimes =new System.Collections.ObjectModel.ObservableCollection<TaskWorkingTime>( projectTask.TaskWorkingTimes.Select(taskworkingtime => { taskworkingtime.User = (from records in geos2context.Users where records.IdUser == taskworkingtime.IdUser select records).SingleOrDefault(); return taskworkingtime; }).ToList());

                        }
                    }
                }
            }            
           
            return projectTask;
        }

        /// <summary>
        /// This method is to get all open task watcher related to user id
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <returns>List of all open task watcher related to user id</returns>
        public IList<ProjectTask> GetOpenTaskWatchersByUserId(Int32 idUser)
        {
            IList<string> lookupvalues = null;
            IList<ProjectTask> projectTasks = null;
            IList<Int64> userTasks = null;
            using (var epccontext = new EpcContext())
            {
                using (var geos2context = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 5 select records.Value).ToList();
                        IList<int> convertstringtointArray = lookupvalues.Select((s, i) => int.TryParse(s, out i) ? i : 0).ToArray();
                        userTasks = (from records in epccontext.TaskWatchers where records.IdUser == idUser select records.IdTask).ToList();
                        projectTasks = (from records in epccontext.ProjectTasks.Include("Project").Include("Project.Product").Include("TaskPriority").Include("TaskStatus").Include("TaskType").Include("TaskUsers") where convertstringtointArray.Contains(records.IdTaskStatus
                                        ) && userTasks.Contains(records.IdTask) select records).ToList();
                        if(projectTasks != null || projectTasks.Count > 0)
                        projectTasks = projectTasks.Select(projecttask => { projecttask.TaskUsers =new System.Collections.ObjectModel.ObservableCollection<TaskUser>( projecttask.TaskUsers.Select(taskuser => { taskuser.User = (from records in geos2context.Users where records.IdUser == taskuser.IdUser select records).SingleOrDefault(); return taskuser; }).ToList()); projecttask.Project.Customer = GetCustomerByCustomerId(projecttask.Project.IdCustomer); projecttask.Project.Offer = (from records in geoscontext.Offers where records.IdOffer == projecttask.Project.IdOffer select records).SingleOrDefault(); projecttask.Owner = (from records in geos2context.Users where records.IdUser == projecttask.IdOwner select records).SingleOrDefault(); return projecttask; }).ToList();
                    }
                }
            }
            return projectTasks;
        }

        /// <summary>
        /// This method is to get total working time related to task id
        /// </summary>
        /// <param name="idTask">Get task id</param>
        /// <returns>Get toatl working time related to task id</returns>
        public float GetTotalTaskWorkingTime(Int64 idTask)
        {
            float totalTaskWorkingTime = 0;
            List<TaskWorkingTime> taskWorkingTimes = null;
            using (var epccontext = new EpcContext())
            {
                //totalTaskWorkingTime = epccontext.TaskWorkingTimes.Where(taskworkingtime => taskworkingtime.IdTask == idTask).Sum(workingtime => workingtime.WorkingTimeInHours);
                taskWorkingTimes = epccontext.TaskWorkingTimes.Where(taskworkingtime => taskworkingtime.IdTask == idTask).ToList();
                if(taskWorkingTimes.Count>0 || taskWorkingTimes!=null)
                    totalTaskWorkingTime = taskWorkingTimes.Sum(workingtime => workingtime.WorkingTimeInHours);
            }
            return totalTaskWorkingTime;
        }

        /// <summary>
        /// This method is to get list of user details in team related to user id
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <returns>List of user</returns>
        public IList<User> GetUserTeams(Int32 idUser)
        {
            IList<Int32> userids = null;
            IList<User> users = null;
            byte? idTeam = null;
            using (var epccontext = new EpcContext())
            {
                using (var workbenchcontext = new WorkbenchContext())
                {
                    idTeam = (from records in epccontext.UserTeams where records.IdUser == idUser select records.IdTeam).FirstOrDefault();
                    if (idTeam != null)
                    {
                        userids = epccontext.UserTeams.Where(userteam => userteam.IdTeam == idTeam).Select(userid=>userid.IdUser).ToList();
                        if(userids.Count>0 || userids!=null)
                        users = (from record in workbenchcontext.Users where userids.Contains(record.IdUser) select record).ToList();
                    }
                }
            }
            return users;
        }

        /// <summary>
        /// This method is to get user related to team id
        /// </summary>
        /// <param name="idTeams">Get id team</param>
        /// <returns>List of user</returns>
        public IList<User> GetUserByTeamId(List<Team> teams)
        {
            IList<Int32> userids = null;
            IList<User> users = null;
          
            using (var epccontext = new EpcContext())
            {
                using (var workbenchcontext = new WorkbenchContext())
                {
                    List<byte> idTeams = teams.Select(i => i.IdTeam).ToList();
                    if (idTeams != null|| idTeams.Count>0)
                    {
                        userids = (from record in epccontext.UserTeams where idTeams.Contains(record.IdTeam) select record.IdUser).ToList();
                        if (userids.Count > 0 || userids != null)
                            users = (from record in workbenchcontext.Users where userids.Contains(record.IdUser) select record).ToList();
                    }
                }
            }
            return users;
        }

        /// <summary>
        /// This method is to get all open task request assistance related to request from id
        /// </summary>
        /// <param name="idRequestFrom">Get request from</param>
        /// <returns>List of all open task request assistance related to request from id</returns>
        public IList<ProjectTask> GetRequestAssistanceByRequestedFrom(Int32 idRequestFrom)
        {
            IList<string> lookupvalues = null;
            IList<ProjectTask> projectTasks = null;
            IList<Int64> userTasks = null;
            using (var epccontext = new EpcContext())
            {
                using (var geos2context = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 5 select records.Value).ToList();
                        IList<int> convertstringtointArray = lookupvalues.Select((s, i) => int.TryParse(s, out i) ? i : 0).ToArray();
                        userTasks = (from records in epccontext.TaskAssistances where records.IdRequestFrom == idRequestFrom && records.EndDate==null select records.IdTask).ToList();
                        projectTasks = (from records in epccontext.ProjectTasks.Include("Project").Include("Project.Product").Include("TaskPriority").Include("TaskType") where convertstringtointArray.Contains(records.IdTaskStatus) && userTasks.Contains(records.IdTask) select records).ToList();
                        if(projectTasks!=null || projectTasks.Count > 0)
                        projectTasks = projectTasks.Select(projecttask => { projecttask.TaskAssistances = new System.Collections.ObjectModel.ObservableCollection<TaskAssistance>((from records in epccontext.TaskAssistances where records.IdTask == projecttask.IdTask && records.IdRequestFrom == idRequestFrom && records.EndDate == null select records).ToList()); return projecttask; }).ToList();
                        projectTasks = projectTasks.Select(projecttask => { projecttask.ProductRoadmap = (from records in epccontext.ProductRoadmaps.Include("RoadmapSource").Include("ProductRoadmapType").Include("RoadmapStatus").Include("RoadmapPriority") where records.IdProductRoadmap == projecttask.IdProductRoadmap select records).SingleOrDefault(); projecttask.Project.Customer = GetCustomerByCustomerId(projecttask.Project.IdCustomer); projecttask.Project.Offer = (from records in geoscontext.Offers where records.IdOffer == projecttask.Project.IdOffer select records).SingleOrDefault(); projecttask.Owner = (from records in geos2context.Users where records.IdUser == projecttask.IdOwner select records).SingleOrDefault(); projecttask.TaskAssistances = new  System.Collections.ObjectModel.ObservableCollection<TaskAssistance>( projecttask.TaskAssistances.Select(taskAssistance => { taskAssistance.RequestTo = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestTo select records).SingleOrDefault(); taskAssistance.RequestFrom = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestFrom select records).SingleOrDefault(); return taskAssistance; }).ToList()); return projecttask; }).ToList();
                    }
                }
            }
            return projectTasks;
        }

        /// <summary>
        /// This method is to get request assistance related to requested to
        /// </summary>
        /// <param name="idRequestTo">Get request to id</param>
        /// <returns>List of project task</returns>
        public IList<ProjectTask> GetRequestAssistanceByRequestedTo(Int32 idRequestTo)
        {
            IList<string> lookupvalues = null;
            IList<ProjectTask> projectTasks = null;
            IList<Int64> userTasks = null;
            using (var epccontext = new EpcContext())
            {
                using (var geos2context = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 5 select records.Value).ToList();
                        IList<int> convertstringtointArray = lookupvalues.Select((s, i) => int.TryParse(s, out i) ? i : 0).ToArray();
                        userTasks = (from records in epccontext.TaskAssistances where records.IdRequestTo == idRequestTo && records.EndDate == null select records.IdTask).ToList();
                        projectTasks = (from records in epccontext.ProjectTasks.Include("Project").Include("Project.Product").Include("TaskPriority").Include("TaskType") where convertstringtointArray.Contains(records.IdTaskStatus) && userTasks.Contains(records.IdTask) select records).ToList();
                        if (projectTasks != null || projectTasks.Count > 0)
                            projectTasks = projectTasks.Select(projecttask => { projecttask.TaskAssistances = new System.Collections.ObjectModel.ObservableCollection<TaskAssistance>( (from records in epccontext.TaskAssistances where records.IdTask==projecttask.IdTask && records.IdRequestTo == idRequestTo && records.EndDate == null select records).ToList()); return projecttask; }).ToList();
                        projectTasks = projectTasks.Select(projecttask => { projecttask.ProductRoadmap = (from records in epccontext.ProductRoadmaps.Include("RoadmapSource").Include("ProductRoadmapType").Include("RoadmapStatus").Include("RoadmapPriority") where records.IdProductRoadmap == projecttask.IdProductRoadmap select records).SingleOrDefault(); projecttask.Project.Customer = GetCustomerByCustomerId(projecttask.Project.IdCustomer); projecttask.Project.Offer = (from records in geoscontext.Offers where records.IdOffer == projecttask.Project.IdOffer select records).SingleOrDefault(); projecttask.Owner = (from records in geos2context.Users where records.IdUser == projecttask.IdOwner select records).SingleOrDefault(); projecttask.TaskAssistances = new System.Collections.ObjectModel.ObservableCollection<TaskAssistance>( projecttask.TaskAssistances.Select(taskAssistance => { taskAssistance.RequestTo = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestTo select records).SingleOrDefault(); taskAssistance.RequestFrom = (from records in geos2context.Users where records.IdUser == taskAssistance.IdRequestFrom select records).SingleOrDefault(); return taskAssistance; }).ToList()); return projecttask; }).ToList();
                    }
                }
            }
            return projectTasks;
        }


        /// <summary>
        /// This method is to get all request assistance by task id
        /// </summary>
        /// <param name="idTask">Get task id</param>
        /// <param name="idRequestFrom">Get request from id</param>
        /// <returns>List of all project task</returns>
        public IList<ProjectTask> GetRequestAssistanceByTask(Int64 idTask, Int32? idRequestFrom = null)
        {
            IList<string> lookupvalues = null;
            IList<ProjectTask> projectTasks = null;
            IList<Int64> userTasks = null;
            using (var epccontext = new EpcContext())
            {
                using (var geos2context = new WorkbenchContext())
                {
                    using (var geoscontext = new GeosContext())
                    {
                        lookupvalues = (from records in epccontext.LookupValues where records.IdLookupKey == 5 select records.Value).ToList();
                        IList<int> convertstringtointArray = lookupvalues.Select((s, i) => int.TryParse(s, out i) ? i : 0).ToArray();
                        if (idRequestFrom==null)
                        userTasks = (from records in epccontext.TaskAssistances where records.IdTask == idTask select records.IdTask).ToList();
                        else
                         userTasks = (from records in epccontext.TaskAssistances where records.IdTask == idTask && records.IdRequestFrom == idRequestFrom select records.IdTask).ToList();
                        projectTasks = (from records in epccontext.ProjectTasks.Include("Project").Include("Project.Product").Include("TaskPriority").Include("TaskType") where convertstringtointArray.Contains(records.IdTaskStatus) && userTasks.Contains(records.IdTask) select records).ToList();
                        if (projectTasks != null || projectTasks.Count > 0)
                            projectTasks = projectTasks.Select(c => { c.Project.Offer = (from records in geoscontext.Offers where records.IdOffer == c.Project.IdOffer select records).SingleOrDefault(); c.Owner = (from records in geos2context.Users where records.IdUser == c.IdOwner select records).SingleOrDefault(); return c; }).ToList();
                    }
                }
            }
            return projectTasks;
        }

        /// <summary>
        /// This method is to get products
        /// </summary>
        /// <param name="idParentProduct">Get product parent id</param>
        /// <param name="isHierarchicalProduct">Get hierarchical product</param>
        /// <returns>List of products</returns>
        public IList<Product> GetProducts(Int64? idparentproduct = null, bool isHierarchicalProduct = false)
        {
            IList<Product> list;
            IList<Product> hieararchy;

            using (var context = new EpcContext())
            {
                if (idparentproduct == null )
                {
                    if (isHierarchicalProduct == true)
                    {
                        list = (from records in context.Products select records).ToList();

                        hieararchy = list.Where(c => c.IdParent == idparentproduct)
                                    .Select(c => new Product()
                                    {
                                        IdProduct = c.IdProduct,
                                        ProductName = c.ProductName,
                                        Description = c.Description,
                                        IdParent = c.IdParent,
                                        HtmlColor = c.HtmlColor,
                                        ParentProduct = (from records in context.Products where records.IdProduct == c.IdParent select records).FirstOrDefault(),
                                        Childrens = GetProductChildren(list, c.IdProduct)
                                    })
                                    .ToList();

                        ProductHieararchyWalk(hieararchy);
                        
                    }
                    else
                    {
                        hieararchy = (from records in context.Products select records).ToList();
                    
                    }
                }
                else
                {
                    if (isHierarchicalProduct == true)
                    {
                        list = (from records in context.Products where records.IdParent == idparentproduct select records).ToList();

                        hieararchy = list.Where(c => c.IdParent == idparentproduct)
                                    .Select(c => new Product()
                                    {
                                        IdProduct = c.IdProduct,
                                        ProductName = c.ProductName,
                                        Description = c.Description,
                                        IdParent = c.IdParent,
                                        HtmlColor = c.HtmlColor,
                                        ParentProduct = (from records in context.Products where records.IdProduct == c.IdParent select records).FirstOrDefault(),
                                        Childrens = GetProductChildren(list, c.IdProduct)
                                    })
                                    .ToList();

                        ProductHieararchyWalk(hieararchy);
                    }
                    else
                    {
                        hieararchy = (from records in context.Products where records.IdParent == idparentproduct select records).ToList();
                    }
                }
            }

            return hieararchy;
        }

        private IList<Product> GetProductChildren(IList<Product> products, Int64  parentId)
        {
            return products
                    .Where(c => c.IdParent == parentId)
                    .Select(c => new Product
                    {
                        IdProduct = c.IdProduct,
                        ProductName = c.ProductName,
                        Description = c.Description,
                        IdParent = c.IdParent,
                        HtmlColor = c.HtmlColor,
                        ParentProduct=c.ParentProduct,
                        Childrens = GetProductChildren(products, c.IdProduct)
                    })
                    .ToList();
        }

        private void ProductHieararchyWalk(IList<Product> hierarchy)
        {
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    ProductHieararchyWalk(item.Childrens);
                }
            }
        }
    }
}
