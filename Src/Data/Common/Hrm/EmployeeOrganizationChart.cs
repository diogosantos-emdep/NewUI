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


        string monStartTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string monEndTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string monBreakTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string tueStartTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string tueEndTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string tueBreakTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string wedStartTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string wedEndTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string wedBreakTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string thuStartTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string thuEndTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string thuBreakTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string friStartTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string friEndTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string friBreakTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string satStartTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string satEndTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string satBreakTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string sunStartTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string sunEndTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string sunBreakTime;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string totalWorkingHours;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string shiftName;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        string shiftType;//[Sudhir.Jangra][GEOS2-4038][03/07/2023]
        DateTime accountingDate;//[Sudhir.Jangra][GEOS2-4038][11/07/2023]
        string backupEmployeeCode;//[Sudhir.Jangra][GEOS2-4038][17/07/2023]



        #region GEOS2-4038  Total Weekly HOurs
       public TimeSpan monstartTimeTotal { get; set; }
        public TimeSpan monEndTimeTotal { get; set; }

        public TimeSpan monBreakTimeTotal { get; set; }
        public TimeSpan tueStartTimeTotal { get; set; }

        public TimeSpan tueEndTimeTotal { get; set; }
        public TimeSpan tueBreakTimeTotal { get; set; }
        public TimeSpan wedStartTimeTotal { get; set; }
        public TimeSpan wedEndTimeTotal { get; set; }
        public TimeSpan wedBreakTimeTotal { get; set; }
        public TimeSpan thuStartTimeTotal { get; set; }
        public TimeSpan thuEndTimeTotal { get; set; }
        public TimeSpan thuBreakTimeTotal { get; set; }
        public TimeSpan friStartTimeTotal { get; set; }
        public TimeSpan friEndTimeTotal { get; set; }
        public TimeSpan friBreakTimeTotal { get; set; }
        public TimeSpan satStartTimeTotal { get; set; }
        public TimeSpan satEndTimeTotal { get; set; }
        public TimeSpan satBreakTimeTotal { get; set; }
        public TimeSpan sunStartTimeTotal { get; set; }
        public TimeSpan sunEndTimeTotal { get; set; }
        public TimeSpan sunBreakTimeTotal { get; set; }
        #endregion
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

        #region GEOS2-4038 Sudhir.Jangra 04/07/2023
        [NotMapped]
        [DataMember]
        public string MonStartTime
        {
            get { return monStartTime; }
            set
            {
                monStartTime = value;
                OnPropertyChanged("MonStartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string MonEndTime
        {
            get { return monEndTime; }
            set
            {
                monEndTime = value;
                OnPropertyChanged("MonEndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string MonBreakTime
        {
            get { return monBreakTime; }
            set
            {
                monBreakTime = value;
                OnPropertyChanged("MonBreakTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string TueStartTime
        {
            get { return tueStartTime; }
            set
            {
                tueStartTime = value;
                OnPropertyChanged("TueStartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string TueEndTime
        {
            get { return tueEndTime; }
            set
            {
                tueEndTime = value;
                OnPropertyChanged("TueEndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string TueBreakTime
        {
            get { return tueBreakTime; }
            set
            {
                tueBreakTime = value;
                OnPropertyChanged("TueBreakTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string WedStartTime
        {
            get { return wedStartTime; }
            set
            {
                wedStartTime = value;
                OnPropertyChanged("WedStartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string WedEndTime
        {
            get { return wedEndTime; }
            set
            {
                wedEndTime = value;
                OnPropertyChanged("WedEndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string WedBreakTime
        {
            get { return wedBreakTime; }
            set
            {
                wedBreakTime = value;
                OnPropertyChanged("WedBreakTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string ThuStartTime
        {
            get { return thuStartTime; }
            set
            {
                thuStartTime = value;
                OnPropertyChanged("ThuStartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string ThuEndTime
        {
            get { return thuEndTime; }
            set
            {
                thuEndTime = value;
                OnPropertyChanged("ThuEndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string ThuBreakTime
        {
            get { return thuBreakTime; }
            set
            {
                thuBreakTime = value;
                OnPropertyChanged("ThuBreakTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string FriStartTime
        {
            get { return friStartTime; }
            set
            {
                friStartTime = value;
                OnPropertyChanged("FriStartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string FriEndTime
        {
            get { return friEndTime; }
            set
            {
                friEndTime = value;
                OnPropertyChanged("FriEndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string FriBreakTime
        {
            get { return friBreakTime; }
            set
            {
                friBreakTime = value;
                OnPropertyChanged("FriBreakTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string SatStartTime
        {
            get { return satStartTime; }
            set
            {
                satStartTime = value;
                OnPropertyChanged("SatStartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string SatEndTime
        {
            get { return satEndTime; }
            set
            {
                satEndTime = value;
                OnPropertyChanged("SatEndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string SatBreakTime
        {
            get { return satBreakTime; }
            set
            {
                satBreakTime = value;
                OnPropertyChanged("SatBreakTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string SunStartTime
        {
            get { return sunStartTime; }
            set
            {
                sunStartTime = value;
                OnPropertyChanged("SunStartTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string SunEndTime
        {
            get { return sunEndTime; }
            set
            {
                sunEndTime = value;
                OnPropertyChanged("SunEndTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string SunBreakTime
        {
            get { return sunBreakTime; }
            set
            {
                sunBreakTime = value;
                OnPropertyChanged("SunBreakTime");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalWorkingHours
        {
            get { return totalWorkingHours; }
            set
            {
                totalWorkingHours = value;
                OnPropertyChanged("TotalWorkingHours");
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
        public string BackupEmployeeCode
        {
            get { return backupEmployeeCode; }
            set
            {
                backupEmployeeCode = value;
                OnPropertyChanged("BackupEmployeeCode");
            }
        }
        #endregion


        #endregion


        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
