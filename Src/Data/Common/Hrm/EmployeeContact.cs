using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("employee_contacts")]
    [DataContract]
    public class EmployeeContact : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idEmployeeContact;
        Int32 idEmployee;
        Int32 employeeContactIdType;
        string employeeContactValue;
        string employeeContactRemarks;
        byte isCompanyUse;

        LookupValue employeeContactType;

        #endregion

        #region Constructor

        public EmployeeContact()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeContact")]
        [DataMember]
        public UInt64 IdEmployeeContact
        {
            get { return idEmployeeContact; }
            set
            {
                idEmployeeContact = value;
                OnPropertyChanged("IdEmployeeContact");
            }
        }

        [Column("IdEmployee")]
        [DataMember]
        public int IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [Column("EmployeeContactIdType")]
        [DataMember]
        public int EmployeeContactIdType
        {
            get { return employeeContactIdType; }
            set
            {
                employeeContactIdType = value;
                OnPropertyChanged("EmployeeContactIdType");
            }
        }

        [Column("EmployeeContactValue")]
        [DataMember]
        public string EmployeeContactValue
        {
            get { return employeeContactValue; }
            set
            {
                employeeContactValue = value;
                OnPropertyChanged("EmployeeContactValue");
            }
        }

        [Column("EmployeeContactRemarks")]
        [DataMember]
        public string EmployeeContactRemarks
        {
            get { return employeeContactRemarks; }
            set
            {
                employeeContactRemarks = value;
                OnPropertyChanged("EmployeeContactRemarks");
            }
        }

        [Column("IsCompanyUse")]
        [DataMember]
        public byte IsCompanyUse
        {
            get { return isCompanyUse; }
            set
            {
                isCompanyUse = value;
                OnPropertyChanged("IsCompanyUse");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue EmployeeContactType
        {
            get { return employeeContactType; }
            set
            {
                employeeContactType = value;
                OnPropertyChanged("EmployeeContactType");
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
            EmployeeContact employeeContact = (EmployeeContact)this.MemberwiseClone();

            if (employeeContact.EmployeeContactType != null)
                employeeContact.EmployeeContactType = (LookupValue)this.EmployeeContactType.Clone();

            return employeeContact;
        }

        #endregion
    }
}
