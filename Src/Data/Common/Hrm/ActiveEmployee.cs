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
    [DataContract]
    public class ActiveEmployee : ModelBase, IDisposable
    {
        Int32 idEmployee;
        [Key]
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

        bool isActive;
        [Key]
        [Column("IsActive")]
        [DataMember]
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        string employeeCode;
        [Column("EmployeeCode")]
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

        string firstName;
        [Column("FirstName")]
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        string lastName;
        [Column("LastName")]
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        byte isEnabled;
        [Column("IsEnabled")]
        [DataMember]
        public byte IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        Int32? idEmployeeStatus;
        [Column("IdEmployeeStatus")]
        [DataMember]
        public Int32? IdEmployeeStatus
        {
            get { return idEmployeeStatus; }
            set
            {
                idEmployeeStatus = value;
                OnPropertyChanged("IdEmployeeStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { }
        }

        Int32 idCompany;
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

        string company;
        [Column("Company")]
        [DataMember]
        public string Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        string contractSituation;
        [Column("Company")]
        [DataMember]
        public string ContractSituation
        {
            get { return contractSituation; }
            set
            {
                contractSituation = value;
                OnPropertyChanged("ContractSituation");
            }
        }

        DateTime contractSituationStartDate;
        [DataMember]
        public DateTime ContractSituationStartDate
        {
            get { return contractSituationStartDate; }
            set
            {
                contractSituationStartDate = value;
                OnPropertyChanged("ContractSituationStartDate");
            }
        }

        DateTime contractSituationEndDate;
        [DataMember]
        public DateTime ContractSituationEndDate
        {
            get { return contractSituationEndDate; }
            set
            {
                contractSituationEndDate = value;
                OnPropertyChanged("ContractSituationEndDate");
            }
        }

        string isEmployee;
        [DataMember]
        public string IsEmployee
        {
            get { return isEmployee; }
            set
            {
                isEmployee = value;
                OnPropertyChanged("IsEmployee");
            }
        }

        bool isAnnualLeavesExist;
        [DataMember]
        public bool IsAnnualLeavesExist
        {
            get { return isAnnualLeavesExist; }
            set
            {
                isAnnualLeavesExist = value;
                OnPropertyChanged("IsAnnualLeavesExist");
            }
        }

        Int32 idHoliday;
        [DataMember]
        public Int32 IdHoliday
        {
            get { return idHoliday; }
            set
            {
                idHoliday = value;
                OnPropertyChanged("IdHoliday");
            }
        }

        string holiday;
        [DataMember]
        public string Holiday
        {
            get { return holiday; }
            set
            {
                holiday = value;
                OnPropertyChanged("Holiday");
            }
        }

        Int32 modifiedBy;
        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        DateTime modifiedIn;
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        Int32 employeeYearsOfService;
        [DataMember]
        public Int32 EmployeeYearsOfService
        {
            get { return employeeYearsOfService; }
            set
            {
                employeeYearsOfService = value;
                OnPropertyChanged("EmployeeYearsOfService");
            }
        }

        double holidayDays;
        [DataMember]
        public double HolidayDays
        {
            get { return holidayDays; }
            set
            {
                holidayDays = value;
                OnPropertyChanged("HolidayDays");
            }
        }

        double dailyHoursCount;
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

        double totalHours;
        [DataMember]
        public double TotalHours
        {
            get { return totalHours; }
            set
            {
                totalHours = value;
                OnPropertyChanged("TotalHours");
            }
        }
        #region Methods

        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            ActiveEmployee activeEmployee = (ActiveEmployee)this.MemberwiseClone();
            return activeEmployee;
        }

        #endregion
    }
}
