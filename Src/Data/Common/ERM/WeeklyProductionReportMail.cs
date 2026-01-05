using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class WeeklyProductionReportMail : ModelBase, IDisposable
    {
        #region Declarations
        private UInt32 idEmployee;
        private UInt32 idCompany;
        private string jobDescriptionCode;
        private string employeeContactValue;
        private UInt16 idRegion;
        private UInt32 idCountry;
        private UInt32 idJobDescription;
        private string firstName;
        private string lastName;
        private string employeeCode;

        #endregion

        #region Properties




        [DataMember]
        public UInt32 IdJobDescription
        {
            get { return idJobDescription; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }

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
        public UInt32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [DataMember]
        public string JobDescriptionCode
        {
            get { return jobDescriptionCode; }
            set
            {
                jobDescriptionCode = value;
                OnPropertyChanged("JobDescriptionCode");
            }
        }

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

        [DataMember]
        public UInt32 IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [DataMember]
        public UInt16 IdRegion
        {
            get { return idRegion; }
            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
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
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
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
