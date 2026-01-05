using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_EmployeeDetails : ModelBase, IDisposable
    {
        #region Field
        //Employee
        private Int32 idEmployee;
        private string employeeCode;
        private string employeeName;
        private Int32 idUser;
        //Jobdescription
        private Int32 idJobDescription;
        private DateTime? jobDescriptionStartDate;
        private DateTime? jobDescriptionEndDate;
        private Int32 jobDescriptionUsage;
        private Int32 isMainJobDescription;
        //Site
        private Int32 idSite;
        private string siteName;
        #endregion
        #region Property
        //Employee
        [DataMember]
        public Int32 IdEmployee
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
        public string EmployeeCode
        {
            get
            {
                return employeeCode;
            }

            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }
        [DataMember]
        public string EmployeeName
        {
            get
            {
                return employeeName;
            }

            set
            {
                employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }
        [DataMember]
        public Int32 IdUser
        {
            get
            {
                return idUser;
            }

            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        //Jobdescription
        [DataMember]
        public Int32 IdJobDescription
        {
            get { return idJobDescription; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }

        [DataMember]
        public DateTime? JobDescriptionStartDate
        {
            get
            {
                return jobDescriptionStartDate;
            }

            set
            {
                jobDescriptionStartDate = value;
                OnPropertyChanged("JobDescriptionStartDate");
            }
        }
        [DataMember]
        public DateTime? JobDescriptionEndDate
        {
            get
            {
                return jobDescriptionEndDate;
            }

            set
            {
                jobDescriptionEndDate = value;
                OnPropertyChanged("JobDescriptionEndDate");
            }
        }

        [DataMember]
        public Int32 JobDescriptionUsage
        {
            get
            {
                return jobDescriptionUsage;
            }

            set
            {
                jobDescriptionUsage = value;
                OnPropertyChanged("JobDescriptionUsage");
            }
        }
        [DataMember]
        public Int32 IsMainJobDescription
        {
            get
            {
                return isMainJobDescription;
            }

            set
            {
                isMainJobDescription = value;
                OnPropertyChanged("IsMainJobDescription");
            }
        }
        //Site
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [DataMember]
        public string SiteName
        {
            get
            {
                return siteName;
            }

            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
            }
        }
        #endregion
        #region Constructor
        public ERM_EmployeeDetails()
        {

        }
        #endregion
        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
