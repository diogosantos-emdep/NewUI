using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_Employee_Attendance : ModelBase, IDisposable
    {
        #region Field
        //Employee
        private Int32 idEmployee;
        //Attendance
        private DateTime? attendanceStartDate;
        private DateTime? attendanceEndDate;
        private DateTime? accountingDate;
        private string attendanceType;
        private string attendanceTypeColor;
        private bool? attendanceTypeInUse;
        // Shift Details
        private string shiftName; //[GEOS2-6040][gulab lakade] [13 08 2024]
        private Int32 idCompanyShift;//[GEOS2-6040][gulab lakade] [13 08 2024]
        TimeSpan shiftStartTime;
        TimeSpan shiftEndTime;
        private Int32 isNightShift;  // [GEOS2-5515][gulab lakade][28 03 2024]
        TimeSpan breakTime;
        private string workingDay; //[GEOS2-5911][gulab lakade][25 07 2024]

        //private Int32 attendanceTimeDifference;
        private string calenderWeek;
        #endregion
        #region Property
        [DataMember]
        public Int32 IdEmployee
        {
            get
            {
                return idEmployee;
            }

            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }
        [DataMember]
        public DateTime? AttendanceStartDate
        {
            get
            {
                return attendanceStartDate;
            }

            set
            {
                attendanceStartDate = value;
                OnPropertyChanged("AttendanceStartDate");
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
        public DateTime? AttendanceEndDate
        {
            get
            {
                return attendanceEndDate;
            }

            set
            {
                attendanceEndDate = value;
                OnPropertyChanged("AttendanceEndDate");
            }
        }
        [DataMember]
        public DateTime? AccountingDate
        {
            get
            {
                return accountingDate;
            }

            set
            {
                accountingDate = value;
                OnPropertyChanged("AccountingDate");
            }
        }
        //HTMLCOlOR
        [DataMember]
        public string AttendanceType
        {
            get
            {
                return attendanceType;
            }

            set
            {
                attendanceType = value;
                OnPropertyChanged("AttendanceType");
            }
        }
        [DataMember]
        public string AttendanceTypeColor
        {
            get
            {
                return attendanceTypeColor;
            }

            set
            {
                attendanceTypeColor = value;
                OnPropertyChanged("AttendanceTypeColor");
            }
        }
        [DataMember]
        public bool? AttendanceTypeInUse
        {
            get
            {
                return attendanceTypeInUse;
            }

            set
            {
                attendanceTypeInUse = value;
                OnPropertyChanged("AttendanceTypeInUse");
            }
        }
        //Shift details
        [DataMember]
        public string ShiftName
        {
            get
            {
                return shiftName;
            }

            set
            {
                shiftName = value;
                OnPropertyChanged("ShiftName");
            }
        }
        [DataMember]
        public Int32 IdCompanyShift
        {
            get
            {
                return idCompanyShift;
            }

            set
            {
                idCompanyShift = value;
                OnPropertyChanged("IdCompanyShift");
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
        [NotMapped]
        [DataMember]
        public TimeSpan BreakTime
        {
            get { return breakTime; }

            set
            {
                breakTime = value;
                OnPropertyChanged("BreakTime");
            }
        }
        [DataMember]
        public string WorkingDay
        {
            get
            {
                return workingDay;
            }

            set
            {
                workingDay = value;
                OnPropertyChanged("WorkingDay");
            }
        }
        #endregion
        #region Constructor
        public ERM_Employee_Attendance()
        {

        }
        #endregion
        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
