using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class WeeklyProductionEmployeeTime : ModelBase, IDisposable
    {
        #region Field

        private Int64 idEmployee;
        private DateTime? startDate;
        private DateTime? endDate;
       
        private string employeeCode;
        
        private string employeeeName;
        private string week;
        private string totalWorkedHours;
        private Double totalWorkedHoursBYJobDescription;
      
       // private string dayName;
        private Int32 idJobDescription;
        private Int32 dailyHoursCount;
        //private double dailyHoursCount;//[GEOS2-5856][gulab lakade][20 06 2024]
        private Int32 idCompany;
        private Decimal jobDescriptionUsage;
        private Int32 idEmployeeJobDescription;
        private Int32 idStage;
        private List<Int32> idStageList;
        private TimeSpan breakTime;//[GEOS2-5856][gulab lakade][20 06 2024]
        private string attendanceType;//[GEOS2-6058][gulab lakade][27 08 2024]
        #region [GEOS2-6058][gulab lakade][25 09 2024]
        private TimeSpan attendanceTime;
        private TimeSpan regulartime;
        private TimeSpan overTime;
        private TimeSpan workingTime;
        private TimeSpan not_WorkTime;
        private string stageName;
        #endregion
        #endregion
        #region Property
        [DataMember]
        public Int64 IdEmployee
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
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCod;");
            }
        }
        [DataMember]
        public string EmployeeeName
        {
            get { return employeeeName; }
            set
            {
                employeeeName = value;
                OnPropertyChanged("EmployeeeName");
            }
        }
        [DataMember]
        public string Week
        {
            get { return week; }
            set
            {
                week = value;
                OnPropertyChanged("Week");
            }
        }
        [DataMember]
        public string TotalWorkedHours
        {
            get { return totalWorkedHours; }
            set
            {
                totalWorkedHours = value;
                OnPropertyChanged("TotalWorkedHours");
            }
        }
        [DataMember]
        public Double TotalWorkedHoursBYJobDescription
        {
            get { return totalWorkedHoursBYJobDescription; }
            set
            {
                totalWorkedHoursBYJobDescription = value;
                OnPropertyChanged("TotalWorkedHoursBYJobDescription");
            }
        }
        [DataMember]
        public Int32 IdJobDescription
        {
            get { return idJobDescription ; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }
        [DataMember]
        public Int32 DailyHoursCount
        {
            get { return dailyHoursCount; }
            set
            {
                dailyHoursCount = value;
                OnPropertyChanged("DailyHoursCount");
            }
        }
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }
        [DataMember]
        public Decimal JobDescriptionUsage
        {
            get { return jobDescriptionUsage; }
            set
            {
                jobDescriptionUsage = value;
                OnPropertyChanged("JobDescriptionUsage");
            }
        }
        [DataMember]
        public Int32 IdEmployeeJobDescription
        {
            get { return idEmployeeJobDescription; }
            set
            {
                idEmployeeJobDescription = value;
                OnPropertyChanged("IdEmployeeJobDescription");
            }
        }
        [DataMember]
        public Int32 IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }
        [DataMember]
        public List<Int32> IdStageList
        {
            get { return idStageList; }
            set
            {
                idStageList = value;
                OnPropertyChanged("IdStageList");
            }
        }
        //[GEOS2-5856][gulab lakade][20 06 2024]
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
        //[GEOS2-6058][gulab lakade][27 08 2024]
        [DataMember]
        public string AttendanceType
        {
            get { return attendanceType; }
            set
            {
                attendanceType = value;
                OnPropertyChanged("AttendanceType");
            }
        }

        #region [GEOS2-6058][gulab lakade][25 09 2024]
       
        [DataMember]
        public TimeSpan AttendanceTime
        {
            get { return attendanceTime; }
            set
            {
                attendanceTime = value;
                OnPropertyChanged("AttendanceTime");
            }
        }

        [DataMember]
        public TimeSpan Regulartime
        {
            get { return regulartime; }
            set
            {
                regulartime = value;
                OnPropertyChanged("Regulartime");
            }
        }
        [DataMember]
        public TimeSpan OverTime
        {
            get { return overTime; }
            set
            {
                overTime = value;
                OnPropertyChanged("OverTime");
            }
        }
        [DataMember]
        public TimeSpan WorkingTime
        {
            get { return workingTime; }
            set
            {
                workingTime = value;
                OnPropertyChanged("WorkingTime");
            }
        }
        [DataMember]
        public TimeSpan Not_WorkTime
        {
            get { return not_WorkTime; }
            set
            {
                not_WorkTime = value;
                OnPropertyChanged("Not_WorkTime");
            }
        }
        [DataMember]
        public string StageName
        {
            get { return stageName; }
            set
            {
                stageName = value;
                OnPropertyChanged("StageName");
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
    }
}
