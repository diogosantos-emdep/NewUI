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

    [DataContract]
    public class EmployeeOrganizationChart : ModelBase, IDisposable
    {
        #region Fields

        Int32 idEmployee;
        string jdCode;
        string jobCode;
        string departmentName;
        string organization;
        UInt16 jobDescriptionUsage;
        string employeeCode;
        string firstName;
        string lastName;
        string jobTitle;
        DateTime? birthDate;
        DateTime? hireDate;
        string employeeStatus;
        string companyLocation;
        string company;
        List<EmployeeContractSituation> employeeContractSituations;
        string lengthOfServiceString;
        string jdRemote;
        #endregion


        #region Properties

      [Key]
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

        [DataMember]
        public string JDCode
        {
            get { return jdCode; }
            set
            {
                jdCode = value;
                OnPropertyChanged("JDCode");
            }
        }

        [DataMember]
        public string JobCode
        {
            get { return jobCode; }
            set
            {
                jobCode = value;
                OnPropertyChanged("JobCode");
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

        [DataMember]
        public string JDRemote
        {
            get { return jdRemote; }
            set
            {
                jdRemote = value;
                OnPropertyChanged("JDRemote");
            }
        }

        [DataMember]
        public string Organization
        {
            get { return organization; }
            set
            {
                organization = value;
                OnPropertyChanged("Organization");
            }
        }


        [NotMapped]
        [DataMember]
        public UInt16 JobDescriptionUsage
        {
            get { return jobDescriptionUsage; }
            set
            {
                jobDescriptionUsage = value;
                OnPropertyChanged("JobDescriptionUsage");
            }
        }

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
        public string JobTitle
        {
            get { return jobTitle; }
            set
            {
                jobTitle = value;
                OnPropertyChanged("JobTitle");
            }
        }

        [DataMember]
        public DateTime? BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }

        [DataMember]
        public DateTime? HireDate
        {
            get { return hireDate; }
            set
            {
                hireDate = value;
                OnPropertyChanged("HireDate");
            }
        }

        [DataMember]
        public string EmployeeStatus
        {
            get { return employeeStatus; }
            set
            {
                employeeStatus = value;
                OnPropertyChanged("EmployeeStatus");
            }
        }

        [DataMember]
        public string CompanyLocation
        {
            get { return companyLocation; }
            set
            {
                companyLocation = value;
                OnPropertyChanged("CompanyLocation");
            }
        }

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

        [NotMapped]
        [DataMember]
        public List<EmployeeContractSituation> EmployeeContractSituations
        {
            get { return employeeContractSituations; }
            set
            {
                employeeContractSituations = value;
                OnPropertyChanged("EmployeeContractSituations");
            }
        }

        [NotMapped]
        [DataMember]
        public string LengthOfServiceString
        {
            get { return lengthOfServiceString; }
            set
            {
                lengthOfServiceString = value;
                OnPropertyChanged("LengthOfServiceString");
            }
        }

     
        #endregion


        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }       

        #endregion

    }
}
