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
    public class EmployeeHoliday : ModelBase, IDisposable
    {

        #region Fields
        Int32 idEmployee;
        string firstName;
        string lastName;
        string company;
        DateTime jDStartDate;
        List<EmployeeAnnualLeave> employeeAnnualLeaves;
        string jDCode;
        string email;
        int idJobDescription;
        DateTime jDEndDate;
        int idJDScope;
        int idCompany;
        string region;
        int idCountry;
        string companyLocation;//[GEOS2-3093]
        JobDescription jobDescription;//[GEOS2-3093]
        DateTime leaveStartDate;//[GEOS2-3093]
        DateTime leaveEndDate;//[GEOS2-3093]

        #endregion

        #region Constructor

        public EmployeeHoliday()
        {
        }

        #endregion

        #region Properties

        //[GEOS2-3093]
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

        //[GEOS2-3093]
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
        public DateTime LeaveStartDate
        {
            get
            {
                return leaveStartDate;
            }

            set
            {
                leaveStartDate = value;
                OnPropertyChanged("LeaveStartDate");
            }
        }
        
        [NotMapped]
        [DataMember]
        public DateTime LeaveEndDate
        {
            get
            {
                return leaveEndDate;
            }

            set
            {
                leaveEndDate = value;
                OnPropertyChanged("LeaveEndDate");
            }
        }

        [DataMember]
        public List<EmployeeAnnualLeave> EmployeeAnnualLeaves
        {
            get
            {
                return employeeAnnualLeaves;
            }

            set
            {
                employeeAnnualLeaves = value;
                OnPropertyChanged("EmployeeAnnualLeaves");
            }
        }

        [DataMember]
        public string JDCode
        {
            get
            {
                return jDCode;
            }

            set
            {
                jDCode = value;
                OnPropertyChanged("JDCode");
            }
        }

        [DataMember]
        public int IdEmployee
        {
            get
            {
                return idEmployee;
            }

            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [DataMember]
        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        [DataMember]
        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        [DataMember]
        public string Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }
        
        [Obsolete("Use LeaveStartDate,LeaveEndDate for leaves.")]        
        [DataMember]
        public DateTime JDStartDate
        {
            get
            {
                return jDStartDate;
            }

            set
            {
                jDStartDate = value;
                OnPropertyChanged("JDStartDate");
            }
        }

        [DataMember]
        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { }
        }

        [DataMember]
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        [DataMember]
        public int IdJobDescription
        {
            get
            {
                return idJobDescription;
            }

            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }

        [Obsolete("Use LeaveStartDate,LeaveEndDate for leaves.")]
        [DataMember]
        public DateTime JDEndDate
        {
            get
            {
                return jDEndDate;
            }

            set
            {
                jDEndDate = value;
                OnPropertyChanged("JDEndDate");
            }
        }

        [DataMember]
        public int IdJDScope
        {
            get
            {
                return idJDScope;
            }

            set
            {
                idJDScope = value;
                OnPropertyChanged("IdJDScope");
            }
        }

        [DataMember]
        public int IdCompany
        {
            get
            {
                return idCompany;
            }

            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [DataMember]
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [DataMember]
        public int IdCountry
        {
            get
            {
                return idCountry;
            }

            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
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
            EmployeeHoliday EmployeeHoliday = (EmployeeHoliday)this.MemberwiseClone();

            return EmployeeHoliday;
        }

        #endregion
    }
}
