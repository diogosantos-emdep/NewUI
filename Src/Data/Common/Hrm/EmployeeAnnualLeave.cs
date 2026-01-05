using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("employee_annual_leaves")]
    [DataContract]
    public class EmployeeAnnualLeave : ModelBase, IDisposable
    {
        #region Fields

        Int64 idEmployeeAnnualLeave;
        Int32 idEmployee;
        Int32 year;
        Int64 idLeave;
        decimal regularHoursCount;
        decimal additionalHoursCount;
        Employee employee;
        CompanyLeave companyLeave;
        decimal enjoyed;
        //GEOS2-2501
        decimal enjoyedTillYesterday;
        decimal enjoyingFromToday;
        decimal remaining;
        //Int32 leaveBackground;
        decimal totalHoursCount;
        string regularHoursInDays;
        string additionalHoursInDays;
        string totalHoursInDays;
        string enjoyedHoursInDays;
        string remainingHoursInDays;
        JobDescription jobDescription;
        string companyLocation;
        private decimal backlogHoursCount;
        List<EmployeeAnnualAdditionalLeave> selectedAdditionalLeaves;
        //GEOS2-2607
        string backlogHoursInDays;
        string enjoyedTillYesterdayInDays;
        string enjoyingFromTodayInDays;
        int leaveInUse;
        decimal convertedHours;//[Sudhir.jangra][GEOs2-5336]
        bool isExistinEmpLocation;
        bool isLeaveAvailable;
        #endregion

        #region Properties

        [DataMember]
        public bool IsLeaveAvailable
        {
            get { return isLeaveAvailable; }
            set
            {
                isLeaveAvailable = value;
                OnPropertyChanged("IsLeaveAvailable");
            }
        }
        [DataMember]
        public bool IsExistinEmpLocation
        {
            get { return isExistinEmpLocation; }
            set
            {
                isExistinEmpLocation = value;
                OnPropertyChanged("IsExistinEmpLocation");
            }
        }
        [Key]
        [Column("IdEmployeeAnnualLeave")]
        [DataMember]
        public Int64 IdEmployeeAnnualLeave
        {
            get { return idEmployeeAnnualLeave; }
            set
            {
                idEmployeeAnnualLeave = value;
                OnPropertyChanged("IdEmployeeAnnualLeave");
            }
        }

        [Column("IdEmployee")]
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

        [Column("Year")]
        [DataMember]
        public Int32 Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        [Column("IdLeave")]
        [DataMember]
        public Int64 IdLeave
        {
            get { return idLeave; }
            set
            {
                idLeave = value;
                OnPropertyChanged("IdLeave");
            }
        }


        [Column("RegularHoursCount")]
        [DataMember]
        public decimal RegularHoursCount
        {
            get { return regularHoursCount; }
            set
            {
                regularHoursCount = value;

                OnPropertyChanged("RegularHoursCount");
            }
        }

        [Obsolete("Use SelectedAdditionalLeaves")]
        [Column("AdditionalHoursCount")]
        [DataMember]
        public decimal AdditionalHoursCount
        {
            get { return additionalHoursCount; }
            set
            {
                additionalHoursCount = value;

                OnPropertyChanged("AdditionalHoursCount");
            }
        }

        [NotMapped]
        [DataMember]
        public Employee Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }

        [NotMapped]
        [DataMember]
        public CompanyLeave CompanyLeave
        {
            get { return companyLeave; }
            set
            {
                companyLeave = value;
                OnPropertyChanged("CompanyLeave");
            }
        }

        [Obsolete("Use EnjoyedTillYesterday and EnjoyingFromToday")]
        [NotMapped]
        [DataMember]
        public decimal Enjoyed
        {
            get { return enjoyed; }
            set
            {
                enjoyed = value;
                OnPropertyChanged("Enjoyed");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal EnjoyedTillYesterday
        {
            get { return enjoyedTillYesterday; }
            set
            {
                enjoyedTillYesterday = value;
                OnPropertyChanged("EnjoyedTillYesterday");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal EnjoyingFromToday
        {
            get { return enjoyingFromToday; }
            set
            {
                enjoyingFromToday = value;
                OnPropertyChanged("EnjoyingFromToday");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal Remaining
        {
            get { return remaining; }
            set
            {
                remaining = value;
                OnPropertyChanged("Remaining");
            }
        }

        //[NotMapped]
        //[DataMember]
        //public int LeaveBackground
        //{
        //    get { return leaveBackground; }
        //    set
        //    {
        //        leaveBackground = value;
        //        OnPropertyChanged("LeaveBackground");
        //    }
        //}

        [NotMapped]
        [DataMember]
        public decimal TotalHoursCount
        {
            get { return totalHoursCount; }
            set
            {
                totalHoursCount = value;

                OnPropertyChanged("TotalHoursCount");
            }
        }

        [NotMapped]
        [DataMember]
        public string BacklogHoursInDays
        {
            get { return backlogHoursInDays; }
            set
            {
                backlogHoursInDays = value;

                OnPropertyChanged(nameof(BacklogHoursInDays));
            }
        }

        [NotMapped]
        [DataMember]
        public string RegularHoursInDays
        {
            get { return regularHoursInDays; }
            set
            {
                regularHoursInDays = value;

                OnPropertyChanged("RegularHoursInDays");
            }
        }

        [Obsolete("Use SelectedAdditionalLeaves")]
        [NotMapped]
        [DataMember]
        public string AdditionalHoursInDays
        {
            get { return additionalHoursInDays; }
            set
            {
                additionalHoursInDays = value;

                OnPropertyChanged("AdditionalHoursInDays");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalHoursInDays
        {
            get { return totalHoursInDays; }
            set
            {
                totalHoursInDays = value;

                OnPropertyChanged("TotalHoursInDays");
            }
        }

        //GEOS2-2607
        [NotMapped]
        [DataMember]
        public string EnjoyedTillYesterdayInDays
        {
            get { return enjoyedTillYesterdayInDays; }
            set
            {
                enjoyedTillYesterdayInDays = value;
                OnPropertyChanged(nameof(EnjoyedTillYesterdayInDays));
            }
        }

        [NotMapped]
        [DataMember]
        public string EnjoyingFromTodayInDays
        {
            get { return enjoyingFromTodayInDays; }
            set
            {
                enjoyingFromTodayInDays = value;
                OnPropertyChanged(nameof(EnjoyingFromTodayInDays));
            }
        }

        [Obsolete("Use EnjoyedTillYesterdayInDays, EnjoyingFromTodayInDays")]
        [NotMapped]
        [DataMember]
        public string EnjoyedHoursInDays
        {
            get { return enjoyedHoursInDays; }
            set
            {
                enjoyedHoursInDays = value;
                OnPropertyChanged("EnjoyedHoursInDays");
            }
        }
        [NotMapped]
        [DataMember]
        public string RemainingHoursInDays
        {
            get { return remainingHoursInDays; }
            set
            {
                remainingHoursInDays = value;

                OnPropertyChanged("RemainingHoursInDays");
            }
        }

        [NotMapped]
        [DataMember]
        public JobDescription JobDescription
        {
            get
            {
                return jobDescription;
            }

            set
            {
                jobDescription = value;
                OnPropertyChanged("JobDescription");
            }
        }

        [NotMapped]
        [DataMember]
        public string CompanyLocation
        {
            get
            {
                return companyLocation;
            }

            set
            {
                companyLocation = value;
                OnPropertyChanged("CompanyLocation");
            }
        }


        [Column("BacklogHoursCount")]
        [DataMember]
        public decimal BacklogHoursCount
        {
            get { return backlogHoursCount; }
            set
            {
                backlogHoursCount = value;

                OnPropertyChanged("BacklogHoursCount");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeAnnualAdditionalLeave> SelectedAdditionalLeaves
        {
            get
            {
                return selectedAdditionalLeaves;
            }

            set
            {
                this.selectedAdditionalLeaves = value;
                OnPropertyChanged(nameof(SelectedAdditionalLeaves));
            }
        }

        [NotMapped]
        [DataMember]
        public int LeaveInUse
        {
            get { return leaveInUse; }
            set
            {
                leaveInUse = value;

                OnPropertyChanged("LeaveInUse");
            }
        }

        //[Sudhir.jangra][GEOS2-5336]
        [NotMapped]
        [DataMember]
        public decimal ConvertedHours
        {
            get { return convertedHours; }
            set
            {
                convertedHours = value;
                OnPropertyChanged("ConvertedHours");
            }
        }

        #endregion

        #region Constructor

        public EmployeeAnnualLeave()
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
