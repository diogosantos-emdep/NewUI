using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Emdep.Geos.Data.Common.Epc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Emdep.Geos.Data.Common.Hrm
{

    [DataContract]
    public class EmployeeWithInactiveBackupEmployees : ModelBase, IDisposable
    {
        #region Fields
        Int32 idCompany;
        Int32 idEmployeeStatus;
        Int32 idEmployeeInactive;
        string employeeCodeInactive;
        string inactiveEmployeeFirstName;
        string inactiveEmployeeLastName;
        Int32 idEmployee;
        string employeeCode;
        string firstName;
        string lastName;
        #endregion

        #region Properties
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

        [DataMember]
        public Int32 IdEmployeeInactive
        {
            get { return idEmployeeInactive; }
            set
            {
                idEmployeeInactive = value;
                OnPropertyChanged("IdEmployeeInactive");
            }
        }

        [DataMember]
        public string EmployeeCodeInactive
        {
            get { return employeeCodeInactive; }
            set
            {
                employeeCodeInactive = value;
                OnPropertyChanged("EmployeeCodeInactive");
            }
        }

        [DataMember]
        public string InactiveEmployeeFirstName
        {
            get { return inactiveEmployeeFirstName; }
            set
            {
                inactiveEmployeeFirstName = value;
                OnPropertyChanged("InactiveEmployeeFirstName");
            }
        }

        [DataMember]
        public string InactiveEmployeeLastName
        {
            get { return inactiveEmployeeLastName; }
            set
            {
                inactiveEmployeeLastName = value;
                OnPropertyChanged("InactiveEmployeeLastName");
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
        #endregion

        #region Constructor
        public EmployeeWithInactiveBackupEmployees()
        { }
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