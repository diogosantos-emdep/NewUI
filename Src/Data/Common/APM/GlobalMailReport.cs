using Emdep.Geos.Data.Common.Hrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
//[shweta.thube][GEOS2-6048][03.09.2025]
namespace Emdep.Geos.Data.Common.APM
{
    public class GlobalMailReport : ModelBase, IDisposable
    {
        #region Declarations
        private UInt32 idCompany;
        private string toDo;
        private string alias;
        private string inProgress;
        private string blocked;
        private string done;
        private UInt32 idEmployee;
        private string jobDescriptionCode;
        private string employeeContactValue;
        private UInt16 idRegion;
        private UInt32 idCountry;
        private UInt32 idJobDescription;
        private string firstName;
        private string lastName;
        private string employeeCode;
        private string openTaskCount;
        private string closedTaskCount;
        private string totalNumberOfTasks;
        private int createdCount;
        #endregion

        #region Properties

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
        public string Alias
        {
            get { return alias; }
            set
            {
                alias = value;
                OnPropertyChanged("Alias");
            }
        }

        [DataMember]
        public string ToDo
        {
            get { return toDo; }
            set
            {
                toDo = value;
                OnPropertyChanged("ToDo");
            }
        }

        [DataMember]
        public string InProgress
        {
            get { return inProgress; }
            set
            {
                inProgress = value;
                OnPropertyChanged("InProgress");
            }
        }

        [DataMember]
        public string Done
        {
            get { return done; }
            set
            {
                done = value;
                OnPropertyChanged("Done");
            }
        }

        [DataMember]
        public string Blocked
        {
            get { return blocked; }
            set
            {
                blocked = value;
                OnPropertyChanged("Blocked");
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
        public string OpenTaskCount
        {
            get { return openTaskCount; }
            set
            {
                openTaskCount = value;
                OnPropertyChanged("OpenTaskCount");
            }
        }

        [DataMember]
        public string ClosedTaskCount
        {
            get { return closedTaskCount; }
            set
            {
                closedTaskCount = value;
                OnPropertyChanged("ClosedTaskCount");
            }
        }

        [DataMember]
        public string TotalNumberOfTasks
        {
            get { return totalNumberOfTasks; }
            set
            {
                totalNumberOfTasks = value;
                OnPropertyChanged("TotalNumberOfTasks");
            }
        }
        [DataMember]
        public int CreatedCount
        {
            get { return createdCount; }
            set
            {
                createdCount = value;
                OnPropertyChanged("CreatedCount");
            }
        }
        
        #endregion

        #region Constructor
        public GlobalMailReport()
        {

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
