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
        decimal remaining;
        Int32 leaveBackground;
        decimal totalHoursCount;
        string regularHoursInDays;
        string additionalHoursInDays;
        string totalHoursInDays;
        string enjoyedHoursInDays;
        string remainingHoursInDays;
        #endregion

        #region Properties

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
        public decimal Remaining
        {
            get { return remaining; }
            set
            {
                remaining = value;
                OnPropertyChanged("Remaining");
            }
        }

        [NotMapped]
        [DataMember]
        public int LeaveBackground
        {
            get { return leaveBackground; }
            set
            {
                leaveBackground = value;
                OnPropertyChanged("LeaveBackground");
            }
        }

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
        public string RegularHoursInDays
        {
            get { return regularHoursInDays; }
            set
            {
                regularHoursInDays = value;

                OnPropertyChanged("RegularHoursInDays");
            }
        }

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
