using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.APM
{ //[Sudhir.Jangra][GEOS2-6913]
    [DataContract]
    public class DelayedTasks : ModelBase, IDisposable
    {
        #region fields
        private string open12Month;
        private string openCurrentMonth;
        private string overdue12Month;
        private string overdueCurrentMonth;
        private string openDays12Month;
        private string openDaysCurrentMonth;
        private string blocked12Month;
        private string blockedCurrentMonth;
        private string lastDays;
        private string total12Month;
        private string overdue12MonthHigh;
        private string overdue12MonthMedium;
        private string overdue12MonthLow;
        private string overdueCurrentMonthHigh;
        private string overdueCurrentMonthMedium;
        private string overdueCurrentMonthLow;

        private string statusColor;
        private string statusValue;
        private string overdue12MonthTotalColor;
        private string overdueCurrentMonthTotalColor;
        private string open12MonthCustomDays;
        private string plantName; //[Sudhir.Jangra][GEOS2-7739]
        private string currentMonthName;//[Sudhir.Jangra][GEOS2-7739]
        private string closed12Month;   //[shweta.thube][GEOS2-7889]
        private string closedCurrentMonth;   //[shweta.thube][GEOS2-7889]
        private string plantManager;     //[shweta.thube][GEOS2-7891]
        private string newTasksCurrentMonth;//[shweta.thube][GEOS2-8072]
        private string newTasksCurrentMonthHigh;//[shweta.thube][GEOS2-8072]
        private string newTasksCurrentMonthMedium;//[shweta.thube][GEOS2-8072]
        private string newTasksCurrentMonthLow;//[shweta.thube][GEOS2-8072]
        #endregion

        #region properties
        [NotMapped]
        [DataMember]
        public string Open12Month
        {
            get { return open12Month; }
            set
            {
                open12Month = value;
                OnPropertyChanged("Open12Month");
            }
        }
        [NotMapped]
        [DataMember]
        public string OpenCurrentMonth
        {
            get { return openCurrentMonth; }
            set
            {
                openCurrentMonth = value;
                OnPropertyChanged("OpenCurrentMonth");
            }
        }
        [NotMapped]
        [DataMember]
        public string Overdue12Month
        {
            get { return overdue12Month; }
            set
            {
                overdue12Month = value;
                OnPropertyChanged("Overdue12Month");
            }
        }
        [NotMapped]
        [DataMember]
        public string OverdueCurrentMonth
        {
            get { return overdueCurrentMonth; }
            set
            {
                overdueCurrentMonth = value;
                OnPropertyChanged("OverdueCurrentMonth");
            }
        }

        [NotMapped]
        [DataMember]
        public string OpenDays12Month
        {
            get { return openDays12Month; }
            set
            {
                openDays12Month = value;
                OnPropertyChanged("OpenDays12Month");
            }
        }
        [NotMapped]
        [DataMember]
        public string OpenDaysCurrentMonth
        {
            get { return openDaysCurrentMonth; }
            set
            {
                openDaysCurrentMonth = value;
                OnPropertyChanged("OpenDaysCurrentMonth");
            }
        }

        [NotMapped]
        [DataMember]
        public string Blocked12Month
        {
            get { return blocked12Month; }
            set
            {
                blocked12Month = value;
                OnPropertyChanged("Blocked12Month");
            }
        }

        [NotMapped]
        [DataMember]
        public string BlockedCurrentMonth
        {
            get { return blockedCurrentMonth; }
            set
            {
                blockedCurrentMonth = value;
                OnPropertyChanged("BlockedCurrentMonth");
            }
        }

        [NotMapped]
        [DataMember]
        public string LastDays
        {
            get { return lastDays; }
            set
            {
                lastDays = value;
                OnPropertyChanged("LastDays");
            }
        }
        [NotMapped]
        [DataMember]
        public string Total12Month
        {
            get { return total12Month; }
            set
            {
                total12Month = value;
                OnPropertyChanged("Total12Month");
            }
        }

        [NotMapped]
        [DataMember]
        public string Overdue12MonthHigh
        {
            get { return overdue12MonthHigh; }
            set
            {
                overdue12MonthHigh = value;
                OnPropertyChanged("Overdue12MonthHigh");
            }
        }
        [NotMapped]
        [DataMember]
        public string Overdue12MonthMedium
        {
            get { return overdue12MonthMedium; }
            set
            {
                overdue12MonthMedium = value;
                OnPropertyChanged("Overdue12MonthMedium");
            }
        }
        [NotMapped]
        [DataMember]
        public string Overdue12MonthLow
        {
            get { return overdue12MonthLow; }
            set
            {
                overdue12MonthLow = value;
                OnPropertyChanged("Overdue12MonthLow");
            }
        }

        [NotMapped]
        [DataMember]
        public string OverdueCurrentMonthHigh
        {
            get { return overdueCurrentMonthHigh; }
            set
            {
                overdueCurrentMonthHigh = value;
                OnPropertyChanged("OverdueCurrentMonthHigh");
            }
        }
        [NotMapped]
        [DataMember]
        public string OverdueCurrentMonthMedium
        {
            get { return overdueCurrentMonthMedium; }
            set
            {
                overdueCurrentMonthMedium = value;
                OnPropertyChanged("OverdueCurrentMonthMedium");
            }
        }
        [NotMapped]
        [DataMember]
        public string OverdueCurrentMonthLow
        {
            get { return overdueCurrentMonthLow; }
            set
            {
                overdueCurrentMonthLow = value;
                OnPropertyChanged("OverdueCurrentMonthLow");
            }
        }

        [NotMapped]
        [DataMember]
        public string StatusValue
        {
            get { return statusValue; }
            set
            {
                statusValue = value;
                OnPropertyChanged("StatusValue");
            }
        }

        [NotMapped]
        [DataMember]
        public string StatusColor
        {
            get { return statusColor; }
            set
            {
                statusColor = value;
                OnPropertyChanged("StatusColor");
            }
        }

        [NotMapped]
        [DataMember]
        public string Overdue12MonthTotalColor
        {
            get { return overdue12MonthTotalColor; }
            set
            {
                overdue12MonthTotalColor = value;
                OnPropertyChanged("Overdue12MonthTotalColor");
            }
        }

        [NotMapped]
        [DataMember]

        public string OverdueCurrentMonthTotalColor
        {
            get { return overdueCurrentMonthTotalColor; }
            set
            {
                overdueCurrentMonthTotalColor = value;
                OnPropertyChanged("OverdueCurrentMonthTotalColor");
            }
        }

        [NotMapped]
        [DataMember]
        public string Open12MonthCustomDays
        {
            get { return open12MonthCustomDays; }
            set
            {
                open12MonthCustomDays = value;
                OnPropertyChanged("Open12MonthCustomDays");
            }
        }

        //[Sudhir.Jangra][GEOS2-7739]
        [NotMapped]
        [DataMember]
        public string CurrentMonthName
        {
            get { return currentMonthName; }
            set
            {
                currentMonthName = value;
                OnPropertyChanged("CurrentMonthName");
            }
        }
        //[Sudhir.Jangra][GEOS2-7739]
        [NotMapped]
        [DataMember]
        public string PlantName
        {
            get { return plantName; }
            set
            {
                plantName = value;
                OnPropertyChanged("PlantName");
            }
        }

        //[shweta.thube][GEOS2-7889]
        [NotMapped]
        [DataMember]
        public string Closed12Month
        {
            get { return closed12Month; }
            set
            {
                closed12Month = value;
                OnPropertyChanged("Closed12Month");
            }
        }
        //[shweta.thube][GEOS2-7889]
        [NotMapped]
        [DataMember]
        public string ClosedCurrentMonth
        {
            get { return closedCurrentMonth; }
            set
            {
                closedCurrentMonth = value;
                OnPropertyChanged("ClosedCurrentMonth");
            }
        }
        //[shweta.thube][GEOS2-7891]
        [NotMapped]
        [DataMember]
        public string PlantManager
        {
            get { return plantManager; }
            set
            {
                plantManager = value;
                OnPropertyChanged("PlantManager");
            }
        }

        //[shweta.thube][GEOS2-8072]
        [NotMapped]
        [DataMember]
        public string NewTasksCurrentMonth
        {
            get { return newTasksCurrentMonth; }
            set
            {
                newTasksCurrentMonth = value;
                OnPropertyChanged("NewTasksCurrentMonth");
            }
        }
        //[shweta.thube][GEOS2-8072]
        [NotMapped]
        [DataMember]
        public string NewTasksCurrentMonthHigh
        {
            get { return newTasksCurrentMonthHigh; }
            set
            {
                newTasksCurrentMonthHigh = value;
                OnPropertyChanged("NewTasksCurrentMonthHigh");
            }
        }
        //[shweta.thube][GEOS2-8072]
        [NotMapped]
        [DataMember]
        public string NewTasksCurrentMonthMedium
        {
            get { return newTasksCurrentMonthMedium; }
            set
            {
                newTasksCurrentMonthMedium = value;
                OnPropertyChanged("NewTasksCurrentMonthMedium");
            }
        }
        //[shweta.thube][GEOS2-8072]
        [NotMapped]
        [DataMember]
        public string NewTasksCurrentMonthLow
        {
            get { return newTasksCurrentMonthLow; }
            set
            {
                newTasksCurrentMonthLow = value;
                OnPropertyChanged("NewTasksCurrentMonthLow");
            }
        }
        #endregion

        #region constructor
        public DelayedTasks() { }
        #endregion

        #region methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }
}
