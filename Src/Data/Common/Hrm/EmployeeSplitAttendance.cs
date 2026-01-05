using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class EmployeeSplitAttendance : ModelBase, IDisposable
    {
        #region Fields
        Int32 idEmployee;
        string employeeCode;
        string firstName;
        string lastName;
        DateTime startDate;
        DateTime endDate;
        DateTime accountingDate;
        Int32 idCompanyWork;
        string shiftType;
        Int32 idCompanyShift;
        string shiftName;
        Int32 idEmployeeStatus;
        bool isShiftTypeRegular;
        #endregion

        #region Properties
        [NotMapped]
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
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { }
        }

        [NotMapped]
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

        [NotMapped]
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
        public DateTime AccountingDate
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
        public Int32 IdCompanyWork
        {
            get { return idCompanyWork; }
            set
            {
                idCompanyWork = value;
                OnPropertyChanged("IdCompanyWork");
            }
        }

        [NotMapped]
        [DataMember]
        public string ShiftType
        {
            get { return shiftType; }
            set
            {
                shiftType = value;
                OnPropertyChanged("ShiftType");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public string ShiftName
        {
            get { return shiftName; }
            set
            {
                shiftName = value;
                OnPropertyChanged("ShiftName");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdEmployeeStatus
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
        public bool IsShiftTypeRegular
        {
            get { return isShiftTypeRegular; }
            set
            {
                isShiftTypeRegular = value;
                OnPropertyChanged("IsShiftTypeRegular");
            }
        }
        #endregion

        #region Constructor
        public EmployeeSplitAttendance()
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
