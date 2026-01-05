using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMProductionTimelineSprint : ModelBase, IDisposable
    {
        #region Fields
        private string plant;
        private DateTime? date;
        private string employee;
        private string stage;
        private TimeSpan total_Shift;
        private TimeSpan total_Logged;
        private TimeSpan total_Attendance;
        private TimeSpan wOTime;
        private string maintance;
        private string meeting;
        #endregion

        #region Properties 

        [DataMember]
        public string Plant
        {
            get
            {
                return plant;
            }

            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [DataMember]
        public DateTime? Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        [DataMember]
        public string Employee
        {
            get
            {
                return employee;
            }

            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }
        [DataMember]
        public string Stage
        {
            get
            {
                return stage;
            }

            set
            {
                stage = value;
                OnPropertyChanged("Stage");
            }
        }
        [DataMember]
        public TimeSpan Total_Shift
        {
            get
            {
                return total_Shift;
            }

            set
            {
                total_Shift = value;
                OnPropertyChanged("Total_Shift");
            }
        }
        [DataMember]
        public TimeSpan Total_Logged
        {
            get
            {
                return total_Logged;
            }

            set
            {
                total_Logged = value;
                OnPropertyChanged("Total_Logged");
            }
        }
        [DataMember]
        public TimeSpan Total_Attendance
        {
            get
            {
                return total_Attendance;
            }

            set
            {
                total_Attendance = value;
                OnPropertyChanged("Total_Attendance");
            }
        }
        [DataMember]
        public TimeSpan WOTime
        {
            get
            {
                return wOTime;
            }

            set
            {
                wOTime = value;
                OnPropertyChanged("WOTime");
            }
        }
        [DataMember]
        public string Maintance
        {
            get
            {
                return maintance;
            }

            set
            {
                maintance = value;
                OnPropertyChanged("Maintance");
            }
        }
        [DataMember]
        public string Meeting
        {
            get
            {
                return meeting;
            }

            set
            {
                meeting = value;
                OnPropertyChanged("Meeting");
            }
        }

        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
