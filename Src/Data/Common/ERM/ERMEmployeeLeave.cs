using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMEmployeeLeave : ModelBase, IDisposable
    {
        #region Field
        private Int32 idEmployee;
       
        private DateTime? startDate;
        private DateTime? endDate;
        private Int32 isAllDayEvent;
        private Int32 idCompanyShift;
        private Int16 idLeave; //[GEOS2-4707][rupali sarode][25-07-2023]
        private string leaveType; //[GEOS2-4707][rupali sarode][25-07-2023]
        private decimal leaveMinutes; //[GEOS2-4707][rupali sarode][25-07-2023]
        private string calenderWeek; //[GEOS2-4707][rupali sarode][25-07-2023]
        private TimeSpan monBreakTime;
        private TimeSpan tueBreakTime;
        private TimeSpan wedBreakTime;
        private TimeSpan thuBreakTime;
        private TimeSpan friBreakTime;
        private TimeSpan satBreakTime;
        private TimeSpan sunBreakTime;
        #region [gulab lakade][08 08 2025]
        private string leaveTypeColor;
        private bool? leaveTypeInUse;
        TimeSpan shiftStartTime;
        TimeSpan shiftEndTime;
        private Int32 isNightShift;  // [GEOS2-5515][gulab lakade][28 03 2024]
        #endregion

        #endregion
        #region Property
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
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        [DataMember]
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

       
        [DataMember]
        public Int32 IsAllDayEvent
        {
            get { return isAllDayEvent; }
            set
            {
                isAllDayEvent = value;
                OnPropertyChanged("IsAllDayEvent");
            }
        }
        [DataMember]
        public Int32 IdCompanyShift
        {
            get { return idCompanyShift; }
            set
            {
                idCompanyShift = value;
                OnPropertyChanged("IdCompanyShift");
            }
        }

        [DataMember]
        public Int16 IdLeave
        {
            get { return idLeave; }
            set
            {
                idLeave = value;
                OnPropertyChanged("IdLeave");
            }
        }

        [DataMember]
        public string LeaveType
        {
            get { return leaveType; }
            set
            {
                leaveType = value;
                OnPropertyChanged("LeaveType");
            }
        }
        
        [DataMember]
        public decimal LeaveMinutes
        {
            get { return leaveMinutes; }
            set
            {
                leaveMinutes = value;
                OnPropertyChanged("LeaveMinutes");
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

        [DataMember]
        public TimeSpan MonBreakTime
        {
            get { return monBreakTime; }
            set
            {
                monBreakTime = value;
                OnPropertyChanged("MonBreakTime");
            }
        }

        [DataMember]
        public TimeSpan TueBreakTime
        {
            get { return tueBreakTime; }
            set
            {
                tueBreakTime = value;
                OnPropertyChanged("TueBreakTime");
            }
        }

        [DataMember]
        public TimeSpan WedBreakTime
        {
            get { return wedBreakTime; }
            set
            {
                wedBreakTime = value;
                OnPropertyChanged("WedBreakTime");
            }
        }

        [DataMember]
        public TimeSpan ThuBreakTime
        {
            get { return thuBreakTime; }
            set
            {
                thuBreakTime = value;
                OnPropertyChanged("ThuBreakTime");
            }
        }

        [DataMember]
        public TimeSpan FriBreakTime
        {
            get { return friBreakTime; }
            set
            {
                friBreakTime = value;
                OnPropertyChanged("FriBreakTime");
            }
        }

        [DataMember]
        public TimeSpan SatBreakTime
        {
            get { return satBreakTime; }
            set
            {
                satBreakTime = value;
                OnPropertyChanged("SatBreakTime");
            }
        }

        [DataMember]
        public TimeSpan SunBreakTime
        {
            get { return sunBreakTime; }
            set
            {
                sunBreakTime = value;
                OnPropertyChanged("SunBreakTime");
            }
        }
        #region [gulab lakade][08 08 2025]

        [DataMember]
        public bool? LeaveTypeInUse
        {
            get
            {
                return leaveTypeInUse;
            }

            set
            {
                leaveTypeInUse = value;
                OnPropertyChanged("LeaveTypeInUse");
            }
        }

        [DataMember]
        public string LeaveTypeColor
        {
            get
            {
                return leaveTypeColor;
            }

            set
            {
                leaveTypeColor = value;
                OnPropertyChanged("LeaveTypeColor");
            }
        }
        [NotMapped]
        [DataMember]
        public TimeSpan ShiftStartTime
        {
            get { return shiftStartTime; }

            set
            {
                shiftStartTime = value;
                OnPropertyChanged("ShiftStartTime");
            }
        }
        [NotMapped]
        [DataMember]
        public TimeSpan ShiftEndTime
        {
            get { return shiftEndTime; }

            set
            {
                shiftEndTime = value;
                OnPropertyChanged("ShiftEndTime");
            }
        }
        [DataMember]
        public Int32 IsNightShift
        {
            get { return isNightShift; }
            set
            {
                isNightShift = value;
                OnPropertyChanged("IsNightShift");
            }
        }
        #endregion
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
        #region Constructor
        public ERMEmployeeLeave()
        {

        }
        #endregion
    }
}
