using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("tasks")]
    [DataContract(IsReference = true)]
    public class ProjectTask : ModelBase,IDisposable
    {
        #region  Fields
        Int64 idTask;
        Int32 idTaskType;
        Int32 idTaskStatus;
        Int64 idProductRoadmap;
        Int32 idTaskPriority;
        Int64? idParentTask;
        string description;
        DateTime? dueDate;
        Int32? effortPoints;
        DateTime? endDate;
        Single plannedHours;
        DateTime? startDate;
        string taskTitle;
        Single workedHours;
        Int32 progress;
        ProjectTask parentTask;
        LookupValue taskStatus;
        LookupValue taskPriority;
        ProductRoadmap productRoadmap;
        LookupValue taskType;
        IList<ProjectTask> childrens;
        Int32 idOwner;
        Int64 idProject;
        Project project;
        User owner;
        Int32 idCategory;
        LookupValue category;
        DateTime? createdDate;
        ObservableCollection<TaskAssistance> taskAssistances;
        ObservableCollection<TaskWatcher> taskWatchers;
        ObservableCollection<TaskAttachment> taskAttachments;
        ObservableCollection<TaskLog> taskLogs;
        ObservableCollection<TaskUser> taskUsers;
        ObservableCollection<TaskWorkingTime> taskWorkingTimes;
        ObservableCollection<TaskComment> taskComments;
        float? efficiency;
        bool isRequestAssistance;
        #endregion

          #region Constructor
        public ProjectTask()
        {
            //this.TaskWatchers = new List<TaskWatcher>();
            //this.TaskAssistances = new List<TaskAssistance>();
            //this.TaskLogs = new List<TaskLog>();
            //this.TaskUsers = new List<TaskUser>();
            //this.TaskWorkingTimes = new List<TaskWorkingTime>();
            //this.TaskAttachments = new List<TaskAttachment>();
            //this.TaskComments = new List<TaskComment>();
        }
        #endregion

        #region Properties
       
        [Key]
        [Column("IdTask")]
        [DataMember]
        public Int64 IdTask
        {
            get
            {
                return idTask;
            }
            set
            {
                idTask = value;
                OnPropertyChanged("IdTask");
            }
        }

        [Column("IdTaskType")]
        [ForeignKey("TaskType")]
        [DataMember]
        public Int32 IdTaskType
        {
            get
            {
                return idTaskType;
            }
            set
            {
                this.idTaskType = value;
                OnPropertyChanged("IdTaskType");
            }
        }

        [Column("IdParentTask")]
        [ForeignKey("ParentTask")]
        [DataMember]
        public Int64? IdParentTask
        {
            get
            {
                return idParentTask;
            }
            set
            {
                idParentTask = value;
                OnPropertyChanged("IdParentTask");
            }
        }

        [Column("IdTaskPriority")]
        [ForeignKey("TaskPriority")]
        [DataMember]
        public Int32 IdTaskPriority
        {
            get
            {
                return idTaskPriority;
            }
            set
            {
                idTaskPriority = value;
                OnPropertyChanged("IdTaskPriority");
            }
        }

        [Column("IdProductRoadmap")]
        [ForeignKey("ProductRoadmap")]
        [DataMember]
        public Int64 IdProductRoadmap
        {
            get
            {
                return idProductRoadmap;
            }
            set
            {
                idProductRoadmap = value;
                OnPropertyChanged("IdProductRoadmap");
            }
        }


        [Column("IdTaskStatus")]
        [ForeignKey("TaskStatus")]
        [DataMember]
        public Int32 IdTaskStatus
        {
            get
            {
                return idTaskStatus;
            }
            set
            {
                idTaskStatus = value;
                OnPropertyChanged("IdTaskStatus");
            }
        }

        [Column("IdCategory")]
        [ForeignKey("Category")]
        [DataMember]
        public Int32 IdCategory
        {
            get
            {
                return idCategory;
            }
            set
            {
                idCategory = value;
                OnPropertyChanged("IdCategory");
            }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("DueDate")]
        [DataMember]
        public DateTime? DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
                OnPropertyChanged("DueDate");
            }
        }

       [Column("EndDate")]
        [DataMember]
        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

       [Column("StartDate")]
       [DataMember]
        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }


        [Column("CreatedDate")]
        [DataMember]
        public DateTime? CreatedDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                createdDate = value;
                OnPropertyChanged("CreatedDate");
            }
        }

        [Column("EffortPoints")]
        [DataMember]
        public Int32? EffortPoints
        {
            get
            {
                return effortPoints;
            }
            set
            {
                effortPoints = value;
                OnPropertyChanged("EffortPoints");
            }
        }

        [Column("PlannedHours")]
        [DataMember]
        public Single PlannedHours
        {
            get
            {
                return plannedHours;
            }
            set
            {
                plannedHours = value;
                OnPropertyChanged("PlannedHours");
            }
        }

        [Column("TaskTitle")]
        [DataMember]
        public string TaskTitle
        {
            get
            {
                return taskTitle;
            }
            set
            {
                taskTitle = value;
                OnPropertyChanged("TaskTitle");
            }
        }

        [Column("WorkedHours")]
        [DataMember]
        public Single WorkedHours
        {
            get
            {
                return workedHours;
            }
            set
            {
                workedHours = value;
                OnPropertyChanged("WorkedHours");
            }
        }

        [Column("IdProject")]
        [ForeignKey("Project")]
        [DataMember]
        public Int64 IdProject
        {
            get
            {
                return idProject;
            }
            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }



        [Column("IdOwner")]
        [ForeignKey("Owner")]
        [DataMember]
        public Int32 IdOwner
        {
            get
            {
                return idOwner;
            }
            set
            {
                idOwner = value;
                OnPropertyChanged("IdOwner");
            }
        }

        [Column("Progress")]
        [DataMember]
        public Int32 Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        [NotMapped]
        [DataMember]
        public float? Efficiency
        {
            get
            {
                return efficiency;
            }
            set
            {
                efficiency = value;
                OnPropertyChanged("Efficiency");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsRequestAssistance
        {
            get
            {
                return isRequestAssistance;
            }
            set
            {
                isRequestAssistance = value;
                OnPropertyChanged("IsRequestAssistance");
            }
        }

        [DataMember]
        public virtual User Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
                OnPropertyChanged("Owner");
            }
        }

        [DataMember]
        public virtual ProductRoadmap ProductRoadmap
        {
            get
            {
                return productRoadmap;
            }

            set
            {
                productRoadmap = value;
                OnPropertyChanged("ProductRoadmap");
            }
        }

        [DataMember]
        public virtual Project Project
        {
            get
            {
                return project;
            }

            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }


        [DataMember]
        public virtual LookupValue TaskStatus
        {
            get
            {
                return taskStatus;
            }

            set
            {
                taskStatus = value;
                OnPropertyChanged("TaskStatus");
            }
        }

        [DataMember]
        public virtual LookupValue TaskPriority
        {
            get
            {
                return taskPriority;
            }

            set
            {
                taskPriority = value;
                OnPropertyChanged("TaskPriority");
            }
        }

        [DataMember]
        public virtual LookupValue Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        [DataMember]
        public virtual LookupValue TaskType
        {
            get
            {
                return taskType;
            }

            set
            {
                taskType = value;
                OnPropertyChanged("TaskType");
            }
        }

        [DataMember]
        public virtual ProjectTask ParentTask
        {
            get
            {
                return parentTask;
            }

            set
            {
                parentTask = value;
                OnPropertyChanged("ParentTask");
            }
        }

        [DataMember]
        public virtual IList<ProjectTask> Childrens
        {
            get
            {
                return childrens;
            }

            set
            {
                childrens = value;
                OnPropertyChanged("Childrens");
            }
        }

        [DataMember]
        public virtual ObservableCollection<TaskAssistance> TaskAssistances
        {
            get
            {
                return taskAssistances;
            }

            set
            {
                taskAssistances = value;
                OnPropertyChanged("TaskAssistances");
            }
        }

        [DataMember]
        public virtual ObservableCollection<TaskWatcher> TaskWatchers
        {
            get
            {
                return taskWatchers;
            }

            set
            {
                taskWatchers = value;
                OnPropertyChanged("TaskWatchers");
            }
        }

        [DataMember]
        public virtual ObservableCollection<TaskAttachment> TaskAttachments
        {
            get
            {
                return taskAttachments;
            }

            set
            {
                taskAttachments = value;
                OnPropertyChanged("TaskAttachments");
            }
        }

        [DataMember]
        public virtual ObservableCollection<TaskLog> TaskLogs
        {
            get
            {
                return taskLogs;
            }

            set
            {
                taskLogs = value;
                OnPropertyChanged("TaskLogs");
            }
        }

        [DataMember]
        public virtual ObservableCollection<TaskUser> TaskUsers
        {
            get
            {
                return taskUsers;
            }

            set
            {
                taskUsers = value;
                OnPropertyChanged("TaskUsers");
            }
        }

        [DataMember]
        public virtual ObservableCollection<TaskWorkingTime> TaskWorkingTimes
        {
            get
            {
                return taskWorkingTimes;
            }

            set
            {
                taskWorkingTimes = value;
                OnPropertyChanged("TaskWorkingTimes");
            }
        }

        [DataMember]
        public virtual ObservableCollection<TaskComment> TaskComments
        {
            get
            {
                return taskComments;
            }

            set
            {
                taskComments = value;
                OnPropertyChanged("TaskComments");
            }
        }
        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
