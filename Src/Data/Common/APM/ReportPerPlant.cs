using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
//[shweta.thube][GEOS2-6047][03.09.2025]
namespace Emdep.Geos.Data.Common.APM
{
    public class ReportPerPlant : ModelBase, IDisposable
    {
        #region Declarations
        private Int32 pendingTaskCount;
        private string year;
        private Int32 openIssues;
        private Int32 tasksCreated;
        private Int32 issuesResolved;
        private Int32 cumulativeTasksResolved;
        private Int32 cumulativeTasksCreated;
        private string toDo;
        private string inProgress;
        private string closedTaskCount;
        private string theme;
        private string totalNumberOfTasks;
        private string openTaskCount;
        #endregion

        #region Properties

        [DataMember]
        public Int32 PendingTaskCount
        {
            get { return pendingTaskCount; }
            set
            {
                pendingTaskCount = value;
                OnPropertyChanged("PendingTaskCount");
            }
        }
        [NotMapped]
        [DataMember]
        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }
        [DataMember]
        public Int32 IssuesResolved
        {
            get { return issuesResolved; }
            set
            {
                issuesResolved = value;
                OnPropertyChanged("IssuesResolved");
            }
        }

        [DataMember]
        public Int32 OpenIssues
        {
            get { return openIssues; }
            set
            {
                openIssues = value;
                OnPropertyChanged("OpenIssues");
            }
        }

        [DataMember]
        public Int32 TasksCreated
        {
            get { return tasksCreated; }
            set
            {
                tasksCreated = value;
                OnPropertyChanged("TasksCreated");
            }
        }
        [DataMember]
        public Int32 CumulativeTasksResolved
        {
            get { return cumulativeTasksResolved; }
            set
            {
                cumulativeTasksResolved = value;
                OnPropertyChanged("CumulativeTasksResolved");
            }
        }
        [DataMember]
        public Int32 CumulativeTasksCreated
        {
            get { return cumulativeTasksCreated; }
            set
            {
                cumulativeTasksCreated = value;
                OnPropertyChanged("CumulativeTasksCreated");
            }
        }
        [DataMember]
        public string ToDo
        {
            get { return toDo; }
            set
            {
                toDo = value;
                OnPropertyChanged("ToDo");
            }
        }

        [DataMember]
        public string InProgress
        {
            get { return inProgress; }
            set
            {
                inProgress = value;
                OnPropertyChanged("InProgress");
            }
        }
        [DataMember]
        public string ClosedTaskCount
        {
            get { return closedTaskCount; }
            set
            {
                closedTaskCount = value;
                OnPropertyChanged("ClosedTaskCount");
            }
        }
        [DataMember]
        public string Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                OnPropertyChanged("Theme");
            }
        }
        [DataMember]
        public string TotalNumberOfTasks
        {
            get { return totalNumberOfTasks; }
            set
            {
                totalNumberOfTasks = value;
                OnPropertyChanged("TotalNumberOfTasks");
            }
        }

        [DataMember]
        public string OpenTaskCount
        {
            get { return openTaskCount; }
            set
            {
                openTaskCount = value;
                OnPropertyChanged("OpenTaskCount");
            }
        }
        #endregion

        #region Constructor
        public ReportPerPlant()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
