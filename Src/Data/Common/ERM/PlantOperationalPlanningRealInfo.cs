using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.ERM
{
   public class PlantOperationalPlanningRealInfo : ModelBase, IDisposable
    {
        #region Field
        private Int64 idCounterpart;
        private Int64 idReason;
        private string reasonValue;
        private int idstage;
        private DateTime startTime;
        private DateTime endTime;
        private Int64 currentTime;
        private Int32 idEmployee;
        private float timeDifferenceInMinutes;
        private string calenderWeek;

        #endregion

        #region Property
        [DataMember]
        public Int64 IdCounterpart
        {
            get
            {
                return idCounterpart;
            }

            set
            {
                idCounterpart = value;
                OnPropertyChanged("IdCounterpart");
            }
        }


        [DataMember]
        public Int64 IdReason
        {
            get { return idReason; }
            set
            {
                idReason = value;
                OnPropertyChanged("IdReason");
            }
        }
        [DataMember]
        public string ReasonValue
        {
            get { return reasonValue; }
            set
            {
                reasonValue = value;
                OnPropertyChanged("ReasonValue");
            }
        }


        [DataMember]
        public int Idstage
        {
            get { return idstage; }
            set
            {
                idstage = value;
                OnPropertyChanged("Idstage");
            }
        }

        [DataMember]
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        [DataMember]
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        [DataMember]
        public Int64 CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        [DataMember]
        public Int32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        
        [DataMember]
        public float TimeDifferenceInMinutes
        {
            get { return timeDifferenceInMinutes; }
            set
            {
                timeDifferenceInMinutes = value;
                OnPropertyChanged("TimeDifferenceInMinutes");
            }
        }

        [DataMember]
        public string CalenderWeek
        {
            get { return calenderWeek; }
            set
            {
                calenderWeek = value;
                OnPropertyChanged("CalenderWeek");
            }
        }

        #endregion
        #region Constructor
        public PlantOperationalPlanningRealInfo()
        {

        }
        #endregion
        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {

            return this.MemberwiseClone();
        }
        #endregion
    }
}
