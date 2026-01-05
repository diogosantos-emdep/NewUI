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
    [Table("employee_attendance")]
    [DataContract]
    public class EmployeeAttendance : ModelBase, IDisposable
    {
        #region Fields

        Int64 idEmployeeAttendance;
        Int32 idEmployee;
        Int32 idCompanyWork;
        Int32 idCompanyShift;
        DateTime startDate;
        TimeSpan? startTime;
        DateTime endDate;
        TimeSpan? endTime;
        Employee employee;
        CompanyWork companyWork;
        CompanyShift companyShift;
        string employeeCode;
        TimeSpan? totalTime;
        Int32 weekNumber;
        string clockID;
        Company companyContract;
        DateTime? accountingDate;
        string currentContractForAttendance;
        byte isManual;
        #endregion

        #region Properties

       [Key]
        [Column("IdEmployeeAttendance")]
        [DataMember]
        public Int64 IdEmployeeAttendance
        {
            get { return idEmployeeAttendance; }
            set
            {
                idEmployeeAttendance = value;
                OnPropertyChanged("IdEmployeeAttendance");
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

        [Column("IdCompanyWork")]
        [DataMember]
        public Int32 IdCompanyWork
        {
            get { return idCompanyWork; }
            set
            {
                idCompanyWork = value;
                OnPropertyChanged("IdCompanyWork");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }


        [Column("EndDate")]
        [DataMember]
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;

                OnPropertyChanged("EndDate");
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
        public CompanyWork CompanyWork
        {
            get { return companyWork; }
            set
            {
                companyWork = value;
                OnPropertyChanged("CompanyWork");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdCompanyShift
        {
            get { return idCompanyShift; }
            set
            {
                idCompanyShift = value;
                OnPropertyChanged("IdCompanyShift");
            }
        }

        [NotMapped]
        [DataMember]
        public CompanyShift CompanyShift
        {
            get { return companyShift; }
            set
            {
                companyShift = value;
                OnPropertyChanged("CompanyShift");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? TotalTime
        {
            get { return totalTime; }
            set
            {
                totalTime = value;
                OnPropertyChanged("TotalTime");
            }
        }


        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public int WeekNumber
        {
            get { return weekNumber; }
            set
            {
                weekNumber = value;
                OnPropertyChanged("WeekNumber");
            }
        }

        [NotMapped]
        [DataMember]
        public string ClockID
        {
            get { return clockID; }
            set
            {
                clockID = value;
                OnPropertyChanged("ClockID");
            }
        }


        [NotMapped]
        [DataMember]
        public Company CompanyContract
        {
            get { return companyContract; }
            set
            {
                companyContract = value;
                OnPropertyChanged("CompanyContract");
            }
        }


        [Column("AccountingDate")]
        [DataMember]
        public DateTime? AccountingDate
        {
            get { return accountingDate; }
            set
            {
                accountingDate = value;

                OnPropertyChanged("AccountingDate");
            }
        }

        [NotMapped]
        [DataMember]
        public string CurrentContractForAttendance
        {
            get { return currentContractForAttendance; }
            set
            {
                currentContractForAttendance = value;

                OnPropertyChanged("CurrentContractForAttendance");
            }
        }


        [NotMapped]
        [DataMember]
        public byte IsManual
        {
            get { return isManual; }
            set
            {
                isManual = value;
                OnPropertyChanged("IsManual");
            }
        }
        #endregion

        #region Constructor

        public EmployeeAttendance()
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
