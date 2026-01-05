using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMEmployeeInformation : ModelBase, IDisposable
    {
        #region Field
        private Int32 idEmployee;
        //private Int32 idCompany;
        private string employeeName;
        private Int32 dailyHoursCount;
        private Int32 weeklyHoursCount;
        //private TimeSpan breaktime;
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
        //[DataMember]
        //public TimeSpan BreakTime
        //{
        //    get { return breaktime; }
        //    set
        //    {
        //        breaktime = value;
        //        OnPropertyChanged("Breaktime");
        //    }
        //}
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
        public Int32 WeeklyHoursCount
        {
            get { return weeklyHoursCount; }
            set
            {
                weeklyHoursCount = value;
                OnPropertyChanged("WeeklyHoursCount");
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
        public ERMEmployeeInformation()
        {

        }
        #endregion
    }
}
