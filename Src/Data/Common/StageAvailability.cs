using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class StageAvailability
    {
        private Stage stageInfo;

        private TimeSpan startTimeMonday;
        private TimeSpan endTimeMonday;

        private TimeSpan startTimeTuesday;
        private TimeSpan endTimeTuesday;

        private TimeSpan startTimeWednesday;
        private TimeSpan endTimeWednesday;

        private TimeSpan startTimeThursday;
        private TimeSpan endTimeThursday;

        private TimeSpan startTimeFriday;
        private TimeSpan endTimeFriday;

        private TimeSpan startTimeSaturday;
        private TimeSpan endTimeSaturday;

        private TimeSpan startTimeSunday;
        private TimeSpan endTimeSunday;

        #region Property

        [Column("MonStartTime")]
        [DataMember]
        public TimeSpan StartTimeMonday
        {
            get { return startTimeMonday; }
            set
            {
                startTimeMonday = value;
                OnPropertyChanged("StartTimeMonday");
            }
        }

        [Column("MonEndTime")]
        [DataMember]
        public TimeSpan EndTimeMonday
        {
            get { return endTimeMonday; }
            set
            {
                endTimeMonday = value;
                OnPropertyChanged("EndTimeMonday");
            }
        }

        [Column("TueStartTime")]
        [DataMember]
        public TimeSpan StartTimeTuesday
        {
            get { return startTimeTuesday; }
            set
            {
                startTimeTuesday = value;
                OnPropertyChanged("StartTimeTuesday");
            }
        }

        [Column("TueEndTime")]
        [DataMember]
        public TimeSpan EndTimeTuesday
        {
            get { return endTimeTuesday; }
            set
            {
                endTimeTuesday = value;
                OnPropertyChanged("EndTimeTuesday");
            }
        }

        [Column("WedStartTime")]
        [DataMember]
        public TimeSpan StartTimeWednesday
        {
            get { return startTimeWednesday; }
            set
            {
                startTimeWednesday = value;
                OnPropertyChanged("StartTimeWednesday");
            }
        }

        [Column("WedEndTime")]
        [DataMember]
        public TimeSpan EndTimeWednesday
        {
            get { return endTimeWednesday; }
            set
            {
                endTimeWednesday = value;
                OnPropertyChanged("EndTimeWednesday");
            }
        }

        [Column("ThuStartTime")]
        [DataMember]
        public TimeSpan StartTimeThursday
        {
            get { return startTimeThursday; }
            set
            {
                startTimeThursday = value;
                OnPropertyChanged("StartTimeThursday");
            }
        }

        [Column("ThuEndTime")]
        [DataMember]
        public TimeSpan EndTimeThursday
        {
            get { return endTimeThursday; }
            set
            {
                endTimeThursday = value;
                OnPropertyChanged("EndTimeThursday");
            }
        }

        [Column("FriStartTime")]
        [DataMember]
        public TimeSpan StartTimeFriday
        {
            get { return startTimeFriday; }
            set
            {
                startTimeFriday = value;
                OnPropertyChanged("StartTimeFriday");
            }
        }

        [Column("FriEndTime")]
        [DataMember]
        public TimeSpan EndTimeFriday
        {
            get { return endTimeFriday; }
            set
            {
                endTimeFriday = value;
                OnPropertyChanged("EndTimeFriday");
            }
        }

        [Column("SatStartTime")]
        [DataMember]
        public TimeSpan StartTimeSaturday
        {
            get { return startTimeSaturday; }
            set
            {
                startTimeSaturday = value;
                OnPropertyChanged("StartTimeSaturday");
            }
        }

        [Column("SatEndTime")]
        [DataMember]
        public TimeSpan EndTimeSaturday
        {
            get { return endTimeSaturday; }
            set
            {
                endTimeSaturday = value;
                OnPropertyChanged("EndTimeSaturday");
            }
        }

        [Column("SunStartTime")]
        [DataMember]
        public TimeSpan StartTimeSunday
        {
            get { return startTimeSunday; }
            set
            {
                startTimeSunday = value;
                OnPropertyChanged("StartTimeSunday");
            }
        }

        [Column("SunEndTime")]
        [DataMember]
        public TimeSpan EndTimeSunday
        {
            get { return endTimeSunday; }
            set
            {
                endTimeSunday = value;
                OnPropertyChanged("EndTimeSunday");
            }
        }

        
        [DataMember]
        public Stage StageInfo
        {
            get
            {
                return stageInfo;
            }

            set
            {
                stageInfo = value;
                OnPropertyChanged("EndTimeSunday");
            }
        }
        #endregion


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
