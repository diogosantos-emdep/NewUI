using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMPlantORG_EmployeeDetails : ModelBase, IDisposable
    {
        #region Field
        private Int32 idEmployee;
        private Int32 idCompany;
        private string employeeName;
        private double dailyHoursCount;
        private string employeeCode;
        private Int32 idJobDescription;
        private Int32 jobDescriptionUsage;
        private DateTime? jobDescriptionStartDate;
        private DateTime? jobDescriptionEndDate;
        private DateTime? holidayStartDate;
        private DateTime? holidayEndDate;
        private Int16 isAllDayEvent;
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
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }
        // [NotMapped]
        [DataMember]
        public string EmployeeName
        {
            get { return employeeName; }
            set
            {
                employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }
        [DataMember]
        public double DailyHoursCount
        {
            get { return dailyHoursCount; }
            set
            {
                dailyHoursCount = value;
                OnPropertyChanged("DailyHoursCount");
            }
        }
        [DataMember]
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }

        [DataMember]
        public Int32 IdJobDescription
        {
            get { return idJobDescription; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }
        [DataMember]
        public Int32 JobDescriptionUsage
        {
            get { return jobDescriptionUsage; }
            set
            {
                jobDescriptionUsage = value;
                OnPropertyChanged("JobDescriptionUsage");
            }
        }
        [DataMember]
        public DateTime? JobDescriptionStartDate
        {
            get { return jobDescriptionStartDate; }
            set
            {
                jobDescriptionStartDate = value;
                OnPropertyChanged("JobDescriptionStartDate");
            }
        }
        [DataMember]
        public DateTime? JobDescriptionEndDate
        {
            get { return jobDescriptionEndDate; }
            set
            {
                jobDescriptionEndDate = value;
                OnPropertyChanged("JobDescriptionEndDate");
            }
        }
        [DataMember]
        public DateTime? HolidayStartDate
        {
            get { return holidayStartDate; }
            set
            {
                holidayStartDate = value;
                OnPropertyChanged("HolidayStartDate");
            }
        }
        [DataMember]
        public DateTime? HolidayEndDate
        {
            get { return holidayEndDate; }
            set
            {
                holidayEndDate = value;
                OnPropertyChanged("HolidayEndDate");
            }
        }
        [DataMember]
        public Int16 IsAllDayEvent
        {
            get { return isAllDayEvent; }
            set
            {
                isAllDayEvent = value;
                OnPropertyChanged("IsAllDayEvent");
            }
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
        #region Constructor
        public ERMPlantORG_EmployeeDetails()
        {

        }
        #endregion
    }
}
