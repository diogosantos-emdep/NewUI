using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
   public class EmployeeWithBackupEmployeeDetails : ModelBase, IDisposable
    { 
        #region Fields
        Int32 idEmployeeStatus;
        Int32 idEmployee;
        string employeeCode;
        string firstName;
        string lastName;
        string backupEmployeeCode;
        Int32 idEmployeeBackup;
        string backupEmployeeFirstName;
        string backupEmployeeLastName;
        List<EmployeeLeave> backupEmployeeLeaves;
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
        public Int32 IdEmployeeBackup
        {
            get { return idEmployeeBackup; }
            set
            {
                idEmployeeBackup = value;
                OnPropertyChanged("IdEmployeeBackup");
            }
        }

        [DataMember]
        public string BackupEmployeeCode
        {
            get { return backupEmployeeCode; }
            set
            {
                backupEmployeeCode = value;
                OnPropertyChanged("BackupEmployeeCode");
            }
        }

        [DataMember]
        public string BackupEmployeeFirstName
        {
            get { return backupEmployeeFirstName; }
            set
            {
                backupEmployeeFirstName = value;
                OnPropertyChanged("BackupEmployeeFirstName");
            }
        }

        [DataMember]
        public string BackupEmployeeLastName
        {
            get { return backupEmployeeLastName; }
            set
            {
                backupEmployeeLastName = value;
                OnPropertyChanged("BackupEmployeeLastName");
            }
        }

        [DataMember]
        public List<EmployeeLeave> BackupEmployeeLeaves
        {
            get { return backupEmployeeLeaves; }
            set
            {
                backupEmployeeLeaves = value;
                OnPropertyChanged("BackupEmployeeLeaves");
            }
        }
        #endregion

        #region Constructor
        public EmployeeWithBackupEmployeeDetails()
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
