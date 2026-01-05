using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{//[sudhir.jangra][GEOS2-4816]
    public class Traveller : ModelBase, IDisposable
    {
        #region Fields
        UInt32 idDepartment;
        string departmentName;
        private UInt32 idEmployee;
        private string employeeCode;
        private string firstName;
        private string lastName;
        private int idGender;
        private Int32 idCompanyShift;
        Currency currency;
        Int32 idOrganizationCode;
        string organizationCode;
        Int32 idOrganizationRegion;
        string organizationRegion;
        int idEmployeeStatus;
        bool isInActive;
        string email;
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }
        [DataMember]
        public bool IsInActive
        {
            get { return isInActive; }
            set
            {
                isInActive = value;
                OnPropertyChanged("IdEmployee");
            }
        }
        [DataMember]
        public string Code
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("Code");
            }
        }

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
        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { }
        }
        [DataMember]
        public int IdGender
        {
            get { return idGender; }
            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
            }
        }

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
        public Currency Currency
        {
            get
            {
                return currency;
            }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        [DataMember]
        public Int32 IdOrganizationCode
        {
            get { return idOrganizationCode; }
            set
            {
                idOrganizationCode = value;
                OnPropertyChanged("IdOrganizationCode");
            }
        }

        [DataMember]
        public string Organization
        {
            get { return organizationCode; }
            set
            {
                organizationCode = value;
                OnPropertyChanged("Organization");
            }
        }

        [DataMember]
        public Int32 IdOrganizationRegion
        {
            get { return idOrganizationRegion; }
            set
            {
                idOrganizationRegion = value;
                OnPropertyChanged("IdOrganizationRegion");
            }
        }

        [DataMember]
        public string OrganizationRegion
        {
            get { return organizationRegion; }
            set
            {
                organizationRegion = value;
                OnPropertyChanged("OrganizationRegion");
            }
        }

        [DataMember]
        public int IdEmployeeStatus
        {
            get { return idEmployeeStatus; }
            set
            {
                idEmployeeStatus = value;
                OnPropertyChanged("IdEmployeeStatus");
            }
        }
		// [nsatpute][22-10-2024][GEOS2-6656]
        [DataMember]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        [DataMember]
        public UInt32 IdDepartment
        {
            get { return idDepartment; }
            set
            {
                idDepartment = value;
                OnPropertyChanged("IdDepartment");
            }
        }

        [DataMember]
        public string DepartmentName
        {
            get { return departmentName; }
            set
            {
                departmentName = value;
                OnPropertyChanged("DepartmentName");
            }
        }
        int idApprovalResponsibleJD;
        [DataMember]
        public int IdApprovalResponsibleJD
        {
            get { return idApprovalResponsibleJD; }
            set
            {
                idApprovalResponsibleJD = value;
                OnPropertyChanged("IdApprovalResponsibleJD");
            }
        }
        
        #endregion


        #region Constructor
        public Traveller()
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

        public override string ToString()
        {
            return FullName;
        }

        #endregion
    }
}
